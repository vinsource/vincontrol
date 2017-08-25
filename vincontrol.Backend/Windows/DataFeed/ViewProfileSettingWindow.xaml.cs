using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels;

namespace vincontrol.Backend.Windows.DataFeed
{
    /// <summary>
    /// Interaction logic for ViewProfileSettingWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ViewProfileSettingWindow : Window, IEditView
// ReSharper restore RedundantExtendsListEntry
    {
        public ViewProfileSettingWindow(int profileId)
        {
            InitializeComponent();
            Id = profileId;
            Loaded += delegate
            {
                Loaded += delegate
                {
// ReSharper disable ObjectCreationAsStatement
                    new ViewProfileSettingViewModel(this);
// ReSharper restore ObjectCreationAsStatement
                };
                //_vm = DuplicateView(parentvm);
                //_vm.SaveReplaceListCommandComplete += (sender, e) => Close();
                //_vm.ReBind(this);
            };
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion

        #region Implementation of IIdentityView

        public bool IsAdded { get; set; }
        public int Id { get; set; }

        #endregion
    }
}
