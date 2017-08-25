using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;
using Vincontrol.Web.DatabaseModel;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Models
{
    public class AdminViewModel : DealershipViewModel
    {
        //private void Init(Setting dealerSetting)
        //{
        //    if (dealerSetting != null)
        //    {
               
        //        //DefaultStockImageURL = dealerSetting.DefaultStockImageUrl;
        //    }
        //}

        public AdminViewModel()
        {
            
        }

        public AdminViewModel(DealershipViewModel existingDealer)
        {
           

            DealershipId = existingDealer.DealershipId;
            DealerGroupId = existingDealer.DealerGroupId;
            Address = existingDealer.Address;
            City = existingDealer.City;
            ZipCode = existingDealer.ZipCode;
            State = existingDealer.State ?? "CA";
            Latitude = existingDealer.Latitude;
            Longtitude = existingDealer.Longtitude;
            DealershipName = existingDealer.DealershipName;
            DealershipAddress = existingDealer.Address;
            Phone = existingDealer.Phone;
            Email = existingDealer.Email;
            DealerSetting = existingDealer.DealerSetting ?? new DealerSettingViewModel();
            DealerCraigslistSetting = existingDealer.DealerCraigslistSetting ?? new DealerCraigslistSetting();
            NotificationSetting = existingDealer.NotificationSetting ?? new NotificationViewModel();
        }

        public string RetailPriceText { get; set; }
        public string DealerDiscountText { get; set; }
        public string ManufactureReabateText { get; set; }
        public string SalePriceText { get; set; }
        public string ManufacturerWarranty { get; set; }
        public string DealerCertified { get; set; }
        public string ManufacturerCertified { get; set; }
        public string DealerCertifiedDuration { get; set; }
        public string ManufacturerCertifiedDuration { get; set; }
    
     
        public string TermsAndCondition { get; set; }
        public string StartDescriptionSentence { get; set; }
        public string EndDescriptionSentence { get; set; }
        public IEnumerable<ExtendedSelectListItem> SortSetList { get; set; }
        public IEnumerable<ExtendedSelectListItem> SoldActionList { get; set; }
        public string AutoCheck { get; set; }
        [DataType(DataType.Password)]
        public string AutoCheckPassword { get; set; }
        public IEnumerable<UserRoleViewModel> Users { get; set; }
      public bool MutipleDealer { get; set; }


        [DataType(DataType.Password)]
        public string EncodeCarFaxPassword { get; set; }

        public bool CarFaxPasswordChanged { get; set; }

        
        [DataType(DataType.Password)]
        public string EncodeManheimPassword { get; set; }

        public bool ManheimPasswordChanged { get; set; }

       [DataType(DataType.Password)]
        public string EncodeKellyPassword { get; set; }

        public bool KellyPasswordChanged { get; set; }
        
        [DataType(DataType.Password)]
        public string EncodeBlackBookPassword { get; set; }

        public bool BlackBookPasswordChanged { get; set; }

  
        public DealerGroupViewModel DealerGroup { get; set; }



        public IEnumerable<ExtendedSelectListItem> DealerList { get; set; }

        public string LandingPage { get; set; }

        public int SelectedInterval { get; set; }

        public IEnumerable<ExtendedSelectListItem> IntervalList { get; set; }

        public IEnumerable<ExtendedSelectListItem> YearsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> MakesList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ModelsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> TrimsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> BodyTypeList { get; set; }

        public string SelectedYear { get; set; }

        public string SelectedMake { get; set; }

        public string SelectedModel { get; set; }

        public string SelectedTrim { get; set; }

        public string SelectedBodyType { get; set; }

        public List<ManafacturerRebateDistinctModel> RebateList { get; set; }

        public IEnumerable<TradeinCommentViewModel> Comments { get; set; }

        public IEnumerable<WarrantyTypeViewModel> WarrantyTypes { get; set; }

        public int SelectedWarrantyType { get; set; }

        public IList<SelectListItem> BasicWarrantyTypes { get; set; }

        public IList<ButtonPermissionViewModel> ButtonPermissions { get; set; }

        public string SelectedBrandName { get; set; }
    }
}
