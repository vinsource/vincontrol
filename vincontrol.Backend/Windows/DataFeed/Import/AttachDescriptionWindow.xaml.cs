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
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed.Import
{
    /// <summary>
    /// Interaction logic for AttachDescriptionWindow.xaml
    /// </summary>
    public partial class AttachDescriptionWindow : Window,IView
    {
        public AttachDescriptionWindow(ProfileViewModel parentvm)
        {
            InitializeComponent();
            Loaded += delegate
            {
                // ReSharper disable ObjectCreationAsStatement
                new AttachDescriptionViewModel(this, parentvm);
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
    }
}
