<%@ Page Title="Choose dealer for user" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.DealerGroupViewModel>" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.HelperClass" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="background: #111 !important;">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Default Dealership Login</title>
    <link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.js"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <!-- the jScrollPane script -->
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            background: #222;
            color: #ddd;
        }
        #container
        {
            background: #333;
            padding: 1em;
        }
        h3, ul
        {
            margin: 0;
        }
        input[type="text"]
        {
            width: 200px;
        }
        span.label
        {
            display: block;
            width: 150px;
            float: left;
            clear: right;
        }
        input[type="submit"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 0.8em;
            font-weight: bold;
            padding: .5em;
            margin-top: -2em;
        }
        input[type="button"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 0.8em;
            font-weight: bold;
            padding: .5em;
            margin-top: -2em;
        }
        #fancybox-content
        {
            background: #111 !important;
            text-align: center;
        }
        .short
        {
            width: 50px !important;
        }
        #Checkbox1
        {
            height: 0px;
        }
    </style>
</head>
<body style="background: #111 !important;">
    <h3>
        Choose Dealerships</h3>
    <div id="container">
        <table style="width: 100%;">
            <%foreach (var tmp in Model.DealerList)
              {%>
            <tr>
                <%if (tmp.DealershipId == Model.DefaultLoginForUser)
                  {%>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" checked="checked" /><%=tmp.DealershipName %>
                </td>
                <% }
                  else
                  { %>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" /><%=tmp.DealershipName %>
                </td>
                <% } %>
            </tr>
            <% } %>
            <%
                if (Model.DealershipGroupName != null)
                {
            %>
            <tr>
                <%
                           if (Constanst.DealerGroupConst == Model.DefaultLoginForUser)
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
            <%
                       }
               
            %>
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
