using System.Windows;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.Windows.DataFeed.Import.PopupChildWindow
{
    /// <summary>
    /// Interaction logic for ExpressionWindow.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class ExpressionWindow : Window, IView
// ReSharper restore RedundantExtendsListEntry
    {
        private ExpressionListViewModel _vm;

        public ExpressionWindow(ProfileMappingViewModel parentvm)
        {
            InitializeComponent();
            Loaded += delegate
            {
                _vm = DuplicateView(parentvm);
                //_vm.SaveConditionListCommandComplete += (sender, e) => Close();
                _vm.ReBind(this);
            };
        }

        private ExpressionListViewModel DuplicateView(ProfileMappingViewModel parentvm)
        {
            //var item = parentvm.Expression ?? new Expression {DBField1 = String.Empty, DBField2 = string.Empty, DBField3 = string.Empty};

            //    return new ExpressionListViewModel() { Expression = item,SelectedExpression = item};
            var newReplaceListViewModel = new ExpressionListViewModel { ParentViewModel = parentvm };
            if (parentvm.Expression != null)
            {
                newReplaceListViewModel.Expression =  parentvm.Expression ;
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
