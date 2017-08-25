using System.Windows;
using System.Windows.Controls;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Controls.DataFeed.Import
{
    /// <summary>
    /// Interaction logic for IncomingProfileManagement.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class IncomingProfileManagement : UserControl, IView
// ReSharper restore RedundantExtendsListEntry
    {
        public IncomingProfileManagement()
        {
            InitializeComponent();
            Loaded += delegate
                          {
// ReSharper disable ObjectCreationAsStatement
                              new IncomingProfileManagementViewModel(this); 
// ReSharper restore ObjectCreationAsStatement
                          };
        }


        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        public void Close()
        {
            var parentwin = Window.GetWindow(this);
            if (parentwin != null) parentwin.Close();
        }

        #endregion
    }
}
