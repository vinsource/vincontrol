using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class SessionHelper
    {
        public static int CheckStockExist(string stock, InventoryFormViewModel inventory, DealershipViewModel dealer)
        {
            if (inventory.CarsList.Any(o => o.StockNumber.Contains(stock)))
            {
                int numberofResult = inventory.CarsList.Count(o => o.StockNumber.Contains(stock));
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
    }
}
