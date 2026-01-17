using FocusCycle.Models;
using System.Globalization;
using System.Windows.Data;

namespace FocusCycle.Infrasctructure.Converters
{
    public class CurrentCycleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cycleString = string.Empty;
            if (value is Cycle cycle)
                switch (cycle)
                {
                    case Cycle.Work:
                        cycleString = "Р а б о т а";
                        break;
                    case Cycle.Break:
                        cycleString = "П е р е р ы в";
                        break;
                    default:
                        break;
                }
            return cycleString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
