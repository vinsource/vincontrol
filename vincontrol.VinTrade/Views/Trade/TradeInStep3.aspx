<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/TradeIn.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.TradeInManagement.TradeInVehicleModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trade In | Step 3
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeaderHolder" runat="server">
    <div class="tradeInHeader_steps">
    </div>
    <div class="tradeInHeader_step1Btn">
    </div>
    <div class="tradeInHeader_step2Btn">
    </div>
    <div class="tradeInHeader_step3Btn">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("GetTradeInValue", "Trade", FormMethod.Post, new { id = "TradeInCustomerForm" }); %>
      <input id="DealerName" name="DealerName" type="hidden" value="<%= Model.DealerName %>" />
    <div class="tstep3_content_holder">
        <div class="tstep3_content_left">
            <div class="tstep3_left_vh">
                Your <%= Model.SelectedYear %> <%= Model.SelectedMakeValue %> <%= Model.SelectedModelValue %>'s Value is in!
            </div>
            <div class="tstep3_left_img">
                <div class="tstep3_car_img">
                    <img src="<%= !String.IsNullOrEmpty(Model.ImageUrl) ? Model.ImageUrl : "/Content/Images/TradeInStep/car.png"%>">
                </div>
                <div class="tstep_ad_img">
                    <img src="<%=Url.Content("~/Content/Images/TradeInStep/img-3.png")%>">
                </div>
            </div>
        </div>
        <div class="tstep3_content_right">
            <div class="error-wrap">
                <p class="error" title="Click to Close">
                </p>
            </div>
            <div class="tstep3_right_title">
                Please fill out the form below to recieve your free Trade Report!</div>
            <div class="tstep3_input_items">
                <label>First Name</label>
                <input id="CustomerFirstName" name="CustomerFirstName" value="<%= Model.CustomerFirstName %>" type="text" placeholder="Enter First Name" />
            </div>
            <div class="tstep3_input_items">
                <label>Last Name</label>
                <input id="CustomerLastName" name="CustomerLastName" value="<%= Model.CustomerLastName %>" type="text" placeholder="Enter Last Name" />
            </div>
            <div class="tstep3_input_items">
                <label>E-Mail</label>
                <input id="CustomerEmail" name="CustomerEmail" value="<%= Model.CustomerEmail %>" type="text" placeholder="Enter E-Mail" />
            </div>
            <div class="tstep3_input_items">
                <label>Phone Number</label>
                <input id="CustomerPhone" name="CustomerPhone" value="<%= Model.CustomerPhone %>" type="text" placeholder="Enter Phone Number" />
            </div>
          
        </div>
        <div style="clear: both">
        </div>
    </div>
    <%Html.EndForm(); %>
    <a class="tradeIn_links" href="javascript:;" onclick="javascript:TradeInCustomerFormSubmit();">
        <div class="tradeIn_step_goto">
            See the Value and Get an Emailed Copy!</div>
    </a>
    <script type="text/javascript">
        $(".tradeInHeader_step1Btn").addClass("tradeInHeader_step1Btn_active");
        $(".tradeInHeader_step2Btn").addClass("tradeInHeader_step2Btn_active");
        $(".tradeInHeader_step3Btn").addClass("tradeInHeader_step3Btn_active");
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script src="<%=Url.Content("~/Scripts/jquery.maskedinput-1.3.js")%>" type="text/javascript"></script>
<script type="text/javascript">
    function TradeInCustomerFormSubmit() {
        if (validateForm()) {
            $("#TradeInCustomerForm").submit();
        }
    }

    jQuery(function ($) {
        $("#CustomerPhone").mask("(999) 999-9999");        
    });

    function validateForm() {
        var errorString = "We're missing some information : ";
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
</asp:Content>
