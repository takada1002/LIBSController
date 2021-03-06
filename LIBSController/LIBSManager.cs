using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLibrary;
using MelsecInverterLib;
using CounterBoardLib;
using LIBSController.Properties;
using System.Diagnostics;

namespace LIBSController
{
	#region イベント引数

	public class LIBSReadyEventArgs : EventArgs
    {
        private Boolean ready;
        public Boolean Ready
        {
            get { return this.ready; }
        }

        public LIBSReadyEventArgs(Boolean ready)
        {
            this.ready = ready;
        }
    }

    public class LIBSConnectionEventArgs : EventArgs
    {
        private Boolean isConnect;
        public Boolean IsConnect
        {
            get { return this.isConnect; }
        }

        public LIBSConnectionEventArgs(Boolean isConnect)
        {
            this.isConnect = isConnect;
        }
    }

    public class LIBSAnalysisEventArgs : EventArgs
    {
        // 素材分析結果
        private Boolean result;
        public Boolean Result
        {
            get { return this.result; }
        }
        // 分析結果グループ
        private LIBSManager.MaterialGroup group;
        public LIBSManager.MaterialGroup Group
        {
            get { return this.group; }
        }
        // カウント値
        private UInt32 counter;
        public UInt32 Counter
        {
            get { return this.counter; }
        }
        // Y方向位置
        private Int32 posY;
        public Int32 PosY
        {
            get { return this.posY; }
        }
        // 
        public LIBSAnalysisEventArgs(Boolean result, LIBSManager.MaterialGroup group, UInt32 counter, Int32 posY)
        {
            this.result = result;
            this.group = group;
            this.counter = counter;
            this.posY = posY;
        }
    }

	#endregion

    // LIBS 装置管理クラス
    public class LIBSManager
    {
		#region 列挙体

		// LIBS 通信コマンド
		public enum LIBSCommand : byte
        {
            START           = 0x80,
            STOP            = 0x40,
            STATUS          = 0x20,
            RESETCOUNTER    = 0x10,
            CLASSLIST       = 0x81,
            RESULT          = 0x08,
        }

        // LIBS 装置状態
        public enum LIBSStatus : byte
        {
            NOT_READY       = 0x00,
            READY           = 0x01,
            RUNNING         = 0x02,
            TIMEOUT         = 0x03,
            NO_MESURE       = 0x04,
            INTERLOCK       = 0x05,
            MESURE_SUCCESS  = 0x06,
            MESURE_FAILED   = 0x07,
            UNKNOWN         = 0xFF,
        }

        // 受信結果のグループ
        // 選別対象を選択可能にするから固定定義は消去
        public enum MaterialGroup : byte
        {
            NO_MEASURE		= 101,
            UNKNOWN			= 102,
			TOO_SLOW		= 103,
			WEAK_INTENSITY	= 104

        }

		#endregion

		#region 定数

		// ラインフィード
		private const Byte Linefeed = 0x0A;

		#endregion

		#region イベント

		// 状態変更イベント
		public event EventHandler<LIBSReadyEventArgs> readyStatusChanged;

		// 分析データ受信イベント
		public event EventHandler<LIBSAnalysisEventArgs> analysisResultReceived;

		// 接続状態変化イベント
		public event EventHandler<LIBSConnectionEventArgs> connectionChanged;

		#endregion

		#region プライベートフィールド

		// アプリケーションログ
		private ApplicationLog appLog = null;

        // 初回フラグ
        private Boolean firstFlag = true;

        // ひとつ前に受信した Live ビット
        private Boolean beforeLiveBit = false;

        // 状態チェック用タイマー
        private System.Timers.Timer checkTimer = null;

        // LIBS のレディー状態
        private Boolean libsReady = false;
        
		// Ethernet
        private ClientCommunicator clientComm = null;
		
		// 運転管理キュー
		private List<DrivingQueue> drivingQueue = null;
		
		// カウンタボード
		private List<CounterBoard> counterBoard = null;
        
		// 受信データ保存領域
        private List<Byte> receiveData = new List<Byte>();
        
