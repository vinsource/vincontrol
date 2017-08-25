using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Export;

namespace vincontrol.Backend.Windows.DataFeed.Export
{
    /// <summary>
    /// Interaction logic for ViewExportDealerOfProfileWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class ViewExportDealerOfProfileWindow : Window, IEditNameView
    // ReSharper restore RedundantExtendsListEntry
    {
        public ViewExportDealerOfProfileWindow(ExportProfileViewModel vm)
        {
            InitializeComponent();
            Loaded += delegate
            {
                // ReSharper disable ObjectCreationAsStatement
                new ViewExportDealerOfProfileViewModel(this, vm);
                // ReSharper restore ObjectCreationAsStatement
                //_vm.CreateProfileCompleted += VmCreateProfileCompleted;
            };
        }
        public string ProfileName { get; set; }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion

        #region Implementation of IEditView

        public bool IsAdded { get; set; }
        public int Id { get; set; }

        #endregion




    }
}
