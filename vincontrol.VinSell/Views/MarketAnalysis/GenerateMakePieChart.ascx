<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<div style="width: 100%; text-align: center;">
    <%: Html.DropDownList("Make", Model.Make.ToSelectItemList(m => m.Value, m => m.Text, false), "Make", new { @class="make", style="width:70px;" })%>
</div>
<div id="editor-render-1" style="width: 300px; height: 300px; display: inline-block;">
   
</div>
 <div id="legendContainer2">
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        basic_pie((document.getElementById("editor-render-1")), 'Make');

    });

    function basic_pie(container, title) {
        var auctionList = JSON.parse('<%=Model.JsonAuctionList%>');
        var graph;
        var dataArray = [];

        var total = 0;

        for (var i = 0; i < auctionList.length; i++) {
            total += auctionList[i].count;
        }

        for (i = 0; i < auctionList.length; i++) {
            //dataArray.push({ data: [[0, auctionList[i].count]], label: auctionList[i].make + '-' + auctionList[i].count + ' cars (' + (auctionList[i].count * 100 / total).toFixed(2) + '%)' });
            dataArray.push({ data: [[0, auctionList[i].count]], label: auctionList[i].make });
        }

        Flotr.draw(container, dataArray, {
            //title: title,
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
            colors: ['#49DA1C', '#072C36', '#A399BC', '#26D4BD', '#2222FC', '#B360C6', '#67C83E', '#CD167C', '#282FBD', '#CDFEFE', '#AA114D', '#704326', '#2D3249', '#E5115B', '#E10DE9', '#C03CA7', '#A11D32', '#34F663', '#E58F6B', '#B4B52A'],
            mouse: {
                track: true,
                lineColor: 'purple',
                relative: true,
                position: 'ne',
                sensibility: 1,
                trackDecimals: 2,
                trackFormatter: function (obj) {
                    return obj.y.split('.')[0] + ' - ' + obj.series.label + '';
                }
            },
            legend: {
                //                    position: 'se',
                //                    backgroundColor: '#D2E8FF'
                container: $("#legendContainer2"),
                noColumns: 2
            }
        });

        Flotr.EventAdapter.observe(container, 'flotr:click', function (position, name) {
            
            $.each(name.data, function (index, value) {
                if (value.label == position.hit.series.label) {
                    $('#Make').val(value.label);
                    $('#Make option[value="' + value.label + '"]').attr('selected', 'selected');
                    //break;
                }
            });

            FilterMake();
            $('#chartWeekPrice').hide();
        });
    }

</script>
