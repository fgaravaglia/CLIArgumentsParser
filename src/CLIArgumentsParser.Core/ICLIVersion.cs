using System;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// wrapper for version of CLI
	/// </summary>
	public interface ICLIVersion
	{
		/// <summary>
		/// MAjor version of CLI
		/// </summary>
		int MajorNumber { get; }
		/// <summary>
		/// Minor version of CLI
		/// </summary>
		int MinorNumber { get; }
		/// <summary>
		/// Date of release
		/// </summary>
		DateTime ReleasedOn { get; }
	}
}
