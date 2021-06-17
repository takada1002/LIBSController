namespace LIBSEmulator
{
    partial class LIBSEmulator
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.buttonSendStatusInformation = new System.Windows.Forms.Button();
			this.buttonSendData = new System.Windows.Forms.Button();
			this.textBoxLog = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxAnalysisOK = new System.Windows.Forms.CheckBox();
			this.checkBoxCounterBoard = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.labelGroup = new System.Windows.Forms.Label();
			this.textBoxMaterialLengthCount = new System.Windows.Forms.TextBox();
			this.textBoxGroup = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.labelCounter = new System.Windows.Forms.Label();
			this.textBoxCounterEnd = new System.Windows.Forms.TextBox();
			this.textBoxCounterStart = new System.Windows.Forms.TextBox();
			this.textBoxCounter = new System.Windows.Forms.TextBox();
			this.buttonSendClassInformation = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textBoxClassInformation = new System.Windows.Forms.TextBox();
			this.checkBoxIsReady = new System.Windows.Forms.CheckBox();
			this.checkBoxContinuousSend = new System.Windows.Forms.CheckBox();
			this.textBoxGroup5 = new System.Windows.Forms.TextBox();
			this.textBoxGroup3 = new System.Windows.Forms.TextBox();
			this.textBoxGroup2 = new System.Windows.Forms.TextBox();
			this.textBoxGroup1 = new System.Windows.Forms.TextBox();
			this.textBoxGroup4 = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonSendStatusInformation
			// 
			this.buttonSendStatusInformation.BackColor = System.Drawing.Color.Yellow;
			this.buttonSendStatusInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSendStatusInformation.Location = new System.Drawing.Point(149, 12);
			this.buttonSendStatusInformation.Name = "buttonSendStatusInformation";
			this.buttonSendStatusInformation.Size = new System.Drawing.Size(123, 40);
			this.buttonSendStatusInformation.TabIndex = 0;
			this.buttonSendStatusInformation.Text = "ステータス情報送信 ON";
			this.buttonSendStatusInformation.UseVisualStyleBackColor = false;
			this.buttonSendStatusInformation.Click += new System.EventHandler(this.OnClickButtonSendStatus);
			// 
			// buttonSendData
			// 
			this.buttonSendData.BackColor = System.Drawing.Color.White;
			this.buttonSendData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSendData.Location = new System.Drawing.Point(137, 197);
			this.buttonSendData.Name = "buttonSendData";
			this.buttonSendData.Size = new System.Drawing.Size(117, 40);
			this.buttonSendData.TabIndex = 1;
			this.buttonSendData.Text = "分析結果送信";
			this.buttonSendData.UseVisualStyleBackColor = false;
			this.buttonSendData.Click += new System.EventHandler(this.OnClickButtonSendData);
			// 
			// textBoxLog
			// 
			this.textBoxLog.Location = new System.Drawing.Point(12, 543);
			this.textBoxLog.Multiline = true;
			this.textBoxLog.Name = "textBoxLog";
			this.textBoxLog.Size = new System.Drawing.Size(260, 72);
			this.textBoxLog.TabIndex = 3;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBoxAnalysisOK);
			this.groupBox1.Controls.Add(this.checkBoxContinuousSend);
			this.groupBox1.Controls.Add(this.checkBoxCounterBoard);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.labelGroup);
			this.groupBox1.Controls.Add(this.textBoxMaterialLengthCount);
			this.groupBox1.Controls.Add(this.textBoxGroup1);
			this.groupBox1.Controls.Add(this.textBoxGroup2);
			this.groupBox1.Controls.Add(this.textBoxGroup3);
			this.groupBox1.Controls.Add(this.textBoxGroup4);
			this.groupBox1.Controls.Add(this.textBoxGroup5);
			this.groupBox1.Controls.Add(this.textBoxGroup);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.labelCounter);
			this.groupBox1.Controls.Add(this.textBoxCounterEnd);
			this.groupBox1.Controls.Add(this.textBoxCounterStart);
			this.groupBox1.Controls.Add(this.textBoxCounter);
			this.groupBox1.Controls.Add(this.buttonSendData);
			this.groupBox1.Location = new System.Drawing.Point(12, 58);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(260, 336);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "分析結果";
			// 
			// checkBoxAnalysisOK
			// 
			this.checkBoxAnalysisOK.AutoSize = true;
			this.checkBoxAnalysisOK.Checked = true;
			this.checkBoxAnalysisOK.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAnalysisOK.Location = new System.Drawing.Point(191, 18);
			this.checkBoxAnalysisOK.Name = "checkBoxAnalysisOK";
			this.checkBoxAnalysisOK.Size = new System.Drawing.Size(63, 16);
			this.checkBoxAnalysisOK.TabIndex = 18;
			this.checkBoxAnalysisOK.Text = "分析OK";
			this.checkBoxAnalysisOK.UseVisualStyleBackColor = true;
			// 
			// checkBoxCounterBoard
			// 
			this.checkBoxCounterBoard.AutoSize = true;
			this.checkBoxCounterBoard.Checked = true;
			this.checkBoxCounterBoard.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCounterBoard.Location = new System.Drawing.Point(111, 40);
			this.checkBoxCounterBoard.Name = "checkBoxCounterBoard";
			this.checkBoxCounterBoard.Size = new System.Drawing.Size(143, 16);
			this.checkBoxCounterBoard.TabIndex = 17;
			this.checkBoxCounterBoard.Text = "カウンタボードの値を利用";
			this.checkBoxCounterBoard.UseVisualStyleBackColor = true;
			this.checkBoxCounterBoard.CheckedChanged += new System.EventHandler(this.OnCheckedChangedCounterBoard);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.Location = new System.Drawing.Point(6, 165);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 19);
			this.label3.TabIndex = 16;
			this.label3.Text = "素材カウント：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelGroup
			// 
			this.labelGroup.BackColor = System.Drawing.SystemColors.Control;
			this.labelGroup.Location = new System.Drawing.Point(6, 140);
			this.labelGroup.Name = "labelGroup";
			this.labelGroup.Size = new System.Drawing.Size(71, 19);
			this.labelGroup.TabIndex = 16;
			this.labelGroup.Text = "グループ：";
			this.labelGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxMaterialLengthCount
			// 
			this.textBoxMaterialLengthCount.Location = new System.Drawing.Point(83, 165);
			this.textBoxMaterialLengthCount.Name = "textBoxMaterialLengthCount";
			this.textBoxMaterialLengthCount.Size = new System.Drawing.Size(171, 19);
			this.textBoxMaterialLengthCount.TabIndex = 15;
			this.textBoxMaterialLengthCount.Text = "10";
			// 
			// textBoxGroup
			// 
			this.textBoxGroup.Location = new System.Drawing.Point(83, 140);
			this.textBoxGroup.Name = "textBoxGroup";
			this.textBoxGroup.Size = new System.Drawing.Size(171, 19);
			this.textBoxGroup.TabIndex = 15;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.label2.Location = new System.Drawing.Point(6, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 19);
			this.label2.TabIndex = 14;
			this.label2.Text = "終了カウンタ：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.Location = new System.Drawing.Point(6, 87);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 19);
			this.label1.TabIndex = 14;
			this.label1.Text = "開始カウンタ：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCounter
			// 
			this.labelCounter.BackColor = System.Drawing.SystemColors.Control;
			this.labelCounter.Location = new System.Drawing.Point(6, 62);
			this.labelCounter.Name = "labelCounter";
			this.labelCounter.Size = new System.Drawing.Size(71, 19);
			this.labelCounter.TabIndex = 14;
			this.labelCounter.Text = "カウンタ値：";
			this.labelCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxCounterEnd
			// 
			this.textBoxCounterEnd.Location = new System.Drawing.Point(83, 112);
			this.textBoxCounterEnd.Name = "textBoxCounterEnd";
			this.textBoxCounterEnd.Size = new System.Drawing.Size(171, 19);
			this.textBoxCounterEnd.TabIndex = 13;
			// 
			// textBoxCounterStart
			// 
			this.textBoxCounterStart.Location = new System.Drawing.Point(83, 87);
			this.textBoxCounterStart.Name = "textBoxCounterStart";
			this.textBoxCounterStart.Size = new System.Drawing.Size(171, 19);
			this.textBoxCounterStart.TabIndex = 13;
			// 
			// textBoxCounter
			// 
			this.textBoxCounter.Location = new System.Drawing.Point(83, 62);
			this.textBoxCounter.Name = "textBoxCounter";
			this.textBoxCounter.Size = new System.Drawing.Size(171, 19);
			this.textBoxCounter.TabIndex = 13;
			// 
			// buttonSendClassInformation
			// 
			this.buttonSendClassInformation.BackColor = System.Drawing.Color.White;
			this.buttonSendClassInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSendClassInformation.Location = new System.Drawing.Point(131, 100);
			this.buttonSendClassInformation.Name = "buttonSendClassInformation";
			this.buttonSendClassInformation.Size = new System.Drawing.Size(123, 27);
			this.buttonSendClassInformation.TabIndex = 5;
			this.buttonSendClassInformation.Text = "クラス情報送信";
			this.buttonSendClassInformation.UseVisualStyleBackColor = false;
			this.buttonSendClassInformation.Click += new System.EventHandler(this.OnClickButtonSendClass);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textBoxClassInformation);
			this.groupBox2.Controls.Add(this.buttonSendClassInformation);
			this.groupBox2.Location = new System.Drawing.Point(12, 400);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(260, 137);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "クラス情報";
			// 
			// textBoxClassInformation
			// 
			this.textBoxClassInformation.Location = new System.Drawing.Point(8, 18);
			this.textBoxClassInformation.Multiline = true;
			this.textBoxClassInformation.Name = "textBoxClassInformation";
			this.textBoxClassInformation.Size = new System.Drawing.Size(246, 76);
			this.textBoxClassInformation.TabIndex = 6;
			this.textBoxClassInformation.Text = "A\r\nB\r\nC\r\nD\r\nE";
			// 
			// checkBoxIsReady
			// 
			this.checkBoxIsReady.AutoSize = true;
			this.checkBoxIsReady.Location = new System.Drawing.Point(80, 12);
			this.checkBoxIsReady.Name = "checkBoxIsReady";
			this.checkBoxIsReady.Size = new System.Drawing.Size(56, 16);
			this.checkBoxIsReady.TabIndex = 18;
			this.checkBoxIsReady.Text = "Ready";
			this.checkBoxIsReady.UseVisualStyleBackColor = true;
			this.checkBoxIsReady.CheckedChanged += new System.EventHandler(this.checkBoxIsReady_CheckedChanged);
			// 
			// checkBoxContinuousSend
			// 
			this.checkBoxContinuousSend.AutoSize = true;
			this.checkBoxContinuousSend.Location = new System.Drawing.Point(150, 299);
			this.checkBoxContinuousSend.Name = "checkBoxContinuousSend";
			this.checkBoxContinuousSend.Size = new System.Drawing.Size(72, 16);
			this.checkBoxContinuousSend.TabIndex = 17;
			this.checkBoxContinuousSend.Text = "連続送信";
			this.checkBoxContinuousSend.UseVisualStyleBackColor = true;
			this.checkBoxContinuousSend.CheckedChanged += new System.EventHandler(this.checkBoxContinuousSend_CheckedChanged);
			// 
			// textBoxGroup5
			// 
			this.textBoxGroup5.Location = new System.Drawing.Point(8, 299);
			this.textBoxGroup5.Name = "textBoxGroup5";
			this.textBoxGroup5.Size = new System.Drawing.Size(116, 19);
			this.textBoxGroup5.TabIndex = 15;
			this.textBoxGroup5.Text = "4";
			// 
			// textBoxGroup3
			// 
			this.textBoxGroup3.Location = new System.Drawing.Point(8, 247);
			this.textBoxGroup3.Name = "textBoxGroup3";
			this.textBoxGroup3.Size = new System.Drawing.Size(116, 19);
			this.textBoxGroup3.TabIndex = 15;
			this.textBoxGroup3.Text = "2";
			// 
			// textBoxGroup2
			// 
			this.textBoxGroup2.Location = new System.Drawing.Point(8, 222);
			this.textBoxGroup2.Name = "textBoxGroup2";
			this.textBoxGroup2.Size = new System.Drawing.Size(116, 19);
			this.textBoxGroup2.TabIndex = 15;
			this.textBoxGroup2.Text = "1";
			// 
			// textBoxGroup1
			// 
			this.textBoxGroup1.Location = new System.Drawing.Point(8, 197);
			this.textBoxGroup1.Name = "textBoxGroup1";
			this.textBoxGroup1.Size = new System.Drawing.Size(116, 19);
			this.textBoxGroup1.TabIndex = 15;
			this.textBoxGroup1.Text = "0";
			// 
			// textBoxGroup4
			// 
			this.textBoxGroup4.Location = new System.Drawing.Point(8, 274);
			this.textBoxGroup4.Name = "textBoxGroup4";
			this.textBoxGroup4.Size = new System.Drawing.Size(116, 19);
			this.textBoxGroup4.TabIndex = 15;
			this.textBoxGroup4.Text = "3";
			// 
			// LIBSEmulator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 625);
			this.Controls.Add(this.checkBoxIsReady);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textBoxLog);
			this.Controls.Add(this.buttonSendStatusInformation);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LIBSEmulator";
			this.Text = "LIBS装置デバッグツール";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.Load += new System.EventHandler(this.LIBSEmulator_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSendStatusInformation;
        private System.Windows.Forms.Button buttonSendData;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxAnalysisOK;
        private System.Windows.Forms.CheckBox checkBoxCounterBoard;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.TextBox textBoxGroup;
        private System.Windows.Forms.Label labelCounter;
        private System.Windows.Forms.TextBox textBoxCounter;
        private System.Windows.Forms.Button buttonSendClassInformation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxClassInformation;
		private System.Windows.Forms.CheckBox checkBoxIsReady;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxCounterEnd;
		private System.Windows.Forms.TextBox textBoxCounterStart;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxMaterialLengthCount;
		private System.Windows.Forms.CheckBox checkBoxContinuousSend;
		private System.Windows.Forms.TextBox textBoxGroup1;
		private System.Windows.Forms.TextBox textBoxGroup2;
		private System.Windows.Forms.TextBox textBoxGroup3;
		private System.Windows.Forms.TextBox textBoxGroup4;
		private System.Windows.Forms.TextBox textBoxGroup5;
	}
}

