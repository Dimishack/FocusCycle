using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Infrasctructure.Commands.Base;
using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace FocusCycle.ViewModels
{
    class SettingsViewModel : ViewModel
    {
        private const string REGISTRYNAME = "FocusCycle";
        private const string REGISTRYKEY_PATH = "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\";

        private readonly IMessageBus _messageBus;
        private readonly IDisposable _getSettingsSubscription;
        private TimerSettings? _edittingSettings;
        private bool _currentAutorun;

        #region Properties...

        #region IsAutorun : bool - Автозапуск

        ///<summary>Автозапуск</summary>
        private bool _isAutorun;

        ///<summary>Автозапуск</summary>
        public bool IsAutorun
        {
            get => _isAutorun;
            set
            {
                if (!Set(ref _isAutorun, value)) return;
                ((Command)EditSettingsCommand).OnRaiseCanExecuted();
            }
        }

        #endregion

        #region WorkTime : TimeSpan - Время работы

        ///<summary>Время работы</summary>
        private TimeSpan _workTime;

        ///<summary>Время работы</summary>
        public TimeSpan WorkTime
        {
            get => _workTime;
            set
            {
                if (!Set(ref _workTime, value)) return;
                ((Command)EditSettingsCommand).OnRaiseCanExecuted();
            }
        }

        #endregion

        #region BreakTime : TimeSpan - Время отдыха

        ///<summary>Время отдыха</summary>
        private TimeSpan _breakTime;

        ///<summary>Время отдыха</summary>
        public TimeSpan BreakTime
        {
            get => _breakTime;
            set
            {
                if (!Set(ref _breakTime, value)) return;
                ((Command)EditSettingsCommand).OnRaiseCanExecuted();
            }
        }

        #endregion

        #region Volume : double - Громкость

        ///<summary>Громкость</summary>
        private double _volume;

        ///<summary>Громкость</summary>
        public double Volume
        {
            get => _volume;
            set
            {
                if(!Set(ref _volume, value)) return;
                VolumeProcent = value * 100.0;
                ((Command)EditSettingsCommand).OnRaiseCanExecuted();
            }
        }

        #endregion

        #region VolumeProcent : double - Громкость в процентах

        ///<summary>Громкость в процентах</summary>
        private double _volumeProcent;

        ///<summary>Громкость в процентах</summary>
        public double VolumeProcent
        {
            get => _volumeProcent;
            set
            {
                if(!Set(ref _volumeProcent, value)) return;
                Volume = value / 100.0;
            }
        }

        #endregion

        #endregion

        #region Commands...

        #region EditSettingsCommand - Команда - изменить настройки

        ///<summary>Команда - изменить настройки</summary>
        private ICommand? _editSettingsCommand;

        ///<summary>Команда - изменить настройки</summary>
        public ICommand EditSettingsCommand => _editSettingsCommand
            ??= new LambdaCommand(OnEditSettingsCommandExecuted, CanEditSettingsCommandExecute);

        ///<summary>Проверка возможности выполнения - изменить настройки</summary>
        private bool CanEditSettingsCommandExecute(object? p)
            => _edittingSettings is not null
            && (_currentAutorun != _isAutorun
             || _edittingSettings.WorkTime != _workTime
             || _edittingSettings.BreakTime != _breakTime
             || _edittingSettings.Volume != _volume);

        ///<summary>Логика выполнения - изменить настройки</summary>
        private void OnEditSettingsCommandExecuted(object? p)
        {
            TimerSettings editiingSettings = new()
            {
                WorkTime = _workTime,
                BreakTime = _breakTime,
                Volume = _volume,
            };
            if (_currentAutorun != _isAutorun)
                UpdateRegistry(!_isAutorun);
            SendEdittingSettings(editiingSettings);
            CloseWindow();
        }

        #endregion

        #region CancelEditCommand - Команда - отменить изменения

        ///<summary>Команда - отменить изменения</summary>
        private ICommand? _cancelEditCommand;

        ///<summary>Команда - отменить изменения</summary>
        public ICommand CancelEditCommand => _cancelEditCommand
            ??= new LambdaCommand(OnCancelEditCommandExecuted);

        ///<summary>Логика выполнения - отменить изменения</summary>
        private void OnCancelEditCommandExecuted(object? p)
        {
            SendEdittingSettings(null);
            CloseWindow();
        }

        #endregion

        #endregion

        #region Methods...

        #region UpdateRegistry : void - Обновить реестр

        ///<summary>Обновить реестр</summary>
        private void UpdateRegistry(bool isDelete)
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey(REGISTRYKEY_PATH, true))
            {
                if (isDelete)
                    registryKey?.DeleteValue(REGISTRYNAME);
                else
                {
                    string? processPath = Environment.ProcessPath;
                    if (processPath != null)
                        registryKey?.SetValue(REGISTRYNAME, processPath);
                    else
                        MessageBox.Show("Не удалось добавить программу в автозапусе, " +
                            "так как не удается получить путь к программе");
                }
            }
        }

        #endregion

        #region CloseWindow : void - Закрыть окно

        ///<summary>Закрыть окно</summary>
        private void CloseWindow()
        {
            _edittingSettings = null;
            _getSettingsSubscription.Dispose();
            App.CloseConnectedWindow(this);
        }

        #endregion

        #region SendEdittingSettings : void - Отправить измененные настройки

        ///<summary>Отправить измененные настройки</summary>
        private void SendEdittingSettings(TimerSettings? settings)
            => _messageBus.Send(this, settings);

        #endregion

        #endregion

        #region Subscriptions...

        #region GetSettings : void - Получить настройки

        ///<summary>Получить настройки</summary>
        private void GetSettings(TimerSettings edittingSettings)
        {
            _edittingSettings = edittingSettings;
            using (var registryKey = Registry.CurrentUser.OpenSubKey(REGISTRYKEY_PATH))
                _currentAutorun = registryKey?.GetValue(REGISTRYNAME) is not null;
            IsAutorun = _currentAutorun;
            WorkTime = edittingSettings.WorkTime;
            BreakTime = edittingSettings.BreakTime;
            Volume = edittingSettings.Volume;
        }

        #endregion

        #endregion

        public SettingsViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _getSettingsSubscription = messageBus
                .RegisterHandler<TimerSettings>(GetSettings);
        }
    }
}
