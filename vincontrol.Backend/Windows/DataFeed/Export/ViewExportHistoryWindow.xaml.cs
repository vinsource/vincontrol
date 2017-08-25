using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Windows.DataFeed.Export
{
    /// <summary>
    /// Interaction logic for ViewExportHistoryWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ViewExportHistoryWindow : Window, IView
// ReSharper restore RedundantExtendsListEntry
    {
        public ViewExportHistoryWindow(ExportProfileViewModel vm)
        {
            InitializeComponent();
            Loaded += delegate
            {
// ReSharper disable ObjectCreationAsStatement
                new ViewExportHistoryViewModel(this, vm);
// ReSharper restore ObjectCreationAsStatement
                //_vm.CreateProfileCompleted += VmCreateProfileCompleted;
            };
        }

        public void SetDataContext(object context)
        {
            DataContext = context;
        }
    }
}
