$(document).ready(function () {
    var currentStep = 1;
    $(".tradeInHeader_step1Btn").click(function () {
        $(".tradeIn_container").children("div").hide();
        $("#tradeIn_step1_holder").show();
        $(this).addClass("tradeInHeader_step1Btn_active");
        $(".tradeInHeader_step3Btn").removeClass("tradeInHeader_step3Btn_active");
        $(".tradeInHeader_step2Btn").removeClass("tradeInHeader_step2Btn_active");

        currentStep = 1;
    });

    $(".tradeInHeader_step2Btn").click(function () {
        $(".tradeIn_container").children("div").hide();
        $("#tradeIn_step2_holder").show();
        $(this).addClass("tradeInHeader_step2Btn_active");
        $(".tradeInHeader_step3Btn").removeClass("tradeInHeader_step3Btn_active");
        $(".tradeInHeader_step1Btn").removeClass("tradeInHeader_step1Btn_active");

        currentStep = 2;
    });

    $(".tradeInHeader_step3Btn").click(function () {
        $(".tradeIn_container").children("div").hide();
        $("#tradeIn_step3_holder").show();
        $(this).addClass("tradeInHeader_step3Btn_active");
        $(".tradeInHeader_step2Btn").removeClass("tradeInHeader_step2Btn_active");
        $(".tradeInHeader_step1Btn").removeClass("tradeInHeader_step1Btn_active");

        currentStep = 3;
    });

    $(".tradeIn_step_goto").click(function () {
        switch (currentStep) {
            case 1:
                $(".tradeInHeader_step2Btn").trigger("click");
                break;
            case 2:
                $(".tradeInHeader_step3Btn").trigger("click");
                break;
            case 3:
                break;

        }
    });

    $(".tradeInHeader_step1Btn").trigger("click");

    $(".tstep2_items_text").find("input").change(function () {
        if ($(this).attr("checked")) {
            $(this).parent().parent().parent().css("background-color", "#6685C2");
        } else {
            $(this).parent().parent().parent().css("background-color", "#EEEEEE");
        }
    });
});