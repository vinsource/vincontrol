using System;
using System.Drawing;
using System.Windows.Forms;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class StartLoginForm : Form
    {
        public StartLoginForm()
        {
            InitializeComponent();
            tableLayoutPanel1.CellPaint += tableLayoutPanel_CellPaint;
            this.Load += new EventHandler(StartLoginForm_Load);
        }

        void StartLoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text=Properties.Settings.Default.Username;
            txtPassword.Text=Properties.Settings.Default.Password;
            cbRemember.Checked=Properties.Settings.Default.IsRemember;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var dealerUser = UserHelper.CheckUserExistWithStatus(txtUsername.Text, txtPassword.Text);
            if (dealerUser == null)
            {

                MessageBox.Show("Incorrect user name or password.", "Critical Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            else
            {
                if (cbRemember.Checked)
                {
                    Properties.Settings.Default.Username = txtUsername.Text;
                    Properties.Settings.Default.Password = txtPassword.Text;
                    Properties.Settings.Default.IsRemember = cbRemember.Checked;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Username = "";
                    Properties.Settings.Default.Password = "";
                    Properties.Settings.Default.IsRemember = cbRemember.Checked;
                    Properties.Settings.Default.Save();
                }
                SessionVar.CurrentDealer = dealerUser;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }



        //private void cbRemember_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (cbRemember.Checked)
        //    {
        //        Properties.Settings.Default.Username = txtUsername.Text;
        //        Properties.Settings.Default.Password = txtPassword.Text;
        //        Properties.Settings.Default.Save();
        //    }
        //    else
        //    {
        //        Properties.Settings.Default.Username = "";
        //        Properties.Settings.Default.Password = "";
        //        Properties.Settings.Default.Save();
        //    }
        //}

        private void tableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row == 0 || e.Row == 1 || e.Row == 2)
            {
                var rectangle = e.CellBounds;
                rectangle.Inflate(-1, -1);


                ControlPaint.DrawBorder(e.Graphics, rectangle, Color.Blue, ButtonBorderStyle.Solid); // dotted border
            }




        }

        private void btnCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
