using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vincontrol.VinSell.Extensions
{
    public static class SelectDictionaryBuilder
    {
        public static IDictionary<int, string> ToSelectDictionary<TSource>(this IEnumerable<TSource> enumerable,
            Func<TSource, int> keySelector, Func<TSource, string> elementSelector, bool addEmptyField = true, string overWriteEmptyFieldName = null)
        {
            var dict = new Dictionary<int, string>();
            string empty = overWriteEmptyFieldName ?? string.Empty;
            if (addEmptyField)
                dict.Add(-1, empty);

            foreach (var item in enumerable)
            {
                dict.Add(keySelector(item), elementSelector(item));
            }
            return dict;
        }

        public static IEnumerable<SelectListItem> ToSelectItemList<TSource>(this IEnumerable<TSource> enumerable,
            Func<TSource, int> keySelector, Func<TSource, string> elementSelector, bool addEmptyField = true, string overWriteEmptyFieldName = null)
        {
            var dict = new List<SelectListItem>();
            string empty = overWriteEmptyFieldName ?? string.Empty;
            if (addEmptyField)
                dict.Add(new SelectListItem() { Selected = false, Text = empty, Value = "0" });

            foreach (var item in enumerable)
            {
                dict.Add(new SelectListItem() { Selected = false, Value = keySelector(item).ToString(), Text = elementSelector(item) });
            }
            return dict;
        }

        public static IEnumerable<SelectListItem> ToSelectItemList<TSource>(this IEnumerable<TSource> enumerable,
            Func<TSource, string> keySelector, Func<TSource, string> elementSelector, bool addEmptyField = true, string overWriteEmptyFieldName = null)
        {
            var dict = new List<SelectListItem>();
            string empty = overWriteEmptyFieldName ?? string.Empty;
            if (addEmptyField)
                dict.Add(new SelectListItem() { Selected = false, Text = empty, Value = "0" });

            foreach (var item in enumerable)
            {
                dict.Add(new SelectListItem() { Selected = false, Value = keySelector(item).ToString(), Text = elementSelector(item) });
            }
            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="keySelector"></param>
        /// <param name="elementSelector"></param>
        /// <param name="selectedElementSelector"></param>
        /// <param name="addEmptyField"></param>
        /// <param name="overWriteEmptyFieldName"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectItemList<TSource>(this IEnumerable<TSource> enumerable,
            Func<TSource, int> keySelector,
            Func<TSource, string> elementSelector,
            Func<TSource, bool> selectedElementSelector,
            bool addEmptyField = true,
            string overWriteEmptyFieldName = null)
        {
            var dict = new List<SelectListItem>();
            var empty = overWriteEmptyFieldName ?? string.Empty;
            if (addEmptyField)
                dict.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = empty,
                    Value = "0"
                });

            foreach (var item in enumerable)
            {
                dict.Add(new SelectListItem()
                {
                    Selected = selectedElementSelector(item),
                    Value = keySelector(item).ToString(),
                    Text = elementSelector(item)
                });
            }
            return dict;
        }

        public static IEnumerable<SelectListItem> ToSelectItemList<TSource>(this IEnumerable<TSource> enumerable,
            Func<TSource, string> keySelector,
            Func<TSource, string> elementSelector,
            Func<TSource, bool> selectedElementSelector,
            bool addEmptyField = true,
            string overWriteEmptyFieldName = null)
        {
            var dict = new List<SelectListItem>();
            var empty = overWriteEmptyFieldName ?? string.Empty;
            if (addEmptyField)
                dict.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = empty,
                    Value = "0"
                });

            foreach (var item in enumerable)
            {
                dict.Add(new SelectListItem()
                {
                    Selected = selectedElementSelector(item),
                    Value = keySelector(item).ToString(),
                    Text = elementSelector(item)
                });
            }
            return dict;
        }
    }
}