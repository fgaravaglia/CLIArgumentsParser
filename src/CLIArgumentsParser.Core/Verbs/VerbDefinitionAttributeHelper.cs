using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLIArgumentsParser.Core.Verbs
{
	internal static class VerbDefinitionAttributeHelper
	{
		internal static Dictionary<PropertyInfo, VerbDefinitionAttribute> ExtractPropertiesMarkedWithVerbAttribute(Type targetType)
		{
			var dictionary = new Dictionary<PropertyInfo, VerbDefinitionAttribute>();
			var properties = targetType.GetProperties().ToList();
			properties = properties.Where(x =>
			{
				// get attributes put on class definition
				VerbDefinitionAttribute classAttribute = x.PropertyType.GetCustomAttribute<VerbDefinitionAttribute>();
				if (classAttribute == null)
					return false;

				return true;
			}).ToList();
			properties.ForEach(p =>
			{
				VerbDefinitionAttribute attribute = p.PropertyType.GetCustomAttribute<VerbDefinitionAttribute>();
				dictionary.Add(p, attribute);
			});
			return dictionary;
		}
	}
}
