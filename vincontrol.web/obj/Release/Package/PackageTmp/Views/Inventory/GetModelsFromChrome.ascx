<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
    
<select id="vinbrochure_select_model">
<% foreach (var item in Model)
   {%>
    <option value="<%=item.Value %>"><%=item.Text %></option>
  <% } %>
</select>