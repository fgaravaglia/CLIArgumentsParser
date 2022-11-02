using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser
{
    /// <summary>
    /// Abstraction fo Component that generate well formatted command, availble for the CLI
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Registers a specific Type of Command for the given <paramref name="verb"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="verb"></param>
        /// <returns></returns>
        ICommandFactory RegisterCommand<T>(string verb) where T : CliCommand;
        /// <summary>
        /// instances a new command of given verb <paramref name="verb"/>
        /// </summary>
        /// <param name="verb"></param>
        /// <returns></returns>
        CliCommand GetCommand(string verb);
        /// <summary>
        /// GEts the lsit of available commands
        /// </summary>
        /// <returns></returns>
        IEnumerable<CliCommand> GetAvailableCommands();
    }
    /// <summary>
    /// concrete implementation for Command Factory
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        readonly Dictionary<string, Type> _Registry;

        public CommandFactory()
        {
            this._Registry = new Dictionary<string, Type>();
        }

        /// <inheritdoc />
        public ICommandFactory RegisterCommand<T>(string verb) where T : CliCommand
        {
            if (String.IsNullOrEmpty(verb))
                throw new ArgumentNullException(nameof(verb));

            if (this._Registry.ContainsKey(verb))
                throw new ArgumentException($"Unable to add command {verb}: item already added", nameof(verb));

            this._Registry.Add(verb, typeof(T));

            return this;
        }
        /// <inheritdoc />
        public CliCommand GetCommand(string verb)
        {
            if (!this._Registry.ContainsKey(verb))
                throw new UnknownVerbException(verb);

            var targetType = this._Registry[verb];
            var instance = Activator.CreateInstance(targetType);
            if(instance is null)
                throw new ReflectionTypeLoadException(new Type[]{ targetType}, null);
            ((CliCommand)instance).SetDefaultValues();
            return (CliCommand)instance;
        }
        /// <inheritdoc />
        public IEnumerable<CliCommand> GetAvailableCommands()
        {
            var commands = new List<CliCommand>();

            foreach (var item in this._Registry)
            {
                commands.Add(GetCommand(item.Key));
            }

            return commands;
        }
    }
}
