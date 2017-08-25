using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.DatabaseModel;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class EmailQueryHelpers
    {
        public static IEnumerable<string> GetEmailsForChangePriceNotification(int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var result =
                        from e in context.whitmanenterpriseusersnotifications
                        from et in context.whitmanenterpriseusers
                        where
                            e.DealershipId == dealerId && e.PriceChangeNotification.Value && et.Active.Value && e.UserName == et.UserName

                        select new
                        {
                            et.Email,

                        };

                return result.Select(x => x.Email).AsEnumerable();
            }
        }

        public static IEnumerable<string> GetEmailsForInventoryNotification(int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var result =
                        from e in context.whitmanenterpriseusersnotifications
                        from et in context.whitmanenterpriseusers
                        where
                            e.DealershipId == dealerId && e.InventoryNotification.Value && et.Active.Value && e.UserName == et.UserName

                        select new
                        {
                            et.Email,

                        };

                return result.Select(x => x.Email).AsEnumerable();
            }
        }
    }
}