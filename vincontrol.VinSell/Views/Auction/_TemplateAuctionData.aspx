<script id="AuctionTemplate" type="text/x-jsrender">
       {{if ListItems.length > 0}}
         <div class="ptr_items_content_items">
                                           <div class="ptr_items_content_value_auction">
                                            </div>
                                          <div class="ptr_items_content_value_auctionmore">
                                             <div class="ptri_content_value_text_auction_number-right">
                                                Regional
                                            </div>
                                             <div class="ptri_content_value_text_auction_number-right">
                                                Vs
                                            </div>
                                               <div class="ptri_content_value_text_auction_number-right">
                                                National
                                            </div>
                                          </div>
                                          
                                        </div>
                                      <div class="ptr_items_content_items">
                                           <div class="ptr_items_content_value_auction">
                                                  # Vehicles
                                            </div>
                                          <div class="ptr_items_content_value_auctionmore">
                                                                     
                                                <div class="ptri_content_value_text_auction_number">
                                                    <a id="linknation" style="color: blue; text-decoration:underline" class="iframe iframeManHeim"  href="/Auction/ManheimTransaction?listingId= {{:ListItems[0].ListingId}}&vehicleStatus= {{:ListItems[0].VehicleStatus}}&auctionRegion=1">
                                                  {{:ListItems[0].NoOfVehicles}}
                                                        </a>
                                            </div>
                                                                         
                                             <div class="ptri_content_value_text_auction_number-right">
                                                 
                                            </div>
                                               <div class="ptri_content_value_text_auction_number-right">
                                                   <a id="linkregion" style="color: blue; text-decoration:underline" class="iframe iframeManHeim"  href="/Auction/ManheimTransaction?listingId={{:ListItems[1].ListingId}}&vehicleStatus= {{:ListItems[0].VehicleStatus}}&auctionRegion=2">
                                                  {{:ListItems[1].NoOfVehicles}}
                                                       </a>
                                            </div>
                                          </div>
                                          
                                        </div>
                            <div class="ptr_items_content_items">
                                           <div class="ptr_items_content_value_auction">
                                                  Avg. Ododmeter
                                            </div>
                                          <div class="ptr_items_content_value_auctionmore">
                                                                     
                                                <div class="ptri_content_value_text_auction_number" id="NationOdometer">
                                                        {{:ListItems[0].AvgeOdometer}}
                                                    
                                                </div>
                                                                         
                                             <div class="ptri_content_value_text_auction_number-right">
                                                 
                                            </div>
                                               <div class="ptri_content_value_text_auction_number-right" id="RegionOdometer">
                                                  {{:ListItems[1].AvgeOdometer}}
                                                </div>
                                          </div>
                                          
                                        </div>
                                <div class="ptr_items_content_items">
                                           <div class="ptr_items_content_value_auction">
                                                  Avg. Auction Price
                                            </div>
                                          <div class="ptr_items_content_value_auctionmore">
                                                                     
                                                <div class="ptri_content_value_text_auction_number" id="NationPrice">
                                                    
                                                  {{:ListItems[0].AvgAuctionPrice}}
                                                    
                                            </div>
                                                                         
                                             <div class="ptri_content_value_text_auction_number-right">
                                                 
                                            </div>
                                               <div class="ptri_content_value_text_auction_number-right" id="RegionPrice">
                                                  {{:ListItems[1].AvgAuctionPrice}}
                                            </div>
                                          </div>
                                          
                                        </div>
                                  
</script>
