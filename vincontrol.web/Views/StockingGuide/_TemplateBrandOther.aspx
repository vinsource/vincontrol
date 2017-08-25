<script id="brandOtherTemplate" type="text/x-jsrender">  
     <div class="v3BrandOther_row">      
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumFirst">{{>Model}}</div>
         <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>History}}</div>
         <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" id="divStock_{{>SGDealerSegmentId}}">{{>Stock}}</div>
         {{if History == 0 && Stock == 0 && Guide == 0 }}
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Guide}}</div>
         {{else}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">
            <input type="text" class="txtGuide" id="txtGuide_{{>SGDealerSegmentId}}" value="{{>Guide}}" /><input type="hidden" id="hdGuide_{{>SGDealerSegmentId}}" value="{{>Guide}}" />
        </div>
         {{/if}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" id="divBalance_{{>SGDealerSegmentId}}">
            {{if Balance>0}}
            <div class="v3BrandOther_barTotal_holder v3BalanceOver">
                +{{>Balance}}
            </div>
            <div class="v3BrandOther_barColor_holder">
                <div class="v3BrandOther_barColor_right"></div>
                <div class="v3BrandOther_barColor_left">
                    <div class="v3BrandOther_barLeft_value v3BalanceOver" style="width: {{>BalancePercent}}"></div>
                </div>
            </div>
            {{else if Balance<0}}
            <div class="v3BrandOther_barTotal_holder v3BalanceUnder">
                {{>Balance}}
            </div>
            <div class="v3BrandOther_barColor_holder">
                <div class="v3BrandOther_barColor_right">
                    <div class="v3BrandOther_barRight_value v3BalanceUnder" style="width: {{>BalancePercent}}"></div>
                </div>
                <div class="v3BrandOther_barColor_left"></div>
            </div>
            {{else}}
            <div class="v3BrandOther_barTotal_holder v3BalanceEqual">
                {{>Balance}}
            </div>
            <div class="v3BrandOther_barColor_holder v3BalanceEqualBar">
                <div class="v3BrandOther_barColor_right"></div>
                <div class="v3BrandOther_barColor_left">
                </div>
            </div>
            {{/if}}
        </div>
         <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="font-size: 10px">{{>Supply}} days</div>
         <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Age}}</div>
         <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>StrTurnOver}}</div>
         {{if History == 0 && Stock == 0 && Guide == 0 }}
            <div class="v3BrandOther_collumn v3BrandOther_collumnMedium">$
                <input type="text" disabled="disabled" class="txtGrossPerUnit" id="txtGrossPerUnit_{{>SGDealerSegmentId}}" value="{{>GrossPerUnit}}" /><input type="hidden" id="hdGrossPerUnit_{{>SGDealerSegmentId}}" value="{{>GrossPerUnit}}" /></div>
         {{else}}
            <div class="v3BrandOther_collumn v3BrandOther_collumnMedium">$
                <input type="text" class="txtGrossPerUnit" id="txtGrossPerUnit_{{>SGDealerSegmentId}}" value="{{>GrossPerUnit}}" /><input type="hidden" id="hdGrossPerUnit_{{>SGDealerSegmentId}}" value="{{>GrossPerUnit}}" /></div>
         {{/if}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Recon}}</div>
         <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumLast">
             <div class="v3CollumLast_text" id="divWishListText_{{>SGDealerSegmentId}}">
                 {{if Balance>0}}                    
                    <label style="color: blue">BUY</label>
                 {{else if Balance<0}}
                    OVERSTOCK
                {{else}}
                    .................
                {{/if}}
             </div>
             <div class="v3CollumLast_cb">
                 <%--{{if History == 0 && Stock == 0 && Guide == 0 }}
                    
                {{else}}
                    {{if IsWishList==true}}
                        <input type="checkbox" id="chkWishList_{{>SGDealerSegmentId}}" checked="checked" />
                    {{else}}
                        <input type="checkbox" id="chkWishList_{{>SGDealerSegmentId}}" />
                    {{/if}}
                {{/if}}--%>
             </div>
         </div>
         <div class="v3BrandOther_Segments_Detail">
             <div class="v3BrandOther_SD_btn">
                 <div class="v3BrandOther_btn_item" id="v3BrandOtherInventory_{{>SGDealerSegmentId}}">
                     Inventory
                 </div>
                 <div class="v3BrandOther_btn_item" id="v3BrandOtherMarket_{{>SGDealerSegmentId}}">
                     Your Market
                 </div>
                 <div id="v3BrandDetailClose_{{>SGDealerSegmentId}}" title="close" class="v3BrandDetailClose">x</div>
             </div>
             <div id="SDTableMarket_{{>SGDealerSegmentId}}" class="v3BrandOther_SD_table">
                 <div class="v3BrandOther_SD_row v3BrandOther_SD_rowTop">
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" value="marketMake" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Make
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" value="marketModel" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Model
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" style="background-color: #3266CC" value="marketYourStock" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Your Stock
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium v3BalanceEqual" value="marketMarketStock" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Market Stock
                     </div>

                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketHistory" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         History(30d)
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketSupply" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Supply
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketAge" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Age
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketTurn" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Turn
                     </div>

                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnLarge" value="marketWishList" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGMarketDealerSegmentDetailId}}">
                         Wishlist
                     </div>
                 </div>
                 <div id="SDRowsMarket_{{>SGDealerSegmentId}}">
                     {{for SGMarketDealerSegmentDetails tmpl='#brandOtherDetailTemplate' /}}
                 </div>
             </div>
             <div id="SDTableInventory_{{>SGDealerSegmentId}}" class="v3BrandOther_SD_table" style="display: none">
                 <div class="v3BrandOther_SD_row v3BrandOther_SD_rowTop">
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" value="inventoryMake" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Make
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" value="inventoryModel" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Model
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventoryInStock" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         In Stock
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventoryOU" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         O/U
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventoryGuide" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Guide
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventoryHistory" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         History
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventoryRecon" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Recon
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventoryTurnOver" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Turn
                     </div>
                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" value="inventorySupply" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Supply
                     </div>

                     <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnLarge" value="inventoryWishList" sortDirection="up" dealersegmentid="{{>SGDealerSegmentId}}" detailid="{{>SGInventoryDealerSegmentDetailId}}">
                         Wishlist
                     </div>
                 </div>
                 <div id="SDRowsInventory_{{>SGDealerSegmentId}}">
                     {{for SGInventoryDealerSegmentDetails tmpl='#brandOtherInventoryDetailTemplate' /}}
                 </div>
                 <%--<div class="v3BrandOther_SD_row v3BrandOther_rowZero">
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
                        Audi
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
                        EL
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        5
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceUnder">
                        -5
                    </div>

                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        0
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        0
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        0
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        0
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        0
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnLarge">
                        <div class="v3CollumLast_text">
                            -------
                        </div>
                        <div class="v3CollumLast_cb">
                            <input type="checkbox" />
                        </div>
                    </div>
                </div>--%>
             </div>
         </div>
     </div>
