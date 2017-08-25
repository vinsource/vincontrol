using System.Collections.Generic;
using System.Windows;
using VINCapture.UploadImage.Interface;
using VINCapture.UploadImage.Models;
using VINCapture.UploadImage.ViewModels;

namespace VINCapture.UploadImage.View
{
    /// <summary>
    /// Interaction logic for EmailListWindow.xaml
    /// </summary>
    public partial class EmailListWindow : Window, IView
    {
        public List<EmailItem> EmailList { get; set; }
        public EmailListWindow(List<EmailItem> list)
        {
            EmailList = list;
            InitializeComponent();
            Loaded += EmailListWindowLoaded;
        }

        void EmailListWindowLoaded(object sender, RoutedEventArgs e)
        {
            new EmailListViewModel(this, EmailList, ((App)Application.Current).Dealer.DealerId);
        }
     

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        public string Path { get; set; }

        #endregion

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
