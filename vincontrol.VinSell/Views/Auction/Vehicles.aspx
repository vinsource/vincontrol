<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.AuctionListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VinSell | Auction
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hdnState" name="hdnState" />
    <input type="hidden" id="hdnRegionCode" name="hdnRegionCode" />
    <input type="hidden" id="hdnRegionName" name="hdnRegionName" />
    <input type="hidden" id="SelectedLane" name="SelectedLane" value="<%= Model.SelectedLane %>" />
    <input type="hidden" id="SelectedRun" name="SelectedRun" value="<%= Model.SelectedRun %>" />
    <input type="hidden" id="SelectedCategory" name="SelectedCategory" value="<%= Model.SelectedCategory %>" />
    <div class="inner-wrap">
         <script type="text/javascript">
             function popupRegion(state) {
               
                 if (state == '') {
                 } else {
                     
                     $.fancybox({
                         href: '/Auction/GetAuctions?state=' + state,
                         'type': 'iframe',
                         'width': 600,
                         'height': 250,
                         'scrolling': 'no',
                         'hideOnOverlayClick': true,
                         //'centerOnScroll': true,
                         'onCleanup': function() {
                         },
                         onClosed: function() {
                             if ($("#hdnRegionCode").val() != "" && $("#hdnState").val() != "") {
                                 blockUI(waitingImage);
                                 window.location.href = "/Auction/GetVehicles?auctionCode=" + $("#hdnRegionCode").val() + "&auctionName=" + $("#hdnRegionName").val() + "&state=" + $("#hdnState").val();
                             }
                         }
                     });
                 }
             }

         </script>
        <input type="hidden" id="AuctionCode" name="AuctionCode" value="<%= (string)ViewData["AUCTIONCODE"] %>" />
        <div class="page-info">
            <span>Auctions<br /></span>
            <span>
            <a  href="javascript:popupRegion('<%= Model.State%>');"> <%= Model.State %></a> - 
            <%= Html.ActionLink(Model.Auction, "GetVehicles", "Auction", new { auctionCode = Model.AuctionCode, auctionName = Model.Auction, state = Model.State },null)%></span>
            <h3>
                Auction Browser</h3>
            <div class="calendar-selection">
                <% if (Model.Date.Count > 0) {%>
                <div class="calendar-header">Select a Day</div>
                
                <% 
                    foreach (var date in Model.Date){
                %>
                <div id="viewByDate_<%= date.ToString("MM/dd/yyyy") %>" <%= date.Equals(Model.Date.FirstOrDefault()) ? "class=active" : "" %>><%= date.ToString("MM/dd/yyyy") %></div>
                <%}%>                
                <%}%>
            </div>
        </div>
        <div id="vehiclesInAuction">
        <%= Html.Partial("VehiclesInAuction", Model) %>
        </div>
  
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
<link href="<%=Url.Content("~/Content/list.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script type="text/javascript">
    var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
    $(document).ready(function () {
        unblockUI();

        $('div[id^="viewByDate_"]').live('click', function () {
            $('div[id^="viewByDate_"]').removeClass('active');
            $(this).addClass('active');
            var date = this.id.split('_')[1];
            blockUI(waitingImage);
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Auction/GetVehiclesInAuction?date=' + date + "&auctionCode=" + $('#AuctionCode').val(),
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#vehiclesInAuction').html(result);
                    unblockUI();
                },
                error: function (err) {
                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    unblockUI();
                }
            });
        });

    });
</script>
</asp:Content>
