using System;

namespace vincontrol.Backend.Model
{
    public class DealerImportSetting : ModelBase
    {
        private string _fileUrl;
        private DateTime? _lastDepositedDate;
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public bool Discontinued { get; set; }
        public string FeedUrl { get; set; }

        public string FileUrl
        {
            get { return _fileUrl; }
            set
            {
                if (_fileUrl != value)
                {
                    _fileUrl = value;
                    OnPropertyChanged("FileUrl");
                }

            }
        }

        public DateTime? LastDepositedDate
        {
            get { return _lastDepositedDate; }
            set
            {
                if (_lastDepositedDate != value)
                {
                    _lastDepositedDate = value;
                    OnPropertyChanged("LastDepositedDate");
                }

            }
        }

        #region Overrides of ModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
