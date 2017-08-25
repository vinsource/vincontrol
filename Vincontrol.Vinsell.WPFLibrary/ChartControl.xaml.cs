using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using vincontrol.DomainObject;

namespace Vincontrol.Vinsell.WPFLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChartControl : UserControl
    {
        public ChartControl(List<List<VinsellChartVehicle>> initChartData, double minMileage, double maxMileage, double minPrice, double maxPrice)
        {
            InitializeComponent();
            var item = initChartData[initChartData.Count - 1][0];
            CurrentVehicle.DataContext = new
                {
                    Title = string.Format("{0} {1} {2} {3}", item.Year, item.Make, item.Model, item.Trim), item.Mileage, item.Price
                };

            lineChart.DataContext = initChartData;
            var yAxis = lineChart.Axes.OfType<LinearAxis>().FirstOrDefault(ax => ax.Orientation == AxisOrientation.Y);
            var xAxis = lineChart.Axes.OfType<LinearAxis>().FirstOrDefault(ax => ax.Orientation == AxisOrientation.X);
            if (yAxis != null)
            {
                yAxis.Maximum = maxPrice>item.Price?maxPrice:item.Price;
                yAxis.Minimum = minPrice<item.Price?minPrice:item.Price;
            }

            if (xAxis != null)
            {
                xAxis.Maximum = maxMileage>item.Mileage?maxMileage:item.Mileage;
                xAxis.Minimum = minMileage<item.Mileage?minMileage:item.Mileage;
            }
        }

        private void DataPointSeries_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            var item = (VinsellChartVehicle)e.AddedItems[0];
            VehicleDetail.DataContext = new
                {
                    item.VehicleAutoId, item.Year, item.Make, item.Model, item.Trim, item.Distance, item.Seller, item.SellerAddress, item.Mileage, item.Price, item.AutoTrader, item.CarsCom, item.Franchise, item.Indepedent,item.ThumbnailURL,
                    Title = string.Format("{0} {1} {2} {3}", item.Year, item.Make, item.Model, item.Trim)
                };

            
        }
    }
}
