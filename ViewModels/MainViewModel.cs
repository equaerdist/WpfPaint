using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaint.Models;

namespace WpfPaint.ViewModels
{
    class MainViewModel : BaseViewModel
    {
		private Color _color = System.Windows.Media.Color.FromRgb(85, 35, 35);

		public Color Color
		{
			get { return _color; }
			set => Set(ref _color, value);
		}
		private int _brushSize = 10;

		public int BrushSize
		{
			get { return _brushSize; }
			set => Set(ref _brushSize, value);
		}
		private Figure _figure;

		public Figure Figure
		{
			get => _figure;
			set => Set(ref _figure, value);
		}
		private bool _fill;

		public bool Fill
		{
			get { return _fill; }
			set => Set(ref _fill, value);
		}




	}
}
