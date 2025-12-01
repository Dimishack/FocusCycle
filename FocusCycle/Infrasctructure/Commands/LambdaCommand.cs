namespace FocusCycle.Infrasctructure.Commands
{
    internal class LambdaCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : Base.Command
    {
        private Action<object?> _execute = execute;
        private Func<object?, bool>? _canExecute = canExecute;

        protected override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        protected override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
                _execute(parameter);
            OnRaiseCanExecuted();
        }
    }

    internal class LambdaCommand<T>(Action<T> execute, Func<T, bool>? canExecute = null) : Base.Command
    {
        private Action<T> _execute = execute;
        private Func<T, bool>? _canExecute = canExecute;

        protected override bool CanExecute(object? parameter)
            => parameter is T val && (_canExecute?.Invoke(val) ?? true);

        protected override void Execute(object? parameter)
        {
            if (CanExecute(parameter) && parameter is T val)
                _execute(val);
            OnRaiseCanExecuted();
        }
    }
}
