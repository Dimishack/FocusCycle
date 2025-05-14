using FocusCycle.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FocusCycle.ViewModels.RLViewModels
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddTransient<StartWViewModel>()
            .AddTransient(services =>
            {
                var window = new SettingsWindow();
                return window;
            })
            .AddTransient(services =>
            {
                var window = new TimerWindow();
                return window;
            })
            ;
    }
}
