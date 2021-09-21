using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LIBSController.Properties;

namespace LIBSController
{
    public partial class AirNozzleSettingForm : Form
    {

		const int NOZZLE_MAX = 3;

		const int TIME_MAX = 200;

		private NozzleSettings nozzleSettings = NozzleSettings.Instance;

		private List<int> maxMaterialLengthList = null;

		// 前噴射時間減長押しタイマー
		Timer beforeTimeBackTimer = null;

		// 前噴射時間増長押しタイマー
		Timer beforeTimeNextTimer = null;

		// 後噴射時間減長押しタイマー
		Timer afterTimeBackTimer = null;

		// 後噴射時間増長押しタイマー
		Timer afterTimeNextTimer = null;

		public AirNozzleSettingForm()
        {
            InitializeComponent();
            initializeControl();

			this.beforeTimeBackTimer = new System.Windows.Forms.Timer();
			this.beforeTimeBackTimer.Interval = 100;
			this.beforeTimeBackTimer.Tick += BeforeTimeBackTimer_Tick;

			this.beforeTimeNextTimer = new System.Windows.Forms.Timer();
			this.beforeTimeNextTimer.Interval = 100;
			this.beforeTimeNextTimer.Tick += BeforeTimeNextTimer_Tick;

			this.afterTimeBackTimer = new System.Windows.Forms.Timer();
			this.afterTimeBackTimer.Interval = 100;
			this.afterTimeBackTimer.Tick += AfterTimeBackTimer_Tick;

			this.afterTimeNextTimer = new System.Windows.Forms.Timer();
			this.afterTimeNextTimer.Interval = 100;
			this.afterTimeNextTimer.Tick += AfterTimeNextTimer_Tick;
        }

        // 表示の初期化
        private void initializeControl()
        {
			//numericUpDownBeforeTime.Value = Settings.Default.AirNozzleBeforeTime;
			//numericUpDownAfterTime.Value = Settings.Default.AirNozzleAfterTime;
			// ノズル番号表示
			this.labelNozzleNumber.Text = "1";
			UpdateData();

			this.maxMaterialLengthList = new List<int>() 
			{ 
				200,
				250,
				300,
				350,
				400,
				450,
				500,
				550,
				600
			};

			this.comboBoxMaxMaterilaLength.DataSource = this.maxMaterialLengthList;
			this.comboBoxMaxMaterilaLength.SelectedItem = Settings.Default.MaxMaterialLength;
		}

		// 前時間変更イベント
		private void numericUpDownBeforeTime_Leave(object sender, EventArgs e)
		{
			//Settings.Default.AirNozzleBeforeTime = Convert.ToInt32(numericUpDownBeforeTime.Value);
			GetNozzleSetting().BeforeInjectionTime = Convert.ToInt32(numericUpDownBeforeTime.Value);
		}

		// 後時間変更イベント
		private void numericUpDownAfterTime_Leave(object sender, EventArgs e)
		{
			//Settings.Default.AirNozzleAfterTime = Convert.ToInt32(numericUpDownAfterTime.Value);
			GetNozzleSetting().AfterInjectionTime = Convert.ToInt32(numericUpDownAfterTime.Value);
		}

		// 設定終了ボタンクリックイベント
		private void buttonClose_Click(object sender, EventArgs e)
        {
            // 変更した設定反映
            //Settings.Default.Save();
			NozzleSettings.Serialize();
			Settings.Default.MaxMaterialLength = (int)this.comboBoxMaxMaterilaLength.SelectedItem;
			Settings.Default.Save();
            this.Close();
        }

		// ノズル前ボタンクリックイベント
		private void buttonNozzleBack_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(labelNozzleNumber.Text);
			if (num > 1)
			{
				labelNozzleNumber.Text = (num - 1).ToString();
			}
		}

