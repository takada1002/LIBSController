using LIBSController.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBSController
{
    /// <summary>
    /// 素材情報管理キュー
    /// </summary>
    public class DrivingQueue
    {
        public static List<DrivingQueue> Instance { get; } = new List<DrivingQueue>()
		{
			new DrivingQueue(),
			new DrivingQueue(),
			new DrivingQueue(),
			new DrivingQueue(),
			new DrivingQueue(),
			new DrivingQueue()
		};

        // lock オブジェクト
        private Object lockObj = new Object();

        // 素材管理キュー
        private Queue<DrivingData> dataQueue = new Queue<DrivingData>();

        // 素材管理キューにデータを追加
        public Boolean Enqueue(DrivingData data)
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
        public DrivingData Dequeue()
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

        // パドル駆動の有無がセットされているデータがあるかチェック
        public Boolean CheckPaddleDrivingSetData()
        {
            Boolean result = false;

            lock (lockObj)
            {
                foreach (DrivingData data in dataQueue)
                {
                    // 1つでもセットされていれば良い
                    result = true;
                    break;
                }
            }

            return result;
        }

        // 保持データのクリア
        public void ClearMaterialData()
        {
            lock (lockObj)
            {
                this.dataQueue.Clear();
            }
        }
    }

    /// <summary>
    /// 運転情報
    /// </summary>
    public class DrivingData
	{
        // 位置計測時の X方向 (カウンタ値)
        private UInt32 PosX;

        // X軸方向の長さ
        public UInt32 LengthX = 0;

		// ノズル番号
		public int NozzleNumber;

		// パドル駆動タイミングを計算したかどうか
		private bool IsDrivingCounterCalc = false;

		// パドル駆動タイミング (カウント値)
		private UInt32 _DrivingCounter;

        public UInt32 DrivingCounter
        {
            get
			{
				if(!IsDrivingCounterCalc)
				{ 
					if (NozzleNumber > 0)
					{
						uint nozzleTiming = ConveyorSettings.GetNozzleTiming(NozzleNumber);
						int speed = ConveyorSettings.GetSelectedConveyor().Speed;
						int beforeTimeMs = NozzleSettings.Instance.DataList[NozzleNumber - 1].BeforeInjectionTime;
						uint beforeCount = Convert.ToUInt32(beforeTimeMs * (speed / 1000D * 4D));
						if ((nozzleTiming - beforeCount) > 100)
							this._DrivingCounter = this.PosX + nozzleTiming - beforeCount;
						else
							this._DrivingCounter = this.PosX + 100;
					}
					IsDrivingCounterCalc = true;
				}
				return _DrivingCounter;
			}
        }

		// 第3引数は物体のX軸方向の長さ
		public DrivingData(UInt32 posX,UInt32 lengthX, int nozzleNumber)
		{
			this.PosX = posX;
            this.LengthX = lengthX;
			this.NozzleNumber = nozzleNumber;
		}
	}
}
