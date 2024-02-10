using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using WpfPaint.Models;
using System.Collections.ObjectModel;

namespace WpfPaint.ViewModels
{
    class MainViewModel : BaseViewModel
    {
		#region Цвет
		private Color _color;
		public ObservableCollection<Color> Colors { get; set; }
		public Color Color
		{
			get { return _color; }
			set => Set(ref _color, value);
		}
		#endregion
		#region Размер кистей
		private int _brushSize = 10;
		public int BrushSize
		{
			get { return _brushSize; }
			set => Set(ref _brushSize, value);
		}
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
        private bool _fill;

		public bool Fill
		{
			get { return _fill; }
			set => Set(ref _fill, value);
		}
		public MainViewModel()
		{
			BrushSizes = Enumerable.Range(2, 15);
			Colors = new ObservableCollection<Color>() 
			{
				Color.FromRgb(23,35,145),
				Color.FromRgb(56,87,23),
				Color.FromRgb(54,32,199),
				Color.FromRgb(97,145,225)
			};
			Figures = new()
			{
				Figure.Triangle, Figure.Square,
				Figure.Circle, Figure.Line, Figure.None
			};
			Color = Colors.First();
			Figure = Figures.First();
		}



	}
}
