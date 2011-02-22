using System.Linq;
using FluentAssertions;
using Rogue.Ptb.Core;
using TechTalk.SpecFlow;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class VersionSteps
	{
		private readonly Context _context;
		private DatabaseVersion _version;

		public VersionSteps(Context context)
		{
			_context = context;
		}

		[Then(@"I should be able to read the database version from the database")]
		public void ThenIShouldBeAbleToReadTheDatabaseVersionFromTheDatabase()
		{
			using (var repos = _context.Get<IRepository<DatabaseVersion>>())
			{
				_version = repos.FindAll().First();
			}
		}

		[Then(@"the version should be higher than 0")]
		public void ThenTheVersionShouldBeHigherThan0()
		{
			_version.Number.Should().BeGreaterThan(0);
		}

	}
}
