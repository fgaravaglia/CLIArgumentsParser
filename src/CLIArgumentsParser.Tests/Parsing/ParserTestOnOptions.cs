using System;
using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Parsing
{
	[TestClass]
	public class ParserTestOnOptions : BaseUnitTest
	{
		Parser _Parser;

		protected override void InitializeMyTestData()
		{
			base.InitializeMyTestData();

			this._Parser = new Parser();
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ParsingNullArgumentsThrowsException()
		{
			//******** GIVEN

			//******** WHEN
			this._Parser.Parse<OptionArguments>(null);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingNotExpectedArgumentsThrowsException()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-unexpectedOption" };
			Exception thrownEx = null;

			//******** WHEN
			this._Parser.Parse<EmptyArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			Assert.IsNotNull(thrownEx, "Expected an exception at this stage");
			Assert.IsInstanceOfType(thrownEx, typeof(InvalidOperationException));
			Assert.IsTrue(thrownEx.Message.ToLowerInvariant().StartsWith("Unable to find an option of verb with name unexpectedOption".ToLowerInvariant()),
						$"Wrong exception message: found <{thrownEx.Message}>");
		}

		#region Option Tests

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingNotMandatoryOptionByCode_FillRelatedProperty()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-s" };
			Exception thrownEx = null;

			//******** WHEN
			OptionArguments parsed = this._Parser.Parse<OptionArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = "Expected no exception at this stage";
			if (thrownEx != null)
				message += (": " + thrownEx.Message);
			Assert.IsNull(thrownEx, message);
			Assert.IsNotNull(parsed);
			Assert.AreEqual(parsed.IsSingleOutput, true);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingNotMandatoryOptionByLongCode_FillRelatedProperty()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-singleOutput" };
			Exception thrownEx = null;

			//******** WHEN
			OptionArguments parsed = this._Parser.Parse<OptionArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = "Expected no exception at this stage";
			if (thrownEx != null)
				message += (": " + thrownEx.Message);
			Assert.IsNull(thrownEx, message);
			Assert.IsNotNull(parsed);
			Assert.AreEqual(parsed.IsSingleOutput, true);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingOptionWithArguments_FillRelatedProperty()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-v MYVERBOSITY" };
			Exception thrownEx = null;

			//******** WHEN
			OptionWithArguments parsed = this._Parser.Parse<OptionWithArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = "Expected no exception at this stage";
			if (thrownEx != null)
				message += (": " + thrownEx.Message);
			Assert.IsNull(thrownEx, message);
			Assert.IsNotNull(parsed);
			Assert.AreEqual(parsed.Verbosity, "MYVERBOSITY");
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingOptionWithArgumentsAndDefault_FillRelatedProperty()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-v" };
			Exception thrownEx = null;

			//******** WHEN
			OptionWithArguments parsed = this._Parser.Parse<OptionWithArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = "Expected no exception at this stage";
			if (thrownEx != null)
				message += (": " + thrownEx.Message);
			Assert.IsNull(thrownEx, message);
			Assert.IsNotNull(parsed);
			Assert.AreEqual(parsed.Verbosity, "DEFVAL");
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingMandatoryOptionWithArgumentsAndDefault_FillRelatedProperty()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-v" };
			Exception thrownEx = null;

			//******** WHEN
			MandatoryOptionWithArguments parsed = this._Parser.Parse<MandatoryOptionWithArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = "Expected no exception at this stage";
			if (thrownEx != null)
				message += (": " + thrownEx.Message);
			Assert.IsNull(thrownEx, message);
			Assert.IsNotNull(parsed);
			Assert.AreEqual(parsed.Verbosity, "DEFVAL");
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingMandatoryOptionWithArgumentsAndDefault_ThrowsExceptionIfMissing()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-singleOutput" };
			Exception thrownEx = null;

			//******** WHEN
			MandatoryOptionWithArguments parsed = this._Parser.Parse<MandatoryOptionWithArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			Assert.IsNotNull(thrownEx);
			Assert.IsInstanceOfType(thrownEx, typeof(InvalidCLIArgumentException), $"Wrong type!!!" + thrownEx.Message);
			Assert.AreEqual("verbosity", ((InvalidCLIArgumentException)thrownEx).ArgumentCode, "Wrong invalid argument!!!");
		}

		#endregion

		#region Used Types

		public class EmptyArguments : CLIArguments
		{

		}

		public class OptionArguments : CLIArguments
		{
			[OptionDefinition("s", "singleOutput", description: @"manage output as unique file instead to split it into several ones")]
			public bool IsSingleOutput { get; set; }
		}

		public class OptionWithArguments : CLIArguments
		{
			[OptionDefinition("v", "verbosity", description: @"manage output as unique file instead to split it into several ones", mandatory: false, defaultValue: "DEFVAL")]
			public string Verbosity { get; set; }
		}

		public class MandatoryOptionWithArguments : CLIArguments
		{
			[OptionDefinition("s", "singleOutput", description: @"manage output as unique file instead to split it into several ones")]
			public bool IsSingleOutput { get; set; }

			[OptionDefinition("v", "verbosity", description: @"manage output as unique file instead to split it into several ones", mandatory: true, defaultValue: "DEFVAL")]
			public string Verbosity { get; set; }
		}

		#endregion
	}
}
