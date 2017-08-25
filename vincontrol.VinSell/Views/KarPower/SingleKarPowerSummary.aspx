<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>KBB Market Info</title>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.cookie.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var warningMessage = "Please change Vin or Mileage, then click Get Value again!"
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#DownloadTokenValueId').val(token);
            blockUIPopUp();
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie('fileDownloadToken');
                if (cookieValue == token)
                    finishDownload();
            }, 1000);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie('fileDownloadToken', null); //clears this cookie value
            unblockUI();
        }

        $(document).ready(function () {
            $("#txtMileage").forceNumeric();

            blockUIPopUp();
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '<%= Url.Action("HasKBBAuthorization", "KarPower")%>',
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    if (result == 'False') {
                        $('#FailedAuthorization').show();
                        $('#divEnterMilage').hide();
                        unblockUI();
                    } else {
                        unblockUI();
                        GetValuation($('#txtVin').val(), $('#txtMileage').val(), $('#txtModel').val(), $('#txtTrim').val(), $('#txtType').val(), $('#txtHasVin').val());
                    }
                },
                error: function (err) {
                    $('#FailedAuthorization').show();
                    $('#divEnterMilage').hide();
                    unblockUI();
                }
            });

            $("#SelectedEngineId").live("change", function () {
                if ($("#SelectedTransmissionId").val() == 0) return false;

                blockUIPopUp();

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingTrimAndTransmission?trimId=" + $("#SelectedTrimId").val() + "&transmissionId=" + $("#SelectedTransmissionId").val() + "&engineId=" + $("#SelectedEngineId").val(),
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                        unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        unblockUI();
                        //alert(xhr.status + ' ' + thrownError);
                        console.log(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $("#SelectedMakeId").live("change", function () {
                return false;
            });

            $("#SelectedModelId").live("change", function () {
                if ($("#SelectedModelId").val() == 0) return false;

                blockUIPopUp();

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingModel?modelId=" + $("#SelectedModelId").val(),
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                        $('#txtModel').val($("#SelectedModelId").val());
                        unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        unblockUI();
                        //alert(xhr.status + ' ' + thrownError);
                        console.log(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $("#SelectedTrimId").live("change", function () {
                if ($("#SelectedTrimId").val() == 0) return false;

                blockUIPopUp();

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingTrim?trimId=" + $("#SelectedTrimId").val(),
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                        $('#txtTrim').val($("#SelectedTrimId").val());
                        unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        unblockUI();
                        //alert(xhr.status + ' ' + thrownError);
                        console.log(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $("#SelectedTransmissionId").live("change", function () {
                if ($("#SelectedTransmissionId").val() == 0) return false;

                blockUIPopUp();

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingTrimAndTransmission?trimId=" + $("#SelectedTrimId").val() + "&transmissionId=" + $("#SelectedTransmissionId").val() + "&engineId=" + $("#SelectedEngineId").val(),
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                        unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        unblockUI();
                        //alert(xhr.status + ' ' + thrownError);
                        console.log(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $("#SelectedDriveTrainId").live("change", function () {
                //alert(warningMessage);
                return false;
            });

            $('input[name^="option_"]').live("click", function () {
                var listingId = this.id.split('_')[1];
                var isChecked = $(this).is(":checked");

                $.ajax({
                    type: "GET",
                    url: "/Market/UpdateValuationByOptionalEquipmentInSingleMode?listingId=" + listingId + "&isChecked=" + isChecked + "&trimId=" + $('#txtTrim').val(),
                    data: {},
                    dataType: 'html',
                    success: function (data) {

                        $("#result").html(data);
                        unblockUI();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        unblockUI();
                        //alert(xhr.status + ' ' + thrownError);
                        console.log(xhr.status + ' ' + thrownError);
                    }
                });
            });

            $('#btnPrint').live('click', function () {
                blockUIPopUp();

                var selectedOptionIds = "";
                $('input[name^="option_"]:checked').each(function () {
                    selectedOptionIds += this.id.split('_')[1] + ',';
                });
                $("#SelectedOptionIds").val(selectedOptionIds);

                $("#Type").val($("#txtType").val());
                $("#BaseWholesale").val($("#baseWholesale").val());
                $("#Wholesale").val($("#wholesale").val());

                //                $.ajax({
                //                    url: "/Market/PrintReport",
                //                    type: "POST",
                //                    enctype: 'multipart/form-data',
                //                    data: $('#data').serialize(),
                //                    //dataType: 'html',
                //                    success: function (data) {
                //                        // Set a flag to know if we need to reload profile page after closing the Chart                
                //                        window.parent.document.getElementById('NeedToReloadPage').value = true;
                //                         $.unblockUI();
                //                    },
                //                    error: function (xhr, ajaxOptions, thrownError) {
                //                         $.unblockUI();
                //                        alert(xhr.status + ' ' + thrownError);
                //                    }
                //                })

                // Set a flag to know if we need to reload profile page after closing the Chart                
                window.parent.document.getElementById('NeedToReloadPage').value = true;

                //$('#data').submit(function () {
                //    blockUIForDownload();
                //});
                document.forms[0].submit();
                var timeout = setTimeout(function () {
                    unblockUI();
                }, 5000);

            });

            $("#btnDecode").click(function () {
                if ($('#txtVin').val() == '') {
                    ShowWarningMessage('Vin is required!');
                    return false;
                }

                if ($('#txtMileage').val() == '') {
                    ShowWarningMessage('Mileage is required!');
                    return false;
                }

                GetValuation($('#txtVin').val(), $('#txtMileage').val(), $('#txtModel').val(), $('#txtTrim').val(), $('#txtType').val(), $('#txtHasVin').val());

            });

            $("#btnSave").live('click', function () {
                var selectedOptionIds = "";
                $('input[name^="option_"]:checked').each(function () {
                    selectedOptionIds += this.id.split('_')[1] + ',';
                });
                $('#txtModel').val($("#SelectedModelId").val());
                $('#txtTrim').val($("#SelectedTrimId").val());
                blockUIPopUp();

                $.ajax({
                    type: "POST",
                    url: "/Karpower/SaveKarPowerOptions?vin=" + $('#txtVin').val() + "&modelId=" + $('#txtModel').val() + "&trimId=" + $('#txtTrim').val() + "&selectedOptionIds=" + selectedOptionIds + "&engineId=" + $("#SelectedEngineId").val() + "&transmissionId=" + $("#SelectedTransmissionId").val() + "&driveTrainId=" + $("#SelectedDriveTrainId").val() + "&baseWholesale=" + $("#baseWholesale").val() + "&wholesale=" + $("#wholesale").val() + "&type=" + $('#txtType').val(),
                    data: {},
                    dataType: 'html',
                    success: function () {
                        unblockUI();
                        // Set a flag to know if we need to reload profile page after closing the Chart                
                        window.parent.document.getElementById('NeedToReloadPage').value = true;
                        parent.$.fancybox.close();
                        window.parent.location = $('#hdUrlDetail').val();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        unblockUI();
                        //alert(xhr.status + ' ' + thrownError);
                        console.log(xhr.status + ' ' + thrownError);
                    }
                });
            });

            // Each element in this array is a KBB.Karpower.WebServices.LightOption.
            // We add a new element each time the user checks or unchecks an option.
            // The items are in chronological order.
            var optionSelectionHistory = new Array();
            // the user clicked one of the checkboxes in the Optional Equipment checkboxlist
            function OptionChanged(checkbox, optionId) {
                ShowWarningMessage(optionId);
            }

        });

        function GetValuation(vin, mileage, model, trim, type, hasVin) {
            blockUIPopUp();

            $.ajax({
                type: "POST",
                url: "/Karpower/KarPowerResultInSingleMode?vin=" + vin + "&mileage=" + mileage + "&modelId=" + model + "&trimId=" + trim + "&type=" + type + "&hasVin=" + hasVin,
                data: {},
                dataType: 'html',
                success: function (data) {

                    $("#result").html(data);
                    unblockUI();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    unblockUI();
                    //alert(xhr.status + ' ' + thrownError);
                    console.log(xhr.status + ' ' + thrownError);
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
            padding-left: 20px;
			margin: 0 auto;
			margin-top: 20px;
			background: #003399;
		}
		
		input[type="button"], input[type="sumbit"] {background: #3366cc !important; border:0; color:White; padding:3px 6px;}
		input[type="button"]:hover, input[type="sumbit"]:hover {background: #3366cc !important;}
    </style>
        
</head>
<body>
    <div id="divEnterMilage" style="margin-left:0px;">
    <div>
        <%--<form id="data" method="post" action="">--%>
        <div style="text-align:center;">
            <input type="hidden" name="txtModel" id="txtModel" value="<%= (String)ViewData["MODELID"] %>" />
            <input type="hidden" name="txtTrim" id="txtTrim" value="<%= (String)ViewData["TRIMID"] %>" />
            <input type="hidden" name="txtVin" id="txtVin" value="<%= (String)ViewData["VIN"] %>" />
            <input type="hidden" name="txtType" id="txtType" value="<%= (String)ViewData["TYPE"] %>" />
            <input type="hidden" name="txtHasVin" id="txtHasVin" value="<%= (bool)ViewData["HASVIN"] %>" />
            <input type="hidden" name="hdUrlDetail" id="hdUrlDetail" value="<%= ViewData["URLDeatail"] %>" />
            <%--<font style="color:white;">Vin:</font>
            <input type="text" id="txtVin" name="txtVin" maxlength="17" size="30" value="<%= (String)ViewData["VIN"] %>" disabled="disabled" />--%>
            &nbsp; 
            <font style="color:white;">Mileage:</font>
            <input type="text" id="txtMileage" name="txtMileage" maxlength="17" size="17" value="<%= ViewData["MILEAGE"] %>" />
            &nbsp;
            <input type="button" name="btnDecode" id="btnDecode" value="Get Value" />
        </div>
        <%--</form>--%>
    </div>
    <div id="result">
    </div>
    </div>
    <div id="FailedAuthorization" style="display:none;font-size:18px;color:White;">This feature is available with a Karpower account. Please contact Vincontrol support for more detail.</div>
</body>
</html>
