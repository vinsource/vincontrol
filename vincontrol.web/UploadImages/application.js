/*
 * jQuery File Upload Plugin JS Example 5.0.2
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://creativecommons.org/licenses/MIT/
 */

/*jslint nomen: true */
/*global $ */

$(function() {
    'use strict';
    //    var arrayUpPic = new Array();

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({


});


$('#fileupload').bind('fileuploaddone', function(e, data) {
    if (data.jqXHR.responseText || data.result) {
        var fu = $('#fileupload').data('fileupload');
        var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
        fu._adjustMaxNumberOfFiles(JSONjQueryObject.files.length);

        fu._renderDownload(JSONjQueryObject.files)
                .appendTo($('#fileupload .files'))
                .hide(function() {
                    // Fix for IE7 and lower:
                    $(this).show();
                });

        var imageSrc = JSONjQueryObject.files[0].Image_url;
        
        var thubmnailImageSrc = JSONjQueryObject.files[0].Thumbnail_url;

        $("#uploaded").append("<div class=\"imageWrap\"><img width=\"100\" height=\"90\" src=\"" + imageSrc + "\"/><input class=\"check\" type=\"checkbox\" /></div>");

        var tmpNormalImage = $("#ImageUploadFiles").val() + "," + imageSrc;

        var tmpThumbnailImage = $("#ThumbnailImageUploadFiles").val() + "," + thubmnailImageSrc;

        $("#ImageUploadFiles").val(tmpNormalImage);

        $("#ThumbnailImageUploadFiles").val(tmpThumbnailImage);

    }


});

// Open download dialogs via iframes,
// to prevent aborting current uploads:
$('#fileupload .files a:not([target^=_blank])').live('click', function(e) {
    e.preventDefault();
    $('<iframe style="display:none;"></iframe>')
            .prop('src', this.href)
            .appendTo('body');
});

});