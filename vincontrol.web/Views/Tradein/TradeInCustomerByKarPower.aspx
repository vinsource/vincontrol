<%@ Page Title="Trade In" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.TradeInVehicleModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer</title>
    <link href="<%= Url.Content("~/Css/TradeIn/style.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <%--<script src="http://code.jquery.com/jquery-latest.js"></script>--%>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/jquery.maskedinput-1.3.js")%>"></script>
    <!--[if lte IE 7]>
	<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Css/TradeIn/ie-7.css") %>">
	<![endif]-->
</head>
<body>
    <div id="container">
        <% Html.BeginForm("GetTradeInValueByKarPower", "TradeIn", FormMethod.Post, new { id = "TradeInCustomerForm", onsubmit = "return validateForm()" }); %>
        <div id="header">
            <div class="logo">
            </div>
            <div class="mask">
                <div class="text-wrap">
                    <h1>
                        Get Your Trade In Value!</h1>
                </div>
            </div>
            <div class="steps">
                <div id="step-1" class="step">
                    <img src="<%= Url.Content("~/images/step-1.png")%>" alt="step 1" /></div>
                <div id="step-2" class="step">
                    <img src="<%= Url.Content("~/images/step-2.png")%>" alt="step 2" /></div>
                <div id="step-3" class="step">
                    <img src="<%= Url.Content("~/images/on-step-3.png")%>" alt="step 3" /></div>
            </div>
        </div>
        <div class="slide-wrapper">
            <h3 class="description-header">
                Final Step!</h3>
            <div <%--id="options"--%> class="info-box">
                <div class="error-wrap">
                    <p class="error" title="Click to Close">
                    </p>
                </div>
                <div class="row">
                    <div id="contact-info" class="row-cell">
                        <h3 class="description-header">
                            Contact Info</h3>
                        <div class="contact-info form-wrap">
                            <div class="input-wrap">
                                <div class="lable">
                                    First :</div>
                                <%=Html.TextBoxFor(x=>x.CustomerFirstName) %><%=Html.ValidationMessageFor(x => x.CustomerFirstName, string.Empty, new { style = "color : red" })%>
                            </div>
                            <div class="input-wrap">
                                <div class="lable">
                                    Last :</div>
                                <%=Html.TextBoxFor(x=>x.CustomerLastName) %><%=Html.ValidationMessageFor(x => x.CustomerLastName, string.Empty, new { style = "color : red" })%>
                            </div>
                            <div class="input-wrap">
                                <div class="lable">
                                    Email:</div>
                                <%=Html.TextBoxFor(x=>x.CustomerEmail) %><%=Html.ValidationMessageFor(x=>x.CustomerEmail,string.Empty,new {style="color : red"}) %>
                            </div>
                            <div class="input-wrap">
                                <div class="lable">
                                    Phone:</div>
                                <%=Html.HiddenFor(x=>x.SelectedYear) %>
                                <%=Html.HiddenFor(x=>x.SelectedMake) %>
                                <%=Html.HiddenFor(x => x.SelectedMakeValue)%>
                                <%=Html.HiddenFor(x=>x.SelectedModel) %>
                                <%=Html.HiddenFor(x => x.SelectedModelValue)%>
                                <%=Html.HiddenFor(x=>x.SelectedTrim) %>
                                <%=Html.HiddenFor(x => x.SelectedTrimValue)%>
                                <%=Html.HiddenFor(x=>x.VehicleId) %>
                                <%=Html.TextBoxFor(x=>x.CustomerPhone) %>
                                <%=Html.HiddenFor(x=>x.SelectedOptions) %>
                                <%=Html.HiddenFor(x=>x.EncryptVehicleId) %>
                                <%=Html.HiddenFor(x=>x.Vin) %>
                                <%=Html.HiddenFor(x=>x.Mileage) %>
                                <%=Html.HiddenFor(x=>x.Condition) %>
                            </div>
                        </div>
                    </div>
                    <div id="reviews" class="row-cell">
                        <h3 class="description-header">
                            Reviews</h3>
                        <div class="reviews text-wrap" style="font-size: .8em">
                            <% foreach (var tmp in Model.ReviewList)
                               {
                                    %>
                            <p>
                                <%=tmp.ReviewContent %>
                                <span class="quote">
                                    <%=tmp.Name %>
                                    <%=tmp.City %>,
                                    <%=tmp.State %></span>
                            </p>
                            <%} %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="controls">
            <a class="next" onclick="javascript:TradeInCustomerFormSubmit();">Next Step ></a>
            <a href="javascript: history.go(-1)" class="prev">< Previous Step</a>
        </div>
        <%Html.EndForm(); %>
    </div>
    <script src="<%=Url.Content("~/js/trade-in.js")%>" type="text/javascript"></script>
</body>
<script type="text/javascript">
    function TradeInCustomerFormSubmit() {
        $("#TradeInCustomerForm").submit();
    }
    jQuery(function ($) {
        $("#CustomerPhone").mask("(999) 999-9999");


    });

    function validateForm() {
        var errorString = "Please correct all errors below : ";
        var flag = true;
        var errorCount = 0;

        if (trimString($("#CustomerFirstName").val()) == "") {
            $("#CustomerFirstName").val("");
            errorString += "<li>First Name is required</li>";
            errorCount++;
            flag = false;
        }
        if (trimString($("#CustomerLastName").val()) == "") {
            $("#CustomerLastName").val("");
            errorString += "<li>Last Name is required</li>";
            errorCount++;
            flag = false;
        }
        if (trimString($("#CustomerEmail").val()) == "") {
            $("#CustomerEmail").val("");
            errorString += "<li>Email Information is required</li>";
            errorCount++;
            flag = false;
        } else {
            var validEmail = trimString($("#CustomerEmail").val());

            if (!isValidEmailAddress(validEmail)) {
                errorString += "<li>A valid email is required</li>";
                errorCount++;
                flag = false;
            }
        }
        
        if (flag == false) {
            if (errorCount == 1)
                error(errorString, "60px");
            else if (errorCount == 2)
                error(errorString, "90px");
            else
                error(errorString, "120px");
            return false;
        } else {

            return true;
        }


    }

    function isValidEmailAddress(emailAddress) {
        var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
        return pattern.test(emailAddress);
    };

    function trimString(text) {
        return text.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
    }
</script>
</html>
