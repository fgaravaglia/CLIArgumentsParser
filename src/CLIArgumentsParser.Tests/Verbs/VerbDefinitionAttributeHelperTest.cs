using System.Linq;
using CLIArgumentsParser.Core.Verbs;
using CLIArgumentsParser.Tests.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Verbs
{
	[TestClass]
	public class VerbDefinitionAttributeHelperTest : BaseUnitTest
	{
		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void FromClassWithPropertyMarkedAsVerb_PropertyIsExtracted()
		{
			//******** GIVEN
			var targetType = typeof(ParserTestOnVerbs.TestArguments);

			//******** WHEN
			var extractedProperties = VerbDefinitionAttributeHelper.ExtractPropertiesMarkedWithVerbAttribute(targetType);

			//******** ASSERT
			Assert.IsNotNull(extractedProperties);
			Assert.IsTrue(extractedProperties.Count(x => x.Key.Name == "Copy") > 0, $"Unable to find property Copy inside extracted list");

		}
	}
}