		// 選別対象設定
        private MaterialSetting materialSetting = null;

		// ステータステレグラムがタイムアウトした回数
		private int statusTimeoutCount = 0;

		#endregion

		#region プロパティ

		// 接続状態
		private Boolean isConnected = false;
        public Boolean IsConnected
        {
            get { return this.isConnected; }
            set { this.isConnected = value; }
        }

		// LIBS の最終ステータス
		private LIBSStatus libsStatus = LIBSStatus.UNKNOWN;
		public LIBSStatus LibsStatus
		{
			get { return libsStatus; }
		}

		#endregion

		#region 公開メソッド

		// 初期化
		public Boolean initialize()
		{
			// ログ出力クラスのインスタンス取得
			this.appLog = ApplicationLog.getInstance();

			// カウンタボードのインスタンスを取得
			this.counterBoard = CounterBoard.Instance;
			// 素材管理キューのインスタンス取得
			//this.materialQueue = MaterialQueue.Instance;
			// 選別対象設定のインスタンス取得
			this.materialSetting = MaterialSetting.getInstance();

			// LIBS 装置と接続確立
			this.clientComm = new ClientCommunicator(Settings.Default.LIBSIPAddress,
												Settings.Default.LIBSPort);

			this.clientComm.dataReceived = OnReceiveMessage;
			this.clientComm.connectionAccepted = OnAcceptedConnection;
			this.clientComm.connectionClosed = OnClosedConnection;

			// チェックタイマーの初期化
			this.checkTimer = new System.Timers.Timer(Settings.Default.CheckTimerInterval);
			this.checkTimer.Elapsed += OnElapsedCheckTimer;

			if (Settings.Default.LibsDebug)
			{
				this.connectionChanged(this, new LIBSConnectionEventArgs(true));
				this.readyStatusChanged(this, new LIBSReadyEventArgs(true));
			}
			else
			{
				// チェックタイマースタート
				this.checkTimer.Start();
			}
			this.appLog.Info("初期化完了 ---LIBS装置管理---");

			return true;
		}

		// 接続確立
		public Boolean connectLIBS()
		{
			if (Settings.Default.LibsDebug)
				return true;

			Boolean result = false;

			if (this.clientComm != null)
			{
				result = this.clientComm.connect(10000);
			}

			return result;
		}

		// 切断
		public void closeLIBS()
		{
			if (Settings.Default.LibsDebug)
			{
				this.connectionChanged(this, new LIBSConnectionEventArgs(false));
				return;
			}

			try
			{
				if (this.clientComm != null)
				{
					this.clientComm.close();
				}
			}
			catch (Exception)
			{

			}

			this.isConnected = false;

			if (this.connectionChanged != null)
			{
				this.connectionChanged(this, new LIBSConnectionEventArgs(this.isConnected));
			}
		}

		// インターロックを掛ける
		public Boolean lockInterlock()
		{
			Boolean result = false;

			this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT7, false);
			// 出力
			result = this.counterBoard[0].outputDO();

			if (!result)
			{
				throw new ApplicationException(this.counterBoard[0].ErrorCode.ToString());
			}

			return result;
		}

		// インターロックを解除
		public Boolean unlockInterlock()
		{
			Boolean result = false;

			this.counterBoard[0].setOutput(CounterBoard.IOPort.PORT7, true);
			// 出力
			result = this.counterBoard[0].outputDO();

			if (!result)
			{
				throw new ApplicationException(this.counterBoard[0].ErrorCode.ToString());
			}

			return result;
		}

