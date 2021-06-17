namespace LIBSController
{
    partial class ConfirmForm
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.BackColor = System.Drawing.Color.White;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonOK.Font = new System.Drawing.Font("MS UI Gothic", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonOK.Location = new System.Drawing.Point(115, 646);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(307, 77);
			this.buttonOK.TabIndex = 45;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = false;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.BackColor = System.Drawing.Color.White;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonCancel.Font = new System.Drawing.Font("MS UI Gothic", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonCancel.Location = new System.Drawing.Point(593, 646);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(307, 77);
			this.buttonCancel.TabIndex = 45;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = false;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.Yellow;
			this.lblTitle.Font = new System.Drawing.Font("MS UI Gothic", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(1024, 40);
			this.lblTitle.TabIndex = 46;
			this.lblTitle.Text = "タイトル";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.Location = new System.Drawing.Point(51, 147);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(913, 317);
			this.lblMessage.TabIndex = 47;
			this.lblMessage.Text = "テキスト";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ConfirmForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ConfirmForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ConfirmForm";
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMessage;
    }
}