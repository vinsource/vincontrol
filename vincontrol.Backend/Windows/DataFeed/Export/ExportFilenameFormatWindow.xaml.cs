using System.Windows;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Model;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Windows.DataFeed.Export
{
    /// <summary>
    /// Interaction logic for ExportFilenameFormatWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ExportFilenameFormatWindow : Window, IView
// ReSharper restore RedundantExtendsListEntry
    {
        private ExportFilenameFormatViewModel _vm;

        public ExportFilenameFormatWindow(FileNameFormat fileNameFormat, ExportType exportType, int? dealerId, int? profileID)
        {
            InitializeComponent();
            Loaded += delegate
            {
// ReSharper disable ObjectCreationAsStatement
                 _vm = new ExportFilenameFormatViewModel(this, fileNameFormat, exportType, dealerId, profileID);
// ReSharper restore ObjectCreationAsStatement
               
            };
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }


        #endregion

        public bool IsDirty { get { return _vm.IsDirty; } }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
