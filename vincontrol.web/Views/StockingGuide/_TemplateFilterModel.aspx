<script id="filterModelsTemplate" type="text/x-jsrender">
    <optgroup label="{{>Make}}">
        {{for StockingGuideBrandData}}
            <option value="{{>SGDealerBrandId}}">{{>Model}}</option>
        {{/for}}
    </optgroup>
</script>