using FocusCycle.Models;
using FocusCycle.Services.Interfaces;
using MessagePack;
using System.IO;

namespace FocusCycle.Services
{
    class TimerSettingsService : ITimerSettings
    {
        private readonly TimerSettings _settings;
        private readonly string _path = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty,
            "settings.bin");

        public event EventHandler? SettingsChanged;

        #region Properties...

        #region BreakTime : TimeSpan - Время перерыва

        ///<summary>Время перерыва</summary>
        public TimeSpan BreakTime => _settings.BreakTime;

        #endregion

        #region VolumeNotify : float - Громкость уведомления

        ///<summary>Громкость уведомления</summary>
        public float VolumeNotify => _settings.Volume;

        #endregion

        #region WorkTime : TimeSpan - Время работы

        ///<summary>Время работы</summary>
        public TimeSpan WorkTime => _settings.WorkTime;

        #endregion

        #endregion

        #region Methods...

        #region ReadSettings : TimerSettings? - Прочитать настройки

        ///<summary>Прочитать настройки</summary>
        private TimerSettings? ReadSettings()
        {
            try
            {
                using (FileStream fs = new(_path, FileMode.Open))
                    return MessagePackSerializer.Deserialize<TimerSettings>(fs);
            }
            catch (Exception) { return null; }
        }

        #endregion

        #region UpdateSettingsAsync : async Task<bool> - Обновить настройки

        ///<summary>Обновить настройки</summary>
        public async Task UpdateSettingsAsync(TimerSettings? newSettings)
        {
            if (Update(newSettings))
                using (FileStream fs = new(_path, FileMode.Create, FileAccess.Write))
                    await MessagePackSerializer.SerializeAsync(fs, newSettings);
        }

        #endregion

        #region UpdateSettings : bool - Обновить настройки

        ///<summary>Обновить настройки</summary>
        public void UpdateSettings(TimerSettings? newSettings)
        {
            if (Update(newSettings))
                using (FileStream fs = new(_path, FileMode.Create, FileAccess.Write))
                    MessagePackSerializer.Serialize(fs, newSettings);
        }

        #endregion

        public void Dispose() { }

        #region Update : bool - Обновить настройки

        ///<summary>Обновить настройки</summary>
        private bool Update(TimerSettings? newSettings)
        {
            if (newSettings is null
             || newSettings.WorkTime == _settings.WorkTime
             && newSettings.BreakTime == _settings.BreakTime
             && newSettings.Volume == _settings.Volume) return false;
            _settings.WorkTime = newSettings.WorkTime;
            _settings.BreakTime = newSettings.BreakTime;
            _settings.Volume = newSettings.Volume;

            SettingsChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        #endregion

        #endregion

        public TimerSettingsService()
        {
            var settings = ReadSettings();
            _settings = settings ?? new TimerSettings();
        }
    }
}
