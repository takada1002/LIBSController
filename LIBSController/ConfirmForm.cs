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
    public partial class ConfirmForm : Form
    {
        // コンストラクタ
        public ConfirmForm(string title , string message)
        {
            InitializeComponent();

            this.lblTitle.Text = title;
            this.lblMessage.Text = message;
        }

        // OKボタンクリック
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // キャンセルボタンクリック
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
