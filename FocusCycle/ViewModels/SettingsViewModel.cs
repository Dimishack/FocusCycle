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
        private readonly RegistryKey _registryKey;
        private readonly ITimerSettings _settings;

        private readonly bool _currentAutorun;

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
                ((Command)EditSettingsAsyncCommand).OnRaiseCanExecuted();
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
                ((Command)EditSettingsAsyncCommand).OnRaiseCanExecuted();
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
                ((Command)EditSettingsAsyncCommand).OnRaiseCanExecuted();
            }
        }

        #endregion

        #region Volume : float - Громкость

        ///<summary>Громкость</summary>
        private float _volume;

        ///<summary>Громкость</summary>
        public float Volume
        {
            get => _volume;
            set
            {
                if (!Set(ref _volume, value)) return;
                VolumeProcent = value * 100.0F;
                ((Command)EditSettingsAsyncCommand).OnRaiseCanExecuted();
            }
        }

        #endregion

        #region VolumeProcent : double - Громкость в процентах

        ///<summary>Громкость в процентах</summary>
        private float _volumeProcent;

        ///<summary>Громкость в процентах</summary>
        public float VolumeProcent
        {
            get => _volumeProcent;
            set
            {
                if (!Set(ref _volumeProcent, value)) return;
                Volume = value / 100.0F;
            }
        }

        #endregion

        #endregion

        #region Commands...

        #region EditSettingsAsyncCommand - Команда - изменить настройки

        ///<summary>Команда - изменить настройки</summary>
        private ICommand? _editSettingsAsyncCommand;

        ///<summary>Команда - изменить настройки</summary>
        public ICommand EditSettingsAsyncCommand => _editSettingsAsyncCommand
            ??= new LambdaCommandAsync(OnEditSettingsCommandExecuted, CanEditSettingsCommandExecute);

        ///<summary>Проверка возможности выполнения - изменить настройки</summary>
        private bool CanEditSettingsCommandExecute(object? p)
            => _currentAutorun != _isAutorun
             || _settings.WorkTime != _workTime
             || _settings.BreakTime != _breakTime
             || _settings.VolumeNotify != _volume;

        ///<summary>Логика выполнения - изменить настройки</summary>
        private async Task OnEditSettingsCommandExecuted(object? p)
        {
            TimerSettings editiingSettings = new()
            {
                WorkTime = _workTime,
                BreakTime = _breakTime,
                Volume = _volume,
            };
            UpdateRegistry(_isAutorun);
            await _settings.UpdateSettingsAsync(editiingSettings);
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
            _settings.UpdateSettings(null);
            CloseWindow();
        }

        #endregion

        #endregion

        #region Methods...

        #region UpdateRegistry : void - Обновить реестр

        ///<summary>Обновить реестр</summary>
        private bool UpdateRegistry(bool isAutorun)
        {
            if (isAutorun == _currentAutorun) return false;
            try
            {
                if (!isAutorun)
                    _registryKey.DeleteValue(REGISTRYNAME);
                else
                {
                    string? processPath = Environment.ProcessPath;
                    if (processPath is null)
                    {
                        MessageBox.Show("Не удалось добавить программу в автозапусе, " +
                            "так как не удается получить путь к программе");
                        return false;
                    }
                    _registryKey.SetValue(REGISTRYNAME, processPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        #endregion

        #region CloseWindow : void - Закрыть окно

        ///<summary>Закрыть окно</summary>
        private void CloseWindow()
        {
            _registryKey.Dispose();
            App.CloseConnectedWindow(this);
        }

        #endregion

        #endregion

        public SettingsViewModel(ITimerSettings settings)
        {
            _registryKey = Registry.CurrentUser.OpenSubKey(REGISTRYKEY_PATH, true)
                ?? throw new ArgumentException("Нет ключа");
            _isAutorun = _currentAutorun = _registryKey.GetValue(REGISTRYNAME) is not null;
            _settings = settings;
            _workTime = settings.WorkTime;
            _breakTime = settings.BreakTime;
            _volume = settings.VolumeNotify;
            _volumeProcent = _volume * 100.0F;
        }
    }
}
