<%@ Page Title="Choose dealer for user" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.DealerGroupViewModel>" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.HelperClass" %>
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
    <script>

</script>
    <h3>
        Choose Dealerships</h3>
    <div id="container">
        <input type="checkbox" name="SelectAll" id="SelectAllCheckbox" />Select All
        <table style="width: 100%;">
            <%foreach (var tmp in Model.DealerList)
              {%>
            <tr>
                <td>
                    <input type="checkbox" name="DealerSelect" value="<%=tmp.DealershipId %>" /><%=tmp.DealershipName %>
                </td>
                <%if (tmp.DealershipId == Model.DealershipGroupDefaultLogin)
                  {%>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" checked="checked" />Default
                </td>
                <% }
                  else
                  { %>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" />Default
                </td>
                <% } %>
            </tr>
            <% } %>
            <%
                if (Model.DealershipGroupName != null)
                {
            %>
            <tr>
                <td>
                    <%=Model.DealershipGroupName %>
                </td>
                <%
                    if (Constanst.DealerGroupConst == Model.DefaultLoginForUser)
                    {%>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%=Constanst.DealerGroupConst %>"
                        checked="checked" />Default
                </td>
                <% }
                    else
                    { %>
                <td>
                    <input type="radio" name="DefaultLogin" value="<%=Constanst.DealerGroupConst%>" />Default
                </td>
                <% }%>
            </tr>
            <%
                }%>
        </table>
    </div>
    <p>
        <input type="button" name="Add User" value="Add User" id="btnAddUserInGroup" />
    </p>
</body>
<script type="text/javascript">



    $(document).ready(function () {

        $("#SelectAllCheckbox").click(function (event) {

            var checks = $('input[name="DealerSelect"]');

            if ($(this).is(':checked')) {

                for (i in checks) {
                    checks[i].checked = true;

                }

            } else {

                for (i in checks) {

                    checks[i].checked = false;

                }
            }
        });


        $("#btnAddUserInGroup").click(function (event) {

            if ($("#CusErrorIn").length > 0) {
                $("#CusErrorIn").remove();
            }
            var checks = $('input[name="DealerSelect"]');
            var radios = $('input[name="DefaultLogin"]');
            var flag = false;
            var selectDealer = "";

            var defaultLogin = "";
            for (i in radios) {
                if (radios[i].checked && radios[i].value != undefined) {
                    defaultLogin = radios[i].value;
                }
            }

            for (i in checks) {
                if (checks[i].checked && checks[i].value != undefined) {
                    flag = true;
                    selectDealer += checks[i].value + ",";
                }
            }

            if (flag == false) {
                var errorString = "<strong id='CusErrorIn'><font color='Red'  size='2' >There are some following errors : <br><ul>";
                errorString += "<li>Please choose at least one dealership</li>";
                errorString += "</ul></font></strong>";
                $("#container").prepend(errorString);



            } else {

                if ((selectDealer.indexOf(defaultLogin) == -1)&&(defaultLogin.toString()!='<%=Constanst.DealerGroupConst%>')) {
                    errorString = "<strong id='CusErrorIn'><font color='Red'  size='1.5' >There are some following errors : <br><ul>";
                    errorString += "<li>Default dealership is not in the list of dealeships you choose for this user</li>";
                    errorString += "</ul></font></strong>";
                    $("#container").prepend(errorString);
                } else {
                    selectDealer = selectDealer.substring(0, selectDealer.length - 1);

                    $.post('<%= Url.Content("~/Admin/AddUser") %>', { Name: $("#NewName").val(), UserName: $("#NewUsername").val(), Password: $("#NewPassword").val(), Email: $("#NewEmail").val(), CellPhone: $("#NewPhone").val(), UserLevel: $("#UserLevel").val(), DealerList: selectDealer, DefaultLogin: defaultLogin }, function (user) {
                        console.log(user);
                        $('#elementID').addClass('hideLoader');
                        var actionUrl;
                        if (user.SessionTimeOut == "TimeOut") {
                            alert("Your session is timed out. Please login back again");
                            actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        } else if (user.IsUserExist == "Exist") {
                            alert("Username existed. Please choose another username");

                        } else {
                            actionUrl = '<%= Url.Action("AdminSecurityLanding", "Admin",new { LandingPage = "PLACEHOLDER" } ) %>';
                            actionUrl = actionUrl.replace('PLACEHOLDER', 'UserRights');
                            window.location = actionUrl;
                        }
                    });
                }


            }


        });
    });


</script>
</html>
