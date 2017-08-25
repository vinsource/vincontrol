<%@ Page Title="Choose dealer for user" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AdminViewModel>" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.HelperClass" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="background: #111 !important;">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Default Dealership Login</title>
    <link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/Admin.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.js"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <!-- the jScrollPane script -->
 
</head>
<body>
    <h3> Rebate</h3>
    <div id="editUser" style=" width: 300px">
        <table style="width: 100%;">
            <tr>
                <td >Year</td>
                <td >Year</td>
            </tr>
            <tr>
                <td >Make</td>
                <td >Make</td>
            </tr>
            <tr>
                <td >Model</td>
                <td >Model</td>
            </tr>
            <tr>
                <td >Trim</td>
                <td >Trim</td>
            </tr>
        </table>
    </div>
    <p>
        <%=Html.Hidden("HiddenUserName",ViewData["UserName"]) %>
        <input type="button" name="Update" value="Update" id="btnUpdateUserInGroup" />
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
            console.log($("#HiddenUserName").val());

            $.post('<%= Url.Content("~/Admin/UpdateDefaultLogin") %>', { UserName: $("#HiddenUserName").val(), DefaultLogin: defaultLogin }, function (user) {
                console.log(user);
                $('#elementID').addClass('hideLoader');
                var actionUrl;
                if (user.SessionTimeOut == "TimeOut") {
                    alert("Your session is timed out. Please login back again");
                    actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                } else {
                    actionUrl = '<%= Url.Action("AdminSecurityLanding", "Admin",new { LandingPage = "PLACEHOLDER" } ) %>';
                    actionUrl = actionUrl.replace('PLACEHOLDER', 'UserRights');
                    window.location = actionUrl;
                }
            });
        });
    });


</script>
</html>
