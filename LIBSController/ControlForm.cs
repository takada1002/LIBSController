using LIBSController.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using CounterBoardLib;

namespace LIBSController
{
    public partial class ControlForm : Form
    {
		#region 定数

		const int NOZZLE_MAX = 5;

		#endregion

		#region フィールド

		// アプリケーションログ
		private ApplicationLog appLog = null;

		// 選別対象設定クラス
		private MaterialSetting materialSetting = null;

        // LIBS ソータコントローラ
        LIBSSorter libsSorter = null;

        // 速度変更用タイマー
        System.Timers.Timer changeTimerS = null;
        // 速度値変更中フラグ
        Boolean isChangingS = false;
        // 速度変更前の値
        Int32 beforeSpeed = 0;

        // タイミング変更用タイマー
        System.Timers.Timer changeTimerT = null;
        // タイミング値変更中フラグ
        Boolean isChangingT = false;
		// タイミング変更前の値
		//UInt32 beforeTiming = 0;
		List<UInt32> beforeTiming = new List<uint>();

#if DEBUG
        DebugView debugView = new DebugView();
#endif
		CounterBoardDebugView boardDebugView;

		ConveyorDebugView conveyorDebugView;

		// I/O 出力
		Dictionary<CounterBoard.IOPort, Boolean> outputDict = new Dictionary<CounterBoard.IOPort, Boolean>()
                                                        {
                                                            {CounterBoard.IOPort.PORT1,  false},
                                                            {CounterBoard.IOPort.PORT2,  false},
                                                            {CounterBoard.IOPort.PORT3,  false},
                                                            {CounterBoard.IOPort.PORT4,  false},
                                                            {CounterBoard.IOPort.PORT5,  false},
                                                            {CounterBoard.IOPort.PORT6,  false},
                                                            {CounterBoard.IOPort.PORT7,  false},
                                                            {CounterBoard.IOPort.PORT8,  false},
                                                            {CounterBoard.IOPort.PORT9,  false},
                                                            {CounterBoard.IOPort.PORT10, false},
                                                            {CounterBoard.IOPort.PORT11, false},
                                                            {CounterBoard.IOPort.PORT12, false},
                                                            {CounterBoard.IOPort.PORT13, false},
                                                            {CounterBoard.IOPort.PORT14, false},
                                                            {CounterBoard.IOPort.PORT15, false},
                                                            {CounterBoard.IOPort.PORT16, false},
                                                        };

        // カウンターボードライブラリ
        private List<CounterBoard> counterBoard = null;

		// ソフト終了用のチェック変数
		private Int32 exitStatus = 0;

		private Queue<PaddleDriveEventArgs> paddleDriveQueue = new Queue<PaddleDriveEventArgs>();

		private Queue<LIBSAnalysisEventArgs> analysisQueue = new Queue<LIBSAnalysisEventArgs>();

		private System.Windows.Forms.Timer viewUpdateTimer;

		#endregion

		#region 初期化

		// コンストラクタ
		public ControlForm()
        {
            this.counterBoard = CounterBoard.Instance;

			if(Settings.Default.CounterBoardDebug)
				CounterBoard.IsCounterBoardDebug = true;

			InitializeComponent();
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();

            // 素材選別設定のインスタンス取得
            this.materialSetting = MaterialSetting.getInstance();

            this.appLog.Info("<----LIBS制御ソフトウェア起動---->");

            // LIBS ソータの初期化
            initialize();
        }

