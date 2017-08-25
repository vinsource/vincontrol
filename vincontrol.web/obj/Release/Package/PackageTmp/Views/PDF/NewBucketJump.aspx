<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.BucketJumpViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Bucket Jump Report</title>
    <style type="text/css">
    .bj_notes_holder {
	    width: 95%;
	    margin: 5px auto;
    }
    .bj_notes_title {
	    font-weight: bold;
	    color: #444;
	    font-size: 15px;
    }
    .bj_notes_value {
	    height: 120px;
	    border: 1px solid #000;
    }
    .bj_signature_holder {
	    width: 95%;
	    height: 50px;
	    margin: 30px auto;
    }
    .bj_signature_left {
	    width: 40%;
	    float: left;
    }
    .bj_signature_right {
	    width: 40%;
	    float: right;
    }
    .bj_signature_borderbottom {
	    font-weight: bold;
	    color: #444;
	    font-size: 15px;
	    border-bottom: 2px solid #555;
	    padding: 3px;
	    height: 20px;
    }
    .bj_signature_date {
	    font-weight: bold;
	    color: #444;
	    font-size: 15px;
    }

    .bj_signature_date {
	    font-weight: bold;
	    color: #444;
	    font-size: 15px;
    }
    .bj_cpricing {
	    margin-top: 10px;
	    font-weight: bold;
	    font-size: 35px;
	    text-align: center;
    }
    .bj_option_adjustment_holder {
	    width: 50%;
	    margin: 0px auto;
	    height: 700px;
    }
    .bj_oa_title {
	    /*padding-left: 42px;*/
	    font-weight: bold;
	    float: left;
	    width: 47%;
	    text-align: right;
    }
    .bj_oa_items_list {

    }
    .bj_oa_items {
	    clear: both;
	    height: 22px;
    }
    .bj_oa_items_text {
	    float: left;	    
	    font-weight: bold;
	    width: 47%;
	    text-align: right;
	    color: #444;
	    font-size: 14px;
    }
    .bj_oa_items_value {
	    /*float: right;
	    width: 40%;*/
	    float: left;	    
	    font-weight: bold;
	    width: 47%;
	    text-align: right;
	    background-color: #CDCDCD;
	    border: 1px solid #000;
	    font-size: 15px;
	    font-weight: bold;
	    color: #333;
    }
    .bj_adjustment_holder {
	    clear: both;
	    width: 80%;
	    margin: 0px auto;
    }
    .bj_adjustment_items {
	    clear: both;
	    height: 40px;
	    margin-top: 7px;
    }
    .bj_adjustment_title {
	    text-align: center;
	    float: left;
	    margin-left: 5px;
	    width: 240px;
	    font-weight: bold;
	    font-size: 14px;
    }
    .bj_adjustment_input_white {
	    padding: 3px;
	    width: 120px;
	    border: 1px solid #333;
	    background-color: white;
	    font-weight: bold;
	    font-size: 14px;
	    text-align: center;
	    float: left;
	    margin-left: 5px;
    }
    .bj_adjustment_input_gray {
	    padding: 3px;
	    width: 120px;
	    border: 1px solid #333;
	    background-color: #CDCDCD;
	    font-weight: bold;
	    font-size: 14px;
	    text-align: center;
	    float: left;
	    margin-left: 5px;
    }
    .bj_vh_holder {

    }
    .bj_vh_holder > div {
	    margin-top: 5px;
	    box-sizing: border-box;
	    -moz-box-sizing: border-box; /* Firefox */
	    width: 43%;
    }
    .bj_vh_left {
	    float: left;
    }
    .bj_vh_right {
	    float: right;
    }
    .bj_vh_title {
	    font-size: 14px;
	    font-weight: bold;
    }
    .bj_vh_info {
	    height: 130px;
    }
    .bj_vh_info_img {
	    width: 40%;
	    float: left;
	    padding: 3px 0px;
	    border: 1px solid #000;
    }
    .bj_vh_info_img img {
	    width: 100%;
    }
    .bj_vh_info_pm {
	    width: 58%;
	    float: right;
    }
    .bj_vh_info_price {
	    text-align: center;
    }
    .bj_vh_info_title {
	    font-size: 14px;
	    font-weight: bold;
	    color: #555;
    }
    .bj_vh_info_value {
	    border: 2px solid #777;
	    font-size: 20px;
	    color: #FFF;
	    background-color: #039;
	    font-weight: bold;
    }
    .bj_vh_info_mileage {
	    text-align: center;
    }
    .bj_vh_info_mileage .bj_vh_info_value {
	    font-size: 15px !important;
	    padding: 3px;
    }
    .bucketjump_container 
    {
    	margin:0px auto;
	    width: 990px;
    }
    .bucketjump_vinlogo img {
	    margin: 10px auto;
	    display: block;
    }

    .bucketjump_age_holder {
	    width: 42%;
	    height: 30px;
	    margin: 0px auto;
    }

    .bucketjump_age_items {
	    float: left;
	    border: 1px solid #333;
	    font-size: 14px;
	    width: 23px;
	    text-align: center;
	    padding: 5px;
	    font-weight: bold;
    }

    .bucketjump_age_title, .bucketjump_age_chosen {
	    background-color: #CDCDCD;
    }

    .bucketjump_dealer_compare_holder {
	    clear: both;
	    border: 2px solid #333;
	    height: 28px;
	    margin-top: 15px;
    }
    .bucketjump_dealer_compare_holder > div {
	    box-sizing: border-box;
	    -moz-box-sizing: border-box; /* Firefox */
	    float: left;
	    text-align: center;
	    padding: 5px;
	    font-size: 15px;
	    font-weight: bold;
    }
    .bj_dc_left {
	    width: 43%;
    }

    .bj_dc_middle {
	    background-color: #039;
	    color: white;
	    width: 14%;
	    border-left: 2px solid #777;
	    border-right: 2px solid #777;
    }

    .bj_dc_right {
	    width: 43%;
    }
    #vehicle-list table { width: 980px; font-size: 1.0em;}
    #vehicle-list td { padding: .3em .7em .3em .7em; border-bottom: 1px #bbbbbb solid;}
    #vehicle-list tr:nth-child(2n+2) td {background: #cccccc;}
    #vehicle-list tr.highlight td {background: green; color: #fff; }
    /*#vehicle-list tr:nth-child(1) td {font-weight: bold;color: white;background: #222222;}*/
    .isTargetCar { background-color:Green; }

    #graph-title-bar
    {
        position: relative;
        height: 50px !important;
        max-height: 50px !important;
        overflow: hidden;
        width: 99%;
        display: block;
    }
    #graph-title-bar h2
    {
        display: inline-block;
        padding-bottom: 0;
        margin-bottom: 0;
    }
    #graph-title-bar a
    {
        margin-left: 20px;
        padding: .2em .5em .2em .5em;
        background: #c80000;
        color: white;
        position: relative;
        top: -3px;
        font-size: .9em;
        cursor: pointer;
    }
    #graph-title-bar a:hover
    {
        background: #880000;
    }
    #printable-list
    {
        display: block;
    }
    </style>
