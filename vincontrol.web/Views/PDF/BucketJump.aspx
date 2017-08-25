<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.BucketJumpViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bucket Jump Report</title>
    <style type="text/css">
        body
        {
            /*background-color: #333;*/
        }
        #in-container
        {
            width: 1150px/*900px*/;
            min-height: 1200px/*100%*/;
            margin-left: auto;
            margin-right: auto;
            background-color: white;
            border: 1px solid;
        }
        #in-header
        {
            width: 100%;
            height: 110px;
        }
        #in-logo
        {
            margin-left: 20px;
            margin-top: 20px;
            float: left;
        }
        #in-header-text
        {
            float: right;
            width: 1000px/*750px*/;
            height: 100%;
        }
        #in-header-text h4
        {
            float: left;
            margin-top: 80px;
        }
        #in-grid-conditions
        {
            width: 100%;
            height: 200px;
        }
        #in-grid
        {
            width: 100%;
            height: 100px;
        }
        #in-conditions
        {
            width: 100%;
            height: 100px;
        }
        .gr-cond-text
        {
            text-align: center;
            font-weight: bold;
            margin-left: 5px;
            margin-top: 40px;
            width: 82px;
            float: left;
        }
        .in-grid-sharps
        {
            padding-top: 19px;
            text-align: center;
            border-radius: 32px;
            margin-top: 25px;
            width: 60px;
            height: 40px;
            border: 1px solid gray;
            float: left;
            margin-left: 26px;
        }
        .in-grid-sharps-active
        {
        	padding-top: 19px;
            text-align: center;
            border-radius: 32px;
            margin-top: 25px;
            width: 60px;
            height: 40px;
            border: 1px solid gray;
            float: left;
            margin-left: 26px;
        	background-color: Gray;
        	color: White;
        }
        #in-tables
        {
            width: 100%;
            height: 800px;
        }
        .in-table
        {
            margin-top: 40px;
            width: 550px/*400px*/;
            height: 300px;
            float: left;
            margin-left: 20px;
        }
        .in-table-header
        {
            font-weight: bold;
            text-align: center;
            padding-top: 5px;
            width: 99.7%/*99.5%*/;
            height: 24px;
            border-left: 1px solid;
            border-top: 1px solid;
            border-right: 1px solid;
        }
        .in-table-detail
        {
            width: 100%;
        }
        .in-table-detail tr
        {
            line-height: 27px;
            border: 1px solid;
        }
        .td-title
        {
            text-align: center;
            border-right: 1px solid;
            width: 30%;
            font-weight: bold;
        }
        table
        {
            border-collapse: collapse;
        }
        table
        {
            border: none;
        }
        #in-footer
        {
            width: 100%;
            height: 70px;
        }
        #in-footer h4
        {
            float: left;
            padding-left: 10px;
        }
        #vehicle-list table { width: 980px; font-size: 1.0em;}
        #vehicle-list td { padding: .3em .7em .3em .7em; border-bottom: 1px #bbbbbb solid;}
        #vehicle-list tr:nth-child(2n+2) td {background: #cccccc;}
        #vehicle-list tr.highlight td {background: green; color: #fff; }
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
    <div id="in-container">
        <div style="width:100%;text-align:center;font-size:1.2em;font-weight:bold;margin-top:50px;"><%= Model.DealerName %></div>
        <div id="in-header">
            <div id="in-logo">
                <img src="imgs/logo.jpg" alt="" />
            </div>
            <div id="in-header-text">
                <h4 style="margin-left:0px;width: 82%">
                    Everyday Low Pricing Homework</h4>
                <h4 style="width: 18%">
                    Date: <%= DateTime.Now.ToString("MM/dd/yyyy") %></h4>
            </div>
        </div>
        <div id="in-grid-conditions">
            <div id="in-grid">
                <label class="gr-cond-text">
                    Grids :</label>
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[0])){%>
                <div class="in-grid-sharps-active"><%= Model.AvailableDaysInInventory[0]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[0]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[1])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[1]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[1]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[2])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[2]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[2]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[3])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[3]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[3]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[4])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[4] %></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[4]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[5])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[5]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[5]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[6])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[6]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[6]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[7])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[7] %></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[7]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[8])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[8]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[8]%></div>
                <%}%>
                
                <% if (Model.HighlightedDaysInInventory.Contains(Model.AvailableDaysInInventory[9])){%>
                <div class="in-grid-sharps-active"><%=Model.AvailableDaysInInventory[9]%></div>
                <%}else{%>
                <div class="in-grid-sharps"><%=Model.AvailableDaysInInventory[9]%></div>
                <%}%>
                
                <div class="in-grid-sharps">&nbsp;</div>
            </div>
            <div id="in-conditions">
                <label class="gr-cond-text">
                    Conditions :</label>
                <div class="in-grid-sharps">
                    A
                </div>
                <div class="in-grid-sharps">
                    B
                </div>
                <div class="in-grid-sharps">
                    C
                </div>
            </div>
        </div>
        <div id="in-tables">
            <div class="in-table">
                <div class="in-table-header">
                    Comparator Vehicle
                </div>
                <table class="in-table-detail">
                    <tr>
                        <td class="td-title">
                            Dealer
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Dealer %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Price
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Price.ToString("c0")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Year
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Year %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Make
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Make %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Model
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Model %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Color
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Color %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Miles
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Miles.ToString("#,##0")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Vin#
                        </td>
                        <td>&nbsp;
                          <%= Model.CarOnMarket.Vin %>
                        </td>
                    </tr>
                    <tr style="line-height: 58px;">
                        <td class="td-title">
                            Specification
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div class="in-table">
                <div class="in-table-header">
                    Own Vehicle
                </div>
                <table class="in-table-detail">
                    <tr>
                        <td class="td-title">
                            Stock Number
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.StockNumber %>
                        </td>
                    </tr>
                       <tr>
                        <td class="td-title">
                            Price
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.Price.ToString("c0")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Year
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.Year %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Make
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.Make %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Model
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.Model %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Color
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.Color %>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Miles
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.Miles.ToString("#,##0")%>
                        </td>
                    </tr>
                     <tr>
                        <td class="td-title">
                            Vin#
                        </td>
                        <td>&nbsp;
                           <%= Model.CarOfDealer.Vin %>
                        </td>
                    </tr>
                    <tr style="line-height: 58px;">
                        <td class="td-title">
                            Specification
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div class="in-table" style="height: 379px;">
                <div class="in-table-header">
                    Retail Price Adjustment
                </div>
                <table class="in-table-detail">
                    <tr style="line-height: 22px">
                        <td class="td-title">
                            Comparator Vehicle
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Price.ToString("c0")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title" style="background-color: black; border-left: 1px solid">
                        </td>
                        <td style="text-align: center; font-weight: bold">
                            Price Adjustment
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Year
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Year%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Make +/-
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Make%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Model +/-
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOnMarket.Model%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Certified +/-
                        </td>
                        <td>&nbsp;
                            <%= Model.CarOfDealer.CertifiedAmount.ToString("c0")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Miles +/-
                        </td>
                        <td>&nbsp;
                            <%if (Model.CarOfDealer.Miles > Model.CarOnMarket.Miles){%>
                            +<%= (Model.CarOfDealer.Miles - Model.CarOnMarket.Miles).ToString("#,##0")%>
                            <%}else{%>
                            -<%= (Model.CarOnMarket.Miles - Model.CarOfDealer.Miles).ToString("#,##0")%>
                            <%}%>
                            
                            
                        </td>
                    </tr>
                       <tr>
                        <td class="td-title">
                            Price +/- (From KBB)
                        </td>
                        <td>&nbsp;
                           <%=(Model.MileageAdjustmentDiff).ToString("c0")%>
                            
                        </td>
                    </tr>
                    <tr style="line-height: 58px;">
                        <td class="td-title">
                            Options +/-
                        </td>
                        <td>&nbsp; <%=(Model.OptionsPrice).ToString("c0")%>
                        </td>
                    </tr>
                      <tr style="line-height: 58px;">
                        <td class="td-title">
                            Miscellaneous +/-
                        </td>
                        <td>&nbsp; <%=(Model.CarOfDealer.ExpandedMileageAdjustment).ToString("c0")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            UCM
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-title">
                            Retail Price
                        </td>
                        <td>
                             <%=Model.CarOfDealer.SuggestedRetailPrice.ToString("c0")%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="in-table" style="height: 379px; border: 1px solid">
                <div class="in-table-header" style="border-bottom: 1px solid !important; border: 0px;">
                    Notes
                </div>
                <table class="in-table-detail">
                <%=Model.CarOfDealer.Note %>
                </table>
            </div>
        </div>
        <br/>
        <div id="in-footer">
            <h4 style="width: 63%">
                Sale Manager: .................................................................................</h4>
            <h4 style="width: 33%">
                Date:.........................................</h4>
        </div>
        <% if (Model.ChartGraph != null && Model.ChartGraph.ChartModels.Count > 0){%>
        <div class="graph-title-bar" style="margin-bottom: 30px; margin-top: 320px; text-align:center;">
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
    </div>
    
</body>
</html>
