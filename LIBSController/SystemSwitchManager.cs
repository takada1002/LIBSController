using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterBoardLib;

namespace LIBSController
{
    class SystemSwitchManager
    {
        // アプリケーションログ
        private ApplicationLog appLog = null;
		// システムスイッチ押下イベント
		public event EventHandler<EventArgs> systemSwitchOn;
        // システムスイッチ復帰イベント
        public event EventHandler<EventArgs> systemSwitchOff;
        // カウンタボード
        private List<CounterBoard> counterBoard = null;

        // 初期化
        public Boolean initialize()
        {
            // ログ出力クラスのインスタンス取得
            this.appLog = ApplicationLog.getInstance();

            counterBoard = CounterBoard.Instance;
            counterBoard[0].ioInterrupted += OnDioInterrupted;

            this.appLog.Info("初期化完了 ---システムスイッチ管理---");

            return true;
        }

        // 汎用入力の割り込みイベント
        private void OnDioInterrupted(object sender, CounterBoardEventArgs e)
        {
            // SU_IN13 SD_IN13 のフラグが立っているか
            if ((e.EventMask & (uint)0x00001000) > 0)
            {
                // 立ち上がりで レーザースイッチプル
                if (systemSwitchOff != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
						// レーザースイッチプルイベントをコール
						systemSwitchOff(this, new EventArgs());
                    });
                }
            }
            else if ((e.EventMask & (uint)0x10000000) > 0)
            {
                // 立ち下がりで レーザースイッチプッシュ
                if (systemSwitchOn != null)
                {
                    Task tsk = Task.Factory.StartNew(() =>
                    {
						// レーザースイッチプッシュイベントをコール
						systemSwitchOn(this, new EventArgs());
                    });
                }
            }
        }
    }
}
