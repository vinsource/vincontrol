using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Helper
{
    public sealed class LogicHelper
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


        public static int GetHealthLevel(Inventory tmp)
        {
            int hasImage = String.IsNullOrEmpty(tmp.ThumbnailUrl) ? 1 : 0;

            int hasDescription = String.IsNullOrEmpty(tmp.Descriptions) ? 1 : 0;

            int hasPrice = tmp.SalePrice.GetValueOrDefault() > 0 ? 0 : 1;

            if (!String.IsNullOrEmpty(tmp.ThumbnailUrl) && !String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage))
            {
                hasImage = tmp.ThumbnailUrl.Equals(tmp.Vehicle.DefaultStockImage) ? 1 : 0;
            }


            int valueReturn = hasImage + hasDescription + hasPrice;

            return valueReturn;
        }


        public static int GetHealthLevelSoldOut(SoldoutInventory tmp)
        {
            int hasImage = String.IsNullOrEmpty(tmp.PhotoUrl) ? 1 : 0;

            int hasDescription = String.IsNullOrEmpty(tmp.Descriptions) ? 1 : 0;

            int hasPrice = tmp.SalePrice.GetValueOrDefault() > 0 ? 1 : 0;

            if (!String.IsNullOrEmpty(tmp.ThumbnailUrl) && !String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage))
            {
                hasImage = tmp.ThumbnailUrl.Equals(tmp.Vehicle.DefaultStockImage) ? 1 : 0;
            }


            int valueReturn = hasImage + hasDescription + hasPrice;

            return valueReturn;
        }
    }
}
