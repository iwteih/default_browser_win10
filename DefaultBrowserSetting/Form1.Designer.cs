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
            this.btnDefaultBrowser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDefaultBrowser
            // 
            this.btnDefaultBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDefaultBrowser.Location = new System.Drawing.Point(0, 0);
            this.btnDefaultBrowser.Name = "btnDefaultBrowser";
            this.btnDefaultBrowser.Size = new System.Drawing.Size(384, 61);
            this.btnDefaultBrowser.TabIndex = 1;
            this.btnDefaultBrowser.Text = "Set Default Browser";
            this.btnDefaultBrowser.UseVisualStyleBackColor = true;
            this.btnDefaultBrowser.Click += new System.EventHandler(this.btnDefaultBrowser_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 61);
            this.Controls.Add(this.btnDefaultBrowser);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Default Brower Settings for Win10";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDefaultBrowser;
    }
}

