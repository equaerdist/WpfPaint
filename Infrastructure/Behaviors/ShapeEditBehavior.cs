using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using WpfPaint.Infrastructure.Extensions;

namespace WpfPaint.Infrastructure.Behaviors
{
    class ShapeEditBehavior : Behavior<Shape>
    {
        private Point startPoint;
        private TransformGroup transformGroup = null!;
        private RotateTransform rotateTransform = null!;
        private TranslateTransform translateTransform = null!;
        private ScaleTransform scaleTransform = null!;
        private double smoothFactor = 0.05;
        private Canvas? _canvas;
        private DateTime? _lastMove = null;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.MouseDown += OnMouseDown;
            AssociatedObject.MouseUp += OnMouseUp;

            rotateTransform = new RotateTransform();
            translateTransform = new TranslateTransform();
            scaleTransform = new ScaleTransform();
            transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            AssociatedObject.RenderTransformOrigin = new Point(0.5, 0.5);

            AssociatedObject.RenderTransform = transformGroup;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            AssociatedObject.Cursor = Cursors.None;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            AssociatedObject.Cursor = Cursors.Hand;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseEnter -= OnMouseEnter;
            AssociatedObject.MouseLeave -= OnMouseLeave;
            AssociatedObject.MouseDown -= OnMouseDown;
            AssociatedObject.MouseUp -= OnMouseUp;
            

            if (_canvas is not null)
            {
                _canvas.MouseUp -= OnMouseUpCanvas;
                _canvas.MouseMove -= OnMouseMove;
            }

        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(_canvas is null)
                _canvas = AssociatedObject.FindVisualParent<Canvas>();
            if (_canvas is null)
                throw new ArgumentNullException();
            _canvas.MouseMove += OnMouseMove;
            _canvas.MouseUp += OnMouseUpCanvas;
            startPoint = e.GetPosition(AssociatedObject);
        }

        private void OnMouseUpCanvas(object sender, MouseButtonEventArgs e)
        {
            if(_canvas is not null)
            {
                _canvas.MouseUp -= OnMouseUpCanvas;
                _canvas.MouseMove -= OnMouseMove;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(AssociatedObject);

            double dx = currentPoint.X - startPoint.X;
            double dy = currentPoint.Y - startPoint.Y;
            if (Math.Abs(dx) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(dy) > SystemParameters.MinimumVerticalDragDistance)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double angle = Math.Atan2(dy, dx) * (180 / Math.PI);
                    angle = smoothFactor * angle;
                    rotateTransform.Angle += angle;

                }
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    var scaleY = (Math.Sqrt(dy * dy) / 500.0 * (dy > 0 ? 1 : dy == 0 ? 0 : -1));
                    if (Math.Abs(scaleY) > 1)
                        scaleY *= 0.2;
                    var scaleX = (Math.Sqrt(dx * dx) / 500.0 * (dx > 0 ? 1 : dx == 0 ? 0 : -1));
                    if (Math.Abs(scaleX) > 1)
                        scaleX *= 0.2;
                    if(scaleTransform.ScaleY + scaleY < 3 && scaleTransform.ScaleY + scaleY > 0.25)
                        scaleTransform.ScaleY += scaleY;
                    if (scaleTransform.ScaleX + scaleX < 3 && scaleTransform.ScaleX + scaleX > 0.25)
                        scaleTransform.ScaleX += scaleX;
                }
                if(e.MiddleButton == MouseButtonState.Pressed)
                {
                    translateTransform.X += dx / 2;
                    translateTransform.Y += dy / 2;
                }
                startPoint = currentPoint;
               
            }
            _lastMove = DateTime.Now;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_canvas is null)
                _canvas = AssociatedObject.FindVisualParent<Canvas>();
            if (_canvas is null)
                throw new ArgumentNullException();
            _canvas.MouseMove -= OnMouseMove;
        }
    }
}