		// 運転開始コマンド送信
		public void startLIBS()
		{
			if (Settings.Default.LibsDebug)
				return;

			Byte[] sendData = new Byte[3];

			// STRAT コマンド
			sendData[0] = (Byte)LIBSCommand.START;

			// Analysis Method
			sendData[1] = Settings.Default.LIBSAnalysisMethod;

			// Reserve
			sendData[2] = 0x00;

			try
			{
				if (this.isConnected)
				{
					// コマンド送信
					this.clientComm.sendMessage(sendData);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// 運転停止
		public void stopLIBS()
		{
			if (Settings.Default.LibsDebug)
				return;

			Byte[] sendData = new Byte[3];

			// STOP コマンド
			sendData[0] = (Byte)LIBSCommand.STOP;

			// Reserve
			sendData[1] = 0x00;

			// Reserve
			sendData[2] = 0x00;

			try
			{
				if (this.isConnected)
				{
					// コマンド送信
					this.clientComm.sendMessage(sendData);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// LIBS 内部カウンタリセット
		public void resetCounter()
		{
			if (Settings.Default.LibsDebug)
				return;

			Byte[] sendData = new Byte[3];

			// RESET コマンド
			sendData[0] = (Byte)LIBSCommand.RESETCOUNTER;

			// Reserve
			sendData[1] = 0x00;
			sendData[2] = 0x00;

			try
			{
				if (this.isConnected)
				{
					// コマンド送信
					this.clientComm.sendMessage(sendData);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// 装置状態の要求 (どういう場面で使うのか未定)
		public void requestStatus()
		{
			if (Settings.Default.LibsDebug)
				return;
			Byte[] sendData = new Byte[3];

			// STATUS 要求コマンド
			sendData[0] = (Byte)LIBSCommand.STATUS;

			// Reserve
			sendData[1] = 0x00;
			sendData[2] = 0x00;

			try
			{
				if (this.isConnected)
				{
					// コマンド送信
					this.clientComm.sendMessage(sendData);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region イベントハンドラ

		// チェックタイマーイベント
		private void OnElapsedCheckTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
			//appLog.Debug("LIBSタイマー処理を実施します");

			try
			{
				this.checkTimer.Stop();

				// 接続中にSTATUS テレグラムが来なくなった場合
				if (isConnected)
				{
					//if (++statusTimeoutCount >= 5 )
					{
						statusTimeoutCount = 0;
						appLog.Debug("LIBSタイマ経過により切断します");
						closeLIBS();
					}

					if (this.connectionChanged != null)
					{
						this.connectionChanged(this, new LIBSConnectionEventArgs(this.isConnected));
					}
				}

				// 接続されていなければ再接続
				else
                {
					//appLog.Debug("LIBS接続を試みます");
					// 接続再試行
					this.connectLIBS();
                }
			}
            catch
            {

            }
            finally
            {
                this.checkTimer.Start();
            }
        }

        // 接続が受け入れられた際のイベント
        private void OnAcceptedConnection(TcpSocket tcpSocket)
        {
			this.checkTimer.Stop();

			appLog.Debug("LIBS接続を受け入れました");

			this.isConnected = true;

            if (this.connectionChanged != null)
            {
                this.connectionChanged(this, new LIBSConnectionEventArgs(this.isConnected));
            }
			//if (this.readyStatusChanged != null)
			//{
			//	this.readyStatusChanged(this, new LIBSReadyEventArgs(true));
			//}

			statusTimeoutCount = 0;
			this.checkTimer.Start();
		}

        // 接続が切れた際のイベント
        private void OnClosedConnection()
        {
			appLog.Debug("LIBS接続が切れました");

			this.isConnected = false;

            if (this.connectionChanged != null)
            {
                this.connectionChanged(this, new LIBSConnectionEventArgs(this.isConnected));
            }
        }

		// LIBS 装置からのデータ受信イベント
		private void OnReceiveMessage(TcpSocket tcpSocket, TcpLibrary.DataReceivedEventArgs args)
        {
            try
            {
                //Console.WriteLine("Manager received");

                // 受信データを連結
                this.receiveData.AddRange(args.receiveByte);

                while (this.receiveData.Count > 0)
                {
                    // CLASSLIST のテレグラムは分割送信でかつ終端記号が無いため
                    // 次の Ready 信号が来るまで貯め込み処理する

                    Boolean check = false;
                    Int32 checkIndex = 1;

                    // データチェック
                    foreach (byte b in this.receiveData)
                    {
                        // 次の Ready 信号 か Result データがあるかの確認
                        if (((LIBSCommand)b == LIBSCommand.STATUS))
                        {
                            // STATUS コードは 0x20 でスペースと見分けがつかないため次のデータもチェック
                            // STATUS コードなら 2 byte 目は 0x08 以下
                            if (this.receiveData.Count > checkIndex)
                            {
#if(true)
                                // libs start するとindex 4 bit が立つことがあるのでチェックしない
                                check = true;
#else
                                if (this.receiveData[checkIndex] < 0x08)
                                {
                                    check = true;
                                }
#endif
                            }
                        }
                        else if ((LIBSCommand)b == LIBSCommand.RESULT)
                        {
                            check = true;
                        }
                        checkIndex++;
                    }

                    if (!check)
                    {
                        break;
                    }

                    // 受信データの先頭1バイトを取り出す
                    byte cmd = this.receiveData[0];

                    // 受信処理
                    switch ((LIBSCommand)cmd)
                    {
                        // LIBS 装置からステータス情報を受信
                        case LIBSCommand.STATUS:

							//appLog.Debug("STATUS テレグラムを受信しました。");

                            //Console.WriteLine("status received");

                            // Live & Ready ビットの情報取得
                            Byte status = this.receiveData[1];

                            // Live ビットを取得
                            Boolean live = ((status & 0x02) == 0x02);
                            // Ready ビットを取得
                            Boolean ready = ((status & 0x01) == 0x01);

							// force ready
							//Boolean ready = true;

							// Ready 状態変更イベントをコール
							if (this.readyStatusChanged != null)
							{
								this.readyStatusChanged(this, new LIBSReadyEventArgs(ready));
							}

							// ステータス詳細の取得
							Byte detail = this.receiveData[2];

                            // 現ステータスを更新
                            this.libsStatus = (LIBSStatus)detail;

							// タイマーストップ、スタート (タイマーリセット)
							this.checkTimer.Stop();
							this.checkTimer.Start();

							// 読み出しデータ分だけ受信データから削除
							this.receiveData.RemoveRange(0, 3);

							break;
                        case LIBSCommand.CLASSLIST:

							appLog.Debug("CLASSLIST テレグラムを受信しました。");

							//Console.WriteLine("class received ");

							// クラスリスト格納用
							List<List<Byte>> classList = new List<List<Byte>>();

                            // コマンド部分のデータを削除
                            this.receiveData.RemoveRange(0, 1);

                            // LIBS 装置からクラスリストを受信
                            while (true)
                            {
                                int index = 1;

                                foreach (byte b in this.receiveData)
                                {
                                    // LF か 0x20 か 0x08 があればそこまでを切り取る
                                    if ((b == Linefeed) || ((LIBSCommand)b == LIBSCommand.RESULT) || (LIBSCommand)b == LIBSCommand.CLASSLIST)
                                    {
                                        break;
                                    }
                                    else if((LIBSCommand)b == LIBSCommand.STATUS)
                                    {
                                        // STATUS コードは 0x20 でスペースと見分けがつかないため次のデータもチェック
                                        // STATUS コードなら 2 byte 目は 0x08 以下
                                        if (this.receiveData.Count > index)
                                        {
                                            if (this.receiveData[index] < 0x08)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    index++;
                                }
                                
                                classList.Add(this.receiveData.GetRange(0, index - 1));

                                // 読み出しデータを削除
                                this.receiveData.RemoveRange(0, index - 1);

                                if (this.receiveData[0] == Linefeed)
                                {
                                    this.receiveData.RemoveRange(0, 1);
                                }

                                // 受信データが空になったら抜ける
                                if (this.receiveData.Count == 0)
                                {
                                    break;
                                }

                                // 先頭1バイトを取り出す
                                Byte nextByte = this.receiveData[0];

                                // ステータスコマンドか結果送信コマンドと一致したら抜ける
                                if (((LIBSCommand)nextByte == LIBSCommand.STATUS) || ((LIBSCommand)nextByte == LIBSCommand.RESULT) || (LIBSCommand)nextByte == LIBSCommand.CLASSLIST)
                                {
                                    break;
                                }
                            }

                            for (int i = 0; i < classList.Count; i++)
                            {
                                string className = System.Text.Encoding.ASCII.GetString(classList[i].ToArray());

								if(this.materialSetting.materialClassList.Count > i)
								{ 
									this.materialSetting.materialClassList[i].className = className;
									this.materialSetting.materialClassList[i].classCode = i + 1;
								}
								else
								{
									MaterialClass materialClass = new MaterialClass();
									materialClass.className = className;
									materialClass.classCode = i+1;

									this.materialSetting.materialClassList.Add(materialClass);
								}
                            }

							while(this.materialSetting.materialClassList.Count > classList.Count)
							{
								this.materialSetting.materialClassList.Remove( this.materialSetting.materialClassList.Last() );
							}

							this.materialSetting.materialClassList.ForEach(m=>
							{
								appLog.Debug($"CLASSLIST CODE:[{m.classCode}] NAME:[{m.className??""}] NOZZLE:[{m.nozzleNumber}]");
							});

							this.materialSetting.Serialize();

							// タイマーストップ、スタート (タイマーリセット)
							this.checkTimer.Stop();
							this.checkTimer.Start();

							break;
                        case LIBSCommand.RESULT:

							//appLog.Debug("RESULT テレグラムを受信しました。");

							//Console.WriteLine("result received ");

							// OK or NotOK
							Byte basicStatus = this.receiveData[1];
                            Boolean result = (basicStatus == 1) ? true : false;
                            // 分析結果
                            Byte groupNumber = this.receiveData[2];

							Int32 pulseCounterStart = 0;
							UInt32 pulseCounterStartPC = 0;
							Int32 pulseCounterEnd = 0;
							UInt32 pulseCounterEndPC = 0;

							if(groupNumber < 100)
							{ 

								Byte[] startByte = new Byte[4];
								Byte[] endByte = new Byte[4];
								Byte[] posYByte = new Byte[4];

								for (int i = 0; i < 4; i++)
								{
									startByte[i] = this.receiveData[6 - i];
									endByte[i] = this.receiveData[10 - i];
								}

								// レーザ照射開始カウント、終了カウント 制御 PC 側は UINT なので 2147483648 を加算
								pulseCounterStart = BitConverter.ToInt32(startByte, 0);
								pulseCounterStartPC = (UInt32)(pulseCounterStart + 2147483648);
								pulseCounterEnd = BitConverter.ToInt32(endByte, 0);
								pulseCounterEndPC = (UInt32)(pulseCounterEnd + 2147483648);

								appLog.Debug($"RESULT テレグラムを受信しました。start:{pulseCounterStart} end:{pulseCounterEnd}");
							}
							else
							{

								appLog.Debug($"RESULT テレグラムを受信しました。");
							}

							Int32 posY = 0;

							// LIBS 装置から分析結果を受信
							if (analysisResultReceived != null)
                            {
#if (true)
                                UInt32 materialLength = 0;
                                if(pulseCounterEndPC > pulseCounterStartPC)
                                {
									materialLength = pulseCounterEndPC - pulseCounterStartPC;
								}
                                else
                                {
                                    materialLength = ( UInt32.MaxValue - pulseCounterStartPC ) + pulseCounterEndPC;
								}

                                analysisResultReceived(this, new LIBSAnalysisEventArgs(result, (LIBSManager.MaterialGroup)groupNumber, materialLength, posY));
#else
                                analysisResultReceived(this, new LIBSAnalysisEventArgs(result, (LIBSManager.MaterialGroup)groupNumber, pulseCounterStartPC, posY));
#endif
                            }

                            // 先頭から 13 byte 切り出して再度文字列変換
                            this.receiveData.RemoveRange(0, 13);

                            break;
                        default:
                            // 受信データをクリア
                            this.receiveData.Clear();
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // 受信データをクリア
                this.receiveData.Clear();
            }
        }

		#endregion
    }
}
