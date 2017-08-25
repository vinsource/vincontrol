using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Windows.Forms;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Model;
using vincontrol.Manheim;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using Vincontrol.Vinsell.WindesktopVersion.Models;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;
using vincontrol.Helper;
using System.Net;
using System.Text.RegularExpressions;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class ManheimForm : Form
    {
        private DealerUser _dealer;
        private string _vin;
        //private bool _isFavorite;
        //private string _note;
        private ExtractedManheimVehicle _manheimVehicle;
        private NoteFavouriteInfo _noteFavourite;
        private IAuctionManagement _manheimAuctionManagement;
        private ChartGraph _marketData;
        private List<SmallKarPowerViewModel> _kbbData;
        private List<ManheimWholesaleViewModel> _manheimData;
        private CarFax _carFaxData;

        #region show loading
        public void ShowManheimLoading()
        {
            //pnlManheim.Enabled = false;
            pbManheimLoading.Visible = true;
        }

        public void HideManheimLoading()
        {
            //pnlManheim.Enabled = true;
            pbManheimLoading.Visible = false;
        }

        public void ShowKbbLoading()
        {
            //pnlKBB.Enabled = false;
            pbKBBLoading.Visible = true;
        }

        public void HideKbbLoading()
        {
            //pnlKBB.Enabled = true;
            pbKBBLoading.Visible = false;
        }

        public void ShowMarketLoading()
        {
            //pnlMarket.Enabled = false;
            pbMarketLoading.Visible = true;
        }

        public void HideMarketLoading()
        {
            //pnlMarket.Enabled = true;
            pbMarketLoading.Visible = false;

        }

        public void ShowNoteFavouriteLoading()
        {
            //pnlFavAndNote.Enabled = false;
            rbNote.Enabled = false;
        }

        public void HideNoteFavouriteLoading()
        {
            //pnlFavAndNote.Enabled = true;
            rbNote.Enabled = true;
        }

        public void ShowCarfaxLoading()
        {
            //pnlCARFAXNo.Enabled = false;
            //pnlAccident.Enabled = false;
            pbCARFAXLoading.Visible = true;
        }

        public void HideCarfaxLoading()
        {
            //pnlCARFAXNo.Enabled = true;
            //pnlAccident.Enabled = true;
            pbCARFAXLoading.Visible = false;
        }

        public void ShowBrowserLoading()
        {
            //btnForward.Enabled = false;
            //btnBack.Enabled = false;
            //manheimBrowser.Enabled = false;
            pbBrowserLoading.Visible = true;
        }

        public void HideBrowserLoading()
        {
            //btnForward.Enabled = true;
            //btnBack.Enabled = true;
            //manheimBrowser.Enabled = true;
            pbBrowserLoading.Visible = false;
        }
        #endregion

        public ManheimForm()
        {
           
            InitializeComponent();
          
            tableLayoutPanel7.CellPaint += tableLayoutPanel_CellPaint;
            tableLayoutPanel10.CellPaint += tableLayoutPanel_CellPaint;
            tableLayoutPanel13.CellPaint += tableLayoutPanel_CellPaint;
        }

        private void Form1Load(object sender, EventArgs e)
        {
            _manheimAuctionManagement = new AuctionManagement();
            _dealer = SessionVar.CurrentDealer;
            manheimBrowser.AllowNewWindows = true;
            manheimBrowser.AllowNavigation = true;

            //webKitBrowser
            manheimBrowser.NewWindowCreated += manheimBrowser_NewWindowCreated;
            manheimBrowser.DocumentText = OpenManaheimLoginWindow();
            manheimBrowser.Navigating += manheimBrowser_Navigating;
            manheimBrowser.Navigated += manheimBrowser_Navigated;
            manheimBrowser.DocumentCompleted += manheimBrowser_DocumentCompleted;
            manheimBrowser.Error += manheimBrowser_Error;

        }

        void manheimBrowser_Error(object sender, WebKit.WebKitBrowserErrorEventArgs e)
        {

            BindData(_vin);
        }

        void manheimBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            ShowBrowserLoading();
            var url = ((WebKit.WebKitBrowser)(sender)).Url != null ? ((WebKit.WebKitBrowser)(sender)).Url.AbsoluteUri : string.Empty;
            if (url.IndexOf("simulcast.manheim.com") > 0)
            {
                manheimBrowser.IsScriptingEnabled = true;
                manheimBrowser.IsWebBrowserContextMenuEnabled = true;
                manheimBrowser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                //manheimBrowser.Navigate(url);
            }
        }

        void manheimBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (e.Url != null)
                {
                    _vin = HttpUtility.ParseQueryString(e.Url.Query).Get("vin");
                    BindData(_vin);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            HideBrowserLoading();
        }

        private void BindData(string _vin)
        {
          

            if (String.IsNullOrEmpty(_vin))
            {
                RefreshKbbContent();
                RefreshManheimContent();
                RefreshMarketContent();
                RefreshNoteAndMarket();
            }
            else
            {
                _manheimVehicle = WebDataExtractHelper.ExtractFromManheimProfile(manheimBrowser.DocumentText);
                ShowKbbLoading();
                ShowManheimLoading();
                ShowMarketLoading();

                LoadData(LoadKarpowerWorkertranactionDoWork, LoadKarpowerWorkertranactionFinsihed);
                LoadData(LoadManheimWorkertranactionDoWork, LoadManheimWorkertranactionFinsihed);
                LoadData(LoadMarketWorkertranactionDoWork, LoadMarketWorkertranactionFinsihed);
             
            }


        }

        public string OpenManaheimLoginWindow()
        {
            var encode = System.Text.Encoding.GetEncoding("utf-8");

            const string manheimLink = "https://www.manheim.com/members/presale/control/?WT.svl=m_hdr_mnav_buy_presale";

            var myRequest = (HttpWebRequest)WebRequest.Create(manheimLink);

            myRequest.Method = "GET";

            var myResponse = myRequest.GetResponse();

            var receiveStream = myResponse.GetResponseStream();

            if (receiveStream != null)
            {
                var readStream = new StreamReader(receiveStream, encode);

                string result = "";
                string line;
                while ((line = readStream.ReadLine()) != null)
                {
                    if (line.Contains("stylesheets"))

                        line = line.Replace("href=\"/stylesheets", "href=\"https://www.manheim.com/stylesheets");
                    else if (line.Contains("javascripts"))
                        line = line.Replace("src=\"/javascripts", "src=\"https://www.manheim.com/javascripts");
                    else if (line.Contains("form accept-charset=\"UTF-8\" action"))
                        line = line.Replace("action=\"/login/authenticate\"", "action=\"https://www.manheim.com/login/authenticate\"");
                    else
                    {
                        if (!line.Contains("https://www.manheim.com"))
                            line = line.Replace("href=\"", "href=\"https://www.manheim.com");
                    }

                    //FINAL LOGIN
                    if (line.Contains("Username:"))
                        line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + SessionVar.CurrentDealer.DealerSetting.MainheimUserName + "\" />" + Environment.NewLine;

                    if (line.Contains("Password:"))

                        line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + SessionVar.CurrentDealer.DealerSetting.MainheimPassword + "\" />" + Environment.NewLine;

                    if (line.Contains("</body>"))
                    {
                        line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                    }

                    result += line;
                }

                return result;
            }
            return null;
        }

        void manheimBrowser_NewWindowCreated(object sender, WebKit.NewWindowCreatedEventArgs e)
        {
            var manheimService = new ManheimService();
            var simulcastUrl = ((WebKit.WebKitBrowser)(sender)).Url.AbsoluteUri;
            if (simulcastUrl.IndexOf("simulcast.manheim.com") > 0)
            {
                var simulcastInstallationUrl = "https://chrome.google.com/webstore/detail/manheim-media-player/ocdfcabeedcfbaoabffcbecdjdnepgcl";
                var url = "https://simulcast.manheim.com/simulcast/initBuyerAuction.do?vehicleGroupKey=a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43&vehicleGroupKey=a:CADE_s:76108_c:OPEN_l:1_v:1_q:76-900&vehicleGroupKey=a:CADE_s:76108_c:REDL_l:1_v:1_q:44-75&vehicleGroupKey=a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900&locale=en_US&isManheimAVPluginInstalled=false&CLIENT_TYPE=LIBERATOR";
                
                manheimService.LogOn(SessionVar.CurrentDealer.DealerSetting.MainheimUserName, SessionVar.CurrentDealer.DealerSetting.MainheimPassword);
                manheimService.WebRequestGet(simulcastUrl);
                try
                {
                    manheimService.PostSimulcastData(simulcastUrl, new SimulcastContract()
                    {
                        vehicleGroupGoto = "a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43",
                        isManheimAVPluginInstalled = "false",
                        saleEventKey = "CADE_76108_01",
                        vehicleGroupKey = "a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900",
                        dealerships = "5131094,a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43,a:CADE_s:76108_c:OPEN_l:1_v:1_q:76-900,a:CADE_s:76108_c:REDL_l:1_v:1_q:44-75,a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900",
                        initalDealer = "CADE_s:76108_c:OPEN_l:1_v:1_q:1-43,a:CADE_s:76108_c:OPEN_l:1_v:1_q:76-900,a:CADE_s:76108_c:REDL_l:1_v:1_q:44-75,a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900",
                        email = "sbrown@jlr-mv.com",
                        cellphoneNPA = "714",
                        cellphoneNXX = "348",
                        cellphoneStationCode = "8351",
                        faxNPA = "714",
                        faxNXX = "242",
                        faxStationCode = "1875",
                        paymentMethod = "CHECK",
                        postSaleInspection = "7",
                        title = "LOT",
                        transportation = "DEALER",
                        transportContactName = "al american transport",
                        transportNPA = "714",
                        transportNXX = "400",
                        transportStationCode = "7057",
                        confirmPreferences = "on"
                    });

                    url = manheimService.GetSimulcastUrl(manheimService.Result);
                }
                catch (Exception)
                {

                }

                manheimBrowser.IsScriptingEnabled = true;
                manheimBrowser.IsWebBrowserContextMenuEnabled = true;
                manheimBrowser.Navigate(url);
            }
            else if (simulcastUrl.IndexOf("www.manheim.com/members/powersearch/redirect.do") > 0)
            {
                manheimService.LogOn(SessionVar.CurrentDealer.DealerSetting.MainheimUserName, SessionVar.CurrentDealer.DealerSetting.MainheimPassword);
                var content = manheimService.WebRequestGet(simulcastUrl);
                var nextUrlPattern = "onclick=\"javascript:window.open([^\\\"]*)\"";//"<input onclick=\"([^\\\"]*)\" onmousedown=\"([^\\\"]*)\" name=\"Enter Simulcast Sale\" type=\"button\" value=\"Enter Sale\" class=\"btnInput btnPrimary\" />";
                var authTokenPattern = new Regex(nextUrlPattern);
                var nextUrl = authTokenPattern.Matches(content)[1].Value;
                nextUrl = nextUrl.Replace("(", "").Replace(")", "").Replace("\"", "").Replace("'", "").Replace(";", "").Replace(",", "").Replace("onclick=", "").Replace("javascript:window.open", "").Replace("simulcastDetail", "").Replace("return false", "");
                manheimBrowser.IsScriptingEnabled = true;
                manheimBrowser.IsWebBrowserContextMenuEnabled = true;
                manheimBrowser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                manheimBrowser.Navigate(nextUrl);
            }
            
        }

        void manheimBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            try
            {
                if (e.Url != null)
                {
                    _vin = HttpUtility.ParseQueryString(e.Url.Query).Get("vin");
                    if (String.IsNullOrEmpty(_vin))
                    {
                        RefreshData();
                        btnPrint.Enabled = false;
                    }
                    else
                    {
                        LoadData(_vin, _dealer);
                        btnPrint.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void RefreshData()
        {
            //RefreshMarketContent();
            //RefreshManheimContent();
            //RefreshKbbContent();
            RefreshCarfaxContent();
        }

        private void RefreshCarfaxContent()
        {
            labelNoOfOwner.Text = String.Empty;
            labelNoOfServiceRecords.Text = String.Empty;
            pnlAcccidentCount.Visible = false;
        }

        private void RefreshMarketContent()
        {
            btnCarMarketNo.Text = String.Empty;
            lblMarketAbove.Text = String.Empty;
            lblMarketAvg.Text = String.Empty;
            lblMarketBelow.Text = String.Empty;

        }

        private void RefreshNoteAndMarket()
        {
            rbNote.Text = String.Empty;
        }

        private void RefreshManheimContent()
        {
            cbbManheimTrims.Text = "";
            cbbManheimTrims.Items.Clear();
            lblManheimAbove.Text = String.Empty;
            lblManheimAvg.Text = String.Empty;
            lblManheimBelow.Text = String.Empty;


        }

        private void RefreshKbbContent()
        {
            cbbKBBTrims.Text = "";
            cbbKBBTrims.Items.Clear();

            lblKBBAdj.Text = String.Empty;
            lblKBBValue.Text = String.Empty;
            lblKBBAvg.Text = String.Empty;
            lblKbbNum.Text = String.Empty;


        }

        private void tableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row == 0 || e.Row == 1 || e.Row == 2)
            {
                var rectangle = e.CellBounds;
                rectangle.Inflate(-1, -1);

                ControlPaint.DrawBorder(e.Graphics, rectangle, System.Drawing.Color.DarkBlue, ButtonBorderStyle.Dotted); // dotted border
            }
        }

        private void MarketPanelClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_vin))
            {
                var marketForm = new MarketForm(_vin, _manheimVehicle);
                marketForm.Show();
                marketForm.Activate();
            }
        }

        private void KbbPanelClick(object sender, EventArgs e)
        {
            if (cbbKBBTrims.Items.Count > 0)
            {
                var selectedTrim = (TrimItem)cbbKBBTrims.SelectedItem;

                var kbbForm = new KbbForm(_vin, _manheimVehicle.Odometer, selectedTrim.TrimID);
                kbbForm.Show();
                kbbForm.Activate();
            }
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            manheimBrowser.GoBack();

        }

        private void BtnForwardClick(object sender, EventArgs e)
        {
            manheimBrowser.GoForward();

        }

        #region Data

        public void LoadData(string vin, DealerUser dealer)
        {
            ShowCarfaxLoading();
            ShowNoteFavouriteLoading();
            LoadData(LoadFavouriteAndNoteDoWork, LoadFavouriteAndNoteFinished);
            LoadData(LoadCarfaxWorkertranactionDoWork, LoadCarfaxWorkertranactionFinsihed);
            //LoadData(LoadMarketWorkertranactionDoWork, LoadMarketWorkertranactionFinsihed);
            //LoadData(LoadManheimWorkertranactionDoWork, LoadManheimWorkertranactionFinsihed);
        }

        private void LoadFavouriteAndNoteFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            _noteFavourite = (NoteFavouriteInfo)e.Result;
            btnAddToFav.Image = !_noteFavourite.IsFavourite ? Properties.Resources.add_favorite : Properties.Resources.remove_favorite;
            rbNote.Text = _noteFavourite.Note;
            HideNoteFavouriteLoading();
        }

        private void LoadFavouriteAndNoteDoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = new NoteFavouriteInfo { IsFavourite = _manheimAuctionManagement.IsFavorite(0, _dealer.DealerId, _dealer.DealerId), Note = _manheimAuctionManagement.GetNote(0, _dealer.DealerId, _dealer.DealerId) };
        }

        private void LoadData(DoWorkEventHandler doWorkFunction, RunWorkerCompletedEventHandler doWorkFinishedFunction)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += doWorkFunction;
            worker.RunWorkerCompleted += doWorkFinishedFunction;
            worker.RunWorkerAsync();
        }

        private void LoadMarketWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
          
            var vehicle = new VehicleViewModel()
            {
                Mileage = _manheimVehicle.Odometer,
                Year = _manheimVehicle.Year,
                Make = _manheimVehicle.Make,
                Model = _manheimVehicle.Model,
                Trim = _manheimVehicle.Trim,
                Vin = _manheimVehicle.Vin,
                Mmr = _manheimVehicle.Price,
                DealerId = _dealer.DealerId

            };
            try
            {
                using (var context = new VincontrolEntities())
                {
                    var queryDealer = context.Dealers.FirstOrDefault(d => d.DealerId == vehicle.DealerId);

                    var dealer = new DealershipViewModel(queryDealer);

                    if (!String.IsNullOrEmpty(vehicle.Vin))
                    {
                        var autoService = new ChromeAutoService();

                        var vehicleInfo = autoService.GetVehicleInformationFromVin(vehicle.Vin);

                        if (vehicleInfo != null)
                        {

                            vehicle.Make = vehicleInfo.bestMakeName;

                            vehicle.Year = vehicleInfo.modelYear;

                            vehicle.Model = vehicleInfo.bestModelName;

                            if (!String.IsNullOrEmpty(vehicleInfo.bestTrimName) &&
                                !vehicleInfo.bestTrimName.Contains("/"))
                                vehicle.Trim = vehicleInfo.bestTrimName;

                            if (!String.IsNullOrEmpty(vehicleInfo.vinDescription.bodyType))
                            {
                                vehicle.BodyStyle = vehicleInfo.vinDescription.bodyType;
                            }
                        }
                    }
                    _marketData = MarketHelper.GetMarketCarsForNationwideMarketOnVinsell(vehicle, dealer);

                }
            }
            catch (Exception ex)
            {
                _marketData.Error = ex.Message;
            }

        }

        void LoadMarketWorkertranactionFinsihed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_marketData == null || _marketData.ChartModels == null)
            {
                RefreshMarketContent();
            }
            else
            {
                SessionVar.MemoryChartGraph = _marketData;

                MarketHelper.SetValuesOnMarketToCarList(_marketData,
                    _marketData.TypedChartModels.Where(x => x.Distance <= 100).ToList());

                btnCarMarketNo.Text = string.Format("{0} cars on market",
                                                    _marketData.Market.CarsOnMarket.ToString(
                                                        CultureInfo.InvariantCulture));
                lblMarketAvg.Text = _marketData.Market.AveragePrice.ToString("c0");
                lblMarketAbove.Text = _marketData.Market.MaximumPrice.ToString("c0");
                lblMarketBelow.Text = _marketData.Market.MinimumPrice.ToString("c0");
            }
            HideMarketLoading();
        }

        void LoadManheimWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            _manheimData = new List<ManheimWholesaleViewModel>();
            if (!string.IsNullOrEmpty(_vin))
            {
                var model = _manheimAuctionManagement.GetVehicle(_vin);

                if (model.Mmr > 0 && model.MmrAbove > 0 && model.MmrBelow > 0)
                {
                    var newRecord = new ManheimWholesaleViewModel
                        {
                            LowestPrice =Convert.ToDecimal(model.MmrBelow),
                            AveragePrice =Convert.ToDecimal(model.Mmr),
                            HighestPrice =Convert.ToDecimal(model.MmrAbove),
                            Year = model.Year,
                            TrimName = model.Trim
                        };
                    _manheimData.Add(newRecord);
                }
                else
                {
                    var client = new WebClient();
                    client.Headers["Content-type"] = "application/json";

                    // invoke the REST method
                    byte[] data = client.DownloadData(string.Format("http://www.carslisting.us:3311/Vinsell/ManheimData/{0}/{1}/{2}",
                              _vin, _manheimVehicle.Odometer, _dealer.DealerId));

                    // put the downloaded data in a memory stream
                    var ms = new MemoryStream(data);

                    // deserialize from json
                    var ser =
                        new DataContractJsonSerializer(typeof(List<ManheimWholesaleViewModel>));

                    _manheimData = ser.ReadObject(ms) as List<ManheimWholesaleViewModel>;

                }
            }


         
        }

        void LoadManheimWorkertranactionFinsihed(object sender, RunWorkerCompletedEventArgs e)
        {

            if (_manheimData == null || _manheimData.Count <= 0)
            {
                RefreshManheimContent();
            }
            else
            {
                var firstItem = _manheimData[0];

                var trimList =
                    _manheimData.Select(
                        data =>
                        new TrimItem
                        {
                            TrimID = data.TrimServiceId.ToString(CultureInfo.InvariantCulture),
                            TrimName = data.TrimName
                        }).ToList();

                foreach (var trimItem in trimList)
                {
                    cbbManheimTrims.Items.Add(trimItem);
                }




                if (cbbManheimTrims.Items.Count > 0)
                    cbbManheimTrims.SelectedIndex = 0;


                lblManheimAbove.Text = firstItem.HighestPrice.ToString();
                lblManheimAvg.Text = firstItem.AveragePrice.ToString();
                lblManheimBelow.Text = firstItem.LowestPrice.ToString();
            }
            HideManheimLoading();
        }

        private void LoadKarpowerWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {

            var client = new WebClient();
            client.Headers["Content-type"] = "application/json";

            // invoke the REST method
            byte[] data = client.DownloadData(string.Format("http://www.carslisting.us:3311/Vinsell/KarPowerData/{0}/{1}/{2}",
                    _vin, _manheimVehicle.Odometer, _dealer.DealerId));

            // put the downloaded data in a memory stream
            var ms = new MemoryStream(data);

            // deserialize from json
            var ser =
                new DataContractJsonSerializer(typeof(List<SmallKarPowerViewModel>));

            _kbbData = ser.ReadObject(ms) as List<SmallKarPowerViewModel>;

           

        }

        private void LoadKarpowerWorkertranactionFinsihed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_kbbData == null || _kbbData.Count <= 0)
            {
                RefreshKbbContent();
            }
            else
            {
                var firstItem = _kbbData[0];

                var trimList =
                    _kbbData.Select(
                        data =>
                        new TrimItem
                        {
                            TrimID = data.SelectedTrimId.ToString(CultureInfo.InvariantCulture),
                            TrimName = data.SelectedTrimName
                        }).ToList();

                foreach (var trimItem in trimList)
                {
                    cbbKBBTrims.Items.Add(trimItem);
                }




                if (cbbKBBTrims.Items.Count > 0)
                    cbbKBBTrims.SelectedIndex = 0;

                lblKbbNum.Text = "(" + cbbKBBTrims.Items.Count + ")";
                kbbPanel.Enabled = true;
                lblKBBValue.Text = firstItem.Wholesale.ToString();
                lblKBBAdj.Text = firstItem.MileageAdjustment.ToString();
                lblKBBAvg.Text = firstItem.BaseWholesale.ToString();
            }

            HideKbbLoading();
        }

        void LoadCarfaxWorkertranactionDoWork(object sender, DoWorkEventArgs e)
        {
            _carFaxData = Vincontrol.Vinsell.WindesktopVersion.Helper.DataHelper.GetCarfaxFromWebService(_dealer.DealerId, _vin);

        }

        void LoadCarfaxWorkertranactionFinsihed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_carFaxData != null)
            {
                labelNoOfOwner.Text = string.Format("{0} Owner(s)", String.IsNullOrEmpty(_carFaxData.Owner) ? "0" : _carFaxData.Owner);
                labelNoOfServiceRecords.Text = string.Format("{0} Service Records", String.IsNullOrEmpty(_carFaxData.Service) ? "0" : _carFaxData.Service);
                if (_carFaxData.AccidentCounts > 0)
                {
                    pnlAcccidentCount.Visible = true;
                }
            }
            else
            {
                RefreshCarfaxContent();
            }
            HideCarfaxLoading();
        }
        #endregion

        private void cbbManheimTrims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_manheimData != null)
            {
                if (cbbManheimTrims.SelectedItem != null)
                {
                    SetManheimData(
                        _manheimData.FirstOrDefault(
                            i => i.TrimServiceId.ToString(CultureInfo.InvariantCulture).Equals(((TrimItem)cbbManheimTrims.SelectedItem).TrimID)));
                }
            }
        }

        private void SetManheimData(ManheimWholesaleViewModel firstItem)
        {
            if (firstItem != null)
            {
                lblManheimAbove.Text = firstItem.HighestPrice.ToString();
                lblManheimAvg.Text = firstItem.AveragePrice.ToString();
                lblManheimBelow.Text = firstItem.LowestPrice.ToString();
            }
        }

        private void cbbKBBTrims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_kbbData != null)
            {
                if (cbbKBBTrims.SelectedItem != null)
                {
                    SetKbbData(
                        _kbbData.FirstOrDefault(
                            i => i.SelectedTrimId.ToString(CultureInfo.InvariantCulture).Equals(((TrimItem)cbbKBBTrims.SelectedItem).TrimID)));
                }
            }
        }

        private void SetKbbData(SmallKarPowerViewModel firstItem)
        {
            if (firstItem != null)
            {
                lblKBBAvg.Text = firstItem.Wholesale.ToString();
                lblKBBAdj.Text = firstItem.MileageAdjustment.ToString();
                lblKBBValue.Text = firstItem.BaseWholesale.ToString();
            }
        }

        private void btnAddToFav_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_vin))
            {
                _manheimAuctionManagement.MarkFavorite(0, SessionVar.CurrentDealer.DealerId,
                                                       SessionVar.CurrentDealer.DealerId);
                if (_noteFavourite.IsFavourite)
                {
                    MessageBox.Show("You have removed the car to your favourite list.");
                    btnAddToFav.Image = Properties.Resources.add_favorite;
                }
                else
                {
                    MessageBox.Show("You have added the car to your favourite list.");
                    btnAddToFav.Image = Properties.Resources.remove_favorite;

                }
            }

        }

        private void btnAddToNote_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_vin))
            {
                IAuctionManagement manheimAuctionManagement = new AuctionManagement();
                manheimAuctionManagement.MarkNote(0, rbNote.Text, SessionVar.CurrentDealer.DealerId,
                                                  SessionVar.CurrentDealer.DealerId);
                MessageBox.Show("You have successfully added the note.");
            }
        }

        private void carfaxPanel_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_vin))
            {
                var carfaxForm = new CarfaxForm(_vin);
                carfaxForm.Show();
                carfaxForm.Activate();
            }

        }

        private void pnManheim_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_vin))
            {
                var manheimForm = new ManheimDetailForm(_vin, _manheimVehicle.Year, _manheimVehicle.Make, _manheimVehicle.Model);
                manheimForm.Show();
            }
        }

        private void lblNotes_Click(object sender, EventArgs e)
        {
            var form = new FavouriteVehicleForm(false);
            form.Show();
        }

        private void lblFavourites_Click(object sender, EventArgs e)
        {
            var form = new FavouriteVehicleForm(true);
            form.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var token = EncryptionHelper.EncryptString(String.Format("{0}|{1}", SessionVar.CurrentDealer.DealerId,
                                                          SessionVar.CurrentDealer.DealerName));
            var printForm = new PrintForm(token, _vin, cbbManheimTrims.SelectedItem == null ? String.Empty : ((TrimItem)cbbManheimTrims.SelectedItem).TrimID, cbbKBBTrims.SelectedItem==null?String.Empty:((TrimItem)cbbKBBTrims.SelectedItem).TrimID);
            printForm.Show();
        }

    }

    public class TrimItem
    {
        public string TrimName { get; set; }
        public string TrimID { get; set; }
        public override string ToString()
        {
            return TrimName;
        }
    }

    public class NoteFavouriteInfo
    {
        public string Note { get; set; }
        public bool IsFavourite { get; set; }
    }
}
