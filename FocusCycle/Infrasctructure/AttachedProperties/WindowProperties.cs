using System.Windows;

namespace FocusCycle.Infrasctructure.AttachedProperties
{
    public class WindowProperties : DependencyObject
    {
        #region ShowSettings : bool - Отобразить настройки

        ///<summary> Отобразить настройки (DependencyProperty). </summary>
        public static readonly DependencyProperty ShowSettingsProperty =
            DependencyProperty.Register("ShowSettings",
                    typeof(bool),
                    typeof(WindowProperties),
                    new PropertyMetadata(false));

        public static void SetShowSettings(DependencyObject element, bool value)
            => element.SetValue(ShowSettingsProperty, value);

        public static bool GetShowSettings(DependencyObject element)
            => (bool)element.GetValue(ShowSettingsProperty);

        #endregion

    }
}
