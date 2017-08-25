<script id="fullSavedBucketJumpTemplate" type="text/x-jsrender">
     {{if #index%2 == 0}}
    <div class="contain_list list_even" style="height: 65px;">
    {{else}}
    <div class="contain_list list_odd" style="height: 65px;">
    {{/if}} 
     <div class="right_content_items nav_first">
                {{if IsFeatured==true}}
                <input type="checkbox" title="Is featured car?" name="IsFeatured_{{>ListingId}}"
                   id="IsFeatured_{{>ListingId}}" value="true" checked="checked">
                 {{else}}
                <input type="checkbox" title="Is featured car?" name="IsFeatured_{{>ListingId}}"
                     id="IsFeatured_{{>ListingId}}" value="true">
                 {{/if}}
            </div>
            <a class="vin_editProfile invent_expanded" href="{{>URLDetail}}" style="display: inline;">
                <div class="right_content_items  invent_expanded invent_expanded_img" style="display: block;">
                  <img src="{{>~getPhoto(SinglePhoto)}}" /> 
                </div>
            </a>
            
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
            </a><a class="vin_editProfile" href="{{>URLEdit}}">
                <div class="right_content_items nav_long">
                    {{>Stock}}
                </div>
            </a><a class="vin_editProfile" href="{{>URLEdit}}">
                <div class="right_content_items nav_long">
                   {{>Year}}
                </div>
            </a><a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Make}}">
                <div class="right_content_items nav_long">
                    {{>Make}}
                </div>
            </a><a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Model}}">
                <div class="right_content_items nav_long">
                  {{>~getsubString(Model,0,10)}}
                </div>
            </a><a class="vin_editProfile" href="{{>URLEdit}}" title="{{>Trim}}">
                <div class="right_content_items nav_long">
                    {{>~getsubString(Trim,0,8)}}
                </div>
            </a>

           <div class="right_content_items nav_long" title="{{>ExteriorColor}}">
                 {{>~getsubString(ExteriorColor,0,6)}}
            </div>
    
            <form name="SaveSalePriceForm" class="SalePriceForm">
                <div class="right_content_items nav_long vehicle_miles divValue">
                 
                      {{>~formatPrice(Mileage)}}
                </div>
                <div class="right_content_items nav_long vehicle_price divValue">
                    <input type="text" id="Text4" name="price" class="sForm" maxlength="18" data-validation-engine="validate[funcCall[checkSalePrice]]" value="{{>~formatPrice(SalePrice)}}" />
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
            
        
          <%--  <div class="right_content_items right_content_items_owners_new nav_long">
                {{>CarFaxOwner}}
            </div>--%>
           <div class="right_content_items nav_long" style="font-size:12px">
                  {{if Certified }}
                  <h1>C</h1>
                  {{/if}}
                </div>
     
            <div class="inventory_action_btnsExpand_new" style="width:160px;">                
                {{if NotDoneBucketJump}}
                    <a id="doneTodayBucketJump_{{>ListingId}}_{{>DaysInInvenotry}}" name="doneTodayBucketJump_{{>ListingId}}_{{>DaysInInvenotry}}" title="Today Bucket Jump" style="margin-left:0px;" href="javascript:doneTodayBucketJump({{>ListingId}},{{>DaysInInvenotry}});">
                        <img src="../content/images/vincontrol/done.png" />
                    </a>
           
                 
                 {{else}}
                <a id="applyMassBucketJump_{{>ListingId}}_{{>DaysInInvenotry}}" name="applyMassBucketJump_{{>ListingId}}_{{>DaysInInvenotry}}" title="Apply Mass Bucket Jump" style="margin-left:-20px;" href="javascript:;">
                        <img src="../content/images/vincontrol/apply.png" />
                    </a>
                  {{/if}}
                
            </div>
            
             <div class="invent_expanded_action_holder invent_expanded" style="display: block;">                
                {{if !NotDoneBucketJump}}
                <label style="font-size:13px;display:inline-block;width:220px;font-style:italic;font-weight:bold;">Last Time : {{>DisplaySavedBucketJumpDate}}</label>
                {{/if}}
            </div>
            <div class="invent_expanded_location_holder invent_expanded" style="display: block;">                
               <label style="font-size:13px;display:inline-block;width:130px;font-style:italic;font-weight:bold;">{{>Location}}</label> 
            </div>
            <div class="invent_expanded_userstamp_holder invent_expanded" style="display: block;">     
      {{if UserStamp}}           
               <label style="font-size:13px;display:inline-block;width:130px;font-style:italic;font-weight:bold;">By {{>UserStamp}}</label> 
      {{/if}}
            </div>
            <div style="clear: both">
            </div>
       
    </div>




