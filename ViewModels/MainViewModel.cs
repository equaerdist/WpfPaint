using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using WpfPaint.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfPaint.Services.UserDialogs;
using System.Windows.Controls;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using WpfPaint.Services.FileHandler;
using System.Threading.Tasks;
using WpfPaint.Infrastructure.Commands;
using System.Windows;
using WpfPaint.Infrastructure.Behaviors;
using Microsoft.Xaml.Behaviors;
using System.Windows.Media.Media3D;

namespace WpfPaint.ViewModels
{
	public record class ColorPicker();
    class MainViewModel : BaseViewModel
    {
		private string _openFilter = "Все поддерживаемые форматы|*.png;*.jpg;*.jpeg;*.bmp;*.gif|PNG Files (*.png)|*.png|JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif";
        #region Цвет
        private Color _color;
		public ObservableCollection<object> Colors { get; set; }
		public Color Color
		{
			get { return _color; }
			set 
			{ 
				if (Set(ref _color, value))
				{
					if(!Colors.Contains(value))
					{
						Colors.RemoveAt(Colors.Count - 2);
						Colors.Add(value);
                    }
                }
            }
		}
        #endregion
        #region Размер кистей
        private int _brushSize = 10;
		public int BrushSize
		{
			get { return _brushSize; }
			set => Set(ref _brushSize, value);
		}

        private readonly IUserDialogs _dialogs;
        private readonly IFileHandler _fileHandler;

        public IEnumerable<int> BrushSizes { get; }
        #endregion
        #region фигура
        private Figure _figure;
		public Figure Figure
		{
			get => _figure;
			set => Set(ref _figure, value);
		}
		public ObservableCollection<Figure> Figures { get; set; }
        #endregion
        #region заливка
        private bool _fill;

		public bool Fill
		{
			get { return _fill; }
			set => Set(ref _fill, value);
		}
        #endregion
        #region команда сохранения рисунка
        public ICommand SaveFileCommand { get; }
		private async void OnSaveFileExecuted(object? p)
		{
			if (p is not Canvas) throw new ArgumentException();
			var canvas = (Canvas)p;
            string fileName = string.Empty;
			if (!_dialogs.AskForFile(out fileName,  "PNG Files (*.png)|*.png", true, "png"))
				return;

            var renderTargetBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, 
				(int)canvas.ActualHeight, 
				96, 96, 
				PixelFormats.Pbgra32);
         
            renderTargetBitmap.Render(canvas);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			await _fileHandler.HandleFileAsync(async () =>
			{
				await SaveFileFromPng(encoder, fileName);
			});
			
        }
		private bool CanSaveFileExecuted(object? p) => p is Canvas;
		private async Task SaveFileFromPng(PngBitmapEncoder encoder, string fileName)
		{
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(fileStream);
				await fileStream.FlushAsync();
            }
		}
        #endregion
        #region команда загрузки рисунка
        public ICommand OpenImageCommand { get; }
		private void OnOpenImageExecuted(object? p)
		{
			if (!(p is Canvas))
				return;
			var canvas = (Canvas)p;
			string fileName = string.Empty;
			if (!_dialogs.AskForFile(out fileName, _openFilter, false))
				return;
            try
            {
                Image image = new Image();

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fileName, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                image.Source = bitmap;
                var behavior = new ShapeEditBehavior();
				Interaction.GetBehaviors(image).Add(behavior);
                canvas.Children.Add(image);
            }
            catch (Exception ex)
            {
				_dialogs.ShowError($"Ошибка загрузки изображения: {ex.Message}");
            }
        }
		private bool CanOpenImageExecuted(object? p) => p is Canvas canvas;
        #endregion
        #region команда выбора цвета
		public ICommand PickColorCommand { get; set; }
		private void OnPickColorExecuted(object? p)
		{
			Color selectedColor = Color;
			if (!_dialogs.ShowColorPickerDialog(out selectedColor))
				return;
			Color = selectedColor;
		}

        private bool CanPickColorExecute(object? p) => true;
        #endregion
        public MainViewModel(IUserDialogs dialogs, IFileHandler fileHandler)
		{
			PickColorCommand = new RelayCommand(OnPickColorExecuted, CanPickColorExecute);
			OpenImageCommand = new RelayCommand(OnOpenImageExecuted, CanOpenImageExecuted);
			SaveFileCommand = new RelayCommand(OnSaveFileExecuted, CanSaveFileExecuted);
			_dialogs = dialogs;
			_fileHandler = fileHandler;
			BrushSizes = Enumerable.Range(2, 15);
			Colors = new ObservableCollection<object>() 
			{
				Color.FromRgb(23,35,145),
                Color.FromRgb(43,43,43),
                Color.FromRgb(56,87,23),
				Color.FromRgb(54,32,199),
				Color.FromRgb(97,145,225),
				new ColorPicker()
			};
			Figures = new()
			{
				Figure.Triangle, Figure.Square,
				Figure.Circle, Figure.Line
			};
			Color = (Color)Colors.First(t => t.GetType() == typeof(Color));
			Figure = Figures.First();
		}



	}
}
