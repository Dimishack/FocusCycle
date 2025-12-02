using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace FocusCycle.Infrasctructure.Behaviors
{
    class TextBlockFlickeringBehavior : Behavior<TextBlock>
    {
        #region PlayFlicker : bool - запустить мерцание

        ///<summary> запустить мерцание (DependencyProperty). </summary>
        public static readonly DependencyProperty PlayFlickerProperty =
            DependencyProperty.Register(nameof(PlayFlicker),
                    typeof(bool),
                    typeof(TextBlockFlickeringBehavior),
                    new PropertyMetadata(false, OnPlayFlickerChanged));

        private static async void OnPlayFlickerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlockFlickeringBehavior behavior && (bool)e.NewValue)
                await behavior.PlayFlickerAnimation();
        }

        ///<summary> запустить мерцание. </summary>
        public bool PlayFlicker
        {
            get => (bool)GetValue(PlayFlickerProperty);
            set => SetValue(PlayFlickerProperty, value);
        }

        #endregion

        private async Task PlayFlickerAnimation()
        {
            await Task.Delay(1000);
            while (PlayFlicker)
            {
                AssociatedObject.Opacity = 0.0;
                await Task.Delay(1000);
                AssociatedObject.Opacity = 1.0;
                await Task.Delay(1000);
            }
        }

    }
}
