using FocusCycle.Models;
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
        private Window? _topmostTimerWindow;

        public bool IsOpenTopmostTimerWindow => _topmostTimerWindow is not null;

        public void OpenSettingsWindow()
        {
            if (CheckOpenWindow(_settingsWindow)) return;
            var window = App.Services.GetRequiredService<SettingsWindow>();
            window.Closed += (_, _) => _settingsWindow = null;
            _settingsWindow = window;
            _settingsWindow.Show();
        }

        public void OpenTimerWindow()
        {
            if (CheckOpenWindow(_timerWindow)) return;
            var window = App.Services.GetRequiredService<TimerWindow>();
            window.Closed += (_, _) => _timerWindow = null;
            _timerWindow = window;
            _timerWindow.Show();
        }

        public void OpenTopmostTimerWindow()
        {
            if(CheckOpenWindow(_topmostTimerWindow)) return;
            var window = App.Services.GetRequiredService<TopmostTimerWindow>();
            window.Closed += (_, _) => _topmostTimerWindow = null;
            _topmostTimerWindow = window;
            _topmostTimerWindow.Show();
        }

        private bool CheckOpenWindow(Window? window)
        {
            window?.Show();
            return window is not null;
        }
    }
}
