using FocusCycle.Models;

namespace FocusCycle.Services.Interfaces
{
    interface IOpenWindows
    {
        bool IsOpenTopmostTimerWindow { get; }

        void OpenSettingsWindow();
        void OpenTimerWindow();
        void OpenTopmostTimerWindow();
    }
}
