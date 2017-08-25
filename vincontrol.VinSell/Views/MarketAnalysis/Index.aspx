<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>

<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VinSell | Market Analysis
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
            height: 100px;
            width: 300px;
            
        }
        #legendContainer1, #legendContainer2
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
            height: 100px;
            width: 300px;
            
        }
        .bottom-cap { padding:0; }
    </style>
    <div class="inner-wrap">
        <div class="page-info">
            <span>
                <br />
            </span><span>
                <br />
            </span>
            <h3>
                Market Analysis</h3>
        </div>
        <div class="filter-box">
        </div>
        <div id="data">
            <div id="list-render-0" class="content" style="text-align: center;height: 700px !important;" >
                <div style="display: inline-block;float:left; margin-left:20px;">
                  
                    <div id="chartYear" style="width: 300px; min-height: 300px; display: inline-block;">
                        <div style="width: 100%; text-align: center;">
                            <%: Html.DropDownList("Year", Model.Year.ToSelectItemList(m => m.Value, m => m.Text, false), "Year", new { @class="year", style="width:70px;" })%>
                        </div>
                        <div id="editor-render-0" style="width: 300px; height: 300px; display: inline-block;
                            border: 0;">
                        </div>
                          <div id="legendContainer">
                    </div>
                    </div>
                </div>
                <div id="chartMake" style="width: 312px; min-height: 200px; display: inline-block; float: left">
                </div>
                <div id="chartModel" style="width: 312px; min-height: 200px; display: inline-block; float: left">
                </div>
                <div id="marketValue" style="padding: 20px; text-align: center; display: none;">
                    <div id="numberOfSoldVehicles">
                    </div>
                    <div id="averageMarketPrice">
                    </div>
                </div>
                <div id="chartWeekPrice" style="width: 800px; height: 200px; display: inline-block; float: clear; margin-top: 25px; border: 0;" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/Scripts/flotr2/flotr2.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
        $(document).ready(function () {
            basic_pie((document.getElementById("editor-render-0")), 'Year');

            $('#Year').change(function () {
                if ($('#Year').val() != '') {
                    Filter();
                    $('#chartModel').html('');
                    $('#chartWeekPrice').hide();
                } else {
                    $('#chartMake').html('');
                    $('#chartModel').html('');
                    $('#marketValue').hide();
                    $('#chartWeekPrice').hide();                    
                }
            });

            $('#Make').live('change', function () {
                if ($('#Make').val() != '') {
                    FilterMake();
                    $('#chartWeekPrice').hide();
                } else {
                    $('#chartModel').html('');
                    $('#marketValue').hide();
                    $('#chartWeekPrice').hide();
                }
            });

            $('#Model').live('change', function () {
                $('#chartWeekPrice').show();
                if ($('#Model').val() != '') {
                    FilterModel();
                } else {
                    $('#marketValue').hide();
                    $('#chartWeekPrice').hide();
                }
            });
        });

        function Filter() {
            blockUI(waitingImage);
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/MarketAnalysis/GenerateMakePieChart?year=' + $('#Year').val(),
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#chartMake').html(result);
                    $('#marketValue').hide();
                    unblockUI();
                },
                error: function (err) {
                    //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    unblockUI();
                }
            });
        }

        function FilterMake() {
            $('#chartWeekPrice').show();
            blockUI(waitingImage);
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/MarketAnalysis/GenerateModelPieChart?year=' + $('#Year').val() + '&make=' + $('#Make').val(),
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#chartModel').html(result);
                    $('#marketValue').hide();
                    unblockUI();
                },
                error: function (err) {
                    //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    unblockUI();
                }
            });
        }

        function FilterModel() {
            blockUI(waitingImage);
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/MarketAnalysis/GetMarketValue?year=' + $('#Year').val() + '&make=' + $('#Make').val() + '&model=' + $('#Model').val(),
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#marketValue').show();
                    drawLineChart(document.getElementById("chartWeekPrice"), result);
                    unblockUI();
                },
                error: function (err) {
                    //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    unblockUI();
                }
            });
        }

        function basic_pie(container, title) {
            var auctionList = JSON.parse('<%=Model.JsonAuctionList%>');
            var graph;
            var dataArray = [];

            var total = 0;

            for (var i = 0; i < auctionList.length; i++) {
                total += auctionList[i].count;
            }

            for (i = 0; i < auctionList.length; i++) {
                dataArray.push({ data: [[0, auctionList[i].count]], label: auctionList[i].year });
            }

            Flotr.draw(container, dataArray, {
                //title: title,
                HtmlText: false,
                grid: {
                    verticalLines: false,
                    horizontalLines: false,
                    backgroundColor: null
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
                colors: ['#49DA1C', '#072C36', '#A399BC', '#26D4BD', '#2222FC', '#B360C6', '#67C83E', '#CD167C', '#282FBD', '#CDFEFE', '#AA114D', '#704326', '#2D3249', '#E5115B', '#E10DE9', '#C03CA7', '#A11D32', '#34F663', '#E58F6B', '#B4B52A'],
                mouse: {
                    track: true,
                    lineColor: 'purple',
                    relative: true,
                    position: 'ne',
                    sensibility: 1,
                    trackDecimals: 2,
                    trackFormatter: function (obj) {
                        return obj.y.split('.')[0] + ' - ' + obj.series.label + 's';
                    }
                },
                legend: {
                    container: $("#legendContainer"),
                    noColumns: 5
                }

            });

            /**
            * Observe the 'flotr:click' event. When the graph is clicked, add the click
            * position to the d1 series and redraw the graph.
            */
            Flotr.EventAdapter.observe(container, 'flotr:click', function (position, name) {

                $.each(name.data, function (index, value) {
                    if (value.label == position.hit.series.label) {
                        $('#Year').val(value.label);
                        $('#Year option[value=' + value.label + ']').attr('selected', 'selected');
                        //break;
                    }
                });

                Filter();
                $('#chartModel').html('');
                $('#chartWeekPrice').hide();
            });

            //            Flotr.EventAdapter.observe(container, 'flotr:mousemove', function (event, position, item, name) {
            //                var tmp = "";
            //                $.each(dataArray.data, function (index, value) {
            //                    if (value.data[0][1] == (position.relY)) {
            //                        var tmp = value.data[0][1] + ' ' + value.label + 's';
            //                        //break;
            //                    }
            //                });

            //                $("#chartYear .flotr-mouse-value").css({ left: '0px' });
            //                if (tmp == '') {
            //                    $("#chartYear .flotr-mouse-value").hide();
            //                } else {
            //                    $("#chartYear .flotr-mouse-value").show();
            //                    $("#chartYear .flotr-mouse-value").html(tmp);
            //                }
            //            });
        }

        function drawLineChart(container, weekData) {
            var weekList = JSON.parse(weekData);

            var d1 = [];
            for (var i = 0; i < weekList.length; i++) {
                d1.push([new Date(parseInt(weekList[i].day.substr(6))), weekList[i].averageprice]);

            }

            console.log(weekList);
            console.log(d1);

            // Draw Graph
            Flotr.draw(container, [d1], {
                  mouse: {
                track: true,
                relative: true
            },
                xaxis: {                   
                   mode: "time",
                   noTicks: 6
                }, lines : { show : true },
                 markers: {
                show: true,
                position: 'ct'
              },
                points : { show : true },
                grid: {                 
                    verticalLines: false
                }
            });
        }    
  
    </script>
</asp:Content>
