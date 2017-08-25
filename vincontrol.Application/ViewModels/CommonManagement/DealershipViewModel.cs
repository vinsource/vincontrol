using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class DealershipViewModel
    {
        public DealershipViewModel()
        {
            DealerCraigslistSetting = new DealerCraigslistSetting();
        }

        public DealershipViewModel(Dealer existingDealer)
        {
            if (existingDealer != null)
            {
                DealershipId = existingDealer.DealerId;
                DealerGroupId = String.IsNullOrEmpty(existingDealer.DealerGroupId) ? "" : existingDealer.DealerGroupId;
                Address = existingDealer.Address;
                City = existingDealer.City;
                ZipCode = existingDealer.ZipCode;
                State = existingDealer.State ?? "CA";
                Latitude = existingDealer.Lattitude ?? 0;
                Longtitude = existingDealer.Longtitude ?? 0;
                DealershipName = existingDealer.Name;
                DealershipAddress = existingDealer.Address;
                Phone = existingDealer.Phone;
                Email = existingDealer.Email;
                Logo = existingDealer.Logo;
                Region = existingDealer.Region;
                
                string pendragonStore = null;
                try
                {
                     pendragonStore = ConfigurationManager.AppSettings["Pendragon"].ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception e) { }
                IsPendragon = pendragonStore!=null && DealerGroupId.Equals(pendragonStore);

                var pendragonWholesale = new int[] {};
                try
                {
                    pendragonWholesale = ConfigurationManager.AppSettings["PendragonWholesale"] != null
                        ? Array.ConvertAll(
                            ConfigurationManager.AppSettings["PendragonWholesale"].ToString(CultureInfo.InvariantCulture)
                                .Split(','), int.Parse)
                        : new int[] {};
                }
                catch (Exception e) { }
                IsPendragonWholesale = pendragonWholesale.Contains(DealershipId);

                DealerSetting = existingDealer.Setting != null ? new DealerSettingViewModel(existingDealer.Setting) : new DealerSettingViewModel();
                if (existingDealer.Setting != null)
                {
                    EmailFormat = existingDealer.Setting.EmailFormat.GetValueOrDefault();
                    FacebookUrl = existingDealer.Setting.FacebookUrl;
                }
                DealerCraigslistSetting = existingDealer.CraigslistSettings.Any() ? new DealerCraigslistSetting(existingDealer.CraigslistSettings.First()) : new DealerCraigslistSetting();

                if (existingDealer.NotificationSetting != null)
                {
                    NotificationSetting = new NotificationViewModel(existingDealer.NotificationSetting);
                }
                
                DealerHours = new List<DealerHourSetting>();
                foreach (var tmp in existingDealer.DealerHours)
                {
                    var dealerTmpHourSetting = new DealerHourSetting()
                    {
                        Day = tmp.Day,
                        Hours = tmp.Hours
                    };

                    DealerHours.Add(dealerTmpHourSetting);
                }
            }
        }

        public string DealershipName { get; set; }

        public string DealershipPhoneNumber { get; set; }

        public string DealershipAddress { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int DealershipId { get; set; }

        public string DealerGroupId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longtitude { get; set; }

        public DealerSettingViewModel DealerSetting { get; set; }
        public NotificationViewModel NotificationSetting { get; set; }
        public DealerCraigslistSetting DealerCraigslistSetting { get; set; }
        public string EncryptDealerId { get; set; }

        public string EbayInventoryUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string Logo { get; set; }
        public bool IsPendragon { get; set; }
        public bool IsPendragonWholesale { get; set; }
        public int EmailFormat { get; set; }
        public string Region { get; set; }
        public string FacebookUrl { get; set; }
        public List<DealerHourSetting> DealerHours { get; set; }

    }

    public class NotificationViewModel
    {
        public NotificationViewModel()
        { }

        public NotificationViewModel(NotificationSetting setting)
        {
            AppraisalNotification = setting.AppraisalNotified;
            WholeSaleNotfication = setting.WholesaleNotified;
            InventoryNotfication = setting.InventoryNotified;
            TwentyFourHourNotification = setting.C24hNotified;
            NoteNotification = setting.NoteNotified;
            PriceChangeNotification = setting.PriceChangeNotified;
            MarketPriceRangeChangeNotification = setting.MarketPriceRangeNotified;
            AgeingBucketJumpNotification = setting.AgingNotified;
            ImageUploadNotification = setting.ImageUploadNotified;
            BucketJumpReportNotification = setting.BucketJumpNotified;

        }

        public bool AppraisalNotification { get; set; }
        public bool WholeSaleNotfication { get; set; }
        public bool InventoryNotfication { get; set; }
        public bool TwentyFourHourNotification { get; set; }
        public bool NoteNotification { get; set; }
        public bool PriceChangeNotification { get; set; }
        public bool MarketPriceRangeChangeNotification { get; set; }
        public bool AgeingBucketJumpNotification { get; set; }
        public bool ImageUploadNotification { get; set; }
        public bool BucketJumpReportNotification { get; set; }
    }

    public class DealerSettingViewModel
    {
        public DealerSettingViewModel() { }

        public DealerSettingViewModel(Setting setting)
        {
            EmailFormat = setting.EmailFormat.GetValueOrDefault();
            PriceVariance = setting.PriceVariance.GetValueOrDefault();
            InventorySorting = setting.InventorySorting;
            SoldOut = setting.SoldOut;
            DefaultStockImageUrl = setting.DefaultStockImageUrl;
            OverrideStockImage = setting.OverideStockImage.GetValueOrDefault();
            DealerInfo = setting.DealerInfo;
            DealerWarranty = setting.DealerWarranty;
            TermConditon = setting.TermsAndCondition;
            EbayToken = setting.EbayToken;
            EbayInventoryUrl = setting.EbayInventoryUrl;
            CreditUrl = setting.CreditUrl;
            WebSiteUrl = setting.WebSiteUrl;
            ContactUsUrl = setting.ContactUsUrl;
            FacebookUrl = setting.FacebookUrl;
            LogoUrl = setting.LogoUrl;
            ContactPerson = setting.ContactPerson;

            CarFax = setting.CarFax;
            CarFaxPassword = setting.CarFaxPassword;
            Manheim = setting.Manheim;
            ManheimPassword = setting.ManheimPassword;

            Ebay = setting.Ebay;
            EbayPassword = setting.EbayPassword;

            KellyBlueBook = setting.KellyBlueBook;
            KellyPassword = setting.KellyPassword;
            BlackBook = setting.BlackBook;
            BlackBookPassword = setting.BlackBookPassword;
            EnableAutoDescription = setting.AutoDescription.GetValueOrDefault();

            YoutubeUsername = setting.YoutubeUsername;
            YoutubePassword = setting.YoutubePassword;

            EndSentence = setting.EndDescriptionSentence;
            EndSentenceForNew = setting.EndDescriptionSentenceForNew;
            StartSentence = setting.StartDescriptionSentence;

            IntervalBucketJump = setting.IntervalBucketJump.GetValueOrDefault();
            LoanerSentence = setting.LoanerSentence;
            AuctionSentence = setting.AuctionSentence;
            CanViewBucketJumpReport = setting.CanViewBucketJump.GetValueOrDefault();
            
            AutoCheck = setting.AutoCheck;
            AutoCheckPassword = setting.AutoCheckPassword;

            FirstTimeRangeBucketJump = setting.FirstTimeRangeBucketJump ?? 0;
            SecondTimeRangeBucketJump = setting.SecondTimeRangeBucketJump ?? 0;
            
            OverideStockImage = setting.OverideStockImage ?? false;
            AutoDescription = setting.AutoDescription ?? false;
            AutoDescriptionSubscribe = setting.AutoDescriptionSubscribe ?? false;

            RetailPriceWsNotification = setting.RetailPrice ?? false;
            DealerDiscountWSNotification = setting.DealerDiscount ?? false;
            ManufacturerReabteWsNotification = setting.ManufacturerReabte ?? false;
            SalePriceWsNotification = setting.SalePrice ?? false;
            RetailPriceWSNotificationText = setting.RetailPriceText;
            DealerDiscountWSNotificationText = setting.DealerDiscountText;
            ManufacturerReabteWsNotificationText = setting.ManufactureReabateText;
            SalePriceWsNotificationText = setting.SalePriceText;
            ShippingInfo = setting.ShippingInfo;
            AutoCheck = setting.AutoCheck;
            AutoCheckPassword = setting.AutoCheckPassword;
            EbayContactInfoName = setting.EbayContactInfoName;
            EbayContactInfoPhone = setting.EbayContactInfoPhone;
            EbayContactInfoEmail = setting.EbayContactInfoEmail;

            VehicleTireCost = setting.TireCost ?? 0;
            FrontBumperCost = setting.FrontBumperCost ?? 0;
            RearBumperCost = setting.RearBumperCost ?? 0;
            GlassCost = setting.GlassCost ?? 0;
            FrontEndCost = setting.FrontEndCost ?? 0;
            RearEndCost = setting.RearEndCost ?? 0;
            DriverSideCost = setting.DriverSideCost ?? 0;
            PassengerSideCost = setting.PassengerSideCost ?? 0;
            LightBulbCost = setting.LightBulbCost ?? 0;
            VehicleTireRetail = setting.TireRetail ?? 0;
            FrontBumperRetail = setting.FrontBumperRetail ?? 0;
            RearBumperRetail = setting.RearBumperRetail ?? 0;
            GlassRetail = setting.GlassRetail ?? 0;
            FrontEndRetail = setting.FrontEndRetail ?? 0;
            RearEndRetail = setting.RearEndRetail ?? 0;
            DriverSideRetail = setting.DriverSideRetail ?? 0;
            PassengerSideRetail = setting.PassengerSideRetail ?? 0;
            LightBulbRetail = setting.LightBulbRetail ?? 0;

            FrontWindShieldRetail = setting.FrontWindShieldRetail ?? 0;
            RearWindShieldRetail = setting.RearWindShieldRetail ?? 0;
            DriverWindowRetail = setting.DriverWindowRetail ?? 0;
            PassengerWindowRetail = setting.PassengerWindowRetail ?? 0;
            DriverSideMirrorRetail = setting.DriverSideMirrorRetail ?? 0;
            PassengerSideMirrorRetail = setting.PassengerSideMirrorRetail ?? 0;
            ScratchRetail = setting.ScratchRetail ?? 0;
            MajorScratchRetail = setting.MajorScratchRetail ?? 0;
            DentRetail = setting.DentRetail ?? 0;
            MajorDentRetail = setting.MajorDentRetail ?? 0;
            PaintDamageRetail = setting.PaintDamageRetail ?? 0;
            RepaintedPanelRetail = setting.RepaintedPanelRetail ?? 0;
            RustRetail = setting.RustRetail ?? 0;
            AcidentRetail = setting.AcidentRetail ?? 0;
            MissingPartsRetail = setting.MissingPartsRetail ?? 0;

            FrontWindShieldCost = setting.FrontWindShieldCost ?? 0;
            RearWindShieldCost = setting.RearWindShieldCost ?? 0;
            DriverWindowCost = setting.DriverWindowCost ?? 0;
            PassengerWindowCost = setting.PassengerWindowCost ?? 0;
            DriverSideMirrorCost = setting.DriverSideMirrorCost ?? 0;
            PassengerSideMirrorCost = setting.PassengerSideMirrorCost ?? 0;
            ScratchCost = setting.ScratchCost ?? 0;
            MajorScratchCost = setting.MajorScratchCost ?? 0;
            DentCost = setting.DentCost ?? 0;
            MajorDentCost = setting.MajorDentCost ?? 0;
            PaintDamageCost = setting.PaintDamageCost ?? 0;
            RepaintedPanelCost = setting.RepaintedPanelCost ?? 0;
            RustCost = setting.RustCost ?? 0;
            AcidentCost = setting.AcidentCost ?? 0;
            MissingPartsCost = setting.MissingPartsCost ?? 0;

            BrandName = setting.BrandName ?? string.Empty;

            AverageCost = setting.AverageCost == null ? 500 : setting.AverageCost.GetValueOrDefault();

            AverageProfit = setting.AverageProfit == null ? 0 : setting.AverageProfit.GetValueOrDefault();

            AverageProfitPercentage = setting.AverageProfitPercentage == null ? 12 : setting.AverageProfitPercentage.GetValueOrDefault();

            AverageProfitUsage = setting.AverageProfitUsage == null ? (short)2 : setting.AverageProfitUsage.GetValueOrDefault();

            HeaderOverlayUrl = setting.HeaderOverlayURL;
            FooterOverlayUrl = setting.FooterOverlayURL;
        }

        public string ShippingInfo { get; set; }

        [Required]
        public string InventorySorting { get; set; }

        public string DefaultStockImageUrl { get; set; }

        public string SoldOut { get; set; }

        public bool ValidUser { get; set; }

        public bool OverrideStockImage { get; set; }

        public bool OverrideDealerKbbReport { get; set; }

        public bool DefaultLogin { get; set; }

        public string PayPalEmailAddress { get; set; }

        public string DealerInfo { get; set; }

        public string DealerWarranty { get; set; }

        public string TermConditon { get; set; }

        public string EbayToken { get; set; }

        public string EbayInventoryUrl { get; set; }

        public string CreditUrl { get; set; }

        public string WebSiteUrl { get; set; }

        public string ContactUsUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string LogoUrl { get; set; }

        public string ContactPerson { get; set; }

        public string CarFax { get; set; }

        [DataType(DataType.Password)]
        public string CarFaxPassword { get; set; }

        public string Manheim { get; set; }

        [DataType(DataType.Password)]
        public string ManheimPassword { get; set; }

        public string KellyBlueBook { get; set; }

        [DataType(DataType.Password)]
        public string KellyPassword { get; set; }

        public string BlackBook { get; set; }

        [DataType(DataType.Password)]
        public string BlackBookPassword { get; set; }

        public string YoutubeUsername { get; set; }

        [DataType(DataType.Password)]
        public string YoutubePassword { get; set; }

        public string StartSentence { get; set; }

        public string EndSentence { get; set; }

        public string EndSentenceForNew { get; set; }

        public string AuctionSentence { get; set; }

        public string LoanerSentence { get; set; }

        public string EncryptDealerId { get; set; }

        public short EmailFormat { get; set; }

        public bool EnableAutoDescription { get; set; }

        public bool CanViewBucketJumpReport { get; set; }

        public decimal PriceVariance { get; set; }

        public int IntervalBucketJump { get; set; }
        public string Ebay { get; set; }
        public string EbayPassword { get; set; }
        public string AutoCheck { get; set; }
        public string AutoCheckPassword { get; set; }
        public string EbayContactInfoName { get; set; }
        public string EbayContactInfoPhone { get; set; }
        public string EbayContactInfoEmail { get; set; }
        public int FirstTimeRangeBucketJump { get; set; }
        public int SecondTimeRangeBucketJump { get; set; }

        public bool OverideStockImage { get; set; }
        public bool AutoDescription { get; set; }
        public bool AutoDescriptionSubscribe { get; set; }

        public bool RetailPriceWsNotification { get; set; }
        public bool DealerDiscountWSNotification { get; set; }
        public bool ManufacturerReabteWsNotification { get; set; }
        public bool SalePriceWsNotification { get; set; }
        public string RetailPriceWSNotificationText { get; set; }
        public string DealerDiscountWSNotificationText { get; set; }
        public string ManufacturerReabteWsNotificationText { get; set; }
        public string SalePriceWsNotificationText { get; set; }

        public decimal AverageCost { get; set; }
        public decimal AverageProfit { get; set; }
        public decimal AverageProfitPercentage { get; set; }
        public short AverageProfitUsage { get; set; }
        public string BrandName { get; set; }
        public decimal VehicleTireCost { get; set; }
        public decimal FrontBumperCost { get; set; }
        public decimal RearBumperCost { get; set; }
        public decimal GlassCost { get; set; }
        public decimal FrontEndCost { get; set; }
        public decimal RearEndCost { get; set; }
        public decimal DriverSideCost { get; set; }
        public decimal PassengerSideCost { get; set; }
        public decimal LightBulbCost { get; set; }
        public decimal VehicleTireRetail { get; set; }
        public decimal FrontBumperRetail { get; set; }
        public decimal RearBumperRetail { get; set; }
        public decimal GlassRetail { get; set; }
        public decimal FrontEndRetail { get; set; }
        public decimal RearEndRetail { get; set; }
        public decimal DriverSideRetail { get; set; }
        public decimal PassengerSideRetail { get; set; }
        public decimal LightBulbRetail { get; set; }

        public decimal FrontWindShieldCost { get; set; }
        public decimal RearWindShieldCost { get; set; }
        public decimal DriverWindowCost { get; set; }
        public decimal PassengerWindowCost { get; set; }
        public decimal DriverSideMirrorCost { get; set; }
        public decimal PassengerSideMirrorCost { get; set; }
        public decimal ScratchCost { get; set; }
        public decimal MajorScratchCost { get; set; }
        public decimal DentCost { get; set; }
        public decimal MajorDentCost { get; set; }
        public decimal PaintDamageCost { get; set; }
        public decimal RepaintedPanelCost { get; set; }
        public decimal RustCost { get; set; }
        public decimal AcidentCost { get; set; }
        public decimal MissingPartsCost { get; set; }

        public decimal FrontWindShieldRetail { get; set; }
        public decimal RearWindShieldRetail { get; set; }
        public decimal DriverWindowRetail { get; set; }
        public decimal PassengerWindowRetail { get; set; }
        public decimal DriverSideMirrorRetail { get; set; }
        public decimal PassengerSideMirrorRetail { get; set; }
        public decimal ScratchRetail { get; set; }
        public decimal MajorScratchRetail { get; set; }
        public decimal DentRetail { get; set; }
        public decimal MajorDentRetail { get; set; }
        public decimal PaintDamageRetail { get; set; }
        public decimal RepaintedPanelRetail { get; set; }
        public decimal RustRetail { get; set; }
        public decimal AcidentRetail { get; set; }
        public decimal MissingPartsRetail { get; set; }

        public string HeaderOverlayUrl { get; set; }
        public string FooterOverlayUrl { get; set; }
    }

    public class DealerCraigslistSetting
    {
        public DealerCraigslistSetting()
        {
            Email = string.Empty;
            Password = string.Empty;
            State = string.Empty;
            Location = string.Empty;
            LocationId = 0;
            City = string.Empty;
            CityUrl = "0";
        }

        public DealerCraigslistSetting(vincontrol.Data.Model.CraigslistSetting obj)
        {
            CraigslistSettingId = obj.CraigslistSettingId;
            DealerId = obj.DealerId;
            Email = obj.Email;
            Password = obj.Password;
            EncodePassword = EncryptString(obj.Password);
            SpecificLocation = obj.SpecificLocation;
            State = obj.State;
            City = obj.City;
            CityUrl = obj.CityUrl;
            Location = obj.Location;
            LocationId = obj.LocationId.GetValueOrDefault();
            EndingSentence = obj.EndingSentence;
            States = new List<StateViewModel>();
        }

        public int CraigslistSettingId { get; set; }
        public int DealerId { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string EncodePassword { get; set; }
        public bool PasswordChanged { get; set; }

        public string SpecificLocation { get; set; }
        public string State { get; set; }

        public string City { get; set; }
        public string CityUrl { get; set; }

        public string Location { get; set; }
        public int LocationId { get; set; }
        public string EndingSentence { get; set; }
        public List<StateViewModel> States { get; set; }

        private static string EncryptString(string clearText)
        {
            if (!string.IsNullOrEmpty(clearText))
            {

                byte[] clearTextBytes = Encoding.UTF8.GetBytes(clearText);

                var rijn = SymmetricAlgorithm.Create();

                var ms = new MemoryStream();
                byte[] rgbIV = Encoding.ASCII.GetBytes("ayojvlzmdalyglrj");
                byte[] key = Encoding.ASCII.GetBytes("hlxilkqbbhczfeultgbskdmaunivmfuo");
                var cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV),
                                          CryptoStreamMode.Write);

                cs.Write(clearTextBytes, 0, clearTextBytes.Length);

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            return string.Empty;
        }
    }


    public class DealerHourSetting
    {
        public string Day { get; set; }

        public string Hours { get; set; }
    }
}
