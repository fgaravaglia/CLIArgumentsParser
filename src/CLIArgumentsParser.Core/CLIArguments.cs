using System.Collections.Generic;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	///  marker for cli arguments
	/// </summary>
	public interface ICLIArguments
	{
		/// <summary>
		/// Gets the list of examples for the usages
		/// </summary>
		IEnumerable<CLIUsageExample> Examples { get; }
		/// <summary>
		/// Get the string representation as CLI argument
		/// </summary>
		/// <returns></returns>
		string ToCLIString();
	}
	/// <summary>
	/// base class to store settings parsed by CLIArgumentsParser assembly
	/// </summary>
	public class CLIArguments : ICLIArguments
	{
		/// <summary>
		/// Gets the list of examples for the usages
		/// </summary>
		public IEnumerable<CLIUsageExample> Examples { get { return BuildExamples(); } }
		/// <summary>
		/// Get the string representation as CLI argument
		/// </summary>
		/// <returns></returns>
		public virtual string ToCLIString()
		{
			return "";
		}
		/// <summary>
		/// Generates the hard-coded list of examples for current argument
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerable<CLIUsageExample> BuildExamples()
		{
			return new List<CLIUsageExample>();
		}
	}
}
