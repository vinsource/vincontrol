using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;
using Vincontrol.Web.Models;

namespace System.Web.Mvc.Html
{
    public static class CheckBoxGroupHelper
    {

        public static string CusDropDown(this HtmlHelper htmlHelper,string id,string className, string selected,
                                                  IEnumerable<SelectListItem> selectList, string vaidationEngine, string errorMessage)
        {
            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("id", id);
            _params.Add("className", className);
            _params.Add("Items", selectList);
            _params.Add("selected", selected);
            _params.Add("vaidationEngine", vaidationEngine);
            _params.Add("errorMessage", errorMessage);

            string content = string.Empty;
            content = NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\CustomControl\CusDropDown.htm", _params).ToString();
            return content;
        }


        public static string CusDropDown(this HtmlHelper htmlHelper, string id, string className, string selected,
                                                  IEnumerable<ExtendedSelectListItem> selectList, string vaidationEngine, string errorMessage)
        {
            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("id", id);
            _params.Add("className", className);
            _params.Add("Items", selectList);
            _params.Add("selected", selected);
            _params.Add("vaidationEngine", vaidationEngine);
            _params.Add("errorMessage", errorMessage);

            string content = string.Empty;
            content = NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\CustomControl\CusDropDown.htm", _params).ToString();
            return content;
        }

        public static string CheckBoxGroupPackage(this HtmlHelper htmlHelper, string name,
                                                  IEnumerable<SelectListItem> selectList)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                foreach (SelectListItem item in selectList)
                {
                    listItemBuilder.AppendLine("<li>");
                    listItemBuilder.AppendLine(ListItemToCheckBox(item, name) + ListItemToLabel(item, name) + "<br />");
                    listItemBuilder.AppendLine("</li>");

                }
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No packages available for this model" +
                                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }

