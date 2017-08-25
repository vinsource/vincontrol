using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VINCapture.UploadImage.Models;
using vincontrol.Data.Model;

namespace VINCapture.UploadImage.View
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly string _path;
       
        public LoginPage(string path)
        {
            _path = path;
       

            InitializeComponent();
            if (chkRemember.IsChecked ?? false)
            {
                if (!String.IsNullOrEmpty(Properties.Settings.Default.UserName))
                {
                    txtUsername.Text = Properties.Settings.Default.UserName;
                }

                if (!String.IsNullOrEmpty(Properties.Settings.Default.Password))
                {
                    txtPassword.Password = Properties.Settings.Default.Password;
                }
            }


        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dealerUser = CheckUserExistWithStatus(txtUsername.Text, txtPassword.Password);
                if (dealerUser == null)
                {
                    //Please change pass
                    MessageBox.Show("Incorrect user name or password","Error", MessageBoxButton.OK,MessageBoxImage.Error);
                }
                else
                {
                    ((App)Application.Current).Dealer = dealerUser;
                    if (!String.IsNullOrEmpty(_path))
                    {

                        var drive = new DriveInfo(_path);
                        if (drive.IsReady)
                        {
                            if (chkRemember.IsChecked ?? false)
                            {
                                Properties.Settings.Default.UserName = txtUsername.Text;
                                Properties.Settings.Default.Password = txtPassword.Password;
                            }
                            var navigationService = NavigationService;
                            if (navigationService != null) navigationService.Navigate(new DetailPage(_path));
                        }
                        else
                        {
                            MessageBox.Show("Please enable storage in order to continue. ", "Error", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        if (chkRemember.IsChecked ?? false)
                        {
                            Properties.Settings.Default.UserName = txtUsername.Text;
                            Properties.Settings.Default.Password = txtPassword.Password;
                        }
                        var navigationService = NavigationService;
                        if (navigationService != null) navigationService.Navigate(new DetailPage(_path));
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + " " + ex.InnerException + " " + ex.Source + " " + ex.StackTrace;
                MessageBox.Show(errorMessage);
            }

            
        }

        public static DealerUser CheckUserExistWithStatus(string userName, string passWord)
        {
            var user = new DealerUser();
            using (var context = new VincontrolEntities())
            {
                //check if master login
                var firstOrDefault =
                    context.Users.FirstOrDefault(o => o.UserName == userName && o.Password == passWord && o.Active.Value);

                if (firstOrDefault != null)
                {

                    var firstDealer =
                        context.Dealers.FirstOrDefault(
                            x => x.DealerId == firstOrDefault.DefaultLogin);
                    
                    user.DealerId = firstOrDefault.DefaultLogin.GetValueOrDefault();
                    
                    user.Username = firstOrDefault.UserName;
                    
                    user.Name = firstOrDefault.Name;
                    
                    if (firstDealer != null)
                    {
                        user.DealerName = firstDealer.Name;

                        user.HeaderOverlayUrl = firstDealer.Setting.HeaderOverlayURL;

                        user.FooterOverlayUrl = firstDealer.Setting.FooterOverlayURL;
                    }

                }
                else
                {
                    return null;
                }
            }

            return user;
        }
    }

    public enum RoleType
    {
        Admin, Manager, Employee, Master
    }
}

