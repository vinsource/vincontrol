var emailCheck = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
var specialChar = /[^@0-9a-zA-Z]/;
var phoneNumber = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;

var baseUrlTemp = "https://creditapp.vincontrol.com//credit-application/v2/";
$(document).ready(function () {

    // FINANCING LINK
    // Should open as popup with no controls but
    // address bar must be visible!
    $('#financing-btn').click(function () {
        var baseCode = $.base64.encode(dealerId);
        newPopup('https://creditapp.vincontrol.com/credit-application/v2/' + $('#vin').val() + '/' +  baseCode, 650, 650);
    });

    // TRADEIN LINK
    $('#value-trade-btn').click(function () {
        var dealerName = $('#dealerName').val();
        var vin = $('#vin').val();
        var url = '/trade/' + dealerName + '/step-one-decode';
        if (vin != undefined || vin != '') url = '/trade/' + dealerName + '/' + vin + '/step-one-decode-vin';
        window.location.href = url;
    });

    // CARFAX LINK
    $('.carfax-logo-and-button button').click(function () {

    });

    $('.bxslider').bxSlider({
        pager: false
    });

    $("#test-date").datepicker({
        minDate: 0
    });

    $("#back-top-btn").click(function () {
        $("html, body").animate({ scrollTop: $('#highlights').offset().top }, 1000);
    });

    $(".mask_phone").mask("999-999-9999");
    $("#phone_offer").focus(function () {
        if ($(this).attr("value") == "999-999-9999") {
            //$(this).attr("value","");
        }
    }).blur(function () {
        if ($(this).val() == "") {
            $(this).val("999-999-9999");
        }
    });

    $("#pdaPhone").focus(function () {
        if ($(this).attr("value") == "999-999-9999") {
            //$(this).attr("value","");
        }
    }).blur(function () {
        if ($(this).val() == "") {
            $(this).val("999-999-9999");
        }
    });

    $('a.iframe').fancybox({
        href: $(this).attr('src'),
        //'type': 'iframe',
        'width': 1000,
        'height': 900,
        'scrolling': 'yes',
        'hideOnOverlayClick': true,
        //'centerOnScroll': true,
        'onCleanup': function () {
        },
        onClosed: function () {

        }
    });

    $("a.make_offer").click(function () {
        $.fancybox({
            href: '#make-offer',
            //'type': 'iframe',
            'width': 1000,
            'height': 900,
            'scrolling': 'yes',
            'hideOnOverlayClick': true,
            //'centerOnScroll': true,
            'onCleanup': function () {
            },
            onClosed: function () {

            }
        });
    });

    $("a.test_drive").click(function () {
        $.fancybox({
            href: '#test_drive',
            //'type': 'iframe',
            'width': 1000,
            'height': 900,
            'scrolling': 'yes',
            'hideOnOverlayClick': true,
            //'centerOnScroll': true,
            'onCleanup': function () {
            },
            onClosed: function () {

            }
        });
    });

    $("a.more-info").click(function () {
        $.fancybox({
            href: '#more-info',
            //'type': 'iframe',
            'width': 1000,
            'height': 900,
            'scrolling': 'yes',
            'hideOnOverlayClick': true,
            //'centerOnScroll': true,
            'onCleanup': function () {
            },
            onClosed: function () {

            }
        });
    });

    $("#button-offer-info").click(function () {
        var checkOfferInfo = true;
        $("span.help-inline").remove();

        if ($("#fname_offer").val() == "" || $("#fname_offer").val() == "First Name") {
            $("#fname_offer").parent().children(".lb-info").css("color", "red");

            $("#fname_offer").attr("title", "This field is required.");

            if (checkOfferInfo) {
                checkOfferInfo = false;
                $("#fname_offer").focus();
            }
        } else {
            $("#fname_offer").parent().children(".lb-info").css("color", "#333");
        }

        if ($("#lname_offer").val() == "" || $("#lname_offer").val() == "Last Name") {
            $("#lname_offer").parent().children(".lb-info").css("color", "red");
            $("#lname_offer").attr("title", "This field is required.");

            if (checkOfferInfo) {
                checkOfferInfo = false;
                $("#lname_offer").focus();
            }
        } else {
            $("#lname_offer").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#email_offer").val() == "" || $("#email_offer").val() == "Email Address") {
            $("#email_offer").parent().children(".lb-info").css("color", "red");
            $("#email_offer").attr("title", "This field is required.");
            if (checkOfferInfo) {
                checkOfferInfo = false;
                $("#email_offer").focus();
            }
        } else {
            $("#email_offer").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#phone_offer").val() == "" || $("#phone_offer").val() == "999-999-9999") {
            $("#phone_offer").parent().children(".lb-info").css("color", "red");
            $("#phone_offer").attr("title", "This field is required.");
            if (checkOfferInfo) {
                checkOfferInfo = false;
                $("#phone_offer").focus();
            }
        } else {
            $("#phone_offer").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#offer_value").val() == "" || $("#offer_value").val() == "Offer Value") {
            $("#offer_value").parent().children(".lb-info").css("color", "red");
            $("#offer_value").attr("title", "This field is required.");
            if (checkOfferInfo) {
                checkOfferInfo = false;
                $("#offer_value").focus();
            }
        } else {
            $("#offer_value").parent().children(".lb-info").css("color", "#333");
        }

        if ($("#email_offer").val() != "" && $("#email_offer").val() != "Email Address") {
            if (validate($("#email_offer").val(), emailCheck)) {
                $("#email_offer").attr("title", "The email format is invalid.");
                $("#email_offer").parent().find("label").css("color", "red");
                if (checkOfferInfo) {
                    checkOfferInfo = false;
                    $("#email_offer").focus();
                }
            } else {
                $("#email_offer").attr("title");
                $("#email_offer").parent().find("label").css("color", "black");
            }

        }

        if (!checkOfferInfo) {
            return false;
        }

        if ($("#more-yes").attr('checked')) {
            contact_prefer = "Email";
        } else {
            contact_prefer = "Phone";
        }

        //type = 3 : Make offer
        if ($("#comment_offer").val() == "Additional Comments") {
            $("#comment_offer").val("");
        }
        insert_make_offer(dealerId, type = 3, $("#fname_offer").val(), $("#lname_offer").val(), contact_prefer, $("#email_offer").val(), $("#phone_offer").val(), $("#comment_offer").val(), vinNumber, $("#offer_value").val());
        $.fancybox.close();

    });

    $("#button-more-info").click(function () {
        var checkMoreInfo = true;
        $("span.help-inline").remove();

        if ($("#fname_more").val() == "" || $("#fname_more").val() == "First Name") {
            $("#fname_more").parent().children(".lb-info").css("color", "red");

            $("#fname_more").attr("title", "This field is required.");

            if (checkMoreInfo) {
                $("#fname_more").focus();
                checkMoreInfo = false;
            }
        } else {
            $("#fname_more").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#lname_more").val() == "" || $("#lname_more").val() == "Last Name") {
            $("#lname_more").parent().children(".lb-info").css("color", "red");

            $("#lname_more").attr("title", "This field is required.");

            if (checkMoreInfo) {
                $("#lname_more").focus();
                checkMoreInfo = false;
            }
        } else {
            $("#lname_more").parent().children(".lb-info").css("color", "#333");
        }

        if ($("#email_more").val() == "" || $("#email_more").val() == "Email Address") {
            $("#email_more").parent().children(".lb-info").css("color", "red");

            $("#email_more").attr("title", "This field is required.");

            if (checkMoreInfo) {
                $("#email_more").focus();
                checkMoreInfo = false;
            }
        } else {
            $("#email_more").parent().children(".lb-info").css("color", "#333");
        }

        if ($("#phone_more").val() == "" || $("#phone_more").val() == "999-999-9999") {
            $("#phone_more").parent().children(".lb-info").css("color", "red");

            $("#phone_more").attr("title", "This field is required.");

            if (checkMoreInfo) {
                $("#phone_more").focus();
                checkMoreInfo = false;
            }
        } else {
            $("#phone_more").parent().children(".lb-info").css("color", "#333");
        }

        if ($("#email_more").val() != "" && $("#email_more").val() != "Email Address") {
            if (validate($("#email_more").val(), emailCheck)) {
                $("#email_more").attr("title", "The email format is invalid.");
                $("#email_more").parent().find("label").css("color", "red");

                if (checkMoreInfo) {
                    $("#email_more").focus();
                    checkMoreInfo = false;
                }
            } else {
                $("#email_more").removeAttr("title");
                $("#email_more").parent().find("label").css("color", "black");
            }
        }




        if (!checkMoreInfo) {
            return false;
        }

        if ($("#more-email").attr('checked')) {
            contact_prefer = "Email";
        } else {
            contact_prefer = "Phone";
        }
        // type = 1 : request information
        if ($("#comment_more").val() == "Additional Comments") {
            $("#comment_more").val("");
        }
        insert_request_info(dealerId, type = 1, $("#fname_more").val(), $("#lname_more").val(), contact_prefer, $("#email_more").val(), $("#phone_more").val(), $("#comment_more").val(), vinNumber);
        $.fancybox.close();

    });

    $("#button-test-info").click(function () {
        var checkTestDrive = true;
        $("span.help-inline").remove();

        if ($("#fname_test").val() == "" || $("#fname_test").val() == "First Name") {
            $("#fname_test").parent().children(".lb-info").css("color", "red");

            $("#fname_test").attr("title", "This field is required.");

            if (checkTestDrive) {
                checkTestDrive = false;
                $("#fname_test").focus();
            }
        } else {
            $("#fname_test").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#lname_test").val() == "" || $("#lname_test").val() == "Last Name") {
            $("#lname_test").parent().children(".lb-info").css("color", "red");

            $("#lname_test").attr("title", "This field is required.");

            if (checkTestDrive) {
                checkTestDrive = false;
                $("#lname_test").focus();
            }
        } else {
            $("#lname_test").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#email_test").val() == "" || $("#email_test").val() == "Email Address") {
            $("#email_test").parent().children(".lb-info").css("color", "red");

            $("#email_test").attr("title", "This field is required.");

            if (checkTestDrive) {
                checkTestDrive = false;
                $("#email_test").focus();
            }
        } else {
            $("#email_test").parent().children(".lb-info").css("color", "#333");
        }
        if ($("#phone_test").val() == "" || $("#phone_test").val() == "999-999-9999") {
            $("#phone_test").parent().children(".lb-info").css("color", "red");

            $("#phone_test").attr("title", "This field is required.");

            if (checkTestDrive) {
                checkTestDrive = false;
                $("#phone_test").focus();
            }
        } else {
            $("#phone_test").parent().children(".lb-info").css("color", "#333");
        }

        if ($("#input-prepend-date").children('input[name="date"]').val() == "" || $("#input-prepend-date").children('input[name="date"]').val() == "Date (Ex: mm/dd/yyyy)") {
            $("#input-prepend-date").children(".lb-info").css("color", "red");

            $("#input-prepend-date").attr("title", "This field is required.");

            if (checkTestDrive) {
                checkTestDrive = false;
                $("#input-prepend-date").children('input[name="date"]').focus();
            }
        } else {
            $("#input-prepend-date").children(".lb-info").css("color", "#333");
        }
        //date for browser mm/dd/yyyy
        var date = $("#input-prepend-date").children('input[name="date"]').val();
        parsedate = date.split('/');
        if (parsedate[0] >= 13) {
            $("#input-prepend-date").append('<span class="help-inline">Month(mm) is over 12, following by this format(mm/dd/yyyy)</span>');
        } else if (parsedate[1] >= 32) {
            $("#input-prepend-date").append('<span class="help-inline">Day(dd) is over 31, following by this format(mm/dd/yyyy)</span>');
        } else {
            //date in database yyyy/mm/dd
            date = [parsedate[2], parsedate[0], parsedate[1]].join('/');
        }

        if ($("#email_test").val() != "" && $("#email_test").val() != "Email Address") {
            if (validate($("#email_test").val(), emailCheck)) {
                $("#email_test").attr("title", "The email format is invalid.");
                $("#email_test").parent().find("label").css("color", "red");
                if (checkTestDrive) {
                    checkTestDrive = false;
                    $("#email_test").focus();
                }
            } else {
                $("#email_test").removeAttr("title");
                $("#email_test").parent().find("label").css("color", "black");
            }
        }

        if (!checkTestDrive) {
            return false;
        }

        if ($("#contact_email_test").attr('checked')) {
            contact_prefer = "Email";
        } else {
            contact_prefer = "Phone";
        }

        if ($("#comment_test").val() == "Additional Comments") {
            $("#comment_test").val("");
        }

        //type = 2 : take a test drive
        insert_test_drive(dealerId, type = 2, $("#fname_test").val(), $("#lname_test").val(), contact_prefer, $("#email_test").val(), $("#phone_test").val(), $("#comment_test").val(), vinNumber, date, $("#time").val());
        $.fancybox.close();

    });

    $("#price-alert-btn").click(function () {
        var checkTestDrive = true;

        if ($('#pdaFirstName').val() == '') {
            checkTestDrive = false;
            $('#pdaFirstName').css("background-color", "#d12c00");
        }

        if ($('#pdaLastName').val() == '') {
            checkTestDrive = false;
            $('#pdaLastName').css("background-color", "#d12c00");
        }

        if ($('#pdaPhone').val() == '') {
            checkTestDrive = false;
            $('#pdaPhone').css("background-color", "#d12c00");
        }

        if ($('#pdaEmail').val() == '' || validate($("#pdaEmail").val(), emailCheck)) {
            checkTestDrive = false;
            $('#pdaEmail').css("background-color", "#d12c00");
        }

        if (!checkTestDrive) {
            return false;
        }

        insert_price_alert(dealerId, vehicleId, $('#pdaFirstName').val(), $('#pdaLastName').val(), $('#pdaPhone').val(), $('#pdaEmail').val(), 1);
    });
});

