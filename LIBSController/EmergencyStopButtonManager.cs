using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterBoardLib;

namespace LIBSController
{
    class EmergencyStopButtonManager
    {
        // アプリケーションログ
        private ApplicationLog appLog = null;
        // 緊急停止イベント
        public event EventHandler<EventArgs> emergencyStopped;
        // 緊急停止解除イベント
        public event EventHandler<EventArgs> emergencyCancelled;
        // カウンタボード
        private List<CounterBoard> counterBoard = null;

        // 初期化
        public Boolean initialize()
        {
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();

            counterBoard = CounterBoard.Instance;
            counterBoard[0].ioInterrupted += OnDioInterrupted;

            this.appLog.Info("初期化完了 ---非常停止スイッチ管理---");

            return true;
        }

        // 汎用入力の割り込みイベント
        private void OnDioInterrupted(object sender, CounterBoardEventArgs e)
        {
            // SU_IN15 SD_IN15 のフラグが立っているか
            if ((e.EventMask & (uint)0x00004000) > 0)
            {
                // 立ち上がりで LIBS 非常停止
                if (emergencyStopped != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
                        //// 非常停止イベントをコール
                        //emergencyStopped(this, new EventArgs());
                        // 非常停止解除イベントをコール
                        emergencyCancelled(this, new EventArgs());
                    });
                }
            }
            else if ((e.EventMask & (uint)0x40000000) > 0)
            {
                // 立ち下がりで LIBS 非常停止解除
                if (emergencyCancelled != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
                        //// 非常停止解除イベントをコール
                        //emergencyCancelled(this, new EventArgs());
                        // 非常停止イベントをコール
                        emergencyStopped(this, new EventArgs());
                    });
                }
            }
        }
    }
}
