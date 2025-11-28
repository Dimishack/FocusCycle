using FocusCycle.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FocusCycle.Views.Windows
{
    internal static class WindowsRegistrator
    {
        public static IServiceCollection AddWindows(this IServiceCollection services) => services
            .AddTransient(services =>
            {
                var viewModel = App.Services.GetRequiredService<StartWViewModel>();
                var window = new StartWindow() { DataContext = viewModel };

                InitializeCenterMainScreen(window);
                return window;
            })
            .AddTransient(services =>
            {
                var window = new SettingsWindow()
                {
                    Owner = App.AcivedWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                return window;
            })
            .AddTransient(services =>
            {
                var viewModel = App.Services.GetRequiredService<TimerViewModel>();
                var window = new TimerWindow() { DataContext = viewModel };
                InitializeCenterMainScreen(window);
                return window;
            });

        private static void InitializeCenterMainScreen(Window window)
        {
            window.Left = (SystemParameters.WorkArea.Width - window.Width) / 2;
            window.Top = (SystemParameters.WorkArea.Height - window.Height) / 2;
        }
    }
}
