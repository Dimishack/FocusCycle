using FocusCycle.Models;

namespace FocusCycle.Services.Interfaces
{
    interface ITimerSettings : IDisposable
    {
        event EventHandler? SettingsChanged;
        float VolumeNotify { get; }
        TimeSpan WorkTime { get; }
        TimeSpan BreakTime { get; }
        Task UpdateSettingsAsync(TimerSettings? newSettings);
        void UpdateSettings(TimerSettings? newSettings);
    }
}
