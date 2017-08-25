<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<%: Html.DropDownList("Model", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Model", new { @class = "model", style = "width:70px;" })%>