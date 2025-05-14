using Microsoft.Extensions.DependencyInjection;

namespace FocusCycle.ViewModels.RLViewModels
{
    internal class ViewModelsLocator
    {
        public static StartWViewModel StartWViewModel => App.Services.GetRequiredService<StartWViewModel>();
    }
}
