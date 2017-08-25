<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AdminBuyerGuideViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Buyer's Guide</title>
    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ckeditor/ckeditor.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ckeditor/adapters/jquery.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {

             $.unblockUI();

            CKEDITOR.replace('SystemCovered',
            {
                height: 300,
                width: 450
            });

            CKEDITOR.replace('Durations',
            {
                height: 300,
                width: 450
            });

            CKEDITOR.replace('SystemCoveredAndDurations',
            {
                height: 300,
                width: 945
            });

            if ($("#IsAsWarranty").val() == "True")
                $("#checkboxWarrantyType").html('✔');

            if ($("#IsWarranty").val() == "True")
                $("#warranty").html('✔');

            if ($("#IsFullWarranty").val() == "True")
                $("#fullWarranty").html('✔');

            if ($("#IsLimitedWarranty").val() == "True")
                $("#limitedWarranty").html('✔');

            if ($("#IsServiceContract").val() == "True")
                $("#serviceContract").html('✔');

            if ($("#IsMixed").val() == "True") {
                $("#combineSystemCoveredAndDurations").html('✔');
                $("#areaSystemCovered").slideUp("slow");
                $("#areaDurations").slideUp("slow");
                $("#areaSystemCoveredAndDurations").slideDown("slow");
            }

            $("span#btnSave").click(function() {
                $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajax-loader1.gif") %>"  /></div>', css: { width: '350px', backgroundColor: 'none', border: 'none'} });

                //                $.ajax({
                //                    type: "POST",
                //                    url: "/Report/CreateBuyerGuide",
                //                    data: $("form").serialize(),
                //                    success: function(results) {
                //                         $.unblockUI();
                //                        alert(results);
                //                    }
                //                });
                $("#IsPreview").val('False');
                $("form").submit();
            });

            $("span#btnPreview").click(function() {
                $("#IsPreview").val('True');
                $("form").submit();
            });

            $("div#checkboxWarrantyType").click(function() {
                var isAswarranty = $("#IsAsWarranty").val();
                if (isAswarranty == "" || isAswarranty == "False") {
                    $("#checkboxWarrantyType").html('✔');
                    $("#IsAsWarranty").val('True');
                }
                else {
                    $("#checkboxWarrantyType").html('');
                    $("#IsAsWarranty").val('False');
                }
            });

            $("div#warranty").click(function() {
                var isWarranty = $("#IsWarranty").val();
                if (isWarranty == "" || isWarranty == "False") {
                    $("#warranty").html('✔');
                    $("#IsWarranty").val('True');
                }
                else {
                    $("#warranty").html('');
                    $("#IsWarranty").val('False');
                }
            });

            $("div#fullWarranty").click(function() {
                var isFullWarranty = $("#IsFullWarranty").val();
                if (isFullWarranty == "" || isFullWarranty == "False") {
                    $("#fullWarranty").html('✔');
                    $("#IsFullWarranty").val('True');
                }
                else {
                    $("#fullWarranty").html('');
                    $("#IsFullWarranty").val('False');
                }
            });

            $("div#limitedWarranty").click(function() {
                var isLimitedWarranty = $("#IsLimitedWarranty").val();
                if (isLimitedWarranty == "" || isLimitedWarranty == "False") {
                    $("#limitedWarranty").html('✔');
                    $("#IsLimitedWarranty").val('True');
                }
                else {
                    $("#limitedWarranty").html('');
                    $("#IsLimitedWarranty").val('False');
                }
            });

            $("div#serviceContract").click(function() {
                var isServiceContract = $("#IsServiceContract").val();
                if (isServiceContract == "" || isServiceContract == "False") {
                    $("#serviceContract").html('✔');
                    $("#IsServiceContract").val('True');
                }
                else {
                    $("#serviceContract").html('');
                    $("#IsServiceContract").val('False');
                }
            });

            $("div#combineSystemCoveredAndDurations").click(function() {
                var isMixed = $("#IsMixed").val();
                if (isMixed == "" || isMixed == "False") {
                    $("#combineSystemCoveredAndDurations").html('✔');
                    $("#IsMixed").val('True');
                    $("#areaSystemCovered").slideUp("slow");
                    $("#areaDurations").slideUp("slow");
                    $("#areaSystemCoveredAndDurations").slideDown("slow");
                }
                else {
                    $("#combineSystemCoveredAndDurations").html('');
                    $("#IsMixed").val('False');
                    $("#areaSystemCovered").slideDown("slow");
                    $("#areaDurations").slideDown("slow");
                    $("#areaSystemCoveredAndDurations").slideUp("slow");
                }
            });

            if ($("#Message").val() != "")
                alert($("#Message").val());

        });
