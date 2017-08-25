<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<WhitmanEnterpriseMVC.Models.TradeinCommentViewModel>>" %>
<table>
    <tr>
     <th>
            Name
        </th>
        <th>
            City
        </th>
        <th>
            State
        </th>
        <th>
            Content
        </th>
        <th>
        </th>
    </tr>
    <% foreach (var item in Model)
       { %>
    <tr>
     <td>
            
            <input type="text" id="txtName_<%= item.ID %>" value="<%= item.Name %>" />
        </td>
        <td>
            <%--<%= Html.TextBox(item.City) %>--%>
            <input type="text" id="txtCity_<%= item.ID %>" value="<%= item.City %>" />
        </td>
        <td>
            <input type="text" id="txtState_<%= item.ID %>" value="<%= item.State %>" />
        </td>
        <td>
            <textarea rows="3" name="Content" id="txtContent_<%= item.ID %>"  cols="20"><%= item.Content %></textarea>
        </td>
        <td>
            <input type="button" id="btnSave_<%= item.ID %>" name="save-comment" value="Save" />
            <input type="button" id="btnDelete_<%= item.ID %>" name="delete-comment" value="Delete" />
        </td>
    </tr>
    <% } %>
</table>
