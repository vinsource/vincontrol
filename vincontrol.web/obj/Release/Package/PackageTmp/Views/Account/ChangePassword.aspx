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

            #changePasswordForm
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
                margin-bottom: 50px;
                text-align: center;
            }

            .lblInput
            {
                float:left;
                margin: 10px 0px 10px 30px;
                text-align: left;
                width: 150px;
            }

            .txtInput
            {
                float: left;
                margin: 10px 30px 10px 30px;
                border: 2px solid black;
                width: 250px;
            }

            .confirmButton
            {
                background-color: #3366cc;
                color: white;
                border: 0;
                float: left;
                margin: 30px 10px 20px 30px;
                padding: 10px 10px;
            }

            .cancelButton
            {
                float: left;
                border: 0;
                margin: 30px 30px 20px 0px;
                background-color: #414342;
                color: white;
                padding: 10px 40px;
            }

            .errorPageHolder
            {
                width: 850px;
                border: 2px solid #B0B0B0;
                padding: 70px 30px;
                border-radius: 10px;
                background: #FAFAFA;
            }

            .expireMessage
            {
                font-size: 20px;
                font-weight: bold;
                margin: 10px;
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
        <script type="text/javascript">
            
            function ValidatePass(pass) {
                var re = /^(?=.*\d)(?=.*[a-zA-Z]).{4,10}$/;
                return re.test(pass);
            }

            function checkPass(field, rules, i, options) {
                var pass = field.val();
                var validPassword = ValidatePass(pass);
                console.log(pass.length);
                if (!validPassword) {
                    return "Password invalid, Password should be 4-10 characters, should include at least one alphabet and at least one number.";
                }
            }

            function checkConfirmPass(field, rules, i, options) {
                var pass = field.val();

                if (pass != $('#newPassword').val()) {
                    return "Confirm New Password does not match with New Password.";
                }
            }

            function redirectToHomePage() {
                window.location.href = "<%=Url.Action("Index", "Home")%>";
            }

            var bIsResetPassword = false;
            var userId = '<%= (int)ViewData["USERID"] %>';
            var forgotPasswordId = '<%= (string)ViewData["FORGOTPASSWORDID"] %>';
            

            $(document).ready(function () {
                $("#displayForm").show();
                $("#displayError").hide();

                if (userId != "" && forgotPasswordId != "") {
                    bIsResetPassword = true;
                    $("#btnConfirm").val("Submit");
                    $("#btnConfirm").css("padding", "10px 40px");
                    $("#passwordTitle").text("Password Recovery");
                }
                else {
                    bIsResetPassword = false;
                }

                $("#changePasswordForm").validationEngine('attach', { promptPosition: "topLeft", scroll: false });

                if (bIsResetPassword) {
                    var bIsExpired = '<%= (int)ViewData["ISEXPIRED"] %>';

                    if (bIsExpired == 1) {
                        $("#displayForm").hide();
                        $("#displayError").show();
                        $("#inner").css("width", "900px");
                    }
                    else {
                        $("#rowCurrentPassword").css("display", "none");
                    }
                }

                $("#btnCancel").click(function (event) {
                    if (bIsResetPassword == false) {
                        parent.$.fancybox.close();
                    }
                    else {
                        redirectToHomePage();
                    }
                        
                });

                $("#btnConfirm").click(function (event) {
                    var flag = true;

                    if (bIsResetPassword == false) {

                        if (jQuery('#currentPassword').validationEngine('validate') == true)
                            flag = false;
                        if (jQuery('#newPassword').validationEngine('validate') == true)
                            flag = false;
                        if (jQuery('#confirmPassword').validationEngine('validate') == true)
                            flag = false;

                        if (flag == true) {
                            var urlSubmit = "/Account/UpdateCurrentUserPassword";

                            $.ajax({
                                type: "POST",
                                url: urlSubmit,
                                data: { currentPass: $('#currentPassword').val(), newPass: $('#newPassword').val() },
                                success: function (results) {
                                    if (results == 'Incorrect password') {
                                        jAlert('The current password you gave is incorrect.', 'Warning',
                                            function () {
                                                $('#currentPassword').val("");
                                                $('#newPassword').val("");
                                                $('#confirmPassword').val("");
                                            });
                                    }
                                    else {
                                        jAlert('Your password has been changed successfully.', 'Warning',
                                            function () {
                                                parent.$.fancybox.close();
                                            });
                                    }
                                }
                            });
                        }
                    }
                    else {
                        if (jQuery('#newPassword').validationEngine('validate') == true)
                            flag = false;
                        if (jQuery('#confirmPassword').validationEngine('validate') == true)
                            flag = false;

                        if (flag == true) {
                            var urlSubmit = "/Account/ResetPassword?userId=" + userId + "&forgotPasswordId=" + forgotPasswordId;

                            $.ajax({
                                type: "POST",
                                url: urlSubmit,
                                data: { newPass: $('#newPassword').val() },
                                success: function (results) {
                                    if (results == 'Updated') {
                                        jAlert('Your password has been changed successfully. Please log in again.', 'Warning',
                                            function () {
                                                redirectToHomePage();
                                            });
                                    }
                                    else {
                                        jAlert('Your password has not been changed.', 'Warning',
                                            function () {
                                                redirectToHomePage();
                                            });
                                    }
                                }
                            });
                        }
                    }
                });

            });
        </script>
    </head>
    <body>
        <div id="outer">
            <div id="container">
                <div id="inner">
                    <div id="displayForm">
                        <form id="changePasswordForm">
                            <table width="100%" style="border-spacing: 0px" cellpadding="0">
                                <tr>
                                    <td colspan="2">
                                        <div class="formTitle" id ="passwordTitle">Change Password</div>
                                    </td>
                                </tr>
                                <tr id="rowCurrentPassword">
                                    <td>
                                        <div class="lblInput">Current password:</div>
                                    </td>
                                    <td>
                                        <input class="txtInput" id="currentPassword" type="password" data-validation-engine="validate[required,funcCall[checkPass]]"
                                                        data-errormessage-value-missing="Password is required!" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="lblInput">New password:</div>
                                    </td>
                                    <td>
                                        <input class="txtInput" id="newPassword" type="password" data-validation-engine="validate[required,funcCall[checkPass]]"
                                                        data-errormessage-value-missing="Password is required!" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="lblInput">Confirm new password:</div>
                                    </td>
                                    <td>
                                        <input class="txtInput" id="confirmPassword" type="password" data-validation-engine="validate[funcCall[checkConfirmPass]]" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <input class="confirmButton" type="button" value="Change Password" id="btnConfirm"/>
                                        <input class="cancelButton" type="button" value="Cancel" id="btnCancel"/>
                                    </td>
                                </tr>
                            </table>
                        </form>
                    </div>
                    <div id="displayError" class="errorPageHolder" >
                        <div style="padding-bottom: 20px" >
                            <img alt="" src="/Content/images/vincontrol/FadeLogo.png" />
                        </div>
                        <div class="expireMessage">
                            Password reset link expires after 24 hour as a security reason.
                        </div>
                        <div class="expireMessage">
                            Please simply click on "Forgot password" on vincontrol login screen again to have a new link sent.
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </body>
</html>
