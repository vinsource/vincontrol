using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Backend.Data;
using vincontrol.Backend.Interface;
using System.Windows;

namespace vincontrol.Backend.ViewModels.Import
{
    public class ImportDataHistoryViewModel : ViewModelBase
    {
        private int _profileId;
        public List<InventoryItem> Inventories
        {
            get { return _inventories; }
            set
            {
                if (_inventories != value)
                {
                    _inventories = value;
                    base.OnPropertyChanged("Inventories");
                }
            }
        }

        public List<InventoryItem> SoldoutInventories
        {
            get { return _soldoutInventories; }
            set
            {
                if (_soldoutInventories != value)
                {
                    _soldoutInventories = value;
                    base.OnPropertyChanged("SoldoutInventories");
                }
            }
        }

        private IView _view;
        private List<InventoryItem> _inventories;
        private List<InventoryItem> _soldoutInventories;

        public ImportDataHistoryViewModel(IView view, int profileId)
        {
            _view = view;
            _profileId = profileId;
            InitData();
            _view.SetDataContext(this);
        }

        private void InitData()
        {
            DoPendingTask(InitDataAsync);
        }

        private void InitDataAsync()
        {
            var vincontrolcontext = new vincontrolwarehouseEntities();
            var dateInfo = vincontrolcontext.importservicehistories.OrderByDescending(i => i.RunningDate).FirstOrDefault(i => i.ImportProfileId == _profileId);
            if (dateInfo != null && dateInfo.CompletedDate.HasValue && dateInfo.RunningDate.HasValue)
            {
                var startDate = dateInfo.RunningDate.Value;
                var endDate = dateInfo.CompletedDate.Value;
                var context = new whitmanenterprisewarehouseEntities();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                                          {
                                                                              Inventories =
                                                                                  context.
                                                                                      whitmanenterprisedealershipinventories
                                                                                      .Where(
                                                                                          i =>
                                                                                          i.LastUpdated > startDate &&
                                                                                          i.LastUpdated < endDate).
                                                                                      Select(
                                                                                          i => new InventoryItem()
                                                                                                   {
                                                                                                       ACV = i.ACV,
                                                                                                       CarImageUrl =
                                                                                                           i.CarImageUrl,
                                                                                                       CarOptions =
                                                                                                           i.CarsOptions,
                                                                                                       CarPackages =
                                                                                                           i.
                                                                                                           CarsPackages,
                                                                                                       Certified =
                                                                                                           i.Certified,
                                                                                                       DateInStock =
                                                                                                           i.DateInStock,
                                                                                                       DealerMSRP =
                                                                                                           i.DealerMSRP,
                                                                                                       DefaultImageURL =
                                                                                                           i.
                                                                                                           DefaultImageUrl,
                                                                                                       Make = i.Make,
                                                                                                       ModelYear =
                                                                                                           i.ModelYear,
                                                                                                       MSRP = i.MSRP,
                                                                                                       Model = i.Model,
                                                                                                       SalePrice =
                                                                                                           i.SalePrice,
                                                                                                       StandardOptions =
                                                                                                           i.
                                                                                                           StandardOptions,
                                                                                                       Trim = i.Trim,
                                                                                                       VINNumber =
                                                                                                           i.VINNumber
                                                                                                   }).ToList();
                                                                              SoldoutInventories =
                                                                                  context.
                                                                                      whitmanenterprisedealershipinventorysoldouts
                                                                                      .Where(
                                                                                          i =>
                                                                                          i.LastUpdated > startDate &&
                                                                                          i.LastUpdated < endDate).
                                                                                      Select(
                                                                                          i => new InventoryItem()
                                                                                                   {
                                                                                                       ACV = i.ACV,
                                                                                                       CarImageUrl =
                                                                                                           i.CarImageUrl,
                                                                                                       CarOptions =
                                                                                                           i.CarsOptions,
                                                                                                       CarPackages =
                                                                                                           i.
                                                                                                           CarsPackages,
                                                                                                       Certified =
                                                                                                           i.Certified,
                                                                                                       DateInStock =
                                                                                                           i.DateInStock,
                                                                                                       DealerMSRP =
                                                                                                           i.DealerMSRP,
                                                                                                       DefaultImageURL =
                                                                                                           i.
                                                                                                           DefaultImageUrl,
                                                                                                       Make = i.Make,
                                                                                                       ModelYear =
                                                                                                           i.ModelYear,
                                                                                                       MSRP = i.MSRP,
                                                                                                       Model = i.Model,
                                                                                                       SalePrice =
                                                                                                           i.SalePrice,
                                                                                                       StandardOptions =
                                                                                                           i.
                                                                                                           StandardOptions,
                                                                                                       Trim = i.Trim,
                                                                                                       VINNumber =
                                                                                                           i.VINNumber
                                                                                                   }).ToList();
                                                                          }));
            }
        }

        #region Overrides of ModelBase

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class InventoryItem
    {
        public string ACV { get; set; }
        public string CarImageUrl { get; set; }
        public string CarOptions { get; set; }
        public string CarPackages { get; set; }
        public bool? Certified { get; set; }
        public DateTime? DateInStock { get; set; }
        public string DealerMSRP { get; set; }
        public string DefaultImageURL { get; set; }
        public string Make { get; set; }
        public int? ModelYear { get; set; }
        public string MSRP { get; set; }
        public string Model { get; set; }
        public string SalePrice { get; set; }
        public string StandardOptions { get; set; }
        public string Trim { get; set; }
        public string VINNumber { get; set; }
    }
}