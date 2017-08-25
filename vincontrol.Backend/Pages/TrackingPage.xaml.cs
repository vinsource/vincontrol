using System;
using System.Collections.Generic;
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
using vincontrol.Backend.Interface;

namespace vincontrol.Backend.Pages
{
    /// <summary>
    /// Interaction logic for TrackingPage.xaml
    /// </summary>
    public partial class TrackingPage : Page, IView
    {
        public TrackingPage()
        {
            InitializeComponent();
            Loaded += delegate
            {
                // ReSharper disable ObjectCreationAsStatement
                new TrackingPageViewModel(this);
                // ReSharper restore ObjectCreationAsStatement
                //_vm.CreateProfileCompleted += VmCreateProfileCompleted;
            };
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