</script>

<script id="brandHeaderOtherTemplate" type="text/x-jsrender">
    <div class="v3BrandOther_row v3BrandOther_rowTop">
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumFirst">Used Other</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>History}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" >{{>Stock}} </div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" id="divGuideOtherHeader">{{>Guide}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium">
            {{if Balance>0}}
            <div class="v3BrandOther_barTotal_holder v3BalanceOver">
                +{{>Balance}}
            </div>
            <div class="v3BrandOther_barColor_holder">
                <div class="v3BrandOther_barColor_right"></div>
                <div class="v3BrandOther_barColor_left">
                    <div class="v3BrandOther_barLeft_value v3BalanceOver" style="width: {{>BalancePercent}}"></div>
                </div>
            </div>
            {{else if Balance<0}}
            <div class="v3BrandOther_barTotal_holder v3BalanceUnder">
                {{>Balance}}
            </div>
            <div class="v3BrandOther_barColor_holder">
                <div class="v3BrandOther_barColor_right">
                    <div class="v3BrandOther_barRight_value v3BalanceUnder" style="width: {{>BalancePercent}}"></div>
                </div>
                <div class="v3BrandOther_barColor_left"></div>
            </div>
            {{else}}
            <div class="v3BrandOther_barTotal_holder v3BalanceEqual">
                {{>Balance}}
            </div>
            <div class="v3BrandOther_barColor_holder v3BalanceEqualBar">
                <div class="v3BrandOther_barColor_right"></div>
                <div class="v3BrandOther_barColor_left">
                </div>
            </div>
            {{/if}}
        </div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="font-size: 10px">{{>Supply}} days</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Age}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>StrTurnOver}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" id="divGrossPerUnitHeader">{{>StrGrossPerUnit}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Recon}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumLast"></div>
    </div>
    <div class="v3BrandOther_row v3BrandOther_rowTitle">
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumFirst" value="HeaderSegments" sortDirection="up">Segments</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderHistory" sortDirection="up">History</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderStock" sortDirection="up">Stock</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderGuide" sortDirection="up">Guide</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" value="HeaderBalance" sortDirection="up">
            <span>(under)</span><label>Balance</label><span>(over)</span>
        </div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderSupply" sortDirection="up" >Supply</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderAge" sortDirection="up">Age</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderTurn" sortDirection="up">Turn</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" value="HeaderGrossPerUnit" sortDirection="up">Gross Per Unit</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" value="HeaderRecon" sortDirection="up">Recon</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumLast">Wishlist</div>
    </div>
</script>
<script type="text/javascript">
    $.views.helpers({ getPhoto: vm.getPhoto, getVin: vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });
</script>
