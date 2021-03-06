using CounterBoardDebug.Properties;
using CounterBoardLib;
using MelsecInverterLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpLibrary;

namespace CounterBoardDebug
{
    public partial class CounterBoardDebug : Form
    {
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

        // インバータとのコミュニケータ
        private ICommunicator inverterComm = null;
        // インバータ通信の実装
        private InverterProtocol inverterProtocol = null;
        // Ethernet
        private ClientCommunicator clientComm = null;

        // lock オブジェクト
        private Object lockObj = new Object();

        // カウンタモニタ用タイマー
        private System.Timers.Timer checkTimer = null;
        // 1分周期のカウンタチェック用タイマー
        private System.Timers.Timer checkMinuteTimer = null;
        private int minuteCount = 0;

        // 接続状態
        private Boolean isConnected = false;

        public CounterBoardDebug()
        {
            InitializeComponent();
            this.initialize();
        }

        // 初期化
        private void initialize()
        {
            this.counterBoard = CounterBoard.Instance;

            // オープン
            if (!this.counterBoard[0].open(true))
            {
                throw new ApplicationException(String.Format("カウンタボードのオープンに失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }
            // カウンタ設定
            if (!this.counterBoard[0].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
            {
                throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }
            // カウンタボードのイベント有効化
            if (!this.counterBoard[0].setEvent())
            {
                throw new ApplicationException(String.Format("カウンタボードのイベント設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }

			// カウンタボードのイベント
			this.counterBoard[0].ioInterrupted += OnDioInterrupted;
            this.counterBoard[0].setEventMask(0x0200, 0x01800180, 1);


            // デジタルフィルタの設定
            this.counterBoard[0].setFilter(0x0100, 0x8A);
            this.counterBoard[0].setFilter(0x0200, 0x8A);

            // カウント値一致のイベント
            this.counterBoard[0].channel1Interrupted += OnChannel1Interrupted;
            this.counterBoard[0].channel2Interrupted += OnChannel1Interrupted;
            this.counterBoard[0].channel3Interrupted += OnChannel1Interrupted;
            this.counterBoard[0].channel4Interrupted += OnChannel1Interrupted;
            this.counterBoard[0].channel5Interrupted += OnChannel1Interrupted;
            this.counterBoard[0].channel6Interrupted += OnChannel1Interrupted;
			this.counterBoard[0].setEventMask(1, 0x00000004, 1);
			this.counterBoard[0].setEventMask(2, 0x00000004, 1);
			this.counterBoard[0].setEventMask(3, 0x00000004, 1);
			this.counterBoard[0].setEventMask(4, 0x00000004, 1);
			this.counterBoard[1].setEventMask(1, 0x00000004, 1);
			this.counterBoard[1].setEventMask(2, 0x00000004, 1);

			// RS232C によるインバータとの通信
			inverterComm = new RS232Communicator(Settings.Default.COMPort,
                                                 (RS232Communicator.Baudrate)Settings.Default.BaudRate,
                                                 Settings.Default.Parity,
                                                 Settings.Default.StopBits);

            this.inverterProtocol = new InverterProtocol(inverterComm, Settings.Default.InverterNo);

            // LIBS 装置と接続確立
            this.clientComm = new ClientCommunicator(Settings.Default.LIBSIPAddress,
                                                Settings.Default.LIBSPort);

            this.clientComm.dataReceived = OnReceiveMessage;
            this.clientComm.connectionAccepted = OnAcceptedConnection;
            this.clientComm.connectionClosed = OnClosedConnection;

            // チェックタイマーの初期化
            this.checkTimer = new System.Timers.Timer(16);
            this.checkTimer.Elapsed += OnElapsedCheckTimer;
            this.checkTimer.Start();

            // チェックタイマーの初期化
            this.checkMinuteTimer = new System.Timers.Timer(60000);
            this.checkMinuteTimer.Elapsed += OnElapsedCheckMinuteTimer;
        }

        private void OnClosedConnection()
        {
            this.isConnected = false;
        }

        private void OnAcceptedConnection(TcpSocket tcpSocket)
        {
            this.isConnected = true;
        }

        private void OnReceiveMessage(TcpSocket tcpSocket, DataReceivedEventArgs e)
        {
            // 何もしない
        }

        private void OnElapsedCheckMinuteTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, System.Timers.ElapsedEventArgs>(OnElapsedCheckMinuteTimer), new object[] { sender, e });
                return;
            }

            this.minuteCount++;
            uint counter = 0;
            this.counterBoard[0].getCounter(1, out counter);

            this.textBoxCounterLog.Text += string.Format("{0}分経過 - カウンタ値:{1} \r\n", this.minuteCount.ToString(), counter.ToString());
        }

        private void OnElapsedCheckTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<object, System.Timers.ElapsedEventArgs>(OnElapsedCheckTimer), new object[] { sender, e });
                return;
            }

            uint counter = 0;
            this.counterBoard[0].getCounter(1, out counter);

            this.textBoxCurrentCounter.Text = counter.ToString();
        }

        private void OnClickButtonLED1(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT10] = !outputDict[CounterBoard.IOPort.PORT10];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT10, outputDict[CounterBoard.IOPort.PORT10]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT10])
            {
                this.buttonLED1.BackColor = Color.Green;
            }
            else
            {
                this.buttonLED1.BackColor = Color.White;
            }
        }

        private void OnClickButtonLED2(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT11] = !outputDict[CounterBoard.IOPort.PORT11];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT11, outputDict[CounterBoard.IOPort.PORT11]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT11])
            {
                this.buttonLED2.BackColor = Color.Red;
            }
            else
            {
                this.buttonLED2.BackColor = Color.White;
            }
        }

        private void OnClickButtonLED3(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT12] = !outputDict[CounterBoard.IOPort.PORT12];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT12, outputDict[CounterBoard.IOPort.PORT12]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT12])
            {
                this.buttonLED3.BackColor = Color.Green;
            }
            else
            {
                this.buttonLED3.BackColor = Color.White;
            }
        }

        private void OnClickButtonLED4(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT13] = !outputDict[CounterBoard.IOPort.PORT13];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT13, outputDict[CounterBoard.IOPort.PORT13]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT13])
            {
                this.buttonLED4.BackColor = Color.Green;
            }
            else
            {
                this.buttonLED4.BackColor = Color.White;
            }
        }

        private void OnClickButtonLED5(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT14] = !outputDict[CounterBoard.IOPort.PORT14];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT14, outputDict[CounterBoard.IOPort.PORT14]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT14])
            {
                this.buttonLED5.BackColor = Color.Red;
            }
            else
            {
                this.buttonLED5.BackColor = Color.White;
            }
        }

        private void OnClickButtonLED6(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT15] = !outputDict[CounterBoard.IOPort.PORT15];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT15, outputDict[CounterBoard.IOPort.PORT15]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT15])
            {
                this.buttonLED6.BackColor = Color.Green;
            }
            else
            {
                this.buttonLED6.BackColor = Color.White;
            }
        }

        private void OnClickButtonLED7(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT16] = !outputDict[CounterBoard.IOPort.PORT16];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT16, outputDict[CounterBoard.IOPort.PORT16]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT16])
            {
                this.buttonLED7.BackColor = Color.Green;
            }
            else
            {
                this.buttonLED7.BackColor = Color.White;
            }
        }

        private void OnClickButtonPaddle1(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT1] = !outputDict[CounterBoard.IOPort.PORT1];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT1, outputDict[CounterBoard.IOPort.PORT1]);

            //this.counterBoard.setOutput(CounterBoard.IOPort.PORT3, outputDict[CounterBoard.IOPort.PORT1]);
            //this.counterBoard.setOutput(CounterBoard.IOPort.PORT4, outputDict[CounterBoard.IOPort.PORT1]);

            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT1])
            {
                this.buttonPaddle1.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddle1.BackColor = Color.White;
            }
        }

        private void OnClickButtonPaddle2(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT2] = !outputDict[CounterBoard.IOPort.PORT2];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT2, outputDict[CounterBoard.IOPort.PORT2]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT2])
            {
                this.buttonPaddle2.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddle2.BackColor = Color.White;
            }
        }

        private void OnClickButtonPaddle3(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT3] = !outputDict[CounterBoard.IOPort.PORT3];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT3, outputDict[CounterBoard.IOPort.PORT3]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT3])
            {
                this.buttonPaddle3.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddle3.BackColor = Color.White;
            }
        }

        private void OnClickButtonPaddle4(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT4] = !outputDict[CounterBoard.IOPort.PORT4];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT4, outputDict[CounterBoard.IOPort.PORT4]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT4])
            {
                this.buttonPaddle4.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddle4.BackColor = Color.White;
            }
        }

        private void OnClickButtonPaddle5(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT5] = !outputDict[CounterBoard.IOPort.PORT5];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT5, outputDict[CounterBoard.IOPort.PORT5]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT5])
            {
                this.buttonPaddle5.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddle5.BackColor = Color.White;
            }
        }

        private void OnClickButtonPaddle6(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT6] = !outputDict[CounterBoard.IOPort.PORT6];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT6, outputDict[CounterBoard.IOPort.PORT6]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT6])
            {
                this.buttonPaddle6.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddle6.BackColor = Color.White;
            }
        }

        private void OnClickButtonInterlock(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT7] = !outputDict[CounterBoard.IOPort.PORT7];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT7, outputDict[CounterBoard.IOPort.PORT7]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT7])
            {
                this.buttonInterlock.BackColor = Color.Green;
            }
            else
            {
                this.buttonInterlock.BackColor = Color.White;
            }
        }

        private void OnClickSetEQCount(object sender, EventArgs e)
        {
            UInt32 eqCounter = 0;
            if (UInt32.TryParse(this.textBoxEQCounter.Text, out eqCounter))
            {
                this.counterBoard[0].setEqualCounter(1, eqCounter);
            }
            else
            {
                MessageBox.Show("カウント値を入力してください。");
            }
        }

        private void OnClickResetCounter(object sender, EventArgs e)
        {
            this.counterBoard[0].resetCounter(1);

            // カウンタ設定
            if (!this.counterBoard[0].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
            {
                throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }
        }

        // 汎用入力の割り込みイベント
        private void OnDioInterrupted(object sender, CounterBoardEventArgs e)
        {
            // SU_IN9 SD_IN9 のフラグが立っているか
            if ((e.EventMask & (uint)0x00000100) > 0)
            {
                // 立ち上がりで LIBS 装置運転停止
                this.buttonKeySW.BackColor = Color.White;
            }
            else if ((e.EventMask & (uint)0x01000000) > 0)
            {
                // 立ち下がりで LIBS 装置運転開始
                this.buttonKeySW.BackColor = Color.Red;
            }
            else if ((e.EventMask & (uint)0x00000080) > 0)
            {
                // 立ち上がりで緊急停止信号受け取り
                this.buttonEmengency.BackColor = Color.Red;
                OnClickButtonConveyorStop(this, new EventArgs());
            }
            else if ((e.EventMask & (uint)0x00800000) > 0)
            {
                // 立ち下がりで緊急停止信号解除
                this.buttonEmengency.BackColor = Color.White;
            }
        }

        // カウンタボードのチャンネル1の割り込みイベント
        private void OnChannel1Interrupted(object sender, CounterBoardEventArgs e)
        {
            // EQ フラグが立っているか
            if ((e.EventMask & (uint)0x00000004) > 0)
            {
                // 電磁パドル駆動
                this.buttonEQOut.BackColor = Color.Green;

                /*
                this.counterBoard.setOutput(CounterBoard.IOPort.PORT3, true);
                this.counterBoard.setOutput(CounterBoard.IOPort.PORT4, true);
                */

                if (this.checkBoxPaddle1.Checked)
                {
                    this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT1, true);
                }
                if (this.checkBoxPaddle2.Checked)
                {
                    this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT2, true);
                }
                if (this.checkBoxPaddle3.Checked)
                {
                    this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT3, true);
                }
                if (this.checkBoxPaddle4.Checked)
                {
                    this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT4, true);
                }
                if (this.checkBoxPaddle5.Checked)
                {
                    this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT5, true);
                }
                if (this.checkBoxPaddle6.Checked)
                {
                    this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT6, true);
                }
                this.counterBoard[0].outputDO();

                System.Threading.Thread.Sleep(125);

                // 電磁パドル駆動
                this.buttonEQOut.BackColor = Color.White;

                this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT1, false);
                this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT2, false);
                this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT3, false);
                this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT4, false);
                this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT5, false);
                this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT6, false);

                this.counterBoard[0].outputDO();
            }
        }

        private void OnClickButtonGetCounter(object sender, EventArgs e)
        {
            UInt32 counter;

            // カウンタ値の取得
            this.counterBoard[0].getCounter(1, out counter);

            this.textBoxCounter.Text = counter.ToString();
        }

        private void OnClickButtonConveyorStart(object sender, EventArgs e)
        {
            try
            {
                // カウンタ値ログを消去
                this.textBoxCounterLog.Text = string.Empty;
                this.minuteCount = 0;

                //カウンタ値リセット
                this.counterBoard[0].resetCounter(1);

                // カウンタ設定
                if (!this.counterBoard[0].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
                {
                    throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
                }

                OnClickButtonLIBSReset(this, new EventArgs());
                OnClickButtonLIBSStart(this, new EventArgs());

                System.Threading.Thread.Sleep(1000);


                // 正転で運転指令
                //this.inverterProtocol.setRunning(InverterProtocol.RunningCommand.STR, false);
                this.inverterProtocol.setRunning(InverterProtocol.RunningCommand.STF, false);
                //this.checkTimer.Start();
                this.checkMinuteTimer.Start();
            }
            catch (InverterException iEx)
            {
                MessageBox.Show(iEx.ErrorCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickButtonConveyorStop(object sender, EventArgs e)
        {
            try
            {
                OnClickButtonLIBSStop(this, new EventArgs());

                // 停止
                this.inverterProtocol.setRunning(InverterProtocol.RunningCommand.STOP, false);

                //this.checkTimer.Stop();
                this.checkMinuteTimer.Stop();
            }
            catch (InverterException iEx)
            {
                MessageBox.Show(iEx.ErrorCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickButtonChangeFrequency(object sender, EventArgs e)
        {
            UInt32 frequency = 6000;

            try
            {
                UInt32.TryParse(this.textBoxOutputFrequency.Text, out frequency);

                // インバータの出力周波数変更
                this.inverterProtocol.setFrequency(frequency, true);
            }
            catch (InverterException iEx)
            {
                MessageBox.Show(iEx.ErrorCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickButtonMonitorFrequency(object sender, EventArgs e)
        {
            try
            {
                // インバータの出力周波数取得
                UInt32 frequency = this.inverterProtocol.getFrequency();

                this.textBoxMonitorFrequency.Text = frequency.ToString();
            }
            catch (InverterException iEx)
            {
                MessageBox.Show(iEx.ErrorCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickButtonConnectionOpen(object sender, EventArgs e)
        {
            try
            {
                // 接続を開く
                this.inverterProtocol.open();

                // インバータの制御をネットワークモードに変更
                this.inverterProtocol.setRunningMode(InverterProtocol.RunningMode.NetworkMode);
            }
            catch (InverterException iEx)
            {
                MessageBox.Show(iEx.ErrorCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickButtonConnectionClose(object sender, EventArgs e)
        {
            try
            {
                // 接続を閉じる
                this.inverterProtocol.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClickButtonLIBSStart(object sender, EventArgs e)
        {
            Byte[] sendData = new Byte[3];

            // STRAT コマンド
            sendData[0] = 0x80;

            // Analysis Method
            sendData[1] = 0x00;

            // Reserve
            sendData[2] = 0x00;

            if (this.isConnected)
            {
                // コマンド送信
                this.clientComm.sendMessage(sendData);
            }
        }

        private void OnClickButtonLIBSStop(object sender, EventArgs e)
        {
            Byte[] sendData = new Byte[3];

            // STOP コマンド
            sendData[0] = 0x40;

            // Reserve
            sendData[1] = 0x00;
            sendData[2] = 0x00;

            if (this.isConnected)
            {
                // コマンド送信
                this.clientComm.sendMessage(sendData);
            }
        }

        private void OnClickButtonLIBSReset(object sender, EventArgs e)
        {
            Byte[] sendData = new Byte[3];

            // RESET コマンド
            sendData[0] = 0x10;

            // Reserve
            sendData[1] = 0x00;
            sendData[2] = 0x00;

            if (this.isConnected)
            {
                // コマンド送信
                this.clientComm.sendMessage(sendData);
            }
        }

        private void OnClickButtonConnect(object sender, EventArgs e)
        {
            if (this.clientComm != null)
            {
                if (!this.clientComm.connect(10000))
                {
                    MessageBox.Show("接続に失敗しました。");
                }
            }
        }

        private void OnClickButtonClose(object sender, EventArgs e)
        {
            if (this.clientComm != null)
            {
                this.clientComm.close();
            }
        }

        private void OnClickButtonPaddleAll(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT1] = !outputDict[CounterBoard.IOPort.PORT1];

            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT1, outputDict[CounterBoard.IOPort.PORT1]);
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT2, outputDict[CounterBoard.IOPort.PORT1]);
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT3, outputDict[CounterBoard.IOPort.PORT1]);
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT4, outputDict[CounterBoard.IOPort.PORT1]);
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT5, outputDict[CounterBoard.IOPort.PORT1]);
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT6, outputDict[CounterBoard.IOPort.PORT1]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT1])
            {
                this.buttonPaddleAll.BackColor = Color.Green;
            }
            else
            {
                this.buttonPaddleAll.BackColor = Color.White;
            }
        }


    }
}
