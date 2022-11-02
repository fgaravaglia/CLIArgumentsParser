using System;
using System.Collections.Generic;
using System.Text;
using CliArgumentParser.Decorator;

namespace CliArgumentParser
{
    /// <summary>
    /// Components to convert argument to string
    /// </summary>
    internal interface ICommandStringConverter
    {
        /// <summary>
        /// Convert the actual command into string, with values passed from cli
        /// </summary>
        /// <returns></returns>
        string StatementToString();
        /// <summary>
        /// Convert the available options to string
        /// </summary>
        /// <returns></returns>
        string OptionsToString();
        /// <summary>
        /// Convert the given command into string representation, to provide a definition of the given cmd
        /// </summary>
        /// <returns></returns>
        string DefinitionToString();
    }

    internal class CommandStringConverter : ICommandStringConverter
    {
        readonly CliCommand _Cmd;

        public CommandStringConverter(CliCommand cmd)
        {
            this._Cmd = cmd ?? throw new ArgumentNullException(nameof(cmd));
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public string DefinitionToString()
        {
            StringBuilder message = new StringBuilder();
            message.Append(_Cmd.Verb).Append("\t").Append(this._Cmd.Description).AppendLine();
            message.AppendLine(this.OptionsToString());
            return message.ToString();
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public string OptionsToString()
        {
            StringBuilder printer = new StringBuilder();
            printer.AppendLine("Options:");

            var attributes = _Cmd.GetOptionAttribute();
            foreach (var attribute in attributes)
            {
                string notes = attribute != null ? attribute.UsageNotes : "";
                printer.Append(attribute?.Name).Append("\t\t").Append(attribute?.Description).Append("; " + notes).AppendLine();

            }
            return printer.ToString();
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public string StatementToString()
        {
            StringBuilder message = new StringBuilder();
            message.Append(this._Cmd.Verb + " ");
            foreach (var arg in this._Cmd.Arguments)
            {
                message.Append(arg.Name).Append("=" + arg.Value);
                message.Append(" ");
            }
            return message.ToString();
        }

        public string UsageToString()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("---------------------------------------------------------------");
            message.AppendLine(this.DefinitionToString());
            foreach (var example in this._Cmd.Examples())
            {
                message.AppendLine(example.Example);
            }
            message.AppendLine("---------------------------------------------------------------");
            return message.ToString();
        }
    }
}
