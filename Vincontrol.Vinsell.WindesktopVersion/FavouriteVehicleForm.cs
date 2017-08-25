using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vincontrol.Vinsell.WPFLibrary;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using System.Windows.Forms.Integration;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class FavouriteVehicleForm : Form
    {
        private IAuctionManagement _manheimAuctionManagement;
        private readonly bool _isFavourite;
        private IList<VehicleViewModel> _data;

        public FavouriteVehicleForm(bool isFavourite)
        {
            _isFavourite = isFavourite;
            InitializeComponent();
            ShowLoading();
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //var wpfctl = new GroupGridView(_data);
           
            ////wpfctl.Height = 300;
            ////elementHost.Width = 1104;
            ////elementHost.Height = 574;
            ////elementHost.Width = 500;
            ////elementHost.Height = 200;
            ////elementHost.Child = wpfctl;
            
            //elementHost.Child = wpfctl;
            //elementHost.Dock = DockStyle.Fill;
            //HideLoading();
            //ElementHost host = new ElementHost()
            //{
            //    //BackColor = Color.Transparent,
            //    Child = wpfctl,
            //    Dock = DockStyle.Fill,
            //};

            ///* now add the ElementHost to our controls collection 
            // * normally */
            //panelLayout.a .Add(host);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _manheimAuctionManagement = new AuctionManagement();
            _data = _isFavourite ? _manheimAuctionManagement.GetFavoriteVehicles(SessionVar.CurrentDealer.DealerId, SessionVar.CurrentDealer.DealerId) : _manheimAuctionManagement.GetNotedVehicles(SessionVar.CurrentDealer.DealerId, SessionVar.CurrentDealer.DealerId);
        }

        private void ShowLoading()
        {
            pbxLoading.Visible = true;
            elementHost.Enabled = false;
        }

        private void HideLoading()
        {
            pbxLoading.Visible = false;
            elementHost.Enabled = true;
        }
    }
}
