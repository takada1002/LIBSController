namespace LIBSController
{
    partial class StatusLabel
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.labelStatus = new System.Windows.Forms.Label();
			this.labelMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelStatus
			// 
			this.labelStatus.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelStatus.Location = new System.Drawing.Point(0, 0);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(200, 40);
			this.labelStatus.TabIndex = 0;
			this.labelStatus.Text = "運転不可能";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelMessage
			// 
			this.labelMessage.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelMessage.Location = new System.Drawing.Point(200, 0);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(731, 40);
			this.labelMessage.TabIndex = 1;
			this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StatusLabel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Orange;
			this.Controls.Add(this.labelMessage);
			this.Controls.Add(this.labelStatus);
			this.Name = "StatusLabel";
			this.Size = new System.Drawing.Size(1024, 40);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelMessage;
	}
}
