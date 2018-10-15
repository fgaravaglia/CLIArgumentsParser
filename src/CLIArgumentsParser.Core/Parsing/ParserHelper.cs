using CLIArgumentsParser.Core.Parsing;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// Extension class for parser
	/// </summary>
	public static class ParserHelper
	{
		/// <summary>
		/// Instances a new parser
		/// </summary>
		/// <returns></returns>
		public static Parser DefaultParser()
		{
			Parser parser = new Parser();
			return parser;
		}
	}
}
