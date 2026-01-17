using FocusCycle.Models;

namespace FocusCycle.Services.Interfaces
{
    interface ICycleTimer: IDisposable, IReadOnlyCycleTimer
    {
        event EventHandler<TimerAction> TimerActionChanged;
        void ChangeTimers(TimeSpan timeWork, TimeSpan timeBreak);
        void Pause();
        void PlayNext();
        void Restart();
        void Resume();
    }
}
