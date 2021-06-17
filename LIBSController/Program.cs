using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIBSController
{
    static class Program
    {
        /// <summary>
        /// 多重起動禁止用オブジェクト
        /// </summary>
        private static Mutex globalMutex;

        // アプリケーション再起動用

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Mutex オブジェクト名 (アプリ名)
                String appName = @"Global\" + Application.ProductName;
                globalMutex = new Mutex(false, appName);
            }
            catch
            {
                return;
            }

            if(globalMutex.WaitOne(0, true))
            {
                try
                {
                    // 実行ファイルパスを取得
                    // 標準で Windows CE の work ディレクトリは 「\」 配下になるらしい
                    Assembly myAssembly = Assembly.GetExecutingAssembly();
                    String exePath = System.IO.Path.GetDirectoryName(myAssembly.GetModules()[0].FullyQualifiedName);

					// ログのインスタンスは一度ここで生成しておく
					ApplicationLog.settingFileDirectory = exePath;
					ApplicationLog.Deserialize();
					ApplicationLog log = ApplicationLog.getInstance();

                    // リリース時はここをリリースに変更する
#if DEBUG
                    log.Level = (byte)ApplicationLog.LogLevel.ALL;
#else
                    log.Level = (byte)ApplicationLog.LogLevel.Release;
#endif
                    log.OutputDirectory = exePath + "\\log";
                    log.NamePrefix = "applog";



					String filePath = exePath + "\\MaterialSetting.xml";

                    MaterialSetting materialSetting = MaterialSetting.Deserialize(filePath);

					filePath = exePath + "\\ConveyorSettings.xml";

					ConveyorSettings.Deserialize(filePath);

					filePath = exePath + "\\NozzleSettings.xml";

					NozzleSettings.Deserialize(filePath);

					Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new ControlForm());
                }
                catch (Exception ex)
                {
                    // 最上位でのエラーキャッチ
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Mutex オブジェクト開放
                    globalMutex.ReleaseMutex();
                }

            }

            globalMutex.Close();
        }
    }
}
