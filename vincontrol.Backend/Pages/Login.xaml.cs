using System.Threading;
using System.Windows.Controls;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class Login : Page, INavigate
// ReSharper restore RedundantExtendsListEntry
    {
        public Login()
        {
            InitializeComponent();
            Loaded += delegate
            {
                // ReSharper disable ObjectCreationAsStatement
                new LoginViewModel(this);
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
            throw new System.NotImplementedException();
        }

        public void Navigate(object item)
        {
            var navigationService = this.NavigationService;
            if (navigationService != null) navigationService.Navigate(item);
        }

        #endregion
    }
}
