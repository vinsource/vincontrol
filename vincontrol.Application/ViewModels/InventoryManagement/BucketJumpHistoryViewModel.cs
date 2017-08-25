using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.InventoryManagement
{
    public class BucketJumpHistoryViewModel
    {
        public BucketJumpHistoryViewModel(){}

        public BucketJumpHistoryViewModel(BucketJumpHistory obj)
        {
            BucketJumpHistoryId = obj.BucketJumpHistoryId;
            ListingId = obj.ListingId;
            DealerId = obj.DealerId.GetValueOrDefault();
            VIN = obj.VIN;
            Stock = obj.Stock;
            Store = obj.Store;
            UserStamp = obj.UserStamp;
            UserFullName = obj.UserFullName;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            AttachFile = obj.AttachFile;
            SalePrice = obj.SalePrice.GetValueOrDefault();
            RetailPrice = obj.RetailPrice.GetValueOrDefault();
            CertifiedAmount = obj.CertifiedAmount.GetValueOrDefault();
            MileageAdjustment = obj.MileageAdjustment.GetValueOrDefault();
            VehicleStatusId = (byte)obj.VehicleStatusCodeId;
            VehicleStatusName = obj.VehicleStatusCode != null ? obj.VehicleStatusCode.Value : string.Empty;
            BucketJumpDayAlert = obj.BucketJumpDayAlert.GetValueOrDefault();
            BucketJumpCompleteDate = obj.BucketJumpCompleteDate.GetValueOrDefault();
            DaysAged = obj.DaysAged.GetValueOrDefault();
            Note = obj.Note;
        }

        public int BucketJumpHistoryId { get; set; }
        public int DealerId { get; set; }
        public int ListingId { get; set; }
        public string VIN { get; set; }
        public string Stock { get; set; }
        public string Store { get; set; }
        public string UserStamp { get; set; }
        public string UserFullName { get; set; }
        public DateTime DateStamp { get; set; }
        public string AttachFile { get; set; }
        public decimal SalePrice { get; set; }
        public decimal RetailPrice { get; set; }
        public string VehicleStatusName { get; set; }
        public byte VehicleStatusId { get; set; }
        public decimal CertifiedAmount { get; set; }
        public decimal MileageAdjustment { get; set; }
        public int BucketJumpDayAlert { get; set; }
        public DateTime BucketJumpCompleteDate { get; set; }
        public string Note { get; set; }
        public int DaysAged { get; set; }
    }

    public class DailyBuckẹtumpHistoryViewModel
    {
        public DailyBuckẹtumpHistoryViewModel()
        {
            List = new List<BucketJumpHistoryViewModel>();
        }
        
        public string Store { get; set; }
        public List<BucketJumpHistoryViewModel> List { get; set; }
    }
}