function insert_make_offer(dealerId, ctmr_type, ctmr_fname, ctmr_lname, ctmr_prefer, ctmr_email, ctmr_phone, ctmr_comment, vinnumber, ctmr_offer) {
    var wk_data = {
        "dealerId": dealerId,
        "contact_type": ctmr_type,
        "firstname": ctmr_fname,
        "lastname": ctmr_lname,
        "contact_prefer": ctmr_prefer,
        "email_address": ctmr_email,
        "phone_number": ctmr_phone,
        "comment": ctmr_comment,
        "vinnumber": vinnumber,
        "offer_value": ctmr_offer,
        "ModelYear": year,
        "Make": make,
        "Model": model,
        "Trim": trim,
        "StockNumber": stockNumber,
        "IsSolded": isSolded,
        "RegistDate": $.format.date(new Date(), "yyyy-MM-dd HH:mm:ss"),
        "DealerEmail": dealerEmail
    };

    $.ajax({
        type: "POST",
        url: insertCustomerInfo_api,
        data: wk_data,
        cache: false,
        success: function (data) {
            return true;
        },
        error: function (request, status, thrown) {
        }
    });
}

function insert_test_drive(dealerId, ctmr_type, ctmr_fname, ctmr_lname, ctmr_prefer, ctmr_email, ctmr_phone, ctmr_comment, vinnumber, ctmr_date, ctmr_time) {
    var wk_data = {
        "dealerId": dealerId,
        "contact_type": ctmr_type,
        "firstname": ctmr_fname,
        "lastname": ctmr_lname,
        "contact_prefer": ctmr_prefer,
        "email_address": ctmr_email,
        "phone_number": ctmr_phone,
        "comment": ctmr_comment,
        "vinnumber": vinnumber,
        "schedule_date": ctmr_date,
        "schedule_time": ctmr_time,
        "ModelYear": year,
        "Make": make,
        "Model": model,
        "Trim": trim,
        "StockNumber": stockNumber,
        "IsSolded": isSolded,
        "RegistDate": $.format.date(new Date(), "yyyy-MM-dd HH:mm:ss"),
        "DealerEmail": dealerEmail
    };

    $.ajax({
        type: "POST",
        url: insertCustomerInfo_api,
        data: wk_data,
        cache: false,
        success: function (data) {
            return true;
        },
        error: function (request, status, thrown) {
        }
    });
}

