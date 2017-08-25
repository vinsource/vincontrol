using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class DealerRepository : IDealerRepository
    {
        private VincontrolEntities _context;

        public DealerRepository(VincontrolEntities context)
        {
            _context = context;
        }

        #region IDealerRepository' Members

        public IQueryable<Dealer> GetAll()
        {
            return _context.Dealers.Include("Setting").Where(x => x.Active.HasValue && x.Active.Value && x.DealerGroupId != null && x.DealerGroupId != "").OrderBy(x => x.DealerGroupId).ThenBy(x => x.Name);
        }

        public Dealer GetDealerById(int dealerId)
        {
            return _context.Dealers.FirstOrDefault(i => i.DealerId==dealerId);
        }

        public Dealer GetDealerByName(string name)
        {
            return _context.Dealers.FirstOrDefault(
                i =>i.Name.Replace(" ", String.Empty).Replace("-", String.Empty).ToLower().Equals(name.Replace(" ", String.Empty).Replace("-", String.Empty).ToLower()));

         
        }

        public DealerWSTemplate GetDealerWindowStickerTemplate(int dealerId, int templateId)
        {
            return _context.DealerWSTemplates.FirstOrDefault(i => i.DealerId == dealerId && i.TemplateId==templateId);
        }

        public IQueryable<DealerWSTemplate> GetDealerWindowStickerTemplate(int dealerId)
        {
            return _context.DealerWSTemplates.Where(i => i.DealerId == dealerId);
        }

        public IQueryable<Rebate> GetRebates(int dealerId)
        {
            return _context.Rebates.Where(i => i.DealerId == dealerId);
        }

        public void AddNewCraigslistHistory(int userId, int inventoryId, long paymentId, long postingId)
        {
            _context.AddToCraigslistHistories(new CraigslistHistory() { 
            UserId = userId,
            InventoryId = inventoryId,
            PaymentId = paymentId,
            PostingId = postingId,
            DateStamp = DateTime.Now
            });
        }

        public void UpdateBuyerGuide( BuyerGuide buyerGuide)
        {
            var buyerGuideSetting =
                       _context.BuyerGuides.FirstOrDefault(
                           bg =>
                           bg.DealerId == buyerGuide.DealerId && bg.LanguageVersion == buyerGuide.LanguageVersion &&
                           bg.WarrantyType == buyerGuide.WarrantyType);
            if (buyerGuideSetting != null)
            {
                buyerGuideSetting.Vin = buyerGuide.Vin;
                buyerGuideSetting.Year = buyerGuide.Year;
                buyerGuideSetting.Make = buyerGuide.Make;
                buyerGuideSetting.Model = buyerGuide.Model;
                buyerGuideSetting.Stock = buyerGuide.Stock;
                buyerGuideSetting.WarrantyType = buyerGuide.WarrantyType;
                buyerGuideSetting.IsServiceContract = buyerGuide.IsServiceContract;
                buyerGuideSetting.IsAsWarranty = buyerGuide.IsAsWarranty;
                buyerGuideSetting.IsWarranty = buyerGuide.IsWarranty;
                buyerGuideSetting.IsFullWarranty = buyerGuide.IsFullWarranty;
                buyerGuideSetting.IsLimitedWarranty = buyerGuide.IsLimitedWarranty;
                buyerGuideSetting.SystemCovered = buyerGuide.SystemCovered;
                buyerGuideSetting.Durations = buyerGuide.Durations;
                buyerGuideSetting.PercentageOfLabor = buyerGuide.PercentageOfLabor;
                buyerGuideSetting.PercentageOfPart = buyerGuide.PercentageOfPart;
                buyerGuideSetting.PriorRental = buyerGuide.PriorRental;
                buyerGuideSetting.IsMixed = buyerGuide.IsMixed;
                buyerGuideSetting.SystemCoveredAndDurations = buyerGuide.SystemCoveredAndDurations;
                buyerGuideSetting.IsManufacturerWarranty = buyerGuide.IsManufacturerWarranty;
                buyerGuideSetting.IsManufacturerUsedVehicleWarranty = buyerGuide.IsManufacturerUsedVehicleWarranty;
                buyerGuideSetting.IsOtherWarranty = buyerGuide.IsOtherWarranty;
            }
            else
            {
                _context.AddToBuyerGuides(buyerGuide);
               
            }
        }

        public CraigslistHistory GetCraigslistHistoryByInventoryId(int inventoryId)
        {
            return _context.CraigslistHistories.FirstOrDefault(i => i.InventoryId.Equals(inventoryId));
        }

        public Setting GetDealerSettingById(int dealerId)
        {
            return _context.Settings.FirstOrDefault(i => i.DealerId == dealerId);
        }

        public BuyerGuide GetBuyerGuide(int dealerId, int languageVersion, int warrantyType)
        {
            return _context.BuyerGuides.FirstOrDefault(i => i.DealerId == dealerId && i.LanguageVersion==languageVersion&& i.WarrantyType==warrantyType);
        }

        public Dealer GetDealer(int dealerId)
        {
            return _context.Dealers.Include("Setting").FirstOrDefault(x => x.DealerId == dealerId && (x.Active.HasValue && x.Active.Value));
        }

        public IQueryable<Dealer> GetDealers(string dealerGroupId)
        {
            return _context.Dealers.Include("Setting").Where(x => x.DealerGroupId == dealerGroupId && (x.Active.HasValue && x.Active.Value));
        }

        public IQueryable<Dealer> GetDealers(IEnumerable<int> dealers)
        {
            return _context.Dealers.Include("Setting").Where(LinqExtendedHelper.BuildContainsExpression<Dealer, int>(e => e.DealerId, dealers));

        }

        public IQueryable<int?> GetYearList(int dealerId)
        {
            return _context.Inventories.Where(x => x.DealerId == dealerId && x.Condition == Constanst.ConditionStatus.New).Select(x => x.Vehicle.Year).Distinct();
        }

        public List<BasicWarrantyType> GetBasicWarrantyTypes()
        {
            return _context.BasicWarrantyTypes.ToList();
        }

        public IQueryable<WarrantyType> GetWarrantyTypes(int dealerId)
        {
            return _context.WarrantyTypes.Where(x => x.DealerId == dealerId);
        }

        public IQueryable<Role> GetNonAdminRoles()
        {
            return _context.Roles.Where(i => i.RoleId != Constanst.RoleType.Admin);
        }

        public IQueryable<Button> GetButtons(string screen)
        {
            return _context.Buttons.Where(x => x.Screen == screen);
        }

        public void UpdateOverideStockImage(int dealershipId, bool overideStockImage)
        {
            var searchResult = _context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);

            if (searchResult != null)
            {
                searchResult.OverideStockImage = overideStockImage;

             
            }
        }

        public void UpdateWindowStickerNotification(bool notify, int dealerId, int notificationkind)
        {
            var setting = _context.Settings.FirstOrDefault(x => x.DealerId == dealerId);
            if (setting != null)
            {
                if (notificationkind == Constanst.NotificationType.RetailPrice)
                    setting.RetailPrice = notify;
                else if (notificationkind == Constanst.NotificationType.DealerDiscount)
                    setting.DealerDiscount = notify;
                else if (notificationkind == Constanst.NotificationType.Manufacturer)
                    setting.ManufacturerReabte = notify;
                else if (notificationkind == Constanst.NotificationType.SalePrice)
                    setting.SalePrice = notify;

            }
        }

        public void UpdateDefaultStockImageUrl(int dealershipId, string defaultStockImageUrl)
        {
            var searchResult = _context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);

            if (searchResult != null)
            {

                searchResult.DefaultStockImageUrl = defaultStockImageUrl;

               
            }
        }

        public void AddNewRebate(Rebate rebate)
        {
            _context.AddToRebates(rebate);
        }

        public bool CheckAnyRebate(int year, string make, string model, string trim, int dealerId)
        {
            return
                _context.Rebates.Any(
                    x =>
                        x.Year == year && x.Make == make && x.Model == model && x.Trim == trim && x.DealerId == dealerId);
        }

        public void DeleteRebate(int rebateId)
        {

            var searchrebate = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
            if (searchrebate != null)
            {
                var result =
                    _context.Inventories.Where(x => x.DealerId == searchrebate.DealerId &&
                                                    x.Condition == Constanst.ConditionStatus.New &&
                                                    x.Vehicle.Year == searchrebate.Year &&
                                                    x.Vehicle.Make == searchrebate.Make &&
                                                    x.Vehicle.Model == searchrebate.Model &&
                                                    x.Vehicle.Trim == searchrebate.Trim);
                foreach (var tmp in result)
                {
                    tmp.ManufacturerRebate = null;
                    tmp.Disclaimer = null;
                    tmp.WindowStickerPrice = tmp.SalePrice - tmp.DealerDiscount;
                }

                _context.Attach(searchrebate);
                _context.DeleteObject(searchrebate);

            }

        }

        public void SetDisclaimerContent(string content, int rebateId)
        {
            var searchrebate = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
            if (searchrebate != null)
            {
                var result =
                    _context.Inventories.Where(x => x.DealerId == searchrebate.DealerId &&
                                                    x.Condition == Constanst.ConditionStatus.New &&
                                                    x.Vehicle.Year == searchrebate.Year &&
                                                    x.Vehicle.Make == searchrebate.Make &&
                                                    x.Vehicle.Model == searchrebate.Model &&
                                                    x.Vehicle.Trim == searchrebate.Trim);

                foreach (var tmp in result)
                {
                    tmp.Disclaimer = content;
                }

                searchrebate.Disclaimer = content;


            }
        }

        public void ActivateRebate(int rebateId)
        {
             var searchrebate = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
             IQueryable<Inventory> result;
            if (searchrebate != null)
            {
                if (searchrebate.Trim == "All Trims")
                {
                    result = _context.Inventories.Where(
                            x => x.DealerId == searchrebate.DealerId
                                 && x.Condition == Constanst.ConditionStatus.New
                                 && x.Vehicle.Year == searchrebate.Year
                                 && x.Vehicle.Make == searchrebate.Make
                                 && x.Vehicle.Model == searchrebate.Model);
                }
                else
                {
                    result = _context.Inventories
                        .Where(
                            x => x.DealerId == searchrebate.DealerId
                                 && x.Condition == Constanst.ConditionStatus.New
                                 && x.Vehicle.Year == searchrebate.Year
                                 && x.Vehicle.Make == searchrebate.Make
                                 && x.Vehicle.Model == searchrebate.Model
                                 && x.Vehicle.Trim == searchrebate.Trim);
                }

                foreach (var tmp in result)
                {
                    tmp.ManufacturerRebate =
                        Convert.ToInt32(LinqExtendedHelper.RemoveSpecialCharactersForNumber(searchrebate.ManufactureReabte));
                    tmp.Disclaimer = searchrebate.Disclaimer;
                    tmp.WindowStickerPrice = tmp.RetailPrice - tmp.DealerDiscount - tmp.ManufacturerRebate;
                    var vehiclelog = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description = Constanst.VehicleLogSentence.RebateApplied
                            .Replace("[rebate amount]", searchrebate.ManufactureReabte)
                            .Replace("[sales price]", tmp.SalePrice.ToString())
                            .Replace("[rebate expiration date]", searchrebate.ExpiredDate.GetValueOrDefault().ToShortDateString()),
                        InventoryId = tmp.InventoryId,
                        UserId = null
                    };

                    _context.AddToVehicleLogs(vehiclelog);
                }
            }
        }

        public void DeactivateRebate(int rebateId)
        {
            var searchrebate = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
            IQueryable<Inventory> result;
            if (searchrebate != null)
            {
                if (searchrebate.Trim == "All Trims")
                {
                    result = _context.Inventories.Where(
                            x => x.DealerId == searchrebate.DealerId
                                 && x.Condition == Constanst.ConditionStatus.New
                                 && x.Vehicle.Year == searchrebate.Year
                                 && x.Vehicle.Make == searchrebate.Make
                                 && x.Vehicle.Model == searchrebate.Model);
                }
                else
                {
                    result = _context.Inventories
                        .Where(
                            x => x.DealerId == searchrebate.DealerId
                                 && x.Condition == Constanst.ConditionStatus.New
                                 && x.Vehicle.Year == searchrebate.Year
                                 && x.Vehicle.Make == searchrebate.Make
                                 && x.Vehicle.Model == searchrebate.Model
                                 && x.Vehicle.Trim == searchrebate.Trim);
                }

                foreach (var tmp in result)
                {
                    tmp.ManufacturerRebate = null;
                    tmp.Disclaimer = null;
                    tmp.WindowStickerPrice = tmp.RetailPrice - tmp.DealerDiscount;
                    var vehiclelog = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description = Constanst.VehicleLogSentence.RebateExpired
                            .Replace("[rebate amount]", searchrebate.ManufactureReabte)
                            .Replace("[sales price]", tmp.SalePrice.ToString()),

                        InventoryId = tmp.InventoryId,
                        UserId = null
                    };

                    _context.AddToVehicleLogs(vehiclelog);
                }
            }
        }

        public Rebate GetRebate(int rebateId)
        {
            return _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
        }

        public void UpdateRebateCreateDate(string date, int rebateId)
        {
            var rebate = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
            if (rebate != null)
            {
                if (rebate.DateStamp.GetValueOrDefault().Date >= rebate.ExpiredDate.GetValueOrDefault().Date)
                {
                    var newExpire = rebate.DateStamp.GetValueOrDefault().AddDays(1);
                    rebate.ExpiredDate = newExpire;
                }
                rebate.DateStamp = Convert.ToDateTime(date);

            }
        }

        public void UpdateRebateExpirationDate(string date, int rebateId)
        {

            var rebate = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
            if (rebate != null)
            {
                rebate.ExpiredDate = Convert.ToDateTime(date);

            }
        }

        public void UpdateRebateAmount(string rebateAmount, int rebateId)
        {
            var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var searchResult = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId && x.ExpiredDate >= todayDate);
            if (searchResult != null)
            {
                var result =
                    _context.Inventories.Where(
                        x =>
                            x.DealerId == searchResult.DealerId && x.Condition == Constanst.ConditionStatus.New &&
                            x.Vehicle.Year == searchResult.Year && x.Vehicle.Make == searchResult.Make &&
                            x.Vehicle.Model == searchResult.Model && x.Vehicle.Trim == searchResult.Trim);

                foreach (var tmp in result)
                {
                    tmp.ManufacturerRebate =
                        Convert.ToInt32(LinqExtendedHelper.RemoveSpecialCharactersForNumber(rebateAmount));

                    var vehiclelog = new VehicleLog()
                    {
                        DateStamp = DateTime.Now,
                        Description = Constanst.VehicleLogSentence.RebateRevision
                            .Replace("[rebate amount]", searchResult.ManufactureReabte)
                            .Replace("[sales price]", tmp.SalePrice.ToString())
                            .Replace("[rebate expiration date]", searchResult.ExpiredDate.GetValueOrDefault().ToShortDateString()),
                        InventoryId = tmp.InventoryId,
                        UserId = null
                    };

                    _context.AddToVehicleLogs(vehiclelog);

                    tmp.WindowStickerPrice = tmp.RetailPrice - tmp.DealerDiscount - tmp.ManufacturerRebate;
                }


                searchResult.ManufactureReabte = rebateAmount;

            }
        }

        public void UpdateRebateDisclaimer(string rebateDisclaimer, int rebateId)
        {
            var searchResult = _context.Rebates.FirstOrDefault(x => x.RebateId == rebateId);
            if (searchResult != null)
            {
               
                var result =
                     _context.Inventories.Where(
                         x => x.DealerId == searchResult.DealerId && x.Condition == Constanst.ConditionStatus.New && x.Vehicle.Year == searchResult.Year && x.Vehicle.Make == searchResult.Make && x.Vehicle.Model == searchResult.Model && x.Vehicle.Trim == searchResult.Trim);

                foreach (var tmp in result)
                {
                    tmp.Disclaimer = rebateDisclaimer;
                }

                searchResult.Disclaimer = rebateDisclaimer;
            }
        }

        public bool RemoveBuyerGuide(int id)
        {
            var existingBuyerGuide = _context.WarrantyTypes.FirstOrDefault(i => i.WarrantyTypeId == id);
            if (existingBuyerGuide != null)
            {
                _context.DeleteObject(existingBuyerGuide);
                return true;
                
            }
            return false;
        }

        public bool CheckBuyerGuide(int dealerId, string name)
        {
            return _context.WarrantyTypes.Any(i => i.DealerId == dealerId && i.Name.ToLower().Trim().Equals(name.ToLower().Trim()));

        }

        public int AddBuyerGuide(int dealerId, string name, int category)
        {
            if (!CheckBuyerGuide(dealerId, name))
            {
                var newBuyerGuide = new WarrantyType
                {
                    DealerId = dealerId,
                    IsActive = true,
                    Name = name,
                    Category = category
                };
                _context.AddToWarrantyTypes(newBuyerGuide);
                _context.SaveChanges();
                return newBuyerGuide.WarrantyTypeId;
            }
            return 0;

        }

        public void UpdateContentAppSetting(int dealershipId, int category)
        {
            var searchResult = _context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);
            if (searchResult != null)
            {
                switch (category)
                {
                    case Constanst.DescriptionSettingCategory.LoanerSentence:
                        searchResult.LoanerSentence = string.Empty;
                        break;
                    case Constanst.DescriptionSettingCategory.AuctionSentence:
                        searchResult.AuctionSentence = string.Empty;
                        break;
                    case Constanst.DescriptionSettingCategory.DealerWarranty:
                        searchResult.DealerWarranty = string.Empty;
                        break;
                    case Constanst.DescriptionSettingCategory.EndSentence:
                        searchResult.EndDescriptionSentence = string.Empty;
                        break;
                    case Constanst.DescriptionSettingCategory.ShippingInfo:
                        searchResult.ShippingInfo = string.Empty;
                        break;
                    case Constanst.DescriptionSettingCategory.StartSentence:
                        searchResult.StartDescriptionSentence = string.Empty;
                        break;
                    case Constanst.DescriptionSettingCategory.TermConditon:
                        searchResult.TermsAndCondition = string.Empty;
                        break;
                    default:

                        break;
                }
             


            }
        }

        public void UpdateContentAppSetting(int dealershipId, string descriptionNew, string descriptionUsed, int category)
        {
            var searchResult = _context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);
            if (searchResult != null)
            {
                switch (category)
                {
                    case Constanst.DescriptionSettingCategory.EndSentence:
                        searchResult.EndDescriptionSentence = descriptionUsed;
                        searchResult.EndDescriptionSentenceForNew = descriptionNew;
                        break;
                    default:

                        break;
                }
              
            }
        }

        public void UpdateContentAppSetting(int dealershipId, string description, int category)
        {
            var searchResult = _context.Settings.FirstOrDefault(x => x.DealerId == dealershipId);
            if (searchResult != null)
            {
                switch (category)
                {
                    case Constanst.DescriptionSettingCategory.LoanerSentence:
                        searchResult.LoanerSentence = description;
                        break;
                    case Constanst.DescriptionSettingCategory.AuctionSentence:
                        searchResult.AuctionSentence = description;
                        break;
                    case Constanst.DescriptionSettingCategory.DealerWarranty:
                        searchResult.DealerWarranty = description;
                        break;
                    case Constanst.DescriptionSettingCategory.EndSentence:
                        searchResult.EndDescriptionSentence = description;
                        break;
                    case Constanst.DescriptionSettingCategory.EndSentenceForNew:
                        searchResult.EndDescriptionSentenceForNew = description;
                        break;
                    case Constanst.DescriptionSettingCategory.ShippingInfo:
                        searchResult.ShippingInfo = description;
                        break;
                    case Constanst.DescriptionSettingCategory.StartSentence:
                        searchResult.StartDescriptionSentence = description;
                        break;
                    case Constanst.DescriptionSettingCategory.TermConditon:
                        searchResult.TermsAndCondition = description;
                        break;
                    default:

                        break;
                }
            }
        }

        public void UpdateButtonPermission(int dealerId,int groupId, int buttonId, bool canSee)
        {
            var existingPermission =
                  _context.DealerButtons.FirstOrDefault(
                      i =>
                      i.DealerId == dealerId && i.GroupId == groupId &&
                      i.ButtonId == buttonId);
            if (existingPermission != null)
            {
                existingPermission.CanSee = canSee;
                
            }
            else
            {
                var newPermissoin = new DealerButton
                {
                    ButtonId = buttonId,
                    GroupId = groupId,
                    DealerId = dealerId,
                    CanSee = canSee
                };
                _context.AddToDealerButtons(newPermissoin);
               
            }
        }

        #endregion
    }
}
