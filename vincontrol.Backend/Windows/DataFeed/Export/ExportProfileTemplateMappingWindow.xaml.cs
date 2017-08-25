using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Windows.DataFeed.Export
{
    /// <summary>
    /// Interaction logic for ExportProfileTemplateMappingWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ExportProfileTemplateMappingWindow : Window, IEditView
// ReSharper restore RedundantExtendsListEntry
    {
       public bool IsAdded { get; set; }

        public ExportProfileTemplateMappingWindow(ExportProfileViewModel exportProfilevm, ExportProfileManagementViewModel exportProfileManagementViewModel)
        {
            InitializeComponent();
            //this.Id = exportProfilevm.Id;
            IsAdded = false;
            Loaded += delegate
                          {
// ReSharper disable ObjectCreationAsStatement
                              new ExportProfileTemplateViewModel(this, exportProfilevm, exportProfileManagementViewModel);
// ReSharper restore ObjectCreationAsStatement
                          };
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion

        #region Implementation of IIdentityView

        public int Id { get; set; }

        #endregion

      
    }
}
