using System.Configuration;

namespace vincontrol.ConfigurationManagement
{
    public class ConfigurationHandler
    {
        private const string _caxFaxProductDataId = "CaxFaxProductDataId";
        private const string _caxFaxServerURL = "CaxFaxServerURL";
        private const string _smtpServer = "SMTPServer";
        private const string _defaultFromEmail = "DefaultFromEmail";
        private const string _displayName = "DisplayName";
        private const string _trackEmailAccount = "TrackEmailAccount";
        private const string _trackEmailPass = "TrackEmailPass";
        private const string _pdfSerialNumber = "PDFSerialNumber";
        private const string _tradeInEmail = "TradeInEmail";
        private const string _tradeInDisplayName = "TradeInDisplayName";
        private const string _uploadFolderPath = "UploadFolderPath";
        private const string _dealerImages = "DealerImages";
        private const string _pendragon = "Pendragon";
        private const string _disableSilverlight = "DisableSilverlight";

        // Web Server
        private const string _webServerUrl = "WebServerURL";

        // Email
        private const string _isTestEmail = "IsTestEmail";
        private const string _testEmail = "TestEmail";
        
        // Facebook API
        private const string _appId = "facebook:AppID";
        private const string _appSecret = "facebook:AppSecret";
        private const string _pageToken = "facebook:PageToken";
        private const string _personalId = "facebook:PersonalId";
        private const string _fanPage = "facebook:FanPage";
        private const string _graphAPI = "facebook:GraphAPI";

        private const string _vinsocial = "VINSocial";

        //Youtube API
        private const string _youtubeClientId = "youtube:ClientID";
        private const string _youtubeClientSecret = "youtube:ClientSecret";
        private const string _youtubeDeveloperKey = "youtube:DeveloperKey";
        private const string _youtubeUserId = "youtube:UserID";

        //Google API
        private const string _googleAPIKey = "google:APIKey";

        //SSRS
        private const string _ssrsReportHost = "ssrs:ReportHost";
        private const string _ssrsGeneralReportFolder = "ssrs:GeneralReportFolder";

        //Craigslist
        private const string _craigslistEmail = "craigslist:Email";
        private const string _craigslistPassword = "craigslist:Password";
        private const string _secureNetUsername = "SecureNetUserName";
        private const string _secureNetPassword = "SecureNetPassword";

        public static string Pendragon
        {
            get
            {
                return (ConfigurationManager.AppSettings[_pendragon]) ?? string.Empty;
            }
        }

        public static string WebServerUrl
        {
            get
            {
                return (ConfigurationManager.AppSettings[_webServerUrl]) ?? string.Empty;
            }
        }

        public static bool IsTestEmail
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings[_isTestEmail]);
            }
        }

        public static string TestEmail
        {
            get
            {
                return ConfigurationManager.AppSettings[_testEmail];
            }
        }

        public static string CaxFaxProductDataId
        {
            get
            {
                return ConfigurationManager.AppSettings[_caxFaxProductDataId];
            }
        }

        public static string CaxFaxServerURL
        {
            get
            {
                return ConfigurationManager.AppSettings[_caxFaxServerURL];
            }
        }

        public static string SMTPServer
        {
            get
            {
                return ConfigurationManager.AppSettings[_smtpServer];
            }
        }

        public static string DefaultFromEmail
        {
            get
            {
                return ConfigurationManager.AppSettings[_defaultFromEmail];
            }
        }

        public static string DisplayName
        {
            get
            {
                return ConfigurationManager.AppSettings[_displayName];
            }
        }

        public static string TrackEmailAccount
        {
            get
            {
                return ConfigurationManager.AppSettings[_trackEmailAccount];
            }
        }

        public static string TrackEmailPass
        {
            get
            {
                return ConfigurationManager.AppSettings[_trackEmailPass];
            }
        }

        public static string PdfSerialNumber
        {
            get
            {
                return ConfigurationManager.AppSettings[_pdfSerialNumber];
            }
        }

        public static string TradeInEmail
        {
            get
            {
                return ConfigurationManager.AppSettings[_tradeInEmail];
            }
        }

        public static string TradeInDisplayName
        {
            get
            {
                return ConfigurationManager.AppSettings[_tradeInDisplayName];
            }
        }

        public static string FacebookAppId
        {
            get
            {
                return ConfigurationManager.AppSettings[_appId];
            }
        }

        public static string FacebookAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings[_appSecret];
            }
        }

        public static string FacebookPageToken
        {
            get
            {
                return ConfigurationManager.AppSettings[_pageToken];
            }
        }

        public static string FacebookPersonalId
        {
            get
            {
                return ConfigurationManager.AppSettings[_personalId];
            }
        }

        public static string FacebookFanPage
        {
            get
            {
                return ConfigurationManager.AppSettings[_fanPage];
            }
        }

        public static string FacebookGraphAPI
        {
            get
            {
                return ConfigurationManager.AppSettings[_graphAPI];
            }
        }

        public static string UploadFolderPath
        {
            get
            {
                return ConfigurationManager.AppSettings[_uploadFolderPath];
            }
        }

        public static string DealerImages
        {
            get
            {
                return ConfigurationManager.AppSettings[_dealerImages];
            }
        }

        public static string VINSocial
        {
            get
            {
                return ConfigurationManager.AppSettings[_vinsocial];
            }
        }

        public static string YoutubeClientId
        {
            get
            {
                return ConfigurationManager.AppSettings[_youtubeClientId];
            }
        }

        public static string YoutubeClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings[_youtubeClientSecret];
            }
        }

        public static string YoutubeDeveloperKey
        {
            get
            {
                return ConfigurationManager.AppSettings[_youtubeDeveloperKey];
            }
        }

        public static string YoutubeUserId
        {
            get
            {
                return ConfigurationManager.AppSettings[_youtubeUserId];
            }
        }

        public static string GoogleAPIKey
        {
            get
            {
                return ConfigurationManager.AppSettings[_googleAPIKey];
            }
        }

        public static string SSRSReportHost
        {
            get
            {
                return ConfigurationManager.AppSettings[_ssrsReportHost];
            }
        }

        public static string SSRSGeneralReportFolder
        {
            get
            {
                return ConfigurationManager.AppSettings[_ssrsGeneralReportFolder];
            }
        }

        public static string CraigslistEmail
        {
            get
            {
                return ConfigurationManager.AppSettings[_craigslistEmail];
            }
        }

        public static string CraigslistPassword
        {
            get
            {
                return ConfigurationManager.AppSettings[_craigslistPassword];
            }
        }

        public static string SecureNetUserName
        {
            get
            {
                return ConfigurationManager.AppSettings[_secureNetUsername];
            }
        }

        public static string SecureNetPassword
        {
            get
            {
                return ConfigurationManager.AppSettings[_secureNetPassword];
            }
        }

        public static string DisableSilverlight
        {
            get
            {
                return (ConfigurationManager.AppSettings[_disableSilverlight]) ?? string.Empty;
            }
        }
    }
}
