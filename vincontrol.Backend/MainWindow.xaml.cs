using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using vincontrol.Backend.Pages;


namespace vincontrol.Backend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var chldWindow = new AddDealerGroupWindow();
            chldWindow.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var chldWindow = new AddDealerWindow();
            chldWindow.ShowDialog();
        }

        public ObservableCollection<OutgoingMappingProfile> OutGoingProfileList
        {
            get;
            set;
        }

        private void dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var chldWindow = new ViewDealerGroupDatafeedWindow();
            chldWindow.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var chldWindow = new ViewDealerWindow();
            chldWindow.ShowDialog();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var chldWindow = new AddIncomingProfileWindow();
            chldWindow.ShowDialog();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           //ImportManagementPage importManagementPage = new ImportManagementPage();
           // importManagementPage.
        }

      
    }

    public class OutgoingMappingProfile //better to choose a appropriate name
    {
        public string CompanyName { get; set; }
    }


}
