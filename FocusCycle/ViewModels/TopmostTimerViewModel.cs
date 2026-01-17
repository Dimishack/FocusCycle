using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using System.Windows.Input;
using System.Windows.Threading;

namespace FocusCycle.ViewModels
{
    class TopmostTimerViewModel : ViewModel
    {
        private readonly DispatcherTimer _flickerTimer;

        #region Properties...

        #region CycleTimer : ICycleTimer - таймер циклов

        ///<summary>таймер циклов</summary>
        private readonly ICycleTimer _cycleTimer;

        ///<summary>таймер циклов</summary>
        public IReadOnlyCycleTimer CycleTimer => _cycleTimer;

        #endregion

        #region TimerOpacity : double - Видимость таймера

        ///<summary>Видимость таймера</summary>
        private double _timerOpacity;

        ///<summary>Видимость таймера</summary>
        public double TimerOpacity
        {
            get => _timerOpacity;
            set => Set(ref _timerOpacity, value);
        }

        #endregion

        #endregion

        #region Commands...

        #region CloseWindowCommand - Команда - закрыть окно

        ///<summary>Команда - закрыть окно</summary>
        private ICommand? _closeWindowCommand;

        ///<summary>Команда - закрыть окно</summary>
        public ICommand CloseWindowCommand => _closeWindowCommand
            ??= new LambdaCommand(OnCloseWindowCommandExecuted);

        ///<summary>Логика выполнения - закрыть окно</summary>
        private void OnCloseWindowCommandExecuted(object? p)
        {
            _cycleTimer.TimerActionChanged -= TimerActionChanged;
            _flickerTimer.Tick -= FlickerTimer_Tick;
            App.CloseConnectedWindow(this);
        }

        #endregion

        #endregion

        #region Events...

        private void TimerActionChanged(object? sender, TimerAction e)
        {
            switch (e)
            {
                case TimerAction.Start:
                case TimerAction.Resume:
                case TimerAction.Restart:
                    _flickerTimer.Stop();
                    TimerOpacity = 1.0;
                    break;
                case TimerAction.End:
                case TimerAction.Pause:
                    _flickerTimer.Start();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public TopmostTimerViewModel(ICycleTimer cycleTimer)
        {
            _cycleTimer = cycleTimer;
            _cycleTimer.TimerActionChanged += TimerActionChanged;
            _timerOpacity = 1.0;
            _flickerTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _flickerTimer.Tick += FlickerTimer_Tick;
            if (cycleTimer.IsPause)
                _flickerTimer.Start();
        }

        private void FlickerTimer_Tick(object? sender, EventArgs e)
            => TimerOpacity = 1.0 - _timerOpacity;
    }
}
