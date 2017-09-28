using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Microsoft.SqlServer.Server;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.EmailHelper;
using vincontrol.Helper;
using vincontrol.Data.Model;
using vincontrol.Data.Model.CLDMS;

namespace VINControl.Craigslist
{
    public class CraigslistService
    {
        //Email: TestyFlavor518@yahoo.com - Password: wCjdpMyD02
        #region Const
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0;" +
                                         "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                         ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                         "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private const string CryptedStepCheckPattern = "<input type=\"hidden\" name=\"cryptedStepCheck\" value=\"([^\\\"]*)\">";
        private const string AcceptLanguage = "Accept-Language: en-US,en;q=0.5";
        private const string AcceptEncoding = "Accept-Encoding: gzip, deflate";
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string LogInUrl = "https://accounts.craigslist.org/login";
        private const string SubmitLogInUrl = "https://accounts.craigslist.org/login";
        #endregion

        private ICommonManagementForm _commonManagementForm;
        private IDealerManagementForm _dealerManagementForm;
        private IInventoryManagementForm _inventoryManagementForm;

        #region Properties

        private IEmail _emailHelper;
        public string Email { get; set; }
        public string Password { get; set; }
        public int StatusCode { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public string CryptedStepCheck { get; set; }
        #endregion

        #region Constructor
        public CraigslistService()
        {
            CookieContainer = new CookieContainer();
            CookieCollection = new CookieCollection();
            _emailHelper = new vincontrol.EmailHelper.Email();
            _commonManagementForm = new CommonManagementForm();
            _dealerManagementForm = new DealerManagementForm();
            _inventoryManagementForm = new InventoryManagementForm();
        }
        #endregion

        #region Public Methods
        public void Execute(string email, string password, DealerViewModel dealer, CarShortViewModel car)
        {
            //Step 1: check existing cookies
            WebRequestGet();
            //Step 2: log on
            WebRequestPost(email, password);
            //Step 3: get location & Crypted code
            var locationPostUrl = GetLocationPostUrl(dealer.CraigslistSetting.CityUrl);
            var locationUrl = GetEncodedLocationUrl(locationPostUrl);
            var typePostingUrl = GetTypePostingUrl(locationUrl);

            var cryptedStepCheck = GetCryptedStepCheckFromUrl(typePostingUrl);

            //Step 3: get category & Crypted code
            var categoryChoosingUrl = GetCategoryChoosingUrl(locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(categoryChoosingUrl);

            //Step 4: get sub location & Crypted code
            var subLocationChoosingUrl = GetSubLocationChoosingUrl(locationUrl, cryptedStepCheck);
            GetSubLocationList(subLocationChoosingUrl);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(subLocationChoosingUrl);

            //Step 4: go to create posting & get Crypted code
            var createPostingUrl = GetCreatePostingUrl(dealer.CraigslistSetting.LocationId, locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(createPostingUrl);

            //Step 5: posting
            var imageEditingUrl = Posting(dealer, car, locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(imageEditingUrl);

            //Step 6: upload images
            UploadImages(locationUrl, cryptedStepCheck, dealer, car);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(imageEditingUrl);

            //Step 7: preview
            var previewUrl = GetPreviewUrl(locationUrl, cryptedStepCheck);
            PreviewPosting(previewUrl, dealer, car);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(previewUrl);

            //Step 8: go to billing page & get Crypted code
            var billingUrl = GetBillingUrl(locationUrl, cryptedStepCheck, 1);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(billingUrl);
            GetBillingUrl(locationUrl, cryptedStepCheck, 2);

            //Step 9: payment
            var paymentUrl = GetPaymentUrl(billingUrl);
            var confirmationPaymentUrl = Purchase(paymentUrl, new CreditCardInfo());
        }

        public PostingPreview GoToPostingPreviewPage(string email, string password, DealerViewModel dealer, CarShortViewModel car)
        {
            //Step 2: log on
            WebRequestPost(email, password);
            if (StatusCode != 302)
            {
                return new PostingPreview { Post = null, Warning = "You forgot to input Username/Password in Admin setting? or Your account is invalid." };
            }

            if (string.IsNullOrEmpty(dealer.CraigslistSetting.CityUrl))
            {
                return new PostingPreview { Post = null, Warning = "You forgot to set State/City/Location in Admin setting. Let's do that first." };
            }

            var locationPostUrl = GetLocationPostUrl(dealer.CraigslistSetting.CityUrl);
            var locationUrl = GetEncodedLocationUrl(locationPostUrl);
            var typePostingUrl = GetTypePostingUrl(locationUrl);

            var cryptedStepCheck = GetCryptedStepCheckFromUrl(typePostingUrl);

            //Step 3: get category & Crypted code
            var categoryChoosingUrl = GetCategoryChoosingUrl(locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(categoryChoosingUrl);

            //Step 4: get sub location & Crypted code
            var subLocationChoosingUrl = GetSubLocationChoosingUrl(locationUrl, cryptedStepCheck);            
            cryptedStepCheck = GetCryptedStepCheckFromUrl(subLocationChoosingUrl);

            //Step 4: go to create posting & get Crypted code
            var createPostingUrl = GetCreatePostingUrl(dealer.CraigslistSetting.LocationId, locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(createPostingUrl);

            //Step 5: posting
            var imageEditingUrl = Posting(dealer, car, locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(imageEditingUrl);

            //Step 6: upload images
            UploadImages(locationUrl, cryptedStepCheck, dealer, car);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(imageEditingUrl);

            //Step 7: preview
            var previewUrl = GetPreviewUrl(locationUrl, cryptedStepCheck);
            var post = PreviewPosting(previewUrl, dealer, car);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(previewUrl);

            //Step 8: go to billing page & get Crypted code
            var billingUrl = GetBillingUrl(locationUrl, cryptedStepCheck, 1);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(billingUrl);
            string warning = null;
            if (billingUrl.Contains("s=mailoop"))
                warning = "This is your first post on this device so you should receive an email shortly, with a link to confirm your ad. Please check Inbox or Spam " + email;

            GetBillingUrl(locationUrl, cryptedStepCheck, 2);

            //Step 9: payment
            var paymentUrl = GetPaymentUrl(billingUrl);

            return new PostingPreview() 
            {
                Post = post,
                CryptedStepCheck = cryptedStepCheck,
                LocationUrl = paymentUrl,
                Warning = warning
            };
        }

        public ConfirmationPayment GoToPurchasingPage(string email, string password, CreditCardInfo creditCard)
        {
            WebRequestPost(email, password);                        
            var confirmationPaymentUrl = Purchase(creditCard.LocationUrl, creditCard);

            return GetConfirmationPaymentInfo(confirmationPaymentUrl);
        }

        public string UpdatePrice(string email, string password, int inventoryId, int newPrice)
        {
            var postingId = _dealerManagementForm.GetCraigslistPostingId(inventoryId);
            if (postingId.Equals(0)) return "NotPosted";

            //Step 1: check existing cookies
            WebRequestGet();
            //Step 2: log on
            WebRequestPost(email, password);
            
            var managedUrl = "https://post.craigslist.org/manage/" + postingId;
            var cryptedStepCheck = GetCryptedStepCheckForEditting(managedUrl);
            if (String.IsNullOrEmpty(cryptedStepCheck)) return "ExpiredOrDeleted";

            try
            {
                var car = _inventoryManagementForm.GetCarInfo(inventoryId);
                var dealer = _dealerManagementForm.GetDealer(car.DealerId);

                //Step 4: go to create posting & get Crypted code
                var locationUrl = String.Format("https://post.craigslist.org{0}", GetLocationUrlForEditting(managedUrl, cryptedStepCheck));
                cryptedStepCheck = GetCryptedStepCheckFromUrl(locationUrl + "?s=edit");

                //Step 5: posting
                car.Price = newPrice;
                var imageEditingUrl = Posting(dealer, car, locationUrl, cryptedStepCheck);
                cryptedStepCheck = GetCryptedStepCheckFromUrl(imageEditingUrl);

                //Step 7: preview
                var previewUrl = GetPreviewUrl(locationUrl, cryptedStepCheck);
                PreviewPosting(previewUrl, dealer, car);
                cryptedStepCheck = GetCryptedStepCheckFromUrl(previewUrl);

                //Step 8: go to billing page & get Crypted code
                var billingUrl = GetBillingUrl(locationUrl, cryptedStepCheck, 1);
                cryptedStepCheck = GetCryptedStepCheckFromUrl(billingUrl);

                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }

        #endregion

        #region WebRequestGet
        public void WebRequestGet()
        {
            var request = (HttpWebRequest)WebRequest.Create(LogInUrl);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Host = "accounts.craigslist.org";
            request.KeepAlive = true;
            request.Headers.Add(AcceptLanguage);
            request.Headers.Add(AcceptEncoding);
            request.UserAgent = UserAgent;
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);
            //Get the response from the server and save the cookies from the first request..
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                CookieCollection = response.Cookies;
            }
        }
        #endregion

        #region WebRequestPost
        public void WebRequestPost(string email, string password)
        {
            var postData = String.Format("step={0}&rt={1}&rp={2}&inputEmailHandle={3}&inputPassword={4}", "confirmation", string.Empty, string.Empty, WebUtility.UrlEncode(email), WebUtility.UrlEncode(password));

            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create(SubmitLogInUrl);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Host = "accounts.craigslist.org";
            request.Method = "POST";
            request.UserAgent = UserAgent;
            request.ContentLength = postData.Length;
            request.Headers.Add("Accept-Language: en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = ContentType;
            request.Accept = Accept;
            request.Referer = "https://accounts.craigslist.org/";
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            //var result = string.Empty;
            try
            {
                // Post to the login form.
                var streamWriter = new StreamWriter(request.GetRequestStream());
                streamWriter.Write(postData);
                streamWriter.Close();

                // Get the response.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // Have some cookies.
                    CookieCollection = response.Cookies;
                    StatusCode = (int)response.StatusCode;
                    // Read the response
                    //var streamReader = new StreamReader(response.GetResponseStream());
                    //var result = streamReader.ReadToEnd();
                    //if (result.Contains("Please try again")) StatusCode = 302;
                    //streamReader.Close();
                }
            }
            catch (Exception)
            {
                //result = string.Empty;
            }
        }
        #endregion

        #region GET
        private string GetCryptedStepCheckForEditting(string url)
        {            
            var content = WebHandler.DownloadContent(url, CookieContainer, CookieCollection);
            var cryptedStepCheck = new Regex("<input type=\"hidden\" name=\"crypt\" value=\"([^\\\"]*)\">");
            return cryptedStepCheck.Match(content).Groups[1].Value;            
        }

        public string GetEncodedLocationUrl(string locationPostUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(locationPostUrl);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Host = "post.craigslist.org";
            request.KeepAlive = true;
            request.Headers.Add(AcceptLanguage);
            request.Headers.Add(AcceptEncoding);
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.AllowAutoRedirect = false;
            request.Referer = locationPostUrl;
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                return String.Format("https://post.craigslist.org{0}", response.Headers["Location"]);
            }
        }

        public string GetTypePostingUrl(string locationUrl)
        {
            return ExecuteGet(locationUrl);
        }
        
        public string GetPaymentUrl(string billingUrl)
        {
            return ExecuteGet(billingUrl);
        }
        #endregion

        #region POST
        public string GetLocationUrlForEditting(string managedUrl, string cryptedStepCheck)
        {
            var postData = String.Format("action=edit&crypt={0}&go=Edit+this+Posting", cryptedStepCheck);
            return ExecutePost(managedUrl, postData);
        }

        public string GetCategoryChoosingUrl(string locationUrl, string cryptedStepCheck)
        {
            var postData = String.Format("id=fsd&cryptedStepCheck={0}&go=Continue", cryptedStepCheck);
            return ExecutePost(locationUrl, postData);
        }

        public string GetSubLocationChoosingUrl(string location)
        {
            var locationPostUrl = GetLocationPostUrl(location);
            var locationUrl = GetEncodedLocationUrl(locationPostUrl);
            var typePostingUrl = GetTypePostingUrl(locationUrl);

            var cryptedStepCheck = GetCryptedStepCheckFromUrl(typePostingUrl);
            var categoryChoosingUrl = GetCategoryChoosingUrl(locationUrl, cryptedStepCheck);
            cryptedStepCheck = GetCryptedStepCheckFromUrl(categoryChoosingUrl);

            return GetSubLocationChoosingUrl(locationUrl, cryptedStepCheck);
        }

        public string GetSubLocationChoosingUrl(string locationUrl, string cryptedStepCheck)
        {
            var postData = String.Format("id=146&cryptedStepCheck={0}&go=Continue", cryptedStepCheck);
            return ExecutePost(locationUrl, postData);
        }

        public string GetCreatePostingUrl(int subLocationId, string locationUrl, string cryptedStepCheck)
        {
            var postData = String.Format("n={0}&cryptedStepCheck={1}&go=Continue", subLocationId, cryptedStepCheck);
            return ExecutePost(locationUrl, postData);
        }

        private string GeneratePostingBody(DealerViewModel dealer, CarShortViewModel car)
        {
            var postingBody = EmailTemplateReader.GetCraigslistAdsBodyContent();
            postingBody = postingBody.Replace(EmailTemplateReader.DealerName, dealer.Name);
            postingBody = postingBody.Replace(EmailTemplateReader.Address, string.Format("{0} {1},{2} {3}", dealer.Address, dealer.City, dealer.State, dealer.ZipCode));
            postingBody = postingBody.Replace(EmailTemplateReader.Phone, dealer.Phone);
            postingBody = postingBody.Replace(EmailTemplateReader.DealerWebsite, String.IsNullOrEmpty(dealer.Setting.WebsiteUrl) ? dealer.Name : dealer.Setting.WebsiteUrl);
            postingBody = postingBody.Replace(EmailTemplateReader.Year, car.ModelYear.ToString());
            postingBody = postingBody.Replace(EmailTemplateReader.Make, car.Make);
            postingBody = postingBody.Replace(EmailTemplateReader.Model, car.Model);
            postingBody = postingBody.Replace(EmailTemplateReader.Trim, car.Trim);
            postingBody = postingBody.Replace(EmailTemplateReader.ExteriorColor, car.ExteriorColor);
            postingBody = postingBody.Replace(EmailTemplateReader.Transmission, car.Tranmission);
            postingBody = postingBody.Replace(EmailTemplateReader.Stock, car.StockNumber);
            postingBody = postingBody.Replace(EmailTemplateReader.Vin, car.Vin);
            postingBody = postingBody.Replace(EmailTemplateReader.Option, car.CarsOptions);
            postingBody = postingBody.Replace(EmailTemplateReader.EndingSentence, dealer.CraigslistSetting.EndingSentence);
            if (!String.IsNullOrEmpty(car.Description))
                postingBody = postingBody.Replace(EmailTemplateReader.Description, "<br/><b>Description</b><br/><br/>" + CommonHelper.ReplaceEmails(car.Description, string.Empty));
            
            return postingBody;
        }

        public string Posting(DealerViewModel dealer, CarShortViewModel car, string locationUrl, string cryptedStepCheck)
        {
            var postData = String.Format("language=5&condition=40&id2={0}&" +
                                         "browserinfo={1}&" +
                                         "contact_method={2}&" +
                                         "contact_phone_ok=1&" +
                                         "contact_phone={3}&" +
                                         "contact_name={4}&" +
                                         "FromEMail={5}&" +
                                         "PostingTitle={6}&" +
                                         "Ask={7}&" +
                                         "GeographicArea={8}&" +
                                         "postal={9}&" +
                                         "PostingBody={10}&" +
                                         "auto_year={11}&" +
                                         "auto_make_model={12}&" +
                                         "auto_miles={13}&" +
                                         "auto_vin={14}&" +
                                         "auto_fuel_type={15}&" +
                                         "auto_transmission={16}&" +
                                         "see_my_other={17}&" +
                                         "auto_title_status={18}&" +
                                         "Privacy=C&cryptedStepCheck={19}&condition={20}&sale_condition=excellent&oc=1&go=Continue",
                                         "1903x1045X1903x602X1920x1080",
                                         "%257B%250A%2509%2522plugins%2522%253A%2520%2522Plugin%25200%253A%2520Google%2520Update%253B%2520Google%2520Update%253B%2520npGoogleUpdate3.dll%253B%2520%2528%253B%2520application%2Fx-vnd.google.update3webcontrol.3%253B%2520%2529%2520%2528%253B%2520application%2Fx-vnd.google.oneclickctrl.9%253B%2520%2529.%2520Plugin%25201%253A%2520Silverlight%2520Plug-In%253B%25204.0.50826.0%253B%2520npctrl.dll%253B%2520%2528npctrl%253B%2520application%2Fx-silverlight%253B%2520scr%2529%2520%2528%253B%2520application%2Fx-silverlight-2%253B%2520%2529.%2520%2522%252C%250A%2509%2522timezone%2522%253A%2520480%252C%250A%2509%2522video%2522%253A%2520%25221920x1080x16%2522%252C%250A%2509%2522supercookies%2522%253A%2520%2522DOM%2520localStorage%253A%2520Yes%252C%2520DOM%2520sessionStorage%253A%2520Yes%252C%2520IE%2520userData%253A%2520No%2522%250A%257D",
                                         1,
                                         dealer.Phone,
                                         dealer.Name,
                                         this.Email,
                                         String.Format("{0} {1} {2} {3}", car.ModelYear, car.Make, car.Model, car.Trim),
                                         car.Price,
                                         dealer.CraigslistSetting.SpecificLocation,
                                         dealer.ZipCode,
                                         HttpUtility.UrlEncode(GeneratePostingBody(dealer, car)),
                                         car.ModelYear,
                                         String.Format("{0} {1}", car.Make, car.Model),
                                         car.Odometer, 
                                         car.Vin,
                                         GetFuelType(car),
                                         GetTransmission(car),
                                         1,
                                         1,
                                         cryptedStepCheck,
                                         GetCondition(car));

            return ExecutePost(locationUrl, postData);
        }

        private int GetCondition(CarShortViewModel car)
        {
            return car.Condition.ToLower().Equals("new") ? 10 : 40;
        }

        private int GetFuelType(CarShortViewModel car)
        {
            var fuel = car.Fuel.ToLower();
            if (fuel.Contains("gas")) return 1;
            if (fuel.Contains("diesel")) return 2;
            if (fuel.Contains("hybrid")) return 3;
            if (fuel.Contains("electric")) return 4;

            return 6;
        }

        private int GetTransmission(CarShortViewModel car)
        {
            var tm = car.Tranmission.ToLower();
            if (tm.Contains("manual")) return 1;
            if (tm.Contains("automatic")) return 2;
            
            return 3;
        }

        private int GetBodyStyle(CarShortViewModel car)
        {
            var tm = car.BodyType.ToLower();
            if (tm.Contains("bus")) return 1;
            if (tm.Contains("convertible")) return 2;
            if (tm.Contains("coupe")) return 3;
            if (tm.Contains("hatchback")) return 4;
            if (tm.Contains("mini van")) return 5;
            if (tm.Contains("offroad")) return 6;
            if (tm.Contains("pickup")) return 7;
            if (tm.Contains("sedan")) return 8;
            if (tm.Contains("truck")) return 9;
            if (tm.Contains("suv")) return 10;
            if (tm.Contains("wagon")) return 11;
            if (tm.Contains("van")) return 12;

            return 13;
        }

        public string UploadImages(string locationUrl, string cryptedStepCheck, DealerViewModel dealer, CarShortViewModel car)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var postData = new Dictionary<string, string>(); ;
            postData.Add("cryptedStepCheck", cryptedStepCheck);
            postData.Add("a", "add");
            postData.Add("go", "add image");

            var request = (HttpWebRequest)WebRequest.Create(locationUrl);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Host = "post.craigslist.org";
            request.Method = "POST";
            request.UserAgent = UserAgent;
            request.Headers.Add("Accept-Language: en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Accept = Accept;
            request.Referer = locationUrl + "?s=editimage";
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);
            request.Timeout = 1000000;

            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    postData.WriteMultipartFormData(requestStream, boundary);

                    var dirInfo = new DirectoryInfo(dealer.DealerImagesFolder + "/" + dealer.DealerId + "/" + car.Vin + "/NormalSizeImages");
                    if (dirInfo.Exists)
                    {
                        var limit = 1;
                        foreach (FileInfo fileToUpload in dirInfo.GetFiles().OrderBy(f => f.CreationTime))
                        {
                            if (fileToUpload != null)
                            {
                                fileToUpload.WriteMultipartFormData(requestStream, boundary, "image/jpeg", "file");
                            }

                            if (limit > 24) break; // Only allow to upload 24 images
                            limit++;
                        }
                    }
                    byte[] endBytes = System.Text.Encoding.UTF8.GetBytes("--" + boundary + "--");
                    requestStream.Write(endBytes, 0, endBytes.Length);
                    requestStream.Close();
                }
            }
            catch (Exception)
            {
                //Error with images
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    return response.Headers["Location"];
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetPreviewUrl(string locationUrl, string cryptedStepCheck)
        {
            var postData = String.Format("a=fin&cryptedStepCheck={0}&go=Done+with+Images", cryptedStepCheck);
            return ExecutePost(locationUrl, postData);
        }

        public string GetBillingUrl(string locationUrl, string cryptedStepCheck, int step = 1)
        {
            var postData = step == 1 ? String.Format("continue=y&cryptedStepCheck={0}&go=Continue", cryptedStepCheck) : String.Format("an=ccard&cryptedStepCheck={0}&go=Continue", cryptedStepCheck);
            return ExecutePost(locationUrl, postData);
        }

        public string Purchase(string paymentUrl, CreditCardInfo creditCard)
        {
            var postData = String.Format("cardNumber={0}&" +
                                         "cvNumber={1}&" +
                                         "expMonth={2}&" +
                                         "expYear={3}&" +
                                         "cardFirstName={4}&" +
                                         "cardLastName={5}&" +
                                         "cardAddress={6}&" +
                                         "cardCity={7}&" +
                                         "cardState={8}&" +
                                         "cardPostal={9}&" +
                                         "cardCountry={10}&" +
                                         "contactName={11}&" +
                                         "contactPhone={12}&" +
                                         "contactEmail={13}&" +
                                         "cryptedStepCheck={14}&finishForm=Purchase",
                                         creditCard.CardNumber, creditCard.VerificationNumber, creditCard.ExpirationMonth, creditCard.ExpirationYear,
                                         HttpUtility.UrlEncode(creditCard.FirstName), HttpUtility.UrlEncode(creditCard.LastName), HttpUtility.UrlEncode(creditCard.Address),
                                         HttpUtility.UrlEncode(creditCard.City), creditCard.State, creditCard.Postal, "US", HttpUtility.UrlEncode(creditCard.ContactName),
                                         creditCard.ContactPhone, HttpUtility.UrlEncode(creditCard.ContactEmail), string.Empty);

            return ExecutePost(paymentUrl, postData);
        }
        #endregion

        #region Download HTML Content
        public List<SubLocationChoosing> GetSubLocationList(string subLocationChoosingUrl)
        {
            var list = new List<SubLocationChoosing>();
            var xmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent(subLocationChoosingUrl, CookieContainer, CookieCollection));
            var locationNodes = xmlDocument.SelectNodes("//form/blockquote/label");
            if (locationNodes != null)
            {
                foreach (XmlNode node in locationNodes)
                {
                    var location = new SubLocationChoosing();
                    location.Value = Convert.ToInt32(WebHandler.GetString(node, "./input/@value", null, null, true));
                    location.Name = node.InnerText.Trim();

                    list.Add(location);
                }
            }

            return list;
        }

        public List<StateChoosing> GetStateList()
        {
            var list = new List<StateChoosing>();
            var xmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent("http://www.craigslist.org/about/sites"));
            for (int i = 1; i <= 4; i++)
            {
                var state01Nodes = xmlDocument.SelectNodes(string.Format("//div[@class='colmask'][1]/div[@class='box box_{0}']/h4", i));
                if (state01Nodes != null)
                {
                    var index = 1;
                    foreach (XmlNode node in state01Nodes)
                    {
                        var state = new StateChoosing();
                        state.Name = node.InnerText.Trim();
                        state.Value = _commonManagementForm.AddNewState(state.Name);
                        var location01Nodes = xmlDocument.SelectNodes(string.Format("//div[@class='colmask'][1]/div[@class='box box_1']/ul[{0}]/li", index));
                        if (location01Nodes != null)
                        {
                            var locations = new List<LocationChoosing>();
                            foreach (XmlNode subnode in location01Nodes)
                            {
                                var location = new LocationChoosing();
                                location.Value = WebHandler.GetString(subnode, "./a/@href", null, null, true);
                                location.Name = UppercaseFirst(WebHandler.GetString(subnode, "./a", null, null, true));

                                locations.Add(location);
                                _commonManagementForm.AddNewCity(state.Value, location.Name, location.Value);
                            }
                            state.Locations = locations;
                        }

                        list.Add(state);
                        index++;
                    }
                }   
            }            

            return list;
        }

        public List<StateChoosing> GetStateListForCLDMS()
        {
            var list = new List<StateChoosing>();
            var xmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent("http://www.craigslist.org/about/sites"));
            for (int i = 1; i <= 4; i++)
            {
                var state01Nodes = xmlDocument.SelectNodes(string.Format("//div[@class='colmask'][1]/div[@class='box box_{0}']/h4", i));
                if (state01Nodes != null)
                {
                    var index = 1;
                    foreach (XmlNode node in state01Nodes)
                    {
                        var state = new StateChoosing();
                        state.Name = node.InnerText.Trim();
                        //state.Value = _commonManagementForm.AddNewState(state.Name);
                        var location01Nodes = xmlDocument.SelectNodes(string.Format("//div[@class='colmask'][1]/div[@class='box box_1']/ul[{0}]/li", index));
                        if (location01Nodes != null)
                        {
                            var locations = new List<LocationChoosing>();
                            foreach (XmlNode subnode in location01Nodes)
                            {
                                var location = new LocationChoosing() { SubLocations = new List<SubLocationChoosing>() };
                                location.Value = WebHandler.GetString(subnode, "./a/@href", null, null, true);
                                location.Name = UppercaseFirst(WebHandler.GetString(subnode, "./a/@title", null, null, true));                                
                                
                                var subxmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent(location.Value));
                                var sublocation01Nodes = subxmlDocument.SelectNodes(string.Format("//ul[@class='sublinks']/li"));
                                if (sublocation01Nodes.Count > 0)
                                {
                                    foreach (XmlNode item in sublocation01Nodes)
                                    {
                                        var sublocation = new SubLocationChoosing();
                                        sublocation.Href = WebHandler.GetString(item, "./a/@href", null, null, true);
                                        sublocation.Name = UppercaseFirst(WebHandler.GetString(item, "./a", null, null, true));

                                        var cldms = new CLDMSEntities();
                                        cldms.Cities.AddObject(new vincontrol.Data.Model.CLDMS.City()
                                        {
                                            CityName = location.Name + " - " + sublocation.Name,
                                            CraigsListCityURL = location.Value + sublocation.Href,
                                            State = state.Name
                                        });
                                        cldms.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var cldms = new CLDMSEntities();
                                    cldms.Cities.AddObject(new vincontrol.Data.Model.CLDMS.City()
                                    {
                                        CityName = location.Name,
                                        CraigsListCityURL = location.Value,
                                        State = state.Name
                                    });
                                    cldms.SaveChanges();
                                }
                                
                                locations.Add(location);
                            }
                            state.Locations = locations;                            
                        }

                        list.Add(state);
                        index++;
                    }
                }
            }

            return list;
        }

        public AdsPosting PreviewPosting(string previewUrl, DealerViewModel dealer, CarShortViewModel car)
        {
            var post = new AdsPosting()
            {
                Location = dealer.CraigslistSetting.City,
                SubLocation = dealer.CraigslistSetting.Location,
                Type = "for sale / wanted",
                Category = "cars & trucks - by dealer",
                SpecificLocation = dealer.CraigslistSetting.SpecificLocation,
                Title = String.Format("{0} {1} {2} {3}", car.ModelYear, car.Make, car.Model, car.Trim),
                SalePrice = car.Price,
                Vin = car.Vin,
                Year = car.ModelYear,
                Make = car.Make,
                Model = car.Model,
                Odometer = car.Odometer,
                Transmission = String.Format("{0} Transmission", car.Tranmission),
                Body = GeneratePostingBody(dealer, car),
                Note = "do NOT contact me with unsolicited"
            };

            var xmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent(previewUrl, CookieContainer, CookieCollection));
            var imageNodes = xmlDocument.SelectNodes("//div[@id='thumbs']/a");
            if (imageNodes != null)
            {
                var images = new List<string>();
                foreach (XmlNode node in imageNodes)
                {
                    images.Add(node.Attributes["href"].Value);
                }
                post.Images = images;
            }

            return post;
        }

