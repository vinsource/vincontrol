using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Models;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed.Import.PopupChildWindow
{
    /// <summary>
    /// Interaction logic for ReplaceWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ReplaceWindow : Window, IView
// ReSharper restore RedundantExtendsListEntry
    {
        private ReplaceListViewModel _vm;

        public ReplaceWindow()
        {
        }

        public ReplaceWindow(ProfileMappingViewModel parentvm)
        {
            InitializeComponent();
            Loaded += delegate
                          {
                              _vm = DuplicateView(parentvm);
                              _vm.ReBind(this);
                          };
        }

        private static ReplaceListViewModel DuplicateView(ProfileMappingViewModel parentvm)
        {
            var newReplaceListViewModel = new ReplaceListViewModel {ParentViewModel = parentvm};
            if (parentvm.Replaces != null)
            {
                newReplaceListViewModel.Replaces = new ObservableCollection<ReplaceModel>(new List<ReplaceModel>(parentvm.Replaces.Replaces));
            }

            return newReplaceListViewModel;
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion
    }
}
