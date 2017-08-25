<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.InventoryManagement.DailyBuckẹtumpHistoryViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Daily Bucket Jump Report</title>
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
				<img src="http://apps.vincontrol.com/Content/Images/logo-vincontrol.png" alt="" /><br/>
                <div style="text-align: center;"><h1><%= Model.Store %></h1></div>
			</div>
			
		</div>
    
    <div class="graph-title-bar" style="margin-bottom: 30px; margin-top: 120px; text-align:center;">
        <div id="printable-list" style="padding:0;margin:0 auto;display: block;">
            <div id="vehicle-list" style="font-size: 1.0em">
                <table id="tblVehicles" cellspacing="0" style="text-align:center;" align="center">
                    <thead style="background-color: #000; color: #fff; height: 20px; cursor: pointer;">
                        <tr>
                            <th align="left">VIN</th>
                            <th align="left">Stock</th>
                            <th align="left">Store</th>
                            <th align="left">Certified Amount</th>
                            <th align="left">Miscellaneous Amount</th>
                            <th align="right">Before Price Change</th>
                            <th align="right">After Price Change</th>
                            <th align="right">User Stamp</th>
                            <th align="right">Date Stamp</th>
                            <th align="right">Vehicle Status</th>
                        </tr>
                    </thead>
                    <tbody>
                            
                        <% foreach (var item in Model.List){ %>
                        <tr>
                            <td><%= item.VIN %></td>
                            <td><%= item.Stock %></td>
                            <td><%= item.Store %></td>
                            <td><%= item.CertifiedAmount.ToString("c0") %></td>
                            <td><%= item.MileageAdjustment.ToString("c0") %></td>
                            <td><%= item.SalePrice.ToString("c0") %></td>
                            <td><%= item.RetailPrice.ToString("c0") %></td>
                            <td><%= item.UserStamp %></td>
                            <td><%= item.DateStamp.ToString("MM/dd/yyyy HH:mm") %></td>
                            <td><%= item.VehicleStatusName %></td>
                        </tr>
                        <%}%>                            
                        </tbody>
                </table>
            </div>
        </div>
    </div>
    
</body>
</html>
