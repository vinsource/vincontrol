<style type="text/css">
    .inventory_action_btns {
        /*float: right !important;
        margin-right: 10px !important;*/
        width: 160px !important;        
        margin-top: 9px;
        height: 25px;
        position: absolute;
        right: -10px;
    }
</style>
<script id="inventoryTemplate" type="text/x-jsrender">
    {{if #index%2 == 0}}
    <div class="contain_list list_odd {{>ClassFilter}}">
    {{else}}
    <div class="contain_list {{>ClassFilter}}">
    {{/if}}
        <div class="right_content_items nav_first">
            {{if IsFeatured==true}}
            <input type="checkbox" title="Is featured car?" name="IsFeatured_{{>ListingId}}" id="IsFeatured_{{>ListingId}}" value="true" checked="checked">
            {{else}}
            <input type="checkbox" title="Is featured car?" name="IsFeatured_{{>ListingId}}" id="IsFeatured_{{>ListingId}}" value="true">
            {{/if}}
          
        </div>
        
        <div class="right_content_items nav_middle">
      
                 <a class="vin_viewprofile" href="{{>URLDetail}}">
                  {{if IsUsed }}
			<div class="age_text {{>~getColorStyle(MarketRange)}}">
				  {{if DaysInInvenotry>0}}
                    {{>DaysInInvenotry}}
                {{else}}
                    N/A
                {{/if}}
			</div>
          
            {{else}}
            <div class="age_text_border border_color">
                {{if DaysInInvenotry>0}}
                    {{>DaysInInvenotry}}
                {{else}}
                    N/A
                {{/if}}
             </div>
               {{/if}}
        </a>

		</div>

        <a class="vin_editProfile" href="{{>URLEdit}}">
            <div class="right_content_items right_content_items_maket nav_long">
                {{>~formatMarket(CarRanking,NumberOfCar)}}
            
            </div>
        </a>
              <a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Vin}}">
                <div class="right_content_items right_content_items_vin nav_long">
                    {{>~getVin(Vin)}}
                </div>
            </a>
            
                <a class="vin_editProfile" href="{{>URLEdit}}">
                    <div class="right_content_items nav_long">
                        {{>Stock}}
                    </div>
                </a>
                
                   <a class="vin_editProfile" href="{{>URLEdit}}">
                        <div class="right_content_items nav_long">
                            {{>Year}}
                        </div>
                    </a>
                     <a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Make}}">
                            <div class="right_content_items nav_long">
                                {{>Make}}
                            </div>
                        </a>
    <a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Model}}">
                                <div class="right_content_items nav_long">
                                    {{>Model}}
                                </div>
                            </a>
   <a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Trim}}">
                                    <div class="right_content_items nav_long">
                                        {{>~getsubString(Trim,0,8)}}
                                    </div>
                                </a>
 
        <div class="right_content_items nav_long" title="{{>ExteriorColor}}">
            {{>~getsubString(ExteriorColor,0,7)}}
        </div>
        
        <form name="SaveSalePriceForm" class="SalePriceForm">
            <div class="right_content_items nav_long vehicle_miles divValue">
                
                {{if AllowEditProfile }}
                <input type="text" id="miles_{{>ListingId}}" name="odometer" class="sForm" maxlength="18" data-validation-engine="validate[funcCall[checkMiles]]" value="{{>~formatPrice(Mileage)}}" />
                {{else}}
                <input type="text" id="miles_{{>ListingId}}" name="odometer" class="sForm" disabled="disabled" maxlength="18" data-validation-engine="validate[funcCall[checkMiles]]" value="{{>~formatPrice(Mileage)}}" />
                {{/if}}
                <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="15px"/>
            </div>
            <div class="right_content_items nav_long vehicle_price divValue">
                <input type="text" id="saleprice_{{>ListingId}}" name="price" class="sForm" maxlength="18" data-validation-engine="validate[funcCall[checkSalePrice]]" value="{{>~formatPrice(SalePrice)}}" />
                <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="15px"/>
            </div>
        </form>
        <a class="vin_viewprofile" href="{{>URLDetail}}">
          <div class="right_content_items">
              {{if IsUsed && Type < 4}}
           <img src="{{>~getImageUrl(MarketRange)}}">
            {{/if}}
        </div> 
        </a>
           
        <div class="right_content_items right_content_items_owners_new nav_long">
            {{>CarFaxOwner}}
        </div> 
       
        <div class="right_content_items nav_long_dealer" title="{{>Location}}">
             {{>Location}}
        
        </div>
        <div class="invent_expanded_action_holder invent_expanded">
            <label class="invent_expanded_recon">
                <input type="checkbox" />
                Recon
            </label>
            <div class="invent_expanded_ebay">
                <a title="eBay" class="iframe" href="{{>~getEbayUrl(ListingId)}}"></a>
            </div>
            <div class="invent_expanded_bg">
                <a title="Buyer's Guide" class="iframe" href="{{>~getBGUrl(ListingId)}}">Buyer's Guide</a>
            </div>
            <div class="invent_expanded_ws">
                       <a title="Window Sticker" class="iframe" href="{{>~getWSUrl(ListingId)}}">Window Stickers</a>
            </div>
        </div>
        <div style="clear: both"></div>
    </div>


</script>

<script id="inventoryHeaderTemplate" type="text/x-jsrender">
     <div class="right_content_nav_items nav_first">
            </div>
            <div class="right_content_nav_items nav_age_expended nav_middle" style="margin-left: 0px;"
                value="age">
                <a href="javascript:void(0)">Age</a>
                <img class="imgSort" id="imgSortAge" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="market">
                <a href="javascript:void(0)">Rank</a>
                <img class="imgSort" id="imgSortMarket" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items right_content_items_vin nav_long" value="vin">
                <a href="javascript:void(0)">VIN</a>
                <img class="imgSort" id="imgSortVin" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="stock">
                <a href="javascript:void(0)">Stock</a>
                <img class="imgSort" id="imgSortStock" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="year">
                <a href="javascript:void(0)">Year</a>
                <img class="imgSort" id="imgSortYear" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="make">
                <a href="javascript:void(0)">Make</a>
                <img class="imgSort" id="imgSortMake" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="model">
                <a href="javascript:void(0)">Model</a>
                <img class="imgSort" id="imgSortModel" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="trim">
                <a href="javascript:void(0)">Trim</a>
                <img class="imgSort" id="imgSortTrim" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="color">
                <a href="javascript:void(0)">Color</a>
                <img class="imgSort" id="imgSortColor" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="miles">
                <a href="javascript:void(0)">Odometer</a>
                <img class="imgSort" id="imgSortMiles" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items nav_long" value="price">
                <a href="javascript:void(0)">Price</a>
                <img class="imgSort" id="imgSortPrice" src="" width="12px" style="display: none;" />
            </div>
            <div class="right_content_nav_items right_content_items_owners_new nav_long" value="owners">
                <a href="javascript:void(0)">Owners</a>
                <img class="imgSort" id="imgSortOwners" src="" width="12px" style="display: none;" />
            </div>
            <div id="switch-view" title="Switch to Large Cells">              
                <a onclick="javascript:grid.changeState();">
                    <img src='/Content/images/inventory_list.png' />
                </a>
            </div>
            <div class="right_content_nav_filter_item" onclick="toggleFilterPanel();">
                Filter
                <span style="display: inline-block; float: none;" class="right_content_nav_filter_item_arrow_down"></span>
            </div>
</script>
<script type="text/javascript">
    $.views.helpers({getVin:vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });
</script>
