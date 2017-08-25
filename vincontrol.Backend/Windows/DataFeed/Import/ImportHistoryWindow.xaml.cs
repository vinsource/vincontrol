using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.Windows.DataFeed.Import
{
    /// <summary>
    /// Interaction logic for ImportHistoryWindow.xaml
    /// </summary>
    public partial class ImportHistoryWindow : Window, IView
    {
        public ImportHistoryWindow(int profileId)
        {
            InitializeComponent();
            Loaded += delegate
            {
                // ReSharper disable ObjectCreationAsStatement
                new ImportDataHistoryViewModel(this, profileId);
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
