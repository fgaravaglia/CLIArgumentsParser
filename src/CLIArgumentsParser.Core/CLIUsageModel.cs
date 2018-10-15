using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Verbs;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// Model to map usage of cli
	/// </summary>
	public class CLIUsageModel
	{
		/// <summary>
		/// Type of class that contains arguments for CLI
		/// </summary>
		public Type ArgumentsType { get; private set; }
		/// <summary>
		/// Availbale options
		/// </summary>
		public List<Option> Options { get; private set; }
		/// <summary>
		/// Available verbs
		/// </summary>
		public List<Verb> Verbs { get; private set; }
		/// <summary>
		/// Registry to store mapping between name of option and Property of given <see cref="ArgumentsType"/>
		/// </summary>
		public Dictionary<string, PropertyInfo> OptionPropertyRegistry { get; private set; }
		/// <summary>
		/// Registry to store mapping between name of verb and Property of given <see cref="ArgumentsType"/>
		/// </summary>
		public Dictionary<string, PropertyInfo> VerbPropertyRegistry { get; private set; }

		/// <summary>
		/// Defualt Model
		/// </summary>
		/// <param name="target"></param>
		/// <param name="options"></param>
		/// <param name="verbs"></param>
		public CLIUsageModel(Type target, IEnumerable<Option> options, IEnumerable<Verb> verbs)
		{
			this.ArgumentsType = target ?? throw new ArgumentNullException(nameof(target));
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			if (verbs == null)
				throw new ArgumentNullException(nameof(verbs));

			this.Options = new List<Option>();
			this.Options.AddRange(options.ToList());

			this.Verbs = new List<Verb>();
			this.Verbs.AddRange(verbs.ToList());

			this.OptionPropertyRegistry = new Dictionary<string, PropertyInfo>();
			this.VerbPropertyRegistry = new Dictionary<string, PropertyInfo>();
		}

		internal void MapOption(string longCode, PropertyInfo targetProperty)
		{
			if(targetProperty == null)
				throw new ArgumentNullException(nameof(targetProperty));

			if (this.OptionPropertyRegistry.ContainsKey(longCode))
				throw new InvalidOperationException($"Unable to map Option: item {longCode} already mapped");
			this.OptionPropertyRegistry.Add(longCode, targetProperty);
		}

		internal void MapVerb(string name, PropertyInfo targetProperty)
		{
			if (targetProperty == null)
				throw new ArgumentNullException(nameof(targetProperty));

			if (this.VerbPropertyRegistry.ContainsKey(name))
				throw new InvalidOperationException($"Unable to map Verb: item {name} already mapped");
			this.VerbPropertyRegistry.Add(name, targetProperty);
		}
	}
}
