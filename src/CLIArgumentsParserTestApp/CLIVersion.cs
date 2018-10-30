using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLIArgumentsParser.Core;

namespace CLIArgumentsParserTestApp
{
	class CLIVersion : ICLIVersion
	{
		public int MajorNumber => 1;

		public int MinorNumber => 0;

		public DateTime ReleasedOn => new DateTime(2018, 10, 31).Date;
	}

	class CLIInfo : ICLIInfo
	{
		public ICLIVersion Version => new CLIVersion();

		public string Alias => "CLIArgumentsParserTestApp.exe";
	}
}
