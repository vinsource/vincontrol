<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.RegionActionSummarizeListViewModel>" %>

<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VinSell | Auction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <form id="data">
        <input type="hidden" id="hdnState" name="hdnState" />
        <input type="hidden" id="hdnRegionCode" name="hdnRegionCode" />
        <input type="hidden" id="hdnRegionName" name="hdnRegionName" />
        <div class="page-info">
            <span>
                <br />
            </span><span>Auctions</span>
            <h3>
                Select a State</h3>
            <div class="state-dropdown">
                Or select from the dropdown menu
                <%: Html.DropDownList("State", Model.RegionAuctionSummarizeList.ToSelectItemList(m => m.Name, m => m.Name, false), "Select a State", new { style = "width:110px;" })%>
            </div>
        </div>
        <div class="filter-box">
        </div>
        <div class="content">
            <div class="map" id="map">
            </div>
            <script src="<%=Url.Content("~/Scripts/leaflet/leaflet.js")%>" type="text/javascript"></script>
            <script src="<%=Url.Content("~/Scripts/USmapchart/us-states.js")%>" type="text/javascript"></script>
            <script src="<%=Url.Content("~/Scripts/USmapchart/AuctionMap.js")%>" type="text/javascript"></script>
            <script src="<%=Url.Content("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
            <script type="text/javascript">
                function createSellMap() {
                    var map = L.map('map', { zoomControl: false, minZoom: 4, maxZoom: 4, dragging: false }).setView([34.8, -96], 4);
                    map.doubleClickZoom.disable(); 
//                    L.tileLayer('http://{s}.tile.cloudmade.com/{key}/{styleId}/256/{z}/{x}/{y}.png', {
//                        attribution: 'Map data &copy; 2011 OpenStreetMap contributors, Imagery &copy; 2011 CloudMade',
//                        key: 'BC9A493B41014CAABB98F0471D759707',
//                        styleId: 22677
//                    }).addTo(map);
                    var auctionMap = new AuctionMap(map);
                    auctionMap.init();
                    auctionMap.convertData('<%=Model.JsonRegionAuctionSummarizeList%>');
                    auctionMap.createInformationDiv(map);
                    auctionMap.bindMap();
                    auctionMap.createLegend();
                }
            </script>
        </div>
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        var currentTab = 0;
        var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
        $(document).ready(function () {
            createSellMap();
            $("table#tableResults").tablesorter({
                // prevent first column from being sortable
                headers: {
                    0: { sorter: false },
                    12: { sorter: false }
                }
            });

            $("a[id^='region_']").live('click', function () {
                
                var id = this.id.split("_")[1];                
                blockUI(waitingImage);

                window.location.href = "/Auction/GetVehicles?auctionCode=" + id + "&auctionName=" + $(this).attr("regionname") + "&state=" + $(this).attr("statename");
                
            });

            $("select[id='Year']").live('change', function () {
                var year = $(this).val();
                if (year == '') {
                } else {
                    $("table[id='tableResults_" + currentTab + "'] >tbody >tr").each(function (index, value) {
                        //alert(index + " " + value);
                    });
                }

            });

            $("select[id='State']").live('change', function () {
                var state = $(this).val();
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
                        'onCleanup': function () {
                        },
                        onClosed: function () {
                            if ($("#hdnRegionCode").val() != "" && $("#hdnState").val() != "") {
                                blockUI(waitingImage);
                                window.location.href = "/Auction/GetVehicles?auctionCode=" + $("#hdnRegionCode").val() + "&auctionName=" + $("#hdnRegionName").val() + "&state=" + $("#hdnState").val();
                            }
                        }
                    });
                }

            });

            $("div[id^='viewByDate_']").live('click', function () {
                var id = this.id.split("_")[1];
                $("div[id^='viewByDate_']").removeClass('active');
                $(this).addClass('active');

                //$("table[id^='tableResults_']").slideToggle("slow");
                //$("table[id^='tableResults_" + id + "']").slideUp("slow");
                $("table[id^='tableResults_']").hide();
                $("table[id='tableResults_" + id + "']").show();
                var rowCount = $("table[id='tableResults_" + id + "'] >tbody >tr").length;
                $('#NumberOfAuctions').html(rowCount == 0 ? "No auctions found" : rowCount + ' auction(s)');

                currentTab = id;
            });
        });
    </script>
</asp:Content>
