using System;
using TechTalk.SpecFlow;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class ContainerSteps
	{
		[Then(@"I want to know what's in the container")]
		public void ThenIWantToKnowWhatSInTheContainer()
		{
			var bootstrapper = new Bootstrapper();
			bootstrapper.Bootstrap();
			Console.WriteLine(bootstrapper.Container.WhatDoIHave());
		}

	}
}
