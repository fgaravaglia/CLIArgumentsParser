using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// Info about CLI
	/// </summary>
	public interface ICLIInfo
	{
		/// <summary>
		/// Version of CLI
		/// </summary>
		ICLIVersion Version { get; }
		/// <summary>
		/// ALias for CLI
		/// </summary>
		string Alias { get;  }
	}
}
