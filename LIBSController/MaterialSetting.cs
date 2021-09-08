using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBSController
{
    // 素材選別設定保持用のクラス
    public class MaterialSetting
    {
        // 最大設定数
        //private static int MAX_SETTING = 16;

        // インスタンス
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public static MaterialSetting Instance;
        // 設定ファイル名
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public String fileName { get; set; }
        // Error シグナル
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Boolean errorFlag { get; set; }

        // 設定
        [System.Xml.Serialization.XmlElement("MaterialSetting")]
        public List<MaterialClass> materialClassList = new List<MaterialClass>();

        // インスタンスを取得する
        public static MaterialSetting getInstance()
        {
            if (Instance == null)
            {
                Instance = new MaterialSetting();
            }

            return Instance;
        }

        // シリアライズ化して設定を保存する
        public void Serialize()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialSetting));

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding("UTF-8")))
            {
                serializer.Serialize(writer, this);
            }
        }

        // デシリアライズ化して設定を読み出す
        public static MaterialSetting Deserialize(String fName)
        {
            // すでにインスタンスが存在していればnull参照に置き換え
            if (!(Instance == null))
            {
                Instance = null;
            }

            try
            {
                // デシリアライズしたオブジェクトでインスタンスを生成
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialSetting));
                using (StreamReader reader = new StreamReader(fName, Encoding.GetEncoding("UTF-8")))
                {
                    Instance = (MaterialSetting)serializer.Deserialize(reader);
                }
                Instance.errorFlag = false;

#if DEBUG
                Instance.materialClassList = new List<MaterialClass>()
				{
                    new MaterialClass(){ classCode = 1 , className = "Cu base" },
                    new MaterialClass(){ classCode = 1 , className = "Cu-Zn base" },
                    new MaterialClass(){ classCode = 1 , className = "Zn base" },
                    new MaterialClass(){ classCode = 1 , className = "Fe-Cr-Ni base" },
                    new MaterialClass(){ classCode = 1 , className = "Al base" },
                    new MaterialClass(){ classCode = 1 , className = "Cr-Ni-coating" },
                    new MaterialClass(){ classCode = 1 , className = "Fe base" },
                    new MaterialClass(){ classCode = 1 , className = "Fe-Cr base" },
				};
#endif
            }
            catch (Exception)
            {
                // 例外コール時はインスタンス手動生成
                Instance = new MaterialSetting();
                Instance.errorFlag = true;
            }

            Instance.fileName = fName;

            return Instance;
        }

		public int getNozzleNumber(int classCode)
		{
			int nozzleNumber = 0;

			try
			{
				nozzleNumber = this.materialClassList[classCode].nozzleNumber;
			}
			catch
			{
				// 非選別対象とする。
				nozzleNumber = 0;
			}

			return nozzleNumber;
		}

        public int getNozzleNumber2(int classCode)
		{
			int nozzleNumber2 = 0;

			try
			{
				nozzleNumber2 = this.materialClassList[classCode].nozzleNumber2;
			}
			catch
			{
				// 非選別対象とする。
				nozzleNumber2 = 0;
			}

			return nozzleNumber2;
		}

        public String getClassName(int classCode)
        {
            String className = string.Empty;

            try
            {
				if(this.materialClassList.Count > classCode)
				{
					className = this.materialClassList[classCode].className;
				}
				else
				{
					className = string.Empty;
				}
            }
            catch
            {
                // 規定値以外のコードは全て非選別対象とする。
                className = string.Empty;
            }

            return className;
        }
    }

    // 素材クラス
    public class MaterialClass
    {
        // クラス名 
        [System.Xml.Serialization.XmlElement("ClassName")]
        public String className { get; set; }
        // クラスコード番号
        [System.Xml.Serialization.XmlElement("ClassCode")]
        public Int32 classCode { get; set; }
		// エアノズル番号
		[System.Xml.Serialization.XmlElement("NozzleNumber")]
		public int nozzleNumber { get; set; }
        [System.Xml.Serialization.XmlElement("NozzleNumber2")]
		public int nozzleNumber2 { get; set; }
	}
}