        public ConfirmationPayment GetConfirmationPaymentInfo(string confirmationPaymentUrl)
        {
            var content = WebHandler.DownloadContent(confirmationPaymentUrl, CookieContainer, CookieCollection);
            //var xmlDocument = WebHandler.DownloadDocument(content);
            try
            {
                var regex = new Regex("Payment ID(.*)<br>");
                var paymentId = Convert.ToInt64(regex.Match(content).Value.Replace("<br>", "").Replace("Payment ID", ""));
                regex = new Regex("PostingID(.*)<i>");
                //content = xmlDocument.SelectSingleNode("//table[@id='postingInvoice']/tr[3]/td[2]").InnerXml;
                var postingId = Convert.ToInt64(regex.Match(content).Value.Replace("PostingID", "").Replace(":", "").Replace("<i>", ""));
                return new ConfirmationPayment() { PaymentId = paymentId, PostingId = postingId };
            }
            catch (Exception)
            {
                return new ConfirmationPayment();
            }
        }

        public string GetLocationPostUrl(string locationUrl)
        {
            var xmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent(locationUrl));
            return xmlDocument.SelectSingleNode("//ul[@id='postlks']/li/a/@href").Value;
        }
        #endregion
        
        #region Private Methods
        private string ExecutePost(string url, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Host = "post.craigslist.org";
            request.Method = "POST";
            request.UserAgent = UserAgent;
            request.ContentLength = postData.Length;
            request.Headers.Add("Accept-Language: en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = ContentType;
            request.Accept = Accept;
            request.Referer = url;
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            try
            {
                var streamWriter = new StreamWriter(request.GetRequestStream());
                streamWriter.Write(postData);
                streamWriter.Close();

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    //CookieCollection = response.Cookies;

                    //var streamReader = new StreamReader(response.GetResponseStream());
                    //var result = streamReader.ReadToEnd();
                    //streamReader.Close();

                    return response.Headers["Location"];
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string ExecuteGet(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Host = "post.craigslist.org";
            request.KeepAlive = true;
            request.Headers.Add(AcceptLanguage);
            request.Headers.Add(AcceptEncoding);
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.AllowAutoRedirect = false;
            request.Referer = url;
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                //CookieCollection = response.Cookies;

                //var streamReader = new StreamReader(response.GetResponseStream());
                //var result = streamReader.ReadToEnd();
                //streamReader.Close();
                return response.Headers["Location"];
            }
        }

        private string GetCryptedStepCheckFromUrl(string pageUrl)
        {
            var content = WebHandler.DownloadContent(pageUrl, CookieContainer, CookieCollection);
            var cryptedStepCheck = new Regex(CryptedStepCheckPattern);
            return cryptedStepCheck.Match(content).Groups[1].Value;
        }

        private string GetCryptedStepCheckFromContent(string content)
        {
            var cryptedStepCheck = new Regex(CryptedStepCheckPattern);
            return cryptedStepCheck.Match(content).Groups[1].Value;
        }

        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        #endregion
    }

    public class ConfirmationPayment
    {
        public long PaymentId { get; set; }
        public long PostingId { get; set; }
    }

    public class CreditCardInfo
    {
        public string CardNumber { get; set; }
        public string VerificationNumber { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string CryptedStepCheck { get; set; }
        public string LocationUrl { get; set; }
        public int ListingId { get; set; }
    }

    public class PostingPreview 
    {
        public AdsPosting Post { get; set; }
        public string CryptedStepCheck { get; set; }
        public string LocationUrl { get; set; }
        public string Warning { get; set; }
    }

    public class AdsPosting
    {
        public string Location { get; set; } //los angeles
        public string SubLocation { get; set; } //long beach / 562
        public string Type { get; set; } //for sale / wanted
        public string Category { get; set; } //cars & trucks - by dealer
        public string SpecificLocation { get; set; }
        public string Title { get; set; }
        public decimal SalePrice { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public long Odometer { get; set; }
        public string Transmission { get; set; }
        public string Body { get; set; }
        public string Note { get; set; } //do NOT contact me with unsolicited services or offers
        public List<string> Images { get; set; }
    }

    public class StateChoosing
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public List<LocationChoosing> Locations { get; set; }
    }

    public class LocationChoosing
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public List<SubLocationChoosing> SubLocations { get; set; }
    }

    public class SubLocationChoosing
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }

    }

    public static class DictionaryExtensions
    {
        /// <summary>
        /// Template for a multipart/form-data item.
        /// </summary>
        public const string FormDataTemplate = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n";

        /// <summary>
        /// Writes a dictionary to a stream as a multipart/form-data set.
        /// </summary>
        /// <param name="dictionary">The dictionary of form values to write to the stream.</param>
        /// <param name="stream">The stream to which the form data should be written.</param>
        /// <param name="mimeBoundary">The MIME multipart form boundary string.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="stream" /> or <paramref name="mimeBoundary" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if <paramref name="mimeBoundary" /> is empty.
        /// </exception>
        /// <remarks>
        /// If <paramref name="dictionary" /> is <see langword="null" /> or empty,
        /// nothing wil be written to the stream.
        /// </remarks>
        public static void WriteMultipartFormData(this Dictionary<string, string> dictionary, Stream stream, string mimeBoundary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (mimeBoundary == null)
            {
                throw new ArgumentNullException("mimeBoundary");
            }
            if (mimeBoundary.Length == 0)
            {
                throw new ArgumentException("MIME boundary may not be empty.", "mimeBoundary");
            }
            foreach (string key in dictionary.Keys)
            {
                string item = String.Format(FormDataTemplate, mimeBoundary, key, dictionary[key]);
                byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(item);
                stream.Write(itemBytes, 0, itemBytes.Length);
            }
        }
    }

    public static class FileInfoExtensions
    {
        /// <summary>
        /// Template for a file item in multipart/form-data format.
        /// </summary>
        public const string HeaderTemplate = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";

        /// <summary>
        /// Writes a file to a stream in multipart/form-data format.
        /// </summary>
        /// <param name="file">The file that should be written.</param>
        /// <param name="stream">The stream to which the file should be written.</param>
        /// <param name="mimeBoundary">The MIME multipart form boundary string.</param>
        /// <param name="mimeType">The MIME type of the file.</param>
        /// <param name="formKey">The name of the form parameter corresponding to the file upload.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if any parameter is <see langword="null" />.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if <paramref name="mimeBoundary" />, <paramref name="mimeType" />,
        /// or <paramref name="formKey" /> is empty.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Thrown if <paramref name="file" /> does not exist.
        /// </exception>
        public static void WriteMultipartFormData(this FileInfo file, Stream stream, string mimeBoundary, string mimeType, string formKey)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (!file.Exists)
            {
                throw new FileNotFoundException("Unable to find file to write to stream.", file.FullName);
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (mimeBoundary == null)
            {
                throw new ArgumentNullException("mimeBoundary");
            }
            if (mimeBoundary.Length == 0)
            {
                throw new ArgumentException("MIME boundary may not be empty.", "mimeBoundary");
            }
            if (mimeType == null)
            {
                throw new ArgumentNullException("mimeType");
            }
            if (mimeType.Length == 0)
            {
                throw new ArgumentException("MIME type may not be empty.", "mimeType");
            }
            if (formKey == null)
            {
                throw new ArgumentNullException("formKey");
            }
            if (formKey.Length == 0)
            {
                throw new ArgumentException("Form key may not be empty.", "formKey");
            }
            string header = String.Format(HeaderTemplate, mimeBoundary, formKey, file.Name, mimeType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            stream.Write(headerbytes, 0, headerbytes.Length);
            using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
            }
            byte[] newlineBytes = Encoding.UTF8.GetBytes("\r\n");
            stream.Write(newlineBytes, 0, newlineBytes.Length);
        }
    }
}
