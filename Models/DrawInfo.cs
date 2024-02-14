using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfPaint.Models
{
    public record class DrawInfo((Point, Point) Location, Shape Shape);
}
