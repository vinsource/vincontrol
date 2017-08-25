using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace vincontrol.Data
{
    public class LinqExtendedHelper
    {
        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
    Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }


        public static string RemoveSpecialCharactersForNumber(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                if (input.Contains(".")) input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
            } return "0";
        }

    }
}
