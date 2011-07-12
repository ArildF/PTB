﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rogue.Ptb.UI.ValueConverters
{
	public class BoolToCollapsedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var boolValue = (bool) value;
			return boolValue ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
