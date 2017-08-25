using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.DatabaseModel;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public static class MapperHelper
    {
        public static CarExcelInfoViewModel ConvertToCarExcelViewInfo(this CarInfoFormViewModel drRow, string defaultStockImageUrl)
        {
            string imagesNum = GetImageNum(drRow.CarImageUrl,drRow.DefaultImageUrl, defaultStockImageUrl);

            CarExcelInfoViewModel carExcelInfoViewModel = new CarExcelInfoViewModel()
            {
                Days = DateTime.Now.Subtract(drRow.DateInStock.Value).Days,
                ExteriorColor = drRow.ExteriorColor,
                imagesNum = imagesNum,
                Make = drRow.Make,
                Mileage = drRow.Mileage,
                Model = drRow.Model,
                ModelYear = drRow.ModelYear.ToString(),
                SalePrice = drRow.SalePrice,
                StockNumber = drRow.StockNumber,
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

        public static AppraisalViewFormModel ConvertToAppraisalViewFormModel(this whitmanenterpriseappraisal row)
        {
            var appraisalTmp = new AppraisalViewFormModel
            {
                AppraisalID = row.idAppraisal,
                Make = row.Make,
                ModelYear = row.ModelYear.GetValueOrDefault(),
                AppraisalModel = row.Model,
                Trim = row.Trim,
                VinNumber = row.VINNumber,
                StockNumber = row.StockNumber,
                ACV = row.ACV,
                CarImagesUrl = row.DefaultImageUrl,
                DefaultImageUrl = row.DefaultImageUrl,
                ExteriorColor = row.ExteriorColor,
                AppraisalDate = row.AppraisalDate.Value.ToShortDateString(),
                AppraisalGenerateId = row.AppraisalID,                
            };
            return appraisalTmp;
        }

        public static AppraisalInfoViewModel ConvertToAppraisalViewModel(this whitmanenterpriseappraisal drRow, string defaultStockImageUrl)
        {            
            string imagesNum = GetImageNum(drRow.CarImageUrl, drRow.DefaultImageUrl, defaultStockImageUrl);
            return new AppraisalInfoViewModel
            {
                Days = (drRow.AppraisalDate == null) ? 0 : DateTime.Now.Subtract((DateTime)drRow.AppraisalDate).Days,
                ExteriorColor = drRow.ExteriorColor,
                imagesNum = imagesNum,
                Make = drRow.Make,
                Mileage = drRow.Mileage,
                Model = drRow.Model,
                ModelYear = drRow.ModelYear.ToString(),
                SalePrice = drRow.SalePrice,
                StockNumber = drRow.StockNumber,
                Vin = drRow.VINNumber               
            };          
        }

    }
}
