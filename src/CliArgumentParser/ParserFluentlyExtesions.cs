using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser
{
    /// <summary>
    /// Fluently extensions for parser
    /// </summary>
    public static class ParserFluentlyExtesions
    {
        /// <summary>
        /// Instances the parser give the comamnd Factory
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static CliArgumentParser InstanceFromFactory(this ICommandFactory factory)
        {
            var parser = new CliArgumentParser(factory);
            return parser;
        }
        /// <summary>
        /// Sets the callback in case of error
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        public static CliArgumentParser OnError(this CliArgumentParser parser, Action<Exception> errorCallback)
        {
            parser.OnError(errorCallback);
            return parser;
        }
        /// <summary>
        /// Sets the callback in case of error of given type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static CliArgumentParser OnThisTypeOfError<T>(this CliArgumentParser parser, Func<T, int> callback) where T : Exception
        {
            parser.SetErrorCallback<T>(callback);
            return parser;
        }
        /// <summary>
        /// Enables the default error management that lets you to print on console the error
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static CliArgumentParser UsingDefaultErrorManagement(this CliArgumentParser parser)
        {
            parser.EnableErrorManagement();
            return parser;
        }
        /// <summary>
        /// Parses the arguments
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CliArgumentParser ParseTheseArguments(this CliArgumentParser parser, string[] args)
        {
            parser.ParseArguments(args);
            return parser;
        }
        /// <summary>
        /// sets the parser to run a scpecific callback if succesfully parsed command is of type T
        /// </summary>
        /// <typeparam name="Tcmd"></typeparam>
        /// <param name="parser"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static CliArgumentParser CaseWhen<Tcmd>(this CliArgumentParser parser, Action<Tcmd> callback) where Tcmd : CliCommand
        {
            parser.RunCallbackFor<Tcmd>(callback);
            return parser;
        }

       
    }
}
