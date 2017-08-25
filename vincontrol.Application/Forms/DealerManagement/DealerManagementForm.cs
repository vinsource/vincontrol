using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using Button = vincontrol.Application.ViewModels.CommonManagement.Button;

namespace vincontrol.Application.Forms.DealerManagement
{
    public class DealerManagementForm : BaseForm, IDealerManagementForm
    {
        #region Constructors
        public DealerManagementForm() : this(new SqlUnitOfWork()) { }

        public DealerManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IDealerManagementForm Members

        public List<DealershipViewModel> GetAll()
        {
            var list = UnitOfWork.Dealer.GetAll().Where(i => i.ImportFeed.HasValue && i.ImportFeed.Value);
            return list.Any()
                ? list.AsEnumerable().Select(i => new DealershipViewModel(i)).ToList()
                : new List<DealershipViewModel>();
        }

        public DealerViewModel GetDealer(int dealerId)
        {
            var existingDealer = UnitOfWork.Dealer.GetDealerById(dealerId);
            return existingDealer == null ? null : new DealerViewModel(existingDealer);
        }

        public DealershipViewModel GetDealerById(int dealerId)
        {
            var existingDealer = UnitOfWork.Dealer.GetDealerById(dealerId);
            return existingDealer == null ? null : new DealershipViewModel(existingDealer);
        }

        public DealershipViewModel GetDealerByName(string name)
        {
            var existingDealer = UnitOfWork.Dealer.GetDealerByName(name);
            return existingDealer == null ? null : new DealershipViewModel(existingDealer);
        }

        public DealerWSTemplate GetDealerWindowStickerTemplate(int dealerId, int templateId)
        {
            return UnitOfWork.Dealer.GetDealerWindowStickerTemplate(dealerId,templateId);
        }

        public void AddNewCraigslistHistory(int userId, int inventoryId, long paymentId, long postingId)
        {
            UnitOfWork.Dealer.AddNewCraigslistHistory(userId, inventoryId, paymentId, postingId);
            UnitOfWork.CommitVincontrolModel();
        }

        public IQueryable<DealerWSTemplate> GetDealerWindowStickerTemplate(int dealerId)
        {
            return UnitOfWork.Dealer.GetDealerWindowStickerTemplate(dealerId);
        }

        public BuyerGuide GetBuyerGuide(int dealerId, int languageVersion, int warrantyType)
        {
            return UnitOfWork.Dealer.GetBuyerGuide(dealerId, languageVersion, warrantyType);
        }

        public long GetCraigslistPostingId(int inventoryId)
        {
            var history = UnitOfWork.Dealer.GetCraigslistHistoryByInventoryId(inventoryId);
            return history == null ? 0 : history.PostingId.GetValueOrDefault();
        }

        public Data.Model.Setting GetDealerSettingById(int dealerId)
        {
            return UnitOfWork.Dealer.GetDealerSettingById(dealerId);
        }

        public void UpdateBuyerGuide( BuyerGuide buyerGuide)
        {
            UnitOfWork.Dealer.UpdateBuyerGuide( buyerGuide);
            UnitOfWork.CommitVincontrolModel();
        }

        public IQueryable<Rebate> GetRebates(int dealerId)
        {
            return UnitOfWork.Dealer.GetRebates(dealerId);
        }

        public IQueryable<Dealer> GetDealers(string dealerGroupId)
        {
            return UnitOfWork.Dealer.GetDealers(dealerGroupId);
        }

        public IQueryable<Dealer> GetDealers(IEnumerable<int> dealers)
        {
            return UnitOfWork.Dealer.GetDealers(dealers);
        }

        public Dealer GetSpecificDealer(int dealerId)
        {
            return UnitOfWork.Dealer.GetDealer(dealerId);
        }

        public IQueryable<int?> GetYearList(int dealerId)
        {
            return UnitOfWork.Dealer.GetYearList(dealerId);
        }

        public List<WarrantyTypeViewModel> GetWarrantyTypeList(int dealerId)
        {
            var warrantyList = new List<WarrantyTypeViewModel>();

            var basicList = UnitOfWork.Dealer.GetBasicWarrantyTypes();

            var dealer = UnitOfWork.Dealer.GetDealer(dealerId);

            if (dealer.DealerGroupId != null)
            {
                if (dealer.DealerGroupId.ToLower().Equals(ConfigurationManager.AppSettings["Pendragon"]))
                {
                    basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Dealer Certified")));
                }
            }

            if (ConfigurationManager.AppSettings["MINIOfUniversalCity"] != null && dealerId.Equals(Convert.ToInt32(ConfigurationManager.AppSettings["MINIOfUniversalCity"])))
            {
                basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Manufacturer Warranty")));
                basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Dealer Certified")));
                basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Manufacturer Certified")));
            }

            foreach (var tmp in basicList)
            {
                var warrantyType = new WarrantyTypeViewModel()
                {
                    DealerId = dealer.DealerId,
                    EnglishVersionUrl =
                        "/Report/CreateBuyerGuide?type=" + tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                    SpanishVersionUrl =
                        "/Report/CreateBuyerGuideSpanish?type=" +
                        tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                    Name = tmp.Name,
                    Id = tmp.WarrantyTypeId,
                    CategoryId = tmp.Category.GetValueOrDefault()
                };

                warrantyList.Add(warrantyType);

            }

            var addtionalList = UnitOfWork.Dealer.GetWarrantyTypes(dealerId);

            if (addtionalList.Any())
            {
                foreach (var tmp in addtionalList)
                {
                    var warrantyType = new WarrantyTypeViewModel()
                    {
                        DealerId = dealer.DealerId,
                        EnglishVersionUrl =
                            "/Report/CreateBuyerGuide?type=" +
                            tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                        SpanishVersionUrl =
                            "/Report/CreateBuyerGuideSpanish?type=" +
                            tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                        Name = tmp.Name,
                        Id = tmp.WarrantyTypeId,
                        CategoryId = tmp.Category.GetValueOrDefault()
                    };

                    warrantyList.Add(warrantyType);

                }
            }




            return warrantyList;
        }

