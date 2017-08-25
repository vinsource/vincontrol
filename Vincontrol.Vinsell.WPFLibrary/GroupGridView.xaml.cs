using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;



namespace Vincontrol.Vinsell.WPFLibrary
{
    /// <summary>
    /// Interaction logic for GroupGridView.xaml
    /// </summary>
    public partial class GroupGridView : UserControl
    {
        //private readonly List<vincontrol.Application.ViewModels.ManheimAuctionManagement.VehicleViewModel> _upcomingVehicleList;
        //private readonly List<vincontrol.Application.ViewModels.ManheimAuctionManagement.VehicleViewModel> _pastVehicleList;

        //public GroupGridView(IList<vincontrol.Application.ViewModels.ManheimAuctionManagement.VehicleViewModel> data)
        //{
        //    if (data == null)
        //        data = new List<vincontrol.Application.ViewModels.ManheimAuctionManagement.VehicleViewModel>();
        //    InitializeComponent();
        //    btnUpcoming.Background = new SolidColorBrush(Color.FromRgb(22, 30, 138));
        //    btnPast.Background = new SolidColorBrush(Color.FromRgb(85, 85, 85));

        //    _upcomingVehicleList = data.Where(i => !i.IsSold && !i.IsPast).OrderBy(i => i.RegionName).ToList();
        //    _pastVehicleList = data.Where(i => i.IsSold || i.IsPast).OrderBy(i => i.RegionName).ToList();
        //    BindData(_upcomingVehicleList);
        //}

        private void BindData(IList data)
        {
            var collection = new ListCollectionView(data);
            if (collection.GroupDescriptions != null)
                collection.GroupDescriptions.Add(new PropertyGroupDescription("RegionName"));
            dgData.ItemsSource = collection;
            dgData.IsReadOnly = true;
         
        }

        private void btnUpcoming_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //BindData(_upcomingVehicleList);
            btnUpcoming.Background = new SolidColorBrush(Color.FromRgb(22, 30, 138));
            btnPast.Background = new SolidColorBrush(Color.FromRgb(85, 85, 85));
        }

        private void btnPast_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //BindData(_pastVehicleList);
            btnUpcoming.Background = new SolidColorBrush(Color.FromRgb(85, 85, 85));
            btnPast.Background = new SolidColorBrush(Color.FromRgb(22, 30, 138));

        }
    }
}
