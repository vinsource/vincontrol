using System.Windows;
using vincontrol.Backend.Data.Repository.Implementation;
using vincontrol.Backend.Data.Repository.Interface;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Windows.DataFeed
{
    /// <summary>
    /// Interaction logic for AddDealerGroupWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class AddDealerGroupWindow : Window
// ReSharper restore RedundantExtendsListEntry
    {
        public AddDealerGroupWindow()
        {
            InitializeComponent();
        }

        private void btnCreateDealerGroup_Click(object sender, RoutedEventArgs e)
        {
            IDealerGroupRepository dealerGroupRepository = new DealerGroupRepository();
            dealerGroupRepository.AddDealerGroup(new DealerGroup {DealerGroupId = txtID.Text, DealerGroupName = txtName.Text,MasterLogin = txtMasterlogin.Text, MasterUserName = txtUserName.Text});
        }
    }
}
