<script id="wishListTemplate" type="text/x-jsrender">
    <div class="contain_list list_odd Used borderBottom">
        <div id="deleteBtn{{>Id}}" stocking-id="{{>Id}}" stocking-source="{{>Source}}" title="close" style="background: #3366cc; float: left; color: white; padding: 5px 10px; border: 2px solid white; font-weight: bold; position: absolute; top: 0px; /* right: 0; */ z-index: 100; cursor: pointer;">
                        X
                    </div>
        <div class="divWishListInfoLeft">
            <a class="iframe" href="{{>URLDetail}}">
                <img src="../Content/images/vincontrol/noImageAvailable-wishList.png" height="100" />
            </a>
        </div>
        <div class="divWishListInfoMiddle">
            <div class="WishListInfoHeader"><a class="iframe" href="{{>URLDetail}}">{{>Make}} {{>Model}}</a></div>
            <div style="clear: both"></div>
            <div class="WishListInfoContent">
                <div class="WishListInfoContentLeft">
                    <%--Engine:<br />
                    Style:<br />--%>
                    {{if IsBrand==true}}
                        <div class="contentLeft">Brand: </div>
                        <div class="contentRight">{{>Make}}</div>
                    {{else}}
                        <div class="contentLeft">Segment: </div>
                        <div class="contentRight">{{>Segment}}</div>
                    {{/if}}
                    <div style="clear:both;"></div>
                    <div class="contentLeft">Age: </div>
                    <div class="contentRight">{{>Age}}</div>
                    <div style="clear:both;"></div>
                    <%--<div class="contentLeft">AddedDate: </div>
                    <div class="contentRight">{{>AddedDate}}</div>--%>
                    <div class="contentLeft">Market: </div>
                    <div class="contentRight">N/A</div>
                    <div style="clear:both;"></div>
                    <div class="contentLeft">Auction: </div>
                    <div class="contentRight">N/A</div>
                    <%--Base MSRP:<br />--%>
                </div>
                <%--<div class="WishListInfoContentRight">
                    Exterior Colors<br />
                    <div style="width: 100%">
                        <div style="width: 30px; height: 20px; background-color: black; float: left"></div>
                        <div style="width: 30px; height: 20px; background-color: blue; float: left; margin-left: 5px;"></div>
                        <div style="width: 30px; height: 20px; background-color: yellow; float: left; margin-left: 5px;"></div>
                        <div style="width: 30px; height: 20px; background-color: red; float: left; margin-left: 5px;"></div>
                        <div style="width: 30px; height: 20px; background-color: purple; float: left; margin-left: 5px;"></div>
                    </div>
                    <div style="clear: both; height: 5px;"></div>
                    <div style="width: 100%">
                        <div style="width: 30px; height: 20px; background-color: blue; float: left"></div>
                        <div style="width: 30px; height: 20px; background-color: black; float: left; margin-left: 5px;"></div>
                        <div style="width: 30px; height: 20px; background-color: red; float: left; margin-left: 5px;"></div>
                        <div style="width: 30px; height: 20px; background-color: purple; float: left; margin-left: 5px;"></div>
                        <div style="width: 30px; height: 20px; background-color: yellow; float: left; margin-left: 5px;"></div>
                    </div>
                </div>--%>
            </div>
        </div>
        <div class="divWishListInfoRight">
            <div class="WishListInfoHeaderRight">Market Statistics</div>
            <div style="clear: both"></div>
            <div class="divWishListStatistics">
                <div class="divWishListStatisticsHeader">
                    <div class="divHighest">Highest</div>
                    <div class="divAverage">Average</div>
                    <div class="divLowest">Lowest</div>
                    <div class="divTurn">Turn</div>
                    <div class="divHistory">History</div>
                </div>
                <div class="divWishListStatisticsContent">
                    <div class="divHighestContent" id="Highest">{{>Highest}}</div>
                    <div class="divAverageContent" id="Average">{{>Average}}</div>
                    <div class="divLowestContent" id="Lowest">{{>Lowest}}</div>
                    <div class="divTurnContent">{{>Turn}}</div>
                    <div class="divHistoryContent">{{>History}}</div>
                </div>
            </div>
        </div>
        <div style="clear: both"></div>
    </div>
</script>
<script type="text/javascript">
    $.views.helpers({ getPhoto: vm.getPhoto, getVin: vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });
</script>