</script>    
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/buy-guide.css")%>" />
    <style type="text/css">
    .checkbox, .sml-checkbox { cursor: pointer; }
    .submit {
        background: none repeat scroll 0 0 #860000;
        border: medium none #000000;
        color: #FFFFFF;
        cursor: pointer;
        display: inline-block;
        font-size: 14px;
        font-weight: normal;
        padding: 2px 8px;
        width: 100px;
        text-align: center;
        padding: 8px 2px;
    }
    </style>
</head>
<body>
<form id="buyerGuide" method="post" action="">
<input type="hidden" name="IsPreview" id="IsPreview" value="<%= Model.IsPreview %>" />
<input type="hidden" name="Message" id="Message" value="<%= Model.Message %>" />
<input type="hidden" name="WarrantyType" id="WarrantyType" value="<%= Model.WarrantyType %>" />
<input type="hidden" name="IsAsWarranty" id="IsAsWarranty" value="<%= Model.IsAsWarranty %>" />
<input type="hidden" name="IsManufacturerWarranty" id="IsManufacturerWarranty" value="<%= Model.IsManufacturerWarranty %>" />
<input type="hidden" name="IsDealerCertified" id="IsDealerCertified" value="<%= Model.IsDealerCertified %>" />
<input type="hidden" name="IsManufacturerCertified" id="IsManufacturerCertified" value="<%= Model.IsManufacturerCertified %>" />
<input type="hidden" name="IsWarranty" id="IsWarranty" value="<%= Model.IsWarranty %>" />
<input type="hidden" name="IsFullWarranty" id="IsFullWarranty" value="<%= Model.IsFullWarranty %>" />
<input type="hidden" name="IsLimitedWarranty" id="IsLimitedWarranty" value="<%= Model.IsLimitedWarranty %>" />
<input type="hidden" name="IsServiceContract" id="IsServiceContract" value="<%= Model.IsServiceContract %>" />
<input type="hidden" name="IsMixed" id="IsMixed" value="<%= Model.IsMixed %>" />
<input type="hidden" name="SelectedLanguage" id="SelectedLanguage" value="<%= Model.SelectedLanguage %>" />

