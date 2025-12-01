using Microsoft.Extensions.DependencyInjection;

namespace FocusCycle.ViewModels.RLViewModels
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddTransient<StartWViewModel>()
            .AddTransient<TimerViewModel>()
            .AddTransient<TopmostTimerViewModel>()
            ;
    }
}
