using System;

namespace CLIArgumentsParser.Core.Parsing
{
	internal abstract class ArgumentParser<Tattribute, Tmodel> where Tattribute : Attribute
	{
		readonly protected Tattribute _Attribute;

		protected ArgumentParser(Tattribute attribute)
		{
			this._Attribute = attribute ?? throw new ArgumentNullException(nameof(Attribute));
		}

		/// <summary>
		/// Parse argument (string) to obtain the value
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="targetReturnType"></param>
		/// <returns>the parsed value</returns>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public object Parse(string arg, Type targetReturnType)
		{
			if (string.IsNullOrWhiteSpace(arg)) throw new ArgumentException(nameof(arg));
			if (targetReturnType == null)
				throw new ArgumentNullException(nameof(targetReturnType));

			var parts = arg.Split(' ');
			var key = parts[0].Trim().Remove(0, 1);

			string error;
			if (!IsKeyValid(key, out error))
				throw new InvalidCLIArgumentException($"Invalid Argument {arg}: {error}", key);

			var keyArguments = parts.Length > 1 ? parts[1] : null;
			var returnValue = ParseFromKey(key, keyArguments, targetReturnType);

			//check consistencey
			if (returnValue != null && returnValue.GetType() != targetReturnType)
				throw new InvalidOperationException($"Unable to convert {returnValue.GetType().FullName} into {targetReturnType.FullName} for argument {arg}");
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
		/// <param name="key"></param>
		/// <param name="keyArguments"></param>
		/// <param name="targetReturnType"></param>
		/// <returns></returns>
		protected abstract object ParseFromKey(string key, string keyArguments, Type targetReturnType);
		/// <summary>
		/// maps the definition of argument coming out from attribute into model
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		protected abstract Tmodel FromAttribute(Tattribute attribute);
	}
}
