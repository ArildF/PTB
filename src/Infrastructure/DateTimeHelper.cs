using System;

namespace Rogue.Ptb.Infrastructure
{
	public class DateTimeHelper
	{
		private static Func<DateTime> _now;

		static DateTimeHelper()
		{
			_now = () => DateTime.Now;
		}

		public static DateTime Now { get { return _now(); } }

		public static void SetNow(DateTime now)
		{
			_now = () => now;
		}

		public static void SetNow(Func<DateTime> now)
		{
			_now = now;
		}

		public static void Reset()
		{
			SetNow(() => DateTime.Now);
		}

		public static void MoveAheadBy(TimeSpan span)
		{
			var now = _now;
			_now = () => now() + span;
		}

		public static void Fix()
		{
			SetNow(DateTime.Now);
		}
	}
}
