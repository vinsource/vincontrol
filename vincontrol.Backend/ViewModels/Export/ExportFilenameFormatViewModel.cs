using System;
using System.Collections.ObjectModel;
using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Model;
using vincontrol.Backend.Windows.DataFeed.Export;
using Action = vincontrol.Backend.Helper.UserAction;

namespace vincontrol.Backend.ViewModels.Export
{
    public class ExportFilenameFormatViewModel
    {
        public ObservableCollection<FileNameFormat> FileNameFormatList { get; set; }
        public FileNameFormat OtherFileNameFormat { get; set; }
        public FileNameFormat SelectedFileNameFormat { get; set; }

        private readonly ExportFilenameFormatWindow _view;
        private readonly ExportType _exportType;

        private RelayCommand _saveFileNameCommand;
        private int? _profileId;
        private int? _dealerId;

        public RelayCommand SaveFileNameCommand
        {
            get { return _saveFileNameCommand ?? (_saveFileNameCommand = new RelayCommand(SaveFileName, null)); }
        }

        public bool IsDirty { get; set; }

        void SaveFileName(object paramter)
        {
            var firstItem = FileNameFormatList.FirstOrDefault(i => i.IsSelected);
            SelectedFileNameFormat.Value =
           OtherFileNameFormat.IsSelected
                    ? OtherFileNameFormat.Value
                    : (firstItem == null ? String.Empty : firstItem.Value)
           ;
            if (_profileId != 0)
            {
                var context = new vincontrolwarehouseEntities();

                switch (_exportType)
                {
                    case ExportType.Dealer:
                        var lookup = context.datafeedlookups.FirstOrDefault(i => i.DataFeedProfileId == _profileId && i.DealerId ==_dealerId);
                        if (lookup != null)
                        {
                            lookup.FileName = SelectedFileNameFormat.Value;
                            context.SaveChanges();
                            Tracking.Log(Action.Update, App.CurrentUser.Id, DateTime.Now, lookup.Id,string.Format("Export Profile:{0} Dealer:{1}", lookup.datafeedprofile.ProfileName, lookup.dealer.Name), ItemType.ExportProfileDealer);
                        }
                        break;
                    case ExportType.Profile:
                       
                        var item = context.datafeedprofiles.FirstOrDefault(i => i.Id == _profileId);
                        if (item != null)
                        {
                            item.FileName = SelectedFileNameFormat.Value;
                            context.SaveChanges();
                            Tracking.Log(Action.Update, App.CurrentUser.Id, DateTime.Now, item.Id, string.Format("Export Profile:{0}", item.ProfileName), ItemType.ExportProfile);
                        }
                        break;
                }
            }
            IsDirty = true;
            _view.Close();
        }


        public ExportFilenameFormatViewModel(ExportFilenameFormatWindow view, FileNameFormat fileNameFormat, ExportType exportType, int? dealerId, int? profileId)
        {
            _profileId = profileId;
            _dealerId = dealerId;
            _exportType = exportType;
            SelectedFileNameFormat = fileNameFormat;

            InitializeData(fileNameFormat);
            _view = view;
            _view.SetDataContext(this);
        }

        private void InitializeData(FileNameFormat fileNameFormat)
        {
            FileNameFormatList = _exportType == ExportType.Dealer ? new ObservableCollection<FileNameFormat>
                                     {
                               new FileNameFormat {Example = "1200",IsSelected =false ,Value ="[DealerId]" ,Text = "Option 1"},
                               new FileNameFormat {Example = "1200_NewportCoastAuto_02212013",IsSelected =false ,Value ="[DealerId]_[DealerName]_[DateTime.Now]",Text = "Option 2"},
                               new FileNameFormat {Example = "Used_1200_NewportCoastAuto_02212013",IsSelected =false ,Value ="[Used]_[DealerId]_[DealerName]_[DateTime.Now]",Text = "Option 3"},
                               new FileNameFormat {Example = "ProfileId_NewportCoastAuto_02212013",IsSelected =false ,Value ="[ProfileId]_[DealerName]_[DateTime.Now]",Text = "Option 4"}
                               
                           } : new ObservableCollection<FileNameFormat>
                                     {
                               new FileNameFormat {Example = "ProfileId_NewportCoastAuto_02212013",IsSelected =false ,Value ="[ProfileId]_[DealerName]_[DateTime.Now]",Text = "Option 1"}
                               
                           };

            OtherFileNameFormat = new FileNameFormat();
            if (fileNameFormat != null)
            {
                var result = FileNameFormatList.FirstOrDefault(t => t.Value.Equals(fileNameFormat.Value));
                if (result != null)
                {
                    result.IsSelected = true;
                }
                else if (!string.IsNullOrEmpty(fileNameFormat.Value))
                {

                    OtherFileNameFormat.IsSelected = true;
                    OtherFileNameFormat.Value = fileNameFormat.Value;
                }
            }
        }
    }


}
