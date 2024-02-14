using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfPaint.Models;

namespace WpfPaint.Infrastructure.TemplateSelectors.FigureSelector
{
    class FigureSelector : DataTemplateSelector
    {
        public DataTemplate Circle { get; set; } = null!;
        public DataTemplate Square { get; set; } = null!;
        public DataTemplate Triangle { get; set; } = null!;
        public DataTemplate Line { get;set; } = null!;
        public DataTemplate None { get; set; } = null!;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Figure figure)
            {
                switch (figure)
                {
                    case Figure.Triangle:
                        return Triangle;
                    case Figure.Line:
                        return Line;
                    case Figure.None:
                        return None;
                    case Figure.Square:
                        return Square;
                    case Figure.Circle:
                        return Circle;
                    default:
                        throw new ArgumentException(nameof(figure));
                } 
            }
            return base.SelectTemplate(item, container);
        }
    }
}
