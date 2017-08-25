<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>
<%@ Import Namespace="System.Runtime.Serialization.Json" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<% if (Model.AuctionList.Count > 0)
   {%>
<div class="filter-box">    
    <div style="float:left;color:White;padding-top:10px;">
        <div style="margin-bottom:10px;"><span style="font-size:0.8em;color:White;" id="numberOfVehicles">Currently displaying <%= Model.AuctionList.Count %> car(s)</span></div>
        <a id="aViewList" href="javascript:;" style="font-size:0.8em;color:White;text-decoration:underline;display:none;">View List</a>
        <a id="aViewChart" href="javascript:;" style="font-size:0.8em;color:White;text-decoration:underline;">View Chart</a>
        |
        <a id="aViewLaneDetail" href="javascript:;" style="font-size:0.8em;color:White;text-decoration:underline;">View Lane Detail</a>
    </div>
    <div class="filter">
        Filter
        <%: Html.DropDownList("Year", Model.Year.ToSelectItemList(m => m.Value, m => m.Text, false), "Year", new { @class="year", style="width:70px;" })%>
        <span id="vehiclesInAuction_Make">
        <%: Html.DropDownList("Make", Model.Make.ToSelectItemList(m => m.Value, m => m.Text, false), "Make", new { @class = "make", style = "width:70px;" })%>
        </span>
        <span id="vehiclesInAuction_Model">
        <%: Html.DropDownList("Model", Model.Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Model", new { @class = "model", style = "width:70px;" })%>
        </span>
        <span id="vehiclesInAuction_Trim">
        <%: Html.DropDownList("Trim", Model.Trim.ToSelectItemList(m => m.Value, m => m.Text, false), "Trim", new { @class = "trim", style = "width:70px;" })%>
        </span>
        <%: Html.DropDownList("Lane", Model.Lane.ToSelectItemList(m => m.Value, m => m.Text, false), "Lane", new { @class = "lane", style = "width:70px;" })%>
        <%: Html.DropDownList("Run", Model.Run.ToSelectItemList(m => m.Value, m => m.Text, false), "Run", new { @class = "run", style = "width:70px;" })%>
        <%: Html.DropDownList("Seller", Model.Seller.ToSelectItemList(m => m.Value, m => m.Text, false), "Seller", new { @class = "seller", style = "width:70px;" })%>
        <%: Html.DropDownList("Category", Model.Category.ToSelectItemList(m => m.Value, m => m.Text, false), "Category", new { @class = "seller", style = "width:80px;" })%>
    </div>
</div>
<div id="data">
<%= Html.Partial("Data", Model) %>
</div>
<%} %>
<script type="text/javascript">
    var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
    $(document).ready(function () {
        var lane = $('#SelectedLane').val();
        var run = $('#SelectedRun').val();
        var category = $('#SelectedCategory').val();
        if (lane != 0) {
            $('#Lane').val(lane);
            $('#Run').val(run);
            $('#Category').val(category);
            Filter();
        }

        $('#Year').change(function () {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Ajax/Makes?year=' + $('#Year').val() + "&type=auction",
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#vehiclesInAuction_Make').html(result);
                    $('#vehiclesInAuction_Model').html('<select id="Model" name="Model" style="width:70px;"><option value="">Model</option></select>');
                    $('#vehiclesInAuction_Trim').html('<select id="Trim" name="Trim" style="width:70px;"><option value="">Trim</option></select>');
                    Filter();
                },
                error: function (err) {

                }
            });
        });

        $('#Make').live('change', function () {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Ajax/Models?year=' + $('#Year').val() + '&make=' + $('#Make').val() + "&type=auction",
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#vehiclesInAuction_Model').html(result);
                    $('#vehiclesInAuction_Trim').html('<select id="Trim" name="Trim" style="width:70px;"><option value="">Trim</option></select>');
                    Filter();
                },
                error: function (err) {

                }
            });

        });

        $('#Model').live('change', function () {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Ajax/Trims?year=' + +$('#Year').val() + '&make=' + $('#Make').val() + '&model=' + $('#Model').val() + "&type=auction",
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#vehiclesInAuction_Trim').html(result);
                    Filter();
                },
                error: function (err) {

                }
            });

        });

        $('#Trim').live('change', function () {
            Filter();
        });

        $('#Lane').change(function () {
            Filter();
        });

        $('#Run').change(function () {
            Filter();
        });

        $('#Seller').change(function () {
            Filter();
        });

        $('#Category').change(function () {
            Filter();
        });

        $('a[id^="groupByLane"]').live('click', function () {
            $('#Lane').val(this.id.split('_')[1]);
            Filter();
        });

        $('a[id^="groupByCategory"]').live('click', function () {
            $('#Lane').val(this.id.split('_')[1]);
            $('#Category').val(this.id.split('_')[2]);
            Filter();
        });

        $('#aViewChart').click(function () {
            $('#aViewChart').hide();
            $('#aViewList').show();
            $('#list-render-0').hide();
            $('#editor-render-0').show();
            $('#legendContainer').show();
            $('#viewLaneDetail').hide();
            //unblockUI();
        });

        $('#aViewList').click(function () {
            $('#aViewChart').show();
            $('#aViewList').hide();
            $('#list-render-0').show();
            $('#editor-render-0').hide();
            $('#legendContainer').hide();
            $('#viewLaneDetail').hide();
            //unblockUI();
        });

        $('#aViewLaneDetail').click(function () {
            $('#viewLaneDetail').show();
            $('#list-render-0').hide();
            $('#editor-render-0').hide();
            $('#legendContainer').hide();
            if ($('#aViewChart').is(":hidden")) {
                $('#aViewChart').show();
                $('#aViewList').hide();
            } else {
                $('#aViewChart').hide();
                $('#aViewList').show();
            }
            //unblockUI();
        });
    });

    function Filter() {
        blockUI(waitingImage);
        $.ajax({
            type: "GET",
            dataType: "html",
            url: '/Auction/FilterVehiclesInAuction?year=' + $('#Year').val() + "&make=" + $('#Make').val() + "&model=" + $('#Model').val() + "&trim=" + $('#Trim').val() + "&lane=" + $('#Lane').val() + "&run=" + $('#Run').val() + "&seller=" + $('#Seller').val() + "&category=" + $('#Category').val(),
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
    }

</script>