        public IList<ButtonPermissionViewModel> GetButtonList(int dealerId, string screen)
        {
            var result = new List<ButtonPermissionViewModel>();

            var groups = UnitOfWork.Dealer.GetNonAdminRoles();

            var buttons = UnitOfWork.Dealer.GetButtons(screen).ToList();

            if (buttons.Any())
            {
                foreach (var group in groups)
                {
                    var buttonModels = buttons.Select(i => new Button()
                    {
                        ButtonId = i.ButtonId,
                        ButtonName = i.Button1,
                        CanSee =
                            i.DealerButtons != null &&
                            i.DealerButtons.Any(
                                ii => ii.DealerId == dealerId && ii.ButtonId == i.ButtonId && ii.GroupId == group.RoleId)
                                ? i.DealerButtons.First(ii => ii.DealerId == dealerId && ii.ButtonId == i.ButtonId && ii.GroupId == group.RoleId).CanSee
                                : false
                    }).ToList();
                    var buttonPermission = new ButtonPermissionViewModel()
                    {
                        Buttons = buttonModels,
                        DealershipId = dealerId,
                        GroupId = group.RoleId,
                        GroupName = group.RoleName
                    };
                    result.Add(buttonPermission);
                }
            }

            return result;
        }

        public void UpdateOverideStockImage(int dealershipId, bool overideStockImage)
        {
            UnitOfWork.Dealer.UpdateOverideStockImage(dealershipId,overideStockImage);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateWindowStickerNotification(bool notify, int dealerId, int notificationkind)
        {
            UnitOfWork.Dealer.UpdateWindowStickerNotification(notify,dealerId,notificationkind);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateDefaultStockImageUrl(int dealershipId, string defaultStockImageUrl)
        {
            UnitOfWork.Dealer.UpdateDefaultStockImageUrl(dealershipId,defaultStockImageUrl);
            UnitOfWork.CommitVincontrolModel();
        }

        public void AddNewRebate(Rebate rebate)
        {
            UnitOfWork.Dealer.AddNewRebate(rebate);
            UnitOfWork.CommitVincontrolModel();
        }

        public bool CheckAnyRebate(int year, string make, string model, string trim, int dealerId)
        {
            return UnitOfWork.Dealer.CheckAnyRebate(year, make, model, trim, dealerId);
        }

        public void DeleteRebate(int rebateId)
        {
            UnitOfWork.Dealer.DeleteRebate(rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void SetDisclaimerContent(string content, int rebateId)
        {
            UnitOfWork.Dealer.SetDisclaimerContent(content, rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void ActivateRebate(int rebateId)
        {
            UnitOfWork.Dealer.ActivateRebate(rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void DeactivateRebate(int rebateId)
        {
            UnitOfWork.Dealer.DeactivateRebate(rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public Rebate GetRebate(int rebateId)
        {
            return UnitOfWork.Dealer.GetRebate(rebateId);
        }

        public void UpdateRebateCreateDate(string date, int rebateId)
        {
            UnitOfWork.Dealer.UpdateRebateCreateDate(date,rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateRebateExpirationDate(string date, int rebateId)
        {
            UnitOfWork.Dealer.UpdateRebateExpirationDate(date, rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateRebateAmount(string rebateAmount, int rebateId)
        {
            UnitOfWork.Dealer.UpdateRebateAmount(rebateAmount, rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateRebateDisclaimer(string rebateDisclaimer, int rebateId)
        {
            UnitOfWork.Dealer.UpdateRebateDisclaimer(rebateDisclaimer, rebateId);
            UnitOfWork.CommitVincontrolModel();
        }

        public bool RemoveBuyerGuide(int id)
        {
            var flag= UnitOfWork.Dealer.RemoveBuyerGuide(id);
            UnitOfWork.CommitVincontrolModel();
            return flag;
        }

        public bool CheckBuyerGuide(int dealerId, string name)
        {
            return UnitOfWork.Dealer.CheckBuyerGuide(dealerId, name);
        }

        public int AddBuyerGuide(int dealerId, string name, int category)
        {
            return UnitOfWork.Dealer.AddBuyerGuide(dealerId, name, category);
        }

        public void UpdateContentAppSetting(int dealershipId, int category)
        {
            UnitOfWork.Dealer.UpdateContentAppSetting(dealershipId,category);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateContentAppSetting(int dealershipId, string descriptionNew, string descriptionUsed, int category)
        {
            UnitOfWork.Dealer.UpdateContentAppSetting(dealershipId, descriptionNew, descriptionUsed,category);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateContentAppSetting(int dealershipId, string description, int category)
        {
            UnitOfWork.Dealer.UpdateContentAppSetting(dealershipId, description, category);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateButtonPermission(int dealerId, int groupId, int buttonId, bool canSee)
        {
            UnitOfWork.Dealer.UpdateButtonPermission(dealerId,groupId,buttonId,canSee);
            UnitOfWork.CommitVincontrolModel();
        }

        #endregion
    }
}
