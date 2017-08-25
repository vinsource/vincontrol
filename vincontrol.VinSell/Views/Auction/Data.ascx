<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>
<style type="text/css">
    #legendContainer
    {
        background-color: #fff;
        padding: 2px;
        margin-bottom: 8px;
        border-radius: 3px 3px 3px 3px;
        border: 1px solid #E6E6E6;
        display: inline-block;
        margin: 0 auto;
        overflow-x: auto;
        overflow-y: auto;
        height: 500px;
        width: 20%;
        float: left;
    }
    
    #viewLaneDetail .lane {
        padding: 0px 10px;
        font-size: 14px;
    }
    
    #viewLaneDetail ul 
    {
    	font-size: 12px;
        margin: 5px 0px 10px 0px;
        padding: 0px;
        list-style: none;
    }
    
    #viewLaneDetail li 
    {
    	position: relative;
        margin: 0px 0px 0px 8px;
        padding: 2px 0px 4px 18px;
    	background: url('/content/images/bullet.gif') 9px 6px no-repeat;
    }
</style>
<div id="list-render-0" class="content">
    <div class="scrollableContainer">
        <div class="scrollingArea" id="scrollingArea">
            <table id="tblVehicles" class="list" cellspacing="0">
                <thead>
                    <tr style="cursor: pointer;">
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
                <tbody>
                    <% foreach (var auction in Model.AuctionList)
                       {%>
                    <tr>
                        <td class="fav" width="20">
                            <input id="chkFavorite_<%= auction.Id %>_<%= auction.Vin %>" type="checkbox" <%= auction.IsFavorite ? "checked" : "" %> />
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="year" width="30">
                            <a href="javascript:;">
                                <%= auction.Year %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="make" width="120">
                            <a href="javascript:;">
                                <%= auction.Make.Length > 10 ? auction.Make.Substring(0, 10) : auction.Make %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="model" width="90">
                            <a href="javascript:;">
                                <%= auction.Model %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="trim" width="100">
                            <a href="javascript:;">
                                <%= auction.Trim %></a>
                        </td>
                        <td id="viewDetailVehicle_<%= auction.Id %>" class="vin" width="80">
                            <a href="javascript:;">
                                <%= auction.Vin.Length > 8 ? auction.Vin.Substring(auction.Vin.Length - 8) : auction.Vin %></a>
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
                            <div class="cr">
                                CR</div>
                            <%= auction.Cr %>
                        </td>
                    </tr>
                    <%}%>
                </tbody>
                <tfoot>
                    <tr id="trLoading" style="display: none;">
                        <td colspan="12">
                            <div style="display: inline-block; width: 100%; text-align: center;">
                                <img src="../../Content/Images/ajax-loader1.gif" style="border: 0;" alt="Loading..." /></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
<div id="legendContainer" style="display: none;">
</div>
<div id="editor-render-0" style="width: 77%; min-height: 500px; display: inline-block;">
</div>
<div id="viewLaneDetail" class="content" style="display: none; overflow: auto;">
    <% foreach (var group in Model.AuctionList.OrderBy(i => i.Lane).GroupBy(i => i.Lane)){%>
    <div class="lane">
        <p><b>Lane <%= group.Key %></b> - <%=String.IsNullOrEmpty(group.FirstOrDefault().LaneDescription) ? "": group.FirstOrDefault().LaneDescription.ToUpper() %></p>
        <p>
            <%--<%= Html.ActionLink("List All " + group.Count() + " Vehicles in Lane", "GetVehiclesByLane", "Auction", new { auctionCode = Model.AuctionCode, auctionName = Model.Auction, state = Model.State, lane = group.Key }, null)%>--%>
            <a id="groupByLane_<%= group.Key %>" href="javascript:;">List All <%= group.Count() %> Vehicles in Lane</a>
        </p>
        <ul>
        <% foreach (var subgroup in group.OrderBy(i => i.Category).GroupBy(i => i.Category)){%>
            <li><b><%= subgroup.Key %></b><br />
            <a id="groupByCategory_<%= group.Key %>_<%= subgroup.Key %>" href="javascript:;"><%= subgroup.Count() %> Vehicles</a>
            <%--<%= Html.ActionLink(subgroup.Count() + " Vehicles", "GetVehiclesByCategory", "Auction", new { auctionCode = Model.AuctionCode, auctionName = Model.Auction, state = Model.State, lane = group.Key, category = subgroup.Key }, null)%>--%>
            </li>
        <%}%>
        </ul>
    </div>
    <%}%>
    
