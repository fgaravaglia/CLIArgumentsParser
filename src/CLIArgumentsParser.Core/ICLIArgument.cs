using System;
using System.Collections.Generic;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Verbs;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// abstraction of a argument 
	/// </summary>
	internal interface ICLIArgumentModel
	{
		/// <summary>
		/// Code to identify the argument
		/// </summary>
		string Code { get; }
		/// <summary>
		/// Property type for the option; in case of <see cref="Option"/> it the return type for value, in case of <see cref="Verb"/> it the class that maps it
		/// </summary>
		Type TargetType { get; }
		/// <summary>
		/// Hper to have information about the argument
		/// </summary>
		string HelpText { get; }
		/// <summary>
		/// Gets the type of argument (Verb, Option)
		/// </summary>
		string ArgumentType { get; }
		/// <summary>
		/// Gets the list of examples for the usages
		/// </summary>
		IEnumerable<CLIUsageExample> Examples { get; }
	}
}
