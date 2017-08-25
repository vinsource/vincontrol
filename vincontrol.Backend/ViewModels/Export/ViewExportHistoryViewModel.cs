using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;

namespace vincontrol.Backend.ViewModels.Export
{
    public class ViewExportHistoryViewModel : ViewModelBase
    {
        private readonly IView _view;
        private readonly ExportProfileViewModel _currentvm;
        public List<ServerFile> AllFiles { get; set; }
        public List<ServerFile> Files
        {
            get { return _files; }
            set
            {
                if (_files != value)
                {
                    _files = value;
                    base.OnPropertyChanged("Files");
                }
            }
        }

        public List<KeyValuePair<string, string>> Dealers { get; set; }
        public string SelectedDealer
        {
            get { return _selectedDealer; }
            set
            {
                if (_selectedDealer != value)
                {
                    _selectedDealer = value;
                    UpdateFile();
                    base.OnPropertyChanged("SelectedDealer");
                }
            }
        }

        private void UpdateFile()
        {
            Files = string.IsNullOrEmpty(SelectedDealer) ? AllFiles : AllFiles.Where(i => i.DealerName.Equals(SelectedDealer)).ToList();
        }

        private RelayCommand _downloadFileCommand;
        private string _selectedDealer;
        private List<ServerFile> _files;

        public RelayCommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFileAsync, null)); }
        }

        private void DownloadFileAsync(object parameter)
        {
            DoPendingTask(DownloadFile, parameter);
        }

        private void DownloadFile(object obj)
        {
            if (obj != null)
            {
                DataHelper.DownloadExportFile(obj.ToString());
            }
        }

        public ViewExportHistoryViewModel(IView view, ExportProfileViewModel vm)
        {
            _view = view;
            _currentvm = vm;
            InitData();
            _view.SetDataContext(this);
        }

        private void InitData()
        {
            var context = new vincontrolwarehouseEntities();
            AllFiles = (from p in context.exportservicehistories.Where(i => i.DatafeedProfileId == _currentvm.Id)
                        where p.Status == "Completed"
                        join r in context.dealers
                            on p.DealerId equals r.Id into g
                        from i in g.DefaultIfEmpty()
                        select
                            new ServerFile
                                {
                                    FileUrl = p.ArchiveFileName,
                                    LastDepositedDate = p.RunningDate ?? DateTime.MinValue,
                                    DealerName = i.Name
                                }).OrderByDescending(o => o.LastDepositedDate).ToList();
            Files = AllFiles;
            Dealers = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(String.Empty, string.Empty) };
            var distinctDealerName =
                AllFiles.Distinct(new DealerComparer()).Select(
                    i => new KeyValuePair<string, string>(i.DealerName, i.DealerName)).ToList();
            Dealers.AddRange(distinctDealerName);
        }

        #region Overrides of ModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ServerFile
    {
        public string FileUrl { get; set; }
        public DateTime LastDepositedDate { get; set; }
        public string DealerName { get; set; }
    }
    public class DealerComparer : IEqualityComparer<ServerFile>
    {
        #region Implementation of IEqualityComparer<in ServerFile>

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(ServerFile x, ServerFile y)
        {
            return x.DealerName.Trim().Equals(y.DealerName.Trim());
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(ServerFile obj)
        {
            return obj.DealerName.GetHashCode();
        }

        #endregion
    }
}