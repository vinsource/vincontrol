<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>

<%: Html.DropDownList("SingleMake", SessionHandler.ManheimYearMakeModelList.Make.ToSelectItemList(m => m.Value, m => m.Text, false), "Make", new { @class = "year", style = "width:60px;" })%>