using System.Windows;
using System.Windows.Input;

namespace FocusCycle.Infrasctructure.AttachedProperties
{
    public class WindowProperties : DependencyObject
    {
        #region ShowWindowManagerButtons : bool - Отобразить кнопки управления окном

        ///<summary> Отобразить кнопки управления окном (DependencyProperty). </summary>
        public static readonly DependencyProperty ShowWindowManagerButtonsProperty =
            DependencyProperty.Register("ShowWindowManagerButtons",
                    typeof(bool),
                    typeof(WindowProperties),
                    new PropertyMetadata(false));

        public static void SetShowWindowManagerButtons(DependencyObject element, bool value)
            => element.SetValue(ShowWindowManagerButtonsProperty, value);

        public static bool GetShowWindowManagerButtons(DependencyObject element)
            => (bool)element.GetValue(ShowWindowManagerButtonsProperty);

        #endregion


        #region OpenSettingsCommand : ICommand? - Открыть настройки

        ///<summary> Открыть настройки (DependencyProperty). </summary>
        public static readonly DependencyProperty OpenSettingsCommandProperty =
            DependencyProperty.Register("OpenSettingsCommand",
                    typeof(ICommand),
                    typeof(WindowProperties),
                    new PropertyMetadata(null));

        public static void SetOpenSettingsCommand(DependencyObject element, ICommand? command)
            => element.SetValue(OpenSettingsCommandProperty, command);

        public static ICommand? GetOpenSettingsCommand(DependencyObject element)
            =>(ICommand?)element.GetValue(OpenSettingsCommandProperty);

        #endregion


    }
}
