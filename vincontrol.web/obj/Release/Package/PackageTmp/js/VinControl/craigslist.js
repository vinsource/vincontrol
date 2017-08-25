var flag = true;
var imageWaitingHtml = '<img id="waitingImage" src="../../Content/Images/ajaxloadingindicator.gif" style="border: 0;position:absolute; top: 280px; left: 280px; display: none;"/>';

$(document).ready(function () {

    UploadImage(listingId);
    $('#btnContinue').live('click', function () {
        var dict = {
            "ListingId": $('#listingId').val(),
            "LocationUrl": $('#locationUrl').val(),
            "CryptedStepCheck": $('#cryptedStepCheck').val(),
            "PostingTitle": $('#postingTitle').val()
        };
        $.ajax({
            type: "POST",
            url: '/Craigslist/GoToPurchasingPage',
            data: dict,
            //contentType: "application/json; charset=utf-8",
            //dataType: "json",
            success: function (result) {
                $('#content').html(result);
                $('#prBars_phase3').removeClass("prBars_phase3_active");
                $('#prBars_phase4').addClass("prBars_phase4_active");
            },
            error: function (err) {
                
            }
        });
    });

    $('#btnPurchase').live('click', function () {
        Validation();
        if (flag) {
            $('#content').append(imageWaitingHtml);
            $('#waitingImage').show();
            var postData = {
                'CardNumber': $('CardNumber').val(),
                'VerificationNumber': $('VerificationNumber').val(),
                'ExpirationMonth': $('ExpirationMonth').val(),
                'ExpirationYear': $('ExpirationYear').val(),
                'FirstName': $('FirstName').val(),
                'LastName': $('LastName').val(),
                'Address': $('Address').val(),
                'City': $('City').val(),
                'State': $('State').val(),
                'Postal': $('Postal').val(),
                'ContactName': $('ContactName').val(),
                'ContactPhone': $('ContactPhone').val(),
                'ContactEmail': $('ContactEmail').val(),
                'LocationUrl': $('LocationUrl').val(),
                'CryptedStepCheck': $('CryptedStepCheck').val(),
                'ListingId': $('ListingId').val(),
            };
            $.ajax({
                type: "POST",
                url: '/Craigslist/Purchase',
                dataType: "html",
                data: $('#creditForm').serialize(),
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#waitingImage').hide();
                    if (result == 'Success') {
                        //parent.$.fancybox.close();
                        jAlert('Your ad has been posted successfully.', 'Error');
                    } else {
                        jAlert('Cannot process your request!', 'Error');
                    }
                },
                error: function (err) {
                    $('#waitingImage').hide();
                }
            });
        }
    });
});

function UploadImage(listingId) {
    $('#content').append(imageWaitingHtml);
    $('#waitingImage').show();

    //var uploadImage = window.setInterval(function () {
    $('#prBars_phase1').removeClass("prBars_phase1_active");
    $('#prBars_phase2').addClass("prBars_phase2_active");
    $.ajax({
        type: "GET",
        url: '/Craigslist/UploadImage?listingId=' + listingId,
        data: {},
        //dataType: "html",
        success: function (result) {
            $('#content').html(result);
            $('#prBars_phase2').removeClass("prBars_phase2_active");
            $('#prBars_phase3').addClass("prBars_phase3_active");
            
        },
        error: function (err) {
            
        }
    });
    //}, 5000);
}

function Validation() {
    flag = true;
    $('#CardNumber').removeClass('error');
    $('#VerificationNumber').removeClass('error');
    $('#ExpirationMonth').removeClass('error');
    $('#ExpirationYear').removeClass('error');
    $('#FirstName').removeClass('error');
    $('#LastName').removeClass('error');
    $('#Address').removeClass('error');
    $('#City').removeClass('error');
    $('#State').removeClass('error');
    $('#Postal').removeClass('error');
    $('#ContactName').removeClass('error');
    $('#ContactPhone').removeClass('error');
    $('#ContactEmail').removeClass('error');

    if ($('#CardNumber').val() == '') {
        $('#CardNumber').addClass('error');
        flag = false;
    }

    if ($('#VerificationNumber').val() == '') {
        $('#VerificationNumber').addClass('error');
        flag = false;
    }

    if ($('#ExpirationMonth').val() == 'Month') {
        $('#ExpirationMonth').addClass('error');
        flag = false;
    }

    if ($('#ExpirationYear').val() == 'Year') {
        $('#ExpirationYear').addClass('error');
        flag = false;
    }

    if ($('#FirstName').val() == '') {
        $('#FirstName').addClass('error');
        flag = false;
    }

    if ($('#LastName').val() == '') {
        $('#LastName').addClass('error');
        flag = false;
    }

    if ($('#Address').val() == '') {
        $('#Address').addClass('error');
        flag = false;
    }

    if ($('#City').val() == '') {
        $('#City').addClass('error');
        flag = false;
    }

    if ($('#State').val() == '') {
        $('#State').addClass('error');
        flag = false;
    }

    if ($('#Postal').val() == '') {
        $('#Postal').addClass('error');
        flag = false;
    }

    if ($('#ContactName').val() == '') {
        $('#ContactName').addClass('error');
        flag = false;
    }

    if ($('#ContactPhone').val() == '') {
        $('#ContactPhone').addClass('error');
        flag = false;
    }

    if ($('#ContactEmail').val() == '') {
        $('#ContactEmail').addClass('error');
        flag = false;
    }
}