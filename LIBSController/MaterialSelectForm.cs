using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIBSController
{
    public partial class MaterialSelectForm : Form
    {
        private MaterialSetting materialSetting = null;

		private int topIndex = 0;

		const int NOZZLE_MAX = 3;

		public class RowData
		{
			public MaterialClass Class;
			public Label No;
			public Label Name;
			public Label Nozzle;
			public Label Nozzle2;
			public void Update()
			{
				if(Class != null)
				{
					Name.Text = Class.className;
					Nozzle.Text = Class.nozzleNumber == 0 ? "無" : Class.nozzleNumber.ToString();
					Nozzle2.Text = Class.nozzleNumber2 == 0 ? "無" : Class.nozzleNumber2.ToString();
				}
				else
				{
					Name.Text = string.Empty;
					Nozzle.Text = string.Empty;
					Nozzle2.Text = string.Empty;
				}
			}
		}

		private List<RowData> Rows = new List<RowData>();

        public MaterialSelectForm()
        {
            // 素材選別設定のインスタンス取得
            this.materialSetting = MaterialSetting.getInstance();
			
            InitializeComponent();
            initializeControl();
        }

        // 表示の初期化
        private void initializeControl()
        {
			Rows.Add(new RowData() { No = this.labelNo1, Name = this.labelName1, Nozzle = this.labelNozzle1, Nozzle2 = this.labelNozzle1_2 });
			Rows.Add(new RowData() { No = this.labelNo2, Name = this.labelName2, Nozzle = this.labelNozzle2, Nozzle2 = this.labelNozzle2_2 });
			Rows.Add(new RowData() { No = this.labelNo3, Name = this.labelName3, Nozzle = this.labelNozzle3, Nozzle2 = this.labelNozzle3_2 });
			Rows.Add(new RowData() { No = this.labelNo4, Name = this.labelName4, Nozzle = this.labelNozzle4, Nozzle2 = this.labelNozzle4_2 });
			Rows.Add(new RowData() { No = this.labelNo5, Name = this.labelName5, Nozzle = this.labelNozzle5, Nozzle2 = this.labelNozzle5_2 });
			Rows.Add(new RowData() { No = this.labelNo6, Name = this.labelName6, Nozzle = this.labelNozzle6, Nozzle2 = this.labelNozzle6_2 });
			Rows.Add(new RowData() { No = this.labelNo7, Name = this.labelName7, Nozzle = this.labelNozzle7, Nozzle2 = this.labelNozzle7_2 });
			Rows.Add(new RowData() { No = this.labelNo8, Name = this.labelName8, Nozzle = this.labelNozzle8, Nozzle2 = this.labelNozzle8_2 });

			updatePage();
		}

		private void updatePage()
		{
			for(int i = 0 ; i < Rows.Count; i ++)
			{
				Rows[i].No.Text = (topIndex + i + 1).ToString();
				if(this.materialSetting.materialClassList.Count > topIndex + i)
				{
					Rows[i].Class = this.materialSetting.materialClassList[topIndex + i];
				}
				else
				{
					Rows[i].Class = null;
				}
				Rows[i].Update();
			}
		}

		// 
		public void saveMaterialSetting()
        {
            // シリアライズして設定ファイルに書き出す
            this.materialSetting.Serialize();
        }

        // 
        private void OnClickButtonClose(object sender, EventArgs e)
        {
            this.Close();
        }

		private void MaterialSelectForm_Load(object sender, EventArgs e)
		{
			
		}

		private void buttonNozzleBack_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			int rowIndex = Convert.ToInt32(button.Tag) - 1;
			var row = Rows[rowIndex];
			if (row.Class != null)
			{
				if(row.Class.nozzleNumber > 0)
				{
					row.Class.nozzleNumber--;
					row.Update();
				}
			}
		}

		private void buttonNozzleNext_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			int rowIndex = Convert.ToInt32(button.Tag) - 1;
			var row = Rows[rowIndex];
			if (row.Class != null)
			{
				if(row.Class.nozzleNumber < NOZZLE_MAX)
				{
					row.Class.nozzleNumber++;
					row.Update();
				}
			}
		}

		private void buttonNozzleBack_2_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			int rowIndex = Convert.ToInt32(button.Tag) - 1;
			var row = Rows[rowIndex];
			if (row.Class != null)
			{
				if(row.Class.nozzleNumber2 > 0)
				{
					row.Class.nozzleNumber2--;
					row.Update();
				}
			}
		}

		private void buttonNozzleNext_2_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			int rowIndex = Convert.ToInt32(button.Tag) - 1;
			var row = Rows[rowIndex];
			if (row.Class != null)
			{
				if(row.Class.nozzleNumber2 < NOZZLE_MAX)
				{
					row.Class.nozzleNumber2++;
					row.Update();
				}
			}
		}

		private void buttonPageBack_Click(object sender, EventArgs e)
		{
			if(topIndex > 0)
			{
				topIndex --;
				updatePage();
			}
		}

		private void buttonPageNext_Click(object sender, EventArgs e)
		{
			if(topIndex < this.materialSetting.materialClassList.Count - 8)
			{
				topIndex++;
				updatePage();
			}
		}
	}
}
