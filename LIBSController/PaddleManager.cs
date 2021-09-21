using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLibrary;
using MelsecInverterLib;
using CounterBoardLib;
using LIBSController.Properties;
using System.Threading;

namespace LIBSController
{
    public class PaddleDriveEventArgs : EventArgs
    {
        private Boolean[] drivingFlag;
        public Boolean[] DrivingFlag
        {
            get { return this.drivingFlag; }
        }

        public PaddleDriveEventArgs(Boolean[] drivingFlag)
        {
            this.drivingFlag = drivingFlag;
        }
    }

	public class NozzleExecuter
	{
		public int Number {get; set; }

		public event Action<bool> ShootStateChanged ;

		public DateTime FinishExecTime = DateTime.MinValue;

		private Thread execCheckThread;

		private bool isShoot = false;

		private bool execFlag = true;
		
		public NozzleExecuter()
		{
			execCheckThread = new Thread(ExecCheck);
			execCheckThread.IsBackground = true;
			execCheckThread.Start();
		}

		~NozzleExecuter()
		{
			execFlag = false;
		}

		private void ExecCheck()
		{
			while(execFlag)
			{ 
				if(FinishExecTime > DateTime.Now)
				{
					if(!isShoot)
					{
						ApplicationLog.getInstance().Info($"エアノズル噴射 No.{Number + 1}");
						CounterBoard.Instance[0].outputDO(Number, true);
						if (ShootStateChanged != null)
							ShootStateChanged(true);
						isShoot = true;
					}
				}
				else
				{
					if(isShoot)
					{
						ApplicationLog.getInstance().Info($"エアノズル停止 No.{Number + 1}");
						CounterBoard.Instance[0].outputDO(Number, false);
						// 電磁パドルの駆動終了
						if (ShootStateChanged != null)
							ShootStateChanged(false);
						isShoot = false;
					}
				}
				Thread.Sleep(1);
			}
		}
	}

    // 電磁パドル管理クラス
    class PaddleManager
    {

        enum PADDLE : int
        {
            PADDLE1 = 1,
            PADDLE2,
            PADDLE3,
            PADDLE4,
            PADDLE5,
            PADDLE6,
        }

        // アプリケーションログ
        private ApplicationLog appLog = null;
        // 素材管理キュー
		private List<DrivingQueue> drivingQueue = null;
		// 素材管理データ
		//private MaterialData materialData = null;
		private List<DrivingData> drivingData = new List<DrivingData>() { null,null,null,null,null,null };
		// カウンタボード
		private List<CounterBoard> counterBoard = null;

        // パドル駆動イベント
        public event EventHandler<PaddleDriveEventArgs> paddleDrived;

        // lock オブジェクト
        private Object lockObj = new Object();

		private List<Object> lockObjNozzle = new List<object>() { new object(), new object(), new object(), new object(), new object(), new object() };

		// データ管理スレッド
		Thread dataCheckThread = null;

		List<NozzleExecuter> NozzleExecuters = new List<NozzleExecuter>();

        // 初期化
        public Boolean initialize()
        {
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();
            // カウンタボードのインスタンスを取得
            this.counterBoard = CounterBoard.Instance;
            // 素材管理キューのインスタンスを取得
            this.drivingQueue = DrivingQueue.Instance;

            this.counterBoard[0].channel1Interrupted += OnChannel1Interrupted;
            this.counterBoard[0].channel2Interrupted += OnChannel2Interrupted;
            this.counterBoard[0].channel3Interrupted += OnChannel3Interrupted;
            this.counterBoard[0].channel4Interrupted += OnChannel4Interrupted;
            this.counterBoard[1].channel1Interrupted += OnChannel5Interrupted;
            this.counterBoard[1].channel2Interrupted += OnChannel6Interrupted;

			try
            {
                this.dataCheckThread = new Thread(execPaddleManage);
                this.dataCheckThread.Name = "DataCheckThread";
                this.dataCheckThread.Start();

				for(int i = 0 ; i < 6; i++)
				{
					var executer = new NozzleExecuter();
					executer.Number = i;
					executer.ShootStateChanged += (b) =>
					{ 
						if (this.paddleDrived != null)
							this.paddleDrived(this, new PaddleDriveEventArgs(this.counterBoard[0].GetOutputArray()));
					};
					NozzleExecuters.Add(executer);
				}

			}
            catch
            {

            }

            this.appLog.Info("初期化完了 ---電磁パドル管理---");

            return true;
        }