		// 初期化処理
		private void initialize()
        {
            // 画面表示初期化
            this.labelAnalysisResult.Text = "";
            this.labelAnalysisResult.BackColor = Color.White;
            this.labelAnalysisGroup.Text = "";
            this.labelAnalysisGroup.BackColor = Color.White;

            // コンボボックスとタイミング調整を変更不可に
            this.comboBoxConveyorSpeed.Enabled = false;
            this.numericUpDownPaddleTiming.Enabled = false;
			this.labelNozzleNumber.Enabled = false;
			this.buttonNozzleBack.Enabled = false;
			this.buttonNozzleNext.Enabled = false;

            this.statusLabel.Status = LIBSSorter.SorterStatus.TEST_READY;
            this.statusLabel.LabelText = "";

			// コンボボックスの選択肢作成
			this.comboBoxConveyorSpeed.DataSource = ConveyorSettings.Instance.DataList;
			this.comboBoxConveyorSpeed.DisplayMember = "Name";
			this.comboBoxConveyorSpeed.ValueMember = "PulseFrequency";

			// 保存されている設定値の読み出し
			var conveyorItem = ConveyorSettings.GetSelectedConveyor();
			this.comboBoxConveyorSpeed.SelectedItem = conveyorItem;

			// タイミング表示
			this.numericUpDownPaddleTiming.Value = ConveyorSettings.GetNozzleTiming(Convert.ToInt32(labelNozzleNumber.Text));

			// ノズル番号表示
			this.labelNozzleNumber.Text = "1";

			// LIBS ソータ制御 インスタンス作成
			this.libsSorter = new LIBSSorter();

			// LIBS ソータの状態変更イベント
			this.libsSorter.sorterStatusChanged += OnChangedSorterStatus;

            // LIBS 装置とのコネクション状態変更イベント
            this.libsSorter.libsConnectionChanged += OnChangedLIBSConnection;

            // LIBS ソータのメッセージ出力イベント
            this.libsSorter.messagePrinted += OnPrintedMessage;

            // LIBS 装置からの情報受信イベント
            this.libsSorter.analysisResultReceived += OnReceivedAnalysisResult;

            // 電磁パドル駆動イベント
            this.libsSorter.paddleDrived += OnDrivedPaddle;

            try
            {
                if (!this.libsSorter.initialize())
                {
                    
                    this.changeStatusLabel("LIBS ソータの初期化に失敗しました。", Color.Red);
                }
            }
            catch (Exception ex)
            {
                this.appLog.Error("LIBS ソータの初期化に失敗しました。");
                this.changeStatusLabel(ex.Message, Color.Red);
            }

            // タイマー準備
            this.changeTimerS = new System.Timers.Timer(Settings.Default.ChangeTimer);
            this.changeTimerS.Elapsed += OnElapsedChangeTimerS;
            this.changeTimerT = new System.Timers.Timer(Settings.Default.ChangeTimer);
            this.changeTimerT.Elapsed += OnElapsedChangeTimerT;

			this.viewUpdateTimer = new System.Windows.Forms.Timer();
			this.viewUpdateTimer.Interval = 50;
			this.viewUpdateTimer.Tick += ViewUpdateTimer_Tick;
			this.viewUpdateTimer.Start();

#if DEBUG
			//appLog.LogWrited = (level, log) => { debugView.WriteStrLine(log); };
			appLog.LogWrited = (level, log) => { debugView.WriteStr(log); };
			debugView.Show();
#endif

			if(Settings.Default.CounterBoardDebug)
			{
				boardDebugView = CounterBoardDebugView.Instance;
				boardDebugView.Show();
			}

			if (Settings.Default.ConveyorDebug)
			{
				conveyorDebugView = ConveyorDebugView.Instance;
				conveyorDebugView.Show();
			}
		}

		// ロードイベント
		private void ControlForm_Load(object sender, EventArgs e)
		{

		}

		#endregion

		#region イベントハンドラ（クラス）

		// LIBS 接続状態変化
		private void OnChangedLIBSConnection(object sender, LIBSConnectionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, LIBSConnectionEventArgs>(OnChangedLIBSConnection), new object[] { sender, e });
                return;
            }

