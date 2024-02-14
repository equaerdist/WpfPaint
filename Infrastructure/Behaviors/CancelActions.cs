using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPaint.Infrastructure.Extensions;

namespace WpfPaint.Infrastructure.Behaviors
{
    class CancelActions : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control) 
            {
                var canvasChild = AssociatedObject.FindChildren<Canvas>()?
                    .FirstOrDefault(c => c.Name == "canvas");
                if (canvasChild is null)
                    return;
                if (canvasChild.Children.Count <= 0)
                    return;
                canvasChild.Children.RemoveAt(canvasChild.Children.Count - 1);
            }
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= OnKeyDown;
        }
    }
}
