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
    public partial class DebugView : Form
    {
        public DebugView()
        {
            InitializeComponent();
        }

        public void WriteStr(string str)
        {
            if(InvokeRequired)
            {
                Invoke(new Action(()=> WriteStr(str)));
                return;
            }
            //this.textBox1.Text += str;
            //this.textBox1.ScrollToCaret();
			this.textBox1.AppendText(str);
        }
        public void WriteStrLine(string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => WriteStrLine(str)));
                return;
            }
            //this.textBox1.Text += str + Environment.NewLine;
            //this.textBox1.ScrollToCaret();
			this.textBox1.AppendText(str + Environment.NewLine);
		}
        public void ClearStr()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ClearStr()));
                return;
            }
            this.textBox1.Text = string.Empty;
            this.textBox1.ScrollToCaret();
        }
    }
}
