namespace FocusCycle.Services.Interfaces
{
    interface IOpenWindows
    {
        bool IsOpenSettingWindow { get; }
        void OpenSettingsWindow();

        void OpenTimerWindow();
    }
}
