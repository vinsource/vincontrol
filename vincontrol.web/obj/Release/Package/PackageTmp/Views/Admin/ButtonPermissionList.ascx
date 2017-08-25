<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AdminViewModel>" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<style type="text/css">
    .disable
    {
        display: none;
    }
</style>

<% if (Model.ButtonPermissions != null && Model.ButtonPermissions.Any()) %>
<% { %>
    <% foreach (var group in Model.ButtonPermissions.Where(i => i.GroupId != Constanst.RoleType.Master)) %>
    <% { %>
            <div class="admin_ur_permissions_header">
                <%= group.GroupName %>
            </div>

        <% foreach (var button in group.Buttons) %>
        <% { %>
            <% if (SessionHandler.CanViewBucketJumpReport == null || (SessionHandler.CanViewBucketJumpReport != null &&
                                           SessionHandler.CanViewBucketJumpReport.Value &&
                                           button.ButtonName.Equals(Constanst.ProfileButton.BucketJumpTracking))
                                            || (SessionHandler.CanViewBucketJumpReport != null
                                            && !button.ButtonName.Equals(Constanst.ProfileButton.BucketJumpTracking))
                                            ) %>
            <% { %>
                <% if (group.GroupId == Constanst.RoleType.Manager && button.ButtonName.Equals(Constanst.ProfileButton.EditProfile)) %>
                <% { %>
                        <label>
                            <input type="checkbox" id="chkButtonPermisison_<%= group.GroupId %>_<%= button.ButtonId %>" <%= button.CanSee ? "checked" : "" %> disabled="disabled" />
                            <%= button.ButtonName %>
                        </label>
                <% } %>
                <% else %>
                <% { %>
                        <label>
                            <input type="checkbox" id="chkButtonPermisison_<%= group.GroupId %>_<%= button.ButtonId %>"
                                <%= button.CanSee ? "checked" : "" %> />
                            <%= button.ButtonName %>
                        </label>
                <% } %>
                        
            <% } %>
        <% } %>
    <% } %>
<% } %>