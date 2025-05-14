using FocusCycle.Infrasctructure.Commands;
using FocusCycle.Services.Interfaces;
using FocusCycle.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace FocusCycle.ViewModels
{
    class StartWViewModel(IOpenWindows openWindows) : ViewModel
    {
		private IOpenWindows _openWindows = openWindows;

		#region Properties...

		#endregion

		#region Commands...

		#region OpenTimerWindowCommand - Команда - открыть окно с таймером

		///<summary>Команда - открыть окно с таймером</summary>
		private ICommand? _openTimerWindowCommand;

		///<summary>Команда - открыть окно с таймером</summary>
		public ICommand OpenTimerWindowCommand => _openTimerWindowCommand
			??= new LambdaCommand(OnOpenTimerWindowCommandExecuted);

		///<summary>Логика выполнения - открыть окно с таймером</summary>
		private void OnOpenTimerWindowCommandExecuted(object? p)
		{
			_openWindows.OpenTimerWindow();
			CloseWindow();
		}

		#endregion

		#region OpenSettingsWindowCommand - Команда - открыть окно с настройками

		///<summary>Команда - открыть окно с настройками</summary>
		private ICommand? _openSettingsWindowCommand;

		///<summary>Команда - открыть окно с настройками</summary>
		public ICommand OpenSettingsWindowCommand => _openSettingsWindowCommand
			??= new LambdaCommand(OnOpenSettingsWindowCommandExecuted, CanOpenSettingsWindowCommandExecute);

		///<summary>Проверка возможности выполнения - открыть окно с настройками</summary>
		private bool CanOpenSettingsWindowCommandExecute(object? p) => true;

		///<summary>Логика выполнения - открыть окно с настройками</summary>
		private void OnOpenSettingsWindowCommandExecuted(object? p)
		{
			_openWindows.OpenSettingsWindow();
			CloseWindow();
		}

		#endregion

		#region CloseWindowCommand - Команда - закрыть окно

		///<summary>Команда - закрыть окно</summary>
		private ICommand? _closeWindowCommand;

		///<summary>Команда - закрыть окно</summary>
		public ICommand CloseWindowCommand => _closeWindowCommand
			??= new LambdaCommand(OnCloseWindowCommandExecuted);

        ///<summary>Логика выполнения - закрыть окно</summary>
        private void OnCloseWindowCommandExecuted(object? p) => CloseWindow();

		#endregion

		#endregion

		#region Methods...

		private void CloseWindow() => Application.Current.MainWindow.Close();

		#endregion
	}
}
