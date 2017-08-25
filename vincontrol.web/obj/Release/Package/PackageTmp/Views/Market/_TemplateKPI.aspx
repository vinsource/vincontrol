<script id="kpiTemplate" type="text/x-jsrender">
    {{if #index%2 == 0}}
    <div class="kpi_list_items kpi_list_items_odd {{>ClassFilter}}">
    {{else}}
    <div class="kpi_list_items {{>ClassFilter}}">
    {{/if}}
        <a href="{{>URLDetail}}">	
	        <div class="kpi_list_collum kpi_list_img">
		        <img src="{{>SinglePhoto}}" width="65px;" height="45px;">
	        </div>
	    </a>
        <div class="kpi_list_collum hover">
      
                 <a class="vin_viewprofile" href="{{>URLDetail}}">
                  {{if IsUsed && Type < 4}}
			<div class="age_text {{>~getColorStyle(MarketRange)}}">
				  {{>DaysInInvenotry}}
			</div>
          
            {{else}}
            <div class="age_text_border border_color">
             {{>DaysInInvenotry}}
             </div>
               {{/if}}
        </a>

		</div>
     <a class="vin_viewprofile" href="{{>URLEdit}}">
        <div class="kpi_list_collum kpi_list_marketData hover">
		     {{>~formatMarket(CarRanking,NumberOfCar)}}  
	    </div>
    </a>
        <a href="{{>URLEdit}}">
	        <div class="kpi_list_collum kpi_list_vin hover">
		         {{>~getVin(Vin)}}
	        </div>
	    </a>

        <a href="{{>URLEdit}}">
	        <div class="kpi_list_collum hover">
		         #{{>Stock}}
	        </div>
	    </a>

        <a href="{{>URLEdit}}">
	        <div class="kpi_list_collum hover">
		 	      {{>Year}}
	        </div>
	    </a>

        <a href="{{>URLEdit}}">
	        <div class="kpi_list_collum_medium hover" title="{{>Make}}">
		          {{>Make}}
	        </div>
	    </a>

        <a href="{{>URLEdit}}">
	        <div class="kpi_list_collum_long hover" title="{{>Model}}">
		         {{>Model}}
	        </div>
	    </a>

        <a href="{{>URLEdit}}">
	        <div class="kpi_list_collum_medium hover" title="{{>Trim}}">
	             {{>~getsubString(Trim,0,8)}}
	        </div>
	    </a>
     <a class="vin_viewprofile">
        <div class="kpi_list_collum_long" title="{{>ExteriorColor}}">
		         {{>~getsubString(ExteriorColor,0,7)}}
	    </div>
    </a>
	    <div class="kpi_list_collum kpi_list_miles">
		     {{>~formatPrice(Mileage)}}
	    </div>
	    <div class="kpi_list_collum kpi_list_price">
		      {{>~formatPrice(SalePrice)}}
	    </div>
        <a class="vin_viewprofile" href="{{>URLDetail}}">
          <div class="right_content_items hover" style="margin-top: 0px;">
              {{if IsUsed && Type < 4}}
           <img src="{{>~getImageUrl(MarketRange)}}">
            {{/if}}
        </div> 
        </a>
    <a class="vin_viewprofile">
        <div class="kpi_list_collum kpi_list_carfaxowner">
		     {{>CarFaxOwner}}
	    </div>
     </a>
    </div>


</script>

<script type="text/javascript">
    $.views.helpers({getVin:vm.getVin, getBGUrl: vm.getBGUrl, getWSUrl: vm.getWSUrl, getEbayUrl: vm.getEbayUrl, getsubString: vm.getsubString, getColorStyle: vm.getColorStyle, getImageUrl: vm.getImageUrl, formatCarfax: vm.formatCarfax, formatDaysInInventory: vm.formatDaysInInventory, formatStock: vm.formatStock, formatPrice: vm.formatPrice, formatMarket: vm.formatMarket });
</script>
