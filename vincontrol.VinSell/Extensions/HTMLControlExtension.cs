using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using vincontrol.DomainObject;

namespace System.Web.Mvc.Html
{
    public static class HTMLControlExtension
    {
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>());
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, string optionLabel)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), optionLabel);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), optionLabel, htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, object htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList != null ? selectList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value, Selected = i.Selected }).AsEnumerable() : new List<SelectListItem>(), optionLabel, htmlAttributes);
        }
    }
}