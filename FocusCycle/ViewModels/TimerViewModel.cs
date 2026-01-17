using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace FocusCycle.ViewModels
{
    internal class TimerViewModel(IOpenWindows openWindows, ITimerSettings settings, ICycleTimer cycleTimer) : ViewModel
    {
        private readonly string _settingsPath = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty,
            "settings.bin");
        private MediaModel _media = new();

        private readonly IOpenWindows _openWindows = openWindows;
        private readonly ITimerSettings _settings = settings;

        #region Properties...

        #region CycleTimer : ICycleTimer - Таймер циклов

        ///<summary>Таймер циклов</summary>
        private readonly ICycleTimer _cycleTimer = cycleTimer;

        ///<summary>Таймер циклов</summary>
        public IReadOnlyCycleTimer CycleTimer => _cycleTimer;

        #endregion

        #region ActionTimerString : string - Действие с таймером

        ///<summary>Действие с таймером</summary>
        public string? ActionTimerString => _cycleTimer.IsPause
            ? "Запустить таймер"
            : "Остановить таймер";

        #endregion

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

        #region IsChangeWindowVisibility : bool - Изменить видимость окна

        ///<summary>Изменить видимость окна</summary>
        private bool _isChangeWindowVisibility;

        ///<summary>Изменить видимость окна</summary>
        public bool IsChangeWindowVisibility
        {
            get => _isChangeWindowVisibility;
            set => Set(ref _isChangeWindowVisibility, value);
        }

        #endregion

        #endregion

        #region Commands...

        #region LoadedCommand - Команда - загрузка

        ///<summary>Команда - загрузка</summary>
        private ICommand? _loadedCommand;

        ///<summary>Команда - загрузка</summary>
        public ICommand LoadedCommand => _loadedCommand
            ??= new LambdaCommand(OnLoadedCommandExecuted);

        ///<summary>Логика выполнения - загрузка</summary>
        private void OnLoadedCommandExecuted(object? p)
        {
            _media.Volume = _settings.VolumeNotify;

            _settings.SettingsChanged += SettingsChanged;
            _cycleTimer.ChangeTimers(_settings.WorkTime, _settings.BreakTime);
            _cycleTimer.Restart();
            _cycleTimer.TimerActionChanged += TimerActionChanged;

            OnPropertyChanged(nameof(ActionTimerString));
        }


        #endregion

        #region ChangeWindowVisibilityCommand - Команда - изменить видимость окна

        ///<summary>Команда - изменить видимость окна</summary>
        private ICommand? _changeWindowVisibilityCommand;

        ///<summary>Команда - изменить видимость окна</summary>
        public ICommand ChangeWindowVisibilityCommand => _changeWindowVisibilityCommand
            ??= new LambdaCommand(OnChangeWindowVisibilityCommandExecuted);

        ///<summary>Логика выполнения - изменить видимость окна</summary>
        private void OnChangeWindowVisibilityCommandExecuted(object? p) => IsChangeWindowVisibility = true;

        #endregion

        #region OpenWindowCommand - Команда - открыть окно

        ///<summary>Команда - открыть окно</summary>
        private ICommand? _openWindowCommand;

        ///<summary>Команда - открыть окно</summary>
        public ICommand OpenWindowCommand => _openWindowCommand
            ??= new LambdaCommand(OnOpenWindowCommandExecuted);

        ///<summary>Логика выполнения - открыть окно</summary>
        private void OnOpenWindowCommandExecuted(object? p) => _openWindows.OpenTimerWindow();

        #endregion

        #region OpenTopmostTimerWindowCommand - Команда - открыть окно с передним таймером

        ///<summary>Команда - открыть окно с передним таймером</summary>
        private ICommand? _openTopmostTimerWindowCommand;

        ///<summary>Команда - открыть окно с передним таймером</summary>
        public ICommand OpenTopmostTimerWindowCommand => _openTopmostTimerWindowCommand
            ??= new LambdaCommand(OnOpenTopmostTimerWindowCommandExecuted);

        ///<summary>Логика выполнения - открыть окно с передним таймером</summary>
        private void OnOpenTopmostTimerWindowCommandExecuted(object? p) => _openWindows.OpenTopmostTimerWindow();

        #endregion

        #region OpenSettingsCommand - Команда - открыть настройки

        ///<summary>Команда - открыть настройки</summary>
        private ICommand? _openSettingsCommand;

        ///<summary>Команда - открыть настройки</summary>
        public ICommand OpenSettingsCommand => _openSettingsCommand
            ??= new LambdaCommand(OnOpenSettingsCommandExecuted);

        ///<summary>Логика выполнения - открыть настройки</summary>
        private void OnOpenSettingsCommandExecuted(object? p) => _openWindows.OpenSettingsDialog();

        #endregion

        #region PlayNextCycleCommand - Команда - запустить следующий цикл

        ///<summary>Команда - запустить следующий цикл</summary>
        private ICommand? _playNextCycleCommand;

        ///<summary>Команда - запустить следующий цикл</summary>
        public ICommand PlayNextCycleCommand => _playNextCycleCommand
            ??= new LambdaCommand(OnPlayNextCycleCommandExecuted);

        ///<summary>Логика выполнения - запустить следующий цикл</summary>
        private void OnPlayNextCycleCommandExecuted(object? p)
        {
            _cycleTimer.PlayNext();
            IsTimerPause = false;
            OnPropertyChanged(nameof(ActionTimerString));
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
            if (_cycleTimer.IsPause)
                _cycleTimer.Resume();
            else _cycleTimer.Pause();
            IsTimerPause = _cycleTimer.IsPause;
            OnPropertyChanged(nameof(ActionTimerString));
        }

        #endregion

        #region RestartTimerCommand - Команда - перезапустить таймер

        ///<summary>Команда - перезапустить таймер</summary>
        private ICommand? _restartTimerCommand;

        ///<summary>Команда - перезапустить таймер</summary>
        public ICommand RestartTimerCommand => _restartTimerCommand
            ??= new LambdaCommand(OnRestartTimerCommandExecuted);

        ///<summary>Логика выполнения - перезапустить таймер</summary>
        private void OnRestartTimerCommandExecuted(object? p) => _cycleTimer.Restart();

        #endregion

        #region ShutdownWindowAsyncCommand - Команда - выключить окно

        ///<summary>Команда - выключить окно</summary>
        private ICommand? _shutdownWindowAsyncCommand;

        ///<summary>Команда - выключить окно</summary>
        public ICommand ShutdownWindowAsyncCommand => _shutdownWindowAsyncCommand
            ??= new LambdaCommand(OnShutdownWindowCommandExecuted);

        ///<summary>Логика выполнения - выключить окно</summary>
        private void OnShutdownWindowCommandExecuted(object? p)
        {
            _cycleTimer.TimerActionChanged -= TimerActionChanged;
            _settings.SettingsChanged -= SettingsChanged;
            _cycleTimer.Dispose();
            _media.Dispose();
            _settings.Dispose();
            App.Current.Shutdown();
        }

        #endregion

        #endregion

        #region Methods...

        #region ShowMessageQuestion : MessageBoxResult - Отобразить сообщение-вопрос

        ///<summary>Отобразить сообщение-вопрос</summary>
        private MessageBoxResult ShowMessageQuestion(string question)
            => MessageBox.Show(question, "FocusCycle", MessageBoxButton.YesNo, MessageBoxImage.Question);

        #endregion

        #endregion

        #region Events...

        private void SettingsChanged(object? sender, EventArgs e)
        {
            if (ShowMessageQuestion("Применить изменения на текущие настройки " +
                "\r\n(если нет, то вступят в силу после перезагрузки программы)") == MessageBoxResult.Yes)
            {
                _cycleTimer.ChangeTimers(_settings.WorkTime, _settings.BreakTime);
                _media.Volume = _settings.VolumeNotify;
            }
        }

        private void TimerActionChanged(object? sender, TimerAction e)
        {
            switch (e)
            {
                case TimerAction.Start:
                case TimerAction.Restart:
                    _media.Stop();
                    break;
                case TimerAction.End:
                    _media.Play();
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}
