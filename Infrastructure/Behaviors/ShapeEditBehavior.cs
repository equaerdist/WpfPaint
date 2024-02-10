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
using System.Windows.Shapes;
using WpfPaint.Infrastructure.Extensions;

namespace WpfPaint.Infrastructure.Behaviors
{
    class ShapeEditBehavior : Behavior<Shape>
    {
        private Point startPoint;
        private TransformGroup transformGroup = null!;
        private RotateTransform rotateTransform = null!;
        private ScaleTransform scaleTransform = null!;
        private Canvas? _canvas;
        private DateTime? _lastMove = null;
        public (Point, Point) Location { get; set; }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.MouseLeftButtonDown += OnMouseDown;
            AssociatedObject.MouseLeftButtonUp += OnMouseUp;

            rotateTransform = new RotateTransform();
            scaleTransform = new ScaleTransform();
            transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(scaleTransform);
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
            AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
            AssociatedObject.MouseLeftButtonUp -= OnMouseUp;
            if(_canvas is not null)
                _canvas.MouseMove -= OnMouseMove;

        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(_canvas is null)
                _canvas = AssociatedObject.FindVisualParent<Canvas>();
            if (_canvas is null)
                throw new ArgumentNullException();
            _canvas.Children
            _canvas.MouseMove += OnMouseMove;
            startPoint = e.GetPosition(AssociatedObject);
        }
        private void UnsubscribeMouseMoveOthers()
        {
            if(_canvas is not null)
            {
                foreach(Shape child in _canvas.Children)
                {
                    var shapeEdit = Interaction.GetBehaviors(child).FirstOrDefault();
                }
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
                   
                    Debug.WriteLine(angle);
                    rotateTransform.Angle += angle;

                }
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    double scale = 1 + Math.Sqrt(dx * dx + dy * dy) / 100.0;

                    scaleTransform.ScaleY = scale;
                    scaleTransform.ScaleX = scale;
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
