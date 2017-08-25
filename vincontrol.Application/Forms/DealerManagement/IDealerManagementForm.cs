using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.DealerManagement
{
    public interface IDealerManagementForm
    {
        List<DealershipViewModel> GetAll();
        DealerViewModel GetDealer(int dealerId);
        DealershipViewModel GetDealerById(int dealerId);
        DealershipViewModel GetDealerByName(string name);
        DealerWSTemplate GetDealerWindowStickerTemplate(int dealerId, int templateId);
        void AddNewCraigslistHistory(int userId, int inventoryId, long paymentId, long postingId);
        IQueryable<DealerWSTemplate> GetDealerWindowStickerTemplate(int dealerId);
        BuyerGuide GetBuyerGuide(int dealerId, int languageVersion, int warrantyType);
        long GetCraigslistPostingId(int inventoryId);
        Data.Model.Setting GetDealerSettingById(int dealerId);
        void UpdateBuyerGuide(BuyerGuide buyerGuide);
        IQueryable<Rebate> GetRebates(int dealerId);
        IQueryable<Dealer> GetDealers(string dealerGroupId);
        IQueryable<Dealer> GetDealers(IEnumerable<int> dealers);
        Dealer GetSpecificDealer(int dealerId);
        IQueryable<int?> GetYearList(int dealerId);
        List<WarrantyTypeViewModel> GetWarrantyTypeList(int dealerId);
        IList<ButtonPermissionViewModel> GetButtonList(int dealerId, string screen);
        void UpdateOverideStockImage(int dealershipId, bool overideStockImage);
        void UpdateWindowStickerNotification(bool notify, int dealerId, int notificationkind);
        void UpdateDefaultStockImageUrl(int dealershipId, string defaultStockImageUrl);
        void AddNewRebate(Rebate rebate);
        bool CheckAnyRebate(int year, string make, string model, string trim, int dealerId);
        void DeleteRebate(int rebateId);
        void SetDisclaimerContent(string content, int rebateId);
        void ActivateRebate(int rebateId);
        void DeactivateRebate(int rebateId);
        Rebate GetRebate(int rebateId);
        void UpdateRebateCreateDate(string date, int rebateId);
        void UpdateRebateExpirationDate(string date, int rebateId);
        void UpdateRebateAmount(string rebateAmount, int rebateId);
        void UpdateRebateDisclaimer(string rebateDisclaimer, int rebateId);
        bool RemoveBuyerGuide(int id);
        bool CheckBuyerGuide(int dealerId, string name);
        int AddBuyerGuide(int dealerId, string name, int category);
        void UpdateContentAppSetting(int dealershipId, int category);
        void UpdateContentAppSetting(int dealershipId, string descriptionNew, string descriptionUsed, int category);
        void UpdateContentAppSetting(int dealershipId, string description, int category);
        void UpdateButtonPermission(int dealerId, int groupId, int buttonId, bool canSee);
    }
}
