$(document).ready(function() {
    console.log("before Load Data");
    LoadData();
    Events();
});

function LoadData() {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var dataUrl = '/StockingGuide/StockingGuideRecommendationSoldOut';
    $.ajax({
        type: "POST",
        dataType: "Html",
        url: dataUrl,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            $('#v3OtherListHolder').html(result);
            $.unblockUI();
        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
            $.unblockUI();
        }
    });
}

function Events() {
    $('div[id^=groupByMake_]').live('click', function() {
        var year = this.id.split('_')[1];
        var make = this.id.split('_')[2];
        var model = $('#ddlModel').val();
        if (model == '') {
            if ($('div[id^=groupByModel_' + year + '_' + make + ']').is(":hidden"))
                $('div[id^=groupByModel_' + year + '_' + make + ']').show();
            else {
                $('div[id^=groupByModel_' + year + '_' + make + ']').hide();
                $('div[id^=groupByTrim_' + year + '_' + make + ']').hide();
            }
        } else {
            if ($('div[id^=groupByModel_' + year + '_' + make + model + ']').is(":hidden"))
                $('div[id^=groupByModel_' + year + '_' + make + model + ']').show();
            else {
                $('div[id^=groupByModel_' + year + '_' + make + model + ']').hide();
                $('div[id^=groupByTrim_' + year + '_' + make + model + ']').hide();
            }
        }
        
    });

    $('div[id^=groupByModel_]').live('click', function () {
        var year = this.id.split('_')[1];
        var make = this.id.split('_')[2];
        var model = this.id.split('_')[3];
        if ($('div[id^=groupByTrim_' + year + '_' + make + '_' + model + ']').is(":hidden"))
            $('div[id^=groupByTrim_' + year + '_' + make + '_' + model + ']').show();
        else $('div[id^=groupByTrim_' + year + '_' + make + '_' + model + ']').hide();
    });

    $('#ddlMake').live('change', function () {
        var selectedMake = $(this).val();
        
        if (selectedMake == '') {
            $("#ddlModel option").each(function () {
                if ($(this).val() == '') {
                    $(this).attr('selected', true);
                }
                $(this).show();
            });
        } else
            $("#ddlModel option").each(function () {
                if ($(this).val() == '') {
                    $(this).attr('selected', true);
                } else if ($(this).val().split('_')[0] == selectedMake) $(this).show();
                else $(this).hide();
            });

        $(".main-group").each(function () {
            if (selectedMake == '') $(this).show();
            else {
                if ($.trim((this).id.split('_')[1]) == selectedMake)
                    $(this).show();
                else $(this).hide();
            }
        });
    });

    $('#ddlModel').live('change', function () {
        var selectedModel = $(this).val() == '' ? '' : $(this).val().split('_')[1];
        var selectedMake = $('#ddlMake').val();

        if (selectedModel != '')
            $("#ddlMake option").each(function() {
                if ($(this).val() == selectedMake) $(this).attr('selected', true);
            });

        $("div[id^=groupByModel_]").each(function () {
            if (selectedMake == '' && selectedModel == '') {
                $(this).parent().show();
                $(this).show();
            }
            else if (selectedModel == '') {
                if ($.trim((this).id.split('_')[2]) == selectedMake) {
                    $(this).show();
                    $(this).parent().show();
                } else {
                    $(this).hide();
                }
            }
            else if (selectedMake == '') {
                if ($.trim((this).id.split('_')[3]) == selectedModel) {
                    $(this).show();
                    $(this).parent().show();
                } else {
                    $(this).hide();
                }
            } else {
                if ($.trim((this).id.split('_')[2]) == selectedMake && $.trim((this).id.split('_')[3]) == selectedModel) {
                    $(this).show();
                    $(this).parent().show();
                } else {
                    $(this).hide();
                }
            }
        });
    });
}