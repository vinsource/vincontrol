using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class CommonHelper
    {
        private const double RADIO = 3958.75587; // Mean radius of Earth in Miles
        private const string DisplayDefaultSoldOut = "Delete (Default)";
        private const string Display3DaysSoldOut = "Display as Sold (3 Days)";
        private const string Display5DaysSoldOut = "Display as Sold (5 Days)";
        private const string Display7DaysSoldOut = "Display as Sold (7 Days)";
        private const string Display30DaysSoldOut = "Display as Sold (30 Days)";
        
        public CommonHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        public string UppercaseWords(string value)
        {
            value = value.ToLower();
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        
        public static decimal FormatPurePrice(string salePrice)
        {
            decimal price = 0;

            if (!String.IsNullOrEmpty(salePrice))

                Decimal.TryParse(salePrice.Substring(1, salePrice.Length - 1), out price);

            return price;
        }

        public static int GetMaxLoop(int number, int denominator)
        {
            if (number % denominator == 0)
                return number / denominator;
            return number / denominator + 1;
        }

        public static decimal GetMaxPrice(List<decimal> listPrice)
        {
            if (listPrice.Count != 0)
                return listPrice.ToArray().Max();
            return 0;
        }

        public static decimal GetMinPrice(List<decimal> listPrice)
        {
            if (listPrice.Count != 0)
                return listPrice.ToArray().Min();
            return 0;
        }

        public static decimal GetAveragePrice(List<decimal> listPrice)
        {
            if (listPrice.Count != 0)
                return Math.Round(listPrice.ToArray().Average());
            return 0;
        }

        public static int DistanceBetweenPlaces(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 * Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return Convert.ToInt32(result2);
        }

        public static Int64 DistanceBetweenPlaces(string lat1, string lon1, string lat2, string lon2)
        {
            return Convert.ToInt64(Math.Round(Distance(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2), 'M'), 0));
        }
        
        private static double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }
        
        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        
        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public static string RemoveSpecialCharacters(string input)
        {
            //string tmp = null;

            //tmp = Regex.Replace(input, @"\s+", @" ", RegexOptions.Compiled);

            //tmp = Regex.Replace(tmp, @"[\r\n\b\t\x00\%\x1a\\'""]", @"", RegexOptions.Compiled);
            return Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

            //regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            //tmp = regex.Replace(tmp, @"");

            //return tmp;
        }

        public static string RemoveSpecialCharactersForMsrp(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
            } 
            
            return "0";
        }

        public static string RemoveSpecialCharactersForPurePrice(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {

                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
            } 
            
            return "";
        }

        public static string RemoveSpecialCharactersForSearchStock(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {

                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9-]+", "", RegexOptions.Compiled);
            } 
            
            return "";
        }
        
        public static string TrimString(string s)
        {
            if (s == null)
                return string.Empty;

            int maxSize = 25;
            int count = Math.Min(s.Length, maxSize);
            return s.Substring(0, count);
        }
        
        public static string TrimString(string s, int maxSize)
        {
            if (s == null)
                return string.Empty;

            int count = Math.Min(s.Length, maxSize);
            return s.Substring(0, count);
        }

        public static string ConvertArrayToString(string[] array)
        {
            string result = "";

            foreach (string tmp in array)
            {
                result += tmp + " ";
                break;
            }

            return result;
        }

        public static string ConvertArrayToString2(string[] array)
        {
            string result = "";

            foreach (string tmp in array)
            {
                result += tmp + " ";
            }
            return result;
        }
        
        public static int CountDaysLeft(string soldOut, int daysInSoldInvenotry)
        {
            int result = 0;

            if (soldOut.Equals(DisplayDefaultSoldOut))
            {
                result = -1;
            }
            else if (soldOut.Equals(Display3DaysSoldOut))
            {
                result = 3 - daysInSoldInvenotry;
            }
            else if (soldOut.Equals(Display5DaysSoldOut))
            {
                result = 5 - daysInSoldInvenotry;
            }
            else if (soldOut.Equals(Display7DaysSoldOut))
            {
                result = 7 - daysInSoldInvenotry;
            }
            else if (soldOut.Equals(Display30DaysSoldOut))
            {
                result = 30 - daysInSoldInvenotry;
            }

            return result;
        }
        
        public static DateTime GetNextFridayForKbb()
        {
            DateTime dtNow = DateTime.Now;

            for (int i = 1; i < 8; i++)
            {
                DateTime dt = dtNow.AddDays(i);
                if (dt.DayOfWeek.Equals(DayOfWeek.Friday))
                    return dt;
            }

            return dtNow;
        }

        public static DateTime GetLastFridayForKBB()
        {
            DateTime dtNow = DateTime.Now;

            for (int i = 1; i < 8; i++)
            {
                DateTime dt = dtNow.AddDays(i * (-1));
                if (dt.DayOfWeek.Equals(DayOfWeek.Friday))
                    return dt;
            }

            return dtNow;
        }

        public static string ConvertToCurrency(string price)
        {
            decimal number = 0;

            bool flag = Decimal.TryParse(price, out number);

            return number.ToString("c0");
        }

        public static DateTime GetDateTime(string s)
        {
            return DateTime.Parse(s);
        }

        public static string FormatNumberInThousand(string s)
        {
            string returnResult = "";

            if (!String.IsNullOrEmpty(s))
            {
                int odometerNumber = 0;

                bool odometerFlag = Int32.TryParse(s, out odometerNumber);

                returnResult = odometerNumber.ToString("#,##0");
                if (odometerFlag)

                    returnResult = odometerNumber.ToString("#,##0");
                else
                    returnResult = s;
            }

            return returnResult;
        }

        public static int ConvertStringToInterger(string s)
        {
            int oNumber = 0;

            if (!String.IsNullOrEmpty(s))
            {
                bool Flag = Int32.TryParse(s, out oNumber);

            }

            return oNumber;
        }

        #region Mapping Template Data Feed

        public string GetStringValueFromMappingField(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return string.Empty;
            if (row[mappingField.XMLField] != null)
            {
                var result = row[mappingField.XMLField].ToString();
                if (mappingField.Replaces != null && mappingField.Replaces.Any())
                {
                    foreach (var replace in mappingField.Replaces)
                    {
                        result.Replace(replace.From, replace.To);
                    }
                }

                return result;
            }

            return string.Empty;
        }

        public string GetStringValueFromMappingFieldNoHeader(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return string.Empty;
            if (row[Convert.ToInt32(mappingField.XMLField) - 1] != null)
            {
                var result = row[Convert.ToInt32(mappingField.XMLField) - 1].ToString();
                if (mappingField.Replaces != null && mappingField.Replaces.Any())
                {
                    foreach (var replace in mappingField.Replaces)
                    {
                        result = result.Replace(replace.From, replace.To);
                    }
                }

                return result;
            }

            return string.Empty;
        }

        public int GetIntValueFromMappingField(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return 0;
            return row[mappingField.XMLField] != null ? Convert.ToInt32(row[mappingField.XMLField]) : 0;
        }

        public int GetIntValueFromMappingFieldNoHeader(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return 0;
            return row[Convert.ToInt32(mappingField.XMLField) -1] != null ? Convert.ToInt32(row[Convert.ToInt32(mappingField.XMLField) - 1]) : 0;
        }

        public decimal GetDecimalValueFromMappingField(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return 0M;
            return row[mappingField.XMLField] != null ? Convert.ToDecimal(row[mappingField.XMLField]) : 0M;
        }

        public decimal GetDecimalValueFromMappingFieldNoHeader(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return 0M;
            return row[Convert.ToInt32(mappingField.XMLField) - 1] != null ? Convert.ToDecimal(row[Convert.ToInt32(mappingField.XMLField) - 1]) : 0M;
        }

        public bool GetBoolValueFromMappingField(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return false;
            return row[mappingField.XMLField] != null ? Convert.ToBoolean(row[mappingField.XMLField]) : false;
        }

        public bool GetBoolValueFromMappingFieldNoHeader(DataRow row, Mapping mappingField)
        {
            if (mappingField == null)
                return false;
            return row[Convert.ToInt32(mappingField.XMLField) - 1] != null ? Convert.ToBoolean(row[Convert.ToInt32(mappingField.XMLField) - 1]) : false;
        }

        public string GetStringValueFromAttribute(XmlElement element, string name)
        {
            return element.Attributes[name] != null ? element.Attributes[name].Value : string.Empty;
        }

        public int GetIntValueFromAttribute(XmlElement element, string name)
        {
            return element.Attributes[name] != null ? Convert.ToInt32(element.Attributes[name].Value) : 0;
        }

        public bool GetBoolValueFromAttribute(XmlElement element, string name)
        {
            return element.Attributes[name] != null ? Convert.ToBoolean(element.Attributes[name].Value) : false;
        }

        public XmlElement CreateFieldElement(XmlDocument doc, string node, string name, string value)
        {
            var field = doc.CreateElement(node);
            field.SetAttribute(name, value);

            return field;
        }

        public static byte[] GetBytes(Stream input)
        {
            var buffer = new byte[input.Length];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public MappingViewModel CreateSampleMappingData()
        {
            var model = new MappingViewModel() { Delimeter = "|", HasHeader = true };
            model.Mappings = new List<Mapping>()
                                 {
                                     new Mapping() { Order = 1, DBField = "DealershipId", XMLField = "Dealer ID", Expression = new Expression() },
                                     new Mapping()
                                         {
                                             Order = 2,
                                             DBField = "DealershipName",
                                             XMLField = "Dealer Name",
                                             Expression = new Expression()
                                         },
                                     new Mapping()
                                         {
                                             Order = 3,
                                             DBField = "DealershipAddress",
                                             XMLField = "Address",
                                             Expression = new Expression()
                                         },
                                     new Mapping() { Order = 4, DBField = "DealershipCity", XMLField = "City", Expression = new Expression() },
                                     new Mapping() { Order = 5, DBField = "DealershipState", XMLField = "State", Expression = new Expression() },
                                     new Mapping()
                                         {
                                             Order = 6,
                                             DBField = "DealershipZipCode",
                                             XMLField = "Zip Code",
                                             Expression = new Expression()
                                         },
                                     new Mapping()
                                         {
                                             Order = 7,
                                             DBField = "DealershipPhone",
                                             XMLField = "Phone Number",
                                             Expression = new Expression()
                                         },
                                     new Mapping() { Order = 8, DBField = "ModelYear", XMLField = "Year", Expression = new Expression() },
                                     new Mapping() { Order = 9, DBField = "Make", XMLField = "Make", Expression = new Expression() },
                                     new Mapping() { Order = 10, DBField = "Model", XMLField = "Model", Expression = new Expression() },
                                     new Mapping() { Order = 11, DBField = "Trim", XMLField = "Trim", Expression = new Expression() },
                                     new Mapping() { Order = 12, DBField = "VINNumber", XMLField = "VIN", Expression = new Expression() },
                                     new Mapping() { Order = 13, DBField = "Mileage", XMLField = "Mileage", Expression = new Expression() },
                                     new Mapping() { Order = 14, DBField = "SalePrice", XMLField = "Price", Expression = new Expression() },
                                     new Mapping() { Order = 15, DBField = "", XMLField = "Special_Price", Expression = new Expression() },
                                     new Mapping()
                                         {
                                             Order = 16,
                                             DBField = "ExteriorColor",
                                             XMLField = "Exterior Color",
                                             Expression = new Expression()
                                         },
                                     new Mapping()
                                         {
                                             Order = 17,
                                             DBField = "InteriorColor",
                                             XMLField = "Interior Color",
                                             Expression = new Expression()
                                         },
                                     new Mapping()
                                         {
                                             Order = 18,
                                             DBField = "Tranmission",
                                             XMLField = "Transmission",
                                             Expression = new Expression()
                                         },
                                     new Mapping() { Order = 19, DBField = "CarImageUrl", XMLField = "Image", Expression = new Expression() },
                                     new Mapping() { Order = 20, DBField = "", XMLField = "Comments", Expression = new Expression() },
                                     new Mapping() { Order = 21, DBField = "CarsOptions", XMLField = "Options", Expression = new Expression() },
                                     new Mapping()
                                         {
                                             Order = 22,
                                             DBField = "NewUsed",
                                             XMLField = "Status",
                                             Expression = new Expression(),
                                             Replaces = new List<Replacement>()
                                                            {
                                                                new Replacement() {From = "NR", To = "New"},
                                                                new Replacement() {From = "UR", To = "Used"},
                                                                new Replacement() {From = "UF", To = "Used"}
                                                            },
                                            Conditions = new List<Condition>()
                                                                {
                                                                    new Condition() { XMLField = "Status", Operator = "==", ComparedValue = "UR", DBField = "NewUsed", TargetValue = "Used", Type = "String" },
                                                                    new Condition() { XMLField = "Status", Operator = "==", ComparedValue = "UF", DBField = "NewUsed", TargetValue = "Used", Type = "String" },
                                                                    new Condition() { XMLField = "Status", Operator = "==", ComparedValue = "NR", DBField = "NewUsed", TargetValue = "New", Type = "String" }
                                                                }
                                         }
                                 };

            return model;
        }

        #endregion
    }
}