        public static string CheckBoxGroupPackage(this HtmlHelper htmlHelper, string name,
                                                IEnumerable<SelectDetailListItem> selectList)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                foreach (SelectDetailListItem item in selectList)
                {
                    listItemBuilder.AppendLine("<li>");
                    listItemBuilder.AppendLine(ListItemToCheckBox(item, name) + ListItemToLabel(item, name) + "<br />");
                    listItemBuilder.AppendLine("</li>");

                }
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No packages available for this model" +
                                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }


        public static string CheckBoxGroupOption(this HtmlHelper htmlHelper, string name,
                                                 IEnumerable<SelectListItem> selectList)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                foreach (SelectListItem item in selectList)
                {
                    listItemBuilder.AppendLine("<li>");
                    listItemBuilder.AppendLine(ListItemToCheckBox(item, name) + ListItemToLabel(item, name) + "<br />");
                    listItemBuilder.AppendLine("</li>");

                }
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No options available for this model" +
                                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }

        public static string CheckBoxGroupOption(this HtmlHelper htmlHelper, string name,
                                           IEnumerable<SelectDetailListItem> selectList)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                foreach (SelectDetailListItem item in selectList)
                {
                    listItemBuilder.AppendLine("<li>");
                    listItemBuilder.AppendLine(ListItemToCheckBox(item, name) + ListItemToLabel(item, name) + "<br />");
                    listItemBuilder.AppendLine("</li>");

                }
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No options available for this model" +
                                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }

        public static string CheckBoxGroupPackage(this HtmlHelper htmlHelper, string name,
                                                 IEnumerable<SelectListItem> selectList, List<string> existCarPackages)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                foreach (SelectListItem item in selectList)
                {
                    listItemBuilder.AppendLine("<li>");
                    listItemBuilder.AppendLine(ListItemToCheckBox(item, name, existCarPackages) +
                                               ListItemToLabel(item, name) + "<br />");
                    listItemBuilder.AppendLine("</li>");

                }
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                //listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No packages available for this model" +
                //                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }

        public static string CheckBoxGroupPackage(this HtmlHelper htmlHelper, string name,
                                                  IEnumerable<SelectDetailListItem> selectList, List<string> existCarPackages)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No packages available for this model" +
                                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }

        public static string CheckBoxGroupOption(this HtmlHelper htmlHelper, string name,
                                               IEnumerable<SelectDetailListItem> selectList, List<string> existCarOptions)
        {
            var listItemBuilder = new StringBuilder();
            if (selectList != null && selectList.Any())
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Parameter must be specified.", "name");
                }
                listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
                listItemBuilder.AppendLine("</div>");
            }
            else
            {
                listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
                listItemBuilder.AppendLine("<ul class=\"options\">");
                listItemBuilder.AppendLine("<li><label for=\"" + name + "\">" + "No options available for this model" +
                                           "</label></li>");
                listItemBuilder.AppendLine("</ul>");
                listItemBuilder.AppendLine("</div>");
            }
            return listItemBuilder.ToString();
        }
        public static string CheckBoxGroupPackageByYear(this HtmlHelper htmlHelper, string name)
        {
            var listItemBuilder = new StringBuilder();

            listItemBuilder.AppendLine("<div id=\"Packages\" name=\"Packages\">");
            listItemBuilder.AppendLine("<ul class=\"options\">");
           listItemBuilder.AppendLine("</ul>");
            listItemBuilder.AppendLine("</div>");

            return listItemBuilder.ToString();
        }


        public static string CheckBoxGroupOptionByYear(this HtmlHelper htmlHelper, string name)
        {
            var listItemBuilder = new StringBuilder();
            listItemBuilder.AppendLine("<div id=\"Options\" name=\"Options\">");
            listItemBuilder.AppendLine("<ul class=\"options\">");
            listItemBuilder.AppendLine("</ul>");
            listItemBuilder.AppendLine("</div>");

            return listItemBuilder.ToString();
        }

        internal static string ListItemToCheckBox(SelectListItem item, string name)
        {
            var builder = new TagBuilder("input");

            builder.Attributes["class"] = "z-index";
            builder.Attributes["type"] = "checkbox";
            builder.Attributes["name"] = name;
            builder.Attributes["onclick"] = "javascript:changeMSRP(this)";

            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }

            return builder.ToString(TagRenderMode.Normal);
        }

        internal static string ListItemToCheckBox(SelectListItem item, string name, List<string> existList)
        {
            var builder = new TagBuilder("input");
           
            builder.Attributes["class"] = "z-index";
            builder.Attributes["type"] = "checkbox";
            builder.Attributes["name"] = name;
            builder.Attributes["onclick"] = "javascript:changeMSRP(this)";

            if (existList != null && CheckOptionExist(existList, item.Value.Split('$')[0].Trim()))
                builder.Attributes["checked"] = "checked";


            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }

            return builder.ToString(TagRenderMode.Normal);
        }

        internal static string ListItemToCheckBox(SelectDetailListItem item, string name)
        {
            var builder = new TagBuilder("input");

            builder.Attributes["class"] = "z-index";
            builder.Attributes["type"] = "checkbox";
            builder.Attributes["name"] = name;
            builder.Attributes["title"] = item.Description;
            builder.Attributes["onclick"] = "javascript:changeMSRP(this)";

            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }

            return builder.ToString(TagRenderMode.Normal);
        }

        internal static string ListItemToCheckBox(SelectDetailListItem item, string name, List<string> existList)
        {
            var builder = new TagBuilder("input");

            builder.Attributes["class"] = "z-index";
            builder.Attributes["type"] = "checkbox";
            builder.Attributes["name"] = name;
            builder.Attributes["title"] = item.Description;
            builder.Attributes["onclick"] = "javascript:changeMSRP(this)";

            if (existList != null && CheckOptionExist(existList, item.Value.Split('$')[0].Trim()))
                builder.Attributes["checked"] = "checked";


            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }

            return builder.ToString(TagRenderMode.Normal);
        }

        internal static string ListItemToLabel(SelectDetailListItem item, string name)
        {
            var builder = new TagBuilder("label")
            {
                InnerHtml = item.Text
            };
            builder.Attributes["title"] = item.Description;
            return builder.ToString(TagRenderMode.Normal);
        }


        internal static string ListItemToLabel(SelectListItem item, string name)
        {
            var builder = new TagBuilder("label")
                              {
                                  InnerHtml = item.Text
                              };
            builder.Attributes["for"] = item.Value;
            return builder.ToString(TagRenderMode.Normal);
        }

        private static bool CheckOptionExist(IEnumerable<string> existList, string vtmp)
        {
            return existList.Any(tmp => vtmp.Contains(tmp.Trim()));
        }
    }
}
