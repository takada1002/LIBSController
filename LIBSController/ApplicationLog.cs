//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;

//namespace LIBSController
//{
//    class ApplicationLog
//    {
//        public enum LogLevel
//        {
//            None = 0,
//            Debug = 1,
//            Info = 2,
//            Warn = 4,
//            Error = 8,
//            Fatal = 16,
//            ALL = 31,
//            Release = 30
//        }

//        // インスタンス
//        private static ApplicationLog Instance;

//        // 出力ログのレベルを設定する
//        private LogLevel level = LogLevel.Debug;
//        public LogLevel Level 
//        {
//            get { return level; }
//            set { level = value; } 
//        }

//        // ログ出力ディレクトリの設定
//        private String outputDirectory = String.Empty;
//        public string OutputDirectory
//        {
//            get { return outputDirectory; }
//            set
//            {
//                if (value.EndsWith("log"))
//                {
//                    outputDirectory = value;
//                }
//                else
//                {
//                    outputDirectory = value + "\\log";
//                }

//                if (!Directory.Exists(outputDirectory))
//                {
//                    Directory.CreateDirectory(outputDirectory);
//                }
//            }
//        }

//        // ログファイルの接頭語
//        // xxxxx-日付.log の xxxxx 部分
//        private String namePrefix = String.Empty;
//        public string NamePrefix
//        {
//            get { return namePrefix; }
//            set { namePrefix = value; }
//        }

//        private Encoding encoding = Encoding.GetEncoding("UTF-8");

//        public Action<LogLevel,string> LogWrited = null;

//        // ログはシングルトン
//        public static ApplicationLog getInstance()
//        {
//            if(Instance == null)
//            {
//                Instance = new ApplicationLog();
//            }

//            return Instance;
//        }

//        // ログ出力
//        private void WriteLine(LogLevel level, String msg)
//        {

//            // 日付
//            DateTime now = DateTime.Now;

//            String outStr = string.Format("[{0}] {1} {2}", now.ToString("HH:mm:ss.fff"), level.ToString().ToUpper(), msg);

//            // アウトプットディレクトリが設定されていなければデフォルト
//            if (outputDirectory.Equals(String.Empty))
//            {
//                OutputDirectory = "log";
//            }

//            // ファイル名プリフィックス
//            if (namePrefix.Equals(String.Empty))
//            {
//                NamePrefix = "applog";
//            }

//            String filename = String.Format("{0}\\{1}-{2}.log", outputDirectory, namePrefix, DateTime.Now.ToString("yyyyMMdd"));

//            try
//            {
//                using (StreamWriter sr = new StreamWriter(filename, true, encoding))
//                {
//                    sr.WriteLine(outStr);
//                }
//            }
//            catch
//            {

//            }
//        }

//        // Debug ログ書き
//        public void Debug(string s)
//        {
//            if(LogWrited != null)
//                LogWrited(LogLevel.Debug, s);
//            if ((this.level & LogLevel.Debug) == 0)
//                return;
//            WriteLine(LogLevel.Debug, s);
//        }

//        public void Debug(Exception ex)
//        {
//            if ((this.level & LogLevel.Debug) == 0)
//                return;
//            if (ex.InnerException != null)
//            {
//                Exception inner = ex.InnerException;
//                Debug(string.Format("{0} {{{1}}}{2}{3}", inner.Message, inner.GetType().ToString(), Environment.NewLine, inner.StackTrace));
//            }
//            Debug(string.Format("{0} {{{1}}}{2}{3}", ex.Message, ex.GetType().ToString(), Environment.NewLine, ex.StackTrace));
//        }

//        // Info ログ書き
//        public void Info(string s)
//        {
//            if (LogWrited != null)
//                LogWrited(LogLevel.Info, s);
//            if ((this.level & LogLevel.Info) == 0)
//                return;
//            WriteLine(LogLevel.Info, s);
//        }

