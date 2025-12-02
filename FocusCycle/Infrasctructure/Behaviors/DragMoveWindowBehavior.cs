using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FocusCycle.Infrasctructure.Behaviors
{
    class DragMoveWindowBehavior : Behavior<TextBlock>
    {
        private Window? _parentWindow;

        protected override void OnAttached()
        {
            base.OnAttached();

            DependencyObject parent = AssociatedObject;
            do
            {
                parent = LogicalTreeHelper.GetParent(parent);
            }
            while (parent != null && parent is not Window);
            _parentWindow = parent as Window;
            if(_parentWindow is not null)
            {
                AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
                AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            }
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
                AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            }
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) 
            => _parentWindow?.DragMove();
    }
}
