function AuctionMap(usMap) {
    var clickList = [], map = usMap, info = L.control(), geojson,
        init= function() {
            
        },
    //create auction list
                    createInformationDiv = function () {
                        // control that shows state info on hover
                        $("#fancybox-close.close-map").live('click', function () { L.DomUtil.addClass(info._div, 'hidedivinfo'); });
                        info.onAdd = function () {
                            this._div = L.DomUtil.create('div', 'info');
                            L.DomUtil.addClass(info._div, 'hidedivinfo');
                            this.update();
                            return this._div;
                        };

                        info.update = function (props) {
                            this._div.innerHTML = (props ?
                                '<div><a id="fancybox-close" class="close-map" style="display: inline;"></a></div><div class="auctionsummarycontainer"><div class="auctionsummarytitle">' + props.name + '</div><div class="auctionsummarynumber">' + props.density + ' Auctions</div></div>'
                                    + drawActionContent(props.regions, props.name)
                                : '');
                        };
                        info.addTo(map);
                    },
    //merging server data and map data    
                    convertData = function (JSONData) {
                        var auctionList = JSON.parse(JSONData);

                        $.each(statesData.features, function (i) {
                            //Move Hawaii
                            if (statesData.features[i].properties.name === "Hawaii") {

                                var data = statesData.features[i].geometry.coordinates;
                                for (var j = 0; j < data.length; j++) {
                                    //                                    console.log(data[j].length);
                                    for (var t = 0; t < data[j][0].length; t++) {
                                        data[j][0][t][0] = data[j][0][t][0] + 55;
                                    }
                                    ;
                                }
                            }
                            //Move and scale Alaska
                            else if (statesData.features[i].properties.name === "Alaska") {
                                data = statesData.features[i].geometry.coordinates;
                                for (j = 0; j < data.length; j++) {
                                    for (t = 0; t < data[j][0].length; t++) {
                                        data[j][0][t][1] = data[j][0][t][1] / 1.5 - 16;
                                        data[j][0][t][0] = data[j][0][t][0] / 3 - 65;
                                    }
                                    ;
                                }
                            }

                            //init data for states Data list
                            $.each(auctionList, function (index) {
                                if (auctionList[index].name === statesData.features[i].properties.name) {
                                    statesData.features[i].properties.density = auctionList[index].numberofregions;
                                    statesData.features[i].properties.regions = auctionList[index].regions;
                                    return false;
                                }
                            });
                        });
                    },
    // get color depending on population density value
                    getColor = function (d) {
                        return d > 100 ? '#800026' :
                            d > 50 ? '#BD0026' :
                                d > 20 ? '#E31A1C' :
                                    d > 10 ? '#FC4E2A' :
                                        d > 5 ? '#FD8D3C' :
                                            d > 2 ? '#FEB24C' :
                                                d > 1 ? '#FED976' :
                                                    '#FFEDA0';
                    },
                    resetHighlight = function (e) {
                        geojson.resetStyle(e);
                        info.update();
                    }, onEachFeature = function (feature, layer) {
                        layer.on({
                            click: highlightFeature
                        });
                        drawNumber(feature, layer);
                    },
                    drawNumber = function (feature, layer) {
                        var myIcon = L.divIcon({ html: feature.properties.density });
                        var marker = L.marker(feature.properties.center, { icon: myIcon });
                        marker.layer = layer;
                        marker.addTo(map).on({
                            click: fireLayerClick
                        });
                    },
                    fireLayerClick = function (e) {
                        e.target.layer.fire('click');
                    },
                    style = function (feature) {
                        return {
                            weight: 2,
                            opacity: 1,
                            color: 'white',
                            dashArray: '3',
                            fillOpacity: 1,
                            fillColor: getColor(feature.properties.density)
                        };
                    },
                    highlightFeature = function (e) {
                        L.DomUtil.removeClass(info._div, 'hidedivinfo');
                        for (var i = 0; i < clickList.length; i++) {
                            resetHighlight(clickList[i]);
                        }
                        clickList = [];
                        var layer = e.target;
                        layer.setStyle({
                            weight: 5,
                            color: '#666',
                            dashArray: '',
                            fillOpacity: 1
                        });

                        if (!L.Browser.ie && !L.Browser.opera) {
                            layer.bringToFront();
                        }

                        //                        $("#hdnRegionName").val();
                        $("#hdnState").val();

                        info.update(layer.feature.properties);
                        clickList.push(e.target);
                    },
                    createLegend = function () {
                        var legend = L.control({ position: 'bottomright' });

                        legend.onAdd = function (map) {

                            var div = L.DomUtil.create('div', 'info legend'),
                                grades = [0, 1, 2, 5, 10, 20, 50, 100],
                                labels = [],
                                from, to;

                            for (var i = 0; i < grades.length; i++) {
                                from = grades[i];
                                to = grades[i + 1];

                                labels.push(
                                    '<i style="background:' + getColor(from + 1) + '"></i> ' +
                                        from + (to ? '&ndash;' + to : '+'));
                            }

                            div.innerHTML = labels.join('<br>');
                            return div;
                        };

                        legend.addTo(map);
                    },
    // draw contains of auction vehicles summary pop up
                    drawActionContent = function (regions, stateName) {
                        var result = '';
                        if (regions) {
                            $.each(regions, function (i) {

                                result += '<div class="auctionsummarycontainerrow"><div class="auctionsummarytitlerow" ><a href="javascript:;" regionname="' + regions[i].name + '" statename="' + stateName + '" id="region_' + regions[i].code + '">'
                                    + regions[i].name + '</a></div><div class="auctionsummarynumberrow">(' + regions[i].numberofvehicles + ' vehicles)</div></div>';
                            });
                        }
                        return result;
                    },
                     bindMap = function () {
                         geojson = L.geoJson(statesData, {
                             style: style,
                             onEachFeature: onEachFeature
                         }).addTo(map);
                     };

    return {
        init: init,
        bindMap: bindMap,
        createInformationDiv: createInformationDiv,
        convertData: convertData,
        createLegend: createLegend
    };
}