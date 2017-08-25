<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>

<%: Html.DropDownList("SingleModel", SessionHandler.ManheimYearMakeModelList.Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Model", new { @class = "year", style = "width:65px;" })%>