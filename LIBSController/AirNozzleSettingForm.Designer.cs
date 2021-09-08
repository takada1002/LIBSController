namespace LIBSController
{
    partial class AirNozzleSettingForm
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
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelOnTimeSetting = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelNozzleNumber = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.buttonNozzleNext = new System.Windows.Forms.Button();
			this.buttonNozzleBack = new System.Windows.Forms.Button();
			this.comboBoxMaxMaterilaLength = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.buttonBeforeTimeBack = new System.Windows.Forms.Button();
			this.buttonAfterTimeBack = new System.Windows.Forms.Button();
			this.buttonBeforeTimeNext = new System.Windows.Forms.Button();
			this.buttonAfterTimeNext = new System.Windows.Forms.Button();
			this.textBoxBeforeTime = new System.Windows.Forms.TextBox();
			this.textBoxAfterTime = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// labelTitle
			// 
			this.labelTitle.BackColor = System.Drawing.Color.Yellow;
			this.labelTitle.Font = new System.Drawing.Font("MS UI Gothic", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelTitle.Location = new System.Drawing.Point(0, 0);
			this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(1024, 40);
			this.labelTitle.TabIndex = 39;
			this.labelTitle.Text = "エアーノズル動作設定";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelOnTimeSetting
			// 
			this.labelOnTimeSetting.AutoSize = true;
			this.labelOnTimeSetting.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelOnTimeSetting.Location = new System.Drawing.Point(47, 75);
			this.labelOnTimeSetting.Name = "labelOnTimeSetting";
			this.labelOnTimeSetting.Size = new System.Drawing.Size(207, 33);
			this.labelOnTimeSetting.TabIndex = 40;
			this.labelOnTimeSetting.Text = "動作時間補正";
			// 
			// buttonClose
			// 
			this.buttonClose.BackColor = System.Drawing.Color.White;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonClose.Font = new System.Drawing.Font("MS UI Gothic", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonClose.Location = new System.Drawing.Point(53, 642);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(922, 77);
			this.buttonClose.TabIndex = 3;
			this.buttonClose.Text = "設定終了";
			this.buttonClose.UseVisualStyleBackColor = false;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(386, 213);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(175, 33);
			this.label1.TabIndex = 40;
			this.label1.Text = "前噴射時間";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(386, 350);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(175, 33);
			this.label2.TabIndex = 40;
			this.label2.Text = "後噴射時間";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(668, 160);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(184, 33);
			this.label4.TabIndex = 40;
			this.label4.Text = "(0～200 mS)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label5.Location = new System.Drawing.Point(667, 298);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(184, 33);
			this.label5.TabIndex = 40;
			this.label5.Text = "(0～200 mS)";
			// 
			// labelNozzleNumber
			// 
			this.labelNozzleNumber.BackColor = System.Drawing.Color.White;
			this.labelNozzleNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelNozzleNumber.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelNozzleNumber.Location = new System.Drawing.Point(159, 193);
			this.labelNozzleNumber.Name = "labelNozzleNumber";
			this.labelNozzleNumber.Size = new System.Drawing.Size(106, 72);
			this.labelNozzleNumber.TabIndex = 42;
			this.labelNozzleNumber.Text = "1";
			this.labelNozzleNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelNozzleNumber.TextChanged += new System.EventHandler(this.labelNozzleNumber_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label6.Location = new System.Drawing.Point(167, 167);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(97, 19);
			this.label6.TabIndex = 44;
			this.label6.Text = "ノズル番号";
			// 
			// buttonNozzleNext
			// 
			this.buttonNozzleNext.BackColor = System.Drawing.Color.White;
			this.buttonNozzleNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonNozzleNext.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonNozzleNext.Location = new System.Drawing.Point(271, 193);
			this.buttonNozzleNext.Name = "buttonNozzleNext";
			this.buttonNozzleNext.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonNozzleNext.Size = new System.Drawing.Size(73, 72);
			this.buttonNozzleNext.TabIndex = 43;
			this.buttonNozzleNext.Text = "▶";
			this.buttonNozzleNext.UseVisualStyleBackColor = false;
			this.buttonNozzleNext.Click += new System.EventHandler(this.buttonNozzleNext_Click);
			// 
			// buttonNozzleBack
			// 
			this.buttonNozzleBack.BackColor = System.Drawing.Color.White;
			this.buttonNozzleBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonNozzleBack.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonNozzleBack.Location = new System.Drawing.Point(80, 193);
			this.buttonNozzleBack.Name = "buttonNozzleBack";
			this.buttonNozzleBack.Size = new System.Drawing.Size(73, 72);
			this.buttonNozzleBack.TabIndex = 41;
			this.buttonNozzleBack.Text = "◀";
			this.buttonNozzleBack.UseVisualStyleBackColor = false;
			this.buttonNozzleBack.Click += new System.EventHandler(this.buttonNozzleBack_Click);
			// 
			// comboBoxMaxMaterilaLength
			// 
			this.comboBoxMaxMaterilaLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxMaxMaterilaLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboBoxMaxMaterilaLength.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboBoxMaxMaterilaLength.FormattingEnabled = true;
			this.comboBoxMaxMaterilaLength.Location = new System.Drawing.Point(161, 500);
			this.comboBoxMaxMaterilaLength.Margin = new System.Windows.Forms.Padding(100, 3, 3, 3);
			this.comboBoxMaxMaterilaLength.Name = "comboBoxMaxMaterilaLength";
			this.comboBoxMaxMaterilaLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.comboBoxMaxMaterilaLength.Size = new System.Drawing.Size(178, 72);
			this.comboBoxMaxMaterilaLength.TabIndex = 48;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label8.Location = new System.Drawing.Point(373, 530);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(63, 33);
			this.label8.TabIndex = 46;
			this.label8.Text = "mm";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label7.Location = new System.Drawing.Point(47, 430);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(175, 33);
			this.label7.TabIndex = 47;
			this.label7.Text = "素材長制限";
			// 
			// buttonBeforeTimeBack
			// 
			this.buttonBeforeTimeBack.BackColor = System.Drawing.Color.White;
			this.buttonBeforeTimeBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonBeforeTimeBack.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonBeforeTimeBack.Location = new System.Drawing.Point(567, 193);
			this.buttonBeforeTimeBack.Name = "buttonBeforeTimeBack";
			this.buttonBeforeTimeBack.Size = new System.Drawing.Size(85, 72);
			this.buttonBeforeTimeBack.TabIndex = 49;
			this.buttonBeforeTimeBack.Text = "◀";
			this.buttonBeforeTimeBack.UseVisualStyleBackColor = false;
			this.buttonBeforeTimeBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonBeforeTimeBack_MouseDown);
			this.buttonBeforeTimeBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonBeforeTimeBack_MouseUp);
			// 
			// buttonAfterTimeBack
			// 
			this.buttonAfterTimeBack.BackColor = System.Drawing.Color.White;
			this.buttonAfterTimeBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonAfterTimeBack.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonAfterTimeBack.Location = new System.Drawing.Point(567, 331);
			this.buttonAfterTimeBack.Name = "buttonAfterTimeBack";
			this.buttonAfterTimeBack.Size = new System.Drawing.Size(85, 72);
			this.buttonAfterTimeBack.TabIndex = 49;
			this.buttonAfterTimeBack.Text = "◀";
			this.buttonAfterTimeBack.UseVisualStyleBackColor = false;
			this.buttonAfterTimeBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAfterTimeBack_MouseDown);
			this.buttonAfterTimeBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAfterTimeBack_MouseUp);
			// 
			// buttonBeforeTimeNext
			// 
			this.buttonBeforeTimeNext.BackColor = System.Drawing.Color.White;
			this.buttonBeforeTimeNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonBeforeTimeNext.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonBeforeTimeNext.Location = new System.Drawing.Point(860, 193);
			this.buttonBeforeTimeNext.Name = "buttonBeforeTimeNext";
			this.buttonBeforeTimeNext.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonBeforeTimeNext.Size = new System.Drawing.Size(85, 72);
			this.buttonBeforeTimeNext.TabIndex = 50;
			this.buttonBeforeTimeNext.Text = "▶";
			this.buttonBeforeTimeNext.UseVisualStyleBackColor = false;
			this.buttonBeforeTimeNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonBeforeTimeNext_MouseDown);
			this.buttonBeforeTimeNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonBeforeTimeNext_MouseUp);
			// 
			// buttonAfterTimeNext
			// 
			this.buttonAfterTimeNext.BackColor = System.Drawing.Color.White;
			this.buttonAfterTimeNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonAfterTimeNext.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonAfterTimeNext.Location = new System.Drawing.Point(860, 331);
			this.buttonAfterTimeNext.Name = "buttonAfterTimeNext";
			this.buttonAfterTimeNext.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonAfterTimeNext.Size = new System.Drawing.Size(85, 72);
			this.buttonAfterTimeNext.TabIndex = 50;
			this.buttonAfterTimeNext.Text = "▶";
			this.buttonAfterTimeNext.UseVisualStyleBackColor = false;
			this.buttonAfterTimeNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAfterTimeNext_MouseDown);
			this.buttonAfterTimeNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAfterTimeNext_MouseUp);
			// 
			// textBoxBeforeTime
			// 
			this.textBoxBeforeTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxBeforeTime.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textBoxBeforeTime.Location = new System.Drawing.Point(662, 193);
			this.textBoxBeforeTime.Name = "textBoxBeforeTime";
			this.textBoxBeforeTime.Size = new System.Drawing.Size(189, 71);
			this.textBoxBeforeTime.TabIndex = 51;
			this.textBoxBeforeTime.Text = "1";
			this.textBoxBeforeTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxBeforeTime.TextChanged += new System.EventHandler(this.textBoxBeforeTime_TextChanged);
			// 
			// textBoxAfterTime
			// 
			this.textBoxAfterTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxAfterTime.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textBoxAfterTime.Location = new System.Drawing.Point(662, 331);
			this.textBoxAfterTime.Name = "textBoxAfterTime";
			this.textBoxAfterTime.Size = new System.Drawing.Size(189, 71);
			this.textBoxAfterTime.TabIndex = 51;
			this.textBoxAfterTime.Text = "1";
			this.textBoxAfterTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxAfterTime.TextChanged += new System.EventHandler(this.textBoxAfterTime_TextChanged);
			// 
			// AirNozzleSettingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.textBoxAfterTime);
			this.Controls.Add(this.textBoxBeforeTime);
			this.Controls.Add(this.buttonAfterTimeNext);
			this.Controls.Add(this.buttonBeforeTimeNext);
			this.Controls.Add(this.buttonAfterTimeBack);
			this.Controls.Add(this.buttonBeforeTimeBack);
			this.Controls.Add(this.comboBoxMaxMaterilaLength);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.labelNozzleNumber);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.buttonNozzleNext);
			this.Controls.Add(this.buttonNozzleBack);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelOnTimeSetting);
			this.Controls.Add(this.labelTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AirNozzleSettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AirNozzleSettingForm";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelOnTimeSetting;
        private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelNozzleNumber;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button buttonNozzleNext;
		private System.Windows.Forms.Button buttonNozzleBack;
		private System.Windows.Forms.ComboBox comboBoxMaxMaterilaLength;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button buttonBeforeTimeBack;
		private System.Windows.Forms.Button buttonAfterTimeBack;
		private System.Windows.Forms.Button buttonBeforeTimeNext;
		private System.Windows.Forms.Button buttonAfterTimeNext;
		private System.Windows.Forms.TextBox textBoxBeforeTime;
		private System.Windows.Forms.TextBox textBoxAfterTime;
	}
}