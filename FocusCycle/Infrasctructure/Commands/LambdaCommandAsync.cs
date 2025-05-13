namespace FocusCycle.Infrasctructure.Commands
{
    internal class LambdaCommandAsync(Func<object?, Task> executeAsync, Func<object?, bool>? canExecute = null) : Base.Command
    {
        private Func<object?, Task> _executeAsync = executeAsync;
        private Func<object?, bool>? _canExecute = canExecute;

        private volatile Task? _executingAction;

        protected override bool CanExecute(object? parameter) =>
            _canExecute?.Invoke(parameter) ?? true;

        protected override async void Execute(object? parameter)
        {
            Task task = _executeAsync(parameter);
            _ = Interlocked.Exchange(ref _executingAction, task);
            _executingAction = task;
            OnCanExecuteChanged();
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            OnCanExecuteChanged();
        }
    }

    internal class LambdaCommandAsync<T>(Func<T, Task> executeAsync, Func<T, bool>? canExecute = null) : Base.Command
    {
        private Func<T, Task> _executeAsync = executeAsync;
        private Func<T, bool>? _canExecute = canExecute;

        private volatile Task? _executingAction;

        protected override bool CanExecute(object? parameter)
            => parameter is T val && (_canExecute?.Invoke(val) ?? true);

        protected override async void Execute(object? parameter)
        {
            if (parameter is not T val) return;
            Task task = _executeAsync(val);
            _ = Interlocked.Exchange(ref _executingAction, task);
            _executingAction = task;
            OnCanExecuteChanged();
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            OnCanExecuteChanged();
        }
    }
}
