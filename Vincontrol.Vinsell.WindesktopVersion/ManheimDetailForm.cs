using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using vincontrol.Manheim;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class ManheimDetailForm : Form
    {
        private List<ManheimWholesaleViewModel> _manheimData;
        private readonly VehicleViewModel _vehicleLookUp; 

        #region Background
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (SessionVar.CurrentVehiceViewModel == null)
            {
                SessionVar.CurrentVehiceViewModel = new VehicleViewModel()
                    {
                        Vin = _vehicleLookUp.Vin,
                        ManheimWholesale = _manheimData,
                        ManheimTransactions = TrimInit()
                        
                    };
                e.Result = SessionVar.CurrentVehiceViewModel.ManheimTransactions;
            }
            else
            {
                if (_vehicleLookUp.Vin.Equals(SessionVar.CurrentVehiceViewModel.Vin))
                {
                    e.Result = SessionVar.CurrentVehiceViewModel.ManheimTransactions;
                }
                else
                {
                    SessionVar.CurrentVehiceViewModel = new VehicleViewModel()
                    {
                        Vin = _vehicleLookUp.Vin,
                        ManheimWholesale = _manheimData,
                        ManheimTransactions = TrimInit()

                    };
                    e.Result = SessionVar.CurrentVehiceViewModel.ManheimTransactions;
                }
            }
          
            
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgManheimDetail.DataSource = e.Result;
            HideLoading();
        }

        void trimChangeworker_DoWork(object sender, DoWorkEventArgs e)
        {
            TrimChange();
        }

        void regionChangeworker_DoWork(object sender, DoWorkEventArgs e)
        {
            RegionChange();
        }


        #endregion

        #region Event Handlings
        void cbbTrim_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ShowLoading();
            var trimChangeworker = new BackgroundWorker();
            trimChangeworker.DoWork += trimChangeworker_DoWork;
            trimChangeworker.RunWorkerAsync();
            //TrimChange();
        }

        void cbbRegion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ShowLoading();
            var regionChangeworker = new BackgroundWorker();
            regionChangeworker.DoWork += regionChangeworker_DoWork;
            regionChangeworker.RunWorkerAsync();
            //RegionChange();
        }

        private void lblPriceBelow_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblPriceBelow.DisplayRectangle, System.Drawing.ColorTranslator.FromHtml("#860000"), ButtonBorderStyle.Solid);
            lblPriceBelow.BackColor = System.Drawing.ColorTranslator.FromHtml("#0062a0");
        }

        private void lblPriceAverage_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblPriceAverage.DisplayRectangle, System.Drawing.ColorTranslator.FromHtml("#860000"), ButtonBorderStyle.Solid);
            lblPriceAverage.BackColor = System.Drawing.ColorTranslator.FromHtml("#008000");
        }

        private void lblPriceAbove_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblPriceAbove.DisplayRectangle, System.Drawing.ColorTranslator.FromHtml("#860000"), ButtonBorderStyle.Solid);
            lblPriceAbove.BackColor = System.Drawing.ColorTranslator.FromHtml("#d02c00");
        }

        #endregion

        #region Helpers

        private void ShowLoading()
        {
            pbxLoading.Visible = true;
            pnlFilter.Enabled = false;
        }

        private void HideLoading()
        {
            pbxLoading.Visible = false;
            pnlFilter.Enabled = true;
        }

        private void TrimChange()
        {
            var firstManheim = _manheimData.FirstOrDefault(i => i.TrimServiceId == int.Parse(cbbTrim.SelectedValue.ToString()));
            if (firstManheim != null)
            {
                var manheimService = new ManheimService();
                lblPriceBelow.Text = firstManheim.LowestPrice.ToString();
                lblPriceAverage.Text = firstManheim.AveragePrice.ToString();
                lblPriceAbove.Text = firstManheim.HighestPrice.ToString();
                var data = manheimService.GetManheimTransactions(firstManheim.Year.ToString(CultureInfo.InvariantCulture), firstManheim.MakeServiceId.ToString(CultureInfo.InvariantCulture),
                                                          firstManheim.ModelServiceId.ToString(CultureInfo.InvariantCulture),
                                                          firstManheim.TrimServiceId.ToString(CultureInfo.InvariantCulture), cbbRegion.SelectedValue.ToString());
                dgManheimDetail.DataSource = data;
            }
            HideLoading();
        }

        private void RegionChange()
        {
            var firstManheim = _manheimData.FirstOrDefault(i => i.TrimServiceId == int.Parse(cbbTrim.SelectedValue.ToString()));
            if (firstManheim != null)
            {
                var manheimService = new ManheimService();
                var data = manheimService.GetManheimTransactions(firstManheim.Year.ToString(CultureInfo.InvariantCulture), firstManheim.MakeServiceId.ToString(CultureInfo.InvariantCulture),
                                                          firstManheim.ModelServiceId.ToString(CultureInfo.InvariantCulture),
                                                          firstManheim.TrimServiceId.ToString(CultureInfo.InvariantCulture),
                                                          cbbRegion.SelectedValue.ToString());
                dgManheimDetail.DataSource = data;
            }
            HideLoading();
        }

        private List<ManheimTransactionViewModel> TrimInit()
        {
            var manheimService = new ManheimService();
            return manheimService.GetManheimTransactions(_manheimData[0].Year.ToString(CultureInfo.InvariantCulture), _manheimData[0].MakeServiceId.ToString(CultureInfo.InvariantCulture),
                                                      _manheimData[0].ModelServiceId.ToString(CultureInfo.InvariantCulture),
                                                      _manheimData[0].TrimServiceId.ToString(CultureInfo.InvariantCulture), "NA");

        }

        private static List<KeyValuePair<string, string>> InitNationList()
        {
            return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("NA", "National"),
                    new KeyValuePair<string, string>("SE", "South East"),
                    new KeyValuePair<string, string>("NE", "North East"),
                    new KeyValuePair<string, string>("MW", "Mid West"),
                    new KeyValuePair<string, string>("SW", "South West"),
                    new KeyValuePair<string, string>("WC", "West Coast")
                };
        }
        #endregion


        public ManheimDetailForm(string vin, int year, string make, string model)
        {
            _vehicleLookUp = new VehicleViewModel {Vin = vin, Year = year, Make = make, Model = model};
           
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            var manheimService = new ManheimService();
            _manheimData = manheimService.ManheimReport(_vehicleLookUp, SessionVar.CurrentDealer.DealerSetting.MainheimUserName, SessionVar.CurrentDealer.DealerSetting.MainheimPassword);
            BindEvents();
            
            cbbTrim.DataSource = _manheimData;
            cbbTrim.DisplayMember = "TrimName";
            cbbTrim.ValueMember = "TrimServiceId";

            if (cbbTrim.SelectedValue != null)
            {
                lblPriceBelow.Text = _manheimData[0].LowestPrice.ToString();
                lblPriceAverage.Text = _manheimData[0].AveragePrice.ToString();
                lblPriceAbove.Text = _manheimData[0].HighestPrice.ToString();

                var nationList = InitNationList();
                cbbRegion.DataSource = nationList;
                cbbRegion.DisplayMember = "Value";
                cbbRegion.ValueMember = "Key";

                ShowLoading();
                var worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }

        }

        private void BindEvents()
        {
            cbbTrim.SelectionChangeCommitted += cbbTrim_SelectionChangeCommitted;
            cbbRegion.SelectionChangeCommitted += cbbRegion_SelectionChangeCommitted;
        }

      

     
    }
}
