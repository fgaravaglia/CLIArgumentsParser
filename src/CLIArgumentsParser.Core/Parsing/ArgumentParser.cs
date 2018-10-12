using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIArgumentsParser.Core.Parsing
{
	internal abstract class ArgumentParser<Tattribute, Tmodel> where Tattribute : Attribute
	{
		readonly protected Tattribute _Attribute;
		readonly protected TokenGenerator _Tokenizer;
		readonly protected Type _TargetType;

		protected Tmodel _Model;

		protected ArgumentParser(Tattribute attribute, TokenGenerator tokenizer, Type targetType)
		{
			this._Attribute = attribute ?? throw new ArgumentNullException(nameof(Attribute));
			this._Tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
			this._TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
		}

		/// <summary>
		/// Parse argument (string) to obtain the value
		/// </summary>
		/// <param name="arg"></param>
		/// <returns>the parsed value</returns>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public object Parse(string arg)
		{
			if (string.IsNullOrWhiteSpace(arg)) throw new ArgumentException(nameof(arg));

			// get tokens 
			var tokens = this._Tokenizer.TokenizeThisString(arg).ToList();
			// validate names
			foreach (var token in tokens)
			{
				string error;
				if (!IsKeyValid(token.Name, out error))
					throw new InvalidCLIArgumentException($"Invalid Argument {arg}: {error}", token.Name);
			}

			// obtain value
			var returnValue = ParseFromTokens(tokens);

			//check consistencey
			if (returnValue != null && returnValue.GetType() != this._TargetType)
				throw new InvalidOperationException($"Unable to convert {returnValue.GetType().FullName} into { this._TargetType.FullName} for argument {arg}");
			return returnValue;
		}
		/// <summary>
		/// return the proper entity model for the current argument
		/// </summary>
		/// <returns>the model corresponding to target attribute</returns>
		public object MapToModel()
		{
			return FromAttribute(this._Attribute);
		}

		/// <summary>
		/// True if key is valid compared to attribute definition
		/// </summary>
		protected abstract bool IsKeyValid(string key, out string errorMessage);
		/// <summary>
		/// Executes the parse for the given key/arguments
		/// </summary>
		/// <returns></returns>
		protected abstract object ParseFromTokens(IEnumerable<Token> targetTokens);

		/// <summary>
		/// maps the definition of argument coming out from attribute into model
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		protected abstract Tmodel FromAttribute(Tattribute attribute);
	}
}
