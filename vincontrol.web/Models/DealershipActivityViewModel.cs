using System;
using vincontrol.Constant;
using vincontrol.Data.Model;

namespace Vincontrol.Web.Models
{
    public class DealershipActivityViewModel
    {
        public DealershipActivityViewModel(){}

        public DealershipActivityViewModel(InventoryDealerActivity obj)
        {
            Id = obj.InventoryDealerActivityId;
            DealerId = obj.DealerId;
            UserStamp = obj.User.Name;
            DateStamp = obj.DateStamp;
            DealerActivityTypeCode = Constanst.ActivityTypeId.Inventory;
            DealerActivitySubTypeCode = obj.DealerActivitySubTypeCodeId;
            Title = GetTitle(obj);
            //Detail = obj.Detail;
            Activity = obj.DealerActivitySubTypeCode.Value;
            Vin = obj.VIN;
            Stock = obj.Stock;
            Vehicle = GetVehicleInformation(obj);
            //OldPrice = obj.OldPrice??0;
            //NewPrice = obj.NewPrice??0;
        }

        public DealershipActivityViewModel(FlyerShareDealerActivity obj)
        {
            Id = obj.FlyerShareDealerActivityId;
            DealerId = obj.DealerId;
            UserStamp = obj.User.Name;
            DateStamp = obj.DateStamp;
            DealerActivityTypeCode = Constanst.ActivityTypeId.ShareFlyer;
            //DealerActivitySubTypeCode = obj.DealerActivitySubTypeCodeId;
            Title = GetTitle(obj);
            //Detail = obj.Detail;
            //Activity = obj.DealerActivitySubTypeCode.Value;
            Vin = obj.VIN;
            Stock = obj.Stock;
            Vehicle = GetVehicleInformation(obj);
            //OldPrice = obj.OldPrice??0;
            //NewPrice = obj.NewPrice??0;
        }

        private string GetVehicleInformation(FlyerShareDealerActivity inventoryDealerActivity)
        {
            return String.Format("{0} {1} {2} {3}", inventoryDealerActivity.Year, inventoryDealerActivity.Make, inventoryDealerActivity.Model, inventoryDealerActivity.Trim);

        }

        private string GetTitle(FlyerShareDealerActivity flyerShareDealerActivity)
        {
            if (flyerShareDealerActivity.IsBrochure)
                return string.Format("Send brochure To {0} with email {1}", flyerShareDealerActivity.CustomerName, flyerShareDealerActivity.CustomerEmail);
            else
                return string.Format("Send flyer To {0} with email {1}", flyerShareDealerActivity.CustomerName, flyerShareDealerActivity.CustomerEmail);
        }

        private string GetVehicleInformation(InventoryDealerActivity inventoryDealerActivity)
        {
            return String.Format("{0} {1} {2} {3}", inventoryDealerActivity.Year, inventoryDealerActivity.Make, inventoryDealerActivity.Model, inventoryDealerActivity.Trim);
        }

        private string GetTitle(InventoryDealerActivity inventoryDealerActivity)
        {
            switch (inventoryDealerActivity.DealerActivitySubTypeCodeId)
            {
                case Constanst.SubActivityType.AddToInventory:
                    return string.Format("Add To Inventory {0}", GetVehicleInformation(inventoryDealerActivity));
                case Constanst.SubActivityType.PriceChange:
                    if (inventoryDealerActivity.OldPrice != null && inventoryDealerActivity.NewPrice != null &&
                        !string.IsNullOrEmpty(GetVehicleInformation(inventoryDealerActivity).Trim()))
                    {
                        return string.Format("Price Change from {0} to {1} for {2}", inventoryDealerActivity.OldPrice.Value.ToString("C0"),
                                             inventoryDealerActivity.NewPrice.Value.ToString("C0"),
                                             GetVehicleInformation(inventoryDealerActivity));
                    }
                    else if (inventoryDealerActivity.NewPrice != null &&
                        !string.IsNullOrEmpty(GetVehicleInformation(inventoryDealerActivity).Trim()))
                    {
                        return string.Format("Price Change from 0 to {0} for {1}",
                                             inventoryDealerActivity.NewPrice.Value.ToString("C0"),
                                             GetVehicleInformation(inventoryDealerActivity));
                    }
                    else
                    {
                        return string.Empty;
                    }
            }
            return String.Empty;
        }



        public DealershipActivityViewModel(AppraisalDealerActivity obj)
        {
            Id = obj.AppraisalDealerActivityId;
            DealerId = obj.DealerId;
            UserStamp = obj.User.Name;
            DateStamp = obj.DateStamp;
            DealerActivityTypeCode = Constanst.ActivityTypeId.Appraisal;
            DealerActivitySubTypeCode = obj.DealerActivitySubTypeCodeId;
            Title = GetTitle(obj);
            //Detail = obj.Detail;
            Activity = obj.DealerActivitySubTypeCode.Value;
            Vin = obj.VIN;
            Stock = obj.Stock;
            Vehicle = GetVehicleInformation(obj);
        }

        private string GetVehicleInformation(AppraisalDealerActivity inventoryDealerActivity)
        {
            return String.Format("{0} {1} {2} {3}", inventoryDealerActivity.Year, inventoryDealerActivity.Make, inventoryDealerActivity.Model, inventoryDealerActivity.Trim);
        }

        private string GetTitle(AppraisalDealerActivity inventoryDealerActivity)
        {
            return String.Format("New Appraisal {0}", GetVehicleInformation(inventoryDealerActivity) );
        }

        public DealershipActivityViewModel(UserDealerActivity obj)
        {
            Id = obj.UserDealerActivityId;
            DealerId = obj.DealerId;
            UserStamp = obj.User1 != null ? obj.User1.Name : string.Empty;
            DateStamp = obj.DateStamp;
            DealerActivityTypeCode = Constanst.ActivityTypeId.User;
            DealerActivitySubTypeCode = obj.DealerActivitySubTypeCodeId;
            Title = GetTitle(obj);
            Activity = obj.DealerActivitySubTypeCode.Value;
        }

        private string GetTitle(UserDealerActivity userDealerActivity)
        {
            switch (userDealerActivity.DealerActivitySubTypeCodeId)
            {
                case Constanst.SubActivityType.NewUser:
                {
                    return String.Format("New User {0} with {1} Role", userDealerActivity.UserName, userDealerActivity.Role);
                }
                case Constanst.SubActivityType.ChangePassword:
                {
                    return "Change password";
                }
            }

            return string.Empty;
        }

        public int Id { get; set; }
        public int DealerId { get; set; }
        public string UserStamp { get; set; }
        public DateTime DateStamp { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public short DealerActivityTypeCode { get; set; }
        public short DealerActivitySubTypeCode { get; set; }
        public string Activity { get; set; }
        public string ActivityContent { get; set; }
        public string Vin { get; set; }
        public string Vehicle { get; set; }
        public string Stock { get; set; }
        //public decimal OldPrice { get; set; }
        //public decimal NewPrice { get; set; }
       
    }
}