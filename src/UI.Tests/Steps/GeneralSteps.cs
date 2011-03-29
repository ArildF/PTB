using System.Globalization;
using System.Threading;
using TechTalk.SpecFlow;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class GeneralSteps
	{
		[Given(@"that the thread culture is '(.*)'")]
		public void GivenThatTheThreadCultureIsNb_No(string culture)
		{
			Thread.CurrentThread.CurrentCulture = 
				Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
		}

		
	}
}