		// ノズル次ボタンクリックイベント
		private void buttonNozzleNext_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(labelNozzleNumber.Text);
			if (num < NOZZLE_MAX)
			{
				labelNozzleNumber.Text = (num + 1).ToString();
			}
		}

		// ノズル番号変更イベント
		private void labelNozzleNumber_TextChanged(object sender, EventArgs e)
		{
			UpdateData();
		}

		// 表示更新
		private void UpdateData()
		{
			var nozzleSetting = GetNozzleSetting();
		}

		// 現在ノズル番号のノズル設定を取得
		private NozzleSettings.NozzleSetting GetNozzleSetting()
		{
			return nozzleSettings.DataList[Convert.ToInt32(labelNozzleNumber.Text) - 1];
		}

		private void buttonBeforeTimeBack_MouseDown(object sender, MouseEventArgs e)
		{
			int num = Convert.ToInt32(textBoxBeforeTime.Text);
			if (num > 1)
			{
				textBoxBeforeTime.Text = (num - 1).ToString();
			}
			beforeTimeBackTimer.Start();
		}

		private void buttonBeforeTimeNext_MouseDown(object sender, MouseEventArgs e)
		{
			int num = Convert.ToInt32(textBoxBeforeTime.Text);
			if (num < TIME_MAX)
			{
				textBoxBeforeTime.Text = (num + 1).ToString();
			}
			beforeTimeNextTimer.Start();
		}

		private void buttonAfterTimeBack_MouseDown(object sender, MouseEventArgs e)
		{
			int num = Convert.ToInt32(textBoxAfterTime.Text);
			if (num > 1)
			{
				textBoxAfterTime.Text = (num - 1).ToString();
			}
			afterTimeBackTimer.Start();
		}

		private void buttonAfterTimeNext_MouseDown(object sender, MouseEventArgs e)
		{
			int num = Convert.ToInt32(textBoxAfterTime.Text);
			if (num < TIME_MAX)
			{
				textBoxAfterTime.Text = (num + 1).ToString();
			}
			afterTimeNextTimer.Start();
		}

		private void buttonBeforeTimeBack_MouseUp(object sender, MouseEventArgs e)
		{
			beforeTimeBackTimer.Stop();
		}

		private void buttonBeforeTimeNext_MouseUp(object sender, MouseEventArgs e)
		{
			beforeTimeNextTimer.Stop();
		}

		private void buttonAfterTimeBack_MouseUp(object sender, MouseEventArgs e)
		{
			afterTimeBackTimer.Stop();
		}

		private void buttonAfterTimeNext_MouseUp(object sender, MouseEventArgs e)
		{
			afterTimeNextTimer.Stop();
		}

		private void BeforeTimeBackTimer_Tick(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(textBoxBeforeTime.Text);
			if (num > 1)
			{
				textBoxBeforeTime.Text = (num - 1).ToString();
			}
		}

		private void BeforeTimeNextTimer_Tick(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(textBoxBeforeTime.Text);
			if (num < TIME_MAX)
			{
				textBoxBeforeTime.Text = (num + 1).ToString();
			}
		}

		private void AfterTimeBackTimer_Tick(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(textBoxAfterTime.Text);
			if (num > 1)
			{
				textBoxAfterTime.Text = (num - 1).ToString();
			}
		}

		private void AfterTimeNextTimer_Tick(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(textBoxAfterTime.Text);
			if (num < TIME_MAX)
			{
				textBoxAfterTime.Text = (num + 1).ToString();
			}
		}

		private void textBoxBeforeTime_TextChanged(object sender, EventArgs e)
		{
			if(int.TryParse(textBoxBeforeTime.Text,out var num) && num >= 0 && num <= TIME_MAX)
			{
				GetNozzleSetting().BeforeInjectionTime = num;
			}
			else
			{
				textBoxBeforeTime.Text = GetNozzleSetting().BeforeInjectionTime.ToString();
			}
			
		}

		private void textBoxAfterTime_TextChanged(object sender, EventArgs e)
		{
			if(int.TryParse(textBoxAfterTime.Text,out var num) && num >= 0 && num <= TIME_MAX)
			{
				GetNozzleSetting().AfterInjectionTime = num;
			}
			else
			{
				textBoxAfterTime.Text = GetNozzleSetting().AfterInjectionTime.ToString();
			}
		}
	}
}
