using FocusCycle.Infrasctructure.Commands.Base;
using FocusCycle.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FocusCycle.Infrasctructure.Commands
{
    class OpenSettingsCommand : Command
    {
        private Window? _settingsWindow;

        protected override void Execute(object? parameter)
        {
            if(_settingsWindow is Window settingWindow)
            {
                _settingsWindow.Show();
                return;
            }
            var window = App.Services.GetRequiredService<SettingsWindow>();
            window.Closed += Window_Closed;
            _settingsWindow = window;
            window.Show();
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            if(_settingsWindow != null)
            {
                _settingsWindow.Closed -= Window_Closed;
                _settingsWindow = null;
            }
        }
    }
}
