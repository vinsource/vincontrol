using System;
using System.Collections.Generic;
using System.Windows;
using vincontrol.Backend.Data;
using vincontrol.Backend.Helper;
using vincontrol.Backend.Interface;
using System.Linq;
using vincontrol.Backend.Pages;

namespace vincontrol.Backend.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private RelayCommand _loginCommand;
        private INavigate _view;
        private string _password;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    base.OnPropertyChanged("UserName");
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    base.OnPropertyChanged("Password");
                }
            }
        }

        public RelayCommand LoginCommand { get { return _loginCommand ?? (_loginCommand = new RelayCommand(LoginAsync, null)); } }

        private void LoginAsync(object obj)
        {
            DoPendingTask(Login, obj);
        }

        private void Login(object obj)
        {
            var context = new vincontrolwarehouseEntities();
            var user = context.backendusers.FirstOrDefault(i => i.UserName.Equals(UserName) && i.Password.Equals(Password));
            if (user == null)
            {
                MessageBox.Show("Either username or password is wrong.");
            }
            else
            {
                App.CurrentUser = new User { Id = user.Id, Name = user.UserName, Roles = new List<string> { user.Role } };
                //PageSwitcher pageSwithcher
                Application.Current.Dispatcher.BeginInvoke(new Action(() => _view.Navigate(new Home())));
            }
        }

        public LoginViewModel(INavigate view)
        {
            _view = view;
            _view.SetDataContext(this);
        }

        #region Overrides of ModelBase

        protected override string GetValidationError(string property)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}