using System.Data.Objects;
using System.Linq;
using vincontrol.Backend.Data.Interface;
using vincontrol.Backend.Data.Repository.Interface;

namespace vincontrol.Backend.Data.Repository.Implementation
{
    public class DealerSettingRepository : IDealerSettingRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public DealerSettingRepository()
        {
           
        }

        #region IDealerSettingRepository Members

        //public whitmanenterprisesetting GetDealerSettingByDealerId(int dealerId)
        //{
        //    return _unitOfWork.DealerSetting.Query(i => i.DealershipId == dealerId).FirstOrDefault();
        //}

        //public void UpdateNotification(int dealerId, bool notify, int type)
        //{
        //    var setting = _unitOfWork.DealerSetting.Query(i => i.DealershipId == dealerId).FirstOrDefault();
            
        //    if (setting != null)
        //    {
        //        switch (type)
        //        {
        //            case 0:
        //                setting.AppraisalNotification = notify;
        //                break;
        //            case 1:
        //                setting.WholeNotification = notify;
        //                break;
        //            case 2:
        //                setting.InventoryNotification = notify;
        //                break;
        //            case 3:
        //                setting.C24Hnotification = notify;
        //                break;
        //            case 4:
        //                setting.NoteNotification = notify;
        //                break;
        //            case 5:
        //                setting.PriceChangeNotification = notify;
        //                break;
        //            case 6:
        //                setting.AgeingBucketNotification = notify;
        //                break;
        //            case 7:
        //                setting.MarketPriceRangeChangeNotification = notify;
        //                break;
        //        }

        //        _unitOfWork.Commit();
        //    }
        //}

        //public void UpdateNotificationPerUser(string userName, int dealerId, bool notify, int type)
        //{
            
        //    var alert = _unitOfWork.UserAlert.Query(i => i.user.DefaultDealership == dealerId && i.user.UserName == userName && i.AlertId == type + 1).FirstOrDefault();
        //    if(alert == null && notify)
        //    {
        //        var newAlert = new useralert()
        //                           {
        //                               UserId = _unitOfWork.User.Query(i => i.DefaultDealership == dealerId && i.UserName == userName).FirstOrDefault().Id,
        //                               AlertId = type + 1
        //                           };
        //        _unitOfWork.UserAlert.Add(newAlert);
        //    }

        //    if (alert != null && !notify)
        //        _unitOfWork.UserAlert.Remove(alert);

        //    _unitOfWork.Commit();
        //}

        //public void UpdateWindowStickerNotification(int dealerId, bool notify, int type)
        //{
        //    var setting = GetDealerSettingByDealerId(dealerId);
        //    if (setting == null) return;
            
        //    switch (type)
        //    {
        //        case 1:
        //            setting.RetailPrice = notify;
        //            break;
        //        case 2:
        //            setting.DealerDiscount = notify;
        //            break;
        //        case 3:
        //            setting.ManufacturerReabte = notify;
        //            break;
        //        case 4:
        //            setting.SalePrice = notify;
        //            break;
        //    }

        //    _unitOfWork.Commit();
        //}

        //public void UpdateOverideStockImage(int dealerId, bool overideStockImage)
        //{
        //    var setting = GetDealerSettingByDealerId(dealerId);
        //    if (setting == null) return;

        //    setting.OverideStockImage = overideStockImage;
        //    _unitOfWork.Commit();
        //}

        //public void UpdateDefaultStockImageUrl(int dealerId, string stockImageUrl)
        //{
        //    var setting = GetDealerSettingByDealerId(dealerId);
        //    if (setting == null) return;

        //    setting.DefaultStockImageUrl = stockImageUrl;
        //    _unitOfWork.Commit();
        //}

        #endregion
    }
}
