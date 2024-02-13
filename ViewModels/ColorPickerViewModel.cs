using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfPaint.ViewModels
{
    internal class ColorPickerViewModel : BaseViewModel
    {
		private int _red;

		public int Red
		{
			get { return _red; }
			set { if (value <= 255 && value >= 0) Set(ref _red, value); }
		}
		private int _blue;

		public int Blue
		{
			get { return _blue; }
            set { if (value <= 255 && value >= 0) Set(ref _blue, value); }
        }
		private int _green;

		public int Green
		{
			get { return _green; }
            set { if (value <= 255 && value >= 0) Set(ref _green, value); }
        }
		private Color _currentColor;

		public Color CurrentColor
		{
			get { return _currentColor; }
			set => Set(ref _currentColor, value);
		}




	}
}
