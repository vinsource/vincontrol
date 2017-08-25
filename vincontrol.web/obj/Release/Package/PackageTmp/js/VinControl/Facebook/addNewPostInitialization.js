$(document).ready(function () {
    var access_token = "";
    $.ajax({
        async: true,
        type: "POST",
        url: "/Facebook/GetAccessToken",
        cache: false,
        traditional: true,
        success: function (data) {
            access_token = data;
        }
    });

    $("#Content").focus(function () {
        $("#fb_location_search").hide();
    });

    $("#fb_location").click(function () {
        $("#fb_location_input").val($("#LocationName").val());
        $("#fb_location_search").show();
        $("#fb_location_input").focus();
    });

    $("#fb_file").click(function () {
        $("#fb_location_search").hide();
        $("#fb_file_input").click();
    });

    $("#fb_location_input").change(function () {
        var input = $("#fb_location_input").val();
        if (input == null || input == '') {
            $("#LocationId").val('');
            $("#LocationName").val('');
        }
    });

    $("#fb_location_input").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "https://graph.facebook.com/search",
                dataType: "json",
                data: {
                    type: "place",
                    access_token: access_token,
                    q: request.term
                },
                success: function (data) {
                    response($.map(data.data, function (item) {
                        return {
                            label: item.name,
                            value: item.id
                        }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            event.preventDefault();
            $("#fb_location_input").val(ui.item.label);
            $("#LocationId").val(ui.item.value);
            $("#LocationName").val(ui.item.label);
            
            if (ui.item.label == '') {
                $("#fb_location").removeClass("selected");
                $('#fb_location_name').html('');
            } else {
                $("#fb_location").addClass("selected");
                $('#fb_location_name').html('<span style="display:inline-block;color: #c3c3c3; margin-top: 5px; margin-left: 10px; float: left; margin-right: 5px;">' + ui.item.label + '</span>');
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
        },
        close: function (event, ui) {
            
        }
    });

    var today = new Date();
    $('#PublishDate').datepicker({
        changeMonth: true,
        numberOfMonths: 1,
        minDate: today
    });

    // Cancel
    $("#divCancel").click(function () {
        window.parent.document.getElementById('Action').value = 'Cancel';
        parent.$.fancybox.close();
    });

    // Save
    $("#divSave").click(function () {
        Validation();
        if (!readyToSubmit) {
            ShowWarningMessage(message);
            Reset();
            return false;
        } else {
            blockUIPopUp();
            //$("#PublishDateHidden").val($('#PublishDate').val() + " " + $("#Hour").val() + ":" + $("#Minute").val() + ":00" + " " + $("#HourType").val());
            $("#PublishDateHidden").val($('#PublishDate').val());
            $("#post-form").ajaxSubmit({
                url: "/facebook/addnewpost",
                type: "POST",
                dataType: "text",
                cache: 'false',
                success: function (result) {
                    unblockUI();
                    if (result == 'TimeConflict') {
                        ShowWarningMessage('You already had a post with this publish date.');
                        return false;
                    } else if (result == 'Error') {
                        ShowWarningMessage('You need to set Admin role for VINControl Facebook\'s account (alerts@vincontrol.com) on your page.');
                        return false;
                    }

                    window.parent.document.getElementById('Action').value = 'Save';
                    parent.$.fancybox.close();
                },
                error: function (err) {
                    unblockUI();
                    ShowWarningMessage('An error occurred when processing your request.');
                }
            });
        }
    });

    registerChangeEvent();
});

function Validation() {
    //var today = new Date(Date.now());
    //var publishDate = new Date($('#PublishDate').val().substring(0, 2) + "/" + $('#PublishDate').val().substring(3, 5) + "/" + $('#PublishDate').val().substring(6, 10) + (" " + $("#Hour").val() + ":" + $("#Minute").val() + ":00" + " " + $("#HourType").val()));
    //if (publishDate < today) {
    //    readyToSubmit = false;
    //    message += 'Publish date must be greater than today.<br/>';
    //}

    if ($('#Content').val() == '') {
        readyToSubmit = false;
        message += 'Content is required.<br/>';
    }
}

function Reset() {
    message = "";
    readyToSubmit = true;
}

function registerChangeEvent() {
    $('#fb_file_input').change(function () {
        
        var uploadFile = this.files[0];
        if (!(/\.(gif|jpg|jpeg|png|mpg|mpeg|mp4|mpeg4|wmv|3gp|avi|flv)$/i).test(uploadFile.name)) {
            ShowWarningMessage('You must select an media file only');
            $('#fb_file_input').replaceWith("<input id='fb_file_input' name='attachedFile' type='file' />");
            registerChangeEvent();
        } else if (uploadFile.size > 512000 && ((/\.(gif|jpg|jpeg|png)$/i).test(uploadFile.name))) { // 500kb
            ShowWarningMessage('Please upload a smaller image, max size is 500Kb');
            $('#fb_file_input').replaceWith("<input id='fb_file_input' name='attachedFile' type='file' />");
            registerChangeEvent();
        } else if (uploadFile.size > 4096000 && ((/\.(mpg|mpeg|mp4|mpeg4|wmv|3gp|avi|flv)$/i).test(uploadFile.name))) { // 4Mb
            ShowWarningMessage('Please upload a smaller video, max size is 4Mb');
            $('#fb_file_input').replaceWith("<input id='fb_file_input' name='attachedFile' type='file' />");
            registerChangeEvent();
        } else {
            $("#post-form").ajaxSubmit({
                url: "/facebook/addnewphoto",
                type: "POST",
                dataType: "text",
                cache: 'false',
                success: function (result) {
                    var fileName = result.replace(/^.*(\\|\/|\:)/, '');
                    //var fileExtension = fileName.split('.').pop().toLowerCase();
                    
                    $('#fb_file').addClass('selected');
                    if ((/\.(mpg|mpeg|mp4|mpeg4|wmv|3gp|avi|flv)$/i).test(fileName)) {
                        $('#Picture').val(result);
                        $('#fb_photo_name').html('<span class="fb_post_icon_2" id="fb_video" title="Video"></span>');
                    } else {
                        $('#Picture').val($('#Picture').val() + ',' + result);
                        $('#fb_photo_name').append('<img src="\\Content\\uploads\\' + fileName + '" style="height: 30px;">');
                    }
                },
                error: function (err) {
                    ShowWarningMessage('An error occurred when processing your request.');
                }
            });
        }
    });
};