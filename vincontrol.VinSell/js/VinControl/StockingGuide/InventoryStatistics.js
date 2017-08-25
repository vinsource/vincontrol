//ko.extenders.numeric = function (target, precision) {
//    //create a writeable computed observable to intercept writes to our observable
//    var result = ko.computed({
//        read: target,  //always return the original observables value
//        write: function (newValue) {
//            var current = target(),
//                roundingMultiplier = Math.pow(10, precision),
//                newValueAsNum = isNaN(newValue) ? 0 : parseFloat(+newValue),
//                valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

//            //only write if it changed
//            if (valueToWrite !== current) {
//                target(valueToWrite);
//            } else {
//                //if the rounded value is the same, but a different value was written, force a notification for the current field
//                if (newValue !== current) {
//                    target.notifySubscribers(valueToWrite);
//                }
//            }
//        }
//    }).extend({ notify: 'always' });

//    //initialize with current value to make sure it is rounded appropriately
//    result(target());

//    //return the new computed observable
//    return result;
//};



function formatNumber(num) {
    var p = parseInt(num).toFixed(2).split(".");
    return p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num + (i && !(i % 3) ? "," : "") + acc;
    }, "").replace('-,', '-');
}

function formatDolar(num) {
    var p = parseInt(num).toFixed(2).split(".");
    return '$' + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num + (i && !(i % 3) ? "," : "") + acc;
    }, "").replace('-,', '-');
}

function formatPercent(num) {
    var p = parseInt(num).toFixed(2).split(".");
    return p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num + (i && !(i % 3) ? "," : "") + acc;
    }, "").replace('-,', '-') + '%';
}

function isDifferentNumber(a, b) {
    return Formatter.removeCommas(a) != Formatter.removeCommas(b);
}

var Formatter = (function () {
    var removeCommas = function (value) {
        console.log(value);
        if (typeof (value) != 'string') {
          

            return value;
        }
        
        return (value.replace(/,/g, ''));
    };
    return { removeCommas: removeCommas };
})();

var Caculator = (function () {
    var getDaysSupply = function (stock, history) {
        return Math.ceil(stock * 30 / history);
    };

    var getDollarCost = function (averageCost, totalSales) {
        return averageCost / 100 * totalSales;
    };

    var getAverageDollarCostPerUnit = function (dollarCost, units) {
        return Math.ceil(dollarCost / units);
    };

    var getAveragePricePerUnit = function (totalSales, units) {
        return Math.ceil(totalSales / units);
    };

    var getTotalProfitPerTurn = function (totalSales, grossProfitPerUnit) {
        return totalSales * grossProfitPerUnit / 100;
    };

    var getTurnRate = function (daysSupply) {
        return Math.floor(365 / daysSupply * 100) / 100;
    };

    var getMonthlyProfit = function (totalProfitPerTurn, turnRate) {
        return totalProfitPerTurn * turnRate / 12;
    };

    var getAnnualProfit = function (totalProfitPerTurn, turnRate) {
        return totalProfitPerTurn * turnRate;
    };

    var getAnnualROI = function (annualProfit, dollarCost) {
        return Math.ceil(((annualProfit - dollarCost) / dollarCost) * 100) + 100;
    };

    return {
        getDaysSupply: getDaysSupply,
        getDollarCost: getDollarCost,
        getAverageDollarCostPerUnit: getAverageDollarCostPerUnit,
        getAveragePricePerUnit: getAveragePricePerUnit,
        getTotalProfitPerTurn: getTotalProfitPerTurn,
        getTurnRate: getTurnRate,
        getMonthlyProfit: getMonthlyProfit,
        getAnnualProfit: getAnnualProfit,
        getAnnualROI: getAnnualROI
    };
})();

