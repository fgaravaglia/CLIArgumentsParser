using System;
using System.Linq;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Tests.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Options
{
	[TestClass]
	public class OptionDefinitionAttributeHelperTest
	{
		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void FromClassWithPropertyMarkedAsLOVOption_PropertyIsExtracted()
		{
			//******** GIVEN
			var targetType = typeof(ParserTestOnMixedArgs.TestArguments);
			string testPropertyName = "Verbosity";

			//******** PRECONDITION
			Assert.IsNotNull(targetType.GetProperty(testPropertyName), "Precondition: the target type doesn't contain the test property");

			//******** WHEN
			var extractedProperties = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(targetType);

			//******** ASSERT
			Assert.IsNotNull(extractedProperties);
			Assert.IsTrue(extractedProperties.Count(x => x.Key.Name == testPropertyName) > 0, $"Unable to find property {testPropertyName} inside extracted list");

		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void FromClassWithPropertyMarkedAsOption_PropertyIsExtracted()
		{
			//******** GIVEN
			var targetType = typeof(ParserTestOnMixedArgs.TestArguments);
			string testPropertyName = "UseSingleFileAsOutput";

			//******** PRECONDITION
			Assert.IsNotNull(targetType.GetProperty(testPropertyName), "Precondition: the target type doesn't contain the test property");

			//******** WHEN
			var extractedProperties = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(targetType);

			//******** ASSERT
			Assert.IsNotNull(extractedProperties);
			Assert.IsTrue(extractedProperties.Count(x => x.Key.Name == testPropertyName) > 0, $"Unable to find property {testPropertyName} inside extracted list");

		}
	}
}
