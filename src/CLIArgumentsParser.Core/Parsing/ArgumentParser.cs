using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIArgumentsParser.Core.Parsing
{
	internal abstract class ModelParser<Tmodel>
	{
		readonly protected TokenGenerator _Tokenizer;
		readonly protected Type _TargetType;

		protected Tmodel _Model;

		protected ModelParser(Tmodel model, TokenGenerator tokenizer, Type targetType)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));
			this._Model = model;
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

			return Parse(tokens);
		}
		/// <summary>
		/// PArses tokens to get the right value
		/// </summary>
		/// <param name="tokens"></param>
		/// <returns></returns>
		public object Parse(IEnumerable<Token> tokens)
		{
			if (tokens == null)
				throw new ArgumentException(nameof(tokens));
			// validate names
			foreach (var token in tokens)
			{
				string error;
				if (!IsKeyValid(token.Name, out error))
					throw new InvalidCLIArgumentException($"Invalid Argument {token.AsNaturalString()}: {error}", token.Name);
			}

			// obtain value
			var returnValue = ParseFromTokens(tokens);

			//check consistencey
			if (returnValue != null && returnValue.GetType() != this._TargetType)
				throw new InvalidOperationException($"Unable to convert {returnValue.GetType().FullName} into { this._TargetType.FullName} for argument {String.Join(" ", tokens.Select(x => x.AsNaturalString()))}");
			return returnValue;
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
	}
}
