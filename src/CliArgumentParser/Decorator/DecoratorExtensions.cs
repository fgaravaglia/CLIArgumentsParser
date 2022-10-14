using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CliArgumentParser.Decorator
{
    public static class DecoratorExtensions
    {
        internal static OptionAttribute GetOptionAttributeFromProperty<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (propertyLambda is null)
                throw new ArgumentNullException(nameof(propertyLambda));

            var property = source.GetPropertyInfo<TSource, TProperty>(propertyLambda);
            var decorators = DecoratorExtensions.ExtractDecoratorsFromProperty(property);
            if (!decorators.Any())
                throw new ApplicationException($"Unable to find Decorators for property {property.Name}");
            if (decorators.Count() > 1)
                throw new ApplicationException($"Found more than one Decorator for property {property.Name}");
            return decorators.First();
        }

        internal static OptionAttribute? GetOptionAttributeFromProperty<TSource>(this TSource source, string name) where TSource : CliCommand
        {
            var properties = source.GetType().GetProperties()
                                    .Where(p => DecoratorExtensions.ExtractDecoratorsFromProperty(p).Any()).ToList();

            // select the one with target name
            var targetProperty = properties.SingleOrDefault(p => p.ExtractDecoratorsFromProperty().First().Name == name);

            // return the targetAttribute
            if(targetProperty is null)
                return null;
            return targetProperty.ExtractDecoratorsFromProperty().First();
        }

        internal static IEnumerable<OptionAttribute> ExtractDecoratorsFromProperty(this PropertyInfo property)
        {
            return property.GetCustomAttributes(true)
                            .Where(x => x.GetType().IsSubclassOf(typeof(OptionAttribute))
                                        || x.GetType() == typeof(OptionAttribute))
                            .Select(x => (OptionAttribute)x).ToList();
        }
        /// <summary>
        /// Extracts the lilst of <see cref="OptionAttribute"/> that decorates the properties of a concrete implementation of <see cref="CliCommand"/>
        /// </summary>
        /// <typeparam name="TSource">concrete instance of command</typeparam>
        /// <param name="source"></param>
        /// <returns>list of attributes</returns>
        internal static IEnumerable<OptionAttribute> GetOptionAttribute<TSource>(this TSource source) where TSource : CliCommand
        {
            var properties = source.GetType().GetProperties()
                                    .Where(p => DecoratorExtensions.ExtractDecoratorsFromProperty(p).Any()).ToList();

            return properties.Select(p => DecoratorExtensions.ExtractDecoratorsFromProperty(p).First()).ToList();
        }

    }
}