<div class="wrapper" id="buyer-guide" style="margin-left:-110px;">
    <div style="margin-top: 5px; margin-bottom: 10px;">
        <span id="btnPreview" class="submit">Preview</span> <span id="btnSave" class="submit">
            Save Changes</span>
    </div>
  <div class="print-wrap">
    <div class="header">
      <h1>Buyers Guide</h1>
      <h3>IMPORTANT: Spoken promises are difficult to enforce. Ask the dealer to put all promises in writing. Keep this form.</h3>
    </div>
    <div class="vehicle-info">
      <div class="item make">
        <div class="input">        
        <%= Html.TextBoxFor(m => m.Make, new { style = "border-bottom:none; width: 220px;" })%>
        </div>Vehicle Make
      </div>
      <div class="item model">
        <div class="input">
        <%= Html.TextBoxFor(m => m.VehicleModel, new { style = "border-bottom:none; width: 220px;" })%>        
        </div>Model
      </div>
      <div class="item year">
        <div class="input">
        <%= Html.TextBoxFor(m => m.Year, new { style = "border-bottom:none; width: 220px;" })%>
        </div>Year
      </div>
      <div class="item vin">
        <div class="input">
        <%= Html.TextBoxFor(m => m.Vin, new { style = "border-bottom:none; width: 220px;" })%>
        </div>VIN Number
      </div>
      <div class="item stock">
        <div class="input">
        <%= Html.TextBoxFor(m => m.StockNumber, new { style = "border-bottom:none; width: 220px;" })%>
        </div>Dealer Stock Number (Optional)
      </div>
    </div>
    <h3 style="font-size:0.5em;margin-bottom:10px;" class="warranty-header">Warranties for this vehicle:</h3>
    <div class="as-is">
      <div class="checkbox" id="checkboxWarrantyType">      
      </div>
      <h3>AS IS - NO WARRANTY</h3>
      <p style="margin-top:0px; font-size:0.4em">YOU WILL PAY ALL COSTS FOR ANY REPAIRS. The dealer assumes no responsibility for any repairs regardless of any oral statements about the vehicle.</p>
    </div>
    <div class="warranty">
      <div class="checkbox" id="warranty"></div>
      <h3>WARRANTY</h3>
      <div class="warranty-type">
        <div class="sml-checkbox" id="fullWarranty"></div> <p style="font-size:0.5em">FULL</p>
        <div class="sml-checkbox" id="limitedWarranty"></div> <p style="margin-top:0px; font-size:0.4em">LIMITED WARRANTY - The dealer will pay 
        <%= Html.TextBoxFor(m => m.PercentageOfLabor, new { style = "border-bottom:none; width: 50px;" })%>
        % of the labor and 
        <%= Html.TextBoxFor(m => m.PercentageOfPart, new { style = "border-bottom:none; width: 50px;" })%>
        % of the parts for the covered systems that fail during the warranty period. Ask the dealer for a copy of the warranty document for full explanation of warranty coverage, exclusions, and the dealer's repair obligations. Under state law, 'implied warranties' may give you even more rights.</p>
      </div>
      <div class="warranty-info">
        <div style="font-size: 0.4em; margin-top: 60px;" class="coverage two-column">
          <p>
          </p><h4>SYSTEMS COVERED</h4>          
          <div style="display: inline-block; height: 30px; width: 100%;">
          <div class="sml-checkbox" style="font-size:0.8em; width:20px; height: 20px;" id="combineSystemCoveredAndDurations"></div> 
          <p style="font-size:0.8em; margin-top: 5px;">COMBINE SYSTEMS COVERED & DURATIONS</p>
          </div>
          <div id="areaSystemCovered">
          <%= Html.TextAreaFor(m => m.SystemCovered, new { cols = "50" })%>
          </div>
          <%--<p></p>--%>
        </div>
        <div style="margin-top:60px; font-size:0.4em" class="duration two-column">
          <p>
          </p><h4>DURATION</h4>
          <div style="display: inline-block; height: 30px; width: 100%;"></div>
          <div id="areaDurations">
          <%= Html.TextAreaFor(m => m.Durations, new { cols = "50" })%>
          </div>
          <%--<p></p>--%>
        </div>
          
      </div>
        <div style="margin-top: -60px; margin-bottom: 50px; font-size: 0.4em; display: none;" id="areaSystemCoveredAndDurations">
            <%= Html.TextAreaFor(m => m.SystemCoveredAndDurations, new { cols = "120" })%>
        </div>
      <div style="float: right; display:inline-block; width: 100%;">
      <%= Html.TextBoxFor(m => m.PriorRental, new { style = "border-bottom:none; width: 240px; font-size:0.7em; text-transform:uppercase; float: right" })%>
      </div>
      <div style="margin-top:0px; font-size:0.4em" class="contract" >
        <div style="font-size:1.2em" class="sml-checkbox" id="serviceContract"></div> SERVICE CONTRACT. A service contract is available at an extra charge on this vehicle. Ask for details as to coverage, deductible, price and exclusions. If you buy a  service contract within 90 days of the tiem of sale, state law 'implied warranties' may give you additional rights.
        <br/><br/>
        PRE PURCHASE INSPECTION. ASK THE DEALER IF YOU MAY HAVE THIS VEHICLE INSPECTED BY YOUR MECHANIC EITHER ON OR OFF THE LOT.
        <br/><br/>
        SEE THE BACK OF THIS FORM for important additional information, including a list of some major defects that may occur in used motor vehicles.
      </div>
    </div>
  </div>
</div>
</form>

</body></html>
