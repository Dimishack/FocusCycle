namespace FocusCycle.Services.Interfaces
{
    interface IOpenWindows
    {
        bool IsOpenSettingWindow { get; }
        bool IsOpenTopmostTimerWindow { get; }

        void OpenSettingsWindow();
        void OpenTimerWindow();
        void OpenTopmostTimerWindow();
    }
}
