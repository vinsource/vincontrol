using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Windows;
using Microsoft.Win32;
using Vincontrol.Brochure.Annotations;
using Vincontrol.Brochure.Commands;
using vincontrol.Data.Model;

namespace Vincontrol.Brochure.Models
{
    public class TradeinValueModel : ModelBase
    {
        #region Properties

        private ModelMode _mode = ModelMode.New; 
        private decimal? _tradeinValue;
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Trim { get; set; }

        public long EstimatedMileage
        {
            get { return _estimatedMileage; }
            set
            {
                if (_estimatedMileage != value)
                {
                    _estimatedMileage = value;
                    OnPropertyChanged();
                }
            }
        }
        public decimal? TradeinValue
        {
            get { return _tradeinValue; }
            set
            {
                if (_tradeinValue != value)
                {
                    _tradeinValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SampleVin
        {
            get { return _sampleVin; }
            set
            {
                if (_sampleVin != value)
                {
                    _sampleVin = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<YearMakeItem> _makes;
        private List<ModelItem> _models;
        private List<TrimItem> _trims;
        private long _estimatedMileage;
        private string _sampleVin;
        private RelayCommand _trimChangeCommand;

        public List<int> Years
        {
            get;
            set;
        }

        public List<YearMakeItem> Makes
        {
            get { return _makes; }
            set
            {
                if (_makes != value)
                {
                    _makes = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<ModelItem> Models
        {
            get { return _models; }
            set
            {
                if (_models != value)
                {
                    _models = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<TrimItem> Trims
        {
            get { return _trims; }
            set
            {
                if (_trims != value)
                {
                    _trims = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Command
        private RelayCommand _saveConditionListCommand;
        private RelayCommand _uploadBrochureCommand;
        private RelayCommand _yearChangeCommand;
        private RelayCommand _makeChangeCommand;
        private RelayCommand _modelChangeCommand;

        public RelayCommand YearChangeCommand
        {
            get
            {
                return _yearChangeCommand ??
                    (_yearChangeCommand = new RelayCommand(OnChangeYear, () => true));
            }
        }

        public RelayCommand MakeChangeCommand
        {
            get
            {
                return _makeChangeCommand ??
                    (_makeChangeCommand = new RelayCommand(OnMakeChange, () => true));
            }
        }

        public RelayCommand ModelChangeCommand
        {
            get
            {
                return _modelChangeCommand ??
                    (_modelChangeCommand = new RelayCommand(OnModelChange, () => true));
            }
        }

        public RelayCommand TrimChangeCommand
        {
            get
            {
                return _trimChangeCommand ??
                    (_trimChangeCommand = new RelayCommand(OnTrimChange, () => true));
            }
        }

        public RelayCommand SaveConditionListCommand
        {
            get
            {
                return _saveConditionListCommand ??
                    (_saveConditionListCommand = new RelayCommand(SaveConditionList, CanSaveConditionList));
            }
        }

        public RelayCommand UploadBrochureCommand
        {
            get
            {
                return _uploadBrochureCommand ??
                    (_uploadBrochureCommand = new RelayCommand(UploadBrochure, CanUploadBrochure));
            }
        }

        #endregion

        #region Dropdownlist changed

        private void OnModelChange(object parameter)
        {
            if (parameter != null)
            {
                var selectedModel = (ModelItem)parameter;
                Trims = ChromeHelper.GetTrims(selectedModel.ModelId);
            }
            else
            {
                Trims = null;
            }
        }


        private void OnMakeChange(object parameter)
        {
            if (parameter != null)
            {
                var selectedYear = (YearMakeItem)parameter;
                Models = ChromeHelper.GetModels(selectedYear.YearMakeId);
            }
            else
            {
                Models = null;
            }
        }

        private void OnChangeYear(object parameter)
        {
            if (parameter != null)
            {
                var selectedYear = (int)parameter;
                Makes = ChromeHelper.GetMakes(selectedYear);
                EstimatedMileage = GetEstimatedMileage(selectedYear);
            }
            else
            {
                Makes = null;
            }
        }

        private void OnTrimChange(object parameter)
        {
            if (parameter != null)
            {
                var trimItem = (TrimItem)parameter;
                var tradeinInfo = ChromeHelper.GetTradeInInfo(trimItem.TrimId);
                if (tradeinInfo == null)
                {
                    _mode = ModelMode.New;
                    SampleVin = null;
                    TradeinValue = null;
                }
                else
                {
                    MessageBox.Show("Loading data from database");
                    _mode = ModelMode.Edit;
                    EstimatedMileage = tradeinInfo.EstimatedZeroPointMileage;
                    SampleVin = tradeinInfo.SampleVIN;
                    TradeinValue = tradeinInfo.TradeInValue;
                    //Upde data fileds
                }
            }
        }

        #endregion

        public int GetEstimatedMileage(int year)
        {
            return (DateTime.Now.Year + 1 - year) * 12000;
        }

      
        public TradeinValueModel()
        {
            ValidatedProperties = new string[] { "Year", "Make", "Model", "Trim", "TradeinValue" };
        }

        protected override string GetValidationError(string property)
        {
            if (Array.IndexOf(ValidatedProperties, property) < 0)
                return null;
            string result;
            switch (property)
            {
                case "Year":
                    result = (Year == 0) ? "Year should have value" : null;
                    break;
                case "Make":
                    result = String.IsNullOrEmpty(Make) ? "Make should have value" : null;
                    break;
                case "Model":
                    result = String.IsNullOrEmpty(Model) ? "Model should have value" : null;
                    break;
                case "Trim":
                    result = Trim==null ? "Trim should have value" : null;
                    break;
                case "TradeinValue":
                    result = GetTradeinMessage(TradeinValue);
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }

        private string GetTradeinMessage(decimal? tradeinValue)
        {
            if (TradeinValue == null) return "Trade in  value should have value";
            if (TradeinValue.Value < 0) return "Trade in value should be positive";
            return null;
        }

      

        private void UploadBrochure(object obj)
        {
            Execute();
        }

        private bool CanUploadBrochure()
        {
            return new string[] { "Year", "Make", "Model"}.All(property => GetValidationError(property) == null);
        }

        private bool CanSaveConditionList()
        {
            return PropertiesValid;
        }

        private void SaveConditionList(object parameter)
        {
            if (PropertiesValid)
            {
                if (_mode.Equals(ModelMode.New))
                {
                    CreateData(this);
                }
                else
                {
                    SaveData(this);
                }
            }
        }

        private void SaveData(TradeinValueModel model)
        {
            try
            {
                using (var context = new VincontrolEntities())
                {
                    var tradein = context.TrimTradeIns.FirstOrDefault(i => i.TrimId == model.Trim);
                    if (tradein != null)
                    {
                        tradein.SampleVIN = model.SampleVin;
                        //decimal tradeinValue = 0;
                        //decimal.TryParse(TradeInValue.Text, out tradeinValue);
                        if (model.TradeinValue != null) tradein.TradeInValue = model.TradeinValue.Value;
                        //long estimatedMileage = 0;
                        //long.TryParse(EstimatedMileage.Text, out estimatedMileage);
                        tradein.EstimatedZeroPointMileage = model.EstimatedMileage;
                        context.SaveChanges();
                        MessageBox.Show("Tradein information has been successfully saved.");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There is already this year make model trim in the database.");
            }
        }

        private void CreateData(TradeinValueModel model)
        {
            try
            {
            using (var context = new VincontrolEntities())
            {
                if (model.Trim != null)
                {
                    var tradein = new TrimTradeIn();
                    tradein.TrimId = model.Trim.Value;
                    tradein.SampleVIN = model.SampleVin;
                    //decimal tradeinValue = 0;
                    //decimal.TryParse(TradeInValue.Text, out tradeinValue);
                    if (model.TradeinValue != null) tradein.TradeInValue = model.TradeinValue.Value;
                    //long estimatedMileage = 0;
                    //long.TryParse(EstimatedMileage.Text, out estimatedMileage);
                    tradein.EstimatedZeroPointMileage = model.EstimatedMileage;
                    context.AddToTrimTradeIns(tradein);
                    context.SaveChanges();
                    MessageBox.Show("Tradein information has been successfully added.");
                }
            }
            }
            catch (Exception)
            {
                MessageBox.Show("There is already this year make model trim in the database.");
            }
        }


        public void Execute()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a pdf";
            fileDialog.Filter = "Jpeg files, PDF files|*.jpg;*.pdf;";
            if (fileDialog.ShowDialog().Value)
            {
                //Read from app config
                var uriBuilder = new UriBuilder(ConfigurationManager.AppSettings["UploadBrochureUrl"]);
                var r = new Random();
                uriBuilder.Query = string.Format("Year={0}&Make={1}&Model={2}&PDFFileName={3}&r={4}", this.Year, this.Make, this.Model, fileDialog.SafeFileName, r.Next());

                var webrequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
                webrequest.Method = "POST";
                webrequest.BeginGetRequestStream(new AsyncCallback(WriteCallback), new RequestResult() { WebRequest = webrequest, FileName = fileDialog.FileName, FileStream = fileDialog.OpenFile() });
            }
        }

        private void WriteCallback(IAsyncResult asynchronousResult)
        {
            var result = (RequestResult)asynchronousResult.AsyncState;
            var webrequest = result.WebRequest;
            // End the operation.
            var requestStream = webrequest.EndGetRequestStream(asynchronousResult);
            var buffer = new Byte[4096];
            var bytesRead = 0;

            Stream fileStream = result.FileStream;
            var sr = new System.IO.StreamReader(result.FileName);
            fileStream.Position = 0;
            while ((bytesRead =
            fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();
            }

            fileStream.Close();
            requestStream.Close();
            webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);

            //Stream fileStream = _vm.FileInfo.OpenRead();

        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
                var response = (HttpWebResponse)webrequest.EndGetResponse(asynchronousResult);
                var serializer =
                              new DataContractJsonSerializer(typeof(string));

                var result = serializer.ReadObject(response.GetResponseStream());
                MessageBox.Show(result.ToString());
                //_vm.ImageUrl = result.FileUrl;
                //_vm.ThumbnailImageUrl = result.ThumbnailUrl;
                //Deployment.Current.Dispatcher.BeginInvoke(delegate
                //{
                //    var image = new BitmapImage(new Uri(result.FileUrl));
                //    this._vm.ImageSource = image;
                //    MarkComplete();
                //});

            }
            catch (Exception e)
            {

                //ErrorHandler.ShowWarning(e.InnerException + e.Message);
            }
        }



    }

    public enum ModelMode
    {
        New, Edit
    }
}
