<script id="KBBTemplate" type="text/x-jsrender">
    {{if ListItems.length > 0}}
        <div id="divMultipleKbbTrim">
        <div class="karpowerTrim">
            <div class="ptr_items_content_header">
                <div class="ptr_items_textInfo">
                    {{if ListItems.length > 1}}
                        <select id="DDLKBB">
                            {{for ListItems}}
                                <option value="{{:Wholesale}}|{{:MileageAdjustment}}|{{:BaseWholesale}}|{{:AddsDeducts}}|{{:SelectedTrimId}}|{{:SelectedModelId}}">{{:SelectedTrimName}}</option>
                            {{/for}}
                        </select>
                    {{else}}
                        {{for ListItems}}
                            {{:SelectedTrimName}}
                            <a id="lnkDetail" style="color: black; text-underline: none" class="iframe iframeKBB">
                                <img src="../../Content/images/vincontrol/iconEye.png" height="15" />
                                </a>
                        {{/for}}
                    {{/if}}
                </div>
                {{if ListItems.length > 1}}
                <div style="padding-top: 3px; cursor: pointer" title="View detail">
                    <a id="lnkDetail" style="color: black; text-underline: none" class="iframe iframeKBB">
                        <img src="../../Content/images/vincontrol/iconEye.png" height="15" /></a>
                </div>
                {{/if}}
            </div>
               <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Base Price
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_below_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #0062A1" id="divBaseWholesale">
                        0
                    </div>
                </div>
            </div>
             <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Adds/Deducts
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_below_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #0062A1" id="divAddDeducts">
                        0
                    </div>
                </div>
            </div>
            <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Mileage Adj
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_equal_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #458C00" id="divMileageAdjustment">
                        0
                    </div>
                </div>
            </div>
             <div class="ptr_items_content_items">
                <div class="ptr_items_content_key">
                    Lending Value
                </div>
                <div class="ptr_items_content_value">
                    <div class="ptri_content_value_icons kpi_market_above_icon">
                    </div>
                    <div class="ptri_content_value_text" style="color: #D12C00" id="divWholeSale">
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
