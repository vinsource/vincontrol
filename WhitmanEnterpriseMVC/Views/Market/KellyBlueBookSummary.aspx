<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.KellyBlueBookViewModel>" %>
<%@ Import Namespace="System.Globalization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<!-- jQuery library - I get it from Google API's -->
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript"></script>
	<title>KBB Market Info</title>
	<style type="text/css">

		html {
			background: #111;
			color: white;
		}
			
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
			
		#form-wrap {
			position:relative;
			background: #333;
			overflow: auto;
			margin: 10px;
			display:block;
			padding: 1em;
			margin-top: 0px;
		}

		table {
			text-align: center; 
			background: #000000 !important; 
			width: 100%;
		}

		td {
			width: 150px;
		}

		.title {
			background: #444;
			color: #ddd;
		}

		.fair {
			background: #0062A0;
		}

		.good {
			background: #00aba9;
		}
		.verygood {
			background-color: green;
		}

		.excellent {
			background-color: #D02C00;
		}

		.trim {
			font-weight: bold;
			color: white;
			background:#222;
		}

		.spacer td {height: 1px;}
	.hideLoader {display: none;}
	.table-head{background-color:#860000}
	</style>
</head>

<body>

 <div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajax-loader1.gif")%>" alt="" />
<%if (Model.SessionTimeOut)
  { %>
 <script>
     $('#elementID').removeClass('hideLoader');
     parent.$.fancybox.close();
     alert("Your session is timed out. Please login back again");
     var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
     window.parent.location = actionUrl;

</script>
<% }%>
</div>

<h1 style="text-align: center;">Kelly Blue Book Market Values</h1>
<h2 style="text-align: center;color: red" ><%=ViewData["CarTitle"]%></h2>
<%if (Model.Success)
  {
       %>
      
<h3 style="text-align: center;">

<% Html.BeginForm("GetKellyBlueBookSummaryAdjustMileage", "Market", FormMethod.Post, new { id = "KellyBlueBookForm" }); %>
			
			Mileage <%=Html.EditorFor(x => x.Mileage)%> 
			
			<input  type="submit" name="AdjustMileageButton" id="AdjustMileageButton" value="Get Value" />
			
			   <%foreach (var trimReport in Model.TrimReportList)
                {%>
			    <h5><%=trimReport.MileageZeroPoint.ToString(CultureInfo.InvariantCulture)%> is the "zero point" which will not affect the value</h5>
			
                <%break;
                }%>
<%=Html.HiddenFor(x=>x.Vin) %>
<%=Html.HiddenFor(x=>x.ListingId) %>
 <% Html.EndForm(); %>
 <%if (Model.TrimReportList.Count() == 1)
   {%>    
<table style="font-size: .8em">
                    <tr>
                         <td><input type="radio" name="SelectValueToPrint" value="Trade In" />Trade In</td>
                        <td><input type="radio" name="SelectValueToPrint" value="WholeSale" />WholeSale</td>
                        <td><input type="radio" name="SelectValueToPrint" value="Retail" />Retail</td>
                        <td><input type="radio" name="SelectValueToPrint" value="Lending" />Lending</td>
                        <td><input  type="button" name="PrintKbbReport" id=<%= Model.TrimReportList.First().VehicleId %> value="Print" onclick="javascript:SubmitPrintForm(<%= Model.TrimReportList.First().VehicleId %>,<%= Model.TrimReportList.First().TrimId %>)" /></td>
                    </tr>
                   
               </table>
                 <% }%> 
</h3>
<div id="form-wrap">
<div id="kbb" class="wrapper">
<%if (Model.TrimReportList.Count() > 1)
  {%>    

<table>
    <tr>
        <td></td>
        <td>Trim</td>
        <td>WholeSale Value</td>
        <td>Retail Value</td>
    </tr>
       <% foreach (var trimReport in Model.TrimReportList)
          { %>
          
           <tr>
               <td class="title trim"> <a onclick="javascript: $('#elementID').removeClass('hideLoader');" href="<%=Url.Content("~/Market/GetKellyBlueBookSummaryByTrim?ListingId=") %><%=Model.ListingId %>&TrimId=<%=trimReport.TrimId %>"><%= ViewData["CarTitle"] %></a></td>
               <td class="title trim"> <a onclick="javascript: $('#elementID').removeClass('hideLoader');" href="<%=Url.Content("~/Market/GetKellyBlueBookSummaryByTrim?ListingId=") %><%=Model.ListingId %>&TrimId=<%=trimReport.TrimId %>"><%= trimReport.TrimName %></a></td>
               <td class="title trim"><a onclick="javascript: $('#elementID').removeClass('hideLoader');" href="<%=Url.Content("~/Market/GetKellyBlueBookSummaryByTrim?ListingId=") %><%=Model.ListingId %>&TrimId=<%=trimReport.TrimId %>"><%= trimReport.WholeSale %></a></td>
               <td class="title trim"><a onclick="javascript: $('#elementID').removeClass('hideLoader');" href="<%=Url.Content("~/Market/GetKellyBlueBookSummaryByTrim?ListingId=") %><%=Model.ListingId %>&TrimId=<%=trimReport.TrimId %>"><%= trimReport.Retail %></a></td>
           </tr>
         
         <% } %>
</table>

<% }else{ %>


	<table>
		
	    <%foreach (var trimReport in Model.TrimReportList.OrderBy(x => x.TrimId))
       {%>
       <tr>
		    <td class="table-head">
		     <% Html.BeginForm("", "", FormMethod.Post, new {id="KBBPrint" + trimReport.VehicleId ,  target = "_blank" }); %>
             <%=Html.HiddenFor(x=>x.Vin) %>
             <%=Html.HiddenFor(x=>x.ListingId) %>
              <%=Html.HiddenFor(x=>x.TrimId) %>
            
            <input  type="button" name="PrintKbbReport" id=<%=trimReport.VehicleId  %> value="Print" onclick="javascript:SubmitPrintForm(<%=trimReport.VehicleId  %>,<%=trimReport.TrimId  %>)" />
            <% Html.EndForm(); %>
		    </td>
			
			<td class="table-head">Type</td>
			<td class="table-head">Fair</td>
			<td class="table-head">Good</td>
			<td class="table-head">Very Good</td>
			<td class="table-head">Excellent</td>
			 
		</tr>
		<tr>
			<td class="title trim" rowspan="11"><%=trimReport.TrimName %></td>
			
			<td class="title">Trade-In</td>
			<td class="fair" id="lblTradeInFairPrice"><%=trimReport.TradeInPrice.TradeInFairPrice %></td>
			<td class="good" id="lblTradeInGoodPrice"><%=trimReport.TradeInPrice.TradeInGoodPrice %></td>
			<td class="verygood" id="lblTradeInVeryGoodPrice"><%=trimReport.TradeInPrice.TradeInVeryGoodPrice %></td>
			<td class="excellent" id="lblTradeInExcellentPrice"><%=trimReport.TradeInPrice.TradeInExcellentPrice %></td>
		
		</tr>
		<tr>
			
			<td class="title">Auction</td>
			<td class="fair"><%=trimReport.AuctionPrice.AuctionFairPrice %></td>
			<td class="good"><%=trimReport.AuctionPrice.AuctionGoodPrice%></td>
			<td class="verygood"><%=trimReport.AuctionPrice.AuctionVeryGoodPrice%></td>
			<td class="excellent"><%=trimReport.AuctionPrice.AuctionExcellentPrice%></td>
		</tr>
		<tr>
			
			<td class="title" >WholeSale</td>
		   <td class="title" colspan="3">Base +/- Mileage Adjustment</td>
		   
		   <td class="title">Total Value</td>
		   
		</tr>
		<tr>
			   <td></td>
			
		   <td class="excellent" colspan="3"><%=trimReport.BaseWholesale%> + <%=trimReport.MileageAdjustment.ToString()%></td>
		   
		   <td class="excellent" id="WholesaleValueTextLabel"><%=trimReport.WholeSale %></td>
		   
		</tr>
		
		<tr>
			<td class="title">Retail</td>
			
			 <td class="title" colspan="3">Base +/- Mileage Adjustment</td>
		   
		   <td class="title">Total Value</td>
		
		
		</tr>
		 <tr>
			 <td></td>
			
		   <td class="excellent" colspan="3"><%=trimReport.BaseRetail%> + <%=trimReport.MileageAdjustment.ToString()%></td>
		   
		   <td class="excellent"><%=trimReport.Retail %></td>
	    </tr>
		

		
		<tr>
			<td></td>
			<td class="title">CPO</td>
		
			<td class="excellent"><%=trimReport.CPO%></td>
			
		
			
			<td class="title">MSRP</td>
		
			<td class="excellent"><%=trimReport.MSRP%></td>
		</tr>
		<%    var optionval = trimReport.OptionValuation.ToList();
              foreach (var v in optionval)
              {
  %>
  <%= v.Id%>-<%= v.Price%>
  <%
              }
           %>
		
	   <tr><td class="title" colspan="5">Standard Options</td></tr>
		<tr>
			
			<td colspan="5">
			    <table>
			<%
                var optionList = trimReport.OptionalEquipment.Where(t=>t.IsSelected).ToList();
                int count = 0;
                while (count < optionList.Count())
                {   
                    var firstOption=  optionList.ElementAt(count);
                    WhitmanEnterpriseMVC.Models.ExtendedEquipmentOption secondOption=null;
                    if(count+1< optionList.Count())
                        secondOption=  optionList.ElementAt(count+1);
                    %>
        <tr>
                        
              
			<td align="left"><input id="<%=firstOption.VehicleOptionId%>" name="<%=firstOption.VehicleOptionId%>" type="checkbox" checked="checked"/><%=firstOption.DisplayName%> </td>
			<%if(secondOption!=null){ %>
				<td align="left"><input id="<%=secondOption.VehicleOptionId%>" name="<%=secondOption.VehicleOptionId%>" type="checkbox" checked="checked"/><%=secondOption.DisplayName%></td>
								
			
			<%
                }
                count=count+2;
                } %>
            
            
         </tr>
         </table>
         </td></tr>
        
         
         <tr><td class="title" colspan="5">Additional Options</td></tr>
		<tr>
			
			<td colspan="5">
			    <table>
			<%
                var additionalOptionList = trimReport.OptionalEquipment.Where(t=>t.IsSelected==false).ToList();
                int addtionalCount = 0;
                while (addtionalCount < additionalOptionList.Count())
                {
                    var firstAddtionalOption = additionalOptionList.ElementAt(addtionalCount);
                    WhitmanEnterpriseMVC.Models.ExtendedEquipmentOption secondAdditonalOption = null;
                    if (addtionalCount + 1 < additionalOptionList.Count())
                        secondAdditonalOption = additionalOptionList.ElementAt(addtionalCount + 1);
                    %>
     <tr>
              <%if (firstAddtionalOption.IsSaved)
                {%>          
              
			<td align="left"><input  id="<%= firstAddtionalOption.VehicleOptionId %>" checked="checked"  name="AdditonalOptions" value="<%= firstAddtionalOption.VehicleOptionId %><%= firstAddtionalOption.PriceAdjustmentForWholeSale %><%= firstAddtionalOption.PriceAdjustmentForRetail %>" onclick="javascript:changeKBBValue(this)" type="checkbox"/><%= firstAddtionalOption.DisplayName%> <%= firstAddtionalOption.PriceAdjustmentForWholeSale%>/<%= firstAddtionalOption.PriceAdjustmentForRetail%></td>
			<% }
                else
                { %>
			<td align="left"><input  id="<%= firstAddtionalOption.VehicleOptionId %>"  name="AdditonalOptions" value="<%= firstAddtionalOption.VehicleOptionId %><%= firstAddtionalOption.PriceAdjustmentForWholeSale %><%= firstAddtionalOption.PriceAdjustmentForRetail %>" onclick="javascript:changeKBBValue(this)" type="checkbox"/><%= firstAddtionalOption.DisplayName%> <%= firstAddtionalOption.PriceAdjustmentForWholeSale%>/<%= firstAddtionalOption.PriceAdjustmentForRetail%></td>
			<% } %>
			<%if (secondAdditonalOption != null)
     { %>
     <%if (secondAdditonalOption.IsSaved)
                {%>  
				<td align="left"><input  id="<%=secondAdditonalOption.VehicleOptionId%>" checked="checked"  name="AdditonalOptions" value="<%=secondAdditonalOption.VehicleOptionId%><%=secondAdditonalOption.PriceAdjustmentForWholeSale%><%=secondAdditonalOption.PriceAdjustmentForRetail%>"  onclick="javascript:changeKBBValue(this)" type="checkbox"/><%=secondAdditonalOption.DisplayName%> <%=secondAdditonalOption.PriceAdjustmentForWholeSale%>/<%=secondAdditonalOption.PriceAdjustmentForRetail%></td>
				<% }
                else
                { %>	
                <td align="left"><input  id="<%= secondAdditonalOption.VehicleOptionId %>"  name="AdditonalOptions" value="<%=secondAdditonalOption.VehicleOptionId%><%=secondAdditonalOption.PriceAdjustmentForWholeSale%><%=secondAdditonalOption.PriceAdjustmentForRetail%>"  onclick="javascript:changeKBBValue(this)" type="checkbox"/><%=secondAdditonalOption.DisplayName%> <%=secondAdditonalOption.PriceAdjustmentForWholeSale%>/<%=secondAdditonalOption.PriceAdjustmentForRetail%></td>			
				<% } %>
			<%
                }
     addtionalCount = addtionalCount + 2;
                } %>
            
            
       </tr>
         </table>
         </td></tr>
     <%} %>
     
        
	
	</table>
</div>
<%}%>
</div>
<%}
  else
  {
       %>
       <h2 style="text-align: center;">There are no Kelly Blue Book Market Values for this vehicle</h2>

       <%} %>
         
        <h6 style="text-align: center;"><%=Model.Disclaimer %></h6>
       
</body>
<script language="javascript">
   
    $("#AdjustMileageButton").click(function() {


        $('#elementID').removeClass('hideLoader');
    });

    $("#btnSaveKbbOption").click(function(event) {

        var checks = $('input[type="checkbox"]');
        var itemoption = "";

        for (i in checks) {
            if (checks[i].checked && checks[i].name == "AdditonalOptions") {
                itemoption += checks[i].id + ",";
            }
        }
        if (itemoption.indexOf(",") != -1) {
            itemoption = itemoption.substring(0, itemoption.length - 1);
        }

        $.post('<%= Url.Content("~/Inventory/SaveKBBOptions") %>', { ListingId: $("#ListingId").val(), OptionSelect: itemoption, TrimId: $("#TrimId").val() }, function(data) {
            if (data == "SessionTimeOut") {

                alert("Your session has timed out. Please login back again");
                var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                window.parent.location = actionUrl;
            } else {

                parent.$.fancybox.close();

            }
        });


    });
    function SubmitPrintForm(id, trimId) {

    var checks = $('input[type="checkbox"]');
        var radios = $('input[type="radio"]');
        var itemoption = "";
        var printOption = "";

        for (i in checks) {
            if (checks[i].checked && checks[i].name == "AdditonalOptions") {
                itemoption += checks[i].id + ",";
            }

        }

        for (i in radios) {

            if (radios[i].checked && radios[i].name == "SelectValueToPrint") {
                printOption = radios[i].value;
                break;
            }
        }
        
        if (itemoption.indexOf(",") != -1) {
            itemoption = itemoption.substring(0, itemoption.length - 1);
        }

        $.post('<%= Url.Content("~/Inventory/SaveKBBOptions") %>', { ListingId: $("#ListingId").val(), OptionSelect: itemoption, TrimId: $("#TrimId").val() }, function(data) {
            if (data == "SessionTimeOut") {

                alert("Your session has timed out. Please login back again");
                var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                window.parent.location = actionUrl;
            } else {

                var name = 'KBBPrint' + id;

                var printOptionFinal = 0;


                if (printOption == "Trade In")
                    printOptionFinal = 1;
                else if (printOption == "WholeSale")
                    printOptionFinal = 2;
                else if (printOption == "Retail")
                    printOptionFinal = 3;
               
                var actionUrl = '<%= Url.Action("ViewKBBReport", "MarketReport", new { KBBVehicleId = "PLACEHOLDER",ListingId="PLACEHOLDERID",TrimId="PLACEHOLDERTRIMID",PrintOption="PLACEHOLDERPRINTOPTION" } ) %>';

                actionUrl = actionUrl.replace('PLACEHOLDER', id);

                actionUrl = actionUrl.replace('PLACEHOLDERTRIMID', trimId);

                actionUrl = actionUrl.replace('PLACEHOLDERID', $('#ListingId').val());

                actionUrl = actionUrl.replace('PLACEHOLDERPRINTOPTION', printOptionFinal);

                parent.$.fancybox.close();

                window.open(actionUrl);


            }
        });


    }


    function changeKBBValue(checkbox) {

        if (checkbox.checked) {
            var optionPrice = checkbox.value;
            var optionWholeSalePrice = optionPrice.substring(optionPrice.indexOf("$"), optionPrice.lastIndexOf("$"));
            var optionRetailPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));

            var baseWholeSaleNumber = Number($("#lblBaseWholeSale").text().replace(/[^0-9\.]+/g, ""));
            var wholeSaleNumber = Number($("#lblWholeSale").text().replace(/[^0-9\.]+/g, ""));
            var baseRetailNumber = Number($("#lblBaseRetail").text().replace(/[^0-9\.]+/g, ""));
            var retailNumber = Number($("#lblRetail").text().replace(/[^0-9\.]+/g, ""));
            var tradeInFairPriceNumber = Number($("#lblTradeInFairPrice").text().replace(/[^0-9\.]+/g, ""));
            var tradeInGoodPriceNumber = Number($("#lblTradeInGoodPrice").text().replace(/[^0-9\.]+/g, ""));
            var tradeInVeryGoodPriceNumber = Number($("#lblTradeInVeryGoodPrice").text().replace(/[^0-9\.]+/g, ""));
            var tradeInExcellentPriceNUmber = Number($("#lblTradeInExcellentPrice").text().replace(/[^0-9\.]+/g, ""));


            var optionWholeSaleValue = Number(optionWholeSalePrice.replace(/[^0-9\.]+/g, ""));
            var optionRetailValue = Number(optionRetailPrice.replace(/[^0-9\.]+/g, ""));


            var basewholesaletotal = Number(baseWholeSaleNumber) + Number(optionWholeSaleValue);
            var wholesaletotal = Number(wholeSaleNumber) + Number(optionWholeSaleValue);
            var baseretailtotal = Number(baseRetailNumber) + Number(optionRetailValue);
            var retailtotal = Number(retailNumber) + Number(optionRetailValue);
            var tradeInFairTotal = Number(tradeInFairPriceNumber) + Number(optionWholeSaleValue);
            var tradeInGoodTotal = Number(tradeInGoodPriceNumber) + Number(optionWholeSaleValue);
            var tradeInVeryGoodTotal = Number(tradeInVeryGoodPriceNumber) + Number(optionWholeSaleValue);
            var tradeInExcellentTotal = Number(tradeInExcellentPriceNUmber) + Number(optionWholeSaleValue);


        }
        else {

            optionPrice = checkbox.value;

            optionWholeSalePrice = optionPrice.substring(optionPrice.indexOf("$"), optionPrice.lastIndexOf("$"));
            optionRetailPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));

            baseWholeSaleNumber = Number($("#lblBaseWholeSale").text().replace(/[^0-9\.]+/g, ""));
            wholeSaleNumber = Number($("#lblWholeSale").text().replace(/[^0-9\.]+/g, ""));
            baseRetailNumber = Number($("#lblBaseRetail").text().replace(/[^0-9\.]+/g, ""));
            retailNumber = Number($("#lblRetail").text().replace(/[^0-9\.]+/g, ""));
            tradeInFairPriceNumber = Number($("#lblTradeInFairPrice").text().replace(/[^0-9\.]+/g, ""));
            tradeInGoodPriceNumber = Number($("#lblTradeInGoodPrice").text().replace(/[^0-9\.]+/g, ""));
            tradeInVeryGoodPriceNumber = Number($("#lblTradeInVeryGoodPrice").text().replace(/[^0-9\.]+/g, ""));
            tradeInExcellentPriceNUmber = Number($("#lblTradeInExcellentPrice").text().replace(/[^0-9\.]+/g, ""));


            optionWholeSaleValue = Number(optionWholeSalePrice.replace(/[^0-9\.]+/g, ""));
            optionRetailValue = Number(optionRetailPrice.replace(/[^0-9\.]+/g, ""));

            basewholesaletotal = Number(baseWholeSaleNumber) - Number(optionWholeSaleValue);
            wholesaletotal = Number(wholeSaleNumber) - Number(optionWholeSaleValue);
            baseretailtotal = Number(baseRetailNumber) - Number(optionRetailValue);
            retailtotal = Number(retailNumber) - Number(optionRetailValue);
            tradeInFairTotal = Number(tradeInFairPriceNumber) - Number(optionWholeSaleValue);
            tradeInGoodTotal = Number(tradeInGoodPriceNumber) - Number(optionWholeSaleValue);
            tradeInVeryGoodTotal = Number(tradeInVeryGoodPriceNumber) - Number(optionWholeSaleValue);
            tradeInExcellentTotal = Number(tradeInExcellentPriceNUmber) - Number(optionWholeSaleValue);
        }


        $("#lblBaseWholeSale").text(formatDollar(basewholesaletotal));
        $("#lblWholeSale").text(formatDollar(wholesaletotal));
        $("#lblBaseRetail").text(formatDollar(baseretailtotal));
        $("#lblRetail").text(formatDollar(retailtotal));
        $("#lblTradeInFairPrice").text(formatDollar(tradeInFairTotal));
        $("#lblTradeInGoodPrice").text(formatDollar(tradeInGoodTotal));
        $("#lblTradeInVeryGoodPrice").text(formatDollar(tradeInVeryGoodTotal));
        $("#lblTradeInExcellentPrice").text(formatDollar(tradeInExcellentTotal));



    }

    function formatDollar(num) {
        var p = num.toFixed(2).split(".");
        return "$" + p[0].split("").reverse().reduce(function(acc, num, i, orig) {
            return num + (i && !(i % 3) ? "," : "") + acc;
        }, "") + "." + p[1];
    }
</script>
</html>
