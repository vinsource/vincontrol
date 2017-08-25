function setFullHeight(el) {
    var height = $("html").css("height"); //document.height + "px";
    $("html").css("height");
    var width = document.width + "px";
    el.css("height", height);
    //el.css("width", width);
}

/* end */
/* center element */

var BrowserSize = {
    width: window.innerWidth || document.body.clientWidth,
    height: window.innerHeight || document.body.clientHeight
}

function centerElement(el) {
    el.addClass("isPopup");
    var windowWidth = $(window).width();
    var elWidth = el.css("width").replace("px", "");
    var left_right_size = windowWidth - parseInt(elWidth);
    if (left_right_size > 0) {
        left_right_size = left_right_size / 2;
        left_right_size = left_right_size + $(window).scrollLeft();
        el.css("left", left_right_size);
    } else {
        left_right_size = 0;
        if (el.css("position", "fixed")) {
            el.css("left", left_right_size);
        } else {
            el.css("margin-left", left_right_size);
        }
    }

    var elHeight = el.height();
    var top_bottom_size = BrowserSize.height - parseInt(elHeight);
    if (top_bottom_size < 0) {
        top_bottom_size = 0;
    } else {
        top_bottom_size = top_bottom_size / 2;
    }
    el.css("top", top_bottom_size);
    if (!el.is(":hidden")) {
        setFullHeight($("#opacity-layer"));
        $("#opacity-layer").show();
    }
}

/* end center */

$(document).ready(function () {
    $(window).resize(function () {
        BrowserSize = {
            width: window.innerWidth || document.body.clientWidth,
            height: window.innerHeight || document.body.clientHeight
        }
        var listPopup = $(".isPopup");
        $.each(listPopup, function (index, item) {
            centerElement($(item));
        });
    });

    $(".bucketjump_popup_close").click(function () {
        $("#opacity-layer").hide();
        $("#profile_bucketjump_holder").fadeOut();

    });

    $(".pricetracking_popup_close").click(function () {
        $("#opacity-layer").hide();
        $("#profile_pricetracking_holder").fadeOut();

    });

    $(".ws_popup_close").click(function () {
        $("#opacity-layer").hide();
        $("#profile_ws_holder").fadeOut();
        $(this).parent().fadeOut();

    });

    $(".language_popup_close").click(function () {

        //$("#opacity-layer").hide();
        if ($(".invent_bg_popup").is(":visible")) {

        } else {
            $("#opacity-layer").hide();
        }
        $("#profile_ws_holder").fadeOut();
        $(this).parent().fadeOut();

    });

    $(".ebay_prelisting_popup_close").click(function () {
        if ($(".invent_ebay_popup").is(":visible")) {

        } else {
            $("#opacity-layer").hide();
        }

        $(".ebay_preview_listing_holder").fadeOut();

    });

    $(".profile_ebay_preview_btns").click(function () {
        /*
        setFullHeight($("#opacity-layer"));
        $("#opacity-layer").show();
        $(".ebay_preview_listing_holder").show();
        centerElement($(".ebay_preview_listing_holder"));*/
    });
    $(".ebay_prelisting_tabs").click(function () {
        $(".ebay_prelisting_tabs").removeClass("ebay_prelisting_tabs_active");
        $(this).addClass("ebay_prelisting_tabs_active");
    });

    $(".expend_market").click(function () {
        setFullHeight($("#opacity-layer"));
        $("#opacity-layer").show();
        $(".market_expended_popup_holder").show();
        centerElement($(".market_expended_popup_holder"));
    });

    $(".expendmarket_popup_close").click(function () {
        $(this).parent().fadeOut();
        $("#opacity-layer").hide();
    });

    $(".marketexpend_popup_content").scroll(function () {
        var temp = $(".marketexpend_popup_content").scrollTop();
        $(".mkexpend_right_holder").css("position", "relative").css("top", temp);
    });

});

(function ($) {
    $.fn.extend({
        limiter: function (limit, elem) {
            $(this).keyup(function () {
                setCount(this, elem);
            });
            function setCount(src, elem) {
                var chars = src.value.length;
                if (chars > limit) {
                    src.value = src.value.substr(0, limit);
                    chars = limit;
                }
                elem.html(limit - chars);
            }

            setCount($(this)[0], elem);
        }
    });
})(jQuery); 