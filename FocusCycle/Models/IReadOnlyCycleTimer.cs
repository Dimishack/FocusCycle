namespace FocusCycle.Models
{
    interface IReadOnlyCycleTimer
    {
        Cycle CurrentCycle { get; }
        bool IsPause { get; }
        string TimerString { get; }
    }
}
