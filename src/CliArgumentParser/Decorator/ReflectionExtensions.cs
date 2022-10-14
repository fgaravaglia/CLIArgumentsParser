using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CliArgumentParser.Decorator
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyName<TSource, TField>(this TSource src, Expression<Func<TSource, TField>> Field)
        {
            if (object.Equals(Field, null))
            {
                throw new NullReferenceException("Field is required");
            }

            MemberExpression expr;
            if (Field.Body is MemberExpression)
            {
                expr = (MemberExpression)Field.Body;
            }
            else if (Field.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)Field.Body).Operand;
            }
            else
            {
                const string Format = "Expression '{0}' not supported.";
                string message = string.Format(Format, Field);

                throw new ArgumentException(message, "Field");
            }

            return expr.Member.Name;
        }

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>( this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression? member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));

            PropertyInfo? propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));

            if (type != propInfo.ReflectedType && propInfo.ReflectedType != null && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format("Expression '{0}' refers to a property that is not from type {1}.", propertyLambda.ToString(), type));

            return propInfo;
        }
    }
}
