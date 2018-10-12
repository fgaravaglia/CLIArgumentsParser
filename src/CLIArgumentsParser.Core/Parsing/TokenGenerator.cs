using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIArgumentsParser.Core.Parsing
{
	/// <summary>
	/// Component that generates the list of token given a input string
	/// </summary>
	public class TokenGenerator
	{
		readonly List<string> _TokenIdentifiers;
		readonly string _Splitter;

		/// <summary>
		/// True to add the identifier in token name; FALSE otherwise
		/// </summary>
		bool _HasToAddIdentifierToTokenNames;

		/// <summary>
		/// Default Constructor
		/// </summary>
		public TokenGenerator(IEnumerable<string> tokenIdentifiers)
		{
			if (tokenIdentifiers == null)
				throw new ArgumentNullException(nameof(tokenIdentifiers));
			if (tokenIdentifiers.Count() == 0)
				throw new ArgumentNullException(nameof(tokenIdentifiers));

			this._TokenIdentifiers = new List<string>();
			tokenIdentifiers.ToList().ForEach(x => this._TokenIdentifiers.Add(x));

			this._Splitter = "=";
			this._HasToAddIdentifierToTokenNames = false;
		}
		/// <summary>
		/// instance a token separator with default settings
		/// </summary>
		/// <returns></returns>
		public static TokenGenerator Default()
		{
			return new TokenGenerator(new List<string>() { "--" });
		}
		/// <summary>
		/// Adds the identifiers to token names
		/// </summary>
		/// <returns></returns>
		public TokenGenerator AddingIdentifierToTokenNames()
		{
			this._HasToAddIdentifierToTokenNames = true;
			return this;
		}
		/// <summary>
		/// generates the tokens
		/// </summary>
		/// <param name="text">stirng to split</param>
		/// <returns></returns>
		public IEnumerable<Token> TokenizeThisString(string text)
		{
			List<Token> tokens = new List<Token>();
			if (String.IsNullOrEmpty(text))
				return tokens;

			var parts = text.Split(this._TokenIdentifiers.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (var part in parts)
			{
				// get parts for token
				var tokenParts = part.Split(new string[] { this._Splitter }, StringSplitOptions.None);
				var tokenName = tokenParts[0];
				string tokenValue = tokenParts.Length == 2 ? tokenParts[1].Trim() : "";
				// check consistency
				if (tokenParts.Length != 1 && tokenParts.Length != 2)
					throw new InvalidOperationException($"Unable to generate token for {part}");

				// force to add token identifier
				if (this._HasToAddIdentifierToTokenNames)
				{
					var index = text.IndexOf(part);
					string before = text.Substring(0, index);
					tokenName = before.Split(' ').Last() + tokenName;
				}

				// add token
				tokens.Add(Token.New(tokenName, tokenValue));
			}


			return tokens;
		}
	}
}
