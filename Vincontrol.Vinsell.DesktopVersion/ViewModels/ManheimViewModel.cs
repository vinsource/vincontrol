using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using vincontrol.CarFax;
using vincontrol.Helper;
using Vincontrol.Vinsell.DesktopVersion.Helpers;
using Vincontrol.Vinsell.DesktopVersion.Models;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.DomainObject;
//using vincontrol.Helper;
using vincontrol.WPF.Helpers.Interface;
using DataHelper = Vincontrol.Vinsell.DesktopVersion.Helpers.DataHelper;


//using DataHelper = Vincontrol.Vinsell.DesktopVersion.Helpers.DataHelper;


namespace Vincontrol.Vinsell.DesktopVersion.ViewModels
{
    public class ManheimViewModel
    {
        public KBBModel KBBModel { get; set; }
        public CarfaxModel CarfaxModel { get; set; }
        public MainheimModel MainheimModel { get; set; }
        public MarketModel MarketModel { get; set; }
        private IAuctionManagement _manheimAuctionManagement;
        private string _vin;
        private DealerUser _dealer;

        public ManheimViewModel(IView view)
        {
            KBBModel = new KBBModel();
            CarfaxModel = new CarfaxModel();
            MainheimModel = new MainheimModel();
            MarketModel = new MarketModel();
            Initializer();
            view.SetDataContext(this);
        }

        private void Initializer()
        {
            _manheimAuctionManagement = new AuctionManagement();
        }

        public void LoadData(string vin, DealerUser dealer)
        {
            _vin = vin;
            _dealer = dealer;
            LoadData(LoadCarfaxWorkertranactionDoWork);
            LoadData(LoadMarketWorkertranactionDoWork);
            LoadData(LoadKarpowerWorkertranactionDoWork);
            LoadData(LoadManheimWorkertranactionDoWork);
        }

        private void LoadData(DoWorkEventHandler doWorkFunction)
        {
            var loadCarfaxWorkertranaction = new BackgroundWorker();
            loadCarfaxWorkertranaction.DoWork += doWorkFunction;
            loadCarfaxWorkertranaction.RunWorkerAsync();
        }

        void LoadMarketWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            var marketData = DataHelper.MarketData(_vin, _manheimAuctionManagement);
            if (marketData != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MarketModel.AveragePrice = marketData.Market.AveragePrice;
                        MarketModel.MaximumPrice = marketData.Market.MaximumPrice;
                        MarketModel.MinimumPrice = marketData.Market.MinimumPrice;
                        MarketModel.IsFinishedLoading = true;
                    }
                                                               ));
            }

        }

        void LoadManheimWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            var manheimData = DataHelper.ManheimData(_vin, _dealer.DealerSetting.MainheimUserName, _dealer.DealerSetting.MainheimPassword, _manheimAuctionManagement);

            if (manheimData == null || manheimData.Count <= 0) return;
            var firstItem = manheimData[0];
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainheimModel.AveragePrice = firstItem.AveragePrice.ToString("CO");
                    MainheimModel.HighestPrice = firstItem.HighestPrice.ToString("CO");
                    MainheimModel.LowestPrice = firstItem.LowestPrice.ToString("CO");
                    MainheimModel.IsFinishedLoading = true;
                }));
        }

        private void LoadKarpowerWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            var kbbData = DataHelper.KarpowerData(_vin, "0", _dealer.DealerSetting.KBBUserName,
                                                  _dealer.DealerSetting.KBBPassword, _dealer.DealerId);
            if (kbbData == null || kbbData.Count <= 0) return;
            var firstItem = kbbData[0];
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                   {
                       KBBModel.BaseWholesale = firstItem.BaseWholesale.ToString();
                       KBBModel.MileageAdjustment = firstItem.MileageAdjustment.ToString();
                       KBBModel.Wholesale = firstItem.Wholesale.ToString();
                       KBBModel.IsFinishedLoading = true;
                   }));
        }

        void LoadCarfaxWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            CarFaxService carFaxService = new CarFaxService();
            var carFaxData = carFaxService.XmlSerializeCarFax(_vin, _dealer.DealerSetting.CarFaxUsername, _dealer.DealerSetting.CarFaxPassword);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CarfaxModel.NumberofOwners = carFaxData.NumberofOwners;
                    CarfaxModel.ServiceRecords = carFaxData.ServiceRecords.ToString();
                    CarfaxModel.IsFinishedLoading = true;
                }));
        }

    }
}
