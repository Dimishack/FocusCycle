using FocusCycle.Services.Interfaces;
using FocusCycle.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FocusCycle.Services
{
    class OpenWindowsService : IOpenWindows
    {
        private Window? _settingsWindow;
        private Window? _timerWindow;

        public bool IsOpenSettingWindow => _settingsWindow is not null;

        public void OpenSettingsWindow()
        {
            if(_settingsWindow is { } window)
            {
                window.Show();
                return;
            }
            window = App.Services.GetRequiredService<SettingsWindow>();
            window.Closed += (_, _) => _settingsWindow = null;
            _settingsWindow = window;
            _settingsWindow.Show();
        }

        public void OpenTimerWindow()
        {
            if (_timerWindow is { } window)
            {
                window.Show();
                return;
            }
            window = App.Services.GetRequiredService<TimerWindow>();
            window.Closed += (_, _) => _timerWindow = null;
            _timerWindow = window;
            _timerWindow.Show();
        }
    }
}
