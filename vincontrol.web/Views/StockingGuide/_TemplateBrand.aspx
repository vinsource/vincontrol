<script id="brandTemplate" type="text/x-jsrender">
     {{if History == 0 && Stock == 0 && Guide == 0 }}
     <div class="v3BrandOther_row v3BrandOther_rowZero">
     {{else}}
     <div class="v3BrandOther_row">
     {{/if}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumFirst">
            <a class="iframe" href="{{>URLDetail}}">
                {{>Model}}
            </a>
        </div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>History}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" id="divStock_{{>SGDealerBrandId}}">{{>Stock}}</div>
        {{if History == 0 && Stock == 0 && Guide == 0 }}
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Guide}}</div>
        {{else}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall"><input type="text" class="txtGuide" id="txtGuide_{{>SGDealerBrandId}}" value="{{>Guide}}" /><input type="hidden" id="hdGuide_{{>SGDealerBrandId}}" value="{{>Guide}}" /> </div>
        {{/if}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" id="divBalance_{{>SGDealerBrandId}}">
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
            <div class="v3BrandOther_collumn v3BrandOther_collumnMedium">$ <input type="text" disabled="disabled" class="txtGrossPerUnit txtGrossPerUnit_{{>Make}}" make={{>Make}} id="txtGrossPerUnit_{{>SGDealerBrandId}}" value="{{>GrossPerUnit}}" /><input type="hidden" id="hdGrossPerUnit_{{>SGDealerBrandId}}" value="{{>GrossPerUnit}}" /></div>
        {{else}}
            <div class="v3BrandOther_collumn v3BrandOther_collumnMedium">$ <input type="text" class="txtGrossPerUnit txtGrossPerUnit_{{>Make}}" make={{>Make}} id="txtGrossPerUnit_{{>SGDealerBrandId}}" value="{{>GrossPerUnit}}" /><input type="hidden" id="hdGrossPerUnit_{{>SGDealerBrandId}}" value="{{>GrossPerUnit}}" /></div>
        {{/if}}
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Recon}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumLast">
            <div class="v3CollumLast_text"></div>
            <div class="v3CollumLast_cb">
                {{if History == 0 && Stock == 0 && Guide == 0 }}
                    
                {{else}}
                    {{if IsWishList==true}}
                        <input type="checkbox" id="chkWishList_{{>SGDealerBrandId}}" checked="checked" />
                    {{else}}
                        <input type="checkbox" id="chkWishList_{{>SGDealerBrandId}}" />
                    {{/if}}
                {{/if}}
            </div>
        </div>
    </div>


</script>

<script id="brandHeaderTemplate" type="text/x-jsrender">
    <div class="v3BrandOther_row v3BrandOther_rowTop">
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumFirst">{{>Make}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>History}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Stock}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Guide}}</div>
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
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" id="divGrossPerUnitHeader_{{>Make}}">{{>StrGrossPerUnit}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall">{{>Recon}}</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumLast"></div>
    </div>
    <div class="v3BrandOther_row v3BrandOther_rowTitle" make="{{>Make}}">
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumFirst" style="cursor:pointer" value="HeaderModel" sortDirection="up">Models</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderHistory" sortDirection="up">History</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderStock" sortDirection="up">Stock</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderGuide" sortDirection="up">Guide</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" style="cursor:pointer" value="HeaderBalance" sortDirection="up">
            <span>(under)</span><label>Balance</label><span>(over)</span>
        </div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderSupply" sortDirection="up" >Supply</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderAge" sortDirection="up">Age</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderTurn" sortDirection="up">Turn</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnMedium" style="cursor:pointer" value="HeaderGrossPerUnit" sortDirection="up">Gross Per Unit</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnSmall" style="cursor:pointer" value="HeaderRecon" sortDirection="up">Recon</div>
        <div class="v3BrandOther_collumn v3BrandOther_collumnLarge collumLast" value="HeaderWishlist" sortDirection="up">Wishlist</div>
    </div>
</script>
<script type="text/javascript">
    $.views.helpers({ getPhoto: vm.getPhoto, getVin: vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });
</script>
