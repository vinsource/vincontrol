using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vincontrol.Data.Model;
using vincontrol.Helper;

namespace Vincontrol.Vinsell.DesktopVersion.View
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
            //frame1.Navigate(new Login());
        }

        public enum RoleType
        {
            Admin, Manager, Employee, Master
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            var dealerUser = UserHelper.CheckUserExistWithStatus(txtUsername.Text, txtPassword.Password);
            if (dealerUser == null)
            {
                //Please change pass
                MessageBox.Show("Incorrect user name or password");
            }
            else
            {
                ((App)Application.Current).Dealer = dealerUser;
                var manheimWindow = new ManheimWindow();
                manheimWindow.Show(); // works
                manheimWindow.Focus();
                //var drive = new DriveInfo(_path);
                //if (drive.IsReady)
                //{
                //    if (chkRemember.IsChecked ?? false)
                //    {
                //        Properties.Settings.Default.UserName = txtUsername.Text;
                //        Properties.Settings.Default.Password = txtPassword.Password;
                //    }
                //    var navigationService = NavigationService;
                //    if (navigationService != null) navigationService.Navigate(new DetailPage(_path));
                //}
                //else
                //{
                //    MessageBox.Show("Please enable storage in order to continue. ", "Error", MessageBoxButton.OK);
                //}
            }
        }
    }
}
