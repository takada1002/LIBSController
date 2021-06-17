using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpLibrary;
using MelsecInverterLib;
using CounterBoardLib;

namespace LIBSController
{
    // キースイッチ管理クラス
    class KeySwitchManager
    {
        // アプリケーションログ
        private ApplicationLog appLog = null;
        // 運転開始イベント
        public event EventHandler<EventArgs> systemStarted;
        // 運転停止イベント
        public event EventHandler<EventArgs> systemStopped;
        // カウンタボード
        private List<CounterBoard> counterBoard = null;

        // 初期化
        public Boolean initialize()
        {
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();

            counterBoard = CounterBoard.Instance;
            this.counterBoard[0].ioInterrupted += OnDioInterrupted;

            this.appLog.Info("初期化完了 ---キースイッチ管理---");

            return true;
        }

        // 汎用入力の割り込みイベント
        private void OnDioInterrupted(object sender, CounterBoardEventArgs e)
        {
            // SU_IN9 SD_IN9 のフラグが立っているか
            if ((e.EventMask & (uint)0x00000100) > 0)
            {
                // 立ち上がりで LIBS 装置運転停止
                if (systemStopped != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
                        // 運転停止イベントをコール
                        systemStopped(this, new EventArgs());
                    });
                }
            }
            else if ((e.EventMask & (uint)0x01000000) > 0)
            {
                // 立ち下がりで LIBS 装置運転開始
                if (systemStarted != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
                        // 運転開始イベントをコール
                        systemStarted(this, new EventArgs());
                    });
                }
            }
        }
    }
}
