using System;
using System.Windows.Input;

namespace БДНИЛ_Учёт__деятельности_лабаратории.addition
{
    public class Command : ICommand
    {
        Action _exec;
        Func<bool> _canExec;

        //Присоединение обработчика событий
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //Описание объекта
        public Command(Action exec, Func<bool> canExec = null) {
            _canExec = canExec;
            _exec = exec;
        }

        //Проверка может ли команда определятся в данный момент
        public bool CanExecute(object parameter)
        {
            if (_canExec == null) return true;
            return _canExec.Invoke();
        }

        //Метод вызываемый при вызове данной команды
        public void Execute(object parameter)
        {
            _exec.Invoke();
        }
    }
}
