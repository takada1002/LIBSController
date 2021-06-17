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
    // LED 管理クラス
    class LEDManager
    {
        // アプリケーションログ
        private ApplicationLog appLog = null;

        // LED 一覧
        public enum LED : int
        {
            LED_RUN = 9,    // 運転
            LED_LIBS = 11,	// LIBS 装置
            LED_ERROR = 13, // 非常停止
            LED_SYSTEM = 15,// システム

			LIGHT_RED = 108,      // 表示灯 赤
			LIGHT_YELLOW = 109,   // 表示灯 黄
			LIGHT_GREEN = 110    // 表示灯 緑
		}

        // LED 状態
        public enum LEDStatus
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

        // LED 状態
        private Dictionary<LED, LEDStatus> ledArray = new Dictionary<LED, LEDStatus>()
                                                      {
                                                          {LED.LED_RUN,		LEDStatus.OFF},
                                                          {LED.LED_LIBS,	LEDStatus.OFF},
                                                          {LED.LED_ERROR,	LEDStatus.OFF},
														  {LED.LED_SYSTEM,	LEDStatus.OFF},
														　{LED.LIGHT_RED,	LEDStatus.OFF},
														  {LED.LIGHT_YELLOW,LEDStatus.ON},
														  {LED.LIGHT_GREEN,	LEDStatus.OFF},
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
            blinkTimer.Interval = Settings.Default.LEDBlinkInterval;

            blinkTimer.AutoReset = true;
            blinkTimer.Start();

			outputLED();

			this.appLog.Info("初期化完了 ---LED管理---");

            return true;
        }

        // タイマーイベント
        private void OnBlinkTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 点灯状態の LED の有無
            Boolean isBlink = false;

            Dictionary<LED, LEDStatus> ledArrayCopy = new Dictionary<LED, LEDStatus>();

            /*foreach (KeyValuePair<LED, LEDStatus> led in this.ledArray)
            {
                // 点滅状態がセットされている LED があれば
                if ((led.Value == LEDStatus.BLINK_OFF) || (led.Value == LEDStatus.BLINK_ON))
                {
                    this.ledArray[led.Key] = (led.Value == LEDStatus.BLINK_OFF) ? LEDStatus.BLINK_ON : LEDStatus.BLINK_OFF;
                    isBlink = true;
                }
            }*/
            lock (lockObj)
            {
                // LED の状態チェック
                foreach (KeyValuePair<LED, LEDStatus> led in this.ledArray)
                {
                    LEDStatus status = led.Value;

                    // 点滅状態がセットされている LED があれば
                    if ((led.Value == LEDStatus.BLINK_OFF) || (led.Value == LEDStatus.BLINK_ON))
                    {
                        status = (led.Value == LEDStatus.BLINK_OFF) ? LEDStatus.BLINK_ON : LEDStatus.BLINK_OFF;
                        isBlink = true;
                    }

                    ledArrayCopy.Add(led.Key, status);
                }
            }

            this.ledArray = ledArrayCopy;

            // 点灯状態の有無があれば I/O 出力を変更
            if (isBlink)
            {
                this.outputLED();
            }
        }

        // LED の状態を変更
        public void changeLEDStatus(LED led, LEDStatus status)
        {
            lock (lockObj)
            {
                // 点滅状態から点滅状態へは入れ替えない
                if ((status == LEDStatus.BLINK_OFF || status == LEDStatus.BLINK_ON) &&
                    (this.ledArray[led] == LEDStatus.BLINK_OFF || this.ledArray[led] == LEDStatus.BLINK_ON))
                {
                    // LED 状態を変更しない
                }
                else
                {
					if(this.ledArray[led] != status)
					{ 
						this.ledArray[led] = status;
						this.outputLED();

					}
				}
            }
        }

        // LED 出力関数 (LED 状態に変更があった場合に呼ぶ)
        public void outputLED()
        {
            lock (lockObj)
            {
                // LED の状態を更新
                foreach (KeyValuePair<LED, LEDStatus> led in this.ledArray)
                {
					int boardNumber ;
					CounterBoard.IOPort portNumber;
					if ((int)led.Key < 100)
					{ 
						boardNumber = 0;
						portNumber = (CounterBoard.IOPort)led.Key;
					}
					else
					{
						boardNumber = 1;
						portNumber = (CounterBoard.IOPort)((int)led.Key - 100);
					}

					// LED 状態を汎用出力値にセット
					switch (led.Value)
					{
						case LEDStatus.OFF:
						case LEDStatus.BLINK_OFF:
							this.counterBoard[boardNumber].setOutput(portNumber, false);
							break;
						case LEDStatus.ON:
						case LEDStatus.BLINK_ON:
							this.counterBoard[boardNumber].setOutput(portNumber, true);
							break;
					}
                }
            }

            // 出力
            this.counterBoard[0].outputDO();
			this.counterBoard[1].outputDO();
		}
    }
}
