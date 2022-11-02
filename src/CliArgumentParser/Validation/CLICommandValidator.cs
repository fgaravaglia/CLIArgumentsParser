using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CliArgumentParser.Validation
{
    public interface ICliCommandValidator
    {
        /// <summary>
        /// Esure the command is Valid; otherwhise it thorws exception
        /// </summary>
        /// <param name="cmd"></param>
        void AssertIsValid(CliCommand cmd);
    }

    public class CliCommandValidator : ICliCommandValidator
    {
        public CliCommandValidator()
        {

        }

        static String GetPropertyValueAsString(CliCommand cmd, PropertyInfo prop)
        {
            if(cmd == null)
                throw new ArgumentNullException(nameof(cmd));
            if (prop == null)
                throw new ArgumentNullException(nameof(prop));  

            // get value
            object? propValue = prop.GetValue(cmd);
            string? propValueString = "";
            if (propValue != null)
                propValueString = propValue.ToString();
            
            return propValueString == null ? "" : propValueString;
        }

        public void AssertIsValid(CliCommand cmd)
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));


            // extract the marked properties
            var optionProperties = cmd.GetType().GetProperties()
                                        .Where(p => p.ExtractDecoratorsFromProperty().Any())
                                        .ToList();
            foreach (var prop in optionProperties)
            {
                // get value
                string propValueString = GetPropertyValueAsString(cmd, prop);

                // get decorators
                var attributes = prop.ExtractDecoratorsFromProperty();

                foreach (var a in attributes)
                {
                    if (a.GetType() != typeof(OptionAttribute))
                        throw new WrongOptionUsageException(cmd.Verb, a.GetType());
                        
                    OptionAttribute attribute = a;
                    if (attribute.IsMandatory && String.IsNullOrEmpty(propValueString))
                        throw new WrongOptionUsageException(cmd.Verb, attribute.Name, "Option is Mandatory");

                    if (attribute.ValidValues.Any() && !String.IsNullOrEmpty(propValueString) && !attribute.ValidValues.Contains(propValueString))
                        throw new WrongOptionUsageException(cmd.Verb, attribute.Name, $"Value {propValueString} is not valid for List of values");
                }

            }
        }


    }
}
