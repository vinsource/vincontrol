<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Transfer Vehicle</title>
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
</head>
<body>
    
    <% Html.BeginForm("TransferVehicle", "Inventory", FormMethod.Post, new { id = "VehicleTransferForm", name = "VehicleTransferForm" }); %>
    <div class="profile_tab_holder profile_popup" id="profile_transfer_holder" style="display: block">
        <div class="profle_transfer_header">
            Vehicle Transfer
        </div>
        <div class="profile_transfer_vh">
            <%=Model.ModelYear %> <%=Model.Make %> <%=Model.Model %> <%=Model.Trim %><%=Model.Stock %>
        </div>
        <div class="profile_transfer_currentlocation">
            Current Location: <%=Model.DealerName%>
        </div>
        <div class="profile_transfer_info">
            <div class="profile_transfer_title">
                Transfer To:
            </div>
            <div class="profile_transfer_info_select">
                <% if (Model.TransferDealerGroup != null)
                   { %>
                <%= Html.DropDownListFor(x => x.SelectedDealerTransfer, Model.TransferDealerGroup, new {@class = "DDLEdit", @style = "width:230px"}) %>
                <% } %>
            </div>
            <div class="profile_transfer_info_input">
                <%=Html.TextBoxFor(x => x.Stock, new { @placeholder = "New Stock Number" })%>
            </div>
        </div>
        <div class="profile_transfer_btns_holder">
            <div class="profile_transfer_btns">
                <a onclick="TransferSubmit()">Transfer</a>
            </div>
            <div class="profile_transfer_btns profile_transfer_btns_cancel">
                <a onclick="parent.$.fancybox.close()">Cancel</a>
            </div>
        </div>
    </div>
    <%=Html.HiddenFor(x=>x.ListingId) %>
    <% Html.EndForm(); %>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <%if (Model.SessionTimeOut)
      { %>
    <script type="text/javascript">
        parent.$.fancybox.close();
        alert("Your session is timed out. Please login back again");
        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
        window.parent.location = actionUrl;

    </script>
    <% }%>
    <script type="text/javascript">

        $(document).ready(function () {
            //Setup the AJAX call
            $("#VehicleTransferForm").submit(function (event) {

                $("#elementID").removeClass("hideLoader");
                event.preventDefault();

                TransferVehicle(this);

            });

            $('#fancybox-outer').attr('width', '300px');

            $('#fancybox-content').attr('width', '300px');

        });

        function TransferVehicle(form) {

            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: TransferVehicleClose

            });

        }

        function TransferVehicleClose(result) {

            if (result == "Success") {
                parent.$.fancybox.close();

                alert("This vehicle was successfully transferred");

                var actionUrl = '<%= Url.Action("ViewInventory", "Inventory" ) %>';

                window.parent.location.href = actionUrl;
            }

            else if (result == "DuplicateStock") {
                $("#elementID").addClass("hideLoader");
                $("#StockExist").removeClass("hideLoader");
            }
        }

        function TransferSubmit() {
            $("#VehicleTransferForm").submit();
        }

    </script>
</body>
