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
		private byte _red;

		public byte Red
		{
			get { return _red; }
			set 
			{ 
				if (value <= 255 && value >= 0)
				{
                    if (Set(ref _red, value))
                    {
                        _currentColor.R = _red;
                        OnPropertyChanged(nameof(CurrentColor));
                    }
                }
			}
		}
		private byte _blue;

		public byte Blue
		{
			get { return _blue; }
			set
			{
				if (value <= 255 && value >= 0)
				{
					if (Set(ref _blue, value))
					{ 
						_currentColor.B = _blue;
						OnPropertyChanged(nameof(CurrentColor));
					}
				}
			}
		}
		private byte _green;

		public byte Green
		{
			get { return _green; }
            set {
				if (value <= 255 && value >= 0)
				{
					if (Set(ref _green, value))
					{
						_currentColor.G = _green;
						OnPropertyChanged(nameof(CurrentColor));
					}
				}
            }
        }
		private Color _currentColor;

		public Color CurrentColor
		{
			get { return _currentColor; }
			set => Set(ref _currentColor, value);
		}
		public ColorPickerViewModel()
		{
			Red = 34;
			Blue = 77;
			Green = 99;
			CurrentColor = Color.FromRgb(Red, Green, Blue);
		}
	}
}
