<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<vincontrol.Application.ViewModels.AccountManagement.UserRoleViewModel>>" %>
<%@ Import Namespace="vincontrol.Constant" %>

<%@ Import Namespace="Vincontrol.Web.Handlers" %>


<% if (Model.Any())
   {
       foreach (var user in Model)
       { %>
<div class="admin_userrights_items" id="<%= user.Username %>">
    <div class="adu_collumn">
        <input type="text" class="readonly" readonly="readonly" value="<%= user.Name %>" />
    </div>
    <div class="adu_collumn">
        <input type="text" class="readonly" readonly="readonly" value="<%= user.Username %>" />
    </div>
    <div class="adu_collumn">
        <form name="SavePasswordForm" class="PasswordForm">
            <input type="password" name="pass" value="<%= user.Password %>" data-validation-engine="validate[required,funcCall[checkPass]]"
                                        data-errormessage-value-missing="Password is required!" id="pass_<%= user.UserId %>" />
        </form>
    </div>
    <div class="adu_collumn">
        <form name="SaveEmailForm" class="EmailForm">
            <input type="text" name="email" value="<%= user.Email %>" id="email_<%= user.UserId %>" data-validation-engine="validate[required,funcCall[CheckEmail]]"
                                        data-errormessage-value-missing="Email is required!" />
        </form>
    </div>
    <div class="adu_collumn">
        <form name="SaveCellphoneForm" class="CellphoneForm">
            <input type="text" name="phone" value="<%= user.Cellphone %>" id="phone_<%= user.UserId %>" maxlength="15" 
                data-validation-engine="validate[required]" data-errormessage-value-missing="Cellphone is required!"/>
        </form>
    </div>
    <div class="adu_collumn">
        <select name="userlevel" id="role_<%= user.UserId %>">
            <option value="<%= Constanst.RoleType.Manager %>" <%= (user.RoleId == Constanst.RoleType.Manager) ? "selected=\"selected\"" : String.Empty %>>Manager</option>
            <option value="<%= Constanst.RoleType.Employee %>" <%= (user.RoleId == Constanst.RoleType.Employee) ? "selected=\"selected\"" : String.Empty %>>Employee</option>
            <option value="<%= Constanst.RoleType.Admin %>" <%= (user.RoleId == Constanst.RoleType.Admin) ? "selected=\"selected\"" : String.Empty %>>Admin</option>
        </select>
    </div>

    <% if (SessionHandler.DealerGroup != null && SessionHandler.DealerGroup.DealerList.Count() > 1)
       { %>

    <div>
        <input class="adu_collumn adu_addUser_btn" type="button" name="btnEdit" value="Edit" id="editUser_<%= user.UserId %>" />
    </div>
    <% } %>
    <div>
        <input class="adu_collumn adu_addUser_btn" type="button"
            name="btnDelete" value="Delete" id="deleteUser_<%= user.UserId %>" />

    </div>
</div>
<% }
   }
   else
   {%>
<div style="padding-left: 300px; margin-top: 10px;"> Threre are no users which match your search criteria.</div>
    
   <%} %>
<div style="clear: both">
</div>
