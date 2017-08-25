<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.BlackBookViewModel>" %>

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
	</style>
</head>

<body>
 <div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajax-loader1.gif")%>" alt="" />
</div>
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
<h1 style="text-align: center;">Black Book Market Values</h1>
<%if (Model.Success)
  {
       %>
<h3 style="text-align: center;">
<% Html.BeginForm("GetBlackBookSummaryAdjustMileage", "Market", FormMethod.Post, new { id = "BlackBookForm" }); %>
			
			Mileage <%=Html.EditorFor(x => x.Mileage)%> 
			<input  type="submit" name="AdjustMileageButton" id="AdjustMileageButton" value="Get Value" />
<%=Html.HiddenFor(x => x.Vin)%>
</h3>
<div id="form-wrap">
<div id="kbb" class="wrapper">
	<table>
		<tr>
			<td></td>
			<td>Type</td>
			<td>Rough</td>
			<td>Average</td>
			<td>Clean</td>
			<td>Extra Clean</td>
			
		</tr>
		   <%foreach (WhitmanEnterpriseMVC.Models.BlackBookTrimReport trimReport in Model.TrimReportList)
       {%>
		<tr>
			<td class="title trim" rowspan="12"><%=trimReport.TrimName%></td>
			<td class="title">Base Wholesale</td>
			<td class="fair"><%=trimReport.BaseWholeSaleRough%></td>
			<td class="good"><%=trimReport.BaseWholeSaleAvg%></td>
			<td class="verygood"><%=trimReport.BaseWholeSaleClean%></td>
			<td class="excellent"><%=trimReport.BaseWholeSaleExtraClean%></td>
		</tr>
		<tr>
			<td class="title">MiAdj For WS</td>
			<td class="fair"><%=trimReport.MileageAdjWholeSaleRough%></td>
			<td class="good"><%=trimReport.MileageAdjWholeSaleAvg%></td>
			<td class="verygood"><%=trimReport.MileageAdjWholeSaleClean%></td>
			<td class="excellent"><%=trimReport.MileageAdjWholeSaleExtraClean%></td>
		</tr>
		<tr>
			<td class="title">WholeSale</td>
			<td class="fair"><%=trimReport.WholeSaleRough%></td>
			<td class="good"><%=trimReport.WholeSaleAvg%></td>
			<td class="verygood"><%=trimReport.WholeSaleClean%></td>
			<td class="excellent"><%=trimReport.WholeSaleExtraClean%></td>
		</tr>
		<tr>
			<td class="title">AutoAdj For WS</td>
			<td class="fair"><%=trimReport.ManualOrAutomaticAdjWholeSaleRough%></td>
			<td class="good"><%=trimReport.ManualOrAutomaticAdjWholeSaleAvg%></td>
			<td class="verygood"><%=trimReport.ManualOrAutomaticAdjWholeSaleClean%></td>
			<td class="excellent"><%=trimReport.ManualOrAutomaticAdjWholeSaleExtraClean%></td>
		</tr>
		<tr>
			<td class="title">Base Retail</td>
			<td class="fair"><%=trimReport.BaseRetailRough%></td>
			<td class="good"><%=trimReport.BaseRetailAvg%></td>
			<td class="verygood"><%=trimReport.BaseRetailClean%></td>
			<td class="excellent"><%=trimReport.BaseRetailExtraClean%></td>
		</tr>
		<tr>
			<td class="title">MiAdj For Retail</td>
			<td class="fair"><%=trimReport.MileageAdjRetailRough%></td>
			<td class="good"><%=trimReport.MileageAdjRetailAvg%></td>
			<td class="verygood"><%=trimReport.MileageAdjRetailClean%></td>
			<td class="excellent"><%=trimReport.MileageAdjRetailExtraClean%></td>
		</tr>
		<tr>
			<td class="title">Retail</td>
			<td class="fair"><%=trimReport.RetailRough%></td>
			<td class="good"><%=trimReport.RetailAvg%></td>
			<td class="verygood"><%=trimReport.RetailaClean%></td>
			<td class="excellent"><%=trimReport.RetailExtraClean%></td>
		</tr>
		
		<tr>
			<td class="title">AutoAdj For Retail</td>
			<td class="fair"><%=trimReport.ManualOrAutomaticAdjRetailRough%></td>
			<td class="good"><%=trimReport.ManualOrAutomaticAdjRetailAvg%></td>
			<td class="verygood"><%=trimReport.ManualOrAutomaticAdjRetailClean%></td>
			<td class="excellent"><%=trimReport.ManualOrAutomaticAdjRetailExtraClean%></td>
		</tr>
		<tr>
			<td class="title">Base Trade-In</td>
			<td class="fair"><%=trimReport.BaseTradeInRough%></td>
			<td class="good"><%=trimReport.BaseTradeInAvg%></td>
			<td class="verygood"><%=trimReport.BaseTradeInClean%></td>
		
		</tr>
			<tr>
			<td class="title">MiAdj For Trade-In</td>
			<td class="fair"><%=trimReport.MileageAdjTradeInRough%></td>
			<td class="good"><%=trimReport.MileageAdjTradeInAvg%></td>
			<td class="verygood"><%=trimReport.MileageAdjTradeInClean%></td>
			
		</tr>
		<tr>
			<td class="title">Trade-In</td>
			<td class="fair"><%=trimReport.TradeInRough%></td>
			<td class="good"><%=trimReport.TradeInAvg%></td>
			<td class="verygood"><%=trimReport.TradeInClean%></td>
			
		</tr>
	    <tr>
			<td class="title">AutoAdj For Trade-In</td>
			<td class="fair"><%=trimReport.ManualOrAutomaticAdjTradeInRough%></td>
			<td class="good"><%=trimReport.ManualOrAutomaticAdjTradeInAvg%></td>
			<td class="verygood"><%=trimReport.ManualOrAutomaticAdjTradeInClean%></td>
			
		</tr>
		<tr class="spacer">
			<td colspan="6"></td>
		</tr>
		<%} %>
	</table>
</div>
</div>
<%}
  else
  {
       %>
       <h2 style="text-align: center;">There are no Black Book Market Values for this vehicle</h2>

       <%} %>
</body>
<script language="javascript">

    $("#AdjustMileageButton").click(function() {


        $('#elementID').removeClass('hideLoader');
    });
    
</script>
</html>
