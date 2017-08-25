<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<%: Html.DropDownList("Make", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Make", new { @class = "make", style = "width:70px;" })%>