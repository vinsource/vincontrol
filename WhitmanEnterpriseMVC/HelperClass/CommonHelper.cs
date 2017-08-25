using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public sealed class CommonHelper
    {
        private const double RADIO = 3958.75587; // Mean radius of Earth in Miles
        private const string DisplayDefaultSoldOut = "Delete (Default)";
        private const string Display3DaysSoldOut = "Display as Sold (3 Days)";
        private const string Display5DaysSoldOut = "Display as Sold (5 Days)";
        private const string Display7DaysSoldOut = "Display as Sold (7 Days)";
        private const string Display30DaysSoldOut = "Display as Sold (30 Days)";
        
        public static string UpperFirstLetterOfEachWord(string value)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }



        public static string UppercaseWords(string value)
        {
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

            bool flagtmp = false;

            if (!String.IsNullOrEmpty(salePrice))
                //flagtmp = Decimal.TryParse(salePrice.Substring(1, salePrice.Length - 1), out price);
                flagtmp = Decimal.TryParse(salePrice.Substring(0, salePrice.Length), out price);
            
            return price;
        }

        public static int GetMaxLoop(int number, int denominator)
        {
            if (number % denominator == 0)
                return number / denominator;
            else
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
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return Convert.ToInt32(result2);
        }

        public static Int64 DistanceBetweenPlaces(string lat1, string lon1, string lat2, string lon2)
        {
            return Convert.ToInt64(Math.Round(Distance(Convert.ToDouble(lat1), Convert.ToDouble(lon1), Convert.ToDouble(lat2), Convert.ToDouble(lon2),'M'),0));
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

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        
        public static string RemoveSpecialCharacters(string input)
        {
            //string tmp = null;

            //tmp = Regex.Replace(input, @"\s+", @" ", RegexOptions.Compiled);

            //tmp = Regex.Replace(tmp, @"[\r\n\b\t\x00\%\x1a\\'""]", @"", RegexOptions.Compiled);
            return Regex.Replace(input??String.Empty, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

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
            } return "0";

           
        }

        public static string RemoveSpecialCharactersForPurePrice(string input)
        {

            if (!String.IsNullOrEmpty(input))
            {

                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
            } return "";
        }

        public static int RemoveSpecialCharactersAndReturnNumber(string input)
        {
            int number = 0;

            if (!String.IsNullOrEmpty(input))
            {
                string tmp= Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

                Int32.TryParse(tmp, out number);

            }
            return number;
        }

        public static string RemoveSpecialCharactersForSearchStock(string input)
        {

            if (!String.IsNullOrEmpty(input))
            {

                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9-]+", "", RegexOptions.Compiled);
            } return "";
        }
     
        public static string TrimString(string s)
        {
            if (s == null)
                return string.Empty;

            int maxSize = 25;
            int count = Math.Min(s.Length, maxSize);
            return s.Substring(0, count);
        }

        public static string TrimString(string s,int maxSize)
        {
            if (String.IsNullOrEmpty(s))
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
        
        public static string GetShortDrive(string wheelDrive)
        {
            XmlNode driveNode = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Value=" + wheelDrive);

            if (driveNode != null)

                return driveNode.Attributes["Short"].Value;

            return "";

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
                result= 3 - daysInSoldInvenotry;
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
        
        public static DateTime GetNextFriday()
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

        public static DateTime GetLastFridayForKbb()
        {
            DateTime dtNow = DateTime.Now;

            for (int i = 1; i < 8; i++)
            {
                DateTime dt = dtNow.AddDays(i*(-1));
                if (dt.DayOfWeek.Equals(DayOfWeek.Friday))
                    return dt;
            }
            return dtNow;

        }

        public static string ConvertToCurrency(string price)
        {
            if (price == null) return "$0";
            if (price.Contains("$")) return price;

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

        public static string FormatNumberInThousand(int odometerNumber)
        {
            string returnResult = odometerNumber.ToString("#,##0");


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

        public static string[] GetArrayFromString(string dataString)
        {
            return String.IsNullOrEmpty(dataString.Trim()) ? new[] { "" } : dataString.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        public static string[] GetArrayFromStringWithoutMoney(string dataString)
        {
            return String.IsNullOrEmpty(dataString.Trim()) ? new[] { "" } : dataString.Split(new[] { ",", "$", "." }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        public static byte[] GetBytes(Stream input)
        {
            var buffer = new byte[1024 * 1024];
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
    }
}
