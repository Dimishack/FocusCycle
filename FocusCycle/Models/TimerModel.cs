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
        public event EventHandler? EndTimer;

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool IsTimerPause => !_timer.Enabled;

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

        public string TimeString => $"{Hour:00}:{Minute:00}:{Second:00}";

        public void NextTimer(byte hour, byte minute, byte second)
        {
            _timer.Stop();
            Hour = hour;
            Minute = minute;
            Second = second;
            OnPropertyChanged(nameof(TimeString));
            _timer.Start();
        }

        #region NextTimer : void - Запуск следующего таймера

        ///<summary>Запуск следующего таймера</summary>
        public void NextTimer(TimeSpan time)
        {
            _timer.Stop();
            Hour = (byte)time.Hours;
            Minute = (byte)time.Minutes;
            Second = (byte)time.Seconds;
            OnPropertyChanged(nameof(TimeString));
            _timer.Start();
        }

        #endregion

        #region PauseTimer : void - Остановить таймер

        ///<summary>Остановить таймер</summary>
        public void PauseTimer()
        {
            if (_timer.Enabled)
                _timer.Stop();
        }

        #endregion

        #region ResumeTimer : void - Восстановить таймер

        ///<summary>Восстановить таймер</summary>
        public void ResumeTimer()
        {
            if (!_timer.Enabled)
                _timer.Start();
        }

        #endregion

        public TimerModel()
        {
            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
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
                        EndTimer?.Invoke(this, EventArgs.Empty);
                    }
                    Minute = minute;
                }
                Second = second;
            }

            OnPropertyChanged(nameof(TimeString));
        }
    }
}
