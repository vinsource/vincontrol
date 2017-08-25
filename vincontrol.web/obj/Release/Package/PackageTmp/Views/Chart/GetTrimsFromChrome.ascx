<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<select id="market_search_trim" data-validation-placeholder="0" data-validation-engine="validate[funcCall[checkSelectedTrim]]">
<% foreach (var item in Model) %>
<% { %>
    <option value="<%=item.Value %>"><%=item.Text %></option>
<% } %>
</select>