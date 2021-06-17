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
    public partial class CounterBoardDebug2 : Form
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

        private uint ioflg = 0xFFFFFFFF;

        public CounterBoardDebug2()
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
                throw new ApplicationException(String.Format("カウンタボード1のオープンに失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }
			if (!this.counterBoard[1].open(true))
			{
				throw new ApplicationException(String.Format("カウンタボード2のオープンに失敗しました。{0}", this.counterBoard[1].ErrorCode));
			}
			// カウンタ設定
			if (!this.counterBoard[0].setCounterMode(1, CounterBoard.CountMode.SINGLE_PULSE, true, true, true))
            {
                throw new ApplicationException(String.Format("カウントモードの設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }
            // カウンタボードのイベント有効化
            if (!this.counterBoard[0].setEvent())
            {
                throw new ApplicationException(String.Format("カウンタボード1のイベント設定に失敗しました。{0}", this.counterBoard[0].ErrorCode));
            }
			if (!this.counterBoard[1].setEvent())
			{
				throw new ApplicationException(String.Format("カウンタボード2のイベント設定2に失敗しました。{0}", this.counterBoard[0].ErrorCode));
			}

			// カウンタボードのイベント
			counterBoard[0].ioInterrupted += OnDioInterrupted;
            //this.counterBoard.setEventMask(0x0200, 0x01800180, 1);
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);


            // デジタルフィルタの設定
            this.counterBoard[0].setFilter(0x0100, 0x8A);
            this.counterBoard[0].setFilter(0x0200, 0x8A);

            // カウント値一致のイベント
            //CounterBoard.channel1Interrupted += OnChannel1Interrupted;
            //this.counterBoard.setEventMask(1, 0x00000004, 1);

            //CounterBoard.channel2Interrupted += OnChannel2Interrupted;
            //this.counterBoard.setEventMask(2, 0x00000004, 1);

            //CounterBoard.channel3Interrupted += OnChannel3Interrupted;
            //this.counterBoard.setEventMask(3, 0x00000004, 1);

            //// RS232C によるインバータとの通信
            //inverterComm = new RS232Communicator(Settings.Default.COMPort,
            //                                     (RS232Communicator.Baudrate)Settings.Default.BaudRate,
            //                                     Settings.Default.Parity,
            //                                     Settings.Default.StopBits);

            //this.inverterProtocol = new InverterProtocol(inverterComm, Settings.Default.InverterNo);

            //// LIBS 装置と接続確立
            //this.clientComm = new ClientCommunicator(Settings.Default.LIBSIPAddress,
            //                                    Settings.Default.LIBSPort);

            //this.clientComm.dataReceived = OnReceiveMessage;
            //this.clientComm.connectionAccepted = OnAcceptedConnection;
            //this.clientComm.connectionClosed = OnClosedConnection;

            // チェックタイマーの初期化
            this.checkTimer = new System.Timers.Timer(16);
            this.checkTimer.Elapsed += OnElapsedCheckTimer;
            this.checkTimer.Start();

            // チェックタイマーの初期化
            //this.checkMinuteTimer = new System.Timers.Timer(60000);
            //this.checkMinuteTimer.Elapsed += OnElapsedCheckMinuteTimer;
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
            this.COUNTER1.Text = counter.ToString();

            this.counterBoard[0].getCounter(2, out counter);
            this.COUNTER2.Text = counter.ToString();

            this.counterBoard[0].getCounter(3, out counter);
            this.COUNTER3.Text = counter.ToString();

			this.counterBoard[0].getCounter(4, out counter);
			this.COUNTER4.Text = counter.ToString();

			this.counterBoard[1].getCounter(1, out counter);
			this.COUNTER5.Text = counter.ToString();

			this.counterBoard[1].getCounter(2, out counter);
			this.COUNTER6.Text = counter.ToString();

			this.counterBoard[0].getEqualCounter(1, out counter);
            this.textBox1.Text = counter.ToString();

            this.counterBoard[0].getEqualCounter(2, out counter);
            this.textBox2.Text = counter.ToString();

            this.counterBoard[0].getEqualCounter(3, out counter);
            this.textBox3.Text = counter.ToString();

			this.counterBoard[0].getEqualCounter(4, out counter);
			this.textBox5.Text = counter.ToString();

			this.counterBoard[1].getEqualCounter(1, out counter);
			this.textBox7.Text = counter.ToString();

			this.counterBoard[1].getEqualCounter(2, out counter);
			this.textBox9.Text = counter.ToString();
		}



        private void IO9_Click(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT9] = !outputDict[CounterBoard.IOPort.PORT9];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT9, outputDict[CounterBoard.IOPort.PORT9]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT9])
            {
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
            }
               
        }

        private void OnClickButtonLED1(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT10] = !outputDict[CounterBoard.IOPort.PORT10];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT10, outputDict[CounterBoard.IOPort.PORT10]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT10])
            {
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
            }
        }

        private void IO8_Click(object sender, EventArgs e)
        {
            // 出力反転
            outputDict[CounterBoard.IOPort.PORT8] = !outputDict[CounterBoard.IOPort.PORT8];
            this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT8, outputDict[CounterBoard.IOPort.PORT8]);
            this.counterBoard[0].outputDO();

            if (outputDict[CounterBoard.IOPort.PORT8])
            {
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
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
            if ((e.EventMask & (uint)0x00000001) > 0)
            {
                // 立ち上がりで白
                this.IO1.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00010000) > 0)
            {
                // 立ち下がりで赤
                this.IO1.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000002) > 0)
            {
                // 立ち上がりで白
                this.IO2.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00020000) > 0)
            {
                // 立ち下がりで赤
                this.IO2.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000004) > 0)
            {
                // 立ち上がりで白
                this.IO3.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00040000) > 0)
            {
                // 立ち下がりで赤
                this.IO3.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000008) > 0)
            {
                // 立ち上がりで白
                this.IO4.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00080000) > 0)
            {
                // 立ち下がりで赤
                this.IO4.BackColor = Color.Red;
            }


            if ((e.EventMask & (uint)0x00000010) > 0)
            {
                // 立ち上がりで白
                this.IO5.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00100000) > 0)
            {
                // 立ち下がりで赤
                this.IO5.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000020) > 0)
            {
                // 立ち上がりで白
                this.IO6.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00200000) > 0)
            {
                // 立ち下がりで赤
                this.IO6.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000040) > 0)
            {
                // 立ち上がりで白
                this.IO7.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00400000) > 0)
            {
                // 立ち下がりで赤
                this.IO7.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000080) > 0)
            {
                // 立ち上がりで白
                this.IO8.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x00800000) > 0)
            {
                // 立ち下がりで赤
                this.IO8.BackColor = Color.Red;
            }

            if ((e.EventMask & (uint)0x00000100) > 0)
            {
                // 立ち上がりで白
                this.IO9.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x01000000) > 0)
            {
                // 立ち下がりで赤
                this.IO9.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000200) > 0)
            {
                // 立ち上がりで白
                this.IO10.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x02000000) > 0)
            {
                // 立ち下がりで赤
                this.IO10.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000400) > 0)
            {
                // 立ち上がりで白
                this.IO11.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x04000000) > 0)
            {
                // 立ち下がりで赤
                this.IO11.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00000800) > 0)
            {
                // 立ち上がりで白
                this.IO12.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x08000000) > 0)
            {
                // 立ち下がりで赤
                this.IO12.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00001000) > 0)
            {
                // 立ち上がりで白
                this.IO13.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x10000000) > 0)
            {
                // 立ち下がりで赤
                this.IO13.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00002000) > 0)
            {
                // 立ち上がりで白
                this.IO14.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x20000000) > 0)
            {
                // 立ち下がりで赤
                this.IO14.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00004000) > 0)
            {
                // 立ち上がりで白
                this.IO15.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x40000000) > 0)
            {
                // 立ち下がりで赤
                this.IO15.BackColor = Color.Red;
            }
            if ((e.EventMask & (uint)0x00008000) > 0)
            {
                // 立ち上がりで白
                this.IO16.BackColor = Color.White;
            }
            if ((e.EventMask & (uint)0x80000000) > 0)
            {
                // 立ち下がりで赤
                this.IO16.BackColor = Color.Red;
            }
            //// SU_IN9 SD_IN9 のフラグが立っているか
            //if ((e.EventMask & (uint)0x00000100) > 0)
            //{
            //    // 立ち上がりで LIBS 装置運転停止
            //    this.buttonKeySW.BackColor = Color.White;
            //}
            //else if ((e.EventMask & (uint)0x01000000) > 0)
            //{
            //    // 立ち下がりで LIBS 装置運転開始
            //    this.buttonKeySW.BackColor = Color.Red;
            //}
            //else if ((e.EventMask & (uint)0x00000080) > 0)
            //{
            //    // 立ち上がりで緊急停止信号受け取り
            //    this.buttonEmengency.BackColor = Color.Red;
            //    OnClickButtonConveyorStop(this, new EventArgs());
            //}
            //else if ((e.EventMask & (uint)0x00800000) > 0)
            //{
            //    // 立ち下がりで緊急停止信号解除
            //    this.buttonEmengency.BackColor = Color.White;
            //}
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
                this.inverterProtocol.setRunning(InverterProtocol.RunningCommand.STR, false);
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.counterBoard[0].setEqualCounter(1, (uint)numericUpDown1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.counterBoard[0].setEqualCounter(2, (uint)numericUpDown2.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.counterBoard[0].setEqualCounter(3, (uint)numericUpDown3.Value);
        }

		private void button7_Click(object sender, EventArgs e)
		{
			this.counterBoard[0].setEqualCounter(4, (uint)numericUpDown7.Value);
		}

		private void button9_Click(object sender, EventArgs e)
		{
			this.counterBoard[1].setEqualCounter(1, (uint)numericUpDown9.Value);
		}

		private void button11_Click(object sender, EventArgs e)
		{
			this.counterBoard[1].setEqualCounter(2, (uint)numericUpDown10.Value);
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFFEFFFE;
                
            }
            else
            {
                ioflg |= 0x00010001;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFFDFFFD;

            }
            else
            {
                ioflg |= 0x00020002;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFFBFFFB;

            }
            else
            {
                ioflg |= 0x00040004;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFF7FFF7;

            }
            else
            {
                ioflg |= 0x00080008;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFEFFFEF;

            }
            else
            {
                ioflg |= 0x00100010;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFDFFFDF;

            }
            else
            {
                ioflg |= 0x00200020;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFFBFFFBF;

            }
            else
            {
                ioflg |= 0x00400040;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFF7FFF7F;

            }
            else
            {
                ioflg |= 0x00800080;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFEFFFEFF;

            }
            else
            {
                ioflg |= 0x01000100;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFDFFFDFF;

            }
            else
            {
                ioflg |= 0x02000200;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xFBFFFBFF;

            }
            else
            {
                ioflg |= 0x04000400;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xF7FFF7FF;

            }
            else
            {
                ioflg |= 0x08000800;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xEFFFEFFF;

            }
            else
            {
                ioflg |= 0x10001000;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xDFFFDFFF;

            }
            else
            {
                ioflg |= 0x20002000;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0xBFFFBFFF;

            }
            else
            {
                ioflg |= 0x40004000;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ioflg &= 0x7FFF7FFF;

            }
            else
            {
                ioflg |= 0x80008000;
            }
            this.counterBoard[0].setEventMask(0x0200, ioflg, 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.counterBoard[0].setCounter(1, (uint)this.numericUpDown4.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.counterBoard[0].setCounter(2, (uint)this.numericUpDown5.Value);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.counterBoard[0].setCounter(3, (uint)this.numericUpDown6.Value);
        }

		private void button8_Click(object sender, EventArgs e)
		{
			this.counterBoard[0].setCounter(4, (uint)this.numericUpDown8.Value);
		}

		private void button10_Click(object sender, EventArgs e)
		{
			this.counterBoard[1].setCounter(1, (uint)this.numericUpDown11.Value);
		}

		private void button12_Click(object sender, EventArgs e)
		{
			this.counterBoard[1].setCounter(2, (uint)this.numericUpDown12.Value);
		}


	}
}
