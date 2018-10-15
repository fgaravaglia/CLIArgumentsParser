using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Verbs;

namespace CLIArgumentsParser.Core.Parsing
{
	/// <summary>
	/// Component to aggreagate CLI arguments in workable strings
	/// </summary>
	internal class CLIArgumentAggregator
	{
		readonly CLIUsageModel _Model;

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="model">usage description of the CLI</param>
		internal CLIArgumentAggregator(CLIUsageModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.ArgumentsType == null)
				throw new ArgumentNullException(nameof(model));
			this._Model = model;
		}

		public string[] AdaptFromCLI(string[] cliArgs)
		{
			if (cliArgs == null)
				throw new ArgumentNullException(nameof(cliArgs));

			// make 2 lists
			var verbArgs = cliArgs.Where(x => x.StartsWith(Verb.VERB_IDENTIFIER) && !x.StartsWith(Option.OPTION_IDENTIFIER)).ToList();
			var optionArgs = cliArgs.Where(x => !verbArgs.Contains(x)).ToList();
			if (optionArgs.Count + verbArgs.Count != cliArgs.ToList().Count)
				throw new InvalidOperationException($"Unable to recognize all options and verbs");

			var adaptedArgs = new List<string>();
			var alreadyTakenArgs = new List<string>();
			// check for verbs
			foreach (var verb in this._Model.Verbs)
			{
				if (verbArgs.Contains(Verb.VERB_IDENTIFIER + verb.Name))
				{
					string currentArgument = Verb.VERB_IDENTIFIER + verb.Name;
					alreadyTakenArgs.Add(currentArgument);

					//Chekc for options on the verb
					foreach (var opt in verb.Options)
					{
						if (optionArgs.Exists(x => x.StartsWith(Option.OPTION_IDENTIFIER + opt.Code) || x.StartsWith(Option.OPTION_IDENTIFIER + opt.LongCode)))
						{
							string currentOptionArgument = optionArgs.SingleOrDefault(x => x.StartsWith(Option.OPTION_IDENTIFIER + opt.LongCode));
							if (String.IsNullOrEmpty(currentOptionArgument))
								currentOptionArgument = optionArgs.SingleOrDefault(x => x.StartsWith(Option.OPTION_IDENTIFIER + opt.Code));
							alreadyTakenArgs.Add(currentOptionArgument);

							// concatenate on the same string
							currentArgument += (" " + currentOptionArgument);
						}
					}

					adaptedArgs.Add(currentArgument);
				}
			}
			// check for options
			foreach (var option in this._Model.Options)
			{
				if (optionArgs.Exists(x => x.StartsWith(Option.OPTION_IDENTIFIER + option.Code) || x.StartsWith(Option.OPTION_IDENTIFIER + option.LongCode)))
				{
					string currentArgument = optionArgs.SingleOrDefault(x => x.StartsWith(Option.OPTION_IDENTIFIER + option.LongCode));
					if(String.IsNullOrEmpty(currentArgument))
						currentArgument = optionArgs.SingleOrDefault(x => x.StartsWith(Option.OPTION_IDENTIFIER + option.Code));
					alreadyTakenArgs.Add(currentArgument);
					adaptedArgs.Add(currentArgument);
				}
			}

			// check consistency
			if (alreadyTakenArgs.Count != cliArgs.ToList().Count)
			{
				var msg = String.Join("; ", cliArgs.Where(a => !alreadyTakenArgs.Contains(a)));
				throw new InvalidOperationException($"Unrecognixed options: {msg}");
			}
			return adaptedArgs.ToArray();
		}
	}
}
