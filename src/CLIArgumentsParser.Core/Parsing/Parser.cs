using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using CLIArgumentsParser.Core.Verbs;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// Wrapper for the logic on argument parsing operations
	/// </summary>
	public class Parser
	{
		/// <summary>
		/// Command args
		/// </summary>
		string[] _Arguments;
		/// <summary>
		/// Function to use to create a new instance of option class to parse
		/// </summary>
		Func<CLIArguments> _ArgumentsActivator;
		/// <summary>
		/// Adapter to apply to input arguments
		/// </summary>
		Func<string[], string[]> _ArgumentsAdapter;
		/// <summary>
		/// Last error occurred
		/// </summary>
		Exception _OccurredError;
		/// <summary>
		/// True if parser accept alraedy aggregated arguments
		/// </summary>
		bool _UseAggregatedArgumentsAsInputForVerbs;
		CLIUsageModel _Model;

		/// <summary>
		/// Target type to parse as arguments
		/// </summary>
		public Type TargetArgumentsType { get { return _Model != null ? _Model.ArgumentsType : null; } }

		
		/// <summary>
		/// Default Constructor
		/// </summary>
		internal Parser()
		{
			this._Arguments = null;
			this._OccurredError = null;

			AdaptingCommandArguments(x => { return x; });
			UseFactoryForArguments(() => (CLIArguments)Activator.CreateInstance(this.TargetArgumentsType));
		}
		/// <summary>
		/// Getts the last occurred error
		/// </summary>
		/// <returns>The exception occurred; null if everything is fine</returns>
		public Exception GetLastError()
		{
			return this._OccurredError;
		}
		/// <summary>
		/// Sets a custom adapter to transform the arguments before the parsing operation
		/// </summary>
		/// <param name="argumentsAdapter"></param>
		/// <returns></returns>
		public Parser AdaptingCommandArguments(Func<string[], string[]> argumentsAdapter)
		{
			this._ArgumentsAdapter = argumentsAdapter ?? throw new ArgumentNullException(nameof(argumentsAdapter));
			return this;
		}
		/// <summary>
		/// Specifies if parser accepts already aggregated argumetns such VERB + OPTIONS for a give verb
		/// </summary>
		public Parser UseAggregatedArguments()
		{
			this._UseAggregatedArgumentsAsInputForVerbs = true;
			return this;
		}
		/// <summary>
		/// Specifies how to crete an empty instance of <see cref="TargetArgumentsType"/>
		/// </summary>
		/// <param name="activator"></param>
		/// <exception cref="System.ArgumentException">Thrown if <paramref name="activator"/> is null</exception>
		public Parser UseFactoryForArguments(Func<CLIArguments> activator)
		{
			this._ArgumentsActivator = activator ?? throw new ArgumentNullException(nameof(activator));
			return this;
		}
		/// <summary>
		/// Parses the cli arguments and stores internally the proper models
		/// </summary>
		/// <param name="args">string arguments of the CLI</param>
		/// <exception cref="System.ArgumentException">Thrown if <paramref name="args"/> is null, empty or whitespace.</exception>
		public T Parse<T>(string[] args) where T : CLIArguments
		{
			this._Arguments = args ?? throw new ArgumentNullException(nameof(args));

			// parsing
			if (this._ArgumentsAdapter != null)
				this._Arguments = this._ArgumentsAdapter.Invoke(args);

			// initialize the output
			T parsedArguments = null;

			try
			{
				// analyze the target type and build the model for usage
				var analyzer = new ArgumentModelAnalyzer<T>();
				analyzer.Analyze();
				this._Model = analyzer.BuildModel();

				// if needed, I aggregate options for verbs with the verb itself
				if (!_UseAggregatedArgumentsAsInputForVerbs)
				{ 
					var aggregator = new CLIArgumentAggregator(this._Model);
					this._Arguments = aggregator.AdaptFromCLI(_Arguments);
				}

				// instance a new item 
				parsedArguments = (T)this._ArgumentsActivator();
				List<KeyValuePair<PropertyInfo, Option>> options;
				List<KeyValuePair<PropertyInfo, Verb>> verbs;
				ParseArguments(this._Arguments, parsedArguments, out options, out verbs);

				// check mandatory fields
				foreach (var opt in options.Where(x => x.Value.Mandatory))
				{
					var currentValue = opt.Key.GetValue(parsedArguments);
					if (currentValue == null)
						throw new InvalidCLIArgumentException($"Invalid Argument: {opt.Value.LongCode} is missing", opt.Value.LongCode);
				}
			}
			catch (Exception ex)
			{
				//store the error
				this._OccurredError = ex;
			}

			return parsedArguments;
		}

		private void ParseArguments<T>(string[] args, T parsedArguments, out List<KeyValuePair<PropertyInfo, Option>> options, out List<KeyValuePair<PropertyInfo, Verb>> verbs) where T : CLIArguments
		{
			// output argumetnts
			var localOptions = new List<KeyValuePair<PropertyInfo, Option>>();
			this._Model.OptionPropertyRegistry.ToList().ForEach(x =>
			{
				var model = this._Model.Options.Single(o => o.LongCode == x.Key);
				localOptions.Add(new KeyValuePair<PropertyInfo, Option>(x.Value, model));
			});
			options = localOptions;

			var localVerbs = new List<KeyValuePair<PropertyInfo, Verb>>();
			this._Model.VerbPropertyRegistry.ToList().ForEach(x =>
			{
				var model = this._Model.Verbs.Single(o => o.Name == x.Key);
				localVerbs.Add(new KeyValuePair<PropertyInfo, Verb>(x.Value, model));
			});
			verbs = localVerbs;

			foreach (var arg in args)
			{
				// check if we are facing verb or option
				bool isOption = arg.StartsWith(Option.OPTION_IDENTIFIER);
				bool isVerb = !isOption && arg.StartsWith(Verb.VERB_IDENTIFIER);
				if (!isVerb && !isOption)
					throw new InvalidCLIArgumentException($"Unrecognized argument: {arg}", arg);

				// analyze the option
				string argumentKey = string.Empty;
				if (isOption)
				{
					// get the tokens
					var tokenGenerator = new TokenGenerator(new List<string>() { Option.OPTION_IDENTIFIER });
					var tokens = tokenGenerator.TokenizeThisString(arg).ToList();
					if (tokens.Count != 1)
						throw new InvalidOperationException("Unable to get tokens from Option " + arg);
					argumentKey = tokens[0].Name;

					// get information
					var optionDefinition = this._Model.Options.Single(x => x.Code == argumentKey || x.LongCode == argumentKey);
					var p = this._Model.OptionPropertyRegistry[optionDefinition.LongCode];
					// parse the option
					var parser = new OptionParser(optionDefinition, p.PropertyType);
					var optionValue = parser.Parse(tokens);
					// set the proper value
					p.SetValue(parsedArguments, optionValue);
				}
				else if (isVerb)
				{
					// get the tokens
					var tokenGenerator = new TokenGenerator(new List<string>() { Verb.VERB_IDENTIFIER, Option.OPTION_IDENTIFIER });
					var tokens = tokenGenerator.TokenizeThisString(arg).ToList();
					if (tokens.Count == 0)
						throw new InvalidOperationException("Unable to get tokens from Verb " + arg);
					// the first token must to be the verb, with value null
					if (!String.IsNullOrEmpty(tokens[0].Value))
						throw new InvalidOperationException("Unable to get tokens from Verb " + arg);
					argumentKey = tokens[0].Name;

					// get information
					var verbDefinition = this._Model.Verbs.Single(x => x.Name == argumentKey);
					var p = this._Model.VerbPropertyRegistry[verbDefinition.Name];
					// parse the verb 
					var parser = new VerbParser(verbDefinition, p.PropertyType);
					var optionValue = parser.Parse(tokens);
					// set the proper value
					p.SetValue(parsedArguments, optionValue);
				}
				else
				{
					// no matching item!
					throw new InvalidOperationException($"Unable to find an option or verb [Argument # {Array.IndexOf(args, arg)}]: {arg}");
				}
			}
		}


	}
}
