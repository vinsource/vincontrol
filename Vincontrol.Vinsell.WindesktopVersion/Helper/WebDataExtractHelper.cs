using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.WindesktopVersion.Helper
{
    public class WebDataExtractHelper
    {
        public static ExtractedManheimVehicle ExtractFromManheimProfile(string html)
        {
            var result = new ExtractedManheimVehicle();

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(html);

            var selectManheimProfileNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='ui-p-b ps-bdr-dots clearfix']");

            var selectSubNode = selectManheimProfileNode.SelectNodes(".//div[@class='ui-hm']//label");

            foreach (var tmp in selectSubNode)
            {
                if (tmp.InnerText.Contains("Year:"))
                {
                    if (tmp.ParentNode.ChildNodes.Count > 4)
                    {
                       result.Year = Convert.ToInt32(tmp.ParentNode.ChildNodes[3].InnerText);
                    }
                }

                if (tmp.InnerText.Contains("Make:"))
                {
                    if (tmp.ParentNode.ChildNodes.Count > 4)
                    {
                        result.Make = (tmp.ParentNode.ChildNodes[3].InnerText);
                    }
                }

                if (tmp.InnerText.Contains("Model:"))
                {
                    if (tmp.ParentNode.ChildNodes.Count > 4)
                    {
                        result.Model = (tmp.ParentNode.ChildNodes[3].InnerText);
                    }
                }

                if (tmp.InnerText.Contains("Trim Level:"))
                {
                    if (tmp.ParentNode.ChildNodes.Count > 4)
                    {
                        result.Trim = (tmp.ParentNode.ChildNodes[3].InnerText);
                    }
                }

                if (tmp.InnerText.Contains("Odometer:"))
                {
                    int mileageNumber = 0;
                    if (tmp.ParentNode.ChildNodes.Count > 4)
                    {
                        var mileage = tmp.ParentNode.ChildNodes[3].InnerText;
                        if (!String.IsNullOrEmpty(mileage))
                        {                            
                            mileage = mileage.Replace(",", "");
                            
                            if (mileage.Contains("km"))
                            {
                                mileage = mileage.Replace("km", "");
                                Int32.TryParse(mileage, out mileageNumber);
                                mileageNumber *= 1000;
                            }
                            else
                            {
                                mileage = mileage.Replace("mi", "");
                                Int32.TryParse(mileage, out mileageNumber);
                            }
                        }

                        result.Odometer = mileageNumber;
                    }
                }
            }

            var mmrNode = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='mmrHover']");
            if (mmrNode != null)
            {
                result.Price = mmrNode.InnerText.Contains("Not Available") ? 0 : Int32.Parse(CommonHelper.RemoveSpecialCharacters(mmrNode.InnerText));
               
            }

            if (!String.IsNullOrEmpty(result.Trim))
            {
                if (result.Trim.Contains("Not Specified"))
                {
                    result.Trim = String.Empty;
                }
            }

            return result;
        }
    }

    public class ExtractedManheimVehicle
    {
        public int Odometer { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        public string Trim { get; set; }
        public string Vin { get; set; }
        public int Price { get; set; }
    }
}
