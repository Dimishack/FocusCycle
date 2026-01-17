using FocusCycle.Models;

namespace FocusCycle.Services.Interfaces
{
    interface IOpenWindows
    {
        void OpenTimerWindow();
        void OpenTopmostTimerWindow();
        void OpenSettingsDialog();
    }
}
