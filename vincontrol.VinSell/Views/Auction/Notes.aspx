<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VinSell | Notes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <div class="page-info">
            <span><br/></span><span><br/></span>
            <h3>Notes</h3>
        </div>
        <div class="filter-box">
            <div class="calendar-selection" style="bottom: 0; float: left; margin-top: 35px; padding: 5px 0px; padding-top: 0px; position: relative; right: 0; font-size: 1.2em;">
                <div id="upcoming" class="active">Upcoming Auctions</div>
                <div id="past">Past Auctions</div>
            </div>
            <div class="filter">
                Filter                
                <%: Html.DropDownList("Year", Model.Year.ToSelectItemList(m => m.Value, m => m.Text, false), "Year", new { @class="year", style="width:60px;" })%>
                <span id="notes_Make">
                <%: Html.DropDownList("Make", Model.Make.ToSelectItemList(m => m.Value, m => m.Text, false), "Make", new { @class = "make", style = "width:70px;" })%>
                </span>
                <span id="notes_Model">
                <%: Html.DropDownList("Model", Model.Model.ToSelectItemList(m => m.Value, m => m.Text, false), "Model", new { @class = "model", style = "width:70px;" })%>
                </span>
                <span id="notes_Trim">
                <%: Html.DropDownList("Trim", Model.Trim.ToSelectItemList(m => m.Value, m => m.Text, false), "Trim", new { @class = "trim", style = "width:70px;" })%>
                </span>
                <%: Html.DropDownList("Lane", Model.Lane.ToSelectItemList(m => m.Value, m => m.Text, false), "Lane", new { @class = "lane", style = "width:60px;" })%>
                <%: Html.DropDownList("Run", Model.Run.ToSelectItemList(m => m.Value, m => m.Text, false), "Run", new { @class = "run", style = "width:70px;" })%>
                <%: Html.DropDownList("Seller", Model.Seller.ToSelectItemList(m => m.Value, m => m.Text, false), "Seller", new { @class = "seller", style = "width:65px;" })%>
                <%: Html.DropDownList("Category", Model.Category.ToSelectItemList(m => m.Value, m => m.Text, false), "Category", new { @class = "seller", style = "width:80px;" })%>
            </div>
        </div>
        <div id="data">
            <%= Html.Partial("NotedData", Model) %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
<link href="<%=Url.Content("~/Content/list.css")%>" rel="stylesheet" type="text/css" />
<style type="text/css">
    .list tbody tr:nth-child(2n+2) td {
      background: White;
    }
    
    .list tbody tr[id^=auction_] td {      
      background: White;
      border: 1px solid #ddd;
    }
    
    .list tbody tr[id^=auction_]:hover td 
    {
    	color: Black;
    }
        
    .list tbody tr[id^=date_] td {
      background: #ddd;
    }
    
    .list tbody tr[id^=date_]:hover td
    {
    	color: Black;
    	background: #ddd;
    }
    
    .list tbody tr[id^=viewByAuction_]:hover td {
      background: #161e8a;
      color: white;
    }
</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script type="text/javascript">
    var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
    $(document).ready(function () {
        $('#Year').change(function () {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Ajax/Makes?year=' + $('#Year').val() + "&type=note",
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#notes_Make').html(result);
                    $('#notes_Model').html('<select id="Model" name="Model" style="width:70px;"><option value="">Model</option></select>');
                    $('#notes_Trim').html('<select id="Trim" name="Trim" style="width:70px;"><option value="">Trim</option></select>');
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
                url: '/Ajax/Models?year=' + $('#Year').val() + '&make=' + $('#Make').val() + "&type=note",
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#notes_Model').html(result);
                    $('#notes_Trim').html('<select id="Trim" name="Trim" style="width:70px;"><option value="">Trim</option></select>');
                    Filter();
                },
                error: function (err) {

                }
            });
        });

        $('#Model').live('change',function () {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Ajax/Trims?year=' + +$('#Year').val() + '&make=' + $('#Make').val() + '&model=' + $('#Model').val() + "&type=note",
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#notes_Trim').html(result);
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

        $('#upcoming').click(function () {
            $('#past').removeClass('active');
            $(this).addClass('active');
            $('#tblVehicles_upcoming').show();
            $('#tblVehicles_past').hide();
        });

        $('#past').click(function () {
            $('#upcoming').removeClass('active');
            $(this).addClass('active');
            $('#tblVehicles_upcoming').hide();
            $('#tblVehicles_past').show();
        });
    });

    function Filter() {
        blockUI(waitingImage);
        $.ajax({
            type: "GET",
            dataType: "html",
            url: '/Auction/FilterNotedVehicles?year=' + $('#Year').val() + "&make=" + $('#Make').val() + "&model=" + $('#Model').val() + "&trim=" + $('#Trim').val() + "&lane=" + $('#Lane').val() + "&run=" + $('#Run').val() + "&seller=" + $('#Seller').val() + "&category=" + $('#Category').val(),
            data: {},
            cache: false,
            traditional: true,
            success: function (result) {
                $('#data').html(result);
                $('#aViewChart').show();
                $('#aViewList').hide();
                unblockUI();
            },
            error: function (err) {
                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                unblockUI();
            }
        });
    }
</script>
</asp:Content>
