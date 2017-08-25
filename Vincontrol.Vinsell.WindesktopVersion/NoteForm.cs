using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using vincontrol.Application.Forms.ManheimAuctionManagement;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class NoteForm : Form
    {
        private string _vin;
        public NoteForm(string vin, string note)
        {
            InitializeComponent();
            _vin = vin;
            txtNote.Text = note;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IAuctionManagement manheimAuctionManagement = new AuctionManagement();
            manheimAuctionManagement.MarkNote(0, txtNote.Text, SessionVar.CurrentDealer.DealerId, SessionVar.CurrentDealer.DealerId);
            MessageBox.Show("You have successfully added the note.");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
