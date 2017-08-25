using System;
using System.ComponentModel;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Models
{
    public class ReplaceModel : ModelBase, IDataErrorInfo
    {
        #region Properties

        private string _from;
        public string From
        {
            get { return _from; }

            set
            {
                if (_from != value)
                {
                    _from = value;
                    base.OnPropertyChanged("From");
                }
            }
        }

        private string _to;

        public string To
        {
            get { return _to; }

            set
            {
                if (_to != value)
                {
                    _to = value;
                    base.OnPropertyChanged("To");
                }
            }
        }

        #endregion

        public ReplaceModel()
        {
            ValidatedProperties = new[]{"From", "To"};
        }

        #region Implementation of IDataErrorInfo

        protected override string GetValidationError(string property)
        {
            if (Array.IndexOf(ValidatedProperties, property) < 0)
                return null;
            string result;
            switch (property)
            {
                case "From":
                    result = ValidateFromField();
                    break;
                case "To":
                    result = ValidateToField();
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }

       
        private string ValidateToField()
        {
            string result = String.IsNullOrEmpty(To) ? "To should have value" : null;
            return result;
        }

        private string ValidateFromField()
        {
            string result = String.IsNullOrEmpty(From) ? "From should have value" : null;
            return result;
        }

        ///// <summary>
        ///// Gets an error message indicating what is wrong with this object.
        ///// </summary>
        ///// <returns>
        ///// An error message indicating what is wrong with this object. The default is an empty string ("").
        ///// </returns>
        //public string Error { get; private set; }

        #endregion
    }

   
}
