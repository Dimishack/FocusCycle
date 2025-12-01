using System.Windows.Input;

namespace FocusCycle.Infrasctructure.Commands.Base
{
    internal abstract class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public void OnRaiseCanExecuted()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        private bool _executable = true;

        public bool Executable
        {
            get => _executable;
            set
            {
                if(Equals(value, _executable)) return;
                _executable = value;
            }
        }

        bool ICommand.CanExecute(object? parameter) => _executable && CanExecute(parameter);

        void ICommand.Execute(object? parameter)
        {
            if(CanExecute(parameter))
                Execute(parameter);
        }

        protected virtual bool CanExecute(object? parameter) => true;

        protected abstract void Execute(object? parameter);
    }
}
