using CounterBoardLib;
using LIBSController.Properties;
using MelsecInverterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIBSController
{
    // メッセージ出力用 EventArgs
    public class MessageEventArgs : EventArgs
    {
        private String message;
        public String Message
        {
            get { return message; }
        }

        public MessageEventArgs(String message)
        {
            this.message = message;
        }
    }

    // ソータ状態取得用 EventArgs
    public class SorterStatusEventArgs : EventArgs
    {
        public LIBSSorter.SorterStatus LIBSStatus { set; get; }
		public Boolean InverterResetButtonVisible { set; get; }
		public Boolean EncoderResetButtonVisible { set; get; }
		public Boolean ForceRunButtonVisible { set; get; }
		public Boolean LibsResetButtonVisible { set; get; }

		public SorterStatusEventArgs(
			LIBSSorter.SorterStatus libsStatus , 
			bool inverterResetButtonVisible, 
			bool encoderResetButtonVisible, 
			bool forceRunButtonVisible,
			bool libsResetButtonVisible)
        {
            this.LIBSStatus = libsStatus;
			this.EncoderResetButtonVisible = encoderResetButtonVisible;
			this.InverterResetButtonVisible = inverterResetButtonVisible;
			this.ForceRunButtonVisible = forceRunButtonVisible;
			this.LibsResetButtonVisible = libsResetButtonVisible;
        }
    }

    // LIBS ソータ制御クラス
    public class LIBSSorter
    {
		#region プロパティ

		public enum SorterStatus : int
        {
            NOT_READY   ,   // 停止中運転不可能
            READY       ,   // 停止中運転可能
            RUN         ,   // 運転中
            EMERGENCY   ,   // 緊急停止中
            ERROR		,   // インバーター異常検出中
			TEST_READY	,   // テスト運転可能
			TEST_RUN	,   // テスト運転中
			FORCE_RUN	,   // 強制運転中
			UNKNOWN     ,   // 状態不定
        }

		// 運転状態
		private SorterStatus status = SorterStatus.NOT_READY;
		public SorterStatus Status
		{
			get { return status; }
			set
			{
				if(status != value)
				{ 
					status = value;

					this.appLog.Info($"ステータス変化：{status.ToString()}");
				}

				if (sorterStatusChanged != null)
				{
					bool isForceButtonVisible = false;
					if(!inverterErrorFlag && !encoderErrorFlag)
					{
						if(	value == SorterStatus.NOT_READY ||
							value == SorterStatus.ERROR ||
							value == SorterStatus.FORCE_RUN)
						{
							isForceButtonVisible = true;
						}
					}

					sorterStatusChanged(this, new SorterStatusEventArgs(
						value,
						inverterErrorFlag ,
						encoderErrorFlag,
						isForceButtonVisible,
						libsReadyTimeout || libsReadyDown));
				}
			}
		}

		#endregion

		#region 定数

		// カウンタボードのイベントマスク
		private const UInt32 IOEventMask1 = 0x55805580;
		private const UInt32 IOEventMask2 = 0xC060C060;
		private const UInt32 EQEventMask = 0x00000004;

		#endregion

		#region フィールド

		// アプリケーションログ
		private ApplicationLog appLog = null;

		// lock オブジェクト
		private object lockObj = new object();

        // LED 管理クラス
        private LEDManager ledMgr = null;

        // キースイッチ 管理クラス
        private KeySwitchManager keySwitchMgr = null;

        // 非常停止スイッチ管理クラス
        private EmergencyStopButtonManager emergencyStopButtonManager = null;

        // システムスイッチ管理クラス
        private SystemSwitchManager systemSwitchManager = null;

        // 電磁パドル 管理クラス
        private PaddleManager paddleMgr = null;

        // コンベア 管理クラス
        private ConveyorManager conveyorMgr = null;

        // LIBS 管理クラス
        private LIBSManager libsMgr = null;

        // 選別対象設定クラス
        private MaterialSetting materialSetting = null;

		// 信号管理クラス
		private SignalManager signalManager = null;

		// 素材管理キュー
		private List<MaterialQueue> materialQueue = null;

		// カウンターボード
		private List<CounterBoard> counterBoard = null;

		#endregion

		#region フラグ

		// 緊急停止フラグ
		private Boolean _emergencyRopeFlag = false;
		private Boolean emergencyRopeFlag
		{
			get
			{
				return _emergencyRopeFlag;
			}
			set
			{
				if (_emergencyRopeFlag != value)
				{
					_emergencyRopeFlag = value;
					UpdateStatus();
				}
			}
		}

		// 緊急停止ボタン押下フラグ
		private Boolean _emergencyButtonFlag = false;
		private Boolean emergencyButtonFlag
		{
			get
			{
				return _emergencyButtonFlag;
			}
			set
			{
				if (_emergencyButtonFlag != value)
				{
					_emergencyButtonFlag = value;
					UpdateStatus();
				}
			}
		}

		// システムスイッチ押下フラグ
		private Boolean _systemSwitchFlag = false;
		private Boolean systemSwitchFlag
		{
			get
			{
				return _systemSwitchFlag;
			}
			set
			{
				if (_systemSwitchFlag != value)
				{
					_systemSwitchFlag = value;
					UpdateStatus();
				}
			}
		}

		// インバーターエラーフラグ
		private Boolean _inverterErrorFlag = false;
		private Boolean inverterErrorFlag
		{
			get
			{
				return _inverterErrorFlag;
			}
			set
			{
				if (_inverterErrorFlag != value)
				{
					_inverterErrorFlag = value;
					UpdateStatus();
				}
			}
		}

		// インバーターエラーフラグ
		private Boolean _encoderErrorFlag = false;
		private Boolean encoderErrorFlag
		{
			get
			{
				return _encoderErrorFlag;
			}
			set
			{
				if (_encoderErrorFlag != value)
				{
					_encoderErrorFlag = value;
					UpdateStatus();
				}
			}
		}

		// LIBS 通信フラグ
		private Boolean _libsConnectFlag = false;
		private Boolean libsConnectFlag
		{
			get
			{
				return _libsConnectFlag;
			}
			set
			{
				if (_libsConnectFlag != value)
				{
					_libsConnectFlag = value;
					UpdateStatus();
					if(!value)
					{
						libsReadyDown = false;
						libsReadyTimeout = false;
					}
				}
			}
		}

		// LIBS Ready Timeout フラグ
		private Boolean _libsReadyTimeout = false;
		private Boolean libsReadyTimeout
		{
			get
			{
				return _libsReadyTimeout;
			}
			set
			{
				if (_libsReadyTimeout != value)
				{
					_libsReadyTimeout = value;
					UpdateStatus();
				}
			}
		}

		// LIBS Ready Down フラグ
		private Boolean _libsReadyDown = false;
		private Boolean libsReadyDown
		{
			get
			{
				return _libsReadyDown;
			}
			set
			{
				if (_libsReadyDown != value)
				{
					_libsReadyDown = value;
					UpdateStatus();
				}
			}
		}

		// LIBS 接続待ち（起動直後）フラグ
		private Boolean libsStartUpWaitFlag = true;

		// LIBS 運転 Ready 待ちフラグ
		private Boolean _libsRunReadyWaitFlag = false;
		private Boolean libsRunReadyWaitFlag
		{
			get
			{
				return _libsRunReadyWaitFlag;
			}
			set
			{
				if (_libsRunReadyWaitFlag != value)
				{
					_libsRunReadyWaitFlag = value;
					UpdateStatus();
				}
			}
		}

		// LIBS 異常フラグ
		private Boolean libsErrorFlag => (!libsConnectFlag || libsReadyTimeout || libsReadyDown) && !libsStartUpWaitFlag;

		// コンベア動作フラグ
		private Boolean _conveyorRunFlag = false;
		private Boolean conveyorRunFlag
		{
			get
			{
				return _conveyorRunFlag;
			}
			set
			{
				if (_conveyorRunFlag != value)
				{
					_conveyorRunFlag = value;
					UpdateStatus();
				}
			}
		}

		// コンベア動作フラグ
		private Boolean _forceRunFlag = false;
		private Boolean forceRunFlag
		{
			get
			{
				return _forceRunFlag;
			}
			set
			{
				if (_forceRunFlag != value)
				{
					_forceRunFlag = value;
					UpdateStatus();
				}
			}
		}

		#endregion

		#region イベント

		// LIBS の状態変更イベント
		public event EventHandler<SorterStatusEventArgs> sorterStatusChanged;

        // LIBS の接続状態変更イベント
        public event EventHandler<LIBSConnectionEventArgs> libsConnectionChanged;

        // メッセージ出力
        public event EventHandler<MessageEventArgs> messagePrinted;

        // 分析データ受信イベント
        public event EventHandler<LIBSAnalysisEventArgs> analysisResultReceived;

        // パドル駆動イベント
        public event EventHandler<PaddleDriveEventArgs> paddleDrived;

		#endregion

		#region 初期化

        // 初期化
        public Boolean initialize()
        {
            try
            {
                // ログ記載インスタンス
                this.appLog = ApplicationLog.getInstance();

                // 各種管理クラスのインスタンス作成
                this.ledMgr = new LEDManager();
                this.keySwitchMgr = new KeySwitchManager();

                this.emergencyStopButtonManager = new EmergencyStopButtonManager();
                this.systemSwitchManager = new SystemSwitchManager();

                this.paddleMgr = new PaddleManager();
                this.conveyorMgr = new ConveyorManager();
                this.libsMgr = new LIBSManager();

				this.signalManager = new SignalManager();

				// 選別対象設定
				this.materialSetting = MaterialSetting.getInstance();
                
                // カウンタボード
                this.counterBoard = CounterBoard.Instance;
                // 素材管理キュー
                this.materialQueue = MaterialQueue.Instance;

                // 運転開始イベントをセット
                this.keySwitchMgr.systemStarted += OnSystemStarted;
                this.keySwitchMgr.systemStopped += OnSystemStopped;

                // 非常停止ボタンイベントをセット
                this.emergencyStopButtonManager.emergencyStopped += OnEmergencyStopButtonOn;
                this.emergencyStopButtonManager.emergencyCancelled += OnEmergencyStopButtonOff;

                // システムスイッチイベントをセット
                this.systemSwitchManager.systemSwitchOn += OnSystemSwitchOn;
                this.systemSwitchManager.systemSwitchOff += OnSystemSwitchOff;

                // LIBS 装置の状態変化イベントをセット
                this.libsMgr.readyStatusChanged += OnChangedLIBSReady;
                this.libsMgr.analysisResultReceived += OnReceivedAnalysisResult;
                // LIBS 装置の接続状態変化イベントをセット
                this.libsMgr.connectionChanged += OnChangedLIBSConnection;

                // コンベア装置の緊急停止スイッチ
                this.conveyorMgr.emergencyStopped += OnStoppedConveyorEmergency;
                this.conveyorMgr.emergencyCancelled += OnCancelledConveyorEmergency;

				// インバーターエラー
				this.conveyorMgr.inverterErrorOccurred += OnInverterErrorOccurred;
				this.conveyorMgr.inverterErrorRestored += OnInverterErrorRestored;

				// エンコーダーエラー
				this.conveyorMgr.encoderErrorOccurred += OnEncoderErrorOccurred;
				this.conveyorMgr.encoderErrorRestored += OnEncoderErrorRestored;

				// パドル駆動イベント
				this.paddleMgr.paddleDrived += OnDrivedPaddle;

                // カウンタボード利用のための初期化 カウンタボード異常は復帰できない
                // オープン
                if (!this.counterBoard[0].open(true))
                {
                    throw new ApplicationException(String.Format("カウンタボードのオープンに失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }
				if(!this.counterBoard[1].open(true))
				{
					throw new ApplicationException(String.Format("カウンタボードのオープンに失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}
                // カウンタ設定
                if (!this.counterBoard[0].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
                {
                    throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }
				if (!this.counterBoard[0].setCounterMode(2, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
				}
				if (!this.counterBoard[0].setCounterMode(3, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
				}
				if (!this.counterBoard[0].setCounterMode(4, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
				}
				if (!this.counterBoard[1].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}
				if (!this.counterBoard[1].setCounterMode(2, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}
				// カウンタボードのイベント有効化
				if (!this.counterBoard[0].setEvent())
                {
                    throw new ApplicationException(String.Format("カウンタボードのイベント設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }
				if (!this.counterBoard[1].setEvent())
				{
					throw new ApplicationException(String.Format("カウンタボードのイベント設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}
				// カウンタボードの I/O イベント設定 & チャンネルイベント設定
				if (!(	this.counterBoard[0].setEventMask(0x0200, IOEventMask1, 1) &&
						this.counterBoard[0].setEventMask(1, EQEventMask, 1) &&
						this.counterBoard[0].setEventMask(2, EQEventMask, 1) &&
						this.counterBoard[0].setEventMask(3, EQEventMask, 1) &&
						this.counterBoard[0].setEventMask(4, EQEventMask, 1) ))
                {
                    throw new ApplicationException(String.Format("カウンタボードのイベントの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }
				// カウンタボードの I/O イベント設定 & チャンネルイベント設定
				if (!(	this.counterBoard[1].setEventMask(0x0200, IOEventMask2, 1) && 
						this.counterBoard[1].setEventMask(1, EQEventMask, 1) &&
						this.counterBoard[1].setEventMask(2, EQEventMask, 1)))
				{
					throw new ApplicationException(String.Format("カウンタボードのイベントの設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}
				// カウンタボードのデジタルフィルタ設定
				if (!(this.counterBoard[0].setFilter(0x0100, 0x8A) && this.counterBoard[0].setFilter(0x0200, 0x8A)))
                {
                    throw new ApplicationException(String.Format("カウンタボードのフィルタの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }
				if (!(this.counterBoard[1].setFilter(0x0100, 0x8A) && this.counterBoard[1].setFilter(0x0200, 0x8A)))
				{
					throw new ApplicationException(String.Format("カウンタボードのフィルタの設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}

				// 各種管理クラスの初期化
				// LED 管理クラスの初期化
				if (!this.ledMgr.initialize())
                {
                    throw new ApplicationException("LEDManager の初期化に失敗しました。");
                }
				// 信号管理クラスの初期化
				if (!this.signalManager.initialize())
				{
					throw new ApplicationException("LIBSManager の初期化に失敗しました。");
				}
				// キースイッチ管理クラスの初期化
				if (!this.keySwitchMgr.initialize())
                {
                    throw new ApplicationException("KeySwitchManager の初期化に失敗しました。");
                }
				// 非常停止スイッチ管理クラスの初期化
                if(!this.emergencyStopButtonManager.initialize())
                {
                    throw new ApplicationException("EmergencyStopButtonManager の初期化に失敗しました。");
                }
				// システムスイッチ管理クラスの初期化
                if(!this.systemSwitchManager.initialize())
                {
                    throw new ApplicationException("SystemSwitchManager の初期化に失敗しました。");
                }

                // パドル管理クラスの初期化
                if (!this.paddleMgr.initialize())
                {
                    throw new ApplicationException("PaddleManager の初期化に失敗しました。");
                }
                // コンベア管理クラスの初期化
                if (!this.conveyorMgr.initialize())
                {
                    throw new ApplicationException("ConveyorManager の初期化に失敗しました。");
                }
                // LIBS 装置管理クラスの初期化
                if (!this.libsMgr.initialize())
                {
                    throw new ApplicationException("LIBSManager の初期化に失敗しました。");
                }

				uint speed = ConveyorSettings.Instance.DataList.FirstOrDefault(d=>d.Selected).PulseFrequency;

                this.conveyorMgr.changeSpeed(speed);

				if(!Settings.Default.CounterBoardDebug)
				{ 
					// カウンタボードの入力状態を参照
					UInt32 input;
					if (this.counterBoard[0].inpputDI(out input))
					{
						// 7bit目（ポート8）が落ちている場合
						if (((input >> 7) & 0x01) == 0x00)
						{
							// 引き網スイッチが入っている状態
							OnStoppedConveyorEmergency(this, null);
						}

						// 14bit目（ポート15）が立っている場合
						if (((input >> 14) & 0x01) == 0x01)
						{
							// 非常停止状態
							OnEmergencyStopButtonOn(this, null);
						}

						// 12bit目（ポート13）が立っている場合
						if (((input >> 12) & 0x01) == 0x01)
						{
							// システムスイッチON
							OnSystemSwitchOn(this, null);
						}

						// 10bit目（ポート11）が立っている場合
						if (((input >> 10) & 0x01) == 0x01)
						{
							// エンコーダー異常
							OnEncoderErrorOccurred(this, null);
						}
					}
					else
					{
						throw new ApplicationException("カウンタボードとの接続に失敗しました。");
					}

					if (this.counterBoard[1].inpputDI(out input))
					{
						// 14bit目（ポート15）が立っている場合
						if (((input >> 14) & 0x01) == 0x01)
						{
							// インバーター異常
							OnInverterErrorOccurred(this, null);
						}
					}
					else
					{
						throw new ApplicationException("カウンタボードとの接続に失敗しました。");
					}
				}

				DateTime startTime = DateTime.Now;
				// PC 起動パルス点滅開始
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_PULSE, SignalManager.SIGNALStatus.BLINK_ON);
				while (this.signalManager.getSignalStatus(SignalManager.SIGNAL.SIGNAL_PULSE) != SignalManager.SIGNALStatus.BLINK_OFF)
				{
					Thread.Sleep(10);
					if(startTime.AddSeconds(3) < DateTime.Now)
						break;
				}

				Thread.Sleep(100);

				// PC ON 信号出力
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_PCON, SignalManager.SIGNALStatus.ON);

				UpdateStatus();

				//if (!this.libsMgr.connectLIBS())
				//{
				//	libsErrorFlag = true;
				//    throw new ApplicationException("LIBS 装置との接続に失敗しました。");
				//}
			}
            catch (ApplicationException appEx)
            {
				appLog.Error(appEx);
                // 警告を画面に表示
                this.printedMessage(appEx.Message);
            }
            catch (InverterException iEx)
            {
				appLog.Error(iEx);
				// 警告を画面に表示
				inverterErrorFlag = true;
				this.printedMessage("インバータとの通信に失敗しました");
				try
				{
					conveyorMgr.close();
				}
				catch (Exception)
				{
				}
			}
            catch (Exception ex)
            {
				appLog.Error(ex);
				// 警告を画面に表示
				this.printedMessage(ex.Message);
            }

            return true;
        }

		#endregion

		#region イベントハンドラ

		// コンベア装置の緊急停止イベント
		private void OnStoppedConveyorEmergency(object sender, EventArgs e)
        {
			this.appLog.Info("引き網緊急停止を検知しました。");

			// 緊急停止フラグを立てる
			this.emergencyRopeFlag = true;
			this.conveyorRunFlag = false;
			this.forceRunFlag = false;
		}

		// コンベア装置の緊急停止解除イベント
		private void OnCancelledConveyorEmergency(object sender, EventArgs e)
		{
			this.appLog.Info("引き網緊急停止が復帰しました。");

			// 緊急停止フラグをおとす
			this.emergencyRopeFlag = false;
		}

		// 非常停止ボタンONイベント
		private void OnEmergencyStopButtonOn(object sender, EventArgs e)
        {
			this.appLog.Info("非常停止ボタンONを検知しました。");

			// 緊急停止フラグを立てる
			this.emergencyButtonFlag = true;
			this.conveyorRunFlag = false;
			this.forceRunFlag = false;
		}

		// 非常停止ボタンOFFイベント
		private void OnEmergencyStopButtonOff(object sender, EventArgs e)
        {
			this.appLog.Info("非常停止ボタンOFFを検知しました。");

			// 緊急停止フラグをおとす
			this.emergencyButtonFlag = false;
        }

		// システムスイッチONイベント
		private void OnSystemSwitchOn(object sender , EventArgs e)
        {
			this.appLog.Info("システムスイッチONを検知しました。");

			// システムスイッチ押下フラグを立てる
			this.systemSwitchFlag = true;
        }

		// システムスイッチOFFイベント
		private void OnSystemSwitchOff(object sender , EventArgs e)
        {
			this.appLog.Info("システムスイッチOFFを検知しました。");

			// システムスイッチ押下フラグを落とす
			this.systemSwitchFlag = false;
        }

		// インバーターエラー発生イベント
		private void OnInverterErrorOccurred(object sender, EventArgs e)
		{
			this.appLog.Info("インバーターエラー入力を検知しました。");

			// インバーターエラーフラグを立てる
			this.inverterErrorFlag = true;
			this.conveyorRunFlag = false;
			this.forceRunFlag = false;
		}

		// インバーターエラー復帰イベント
		private void OnInverterErrorRestored(object sender, EventArgs e)
		{
			this.appLog.Info("インバーターエラー入力が復帰しました。");

			// インバーターエラーフラグをおとす
			this.inverterErrorFlag = false;
		}

		// エンコーダーエラー発生イベント
		private void OnEncoderErrorOccurred(object sender, EventArgs e)
		{
			this.appLog.Info("エンコーダーエラー入力を検知しました。");

			// エンコーダーエラーフラグを立てる
			this.encoderErrorFlag = true;
			this.conveyorRunFlag = false;
			this.forceRunFlag = false;
		}

		// エンコーダーエラー復帰イベント
		private void OnEncoderErrorRestored(object sender, EventArgs e)
		{
			this.appLog.Info("エンコーダーエラー入力が復帰しました。");

			// エンコーダーエラーフラグをおとす
			this.encoderErrorFlag = false;
		}

		// 分析情報受信処理
		private void OnReceivedAnalysisResult(object sender, LIBSAnalysisEventArgs e)
        {
            try
            {
				this.appLog.Info(string.Format("分析結果を受信 素材長カウンタ値:{0} グループ:{1}", e.Counter, e.Group));

                // 装置状態が Ready 以外は受け付けない
                if (this.status != SorterStatus.RUN)
                {
                    throw new ApplicationException("運転状態以外で分析結果を受信しました。");
                }

                uint counter = 0;

				if (e.Result)
                {
					if((int)e.Group < 100)
                    {
						// 選別対象かどうかを取得して動作を決定する
						int nozzleNumber = materialSetting.getNozzleNumber((int)e.Group);
						if (nozzleNumber > 0)
						{
							if ( nozzleNumber <= 4 )
								this.counterBoard[0].getCounter(nozzleNumber, out counter);
							else
								this.counterBoard[1].getCounter(nozzleNumber - 4, out counter);

							MaterialData materialData = new MaterialData(counter, e.Counter, (int)e.Group);
							materialData.IsDriving = true;
							materialData.IsSet = true;

							//this.appLog.Debug($"データキュー登録 駆動エアノズル番号：{materialData.nozzleNumber} 現在カウント：{counter} 駆動カウント：{materialData.DrivingCounter}");

							// データキューが管理値の最大を越えている
							if (!this.materialQueue[materialData.nozzleNumber - 1].enqueue(materialData))
							{
								this.printedMessage("管理可能な素材データの最大値を越えています。");
							}
						}
                        else
                        {
                            //Console.WriteLine("選別対象以外を認識");
                        }
                    }
                }

                // LIBS 装置から分析結果を受信
                if (analysisResultReceived != null)
                {
					analysisResultReceived(this, e);
                }
            }
            catch (ApplicationException appEx)
            {
				appLog.Error(appEx);
				conveyorRunFlag = false;
				// 警告を画面に表示
				this.printedMessage(appEx.Message);
            }
            catch (Exception ex)
            {
				appLog.Error(ex);
				conveyorRunFlag = false;
				// 警告を画面に表示
				this.printedMessage(ex.Message);
            }
        }

		// パドル駆動イベント
		private void OnDrivedPaddle(object sender, PaddleDriveEventArgs e)
        {
            if (this.paddleDrived != null)
            {
                this.paddleDrived(this, e);
            }
        }

        // LIBS 装置の状態変更イベント
        private void OnChangedLIBSReady(object sender, LIBSReadyEventArgs e)
        {
			if(this.Status == SorterStatus.RUN)
			{
				if(libsRunReadyWaitFlag)
				{
					if(e.Ready)
					{
						this.appLog.Info("LIBS Ready 状態を検知しました");
						libsRunReadyWaitFlag = false;
						if (Settings.Default.CounterBoardDebug)
						{
							counterBoard[0].StartEncoderEmulate();
							counterBoard[1].StartEncoderEmulate();
						}
					}
				}
				else 
				{
					if (!e.Ready)
					{
						this.appLog.Info("LIBS Not Ready 状態を検知しました");
						libsReadyDown = true;
						conveyorRunFlag = false;
					}
				}
			}
		}

		//LIBS 装置の接続状態変化のイベント
		private void OnChangedLIBSConnection(object sender, LIBSConnectionEventArgs e)
		{
			if (e.IsConnect != libsConnectFlag)
				this.appLog.Info($"LIB 装置の接続 {e.IsConnect.ToString()} を検知しました");

			if (!e.IsConnect)
				conveyorRunFlag = false;

			// 初回接続時、フラグを落とす
			if(e.IsConnect && libsStartUpWaitFlag)
				libsStartUpWaitFlag = false;

			libsConnectFlag = e.IsConnect;

			if (this.libsConnectionChanged != null)
			{
				this.libsConnectionChanged(this, e);
			}
		}

		// キースイッチ ON のイベント
		private void OnSystemStarted(object sender, EventArgs e)
        {
			this.appLog.Info($"運転開始 ON を検知しました");

			if (Status == SorterStatus.READY || Status == SorterStatus.TEST_READY)
                conveyorRunFlag = true;
			else
				this.appLog.Info($"運転可能状態ではありません");
		}

		// キースイッチ OFF のイベント
		private void OnSystemStopped(object sender, EventArgs e)
		{
			this.appLog.Info($"運転開始 OFF を検知しました");

			conveyorRunFlag = false;
		}

		#endregion

		#region 非公開メソッド

		// 出力コールバック
		private void printedMessage(String message)
		{
			// メッセージの出力
			if (messagePrinted != null)
			{
				messagePrinted(this, new MessageEventArgs(message));
			}
		}

		// 状態の更新
		private void UpdateStatus()
		{
			lock (lockObj)
			{
				// ------------------------------------------------------------
				// ---- 表示灯
				// ------------------------------------------------------------

				// 表示灯 赤判定
				if (emergencyRopeFlag || emergencyButtonFlag || inverterErrorFlag || libsErrorFlag || encoderErrorFlag)
					ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_RED, LEDManager.LEDStatus.ON);
				else
					ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_RED, LEDManager.LEDStatus.OFF);

				// 表示灯 緑・黄 判定
				if (conveyorRunFlag || forceRunFlag)
				{
					ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_GREEN, LEDManager.LEDStatus.ON);
					ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_YELLOW, LEDManager.LEDStatus.OFF);
				}
				else
				{
					ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_GREEN, LEDManager.LEDStatus.OFF);
					ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_YELLOW, LEDManager.LEDStatus.ON);
				}

				// ------------------------------------------------------------
				// ---- 信号出力
				// ------------------------------------------------------------

				// 非常停止出力
				if (emergencyRopeFlag || emergencyButtonFlag)
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_EMG, SignalManager.SIGNALStatus.ON);
				else
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_EMG, SignalManager.SIGNALStatus.OFF);

				// インバーター異常出力
				if (inverterErrorFlag)
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_INV, SignalManager.SIGNALStatus.ON);
				else
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_INV, SignalManager.SIGNALStatus.OFF);

				// LIBS異常出力
				if (libsErrorFlag)
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_LIBS, SignalManager.SIGNALStatus.ON);
				else
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_LIBS, SignalManager.SIGNALStatus.OFF);


				// コンベア動作動作・停止出力
				if (conveyorRunFlag || forceRunFlag)
				{
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_RUN, SignalManager.SIGNALStatus.ON);
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_STOP, SignalManager.SIGNALStatus.OFF);
				}
				else
				{
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_RUN, SignalManager.SIGNALStatus.OFF);
					signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_STOP, SignalManager.SIGNALStatus.ON);
				}

				// ------------------------------------------------------------
				// ---- 制御盤LED
				// ------------------------------------------------------------

				// 非常停止LED
				if (emergencyRopeFlag || emergencyButtonFlag)
					ledMgr.changeLEDStatus(LEDManager.LED.LED_ERROR, LEDManager.LEDStatus.ON);
				else
					ledMgr.changeLEDStatus(LEDManager.LED.LED_ERROR, LEDManager.LEDStatus.OFF);

				// LIBS LED
				if (libsConnectFlag)
					ledMgr.changeLEDStatus(LEDManager.LED.LED_LIBS, LEDManager.LEDStatus.ON);
				else
					ledMgr.changeLEDStatus(LEDManager.LED.LED_LIBS, LEDManager.LEDStatus.OFF);

				// コンベア動作LED
				if (conveyorRunFlag || forceRunFlag)
					ledMgr.changeLEDStatus(LEDManager.LED.LED_RUN, LEDManager.LEDStatus.ON);
				else
					ledMgr.changeLEDStatus(LEDManager.LED.LED_RUN, LEDManager.LEDStatus.OFF);

				// システム起動 LED
				if (systemSwitchFlag)
					ledMgr.changeLEDStatus(LEDManager.LED.LED_SYSTEM, LEDManager.LEDStatus.ON);
				else
					ledMgr.changeLEDStatus(LEDManager.LED.LED_SYSTEM, LEDManager.LEDStatus.OFF);

				// ------------------------------------------------------------
				// ---- ステータス 
				// ------------------------------------------------------------
				SorterStatus beforeState = Status;

				// 非常停止
				if (emergencyRopeFlag || emergencyButtonFlag)
					this.Status = SorterStatus.EMERGENCY;

				// 強制運転中
				else if (forceRunFlag)
					this.Status = SorterStatus.FORCE_RUN;

				// エラー
				else if (inverterErrorFlag || encoderErrorFlag || libsErrorFlag )
					this.Status = SorterStatus.ERROR;

				// LIBS起動待ち
				else if(libsStartUpWaitFlag)
					this.Status = SorterStatus.NOT_READY;

				// テスト運転中 or 運転中
				else if (conveyorRunFlag)
				{
					// RUN
					if (systemSwitchFlag)
					{
						if(beforeState != SorterStatus.TEST_RUN)
						{
							// コンベア開始
							this.Status = SorterStatus.RUN;
						}
					}

					// TEST RUN
					else
					{
						if (beforeState == SorterStatus.RUN)
						{
                            try
                            {
                                // LIBS 装置のインターロック
                                this.libsMgr.lockInterlock();
                            }
                            catch (Exception ex)
                            {
                                appLog.Error(ex);
                                // 警告を画面に表示
                                this.printedMessage(ex.Message);
                            }

                            try
                            {
                                if (libsConnectFlag)
                                {
                                    // LIBS 装置に運転停止コマンドを送信
                                    this.libsMgr.stopLIBS();
                                }
                            }
                            catch (Exception ex)
                            {
                                appLog.Error(ex);
                                // 警告を画面に表示
                                this.printedMessage(ex.Message);
								libsMgr.closeLIBS();
                            }
                        }
						
						// コンベア開始
						this.Status = SorterStatus.TEST_RUN;
					}
				}


				// テスト運転可
				else if (!systemSwitchFlag)
				{
					this.Status = SorterStatus.TEST_READY;
				}

				// 運転可
				else
				{
					this.Status = SorterStatus.READY;
				}

				// ------------------------------------------------------------
				// ---- メッセージ ----
				// ------------------------------------------------------------

				if (emergencyButtonFlag && emergencyRopeFlag)
					this.printedMessage("緊急停止ボタン、緊急停止引き網スイッチがONです");
				else if (emergencyButtonFlag)
					this.printedMessage("緊急停止ボタンが押されました");
				else if (emergencyRopeFlag)
					this.printedMessage("緊急停止引き網スイッチがONです");
				else if(inverterErrorFlag && encoderErrorFlag)
					this.printedMessage("インバーターエラー、エンコーダーエラーが発生しました");
				else if (inverterErrorFlag)
					this.printedMessage("インバーターエラーが発生しました");
				else if (encoderErrorFlag)
					this.printedMessage("エンコーダーエラーが発生しました");
				else if(libsStartUpWaitFlag)
					this.printedMessage("LIBSを起動して下さい");
				else if (!libsConnectFlag)
					this.printedMessage("LIBSとの通信に失敗しました");
				else if(libsReadyTimeout)
					this.printedMessage("LIBSがReady状態ではありません");
				else if (libsReadyDown)
					this.printedMessage("LIBSがNotReady状態になりました");
				else if(!systemSwitchFlag)
					this.printedMessage("システム起動スイッチがOFFです");
				else if(libsRunReadyWaitFlag)
					this.printedMessage("LIBS Ready 待機中");
				else
					this.printedMessage("");


				// ------------------------------------------------------------
				// ---- コンベア動作 ----
				// ------------------------------------------------------------

				// コンベア動作停止→開始
				if (beforeState == SorterStatus.RUN || 
					beforeState == SorterStatus.TEST_RUN ||
					beforeState == SorterStatus.FORCE_RUN)
				{
					// コンベア動作開始
					if (Status != SorterStatus.RUN && 
						Status != SorterStatus.TEST_RUN &&
						Status != SorterStatus.FORCE_RUN)
					{
						stopLibsSorter();
					}
				}

				// コンベア動作中→停止
				if (beforeState != SorterStatus.RUN && 
					beforeState != SorterStatus.TEST_RUN &&
					beforeState != SorterStatus.FORCE_RUN)
				{
					// コンベア動作停止
					if (Status == SorterStatus.RUN || 
						Status == SorterStatus.TEST_RUN ||
						Status == SorterStatus.FORCE_RUN)
					{
						startLibsSorter();
					}
				}
			}
		}

		// LIBS 装置の運転開始
		private void startLibsSorter()
        {
            try
            {
				this.appLog.Info("LIBS ソータ運転開始");

				// 運転不可判定
				if (emergencyButtonFlag || emergencyRopeFlag || encoderErrorFlag || inverterErrorFlag)
                {
                    throw new ApplicationException("運転可能状態ではありません。");
                }

                // 制御 PC のカウンタリセット
				for(int ch = 1 ; ch <= 6 ; ch++)
				{ 
					if(ch <= 4)
					{ 
						if (!this.counterBoard[0].resetCounter(ch))
						{
							throw new ApplicationException("カウンタボードのリセットに失敗しました。");
						}
					}
					else
					{
						if (!this.counterBoard[1].resetCounter(ch-4))
						{
							throw new ApplicationException("カウンタボードのリセットに失敗しました。");
						}
					}
				}

                // カウンタ方式再設定
                if (!this.counterBoard[0].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
                {
                    throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }
				if (!this.counterBoard[0].setCounterMode(2, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
				}
				if (!this.counterBoard[0].setCounterMode(3, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
				}
				if (!this.counterBoard[0].setCounterMode(4, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
				}
				if (!this.counterBoard[1].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}
				if (!this.counterBoard[1].setCounterMode(2, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
				{
					throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[1].ErrorCode));
				}

				if (Status == SorterStatus.RUN)
				{
					libsRunReadyWaitFlag = true;

					// LIBS 装置にカウンタリセットコマンドを送信
					this.libsMgr.resetCounter();

					// LIBS が本当にリセットされたか返答が無いので 待ちを挟む
					Thread.Sleep(100);
				}
				
				// 速度の再セット
				uint speed = ConveyorSettings.Instance.DataList.FirstOrDefault(d => d.Selected).PulseFrequency;
				this.conveyorMgr.changeSpeed(speed);

				// コンベア装置の搬送開始
				this.conveyorMgr.startConveyor();

				// コンベア装置のインバータ出力チェック
				while (!this.conveyorMgr.checkOutputFrequency())
                {
                    // 10ms Sleep
                    Thread.Sleep(10);
                }

				if(Status == SorterStatus.RUN)
				{ 
					// LIBS 装置インターロック解除
					this.libsMgr.unlockInterlock();

					DateTime checkTime = DateTime.Now;

					while(libsRunReadyWaitFlag)
					{
						if(checkTime.AddMilliseconds(Settings.Default.LIBSReadyTimeout) <= DateTime.Now)
						{
							libsRunReadyWaitFlag = false;
							libsReadyTimeout = true;
							conveyorRunFlag = false;
							return;
						}
						Thread.Sleep(10);
					}

					paddleMgr.clearMaterialData();

					if (Settings.Default.CounterBoardDebug)
					{ 
						counterBoard[0].StartEncoderEmulate();
						counterBoard[1].StartEncoderEmulate();
					}

					// LIBS 装置運転開始
					this.libsMgr.startLIBS();
                }
			}
            catch (ApplicationException appEx)
            {
				appLog.Error(appEx);
				// 警告を画面に表示
				conveyorRunFlag = false;
				this.printedMessage(appEx.Message);
            }
            catch (InverterException iEx)
            {
				appLog.Error(iEx);
				// 警告を画面に表示
				inverterErrorFlag = true;
				conveyorRunFlag = false;
				this.printedMessage("インバータとの通信に失敗しました");
				try
				{ 
					conveyorMgr.close();
				}
				catch(Exception)
				{
				}
			}
            catch (Exception ex)
            {
				appLog.Error(ex);
				// 警告を画面に表示
				conveyorRunFlag = false;
				this.printedMessage(ex.Message);
            }
        }

		// LIBS 装置の運転停止
		private void stopLibsSorter()
        {
			try
			{
				this.appLog.Info("LIBS ソータ運転停止");

				libsRunReadyWaitFlag = false;

				if (Settings.Default.CounterBoardDebug)
				{
					counterBoard[0].StopEncoderEmulate();
					counterBoard[1].StopEncoderEmulate();
				}

				try
                {
                    // コンベア装置の搬送停止
                    this.conveyorMgr.stopConveyor();
                }
                catch (Exception ex)
                {
                    appLog.Error(ex);
                    inverterErrorFlag = true;
					// 警告を画面に表示
					this.printedMessage("インバータとの通信に失敗しました");
					try
					{
						conveyorMgr.close();
					}
					catch (Exception)
					{
					}
				}


                // エラーが発生してもコンベアを停止するように
                // LIBS STOP 命令のみでキャッチする
                try
				{ 
					if (libsConnectFlag)
					{
						// LIBS 装置に運転停止コマンドを送信
						this.libsMgr.stopLIBS();
					}
				}
				catch (Exception ex)
				{
					appLog.Error(ex);
					// 警告を画面に表示
					this.printedMessage(ex.Message);
					libsMgr.closeLIBS();
				}

				try
				{
					// LIBS 装置のインターロック
					this.libsMgr.lockInterlock();
				}
				catch (Exception ex)
				{
					appLog.Error(ex);
					// 警告を画面に表示
					this.printedMessage(ex.Message);
				}

				// 保持素材データの破棄
				this.materialQueue.ForEach(q=>q.clearMaterialData());


				if(Settings.Default.CounterBoardDebug)
				{
					counterBoard[0].StopEncoderEmulate();
					counterBoard[1].StopEncoderEmulate();
				}

			}
			catch (ApplicationException appEx)
			{
				appLog.Error(appEx);
				// 警告を画面に表示
				conveyorRunFlag = false;
				this.printedMessage(appEx.Message);
			}
			catch (InverterException iEx)
			{
				appLog.Error(iEx);
				// 警告を画面に表示
				inverterErrorFlag = true;
				conveyorRunFlag = false;
				this.printedMessage("インバータとの通信に失敗しました");
				try
				{
					conveyorMgr.close();
				}
				catch (Exception)
				{
				}
			}
			catch (Exception ex)
			{
				appLog.Error(ex);
				// 警告を画面に表示
				conveyorRunFlag = false;
				this.printedMessage(ex.Message);
			}
		}

		#endregion

		#region 公開メソッド

		// 搬送速度の変更
		public void changeConveyorSpeed(uint speed)
        {
            try
            {
                // 装置状態が READY か NOT_READY 以外は受け付けない
                if (conveyorRunFlag)
                {
                    throw new ApplicationException("停止状態以外での速度の変更は行えません。");
                }

                this.conveyorMgr.changeSpeed(speed);

                this.appLog.Info("コンベア搬送速度変更");

            }
            catch (ApplicationException appEx)
            {
				appLog.Error(appEx);
				// 警告を画面に表示
				this.printedMessage(appEx.Message);
            }
            catch (Exception ex)
            {
				appLog.Error(ex);
				// 警告を画面に表示
				this.printedMessage(ex.Message);
            }
        }

		// インバーターエラーリセット
		public void ResetInverterError()
		{
			this.appLog.Info("インバーターエラーリセットを実施します");

			if (inverterErrorFlag)
			{
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_INV_RST, SignalManager.SIGNALStatus.ON);
				Thread.Sleep(200);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_INV_RST, SignalManager.SIGNALStatus.OFF);

				if (this.counterBoard[1].inpputDI(out var input))
				{
					// インバータエラー入力が落ちている場合
					if (((input >> 14) & 0x01) == 0x00)
					{
						inverterErrorFlag = false;

						this.appLog.Info("インバーターエラー入力がOFFなので、インバーターエラーを解除します");
					}
				}
			}
		}

		// エンコーダーエラーリセット
		public void ResetEncoderError()
		{
			this.appLog.Info("エンコーダーエラーリセットを実施します");

			if (encoderErrorFlag)
			{
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ENC_RST, SignalManager.SIGNALStatus.ON);
				Thread.Sleep(200);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ENC_RST, SignalManager.SIGNALStatus.OFF);
			}
		}

		// LIBSエラーリセット
		public void ResetLibsError()
		{
			libsReadyDown = false;
			libsReadyTimeout = false;
			UpdateStatus();
		}

        // LIBS コントローラの終了
        public void exitLIBSController()
        {
            try
            {
				this.appLog.Info("LIBS コントローラ 終了処理を実施します");

				// スレッドの停止
				this.paddleMgr.exitPaddleManage();

				try
				{ 
					if (libsConnectFlag)
					{
						// LIBS 装置に運転停止コマンドを送信
						this.libsMgr.stopLIBS();

						// LIBS 装置のネットワーク切断
						this.libsMgr.closeLIBS();
					}
				}
				catch (Exception)
				{
				}

				try
				{
					// コンベア装置の搬送停止
					this.conveyorMgr.stopConveyor();
				}
				catch (Exception)
				{
				}

				// LIBS 装置のインターロック
				this.libsMgr.lockInterlock();

                // LED 出力の初期化
                this.ledMgr.changeLEDStatus(LEDManager.LED.LED_RUN, LEDManager.LEDStatus.OFF);
                this.ledMgr.changeLEDStatus(LEDManager.LED.LED_LIBS, LEDManager.LEDStatus.OFF);
				this.ledMgr.changeLEDStatus(LEDManager.LED.LED_SYSTEM, LEDManager.LEDStatus.OFF);
				this.ledMgr.changeLEDStatus(LEDManager.LED.LED_ERROR, LEDManager.LEDStatus.OFF);
				this.ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_GREEN,LEDManager.LEDStatus.OFF);
				this.ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_YELLOW, LEDManager.LEDStatus.OFF);
				this.ledMgr.changeLEDStatus(LEDManager.LED.LIGHT_RED, LEDManager.LEDStatus.OFF);

				// 信号出力の初期化
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_EMG,SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_INV, SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_ERROR_LIBS, SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_INV_RST, SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_RUN, SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_STOP, SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_PCON, SignalManager.SIGNALStatus.OFF);
				this.signalManager.changeSignalStatus(SignalManager.SIGNAL.SIGNAL_PULSE, SignalManager.SIGNALStatus.OFF);




				// カウンタボード切断
				this.counterBoard.ForEach(c => c.close());


            }
            catch (Exception)
            {
                // 終了時にしか呼ばないため特に何もしない
            }
        }

		// 強制運転開始
		public void forceStart()
		{
			this.appLog.Info("強制運転 ON を検知しました");

			if (!conveyorRunFlag && 
				!inverterErrorFlag && 
				!encoderErrorFlag &&
				!emergencyButtonFlag &&
				!emergencyRopeFlag &&
				(!libsConnectFlag || libsReadyTimeout || libsReadyDown))
				this.forceRunFlag = true;
			else
				this.appLog.Info("強制運転可能な状態ではありません");
		}

		// 強制運転停止
		public void forceStop()
		{
			this.appLog.Info("強制運転 OFF を検知しました");

			this.forceRunFlag = false;
		}

		#endregion
    }
}
