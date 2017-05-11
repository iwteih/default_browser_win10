namespace DefaultBrowserSetting
{
    partial class Form1
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
            this.lbMessage = new System.Windows.Forms.Label();
            this.btnDefaultBrowser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = true;
            this.lbMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMessage.ForeColor = System.Drawing.Color.Red;
            this.lbMessage.Location = new System.Drawing.Point(0, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(189, 15);
            this.lbMessage.TabIndex = 0;
            this.lbMessage.Text = "Failed to set default browser";
            this.lbMessage.Visible = false;
            // 
            // btnDefaultBrowser
            // 
            this.btnDefaultBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDefaultBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefaultBrowser.Location = new System.Drawing.Point(0, 15);
            this.btnDefaultBrowser.Name = "btnDefaultBrowser";
            this.btnDefaultBrowser.Size = new System.Drawing.Size(344, 76);
            this.btnDefaultBrowser.TabIndex = 2;
            this.btnDefaultBrowser.Text = "Set Default Browser";
            this.btnDefaultBrowser.UseVisualStyleBackColor = true;
            this.btnDefaultBrowser.Click += new System.EventHandler(this.btnDefaultBrowser_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 91);
            this.Controls.Add(this.btnDefaultBrowser);
            this.Controls.Add(this.lbMessage);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Default Brower Settings for Win10";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.Button btnDefaultBrowser;
    }
}

