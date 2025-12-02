using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using System.Windows.Input;

namespace FocusCycle.ViewModels
{
    class TopmostTimerViewModel : ViewModel
    {
        private readonly IMessageBus _messageBus;
        private readonly IDisposable _getCurrentTimerSubscription;

        #region Properties...

        #region PlayFlicker : bool - Запустить мерцание

        ///<summary>Запустить мерцание</summary>
        private bool _playFlicker;

        ///<summary>Запустить мерцание</summary>
        public bool PlayFlicker
        {
            get => _playFlicker;
            set => Set(ref _playFlicker, value);
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
                PlayFlicker = true;
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
                    PlayFlicker = false;
                    break;
                case TimerAction.End:
                case TimerAction.Stop:
                    PlayFlicker = true;
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
        }
    }
}
