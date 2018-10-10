using System;
using System.Collections.Generic;

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
			Parser parser = new Parser()
				.AdaptingCommandArguments(args =>
				{
					List<int> indexesAlreadyUsed = new List<int>();
					List<string> rebuiltArguments = new List<string>();
					foreach (var arg in args)
					{
						int index = Array.IndexOf(args, arg);
						if (indexesAlreadyUsed.Contains(index))
							continue;

						if (arg.StartsWith("-"))
						{
							var option = arg;
							if (index + 1 <= args.Length)
							{
								option += (" " + args[index + 1]);
								indexesAlreadyUsed.Add(index + 1);
							}
							rebuiltArguments.Add(option);
						}
						else
							rebuiltArguments.Add(arg);
						indexesAlreadyUsed.Add(index);
					}
					return rebuiltArguments.ToArray();
				});
			return parser;
		}
	}
}
