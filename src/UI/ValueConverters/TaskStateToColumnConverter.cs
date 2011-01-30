using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ValueConverters
{
	public class TaskStateToColumnConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var state = (TaskState) value;
			switch (state)
			{
				case TaskState.Complete:
					return 4;
				case TaskState.InProgress:
					return 2;
				case TaskState.NotStarted:
					return 0;
			}

			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
