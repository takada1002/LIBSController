using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLibrary;
using MelsecInverterLib;
using CounterBoardLib;
using LIBSController.Properties;

namespace LIBSController
{
    // 信号 管理クラス
    class SignalManager
    {
        // アプリケーションログ
        private ApplicationLog appLog = null;

        // 信号 一覧
        public enum SIGNAL : int
        {
			SIGNAL_RUN = 0,			// 運転中
			SIGNAL_STOP = 1,		// 停止中
			SIGNAL_ERROR_EMG = 2,	// 非常停止中
			SIGNAL_ERROR_INV = 3,	// インバーター異常
			SIGNAL_ERROR_LIBS = 4,  // LIBS異常
			SIGNAL_ENC_RST = 7,		// エンコーダー異常リセット
			SIGNAL_PULSE = 11,		// 0.5Hzパルス出力
			SIGNAL_PCON = 12,		// PC起動信号
			SIGNAL_INV_RST = 13,	// インバーター異常リセット
		}

        // LED 状態
        public enum SIGNALStatus
		{
            OFF = 0,            // 消灯
            ON = 1,             // 点灯
            BLINK_OFF = 2,      // 点滅 OFF
            BLINK_ON = 3,       // 点滅 ON
        }

        // LED 点灯用タイマー
        private System.Timers.Timer blinkTimer = null;

        // カウンタボード
        private List<CounterBoard> counterBoard = null;

        // LED 状態 (初期値は全OFF)
        private Dictionary<SIGNAL, SIGNALStatus> signalArray = new Dictionary<SIGNAL, SIGNALStatus>()
                                                      {
                                                          {SIGNAL.SIGNAL_RUN,		SIGNALStatus.OFF},
                                                          {SIGNAL.SIGNAL_STOP,		SIGNALStatus.OFF},
                                                          {SIGNAL.SIGNAL_ERROR_EMG, SIGNALStatus.OFF},
														  {SIGNAL.SIGNAL_ERROR_INV, SIGNALStatus.OFF},
														　{SIGNAL.SIGNAL_ERROR_LIBS,SIGNALStatus.OFF},
														  {SIGNAL.SIGNAL_ENC_RST,	SIGNALStatus.OFF},
														  {SIGNAL.SIGNAL_PULSE,		SIGNALStatus.OFF},
														  {SIGNAL.SIGNAL_PCON,		SIGNALStatus.OFF},
														  {SIGNAL.SIGNAL_INV_RST,	SIGNALStatus.OFF},
													  };

        // lock オブジェクト
        private Object lockObj = new Object();

        // 初期化
        public Boolean initialize()
        {
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();

            // カウンターボードのインスタンスを取得
            counterBoard = CounterBoard.Instance;

            // 点滅用タイマーの作成
            blinkTimer = new System.Timers.Timer();
            blinkTimer.Elapsed += OnBlinkTimerElapsed;
            blinkTimer.Interval = Settings.Default.SignalBlinkInterval;

            blinkTimer.AutoReset = true;
            blinkTimer.Start();

            this.appLog.Info("初期化完了 ---信号管理---");

            return true;
        }

        // タイマーイベント
        private void OnBlinkTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 点灯状態の LED の有無
            Boolean isBlink = false;

            Dictionary<SIGNAL, SIGNALStatus> signalArrayCopy = new Dictionary<SIGNAL, SIGNALStatus>();

            lock (lockObj)
            {
                // LED の状態チェック
                foreach (KeyValuePair<SIGNAL, SIGNALStatus> signal in this.signalArray)
                {
					SIGNALStatus status = signal.Value;

                    // 点滅状態がセットされている LED があれば
                    if ((signal.Value == SIGNALStatus.BLINK_OFF) || (signal.Value == SIGNALStatus.BLINK_ON))
                    {
                        status = (signal.Value == SIGNALStatus.BLINK_OFF) ? SIGNALStatus.BLINK_ON : SIGNALStatus.BLINK_OFF;
                        isBlink = true;
                    }

                    signalArrayCopy.Add(signal.Key, status);
                }

				this.signalArray = signalArrayCopy;

				// 点灯状態の有無があれば I/O 出力を変更
				if (isBlink)
				{
					this.outputSignal();
				}
			}
        }

        // SIGNAL の状態を変更
        public void changeSignalStatus(SIGNAL signal, SIGNALStatus status)
        {
            lock (lockObj)
            {
                // 点滅状態から点滅状態へは入れ替えない
                if ((status == SIGNALStatus.BLINK_OFF || status == SIGNALStatus.BLINK_ON) &&
                    (this.signalArray[signal] == SIGNALStatus.BLINK_OFF || this.signalArray[signal] == SIGNALStatus.BLINK_ON))
                {
                    // LED 状態を変更しない
                }
                else
                {
					if(this.signalArray[signal] != status)
                    {
						this.signalArray[signal] = status;
						this.outputSignal();
					}
                }
            }
        }

		// SIGNAL の状態を取得
		public SIGNALStatus getSignalStatus(SIGNAL signal)
		{
			return this.signalArray[signal];
		}

        // SIGNAL 出力関数 (SIGNAL 状態に変更があった場合に呼ぶ)
        public void outputSignal()
        {
            // SIGNAL の状態を更新
            foreach (KeyValuePair<SIGNAL, SIGNALStatus> signal in this.signalArray)
            {
				// SIGNAL 状態を汎用出力値にセット
				switch (signal.Value)
				{
					case SIGNALStatus.OFF:
					case SIGNALStatus.BLINK_OFF:
						this.counterBoard[1].setOutput((CounterBoard.IOPort)signal.Key, false);
						break;
					case SIGNALStatus.ON:
					case SIGNALStatus.BLINK_ON:
						this.counterBoard[1].setOutput((CounterBoard.IOPort)signal.Key, true);
						break;
				}
            }

            // 出力
			this.counterBoard[1].outputDO();
		}
    }
}
