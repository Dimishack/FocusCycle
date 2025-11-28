using FocusCycle.Infrasctructure.Commands.Base;
using System.Windows;

namespace FocusCycle.Infrasctructure.Commands
{
    internal class MinimizeWindowCommand : Command
    {
        protected override bool CanExecute(object? parameter) => parameter is Window;

        protected override void Execute(object? parameter)
        {
            if (parameter is null || !CanExecute(parameter)) return;
            ((Window)parameter).WindowState = WindowState.Minimized;
        }
    }
}
