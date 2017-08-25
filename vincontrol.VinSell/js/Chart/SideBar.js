var VINControl = VINControl || {};
VINControl.Chart = VINControl.Chart || {};
//var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange: { min: 0, max: 100 }, isSoldView: false };

VINControl.Chart.SideBar =
{
    updateSideBar: function(index, list, dealerCar) {
        $('#low').html("...");
        $('#middle').html("...");
        $('#high').html("...");
        var miles = $('#carInfo #miles');
        var price = $('#carInfo #price');
        var seller = $('#carInfo #seller');
        var address = $('#carInfo #address');
        var carThumb = $('#carInfo #car-thumb');
        var autotrader = $('#carInfo #AutoTraderLink');
        var carscom = $('#carInfo #CarsComLink');
        var carmax = $('#carInfo #CarmaxLink');

        
        var commercialtruck = $('#carInfo #CommercialTruckLink');
        var diffM = $('#carInfo #diffM');
        var diffP = $('#carInfo #diffP');
        //var daysOnMarket = $('#carInfo #daysOnMarket');
        var distance = $('#carInfo #distance');

        //var certified = $('#carInfo #certified-span');
        var car = $('#car');
        if (dealerCar != null && dealerCar.listingid == index) {
            miles.text(dealerCar.miles);
            price.text(dealerCar.price);
            diffM.text('');
            diffP.text('');
            seller.text(dealerCar.seller);
            address.text(dealerCar.address);

            $('#carInfo').show();
            $('#divNoCarInfo').hide();
            console.log('++++++++++++++');
            console.log(dealerCar);
            console.log('++++++++++++++');
            $('#sp_vin').html(dealerCar.vin);
            if (dealerCar.title.trim == 'Other') {
                car.html(dealerCar.title.year + ' ' + dealerCar.title.make + ' ' + dealerCar.title.model);
            } else {
                car.html(dealerCar.title.year + ' ' + dealerCar.title.make + ' ' + dealerCar.title.model + ' ' + dealerCar.title.trim);
            }

            if (dealerCar.thumbnail != '')
                carThumb.html('<img width="100" src="' + dealerCar.thumbnail + '" alt="car-thumbnail" />');
            else {
                carThumb.html('');
            }

            if (dealerCar.carscom && !dealerCar.commercialtruck) {
                if (typeof(isSoldView) !== 'undefined' && isSoldView) {
                    carscom.html("");
                } else {
                    carscom.html('<a target="_blank"  href="' + dealerCar.carscomlistingurl + '"><img src="/Content/images/carscom.png" height="18px"></a>');
                }
            } else {
                carscom.html("");
            }

            if (dealerCar.autotrader && !dealerCar.commercialtruck) {
                if (typeof(isSoldView) !== 'undefined' && isSoldView) {
                    autotrader.html("");
                } else {
                    autotrader.html('<a target="_blank"  href="' + dealerCar.autotraderlistingurl + '"><img src="/Content/images/autotrader.png" height="18px"></a>');
                }
            } else
                autotrader.html("");

            if (dealerCar.carmax && !dealerCar.commercialtruck) {
                if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                    carmax.html("");
                } else {
                    carmax.html('<a target="_blank"  href="' + dealerCar.carmaxlistingurl + '"><img src="/Content/images/carmax-logo.png" height="18px"></a>');
                }
            } else
                carmax.html("");

            if (dealerCar.commercialtruck) {

                if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                    commercialtruck.html("");
                } else {
                    commercialtruck.html('<a target="_blank"  href="' + dealerCar.commercialTruckListingURL + '"><img src="/Content/images/CommericalTruckLogo.png" height="18px"></a>');
                }
            } else
                commercialtruck.html("");

            distance.text('0 mi');

            try {
                if (!dealerCar.certified) {
                    certified.text('No');
                } else {
                    certified.text('Yes');
                }
            } catch (e) {

            }

            if (typeof ($selectedCar) !== 'undefined') {
                $selectedCar = dealerCar;
            }

        } else {
           
            for (var i = 0; i < list.length; i++) {
                if (list[i].listingid == index) {
                    miles.text(ChartHelper.addCommas(list[i].miles));
                    price.text(ChartHelper.addCommas(list[i].price));
                    diffM.text(ChartHelper.addCommas(ChartHelper.posNeg(dealerCar == null ? 0 : dealerCar.data[0][0] - list[i].miles)));
                    diffP.text(ChartHelper.addCommas(ChartHelper.posNeg(dealerCar == null ? 0 : dealerCar.data[0][1] - list[i].price)));
                    seller.text(list[i].seller);
                    address.text(list[i].address);

                    $('#lblAge').text(list[i].uptime);

                    $('#carInfo').show();
                    $('#divNoCarInfo').hide();

                    if (list[i].title.trim == 'Other') {
                        car.html(list[i].title.year + ' ' + list[i].title.make + ' ' + list[i].title.model);
                    } else {
                        car.html(list[i].title.year + ' ' + list[i].title.make + ' ' + list[i].title.model + ' ' + list[i].title.trim);
                    }

                    if (list[i].thumbnail != '')
                        carThumb.html('<img width="100" src="' + list[i].thumbnail + '" alt="car-thumbnail" />');

                    if (list[i].carscom && !list[i].commercialtruck) {
                        
                        if (typeof(isSoldView) !== 'undefined' && isSoldView) {
                            carscom.html("");
                        } else {
                            carscom.html('<a target="_blank"  href="' + list[i].carscomlistingurl + '"><img src="/Content/images/carscom.png" height="18px"></a>');
                        }
                    } else {
                        carscom.html("");
                    }
                    if (list[i].autotrader &&!list[i].commercialtruck) {
                        if (typeof(isSoldView) !== 'undefined' && isSoldView) {
                            autotrader.html("");
                        } else {
                            autotrader.html('<a target="_blank"  href="' + list[i].autotraderlistingurl + '"><img src="/Content/images/autotrader.png" height="18px"></a>');
                        }
                    } else
                        autotrader.html("");

                    if (list[i].carmax) {

                        if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                            carmax.html("");
                        } else {

                            carmax.html('<a target="_blank"  href="' + list[i].carmaxlistingurl + '"><img src="/Content/images/carmax-logo.png" height="25px"></a>');
                        }
                    } else
                        carmax.html("");

                    if (list[i].commercialtruck) {

                        if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                            commercialtruck.html("");
                        } else {

                            commercialtruck.html('<a target="_blank"  href="' + list[i].commercialTruckListingURL + '"><img src="/Content/images/CommericalTruckLogo.png" height="25px"></a>');
                        }
                    } else
                        commercialtruck.html("");

                    distance.text(ChartHelper.addCommas(list[i].distance) + ' miles');

                    if (typeof($selectedCar) !== 'undefined') {
                        $selectedCar = list[i];
                        console.log($selectedCar.vin);
                        $('#sp_vin').html($selectedCar.vin);
                    }
                    //console.log("draw selected car");
                    break;

                }
            }
        }
        //if (dealerCar != null) {
        //    // display express bucket jump values
        //    var url = "/PDF/ExpressBucketJumpWithKarPowerOptions?listingId={0}&dealer={1}&price={2}&year={3}&make={4}&model={5}&color=&miles={6}&plusPrice=0&certified=false&independentAdd=0&certifiedAdd=0&wholesaleWithoutOptions=0&wholesaleWithOptions=0&options=&chartCarType=0&Trims=0&isAll=True&isFranchise=False&isIndependant=False&inventoryType=3&ranges=100&selectedVin={7}".format(dealerCar.listingid, $selectedCar.seller, $selectedCar.price, $selectedCar.title.year, $selectedCar.title.make, $selectedCar.title.model, $selectedCar.miles, $selectedCar.vin);
        //    $.getJSON(url)
        //        .done(function(data) {
        //            $('#low').html(data.suggestedPrice);
        //            $('#middle').html(data.mileageAdjustment);
        //            $('#high').html(data.sellingPrice);
        //        })
        //        .fail(function(jqXhr, textStatus, err) {

        //        });
        //}
    },
    refreshSidebar: function(i, dcar, data) {
        var auto = i;
        ChartInfo.$filter = {};

     

        var m = new VINControl.Chart.Marketmetrics(data, dcar);
        // get sidebar elements and assign to variables
        var car = $('#car'),
        
            mileage = $('#miles'),
            price = $('#price'),
            diffM = $('#diffM'),
            diffP = $('#diffP'),
            distance = $('#distance'),
            // certified = $('#certified-span'),
            seller = $('#seller'),
            thumb = $('#car-thumb'),
            address = $('#address');
        var autotraderLink = $('#AutoTraderLink');
        var carscomLink = $('#CarsComLink');
        var carmaxLink = $('#CarmaxLink');
        var commercialtruck = $('#CommercialTruckLink');
        $('#carInfo').show();
        $('#divNoCarInfo').hide();

        if (auto.title.trim == 'Other') {
            car.html(auto.title.year + ' ' + auto.title.make + ' ' + auto.title.model);
        } else {
            car.html(auto.title.year + ' ' + auto.title.make + ' ' + auto.title.model + ' ' + auto.title.trim);
        }
        mileage.html(formatNumber(auto.miles));
        price.html(formatNumber(auto.price));
        thumb.html('<img width="100" src="' + auto.thumbnail + '" alt="car-thumbnail" />');

        
        if (auto.carscom && (dcar == null || !dcar.commercialtruck)) {

            if (typeof(isSoldView) !== 'undefined' && isSoldView) {
                carscomLink.html("");
            } else {
                carscomLink.html('<a target="_blank"  href="' + auto.carscomlistingurl + '"><img src="/Content/images/carscom.png" height="18px"></a>');
            }
        } else {
            carscomLink.html("");
        }

        if (auto.autotrader && (dcar == null || !dcar.commercialtruck)) {

            if (typeof(isSoldView) !== 'undefined' && isSoldView) {
                autotraderLink.html("");
            } else {
                autotraderLink.html('<a target="_blank"  href="' + auto.autotraderlistingurl + '"><img src="/Content/images/autotrader.png" height="18px"></a>');
            }
        } else
            autotraderLink.html("");

        if (auto.carmax && (dcar == null || !dcar.commercialtruck)) {

            if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                carmaxLink.html("");
            } else {
                carmaxLink.html('<a target="_blank"  href="' + auto.carmaxlistingurl + '"><img src="/Content/images/carmax-logo.png" height="18px"></a>');
            }
        } else
            carmaxLink.html("");
        
        if (dcar!=null && dcar.commercialtruck) {

            if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                commercialtruck.html("");
            } else {

                commercialtruck.html('<a target="_blank"  href="' + auto.commercialTruckListingURL + '"><img src="/Content/images/CommericalTruckLogo.png" height="25px"></a>');
            }
        } else
            commercialtruck.html("");

        distance.html(auto.distance + " miles");
        seller.html(auto.seller);
        address.html(auto.address);
        diffM.html(formatNumber(m.difference(auto.miles, dcar == null ? 0 : dcar.data[0][0])));
        diffP.html(formatNumber(m.difference(auto.price, dcar == null ? 0 : dcar.data[0][1])));
        
        $('#sp_vin').html(auto.vin);
        //certified.html(is_certified(auto.certified));
    },
    InitializeSidebar: function(dealerCar) {
        var miles = $('#carInfo #miles');
        var price = $('#carInfo #price');
        var seller = $('#carInfo #seller');
        var address = $('#carInfo #address');
        var carThumb = $('#carInfo #car-thumb');
        var autotrader = $('#carInfo #AutoTraderLink');
        var carscom = $('#carInfo #CarsComLink');
        var carmax = $('#carInfo #CarmaxLink');
        var diffM = $('#carInfo #diffM');
        var diffP = $('#carInfo #diffP');
        //var daysOnMarket = $('#carInfo #daysOnMarket');
        var distance = $('#carInfo #distance');
        //    var certified = $('#carInfo #certified-span');
        var car = $('#car');
        miles.text(dealerCar.miles);
        price.text(dealerCar.price);
        diffM.text('');
        diffP.text('');
        seller.text(dealerCar.seller);
        address.text(dealerCar.address);

        $('#carInfo').show();
        $('#divNoCarInfo').hide();

        if (dealerCar.title.trim == 'Other') {
            car.html(dealerCar.title.year + ' ' + dealerCar.title.make + ' ' + dealerCar.title.model);
        } else {
            car.html(dealerCar.title.year + ' ' + dealerCar.title.make + ' ' + dealerCar.title.model + ' ' + dealerCar.title.trim);
        }
        if (dealerCar.thumbnail != '')
            carThumb.html('<img width="100" src="' + dealerCar.thumbnail + '" alt="car-thumbnail" />');
        else {
            carThumb.html('');
        }
        if (dealerCar.carscom && !dealerCar.commercialtruck) {
            if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                carscom.html("");
            } else {
                carscom.html('<a target="_blank"  href="' + dealerCar.carscomlistingurl + '"><img src="/Content/images/carscom.png" height="18px"></a>');
            }
        } else {
            carscom.html("");
        }

        if (dealerCar.autotrader && !dealerCar.commercialtruck) {
            if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                autotrader.html("");
            } else {
                autotrader.html('<a target="_blank"  href="' + dealerCar.autotraderlistingurl + '"><img src="/Content/images/autotrader.png" height="18px"></a>');
            }
        } else
            autotrader.html("");

        if (dealerCar.carmax && !dealerCar.commercialtruck) {
            if (typeof (isSoldView) !== 'undefined' && isSoldView) {
                carmax.html("");
            } else {
                carmax.html('<a target="_blank"  href="' + dealerCar.carmaxlistingurl + '"><img src="/Content/images/carmax-logo.png" height="18px"></a>');
            }
        } else
            autotrader.html("");

        distance.text('0 mi');

        //    if (!dealerCar.certified) {
        //        certified.text('No');
        //    } else {
        //        certified.text('Yes');
        //    }
    }
};

