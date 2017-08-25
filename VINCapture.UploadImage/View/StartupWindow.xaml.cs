using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VINCapture.UploadImage.View
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private FrameworkElement _title;
        public Button _close;
        private string Name;
        private static StartupWindow _sForm = null;


        public StartupWindow(string name,bool portable)
        {
            InitializeComponent();
            Loaded += StartupWindowLoaded;
            _mainFrame.Navigate(new StartupPage(name, portable));
            Name = name;

        }


        void StartupWindowLoaded(object sender, RoutedEventArgs e)
        {
            _close = (Button)Template.FindName("PART_Close", this);
            if (null != _close)
            {
                _close.Click += CloseClick;
            }

            _title = (FrameworkElement)Template.FindName("PART_Title", this);
            if (null != _title)
            {
                _title.MouseLeftButtonDown += TitleMouseLeftButtonDown;
            }
        }

        void CloseClick(object sender, RoutedEventArgs e)
        {
            //if (((App)Application.Current).FormList != null)
            //{
            //    if (((App) Application.Current).FormList.ContainsKey(Name))
            //    {
            //        ((App) Application.Current).FormList.Remove(Name);
            //    }
            //}
            //Close();
            var localProcess = Process.GetCurrentProcess();
            localProcess.Kill();
        }

        private void TitleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
