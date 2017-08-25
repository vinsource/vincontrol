using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using vincontrol.Backend.Model;

namespace vincontrol.DataFeed.Model
{
    public class MappingViewModel
    {
        public string SchemaURL { get; set; }
        public string FTPHost { get; set; }
        public string FTPUserName { get; set; }
        public string FTPPassword { get; set; }
        public string Email { get; set; }
        public string ExportFileType { get; set; }
        public bool Bundle { get; set; }
        public string FileName { get; set; }
        public bool SplitImage { get; set; }

        public int Frequency { get; set; }
        public DateTime? RunningTime { get; set; }
        public string ProfileName { get; set; }

        public string SampleData { get; set; }
        public bool HasHeader { get; set; }
        public string Delimeter { get; set; }
        public string InventoryStatus { get; set; }
        public IList<Mapping> Mappings { get; set; }
        public bool Discontinued { get; set; }

        public string CompanyName { get; set; }

        public bool UseSpecificMapping { get; set; }
    }

    public class Mapping
    {
        public int Order { get; set; }
        public string DBField { get; set; }
        public string XMLField { get; set; }
        public IList<Replacement> Replaces { get; set; }
        public IList<Condition> Conditions { get; set; }
        public Expression Expression { get; set; }
    }

    public class Replacement
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class Condition
    {
        public string DBField { get; set; }
        public string XMLField { get; set; }
        public string ComparedValue { get; set; }
        public string TargetValue { get; set; }
        public string Operator { get; set; }
        public string Type { get; set; }
    }

    public class Expression : ModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        private string _dbField1;
        private string _dbField2;
        private string _dbField3;
        private string _xmlField;
        private string _operator1;
        private string _operator2;

        public string DBField1
        {
            get { return _dbField1; }
            set
            {
                if (_dbField1 != value)
                {
                    _dbField1 = value;
                    OnPropertyChanged("DBField1");
                }
            }
        }

        public string DBField2
        {
            get { return _dbField2; }
            set
            {
                if (_dbField2 != value)
                {
                    _dbField2 = value;
                    OnPropertyChanged("DBField2");
                }
            }
        }

        public string DBField3
        {
            get { return _dbField3; }
            set
            {
                if (_dbField3 != value)
                {
                    _dbField3 = value;
                    OnPropertyChanged("DBField3");
                }
            }
        }

        public string XMLField
        {
            get { return _xmlField; }
            set
            {
                if (_xmlField != value)
                {
                    _xmlField = value;
                    OnPropertyChanged("XMLField");
                }
            }
        }

        public string Operator1
        {
            get { return _operator1; }
            set
            {
                if (_operator1 != value)
                {
                    _operator1 = value;
                    OnPropertyChanged("Operator1");
                }
            }
        }

        public string Operator2
        {
            get { return _operator2; }
            set
            {

                if (_operator2 != value)
                {
                    _operator2 = value;
                    OnPropertyChanged("Operator2");
                }
            }
        }

        public Expression()
        {
            ValidatedProperties = new string[] { "Operator1", "DBField1", "DBField2"};    
        }

        protected override string GetValidationError(string columnName)
        {
            if (Array.IndexOf(ValidatedProperties, columnName) < 0)
                return null;

            string result = null;
            switch (columnName)
            {
                case "Operator1":
                    result = String.IsNullOrEmpty(Operator1) ? "Operator 1 should have value" : null;
                    break;
                case "DBField1":
                    result = String.IsNullOrEmpty(DBField1) ? "DBField 1 should have value" : null;
                    break;
                case "DBField2":
                    result = String.IsNullOrEmpty(DBField2) ? "DBField 2 should have value" : null;
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }

    public class Column
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }

    public class Operators
    {
        public const string Equal = "==";
        public const string LessThan = "&lt";
        public const string LessThanOrEqual = "&lt;=";
        public const string GreaterThan = "&gt;";
        public const string GreaterThanOrEqual = "&gt;=";
        public const string Different = "!=";
        public const string Contain = "Contain";
        public const string StartWith = "StartWith";
        public const string EndWith = "EndWith";
        public const string And = "&amp;";
        public const string Plus = "+";
        public const string Multiply = "*";
        public const string Devide = "/";
    }

    public class RunningStatus
    {
        public const string FTPError = "FTP Server Error";
        public const string Completed = "Completed";
        public const string Running = "Running";
        public const string Error = "Error";
    }
}