//        public void Info(Exception ex)
//        {
//            if ((this.level & LogLevel.Info) == 0)
//                return;
//            if (ex.InnerException != null)
//            {
//                Exception inner = ex.InnerException;
//                Info(string.Format("{0} {{{1}}}{2}{3}", inner.Message, inner.GetType().ToString(), Environment.NewLine, inner.StackTrace));
//            }
//            Info(string.Format("{0} {{{1}}}{2}{3}", ex.Message, ex.GetType().ToString(), Environment.NewLine, ex.StackTrace));
//        }

//        // Warn ログ書き
//        public void Warn(string s)
//        {
//            if (LogWrited != null)
//                LogWrited(LogLevel.Warn, s);
//            if ((this.level & LogLevel.Warn) == 0)
//                return;
//            WriteLine(LogLevel.Warn, s);
//        }

//        public void Warn(Exception ex)
//        {
//            if ((this.level & LogLevel.Warn) == 0)
//                return;
//            if (ex.InnerException != null)
//            {
//                Exception inner = ex.InnerException;
//                Warn(string.Format("{0} {{{1}}}{2}{3}", inner.Message, inner.GetType().ToString(), Environment.NewLine, inner.StackTrace));
//            }
//            Warn(string.Format("{0} {{{1}}}{2}{3}", ex.Message, ex.GetType().ToString(), Environment.NewLine, ex.StackTrace));
//        }

//        // Error ログ書き
//        public void Error(string s)
//        {
//            if (LogWrited != null)
//                LogWrited(LogLevel.Error, s);
//            if ((this.level & LogLevel.Error) == 0)
//                return;
//            WriteLine(LogLevel.Error, s);
//        }

//        public void Error(Exception ex)
//        {
//            if ((this.level & LogLevel.Error) == 0)
//                return;
//            if (ex.InnerException != null)
//            {
//                Exception inner = ex.InnerException;
//                Error(string.Format("{0} {{{1}}}{2}{3}", inner.Message, inner.GetType().ToString(), Environment.NewLine, inner.StackTrace));
//            }
//            Error(string.Format("{0} {{{1}}}{2}{3}", ex.Message, ex.GetType().ToString(), Environment.NewLine, ex.StackTrace));
//        }

//        // Fatal ログ書き
//        public void Fatal(string s)
//        {
//            if (LogWrited != null)
//                LogWrited(LogLevel.Fatal, s);
//            if ((this.level & LogLevel.Fatal) == 0)
//                return;
//            WriteLine(LogLevel.Fatal, s);
//        }

//        public void Fatal(Exception ex)
//        {
//            if ((this.level & LogLevel.Fatal) == 0)
//                return;
//            if (ex.InnerException != null)
//            {
//                Exception inner = ex.InnerException;
//                Fatal(string.Format("{0} {{{1}}}{2}{3}", inner.Message, inner.GetType().ToString(), Environment.NewLine, inner.StackTrace));
//            }
//            Fatal(string.Format("{0} {{{1}}}{2}{3}", ex.Message, ex.GetType().ToString(), Environment.NewLine, ex.StackTrace));
//        }

//        // 特定期間経過のログを削除する
//        public void Delete(TimeSpan span)
//        {
//            DateTime date = DateTime.Now.Subtract(span);

//            if (false == Directory.Exists(outputDirectory))
//                return;

//            string[] files = System.IO.Directory.GetFiles(outputDirectory, namePrefix + "-????????.log");

//            if (files != null)
//            {
//                string fileName;
//                DateTime fileDate = new DateTime();
//                DateTime compDate = date;

//                foreach (string s in files)
//                {
//                    fileName = System.IO.Path.GetFileName(s).Replace(namePrefix + "-", "");

//                    if (!IsYYYYMMDDString(fileName, out fileDate))
//                        continue;

