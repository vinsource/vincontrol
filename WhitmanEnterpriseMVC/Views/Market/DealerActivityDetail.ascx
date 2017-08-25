<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<WhitmanEnterpriseMVC.Models.DealershipActivityViewModel>>" %>

<% foreach (var item in Model){%>
<tr class="aDetailActivity" style="font-size:0.9em;cursor:pointer;" id="aDetailActivity_<%= item.Id %>">
    <td align="left"><%= item.Title %></td>
    <td align="left"><%= item.UserStamp %></td>
    <td align="right"><%= item.DateStamp.ToString("MM/dd/yyyy hh:mm:ss tt") %></td>
</tr>
<% } %>