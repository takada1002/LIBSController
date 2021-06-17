using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIBSController
{
    // 画面下部のステータスラベル
    public partial class StatusLabel : UserControl
    {
		public event Action ResetButtonClicked;

        // 表示メッセージ
        private String labelText = "";
        public String LabelText
        {
            get { return labelText; }
            set 
            {
                SetText(value);
                labelText = value; 
            }
        }

        // LIBS ソータステータス
        private LIBSSorter.SorterStatus status;
        public LIBSSorter.SorterStatus Status
        {
            get { return status; }
            set 
            {
                switch (value)
                {
                    case LIBSSorter.SorterStatus.NOT_READY:
                        this.labelStatus.Text = "運転不可能";
                        this.BackColor = Color.Yellow;
                        break;
                    case LIBSSorter.SorterStatus.READY:
                        this.labelStatus.Text = "運転可能";
                        this.BackColor = Color.Yellow;
                        break;
					case LIBSSorter.SorterStatus.TEST_READY:
						this.labelStatus.Text = "テスト運転可能";
						this.BackColor = Color.Yellow;
						break;
					case LIBSSorter.SorterStatus.RUN:
                        this.labelStatus.Text = "運転中";
                        this.BackColor = Color.Lime;
                        break;
					case LIBSSorter.SorterStatus.TEST_RUN:
						this.labelStatus.Text = "テスト運転中";
						this.BackColor = Color.Lime;
						break;
					case LIBSSorter.SorterStatus.FORCE_RUN:
						this.labelStatus.Text = "強制運転中";
						this.BackColor = Color.Lime;
						break;
					case LIBSSorter.SorterStatus.EMERGENCY:
                        this.labelStatus.Text = "緊急停止";
                        this.BackColor = Color.Red;
                        break;
                    case LIBSSorter.SorterStatus.ERROR:
                        this.labelStatus.Text = "異常発生";
                        this.BackColor = Color.Red;
                        break;
                    case LIBSSorter.SorterStatus.UNKNOWN:
                        this.labelStatus.Text = "状態不定";
                        this.BackColor = Color.Gray;
                        break;
                }


                // 表示メッセージのクリア
                this.LabelText = String.Empty;

                status = value; 
            }
        }

        public StatusLabel()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            if(this.InvokeRequired)
            {
                Invoke(new Action(() => SetText(text)));
                return;
            }
            this.labelMessage.Text = text;
        }

		private void buttonReset_Click(object sender, EventArgs e)
		{
			if(ResetButtonClicked != null)
			{
				Task.Run(new Action(ResetButtonClicked));
			}
		}
	}
}
