using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed.Import
{
    /// <summary>
    /// Interaction logic for AddImportProfileWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class AddImportProfileWindow : Window,IEditView
// ReSharper restore RedundantExtendsListEntry

    {
        public AddImportProfileWindow(ProfileViewModel exportProfilevm, IncomingProfileManagementViewModel exportProfileManagementViewModel)
        {
            InitializeComponent();
            IsAdded = true;
            Loaded += delegate
                          {
// ReSharper disable ObjectCreationAsStatement
                              new IncomingProfileTemplateViewModel(this,exportProfilevm, exportProfileManagementViewModel);
// ReSharper restore ObjectCreationAsStatement
                          };
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        //public void Close()
        //{
        //    var parentwin = Window.GetWindow(this);
        //    if (parentwin != null) parentwin.Close();
        //}

        #endregion

        #region Implementation of IEditView

        public bool IsAdded { get; set; }
        public int Id { get; set; }

        #endregion
    }
}
