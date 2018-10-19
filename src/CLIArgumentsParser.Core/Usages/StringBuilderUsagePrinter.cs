using System.Text;

namespace CLIArgumentsParser.Core.Usages
{
	/// <summary>
	/// Concrete implementation of UsagePrinter, based on string buidler
	/// </summary>
	public class StringBuilderUsagePrinter : UsagePrinter
	{
		readonly StringBuilder _MessageBuilder;

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="model"></param>
		public StringBuilderUsagePrinter(CLIUsageModel model) : base(model)
		{
			this._MessageBuilder = new StringBuilder();
		}

		/// <summary>
		/// Print a line
		/// </summary>
		/// <param name="line"></param>
		protected override void PrintLine(string line)
		{
			this._MessageBuilder.AppendLine(line);
		}
		/// <summary>
		/// Get the text
		/// </summary>
		/// <returns></returns>
		public string GetText()
		{
			return this._MessageBuilder.ToString();
		}
	}
}
