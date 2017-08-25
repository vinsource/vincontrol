using System.Windows.Forms;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    partial class ManheimDetailForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManheimDetailForm));
            this.cbbTrim = new System.Windows.Forms.ComboBox();
            this.lblPriceBelow = new System.Windows.Forms.Label();
            this.lblPriceAverage = new System.Windows.Forms.Label();
            this.lblPriceAbove = new System.Windows.Forms.Label();
            this.dgManheimDetail = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Odometer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Auction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Engine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbbRegion = new System.Windows.Forms.ComboBox();
            this.pbxLoading = new System.Windows.Forms.PictureBox();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgManheimDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbTrim
            // 
            this.cbbTrim.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTrim.FormattingEnabled = true;
            this.cbbTrim.Location = new System.Drawing.Point(43, 19);
            this.cbbTrim.Name = "cbbTrim";
            this.cbbTrim.Size = new System.Drawing.Size(162, 23);
            this.cbbTrim.TabIndex = 10;
            // 
            // lblPriceBelow
            // 
            this.lblPriceBelow.AutoSize = true;
            this.lblPriceBelow.BackColor = System.Drawing.Color.Transparent;
            this.lblPriceBelow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPriceBelow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceBelow.ForeColor = System.Drawing.Color.White;
            this.lblPriceBelow.Location = new System.Drawing.Point(30, 0);
            this.lblPriceBelow.Name = "lblPriceBelow";
            this.lblPriceBelow.Size = new System.Drawing.Size(53, 18);
            this.lblPriceBelow.TabIndex = 1;
            this.lblPriceBelow.Text = "label1";
            this.lblPriceBelow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPriceBelow.Paint += new System.Windows.Forms.PaintEventHandler(this.lblPriceBelow_Paint);
            // 
            // lblPriceAverage
            // 
            this.lblPriceAverage.AutoSize = true;
            this.lblPriceAverage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceAverage.ForeColor = System.Drawing.Color.White;
            this.lblPriceAverage.Location = new System.Drawing.Point(28, 2);
            this.lblPriceAverage.Name = "lblPriceAverage";
            this.lblPriceAverage.Size = new System.Drawing.Size(51, 16);
            this.lblPriceAverage.TabIndex = 2;
            this.lblPriceAverage.Text = "label2";
            this.lblPriceAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPriceAverage.Paint += new System.Windows.Forms.PaintEventHandler(this.lblPriceAverage_Paint);
            // 
            // lblPriceAbove
            // 
            this.lblPriceAbove.AutoSize = true;
            this.lblPriceAbove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceAbove.ForeColor = System.Drawing.Color.White;
            this.lblPriceAbove.Location = new System.Drawing.Point(23, 2);
            this.lblPriceAbove.Name = "lblPriceAbove";
            this.lblPriceAbove.Size = new System.Drawing.Size(51, 16);
            this.lblPriceAbove.TabIndex = 3;
            this.lblPriceAbove.Text = "label3";
            this.lblPriceAbove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPriceAbove.Paint += new System.Windows.Forms.PaintEventHandler(this.lblPriceAbove_Paint);
            // 
            // dgManheimDetail
            // 
            this.dgManheimDetail.AllowUserToAddRows = false;
            this.dgManheimDetail.AllowUserToDeleteRows = false;
            this.dgManheimDetail.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(244)))));
            this.dgManheimDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgManheimDetail.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgManheimDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgManheimDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgManheimDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Odometer,
            this.Price,
            this.SaleDate,
            this.Auction,
            this.Engine,
            this.TR,
            this.Cond,
            this.Color});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgManheimDetail.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgManheimDetail.EnableHeadersVisualStyles = false;
            this.dgManheimDetail.Location = new System.Drawing.Point(3, 3);
            this.dgManheimDetail.Name = "dgManheimDetail";
            this.dgManheimDetail.ReadOnly = true;
            this.dgManheimDetail.RowHeadersVisible = false;
            this.dgManheimDetail.Size = new System.Drawing.Size(903, 444);
            this.dgManheimDetail.TabIndex = 4;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.RoyalBlue;
            this.Type.DefaultCellStyle = dataGridViewCellStyle3;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Odometer
            // 
            this.Odometer.DataPropertyName = "Odometer";
            this.Odometer.HeaderText = "Odometer";
            this.Odometer.Name = "Odometer";
            this.Odometer.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // SaleDate
            // 
            this.SaleDate.DataPropertyName = "SaleDate";
            this.SaleDate.HeaderText = "Sale Date";
            this.SaleDate.Name = "SaleDate";
            this.SaleDate.ReadOnly = true;
            // 
            // Auction
            // 
            this.Auction.DataPropertyName = "Auction";
            this.Auction.HeaderText = "Auction";
            this.Auction.Name = "Auction";
            this.Auction.ReadOnly = true;
            // 
            // Engine
            // 
            this.Engine.DataPropertyName = "Engine";
            this.Engine.HeaderText = "Engine";
            this.Engine.Name = "Engine";
            this.Engine.ReadOnly = true;
            // 
            // TR
            // 
            this.TR.DataPropertyName = "Tr";
            this.TR.HeaderText = "TR";
            this.TR.Name = "TR";
            this.TR.ReadOnly = true;
            // 
            // Cond
            // 
            this.Cond.DataPropertyName = "Cond";
            this.Cond.HeaderText = "Cond";
            this.Cond.Name = "Cond";
            this.Cond.ReadOnly = true;
            // 
            // Color
            // 
            this.Color.DataPropertyName = "Color";
            this.Color.HeaderText = "Color";
            this.Color.Name = "Color";
            this.Color.ReadOnly = true;
            // 
            // cbbRegion
            // 
            this.cbbRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbRegion.FormattingEnabled = true;
            this.cbbRegion.Location = new System.Drawing.Point(764, 54);
            this.cbbRegion.Name = "cbbRegion";
            this.cbbRegion.Size = new System.Drawing.Size(138, 23);
            this.cbbRegion.TabIndex = 5;
            // 
            // pbxLoading
            // 
            this.pbxLoading.Image = global::Vincontrol.Vinsell.WindesktopVersion.Properties.Resources.ajax_loader_mainform;
            this.pbxLoading.Location = new System.Drawing.Point(373, 149);
            this.pbxLoading.Name = "pbxLoading";
            this.pbxLoading.Size = new System.Drawing.Size(102, 103);
            this.pbxLoading.TabIndex = 6;
            this.pbxLoading.TabStop = false;
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlFilter.Controls.Add(this.tableLayoutPanel2);
            this.pnlFilter.Controls.Add(this.label1);
            this.pnlFilter.Controls.Add(this.cbbTrim);
            this.pnlFilter.Controls.Add(this.cbbRegion);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(3, 3);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(911, 91);
            this.pnlFilter.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(312, 18);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(323, 27);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel3.Controls.Add(this.lblPriceBelow);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(106, 21);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Green;
            this.panel4.Controls.Add(this.lblPriceAverage);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(115, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(106, 21);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DarkRed;
            this.panel5.Controls.Add(this.lblPriceAbove);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(227, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(93, 21);
            this.panel5.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(435, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reported Wholesale Auction Sales - With Exact Matches";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pnlFilter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(917, 547);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pbxLoading);
            this.panel2.Controls.Add(this.dgManheimDetail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(911, 444);
            this.panel2.TabIndex = 8;
            // 
            // ManheimDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 547);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManheimDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manheim Detail";
            ((System.ComponentModel.ISupportInitialize)(this.dgManheimDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbTrim;
        private System.Windows.Forms.Label lblPriceBelow;
        private System.Windows.Forms.Label lblPriceAverage;
        private System.Windows.Forms.Label lblPriceAbove;
        private System.Windows.Forms.DataGridView dgManheimDetail;
        private System.Windows.Forms.ComboBox cbbRegion;
        private System.Windows.Forms.PictureBox pbxLoading;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn Odometer;
        private DataGridViewTextBoxColumn Price;
        private DataGridViewTextBoxColumn SaleDate;
        private DataGridViewTextBoxColumn Auction;
        private DataGridViewTextBoxColumn Engine;
        private DataGridViewTextBoxColumn TR;
        private DataGridViewTextBoxColumn Cond;
        private DataGridViewTextBoxColumn Color;
    }
}