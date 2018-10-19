using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Verbs;

namespace CLIArgumentsParser.Core.Parsing
{
	internal class ArgumentModelAnalyzer<T> where T : ICLIArguments
	{
		readonly Type _TargetType;
		readonly List<Option> _Options;
		readonly List<Verb> _Verbs;
		readonly Dictionary<PropertyInfo, VerbDefinitionAttribute> _VerbDefinitions;
		readonly Dictionary<PropertyInfo, OptionDefinitionAttribute> _OptionDefinitions;

		/// <summary>
		/// Defualt Constructor
		/// </summary>
		public ArgumentModelAnalyzer()
		{
			_TargetType = typeof(T);
			this._Options = new List<Option>();
			this._Verbs = new List<Verb>();

			this._VerbDefinitions = VerbDefinitionAttributeHelper.ExtractPropertiesMarkedWithVerbAttribute(this._TargetType);
			this._OptionDefinitions = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(this._TargetType);
		}

		/// <summary>
		/// Analyze the type and build the usage model
		/// </summary>
		public void Analyze()
		{
			foreach (var v in _VerbDefinitions)
			{
				if (_Verbs.Exists(x => x.Name == v.Value.Name))
					throw new InvalidOperationException($"Verb {v.Value.Name} already analyzed");
				var verb = Verb.FromAttribute(v.Value).OnTargetProperty(v.Key.PropertyType);
				// get options
				var optionDefinitionForCUrrentVerb = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(v.Key.PropertyType);
				foreach (var otp in optionDefinitionForCUrrentVerb)
				{ 
					verb.AddOptionFromAttribute(otp.Value, otp.Key.PropertyType);
				}

				// get examples
				ICLIArguments concreteSettings = Activator.CreateInstance(v.Key.PropertyType) as ICLIArguments;
				if (concreteSettings == null)
					throw new InvalidOperationException($"Unable to instance {this._TargetType.Name}: is not a valid {typeof(ICLIArguments).Name}");
				concreteSettings.Examples.ToList().ForEach(x => verb.AddExample(x));

				this._Verbs.Add(verb);
			}

			foreach (var opt in this._OptionDefinitions)
			{
				if (this._Options.Exists(x => x.Code == opt.Value.Code))
					throw new InvalidOperationException($"Option {opt.Value.Code} already analyzed");
				var option = Option.FromAttribute(opt.Value).OnTargetProperty(opt.Key.PropertyType);
				this._Options.Add(option);
			}
		}
		/// <summary>
		/// BUilds the model to map CLI usage
		/// </summary>
		/// <returns></returns>
		public CLIUsageModel BuildModel()
		{
			var model = new CLIUsageModel(this._TargetType, this._Options, this._Verbs);
			foreach (var opt in this._OptionDefinitions)
				model.MapOption(opt.Value.LongCode, opt.Key);
			foreach (var opt in this._VerbDefinitions)
				model.MapVerb(opt.Value.Name, opt.Key);

			// get examples
			ICLIArguments concreteSettings = Activator.CreateInstance(this._TargetType) as ICLIArguments;
			if (concreteSettings == null)
				throw new InvalidOperationException($"Unable to instance {this._TargetType.Name}: is not a valid {typeof(ICLIArguments).Name}");
			concreteSettings.Examples.ToList().ForEach(x => model.AddExample(x));

			return model;
		}

	}
}
