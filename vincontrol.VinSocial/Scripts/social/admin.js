$(document).ready(function () {
    $(".admin_tab").click(function () {
        $(".admin_tab").removeClass("admin_tab_active");
        $(this).addClass("admin_tab_active");
    });
    adminEvents();
    $("#admin_tab_teams").trigger("click");
});

function adminEvents() {
    $("#admin_tab_teams").click(function () {
        $(".admin_holder").hide();
        $(".admin_teams_holder").show();
    });
    $("#admin_tab_notifications").click(function () {
        $(".admin_holder").hide();
        $(".admin_notifications_holder").show();
    });
    $("#admin_tab_3rd").click(function () {
        $(".admin_holder").hide();
        $(".admin_3rdparty_holder").show();
    });

    $("#admin_tab_users").click(function () {
        $(".admin_holder").hide();
        $(".admin_users_holder").show();
    });

    $("#admin_tab_videosetting").click(function () {
        $(".admin_holder").hide();
        $(".admin_videosettings_holder").show();
    });

    
    

    

    $(".team_add_member").click(function () {

        $(this).parent().children(".team_list_member_holder").slideDown("fast");
    });
    $(".team_listmember_cancel").click(function () {
        $(this).parent().parent().slideUp("fast");
    });

    $(".aun_user").click(function () {
        var hasSlideUp = $(this).hasClass("noti_manage_up");
        $(".admin_notifications_manage_holder").slideUp();
        $(".aun_user").removeClass("noti_manage_up");
        if (!hasSlideUp) {
            $(this).parent().find(".admin_notifications_manage_holder").slideDown();
            $(this).addClass("noti_manage_up");
        } else {
            $(this).parent().find(".admin_notifications_manage_holder").slideUp();
            $(this).removeClass("noti_manage_up");
        }

    });

    $(".admin_3rd_right_name").click(function () {
        var hasSlideUp = $(this).hasClass("rd3_up");
        $(".admin_3rd_right_input").slideUp();
        $(".admin_3rd_right_name").removeClass("rd3_up");
        if (!hasSlideUp) {
            $(this).parent().find(".admin_3rd_right_input").slideDown();
            $(this).addClass("rd3_up");
        } else {
            $(this).parent().find(".admin_3rd_right_input").slideUp();
            $(this).removeClass("rd3_up");
        }
    });

    $("#admin_add_user").fancybox({ 
        width: 300
    });

    $(".au_popup_cancel").click(function () {
        $(".fancybox-close").trigger("click");
    });
}