<script id="brandOtherTemplate" type="text/x-jsrender">  
    <div class="SDRowsMarket">
        {{for #data tmpl='#brandOtherDetailTemplate' /}}
    </div>
</script>

<script id="brandHeaderOtherTemplate" type="text/x-jsrender">
    <div class="v3BrandOther_SD_row v3BrandOther_SD_rowTop">
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" value="marketMake" sortDirection="up" dealersegmentid="" detailid="">
            Make
        </div>
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" value="marketModel" sortDirection="up" dealersegmentid="" detailid="">
            Model
        </div>
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium" style="background-color: #3266CC" value="marketYourStock" sortDirection="up" dealersegmentid="" detailid="">
            Your Stock
        </div>
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium v3BalanceEqual" value="marketMarketStock" sortDirection="up" dealersegmentid="" detailid="">
            Market Stock
        </div>

        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketHistory" sortDirection="up" dealersegmentid="" detailid="">
            History(30d)
        </div>
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketSupply" sortDirection="up" dealersegmentid="" detailid="">
            Supply
        </div>
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketAge" sortDirection="up" dealersegmentid="" detailid="">
            Age
        </div>
        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall v3BalanceEqual" value="marketTurn" sortDirection="up" dealersegmentid="" detailid="">
            Turn
        </div>

        <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnLarge" value="marketWishList" sortDirection="up" dealersegmentid="" detailid="">
            Wishlist
        </div>
    </div>

</script>

<script type="text/javascript">
    $.views.helpers({ getPhoto: vm.getPhoto, getVin: vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });

</script>
