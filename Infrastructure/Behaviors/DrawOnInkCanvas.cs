using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ObjectiveC;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfPaint.Infrastructure.Extensions;
using WpfPaint.Models;

namespace WpfPaint.Infrastructure.Behaviors
{
    class DrawOnInkCanvas : Behavior<Canvas>
    {
        #region зависимое свойство цвета
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color),
            typeof(Color),
            typeof(DrawOnInkCanvas),
            new PropertyMetadata(System.Windows.Media.Color.FromRgb(85,35,35)));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty); 
            set => SetValue(ColorProperty, value);
        }
        #endregion
        #region Зависимое свойство размера кисти
        public static DependencyProperty BrushSizeProperty = DependencyProperty.Register(nameof(BrushSize),
            typeof(int),
            typeof(DrawOnInkCanvas),
            new PropertyMetadata(5));
        public int BrushSize
        {
            get => (int)GetValue(BrushSizeProperty);
            set => SetValue(BrushSizeProperty, value);
        }
        #endregion
        #region свойство заливки
        public static DependencyProperty IsFillProperty = DependencyProperty.Register(nameof(IsFill),
            typeof(bool),
            typeof(DrawOnInkCanvas),
            new PropertyMetadata(false));
        public bool IsFill
        {
            get => (bool)GetValue(IsFillProperty);
            set => SetValue(IsFillProperty, value);
        }
        #endregion
        #region свойство фигуры
        public static DependencyProperty FigureProperty = DependencyProperty.Register(nameof(Figure),
            typeof(Figure),
            typeof(DrawOnInkCanvas),
            new PropertyMetadata(Figure.None));
        public Figure Figure
        {
            get => (Figure)GetValue(FigureProperty);
            set => SetValue(FigureProperty, value);
        }
        #endregion
        private Canvas? _canvas;
        private Point _start;
        private UIElementCollection? _storage;
        /// <summary>
        /// Используется, чтобы при рисовании фигура не перекрывала канвас и тем самым, 'cъедала' событие отжатия клавиши мыши
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            _canvas = (Canvas)AssociatedObject;
            _canvas.MouseLeftButtonDown += OnMouseLeftButtonDown;
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _start = e.GetPosition(_canvas);
            if (_canvas is null)
                throw new ArgumentNullException(nameof(_canvas));
            _storage = new UIElementCollection(_canvas, _canvas) { Capacity = _canvas.Children.Capacity };

            for (int i = 0; i < _canvas.Children.Count; i++)
            {
                _storage.Add(_canvas.Children[i].GetCopy());
            }
            _canvas.MouseMove += OnMouseMove;
            _canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_canvas is null)
                throw new ArgumentNullException(nameof(_canvas));
            _canvas.MouseMove -= OnMouseMove;
            _canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var current = e.GetPosition(_canvas);
            if (_canvas is null || _storage is null)
                throw new ArgumentNullException(nameof(_canvas));
            var diffCount = _canvas.Children.Count - _storage.Count;
            _canvas.Children.RemoveRange(_canvas.Children.Count - diffCount, diffCount);
            if(Figure == Figure.Square)
                DrawRectangle(current);
            if (Figure == Figure.Circle)
                DrawEllipse(current);
        }
        private void DrawEllipse(Point current)
        {
            Ellipse shape = new();
            ConfigureShape((Shape)shape);
            ConfigurePosition(shape, current);
            _canvas?.Children.Add(shape);
        }
        private void ConfigureShape(Shape shape)
        {
            shape.Stroke = new SolidColorBrush(Color);
            shape.StrokeThickness = BrushSize;
            if (IsFill)
                shape.Fill = new SolidColorBrush(Color);
        }
        private void DrawRectangle(Point current)
        {
            Rectangle rect = new();
            ConfigureShape((Shape)rect);
            ConfigurePosition(rect, current);
            _canvas?.Children.Add(rect);
        }
        private void ConfigurePosition(Shape shape, Point current)
        {

            var area = CalculateArea(current);
            shape.Width = area.size.X;
            shape.Height = area.size.Y;
         
            
            shape.SetValue(Canvas.LeftProperty, area.position.X);
            shape.SetValue(Canvas.TopProperty, area.position.Y);
        }
        private (Point size, Point position) CalculateArea(Point current)
        {
            var diff = current - _start;
            var width = Math.Abs(diff.X);
            var height = Math.Abs(diff.Y);
            if (diff.X > 0)
                width -= width / 10;
            if (diff.Y > 0)
                height -= height / 10;
            var x = Math.Min(_start.X, current.X);
            var y = Math.Min(_start.Y, current.Y);
            if (diff.X < 0)
                x += x / 10;
            if (diff.Y < 0)
                y += y / 10;
            return (new(width, height), new(x, y));
        }


        protected override void OnDetaching()
        {
            if (_canvas is null)
                throw new ArgumentNullException(nameof(_canvas));
            _canvas.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            base.OnDetaching();
        }
    }
}
