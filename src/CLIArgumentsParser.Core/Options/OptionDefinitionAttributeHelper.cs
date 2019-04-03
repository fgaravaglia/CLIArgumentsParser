using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLIArgumentsParser.Core.Options
{
    internal static class OptionDefinitionAttributeHelper
    {
        internal static Dictionary<PropertyInfo, OptionDefinitionAttribute> ExtractPropertiesMarkedWithOptionAttribute(Type targetType)
        {
            var dictionary = new Dictionary<PropertyInfo, OptionDefinitionAttribute>();
            var properties = targetType.GetProperties().ToList();
            properties = properties.Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(OptionDefinitionAttribute) || a.AttributeType.BaseType == typeof(OptionDefinitionAttribute))).ToList();
            properties.ForEach(p =>
            {
                var attribute = p.GetCustomAttribute<OptionDefinitionAttribute>();
                dictionary.Add(p, attribute);
            });
            return dictionary;
        }
    }
}
