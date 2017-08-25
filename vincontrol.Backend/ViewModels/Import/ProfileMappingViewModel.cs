using System;
using System.Collections.Generic;
using System.ComponentModel;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Model;
using vincontrol.Backend.Windows.DataFeed.Import.PopupChildWindow;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.ViewModels.Import
{
    public class ProfileMappingViewModel : ModelBase
    {
        public ProfileMappingViewModel(bool hasHeader)
        {
            HasHeader = hasHeader;
            ValidatedProperties = new string[1];

        }
        public List<Column> DBFields
        {
            get;
            set;
        }

        #region Members
        public int Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    PreviousOrder = _order;
                    IsIncrease = value > _order;
                    _order = value;
                    OnPropertyChanged("Order");
                }
            }
        }

        public bool HasHeader { get; set; }

        public int PreviousOrder { get; set; }
        public bool IsIncrease { get; set; }

        //public int Order { get; set; }

        public bool RaisedOrderEvent
        {
            get { return _raisedOrderEvent; }
            set { _raisedOrderEvent = value; }
        }

        private bool _raisedOrderEvent = true;
        public string DBField { get; set; }
        public string XMLField
        {
            get { return _xmlField; }
            set
            {
                if (_xmlField != value)
                {
                    _xmlField = value;
                    OnPropertyChanged("XMLField");
                    OnPropertyChanged("Order");
                }
            }
        }

        //public string Expression { get; set; }
        public string SampleData { get; set; }
        public ReplaceListViewModel Replaces { get; set; }
        public ConditionListViewModel Conditions { get; set; }
        public bool SplitImage { get; set; }

        private RelayCommand _openConditionWindowCommand;

        public RelayCommand OpenConditionWindowCommand
        {
            get { return _openConditionWindowCommand ?? (_openConditionWindowCommand = new RelayCommand(OpenConditionWindow, null)); }
        }

        private RelayCommand _openExpressionWindowCommand;

        public RelayCommand OpenExpressionWindowCommand
        {
            get { return _openExpressionWindowCommand ?? (_openExpressionWindowCommand = new RelayCommand(OpenExpressionWindow, null)); }
        }



        private RelayCommand _openReplaceWindowCommand;
        private int _order;
        private Expression _expression;
        private string _xmlField;

        public RelayCommand OpenReplaceWindowCommand
        {
            get { return _openReplaceWindowCommand ?? (_openReplaceWindowCommand = new RelayCommand(OpenReplaceWindow, null)); }
        }

        void OpenConditionWindow(object parameter)
        {
            var chldWindow = new ConditionWindow(this);
            chldWindow.ShowDialog();
        }

        void OpenExpressionWindow(object parameter)
        {
            var chldWindow = new ExpressionWindow(this);
            chldWindow.Show();
        }

        void OpenReplaceWindow(object parameter)
        {
            var chldWindow = new ReplaceWindow(this);
            chldWindow.ShowDialog();
        }

        public Expression Expression
        {
            get { return _expression; }
            set
            {
                if (_expression != value)
                {
                    _expression = value;
                    OnPropertyChanged("Expression");
                }
            }
        }

        #endregion

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string property)
        {
            if (Array.IndexOf(ValidatedProperties, property) < 0)
                return null;
            switch (property)
            {
                case "Order":
                    return Order == 0 && !String.IsNullOrEmpty(XMLField) && HasHeader ? "Order should have value" : null;
            }
            return null;
        }

        #endregion
    }
}
