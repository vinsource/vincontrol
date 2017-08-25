using vincontrol.Constant;
using vincontrol.Helper;
using Vincontrol.Web.HelperClass;
namespace Vincontrol.Web.Handlers
{
    public class UserRightSetting
    {
        public bool InventoryTab { get; set; }
        public bool AppraisalsTab { get; set; }
        public bool KPITab { get; set; }
        public bool MarketSearchTab { get; set; }
        public bool ActivityTab { get; set; }
        public bool AdminTab { get; set; }
        public bool ReportsTab { get; set; }

        public InventoryUserRight Inventory { get; set; }
        public AppraisalsUserRight Appraisals { get; set; }
        public KPIUserRight KPI { get; set; }
        public MarketSearchUserRight MarketSearch { get; set; }
        public ActivityUserRight Activity { get; set; }
        public AdminUserRight Admin { get; set; }
        public ReportsUserRight Reports { get; set; }

        // default settings corresponding to role
        public UserRightSetting(int role)
        {
            InventoryTab = true;
            AppraisalsTab = true;
            KPITab = true;
            MarketSearchTab = true;
            ActivityTab = true;
            AdminTab = true;
            ReportsTab = true;

            if (role == Constanst.RoleType.Employee)
            {
                KPITab = false;
                MarketSearchTab = false;
                ActivityTab = false;
                AdminTab = false;
                ReportsTab = false;
            }
            else
            {
                if (role == Constanst.RoleType.Manager)
                {
                    AdminTab = false;
                }
            }

            Inventory = new InventoryUserRight(role);
            Appraisals = new AppraisalsUserRight(role);
            KPI = new KPIUserRight(role);
            MarketSearch = new MarketSearchUserRight(role);
            Activity = new ActivityUserRight(role);
            Admin = new AdminUserRight(role);
            Reports = new ReportsUserRight(role);
        }

        public void UpdateInventorySettingsFromDatabase()
        {
            Inventory.UpdateSettings();
        }

        public void UpdateAppraisalsSettingsFromDatabase()
        {
            Appraisals.UpdateSettings();
        }

        public void UpdateKPISettingsFromDatabase()
        {
            KPI.UpdateSettings();
        }

        public void UpdateMarketSearchSettingsFromDatabase()
        {
            MarketSearch.UpdateSettings();
        }

        public void UpdateActivitySettingsFromDatabase()
        {
            Activity.UpdateSettings();
        }

        public void UpdateAdminSettingsFromDatabase()
        {
            Admin.UpdateSettings();
        }

        public void UpdateReportsSettingsFromDatabase()
        {
            Reports.UpdateSettings();
        }

        public void UpdateAllSettingsFromDatabase()
        {
            UpdateInventorySettingsFromDatabase();
            UpdateAppraisalsSettingsFromDatabase();
            UpdateKPISettingsFromDatabase();
            UpdateMarketSearchSettingsFromDatabase();
            UpdateActivitySettingsFromDatabase();
            UpdateAdminSettingsFromDatabase();
            UpdateReportsSettingsFromDatabase();
        }
    }

    public class InventoryUserRight
    {
        // default settings corresponding to role
        public InventoryUserRight(int role)
        {
            if (role == Constanst.RoleType.Employee)
            {
                Used = true;        // readonly???
                New = true;         // readonly???
                Loaner = true;      // readonly???
                Auction = true;     // readonly???
                Recon = true;       // readonly???
                WholeSale = false;
                TradeNotClear = false;
                SoldCar = false;

                ViewProfile_EditProfile = true;     // readonly???
                ViewProfile_Ebay = false;
                ViewProfile_WS = true;
                ViewProfile_BG = true;
                ViewProfile_Transfer = false;
                ViewProfile_Status = false;
                ViewProfile_PriceTracking = false;
                ViewProfile_BucketJump = false;

                ACV = false;
                DealerCost = false;
                Craigslist = false;
            }
            else
            {
                Used = true;
                New = true;
                Loaner = true;
                Auction = true;
                Recon = true;
                WholeSale = true;
                TradeNotClear = true;
                SoldCar = true;

                ViewProfile_EditProfile = true;
                ViewProfile_Ebay = true;
                ViewProfile_WS = true;
                ViewProfile_BG = true;
                ViewProfile_Transfer = true;
                ViewProfile_Status = true;
                ViewProfile_PriceTracking = true;
                ViewProfile_BucketJump = true;

                ACV = true;
                DealerCost = true;
                Craigslist = true;
            }
        }

        public void UpdateSettings()
        {
            UpdateButtonSettings();
        }

