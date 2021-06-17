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
			this.numericUpDownBeforeTime = new System.Windows.Forms.NumericUpDown();
			this.labelOnTimeSetting = new System.Windows.Forms.Label();
			this.labelOnTimeUnit = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDownAfterTime = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelNozzleNumber = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.buttonNozzleNext = new System.Windows.Forms.Button();
			this.buttonNozzleBack = new System.Windows.Forms.Button();
			this.comboBoxMaxMaterilaLength = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBeforeTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAfterTime)).BeginInit();
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
			// numericUpDownBeforeTime
			// 
			this.numericUpDownBeforeTime.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericUpDownBeforeTime.Location = new System.Drawing.Point(664, 193);
			this.numericUpDownBeforeTime.Margin = new System.Windows.Forms.Padding(0);
			this.numericUpDownBeforeTime.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDownBeforeTime.Name = "numericUpDownBeforeTime";
			this.numericUpDownBeforeTime.Size = new System.Drawing.Size(188, 71);
			this.numericUpDownBeforeTime.TabIndex = 1;
			this.numericUpDownBeforeTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownBeforeTime.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numericUpDownBeforeTime.Leave += new System.EventHandler(this.numericUpDownBeforeTime_Leave);
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
			// labelOnTimeUnit
			// 
			this.labelOnTimeUnit.AutoSize = true;
			this.labelOnTimeUnit.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelOnTimeUnit.Location = new System.Drawing.Point(864, 222);
			this.labelOnTimeUnit.Name = "labelOnTimeUnit";
			this.labelOnTimeUnit.Size = new System.Drawing.Size(58, 33);
			this.labelOnTimeUnit.TabIndex = 40;
			this.labelOnTimeUnit.Text = "mS";
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
			this.label1.Location = new System.Drawing.Point(430, 213);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(175, 33);
			this.label1.TabIndex = 40;
			this.label1.Text = "前噴射時間";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(430, 351);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(175, 33);
			this.label2.TabIndex = 40;
			this.label2.Text = "後噴射時間";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(864, 360);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 33);
			this.label3.TabIndex = 40;
			this.label3.Text = "mS";
			// 
			// numericUpDownAfterTime
			// 
			this.numericUpDownAfterTime.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericUpDownAfterTime.Location = new System.Drawing.Point(664, 331);
			this.numericUpDownAfterTime.Margin = new System.Windows.Forms.Padding(0);
			this.numericUpDownAfterTime.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDownAfterTime.Name = "numericUpDownAfterTime";
			this.numericUpDownAfterTime.Size = new System.Drawing.Size(188, 71);
			this.numericUpDownAfterTime.TabIndex = 2;
			this.numericUpDownAfterTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownAfterTime.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numericUpDownAfterTime.Leave += new System.EventHandler(this.numericUpDownAfterTime_Leave);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(694, 160);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 33);
			this.label4.TabIndex = 40;
			this.label4.Text = "(0～200)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label5.Location = new System.Drawing.Point(694, 298);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(131, 33);
			this.label5.TabIndex = 40;
			this.label5.Text = "(0～200)";
			// 
			// labelNozzleNumber
			// 
			this.labelNozzleNumber.BackColor = System.Drawing.Color.White;
			this.labelNozzleNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelNozzleNumber.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelNozzleNumber.Location = new System.Drawing.Point(161, 191);
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
			this.buttonNozzleNext.Location = new System.Drawing.Point(273, 191);
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
			this.buttonNozzleBack.Location = new System.Drawing.Point(82, 191);
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
			// AirNozzleSettingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.comboBoxMaxMaterilaLength);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.labelNozzleNumber);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.buttonNozzleNext);
			this.Controls.Add(this.buttonNozzleBack);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.numericUpDownAfterTime);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.numericUpDownBeforeTime);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelOnTimeUnit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelOnTimeSetting);
			this.Controls.Add(this.labelTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AirNozzleSettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AirNozzleSettingForm";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBeforeTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAfterTime)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.NumericUpDown numericUpDownBeforeTime;
        private System.Windows.Forms.Label labelOnTimeSetting;
        private System.Windows.Forms.Label labelOnTimeUnit;
        private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDownAfterTime;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelNozzleNumber;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button buttonNozzleNext;
		private System.Windows.Forms.Button buttonNozzleBack;
		private System.Windows.Forms.ComboBox comboBoxMaxMaterilaLength;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
	}
}