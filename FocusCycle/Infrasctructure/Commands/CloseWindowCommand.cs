using FocusCycle.Infrasctructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FocusCycle.Infrasctructure.Commands
{
    internal class CloseWindowCommand : Command
    {
        protected override bool CanExecute(object? parameter) => parameter is Window;

        protected override void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;
            (parameter as Window)?.Close();
        }
    }
}