            if(e.IsConnect)
            {
                this.appLog.Info("LIBS 装置との接続が確立されました。");
                this.labelConnection.Text = "接続中";
                this.labelConnection.BackColor = Color.Green;
            }
            else
            {
                this.appLog.Info("LIBS 装置との接続が切断されました。");
                this.labelConnection.Text = "未接続";
                this.labelConnection.BackColor = Color.Red;
            }
        }

		// 電磁パドルの駆動状態変化
		private void OnDrivedPaddle(object sender, PaddleDriveEventArgs e)
        {
			this.paddleDriveQueue.Enqueue(e);
			return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, PaddleDriveEventArgs>(OnDrivedPaddle), new object[] { sender, e });
                return;
            }

			Action<Label,bool> SetLabelColor = (lbl,flg)=>
			{
				if(flg)
					lbl.BackColor = Color.Green;
				else
					lbl.BackColor = Color.White;
			};


			SetLabelColor(this.labelPaddle1, e.DrivingFlag[0]);
			SetLabelColor(this.labelPaddle2, e.DrivingFlag[1]);
			SetLabelColor(this.labelPaddle3, e.DrivingFlag[2]);
			SetLabelColor(this.labelPaddle4, e.DrivingFlag[3]);
			SetLabelColor(this.labelPaddle5, e.DrivingFlag[4]);
			SetLabelColor(this.labelPaddle6, e.DrivingFlag[5]);

            this.Refresh();
        }

        // 受信結果変化
        private void OnReceivedAnalysisResult(object sender, LIBSAnalysisEventArgs e)
        {
			this.analysisQueue.Enqueue(e);
			return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, LIBSAnalysisEventArgs>(OnReceivedAnalysisResult), new object[] { sender, e });
                return;
            }

            this.labelAnalysisGroup.Text = string.Empty;
            this.labelAnalysisResult.Text = string.Empty;

            this.Refresh();

            if (e.Result)
            {
                this.labelAnalysisResult.Text = "OK";
                this.labelAnalysisResult.BackColor = Color.Lime;
                this.labelAnalysisGroup.BackColor = Color.Lime;
            }
            else
            {
                this.labelAnalysisResult.Text = "NG";
                this.labelAnalysisResult.BackColor = Color.Red;
                this.labelAnalysisGroup.BackColor = Color.Red;
            }

            if (e.Group == LIBSManager.MaterialGroup.UNKNOWN || e.Group == LIBSManager.MaterialGroup.NO_MEASURE)
            {
                this.labelAnalysisGroup.Text = e.Group.ToString();
            }
            else
            {
                this.labelAnalysisGroup.Text = this.materialSetting.getClassName((int)e.Group);
            }
        }

		// ソータクラスからの出力メッセージの受け取り
		private void OnPrintedMessage(object sender, MessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, MessageEventArgs>(OnPrintedMessage), new object[] { sender, e });
                return;
            }

			if(this.statusLabel.LabelText != e.Message)
			{
				// 画面出力メッセージはログにも記載
				this.appLog.Info("ステータスメッセージ：" + e.Message);

				// 下部ラベル領域に出力
				this.statusLabel.LabelText = e.Message;
			}
        }

        // ソータの状態変更
        private void OnChangedSorterStatus(object sender, SorterStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, SorterStatusEventArgs>(OnChangedSorterStatus), new object[] { sender, e });
                return;
            }

            // ステータス・ボタンの表示を更新
            this.statusLabel.Status = e.LIBSStatus;
			this.buttonStartLIBSSorter.Visible = e.ForceRunButtonVisible;
			this.buttonStopLIBSSorter.Visible = e.ForceRunButtonVisible;
			this.btnInverterReset.Visible = e.InverterResetButtonVisible;
			this.btnEncoderReset.Visible = e.EncoderResetButtonVisible;
			this.btnLIBSReset.Visible = e.LibsResetButtonVisible;
		}

		// 画面更新タイマ
		private void ViewUpdateTimer_Tick(object sender, EventArgs e)
		{
			PaddleDriveEventArgs pdq = null;
			while(this.paddleDriveQueue.Count > 0)
			{
				pdq = this.paddleDriveQueue.Dequeue();
			}

			if(pdq != null)
			{ 
				Action<Label, bool> SetLabelColor = (lbl, flg) =>
				{
					if (flg)
						lbl.BackColor = Color.Green;
					else
						lbl.BackColor = Color.White;
				};


				SetLabelColor(this.labelPaddle1, pdq.DrivingFlag[0]);
				SetLabelColor(this.labelPaddle2, pdq.DrivingFlag[1]);
				SetLabelColor(this.labelPaddle3, pdq.DrivingFlag[2]);
				SetLabelColor(this.labelPaddle4, pdq.DrivingFlag[3]);
				SetLabelColor(this.labelPaddle5, pdq.DrivingFlag[4]);
				SetLabelColor(this.labelPaddle6, pdq.DrivingFlag[5]);
			}

			LIBSAnalysisEventArgs laq = null;

			while (this.analysisQueue.Count > 0)
			{
				laq = this.analysisQueue.Dequeue();
			}

			if(laq != null)
			{ 
				this.labelAnalysisGroup.Text = string.Empty;
				this.labelAnalysisResult.Text = string.Empty;

				this.Refresh();

				if (laq.Result)
				{
					this.labelAnalysisResult.Text = "OK";
					this.labelAnalysisResult.BackColor = Color.Lime;
					this.labelAnalysisGroup.BackColor = Color.Lime;
				}
				else
				{
					this.labelAnalysisResult.Text = "NG";
					this.labelAnalysisResult.BackColor = Color.Red;
					this.labelAnalysisGroup.BackColor = Color.Red;
				}

				if (laq.Group == LIBSManager.MaterialGroup.UNKNOWN || 
					laq.Group == LIBSManager.MaterialGroup.NO_MEASURE ||
					laq.Group == LIBSManager.MaterialGroup.TOO_SLOW ||
					laq.Group == LIBSManager.MaterialGroup.WEAK_INTENSITY)
				{
					this.labelAnalysisGroup.Text = laq.Group.ToString();
				}
				else
				{
					this.labelAnalysisGroup.Text = this.materialSetting.getClassName((int)laq.Group);
				}
			}
		}

		#endregion

		#region イベントハンドラ（画面）

		// コンベア搬送速度変更ボタンクリックイベント
		private void OnClickButtonSpeedChange(object sender, EventArgs e)
        {
            // 装置状態が RUN のときは受け付けない
            if (this.libsSorter.Status == LIBSSorter.SorterStatus.RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.TEST_RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.FORCE_RUN)
            {
                this.statusLabel.LabelText = "装置が運転中のときに速度変更は行えません。";

                return;
            }

            // 速度変更中なら確定 そうでなければ変更開始
            if (!this.isChangingS)
            {
                // 現在の設定値を取得
                this.beforeSpeed = this.comboBoxConveyorSpeed.SelectedIndex;
                // 有効化
                this.comboBoxConveyorSpeed.Enabled = true;
                // 速度変更中
                this.isChangingS = true;
                // ボタンの色変更 (青)
                this.buttonChangeConveyorSpeed.BackColor = Color.CornflowerBlue;
                // 速度変更タイマースタート
                this.changeTimerS.Start();
            }
            else
            {
                // 速度変更タイマーストップ
                this.changeTimerS.Stop();

                // 無効化
                this.comboBoxConveyorSpeed.Enabled = false;
                // 速度変更終了
                this.isChangingS = false;
                // ボタンの色変更 (白)
                this.buttonChangeConveyorSpeed.BackColor = Color.White;

				var selectedConveyor = ConveyorSettings.Instance.DataList[this.comboBoxConveyorSpeed.SelectedIndex];
				this.libsSorter.changeConveyorSpeed(selectedConveyor.PulseFrequency);

				ConveyorSettings.Instance.DataList.ForEach(d=>
				{
					if(d != this.comboBoxConveyorSpeed.SelectedItem)
						d.Selected = false;
					else
						d.Selected = true;
				});

				if(labelNozzleNumber.Text != "1")
					labelNozzleNumber.Text = "1";

				ConveyorSettings.Serialize();
            }
        }

        // コンベア搬送速度変更タイマーイベント
        private void OnElapsedChangeTimerS(object sender, ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, ElapsedEventArgs>(OnElapsedChangeTimerS), new object[] { sender, e });
                return;
            }

            if (this.isChangingS)
            {
                // 速度変更タイマーストップ
                this.changeTimerS.Stop();

                // 無効化
                this.comboBoxConveyorSpeed.Enabled = false;
                // 速度変更終了
                this.isChangingS = false;
                // ボタンの色変更 (白)
                this.buttonChangeConveyorSpeed.BackColor = Color.White;
                // 設定値を元の値に戻す
                this.comboBoxConveyorSpeed.SelectedIndex = this.beforeSpeed;
            }
        }

        // エアノズル駆動タイミング変更ボタンクリックイベント
        private void OnClickButtonTimingChange(object sender, EventArgs e)
        {
            // 装置状態が RUN のときは受け付けない
            if (this.libsSorter.Status == LIBSSorter.SorterStatus.RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.TEST_RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.FORCE_RUN)
			{
                this.statusLabel.LabelText = "装置が運転中のときにタイミング変更は行えません。";

                return;
            }

            // 速度変更中なら確定 そうでなければ変更開始
            if (!this.isChangingT)
            {
                // 現在の設定値を取得
                //this.beforeTiming = (UInt32)this.numericUpDownPaddleTiming.Value;
				this.beforeTiming = ConveyorSettings.GetNozzleTiming();
                // 有効化
                this.numericUpDownPaddleTiming.Enabled = true;
				this.labelNozzleNumber.Enabled = true;
				this.buttonNozzleBack.Enabled = true;
				this.buttonNozzleNext.Enabled = true;
				// 速度変更中
				this.isChangingT = true;
                // ボタンの色変更 (青)
                this.buttonChangePaddleTiming.BackColor = Color.CornflowerBlue;
                // 速度変更タイマースタート
                this.changeTimerT.Start();
            }
            else
            {
                // 速度変更タイマーストップ
                this.changeTimerT.Stop();

                // 無効化
                this.numericUpDownPaddleTiming.Enabled = false;
				this.labelNozzleNumber.Enabled = false;
				this.buttonNozzleBack.Enabled = false;
				this.buttonNozzleNext.Enabled = false;
				// 速度変更終了
				this.isChangingT = false;
                // ボタンの色変更 (白)
                this.buttonChangePaddleTiming.BackColor = Color.White;
				// パドル出力タイミング調整

				ConveyorSettings.Serialize();
			}
        }

        // エアノズル駆動タイミング変更タイマーイベント
        private void OnElapsedChangeTimerT(object sender, ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, ElapsedEventArgs>(OnElapsedChangeTimerT), new object[] { sender, e });
                return;
            }

            if (this.isChangingT)
            {
                // 速度変更タイマーストップ
                this.changeTimerT.Stop();

                // 無効化
                this.numericUpDownPaddleTiming.Enabled = false;
				this.labelNozzleNumber.Enabled = false;
				this.buttonNozzleBack.Enabled = false;
				this.buttonNozzleNext.Enabled = false;
				// 速度変更終了
				this.isChangingT = false;
                // ボタンの色変更 (白)
                this.buttonChangePaddleTiming.BackColor = Color.White;
                // 設定値を元の値に戻す
                //this.numericUpDownPaddleTiming.Value = Settings.Default.PaddleDriveTiming;
				ConveyorSettings.SetNozzleTiming(beforeTiming);
				ConveyorSettings.Serialize();
				this.numericUpDownPaddleTiming.Value = ConveyorSettings.GetNozzleTiming(Convert.ToInt32(labelNozzleNumber.Text));
			}
        }

        // LIBS ソータの強制運転開始ボタンクリックイベント
        private void OnClickButtonStartLIBSSorter(object sender, EventArgs e)
        {
			// LIBS 装置の開始
			Task.Run(new Action(this.libsSorter.forceStart));
        }

		// LIBS ソータの強制運転停止ボタンクリックイベント
		private void OnClickStopLIBSSorter(object sender, EventArgs e)
        {
			// LIBS 装置の停止
			Task.Run(new Action(this.libsSorter.forceStop));
		}

        // LIBS ソータ再起動ボタンクリックイベント
        private void OnClickResetLIBSSorter(object sender, EventArgs e)
        {
            // 装置状態が RUN のときは受け付けない
            if (this.libsSorter.Status == LIBSSorter.SorterStatus.RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.TEST_RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.FORCE_RUN)
			{
                this.statusLabel.LabelText = "装置が運転中のときは実行できません。";

                return;
            }

            using (ConfirmForm confirmForm = new ConfirmForm("LIBSソータ再起動","LIBSソータを再起動しても\r\n\r\nよろしいでしょうか?"))
            {
                DialogResult res = confirmForm.ShowDialog(this);
                if(res == DialogResult.OK)
                {
                    // LIBS 装置の停止
                    this.libsSorter.exitLIBSController();

                    // 制御 PC の再起動
                    AdjustToken();
                    ExitWindowsEx(ExitWindows.EWX_REBOOT | ExitWindows.EWX_FORCE, 0);
                }
            }
        }

        // エアーノズル動作設定ボタンクリックイベント
        private void OnClickAirNozzleSetting(object sender, EventArgs e)
        {
			// 装置状態が RUN のときは受け付けない
			if (this.libsSorter.Status == LIBSSorter.SorterStatus.RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.TEST_RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.FORCE_RUN)
			{
				this.statusLabel.LabelText = "装置が運転中のときに設定変更は行えません。";

				return;
			}

			using (AirNozzleSettingForm airNozzleSettingForm = new AirNozzleSettingForm())
            {
                airNozzleSettingForm.ShowDialog(this);
            }
        }

		// フォームが閉じられる際のイベント
		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			// LIBS ソータの終了処理
			this.libsSorter.exitLIBSController();
		}

		// ソフト終了用の隠しラベル ダブルクリック
		private void OnDoubleClickLabelExit(object sender, EventArgs e)
		{
			if (exitStatus == 0)
			{
				exitStatus = 1;
			}
			else if (exitStatus == 2)
			{
				// LIBS 装置の停止
				this.libsSorter.exitLIBSController();

				this.Close();
			}
			else
			{
				exitStatus = 0;
			}

		}

		// ソフト終了用の隠しラベル2 ダブルクリック
		private void OnDoubleClickLabelExit2(object sender, EventArgs e)
		{
			if (exitStatus == 1)
			{
				exitStatus = 2;
			}
			else
			{
				exitStatus = 0;
			}
		}

		// 選別素材の選択画面表示ボタンクリックイベント
		private void OnClickMaterialSetting(object sender, EventArgs e)
		{
			// 装置状態が RUN のときは受け付けない
			if (this.libsSorter.Status == LIBSSorter.SorterStatus.RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.TEST_RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.FORCE_RUN)
			{
				this.statusLabel.LabelText = "装置が運転中のときに設定変更は行えません。";

				return;
			}

			using (MaterialSelectForm matSelForm = new MaterialSelectForm())
			{
				matSelForm.ShowDialog(this);

				// ダイアログから処理が戻ってきたら設定を保存する
				matSelForm.saveMaterialSetting();
			}
		}

		// パドル動作タイミング変更イベント
		private void OnValueChangedPaddleTiming(object sender, EventArgs e)
		{
			// 有効状態なら値変更中はタイマーをリセット
			if (this.numericUpDownPaddleTiming.Enabled)
			{
				ConveyorSettings.SetNozzleTiming(Convert.ToInt32(labelNozzleNumber.Text), Convert.ToUInt32(this.numericUpDownPaddleTiming.Value));
				this.changeTimerT.Stop();
				this.changeTimerT.Start();
			}
		}

		// ノズル番号変化イベント
		private void labelNozzleNumber_TextChanged(object sender, EventArgs e)
		{
			if (this.labelNozzleNumber.Enabled)
			{
				this.changeTimerT.Stop();
				this.changeTimerT.Start();
			}

			this.numericUpDownPaddleTiming.Value = ConveyorSettings.GetNozzleTiming(Convert.ToInt32(labelNozzleNumber.Text));
		}

		// ノズル番号 前 クリックイベント
		private void buttonNozzleBack_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(labelNozzleNumber.Text);
			if (num > 1)
			{
				labelNozzleNumber.Text = (num - 1).ToString();
			}
		}

		// ノズル番号 次 クリックイベント
		private void buttonNozzleNext_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(labelNozzleNumber.Text);
			if (num < NOZZLE_MAX)
			{
				labelNozzleNumber.Text = (num + 1).ToString();
			}
		}

		// パドル１ラベルクリックイベント
		private void labelPaddle1_Click(object sender, EventArgs e)
		{
			if (Settings.Default.AirNozzleDebug)
			{
				// 出力反転
				outputDict[CounterBoard.IOPort.PORT1] = !outputDict[CounterBoard.IOPort.PORT1];
				this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT1, outputDict[CounterBoard.IOPort.PORT1]);

				this.counterBoard[0].outputDO();

				if (outputDict[CounterBoard.IOPort.PORT1])
				{
					((Label)sender).BackColor = Color.Green;
				}
				else
				{
					((Label)sender).BackColor = Color.White;
				}
			}
		}

		// パドル２ラベルクリックイベント
		private void labelPaddle2_Click(object sender, EventArgs e)
		{
			if (Settings.Default.AirNozzleDebug)
			{
				// 出力反転
				outputDict[CounterBoard.IOPort.PORT2] = !outputDict[CounterBoard.IOPort.PORT2];
				this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT2, outputDict[CounterBoard.IOPort.PORT2]);
				this.counterBoard[0].outputDO();

				if (outputDict[CounterBoard.IOPort.PORT2])
				{
					((Label)sender).BackColor = Color.Green;
				}
				else
				{
					((Label)sender).BackColor = Color.White;
				}
			}
		}

		// パドル３ラベルクリックイベント
		private void labelPaddle3_Click(object sender, EventArgs e)
		{
			if (Settings.Default.AirNozzleDebug)
			{
				// 出力反転
				outputDict[CounterBoard.IOPort.PORT3] = !outputDict[CounterBoard.IOPort.PORT3];
				this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT3, outputDict[CounterBoard.IOPort.PORT3]);
				this.counterBoard[0].outputDO();

				if (outputDict[CounterBoard.IOPort.PORT3])
				{
					((Label)sender).BackColor = Color.Green;
				}
				else
				{
					((Label)sender).BackColor = Color.White;
				}
			}
		}

		// パドル４ラベルクリックイベント
		private void labelPaddle4_Click(object sender, EventArgs e)
		{
			if (Settings.Default.AirNozzleDebug)
			{
				// 出力反転
				outputDict[CounterBoard.IOPort.PORT4] = !outputDict[CounterBoard.IOPort.PORT4];
				this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT4, outputDict[CounterBoard.IOPort.PORT4]);
				this.counterBoard[0].outputDO();

				if (outputDict[CounterBoard.IOPort.PORT4])
				{
					((Label)sender).BackColor = Color.Green;
				}
				else
				{
					((Label)sender).BackColor = Color.White;
				}
			}
		}

		// パドル５ラベルクリックイベント
		private void labelPaddle5_Click(object sender, EventArgs e)
		{
			if (Settings.Default.AirNozzleDebug)
			{
				// 出力反転
				outputDict[CounterBoard.IOPort.PORT5] = !outputDict[CounterBoard.IOPort.PORT5];
				this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT5, outputDict[CounterBoard.IOPort.PORT5]);
				this.counterBoard[0].outputDO();

				if (outputDict[CounterBoard.IOPort.PORT5])
				{
					((Label)sender).BackColor = Color.Green;
				}
				else
				{
					((Label)sender).BackColor = Color.White;
				}
			}
		}

		// パドル６ラベルクリックイベント
		private void labelPaddle6_Click(object sender, EventArgs e)
		{
			if (Settings.Default.AirNozzleDebug)
			{
				// 出力反転
				outputDict[CounterBoard.IOPort.PORT6] = !outputDict[CounterBoard.IOPort.PORT6];
				this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT6, outputDict[CounterBoard.IOPort.PORT6]);
				this.counterBoard[0].outputDO();

				if (outputDict[CounterBoard.IOPort.PORT6])
				{
					((Label)sender).BackColor = Color.Green;
				}
				else
				{
					((Label)sender).BackColor = Color.White;
				}
			}
		}

		// インバーターリセット押下
		private void btnInverterReset_Click(object sender, EventArgs e)
		{
			Task.Run(new Action(libsSorter.ResetInverterError));
		}

		// エンコーダーリセット押下
		private void btnEncoderReset_Click(object sender, EventArgs e)
		{
			Task.Run(new Action(libsSorter.ResetEncoderError));
		}

		// LIBSエラーリセット押下
		private void btnLIBSReset_Click(object sender, EventArgs e)
		{
			Task.Run(new Action(libsSorter.ResetLibsError));
			this.btnLIBSReset.Visible = false;
		}

		// シャットダウンクリックイベント
		private void OnClickButtonShutdown(object sender, EventArgs e)
		{
			// 装置状態が RUN のときは受け付けない
			if (this.libsSorter.Status == LIBSSorter.SorterStatus.RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.TEST_RUN ||
				this.libsSorter.Status == LIBSSorter.SorterStatus.FORCE_RUN)
			{
				this.statusLabel.LabelText = "装置が運転中のときは実行できません。";

				return;
			}

			using (ConfirmForm confirmForm = new ConfirmForm("シャットダウン", "シャットダウンしても\r\n\r\nよろしいでしょうか?"))
			{
				DialogResult res = confirmForm.ShowDialog(this);
				if (res == DialogResult.OK)
				{
					// LIBS ソータの終了処理
					this.libsSorter.exitLIBSController();

					AdjustToken();
					ExitWindowsEx(ExitWindows.EWX_POWEROFF | ExitWindows.EWX_FORCE, 0);
				}
			}

		}



		#endregion

		#region シャットダウン用 Windows AP

		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean CloseHandle(IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern Boolean OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Boolean LookupPrivilegeValue(String lpSystemName, String lpName, out Int64 lpLuid);

        [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TOKEN_PRIVILEGES
        {
            public Int32 PrivilegeCount;
            public Int64 Luid;
            public Int32 Attributes;
        }

        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern Boolean AdjustTokenPrivileges(IntPtr TokenHandle,
                                                            Boolean DisableAllPrivileges,
                                                            ref TOKEN_PRIVILEGES NewState,
                                                            Int32 BufferLength,
                                                            IntPtr PreviousState,
                                                            IntPtr ReturnLength);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(ExitWindows uFlags, UInt32 dwReason);

        public enum ExitWindows : uint
        {
            EWX_LOGOFF = 0x00,
            EWX_SHUTDOWN = 0x01,
            EWX_REBOOT = 0x02,
            EWX_POWEROFF = 0x08,
            EWX_RESTARTAPPS = 0x40,
            EWX_FORCE = 0x04,
            EWX_FORCEIFHUNG = 0x10,
        }

        private const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x20;
        private const UInt32 TOKEN_QUERY = 0x8;
        private const Int32 SE_PRIVILEGE_ENABLED = 0x2;
        private const String SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

		#endregion

		#region 非公開メソッド

		//シャットダウンするためのセキュリティ特権を有効にする
		private static void AdjustToken()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return;

            IntPtr procHandle = GetCurrentProcess();
            IntPtr tokenHandle;

            //トークンを取得する
            OpenProcessToken(procHandle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out tokenHandle);

            //LUIDを取得する
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.Attributes = SE_PRIVILEGE_ENABLED;
            tp.PrivilegeCount = 1;
            LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out tp.Luid);
            //特権を有効にする
            AdjustTokenPrivileges(tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);

            //閉じる
            CloseHandle(tokenHandle);
        }

        // ステータスラベルの表示
        private void changeStatusLabel(String message, Color backColor)
        {
            this.statusLabel.LabelText = message;
            this.statusLabel.BackColor = backColor;
        }

		#endregion


	}
}
