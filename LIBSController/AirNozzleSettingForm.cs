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

		const int NOZZLE_MAX = 5;

		private NozzleSettings nozzleSettings = NozzleSettings.Instance;

		private List<int> maxMaterialLengthList = null;

		public AirNozzleSettingForm()
        {
            InitializeComponent();
            initializeControl();
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
			numericUpDownBeforeTime.Value = nozzleSetting.BeforeInjectionTime;
			numericUpDownAfterTime.Value = nozzleSetting.AfterInjectionTime;
		}

		// 現在ノズル番号のノズル設定を取得
		private NozzleSettings.NozzleSetting GetNozzleSetting()
		{
			return nozzleSettings.DataList[Convert.ToInt32(labelNozzleNumber.Text) - 1];
		}
	}
}
