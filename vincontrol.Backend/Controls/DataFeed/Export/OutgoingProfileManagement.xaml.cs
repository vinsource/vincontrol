using System.Windows;
using System.Windows.Controls;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Controls.DataFeed.Export
{
    /// <summary>
    /// Interaction logic for OutgoingProfileManagement.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class OutgoingProfileManagement : UserControl, IView
    // ReSharper restore RedundantExtendsListEntry
    {
        public OutgoingProfileManagement()
        {
            InitializeComponent();
            Loaded += delegate
            {
                // ReSharper disable ObjectCreationAsStatement
                new ExportProfileManagementViewModel(this);
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