		// 素材データキューのチェックスレッド
		private void execPaddleManage()
        {
			try
			{ 
				uint counter = 0;

				while (true)
				{
					lock (lockObj)
					{
						for(int i = 0 ; i < 6 ; i++)
						{
							var queue = this.drivingQueue[i];

							try
							{
								//// 計測 PC とMopaLIBS データの整合性をカウンタ値でチェック
								//if (queue.Count > 0)
								//{
								//	if(i<4)
								//		this.counterBoard[0].getCounter(i + 1, out counter);
								//	else
								//		this.counterBoard[1].getCounter(i - 3, out counter);

								//	MaterialData firstData = queue.getFirst();
								//	if (firstData.DrivingCounter < counter)
								//	{
								//		// 先頭データを削除する。
								//		//MaterialData removeData = this.materialQueue.dequeue();
								//		//this.appLog.Info(string.Format("カウント値不整合で先頭データを削除 {0} - {1}", removeData.DrivingCounter, counter));
								//		throw new ApplicationException("カウント値不整合");
								//	}
								//}

								// データキューにデータが積まれておりパドル駆動がセットされていることが条件
								if (queue.Count > 0 && queue.CheckPaddleDrivingSetData())
								{
									// パドル駆動データがからの場合
									if (this.drivingData[i] == null)
									{
										this.drivingData[i] = queue.Dequeue();

										if(i < 4)
											this.counterBoard[0].getCounter(i + 1, out counter);
										else
											this.counterBoard[1].getCounter(i - 3, out counter);

										// ここで現在のカウント値を超えていないかチェックする。
										// 10は処理速度の下駄を履かせている
										var drivingCount = this.drivingData[i].DrivingCounter;
										if (drivingCount > counter + 10)
										{
											// パドル駆動タイミングをセット
											this.setPaddleDrivingTiming(i+1, drivingCount);
											this.appLog.Info(string.Format("EQ カウント値をセット 駆動カウント:{0} 現在カウント:{1}", drivingCount, counter));
										}
										else
										{
											this.drivingData[i] = null;
											this.appLog.Info(string.Format("カウンタ値追い越しでパドル駆動をスキップします。駆動カウント:{0} 現在カウント {1}", drivingCount, counter));
										}
									}
								}
							}
							catch
							{
								// 管理データを初期化する
								queue.ClearMaterialData();
								this.drivingData[i] = null;
							}
						}
					}
					Thread.Sleep(1);
				}
			}
			catch(ThreadAbortException)
			{

			}
        }

        // 素材データキューのチェックスレッド
        public void exitPaddleManage()
        {
            this.dataCheckThread.Abort();
        }

		// 素材クリア
		public void clearMaterialData()
		{
			for(int i = 0 ; i < 6 ; i++)
			{
				this.drivingData[i] = null;
				this.drivingQueue[i].ClearMaterialData();
			}
		}

		// パドル駆動タイミングのセット
		private void setPaddleDrivingTiming(int channel , UInt32 drivingCounter)
        {
            // 指定チャンネルの比較カウンタ値をセット

			if(channel <= 4)
			{ 
				if (!this.counterBoard[0].setEqualCounter(channel, drivingCounter))
				{
					throw new ApplicationException("電磁パドル駆動カウンタの設定に失敗しました。");
				}
			}
			else
			{
				if (!this.counterBoard[1].setEqualCounter(channel - 4, drivingCounter))
				{
					throw new ApplicationException("電磁パドル駆動カウンタの設定に失敗しました。");
				}
			}
        }

