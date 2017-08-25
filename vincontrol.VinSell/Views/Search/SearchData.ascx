<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>

<%--<input type="hidden" id="hdnCount" name="hdnCount" value="<%= Model.AuctionList.Count %>" />--%>
<div id="list-render-0" class="content">
    <div class="scrollableContainer">
        <div class="scrollingArea">
            <table id="tblVehicles" class="list" cellspacing="0">
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
                        <th class="vin" width="70" style="padding-right:10px;">
                            VIN
                        </th>
                        <th class="seller" width="130">
                            Auction
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
                <tbody>
                    <% foreach (var auction in Model.AuctionList)
                    {%>
                    <tr>
                        <td class="fav" width="20">
                            <input id="chkFavorite_<%= auction.Id %>_<%= auction.Vin %>" type="checkbox" <%= auction.IsFavorite ? "checked" : "" %>/>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="year" width="20">
                            <a href="javascript:;"><%= auction.Year %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="make" width="130">
                            <a href="javascript:;"><%= auction.Make.Length > 10 ? auction.Make.Substring(0, 10) : auction.Make %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="model" width="90">
                            <a href="javascript:;"><%= auction.Model %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="trim" width="100">
                            <a href="javascript:;"><%= auction.Trim %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="vin" width="70">
                                <a href="javascript:;"><%= auction.Vin.Length > 8 ? auction.Vin.Substring(auction.Vin.Length-8) : auction.Vin %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="auction" width="130">
                            <%= auction.RegionName %>
                        </td>                        
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="mileage" width="70" align="center">
                            <%= auction.Mileage.ToString("#0,0") %>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="mmr" width="70" align="center">
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
                <tfoot>
                    <tr id="trLoading" style="display:none;"><td colspan="13"><div style="display:inline-block;width:100%;text-align:center;"><img src="../../Content/Images/ajax-loader1.gif" style="border:0;" alt="Loading..."/></div></td></tr>
                </tfoot>
            </table>
        </div>        
    </div><span id="textScroll"></span>    
</div>

<script type="text/javascript">
    var pageNumbers = 1;
    var recordsPerPage = 20;
    var scrollHeight = 200;

    $(document).ready(function () {
        $('.scrollingArea').scroll(function () {
            //$('#textScroll').html('scrollTop ' + $('.scrollingArea').scrollTop() + ' - scrollHeight ' + scrollHeight);
            if ($('.scrollingArea').scrollTop() >= scrollHeight) {
                scrollHeight += 200;
                //block('table#tblVehicles tbody', waitingImage);
                $('#trLoading').show();
                $.ajax({
                    type: "GET",
                    url: "/Search/SearchLazyLoading?pageNumbers=" + (pageNumbers++) + "&recordsPerPage=" + recordsPerPage,
                    data: {},
                    success: function (data) {
                        //unblock('table#tblVehicles tbody');
                        $('#trLoading').hide();
                        $.each(data, function (i, item) {
                            var newTr = "";
                            newTr += '<tr>';
                            newTr += '<td class="fav" width="20">';
                            newTr += '<input id="chkFavorite_' + item.Id + '_' + item.Vin + '" type="checkbox" ' + (item.IsFavorite ? 'checked' : '') + '/>';
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="year" width="30">';
                            newTr += '<a href="javascript:;">' + item.Year + '</a>';
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="make" width="120">';
                            newTr += '<a href="javascript:;">' + (item.Make.length > 10 ? item.Make.substring(0, 10) : item.Make) + '</a>';
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="model" width="90">';
                            newTr += '<a href="javascript:;">' + item.Model + '</a>';
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="trim" width="100">';
                            newTr += '<a href="javascript:;">' + item.Trim + '</a>';
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="vin" width="70">';
                            newTr += '<a href="javascript:;">' + (item.Vin.length > 8 ? item.Vin.substring(0, 8) : item.Vin) + '</a>'
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="region" width="130">';
                            newTr += item.RegionName.lenght > 15 ? item.RegionName.substring(0, 15) : item.RegionName;
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="mileage" width="70">';
                            newTr += accounting.formatNumber(item.Mileage);
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="mmr" width="70">';
                            newTr += accounting.formatMoney(item.Mmr, "$", 0);
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="lane" width="35">';
                            newTr += item.Lane;
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="run" width="35">';
                            newTr += item.Run;
                            newTr += '</td>';
                            newTr += '<td id="viewDetailVehicle_' + item.Id + '" class="condition" width="70">';
                            newTr += '<div class="cr">CR</div>';
                            newTr += ' ' + item.Cr;
                            newTr += '</td>';
                            newTr += '</tr>';

                            $("table#tblVehicles > tbody").append(newTr);
                        });

                    },
                    error: function (err) {
                        //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                        //unblock('table#tblVehicles tbody');
                        $('#trLoading').hide();
                    }
                });

                $("table#tblVehicles").trigger("update");
            }
        });

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

        $("table#tblVehicles").tablesorter({
            // prevent first column from being sortable
            headers: {
                0: { sorter: false },
                7: { sorter: 'price' }, // miles
                8: { sorter: 'price' }, // mmr
                11: { sorter: 'price' }// cr
            }
        });

        $("input[id^='chkFavorite_']").live('click', function () {
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
                    window.location.reload();
                },
                error: function (err) {
                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');

                }
            });
        });

        $("td[id^='viewDetailVehicle_']").live('click', function () {
            var id = this.id.split('_')[1];
            window.location.href = "/Auction/DetailVehicle?id=" + id;
        });
    });
</script>