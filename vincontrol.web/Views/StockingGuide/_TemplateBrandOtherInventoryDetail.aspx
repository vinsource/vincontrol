<script id="brandOtherInventoryDetailTemplate" type="text/x-jsrender">
<div class="v3BrandOther_SD_row">
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
        <a class="iframe" href="{{>URLDetail}}">{{>Make}}
        </a>
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
        <a class="iframe" href="{{>URLDetail}}">{{>Model}}
        </a>
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall" id="divSubStock_{{>SGInventoryDealerSegmentDetailId}}">
        {{>InStock}}
    </div>
    {{if OU >0}}
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceOver" id="divSubBalance_{{>SGInventoryDealerSegmentDetailId}}">
        +{{>OU}}
    </div>
    {{else if OU <0}}
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceUnder" id="divSubBalance_{{>SGInventoryDealerSegmentDetailId}}">
        {{>OU}}
    </div>
    {{else}}
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" id="divSubBalance_{{>SGInventoryDealerSegmentDetailId}}">
        {{>OU}}
    </div>
    {{/if}}

    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
        <input type="text" class="txtSubGuide" id="txtSubGuide_{{>SGInventoryDealerSegmentDetailId}}" value="{{>Guide}}" /><input type="hidden" id="hdSubGuide_{{>SGInventoryDealerSegmentDetailId}}" value="{{>Guide}}" /><input type="hidden" id="hdParentID" value="{{>SGDealerSegmentId}}" />
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
        {{>History}}
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
        {{>Recon}}
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
        {{>StrTurnOver}}
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
        {{>Supply}}
    </div>
    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnLarge">
        <div class="v3CollumLast_text" id="divSubWishList_{{>SGInventoryDealerSegmentDetailId}}">
            {{if OU >0}}
                PREPRICE
            {{else if OU <0}}
                BUY
            {{else}}
                .................
            {{/if}}
        </div>
        <div class="v3CollumLast_cb">
            {{if IsWishList==true}}
                <input type="checkbox" id="chkInventory_{{>SGInventoryDealerSegmentDetailId}}" checked="checked" />
            {{else}}
                <input type="checkbox" id="chkInventory_{{>SGInventoryDealerSegmentDetailId}}" />
            {{/if}}
        </div>
    </div>
</div>
</script>
