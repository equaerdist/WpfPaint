using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfPaint.Infrastructure.Extensions
{
    public static class UIELement
    {
        public static T GetCopy<T>(this T element) where T : UIElement
        {
            using (var ms = new MemoryStream())
            {
                XamlWriter.Save(element, ms);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)XamlReader.Load(ms);
            }
        }
        public static T? FindVisualParent<T>(this DependencyObject element) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (T?)parent;
        }
        public static IEnumerable<T> FindChildren<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

                    if (child is T typedChild)
                    {
                        yield return typedChild;
                    }

                    foreach (T childOfChild in FindChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
