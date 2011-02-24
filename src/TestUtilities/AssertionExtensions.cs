using System;
using FluentAssertions;

namespace TestUtilities
{
	public static class AssertionExtensions
	{
		public static AndConstraint<DateTimeAssertions> BeAboutNow(this DateTimeAssertions self, 
			int deltaMilliseconds = 500)
		{
			return self.BeWithin(TimeSpan.FromSeconds(deltaMilliseconds / 1000.0)).Before(DateTime.Now);
		}
	}
}
