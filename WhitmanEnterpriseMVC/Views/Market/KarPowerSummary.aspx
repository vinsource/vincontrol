<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KBB Market Info</title>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.cookie.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var warningMessage = "Please change Vin or Mileage, then click Get Value again!";
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#DownloadTokenValueId').val(token);
            $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie('fileDownloadToken');
                if (cookieValue == token)
                    finishDownload();
            }, 1000);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie('fileDownloadToken', null); //clears this cookie value
             $.unblockUI();
        }

        $(document).ready(function () {
            $("#txtMileage").forceNumeric();

            GetValuation($('#txtVin').val(), $('#txtMileage').val());

            $("#SelectedEngineId").live("change", function () {
                //alert(warningMessage);
                return false;
            });

            $("#SelectedMakeId").live("change", function () {
                alert(warningMessage);
                return false;
            });

            $("#SelectedModelId").live("change", function () {
                alert(warningMessage);
                return false;
            });

            $("#SelectedTrimId").live("change", function () {
                if ($("#SelectedTrimId").val() == 0) return false;

                $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingTrim?trimId=" + $("#SelectedTrimId").val(),
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                         $.unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                         $.unblockUI();
                        alert(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $("#SelectedTransmissionId").live("change", function () {
                //alert(warningMessage);
                return false;
            });

            $("#SelectedDriveTrainId").live("change", function () {
                //alert(warningMessage);
                return false;
            });

            $('input[name^="option_"]').live("click", function () {
                var listingId = this.id.split('_')[1];
                var isChecked = $(this).is(":checked");
                //$.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

                $.ajax({
                    type: "GET",
                    url: "/Market/UpdateValuationByOptionalEquipment?listingId=" + listingId + "&isChecked=" + isChecked,
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                         $.unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                         $.unblockUI();
                        alert(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $('#btnPrint').live('click', function () {
                $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

                var selectedOptionIds = "";
                $('input[name^="option_"]:checked').each(function () {
                    selectedOptionIds += this.id.split('_')[1] + ',';
                });
                $("#SelectedOptionIds").val(selectedOptionIds);

                // Set a flag to know if we need to reload profile page after closing the Chart                
                window.parent.document.getElementById('NeedToReloadPage').value = true;

                document.forms[0].submit();
                var timeout = setTimeout(function () {
                     $.unblockUI();
                }, 5000);
            });

            $("#btnDecode").click(function () {
                if ($('#txtVin').val() == '') {
                    alert('Vin is required!');
                    return false;
                }

                if ($('#txtMileage').val() == '') {
                    alert('Mileage is required!');
                    return false;
                }

                GetValuation($('#txtVin').val(), $('#txtMileage').val());

            });

            $("#btnSave").live('click', function () {
                var selectedOptionIds = "";
                $('input[name^="option_"]:checked').each(function () {
                    selectedOptionIds += this.id.split('_')[1] + ',';
                });

                $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

                $.ajax({
                    type: "POST",
                    url: "/Market/SaveKarPowerOptions?vin=" + $('#txtVin').val() + "&trimId=" + $('#txtTrim').val() + "&selectedOptionIds=" + selectedOptionIds + "&baseWholesale=" + $("#baseWholesale").val() + "&wholesale=" + $("#wholesale").val(),
                    data: {},
                    dataType: 'html',
                    success: function () {
                         $.unblockUI();
                        // Set a flag to know if we need to reload profile page after closing the Chart                
                        window.parent.document.getElementById('NeedToReloadPage').value = true;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                         $.unblockUI();
                        alert(xhr.status + ' ' + thrownError);
                    }
                });
            });

            // Each element in this array is a KBB.Karpower.WebServices.LightOption.
            // We add a new element each time the user checks or unchecks an option.
            // The items are in chronological order.
            var optionSelectionHistory = new Array();
            // the user clicked one of the checkboxes in the Optional Equipment checkboxlist
            function OptionChanged(checkbox, optionId) {
                alert(optionId);
            }

        });

        function GetValuation(vin, mileage) {
            $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

            $.ajax({
                type: "POST",
                url: "/Market/KarPowerResult?vin=" + vin + "&mileage=" + mileage,
                data: {},
                dataType: 'html',
                success: function (data) {

                    $("#result").html(data);
                     $.unblockUI();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                     $.unblockUI();
                    alert(xhr.status + ' ' + thrownError);
                }
            });
        }

        // forceNumeric() plug-in implementation
        jQuery.fn.forceNumeric = function () {

            return this.each(function () {
                $(this).keydown(function (e) {
                    var key = e.which || e.keyCode;

                    if (!e.shiftKey && !e.altKey && !e.ctrlKey &&
                    // numbers   
                         key >= 48 && key <= 57 ||
                    // Numeric keypad
                         key >= 96 && key <= 105 ||
                    // comma, period and minus, . on keypad
                        key == 190 || key == 188 || key == 109 || key == 110 ||
                    // Backspace and Tab and Enter
                        key == 8 || key == 9 || key == 13 ||
                    // Home and End
                        key == 35 || key == 36 ||
                    // left and right arrows
                        key == 37 || key == 39 ||
                    // Del and Ins
                        key == 46 || key == 45)
                        return true;

                    return false;
                });
            });
        }
    </script>
    <style type="text/css">
        body {
			font-family: "Trebuchet MS", Arial, Helvetica, sans-serif; 
			overflow: auto;
			padding-top: 10px; 
			padding-bottom: 30px;
			width: 700px;
			margin: 0 auto;
			margin-top: 20px;
			background: #111;
		}
		
		input[type="button"], input[type="sumbit"] {background: #860000 !important; border:0; color:White; padding:3px 6px;}
		input[type="button"]:hover, input[type="sumbit"]:hover {background: #860000 !important;}
    </style>
        
</head>
<body>
    <div style="margin-left:0px;">
    <div>
        <%--<form id="search" method="post" action="">--%>
        <div style="text-align:center;">
            <input type="hidden" name="txtVin" id="txtVin" value="<%= (String)ViewData["VIN"] %>" />
            <%--<font style="color:white;">Vin:</font>
            <input type="text" id="txtVin" name="txtVin" maxlength="17" size="30" value="<%= (String)ViewData["VIN"] %>" disabled="disabled" />--%>
            &nbsp; 
            <font style="color:white;">Mileage:</font>
            <input type="text" id="txtMileage" name="txtMileage" maxlength="17" size="17" value="<%= (String)ViewData["MILEAGE"] %>" />
            &nbsp;
            <input type="button" name="btnDecode" id="btnDecode" value="Get Value" />
        </div>
        <%--</form>--%>
    </div>
    <div id="result">
    </div>
    </div>
</body>
</html>
