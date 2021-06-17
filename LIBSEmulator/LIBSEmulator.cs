using CounterBoardLib;
using LIBSEmulator.Properties;
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

namespace LIBSEmulator
{
    public partial class LIBSEmulator : Form
    {
        private ServerCommunicator serverComm = null;
        private Boolean power = true;

        // ステータス状態送信タイマー
        private System.Timers.Timer statusTimer = null;
        // ロックオブジェクト
        private object lockObj = new object();
        // TCP ソケット
        private List<TcpSocket> tcpSocketList = new List<TcpSocket>();
        // Livebit
        private Boolean liveBit = false;
        // カウンターボード
        private List<CounterBoard> counterBoard = null;

		private Timer countViewTimer ;
        
		private Timer ContinuousSendTimer ;

		private int nowGroupNum = 1;

		public LIBSEmulator()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            try
            {
				if (Settings.Default.IsCounterBoardDebug)
					CounterBoard.IsCounterBoardDebug = true;

				this.counterBoard = CounterBoard.Instance;

				//if (!Settings.Default.IsCounterBoardDebug)
				//{ 
					// オープン
					if (!this.counterBoard[0].open(true))
					{
						throw new ApplicationException(String.Format("カウンタボードのオープンに失敗しました。{0}", this.counterBoard[0].ErrorCode));
					}
					if (!this.counterBoard[1].open(true))
					{
						throw new ApplicationException(String.Format("カウンタボードのオープンに失敗しました。{0}", this.counterBoard[1].ErrorCode));
					}
				//}
				//else
				//{
				//	checkBoxCounterBoard.Checked = false;
				//	checkBoxCounterBoard.Visible = false;
				//}

				// タイマー準備
				this.statusTimer = new System.Timers.Timer(1000);
				this.statusTimer.Elapsed += OnElapsedChangeTimerS;
				this.statusTimer.Start();

				this.countViewTimer = new Timer();
				this.countViewTimer.Interval = 100;
				this.countViewTimer.Tick += CountViewTimer_Tick;
				this.countViewTimer.Start();
			}
            catch (Exception)
            {
                this.counterBoard = null;
            }

            this.serverComm = new ServerCommunicator();
            this.serverComm.TerminationCode = string.Empty;
            this.serverComm.dataReceived = OnDataReceived;
            this.serverComm.connectionAccepted = OnConnectionAccepted;
            this.serverComm.connectionClosed = OnConnectionClosed;
            this.serverComm.startListening(Settings.Default.Port,Settings.Default.ServerIPAddress);
        }

		private void CountViewTimer_Tick(object sender, EventArgs e)
		{
			if(this.checkBoxCounterBoard.Checked)
			{
				counterBoard[0].getCounter(1, out var count);
				this.textBoxCounter.Text = count.ToString();
			}
		}

		// タイマーイベント
		private void OnElapsedChangeTimerS(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 送信メッセージ
            byte[] sendData = new byte[3];

            sendData[0] = 0x20;

			sendData[1] = 0x00;

			if (liveBit)
            {
                sendData[1] |= 0x02;
                liveBit = !liveBit;
            }
            else
            {
                liveBit = !liveBit;
            }
			if(checkBoxIsReady.Checked)
			{
				sendData[1] |= 0x01;
			}

			sendData[2] = 0x01;

			lock (lockObj)
            {
                foreach (TcpSocket tcpSocket in this.tcpSocketList)
                {
                    this.serverComm.sendMessage(tcpSocket.IPAddr, sendData);
                }
            }

        }

        // データ受信イベント
        private void OnDataReceived(TcpSocket tcpSocket, DataReceivedEventArgs e)
        {
            string receiveData = e.receiveMsg;

            writeLogTextBox(string.Format("R<-- : {0} \r\n", receiveData));
        }

        // 送受信ログ書きメッセージ
        private void writeLogTextBox(string logMsg)
        {
            if (this.textBoxLog.InvokeRequired)
            {
                BeginInvoke(new Action<string>(writeLogTextBox), new object[] { logMsg });
                return;
            }

			this.textBoxLog.AppendText(logMsg);

            //this.textBoxLog.Text += logMsg;
            //// 表示位置を末尾に移動
            //this.textBoxLog.SelectionStart = this.textBoxLog.Text.Length;
            //this.textBoxLog.Focus();
            //this.textBoxLog.ScrollToCaret();
        }