        private void UpdateButtonSettings()
        {
            if (SessionHandler.CurrentUser.ProfileButtonPermissions != null)
            {
                foreach (var button in SessionHandler.CurrentUser.ProfileButtonPermissions.Buttons)
                {
                    switch (button.ButtonName)
                    {
                        case Constanst.ProfileButton.EditProfile:
                            ViewProfile_EditProfile = button.CanSee;
                            break;
                        case Constanst.ProfileButton.Ebay:
                            ViewProfile_Ebay = button.CanSee;
                            break;
                        case Constanst.ProfileButton.WS:
                            ViewProfile_WS = button.CanSee;
                            break;
                        case Constanst.ProfileButton.BG:
                            ViewProfile_BG = button.CanSee;
                            break;
                        case Constanst.ProfileButton.Transfer:
                            ViewProfile_Transfer = button.CanSee;
                            break;
                        case Constanst.ProfileButton.Status:
                            ViewProfile_Status = button.CanSee;
                            break;
                        case Constanst.ProfileButton.PriceTracking:
                            ViewProfile_PriceTracking = button.CanSee;
                            break;
                        case Constanst.ProfileButton.BucketJumpTracking:
                            ViewProfile_BucketJump = button.CanSee;
                            break;
                        case Constanst.ProfileButton.ACV:
                            ACV = button.CanSee;
                            break;
                        case Constanst.ProfileButton.DealerCost:
                            DealerCost = button.CanSee;
                            break;
                        case Constanst.ProfileButton.Craigslist:
                            Craigslist = button.CanSee;
                            break;
                            
                    }
                }
            }
        }

        public bool Used { get; set; }
        public bool New { get; set; }
        public bool Loaner { get; set; }
        public bool Auction { get; set; }
        public bool Recon { get; set; }
        public bool WholeSale { get; set; }
        public bool TradeNotClear { get; set; }
        public bool SoldCar { get; set; }
        public bool TodayBucketJump { get; set; }
        public bool ACars { get; set; }
        public bool MissingContent { get; set; }
        public bool NoContent { get; set; }

        public bool Used_TodayBucketJump { get; set; }
        public bool Used_ACars { get; set; }
        public bool Used_MissingContent { get; set; }
        public bool Used_NoContent { get; set; }

        public bool New_Brochure { get; set; }

        public bool SoldCar_New { get; set; }
        public bool SoldCar_Used { get; set; }

        public bool ViewProfile_EditProfile { get; set; }
        public bool ViewProfile_Ebay { get; set; }
        public bool ViewProfile_WS { get; set; }
        public bool ViewProfile_BG { get; set; }
        public bool ViewProfile_Transfer { get; set; }
        public bool ViewProfile_Status { get; set; }
        public bool ViewProfile_PriceTracking { get; set; }
        public bool ViewProfile_BucketJump { get; set; }
        
        public bool ACV { get; set; }
        public bool DealerCost { get; set; }
        public bool Craigslist { get; set; }
    }

    public class AppraisalsUserRight
    {
        // default settings corresponding to role
        public AppraisalsUserRight(int role)
        {
            Recent = true;
            Pending = true;
            Advisor = true;

            if (role == Constanst.RoleType.Employee)
            {
                Advisor = false;
            }
        }

        public void UpdateSettings()
        {
        }

        public bool Recent { get; set; }
        public bool Pending { get; set; }
        public bool Advisor { get; set; }
    }

    public class KPIUserRight
    {
        // default settings corresponding to role
        public KPIUserRight(int role)
        {
            PreOwned = true;
            New = true;
        }

        public void UpdateSettings()
        {
        }

        public bool PreOwned { get; set; }
        public bool New { get; set; }
    }

    public class MarketSearchUserRight
    {
        // default settings corresponding to role
        public MarketSearchUserRight(int role)
        {
        }

        public void UpdateSettings()
        {
        }
    }

    public class ActivityUserRight
    {
        // default settings corresponding to role
        public ActivityUserRight(int role)
        {
            Inventory = true;
            Appraisals = true;
            ShareFlyers = true;
            ShareBrochures = true;
            Users = true;
        }

        public void UpdateSettings()
        {
        }

        public bool Inventory { get; set; }
        public bool Appraisals { get; set; }
        public bool ShareFlyers { get; set; }
        public bool ShareBrochures { get; set; }
        public bool Users { get; set; }
    }

    public class AdminUserRight
    {
        // default settings corresponding to role
        public AdminUserRight(int role)
        {
            Content = true;
            Notifications = true;
            UserRights = true;
            Rebate = true;
            Credentials = true;
            StockingGuide = true;
        }

        public void UpdateSettings()
        {
        }

        public bool Content { get; set; }
        public bool Notifications { get; set; }
        public bool UserRights { get; set; }
        public bool Rebate { get; set; }
        public bool Credentials { get; set; }
        public bool StockingGuide { get; set; }
    }

    public class ReportsUserRight
    {
        // default settings corresponding to role
        public ReportsUserRight(int role)
        {
            Inventory = true;
            Appraisal = true;
            BucketJump = true;
            Print = true;
            Tracking = true;
        }

        public void UpdateSettings()
        {
        }

        public bool Inventory { get; set; }
        public bool Appraisal { get; set; }
        public bool BucketJump { get; set; }
        public bool Print { get; set; }
        public bool Tracking { get; set; }
    }
}