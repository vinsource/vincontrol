using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class WindowStickerHelper
    {
        public static string BuildWindowStickerInHtml(string ListingId, int DealerId)
        {
            int LID = Convert.ToInt32(ListingId);

            var context = new whitmanenterprisewarehouseEntities();

            var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

            var settingRow = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == DealerId);

            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(row.StandardOptions))
            {

                var standardOptionList =
                    row.StandardOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                int index = standardOptionList.Count() / 2;

                var optionFirstList = standardOptionList.GetRange(0, index);

                var optionSecondList =
                    standardOptionList.GetRange(index, Math.Min(index, standardOptionList.Count() - index));

                string barCode =
                    "<img height=\"20\" width=\"180\" src=\"http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" +
                    row.VINNumber +
                    "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false\" />";


                builder.AppendLine(" <!DOCTYPE html>");

                builder.AppendLine("<html>");

                builder.AppendLine("<head>");

                builder.AppendLine("<title></title>");

                builder.AppendLine("</head>");

                builder.AppendLine("<body>");

                builder.AppendLine("<font face=\"Trebuchet MS\">");

                builder.AppendLine("<div id=\"bg\">");

                builder.AppendLine("<table id=\"window-sticker\" width=\"320\">");

                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"50\" rowspan=\"2\"></td>");

                builder.AppendLine("<td width=\"50\" ></td>");

                builder.AppendLine("<br />"); builder.AppendLine("<br />");

                builder.AppendLine("</tr>");

                builder.AppendLine("<tr>");
                builder.AppendLine("<td>");

                builder.AppendLine("<table id=\"info-table\" width=\"400\">");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td colspan=\"2\" align=\"center\">");

                string title = row.ModelYear.GetValueOrDefault() + " " + row.Make + " " +
                               row.Model + " " + row.Trim;

                if (title.Length < 32)

                    builder.AppendLine("<font size=\"5\">" + title + "</font><br />");
                else
                {
                    builder.AppendLine("<font size=\"3\">" + title + "</font><br />");
                }

                builder.AppendLine("<font size=\"4\">" + row.Cylinders + " Cylinders " + row.FuelType + "</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"200\" valign=\"top\">");
                builder.AppendLine("<font size=\"4\">");

                int odometerNumber = 0;

                bool odometerFlag = Int32.TryParse(row.Mileage, out odometerNumber);

                if (odometerFlag)
                    builder.AppendLine("<em>Mileage:</em> <b>" + odometerNumber.ToString("#,##0") + "</b><br />");
                builder.AppendLine("<em>Stock Number:</em> <b>" + row.StockNumber + "</b><br />");

                builder.AppendLine("<em>Color:</em> <b>" + CommonHelper.TrimString(row.ExteriorColor, 17) + "</b><br />");
                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("<td valign=\"top\">");

                builder.AppendLine("<font size=\"4\">");

                if (!String.IsNullOrEmpty(row.Tranmission))
                {

                    if (row.Tranmission.ToLower().Contains("auto"))

                        builder.AppendLine("<em>Transmission:</em> <b>Automatic</b><br />");
                    else
                        builder.AppendLine("<em>Transmission:</em> <b>Manual</b><br />");
                }
                else
                {
                    builder.AppendLine("<em>Transmission:</em> <b></b><br />");
                }

                builder.AppendLine("<em>VIN:</em> <b>" + row.VINNumber + "</b><br />");
                builder.AppendLine("<b>" + barCode + "</b><br />");
                builder.AppendLine("</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");


                //CAR OPTIONS

                if (!String.IsNullOrEmpty(row.CarsOptions) || !String.IsNullOrEmpty(row.CarsPackages))
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                    builder.AppendLine("<font size=\"4\"><u>" + "Additional Packages & Options" + "</u></font><br />");
                    builder.AppendLine("</td>");
                    builder.AppendLine("</tr>");


                    builder.AppendLine("<tr>");


                    builder.AppendLine("<font size=\"2\">");
                    builder.AppendLine("<td height=\"150\" valign=\"top\">");

                    var finalPackageAndOptions = new List<string>();

                    if (!String.IsNullOrEmpty(row.CarsPackages))
                    {

                        var addtionalPackagesList = row.CarsPackages.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        finalPackageAndOptions.AddRange(addtionalPackagesList.AsEnumerable());
                    }

                    if (!String.IsNullOrEmpty(row.CarsOptions))
                    {
                        var addtionalOptionList = row.CarsOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        finalPackageAndOptions.AddRange(addtionalOptionList.AsEnumerable());
                    }

                    int addtionalindex = (int)Math.Ceiling((double)finalPackageAndOptions.Count() / 2);

                    if (finalPackageAndOptions.Count() == 1)
                        addtionalindex = 1;

                    var addtionaloptionFirstList = finalPackageAndOptions.GetRange(0, addtionalindex);

                    var addtionaloptionSecondList =
                        finalPackageAndOptions.GetRange(addtionalindex, finalPackageAndOptions.Count - addtionalindex);


                    if (addtionaloptionFirstList.Count() > 10)
                    {
                        foreach (var tmp in addtionaloptionFirstList.Take(10))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in addtionaloptionFirstList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }


                    }


                    builder.AppendLine("</td>");

                    builder.AppendLine("<td>");


                    if (addtionaloptionSecondList.Count() > 10)
                    {
                        foreach (var tmp in addtionaloptionSecondList.Take(10))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in addtionaloptionSecondList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }


                    }


                    builder.AppendLine("</td>");

                    builder.AppendLine("</font>");
                    builder.AppendLine("</tr>");


                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                    builder.AppendLine("<font size=\"4\"><u>" + "Standard Options" + "</u></font><br />");
                    builder.AppendLine("</td>");
                    builder.AppendLine("</tr>");

                    builder.AppendLine("<tr>");

                    builder.AppendLine("<font size=\"2\">");
                    builder.AppendLine("<td height=\"500\" valign=\"top\">");

                    if (optionFirstList.Count() > 15)
                    {
                        foreach (var tmp in optionFirstList.GetRange(0, 15))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionFirstList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 15 - optionFirstList.Count() - addtionaloptionFirstList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }



                    builder.AppendLine("</td>");

                    builder.AppendLine("<td>");


                    if (optionSecondList.Count() > 15)
                    {
                        foreach (var tmp in optionSecondList.GetRange(0, 15))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionSecondList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 15 - optionSecondList.Count() - addtionaloptionSecondList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }

                    if (settingRow.RetailPrice.GetValueOrDefault() == false && settingRow.DealerDiscount.GetValueOrDefault() == false)
                    {
                        builder.AppendLine("<br /><br /><br />");
                    }
                    else if ((settingRow.RetailPrice.GetValueOrDefault() && settingRow.DealerDiscount.GetValueOrDefault()) == false)
                    {
                        builder.AppendLine("<br /><br />");
                    }

                    builder.AppendLine("</td>");

                    builder.AppendLine("</font>");
                    builder.AppendLine("</tr>");
                }
                else
                {

                    //STANDARD OPTIONS

                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                    builder.AppendLine("<font size=\"4\"><u>" + "Standard Options" + "</u></font><br />");
                    builder.AppendLine("</td>");
                    builder.AppendLine("</tr>");

                    builder.AppendLine("<tr>");

                    builder.AppendLine("<font size=\"2\">");
                    builder.AppendLine("<td height=\"800\" valign=\"top\">");

                    if (optionFirstList.Count() > 28)
                    {
                        foreach (var tmp in optionFirstList.Take(28))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionFirstList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 28 - optionFirstList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }



                    builder.AppendLine("</td>");

                    builder.AppendLine("<td>");


                    if (optionSecondList.Count() > 28)
                    {
                        foreach (var tmp in optionSecondList.Take(28))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionSecondList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 28 - optionSecondList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }

                    if (settingRow.RetailPrice.GetValueOrDefault() && settingRow.DealerDiscount.GetValueOrDefault())
                        builder.AppendLine("<br /><br />");
                    else if (settingRow.RetailPrice.GetValueOrDefault() == false &&
                             settingRow.DealerDiscount.GetValueOrDefault() == false)
                    {
                        builder.AppendLine("<br /><br /><br /><br /><br /><br /><br />");
                    }
                    else
                    {
                        builder.AppendLine("<br /><br /><br /><br /><br />");
                    }


                    builder.AppendLine("</td>");

                    builder.AppendLine("</font>");
                    builder.AppendLine("</tr>");


                }

                builder.AppendLine("<tr valign=\"bottom\">");

                builder.AppendLine("<td align=\"right\" width=\"300\" colspan=\"3\">");
                builder.AppendLine("<font size=\"5\">");


                int salePriceNumber = 0;
                int retailPriceNumber = 0;
                int dealerDiscountNumber = 0;
                bool salePriceFlag = Int32.TryParse(row.RetailPrice, out retailPriceNumber);
                bool dealerdiscountFlag = Int32.TryParse(row.DealerDiscount, out dealerDiscountNumber);
                if (settingRow.RetailPrice.GetValueOrDefault())
                {
                    if (salePriceFlag)
                        builder.AppendLine(settingRow.RetailPriceText + ": " + retailPriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;" + "<br />");
                }

                if (settingRow.DealerDiscount.GetValueOrDefault())
                {
                    if (dealerdiscountFlag)
                        builder.AppendLine(settingRow.DealerDiscountText + ": " + dealerDiscountNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;" + "<br /><br />");
                }

                salePriceNumber = retailPriceNumber - dealerDiscountNumber;

                if (settingRow.SalePrice.GetValueOrDefault())
                {
                    builder.AppendLine(settingRow.SalePriceText + ": " + salePriceNumber.ToString("c0")+ "&nbsp;&nbsp;&nbsp;&nbsp;");
                }

                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("</tr>");

                builder.AppendLine("</table>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("</table>");

                builder.AppendLine("</div>");


                builder.AppendLine("</font>");

                builder.AppendLine("</body>");

                builder.AppendLine("</html>");
            }
            else
            {
                string barCode =
                  "<img height=\"20\" width=\"180\" src=\"http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" +
                  row.VINNumber +
                  "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false\" />";

                builder.AppendLine(" <!DOCTYPE html>");

                builder.AppendLine("<html>");

                builder.AppendLine("<head>");

                builder.AppendLine("<title></title>");

                builder.AppendLine("</head>");

                builder.AppendLine("<body>");

                builder.AppendLine("<font face=\"Trebuchet MS\">");

                builder.AppendLine("<div id=\"bg\">");

                builder.AppendLine("<table id=\"window-sticker\" width=\"320\">");

                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"50\" rowspan=\"2\"></td>");

                builder.AppendLine("<td width=\"50\" ></td>");

                builder.AppendLine("<br />"); builder.AppendLine("<br />");

                builder.AppendLine("</tr>");

                builder.AppendLine("<tr>");
                builder.AppendLine("<td>");

                builder.AppendLine("<table id=\"info-table\" width=\"400\">");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                builder.AppendLine("<font size=\"5\">" + row.ModelYear.GetValueOrDefault() + " " + row.Make + " " +
                                   row.Model + " " + row.Trim + "</font><br />");

                builder.AppendLine("<font size=\"4\">" + row.Cylinders + " Cylinders " + row.FuelType + "</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"200\" valign=\"top\">");
                builder.AppendLine("<font size=\"4\">");

                int odometerNumber = 0;

                bool odometerFlag = Int32.TryParse(row.Mileage, out odometerNumber);

                if (odometerFlag)
                    builder.AppendLine("<em>Mileage:</em> <b>" + odometerNumber.ToString("#,##0") + "</b><br />");
                builder.AppendLine("<em>Stock Number:</em> <b>" + row.StockNumber + "</b><br />");

                builder.AppendLine("<em>Color:</em> <b>" + CommonHelper.TrimString(row.ExteriorColor, 17) + "</b><br />");
                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("<td valign=\"top\">");

                builder.AppendLine("<font size=\"4\">");

                if (row.Tranmission != null)
                {
                    builder.AppendLine(row.Tranmission.ToLower().Contains("auto")
                                           ? "<em>Transmission:</em> <b>Automatic</b><br />"
                                           : "<em>Transmission:</em> <b>Manual</b><br />");
                }
                builder.AppendLine("<em>VIN:</em> <b>" + row.VINNumber + "</b><br />");
                builder.AppendLine("<b>" + barCode + "</b><br />");
                builder.AppendLine("</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("<tr>");
                builder.AppendLine("<font size=\"2\">");
                builder.AppendLine("<td height=\"800\" valign=\"top\">");


                if (settingRow.RetailPrice.GetValueOrDefault() && settingRow.DealerDiscount.GetValueOrDefault())
                    builder.AppendLine("<br /><br />");
                else if (settingRow.RetailPrice.GetValueOrDefault() == false && settingRow.DealerDiscount.GetValueOrDefault() == false)
                {
                    builder.AppendLine("<br /><br /><br /><br /><br /><br /><br />");
                }
                else
                {
                    builder.AppendLine("<br /><br /><br /><br /><br />");
                }

                builder.AppendLine("<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />");
                builder.AppendLine("</td>");

                builder.AppendLine("</font>");
                builder.AppendLine("</tr>");
                builder.AppendLine("<tr valign=\"bottom\">");

                builder.AppendLine("<td align=\"right\" width=\"300\" colspan=\"3\">");
                builder.AppendLine("<font size=\"5\">");

                int salePriceNumber = 0;
                int retailPriceNumber = 0;
                int dealerDiscountNumber = 0;
                bool salePriceFlag = Int32.TryParse(row.RetailPrice, out retailPriceNumber);
                bool dealerdiscountFlag = Int32.TryParse(row.DealerDiscount, out dealerDiscountNumber);
                if (settingRow.RetailPrice.GetValueOrDefault())
                {
                    if (salePriceFlag)
                        builder.AppendLine(settingRow.RetailPriceText + ": " + retailPriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;" + "<br />");
                }

                if (settingRow.DealerDiscount.GetValueOrDefault())
                {
                    if (dealerdiscountFlag)
                        builder.AppendLine(settingRow.DealerDiscountText + ": " + dealerDiscountNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;" + "<br /><br />");
                }

                salePriceNumber = retailPriceNumber - dealerDiscountNumber;

                if (settingRow.SalePrice.GetValueOrDefault())
                {
                    builder.AppendLine(settingRow.SalePriceText + ": " + salePriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;");
                }


                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("</tr>");

                builder.AppendLine("</table>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("</table>");

                builder.AppendLine("</div>");


                builder.AppendLine("</font>");

                builder.AppendLine("</body>");

                builder.AppendLine("</html>");
            }

            return builder.ToString();

        }

        public static void BuildWindowStickerBody(StringBuilder builder, whitmanenterprisedealershipinventory row, whitmanenterprisesetting settingRow)
        {
            if (!String.IsNullOrEmpty(row.StandardOptions))
            {
                var standardOptionList =
                    row.StandardOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                int index = standardOptionList.Count() / 2;

                var optionFirstList = standardOptionList.GetRange(0, index);

                var optionSecondList = standardOptionList.GetRange(index,
                                                                   Math.Min(index, standardOptionList.Count() - index));

                string barCode =
                    "<img height=\"20\" width=\"180\" src=\"http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" +
                    row.VINNumber +
                    "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false\" />";

                builder.AppendLine("<div id=\"bg\">");

                builder.AppendLine("<table id=\"window-sticker\" width=\"320\">");

                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"50\" rowspan=\"2\"></td>");

                builder.AppendLine("<td width=\"50\" ></td>");

                builder.AppendLine("<br />");
                builder.AppendLine("<br />");

                builder.AppendLine("</tr>");

                builder.AppendLine("<tr>");
                builder.AppendLine("<td>");

                builder.AppendLine("<table id=\"info-table\" width=\"400\">");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td colspan=\"2\" align=\"center\">");

                string title = row.ModelYear.GetValueOrDefault() + " " + row.Make + " " + row.Model + " " + row.Trim;

                if (title.Length < 32)
                    builder.AppendLine("<font size=\"5\">" + title + "</font><br />");
                else
                {
                    builder.AppendLine("<font size=\"3\">" + title + "</font><br />");
                }

                builder.AppendLine("<font size=\"4\">" + row.Cylinders + " Cylinders " + row.FuelType + "</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"200\" valign=\"top\">");
                builder.AppendLine("<font size=\"4\">");

                int odometerNumber = 0;

                bool odometerFlag = Int32.TryParse(row.Mileage, out odometerNumber);

                if (odometerFlag)
                    builder.AppendLine("<em>Mileage:</em> <b>" + odometerNumber.ToString("#,##0") + "</b><br />");
                builder.AppendLine("<em>Stock Number:</em> <b>" + row.StockNumber + "</b><br />");

                builder.AppendLine("<em>Color:</em> <b>" + CommonHelper.TrimString(row.ExteriorColor, 17) + "</b><br />");
                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("<td valign=\"top\">");

                builder.AppendLine("<font size=\"4\">");

                if (!String.IsNullOrEmpty(row.Tranmission))
                {

                    if (row.Tranmission.ToLower().Contains("auto"))

                        builder.AppendLine("<em>Transmission:</em> <b>Automatic</b><br />");
                    else
                        builder.AppendLine("<em>Transmission:</em> <b>Manual</b><br />");
                }
                else
                {
                    builder.AppendLine("<em>Transmission:</em> <b></b><br />");
                }

                builder.AppendLine("<em>VIN:</em> <b>" + row.VINNumber + "</b><br />");
                builder.AppendLine("<b>" + barCode + "</b><br />");
                builder.AppendLine("</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");


                //CAR OPTIONS

                if (!String.IsNullOrEmpty(row.CarsOptions) || !String.IsNullOrEmpty(row.CarsPackages))
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                    builder.AppendLine("<font size=\"4\"><u>" + "Additional Packages & Options" + "</u></font><br />");
                    builder.AppendLine("</td>");
                    builder.AppendLine("</tr>");


                    builder.AppendLine("<tr>");


                    builder.AppendLine("<font size=\"2\">");
                    builder.AppendLine("<td height=\"150\" valign=\"top\">");

                    var finalPackageAndOptions = new List<string>();

                    if (!String.IsNullOrEmpty(row.CarsPackages))
                    {

                        var addtionalPackagesList =
                            row.CarsPackages.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        finalPackageAndOptions.AddRange(addtionalPackagesList.AsEnumerable());
                    }

                    if (!String.IsNullOrEmpty(row.CarsOptions))
                    {
                        var addtionalOptionList =
                            row.CarsOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        finalPackageAndOptions.AddRange(addtionalOptionList.AsEnumerable());
                    }

                    int addtionalindex = (int)Math.Ceiling((double)finalPackageAndOptions.Count() / 2);

                    if (finalPackageAndOptions.Count() == 1)
                        addtionalindex = 1;

                    var addtionaloptionFirstList = finalPackageAndOptions.GetRange(0, addtionalindex);

                    var addtionaloptionSecondList =
                        finalPackageAndOptions.GetRange(addtionalindex, finalPackageAndOptions.Count - addtionalindex);


                    if (addtionaloptionFirstList.Count() > 10)
                    {
                        foreach (var tmp in addtionaloptionFirstList.Take(10))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in addtionaloptionFirstList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }


                    }


                    builder.AppendLine("</td>");

                    builder.AppendLine("<td>");


                    if (addtionaloptionSecondList.Count() > 10)
                    {
                        foreach (var tmp in addtionaloptionSecondList.Take(10))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in addtionaloptionSecondList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }


                    }


                    builder.AppendLine("</td>");

                    builder.AppendLine("</font>");
                    builder.AppendLine("</tr>");


                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                    builder.AppendLine("<font size=\"4\"><u>" + "Standard Options" + "</u></font><br />");
                    builder.AppendLine("</td>");
                    builder.AppendLine("</tr>");

                    builder.AppendLine("<tr>");

                    builder.AppendLine("<font size=\"2\">");
                    builder.AppendLine("<td height=\"500\" valign=\"top\">");

                    if (optionFirstList.Count() > 20)
                    {
                        foreach (var tmp in optionFirstList.GetRange(0, 20))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionFirstList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 20 - optionFirstList.Count() - addtionaloptionFirstList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }



                    builder.AppendLine("</td>");

                    builder.AppendLine("<td>");


                    if (optionSecondList.Count() > 20)
                    {
                        foreach (var tmp in optionSecondList.GetRange(0, 20))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionSecondList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 20 - optionSecondList.Count() - addtionaloptionSecondList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }

                    if (settingRow.RetailPrice.GetValueOrDefault() == false &&
                        settingRow.DealerDiscount.GetValueOrDefault() == false)
                    {
                        builder.AppendLine("<br /><br /><br />");
                    }
                    else if ((settingRow.RetailPrice.GetValueOrDefault() && settingRow.DealerDiscount.GetValueOrDefault()) ==
                             false)
                    {
                        builder.AppendLine("<br /><br />");
                    }

                    builder.AppendLine("</td>");

                    builder.AppendLine("</font>");
                    builder.AppendLine("</tr>");
                }
                else
                {

                    //STANDARD OPTIONS

                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                    builder.AppendLine("<font size=\"4\"><u>" + "Standard Options" + "</u></font><br />");
                    builder.AppendLine("</td>");
                    builder.AppendLine("</tr>");

                    builder.AppendLine("<tr>");

                    builder.AppendLine("<font size=\"2\">");
                    builder.AppendLine("<td height=\"800\" valign=\"top\">");

                    if (optionFirstList.Count() > 28)
                    {
                        foreach (var tmp in optionFirstList.Take(28))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionFirstList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 28 - optionFirstList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }



                    builder.AppendLine("</td>");

                    builder.AppendLine("<td>");


                    if (optionSecondList.Count() > 28)
                    {
                        foreach (var tmp in optionSecondList.Take(28))
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }
                    }
                    else
                    {
                        foreach (var tmp in optionSecondList)
                        {
                            builder.AppendLine(CommonHelper.TrimString(tmp, 35));
                            builder.Append("<br />");
                        }

                        for (int i = 0; i < 28 - optionSecondList.Count(); i++)
                        {
                            builder.Append("<br />");
                        }
                    }

                    if (settingRow.RetailPrice.GetValueOrDefault() && settingRow.DealerDiscount.GetValueOrDefault())
                        builder.AppendLine("<br /><br />");
                    else if (settingRow.RetailPrice.GetValueOrDefault() == false &&
                             settingRow.DealerDiscount.GetValueOrDefault() == false)
                    {
                        builder.AppendLine("<br /><br /><br /><br /><br /><br /><br />");
                    }
                    else
                    {
                        builder.AppendLine("<br /><br /><br /><br /><br />");
                    }


                    builder.AppendLine("</td>");

                    builder.AppendLine("</font>");
                    builder.AppendLine("</tr>");


                }

                builder.AppendLine("<tr valign=\"bottom\">");

                builder.AppendLine("<td align=\"right\" width=\"300\" colspan=\"3\">");
                builder.AppendLine("<font size=\"5\">");


                int salePriceNumber = 0;
                int retailPriceNumber = 0;
                int dealerDiscountNumber = 0;
                bool salePriceFlag = Int32.TryParse(row.RetailPrice, out retailPriceNumber);
                bool dealerdiscountFlag = Int32.TryParse(row.DealerDiscount, out dealerDiscountNumber);
                if (settingRow.RetailPrice.GetValueOrDefault())
                {
                    if (salePriceFlag)
                        builder.AppendLine(settingRow.RetailPriceText + ": " + retailPriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;"+
                                           "<br />");
                }

                if (settingRow.DealerDiscount.GetValueOrDefault())
                {
                    if (dealerdiscountFlag)
                        builder.AppendLine(settingRow.DealerDiscountText + ": " + dealerDiscountNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;"+
                                           "<br /><br />");
                }

                salePriceNumber = retailPriceNumber - dealerDiscountNumber;

                if (settingRow.SalePrice.GetValueOrDefault())
                {
                    builder.AppendLine(settingRow.SalePriceText + ": " + salePriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;");
                }

                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("</tr>");

                builder.AppendLine("</table>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("</table>");

                builder.AppendLine("</div>");

            }
            else
            {
                string barCode =
                    "<img height=\"20\" width=\"180\" src=\"http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" +
                    row.VINNumber +
                    "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false\" />";

                builder.AppendLine("<div id=\"bg\">");

                builder.AppendLine("<table id=\"window-sticker\" width=\"320\">");

                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"50\" rowspan=\"2\"></td>");

                builder.AppendLine("<td width=\"50\" ></td>");

                builder.AppendLine("<br />");
                builder.AppendLine("<br />");

                builder.AppendLine("</tr>");

                builder.AppendLine("<tr>");
                builder.AppendLine("<td>");

                builder.AppendLine("<table id=\"info-table\" width=\"400\">");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td colspan=\"2\" align=\"center\">");
                builder.AppendLine("<font size=\"5\">" + row.ModelYear.GetValueOrDefault() + " " + row.Make + " " +
                                   row.Model + " " + row.Trim + "</font><br />");

                builder.AppendLine("<font size=\"4\">" + row.Cylinders + " Cylinders " + row.FuelType + "</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("<tr>");

                builder.AppendLine("<td width=\"200\" valign=\"top\">");
                builder.AppendLine("<font size=\"4\">");

                int odometerNumber = 0;

                bool odometerFlag = Int32.TryParse(row.Mileage, out odometerNumber);

                if (odometerFlag)
                    builder.AppendLine("<em>Mileage:</em> <b>" + odometerNumber.ToString("#,##0") + "</b><br />");
                builder.AppendLine("<em>Stock Number:</em> <b>" + row.StockNumber + "</b><br />");

                builder.AppendLine("<em>Color:</em> <b>" + CommonHelper.TrimString(row.ExteriorColor, 17) + "</b><br />");
                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("<td valign=\"top\">");

                builder.AppendLine("<font size=\"4\">");

                if (row.Tranmission != null)
                {
                    builder.AppendLine(row.Tranmission.ToLower().Contains("auto")
                                           ? "<em>Transmission:</em> <b>Automatic</b><br />"
                                           : "<em>Transmission:</em> <b>Manual</b><br />");
                }
                builder.AppendLine("<em>VIN:</em> <b>" + row.VINNumber + "</b><br />");
                builder.AppendLine("<b>" + barCode + "</b><br />");
                builder.AppendLine("</font>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("<tr>");
                builder.AppendLine("<font size=\"2\">");
                builder.AppendLine("<td height=\"800\" valign=\"top\">");


                if (settingRow.RetailPrice.GetValueOrDefault() && settingRow.DealerDiscount.GetValueOrDefault())
                    builder.AppendLine("<br /><br />");
                else if (settingRow.RetailPrice.GetValueOrDefault() == false &&
                         settingRow.DealerDiscount.GetValueOrDefault() == false)
                {
                    builder.AppendLine("<br /><br /><br /><br /><br /><br /><br />");
                }
                else
                {
                    builder.AppendLine("<br /><br /><br /><br /><br />");
                }

                builder.AppendLine(
                    "<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />");
                builder.AppendLine("</td>");

                builder.AppendLine("</font>");
                builder.AppendLine("</tr>");
                builder.AppendLine("<tr valign=\"bottom\">");

                builder.AppendLine("<td align=\"right\" width=\"300\" colspan=\"3\">");
                builder.AppendLine("<font size=\"5\">");

                int salePriceNumber = 0;
                int retailPriceNumber = 0;
                int dealerDiscountNumber = 0;
                bool salePriceFlag = Int32.TryParse(row.RetailPrice, out retailPriceNumber);
                bool dealerdiscountFlag = Int32.TryParse(row.DealerDiscount, out dealerDiscountNumber);
                if (settingRow.RetailPrice.GetValueOrDefault())
                {
                    if (salePriceFlag)
                        builder.AppendLine(settingRow.RetailPriceText + ": " + retailPriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;" + 
                                           "<br />");
                }

                if (settingRow.DealerDiscount.GetValueOrDefault())
                {
                    if (dealerdiscountFlag)
                        builder.AppendLine(settingRow.DealerDiscountText + ": " + dealerDiscountNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;" + 
                                           "<br /><br />");
                }

                salePriceNumber = retailPriceNumber - dealerDiscountNumber;

                if (settingRow.SalePrice.GetValueOrDefault())
                {
                    builder.AppendLine(settingRow.SalePriceText + ": " + salePriceNumber.ToString("c0") + "&nbsp;&nbsp;&nbsp;&nbsp;");
                }


                builder.AppendLine("</font>");

                builder.AppendLine("</td>");
                builder.AppendLine("</tr>");

                builder.AppendLine("</table>");
                builder.AppendLine("</td>");

                builder.AppendLine("</tr>");
                builder.AppendLine("</table>");

                builder.AppendLine("</div>");
            }
        }

        public static string BuildWindowStickerInHtml()
        {
            var context = new whitmanenterprisewarehouseEntities();

            var rows = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(x => x.NewUsed.ToLower().Equals("used") && (x.Recon == null || !((bool)x.Recon)));

            var settingRow = InventoryQueryHelper.GetSingleOrGroupSetting(context);

            var builder = new StringBuilder();

            builder.AppendLine(" <!DOCTYPE html>");
            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine("<title></title>");
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");
            builder.AppendLine("<font face=\"Trebuchet MS\">");

            foreach (var row in rows)
            {
                BuildWindowStickerBody(builder, row, settingRow.FirstOrDefault(i => row.DealershipId == i.DealershipId));
            }

            builder.AppendLine("</font>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }
    }
}
