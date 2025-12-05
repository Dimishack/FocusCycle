using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FocusCycle.Infrasctructure.Behaviors
{
    class OnlyDigitsBehavior : Behavior<TextBox>
    {

        #region MaxValue : int - Максимальное значение

        ///<summary> Максимальное значение (DependencyProperty). </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue),
                    typeof(int),
                    typeof(OnlyDigitsBehavior),
                    new PropertyMetadata(int.MaxValue));

        ///<summary> Максимальное значение. </summary>
        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        #endregion


        protected override void OnAttached()
        {
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            CleanUp();
            base.OnDetaching();
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e) => CleanUp();

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text))
            {
                AssociatedObject.Text = "0";
                AssociatedObject.Select(1, 1);
            }
            else if(int.TryParse(AssociatedObject.Text, out var val)
                && val > MaxValue)
            {
                AssociatedObject.Text = MaxValue.ToString();
                AssociatedObject.Select(AssociatedObject.Text.Length, 1);
            }
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (e.Key < Key.D0 ^ e.Key > Key.D9) && (e.Key < Key.NumPad0 ^ e.Key > Key.NumPad9)
                || e.KeyboardDevice.Modifiers != ModifierKeys.None;
        }

        private void CleanUp()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
                AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            }
        }
    }
}