//                    // 指定日付より前のものは削除
//                    if (fileDate.CompareTo(compDate) <= 0)
//                    {
//                        this.Info("ログファイルを削除：" + s);
//                        System.IO.File.Delete(s);
//                    }
//                }
//            }
//        }

//        private bool IsYYYYMMDDString(string fileName, out DateTime fileDate)
//        {
//            int yyyy, mm, dd;

//            try
//            {
//                for (int i = 0; i < fileName.Length - 8; i++)
//                {
//                    // 年をParse
//                    yyyy = int.Parse(fileName.Substring(i, 4));

//                    // 有効な年か
//                    if (yyyy < 1980 || yyyy > 9999)
//                    {
//                        // 無効な年だった
//                        continue;
//                    }

//                    // 月をParse
//                    mm = int.Parse(fileName.Substring(i + 4, 2));

//                    // 有効な月か
//                    if (mm < 1 || mm > 12)
//                    {
//                        // 無効な月だった
//                        continue;
//                    }

//                    // 日をParse
//                    dd = int.Parse(fileName.Substring(i + 6, 2));

//                    // 有効な日か
//                    if (dd < 1 || dd > DateTime.DaysInMonth(yyyy, mm))
//                    {
//                        // 無効な日だった
//                        continue;
//                    }

//                    fileDate = new DateTime(yyyy, mm, dd);
//                    return true;
//                }
//            }
//            catch (Exception)
//            {
//                fileDate = new DateTime();

//                return false;
//            }

