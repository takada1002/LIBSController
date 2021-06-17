namespace LIBSController
{
	partial class ConveyorDebugView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbText
			// 
			this.tbText.BackColor = System.Drawing.Color.Red;
			this.tbText.Font = new System.Drawing.Font("MS UI Gothic", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tbText.Location = new System.Drawing.Point(33, 26);
			this.tbText.Name = "tbText";
			this.tbText.ReadOnly = true;
			this.tbText.Size = new System.Drawing.Size(231, 54);
			this.tbText.TabIndex = 0;
			this.tbText.Text = "停止中";
			this.tbText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// ConveyorDevugView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(308, 109);
			this.Controls.Add(this.tbText);
			this.Name = "ConveyorDevugView";
			this.Text = "ConveyorDevugView";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbText;
	}
}