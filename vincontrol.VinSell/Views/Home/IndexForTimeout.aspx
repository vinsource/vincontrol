<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.AccountManagement.LogOnViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>VinSell | Log In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="<%=Url.Content("~/Content/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/bootstrap/css/bootstrap-responsive.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Scripts/jquery.alerts.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <style type="text/css">
      html, body {background: #fff; font-family: 'Trebuchet MS', Arial, sans-serif;}
      .well {border: none;}
      .hero-unit {
        text-align: center;
        overflow: hidden;
        position: relative;
        padding-bottom: 85px;
        padding-top: 20px;
        box-shadow: inset 0px 0px 5px #161e8a;
      }
      #login-form {position: absolute; left: 0; right: 0; bottom: 0; margin:0; background: #161e8a;}
      #login-form form {margin: 0 auto;}
      .container {padding-top: 50px;}
      #login-error { background: #222222; color: white; border-color: black;}
      #login-error div {box-shadow: none; border: none;}
      #login-error .modal-footer {background: #111111;}
      .alert { color: Black; }
      .span6 { /*width: 270px;*/ }
    </style>
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.alerts.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnCancel').click(function () {
                window.parent.document.getElementById('Action').value = 'Cancel';
                parent.$.fancybox.close();
            });

            $('#btnLogIn').click(function () {
                LogOn();
            });

            $("#UserName").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    LogOn();
                }
            });

            $("#Password").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    LogOn();
                }
            });
        });

        function LogOn() {
            if ($('#UserName').val() != '' && $('#Password').val() != '') {
                $.ajax({
                    type: "POST",
                    url: "/Account/LogOn",
                    data: $("form").serialize(),
                    success: function (results) {
                        if (results == 'Incorrect') {
                            jAlert('Incorrect Username or Password.', 'Warning');
                            return false;
                        } else if (results == 'Error') {
                            jAlert('System Error!', 'Warning');
                            return false;
                        } else {
                            window.parent.document.getElementById('Action').value = results;
                            parent.$.fancybox.close();
                        }
                    },
                    error: function (err) {
                        jAlert('Request Error: ' + err.status + " - " + err.statusText, 'Warning');
                        return false;
                    }
                });
            } else {
                jAlert('Username/Password are required.', 'Warning');
            }
        }
    </script>
</head>
<body>
    <div class="container" style="padding-top:0px;width:auto;">
        <div class="row">            
            <div style="color:Black;margin: 5px 0; font-size: 15px; font-weight: bold;">Please sign in again.</div>
            <div style="color:Black;margin: 5px 0; font-size: 15px;">For your security, you were signed out.</div>
            <div class="span9">
                <div class="hero-unit span6 offset2">
                    <img src='<%=Url.Content("~/Content/Images/logo.jpg")%>' style="width:270px;border:0;"/>
                    <div id="login-form" class="well">                        
                        <form id="data" class="form-inline">
                        <%=Html.TextBoxFor(x => x.UserName, new { @class = "input-small", @placeholder = "Username" })%>
                        <%=Html.PasswordFor(x => x.Password, new { @class = "input-small", @placeholder = "Password" })%>                        
                        <button id="btnLogIn" type="button" class="btn btn-success">Log In</button>
                        <button id="btnCancel" type="button" class="btn">Cancel</button>                        
                        </form>
                    </div>
                </div>          
            </div>
        </div>
    </div>
</body>
</html>