//            fileDate = new DateTime();
//            return false;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace LIBSController
{
	/// <summary>
	/// ログクラス（シングルトン）
	/// </summary>
	public class ApplicationLog : IDisposable
	{
		#region フィールド

		/// <summary>
		/// ログ管理キュー
		/// </summary>
		private Queue<LogData> logQueue = new Queue<LogData>();

		/// <summary>
		/// ロックオブジェクト
		/// </summary>
		private Object lockObj = new Object();

		/// <summary>
		/// ログ処理スレッド
		/// </summary>
		private Thread loggingThread = null;

		/// <summary>
		/// 設定ファイル名
		/// </summary>
		private static string settingFileName = "ApplicationLogSettings.xml";

		/// <summary>
		/// 設定ファイルディレクトリ
		/// </summary>
		public static string settingFileDirectory = "";

		/// <summary>
		/// ロギングフラグ
		/// </summary>
		private bool loggingFlag = true;

		#endregion

		#region 型

		/// <summary>
		/// ログレベル
		/// </summary>
		public enum LogLevel
		{
			None = 0x00,
			Debug = 0x01,
			SQL = 0x02,
			Info = 0x04,
			Warn = 0x08,
			Error = 0x10,
			Fatal = 0x20,
			ALL = 0xFF,
			Release = 0x3C
		}

		/// <summary>
		/// ログデータクラス
		/// </summary>
		public class LogData
		{
			/// <summary>
			/// ログレベル
			/// </summary>
			public LogLevel Level { get; set; } = LogLevel.None;

			/// <summary>
			/// ログ時刻
			/// </summary>
			public DateTime WriteTime { get; set; }

			/// <summary>
			/// ログ文字列
			/// </summary>
			public String Message { get; set; }

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="level">ログ例外ベル</param>
			/// <param name="writeTime">ログ時刻</param>
			/// <param name="message">ログ文字列</param>
			public LogData(ApplicationLog.LogLevel level, DateTime writeTime, String message)
			{
				this.Level = level;
				this.WriteTime = writeTime;
				this.Message = message;
			}

			/// <summary>
			/// 書き込み文字列取得
			/// </summary>
			/// <returns>書き込み文字列</returns>
			public string GetFullMessage()
			{
				string programName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
				return $"[{WriteTime.ToString("HH:mm:ss.fff")}] {programName} {Level.ToString().ToUpper()} {Message}";
			}
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// インスタンス
		/// </summary>
		public static ApplicationLog Instance { get; set; }

		/// <summary>
		/// ログレベル
		/// </summary>
		public byte Level { get; set; } = (byte)LogLevel.ALL;

		/// <summary>
		/// 外部ファイルへの書き出しの有無
		/// </summary>
		public Boolean IsFileOutput { get; set; } = true;

		/// <summary>
		/// ログ出力ディレクトリ
		/// </summary>
		public string OutputDirectory { get; set; } = "log";

		/// <summary>
		/// ログファイルの接頭語
		/// </summary>
		/// <remarks>
		/// xxxxx_日付.log の xxxxx 部分
		/// </remarks>
		public string NamePrefix { get; set; } = "applog";

		/// <summary>
		/// 過去ログ保存期間
		/// </summary>
		public int PastLogSaveDays { get; set; } = 365;

		/// <summary>
		/// 過去ログ削除有無
		/// </summary>
		public bool IsPastLogDelete { get; set; } = true;




		#endregion

		#region イベント

		/// <summary>
		/// ログ書き込み完了イベント
		/// </summary>
		[XmlIgnore]
		public Action<LogLevel, string> LogWrited = null;

		#endregion

		#region 公開メソッド

		public static ApplicationLog getInstance()
		{
			return Instance;
		}

		/// <summary>
		/// ログ書き込み
		/// </summary>
		/// <param name="level">ログレベル</param>
		/// <param name="s">ログ文字列</param>
		public void Write(LogLevel level, string s)
		{
			if ((Level & (byte)level) == 0)
				return;
			AddLog(new LogData(level, DateTime.Now, s));
		}

		/// <summary>
		/// ログ書き込み
		/// </summary>
		/// <param name="level">ログレベル</param>
		/// <param name="ex">ログ文字列</param>
		public void Write(LogLevel level, Exception ex)
		{
			Write(level, GetExceptionString(ex));
		}

		/// <summary>
		/// Debugログ書き込み
		/// </summary>
		/// <param name="s">ログ文字列</param>
		public void Debug(string s)
		{
			if ((Level & (byte)LogLevel.Debug) == 0)
				return;
			AddLog(new LogData(LogLevel.Debug, DateTime.Now, s));
		}

		/// <summary>
		/// Debugログ書き込み
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		public void Debug(Exception ex)
		{
			Debug(GetExceptionString(ex));
		}

		/// <summary>
		/// Infoログ書き込み
		/// </summary>
		/// <param name="s">ログ文字列</param>
		public void Info(string s)
		{
			if ((Level & (byte)LogLevel.Info) == 0)
				return;
			AddLog(new LogData(LogLevel.Info, DateTime.Now, s));
		}

		/// <summary>
		/// Infoログ書き込み
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		public void Info(Exception ex)
		{
			Info(GetExceptionString(ex));
		}

		/// <summary>
		/// Warnログ書き込み
		/// </summary>
		/// <param name="s">ログ文字列</param>
		public void Warn(string s)
		{
			if ((Level & (byte)LogLevel.Warn) == 0)
				return;
			AddLog(new LogData(LogLevel.Warn, DateTime.Now, s));
		}

		/// <summary>
		/// Warnログ書き込み
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		public void Warn(Exception ex)
		{
			Warn(GetExceptionString(ex));
		}

		/// <summary>
		/// Errorログ書き込み
		/// </summary>
		/// <param name="s">ログ文字列</param>
		public void Error(string s)
		{
			if ((Level & (byte)LogLevel.Error) == 0)
				return;
			AddLog(new LogData(LogLevel.Error, DateTime.Now, s));
		}

		/// <summary>
		/// Errorログ書き込み
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		public void Error(Exception ex)
		{
			Error(GetExceptionString(ex));
		}

		/// <summary>
		/// Fatalログ書き込み
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		public void Fatal(string s)
		{
			if ((Level & (byte)LogLevel.Fatal) == 0)
				return;
			AddLog(new LogData(LogLevel.Fatal, DateTime.Now, s));
		}

		/// <summary>
		/// Fatalログ書き込み
		/// </summary>
		/// <param name="ex">例外オブジェクト</param>
		public void Fatal(Exception ex)
		{
			Fatal(GetExceptionString(ex));
		}

		/// <summary>
		/// SQL書き込み
		/// </summary>
		/// <param name="s">ログ文字列</param>
		public void SQL(string s)
		{
			if ((Level & (byte)LogLevel.SQL) == 0)
				return;
			AddLog(new LogData(LogLevel.SQL, DateTime.Now, s));
		}

		/// <summary>
		/// 特定期間が経過したログを削除する
		/// </summary>
		/// <param name="span">期間</param>
		public void PastLogDelete()
		{
			DateTime compDate = DateTime.Now.Subtract(new TimeSpan(PastLogSaveDays, 0, 0, 0));

			if (!Directory.Exists(OutputDirectory))
				return;

			string[] files = Directory.GetFiles(OutputDirectory, NamePrefix + "_????????.log");

			if (files != null)
			{
				string fileName;

				foreach (string s in files)
				{
					fileName = Path.GetFileName(s).Replace(NamePrefix + "_", "");

					if (!CheckFileNameFormat(fileName, out DateTime fileDate))
						continue;

					// 指定日付より前のものは削除
					if (fileDate.CompareTo(compDate) <= 0)
					{
						this.Info("ログファイルを削除：" + s);
						File.Delete(s);
					}
				}
			}
		}

		#endregion

		#region 非公開メソッド

		/// <summary>
		/// スタティックコンストラクタ
		/// </summary>
		static ApplicationLog()
		{
			try
			{
				//Deserialize();
			}
			catch
			{

			}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ApplicationLog()
		{
			try
			{
				this.loggingThread = new Thread(ExecLogging);
				this.loggingThread.Name = "LoggingThread";
				this.loggingThread.IsBackground = true;
				this.loggingThread.Start();

				// 過去ログ削除有効の場合
				if (IsPastLogDelete)
				{
					// 削除
					PastLogDelete();
				}
			}
			catch
			{

			}
		}

		/// <summary>
		/// ログ処理
		/// </summary>
		private void ExecLogging()
		{
			StringBuilder logTextBuilder = new StringBuilder();

			//while (!waitHandle.WaitOne(0, false))
			while (loggingFlag)
			{

				try
				{
					// データキューにデータが積まれておりパドル駆動がセットされていることが条件
					if (this.logQueue.Count > 0)
					{
						logTextBuilder.Clear();

						// キューメッセージを繋げる
						while (this.logQueue.Count > 0)
						{
							LogData logData = null;
							lock (lockObj)
							{
								logData = this.logQueue.Dequeue();
							}
							// ログ出力
							logTextBuilder.Append(logData.GetFullMessage());
							logTextBuilder.Append(Environment.NewLine);
						}

						// 外部ファイル書き出し
						if (this.IsFileOutput)
						{
							WriteLine(logTextBuilder.ToString());
							System.Diagnostics.Debug.Write(logTextBuilder.ToString());
						}
					}
				}
				catch
				{
					// エラー時は保持データをクリア
					this.ClearLog();
				}
				Thread.Sleep(100);
			}
		}

		/// <summary>
		/// ログデータの追加
		/// </summary>
		/// <param name="logData"></param>
		private void AddLog(LogData logData)
		{
			lock (lockObj)
			{
				this.logQueue.Enqueue(logData);
			}
		}

		/// <summary>
		/// ログ出力
		/// </summary>
		/// <param name="logData"></param>
		private void WriteLine(string writeText)
		{
			// ディレクトリが存在していなかったら作成する
			if (!Directory.Exists(OutputDirectory))
			{
				Directory.CreateDirectory(OutputDirectory);
			}

			// ログの書き出し
			try
			{
				File.AppendAllText(GetLogFileName(), writeText, Encoding.UTF8);
				if (LogWrited != null)
					LogWrited(LogLevel.ALL, writeText);
				//using (StreamWriter sr = new StreamWriter(GetLogFileName(), true, Encoding.UTF8))
				//{
				//	sr.WriteLine(writeText);
				//	sr.Flush();
				//}
			}
			catch
			{
				// ログの書き出しに失敗 (特に何もしない)
			}
		}

		/// <summary>
		/// ログファイル名取得
		/// </summary>
		/// <returns></returns>
		private string GetLogFileName()
		{
			return String.Format("{0}\\{1}_{2}.log", OutputDirectory, NamePrefix, DateTime.Now.ToString("yyyyMMdd"));
		}

		/// <summary>
		/// 保持ログデータのクリア
		/// </summary>
		private void ClearLog()
		{
			lock (lockObj)
			{
				// キューのクリア
				this.logQueue.Clear();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileDate"></param>
		/// <returns></returns>
		private bool CheckFileNameFormat(string fileName, out DateTime fileDate)
		{
			int yyyy, mm, dd;

			try
			{
				for (int i = 0; i < fileName.Length - 8; i++)
				{
					// 年の妥当性をチェック
					yyyy = int.Parse(fileName.Substring(i, 4));
					if (yyyy < 1980 || yyyy > 9999)
						continue;

					// 月の妥当性をチェック
					mm = int.Parse(fileName.Substring(i + 4, 2));
					if (mm < 1 || mm > 12)
						continue;

					// 日の妥当性をチェック
					dd = int.Parse(fileName.Substring(i + 6, 2));
					if (dd < 1 || dd > DateTime.DaysInMonth(yyyy, mm))
						continue;

					fileDate = new DateTime(yyyy, mm, dd);
					return true;
				}
			}
			catch (Exception)
			{
				fileDate = new DateTime();

				return false;
			}

			fileDate = new DateTime();
			return false;
		}

		/// <summary>
		/// 例外文字列取得
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		private string GetExceptionString(Exception ex)
		{
			string exStr = string.Empty;
			if (ex.InnerException != null)
			{
				exStr += GetExceptionString(ex.InnerException);
			}

			exStr += string.Format("{0} {{{1}}}{2}{3}",
								ex.Message,
								ex.GetType().ToString(),
								Environment.NewLine, ex.StackTrace);

			return exStr;
		}

		/// <summary>
		/// シリアライズ
		/// </summary>
		private static void Serialize()
		{
			try
			{
				using (var streamWriter = new StreamWriter($@"{settingFileDirectory}\{settingFileName}", false, Encoding.UTF8))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(ApplicationLog));
					serializer.Serialize(streamWriter, Instance);
					streamWriter.Flush();
				}
			}
			catch (Exception ex)
			{
				Instance.Warn(ex);
			}
		}

		/// <summary>
		/// デシリアライズ
		/// </summary>
		public static void Deserialize()
		{
			try
			{
				if (!File.Exists(settingFileName))
				{
					Instance = new ApplicationLog();
					Serialize();
					return;
				}

				var serializer = new XmlSerializer(typeof(ApplicationLog));
				var xmlSettings = new System.Xml.XmlReaderSettings()
				{
					CheckCharacters = false,
				};

				using (var streamReader = new StreamReader($@"{settingFileDirectory}\{settingFileName}", Encoding.UTF8))
				using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
				{
					Instance = (ApplicationLog)serializer.Deserialize(xmlReader);
				}
			}
			catch (Exception ex)
			{
				Instance = new ApplicationLog();
				Instance.Warn(ex);
				Serialize();
			}
		}

		#endregion

		#region IDisposable メンバー

		// Flag: Has Dispose already been called?
		bool disposed = false;
		~ApplicationLog()
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
				Thread.Sleep(200);
				loggingFlag = false;
				this.loggingThread.Join();
				//Serialize();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
		}
		#endregion
	}
}

