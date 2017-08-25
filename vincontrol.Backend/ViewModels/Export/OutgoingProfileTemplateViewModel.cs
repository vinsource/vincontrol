using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using vincontrol.Backend.Commands.Import;
using vincontrol.Backend.Controls;
using vincontrol.Backend.Interface;
using vincontrol.DataFeed.Helper;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.ViewModels
{
    public class OutgoingProfileTemplateViewModel : BaseProfileTemplateViewModel
    {
        #region Members

        public string FileType { get; set; }
        public string InventoryStatus { get; set; }

        private List<KeyValuePair<string, string>> _fileTypeList;
        public List<KeyValuePair<string, string>> FileTypeList
        {
            get
            {
                return _fileTypeList ?? (_fileTypeList =
                 new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>("txt", "txt"),
                               new KeyValuePair<string, string>("csv", "csv"), 
                               new KeyValuePair<string, string>("xls", "xls"),
                           });
            }
        }

        private List<KeyValuePair<string, string>> _inventoryStatusList;
        public List<KeyValuePair<string, string>> InventoryStatusList
        {
            get
            {
                return _inventoryStatusList ?? (_inventoryStatusList =
                 new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>("Both", "Both"),
                               new KeyValuePair<string, string>("New", "New"), 
                               new KeyValuePair<string, string>("Used", "Used"),
                           });
            }
        }

        private CreateFileCommand _createFileCommand;
        public CreateFileCommand CreateFileCommand
        {
            get { return _createFileCommand ?? (_createFileCommand = new CreateFileCommand(this)); }
        }

        public OutgoingProfileTemplateViewModel(IView view)
            : base(view)
        {
        }

        #endregion

        #region Overrides of BaseProfileTemplateViewModel

        protected override MappingViewModel GetResult()
        {
            var xmlHelper = new ExportXMLHelper();
            var result = xmlHelper.LoadMappingTemplateByCompanyName("default");
            return result;
        }

        protected override void InitializerFields(MappingViewModel result)
        {
            FileType = result.ExportFileType;
            InventoryStatus = "Used";
        }

        #endregion
    }
}
