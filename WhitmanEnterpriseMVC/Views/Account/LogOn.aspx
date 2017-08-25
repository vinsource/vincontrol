<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.LogOnViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>VinControl</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="<%=Url.Content("~/Css/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet"
        type="text/css" />
    <link href="<%=Url.Content("~/Css/bootstrap/css/bootstrap-responsive.css")%>" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        html, body
        {
            background: #000000;
            font-family: 'Trebuchet MS' , Arial, sans-serif;
        }
        .well
        {
            border: none;
        }
        .hero-unit
        {
            text-align: center;
            overflow: hidden;
            position: relative;
            padding-bottom: 85px;
            padding-top: 20px;
            box-shadow: inset 0px 0px 5px #000000;
        }
        #login-form
        {
            position: absolute;
            left: 0;
            right: 0;
            bottom: 0;
            margin: 0;
            background: #222;
        }
        #login-form form
        {
            margin: 0 auto;
        }
        .container
        {
            padding-top: 50px;
        }
        #login-error
        {
            background: #222222;
            color: white;
            border-color: black;
        }
        #login-error div
        {
            box-shadow: none;
            border: none;
        }
        #login-error .modal-footer
        {
            background: #111111;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="span9">
                <div class="hero-unit span6 offset2">
                    <img src='<%=Url.Content("~/Images/bLogo.png")%>' />
                    <%Html.EnableClientValidation(); %>
                    <% Html.BeginForm("LogOn", "Account", FormMethod.Post, new { id = "loginForm", name = "loginForm", @class = "form-inline" }); %>
                    <div id="login-form" class="well">
                        <%=Html.TextBoxFor(x => x.UserName, new { @class = "input-small", @placeholder = "Username" })%>
                        <%=Html.PasswordFor(x => x.Password, new { @class = "input-small", @placeholder = "Password" })%>
                        <%=Html.HiddenFor(x => x.LoginStatus)%>
                        <button type="submit" class="btn btn-success">
                            Log In</button>
                    </div>
                    <% Html.EndForm(); %>
                </div>
                <% Html.EndForm(); %>
                <div id="login-error" class="modal hide fade">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;</button>
                        <h3>
                            Incorrect Login Information</h3>
                    </div>
                    <div class="modal-body">
                        <p>
                            You have entered either the wrong username or the wrong password.</p>
                    </div>
                    <div class="modal-footer">
                        <a href="#" class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Close</a>
                    </div>
                </div>
                <!--[if lt IE 9]>
        <div id="browser-download" class="alert alert-block span5 offset3">
          <h4><i class="icon-warning-sign"></i> Warning!</h4>
          You are using an unsupported browser version! VinControl is optimized for the latest versions of <a href="https://www.google.com/intl/en/chrome/browser/" target="_blank">Google Chrome</a>, <a href="http://www.mozilla.org/en-US/firefox/new/" target="_blank">Mozilla Firefox</a> and <a href="http://windows.microsoft.com/en-us/internet-explorer/products/ie/home" target="_blank">Internet Explore (9)</a>. Please download one of the listed browsers for the best user experience!
        </div>
        <![endif]-->
            </div>
        </div>
    </div>

    <script type="text/javascript" language="javascript" src="http://code.jquery.com/jquery-latest.js"></script>

    <script src="<%=Url.Content("~/Css/bootstrap/js/bootstrap.min.js")%>" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">


        $('#login-error').modal();

    </script>

</body>
</html>