</script>

<script id="inventoryHeaderTemplate" type="text/x-jsrender">
    <div class="right_content_nav_items nav_first">
    </div>
    <div class="right_content_items  invent_expanded invent_expanded_img" style="display: block;">
    </div>
    <div class="right_content_nav_items nav_age_expended nav_middle" value="age">
        <%-- <%= Html.ActionLink("Age", "SortInventory", "Inventory", new { id = "Age" }, null)%>--%>
        <a href="javascript:void(0)">Age</a>
        <img class="imgSort" id="imgSortAge" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="market">
        <%--<%= Html.ActionLink("Market", "SortInventory", "Inventory", new { id = "Market" }, null)%>--%>
        <a href="javascript:void(0)">Rank</a>
        <img class="imgSort" id="imgSortMarket" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items right_content_items_vin nav_long" value="vin">
        <%--<%= Html.ActionLink("VIN", "SortInventory", "Inventory", new { id = "Vin" }, null)%>--%>
        <a href="javascript:void(0)">Vin</a>
        <img class="imgSort" id="imgSortVin" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="stock">
        <%--<%= Html.ActionLink("Stock", "SortInventory", "Inventory", new { id = "Stock" }, null)%>--%>
        <a href="javascript:void(0)">Stock</a>
        <img class="imgSort" id="imgSortStock" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="year">
        <%--<%= Html.ActionLink("Year", "SortInventory", "Inventory", new { id = "Year" }, null)%>--%>
        <a href="javascript:void(0)">Year</a>
        <img class="imgSort" id="imgSortYear" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="make">
        <%--<%= Html.ActionLink("Make", "SortInventory", "Inventory", new { id = "Make" }, null)%>--%>
        <a href="javascript:void(0)">Make</a>
        <img class="imgSort" id="imgSortMake" src="" width="12px" style="display: none;"/>
    </div>
    <div class="right_content_nav_items nav_long" value="model">
        <%-- <%= Html.ActionLink("Model", "SortInventory", "Inventory", new { id = "Model" }, null)%>--%>
        <a href="javascript:void(0)">Model</a>
        <img class="imgSort" id="imgSortModel" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="trim">
        <%-- <%= Html.ActionLink("Trim", "SortInventory", "Inventory", new { id = "Trim" }, null)%>--%>
        <a href="javascript:void(0)">Trim</a>
        <img class="imgSort" id="imgSortTrim" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long " value="color">
        <%-- <%= Html.ActionLink("Color", "SortInventory", "Inventory", new { id = "Color" }, null)%>--%>
        <a href="javascript:void(0)">Color</a>
        <img class="imgSort" id="imgSortColor" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="miles">
        <%--  <%= Html.ActionLink("Miles", "SortInventory", "Inventory", new { id = "Miles" }, null)%>--%>
        <a href="javascript:void(0)">Odometer</a>
        <img class="imgSort" id="imgSortMiles" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="price">
        <%-- <%= Html.ActionLink("Price", "SortInventory", "Inventory", new { id = "Price" }, null)%>--%>
        <a href="javascript:void(0)">Price</a>
        <img class="imgSort" id="imgSortPrice" src="" width="12px" style="display: none;" />
    </div>
    <div class="right_content_nav_items nav_long" value="certified">
        <%--  <%= Html.ActionLink("Owners", "SortInventory", "Inventory", new { id = "Owners" }, null)%>--%>
        <a href="javascript:void(0)">Certified</a>
        <%--<img class="imgSort" id="imgSortOwners" src="" width="12px" style="display: none;" />--%>
    </div>
   
    <div class="right_content_nav_filter_item" onclick="toggleFilterPanel();">
        Filter
        <span style="display: inline-block; float: none;" class="right_content_nav_filter_item_arrow_down"></span>
    </div>
</script>
<script type="text/javascript">
    $.views.helpers({ getPhoto: vm.getPhoto, getVin: vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });


</script>
