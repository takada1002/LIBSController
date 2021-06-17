using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLibrary;
using MelsecInverterLib;
using LIBSController.Properties;
using CounterBoardLib;
using System.Drawing;

namespace LIBSController
{
    // コンベア管理クラス
    public class ConveyorManager
    {
        // アプリケーションログ
        private ApplicationLog appLog = null;

        // 動作周波数値
        private UInt32 frequency = 0;
        public UInt32 Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        // 緊急停止イベント
        public event EventHandler<EventArgs> emergencyStopped;
        // 緊急停止解除イベント
        public event EventHandler<EventArgs> emergencyCancelled;
		// インバーター異常発生イベント
		public event EventHandler<EventArgs> inverterErrorOccurred;
		// インバーター異常復帰イベント
		public event EventHandler<EventArgs> inverterErrorRestored;
		// エンコーダ―異常発生イベント
		public event EventHandler<EventArgs> encoderErrorOccurred;
		// エンコーダ―異常復帰イベント
		public event EventHandler<EventArgs> encoderErrorRestored;

		// インバータとのコミュニケータ
		private ICommunicator inverterComm = null;
        // インバータ通信の実装
        private InverterProtocol inverterProtocol = null;
        // カウンタボード
        private List<CounterBoard> counterBoard = null;

        // 初期化
        public Boolean initialize()
        {
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();

			if (!Settings.Default.ConveyorDebug)
			{ 
				// RS232C によるインバータとの通信
				this.inverterComm = new RS232Communicator(Settings.Default.COMPort,
													 (RS232Communicator.Baudrate)Settings.Default.BaudRate,
													 Settings.Default.Parity,
													 Settings.Default.StopBits,
													 Settings.Default.InverterTimeout,
													 Settings.Default.InverterTimeout);

				this.inverterProtocol = new InverterProtocol(inverterComm, Settings.Default.InverterNo);
				this.inverterProtocol.open();
			}

            // カウンタボードのインスタンス取得
            this.counterBoard = CounterBoard.Instance;
            this.counterBoard[0].ioInterrupted += OnInterruptedDio1;
			this.counterBoard[1].ioInterrupted += OnInterruptedDio2;

			this.appLog.Info("初期化完了 ---コンベア管理---");

            return true;
        }

        // 接続オープン
        public void open()
        {
			if(Settings.Default.ConveyorDebug)
				return;

            // 接続を開く
            this.inverterProtocol.open();

            try
            {
                // インバータの制御をネットワークモードに変更
                this.inverterProtocol.setRunningMode(InverterProtocol.RunningMode.NetworkMode);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

		// 接続クローズ
		public void close()
		{
			if (Settings.Default.ConveyorDebug)
				return;

			// 接続を開く
			this.inverterProtocol.close();

		}

		// 汎用入力の割り込みイベント
		private void OnInterruptedDio1(object sender, CounterBoardEventArgs e)
        {
            // 緊急停止信号の受け取り
            // SU_IN8 SD_IN8 のフラグが立っているか
            if ((e.EventMask & (uint)0x00000080) > 0)
            {
                // 立ち上がりで緊急停止信号受け取り
                if (emergencyStopped != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
                        // 緊急停止イベントをコール
                        emergencyStopped(this, new EventArgs());
                    });
                }
            }
            else if ((e.EventMask & (uint)0x00800000) > 0)
            {
                // 立ち下がりで緊急停止信号解除
                if (emergencyCancelled != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
                        // 緊急停止解除イベントをコール
                        emergencyCancelled(this, new EventArgs());
                    });
                }
            }

			// 緊急停止信号の受け取り
			// SU_IN8 SD_IN11 のフラグが立っているか
			if ((e.EventMask & (uint)0x00000400) > 0)
			{
				// 立ち上がりでエンコーダー異常復帰
				if (encoderErrorRestored != null)
				{
					Task tsk = Task.Factory.StartNew(() =>
					{
						// エンコーダー異常復帰イベントをコール
						encoderErrorRestored(this, new EventArgs());
					});
				}
			}
			else if ((e.EventMask & (uint)0x04000000) > 0)
			{
				// 立ち下がりでエンコーダー異常検知
				if (encoderErrorOccurred != null)
				{
					Task tsk = Task.Factory.StartNew(() =>
					{
						// エンコーダー異常通知イベントをコール
						encoderErrorOccurred(this, new EventArgs());
					});
				}
			}
		}

		// 汎用入力の割り込みイベント
		private void OnInterruptedDio2(object sender, CounterBoardEventArgs e)
		{
			// インバーターエラー信号の受け取り
			// SU_IN15 SD_IN15 のフラグが立っているか
			if ((e.EventMask & (uint)0x00004000) > 0)
			{
				// 立ち上がりで復帰
				if (inverterErrorRestored != null)
				{
					Task tsk = Task.Factory.StartNew(() =>
					{
						// 緊急停止イベントをコール
						inverterErrorRestored(this, new EventArgs());
					});
				}
			}
			else if ((e.EventMask & (uint)0x40000000) > 0)
			{
				// 立ち下がりでエラー
				if (inverterErrorOccurred != null)
				{
					Task tsk = Task.Factory.StartNew(() =>
					{
						// 緊急停止解除イベントをコール
						inverterErrorOccurred(this, new EventArgs());
					});
				}
			}
		}


		// 搬送開始
		public void startConveyor()
        {
			if (Settings.Default.ConveyorDebug)
			{
				ConveyorDebugView.Instance.BeginInvoke(new Action(()=>{
					ConveyorDebugView.Instance.StatusText = "運転中";
					ConveyorDebugView.Instance.StatusColor = Color.Lime;
				}));
				return;
			}

			try
            {
                // 正転で運転指令
                this.inverterProtocol.setRunning(InverterProtocol.RunningCommand.STR, false);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // 搬送停止
        public void stopConveyor()
        {
			if (Settings.Default.ConveyorDebug)
			{
				ConveyorDebugView.Instance.BeginInvoke(new Action(() => {
					ConveyorDebugView.Instance.StatusText = "停止";
					ConveyorDebugView.Instance.StatusColor = Color.Red;
				}));
				return;
			}

			try
            {
                // 停止
                this.inverterProtocol.setRunning(InverterProtocol.RunningCommand.STOP, false);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // 搬送速度の変更
        public void changeSpeed(uint speed)
        {
			if (Settings.Default.ConveyorDebug)
				return;

			try
            {
                // インバータの出力周波数変更
                this.inverterProtocol.setFrequency(speed, true);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // 目標周波数に到達しているかチェック
        public Boolean checkOutputFrequency()
        {
			if (Settings.Default.ConveyorDebug)
				return true;

			Boolean result = false;

            try
            {
                UInt32 outFrequency = this.inverterProtocol.getFrequency();

                // 周波数上限下限 要調整 今のところ 1% 許容 (カタログスペック的には 0.01 % になっている)
                UInt32 upperFrequency = (UInt32)(outFrequency + (outFrequency * Settings.Default.FrequencyError));
                UInt32 lowerFrequency = (UInt32)(outFrequency - (outFrequency * Settings.Default.FrequencyError));

                // 許容範囲に収まる場合に安定とみなす
                if ((upperFrequency > outFrequency) && (lowerFrequency < outFrequency))
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
