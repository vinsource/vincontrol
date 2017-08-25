using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Documents;
using Vincontrol.Brochure.Annotations;
using Vincontrol.Brochure.Commands;
using Vincontrol.Brochure.Models;
using Vincontrol.Brochure.View;
using vincontrol.Data.Model;

namespace Vincontrol.Brochure.ViewModels
{
    public class TradeinValueViewModel:INotifyPropertyChanged
    {
        private IView _view;
    
     

        public TradeinValueModel TradeinValueModel
        {
            get; set;
        }

        public TradeinValueViewModel(IView view)
        {
            _view = view;
            InitData();
            view.SetDataContext(this);
        }

        private void InitData()
        {
            TradeinValueModel = new TradeinValueModel();
            TradeinValueModel.Years = ChromeHelper.GetYears();
        }

       

        private void CreateData(int trimId)
        {
            using (var context = new VincontrolEntities())
            {
                var tradein = new TrimTradeIn();

                //tradein.TrimId = trimId;
                //tradein.SampleVIN = SampleVin.Text;
                //decimal tradeinValue = 0;
                //decimal.TryParse(TradeInValue.Text, out tradeinValue);
                //tradein.TradeInValue = tradeinValue;
                //long estimatedMileage = 0;
                //long.TryParse(EstimatedMileage.Text, out estimatedMileage);
                //tradein.EstimatedZeroPointMileage = estimatedMileage;
                //context.AddToTrimTradeIns(tradein);
                context.SaveChanges();
            }
        }

        public bool AllPropertiesValid
        {
            get
            {
                return TradeinValueModel.PropertiesValid;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
