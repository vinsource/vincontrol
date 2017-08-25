using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed.Import.PopupChildWindow
{
    /// <summary>
    /// Interaction logic for ConditionWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ConditionWindow : Window, IView
// ReSharper restore RedundantExtendsListEntry
    {
        private ConditionListViewModel _vm;

        public ConditionWindow(ProfileMappingViewModel parentvm)
        {
            InitializeComponent();

            Loaded += delegate
            {
                _vm = DuplicateView(parentvm);
                _vm.ReBind(this);
            };
        }

        private ConditionListViewModel DuplicateView(ProfileMappingViewModel parentvm)
        {
            var newReplaceListViewModel = new ConditionListViewModel { ParentViewModel = parentvm };
            if (parentvm.Conditions != null)
            {
                newReplaceListViewModel.Conditions = new ObservableCollection<ConditionModel>(new List<ConditionModel>(parentvm.Conditions.Conditions));
            }

            return newReplaceListViewModel;
        }

        public ConditionWindow()
        {
        }

        #region Implementation of IView

        public void SetDataContext(object context)
        {
            DataContext = context;
        }

        #endregion
    }
}
