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

        public void OpenTimerWindow()
        {
            if (CheckOpenWindow(_timerWindow)) return;
            var window = App.Services.GetRequiredService<TimerWindow>();
            window.Closed += (_, _) => _timerWindow = null;
            _timerWindow = window;
            ShowWindow(_timerWindow);
        }

        public void OpenTopmostTimerWindow()
        {
            if (CheckOpenWindow(_topmostTimerWindow)) return;
            var window = App.Services.GetRequiredService<TopmostTimerWindow>();
            window.Closed += (_, _) => _topmostTimerWindow = null;
            _topmostTimerWindow = window;
            ShowWindow(_topmostTimerWindow);
        }

        private static bool CheckOpenWindow(Window? window)
        {
            if (window != null) ShowWindow(window);
            return window != null;
        }

        private static void ShowWindow(Window window)
        {
            if (!window.IsVisible)
                window.Show();
            window.WindowState = WindowState.Normal;
            window.Activate();
        }

        public void OpenSettingsDialog()
        {
            var dialog = App.Services.GetRequiredService<SettingsDialog>();
            dialog.ShowDialog();
        }
    }
}
