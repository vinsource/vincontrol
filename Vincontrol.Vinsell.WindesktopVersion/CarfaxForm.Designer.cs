namespace Vincontrol.Vinsell.WindesktopVersion
{
    partial class CarfaxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarfaxForm));
            this.carfaxBrowser = new WebKit.WebKitBrowser();
            this.SuspendLayout();
            // 
            // carfaxBrowser
            // 
            this.carfaxBrowser.BackColor = System.Drawing.Color.White;
            this.carfaxBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carfaxBrowser.Location = new System.Drawing.Point(0, 0);
            this.carfaxBrowser.Name = "carfaxBrowser";
            this.carfaxBrowser.Size = new System.Drawing.Size(962, 787);
            this.carfaxBrowser.TabIndex = 0;
            this.carfaxBrowser.Url = null;
            // 
            // CarfaxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 787);
            this.Controls.Add(this.carfaxBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CarfaxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CarFax";
            this.ResumeLayout(false);

        }

        #endregion

        private WebKit.WebKitBrowser carfaxBrowser;


    }
}