</head>
<body>
    <div class="bucketjump_container">
			<div class="bucketjump_vinlogo">
				<img src="http://apps.vincontrol.com/Content/Images/logo-vincontrol.png" alt="" />
			</div>
			<div class="bucketjump_age_holder">
				<div class="bucketjump_age_items bucketjump_age_title">
					Age
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[0]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[0]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[1]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[1]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[2]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[2]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[3]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[3]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[4]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[4]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[5]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[5]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[6]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[6]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[7]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[7]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[8]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[8]%>
				</div>
				<div class="bucketjump_age_items <%= Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[9]) ? "bucketjump_age_chosen" : "" %>">
					<%=Model.AvailableDaysInInventory[9]%>
				</div>
				
			</div>

			<div class="bucketjump_dealer_compare_holder">
				<div class="bj_dc_left">
					<%= Model.DealerName %>
				</div>
				<div class="bj_dc_middle">
					<%if (Model.CarOfDealer.Miles > Model.CarOnMarket.Miles){%>
                    <%= (Model.CarOfDealer.Miles - Model.CarOnMarket.Miles).ToString("#,##0")%>
                    <%}else{%>
                    (<%= (Model.CarOnMarket.Miles - Model.CarOfDealer.Miles).ToString("#,##0")%>)
                    <%}%> 
                    mi
				</div>
				<div class="bj_dc_right">
					<%= Model.CarOnMarket.Dealer %>
				</div>
			</div>

			<div class="bj_vh_holder">
				<div class="bj_vh_left">
					<div class="bj_vh_title">
						<%= Model.CarOfDealer.Year %> <%= Model.CarOfDealer.Make %> <%= Model.CarOfDealer.Model %>
					</div>
					<div class="bj_vh_info">
						<div class="bj_vh_info_img">
							<img src="<%= Model.CarOfDealer.Image %>" alt=""/>
						</div>
						<div class="bj_vh_info_pm">
							<div class="bj_vh_info_price">
								<div class="bj_vh_info_title">
									Price
								</div>
								<div class="bj_vh_info_value">
									<%= Model.CarOfDealer.Price.ToString("c0") %>
								</div>
							</div>
							<div class="bj_vh_info_mileage">
								<div class="bj_vh_info_title">
									Mileage
								</div>
								<div class="bj_vh_info_value">
									<%= Model.CarOfDealer.Miles.ToString("0,0") %> mi
								</div>
							</div>
						</div>
					</div>
				</div>

				<div class="bj_vh_right">
					<div class="bj_vh_title">
						<%= Model.CarOnMarket.Year %> <%= Model.CarOnMarket.Make %> <%= Model.CarOnMarket.Model %>
					</div>
					<div class="bj_vh_info">
						<div class="bj_vh_info_img">
							<img src="<%= Model.CarOnMarket.Image %>" alt=""/>
						</div>
						<div class="bj_vh_info_pm">
							<div class="bj_vh_info_price">
								<div class="bj_vh_info_title">
									Price
								</div>
								<div class="bj_vh_info_value">
									<%= Model.CarOnMarket.Price.ToString("c0") %>
								</div>
							</div>
							<div class="bj_vh_info_mileage">
								<div class="bj_vh_info_title">
									Mileage
								</div>
								<div class="bj_vh_info_value">
									<%= Model.CarOnMarket.Miles.ToString("0,0") %> mi
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
            
			<div class="bj_adjustment_holder" style="margin-top:5px;">
				<div class="bj_adjustment_items">
					<div class="bj_adjustment_title">
						Dealer Value Adjustment
					</div>
					<div class="bj_adjustment_input_white">
						<%= Model.PlusPrice%>%
					</div>
					<div class="bj_adjustment_input_gray">
						<%= Model.IndependentAdd.ToString("c0") %>
					</div>
				</div>

				<div class="bj_adjustment_items">
					<div class="bj_adjustment_title">
						Certified Value Adjustment
					</div>
					<div class="bj_adjustment_input_white">
						<%= (Model.CertifiedAdd * 2).ToString("c0") %> (Retail)
					</div>
					<div class="bj_adjustment_input_gray">
						<%= Model.CertifiedAdd.ToString("c0") %>
					</div>
				</div>

				<div class="bj_adjustment_items">
					<div class="bj_adjustment_title">
						Mileage Value Adjustment
					</div>
					<div class="bj_adjustment_input_white">
					<%if (Model.CarOfDealer.Miles > Model.CarOnMarket.Miles){%>
                    <%= (Model.CarOfDealer.Miles - Model.CarOnMarket.Miles).ToString("#,##0")%>
                    <%}else{%>
                    (<%= (Model.CarOnMarket.Miles - Model.CarOfDealer.Miles).ToString("#,##0")%>)
                    <%}%>
                    (Miles)
					</div>
					<div class="bj_adjustment_input_gray" style="color: #800">
						<%= Model.MileageAdjustmentDiff.ToString("c0") %>
					</div>
				</div>
			</div>

            <% if (Model.CarOfDealer.Options != null && Model.CarOfDealer.Options.Any())
               {%>
			<div class="bj_option_adjustment_holder">
				<div class="bj_oa_title">
					Option Adjustment
				</div>
				<div class="bj_oa_items_list">
					<% foreach (var item in Model.CarOfDealer.Options){%>
                    <div class="bj_oa_items">
						<div class="bj_oa_items_text" style="float:left;">
							<%= item %>
						</div>
						<%--<div class="bj_oa_items_value">
							$0
						</div>--%>
					</div>  
                    <%}%>
                                        
				</div>
                <div class="bj_oa_items_value"><%= Model.OptionsPrice.ToString("c0") %></div>
			</div>
			<%}%>
			<div class="bj_cpricing">
				Competitive Pricing <%= Model.CarOfDealer.SuggestedRetailPrice.ToString("c0") %>
			</div>
			
			<div class="bj_notes_holder">
				<div class="bj_notes_title">Notes</div>
				<div class="bj_notes_value">
					<%=Model.CarOfDealer.Note %>
				</div>
			</div>
			<div class="bj_signature_holder">
				
				<div class="bj_signature_left">
					<div class="bj_signature_borderbottom"></div>
					<div class="bj_signature_date">Signature</div>
				</div>
				<div class="bj_signature_right">
					<div class="bj_signature_borderbottom">Auto Populate (Month/Day/Year)</div>
					
					<div class="bj_signature_date">Date</div>
				</div>
			</div>
		</div>
    <% if (Model.ChartGraph != null && Model.ChartGraph.ChartModels.Count > 0){%>
    <div class="graph-title-bar" style="margin-bottom: 30px; margin-top: 120px; text-align:center;">
        <h1 style="font-size: 30px; text-align: center;">
            List of Charted Vehicles
        </h1>
        <div id="printable-list" style="padding:0;margin:0 auto;display: block;">
            <div id="vehicle-list" style="font-size: 1.0em">
                <table id="tblVehicles" cellspacing="0" style="text-align:center;" align="center">
                    <thead style="background-color: #000; color: #fff; height: 20px; cursor: pointer;">
                        <tr>
                            <th>#</th>
                            <th align="center">Year</th>
                            <th align="center">Make</th>
                            <th align="left">Model</th>
                            <th align="left">Trim</th>
                            <th align="left">Distance</th>
                            <th align="center">Certified</th>
                            <th>Seller</th>
                            <th align="center">Miles</th>
                            <th align="center">Price</th>
                            <th>Carscom</th>
                            <th>Autotrader</th>
                        </tr>
                    </thead>
                    <tbody>
                            
                        <%
            int i = 1;
           foreach (var item in Model.ChartGraph.ChartModels){
                   var targetCar=(bool)item[21];
                   var carscom = (bool)item[14];
                   var autotrader = (bool)item[12];
                   %>
                               <tr <%= targetCar ? "class=isTargetCar" : "" %>>
                                <td><%=i++%></td>
                                <td><%= item[2] %></td>
                                <td><%= item[3] %></td>
                                <td><%= item[4] %></td>
                                <td><%= item[5] %></td>
                                <td><%=item[24]%></td>
                                <td><%="No"%></td>
                                <td><%=item[19]%></td>
                                <td><b><%=item[8]%></b></td>
                                <td><b><%=item[9]%></b></td>
                                 <td><%=carscom ? "Yes" : "No"%></td>
                                <td><%=autotrader ? "Yes" : "No"%></td>
                            </tr>
                            <%}%>                            
                        </tbody>
                </table>
            </div>
        </div>
    </div>
    <%}%>
</body>
</html>
