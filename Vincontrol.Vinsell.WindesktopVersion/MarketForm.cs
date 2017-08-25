using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using VINCONTROL.Services.Business;
using Vincontrol.Vinsell.WPFLibrary;
using Vincontrol.Vinsell.WindesktopVersion.Helper;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace Vincontrol.Vinsell.WindesktopVersion
{
    public partial class MarketForm : Form
    {
        private readonly string _vin;
        private readonly ExtractedManheimVehicle _vehicle;
        private bool _autotrader;
        private Func<VinsellChartVehicle, bool> _query;
        private ManheimVehicle _targetCar;
        private readonly List<VinsellChartVehicle> _marketList;

        public MarketForm()
        {
            InitializeComponent();
        }

        public MarketForm(string vin, ExtractedManheimVehicle vehicle)
        {
            InitializeComponent();
            dGridInventory.AutoGenerateColumns = false;
            _autotrader = true;
            _vin = vin;
            _vehicle = vehicle;
        }

        private void MarketForm_Load(object sender, EventArgs e)
        {
            ShowLoading();
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dGridInventory.DataSource = new BindingList<VinsellChartVehicle>(_marketList);
            cbTrim.DataSource = _marketList.Select(x => x.Trim).Distinct().ToList();
            AddChart(_marketList);
            HideLoading();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var vehicle = new ManheimVehicle
            {
                Year = _vehicle.Year,
                Make = _vehicle.Make,
                Model = _vehicle.Model,
                Trim = _vehicle.Trim,
                MMR = _vehicle.Price,
                Images = String.Empty,
                Vin = _vin,
                Mileage = _vehicle.Odometer

            };

            //if (_targetCar == null)
            //{
            //    _targetCar = vehicle;
            //    _autoTraderMarketList = LinqHelper.GetNationwideAutoTraderMarketDateForVinsell(vehicle,
            //                                                                                             SessionVar
            //                                                                                                 .CurrentDealer
            //                                                                                                 .Latitude,
            //                                                                                             SessionVar
            //                                                                                                 .CurrentDealer
            //                                                                                                 .Longtitude);

            //}
            //else
            //{
            //    if (!_targetCar.Vin.Equals(_vin))
            //    {
            //        _targetCar = vehicle;
            //        _autoTraderMarketList = LinqHelper.GetNationwideAutoTraderMarketDateForVinsell(vehicle,
            //                                                                                                 SessionVar
            //                                                                                                     .CurrentDealer
            //                                                                                                     .Latitude,
            //                                                                                                 SessionVar
            //                                                                                                     .CurrentDealer
            //                                                                                                     .Longtitude);
            //    }
            //}
        }

        private void AddChart(List<VinsellChartVehicle> autoTraderMarketList)
        {
            var data = InitChartData(autoTraderMarketList);
            var wpfctl = new ChartControl(data, autoTraderMarketList.Min(i => i.Mileage), autoTraderMarketList.Max(i => i.Mileage), autoTraderMarketList.Min(i => i.Price), autoTraderMarketList.Max(i => i.Price));
            chartElementHost.Child = wpfctl;
        }

        private List<List<VinsellChartVehicle>> InitChartData(List<VinsellChartVehicle> autoTraderMarketList)
        {
            var value = new MarketValue(autoTraderMarketList);
            return new List<List<VinsellChartVehicle>>
                {
                    new List<VinsellChartVehicle>
                        {
                           new VinsellChartVehicle {Mileage = 0, Price = value.GetUnderBadPrice(0)??0},
                            new VinsellChartVehicle {Mileage = value.GetUnderBadMileage(0)??0, Price = 0}
                        },

                    new List<VinsellChartVehicle>
                        {
                            new VinsellChartVehicle {Mileage = 0, Price = value.GetUnderGoodPrice(0)??0},
                            new VinsellChartVehicle {Mileage = value.GetUnderGoodMileage(0)??0, Price = 0}
                        },

                    new List<VinsellChartVehicle>
                        {
                            new VinsellChartVehicle {Mileage = 0, Price = value.GetGoodPrice(0)??0},
                            new VinsellChartVehicle {Mileage = value.GetGoodMileage(0)??0, Price = 0}
                        },

                    new List<VinsellChartVehicle>
                        {
                            new VinsellChartVehicle {Mileage = 0, Price = value.GetOverGoodPrice(0)??0},
                            new VinsellChartVehicle {Mileage = value.GetOverGoodMileage(0)??0, Price = 0}
                        },

                    new List<VinsellChartVehicle>
                        {
                            new VinsellChartVehicle {Mileage = 0, Price = value.GetOverBadPrice(0)??0},
                            new VinsellChartVehicle {Mileage = value.GetOverBadMileage(0)??0, Price = 0}
                        },

                   autoTraderMarketList,
                        new List<VinsellChartVehicle>
                            {
                                new VinsellChartVehicle
                                    {
                                        Year = _targetCar.Year,
                                        Make = _targetCar.Make,
                                        Model = _targetCar.Model,
                                        Trim = _targetCar.Trim,
                                        Price =_targetCar.MMR,
                                        Mileage = _targetCar.Mileage
                                    }
                            }
                };
        }

        private void rbAutoTrader_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAutoTrader.Checked)
            {
                _autotrader = true;
                dGridInventory.DataSource =
                    new BindingList<VinsellChartVehicle>(_marketList);
                cbTrim.DataSource = _marketList.Select(x => x.Trim).Distinct().ToList();
            }
        }

        private void rbCarsCom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCarsCom.Checked)
            {
                _autotrader = false;

                if (_marketList == null)

                    //_carsComMarketList = LinqHelper.GetNationwideCarsComMarketDateForVinsell(_targetCar,
                    //                                                             SessionVar.CurrentDealer
                    //                                                                       .Latitude,
                    //                                                             SessionVar.CurrentDealer
                    //                                                                       .Longtitude);
                    dGridInventory.DataSource = new BindingList<VinsellChartVehicle>(_marketList).ToList();
                cbTrim.DataSource = _marketList.Select(x => x.Trim).Distinct().ToList();
            }
        }


        private void btn100_Click(object sender, EventArgs e)
        {
            BindingData(_query = x => x.Distance <= 100);
        }

        private void btn250_Click(object sender, EventArgs e)
        {
            BindingData(_query = x => x.Distance <= 250);
        }

        private void btn500_Click(object sender, EventArgs e)
        {
            BindingData(_query = x => x.Distance <= 500);
        }

        private void btnNation_Click(object sender, EventArgs e)
        {
            BindingData(_query = x => true);
        }

        private void btnFranchise_Click(object sender, EventArgs e)
        {
            BindingData(_query = x => x.Franchise);
        }


        private void btnIndependent_Click(object sender, EventArgs e)
        {
            BindingData(_query = x => !x.Franchise);
        }

        private void cbTrim_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BindingDataWithTrim(_query ?? (x => true), cbTrim.SelectedValue.ToString());
        }

        private void BindingData(Func<VinsellChartVehicle, bool> filter)
        {
            var source = _autotrader
                             ? _marketList.Where(filter).ToList()
                             : _marketList.Where(filter).ToList();
            dGridInventory.DataSource = new BindingList<VinsellChartVehicle>(source);
            cbTrim.DataSource = _autotrader
                                    ? _marketList.Where(filter)
                                                .Select(x => x.Trim)
                                                .Distinct()
                                                .ToList()
                                    : _marketList.Where(filter)
                                                .Select(x => x.Trim)
                                                .Distinct()
                                                .ToList();
            AddChart(source);
        }

        private void BindingDataWithTrim(Func<VinsellChartVehicle, bool> filter, string trimName)
        {
            var source = _autotrader
                             ? _marketList.Where(filter).Where(i => i.Trim.Equals(trimName)).ToList()
                             : _marketList.Where(filter).Where(i => i.Trim.Equals(trimName)).ToList();
            dGridInventory.DataSource = new BindingList<VinsellChartVehicle>(source);
            AddChart(source);
        }

        private void ShowLoading()
        {
            pbxLoading.Visible = true;
            pnlMarket.Enabled = false;
        }

        private void HideLoading()
        {
            pbxLoading.Visible = false;
            pnlMarket.Enabled = true;
        }
    }
}
