using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace FocusCycle.Infrasctructure.Behaviors
{
    class WindowVisibilityBehavior : Behavior<Window>
    {
        #region WindowVisibility : Visibility - Видимость окн

        ///<summary> Видимость окн (DependencyProperty). </summary>
        public static readonly DependencyProperty WindowVisibilityProperty =
            DependencyProperty.Register(nameof(WindowVisibility),
                    typeof(Visibility),
                    typeof(WindowVisibilityBehavior),
                    new PropertyMetadata(Visibility.Visible, OnWindowVisibilityChanged));

        private static void OnWindowVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is WindowVisibilityBehavior behavior
                && e.NewValue is Visibility visibility)
            {
                if (visibility == Visibility.Visible)
                    behavior.AssociatedObject.Show();
                else behavior.AssociatedObject.Hide();
            }

        }

        ///<summary> Видимость окн. </summary>
        public Visibility WindowVisibility
        {
            get => (Visibility)GetValue(WindowVisibilityProperty);
            set => SetValue(WindowVisibilityProperty, value);
        }

        #endregion

    }
}
