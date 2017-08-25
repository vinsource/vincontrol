<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<div style="width: 100%; text-align: center;">
    <%: Html.DropDownList("Model", Model.Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Model", new { @class="model", style="width:70px;" })%>
</div>
<div>
    <div id="editor-render-2" style="width: 300px; height: 300px; display: inline-block;">
    </div>
    <div id="legendContainer1">
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        basic_pie((document.getElementById("editor-render-2")), 'Model');
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
            dataArray.push({ data: [[0, auctionList[i].count]], label: auctionList[i].model });
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
                container: $("#legendContainer1"),
                noColumns: 2
            }
        });

        Flotr.EventAdapter.observe(container, 'flotr:click', function (position, name) {
            
            $.each(name.data, function (index, value) {
                if (value.label == position.hit.series.label) {
                    $('#Model').val(value.label);
                    $('#Model option[value="' + value.label + '"]').attr('selected', 'selected');
                    //break;
                }
            });

            $('#chartWeekPrice').show();
            FilterModel();
        });
    }
  
</script>
