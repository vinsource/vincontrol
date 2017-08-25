<%@ Page Title="Trade In" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.TradeInVehicleModel>" %>
<%@ Import Namespace="Vincontrol.Web.HelperClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Trade-In Value</title>
	<link href="<%=Url.Content("~/Css/TradeIn/style.css")%>" rel="stylesheet" type="text/css" />
	<%--<script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>--%>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
<!--[if lte IE 7]>
<link href="<%=Url.Content("~/Css/TradeIn/ie-7.css")%>" rel="stylesheet" type="text/css" />
	
	<![endif]-->

	<!--[if lt IE 9]>
	<style type="text/css">
		#market-data .prices span {
			width: 112px !important;
			margin-right: 110px;
		}

		#market-data .prices span.end{
			margin: 0;
			margin-left: -8px;
		}
	</style>
	<![endif]-->
</head>
<body>
    
      <%--<p><%=Model.VehicleId %></p>--%>
      
   <%-- <%=Model.SelectedOptions %>--%>
    
    
    <div id="container" class="step5">

		<div id="header">

			<div class="logo"></div>

			<div class="mask">
				<div class="text-wrap">
					<h1>Get Your Trade In Value!</h1>
				</div>
			</div>
			<div class="steps">
			<div id="step-1" class="step"><img src="<%= Url.Content("~/images/on-step-1.png")%>" alt="step 1"/></div>
				<div id="step-2" class="step"><img src="<%= Url.Content("~/images/on-step-2.png")%>" alt="step 2"/></div>
				<div id="step-3" class="step"><img src="<%= Url.Content("~/images/on-step-3.png")%>" alt="step 3"/></div>
			</div>
		</div>
		
		<div class="slide-wrapper">

			<div class="info-wrap">
				<h3 class="description-header">Your Vehicle's Trade-In Value</h3>
				<div id="market-data" class="info-box">
					<div class="row">
						<div id="chart" class="row-cell">
							<h3 class="box-header">Market Value</h3>
							<div class="chart">
								<img src="<%= Url.Content("~/images/chart.jpg")%>" />
							
							<div class="prices">
									<span class="start">Call Dealer</span>
									<%if (String.IsNullOrEmpty(Model.TradeInFairPrice) || Model.TradeInFairPrice.Equals("NA") || Model.TradeInFairPrice.Equals("$0"))
           { %>
									
									<span class="start">Call Dealer</span>
									<% }
           else
           {
  
                                      %>
									<span ><%=Model.TradeInFairPrice%></span>
									<%} %>
									<%if (String.IsNullOrEmpty(Model.TradeInGoodPrice) || Model.TradeInGoodPrice.Equals("NA") || Model.TradeInGoodPrice.Equals("$0"))
           { %>
									
									<span class="start">Call Dealer</span>
									<% }
           else
           {
  
                                      %>
									<span><%=Model.TradeInGoodPrice%></span>
									<%} %>
									
								</div>
									</div>
							<p class="disclaimer">* Above numbers are <b><em>ESTIMATED TRADE-IN VALUES</em></b>, dealer offer may vary. *</p>
						</div>
						
						<div id="vehicle" class="row-cell">
							<h3 class="box-header">Your Vehicle</h3>
							<ul>
                                <%if (!string.IsNullOrEmpty(Model.Vin))
                                  { %>
                                  <li>Vin: <label id="SelectedVin"><%=Model.Vin %></label></li>
                                <%} %>
								<li>Year: <label id="SelectedYear"><%=Model.SelectedYear %></label></li>
								<li>Make: <label id="SelectedMake"><%=Model.SelectedMakeValue %></label></li>
								<li>Model: <label id="SelectedModel"><%=Model.SelectedModelValue %></label></li>
								<li>Trim: <label id="SelectedTrim"><%=Model.SelectedTrimValue %></label></li>
				                <li>Mileage: <%=CommonHelper.FormatNumberInThousand(Model.Mileage)%></li>
								<li>Condition: <%=Model.Condition %></li>
								<%if (Model.SelectedOptionAdjustment > 0)
                                  {%>
								
								<li>Options: <%=Model.SelectedOptionList %>
								  
								</li>
								<% } %>
							</ul>
						</div>
					</div>
				</div>
			</div>

			<div class="info-wrap">
			    <%=Html.HiddenFor(x=>x.TradeInCustomerId) %>
				<h3 class="description-header">Vehicle's Market Comparison</h3>
				<div id="market-comparison" class="info-box">
					<div class="row">
						<div id="trade-in" class="row-cell">
							<table>
								<thead>
									<tr>
										<th></th>
										<th>Price</th>
										<th>Mileage</th>
										<th>Color</th>
									
									</tr>
								</thead>
								<tfoot>
									<tr>
										<td class="side-header">Difference</td>
										<td></td>
										<td></td>
										<td></td>
									
									</tr>
								</tfoot>
								<tbody>
									<tr>
										<td class="side-header">Your Car</td>
										<td>
                                        <%if (!(String.IsNullOrEmpty(Model.TradeInGoodPrice) || Model.TradeInGoodPrice.Equals("NA") || Model.TradeInGoodPrice.Equals("$0"))) { %>
									        <%=Model.TradeInFairPrice %>									
									    <% } %>                                        
                                        </td>
										<td><%=CommonHelper.FormatNumberInThousand(Model.Mileage)%></td>
										<td></td>
										
									</tr>
									<tr>
										<td class="disclaimer" colspan="4">
											<p>The prices below are <b>RETAIL</b> prices and <b>WILL BE HIGHER</b> than the Trade-In Value's given for your vehicle.</p>
										</td>
									</tr>
									<tr>
										<td class="side-header">Highest On Market</td>
										<td id="HighestMarketPrice"></td>
										<td id="MileageForHighest"></td>
										<td id="ColorForHighest"></td>
									
									</tr>
									<tr>
										<td class="side-header">Middle On Market</td>
										<td id="AverageMarketPrice"></td>
										<td id="MileageForAverage"></td>
										<td id="ColorForAverage"></td>
									
								    </tr>
								    <tr>
										<td class="side-header">Lowest On Market</td>
										<td id="LowestMarketPrice"></td>
										<td id="MileageForLowest"></td>
										<td id="ColorForLowest"></td>
										
								    </tr>
								</tbody>
							</table>
						</div>
					</div>
				</div>
			</div>
 
        <%if (Model.CarFax.Success)
          {%>
		<div class="info-wrap">
				<h3 class="description-header"><img class="carfax-logo" src="<%= Url.Content("~/images/carfax.gif") %>" />History Report</h3>
				<div id="carfax" class="info-box">
					<div class="row">
						<div class="row-cell">
							<div class="carfax-info">
							    <ul>
                                    <%foreach(var tmp in Model.CarFax.ReportList){  %>
                                        <li><div class="icon"><img class="c-fax-img" src="<%=tmp.Image %>" /></div><%=tmp.Text %>
                                        	
                                        </li>
                                      <%} %>  
                                    </ul>
						

								<div class="owners">
									<div class="number"><%=Model.CarFax.NumberofOwners %> </div> <span class="lable"> Owner(s)</span>
								</div>
								<span class="disclaimer">Not all accidents or other issues are reported to CARFAX. The number of owners is estimated. See the full CARFAX Report for additional information and glossary of terms.</span>
							</div>
						</div>
					</div>
				</div>
			</div>
			<% } %>
			
			
			
		
		</div>
		<div class="controls">
			<%--<a href="<%=Url.Content("~/TradeIn/TradeInVehicle?dealer=" ) %><%=Model.Dealer %>" class="prev" >Appraise a New Vehicle?</a>--%>
            <a href="<%=Url.Content("~/TradeInValue/" ) %><%= Model.DealerName.ToLower().Replace(" ", string.Empty) %>" class="prev" >Appraise a New Vehicle?</a>
		</div>
	</div>
<script src="<%=Url.Content("~/js/trade-in.js")%>" type="text/javascript"></script>
</body>
<script type="text/javascript">

    $(document).ready(function(e) {

        // initialize data set array
        $data = [];
        var tradeId = $("#TradeInCustomerId").val();

        var request_url = "/chart/GetMarketDataBanner?TradeId=" + tradeId ;
        
        $.ajax({
            url: request_url,
            context: document.body
        }).done(function(data) {
            var marketinfo = data.market;

            $("#HighestMarketPrice").html(marketinfo.maximumPrice);

            $("#AverageMarketPrice").html(marketinfo.averagePrice);

            $("#LowestMarketPrice").html(marketinfo.minimumPrice);

            $("#MileageForHighest").html(marketinfo.maximumMileage);

            $("#MileageForAverage").html(marketinfo.averageMileage);

            $("#MileageForLowest").html(marketinfo.minimumMileage);

            $("#ColorForHighest").html(marketinfo.maximumColor);

            $("#ColorForLowest").html(marketinfo.minimumColor);


        });
    });


    
</script>
</html>