function insert_request_info(dealerId, ctmr_type, ctmr_fname, ctmr_lname, ctmr_prefer, ctmr_email, ctmr_phone, ctmr_comment, vinnumber) {

    var wk_data = {
        "dealerId": dealerId,
        "contact_type": ctmr_type,
        "firstname": ctmr_fname,
        "lastname": ctmr_lname,
        "contact_prefer": ctmr_prefer,
        "email_address": ctmr_email,
        "phone_number": ctmr_phone,
        "comment": ctmr_comment,
        "vinnumber": vinnumber,
        "ModelYear": year,
        "Make": make,
        "Model": model,
        "Trim": trim,
        "StockNumber": stockNumber,
        "IsSolded": isSolded,
        "RegistDate": $.format.date(new Date(), "yyyy-MM-dd HH:mm:ss"),
        "DealerEmail": dealerEmail
    };

    $.ajax({
        type: "POST",
        url: insertCustomerInfo_api,
        data: wk_data,
        // dataType : "json",
        cache: false,
        success: function (data) {
            return true;
        },
        error: function (request, status, thrown) {
        }
    });
}

function insert_price_alert(dealerId, vehicleId, ctmr_fname, ctmr_lname, ctmr_phone, ctmr_email, ctmr_type) {

    var wk_data = {
        "DealerId": dealerId,        
        "FirstName": ctmr_fname,
        "LastName": ctmr_lname,        
        "Email": ctmr_email,
        "Phone": ctmr_phone,        
        "VehicleId": vehicleId,
        "Type": ctmr_type        
    };

    $.ajax({
        type: "POST",
        url: insertPriceAlert_api,
        data: wk_data,
        // dataType : "json",
        cache: false,
        success: function (data) {
            resetPriceAlertBox();
            return true;
        },
        error: function (request, status, thrown) {
        }
    });
}

function validate(v, pattern) {
    return !pattern.test(v);
}

function resetPriceAlertBox() {
    $('#pdaFirstName').val('');
    $('#pdaFirstName').css("background-color", "#111517");

    $('#pdaLastName').val('');
    $('#pdaLastName').css("background-color", "#111517");

    $('#pdaPhone').val('');
    $('#pdaPhone').css("background-color", "#111517");

    $('#pdaEmail').val('');
    $('#pdaEmail').css("background-color", "#111517");
}