<script id="brandOtherDetailTemplate" type="text/x-jsrender">
                <div class="v3BrandOther_SD_row">
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
                        <a class="iframe" href="{{>URLDetail}}">{{>Make}}
                        </a>
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
                        <a class="iframe" href="{{>URLDetail}}">{{>Model}}
                        </a>
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
                        <div class="v3BrandOtherSD_yourStock_left">
                            {{>YourStock}}
                        </div>
                        {{if BalanceYourStock >0}}
                        <div class="v3BrandOtherSD_yourStock_right v3BalanceOver">
                            +{{>BalanceYourStock}}
                        </div>
                        {{else if BalanceYourStock <0 }}
                        <div class="v3BrandOtherSD_yourStock_right v3BalanceUnder">
                            {{>BalanceYourStock}}
                        </div>
                        {{else}}
                        <div class="v3BrandOtherSD_yourStock_right v3BalanceEqualYourMarket">
                            ?
                        </div>
                        {{/if}}
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnMedium">
                        <div class="v3BrandOtherSD_marketStock_left">
                            {{>MarketStock}}
                        </div>
                        {{if BalanceMarketStock >0}}
                        <div class="v3BrandOtherSD_marketStock_right v3BalanceOver">
                            +{{>BalanceMarketStock}}
                        </div>
                        {{else if BalanceMarketStock <0}}
                        <div class="v3BrandOtherSD_marketStock_right v3BalanceUnder">
                            {{>BalanceMarketStock}}
                        </div>
                        {{else}}
                        <div class="v3BrandOtherSD_marketStock_right v3BalanceEqual">
                            B
                        </div>
                        {{/if}}
                        
                    </div>

                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        {{>History}}
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        {{>Supply}}
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        {{>Age}}
                    </div>
                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnSmall">
                        {{>StrTurnOver}}
                    </div>

                    <div class="v3BrandOtherSD_collumn v3BrandOtherSD_collumnLarge">
                        <div class="v3CollumLast_text">
                            {{if BalanceYourStock!=0}}
                                {{if BalanceMarketStock>0}}
                                    MARKET OVER, 
                                {{else if BalanceMarketStock<0}}
                                    MARKET UNDER, 
                                {{else}}
                                
                                {{/if}}
                            {{else}}
                                {{if BalanceMarketStock>0}}
                                    MARKET OVER
                                {{else if BalanceMarketStock<0}}
                                    MARKET UNDER
                                {{else}}
                                
                                {{/if}}
                            {{/if}}

                            {{if BalanceYourStock>0}}
                                YOU OVER
                            {{else if BalanceYourStock<0}}
                                YOU UNDER
                            {{else}}
                                
                            {{/if}}
                        </div>
                        <div class="v3CollumLast_cb">
                            {{if IsWishList==true}}
                                <input type="checkbox" id="chkMarket_{{>SGMarketDealerSegmentDetailId}}" checked="checked" />
                            {{else}}
                                <input type="checkbox" id="chkMarket_{{>SGMarketDealerSegmentDetailId}}" />
                            {{/if}}
                        </div>
                    </div>
                </div>
</script>
