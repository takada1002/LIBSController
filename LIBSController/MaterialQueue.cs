using LIBSController.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBSController
{
    // 素材情報管理キュー
    class MaterialQueue
    {
        public static List<MaterialQueue> Instance { get; } = new List<MaterialQueue>()
		{
			new MaterialQueue(),
			new MaterialQueue(),
			new MaterialQueue(),
			new MaterialQueue(),
			new MaterialQueue(),
			new MaterialQueue()
		};

        // lock オブジェクト
        private Object lockObj = new Object();

        // 素材管理キュー
        private Queue<MaterialData> dataQueue = new Queue<MaterialData>();

        // 素材管理キューにデータを追加
        public Boolean enqueue(MaterialData data)
        {
            Boolean result = false;
            lock (lockObj)
            {
                // データ数チェック
                if (this.Count < Settings.Default.MaxDataCount)
                {
                    this.dataQueue.Enqueue(data);
                    result = true;
                }
            }
            return result;
        }

        // 素材管理キューからデータを取出
        public MaterialData dequeue()
        {
            lock (lockObj)
            {
                return this.dataQueue.Dequeue();
            }
        }

        // 保持データ数
        public Int32 Count
        {
            get { return this.dataQueue.Count; }
        }

        // パドル駆動の有無がセットされていない一番先頭のデータを検索
        public MaterialData searchPaddleDrivingUnsetData()
        {
            MaterialData retData = null;

            lock (lockObj)
            {
                foreach (MaterialData data in dataQueue)
                {
                    if (data.IsSet == false)
                    {
                        retData = data;
                        break;
                    }
                }
            }

            return retData;
        }

        // パドル駆動の有無がセットされているデータがあるかチェック
        public Boolean checkPaddleDrivingSetData()
        {
            Boolean result = false;

            lock (lockObj)
            {
                foreach (MaterialData data in dataQueue)
                {
                    // 1つでもセットされていれば良い
                    result = result | data.IsSet;
                }
            }

            return result;
        }

        // 保持データのクリア
        public void clearMaterialData()
        {
            lock (lockObj)
            {
                this.dataQueue.Clear();
            }
        }

        public MaterialData getFirst()
        {
            MaterialData retData = null;

            lock (lockObj)
            {
                if (this.dataQueue.Count > 0)
                {
                    retData = this.dataQueue.First();
                }
            }

            return retData;
        }
    }

    // 素材情報
    class MaterialData
    {
        // 位置計測時の X方向 (カウンタ値)
        private UInt32 posX;
        // X軸方向の長さ
        public UInt32 lengthX = 0;
		// 素材番号
		public int materialNumber;
		// ノズル番号
		public int nozzleNumber;
		// パドル駆動タイミングを計算したかどうか
		private bool isDrivingCounterCalc = false;
		// パドル駆動タイミング (カウント値)
		private UInt32 drivingCounter;
        public UInt32 DrivingCounter
        {
            get
			{
				if(!isDrivingCounterCalc)
				{ 
					nozzleNumber = MaterialSetting.getInstance().getNozzleNumber(materialNumber);
					if (nozzleNumber > 0)
					{
						uint nozzleTiming = ConveyorSettings.GetNozzleTiming(nozzleNumber);
						int speed = ConveyorSettings.GetSelectedConveyor().Speed;
						//int beforeTimeMs = Settings.Default.AirNozzleBeforeTime;
						int beforeTimeMs = NozzleSettings.Instance.DataList[nozzleNumber - 1].BeforeInjectionTime;
						uint beforeCount = Convert.ToUInt32(beforeTimeMs * (speed / 1000D * 4D));
						if ((nozzleTiming - beforeCount) > 100)
							this.drivingCounter = this.posX + nozzleTiming - beforeCount;
						else
							this.drivingCounter = this.posX + 100;
					}
					isDrivingCounterCalc = true;
				}
				return drivingCounter;
			}
        }

        // パドル駆動の有無
        private Boolean isDriving = false;
        public Boolean IsDriving
        {
            get{ return isDriving; }
            set{ isDriving = value; }
        }

        // パドル駆動の有無がセット済かどうか
        private Boolean isSet = false;
        public Boolean IsSet
        {
            get { return isSet; }
            set { isSet = value; }
        }

		// 第3引数は物体のX軸方向の長さ
		public MaterialData(UInt32 posX, UInt32 length , int materialNumber)
		{
			this.lengthX = length;
			this.posX = posX;
			this.materialNumber = materialNumber;

			// 素材からノズルを特定
			nozzleNumber = MaterialSetting.getInstance().getNozzleNumber(materialNumber);

			// パドル駆動タイミングをセット
			//this.drivingCounter = this.posX + Settings.Default.PaddleDriveTiming;
			//if(nozzleNumber > 0)
			//{ 
			//	uint nozzleTiming = ConveyorSettings.GetNozzleTiming(nozzleNumber);
			//	int speed = ConveyorSettings.GetSelectedConveyor().Speed;
			//	//int beforeTimeMs = Settings.Default.AirNozzleBeforeTime;
			//	int beforeTimeMs = NozzleSettings.Instance.DataList[nozzleNumber-1].BeforeInjectionTime;
			//	uint beforeCount = Convert.ToUInt32( beforeTimeMs * (speed / 1000D * 4D));
			//	if( (nozzleTiming - beforeCount) > 100 )
			//		this.drivingCounter = this.posX + nozzleTiming - beforeCount;
			//	else
			//		this.drivingCounter = this.posX + 100;
			//}
		}
	}
}
