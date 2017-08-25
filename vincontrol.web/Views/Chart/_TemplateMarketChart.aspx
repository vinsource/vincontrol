<script id="smallMarketChartTemplate" type="text/x-jsrender">
    <tr id="{{>listingid}}" name="tableRow" onclick="selectedId=this.id;javascript:plotSelectedPoint(this);">
        <td>{{:#index + 1}}</td>
        <td>{{>year}}</td>
        <td>{{>make}}</td>
        <td>{{>model}}</td>
        <td>{{>~ucfirst(trim)}}</td>
         <td>{{>bodyType}}</td>
        <td>{{>distance}}</td>
        <td>{{>seller}}</td>
        <td><b>{{>~addCommas(miles)}}</b></td>
        <td><b>${{>~addCommas(price)}}</b></td>
        <td>{{>uptime}}</td>
        <td>
            {{if certified == true}}
            C
            {{/if}}
        </td>
        <td>        
        {{>~getBooleanValue(carscom)}}        
        </td>
        <td>        
            {{>~getBooleanValue(autotrader)}}        
        </td>
        <td>        
            {{>~getBooleanValue(carmax)}}        
        </td>
        <td name="tdCommercialTruck">        
            {{>~getBooleanValue(commercialtruck)}}        
        </td>
        <td>
            {{if commercialtruck == false}}
            <a id="checkCarFax_{{>carfax}}" class="iframe" href="javascript:;" style="font-size:9px;">CARFAX</a>                        
            {{/if}}        
        </td>
        <td>
            {{if highlighted == true}}
            <img src="/Content/images/saved_icon.png" height="14px">
            {{/if}}
        </td>
    </tr>
</script>


<script id="marketChartTemplate" type="text/x-jsrender">    
    <tr id="{{>listingid}}" name="tableRow" onclick="selectedId=this.id;javascript:plotSelectedPoint(this);">    
        <td>{{:#index + 1}}</td>
        <td>{{>title.year}}</td>
        <td>{{>title.make}}</td>
        <td>{{>title.model}}</td>
        <td>{{>~ucfirst(trim)}}</td>
        <td>{{>bodyType}}</td>
        <td>{{>distance}}</td>
        <td>{{>seller}}</td>
        <td><b>{{>~addCommas(miles)}}</b></td>
        <td><b>${{>~addCommas(price)}}</b></td>
        <td>{{>uptime}}</td>
        <td>
            {{if certified == true}}
            C
            {{/if}}
        </td>
        <td>        
            {{>~getBooleanValue(carscom)}}        
        </td>
        <td>        
            {{>~getBooleanValue(autotrader)}}        
        </td>
        <td>        
            {{>~getBooleanValue(carmax)}}        
        </td>
        <td name="tdCommercialTruck">        
            {{>~getBooleanValue(commercialtruck)}}        
        </td>
        <td>
            {{if commercialtruck == false}}
            <a id="checkCarFax_{{>carfax}}" class="iframe" href="javascript:;" style="font-size:9px;">CARFAX</a>
            {{/if}}    
        </td>
        <td>
            {{if highlighted == true}}
            <img src="/Content/images/saved_icon.png" height="14px">
            {{/if}}
        </td>
    </tr>
</script>

<script id="marketChartHeaderTemplate" type="text/x-jsrender">
    <thead style="background-color: gray; color: #fff; height: 20px; cursor: pointer;">
        <tr>
            <th>#
            </th>
            <th align="center" class="header">Year
            </th>
            <th align="center" class="header">Make
            </th>
            <th align="left" class="header">Model
            </th>
            <th align="left" class="header">Trim
            </th>
             <th align="left" class="header">Body
            </th>
            <th align="left" class="">Dist
            </th>
            <th class="header">Seller
            </th>
            <th align="center" class="header">Miles
            </th>
            <th align="center" class="header">Price
            </th>
            <th align="center" class="header">Age
            </th>
            <th align="center" class="header">
            <img src="/Content/images/certified_icon_white.png" height="18px" title="Certified?">
            </th>
            <th class="header" id="thCarscom">
                <img src="/Content/images/carscom.png" height="18px">
            </th>
            <th class="header" id="thAutotrader">
                <img src="/Content/images/autotrader.png" height="18px">
            </th>
            <th class="header" id="thCarMax">
                <img src="/Content/images/carmax-logo.png" height="18px">
            </th>
            <th class="header" id="thCommercialTruck">
                <img src="/Content/images/CommericalTruckLogo.png" height="18px">
            </th>
            <th class="header"></th>
            <th class="header"></th>
        </tr>
    </thead>
</script>

<script type="text/javascript">

    var vm = {
        getBooleanValue: function(data) {
            return data ? "Yes" : "No";
        },
      
        addCommas: function (nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;

            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
                //x1 = x1.replace(rgx, '$1' + '.' + '$2');
            }

            return x1 + x2;
        }

    };
    $.views.helpers({ getBooleanValue: vm.getBooleanValue, ucfirst: ChartHelper.ucfirst, addCommas: vm.addCommas });

</script>
