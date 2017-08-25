using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using WhitmanEnterpriseMVC.Models;

namespace System.Web.Mvc.Html
{
    public static class DataGridHelper
    {
        public static string DataGrid<T>(this HtmlHelper helper)
        {
            return DataGrid<T>(helper, null, null);
        }

        public static string DataGrid<T>(this HtmlHelper helper, object data)
        {
            return DataGrid<T>(helper, data, null);
        }


        public static string DataGrid<T>(this HtmlHelper helper, object data, string[] columns)
        {
            // Get items
            var items = (IEnumerable<T>)data;
            if (items == null)
                items = (IEnumerable<T>)helper.ViewData.Model;


            // Get column names
            if (columns == null)
                columns = typeof(T).GetProperties().Select(p => p.Name).ToArray();

            // Create HtmlTextWriter
            var writer = new HtmlTextWriter(new StringWriter());

            // Open table tag
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            // Render table header
            writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            RenderHeader(helper, writer, columns);
            writer.RenderEndTag();

            // Render table body
            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
            foreach (var item in items)
                RenderRow<T>(helper, writer, columns, item);
            writer.RenderEndTag();

            // Close table tag
            writer.RenderEndTag();

            // Return the string
            return writer.InnerWriter.ToString();
        }


        private static void RenderHeader(HtmlHelper helper, HtmlTextWriter writer, string[] columns)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (var columnName in columns)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(helper.Encode(columnName));
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }


        private static void RenderRow<T>(HtmlHelper helper, HtmlTextWriter writer, string[] columns, T item)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (var columnName in columns)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                var value = typeof(T).GetProperty(columnName).GetValue(item, null) ?? String.Empty;
                writer.Write(helper.Encode(value.ToString()));
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

    }
}

