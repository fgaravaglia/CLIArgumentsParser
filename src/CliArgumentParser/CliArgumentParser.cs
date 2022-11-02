using CliArgumentParser.ErrorManagement;
using CliArgumentParser.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser
{
    /// <summary>
    /// Class to parse CLI command
    /// </summary>
    /// <example>
    /// Simple usage:
    /// <code>
    ///     var factory = new CommandFactory().RegisterCommand &lt; ScanCommand &gt; ("scan");
    ///     int exitResult = factory.InstanceFromFactory()
    ///         .UsingDefaultErrorManagement()
    ///         .ParseTheseArguments(new string[]
    ///         {
    ///             "scan",
    ///             @"-folder=C:\projects\AnsiblePOC\uc.q8f.ua.sharedlibraries\Odin",
    ///             "-match-exp=.csproj",
    ///             "-to=csv"
    ///         })
    ///         .CaseWhen &lt; ScanCommand &gt;(x => ExploreTheTree(x))
    ///         .Return();
    /// </code>
    /// </example>
    public class CliArgumentParser
    {
        const int PARSER_ERROR = 1;
        const int INTERNAL_ERROR = 2;

        readonly ICommandFactory _CmdFactory;
        readonly Dictionary<Type, Func<Exception, int>> _ErrorCallbackRegistry;

        CliCommand? _ParsedCommand;
        Exception? _OccurredError;
        int _ReturnValue;

        /// <summary>
        /// True if any exception occurred; False otherwise
        /// </summary>
        public bool HasError { get { return this._OccurredError != null; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public CliArgumentParser(ICommandFactory factory)
        {
            this._CmdFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            this._ErrorCallbackRegistry = new Dictionary<Type, Func<Exception, int>>();
            this._ParsedCommand = null;
            this._OccurredError = null;
            this._ReturnValue = 0;
        }

        #region Private methods

        void Initialize()
        {
            this._ParsedCommand = null;
            this._ReturnValue = 0;
            this._OccurredError = null;
        }

        bool ManageError(Exception ex)
        {
            this._OccurredError = ex;
            if (this._ErrorCallbackRegistry.ContainsKey(ex.GetType()))
            {
                this._ReturnValue = this._ErrorCallbackRegistry[ex.GetType()].Invoke(ex);
                return true;
            }

            if (this._ErrorCallbackRegistry.ContainsKey(typeof(Exception)))
            {
                this._ReturnValue = this._ErrorCallbackRegistry[typeof(Exception)].Invoke(ex);
                return true;
            }

            return false;
        }

        private static void PrintException(string caption, Exception ex, bool showStackTrace)
        {
            Console.WriteLine($"*********************| {caption} |*********************");
            Console.WriteLine(ex.Message);
            if (!showStackTrace)
                return;

            Console.WriteLine("Stack trace:");
            Console.WriteLine(ex.StackTrace);
        }

        #endregion

        /// <summary>
        /// Defines the callback to log exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback">callback to execute, that returns the proper exit code value</param>
        public void SetErrorCallback<T>(Func<T, int> callback) where T : Exception
        {
            if (callback is null)
                throw new ArgumentNullException(nameof(callback));
            if (this._ErrorCallbackRegistry.ContainsKey(typeof(Exception)))
                throw new ArgumentException($"Unable to add Error Callback: for type {typeof(T)} it is already registered", nameof(callback));
            this._ErrorCallbackRegistry.Add(typeof(T), x =>
            {
                return callback.Invoke((T)x);
            });
        }
        /// <summary>
        /// Applies default error callbacks
        /// </summary>
        public void EnableErrorManagement()
        {
            this.SetErrorCallback<ArgumentException>(x =>
            {
                PrintException("--FATAL ERROR--", x, false);
                return PARSER_ERROR;
            });
            this.SetErrorCallback<WrongOptionUsageException>(x =>
            {
                PrintException("--FATAL ERROR--", x, false);
                return PARSER_ERROR;
            });
            this.SetErrorCallback<UnknownVerbException>(x =>
            {
                PrintException("--FATAL ERROR--", x, false);
                return PARSER_ERROR;
            });
            this.SetErrorCallback<ApplicationException>(x =>
            {
                PrintException("--FATAL ERROR--", x, false);
                return PARSER_ERROR;
            });

            this.SetErrorCallback<Exception>(x =>
            {
                PrintException("Unexpected Error", x, true);
                return INTERNAL_ERROR;
            });
        }
        /// <summary>
        /// Parses the cli arguments
        /// </summary>
        /// <param name="args"></param>
        public void ParseArguments(string[] args)
        {
            Initialize();

            try
            {
                if (args.Length == 0)
                    throw new CliArgumentParserException($"Required Arguments!");

                if (args[0].StartsWith("-"))
                    throw new UnknownVerbException(args[0]);

                // instance a new command
                CliCommand cmd = this._CmdFactory.GetCommand(args[0]);

                // apply default values if needed
                cmd.SetDefaultValues();

                var options = args.Skip(1).ToList();
                int optIndex = 0;
                while (optIndex != -1)
                {
                    var opt = options[optIndex];
                    if (!opt.StartsWith("-"))
                        throw new WrongOptionUsageException(cmd.Verb, opt);
                    cmd.ParseArgument(opt.Split('='));
                    // move forward
                    optIndex++;
                    if (optIndex == options.Count)
                        optIndex = -1;
                }

                // Validate the parsed command
                var validator = new CliCommandValidator();
                validator.AssertIsValid(cmd);

                this._ParsedCommand = cmd;
            }
            catch (CliArgumentParserException parserEx)
            {
                // manage specific types of errors raised from Parser
                var exType = parserEx.GetType();
                if (exType == typeof(WrongOptionUsageException)
                    || exType == typeof(UnknownVerbException))
                {
                    bool managed = ManageError(parserEx);
                    PrintUsage();
                    if (!managed)
                        throw;
                }
                else
                {
                    Console.WriteLine("Missing Input!");
                    PrintUsage();
                    this._OccurredError = parserEx;
                }
            }
            catch (Exception ex)
            {
                bool managed = ManageError(ex);
                PrintUsage();
                if (!managed)
                    throw;
            }
        }
        /// <summary>
        /// Register proper callback inc as of specific command parsed succesfully
        /// </summary>
        /// <typeparam name="Tcmd">command to handle</typeparam>
        /// <param name="callback"></param>
        public void RunCallbackFor<Tcmd>(Action<Tcmd> callback) where Tcmd : CliCommand
        {
            if (HasError)
                return;

            try
            {
                if (callback is null)
                    throw new ArgumentNullException((nameof(callback)));
                if (this._ParsedCommand is null)
                    throw new ApplicationException($"Parsed Failed: no known command found");

                if (this._ParsedCommand.GetType() != typeof(Tcmd))
                    return;

                callback.Invoke((Tcmd)this._ParsedCommand);
            }
            catch (Exception ex)
            {
                if (!ManageError(ex))
                    throw;
            }
        }
        /// <summary>
        /// Gets the return value of parsing operation
        /// </summary>
        /// <returns></returns>
        public int Return()
        {
            return this._ReturnValue;
        }
        /// <summary>
        /// Print on Console the usage instructions
        /// </summary>
        public void PrintUsage()
        {
            var commands = this._CmdFactory.GetAvailableCommands();
            Console.WriteLine("********************************************");
            Console.WriteLine("\t\t Usage:");
            Console.WriteLine("********************************************");
            foreach (var cmd in commands)
            {
                Console.WriteLine(new CommandStringConverter(cmd).UsageToString());
            }
        }
    }
}
