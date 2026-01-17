using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace FocusCycle.Infrasctructure.Behaviors
{
    class WindowVisibilityBehavior : Behavior<Window>
    {
        #region IsChangeWindowVisibility : bool - Изменить видимость окна

        ///<summary> Изменить видимость окна (DependencyProperty). </summary>
        public static readonly DependencyProperty IsChangeWindowVisibilityProperty =
            DependencyProperty.Register(nameof(IsChangeWindowVisibility),
                    typeof(bool),
                    typeof(WindowVisibilityBehavior),
                    new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsChangeWindowVisibilityd));

        private static void OnIsChangeWindowVisibilityd(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is WindowVisibilityBehavior behavior && (bool)e.NewValue)
            {
                if (behavior.AssociatedObject.Visibility != Visibility.Visible)
                {
                    behavior.AssociatedObject.Show();
                    behavior.AssociatedObject.Activate();
                }
                else behavior.AssociatedObject.Hide();
                App.Current.Dispatcher.BeginInvoke(() => behavior.IsChangeWindowVisibility = false);
            }
        }

        ///<summary> Изменить видимость окна. </summary>
        public bool IsChangeWindowVisibility
        {
            get => (bool)GetValue(IsChangeWindowVisibilityProperty);
            set => SetValue(IsChangeWindowVisibilityProperty, value);
        }

        #endregion
    }
}