        // 接続が受け入れられた際のイベント
        private void OnConnectionAccepted(TcpSocket tcpSocket)
        {
            this.tcpSocketList.Add(tcpSocket);

            sendClassInformation("1000\n2000\n3000\n4000\n5000\n6000\n7000\nAll other");
        }

        private void sendClassInformation(String classMsg)
        {
            byte[] strByte = System.Text.Encoding.ASCII.GetBytes(classMsg);

            // 送信メッセージ
            byte[] sendData = new byte[strByte.Length + 1];

            sendData[0] = 0x81;

            Buffer.BlockCopy(strByte, 0, sendData, 1, strByte.Length);

            lock (lockObj)
            {
                foreach (TcpSocket tcpSocket in this.tcpSocketList)
                {
                    this.serverComm.sendMessage(tcpSocket.IPAddr, sendData);
                }
            }
        }

        // 接続が切れた際のイベント
        private void OnConnectionClosed(TcpSocket tcpSocket)
        {
            this.tcpSocketList.Remove(tcpSocket);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            this.serverComm.stopListening();

            if (this.statusTimer != null)
            {
                this.statusTimer.Stop();
            }

            this.serverComm = null;
            this.statusTimer = null;

			countViewTimer?.Stop();
			ContinuousSendTimer?.Stop();

		}

        private void OnClickButtonSendData(object sender, EventArgs e)
        {
			// カウンタ値の取得
			UInt32 counter = 0;
			if (this.checkBoxCounterBoard.Checked)
			{
				//if (!Settings.Default.IsCounterBoardDebug)
				//{
					if (this.counterBoard != null)
					{
						this.counterBoard[0].getCounter(1, out counter);
					}
				//}
			}
			else
			{
				counter = Convert.ToUInt32(textBoxCounter.Text, 16);
			}

			Int32 counterLibs = (Int32)((Int32)counter - 2147483648);
			Int32 counterEnd = 0;
			Int32 materialLength = Convert.ToInt32(textBoxMaterialLengthCount.Text ?? "0");
			if ((long)counterLibs + (long)materialLength > Int32.MaxValue)
			{
				counterEnd = materialLength - (Int32.MaxValue - counterLibs);
			}

			//UInt32 counterStart = Convert.ToUInt32(textBoxCounterStart.Text);
			//UInt32 counterEnd = Convert.ToUInt32(textBoxCounterEnd.Text);



			// 送信メッセージ
			byte[] sendData = new byte[13];

            sendData[0] = 0x08;
            if(this.checkBoxAnalysisOK.Checked)
            {
                sendData[1] = 0x01;
            }
            else
            {
                sendData[1] = 0x00;
            }

            Int32 group = 0;
            Int32.TryParse(this.textBoxGroup.Text, out group);
            sendData[2] = (byte)group;


			byte[] pulseStart = BitConverter.GetBytes(counterLibs);
			//byte[] pulseStart = BitConverter.GetBytes(counterStart);
			byte[] pulseStartRev = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                pulseStartRev[i] = pulseStart[3 - i];
            }

