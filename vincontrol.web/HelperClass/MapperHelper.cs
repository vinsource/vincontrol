using System;
using System.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Models;

namespace Vincontrol.Web.HelperClass
{
    public static class MapperHelper
    {
        public static CarExcelInfoViewModel ConvertToCarExcelViewInfo(this CarInfoFormViewModel drRow, string defaultStockImageUrl)
        {
            string imagesNum = GetImageNum(drRow.CarImageUrl,drRow.DefaultImageUrl, defaultStockImageUrl);

            var carExcelInfoViewModel = new CarExcelInfoViewModel()
            {
                Days = DateTime.Now.Subtract(drRow.DateInStock.Value).Days,
                ExteriorColor = drRow.ExteriorColor,
                imagesNum = imagesNum,
                Make = drRow.Make,
                Mileage = drRow.Mileage.ToString(),
                Model = drRow.Model,
                ModelYear = drRow.ModelYear.ToString(),
                SalePrice = drRow.SalePrice.ToString(),
                StockNumber = drRow.Stock,
                Vin = drRow.Vin,
                Reconstatus = drRow.Reconstatus                
            };
           

            return carExcelInfoViewModel;
        }

        private static string GetImageNum(string carImgURL, string defaultImageURL, string defaultStockImageUrl)
        {
            string imagesNum = "0";

            if (String.IsNullOrEmpty(carImgURL))
            {
                imagesNum = "0";
            }
            else
            {
                string[] splitArray =
                    carImgURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                        ();

                if (splitArray.Count() > 1)
                {
                    imagesNum = splitArray.Count().ToString();
                }
                else
                {
                    if (!String.IsNullOrEmpty(defaultImageURL) &&
                        !String.IsNullOrEmpty(defaultStockImageUrl) &&
                        !carImgURL.Equals(defaultImageURL) &&
                        !carImgURL.Equals(defaultStockImageUrl))
                    {
                        imagesNum = "1";
                    }
                    else
                    {
                        imagesNum = "1(D)";
                    }
                }
            }
            return imagesNum;
        }

        public static AppraisalViewFormModel ConvertToAppraisalViewFormModel(this Appraisal row)
        {
            var appraisalTmp = new AppraisalViewFormModel
            {
                AppraisalID = row.AppraisalId,
                Make = row.Vehicle.Make,
                ModelYear = row.Vehicle.Year.GetValueOrDefault(),
                AppraisalModel = row.Vehicle.Model,
                Trim = row.Vehicle.Trim,
                VinNumber = row.Vehicle.Vin,
                StockNumber = row.Stock,
                ACV = row.ACV,
                CarImagesUrl = row.Vehicle.DefaultStockImage,
                DefaultImageUrl = row.Vehicle.DefaultStockImage,
                ExteriorColor = row.ExteriorColor,
                AppraisalDate = row.AppraisalDate.Value.ToShortDateString(),
                AppraisalGenerateId = row.AppraisalId.ToString(),                
            };
            return appraisalTmp;
        }

        public static AppraisalInfoViewModel ConvertToAppraisalViewModel(this Appraisal drRow, string defaultStockImageUrl)
        {            
            string imagesNum = GetImageNum(drRow.PhotoUrl, drRow.Vehicle.DefaultStockImage, defaultStockImageUrl);
            return new AppraisalInfoViewModel
            {
                Days = (drRow.AppraisalDate == null) ? 0 : DateTime.Now.Subtract((DateTime)drRow.AppraisalDate).Days,
                ExteriorColor = drRow.ExteriorColor,
                imagesNum = imagesNum,
                Make = drRow.Vehicle.Make,
                Mileage = drRow.Mileage.ToString(),
                Model = drRow.Vehicle.Model,
                ModelYear = drRow.Vehicle.Year.ToString(),
                SalePrice = drRow.SalePrice.GetValueOrDefault().ToString(),
                StockNumber = drRow.Stock,
                Vin = drRow.Vehicle.Vin
            };          
        }

    }
}
