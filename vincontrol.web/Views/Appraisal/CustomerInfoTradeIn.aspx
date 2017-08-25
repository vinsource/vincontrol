<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.CustomeInfoModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CustomerInfo</title>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <% Html.BeginForm("UpdateCustomerInfoTradeIn", "Appraisal", FormMethod.Post, new { id = "CustomerInfoForm", name = "CustomerInfoForm" }); %>
    <%=Html.HiddenFor(x=>x.AppraisalId) %>
    <%=Html.HiddenFor(x=>x.TradeInCustomerId) %>
    <div class="customerInfo_popup_holder profile_popup">
        <div class="customerInfo_popup_header">
            Customer Information
        </div>
        <div class="customerInfo_popup_content">
            <div class="customerInfo_popup_items">
                <label>
                    First Name:
                </label>
                <%=Html.TextBoxFor(x => x.FirstName, new { @class = "customerInfo_input" })%>
            </div>
            <div class="customerInfo_popup_items">
                <label>
                    Last Name:
                </label>
                <%=Html.TextBoxFor(x => x.LastName, new { @class = "customerInfo_input" })%>
            </div>
            <div class="customerInfo_popup_items">
                <label>
                    Address:
                </label>
                <%=Html.TextBoxFor(x => x.Address, new { @class = "customerInfo_input" })%>
            </div>
            <div class="customerInfo_popup_items">
                <label>
                    Zip:
                </label>
                <%=Html.TextBoxFor(x => x.ZipCode, new { @class = "customerInfo_input" })%>
            </div>
            <div class="customerInfo_popup_items">
                <label>
                    City:
                </label>
                <%=Html.TextBoxFor(x => x.City, new { @class = "customerInfo_input" })%>
            </div>
            <div class="customerInfo_popup_items">
                <label>
                    State:
                </label>
                <%=Html.DropDownListFor(x => x.State, Model.States, new { @class = "state" })%>
            </div>
            <div class="customerInfo_popup_items">
                <label>
                    Email:
                </label>
                <%=Html.TextBoxFor(x => x.Email, new { @class = "customerInfo_input" })%>
            </div>
        </div>
        <div class="customerInfo_popup_btns">
            <div class="btns_shadow" id="customerInfo_save" onclick="SaveCustomerInfoSubmit();">
                Save
            </div>
            <div class="btns_shadow" id="customerInfo_cancel">
                <a onclick="parent.$.fancybox.close()">Cancel</a>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
    
    <%if (Model.SessionTimeOut)
      { %>
    <script type="text/javascript">
        parent.$.fancybox.close();
        ShowWarningMessage("Your session is timed out. Please login back again");
        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
            window.parent.location = actionUrl;

    </script>
    <% }%>
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>    
    <script type="text/javascript">

        $(document).ready(function () {
            //Setup the AJAX call
            $("#CustomerInfoForm").submit(function (event) {
                $("#elementID").removeClass("hideLoader");
                event.preventDefault();
                SaveCustomerInfo(this);
            });
        });

        function SaveCustomerInfo(form) {
            blockUI();
            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: SaveCustomerInfoClose
            });
        }

        function SaveCustomerInfoClose(result) {
            if (result == "Success") {
                unblockUI();
                parent.$.fancybox.close();
            }

            else if (result == "DuplicateStock") {
                $("#elementID").addClass("hideLoader");
                $("#StockExist").removeClass("hideLoader");
            }
        }
        function SaveCustomerInfoSubmit() {
            $("#CustomerInfoForm").submit();
        }

    </script>
</body>
</html>
