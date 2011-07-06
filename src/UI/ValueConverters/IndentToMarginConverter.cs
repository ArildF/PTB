using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rogue.Ptb.UI.ValueConverters
{
	public class IndentToMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			const double param = 30.0;

			int indent = (int) value;

			return new Thickness(indent*param, 0, 0, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
