using System;
using Moq;
using Rogue.Ptb.UI.ViewModels;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class ShellSteps
	{
		private readonly Context _context;

		public ShellSteps(Context context)
		{
			_context = context;
		}

		[Then(@"the window title should be ""(.*)""")]
		public void ThenTheWindowTitleShouldBeBar(string title)
		{
			//var moq = new MockFactory(MockBehavior.Loose).Create<IDebugService>();
			//moq.Setup(s => s.Dump(It.IsAny<string>())).Callback<string>(s => Console.WriteLine(s));
			//moq.Object.Dump("Bleh"); 
			//moq.Verify(s => s.Dump("Blah"));

			_context.ShellViewModel.Title.Should().Be(title);
		}
	}
}
