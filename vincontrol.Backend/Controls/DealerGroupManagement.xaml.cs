using System.Windows.Controls;

namespace vincontrol.Backend.Controls
{
    /// <summary>
    /// Interaction logic for DealerGroupManagement.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class DealerGroupManagement : UserControl
// ReSharper restore RedundantExtendsListEntry
    {
        public DealerGroupManagement()
        {
            InitializeComponent();
            //var sqlUnitOfWork = new SqlUnitOfWork();
            //dtgDealerGroup.ItemsSource = sqlUnitOfWork.ImportProfile.GetAll();
            //IDealerGroupRepository dealerGroupRepository = new DealerGroupRepository();
            //dtgDealerGroup.ItemsSource = dealerGroupRepository.GetDealerGroups();
        }

        //private void btnCreateDealerGroup_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    //var chldWindow = new AddDealerGroupWindow();
        //    //chldWindow.ShowDialog();
        //}
    }
}
