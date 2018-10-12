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
		Exception _OccurredError;

		/// <summary>
		/// Target type to parse as arguments
		/// </summary>
		public Type TargetArgumentsType { get; private set; }

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

			this.TargetArgumentsType = typeof(T);
			T parsedArguments = null;

			try
			{
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
			// I need to extract the list of verbs and options
			Dictionary<PropertyInfo, VerbDefinitionAttribute> verbDefinitions = VerbDefinitionAttributeHelper.ExtractPropertiesMarkedWithVerbAttribute(this.TargetArgumentsType);
			Dictionary<PropertyInfo, OptionDefinitionAttribute> optionDefinitions = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(this.TargetArgumentsType);
			// output argumetnts
			var localOptions = new List<KeyValuePair<PropertyInfo, Option>>();
			optionDefinitions.ToList().ForEach(x =>
			{
				var model = new OptionParser(x.Value, typeof(T)).MapToModel();
				localOptions.Add(new KeyValuePair<PropertyInfo, Option>(x.Key, (Option)model));
			});
			options = localOptions;

			var localVerbs = new List<KeyValuePair<PropertyInfo, Verb>>();
			verbDefinitions.ToList().ForEach(x =>
			{
				var model = new VerbParser(x.Value, typeof(T)).MapToModel();
				localVerbs.Add(new KeyValuePair<PropertyInfo, Verb>(x.Key, (Verb)model));
			});
			verbs = localVerbs;

			foreach (var arg in args)
			{
			// check if we are facing verb or option
				// get the code form the string representation of the argument
				var code = arg.Split(' ')[0].Trim();
				if (code.StartsWith("-"))
					code = code.Remove(0, 1);

				// search for verbs or options
				var isVerb = verbDefinitions.Where(x => x.Value.Name == code).Count() > 0;
				var isOption = optionDefinitions.Where(x => x.Value.Code == code || x.Value.LongCode == code).Count() > 0;
				if (isVerb)
				{
					// get information
					var p = verbDefinitions.Single(x => x.Value.Name == code).Key;
					var verbDefinition = verbDefinitions.Single(x => x.Value.Name == code).Value;
					// parse the verb 
					var parser = new VerbParser(verbDefinition, p.PropertyType);
					var optionValue = parser.Parse(arg);
					// set the proper value
					p.SetValue(parsedArguments, optionValue);
				}
				else if (isOption)
				{
					// get information
					var p = optionDefinitions.Single(x => x.Value.Code == code || x.Value.LongCode == code).Key;
					var optionDefinition = optionDefinitions.Single(x => x.Value.Code == code || x.Value.LongCode == code).Value;
					// parse the option
					var parser = new OptionParser(optionDefinition, p.PropertyType);
					var optionValue = parser.Parse(arg);
					// set the proper value
					p.SetValue(parsedArguments, optionValue);
				}
				else
				{
					// no matching item!
					throw new InvalidOperationException($"Unable to find an option of verb with name {code} [Argument # {Array.IndexOf(args, arg)}: {arg}");
				}
			}
		}


	}
}
