using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LIBSController
{
	/// <summary>
	/// コンベア設定
	/// </summary>
	public class ConveyorSettings : IDisposable
	{
		#region プライベートフィールド

		/// <summary>
		/// 設定ファイル名
		/// </summary>
		private static string settingFilePath = @"C:\LIBS\ConveyorSettings.xml";

		#endregion

		#region 静的プロパティ

		/// <summary>
		/// インスタンス
		/// </summary>
		public static ConveyorSettings Instance { get; set; } //= new ConveyorSettings();

		#endregion

		#region 子クラス

		/// <summary>
		/// コンベア設定
		/// </summary>
		public class ConveyorSetting
		{
			/// <summary>
			/// 表示名
			/// </summary>
			public string Name=> (Speed/1000D).ToString("F1")+" m/s";

			/// <summary>
			/// 速度（mm/sec)
			/// </summary>
			public int Speed { get; set; }

			/// <summary>
			/// インバーターパルス周波数
			/// </summary>
			public uint PulseFrequency { get ; set; }

			/// <summary>
			/// 選択中フラグ
			/// </summary>
			public bool Selected { get; set; }

			/// <summary>
			/// ノズルタイミングリスト
			/// </summary>
			public List<uint> NozzleTimingList { get; set; }
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// 設定値
		/// </summary>
		public List<ConveyorSetting> DataList { get; set; } = new List<ConveyorSetting>();

		#endregion

		#region 公開メソッド

		/// <summary>
		/// 選択中コンベアデータを返す
		/// </summary>
		/// <returns></returns>
		public static ConveyorSetting GetSelectedConveyor()
		{
			return Instance.DataList.FirstOrDefault(d=>d.Selected);
		}

		/// <summary>
		/// ノズルタイミングを返す
		/// </summary>
		/// <returns></returns>
		public static uint GetNozzleTiming(int nozzleNumber)
		{
			return GetSelectedConveyor().NozzleTimingList[nozzleNumber - 1];
		}

		/// <summary>
		/// ノズルタイミングを返す
		/// </summary>
		/// <returns></returns>
		public static List<uint> GetNozzleTiming()
		{
			return new List<uint>( GetSelectedConveyor().NozzleTimingList );
		}

		/// <summary>
		/// ノズルタイミングをセットする
		/// </summary>
		/// <returns></returns>
		public static void SetNozzleTiming(int nozzleNumber,uint value)
		{
			GetSelectedConveyor().NozzleTimingList[nozzleNumber - 1] = value;
		}

		/// <summary>
		/// ノズルタイミングをセットする
		/// </summary>
		/// <returns></returns>
		public static void SetNozzleTiming(List<uint> value)
		{
			GetSelectedConveyor().NozzleTimingList = value;
		}

		/// <summary>
		/// シリアライズ
		/// </summary>
		public static void Serialize()
		{
			try
			{
				ApplicationLog.getInstance().Info($"コンベア設定ファイルを書き込みます。パス{settingFilePath}");

				using (var streamWriter = new StreamWriter(settingFilePath, false, Encoding.UTF8))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(ConveyorSettings));
					serializer.Serialize(streamWriter, Instance);
					streamWriter.Flush();
				}
			}
			catch (Exception ex)
			{
			}
		}

		/// <summary>
		/// デシリアライズ
		/// </summary>
		public static void Deserialize(string filePath)
		{
			settingFilePath = filePath;

			try
			{
				// 設定ファイルが見つからない場合、初期値で設定ファイルを生成する
				if (!File.Exists(settingFilePath))
				{
					ApplicationLog.getInstance().Error($"コンベア設定ファイルが見つかりません。初期化します。パス{settingFilePath}");
					Instance = new ConveyorSettings();
					Instance.DataList = new List<ConveyorSetting>()
					{
#if true
						{new ConveyorSetting(){ Speed=1000 , PulseFrequency=2010 , Selected=true , NozzleTimingList = new List<uint>(){ 1000, 2000, 3000, 4000, 5000, 6000 } } },
						{new ConveyorSetting(){ Speed=1500 , PulseFrequency=3015 , Selected=false, NozzleTimingList = new List<uint>(){ 1000, 2000, 3000, 4000, 5000, 6000 } } }
#else
						{new ConveyorSetting(){ Speed=500  , PulseFrequency=1005 , Selected=true} },
						{new ConveyorSetting(){ Speed=1000 , PulseFrequency=2010 , Selected=false} },
						{new ConveyorSetting(){ Speed=1500 , PulseFrequency=4020 , Selected=false} },
						{new ConveyorSetting(){ Speed=2000 , PulseFrequency=6030 , Selected=false} }
#endif
					};

					Serialize();
					return;
				}
				else
				{
					ApplicationLog.getInstance().Info($"コンベア設定ファイルを読み込みます。パス{settingFilePath}");

					var serializer = new XmlSerializer(typeof(ConveyorSettings));
					var xmlSettings = new System.Xml.XmlReaderSettings()
					{
						CheckCharacters = false,
					};

					using (var streamReader = new StreamReader(settingFilePath, Encoding.UTF8))
					using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
					{
						Instance = (ConveyorSettings)serializer.Deserialize(xmlReader);
					}
				}
			}
			catch (Exception ex)
			{
				ApplicationLog.getInstance().Error(ex);
				Instance = new ConveyorSettings();
			}
		}

		#endregion

		#region 非公開メソッド

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ConveyorSettings()
		{
		}

		///// <summary>
		///// スタティックコンストラクタ
		///// </summary>
		//static ConveyorSettings()
		//{
		//	try
		//	{
		//		Deserialize();
		//	}
		//	catch
		//	{

		//	}
		//}

		#endregion

		#region IDisposable メンバー

		// Flag: Has Dispose already been called?
		bool disposed = false;
		~ConveyorSettings()
		{
			Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here.
				//
				//this.waitHandle.Set();
				Serialize();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
		}
		#endregion
	}
}