        // カウンタボードのチャンネル1の割り込みイベント
        private void OnChannel1Interrupted(object sender, CounterBoardEventArgs e)
        {
            // EQ フラグが立っているか
            if ((e.EventMask & (uint)0x00000004) > 0)
            {
                Task tsk = Task.Factory.StartNew(() =>
                {
                    // 電磁パドル駆動
                    this.setDriveTime(1);
                });
            }
        }

		// カウンタボードのチャンネル1の割り込みイベント
		private void OnChannel2Interrupted(object sender, CounterBoardEventArgs e)
		{
			// EQ フラグが立っているか
			if ((e.EventMask & (uint)0x00000004) > 0)
			{
				Task tsk = Task.Factory.StartNew(() =>
				{
					// 電磁パドル駆動
					this.setDriveTime(2);
				});
			}
		}

		// カウンタボードのチャンネル1の割り込みイベント
		private void OnChannel3Interrupted(object sender, CounterBoardEventArgs e)
		{
			// EQ フラグが立っているか
			if ((e.EventMask & (uint)0x00000004) > 0)
			{
				Task tsk = Task.Factory.StartNew(() =>
				{
					// 電磁パドル駆動
					this.setDriveTime(3);
				});
			}
		}

		// カウンタボードのチャンネル1の割り込みイベント
		private void OnChannel4Interrupted(object sender, CounterBoardEventArgs e)
		{
			// EQ フラグが立っているか
			if ((e.EventMask & (uint)0x00000004) > 0)
			{
				Task tsk = Task.Factory.StartNew(() =>
				{
					// 電磁パドル駆動
					this.setDriveTime(4);
				});
			}
		}

		// カウンタボードのチャンネル1の割り込みイベント
		private void OnChannel5Interrupted(object sender, CounterBoardEventArgs e)
		{
			// EQ フラグが立っているか
			if ((e.EventMask & (uint)0x00000004) > 0)
			{
				Task tsk = Task.Factory.StartNew(() =>
				{
					// 電磁パドル駆動
					this.setDriveTime(5);
				});
			}
		}

		// カウンタボードのチャンネル1の割り込みイベント
		private void OnChannel6Interrupted(object sender, CounterBoardEventArgs e)
		{
			// EQ フラグが立っているか
			if ((e.EventMask & (uint)0x00000004) > 0)
			{
				Task tsk = Task.Factory.StartNew(() =>
				{
					// 電磁パドル駆動
					this.setDriveTime(6);
				});
			}
		}

		// 電磁パドル駆動時間セット
		public void setDriveTime(int channel)
        {
            //lock (lockObj)
			lock(lockObjNozzle[channel-1])
            {
                if (this.drivingData[channel-1] != null)
                    {
					var mat = this.drivingData[channel-1];
						try
						{ 
						double materialMillimater = mat.LengthX / 4.0D;
							if(materialMillimater == 0)
								return;

							// 素材長制限を超えた場合、素材長制限値とする
							if (materialMillimater >= Settings.Default.MaxMaterialLength)
							{
							appLog.Debug($"素材長カウンタ値[{mat.LengthX}]=>長さ[{materialMillimater}]mmが最大素材長[{Settings.Default.MaxMaterialLength}]mmを超えた為、最大素材長に変換します");
								materialMillimater = Settings.Default.MaxMaterialLength;
							}

							double driveTimeMS = 0;
							double conveyorSpeed = ConveyorSettings.GetSelectedConveyor().Speed;
							driveTimeMS = 1000D / (conveyorSpeed / materialMillimater);
							//driveTimeMS += Settings.Default.AirNozzleBeforeTime + Settings.Default.AirNozzleAfterTime;
							driveTimeMS += NozzleSettings.Instance.DataList[channel - 1].BeforeInjectionTime +
											NozzleSettings.Instance.DataList[channel - 1].AfterInjectionTime;

							DateTime finishTime = DateTime.Now.AddMilliseconds(driveTimeMS);
							if(NozzleExecuters[channel-1].FinishExecTime < finishTime)
								NozzleExecuters[channel - 1].FinishExecTime = finishTime;
						}
						catch(Exception ex)
						{
							this.appLog.Error(ex);
						}

					this.drivingData[channel - 1] = null;
				}
            }
        }
    }
}
