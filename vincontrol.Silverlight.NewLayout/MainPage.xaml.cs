using System.Windows;
using System.Windows.Controls;
using vincontrol.Silverlight.NewLayout.Interfaces;
using vincontrol.Silverlight.NewLayout.ViewModels;

namespace vincontrol.Silverlight.NewLayout
{
    public partial class MainPage : UserControl,IView
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            new UploadViewModel(this);
        }

        public void SetDataContext(object context)
        {
            DataContext = context;
        }
    }
}
