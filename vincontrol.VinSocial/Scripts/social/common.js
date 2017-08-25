'use strict';

$(document).ready(function () {
    //set to base url of site
    // var baseUrl = 'http://vehicleinventorynetwork.com/socialDemo/'; // demo version

    var current_url = window.location.origin + window.location.pathname;
    var hash = window.location.hash.replace('#', '');

    if (hash) {
        $('div.sub-nav-btn.active').removeClass('active');
        $('div#' + hash + '-btn').addClass('active');
        $('.page-tab').addClass('hidden');
        $('.' + hash).removeClass('hidden');
    };

    $('div.sub-nav-btn').click(function () {
        var id = this.id.replace('-btn', '');
        window.location = current_url + '#' + id;
        var hash = window.location.hash.replace('#', '');
        $('div.sub-nav-btn.active').removeClass('active');
        $('div#' + hash + '-btn').addClass('active');
        $('.page-tab').addClass('hidden');
        $('.' + hash).removeClass('hidden');
    });



    $('div.preview-video-btn').click(function () {

        var id_base = $(this).attr('class').replace('-btn', '');
        $('.' + id_base + '-popup').removeClass('hidden');

    });

    $('.popup-cancel-btn').click(function () {
        $('.popup').addClass('hidden');
    });

});