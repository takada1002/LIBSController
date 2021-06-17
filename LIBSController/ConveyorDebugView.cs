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
	public partial class ConveyorDebugView : Form
	{
		public static ConveyorDebugView Instance = new ConveyorDebugView();

		public String StatusText
		{
			get
			{
				return this.tbText.Text;
			}
			set
			{
				this.tbText.Text = value;
			}
		}

		public Color StatusColor
		{
			get
			{
				return this.tbText.BackColor;
			}
			set
			{
				this.tbText.BackColor = value;
			}
		}


		public ConveyorDebugView()
		{
			InitializeComponent();
		}
	}
}
