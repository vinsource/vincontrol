using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Windows.DataFeed.Export
{
    /// <summary>
    /// Interaction logic for AddProfileWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class AddExportProfileWindow : Window, IEditView
// ReSharper restore RedundantExtendsListEntry
    {
        public AddExportProfileWindow(ExportProfileViewModel exportProfilevm, ExportProfileManagementViewModel exportProfileManagementViewModel)
        {
            InitializeComponent();
            IsAdded = true;
            Loaded += delegate
                          {
// ReSharper disable ObjectCreationAsStatement
                              new ExportProfileTemplateViewModel(this,exportProfilevm, exportProfileManagementViewModel);
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
