using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using WhitmanEnterpriseMVC.DatabaseModel;
namespace WhitmanEnterpriseMVC.HelperClass
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

        public static int GetHealthLevel(vincontrolwholesaleinventory tmp)
        {
            int hasImage = String.IsNullOrEmpty(tmp.ThumbnailImageURL) ? 1 : 0;

            int hasDescription = String.IsNullOrEmpty(tmp.Descriptions) ? 1 : 0;

            int hasPrice = String.IsNullOrEmpty(tmp.SalePrice) ? 1 : 0;

            if (!String.IsNullOrEmpty(tmp.ThumbnailImageURL) && !String.IsNullOrEmpty(tmp.DefaultImageUrl))
            {
                hasImage = tmp.ThumbnailImageURL.Equals(tmp.DefaultImageUrl) ? 1 : 0;
            }
            
            int valueReturn = hasImage + hasDescription + hasPrice;

            return valueReturn;
        }



        public static int GetHealthLevel(whitmanenterprisedealershipinventory tmp)
        {
            int hasImage = String.IsNullOrEmpty(tmp.ThumbnailImageURL) ? 1 : 0;

            int hasDescription = String.IsNullOrEmpty(tmp.Descriptions) ? 1 : 0;

            int hasPrice = String.IsNullOrEmpty(tmp.SalePrice) ? 1 : 0;

            if (!String.IsNullOrEmpty(tmp.ThumbnailImageURL) && !String.IsNullOrEmpty(tmp.DefaultImageUrl))
            {
                hasImage = tmp.ThumbnailImageURL.Equals(tmp.DefaultImageUrl) ? 1 : 0;
            }
            

            int valueReturn = hasImage + hasDescription + hasPrice;

            return valueReturn;
        }


        public static int GetHealthLevelSoldOut(whitmanenterprisedealershipinventorysoldout tmp)
        {
            int hasImage = String.IsNullOrEmpty(tmp.CarImageUrl) ? 1 : 0;

            int hasDescription = String.IsNullOrEmpty(tmp.Descriptions) ? 1 : 0;

            int hasPrice = String.IsNullOrEmpty(tmp.SalePrice) ? 1 : 0;

            if (!String.IsNullOrEmpty(tmp.ThumbnailImageURL) && !String.IsNullOrEmpty(tmp.DefaultImageUrl))
            {
                hasImage = tmp.ThumbnailImageURL.Equals(tmp.DefaultImageUrl) ? 1 : 0;
            }
            

            int valueReturn = hasImage + hasDescription + hasPrice;

            return valueReturn;
        }
    }
}
