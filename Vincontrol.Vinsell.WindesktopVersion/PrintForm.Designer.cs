namespace Vincontrol.Vinsell.WindesktopVersion
{
    partial class PrintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            this.manheimBrowser = new WebKit.WebKitBrowser();
            this.SuspendLayout();
            // 
            // manheimBrowser
            // 
            this.manheimBrowser.BackColor = System.Drawing.Color.White;
            this.manheimBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manheimBrowser.Location = new System.Drawing.Point(0, 0);
            this.manheimBrowser.Name = "manheimBrowser";
            this.manheimBrowser.Size = new System.Drawing.Size(1062, 777);
            this.manheimBrowser.TabIndex = 3;
            this.manheimBrowser.Url = null;
            this.manheimBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.manheimBrowser_DocumentCompleted);
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 777);
            this.Controls.Add(this.manheimBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PrintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintForm";
            this.ResumeLayout(false);

        }

        #endregion

        private WebKit.WebKitBrowser manheimBrowser;

    }
}