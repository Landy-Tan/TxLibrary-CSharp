/*
 * File:        TxDelegateCommand.cs
 * Author:      Landy
 * Date:        2021年4月14日
 * Description：使用委托的命令
 * History
 * <Date>           <Author>    <Description>
 * 2021年4月14日    Landy        创建
 */
using System;
using System.Windows.Input;

namespace TxLibrary.WPF
{
    /// <summary>
    /// 委托命令
    /// </summary>
    public class TxDelegateCommand : ICommand
    {
        #region Event

        private event EventHandler CanExecuteChangedInternal;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        #endregion

        #region Field

        private Action _execute1;
        private Action<object> _execute2;

        private Func<bool> _canExecute1;
        private Predicate<object> _canExecute2;

        #endregion

        #region Constructor Methods

        public TxDelegateCommand(Action execute)
            :this(execute, DefaultCanExecute)
        {
        }

        public TxDelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute1 = execute;
            _canExecute1 = canExecute;
        }

        public TxDelegateCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public TxDelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute2 = execute;
            _canExecute2 = canExecute;
        }

        #endregion

        #region Internal Methods

        private static bool DefaultCanExecute(object arg1) => true;
        private static bool DefaultCanExecute() => true;

        #endregion

        #region External Methods

        public bool CanExecute(object parameter)
        {
            if (_canExecute1 != null)
                return _canExecute1();
            else if (_canExecute2 != null)
                return _canExecute2(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if (_execute1 != null)
                _execute1();
            else if (_execute2 != null)
                _execute2(parameter);
        }

        #endregion
    }
}
