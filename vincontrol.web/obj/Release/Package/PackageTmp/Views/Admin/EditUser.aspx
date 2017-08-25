<%@ Page Title="Choose dealer for user" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.AccountManagement.DealerGroupViewModel>" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Default Dealership Login</title>
    <%--   <link href="<%=Url.Content("~/Content/Admin.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />--%>
    <link href="<%=Url.Content("~/Content/VinControl/popup.css")%>" rel="stylesheet"
        type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.js"></script>
    <%--<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>--%>
    <!-- the jScrollPane script -->
</head>
<body>
    <div class="kpi_print_title">
        Choose Dealerships
    </div>
    <div id="editUser">
        <table style="width: 100%;">
            <% if (Model.DealerList != null)
               {
                   foreach (var tmp in Model.DealerList)
                   { %>
            <tr>
                <% if (tmp.DealershipId == Model.DefaultLoginForUser)
                   { %>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" checked="checked" /><%= tmp.DealershipName %>
                </td>
                <% }
                   else
                   { %>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" /><%= tmp.DealershipName %>
                </td>
                <% } %>
            </tr>
            <% }
               }
               else
               {%>
            <tr>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= SessionHandler.Dealer.DealershipId %>"
                        checked="checked" /><%= SessionHandler.Dealer.DealershipName  %>
                </td>
            </tr>
            <%      
               } %>
            <%if (Model.DealershipGroupName != null)
              {%>
            <tr>
                <% if (Constanst.DealerGroupConst == Model.DefaultLoginForUser)
                   {%>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= Constanst.DealerGroupConst %>"
                        checked="checked" /><%=Model.DealershipGroupName %>
                </td>
                <% }
                   else
                   { %>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%=Constanst.DealerGroupConst %>" /><%=Model.DealershipGroupName %>
                </td>
                <% }%>
            </tr>
            <%}%>
        </table>
    </div>
    <p>
        <%=Html.Hidden("HiddenUserName",ViewData["UserName"]) %>
        <%--<input type="button" name="Update" value="Update" id="btnUpdateUserInGroup" />--%>
        <div class="status_btn_holder">
            <div class="btns_shadow" id="btnUpdateUserInGroup" name="Update" value="Update">
                Update</div>
        </div>
    </p>
</body>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnUpdateUserInGroup").click(function (event) {
            var radios = $('input[name="DefaultLogin"]');
            var flag = false;
            var defaultLogin = "";
            for (i in radios) {
                if (radios[i].checked && radios[i].value != undefined) {
                    defaultLogin = radios[i].value;
                }
            }
           
            $.post('<%= Url.Content("~/Admin/UpdateDefaultLogin") %>', { UserName: $("#HiddenUserName").val(), DefaultLogin: defaultLogin }, function (user) {
      
                $('#elementID').addClass('hideLoader');
                var actionUrl;
                if (user.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session is timed out. Please login back again");
                    actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                } else {
                    parent.$.fancybox.close();
             
                }
            });
        });
    });


</script>
</html>
