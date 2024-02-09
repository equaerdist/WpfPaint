using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfPaint.Infrastructure.Behaviors
{
    class DrawOnInkCanvas : Behavior<InkCanvas>
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
        public static DependencyProperty IsFillProperty = DependencyProperty.Register(nameof(IsFill),
            typeof(bool),
            typeof(DrawOnInkCanvas),
            new PropertyMetadata(false));
        public bool IsFill
        {
            get => (bool)GetValue(IsFillProperty);
            set => SetValue(IsFillProperty, value);
        }
        private InkCanvas? _canvas;
        private Point _start;
        private Array? _storage;
        protected override void OnAttached()
        {
            base.OnAttached();
            _canvas = (InkCanvas)AssociatedObject;
            _canvas.MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _start = e.GetPosition(_canvas);
            if (_canvas is null)
                throw new ArgumentNullException(nameof(_canvas));
            _canvas.Children.CopyTo(_storage, 0);
            _canvas.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var current = e.GetPosition(_canvas);
            if (_canvas is null)
                throw new ArgumentNullException(nameof(_canvas));
            _canvas.Children.Clear();
        }
        private void DrawRectangle(Point current)
        {
            Rectangle rect = new();
            rect.Stroke = new SolidColorBrush(Color);
            _canvas.Children.Add(rect);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
