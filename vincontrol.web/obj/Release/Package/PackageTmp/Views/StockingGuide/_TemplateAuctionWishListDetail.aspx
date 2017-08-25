<script id="auctionWishListDetailTemplate" type="text/x-jsrender">
    {{if #index%2 == 0}}
    <div class="contain_list Used">
    {{else}}
    <div class="contain_list list_odd_detail Used">
    {{/if}}
        <div class="right_content_items_blue nav_Year">
            <a class="vin_viewprofile" href="{{>Url}}">
                {{>Year}}
            </a>
        </div>
        
        <div class="right_content_items_blue nav_Make">
            <a class="vin_viewprofile" href="{{>Url}}">
                {{>Make}}
            </a>
        </div>
        
        <div class="right_content_items_blue nav_Model">
            <a class="vin_editProfile" href="{{>Url}}">
                {{>Model}}
            </a>
        </div>
        
        <div class="right_content_items_blue nav_Trim">
            <a class="vin_editProfile" href="{{>Url}}" >
                {{>Trim}}
            </a>
        </div>       
        
        <a class="vin_editProfile" href="{{>Url}}">
            <div class="right_content_items nav_Seller">
                {{>Seller}}
                <br/>
                {{>SellerAddress}}
            </div>
        </a>
        <a class="vin_editProfile" href="{{>Url}}">
            <div class="right_content_items nav_Distance">
                {{>Distance}}
            </div>
        </a>
        <a class="vin_editProfile" href="{{>Url}}">
            <div class="right_content_items nav_Age">
                {{>Age}}
            </div>
        </a>
        <a class="vin_editProfile" href="{{>Url}}">
            <div class="right_content_items nav_Price">
                {{>Price}}
            </div>
        </a>
        <a class="vin_editProfile" href="{{>Url}}">
            <div class="right_content_items nav_Mileage">
                {{>Mileage}} mi
            </div>
        </a>
        <div style="clear: both"></div>
    </div>


</script>
<script type="text/javascript">
    $.views.helpers({ getPhoto: vm.getPhoto, getVin: vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });
</script>
