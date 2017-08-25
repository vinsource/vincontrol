using System;
using System.Collections.ObjectModel;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using vincontrol.Backend.Model;
using vincontrol.Backend.Models;
using vincontrol.Backend.ViewModels.Import;

namespace vincontrol.Backend.ViewModels
{
    public class ReplaceListViewModel : ModelBase
    {
        #region Members

        public bool AllPropertiesValid
        {
            get
            {
                bool result = true;
                foreach (var replace in Replaces)
                {
                    if (!replace.PropertiesValid)
                    {
                        result = false;
                    }
                }
                return result;
            }
        }

        protected override string GetValidationError(string property)
        {
            throw new NotImplementedException();
        }

        public ProfileMappingViewModel ParentViewModel { get; set; }

        private RelayCommand _addReplaceExpressionCommand;

        public RelayCommand AddReplaceExpressionCommand
        {
            get
            {
                return _addReplaceExpressionCommand ??
                       (_addReplaceExpressionCommand = new RelayCommand(AddReplaceExpression,null));
            }
        }


        private RelayCommand _saveReplaceListCommand;

        public RelayCommand SaveReplaceListCommand
        {

            get
            {
                return _saveReplaceListCommand ??
                       (_saveReplaceListCommand = new RelayCommand(SaveReplaceList, CanSaveReplaceList));
            }
        }

        void AddReplaceExpression(object parameter)
        {
            Replaces.Add(new ReplaceModel());
        }


        private bool CanSaveReplaceList()
        {
            return AllPropertiesValid;
        }

        private void SaveReplaceList(object obj)
        {
            if (AllPropertiesValid)
            {
                //if (ParentViewModel == null)
                //{
                //    ParentViewModel = new ProfileMappingViewModel();
                //}
                ParentViewModel.Replaces = this;
            }
            _view.Close();
        }


        private RelayCommand _deleteReplaceExpressionCommand;

        public RelayCommand DeleteReplaceExpressionCommand
        {

            get
            {
                return _deleteReplaceExpressionCommand ??
                       (_deleteReplaceExpressionCommand = new RelayCommand(DeleteReplaceExpression,null));
            }
        }

        void DeleteReplaceExpression(object parameter)
        {
            Replaces.Remove((ReplaceModel)parameter);
        }

        private IView _view;
        public ObservableCollection<ReplaceModel> Replaces { get; set; }

        #endregion

        public ReplaceListViewModel()
        {
            Replaces = new ObservableCollection<ReplaceModel>();
        }

        public void ReBind(IView view)
        {
            _view = view;
            _view.SetDataContext(this);
        }
    }
}
