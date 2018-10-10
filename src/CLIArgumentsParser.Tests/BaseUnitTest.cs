using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests
{
	[TestClass]
	public abstract class BaseUnitTest
	{
		public const string UNIT = "UNIT";

		[TestInitialize]
		public void PrepareTestData()
		{
			InitializeMyTestData();
		}

		[TestCleanup]
		public void CleanTestData()
		{
		}

		protected virtual void InitializeMyTestData()
		{ 
		
		}
	}
}
