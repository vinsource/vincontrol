<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.EbayFormViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript"></script>

<link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
<link href="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
<title>Ebay Ad Posting Form</title>
</head>

<style>
    input[type="text"], select {
    background-color: #D1D7D4;
    border: 1px solid black;
    height: 20px;
    margin-right: 5px;
}
</style>

<body>
<%if (Model.SessionTimeOut)
  { %>
  
 <script>
   
     parent.$.fancybox.close();
     ShowWarningMessage("Your session is timed out. Please login back again");
     var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
     window.parent.location = actionUrl;

</script>
<% }%>
<script>



    $(document).ready(function () {

        $('#SellerProvidedTitle').keyup(function () {
            var left = 100 - $(this).val().length;
            var text = $(this).val();
            if (left <= 0) {
                left = 0;
                //and if there are use substr to get the text before the limit
                var new_text = text.substr(0, 100);

                //and change the current text with the new text
                $(this).val(new_text);
            }
            $('#charRemain').text(left);
        });

        $("#PreviewEbayForm").submit(function (event) {

            $('#elementID').removeClass('hideLoader');
            $thisval = $('#SelectedAuctionType>option:selected').val();
            if ($("#SellerProvidedTitle") == null || $("#SellerProvidedTitle").val() == '') {
                ShowWarningMessage("Seller provided title is required.");
                $('#elementID').addClass('hideLoader');
                return false;
            }
            else if ($("#SellerProvidedTitle").val().length > 80) {
                ShowWarningMessage("Seller provided title must be less han 80 characters.");
                $('#elementID').addClass('hideLoader');
                return false;
            }
            if ($thisval == 'Chinese' || $thisval == 'ChineseNoBuyItNow') {

                if ($("#StartingPrice").val() == '' || !IsPositiveNumeric($("#StartingPrice").val())) {
                    ShowWarningMessage("Auction starting price for standard auction type is required and positive number");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#ReservePrice").val() == '' || !IsPositiveNumeric($("#ReservePrice").val())) {
                    ShowWarningMessage("Auction reserve price for standard auction type is required and positive number");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }

                else {
                    if (Number($("#StartingPrice").val()) > Number($("#ReservePrice").val())) {
                        ShowWarningMessage("Auction reserve price must be greater than auction starting price in auction");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }
                    else {
                        event.preventDefault();
                        PreviewEbay(this);
                        window.parent.close();

                    }
                }

            }
            else if ($thisval == 'BuyItNowBestOffer') {

                if ($("#MinimumPrice").val() == '' || !IsPositiveNumeric($("#MinimumPrice").val())) {
                    ShowWarningMessage("Auto Decline Price for Buy It Now with Best Offer is required and positive number");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#SellerProvidedTitle").val() == '' || $("#SellerProvidedTitle").val() == null) {
                    ShowWarningMessage("Seller provided title is required.");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#SellerProvidedTitle").val().length > 80) {
                    ShowWarningMessage("Seller provided title must be less or equal than 80 characters.");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else {
                    if (Number($("#MinimumPrice").val()) > Number($("#BuyItNowPrice").val())) {
                        ShowWarningMessage("Auto Decline price must be less than advertised/buy it now price");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    } else {
                        event.preventDefault();


                        PreviewEbay(this);

                    }
                }
            }
         
            else {
                event.preventDefault();


                PreviewEbay(this);

            }





        });

        $("#SelectedAuctionType").change(function () {
            $thisval = $('#SelectedAuctionType>option:selected').val();

            if ($thisval == 'Chinese') {
                $("#SelectedAuctionLength option[value='Days_21']").remove();
                $("#StartingPrice").val("");
                $("#ReservePrice").val("");
                $("#MinimumPrice").val("");
                $("#StartingPrice").removeAttr('readonly');
                $("#ReservePrice").removeAttr('readonly');
                $("#MinimumPrice").attr('readonly', 'readonly');
            }
            else if ($thisval == 'ChineseNoBuyItNow') {
                $("#SelectedAuctionLength option[value='Days_21']").remove();
                $("#StartingPrice").val("");
                $("#ReservePrice").val("");
                $("#MinimumPrice").val("");
                $("#StartingPrice").removeAttr('readonly');
                $("#ReservePrice").removeAttr('readonly');
                $("#MinimumPrice").attr('readonly', 'readonly');
            }
            else if ($thisval == 'BuyItNowBestOffer') {
                $("#StartingPrice").attr('readonly', 'readonly');
                $("#StartingPrice").val("");
                $("#ReservePrice").attr('readonly', 'readonly');
                $("#ReservePrice").val("");
                $("#MinimumPrice").removeAttr('readonly');

            }
            else {
                $("#StartingPrice").attr('readonly', 'readonly');
                $("#StartingPrice").val("");
                $("#ReservePrice").attr('readonly', 'readonly');
                $("#ReservePrice").val("");
                $("#MinimumPrice").attr('readonly', 'readonly');
                $("#MinimumPrice").val("");
                if ($("#SelectedAuctionLength option[value='Days_21']").length == 0)
                    $("#SelectedAuctionLength").append('<option value="Days_21"> 21 days $32.00</option>');
            }

        });

    });


    function SubmitPreview(){
        $('#PreviewEbayForm').submit();
    }

     function IsPositiveNumeric(num) {
         return (num >= 0);

     }

    function PreviewEbay(form) {

        blockUI();

        $.ajax({

            url: form.action,

            type: form.method,

            dataType: "json",

            data: $(form).serialize(),

            success: PreviewEbayClose

        });


    }

    function PreviewEbayClose(result) {
       
        window.parent.close();
        var actionUrl = '<%= Url.Action("PreviewEbayAds", "Market") %>';
        window.parent.PopupNewWindow(actionUrl);
        unblockUI();
    }


</script>
    
<div class="profile_tab_holder" id="profile_ebay_holder" style="display: block">
    <% Html.BeginForm("GetEbayAdsPrice", "Market", FormMethod.Post, new { id = "PreviewEbayForm" }); %>
	<div id="container_right_btn_holder">
		<div id="container_right_btns">
			<a target="_blank" onclick="SubmitPreview();"><div class="btns_shadow profile_ebay_preview_btns">
				Preview Listing
			</div></a>
			
		</div>
	</div>

	<div id="container_right_content" style="min-height: 605px!important;">
		<div class="profile_ebay_top_text">
			The standard insertion fee for an ad eBay Motors can be $50 (Cars, Trucks, RV's/Campers, and Commercial Trucks). This is the base pricing with none of the below options selected.
		</div>
		<div class="profile_ebay_second_text">
			You will be given total pricing information when you preview your generated eBay listing.
		</div>
		<div class="profile_ebay_content_holder">
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Auction Type</div>
				<div class="profile_ebay_items_right">
				    <%=Html.DropDownListFor(x=>x.SelectedAuctionType,Model.AuctionType) %>
				</div>
			</div>
			
		<%--	<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Exterior Color</div>
				<div class="profile_ebay_items_right">
					<%=Html.DropDownListFor(x=>x.SelectedExteriorColor,Model.ExteriorColorList) %>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Interior Color</div>
				<div class="profile_ebay_items_right">
					<%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %>
				</div>
			</div>
			--%>
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Warranty</div>
				<div class="profile_ebay_items_right">
					<%=Html.CheckBoxFor(x=>x.LimitedWarranty) %><span class="subtext">Vehicle has existing factory or dealer warranty.</span>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Listing Options</div>
				<div class="profile_ebay_items_right profile_ebay_listingoptions">
					<%=Html.CheckBoxFor(x=>x.BoldTitle) %><span class="subtext">BoldTitle $5.00</span>
					<%=Html.CheckBoxFor(x=>x.Highlight) %><span class="subtext">Highlight $5.00</span>
                    <%=Html.CheckBoxFor(x=>x.Border) %><span class="subtext">Border $5.00</span>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Listing Packages</div>
				<div class="profile_ebay_items_right">
					<%=Html.CheckBoxFor(x=>x.Propackbundle) %><span class="subtext">ProPackBundle $34.95 (<em>This contains all three above listing options.</em>)</span>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Auction Starting Price</div>
				<div class="profile_ebay_items_right">
					<%=Html.TextBoxFor(x => x.StartingPrice, new { @placeholder = "Insert a Starting Price",@readonly="readonly" })%>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Auction Reserve</div>
				<div class="profile_ebay_items_right">
					<%=Html.TextBoxFor(x => x.ReservePrice, new { @placeholder = "Insert a Reserve Price",@readonly="readonly" })%>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Advertised/Buy It Now Price</div>
				<div class="profile_ebay_items_right">
					<%=Html.TextBoxFor(x => x.BuyItNowPrice, new { @placeholder = "Insert an Advertised Price" })%>
				</div>
			</div>
			
			
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Auction Decline</div>
				<div class="profile_ebay_items_right">
					<%=Html.TextBoxFor(x => x.MinimumPrice, new { @placeholder = "Decline Offers Less Than This Amount",@readonly="readonly" })%>
                    <span class="subtext">For use with "Buy It Now/Best Offer".</span>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Hours to Deposit</div>
				<div class="profile_ebay_items_right">
					<%=Html.DropDownListFor(x=>x.SelectedHoursToDeposit,Model.HoursToDeposit) %>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Auction Length</div>
				<div class="profile_ebay_items_right">
					<%=Html.DropDownListFor(x=>x.SelectedAuctionLength,Model.AuctionLength) %>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Counter</div>
				<div class="profile_ebay_items_right">
					<%=Html.CheckBoxFor(x=>x.HitCounter) %> <span class="subtext">Include Hit Counter</span>
				</div>
			</div>
			
			<div class="profile_ebay_items">
				<div class="profile_ebay_items_left">Seller Provided Title</div>
				<div class="profile_ebay_items_right">
					<div class="profile_ebay_small_text">Remain: <nobr class="text_limit_number"><label id="charRemain">100</label></nobr></div>
					<div class="profile_ebay_small_text">**Limit 80 Characters</div>
					<%=Html.TextAreaFor(x => x.SellerProvidedTitle, 3, 50, new { @class = "profile_ebay_textarea" })%>
				</div>
			</div>
		</div>
	</div>
    <% Html.EndForm(); %>
</div>
<%=Html.HiddenFor(x=>x.ListingId) %>


</body>
</html>
