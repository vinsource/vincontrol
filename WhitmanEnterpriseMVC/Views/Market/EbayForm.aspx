<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.EbayFormViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript"></script>
<title>Ebay Ad Posting Form</title>
<style type="text/css">
		html {
		background: #111;
		color: white;}
		
	body {
		font-family: "Trebuchet MS", Arial, Helvetica, sans-serif; 
		overflow: auto;
		padding-top: 10px; 
		padding-bottom: 30px;
		width: 700px;
		margin: 0 auto;
		margin-top: 10px;
		background: #111;}
		
	label {
		display: inline-block; 
		font-weight: bold;
		width: 220px; 
		text-align:right;
		border-right: 2px solid #bbb; 
		padding: 6px;}
		
	#submit {
		position:absolute;
		font-size: 1.2em;
		font-weight: bold;
		bottom: 10px;
		right: 10px;
		border: 0;
		background: #860000;
		color: white;
		padding: .5em .7em .5em .7em !important;}
	#submit:hover {
		background: #350001;}
	
	#form-wrap {
		position:relative;
		background: #333;
		overflow:hidden
		margin: 10px;
		display:block;
		padding: 1em;
		margin-top: 0px;}
			
	.subtext {
		font-size: .8em;}
	
	#subtotal {
		position: absolute;
		right: 12px;
		bottom: 70px;
		width: 200px;
		text-align: center;
		background: green;
		padding: 10px;}
	
	#subtotal h1 {
		margin-bottom: 0;
		font-size: 3em;}
	
	#disclaimer {
		text-align: center;
		background: #000;
		padding: .1em .7em .1em .7em !important;
		margin-bottom: 10px;
		font-size: .9em;
		}
				
.hideLoader {display: none;}
</style>
</head>

