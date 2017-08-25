<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Suggested Retail Price Confirmation</title>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.cookie.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var warningMessage = "Please change Vin or Mileage, then click Get Value again!";
        var fileDownloadCheckTimer;
        var originalWholeSale = 0;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#DownloadTokenValueId').val(token);
            $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '300px', backgroundColor: 'none', border: 'none'} });
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
            GetValuation($('#txtVin').val(), $('#txtMileage').val(), $('#txtTrim').val(), $('#txtType').val(), $('#txtHasVin').val());


            $("#SelectedEngineId").live("change", function() {

                if ($("#SelectedTransmissionId").val() == 0) return false;

                $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingTrimAndTransmission?trimId=" + $("#SelectedTrimId").val() + "&transmissionId=" + $("#SelectedTransmissionId").val(),
                    data: {},
                    dataType: 'html',
                    success: function(data) {

                        $("#result").html(data);
                        $.unblockUI();
                    },
                    error: function(xhr, ajaxOptions, thrownError) {
                        $.unblockUI();
                        alert(xhr.status + ' ' + thrownError);
                    }
                });
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

                if ($("#SelectedTransmissionId").val() == 0) return false;

                $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

                $.ajax({
                    type: "POST",
                    url: "/Market/UpdateValuationByChangingTrimAndTransmission?trimId=" + $("#SelectedTrimId").val() + "&transmissionId=" + $("#SelectedTransmissionId").val(),
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

            $("#SelectedDriveTrainId").live("change", function () {
                //alert(warningMessage);
                return false;
            });
            $('input[name^="option_"]').live("click", function () {
                var listingId = this.id.split('_')[1];
                var isChecked = $(this).is(":checked");

                $.ajax({
                    type: "GET",
                    url: "/Market/UpdateValuationByOptionalEquipmentForBucketJump?listingId=" + listingId + "&isChecked=" + isChecked,
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
                var selectedOptions = "";
                var plus = $("input:radio[name='radioPlus']:checked").val() == "" || $("input:radio[name='radioPlus']:checked").val() == undefined ? "0" : $("input:radio[name='radioPlus']:checked").val();

                var wholesaleWithoutOptions = originalWholeSale;
                var wholesaleWithOptions = $('#txtBaseWholesale').html();
                $("input:checkbox[name^=option_]:checked").each(function () {
                    selectedOptions += $(this).val() + ", ";
                    //selectedOptions.push($(this).val());
                });

                var url = "/PDF/PrintBucketJumpWithKarPowerOptions?listingId=" + $('#txtListingId').val();
                url += "&dealer=" + $('#txtDealer').val();
                url += "&price=" + $('#txtPrice').val();
                url += "&year=" + $('#txtYear').val();
                url += "&make=" + $('#txtMake').val();
                url += "&model=" + $('#txtModel').val();
                url += "&color=" + $('#txtColor').val();
                url += "&miles=" + $('#txtMileage').val();
                url += "&plusPrice=" + plus;
                url += "&certified=" + $("#chkCertified").is(":checked");
                url += "&wholesaleWithoutOptions=" + wholesaleWithoutOptions;
                url += "&wholesaleWithOptions=" + wholesaleWithOptions;
                url += "&options=" + selectedOptions;

           
                window.location.href = url;
            });

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

            GetValuation($('#txtVin').val(), $('#txtMileage').val(), $('#txtTrim').val(), $('#txtType').val(), $('#txtHasVin').val());

        });

        function GetValuation(vin, mileage, trim, type, hasVin) {
            $.blockUI({ message: '<div><img src="../../images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
            console.log("/Market/KarPowerResultForBucketJump?vin=" + vin + "&mileage=" + mileage + "&trimId=" + trim + "&type=" + type + "&hasVin=" + hasVin);
            $.ajax({
                type: "POST",
                url: "/Market/KarPowerResultForBucketJump?vin=" + vin + "&mileage=" + mileage + "&trimId=" + trim + "&type=" + type + "&hasVin=" + hasVin,
                data: {},
                dataType: 'html',
                success: function (data) {

                    $("#result").html(data);
                    originalWholeSale = $('#BaseWholesaleWithoutOptions').val();
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
           <input type="hidden" name="txtTrim" id="txtTrim" value="<%= ViewData["TRIMID"] %>" />
        <input type="hidden" name="txtListingId" id="txtListingId" value="<%= (String)ViewData["LISTINGID"] %>" />
        <input type="hidden" name="txtVin" id="txtVin" value="<%= (String)ViewData["VIN"] %>" />
        <input type="hidden" id="txtMileage" name="txtMileage" value="<%= (String)ViewData["MILEAGE"] %>" />
        <input type="hidden" id="txtDealer" name="txtDealer" value="<%= (String)ViewData["DEALER"] %>" />
        <input type="hidden" id="txtPrice" name="txtPrice" value="<%= (String)ViewData["PRICE"] %>" />
        <input type="hidden" id="txtYear" name="txtYear" value="<%= (String)ViewData["YEAR"] %>" />
        <input type="hidden" id="txtMake" name="txtMake" value="<%= (String)ViewData["MAKE"] %>" />
        <input type="hidden" id="txtModel" name="txtModel" value="<%= (String)ViewData["MODEL"] %>" />
        <input type="hidden" id="txtColor" name="txtColor" value="<%= (String)ViewData["COLOR"] %>" />
        <input type="hidden" name="txtType" id="txtType" value="<%= (String)ViewData["TYPE"] %>" />
          <input type="hidden" name="txtHasVin" id="txtHasVin" value="<%= (bool)ViewData["HASVIN"] %>" />
        <%--</form>--%>
    </div>
        <table border="0" cellpadding="2" cellspacing="2" width="750px" style="color:Black;background-color:White;">
            <tr>
                <td>
                    <div><input type="radio" id="radioPlus5" name="radioPlus" value="5" />Plus 5%</div>
                    <div style="margin-left:20px;">(0-10k)</div>
                </td>
                <td>
                    <div><input type="radio" id="radioPlus4" name="radioPlus" value="4" />Plus 4%</div>
                    <div style="margin-left:20px;">(10k-21k)</div>
                </td>
                <td>
                    <div><input type="radio" id="radioPlus3" name="radioPlus" value="3" />Plus 3%</div>
                    <div style="margin-left:20px;">(21k-30k)</div>
                </td>
                <td>
                    <div><input type="radio" id="radioPlus2" name="radioPlus" value="2" />Plus 2%</div>
                    <div style="margin-left:20px;">(>31k)</div>
                </td>
                <td><div><input type="checkbox" id="chkCertified" name="chkCertified"/>Certified</div><div>&nbsp;</div></td>
            </tr>
        </table>
    <div id="result">
    </div>
    </div>
</body>
</html>
