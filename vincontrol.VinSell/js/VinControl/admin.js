var buyerGuideUrl = "/Admin/GetBuyerGuideList";
var rebateListUrl = "/Admin/GetRebateList";
var rebateListSortUrl = "/Admin/GetRebateListSort";
var buttompermissionListUrl = "/Admin/GetButtonPermissionList";
var applyrebateurl = "/Admin/ApplyRebate";

function todayDate(isFlash) {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }

    if (isFlash) {
        today = mm + '/' + dd + '/' + yyyy;
    } else {
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function setRebateDateRestriction() {
    $(".admin_rebate_create").live("keypress", function () {
        return false;
    });
    
    $(".admin_rebate_expiration").live("keypress", function () {
        return false;
    });

    $(".admin_rebate_expiration").datepicker();

    $(".admin_rebate_create").each(function (index) {
        var minDate = new Date();

        if ($(this).val() != "") {
            var curCreateDate = new Date($(this).val());

            if (curCreateDate < new Date(minDate)) {
                minDate = curCreateDate;
            }
        }

        $(this).datepicker({
            minDate: minDate,
            onClose: function (selectedDate) {
                var newMinDate = new Date(selectedDate);
                newMinDate.setDate(newMinDate.getDate() + 1);

                var elementExpire = $(this).next();

                if (elementExpire != null && elementExpire.hasClass("admin_rebate_expiration")) {
                    elementExpire.datepicker("option", "minDate", newMinDate);
                }
            }
        });

        $(this).live("keypress", function () {
            return false;
        });
    });

    $(".admin_rebate_expiration").each(function (index) {
        var elementCreate = $(this).prev();

        if (elementCreate != null && elementCreate.hasClass("admin_rebate_create")) {
            if (elementCreate.val() != "") {
                var newMinDate = new Date(elementCreate.val());
                newMinDate.setDate(newMinDate.getDate() + 1);
                $(this).datepicker("option", "minDate", newMinDate);
            }
            else {
                $(this).datepicker("option", "minDate", 1);
            }
        }

        $(this).live("keypress", function () {
            return false;
        });
    });
}


jQuery(function ($) {
    
    $("#NewPhone").mask("(999)999-9999", {
        completed: function () {
        }
    });
    $("#DealerSetting_EbayContactInfoPhone").mask("(999)999-9999", {
        completed: function () {
        }
    });

    setRebateDateRestriction();

    var txts = document.getElementsByTagName('TEXTAREA');

    for (var i = 0, l = txts.length; i < l; i++) {
        if (/^[0-9]+$/.test(txts[i].getAttribute("maxlength"))) {
            var func = function() {
                var len = parseInt(this.getAttribute("maxlength"), 10);

                if (this.value.length > len) {
                    alert('Maximum length exceeded: ' + len);
                    this.value = this.value.substr(0, len);
                    return false;
                }
            }

            txts[i].onkeyup = func;
            txts[i].onblur = func;
        }
    }
});


$('div.admin_top_btns').click(function () {
    $('div.admin_top_btns').each(function () {
        $(this).removeClass('admin_top_btns_active');
    });
    $(this).addClass('admin_top_btns_active');
    if ($(this).attr('id') == 'admin_content_tab') {
        $("#admin_content_holder").show();
        $("#admin_notifications_holder").hide();
        $("#admin_userrights_holder").hide();
        $("#admin_rebates_holder").hide();
        $("#admin_credentials_holder").hide();
        $("#admin_stockingguide_holder").hide();
        $("#party_content_holder").hide();
        $("#SaveChanges_holder").show();

        $("#currentTab").val("admin_content_tab");

    }
    if ($(this).attr('id') == 'admin_notifications_tab') {
        $("#admin_notifications_holder").show();
        $("#admin_content_holder").hide();
        $("#admin_userrights_holder").hide();
        $("#admin_rebates_holder").hide();
        $("#party_content_holder").hide();
        $("#admin_credentials_holder").hide();
        $("#admin_stockingguide_holder").hide();
        $("#SaveChanges_holder").show();

        $("#currentTab").val("admin_notifications_tab");

    }
    if ($(this).attr('id') == 'admin_userRights_tab') {
        $("#admin_userrights_holder").show();
        $("#admin_content_holder").hide();
        $("#admin_notifications_holder").hide();
        $("#admin_rebates_holder").hide();
        $("#party_content_holder").hide();

        $("#admin_credentials_holder").hide();
        $("#admin_stockingguide_holder").hide();
        $("#SaveChanges_holder").hide();

        $("#currentTab").val("admin_userRights_tab");


        $.ajax({
            type: "POST",
            contentType: "text/hmtl; charset=utf-8",
            dataType: "html",
            url: buttompermissionListUrl,
            data: {},
            cache: false,
            traditional: true,
            success: function (result) {
                $(".admin_ur_permissions_items").html(result);

            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
            }
        });

        getUpdatedUserList();
    }
    if ($(this).attr('id') == 'admin_rebate_tab') {
        $("#admin_rebates_holder").show();
        $("#admin_content_holder").hide();
        $("#admin_notifications_holder").hide();
        $("#admin_userrights_holder").hide();
        $("#party_content_holder").hide();
        $("#SaveChanges_holder").hide();
        $("#admin_credentials_holder").hide();
        $("#admin_stockingguide_holder").hide();
        $("#currentTab").val("admin_rebate_tab");

        $.ajax({
            type: "POST",
            contentType: "text/hmtl; charset=utf-8",
            dataType: "html",
            url: rebateListUrl,
            data: {},
            cache: false,
            traditional: true,
            success: function (result) {
                $(".rebate_list_holder").html(result);
                setRebateDateRestriction();
                $('.MBForm').validationEngine({ promptPosition: "centerRight", scroll: false });
            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
            }
        });
    }

    if ($(this).attr('id') == 'admin_credentials_tab') {
        $("#admin_credentials_holder").show();
        $("#admin_stockingguide_holder").hide();
        $("#admin_content_holder").hide();
        $("#admin_notifications_holder").hide();
        $("#admin_userrights_holder").hide();
        $("#admin_rebates_holder").hide();
        $("#party_content_holder").hide();
        $("#SaveChanges_holder").show();
        $("#currentTab").val("admin_credentials_tab");
    }

    if ($(this).attr('id') == 'admin_stockingguide_tab') {
        $("#admin_credentials_holder").hide();
        $("#admin_content_holder").hide();
        $("#admin_notifications_holder").hide();
        $("#admin_userrights_holder").hide();
        $("#admin_rebates_holder").hide();
        $("#party_content_holder").hide();

        $("#admin_stockingguide_holder").show();
        $("#SaveChanges_holder").show();
        $("#currentTab").val("admin_stockingguide_tab");
    }
});

$(document).ready(function () {


    $("#admin_content_tab").trigger("click");




    $.ajax({
        type: "POST",
        contentType: "text/hmtl; charset=utf-8",
        dataType: "html",
        url: buyerGuideUrl,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            $("#BuyerGuideContent").html(result);

        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });




    $("#btnAdd").click(function () {
        $.blockUI({ message: '<div><img src="/images/ajaxloadingindicator.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
        $.ajax({
            type: "POST",
            //url: "/TradeIn/AddComment?city=1&state=1&comment=1",
            url: "/TradeIn/AddComment",
            data: { city: $("#City").val(), state: $("#State").val(), comment: $("#Content").val(), name: $("#Name").val() },
            success: function (results) {
                $("#result").html(results);
                $.unblockUI();
            }
        });
    });

    $("#Variance").blur(function () {
        if ($("#Variance").val() == '') {
            $("#Variance").val(0);
        }
        if (isNaN($("#Variance").val())) {
            $("#lbVarianceValidate").show();

        }
        else {
            $("#lbVarianceValidate").hide();
        }
    });

    $("input[id^=btnSave]").live('click', function () {
        var idValue = this.id.split('_')[1];
        var cityValue = $("#txtCity_" + idValue).val();
        var stateValue = $("#txtState_" + idValue).val();
        var contentValue = $("#txtContent_" + idValue).val();
        var nameValue = $("#txtName_" + idValue).val();
        $.blockUI({ message: '<div><img src="/images/ajaxloadingindicator.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
        $.ajax({
            type: "POST",
            url: "/TradeIn/SaveComment",
            data: { city: cityValue, state: stateValue, comment: contentValue, id: idValue, name: nameValue },
            success: function (results) {
                $("#result").html(results);
                $.unblockUI();
            }
        });
    });

    $("input[id^=btnDelete]").live('click', function () {
        var id = this.id.split('_')[1];
        $.blockUI({ message: '<div><img src="/images/ajaxloadingindicator.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
        $.ajax({
            type: "POST",
            url: "/TradeIn/DeleteComment/" + id,
            data: {},
            success: function (results) {
                $("#result").html(results);
                $.unblockUI();
            }
        });
    });




    //  <!-- JAVASCRIPT FOR BUTTONS -->


    $("input[id^=pass]").live('blur', function () {
        var id = this.id.split('_')[1];
        var pass = this.value;
        if (ValidatePass(pass)) {
            $.post('/Admin/UpdatePassword', { pass: pass, userId: id }, function(data) {
                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }

    });

    $("input[id^=email]").live('blur', function () {
        var id = this.id.split('_')[1];
        var email = this.value;
        if (validateEmail(email)) {
            $.post('/Admin/UpdateEmail', { email: email, userId: id }, function(data) {
                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
                else if (data == "Email Existed") {
                    alert("The email " + email + " already existed");
                }
            });
        }
    });

    $("input[id^=phone]").live('blur', function () {
        var id = this.id.split('_')[1];
        var phone = this.value;
        if (validateCellphone(phone)) {
            $.post('/Admin/UpdateCellPhone', { cellPhone: phone, userId: id }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }

    });
    $("select[id^=role]").live('blur', function () {
        var id = this.id.split('_')[1];
        var phone = this.value;
        $.post('/Admin/ChangeRole', { role: phone, userId: id }, function (data) {
            if (data.SessionTimeOut == "TimeOut") {
                alert("Your session has timed out. Please login back again");
                var actionUrl = logOffURL;
                window.parent.location = actionUrl;
            }
        });

    });


    $("input[id^=editUser]").live('click', function () {
        $('#elementID').removeClass('hideLoader');
        var id = this.id.split('_')[1];

        //        var actionUrl = '<%= Url.Action("ChoooseDealerForUser", "Admin" ) %>';
        var actionUrl = '/Admin/ChoooseDealerForUser?userId=' + id;
        //        var actionUrl = '/Admin/EditUserForDefaultLogin?userId=' + id;

        $("<a class='iframe' href=" + actionUrl + "></a>").fancybox({
            overlayShow: true,
            showCloseButton: true,
            enableEscapeButton: true,
            width: 500,
            height: 330,
            padding: 0,
            margin: 0,
            onClosed: function () {
            }
        }).click();

    });


    $("input[id^=deleteUser]").live('click', function () {
        var answer = confirm("Are you sure you want to delete selected user?");
        if (answer) {
            $('#elementID').removeClass('hideLoader');
            var id = this.id.split('_')[1];
            var button = this;

            $.post('/Admin/DeleteUser', { userId: id }, function (data) {

                $(button).parent().closest('.admin_userrights_items').fadeOut(500);

                $("#Appraisal" + this.id).remove();
                $("#WholeSale" + this.id).remove();
                $("#Inventory" + this.id).remove();
                $("#24H" + this.id).remove();
                $("#Note" + this.id).remove();
                $("#Price" + this.id).remove();
                $('#elementID').addClass('hideLoader');
            });
        }
    });

    $('#EncodeCarFaxPassword').focus(function () {
        $("#hdnOldCarFax").val($(this).val());
    }).blur(function (defaultValue) {
        if ($(this).val() != $("#hdnOldCarFax").val()) {
            $("#CarFaxPasswordChanged").val('True');
        }
    });

    $('#EncodeManheimPassword').focus(function () {
        $("#hdnOldManheim").val($(this).val());
    }).blur(function (defaultValue) {
        if ($(this).val() != $("#hdnOldManheim").val()) {
            $("#ManheimPasswordChanged").val('True');
        }
    });

    $('#EncodeKellyPassword').focus(function () {
        $("#hdnOldKBB").val($(this).val());
    }).blur(function (defaultValue) {
        if ($(this).val() != $("#hdnOldKBB").val()) {
            $("#KellyPasswordChanged").val('True');
        }
    });

    $('#EncodeBlackBookPassword').focus(function () {
        $("#hdnOldBB").val($(this).val());
    }).blur(function (defaultValue) {
        if ($(this).val() != $("#hdnOldBB").val()) {
            $("#BlackBookPasswordChanged").val('True');
        }
    });
    
    $('#DealerCraigslistSetting_EncodePassword').focus(function () {
        $("#hdnOldCraigslist").val($(this).val());
    }).blur(function (defaultValue) {
        if ($(this).val() != $("#hdnOldCraigslist").val()) {
            $("#DealerCraigslistSetting_PasswordChanged").val('True');
        }
    });

    $(".admin_top_btns").click(function () {
        $(".admin_top_btns").removeClass("admin_top_btns_active");
        $(this).addClass("admin_top_btns_active");

        $(".admin_3rdparty_button").css("border", "1px solid black");
        $(".admin_3rdparty_holder").hide();
    });

    $("#admin_userRights_tab").click(function () {
        $(".admin_tab_holder").hide();
        $("#admin_userrights_holder").show();
    });

    $("#admin_content_tab").click(function () {
        $(".admin_tab_holder").hide();
        $("#admin_content_holder").show();
    });

    $("#admin_notifications_tab").click(function () {
        $(".admin_tab_holder").hide();
        $("#admin_notifications_holder").show();
    });

    $("#admin_rebate_tab").click(function () {
        $(".admin_tab_holder").hide();
        $("#admin_rebates_holder").show();
    });

    $(".admin_3rdparty_button").click(function () {
        $(".admin_top_btns").removeClass("admin_top_btns_active");
        $(this).css("border", "1px solid red");

        $(".admin_3rdparty_holder").show();
        $(".admin_tab_holder").hide();

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

    $('#admin_userrights_search_input').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) { //Enter keycode
            $("#admin_userrights_search_btn").click();
        }
    });

    $("#admin_userrights_search_btn").click(function () {
        $("#admin_userlist").html(getLoadingScreen());
        $.get("/Admin/GetUserRightList?Search=" + $("#admin_userrights_search_input").val(), function (data) {
            $("#admin_userlist").html(data);
            $('.PasswordForm').validationEngine({ promptPosition: "centerRight", scroll: false });
            $('.EmailForm').validationEngine({ promptPosition: "centerRight", scroll: false });
            $('.CellphoneForm').validationEngine({ promptPosition: "centerRight", scroll: false });
            $("input[id^=phone]").mask("(999)999-9999", {
                completed: function () {
                }
            });
        });
        }
    );

    contentManageEvents();
});

$("#btnAddManuRebate").live('click', function (e) {
    if (!$("#rebateamount").validationEngine('validate')) {

        if ($("#SelectedTrim").val() != null && $("#SelectedTrim").val() != "0****Trim...") {
            $.blockUI({ message: '<div><img src="/Content/images/ajaxloadingindicator.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            var values = $.map($('#SelectedTrim option'), function(e) { return e.value; });

            // as a comma separated string
            var trims = values.join(',');
            var transferObject =
            {
                YearId: $("#SelectedYear").val(),
                MakeId: $("#SelectedMake").val(),
                ModelId: $("#SelectedModel").val(),
                TrimId: $("#SelectedTrim").val(),
                trims: trims,
                RebateAmount: $("#rebateamount").val(),
                Disclaimer: $("#disclaimerrebate").val(),
                CreateDate: $("#txtCreate").val(),
                ExpirationDate: $("#txtExpiration").val()
            };
            if ($("#SelectedBodyType").val() != null && $("#SelectedBodyType").val() != "0****Body...") {
                transferObject.BodyType = $("#SelectedBodyType").val();
            }


            $.post(applyrebateurl, transferObject, function(data) {

                if (data == "Duplicate") {
                    alert("The reabte you just entered exists. Please edit the rebate in the list below.");
                } else if (data != "Error") {

                    $.ajax({
                        type: "POST",
                        contentType: "text/hmtl; charset=utf-8",
                        dataType: "html",
                        url: rebateListUrl,
                        data: {},
                        cache: false,
                        traditional: true,
                        success: function(result) {
                            $(".rebate_list_holder").html(result);
                            setRebateDateRestriction();
                        },
                        error: function(err) {
                            console.log(err.status + " - " + err.statusText);
                        }
                    });

                    $("#SelectedYear").val("Year");
                    $("#SelectedMake").html("");
                    $("#SelectedModel").html("");
                    $("#SelectedTrim").html("");
                    $("#SelectedBodyType").html("");
                    $("#rebateamount").val("");
                    $("#disclaimerrebate").val("");

                } else {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;

                }
            });
            $.unblockUI();
        } else {
            alert("Please select a trim");
        }
    }
});



   

function getUpdatedUserList() {
    $.get("/Admin/GetUserRightList", function (data) {
        $("#admin_userlist").html(data);
        $('.PasswordForm').validationEngine({ promptPosition: "centerRight", scroll: false });
        $('.EmailForm').validationEngine({ promptPosition: "centerRight", scroll: false });
        $('.CellphoneForm').validationEngine({ promptPosition: "centerRight", scroll: false });
        $("input[id^=phone]").mask("(999)999-9999", {
            completed: function () {
            }
        });
    });
}

function getLoadingScreen() {
    return "<div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>";
}




function contentManageEvents() {

    //$(".acd_right_btns_view").bind("mouseenter",function () {
    //    $(".content_view").hide();
    //    $(".content_edit").hide();
    //    $(this).parent().parent().find(".content_view").show();

    //}).bind("mouseleave",function () {
    //    $(".content_view").hide();
    //});
    
    $(".acd_right_btns_view").bind("click", function () {
        $(".content_view").hide();
        $(".content_edit").hide();
        $(this).parent().parent().find(".content_view").show();

    });

    $(".acd_right_btns_clear").live("click", function () {
        $(".content_edit").hide();
       
        var retVal = confirm("Do you want to clear the content ?");
        if (retVal == true) {
            var category = this.id.split('_')[1];
            var updateadminurl = '/Admin/ClearDescriptionSetting';
            $.post(updateadminurl, { descriptionCategory: category }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }

            });
            $(this).parent().parent().find(".content_view").find(".content_view_content").html("<p></p>");
        }
        
    });

    $(".acd_right_btns_edit").live("click", function () {
        $(".content_edit").hide();
        //	$("#opacity-layer").show();
        var html = $(this).parent().parent().find(".content_view").find(".content_view_content").children("p").html().trim();
        $(this).parent().parent().find(".content_edit").find("textarea").val(html);
        $(this).parent().parent().find(".content_edit").fadeIn();

        //$(".acd_right_btns_view").unbind("mouseenter");

        //$(".acd_right_btns_view").unbind("mouseleave");
    });
    
    $(".acd_right_btns_ending_edit").live("click", function () {
        $(".content_edit").hide();
        //	$("#opacity-layer").show();
        var htmlUsed = $("#endingUsed").children("p").html().trim();
        var htmlNew = $("#endingNew").children("p").html().trim();
        $("#DealerSetting_EndSentenceForNew").val(htmlNew);
        $("#DealerSetting_EndSentence").val(htmlUsed);
        $(this).parent().parent().find(".content_edit").fadeIn();

        //$(".acd_right_btns_view").unbind("mouseenter");

        //$(".acd_right_btns_view").unbind("mouseleave");
    });

    $(".content_btns_cancel").live("click", function () {
        $(".content_edit").fadeOut();
        $(".content_view").fadeOut();
        
        //$(".acd_right_btns_view").bind("mouseenter", function () {
        //    $(".content_view").hide();
        //    $(".content_edit").hide();
        //    $(this).parent().parent().find(".content_view").show();

        //}).bind("mouseleave", function () {
        //    $(".content_view").hide();
        //});
        
        $(".acd_right_btns_view").bind("click", function () {
            $(".content_view").hide();
            $(".content_edit").hide();
            $(this).parent().parent().find(".content_view").show();

        });
    });

    $(".content_btns_save").live("click", function () {
        var value = $(this).parent().parent().find("textarea").val();
        console.log(value);
        $(this).parent().parent().parent().find(".content_view_content").html("<p>" + value + "</p>");
        var category = this.id.split('_')[1];
        var updateadminurl = '/Admin/UpdateDescriptionSetting';
        $.post(updateadminurl, { description: value, descriptionCategory: category }, function (data) {
            if (data.SessionTimeOut == "TimeOut") {
                alert("Your session has timed out. Please login back again");
                var actionUrl = logOffURL;
                window.parent.location = actionUrl;
            }

        });
        $(".content_edit").fadeOut();
        $(".content_view").fadeOut();
        
        //$(".acd_right_btns_view").bind("mouseenter", function () {
        //    $(".content_view").hide();
        //    $(".content_edit").hide();
        //    $(this).parent().parent().find(".content_view").show();

        //}).bind("mouseleave", function () {
        //    $(".content_view").hide();
        //});
        $(".acd_right_btns_view").bind("click", function () {
            $(".content_view").hide();
            $(".content_edit").hide();
            $(this).parent().parent().find(".content_view").show();

        });
    });

    $(".content_btns_ending_save").live("click", function () {
        var descriptionUsed = $('#DealerSetting_EndSentence').val();
        var descriptionNew = $('#DealerSetting_EndSentenceForNew').val();
        $("#endingUsed").html("<p>" + descriptionUsed + "</p>");
        $("#endingNew").html("<p>" + descriptionNew + "</p>");
        var category = this.id.split('_')[1];
        var updateadminurl = '/Admin/UpdateEndingDescriptionSetting';
        $.post(updateadminurl, { descriptionUsed: descriptionUsed, descriptionNew: descriptionNew, descriptionCategory: category }, function (data) {
            if (data.SessionTimeOut == "TimeOut") {
                alert("Your session has timed out. Please login back again");
                var actionUrl = logOffURL;
                window.parent.location = actionUrl;
            }

        });
        $(".content_edit").fadeOut();
        $(".content_view").fadeOut();

        //$(".acd_right_btns_view").bind("mouseenter", function () {
        //    $(".content_view").hide();
        //    $(".content_edit").hide();
        //    $(this).parent().parent().find(".content_view").show();

        //}).bind("mouseleave", function () {
        //    $(".content_view").hide();
        //});
        $(".acd_right_btns_view").bind("click", function () {
            $(".content_view").hide();
            $(".content_edit").hide();
            $(this).parent().parent().find(".content_view").show();

        });
    });

}

function BGEvents() {
    $("#addNewBG").live("click",function () {
        $(".content_new_bg").fadeIn();
    });

    $(".bg_btns_cancel").live("click", function () {
        $(".content_new_bg").fadeOut();
    });

    $(".bg_btns_save").live("click", function () {
        $(".content_new_bg").fadeOut();
    });
}


function IsNumeric(num) {
    return (num >= 0 || num < 0);
}

function ShortRebate(type) {
    var isUp = $('#hdRebateIsUp').val();
    if (isUp == 'true') {
        isUp = false;
    } else {
        isUp = true;
    }
    $('#hdRebateIsUp').val(isUp);
    $.post(rebateListSortUrl, { type: type, isUp: isUp }, function (data) {
        $(".rebate_list_holder").html(data);
        setRebateDateRestriction();
    });
}

function checkMB(field, rules, i, options) {
    if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
        return "MB should <= $100,000,000";
    }
}