using System.Globalization;
using System.Linq;

namespace Vincontrol.Console.ChangeActivityTracking.Temp
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateUserActivity();
            UpdateAppraisalActivity();
            UpdateInventoryActivity();
            UpdatePriceChangeActivity();
        }

        private static void UpdateAppraisalActivity()
        {
            using (var dataContext = new VincontrolEntities())
            {
                foreach (var item in dataContext.AppraisalDealerActivities)
                {
                    if (string.IsNullOrEmpty(item.Detail)||item.Detail.Contains("Stock Number")) continue;
                    var resultString = item.Detail.Split(';');
                    var appraisalIdString = resultString[0];
                    var vinString = resultString[1];
                    //var stockString = resultString[2];
                    var yearString = resultString[2];
                    var makeString = resultString[3];
                    var modelString = resultString[4];
                    var trimString = resultString[5];

                    item.VIN = GetContent(vinString).Trim();
                    //item.Stock = GetContent(stockString).Trim();
                    var oldAppraisalId = int.Parse(GetContent(appraisalIdString).Trim());
                    var appraisal = dataContext.Appraisals.FirstOrDefault(i => i.OldAppraisalId == oldAppraisalId);
                    item.Stock = null;
                    if (appraisal != null)
                    {
                        item.AppraisalId = appraisal.AppraisalId;
                        item.Stock = appraisal.Stock;
                    }
                    item.Year = int.Parse(GetContent(yearString).Trim());
                    item.Make = GetContent(makeString).Trim();
                    item.Model =  GetContent(modelString).Trim();
                    item.Trim =  GetContent(trimString).Trim();
                }
                dataContext.SaveChanges();
            }
        }

        private static void UpdateInventoryActivity()
        {
            using (var dataContext = new VincontrolEntities())
            {
                foreach (var item in dataContext.InventoryDealerActivities.Where(i=>i.DealerActivitySubTypeCodeId == 13))
                {
                    if (string.IsNullOrEmpty(item.Detail)) continue;
                    var resultString = item.Detail.Split(';');
                    var appraisalId = resultString[0];
                    var inventoryId = resultString[1];
                    //var vinString = resultString[1];
                    //var stockString = resultString[2];
                    var yearString = resultString[2];
                    var makeString = resultString[3];
                    var modelString = resultString[4];
                    var trimString = resultString[5];

                    //item.VIN = GetContent(vinString).Trim();
                    //item.Stock = GetContent(stockString).Trim();
                    var oldInventoryId = int.Parse(GetContent(inventoryId).Trim());
                   
                    var inventory = dataContext.Inventories.Include("Vehicle").FirstOrDefault(i => i.OldInventoryId == oldInventoryId);
                    if (inventory != null)
                    {
                        item.InventoryId = inventory.InventoryId ;
                        item.VIN = inventory.Vehicle.Vin;
                        item.Stock = inventory.Stock;
                    }

                    if (!string.IsNullOrEmpty(GetContent(appraisalId).Trim()))
                    {
                        var oldAppraisalId = int.Parse(GetContent(appraisalId));
                        var appraisal = dataContext.Appraisals.FirstOrDefault(i => i.OldAppraisalId == oldAppraisalId);

                        if (appraisal != null)
                        {
                            item.AppraisalId = appraisal.AppraisalId;
                        }

                    }
                    item.Year = int.Parse(GetContent(yearString).Trim());
                    item.Make = GetContent(makeString).Trim();
                    item.Model = GetContent(modelString).Trim();
                    item.Trim = GetContent(trimString).Trim();
                }
                dataContext.SaveChanges();
            }
        }

        //private static void UpdatePriceChangeActivity()
        //{
        //    using (var dataContext = new VincontrolEntities())
        //    {
        //        foreach (var item in dataContext.InventoryDealerActivities.Where(i => i.DealerActivitySubTypeCodeId == 10))
        //        {
        //            if (string.IsNullOrEmpty(item.Detail)) continue;
        //            var resultString = item.Detail.Split(',');
        //            var inventoryId = resultString[0];

        //            //item.VIN = GetContent(vinString).Trim();
        //            //item.Stock = GetContent(stockString).Trim();
        //            var oldInventoryId = int.Parse(GetContent(inventoryId).Trim());
        //            item.OldPrice = decimal.Parse(GetContent(resultString[2].Split(';')[0]).Trim(), NumberStyles.Currency);
        //            if (resultString[2].Split(';').Length >= 2)
        //            {
        //                item.NewPrice = decimal.Parse(GetContent(resultString[2].Split(';')[1]).Trim(),
        //                                              NumberStyles.Currency);
        //            }
        //            var inventory = dataContext.Inventories.Include("Vehicle").FirstOrDefault(i => i.OldInventoryId == oldInventoryId);
        //            if (inventory != null)
        //            {
        //                item.InventoryId = inventory.InventoryId;
        //                item.VIN = inventory.Vehicle.Vin;
        //                item.Stock = inventory.Stock;
        //                item.Year = inventory.Vehicle.Year;
        //                item.Make = inventory.Vehicle.Make;
        //                item.Model = inventory.Vehicle.Model;
        //                item.Trim = inventory.Vehicle.Trim;

        //            }
        //        }

        //        dataContext.SaveChanges();
        //    }
        //}

        private static void UpdatePriceChangeActivity()
        {
            using (var dataContext = new VincontrolEntities())
            {
                foreach (var item in dataContext.InventoryDealerActivities.Where(i => i.DealerActivitySubTypeCodeId == 10))
                {
                    if (string.IsNullOrEmpty(item.Detail)) continue;
                    var resultString = item.Detail.Split(';');
                    item.NewPrice = decimal.Parse(GetContent(resultString[1]).Trim(),
                                                      NumberStyles.Currency);
                    var newResultString = resultString[0].Split('$');
                    item.OldPrice = decimal.Parse(newResultString[1],
                                                 NumberStyles.Currency);
                    var oldInventoryId = int.Parse(GetContent(newResultString[0].Split(',')[0]));

                    var inventory = dataContext.Inventories.Include("Vehicle").FirstOrDefault(i => i.OldInventoryId == oldInventoryId);
                    if (inventory != null)
                    {
                        item.InventoryId = inventory.InventoryId;
                        item.VIN = inventory.Vehicle.Vin;
                        item.Stock = inventory.Stock;
                        item.Year = inventory.Vehicle.Year;
                        item.Make = inventory.Vehicle.Make;
                        item.Model = inventory.Vehicle.Model;
                        item.Trim = inventory.Vehicle.Trim;

                    }
                }

                dataContext.SaveChanges();
            }
        }

        private static string GetContent(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                return null;
            }

            var array = item.Split(':');
            if (array.Length > 1)
            {
                return array[1];
            }

            return null;
        }

        private static void UpdateUserActivity()
        {
            using (var dataContext = new VincontrolEntities())
            {
                foreach (var item in dataContext.UserDealerActivities)
                {
                    if (string.IsNullOrEmpty(item.Detail)) continue;
                    var resultString = item.Detail.Split(';');
                    var usernameString = resultString[0];
                    var usernameStringArray = usernameString.Split(':');
                    if (usernameStringArray.Length > 1)
                    {
                        var username = usernameStringArray[1].Trim();
                        var firstOrDefault = dataContext.Users.FirstOrDefault(i => i.UserName == username);
                        item.UserName = username;
                        item.Email = GetContent(resultString[1]).Trim();
                        item.Phone = GetContent(resultString[2]).Trim();
                        item.Role = GetContent(resultString[3]).Trim();
                        if (firstOrDefault != null)
                            item.UserId = firstOrDefault.UserId;
                    }
                }
                dataContext.SaveChanges();
            }
        }
    }
}
