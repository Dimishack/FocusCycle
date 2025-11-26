using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FocusCycle.ViewModels
{
    internal sealed class StartWViewModel(IOpenWindows openWindows) : ViewModel
    {
		private readonly IOpenWindows _openWindows = openWindows;
        private readonly DispatcherTimer _timer = new()
        {
            Interval = TimeSpan.FromSeconds(6)
        };

        #region Properties...

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
            _timer.Tick += Timer_Tick;
			_timer.Start();
		}

        private void Timer_Tick(object? sender, EventArgs e)
        {
			_timer.Stop();
			_timer.Tick -= Timer_Tick;
			_openWindows.OpenTimerWindow(); 
			Application.Current.MainWindow.Close();
        }

        #endregion

		#endregion
	}
}