</div>
<script src="<%=Url.Content("~/Scripts/flotr2/flotr2.js")%>" type="text/javascript"></script>
<script type="text/javascript">
    (function basic_pie(container) {
        var auctionList = JSON.parse('<%=Model.JsonAuctionList%>');
        var graph;
        var dataArray = [];

        var total = 0;

        for (var i = 0; i < auctionList.length; i++) {
            total += auctionList[i].count;
        }

       
        for (i = 0; i < auctionList.length; i++) {
            dataArray.push({ data: [[0, auctionList[i].count]], label: auctionList[i].make + ' - ' + auctionList[i].count + ' cars (' + (auctionList[i].count * 100 / total).toFixed(2) + '%)' });
        }

        Flotr.draw(container, dataArray, {
            HtmlText: false,
            grid: {
                verticalLines: false,
                horizontalLines: false
            },
            xaxis: { showLabels: false },
            yaxis: { showLabels: false },
            pie: {
                show: true,
                explode: 6,
                labelFormatter: function (total, value) {
                    return '';
                }
            },
            colors: ['#49DA1C', '#072C36', '#A399BC', '#26D4BD', '#2222FC', '#B360C6', '#67C83E', '#CD167C', '#282FBD', '#CDFEFE', '#AA114D', '#704326', '#2D3249', '#E5115B', '#E10DE9', '#C03CA7', '#A11D32', '#34F663', '#E58F6B',
                '#B4B52A'],
            mouse: {
                    track: true,
                    lineColor: 'purple',
                    relative: true,
                    position: 'ne',
                    sensibility: 1,
                    trackDecimals: 2,
                    trackFormatter: function (obj) {
                        return obj.series.label + '';
                    }
                },
            legend: {
                position: 'se',
                backgroundColor: '#D2E8FF',
                  container:$("#legendContainer"),            
            noColumns: 1
                      
            }
        });

        Flotr.EventAdapter.observe(container, 'flotr:click', function (position, name) {
                var make = "";
                $.each(name.data, function (index, value) {
                    if (value.label == position.hit.series.label) {
                        make = value.label.split(' - ')[0];
                        $('#Make').val(make);
                        $('#Make option[value=' + make + ']').attr('selected', 'selected');
                        //break;
                    }
                });

                blockUI(waitingImage);
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: '/Auction/FilterVehiclesInAuction?year=' + $('#Year').val() + "&make=" + make + "&model=" + $('#Model').val() + "&trim=" + $('#Trim').val() + "&lane=" + $('#Lane').val() + "&run=" + $('#Run').val() + "&seller=" + $('#Seller').val() + "&category=" + $('#Category').val(),
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        $('#data').html(result);
                        $('#aViewChart').show();
                        $('#aViewList').hide();
                        var rows = $('table#tblVehicles >tbody >tr').length;
                        $('#numberOfVehicles').html(rows == 0 ? 'No cars found' : 'Currently displaying ' + rows + ' car(s)');
                
                        unblockUI();
                    },
                    error: function (err) {
                        jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');                
                        unblockUI();
                    }
                });
                
            });
    })(document.getElementById("editor-render-0"));

    $(document).ready(function () {
        var scrollTop = '<%= SessionHandler.ScrollTop %>';
        $('.scrollingArea').scrollTop(scrollTop);
        $('#list-render-0').show();
        $('#editor-render-0').hide();
        $('#legendContainer').hide();
        $('#viewLaneDetail').hide();

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
                //11: { sorter: 'price', sortInitialOrder: 'asc'}// cr
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

                },
                error: function (err) {
                    //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                }
            });
        });

        $("td[id^='viewDetailVehicle_']").live('click', function () {
            var id = this.id.split('_')[1];
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Auction/MarkScrollTop?position=' + $('.scrollingArea').scrollTop(),
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {                    
                    window.location.href = "/Auction/DetailVehicle?id=" + id;
                },
                error: function (err) {

                }
            });

        });
    });
</script>
