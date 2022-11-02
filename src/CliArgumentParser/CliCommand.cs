using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CliArgumentParser.Decorator;


namespace CliArgumentParser
{
    /// <summary>
    /// Abstraction of a given Command, exposed ouside the library to extend the command definition
    /// </summary>
    public interface ICliCommand
    {
        /// <summary>
        /// Gets the Name of the command
        /// </summary>
        string Verb { get; }
        /// <summary>
        /// Generic description on objective of specific command
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Sets the devault values for options
        /// </summary>
        void SetDefaultValues();
        /// <summary>
        /// Updates the option value
        /// </summary>
        /// <remarks>It throws exception if option value does not exist</remarks>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void UpdateArgumentValue(string name, string value);
        /// <summary>
        /// Given the cli arguments, it parses the values
        /// </summary>
        /// <param name="tokens"></param>
        void ParseArgument(string[] tokens);
        /// <summary>
        /// Casts the generic base class to specific one
        /// </summary>
        /// <typeparam name="T">concrete command to cast to</typeparam>
        /// <returns>casted type</returns>
        T As<T>() where T : CliCommand;
        /// <summary>
        /// Provides the list of exmaples for the given command
        /// </summary>
        /// <returns></returns>
        List<CliCommandExample> Examples();
    }

    /// <summary>
    /// Base implementation to be inherited for all commands
    /// </summary>
    public abstract class CliCommand : ICliCommand
    {
        /// <inheritdoc></inheritdoc>
        public string Verb { get; set; }
        /// <inheritdoc></inheritdoc>
        public string Description { get; set; }
        /// <inheritdoc></inheritdoc>
        public List<CliArg> Arguments { get; set; }

        protected CliCommand(string verb, string descr)
        {
            this.Verb = verb;
            this.Description = descr;
            this.Arguments = new List<CliArg>();
        }

        #region Protected Methods

        protected void AddOrUpdateArgument<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda, string value) where TSource : CliCommand
        {
            var option = this.As<TSource>().GetOptionAttributeFromProperty<TSource,TProperty>(propertyLambda);
            this.AddOrUpdateArgument(option.Name, option.Description, value);
        }

        protected string GetArgumentValue<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda) where TSource : CliCommand
        {
            var option = this.As<TSource>().GetOptionAttributeFromProperty<TSource, TProperty>(propertyLambda);
            var arg = this.Arguments.SingleOrDefault(x => x.Name == option.Name);
            return arg is null ? string.Empty : arg.Value;
        }

        #endregion

        #region Private Methods

        void AddOrUpdateArgument(string name, string description, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description), "Description is mandatory");

            var existing = this.Arguments.SingleOrDefault(x => x.Name == name);
            if (existing is null)
                this.Arguments.Add(new CliArg(name, value, description));
            else
            {
                existing.UpdateValue(value);
            }
        }

        #endregion

        /// <inheritdoc></inheritdoc>
        public virtual void SetDefaultValues()
        {
           
        }
        /// <inheritdoc></inheritdoc>
        public void UpdateArgumentValue(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var existing = this.Arguments.SingleOrDefault(x => x.Name == name);
            if (existing is null)
                throw new ArgumentException( $"Argument not found!", nameof(name));
            else
            {
                existing.UpdateValue(value);
            }
        }
        /// <inheritdoc></inheritdoc>
        public abstract void ParseArgument(string[] tokens);
        /// <inheritdoc></inheritdoc>
        public T As<T>() where T : CliCommand
        {
            return (T)this;
        }
        /// <inheritdoc></inheritdoc>
        public abstract List<CliCommandExample> Examples();

    }

}
