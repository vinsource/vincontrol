using System.Collections.Generic;
using System.Collections.ObjectModel;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.ViewModels.Import;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.ViewModels
{
    public class ConditionListViewModel
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

        public bool AllPropertiesValid
        {
            get
            {

                bool result = true;
                foreach (var replaceViewModel in Conditions)
                {
                    if (!replaceViewModel.PropertiesValid)
                    {
                        result = false;
                    }
                }
                return result;
            }
        }

        public ProfileMappingViewModel ParentViewModel { get; set; }
        private RelayCommand _addConditionExpressionCommand;

        public RelayCommand AddConditionExpressionCommand
        {
            get { return _addConditionExpressionCommand ?? (_addConditionExpressionCommand = new RelayCommand(AddRelay,null)); }
        }

        private void AddRelay(object parameter)
        {
            Conditions.Add(new ConditionModel());
        }

        private void DeleteCondition(object parameter)
        {
            Conditions.Remove((ConditionModel)parameter);
        }

        private RelayCommand _deleteConditionExpressionCommand;
        public RelayCommand DeleteConditionExpressionCommand
        {
            get { return _deleteConditionExpressionCommand ?? (_deleteConditionExpressionCommand = new RelayCommand(DeleteCondition,null)); }
        }

        private RelayCommand _saveConditionListCommand;

        public RelayCommand SaveConditionListCommand
        {

            get
            {
                return _saveConditionListCommand ??
                    (_saveConditionListCommand = new RelayCommand(SaveConditionList, CanSaveConditionList));
            }
        }

        private bool CanSaveConditionList()
        {
            return AllPropertiesValid;
        }

        private void SaveConditionList(object parameter)
        {
            if (AllPropertiesValid)
            {
                //if (ParentViewModel == null)
                //{
                //    ParentViewModel = new ProfileMappingViewModel();
                //}
                ParentViewModel.Conditions = this;

            }
            _view.Close();
        }

        private IView _view;
        public ConditionListViewModel()
        {
            Conditions = new ObservableCollection<ConditionModel>();
        }

        public ObservableCollection<ConditionModel> Conditions { get; set; }

        #endregion

        public void ReBind(IView view)
        {
            _view = view;
            _view.SetDataContext(this);
        }

    }
}
