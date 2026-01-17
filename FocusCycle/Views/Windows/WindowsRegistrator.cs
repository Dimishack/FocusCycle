using FocusCycle.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FocusCycle.Views.Windows
{
    internal static class WindowsRegistrator
    {
        public static IServiceCollection AddWindows(this IServiceCollection services) => services
            .AddSingleton(services =>
            {
                var viewModel = App.Services.GetRequiredService<TimerViewModel>();
                var window = new TimerWindow() { DataContext = viewModel};
                window.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    ((Window?)s)?.Hide();
                };
                InitializeCenterMainScreen(window);
                return window;
            })
            .AddTransient(services =>
            {
                var viewModel = App.Services.GetRequiredService<TopmostTimerViewModel>();
                var activedWindow = App.AcivedWindow;
                var window = new TopmostTimerWindow()
                {
                    DataContext = viewModel,
                    Top = activedWindow.Top + activedWindow.Width / 2,
                    Left = activedWindow.Left + activedWindow.Height / 2
                };

                return window;
            })
            .AddTransient(services =>
            {
                var viewModel = App.Services.GetRequiredService<SettingsViewModel>();
                var window = new SettingsDialog()
                {
                    DataContext = viewModel,
                    Owner = App.AcivedWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };
                return window;
            })
            ;

        private static void InitializeCenterMainScreen(Window window)
        {
            window.Left = (SystemParameters.WorkArea.Width - window.Width) / 2;
            window.Top = (SystemParameters.WorkArea.Height - window.Height) / 2;
        }
    }
}
