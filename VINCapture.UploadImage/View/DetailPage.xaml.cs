using System.ComponentModel;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VINCapture.UploadImage.Commands;
using System.Windows.Forms;
using VINCapture.UploadImage.Interface;
using VINCapture.UploadImage.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace VINCapture.UploadImage
{
    /// <summary>
    /// Interaction logic for DetailPage.xaml
    /// </summary>
    public partial class DetailPage : Page, IView
    {
        public DetailPage(string path)
        {
            Path = path;
            InitializeComponent();
            
        
            Loaded += new RoutedEventHandler(DetailPage_Loaded);
        }

        void DetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            //var workerThread = new BackgroundWorker();
            //workerThread.DoWork += new DoWorkEventHandler(workerThread_DoWork);
            //workerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerThread_RunWorkerCompleted);
            //workerThread.RunWorkerAsync();
            new DetailPageViewModel(this);
        }

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

      
     


        public string Path { get; set; }
        public int DealerId { get; set; }

      
    }
}
