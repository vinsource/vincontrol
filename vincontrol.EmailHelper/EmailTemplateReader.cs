using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.EmailHelper
{
    public class EmailTemplateReader
    {
        #region Constants
        public const string UserName = "@@UserName";
        public const string Password = "@@Password";
        public const string CustomerName = "@@CustomerName";
        public const string UserFullName = "@@UserFullName";
        public const string DealerName = "@@DealerName";
        public const string DealerWebsite = "@@DealerWebsite";
        public const string Email = "@@Email";
        public const string Phone = "@@Phone";
        public const string Address = "@@Address";
        public const string Vin = "@@Vin";
        public const string Stock = "@@Stock";
        public const string Year = "@@Year";
        public const string Make = "@@Make";
        public const string Model = "@@Model";
        public const string Trim = "@@Trim";
        public const string ExteriorColor = "@@ExteriorColor";
        public const string Transmission = "@@Transmission";
        public const string Option = "@@Option";
        public const string LandingPageURL = "@@LandingPageURL";
        public const string BrochureURL = "@@BrochureURL";
        public const string CarBuilderURL = "@@CarBuilderURL";
        public const string SurveyTemplate = "@@SurveyTemplate";
        public const string SurveyUrl = "@@SurveyUrl";
        public const string DealerReviewWebsite = "@@DealerReviewWebsite";
        public const string CommunicationUrl = "@@CommunicationUrl";
        public const string ReceiverName = "@@ReceiverName";
        public const string FacebookFanPage = "@@FacebookFanPage";
        public const string FacebookPost = "@@FacebookPost";
        public const string FacebookPostUrl = "@@FacebookPostUrl";

        public const string ManagerName = "@@ManagerName";
        public const string SalePersonName = "@@SalePersonName";
        public const string Content = "@@Content";
        public const string DateTime = "@@DateTime";

        public const string EndingSentence = "@@EndingSentence";
        public const string Description = "@@Description";

        #endregion

        public static string GetFlyerEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\FlyerEmail.txt");
        }

        public static string GetBrochureEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\BrochureEmail.txt");
        }

        public static string GetBrochureEmailWithoutLinkContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\BrochureEmailWithoutLink.txt");
        }

        public static string GetSurveyEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\SurveyEmail.txt");
        }

        public static string GetGoodSurveyEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\GoodSurveyEmail.txt");
        }

        public static string GetCommunicationEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\CommunicationEmail.txt");
        }

        public static string GetFacebookPostNotificationEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug","") + "Templates\\FacebookPostNotificationEmail.txt");
        }

        public static string GetForgotPasswordEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\ForgotPasswordEmail.txt");
        }

        public static string GetContactManagerEmailContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\ContactManagerEmail.txt");
        }

        public static string GetCraigslistAdsBodyContent()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Templates\\CraigslistAdsBody.txt");
        }
    }
}
