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
        private readonly IMessageBus _messageBus;
        private readonly IDisposable _getCurrentTimerSubscription;
        private readonly DispatcherTimer _flickerTimer;

        #region Properties...

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

        #region CurrentTimer : TimerModel? - Текущий таймер

        ///<summary>Текущий таймер</summary>
        private TimerModel? _currentTimer;

        ///<summary>Текущий таймер</summary>
        public TimerModel? CurrentTimer
        {
            get => _currentTimer;
            set => Set(ref _currentTimer, value);
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
            if (_currentTimer is not null)
                _currentTimer.TimerActionChanged -= TimerActionChanged;
            _getCurrentTimerSubscription.Dispose();
            _flickerTimer.Tick -= FlickerTimer_Tick;
            App.CloseConnectedWindow(this);
        }

        #endregion

        #endregion

        #region Subscriptions...

        #region GetCurrentTimer : void - Получить текущий таймер

        ///<summary>Получить текущий таймер</summary>
        private void GetCurrentTimer(TimerModel timerModel)
        {
            CurrentTimer = timerModel;
            if (CurrentTimer.IsTimerPause)
                _flickerTimer.Start();
            CurrentTimer.TimerActionChanged += TimerActionChanged;
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
                    _flickerTimer.Stop();
                    TimerOpacity = 1.0;
                    break;
                case TimerAction.End:
                case TimerAction.Stop:
                    _flickerTimer.Start();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public TopmostTimerViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _getCurrentTimerSubscription = messageBus
                .RegisterHandler<TimerModel>(GetCurrentTimer);
            _timerOpacity = 1.0;
            _flickerTimer = new DispatcherTimer();
            _flickerTimer.Interval = TimeSpan.FromSeconds(1);
            _flickerTimer.Tick += FlickerTimer_Tick;
        }

        private void FlickerTimer_Tick(object? sender, EventArgs e) 
            => TimerOpacity = 1.0 - _timerOpacity;
    }
}
