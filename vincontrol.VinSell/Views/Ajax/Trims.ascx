<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<%: Html.DropDownList("Trim", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Trim", new { @class = "trim", style = "width:70px;" })%>