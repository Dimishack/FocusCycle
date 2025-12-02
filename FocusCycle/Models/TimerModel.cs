using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FocusCycle.Models
{
    class TimerModel : INotifyPropertyChanged
    {
        private readonly System.Timers.Timer _timer;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<TimerAction>? TimerActionChanged;

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool IsTimerPause => !_timer.Enabled;

        public bool IsCurrentWorkTimer
        {
            get;
            private set;
        }

        public string CurrentTimerString => IsCurrentWorkTimer
            ? "Р а б о т а"
            : "П е р е р ы в";

        public TimeSpan Work { get; set; }
        public TimeSpan Break { get; set; }

        public byte Hour
        {
            get;
            private set;
        }

        public byte Minute
        {
            get;
            private set;
        }

        public byte Second
        {
            get;
            private set;
        }

        public string TimerString => $"{Hour:00}:{Minute:00}:{Second:00}";

        #region PlayNextTimer : void - Запуск следующего таймера

        ///<summary>Запуск следующего таймера</summary>
        public void PlayNextTimer()
        {
            _timer.Stop();
            TimeSpan time = Work;
            if (IsCurrentWorkTimer)
                time = Break;
            Hour = (byte)time.Hours;
            Minute = (byte)time.Minutes;
            Second = (byte)time.Seconds;
            IsCurrentWorkTimer = !IsCurrentWorkTimer;
            OnPropertyChanged(nameof(CurrentTimerString));
            OnPropertyChanged(nameof(TimerString));
            _timer.Start();
            TimerActionChanged?.Invoke(this, TimerAction.Start);
        }

        #endregion

        #region PauseTimer : void - Остановить таймер

        ///<summary>Остановить таймер</summary>
        public void PauseTimer()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                TimerActionChanged?.Invoke(this, TimerAction.Stop);
            }
        }

        #endregion

        #region ResumeTimer : void - Восстановить таймер

        ///<summary>Восстановить таймер</summary>
        public void ResumeTimer()
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
                TimerActionChanged?.Invoke(this, TimerAction.Resume);
            }
        }

        #endregion

        public TimerModel()
        {
            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
            IsCurrentWorkTimer = false;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (--Second == byte.MaxValue)
            {
                byte second = 59;
                if (--Minute == byte.MaxValue)
                {
                    byte minute = 59;
                    if (--Hour == byte.MaxValue)
                    {
                        _timer.Stop();
                        second = 0;
                        minute = 0;
                        Hour = 0;
                        TimerActionChanged?.Invoke(this, TimerAction.End);
                    }
                    Minute = minute;
                }
                Second = second;
            }

            OnPropertyChanged(nameof(TimerString));
        }
    }
}
