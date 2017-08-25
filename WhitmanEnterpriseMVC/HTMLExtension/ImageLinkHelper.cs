using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;

namespace WhitmanEnterpriseMVC.HTMLExtension
{
    public static class ImageLinkHelper
    {
        public static string ImageLink(this HtmlHelper helper, string actionName, string imageUrl, string alternateText)
        {
            return ImageLink(helper, actionName, imageUrl, alternateText, null, null, null);
        }

        public static string ImageLink(this HtmlHelper helper, string actionName, string imageUrl, string alternateText, object routeValues)
        {
            return ImageLink(helper, actionName, imageUrl, alternateText, routeValues, null, null);
        }

        public static string ImageLink(this HtmlHelper helper, string actionName, string imageUrl, string alternateText, object routeValues, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, routeValues);

            // Create link
            var linkTagBuilder = new TagBuilder("a");
            linkTagBuilder.MergeAttribute("href", url);
            linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            // Create image

            var imageTagBuilder = new TagBuilder("img");
            if (!String.IsNullOrEmpty(imageUrl))
                imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
            imageTagBuilder.MergeAttribute("alt", urlHelper.Encode(alternateText));
            imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

            // Add image to link
            linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

            return linkTagBuilder.ToString();
        }

        public static string ImageLink(this HtmlHelper helper, string actionName, string controllerName, string imageUrl, string alternateText, object routeValues, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, controllerName, routeValues);

            // Create link
            var linkTagBuilder = new TagBuilder("a");
            linkTagBuilder.MergeAttribute("href", url);
            linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            // Create image

            var imageTagBuilder = new TagBuilder("img");
            if (!String.IsNullOrEmpty(imageUrl))
                imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
            imageTagBuilder.MergeAttribute("alt", urlHelper.Encode(alternateText));
            imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

            // Add image to link
            linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

            return linkTagBuilder.ToString();
        }

        public static string ImageLinkToInventoryFromMarket(this HtmlHelper helper,string actionName, string imageUrl, string alternateText, object routeValues, object linkHtmlAttributes, object imageHtmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, routeValues);
            url = url.Replace("Market", "Inventory");
            // Create link
            var linkTagBuilder = new TagBuilder("a");
            linkTagBuilder.MergeAttribute("href", url);
            linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            // Create image

            var imageTagBuilder = new TagBuilder("img");
            if (!String.IsNullOrEmpty(imageUrl))
                imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
            imageTagBuilder.MergeAttribute("alt", urlHelper.Encode(alternateText));
            imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

            // Add image to link
            linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

            return linkTagBuilder.ToString();
        }

        public static string BlankImageLink(this HtmlHelper helper, string actionName, string text, object routeValues)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, routeValues);

            // Create link
            var builder = new StringBuilder();
            builder.Append("<a href=\"" + url + "\">" + text + "</a>" + Environment.NewLine);

            
            return builder.ToString();
        }

        public static string BlankImageLink(this HtmlHelper helper, string actionName, string controllerName, string text, object routeValues)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, controllerName, routeValues);

            // Create link
            var builder = new StringBuilder();
            builder.Append("<a href=\"" + url + "\">" + text + "</a>" + Environment.NewLine);


            return builder.ToString();
        }
    }
}
