<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>

<div id="list-render-0" class="content">
    <div class="scrollableContainer">
        <div class="scrollingArea">
            <table id="tblVehicles_upcoming" class="list" cellspacing="0">
                <thead>
                    <tr style="cursor:pointer;">
                        <th class="fav" width="20">
                            Fav
                        </th>
                        <th class="year" width="20">
                            Year
                        </th>
                        <th class="make" width="120">
                            Make
                        </th>
                        <th class="model" width="90">
                            Model
                        </th>
                        <th class="trim" width="100">
                            Trim
                        </th>
                        <th class="vin" width="70">
                            VIN
                        </th>
                        <th class="seller" width="130">
                            Seller
                        </th>
                        <th class="mileage" width="70">
                            Mileage
                        </th>
                        <th class="mmr" width="70">
                            MMR
                        </th>
                        <th class="lane" width="35">
                            Lane
                        </th>
                        <th class="run" width="35">
                            Run
                        </th>
                        <th class="condition" width="70">
                            Condition
                        </th>
                    </tr>
                </thead>
                <% foreach (var group in Model.AuctionList.Where(i => !i.IsSold && !i.IsPast).OrderBy(i => i.RegionName).GroupBy(i => i.RegionName))
                {%>
                <tbody class="tablesorter-no-sort">
                    <tr id="auction_<%= group.Key %>">
                        <td colspan="12">
                            <b><%= group.Key %></b> &nbsp;&nbsp; <i>(<%= group.Count() > 1 ? group.Count() + " vehicles" : "1 vehicle" %>)</i>
                        </td>
                    </tr>
                </tbody>
                <% foreach (var subgroup in group.OrderByDescending(i => i.AuctionDate).GroupBy(i => i.AuctionDate))
                {%>
                <tbody class="tablesorter-no-sort">
                    <tr id="date_<%= group.Key %>" style="display: none;">
                        <td colspan="12">
                            <i><%= subgroup.Key.ToString("MM/dd/yyyy") %></i>
                        </td>
                    </tr>
                </tbody>
                <tbody>
                    <% foreach (var auction in subgroup.OrderByDescending(i => i.Year).ThenBy(i => i.Seller))
                    {%>
                    <tr id="viewByAuction_<%= group.Key %>_<%= auction.Id %>" style="display:none;">
                        <td class="fav" width="20">
                            <input id="chkFavorite_<%= auction.Id %>_<%= auction.Vin %>" type="checkbox" <%= auction.IsFavorite ? "checked" : "" %>/>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="year" width="30">
                            <a href="javascript:;"><%= auction.Year %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="make" width="120">
                            <a href="javascript:;"><%= auction.Make.Length > 10 ? auction.Make.Substring(0, 10) : auction.Make %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="model" width="90">
                            <a href="javascript:;"><%= auction.Model %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="trim" width="100">
                            <a href="javascript:;"><%= auction.Trim %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="vin" width="80">
                            <a href="javascript:;"><%= auction.Vin.Length > 8 ? auction.Vin.Substring(0, 8) : auction.Vin %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="seller" width="140">
                            <%= auction.Seller.Length > 15 ? auction.Seller.Substring(0, 15) : auction.Seller%>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="mileage" width="70">
                            <%= auction.Mileage.ToString("#0,0") %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="mmr" width="70">
                            <%= auction.Mmr.ToString("c0") %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="lane" width="35" align="center">
                            <%= auction.Lane %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="run" width="35" align="center">
                            <%= auction.Run %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="condition" width="70">
                            <div class="cr">CR</div>
                            <%= auction.Cr %>
                        </td>
                    </tr>
  
                    <%}%>                  
                    
                </tbody>
                <%}%>
                <%}%>
                
            </table>
            <table id="tblVehicles_past" class="list" cellspacing="0" style="display:none;">
                <thead>
                    <tr style="cursor:pointer;">
                        <th class="fav" width="20">
                            Fav
                        </th>
                        <th class="year" width="20">
                            Year
                        </th>
                        <th class="make" width="120">
                            Make
                        </th>
                        <th class="model" width="90">
                            Model
                        </th>
                        <th class="trim" width="100">
                            Trim
                        </th>
                        <th class="vin" width="70">
                            VIN
                        </th>
                        <th class="seller" width="130">
                            Seller
                        </th>
                        <th class="mileage" width="70">
                            Mileage
                        </th>
                        <th class="mmr" width="70">
                            MMR
                        </th>
                        <th class="lane" width="35">
                            Lane
                        </th>
                        <th class="run" width="35">
                            Run
                        </th>
                        <th class="condition" width="70">
                            Condition
                        </th>
                    </tr>
                </thead>
                <% foreach (var group in Model.AuctionList.Where(i => i.IsSold || i.IsPast).OrderBy(i => i.RegionName).GroupBy(i => i.RegionName))
                {%>
                <tbody class="tablesorter-no-sort">
                    <tr id="auction_<%= group.Key %>">
                        <td colspan="12">
                            <b><%= group.Key %></b> &nbsp;&nbsp; <i>(<%= group.Count() > 1 ? group.Count() + " vehicles" : "1 vehicle" %>)</i>
                        </td>
                    </tr>
                </tbody>
                <% foreach (var subgroup in group.OrderByDescending(i => i.AuctionDate).GroupBy(i => i.AuctionDate))
                {%>
                <tbody class="tablesorter-no-sort">
                    <tr id="date_<%= group.Key %>" style="display: none;">
                        <td colspan="12">
                            <i><%= subgroup.Key.ToString("MM/dd/yyyy") %></i>
                        </td>
                    </tr>
                </tbody>
                <tbody>
                    <% foreach (var auction in subgroup.OrderByDescending(i => i.Year).ThenBy(i => i.Seller))
                    {%>
                    <tr id="viewByAuction_<%= group.Key %>_<%= auction.Id %>" style="display:none;">
                        <td class="fav" width="20">
                            <input id="chkFavorite_<%= auction.Id %>_<%= auction.Vin %>" type="checkbox" <%= auction.IsFavorite ? "checked" : "" %>/>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="year" width="30">
                            <a href="javascript:;"><%= auction.Year %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="make" width="120">
                            <a href="javascript:;"><%= auction.Make.Length > 10 ? auction.Make.Substring(0, 10) : auction.Make %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="model" width="90">
                            <a href="javascript:;"><%= auction.Model %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="trim" width="100">
                            <a href="javascript:;"><%= auction.Trim %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="vin" width="80">
                            <a href="javascript:;"><%= auction.Vin.Length > 8 ? auction.Vin.Substring(0, 8) : auction.Vin %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="seller" width="140">
                            <%= auction.Seller.Length > 15 ? auction.Seller.Substring(0, 15) : auction.Seller%>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="mileage" width="70">
                            <%= auction.Mileage.ToString("#0,0") %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="mmr" width="70">
                            <%= auction.Mmr.ToString("c0") %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="lane" width="35" align="center">
                            <%= auction.Lane %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="run" width="35" align="center">
                            <%= auction.Run %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="condition" width="70">
                            <div class="cr">CR</div>
                            <%= auction.Cr %>
                        </td>
                    </tr>
  
                    <%}%>                  
                    
                </tbody>
                <%}%>
                <%}%>
                
            </table>
        </div>        
    </div>    
