<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.LogOnViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Vin Control</title>
<link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript" src="http://code.jquery.com/jquery-latest.js"></script>
<style type="text/css">
html {background: none; overflow: hidden;}
body {background: none; margin: 0 auto !important; padding-top: 175px;}
#wrapper {height: 300px; width: 600px; margin: 0 auto;}
#loginForm {margin-left: 3.6em;}
#loginForm input {float: right; margin-right: 6.1em;}
#loginForm input[type="text"],input[type="password"] {width: 100px;}
.hideLoader {display: none;}
</style>
</head>
<body>
<div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajaxloadingindicator.gif")%>" alt="" />
</div>
<div id="wrapper">
                <img src=<%=Url.Content("~/Content/Images/logo-vincontrol.png")%> />
			   		<%Html.EnableClientValidation(); %>
		<% Html.BeginForm("LogOn", "Account", FormMethod.Post, new { id="loginForm", name="loginForm"}); %>
                                 
            <%=Html.EditorFor(x=>x.UserName) %>
              <font color="red">
            <%=Html.ValidationMessageFor(x=>x.UserName) %>
            </font>
            <br  />

            <%=Html.PasswordFor(x=>x.Password) %>
            <font color="red">
            <%=Html.ValidationMessageFor(x=>x.Password) %>
            </font>
            <br />
              <%=Html.HiddenFor(x=>x.LoginStatus) %>
              <font color="red">
              <%=Html.ValidationMessageFor(x => x.LoginStatus)%>
              </font>
            <br />
           <%=Html.SubmitButton("Login") %>
           <% Html.EndForm(); %>
           
    </div>
    <script language="javascript">

        $("#LoginButton").click(function() {

            $('#elementID').removeClass('hideLoader');
        });
    
</script>
</body>
</html>
