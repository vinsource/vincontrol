<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title>VINControl - Password Recovery</title>
        <link rel="shortcut icon" media="all" type="image/x-icon" href="<%= Url.Content("~/Content/images/vincontrol/icon.ico") %>" />
        <style type="text/css">
			html, body
            {
                height:100%;
                width:100%;
                margin: 0px;
                position: fixed;
			}

			body 
            { 
			    text-align:center;
			}

			#outer
            {
			    height:100%;
			    width:100%;
			    display:table;
			    vertical-align:middle;
			}

			#container 
            {
			    text-align: center;
			    position:relative;
			    vertical-align:middle;
			    display:table-cell;
			} 

			#inner 
            {
			    width: 500px;
			    height: auto;
			    text-align: center;
			    margin-left:auto;
			    margin-right:auto;
			}

            #passwordRecoveryForm
            {
                border: 1px solid blue;
            }

            .formTitle
            {
                background-color: #3366cc;
                color: white;
                font-weight: bold;
                font-size: 30px;
                padding: 10px 10px;
                margin-bottom: 30px;
                text-align: center;
            }

            .lblIntro
            {
                margin: 10px 30px 10px 30px;
                text-align: left;
            }

            .lblInput
            {
                float:left;
                margin: 10px 0px 10px 30px;
                font-weight: bold;
            }

            .txtInput
            {
                float: left;
                margin: 10px 30px 10px 50px;
                border: 2px solid black;
                width: 280px;
                padding: 5px;
            }

            .confirmButton
            {
                background-color: #3366cc;
                color: white;
                border: 0;
                float: left;
                margin: 30px 10px 20px 50px;
                padding: 10px;
            }

            .loginPageLink
            {
                float: left;
                margin: 30px 30px 20px 0px;
                padding: 10px;
            }
        </style>
        <script src="<%= Url.Content("~/Scripts/jquery-1.6.4.min.js") %>" type="text/javascript"></script>
        <script src="<%=Url.Content("~/js/jquery-ui.min.js")%>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/js/jquery.validationEngine-en.js") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/js/jquery.validationEngine.js") %>" type="text/javascript"></script>
        <link href="<%= Url.Content("~/Content/Admin.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%= Url.Content("~/Css/validationEngine.jquery.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%=Url.Content("~/Content/jquery-ui.css")%>" rel="stylesheet" type="text/css" />
        <script src="<%=Url.Content("~/js/jquery.alerts.js")%>" type="text/javascript"></script>
        <link href="<%=Url.Content("~/js/jquery.alerts.css")%>" rel="stylesheet" type="text/css" media="screen" />
        <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

        <script type="text/javascript">
            function validateEmail(email) {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(email);
            }

            function checkEmail(field, rules, i, options) {
                if (!validateEmail(field.val())) {
                    return "Email invalid!";
                }
            }

            function redirectToHomePage() {
                window.location.href = "<%=Url.Action("Index", "Home")%>";
            }

            $(document).ready(function () {
                $("#passwordRecoveryForm").validationEngine('attach', { promptPosition: "topLeft", scroll: false });

                $("#loginPage").click(function (event) {
                    redirectToHomePage();
                });

                $('#email').keypress(function (e) {
                    if (e.which == 13) {
                        $('#btnConfirm').click();
                    }
                });

                $("#btnConfirm").click(function (event) {
                    var flag = true;

                    if (jQuery('#email').validationEngine('validate') == true)
                        flag = false;

                    if (flag == true) {
                        $.blockUI({ message: '<div><img src="<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
                        var urlSubmit = "/Account/SendResetPasswordEmail";

                        $.ajax({
                            type: "POST",
                            url: urlSubmit,
                            data: { email: $('#email').val() },
                            success: function (results) {
                                if (results == 'Incorrect email') {
                                    $.unblockUI();
                                    jAlert('The email you gave is incorrect.', 'Warning',
                                        function () {
                                            $('#email').val("");
                                        });
                                }
                                else {
                                    $.unblockUI();
                                    jAlert('An email had been sent to ' + $('#email').val(), 'Warning',
                                        function () {
                                            redirectToHomePage();
                                        });
                                }
                            }
                        });
                    }
                });

            });
        </script>
    </head>
    <body>
        <div id="outer">
            <div id="container">
                <div id="inner">
                    <form id="passwordRecoveryForm">
                        <table style="border-spacing: 0px" cellpadding="0">
                            <tr>
                                <td colspan="2">
                                    <div class="formTitle">Password Recovery</div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="lblIntro">Type your account email below and we will send you a link to change your password.</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="lblInput">Email Address</div>
                                </td>
                                <td>
                                    <input class="txtInput" id="email" type="text" data-validation-engine="validate[required,funcCall[checkEmail]]"
                                                    data-errormessage-value-missing="Email is required!" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <input class="confirmButton" type="button" value="Recover Password" id="btnConfirm"/>
                                    <a href="#" id="loginPage" class="loginPageLink">Login page</a>
                                </td>
                            </tr>
                        </table>
                    </form>
                </div>
            </div>
        </div>
    </body>
</html>
