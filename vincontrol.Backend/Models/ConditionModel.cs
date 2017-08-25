using System;
using System.Collections.Generic;
using System.ComponentModel;
using vincontrol.Backend.Model;
using vincontrol.DataFeed.Model;


namespace vincontrol.Backend.ViewModels
{
    public class ConditionModel : ModelBase, IDataErrorInfo
    {
        public string DBField
        {
            get { return _dbField; }
            set
            {
                if (_dbField != value)
                {
                    _dbField = value;
                    base.OnPropertyChanged("DBField");
                }
            }
        }

        public string XMLField { get; set; }

        public string ComparedValue
        {
            get { return _comparedValue; }
            set
            {
                if (_comparedValue != value)
                {
                    _comparedValue = value;
                    base.OnPropertyChanged("ComparedValue");
                }
            }
        }

        public string TargetValue
        {
            get { return _targetValue; }
            set
            {
                if (_targetValue != value)
                {
                    _targetValue = value;
                    base.OnPropertyChanged("TargetValue");
                }
            }
        }

        public string Operator
        {
            get { return _operator; }
            set
            {
                if (_operator != value)
                {
                    _operator = value;
                    base.OnPropertyChanged("Operator");
                }
            }
        }

        public string Type { get; set; }

        //public List<Column> DBFields
        //{
        //    get { return Parent.DBFields; }
        //}
        private List<KeyValuePair<string, string>> _operatorList;
        private string _operator;
        private string _targetValue;
        private string _comparedValue;
        private string _dbField;

        public List<KeyValuePair<string, string>> OperatorList
        {
            get
            {
                return _operatorList ?? (_operatorList = new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>(Operators.Equal, "=="),
                               new KeyValuePair<string, string>(Operators.LessThan, "<"), 
                               new KeyValuePair<string, string>(Operators.LessThanOrEqual, "<="),
                                   new KeyValuePair<string, string>(Operators.GreaterThan, ">"),
                                   new KeyValuePair<string, string>(Operators.GreaterThanOrEqual, ">="),
                                   new KeyValuePair<string, string>(Operators.Different, "!="),
                                   new KeyValuePair<string, string>(Operators.Contain, "Contain"),
                                   new KeyValuePair<string, string>(Operators.StartWith, "Start With"),
                                   new KeyValuePair<string, string>(Operators.EndWith, "End With")
                           });
            }
        }

        public ConditionModel()
        {
            ValidatedProperties = new string[] { "DBField", "ComparedValue", "TargetValue","Operator"};
        }

       

        private string ValidateOperatorField()
        {
            return String.IsNullOrEmpty(Operator) ? "Operator should have value" : null;
        }

        private string ValidateTargetValueField()
        {
            return String.IsNullOrEmpty(TargetValue) ? "TargetValue should have value" : null;
        }

        private string ValidateComparedValueField()
        {
            return String.IsNullOrEmpty(ComparedValue) ? "ComparedValue should have value" : null;
        }

        private string ValidateDBField()
        {
            return String.IsNullOrEmpty(DBField) ? "DBField should have value" : null;
        }

        public string Error { get; private set; }

        #region Overrides of ViewModelBase

        protected override string GetValidationError(string columnName)
        {
            if (Array.IndexOf(ValidatedProperties, columnName) < 0)
                return null;

            string result=null;
            switch (columnName)
            {
                case "DBField":
                    result = ValidateDBField();
                    break;
                case "ComparedValue":
                    result = ValidateComparedValueField();
                    break;
                case "TargetValue":
                    result = ValidateTargetValueField();
                    break;
                case "Operator":
                    result = ValidateOperatorField();
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }

        #endregion
    }
}
