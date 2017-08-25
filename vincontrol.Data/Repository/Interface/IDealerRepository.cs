using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IDealerRepository
    {
        IQueryable<Dealer> GetAll();
        Dealer GetDealerById(int dealerId);
        Dealer GetDealerByName(string name);
        DealerWSTemplate GetDealerWindowStickerTemplate(int dealerId, int templateId);
        IQueryable<DealerWSTemplate> GetDealerWindowStickerTemplate(int dealerId);
        IQueryable<Rebate> GetRebates(int dealerId);
        void AddNewCraigslistHistory(int userId, int inventoryId, long paymentId, long postingId);
        void UpdateBuyerGuide(BuyerGuide buyerGuide);
        CraigslistHistory GetCraigslistHistoryByInventoryId(int inventoryId);
        Setting GetDealerSettingById(int dealerId);
        BuyerGuide GetBuyerGuide(int dealerId, int languageVersion, int warrantyType);
        Model.Dealer GetDealer(int dealerId);
        IQueryable<Dealer> GetDealers(string dealerGroupId);
        IQueryable<Dealer> GetDealers(IEnumerable<int> dealers);
        IQueryable<int?> GetYearList(int dealerId);
        List<BasicWarrantyType> GetBasicWarrantyTypes();
        IQueryable<WarrantyType> GetWarrantyTypes(int dealerId);
        IQueryable<Role> GetNonAdminRoles();
        IQueryable<Button > GetButtons(string screen);
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
