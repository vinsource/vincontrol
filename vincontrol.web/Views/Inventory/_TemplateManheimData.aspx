<script id="ManHeimTemplate" type="text/x-jsrender">
    {{if ListItems.length > 0}}
        <div id="divMultipleKbbTrim">
        <div class="manheimTrim">
            <div class="ptr_items_content_header">
                <div class="ptr_items_textInfo">
                    {{if ListItems.length > 1}}
                        <select id="DDLManHeim">
                            {{for ListItems}}
                                <option value="{{:HighestPrice}}|{{:AveragePrice}}|{{:LowestPrice}}|{{:Year}}|{{:MakeServiceId}}|{{:ModelServiceId}}|{{:TrimServiceId}}">{{:TrimName}}</option>
                            {{/for}}
                        </select>
                    {{else}}
                        {{for ListItems}}
                    
                            {{:TrimName}}
                            <a id="lnkDetailManHeim" style="color: black; text-underline: none" class="iframe iframeManHeim">
                                 <img src="../../Content/images/vincontrol/iconEye.png" height="15" />
                                </a>
                    
                        {{/for}}
                    {{/if}}
                </div>
                {{if ListItems.length > 1}}
                <div style="padding-top: 3px; cursor: pointer" title="View detail">
                    <a id="lnkDetailManHeim" style="color: black; text-underline: none" class="iframe iframeManHeim">
                        <img src="../../Content/images/vincontrol/iconEye.png" height="15" /></a>
                </div>
                {{/if}}
            </div>
            <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Above
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_above_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #D12C00" id="divHighestPriceManHeim">
                        0
                    </div>
                </div>
            </div>
            <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Average
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_equal_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #458C00" id="divAveragePriceManHeim">
                        0
                    </div>
                </div>
            </div>
            <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Below
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_below_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #0062A1" id="divLowestPriceManHeim">
                        0
                    </div>
                </div>
            </div>
        </div>
    </div>
    {{else}}
        <div class="manheimTrim">
            <div class="ptr_items_content_header">
                <div class="ptr_items_textInfo">
                    No Data
                </div>
            </div>
        </div>
    {{/if}}
</script>
