using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using System.ComponentModel;
using System.Timers;

namespace FocusCycle.Services
{
    class CycleTimerService : ICycleTimer, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region TimerActionChanged

        public event EventHandler<TimerAction>? TimerActionChanged;
        private void OnTimerActionChanged(TimerAction action)
        {
            void changed() => TimerActionChanged?.Invoke(this, action);
            if (App.Current.Dispatcher.CheckAccess())
                changed();
            else App.Current.Dispatcher.Invoke(changed);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _timer.Stop();
            _timer.Elapsed -= TimerElapsed;
            _timer.Dispose();
        }

        #endregion

        #region Fields...

        private readonly System.Timers.Timer _timer;
        private readonly TimeSpan[] _timers;
        private int _currentCycle;

        private byte _hour;
        private byte _minute;
        private byte _second;

        #endregion

        #region Properties...

        #region CurrentCycle : Cycle - Текущий цикл

        ///<summary>Текущий цикл</summary>
        public Cycle CurrentCycle => (Cycle)_currentCycle;

        #endregion

        #region IsPause : bool - Таймер на паузе?

        ///<summary>Таймер на паузе?</summary>
        public bool IsPause => !_timer.Enabled;

        #endregion

        #region TimerString : string - Таймер

        ///<summary>Таймер</summary>
        public string TimerString => string.Format("{0:00}:{1:00}:{2:00}", _hour, _minute, _second);

        #endregion

        #endregion

        #region Methods...

        #region ChangeTimers : void - Изменить таймеры

        ///<summary>Изменить таймеры</summary>
        public void ChangeTimers(TimeSpan timeWork, TimeSpan timeBreak)
        {
            if (timeWork != TimeSpan.MinValue)
                _timers[0] = timeWork;
            if (timeBreak != TimeSpan.MinValue)
                _timers[1] = timeBreak;
        }

        #endregion

        #region Pause : void - Пауза

        ///<summary>Пауза</summary>
        public void Pause()
        {
            _timer.Stop();
            OnPropertyChanged(nameof(IsPause));
            OnTimerActionChanged(TimerAction.Pause);
        }

        #endregion

        #region PlayNext : void - Запустить следующий цикл

        ///<summary>Запустить следующий цикл</summary>
        public void PlayNext()
        {
            _currentCycle = 1 - _currentCycle;
            RestartTimer();
            OnPropertyChanged(nameof(IsPause));
            OnPropertyChanged(nameof(TimerString));
            OnPropertyChanged(nameof(CurrentCycle));
            OnTimerActionChanged(TimerAction.Start);
        }

        #endregion

        #region Restart : void - Перезапустить

        ///<summary>Перезапустить</summary>
        public void Restart()
        {
            RestartTimer();
            OnPropertyChanged(nameof(IsPause));
            OnPropertyChanged(nameof(TimerString));
            OnTimerActionChanged(TimerAction.Restart);
        }

        #endregion

        #region RestartTimer : void - Перезапустить таймер

        ///<summary>Перезапустить таймер</summary>
        private void RestartTimer()
        {
            _timer.Stop();

            var timer = _timers[_currentCycle];
            _hour = (byte)timer.Hours;
            _minute = (byte)timer.Minutes;
            _second = (byte)timer.Seconds;

            _timer.Start();
        }

        #endregion

        #region Resume : void - Восстановить

        ///<summary>Восстановить</summary>
        public void Resume()
        {
            _timer.Start();
            OnPropertyChanged(nameof(IsPause));
            OnTimerActionChanged(TimerAction.Resume);
        }

        #endregion

        #endregion

        #region Events...

        private void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (--_second == byte.MaxValue)
            {
                byte second = 59;
                if (--_minute == byte.MaxValue)
                {
                    byte minute = 59;
                    if (--_hour == byte.MaxValue)
                    {
                        _timer.Stop();
                        second = 0;
                        minute = 0;
                        _hour = 0;
                        OnTimerActionChanged(TimerAction.End);
                    }
                    _minute = minute;
                }
                _second = second;
            }
            OnPropertyChanged(nameof(TimerString));
        }

        #endregion

        public CycleTimerService()
        {
            _timers = new TimeSpan[2];
            _timer = new(1000);
            _timer.Elapsed += TimerElapsed;
        }
    }
}
