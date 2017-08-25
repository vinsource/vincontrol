using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Models;

namespace Vincontrol.Web.HelperClass
{
    public class SessionHelper
    {
        public static int CheckStockExist(string stock, InventoryFormViewModel inventory, DealershipViewModel dealer)
        {
            if (inventory.CarsList.Any(o => o.Stock.Contains(stock)))
            {
                int numberofResult = inventory.CarsList.Count(o => o.Stock.Contains(stock));
                return numberofResult;
            }

            return 0;
        }
        
        public static int CheckSimilarVinExist(string stock, InventoryFormViewModel inventory, DealershipViewModel dealer)
        {
            if (inventory.CarsList.Any(o => o.Vin.Contains(stock)))
            {
                int numberofResult = inventory.CarsList.Count(o => o.Vin.Contains(stock));
                return numberofResult;
            }

            return 0;
        }

        public static bool AllowToAccessAppraisal(Appraisal appraisal)
        {
            try
            {
                if (appraisal.Dealer.DealerGroup.DealerGroupId != SessionHandler.Dealer.DealerGroupId)
                {
                    return false;
                }

                //if (SessionHandler.IsEmployee)
                //{
                //    if (SessionHandler.UserId != appraisal.AppraisalById)
                //    {
                //        return false;
                //    }

                //    var currentDate = DateTime.Now;
                //    var appraisalDate = appraisal.AppraisalDate.Value;

                //    if (appraisalDate.Year != currentDate.Year || appraisalDate.Month != currentDate.Month || appraisalDate.Day != currentDate.Day)
                //    {
                //        return false;
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

    }
}
