using System.Collections.Generic;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.ViewModels
{
    public class ExpressionListViewModel 
    {
        #region Members

        public string XMLField { get; set; }

        private List<Column> _dbFields;
        public List<Column> DBFields
        {
            get
            {
                return _dbFields ?? (_dbFields = DataHelper.GetInventoryColumnNames());
            }
        }

        public List<KeyValuePair<string, string>> OperatorList
        {
            get
            {
                return _operatorList ?? (_operatorList = new List<KeyValuePair<string, string>>
                           {
                               new KeyValuePair<string, string>(Operators.And, Operators.And),
                               new KeyValuePair<string, string>(Operators.Devide, Operators.Devide), 
                               new KeyValuePair<string, string>(Operators.Multiply, Operators.Multiply),
                                   new KeyValuePair<string, string>(Operators.Plus, Operators.Plus)
                           });
            }
        }

        private RelayCommand _saveExpressionCommand;

        public RelayCommand SaveExpressionCommand
        {
            get
            {
                return _saveExpressionCommand ??
                  (_saveExpressionCommand = new RelayCommand(SaveExpressionList, CanExpressionConditionList));
            }
        }

        private bool CanExpressionConditionList()
        {
            return Expression.PropertiesValid;
        }

        private void SaveExpressionList(object parameter)
        {
            //if (ParentViewModel == null)
            //{
            //    ParentViewModel = new ProfileMappingViewModel();
            //}
            ParentViewModel.Expression = Expression;
            _view.Close();
        }

        public ProfileMappingViewModel ParentViewModel { get; set; }


        private IView _view;
        private List<KeyValuePair<string, string>> _operatorList;

        public ExpressionListViewModel()
        {
            Expression = new Expression();
        }

        public Expression Expression { get; set; }
        public Expression SelectedExpression { get; set; }
        #endregion

        public void ReBind(IView view)
        {
            _view = view;
            _view.SetDataContext(this);
        }
    }
}