$(function () {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });

    $.getJSON("/StockingGuide/GetInventoryStatistics", function (data) {
        //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var currentInventoryStatusViewModel = new InventoryStatusViewModel(false);
        var desiredInventoryStatusViewModel = new InventoryStatusViewModel(true);
        ko.mapping.fromJS(data, {}, desiredInventoryStatusViewModel);
        ko.mapping.fromJS(data, {}, currentInventoryStatusViewModel);
        ko.applyBindings(new ViewModel(currentInventoryStatusViewModel, desiredInventoryStatusViewModel));

        jQuery("#inventoryStatisticForm").validationEngine();
        $("#txtUnits").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });
        $("#txtSalesPerMonth").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });
        $("#txtTotalSalesValue").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });
        $("#txtAverageCost").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });
        $("#txtGrossProfitPerUnit").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });
        //$("#txtDesiredTurnRate").numeric({ decimal: false, negative: false }, function () {
        //    alert("Positive integers only");
        //    this.value = "";
        //    this.focus();
        //});

        $("#txtUnits_opt").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });

        $("#txtSalesPerMonth_opt").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });

        $("#txtTotalSalesValue_opt").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });

        $("#txtAverageCost_opt").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });

        $("#txtGrossProfitPerUnit_opt").numeric({ decimal: false, negative: false }, function () {
            alert("Positive integers only");
            this.value = "";
            this.focus();
        });

        $("#txtUnits").val(formatNumber(Number($("#txtUnits").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtSalesPerMonth").val(formatNumber(Number($("#txtSalesPerMonth").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtTotalSalesValue").val(formatNumber(Number($("#txtTotalSalesValue").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtAverageCost").val(formatNumber(Number($("#txtAverageCost").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtGrossProfitPerUnit").val(formatNumber(Number($("#txtGrossProfitPerUnit").val().replace(/[^0-9\.]+/g, ""))));
        //$("#txtDesiredTurnRate").val(formatNumber(Number($("#txtDesiredTurnRate").val().replace(/[^0-9\.]+/g, ""))));

        $("#txtUnits_opt").val(formatNumber(Number($("#txtUnits_opt").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtSalesPerMonth_opt").val(formatNumber(Number($("#txtSalesPerMonth_opt").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtTotalSalesValue_opt").val(formatNumber(Number($("#txtTotalSalesValue_opt").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtAverageCost_opt").val(formatNumber(Number($("#txtAverageCost_opt").val().replace(/[^0-9\.]+/g, ""))));
        $("#txtGrossProfitPerUnit_opt").val(formatNumber(Number($("#txtGrossProfitPerUnit_opt").val().replace(/[^0-9\.]+/g, ""))));
        $.unblockUI();
    });


    //$("#txtUnits").val(formatNumber(Number($("#txtUnits").val().replace(/[^0-9\.]+/g, ""))));
    //$("#txtSalesPerMonth").val(formatNumber(Number($("#txtSalesPerMonth").val().replace(/[^0-9\.]+/g, ""))));
    //$("#txtTotalSalesValue").val(formatNumber(Number($("#txtTotalSalesValue").val().replace(/[^0-9\.]+/g, ""))));
    //$("#txtAverageCost").val(formatNumber(Number($("#txtAverageCost").val().replace(/[^0-9\.]+/g, ""))));
    //$("#txtGrossProfitPerUnit").val(formatNumber(Number($("#txtGrossProfitPerUnit").val().replace(/[^0-9\.]+/g, ""))));
    //$("#txtDesiredTurnRate").val(formatNumber(Number($("#txtDesiredTurnRate").val().replace(/[^0-9\.]+/g, ""))));
});




function InventoryStatusViewModel(isInputTurnRate) {
    this.ounits = ko.observable("");
    this.osalesPerMonth = ko.observable("");
    this.ototalSalesValue = ko.observable("");
    this.oaverageCost = ko.observable("");
    this.ogrossProfitPerUnit = ko.observable("");
    this.oturnRate = ko.observable("");
    this.isSavingUnits = ko.observable(false);
    this.isSavingSalesPerMonth = ko.observable(false);
    this.isSavingTotalSalesValue = ko.observable(false);
    this.isSavingAverageCost = ko.observable(false);
    this.isSavingGrossProfitPerUnit = ko.observable(false);
    
    this.funits = ko.observable("");
    this.fsalesPerMonth = ko.observable("");
    this.ftotalSalesValue = ko.observable("");
    this.faverageCost = ko.observable("");
    this.fgrossProfitPerUnit = ko.observable("");
    this.fturnRate = ko.observable("");
    //this.desiredTurnRate = ko.observable("");
    var self = this;
    this.units = ko.computed(function () {
        return Formatter.removeCommas(self.funits());
    });
    this.salesPerMonth = ko.computed(function () {
        return Formatter.removeCommas(self.fsalesPerMonth());
    });
    this.totalSalesValue = ko.computed(function () {
        return Formatter.removeCommas(self.ftotalSalesValue());
    });
    this.averageCost = ko.computed(function () {
        return Formatter.removeCommas(self.faverageCost());
    });
    this.grossProfitPerUnit = ko.computed(function () {
        return Formatter.removeCommas(self.fgrossProfitPerUnit());
    });


    this.daysSupply = ko.computed(function () { return Caculator.getDaysSupply(self.units(), self.salesPerMonth()); });
    this.dollarCost = ko.computed(function () { return Caculator.getDollarCost(self.averageCost(), self.totalSalesValue()); });
    this.averageDollarCostPerUnit = ko.computed(function () { return Caculator.getAverageDollarCostPerUnit(self.dollarCost(), self.units()); });
    this.averagePricePerUnit = ko.computed(function () { return Caculator.getAveragePricePerUnit(self.totalSalesValue(), self.units()); });
    this.totalProfitPerTurn = ko.computed(function () { return Caculator.getTotalProfitPerTurn(self.totalSalesValue(), self.grossProfitPerUnit()); });
    //this.turnRate = ko.computed(function () { return Caculator.getTurnRate(self.daysSupply()); });
    this.turnRate = ko.computed(function () {
        if (isInputTurnRate)
            return Formatter.removeCommas(self.fturnRate());
        else
            return Caculator.getTurnRate(self.daysSupply());
    });
    this.monthlyProfit = ko.computed(function () { return Caculator.getMonthlyProfit(self.totalProfitPerTurn(), self.turnRate()); });
    this.annualProfit = ko.computed(function () { return Caculator.getAnnualProfit(self.totalProfitPerTurn(), self.turnRate()); });
    this.annualROI = ko.computed(function () { return Caculator.getAnnualROI(self.annualProfit(), self.dollarCost()); });
    
        this.saveUnits = function(data, event) {
            self.save(data, event, self.ounits(), self.funits(), self.ounits, self.isSavingUnits);
        };
    
        this.saveSalesPerMonth = function (data, event) {
            self.save(data, event, self.osalesPerMonth(), self.fsalesPerMonth(), self.osalesPerMonth, self.isSavingSalesPerMonth);
        };
    
        this.saveTotalSalesValue = function (data, event) {
            console.log('saveTotalSalesValue');
            self.save(data, event, self.ototalSalesValue(), self.ftotalSalesValue(), self.ototalSalesValue, self.isSavingTotalSalesValue);
        };
    
        this.saveAverageCost = function (data, event) {
            self.save(data, event, self.oaverageCost(), self.faverageCost(), self.oaverageCost, self.isSavingAverageCost);
        };
    
        this.saveGrossProfitPerUnit = function (data, event) {
            self.save(data, event, self.ogrossProfitPerUnit(), self.fgrossProfitPerUnit(), self.ogrossProfitPerUnit, self.isSavingGrossProfitPerUnit);
        };
      

        this.saveAll = function (setSavingStatus) {
        $.post("/StockingGuide/SaveInventoryStatisticsValues", {
                units: self.units(),
                salesPerMonth: self.salesPerMonth(),
                totalSalesValue: self.totalSalesValue(),
                averageCost: self.averageCost(),
                grossProfitPerUnit: self.grossProfitPerUnit(),
                turnRate: self.turnRate()
            }, function(returnedData) {
                setSavingStatus(false);
                //alert("Your values has been saved successfully");
            });
    };

    this.save = function(data, event, oldValue, currentValue, setOldValue, setSavingStatus) {
        //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        if (!$("#txtUnits").validationEngine('validate') && !$("#txtSalesPerMonth").validationEngine('validate') && !$("#txtTotalSalesValue").validationEngine('validate') &&
            !$("#txtAverageCost").validationEngine('validate') && !$("#txtGrossProfitPerUnit").validationEngine('validate')) {
            //var target;

            //if (event.target) target = event.target;
            //else if (event.srcElement) target = event.srcElement;

            //// defeat Safari bug
            //if (target.nodeType == 3)
            //    target = target.parentNode;
           
            console.log(oldValue);
            console.log(currentValue);
            if (isDifferentNumber(oldValue, currentValue)) {
                setOldValue(currentValue);

                //var img = $(target.parentNode).find('.imgLoading');
                //img.show();
                setSavingStatus(true);
                self.saveAll(setSavingStatus);
            }
        }
    };

}






function ViewModel(currentInventoryStatusViewModel, desiredInventoryStatusViewModel) {
    var self = this;
    this.currentInventoryStatus = currentInventoryStatusViewModel;
    this.desiredInventoryStatus = desiredInventoryStatusViewModel;

    this.unitsDiffence = ko.computed(function () { return self.desiredInventoryStatus.units() - self.currentInventoryStatus.units(); });
    this.salesPerMonthDiffence = ko.computed(function () { return self.desiredInventoryStatus.salesPerMonth() - self.currentInventoryStatus.salesPerMonth(); });
    this.totalSalesValueDiffence = ko.computed(function () { return self.desiredInventoryStatus.totalSalesValue() - self.currentInventoryStatus.totalSalesValue(); });
    this.averageCostDiffence = ko.computed(function () { return self.desiredInventoryStatus.averageCost() - self.currentInventoryStatus.averageCost(); });
    this.grossProfitPerUnitDiffence = ko.computed(function () { return self.desiredInventoryStatus.grossProfitPerUnit() - self.currentInventoryStatus.grossProfitPerUnit(); });
    this.turnRateDiffence = ko.computed(function () { return self.desiredInventoryStatus.turnRate() - self.currentInventoryStatus.turnRate(); });
    //this.desiredTurnRateDiffence = ko.computed(function () { return self.currentInventoryStatus.de() - self.desiredInventoryStatus.units(); });

    this.daysSupplyDiffence = ko.computed(function () { return self.desiredInventoryStatus.daysSupply() - self.currentInventoryStatus.daysSupply(); });
    this.dollarCostDiffence = ko.computed(function () { return self.desiredInventoryStatus.dollarCost() - self.currentInventoryStatus.dollarCost(); });
    this.averageDollarCostPerUnitDiffence = ko.computed(function () { return self.desiredInventoryStatus.averageDollarCostPerUnit() - self.currentInventoryStatus.averageDollarCostPerUnit(); });
    this.averagePricePerUnitDiffence = ko.computed(function () { return self.desiredInventoryStatus.averagePricePerUnit() - self.currentInventoryStatus.averagePricePerUnit(); });
    this.totalProfitPerTurnDiffence = ko.computed(function () { return self.desiredInventoryStatus.totalProfitPerTurn() - self.currentInventoryStatus.totalProfitPerTurn(); });
    //this.turnRateDiffence = ko.computed(function () { return self.currentInventoryStatus.tu() - self.desiredInventoryStatus.units(); });

    this.monthlyProfitDiffence = ko.computed(function () { return self.desiredInventoryStatus.monthlyProfit() - self.currentInventoryStatus.monthlyProfit(); });
    this.annualProfitDiffence = ko.computed(function () { return self.desiredInventoryStatus.annualProfit() - self.currentInventoryStatus.annualProfit(); });
    this.annualROIDiffence = ko.computed(function () { return self.desiredInventoryStatus.annualROI() - self.currentInventoryStatus.annualROI(); });
};



