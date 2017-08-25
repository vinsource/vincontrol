$(document).ready(function () {
    $("#aSeeSampleVideo").click(function() {
        if ($('#ifrSampleVideo').is(':visible')) {
            $("#aSeeSampleVideo").html("(Click to see sample video)");
            $('#ifrSampleVideo').hide();
            $('#ifrVideo').show();
        } else {
            $("#aSeeSampleVideo").html("(Click to see your video)");
            $('#ifrSampleVideo').show();
            $('#ifrVideo').hide();
        }
    });

    var today = new Date();
    // Cancel
    $("#divCancel").click(function () {
        window.parent.document.getElementById('Action').value = 'Cancel';
        parent.$.fancybox.close();
    });

    // Save
    $("#divSave").click(function () {
        {
            blockUIPopUp();
            $("#post-form").ajaxSubmit({
                url: postUrl,
                type: "POST",
                dataType: "text",
                cache: 'false',
                success: function (result) {
                    unblockUI();
                    if (result == "Error") {
                        ShowWarningMessage('An error occurred when processing your request.');
                        return false;
                    }

                    ShowWarningMessage('Your video will be uploaded to Youtube within 24 hours.');

                    setInterval(function() {
                        window.parent.document.getElementById('Action').value = 'Save';
                        parent.$.fancybox.close();
                    }, 3000);
                    
                },
                error: function (err) {
                    unblockUI();
                    ShowWarningMessage('An error occurred when processing your request.');
                }
            });
        }
    });

});
