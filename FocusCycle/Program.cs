using System.Diagnostics;
using System.Windows;

namespace FocusCycle
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            if (IsAlreadyRunning())
            {
                MessageBox.Show("Данная программа уже запущена");
                return;
            }
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static bool IsAlreadyRunning()
        {
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);

            return processes.Length > 1;
        }
    }
}
