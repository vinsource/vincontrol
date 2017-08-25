using System.Windows;
using System.Windows.Controls;
using VINCONTROL.Silverlight.Interfaces;
using VINCONTROL.Silverlight.ViewModels;

namespace VINCONTROL.Silverlight
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
