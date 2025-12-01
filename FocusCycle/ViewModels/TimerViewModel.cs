using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FocusCycle.ViewModels
{
    internal class TimerViewModel : ViewModel
    {
        private const string SETTINGS_FILENAME = "Settings.bin";
        private TimerSettings? _settings;
        private readonly IOpenWindows _openWindows;

        #region Properties...

        #region TimerModel : TimerModel - Таймер

        ///<summary>Таймер</summary>
        private TimerModel _timerModel;

        ///<summary>Таймер</summary>
        public TimerModel TimerModel
        {
            get => _timerModel;
            set => Set(ref _timerModel, value);
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
            _timerModel.NextTimer(_settings.WorkTimer);
        }

        #endregion

        #region ClosedCommand - Команда - закрытие

        ///<summary>Команда - закрытие</summary>
        private ICommand? _closedCommand;

        ///<summary>Команда - закрытие</summary>
        public ICommand ClosedCommand => _closedCommand
            ??= new LambdaCommand(OnClosedCommandExecuted);

        ///<summary>Логика выполнения - закрытие</summary>
        private void OnClosedCommandExecuted(object? p) => App.Current.Shutdown();

        #endregion

        #region PlayNextTimerCommand - Команда - запустить следующий таймер

        ///<summary>Команда - запустить следующий таймер</summary>
        private ICommand? _playNextTimerCommand;

        ///<summary>Команда - запустить следующий таймер</summary>
        public ICommand PlayNextTimerCommand => _playNextTimerCommand
            ??= new LambdaCommand(OnPlayNextTimerCommandExecuted);

        ///<summary>Логика выполнения - запустить следующий таймер</summary>
        private void OnPlayNextTimerCommandExecuted(object? p)
        {

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
            if (_timerModel.IsTimerPause)
                _timerModel.ResumeTimer();
            else _timerModel.PauseTimer();
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
            => _openWindows.OpenTopmostTimerWindow();

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

        #endregion

        public TimerViewModel(IOpenWindows openWindows)
        {
            _openWindows = openWindows;
            _timerModel = new TimerModel();
        }

    }
}