<body>
<%if (Model.SessionTimeOut)
  { %>
  
 <script>
   
     parent.$.fancybox.close();
     alert("Your session is timed out. Please login back again");
     var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
     window.parent.location = actionUrl;

</script>
<% }%>
<script>



    $(document).ready(function () {


        $("#PreviewEbayForm").submit(function (event) {


            $('#elementID').removeClass('hideLoader');
            $thisval = $('#SelectedAuctionType>option:selected').val();
            if ($("#SellerProvidedTitle") == null || $("#SellerProvidedTitle").val() == '') {
                alert("Seller provided title is required.");
                $('#elementID').addClass('hideLoader');
                return false;
            }
            else if ($("#SellerProvidedTitle").val().length > 80) {
                alert("Seller provided title must be less han 80 characters.");
                $('#elementID').addClass('hideLoader');
                return false;
            }
            if ($thisval == 'Chinese') {

                if ($("#StartingPrice").val() == '' || !IsPositiveNumeric($("#StartingPrice").val())) {
                    alert("Auction starting price for standard auction type is required and positive number");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#ReservePrice").val() == '' || !IsPositiveNumeric($("#ReservePrice").val())) {
                    alert("Auction reserve price for standard auction type is required and positive number");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }

                else {
                    if (Number($("#StartingPrice").val()) > Number($("#ReservePrice").val())) {
                        alert("Auction reserve price must be greater than auction starting price in auction");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }
                    else {
                        event.preventDefault();


                        PreviewEbay(this);
                        window.parent.close();
                        var actionUrl = '<%= Url.Action("PreviewEbayAds", "Market") %>';
                        window.parent.location.href = actionUrl;
                    }
                }

            }
            else if ($thisval == 'BuyItNowBestOffer') {
              
                if ($("#MinimumPrice").val() == '' || !IsPositiveNumeric($("#MinimumPrice").val())) {
                    alert("Auto Decline Price for Buy It Now with Best Offer is required and positive number");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#SellerProvidedTitle").val() == '' || $("#SellerProvidedTitle").val() == null) {
                    alert("Seller provided title is required.");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#SellerProvidedTitle").val().length > 80) {
                    alert("Seller provided title must be less or equal than 80 characters.");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else {
                    if (Number($("#MinimumPrice").val()) > Number($("#BuyItNowPrice").val())) {
                        alert("Auto Decline price must be less than advertised/buy it now price");
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

    });

     function IsPositiveNumeric(num) {
         return (num >= 0);

     }

    function PreviewEbay(form) {



        $.ajax({

            url: form.action,

            type: form.method,

            dataType: "json",

            data: $(form).serialize(),

            success: PreviewEbayClose

        });


    }

    function PreviewEbayClose(result) {
        //console.log(result);
        window.parent.close();
        var actionUrl = '<%= Url.Action("PreviewEbayAds", "Market") %>';
         window.parent.location.href = actionUrl;

    }


</script>
 <div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajax-loader1.gif")%>" alt="" />
</div>
<h1 style="text-align: center;margin-top:0px">eBay Essential Information</h1>
<div id="form-wrap">
<div id="disclaimer">
	<p>
    	The standard insertion fee for an ad eBay Motors can be <b>$50</b> (Cars, Trucks, RV's/Campers, and Commercial Trucks). This is the base pricing with none of the below options selected.</p><p style="background: #860000; padding: 0; padding: 10px;"><b>You will be given total pricing information when you preview your generated eBay listing.</b>
    </p>
</div>
 <% Html.BeginForm("GetEbayAdsPrice", "Market", FormMethod.Post, new { id = "PreviewEbayForm" }); %>
	<legend></legend>
     
    <label>Auction Type</label> 
    <%=Html.DropDownListFor(x=>x.SelectedAuctionType,Model.AuctionType) %>
    <br />
    <label>Exterior Color</label> 
    <%=Html.DropDownListFor(x=>x.SelectedExteriorColor,Model.ExteriorColorList) %>
    <br />
    <label>Interior Color</label> 
    <%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %>
    <br />
    <label>Warranty</label>
    <%=Html.CheckBoxFor(x=>x.LimitedWarranty) %><span class="subtext">Vehicle has existing factory or dealer warranty.</span>
	<br />
   <label>Listing Options</label>
    <%=Html.CheckBoxFor(x=>x.BoldTitle) %><span class="subtext">BoldTitle $5.00</span>
    <%=Html.CheckBoxFor(x=>x.Highlight) %><span class="subtext">Highlight $5.00</span>
    <%=Html.CheckBoxFor(x=>x.Border) %><span class="subtext">Border $5.00</span>
    <label>Listing Packages</label>
    <%=Html.CheckBoxFor(x=>x.Propackbundle) %><span class="subtext">ProPackBundle $34.95 (<em>This contains all three above listing options.</em>)</span>
    <br />
    <label>Auction Starting Price</label>
    <%=Html.TextBoxFor(x => x.StartingPrice, new { @placeholder = "Insert a Starting Price" })%>
    
    <br />
    <label>Auction Reserve</label>
    <%=Html.TextBoxFor(x => x.ReservePrice, new { @placeholder = "Insert a Reserve Price" })%>
    <span class="subtext">For use with "Standard Auction Listing".</span>
    <br />
    <label>Advertised/Buy It Now Price</label>
     <%=Html.TextBoxFor(x => x.BuyItNowPrice, new { @placeholder = "Insert an Advertised Price" })%>
    <br />
    <label>Auto Decline</label>
     <%=Html.TextBoxFor(x => x.MinimumPrice, new { @placeholder = "Decline Offers Less Than This Amount" })%>
     <span class="subtext">For use with "Buy It Now/Best Offer".</span>
    <br />
    <!--
    <label>Paypal</label>
     <%=Html.CheckBoxFor(x=>x.PaPalDeposit) %><span class="subtext">Require PayPal deposit.</span>
    <br />-->
    <label>Hours to Deposit</label>
     <%=Html.DropDownListFor(x=>x.SelectedHoursToDeposit,Model.HoursToDeposit) %>
    <br />
    <label>Auction Length</label>
   <%=Html.DropDownListFor(x=>x.SelectedAuctionLength,Model.AuctionLength) %>
    <br />
  
    <label>Counter</label>
     <%=Html.CheckBoxFor(x=>x.HitCounter) %> <span class="subtext">Include Hit Counter</span>
    <br />
    <label>Seller Provided Title *</label> <%=Html.TextAreaFor(x => x.SellerProvidedTitle,5,50,null)%>
    <label></label>
    <div style="font-size: .6em;color: red" >***Limit 80 characters</div>
    <br />
      <%=Html.HiddenFor(x=>x.ListingId) %>
    <%=Html.SubmitButton("Preview Listing")%>
    
  <% Html.EndForm(); %>
</div>
<script type="text/javascript">

    $("#SelectedAuctionType").change(function() {
        $thisval = $('#SelectedAuctionType>option:selected').val();
        if ($thisval == 'Chinese') {
            $("#SelectedAuctionLength option[value='Days_21']").remove();
        }
        else {
            if ($("#SelectedAuctionLength option[value='Days_21']").length == 0)
                $("#SelectedAuctionLength").append('<option value="Days_21"> 21 days $32.00</option>');
        }

    });
</script>

</body>
</html>
