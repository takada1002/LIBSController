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
	public class NozzleSettings : IDisposable
	{
		#region プライベートフィールド

		/// <summary>
		/// 設定ファイル名
		/// </summary>
		private static string settingFilePath = @"C:\LIBS\NozzleSettings.xml";

		#endregion

		#region 静的プロパティ

		/// <summary>
		/// インスタンス
		/// </summary>
		public static NozzleSettings Instance { get; set; } //= new NozzleSettings();

		#endregion

		#region 子クラス

		/// <summary>
		/// コンベア設定
		/// </summary>
		public class NozzleSetting
		{
			/// <summary>
			/// 前噴射時間
			/// </summary>
			public int BeforeInjectionTime { get; set; }

			/// <summary>
			/// 後噴射時間
			/// </summary>
			public int AfterInjectionTime { get; set; }
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// 設定値
		/// </summary>
		public List<NozzleSetting> DataList { get; set; } = new List<NozzleSetting>();

		#endregion

		#region 公開メソッド

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
					XmlSerializer serializer = new XmlSerializer(typeof(NozzleSettings));
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
					Instance = new NozzleSettings();
					Instance.DataList = new List<NozzleSetting>()
					{
						{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
						{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
						{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
						{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
						{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } }
					};

					Serialize();
					return;
				}
				else
				{
					ApplicationLog.getInstance().Info($"コンベア設定ファイルを読み込みます。パス{settingFilePath}");

					var serializer = new XmlSerializer(typeof(NozzleSettings));
					var xmlSettings = new System.Xml.XmlReaderSettings()
					{
						CheckCharacters = false,
					};

					using (var streamReader = new StreamReader(settingFilePath, Encoding.UTF8))
					using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
					{
						Instance = (NozzleSettings)serializer.Deserialize(xmlReader);
					}
				}
			}
			catch (Exception ex)
			{
				ApplicationLog.getInstance().Error(ex);
				Instance = new NozzleSettings();
				Instance.DataList = new List<NozzleSetting>()
				{
					{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
					{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
					{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
					{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } },
					{new NozzleSetting(){ BeforeInjectionTime = 100 , AfterInjectionTime = 100 } }
				};
				Serialize();
			}
		}

		#endregion

		#region 非公開メソッド

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private NozzleSettings()
		{
		}

		///// <summary>
		///// スタティックコンストラクタ
		///// </summary>
		//static NozzleSettings()
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
		~NozzleSettings()
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
