using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIArgumentsParser.Core.Parsing
{
	/// <summary>
	/// Abstraction of parser for a given object
	/// </summary>
	internal interface IArgumentParser
	{
		/// <summary>
		/// Parse argument (string) to obtain the value
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		object Parse(string arg);
		/// <summary>
		/// return the proper entity model for the current argument
		/// </summary>
		/// <returns></returns>
		object MapToModel();
	}
}