			//byte[] pulseEnd = BitConverter.GetBytes(counterLibs+400);
			byte[] pulseEnd = BitConverter.GetBytes(counterEnd);
			byte[] pulseEndRev = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				pulseEndRev[i] = pulseEnd[3 - i];
			}

			Buffer.BlockCopy(pulseStartRev, 0, sendData, 3, 4);
            Buffer.BlockCopy(pulseEndRev, 0, sendData, 7, 4);

            sendData[11] = 0x00;
            sendData[12] = 0x00;

            lock (lockObj)
            {
                foreach (TcpSocket tcpSocket in this.tcpSocketList)
                {
                    this.serverComm.sendMessage(tcpSocket.IPAddr, sendData);
                }
            }
        }

        private void OnClickButtonSendStatus(object sender, EventArgs e)
        {
            if (!this.power)
            {
                this.power = true;
                this.buttonSendStatusInformation.BackColor = Color.Yellow;
                this.buttonSendStatusInformation.Text = "ステータス情報送信ON";

                // タイマー準備
                this.statusTimer = new System.Timers.Timer(1000);
                this.statusTimer.Elapsed += OnElapsedChangeTimerS;
                this.statusTimer.Start();
            }
            else
            {
                this.power = false;
                this.buttonSendStatusInformation.BackColor = Color.White;
                this.buttonSendStatusInformation.Text = "ステータス情報送信OFF";

                this.statusTimer.Elapsed -= OnElapsedChangeTimerS;
                this.statusTimer.Stop();
            }
        }

        private void OnClickButtonSendClass(object sender, EventArgs e)
        {
            String classMsg = textBoxClassInformation.Text;
            String sendMsg = classMsg.Replace("\r\n", "\n");
            sendClassInformation(sendMsg);
        }

        private void OnCheckedChangedCounterBoard(object sender, EventArgs e)
        {
            this.textBoxCounter.Enabled = !this.checkBoxCounterBoard.Checked;
        }

		private void checkBoxIsReady_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBoxIsReady.Checked)
			{ 
				counterBoard[0].StartEncoderEmulate();
				counterBoard[1].StartEncoderEmulate();
			}
			else
			{
				counterBoard[0].StopEncoderEmulate();
				counterBoard[1].StopEncoderEmulate();
			}
		}

		private void checkBoxContinuousSend_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBoxContinuousSend.Checked)
			{ 
				this.ContinuousSendTimer = new Timer();
				this.ContinuousSendTimer.Interval = 300;
				this.ContinuousSendTimer.Tick += ContinuousSendTimer_Tick; ;
				this.ContinuousSendTimer.Start();
			}
			else
			{
				ContinuousSendTimer.Stop();
			}
		}

		private void ContinuousSendTimer_Tick(object sender, EventArgs e)
		{
			// カウンタ値の取得
			UInt32 counter = 0;
			if (this.checkBoxCounterBoard.Checked)
			{
				if (this.counterBoard != null)
				{
					this.counterBoard[0].getCounter(1, out counter);
				}
			}
			else
			{
				counter = Convert.ToUInt32(textBoxCounter.Text, 16);
			}

			Int32 counterLibs = (Int32)((Int32)counter - 2147483648);
			Int32 counterEnd = 0;
			Int32 materialLength = Convert.ToInt32(textBoxMaterialLengthCount.Text ?? "0");
			if ((long)counterLibs + (long)materialLength > Int32.MaxValue)
			{
				counterEnd = materialLength - (Int32.MaxValue - counterLibs);
			}
			else
			{
				counterEnd = counterLibs + materialLength;
			}

			//UInt32 counterStart = Convert.ToUInt32(textBoxCounterStart.Text);
			//UInt32 counterEnd = Convert.ToUInt32(textBoxCounterEnd.Text);



			// 送信メッセージ
			byte[] sendData = new byte[13];

			sendData[0] = 0x08;
			if (this.checkBoxAnalysisOK.Checked)
			{
				sendData[1] = 0x01;
			}
			else
			{
				sendData[1] = 0x00;
			}

			Int32 group = 0;

			switch(nowGroupNum)
			{
				case 1:
					Int32.TryParse(this.textBoxGroup1.Text, out group);
					break;
				case 2:
					Int32.TryParse(this.textBoxGroup2.Text, out group);
					break;
				case 3:
					Int32.TryParse(this.textBoxGroup3.Text, out group);
					break;
				case 4:
					Int32.TryParse(this.textBoxGroup4.Text, out group);
					break;
				case 5:
					Int32.TryParse(this.textBoxGroup5.Text, out group);
					break;
			}
			sendData[2] = (byte)group;
			nowGroupNum++;
			if(nowGroupNum > 5)
				nowGroupNum = 1;

			byte[] pulseStart = BitConverter.GetBytes(counterLibs);
			byte[] pulseStartRev = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				pulseStartRev[i] = pulseStart[3 - i];
			}

			byte[] pulseEnd = BitConverter.GetBytes(counterEnd);
			byte[] pulseEndRev = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				pulseEndRev[i] = pulseEnd[3 - i];
			}

			Buffer.BlockCopy(pulseStartRev, 0, sendData, 3, 4);
			Buffer.BlockCopy(pulseEndRev, 0, sendData, 7, 4);

			sendData[11] = 0x00;
			sendData[12] = 0x00;

			lock (lockObj)
			{
				foreach (TcpSocket tcpSocket in this.tcpSocketList)
				{
					writeLogTextBox($"SEND {group} {Environment.NewLine}");
					this.serverComm.sendMessage(tcpSocket.IPAddr, sendData);
				}
			}
		}

		private void LIBSEmulator_Load(object sender, EventArgs e)
		{

		}
	}
}
