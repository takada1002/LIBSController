namespace LIBSController
{
    partial class ControlForm
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
			this.comboBoxConveyorSpeed = new System.Windows.Forms.ComboBox();
			this.numericUpDownPaddleTiming = new System.Windows.Forms.NumericUpDown();
			this.buttonChangeConveyorSpeed = new System.Windows.Forms.Button();
			this.buttonChangePaddleTiming = new System.Windows.Forms.Button();
			this.buttonShutdown = new System.Windows.Forms.Button();
			this.buttonResetLIBSSorter = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelConnection = new System.Windows.Forms.Label();
			this.labelAnalysisResult = new System.Windows.Forms.Label();
			this.labelAnalysisGroup = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelPaddle6 = new System.Windows.Forms.Label();
			this.labelPaddle5 = new System.Windows.Forms.Label();
			this.labelPaddle4 = new System.Windows.Forms.Label();
			this.labelPaddle3 = new System.Windows.Forms.Label();
			this.labelPaddle2 = new System.Windows.Forms.Label();
			this.labelPaddle1 = new System.Windows.Forms.Label();
			this.labelExit1 = new System.Windows.Forms.Label();
			this.labelExit2 = new System.Windows.Forms.Label();
			this.buttonMaterialSetting = new System.Windows.Forms.Button();
			this.labelTitle = new System.Windows.Forms.Label();
			this.buttonAirNozzleSetting = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonNozzleBack = new System.Windows.Forms.Button();
			this.buttonNozzleNext = new System.Windows.Forms.Button();
			this.labelNozzleNumber = new System.Windows.Forms.Label();
			this.buttonStopLIBSSorter = new System.Windows.Forms.Button();
			this.buttonStartLIBSSorter = new System.Windows.Forms.Button();
			this.statusLabel = new LIBSController.StatusLabel();
			this.btnEncoderReset = new System.Windows.Forms.Button();
			this.btnLIBSReset = new System.Windows.Forms.Button();
			this.btnInverterReset = new System.Windows.Forms.Button();
			this.buttonTimingBack = new System.Windows.Forms.Button();
			this.buttonTimingNext = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonTimingLargeBack = new System.Windows.Forms.Button();
			this.buttonTimingLargeNext = new System.Windows.Forms.Button();
			this.textBoxPaddleTiming = new System.Windows.Forms.TextBox();
			this.statusLabel = new LIBSController.StatusLabel();
			this.labelVersion = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBoxConveyorSpeed
			// 
			this.comboBoxConveyorSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxConveyorSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboBoxConveyorSpeed.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboBoxConveyorSpeed.FormattingEnabled = true;
			this.comboBoxConveyorSpeed.Name = "comboBoxConveyorSpeed";
			this.comboBoxConveyorSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.comboBoxConveyorSpeed.TabIndex = 10;
			// 
			// numericUpDownPaddleTiming
			// 
			this.numericUpDownPaddleTiming.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericUpDownPaddleTiming.Location = new System.Drawing.Point(341, 263);
			this.numericUpDownPaddleTiming.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.numericUpDownPaddleTiming.Name = "numericUpDownPaddleTiming";
			this.numericUpDownPaddleTiming.Size = new System.Drawing.Size(292, 71);
			this.numericUpDownPaddleTiming.TabIndex = 14;
			this.numericUpDownPaddleTiming.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPaddleTiming.ValueChanged += new System.EventHandler(this.OnValueChangedPaddleTiming);
			// 
			// buttonChangeConveyorSpeed
			// 
			this.buttonChangeConveyorSpeed.BackColor = System.Drawing.Color.White;
			this.buttonChangeConveyorSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonChangeConveyorSpeed.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonChangeConveyorSpeed.Location = new System.Drawing.Point(680, 114);
			this.buttonChangeConveyorSpeed.Name = "buttonChangeConveyorSpeed";
			this.buttonChangeConveyorSpeed.Size = new System.Drawing.Size(150, 72);
			this.buttonChangeConveyorSpeed.TabIndex = 1;
			this.buttonChangeConveyorSpeed.Text = "速度変更";
			this.buttonChangeConveyorSpeed.UseVisualStyleBackColor = false;
			this.buttonChangeConveyorSpeed.Click += new System.EventHandler(this.OnClickButtonSpeedChange);
			// 
			// buttonChangePaddleTiming
			// 
			this.buttonChangePaddleTiming.BackColor = System.Drawing.Color.White;
			this.buttonChangePaddleTiming.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonChangePaddleTiming.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonChangePaddleTiming.Name = "buttonChangePaddleTiming";
			this.buttonChangePaddleTiming.Size = new System.Drawing.Size(150, 72);
			this.buttonChangePaddleTiming.TabIndex = 2;
			this.buttonChangePaddleTiming.Text = "タイミング変更";
			this.buttonChangePaddleTiming.UseVisualStyleBackColor = false;
			this.buttonChangePaddleTiming.Click += new System.EventHandler(this.OnClickButtonTimingChange);
			// 
			// buttonShutdown
			// 
			this.buttonShutdown.BackColor = System.Drawing.Color.White;
			this.buttonShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonShutdown.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonShutdown.Location = new System.Drawing.Point(847, 625);
			this.buttonShutdown.Name = "buttonShutdown";
			this.buttonShutdown.Size = new System.Drawing.Size(150, 72);
			this.buttonShutdown.TabIndex = 9;
			this.buttonShutdown.Text = "シャットダウン";
			this.buttonShutdown.UseVisualStyleBackColor = false;
			this.buttonShutdown.Click += new System.EventHandler(this.OnClickButtonShutdown);
			// 
			// buttonResetLIBSSorter
			// 
			this.buttonResetLIBSSorter.BackColor = System.Drawing.Color.White;
			this.buttonResetLIBSSorter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonResetLIBSSorter.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonResetLIBSSorter.Location = new System.Drawing.Point(847, 537);
			this.buttonResetLIBSSorter.Name = "buttonResetLIBSSorter";
			this.buttonResetLIBSSorter.Size = new System.Drawing.Size(150, 72);
			this.buttonResetLIBSSorter.TabIndex = 8;
			this.buttonResetLIBSSorter.Text = "LIBS ソータ\r\n再起動";
			this.buttonResetLIBSSorter.UseVisualStyleBackColor = false;
			this.buttonResetLIBSSorter.Click += new System.EventHandler(this.OnClickResetLIBSSorter);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelConnection);
			this.groupBox1.Controls.Add(this.labelAnalysisResult);
			this.groupBox1.Controls.Add(this.labelAnalysisGroup);
			this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox1.Location = new System.Drawing.Point(17, 385);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(631, 166);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "LIBS 装置";
			// 
			// labelConnection
			// 
			this.labelConnection.BackColor = System.Drawing.Color.Red;
			this.labelConnection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelConnection.Font = new System.Drawing.Font("MS UI Gothic", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelConnection.Location = new System.Drawing.Point(168, 23);
			this.labelConnection.Name = "labelConnection";
			this.labelConnection.Size = new System.Drawing.Size(448, 62);
			this.labelConnection.TabIndex = 12;
			this.labelConnection.Text = "未接続";
			this.labelConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelAnalysisResult
			// 
			this.labelAnalysisResult.BackColor = System.Drawing.Color.Lime;
			this.labelAnalysisResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelAnalysisResult.Font = new System.Drawing.Font("MS UI Gothic", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelAnalysisResult.Location = new System.Drawing.Point(12, 90);
			this.labelAnalysisResult.Name = "labelAnalysisResult";
			this.labelAnalysisResult.Size = new System.Drawing.Size(150, 62);
			this.labelAnalysisResult.TabIndex = 11;
			this.labelAnalysisResult.Text = "OK";
			this.labelAnalysisResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelAnalysisGroup
			// 
			this.labelAnalysisGroup.BackColor = System.Drawing.Color.White;
			this.labelAnalysisGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelAnalysisGroup.Font = new System.Drawing.Font("MS UI Gothic", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelAnalysisGroup.Location = new System.Drawing.Point(168, 90);
			this.labelAnalysisGroup.Name = "labelAnalysisGroup";
			this.labelAnalysisGroup.Size = new System.Drawing.Size(448, 62);
			this.labelAnalysisGroup.TabIndex = 10;
			this.labelAnalysisGroup.Text = "NO_MESURE";
			this.labelAnalysisGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.labelPaddle6);
			this.groupBox2.Controls.Add(this.labelPaddle5);
			this.groupBox2.Controls.Add(this.labelPaddle4);
			this.groupBox2.Controls.Add(this.labelPaddle3);
			this.groupBox2.Controls.Add(this.labelPaddle2);
			this.groupBox2.Controls.Add(this.labelPaddle1);
			this.groupBox2.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox2.Location = new System.Drawing.Point(17, 580);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(631, 129);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "エアーノズル";
			// 
			// labelPaddle6
			// 
			this.labelPaddle6.BackColor = System.Drawing.Color.White;
			this.labelPaddle6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPaddle6.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPaddle6.Location = new System.Drawing.Point(516, 35);
			this.labelPaddle6.Name = "labelPaddle6";
			this.labelPaddle6.Size = new System.Drawing.Size(98, 72);
			this.labelPaddle6.TabIndex = 5;
			this.labelPaddle6.Text = "６";
			this.labelPaddle6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPaddle6.Visible = false;
			this.labelPaddle6.Click += new System.EventHandler(this.labelPaddle6_Click);
			// 
			// labelPaddle5
			// 
			this.labelPaddle5.BackColor = System.Drawing.Color.White;
			this.labelPaddle5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPaddle5.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPaddle5.Location = new System.Drawing.Point(418, 35);
			this.labelPaddle5.Name = "labelPaddle5";
			this.labelPaddle5.Size = new System.Drawing.Size(98, 72);
			this.labelPaddle5.TabIndex = 4;
			this.labelPaddle5.Text = "５";
			this.labelPaddle5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPaddle5.Visible = false;
			this.labelPaddle5.Click += new System.EventHandler(this.labelPaddle5_Click);
			// 
			// labelPaddle4
			// 
			this.labelPaddle4.BackColor = System.Drawing.Color.White;
			this.labelPaddle4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPaddle4.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPaddle4.Location = new System.Drawing.Point(320, 35);
			this.labelPaddle4.Name = "labelPaddle4";
			this.labelPaddle4.Size = new System.Drawing.Size(98, 72);
			this.labelPaddle4.TabIndex = 3;
			this.labelPaddle4.Text = "４";
			this.labelPaddle4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPaddle4.Visible = false;
			this.labelPaddle4.Click += new System.EventHandler(this.labelPaddle4_Click);
			// 
			// labelPaddle3
			// 
			this.labelPaddle3.BackColor = System.Drawing.Color.White;
			this.labelPaddle3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPaddle3.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPaddle3.Location = new System.Drawing.Point(222, 35);
			this.labelPaddle3.Name = "labelPaddle3";
			this.labelPaddle3.Size = new System.Drawing.Size(98, 72);
			this.labelPaddle3.TabIndex = 2;
			this.labelPaddle3.Text = "３";
			this.labelPaddle3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPaddle3.Click += new System.EventHandler(this.labelPaddle3_Click);
			// 
			// labelPaddle2
			// 
			this.labelPaddle2.BackColor = System.Drawing.Color.White;
			this.labelPaddle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPaddle2.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPaddle2.Location = new System.Drawing.Point(124, 35);
			this.labelPaddle2.Name = "labelPaddle2";
			this.labelPaddle2.Size = new System.Drawing.Size(98, 72);
			this.labelPaddle2.TabIndex = 1;
			this.labelPaddle2.Text = "２";
			this.labelPaddle2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPaddle2.Click += new System.EventHandler(this.labelPaddle2_Click);
			// 
			// labelPaddle1
			// 
			this.labelPaddle1.BackColor = System.Drawing.Color.White;
			this.labelPaddle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPaddle1.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPaddle1.Location = new System.Drawing.Point(26, 35);
			this.labelPaddle1.Name = "labelPaddle1";
			this.labelPaddle1.Size = new System.Drawing.Size(98, 72);
			this.labelPaddle1.TabIndex = 0;
			this.labelPaddle1.Text = "１";
			this.labelPaddle1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPaddle1.Click += new System.EventHandler(this.labelPaddle1_Click);
			// 
			// labelExit1
			// 
			this.labelExit1.Location = new System.Drawing.Point(3, 50);
			this.labelExit1.Name = "labelExit1";
			this.labelExit1.Size = new System.Drawing.Size(56, 60);
			this.labelExit1.TabIndex = 14;
			this.labelExit1.DoubleClick += new System.EventHandler(this.OnDoubleClickLabelExit);
			// 
			// labelExit2
			// 
			this.labelExit2.Location = new System.Drawing.Point(959, 50);
			this.labelExit2.Name = "labelExit2";
			this.labelExit2.Size = new System.Drawing.Size(65, 60);
			this.labelExit2.TabIndex = 15;
			this.labelExit2.Click += new System.EventHandler(this.OnDoubleClickLabelExit2);
			// 
			// buttonMaterialSetting
			// 
			this.buttonMaterialSetting.BackColor = System.Drawing.Color.White;
			this.buttonMaterialSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonMaterialSetting.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonMaterialSetting.Location = new System.Drawing.Point(680, 625);
			this.buttonMaterialSetting.Name = "buttonMaterialSetting";
			this.buttonMaterialSetting.Size = new System.Drawing.Size(150, 72);
			this.buttonMaterialSetting.TabIndex = 4;
			this.buttonMaterialSetting.Text = "選別対象\r\n設定";
			this.buttonMaterialSetting.UseVisualStyleBackColor = false;
			this.buttonMaterialSetting.Click += new System.EventHandler(this.OnClickMaterialSetting);
			// 
			// labelTitle
			// 
			this.labelTitle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.labelTitle.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelTitle.ForeColor = System.Drawing.Color.White;
			this.labelTitle.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.labelTitle.Location = new System.Drawing.Point(0, 0);
			this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(1024, 50);
			this.labelTitle.TabIndex = 17;
			this.labelTitle.Text = " LIBS装置コントローラー";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonAirNozzleSetting
			// 
			this.buttonAirNozzleSetting.BackColor = System.Drawing.Color.White;
			this.buttonAirNozzleSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonAirNozzleSetting.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonAirNozzleSetting.Location = new System.Drawing.Point(680, 537);
			this.buttonAirNozzleSetting.Name = "buttonAirNozzleSetting";
			this.buttonAirNozzleSetting.Size = new System.Drawing.Size(150, 72);
			this.buttonAirNozzleSetting.TabIndex = 3;
			this.buttonAirNozzleSetting.Text = "エアーノズル\r\n動作設定";
			this.buttonAirNozzleSetting.UseVisualStyleBackColor = false;
			this.buttonAirNozzleSetting.Click += new System.EventHandler(this.OnClickAirNozzleSetting);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 19);
			this.label1.TabIndex = 19;
			this.label1.Text = "ノズル番号";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 19);
			this.label2.TabIndex = 19;
			this.label2.Text = "タイミング";
			// 
			// buttonNozzleBack
			// 
			this.buttonNozzleBack.BackColor = System.Drawing.Color.White;
			this.buttonNozzleBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonNozzleBack.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonNozzleBack.Name = "buttonNozzleBack";
			this.buttonNozzleBack.TabIndex = 11;
			this.buttonNozzleBack.Text = "◀";
			this.buttonNozzleBack.UseVisualStyleBackColor = false;
			this.buttonNozzleBack.Click += new System.EventHandler(this.buttonNozzleBack_Click);
			// 
			// buttonNozzleNext
			// 
			this.buttonNozzleNext.BackColor = System.Drawing.Color.White;
			this.buttonNozzleNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonNozzleNext.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonNozzleNext.Name = "buttonNozzleNext";
			this.buttonNozzleNext.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonNozzleNext.TabIndex = 13;
			this.buttonNozzleNext.Text = "▶";
			this.buttonNozzleNext.UseVisualStyleBackColor = false;
			this.buttonNozzleNext.Click += new System.EventHandler(this.buttonNozzleNext_Click);
			// 
			// labelNozzleNumber
			// 
			this.labelNozzleNumber.BackColor = System.Drawing.Color.White;
			this.labelNozzleNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelNozzleNumber.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelNozzleNumber.Name = "labelNozzleNumber";
			this.labelNozzleNumber.TabIndex = 12;
			this.labelNozzleNumber.Text = "1";
			this.labelNozzleNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelNozzleNumber.TextChanged += new System.EventHandler(this.labelNozzleNumber_TextChanged);
			// 
			// buttonStopLIBSSorter
			// 
			this.buttonStopLIBSSorter.BackColor = System.Drawing.Color.White;
			this.buttonStopLIBSSorter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonStopLIBSSorter.Font = new System.Drawing.Font("MS UI Gothic", 16F);
			this.buttonStopLIBSSorter.Name = "buttonStopLIBSSorter";
			this.buttonStopLIBSSorter.Size = new System.Drawing.Size(150, 72);
			this.buttonStopLIBSSorter.TabIndex = 6;
			this.buttonStopLIBSSorter.Text = "強制運転\r\n停止";
			this.buttonStopLIBSSorter.UseVisualStyleBackColor = false;
			this.buttonStopLIBSSorter.Visible = false;
			this.buttonStopLIBSSorter.Click += new System.EventHandler(this.OnClickStopLIBSSorter);
			// 
			// buttonStartLIBSSorter
			// 
			this.buttonStartLIBSSorter.BackColor = System.Drawing.Color.White;
			this.buttonStartLIBSSorter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonStartLIBSSorter.Font = new System.Drawing.Font("MS UI Gothic", 16F);
			this.buttonStartLIBSSorter.Location = new System.Drawing.Point(847, 113);
			this.buttonStartLIBSSorter.Name = "buttonStartLIBSSorter";
			this.buttonStartLIBSSorter.Size = new System.Drawing.Size(150, 72);
			this.buttonStartLIBSSorter.TabIndex = 5;
			this.buttonStartLIBSSorter.Text = "強制運転\r\n開始";
			this.buttonStartLIBSSorter.UseVisualStyleBackColor = false;
			this.buttonStartLIBSSorter.Visible = false;
			this.buttonStartLIBSSorter.Click += new System.EventHandler(this.OnClickButtonStartLIBSSorter);
			// 
			// statusLabel
			// 
			this.statusLabel.BackColor = System.Drawing.Color.Yellow;
			this.statusLabel.LabelText = "";
			this.statusLabel.Location = new System.Drawing.Point(0, 728);
			this.statusLabel.Margin = new System.Windows.Forms.Padding(0);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(1024, 40);
			this.statusLabel.Status = LIBSController.LIBSSorter.SorterStatus.NOT_READY;
			this.statusLabel.TabIndex = 0;
			// 
			// btnEncoderReset
			// 
			this.btnEncoderReset.BackColor = System.Drawing.Color.Red;
			this.btnEncoderReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEncoderReset.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnEncoderReset.ForeColor = System.Drawing.Color.Black;
			this.btnEncoderReset.Location = new System.Drawing.Point(847, 449);
			this.btnEncoderReset.Name = "btnEncoderReset";
			this.btnEncoderReset.Size = new System.Drawing.Size(150, 72);
			this.btnEncoderReset.TabIndex = 20;
			this.btnEncoderReset.Text = "エンコーダー\r\nリセット";
			this.btnEncoderReset.UseVisualStyleBackColor = false;
			this.btnEncoderReset.Click += new System.EventHandler(this.btnEncoderReset_Click);
			// 
			// btnLIBSReset
			// 
			this.btnLIBSReset.BackColor = System.Drawing.Color.Red;
			this.btnLIBSReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLIBSReset.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnLIBSReset.ForeColor = System.Drawing.Color.Black;
			this.btnLIBSReset.Location = new System.Drawing.Point(680, 361);
			this.btnLIBSReset.Name = "btnLIBSReset";
			this.btnLIBSReset.Size = new System.Drawing.Size(150, 72);
			this.btnLIBSReset.TabIndex = 21;
			this.btnLIBSReset.Text = "LIBSエラー\r\nリセット";
			this.btnLIBSReset.UseVisualStyleBackColor = false;
			this.btnLIBSReset.Click += new System.EventHandler(this.btnLIBSReset_Click);
			// 
			// btnInverterReset
			// 
			this.btnInverterReset.BackColor = System.Drawing.Color.Red;
			this.btnInverterReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInverterReset.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnInverterReset.ForeColor = System.Drawing.Color.Black;
			this.btnInverterReset.Location = new System.Drawing.Point(680, 449);
			this.btnInverterReset.Name = "btnInverterReset";
			this.btnInverterReset.Size = new System.Drawing.Size(150, 72);
			this.btnInverterReset.TabIndex = 22;
			this.btnInverterReset.Text = "インバーター\r\nリセット";
			this.btnInverterReset.UseVisualStyleBackColor = false;
			this.btnInverterReset.Click += new System.EventHandler(this.btnInverterReset_Click);
			// 
			// buttonTimingBack
			// 
			this.buttonTimingBack.BackColor = System.Drawing.Color.White;
			this.buttonTimingBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonTimingBack.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonTimingBack.Location = new System.Drawing.Point(354, 302);
			this.buttonTimingBack.Name = "buttonTimingBack";
			this.buttonTimingBack.Size = new System.Drawing.Size(85, 72);
			this.buttonTimingBack.TabIndex = 11;
			this.buttonTimingBack.Text = "◀";
			this.buttonTimingBack.UseVisualStyleBackColor = false;
			this.buttonTimingBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTimingBack_MouseDown);
			this.buttonTimingBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTimingBack_MouseUp);
			// 
			// buttonTimingNext
			// 
			this.buttonTimingNext.BackColor = System.Drawing.Color.White;
			this.buttonTimingNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonTimingNext.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonTimingNext.Location = new System.Drawing.Point(444, 302);
			this.buttonTimingNext.Name = "buttonTimingNext";
			this.buttonTimingNext.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonTimingNext.Size = new System.Drawing.Size(85, 72);
			this.buttonTimingNext.TabIndex = 13;
			this.buttonTimingNext.Text = "▶";
			this.buttonTimingNext.UseVisualStyleBackColor = false;
			this.buttonTimingNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTimingNext_MouseDown);
			this.buttonTimingNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTimingNext_MouseUp);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(271, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 19);
			this.label3.TabIndex = 19;
			this.label3.Text = "コンベア速度";
			// 
			// buttonTimingLargeBack
			// 
			this.buttonTimingLargeBack.BackColor = System.Drawing.Color.White;
			this.buttonTimingLargeBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonTimingLargeBack.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonTimingLargeBack.Location = new System.Drawing.Point(247, 302);
			this.buttonTimingLargeBack.Margin = new System.Windows.Forms.Padding(0);
			this.buttonTimingLargeBack.Name = "buttonTimingLargeBack";
			this.buttonTimingLargeBack.Size = new System.Drawing.Size(102, 72);
			this.buttonTimingLargeBack.TabIndex = 11;
			this.buttonTimingLargeBack.Text = "◀◀";
			this.buttonTimingLargeBack.UseVisualStyleBackColor = false;
			this.buttonTimingLargeBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTimingLargeBack_MouseDown);
			this.buttonTimingLargeBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTimingLargeBack_MouseUp);
			// 
			// buttonTimingLargeNext
			// 
			this.buttonTimingLargeNext.BackColor = System.Drawing.Color.White;
			this.buttonTimingLargeNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonTimingLargeNext.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonTimingLargeNext.Location = new System.Drawing.Point(534, 302);
			this.buttonTimingLargeNext.Margin = new System.Windows.Forms.Padding(0);
			this.buttonTimingLargeNext.Name = "buttonTimingLargeNext";
			this.buttonTimingLargeNext.Size = new System.Drawing.Size(102, 72);
			this.buttonTimingLargeNext.TabIndex = 13;
			this.buttonTimingLargeNext.Text = "▶▶";
			this.buttonTimingLargeNext.UseVisualStyleBackColor = false;
			this.buttonTimingLargeNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTimingLargeNext_MouseDown);
			this.buttonTimingLargeNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTimingLargeNext_MouseUp);
			// 
			// textBoxPaddleTiming
			// 
			this.textBoxPaddleTiming.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxPaddleTiming.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textBoxPaddleTiming.Location = new System.Drawing.Point(246, 221);
			this.textBoxPaddleTiming.Name = "textBoxPaddleTiming";
			this.textBoxPaddleTiming.Size = new System.Drawing.Size(390, 71);
			this.textBoxPaddleTiming.TabIndex = 23;
			this.textBoxPaddleTiming.Text = "1";
			this.textBoxPaddleTiming.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxPaddleTiming.TextChanged += new System.EventHandler(this.textBoxPaddleTiming_TextChanged);
			// 
			// statusLabel
			// 
			this.statusLabel.BackColor = System.Drawing.Color.Yellow;
			this.statusLabel.LabelText = "";
			this.statusLabel.Location = new System.Drawing.Point(0, 728);
			this.statusLabel.Margin = new System.Windows.Forms.Padding(0);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(1024, 40);
			this.statusLabel.Status = LIBSController.LIBSSorter.SorterStatus.NOT_READY;
			this.statusLabel.TabIndex = 0;
			// 
			// labelVersion
			// 
			this.labelVersion.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.labelVersion.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelVersion.ForeColor = System.Drawing.Color.White;
			this.labelVersion.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.labelVersion.Location = new System.Drawing.Point(838, 0);
			this.labelVersion.Margin = new System.Windows.Forms.Padding(0);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(186, 50);
			this.labelVersion.TabIndex = 24;
			this.labelVersion.Text = "Version 3.0.0";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ControlForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.textBoxPaddleTiming);
			this.Controls.Add(this.btnInverterReset);
			this.Controls.Add(this.btnLIBSReset);
			this.Controls.Add(this.btnEncoderReset);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelNozzleNumber);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.buttonAirNozzleSetting);
			this.Controls.Add(this.buttonMaterialSetting);
			this.Controls.Add(this.labelExit2);
			this.Controls.Add(this.labelExit1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonResetLIBSSorter);
			this.Controls.Add(this.buttonShutdown);
			this.Controls.Add(this.buttonStopLIBSSorter);
			this.Controls.Add(this.buttonStartLIBSSorter);
			this.Controls.Add(this.buttonTimingLargeNext);
			this.Controls.Add(this.buttonTimingNext);
			this.Controls.Add(this.buttonNozzleNext);
			this.Controls.Add(this.buttonTimingLargeBack);
			this.Controls.Add(this.buttonTimingBack);
			this.Controls.Add(this.buttonNozzleBack);
			this.Controls.Add(this.buttonChangePaddleTiming);
			this.Controls.Add(this.buttonChangeConveyorSpeed);
			this.Controls.Add(this.numericUpDownPaddleTiming);
			this.Controls.Add(this.comboBoxConveyorSpeed);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ControlForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MainForm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.Load += new System.EventHandler(this.ControlForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddleTiming)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxConveyorSpeed;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddleTiming;
        private System.Windows.Forms.Button buttonChangeConveyorSpeed;
        private System.Windows.Forms.Button buttonChangePaddleTiming;
        private System.Windows.Forms.Button buttonShutdown;
        private System.Windows.Forms.Button buttonResetLIBSSorter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelAnalysisResult;
        private System.Windows.Forms.Label labelAnalysisGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelConnection;
        private System.Windows.Forms.Label labelPaddle6;
        private System.Windows.Forms.Label labelPaddle5;
        private System.Windows.Forms.Label labelPaddle4;
        private System.Windows.Forms.Label labelPaddle3;
        private System.Windows.Forms.Label labelPaddle2;
        private System.Windows.Forms.Label labelPaddle1;
        private System.Windows.Forms.Label labelExit1;
        private System.Windows.Forms.Label labelExit2;
        private System.Windows.Forms.Button buttonMaterialSetting;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonAirNozzleSetting;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private StatusLabel statusLabel;
		private System.Windows.Forms.Button buttonNozzleBack;
		private System.Windows.Forms.Button buttonNozzleNext;
		private System.Windows.Forms.Label labelNozzleNumber;
		private System.Windows.Forms.Button buttonStopLIBSSorter;
		private System.Windows.Forms.Button buttonStartLIBSSorter;
		private System.Windows.Forms.Button btnEncoderReset;
		private System.Windows.Forms.Button btnLIBSReset;
		private System.Windows.Forms.Button btnInverterReset;
		private System.Windows.Forms.Button buttonTimingBack;
		private System.Windows.Forms.Button buttonTimingNext;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonTimingLargeBack;
		private System.Windows.Forms.Button buttonTimingLargeNext;
		private System.Windows.Forms.TextBox textBoxPaddleTiming;
		private System.Windows.Forms.Label labelVersion;
	}
}

