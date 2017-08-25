using System;
using System.Windows.Input;

namespace vincontrol.Backend.Commands
{
    public abstract class CommandBase : ICommand
    {
        readonly Predicate<object> _canExecute;
#pragma warning disable 169
        readonly Action<object> _execute;
#pragma warning restore 169

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="canExecute">The execution status logic.</param>
        protected CommandBase(Predicate<object> canExecute)
        {
            _canExecute = canExecute;           
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            Do(parameter);
            if (CommandComplete != null)
                CommandComplete.Invoke(this, new EventArgs());
        }

        public abstract void Do(object parameter);


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        public event EventHandler CommandComplete;

    }
}
