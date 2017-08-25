using System.Windows.Forms;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    partial class FavouriteVehicleForm 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavouriteVehicleForm));
            this.panelLayout = new System.Windows.Forms.Panel();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.pbxLoading = new System.Windows.Forms.PictureBox();
            this.panelLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLayout
            // 
            this.panelLayout.AutoScroll = true;
            this.panelLayout.AutoSize = true;
            this.panelLayout.Controls.Add(this.pbxLoading);
            this.panelLayout.Controls.Add(this.elementHost);
            this.panelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLayout.Location = new System.Drawing.Point(0, 0);
            this.panelLayout.Name = "panelLayout";
            this.panelLayout.Size = new System.Drawing.Size(1103, 547);
            this.panelLayout.TabIndex = 0;
            // 
            // elementHost
            // 
            this.elementHost.AutoSize = true;
            this.elementHost.Location = new System.Drawing.Point(75, 52);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(1, 1);
            this.elementHost.TabIndex = 0;
            this.elementHost.Text = "elementHost";
            this.elementHost.Child = null;
            // 
            // pbxLoading
            // 
            this.pbxLoading.Image = global::Vincontrol.Vinsell.WindesktopVersion.Properties.Resources.ajax_loader_mainform;
            this.pbxLoading.Location = new System.Drawing.Point(500, 222);
            this.pbxLoading.Name = "pbxLoading";
            this.pbxLoading.Size = new System.Drawing.Size(102, 103);
            this.pbxLoading.TabIndex = 7;
            this.pbxLoading.TabStop = false;
            // 
            // FavouriteVehicleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 547);
            this.Controls.Add(this.panelLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FavouriteVehicleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Favorite / Note";
            this.panelLayout.ResumeLayout(false);
            this.panelLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panelLayout;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private PictureBox pbxLoading;




    }
}