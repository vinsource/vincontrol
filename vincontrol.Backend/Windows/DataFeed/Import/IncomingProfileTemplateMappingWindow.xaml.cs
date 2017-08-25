using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed.Import
{
    /// <summary>
    /// Interaction logic for IncomingProfileTemplateMappingWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class IncomingProfileTemplateMappingWindow : Window, IEditView
// ReSharper restore RedundantExtendsListEntry
    {
        public bool IsAdded { get; set; }

        public IncomingProfileTemplateMappingWindow(ProfileViewModel currentViewModel)
        {
            InitializeComponent();
            //this.Id = Id;
            IsAdded = false;
            Loaded += delegate
                          {
// ReSharper disable ObjectCreationAsStatement
                              new IncomingProfileTemplateViewModel(this, currentViewModel,null);
// ReSharper restore ObjectCreationAsStatement
                          };
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion

        #region Implementation of IIdentityView

        public int Id { get; set; }

        #endregion
    }
}
