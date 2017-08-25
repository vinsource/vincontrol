<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Transfer Vehicle</title>
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <div id="confirm_popup_wholesale" class="confirm_message" style="display: block">
        <div class="confirm_message_title">
            Are you sure to add this car to whole sale?
        </div>
        <div class="confirm_message_btns">
            <div class="btns_shadow confirm_message_yes">
                <a onclick="MoveToWholeSale();">Yes</a>
            </div>
            <div class="btns_shadow confirm_message_no">
                <a onclick="parent.$.fancybox.close()">No</a>
            </div>
        </div>
    </div>
    
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

        function MoveToWholeSale() {
            var type = <%=Model.Type %>;
            $.post('<%=Url.Action("TransferToWholeSaleFromInventoryNew","Inventory") %>', { ListingId: <%=Model.ListingId %>, type: type }, function(data) {
                window.parent.location.href = data.url;
                parent.$.fancybox.close();
            });
        }

    </script>
</body>
</html>
