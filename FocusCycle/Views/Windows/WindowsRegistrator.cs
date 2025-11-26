using Microsoft.Extensions.DependencyInjection;

namespace FocusCycle.Views.Windows
{
    internal static class WindowsRegistrator
    {
        public static IServiceCollection AddWindows(this IServiceCollection services) => services
            .AddTransient(services =>
            {
                var window = new SettingsWindow();
                return window;
            })
            .AddTransient(services =>
            {
                var window = new TimerWindow();
                return window;
            });
    }
}
