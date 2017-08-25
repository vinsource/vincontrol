using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed
{
    /// <summary>
    /// Interaction logic for ViewDealerOfProfileWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ViewDealerOfProfileWindow : Window, IEditView
// ReSharper restore RedundantExtendsListEntry
    {
        public ViewDealerOfProfileWindow(ProfileViewModel profilevm)
        {
            InitializeComponent();
            Loaded += delegate
            {
// ReSharper disable ObjectCreationAsStatement
                new ViewDealerOfProfileViewModel(this, profilevm);
// ReSharper restore ObjectCreationAsStatement
                //_vm.CreateProfileCompleted += VmCreateProfileCompleted;
            };
        }

      

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
