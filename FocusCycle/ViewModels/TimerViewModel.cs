using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Infrasctructure.Commands.Base;
using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using MessagePack;
using System.IO;
using System.Windows.Input;

namespace FocusCycle.ViewModels
{
    internal class TimerViewModel : ViewModel
    {
        private const string SETTINGS_FILENAME = "Settings.bin";
        private TimerSettings? _settings;

        private readonly IMessageBus _messageBus;
        private readonly IOpenWindows _openWindows;

        #region Properties...

        #region IsTimerPause : bool - Таймер на паузе

        ///<summary>Таймер на паузе</summary>
        private bool _isTimerPause;

        ///<summary>Таймер на паузе</summary>
        public bool IsTimerPause
        {
            get => _isTimerPause;
            set => Set(ref _isTimerPause, value);
        }

        #endregion

        #region CurrentTimer : TimerModel - Таймер

        ///<summary>Таймер</summary>
        private TimerModel _currentTimer;

        ///<summary>Таймер</summary>
        public TimerModel CurrentTimer
        {
            get => _currentTimer;
            set => Set(ref _currentTimer, value);
        }

        #endregion

        #endregion

        #region Commands...

        #region LoadedAsyncCommand - Команда - загрузка

        ///<summary>Команда - загрузка</summary>
        private ICommand? _loadedAsyncCommand;

        ///<summary>Команда - загрузка</summary>
        public ICommand LoadedAsyncCommand => _loadedAsyncCommand
            ??= new LambdaCommandAsync(OnLoadedCommandExecuted);

        ///<summary>Логика выполнения - загрузка</summary>
        private async Task OnLoadedCommandExecuted(object? p)
        {
            _settings = await ReadSettingsAsync();
            ((Command)PlayNextCycleCommand).OnRaiseCanExecuted();
            _currentTimer.Work = _settings.WorkTimer;
            _currentTimer.Break = _settings.BreakTimer;
            _currentTimer.PlayNextTimer();
        }

        #endregion

        #region ClosedCommand - Команда - закрытие

        ///<summary>Команда - закрытие</summary>
        private ICommand? _closedCommand;

        ///<summary>Команда - закрытие</summary>
        public ICommand ClosedCommand => _closedCommand
            ??= new LambdaCommandAsync(OnClosedCommandExecuted);

        ///<summary>Логика выполнения - закрытие</summary>
        private async Task OnClosedCommandExecuted(object? p)
        {
            if (_settings is not null)
                await WriteSettingsAsync(_settings);
            App.Current.Shutdown();
        }

        #endregion

        #region PlayNextCycleCommand - Команда - запустить следующий цикл

        ///<summary>Команда - запустить следующий цикл</summary>
        private ICommand? _playNextCycleCommand;

        ///<summary>Команда - запустить следующий цикл</summary>
        public ICommand PlayNextCycleCommand => _playNextCycleCommand
            ??= new LambdaCommand(OnPlayNextCycleCommandExecuted, CanPlayNextCycleCommandExecute);

        ///<summary>Проверка возможности выполнения - запустить следующий цикл</summary>
        private bool CanPlayNextCycleCommandExecute(object? p) => _settings is not null;

        ///<summary>Логика выполнения - запустить следующий цикл</summary>
        private void OnPlayNextCycleCommandExecuted(object? p)
        {
            _currentTimer.PlayNextTimer();
            IsTimerPause = false;
        }

        #endregion

        #region PlayPauseTimerCommand - Команда - запустить/остановить таймер

        ///<summary>Команда - запустить/остановить таймер</summary>
        private ICommand? _playPauseTimerCommand;

        ///<summary>Команда - запустить/остановить таймер</summary>
        public ICommand PlayPauseTimerCommand => _playPauseTimerCommand
            ??= new LambdaCommand(OnPlayPauseTimerCommandExecuted);

        ///<summary>Логика выполнения - запустить/остановить таймер</summary>
        private void OnPlayPauseTimerCommandExecuted(object? p)
        {
            if (_currentTimer.IsTimerPause)
                _currentTimer.ResumeTimer();
            else _currentTimer.PauseTimer();
        }

        #endregion

        #region OpenTopmostTimerWindowCommand - Команда - открыть окно с передним таймером

        ///<summary>Команда - открыть окно с передним таймером</summary>
        private ICommand? _openTopmostTimerWindowCommand;

        ///<summary>Команда - открыть окно с передним таймером</summary>
        public ICommand OpenTopmostTimerWindowCommand => _openTopmostTimerWindowCommand
            ??= new LambdaCommand(OnOpenTopmostTimerWindowCommandExecuted, CanOpenTopmostTimerWindowCommandExecute);

        ///<summary>Проверка возможности выполнения - открыть окно с передним таймером</summary>
        private bool CanOpenTopmostTimerWindowCommandExecute(object? p)
            //=> !_openWindows.IsOpenTopmostTimerWindow;
            => true;

        ///<summary>Логика выполнения - открыть окно с передним таймером</summary>
        private void OnOpenTopmostTimerWindowCommandExecuted(object? p)
        {
            _openWindows.OpenTopmostTimerWindow();
            _messageBus.Send(this, _currentTimer);
        }

        #endregion

        #endregion

        #region Methods...

        #region ReadSettingsAsync : async Task<TimerSettings> - Чтение файла с настройками

        ///<summary>Чтение файла с настройками</summary>
        private async Task<TimerSettings> ReadSettingsAsync()
        {
            var settings = new TimerSettings();
            try
            {
                using (var fs = new FileStream(SETTINGS_FILENAME, FileMode.Open))
                    settings = await MessagePackSerializer.DeserializeAsync<TimerSettings>(fs);
            }
            catch (Exception)
            {
            }
            return settings;
        }

        #endregion

        #region WriteSettingsAsync : async Task - Запись файла с настройками

        ///<summary>Запись файла с настройками</summary>
        private async Task WriteSettingsAsync(TimerSettings settings)
        {
            using (var fs = new FileStream(SETTINGS_FILENAME, FileMode.Create))
                await MessagePackSerializer.SerializeAsync(fs, settings);
        }

        #endregion

        #endregion

        public TimerViewModel(IMessageBus messageBus, IOpenWindows openWindows)
        {
            _messageBus = messageBus;
            _openWindows = openWindows;
            _currentTimer = new TimerModel();
        }

    }
}
