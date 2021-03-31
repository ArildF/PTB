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
		private static readonly List<Tuple<TaskState, int>> _table = new List<Tuple<TaskState, int>>
			{
				Tuple.Create(TaskState.NotStarted, 0),
				Tuple.Create(TaskState.InProgress, 1),
				Tuple.Create(TaskState.Complete, 2),
				Tuple.Create(TaskState.Abandoned, 3),
			};

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return _table.Where(t => t.Item1.Equals(value)).First().Item2;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return _table.Where(t => t.Item2.Equals(value)).First().Item1;
		}
	}
}