</div>

<script type="text/javascript">

    $(document).ready(function () {

        if ($('#upcoming').hasClass('active')) {
            $('#tblVehicles_upcoming').show();
            $('#tblVehicles_past').hide();
        }

        if ($('#past').hasClass('active')) {
            $('#tblVehicles_upcoming').hide();
            $('#tblVehicles_past').show();
        }

        $.tablesorter.addParser({
            // set a unique id 
            id: 'price',
            is: function (s) {
                // return false so this parser is not auto detected 
                return false;
            },
            format: function (s) {
                // format your data for normalization 
                return s.replace('$', '').replace(/,/g, '');
            },
            // set type, either numeric or text 
            type: 'numeric'
        });

        //        $("table[id^=tblVehicles_]").tablesorter({
        //            headers: {
        //                0: { sorter: false },
        //                7: { sorter: 'price' },
        //                8: { sorter: 'price' }
        //            }
        //        });

        $("input[id^='chkFavorite_']").click(function () {
            var id = this.id.split('_')[1];
            var vin = this.id.split('_')[2];
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Auction/MarkFavorite?vehicleId=' + id + "&vin=" + vin,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {

                },
                error: function (err) {
                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');

                }
            });
        });

        $("td[id^='viewDetailVehicle_']").click(function () {
            var id = this.id.split('_')[1];
            window.location.href = "/Auction/DetailVehicle?id=" + id;
        });

        $("#tblVehicles_upcoming tr[id^='auction_']").click(function () {
            var auction = this.id.split('_')[1];
            if ($('#tblVehicles_upcoming tr[id^="date_' + auction + '"]').is(':hidden')) {
                $('#tblVehicles_upcoming tr[id^="date_' + auction + '"]').show();
                $('#tblVehicles_upcoming tr[id^="viewByAuction_' + auction + '"]').show();
            } else {
                $('#tblVehicles_upcoming tr[id^="date_' + auction + '"]').hide();
                $('#tblVehicles_upcoming tr[id^="viewByAuction_' + auction + '"]').hide();
            }
        });

        $("#tblVehicles_past tr[id^='auction_']").click(function () {
            var auction = this.id.split('_')[1];
            if ($('#tblVehicles_past tr[id^="date_' + auction + '"]').is(':hidden')) {
                $('#tblVehicles_past tr[id^="date_' + auction + '"]').show();
                $('#tblVehicles_past tr[id^="viewByAuction_' + auction + '"]').show();
            } else {
                $('#tblVehicles_past tr[id^="date_' + auction + '"]').hide();
                $('#tblVehicles_past tr[id^="viewByAuction_' + auction + '"]').hide();
            }
        });
    });
</script>