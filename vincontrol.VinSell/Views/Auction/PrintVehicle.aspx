<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.VehicleViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>VinSell | Print Vehicle</title>
    <link href="<%=Url.Content("~/Content/style.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/auction.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/charts.css")%>" rel="stylesheet" type="text/css"/>
    <link href="<%=Url.Content("~/Content/vehicle.css")%>" rel="stylesheet" type="text/css"/>
    <style type="text/css">
    @media screen
    {

    }

    @media print
    {

    div.subnav-text {color:#6770f0;}
    div.subheader-nav {border:4px solid #6770f0;color:#6770f0;}
    div#owners {color:#6770f0;}
    div#service-records {color:#6770f0;}
    div.cr {color:#6770f0;border:brown solid 2px}
    }

    @media screen, print
    {

    }
    </style>

    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.alerts.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.easing-1.3.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.mousewheel-3.0.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/util.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var baseUrl = '/Content/Images';
        $(document).ready(function () {
            $(".partialContents").each(function (index, item) {
                var url = $(item).data("url");
                if (url && url.length > 0) {
                    $(item).load(url);
                    
                }
            });

            $('div[id^="karpower-left-control_"]').html('');
            $('div[id^="karpower-right-control_"]').html('');
            window.print();

            setTimeout(function () {
                window.close();
            }, 10000);

        });

</script>
</head>
<body>
    <div class="container">
    <div class="inner-wrap">     
        <div class="page-info">
            <span>Auctions >  <a href="javascript:;"> <%=  Model.State%></a><br />
            </span><span><b> <%= Model.RegionName %></b>
            </span>
            <h3><%= Model.Seller %> <small style="font-size: .65em">[ <a href="javascript:;">Lane <%= Model.Lane %></a> / <a href="javascript:;">Run <%= Model.Run %></a> ]</small></h3>
        </div>
        <div class="car-controls">            
            <% if (Model.IsSold) {%>
            <div style="display:inline-block;float:left;background-color:White;padding:3px;border:2px solid Red;font-weight:bold;color:Red;font-size:1.4em;margin-right:5px;">SOLD</div>
            <%}%>
            <%--<div class="favorite-vehicle">
                <input type="checkbox" name="favorite" id="favorite-checkbox" />
                <img src='<%= Url.Content("~/Content/Images/favorite-ico.png") %>' title="+ Add Favorite">
            </div>--%>
            <h3><%= Model.Year %> <%= Model.Make %> <%= Model.Model %> <%= Model.Trim.Equals("Not Specified") ? "" : Model.Trim %></h3>            
        </div>
        <div id="divDetailVehicle" class="content" style="height: 800px !important;">
            <div class="vehicle" style="width:950px;">
                <div class="information">
                    <div class="images">
                        <div class="main-image">                            
                            <a href="#" target="_blank">
                            <img src='<%= Model.Images.Split(';').ToArray()[0] %>' />
                            </a>
                        </div>
                       
                        <div class="image-list">
                            <%
                                var imageArray = Model.Images.Split(';').ToArray().Distinct();
                                foreach (var image in imageArray)
                            {
                            %>
                                <a class="single_image" rel="group1" href="#">
                                <img src="<%= image %>" />
                                </a>
                            <%
                            }
                            %>                            
                        </div>
                    </div>
                    <div class="carInfo" style="height:auto;">
                        <div class="car-details">
                            <h3>Vehicle Information  <small style="font-size: .8em;font-weight: normal">(<%= Model.Vin %>)</small></h3>
                            <ul>
                               <%-- <li >VIN :</li>--%>
                                <li><b>Year: </b><%= Model.Year %></li>
                                <li><b>Make: </b><%= Model.Make %></li>
                                <li><b>Model: </b><%= Model.Model %></li>
                                <li><b>Trim: </b><%= Model.Trim %></li>
                                <li><b>Mileage: </b><%= Model.Mileage.ToString("0,0") %> mi</li>
                                <li><b>Body Style: </b><%= Model.BodyStyle %></li>
                                <li><b>Exterior. Color: </b><%= Model.ExteriorColor %></li>
                                <li><b>Interior Color: </b><%= Model.InteriorColor %></li>
                                 <li><b>Engine: </b><%= Model.Engine %></li>
                            </ul>
                            <ul>
                               
                                <li><b>Displacement: </b><%= Model.Litters %></li>
                                <li><b>Fuel Type: </b><%= Model.FuelType %></li>
                                <li><b>Transmission: </b><%= Model.Transmission %></li>
                                <li><b>Drive Train: </b><%= Model.DriveTrain %></li>
                                <li><b>Doors: </b><%= Model.Doors.Equals(0) ? "Not Specified" : Model.Doors.ToString() %></li>
                                <li><b>Stereo: </b><%= Model.Stereo %></li>
                                <li><b>Airbags: </b><%= Model.Airbags %></li>
                                <li><b>Interior Type: </b><%= Model.InteriorType %></li>
                            </ul>
                        </div>
                        <% if (Model.Equipments.Count > 0) {%>
                        <div class="equipement">
                            <h3>Equipment</h3>
                            <ul>
                                <% foreach (var option in Model.Equipments.Skip(0).Take(9)){%>
                                    <li><%= option %></li>
                                <%}%>                                
                            </ul>
                            <ul>
                                <% foreach (var option in Model.Equipments.Skip(9).Take(9)){%>
                                    <li><%= option %></li>
                                <%}%>
                            </ul>
                            <div class="cr-large">
                                <div class="cr" style="">CR</div>
                                <br /><%= Model.Cr %>
                            </div>                            
                        </div>
                        <%}%>
                        <% if (!String.IsNullOrEmpty(Model.Comment)) {%>
                        <div class="comment" style="display:inline-block;width:90%;">
                            <h3>Comments</h3>
                            <div style="font-size:0.7em;height:70px;display:inline-block;overflow:auto;"><%= Model.Comment %></div>
                        </div>
                        <%}%>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="notes">
                        <div class="notes-label">Notes</div>
                        <textarea id="vehicleNote"><%= Model.Note %></textarea>
                    </div>
                </div>
                <div class="data">
                    <%if (Model.CarFax.Success) {%>
                    <div class="data-box" id="carfax">
                        <div class="header">
                            <a href="#">
                            <img src='<%= Url.Content("~/Content/Images/carfax.jpg") %>'>
                            </a>
                        </div>
                        <div class="subheader-nav">
                            <div id="owners">
                                <% if (Model.CarFax.NumberofOwners.Equals("0")) {%> 
                                -
                                <%} else {%> 
                                    <b><%= Model.CarFax.NumberofOwners %></b> Owner(s)
                                <%}%>                                
                            </div>
                            <div id="service-records">
                                <b><%= Model.CarFax.ServiceRecords %></b> Service Record(s)</div>
                        </div>
                        <div class="data-content" style="height: 90px;">
                            <ul>
                            <%foreach (var tmp in Model.CarFax.ReportList) { %>
                                <%if (tmp.Text.Contains("Prior Rental") || tmp.Text.Contains("Accident(s) / Damage Reported to CARFAX")){ %>
                                <li style="background-color:Red;"><%= tmp.Text %></li>
                                <%} else {%>
                                <li><%= tmp.Text %></li>
                                <%} %>
                            <%} %>
                            </ul>
                        </div>
                    </div>
                    <%} %>
                    <div class="data-box" id="manheim">
                        <div class="header">
                            <img class="icon" src='<%= Url.Content("~/Content/Images/manheim-ico.jpg") %>'>
                            Manheim</div>
                        <div class="partialContents" data-url="<%= Url.Action("ManheimData", "Auction", new { Model.Vin }) %>">
                            <div class="subheader-nav" style="background:#6770f0;color:White;">
                            </div>
                            <div class="data-content">Loading...</div>
                        
                        </div>
                    </div>
                    <div class="data-box" id="kbb">
                        <div class="header">
                            <img class="icon" src='<%= Url.Content("~/Content/Images/kbb-ico.jpg") %>'>KBB</div>
                        <div class="partialContents" data-url="<%= Url.Action("KarpowerData", "Auction", new { Model.Vin, Model.Mileage }) %>">
                            <div class="subheader-nav" style="background:#6770f0;color:White;">
                            </div>
                            <div class="data-content">Loading...</div>
                        
                        </div>
                    </div>
                    <div class="data-box" id="market">
                        <div class="header">
                            <img class="icon" src='<%= Url.Content("~/Content/Images/market-ico.jpg") %>'>Market</div>
                        <div class="partialContents" data-url="<%= Url.Action("MarketData", "Auction", new { Model.Id }) %>">
                            <div class="subheader-nav" style="background:#6770f0;color:White;">
                            </div>
                            <div class="data-content">Loading...</div>
                        
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- end of inner wrap div-->
    </div>
    </div>
</body>
</html>
