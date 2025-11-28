using FocusCycle.Infrasctructure.Commands;
using FocusCycle.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FocusCycle.ViewModels
{
    internal class TimerViewModel : ViewModel
    {

		#region Properties...

		#endregion

		#region Commands...

		#region ClosedCommand - Команда - закрытие

		///<summary>Команда - закрытие</summary>
		private ICommand? _closedCommand;

		///<summary>Команда - закрытие</summary>
		public ICommand ClosedCommand => _closedCommand
			??= new LambdaCommand(OnClosedCommandExecuted);

        ///<summary>Логика выполнения - закрытие</summary>
        private void OnClosedCommandExecuted(object? p) => App.Current.Shutdown();

        #endregion

        #endregion

    }
}
