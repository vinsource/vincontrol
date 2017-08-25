'use strict';

String.prototype.capitalize = function() {
    return this.replace(/(?:^|\s)\S/g, function(a) { return a.toUpperCase(); });
};

$(document).ready(function(){
  //set to base url of site
  var baseUrl = 'http://localhost/vinsocial/'; // local version
  // var baseUrl = 'http://vehicleinventorynetwork.com/socialDemo/'; // demo version
  
  var current_url = window.location.origin + window.location.pathname;
  var hash = window.location.hash.replace('#', '');

  if (hash) {
    $('div.sub-nav-btn.active').removeClass('active');
    $('div#' + hash + '-btn').addClass('active');
    $('.page-tab').addClass('hidden');
    $('.' + hash).removeClass('hidden');
  };

  $('div.sub-nav-btn').click(function() {
    var id = this.id.replace('-btn', '');
    window.location = current_url + '#' + id;
    var hash = window.location.hash.replace('#', '');
    $('div.sub-nav-btn.active').removeClass('active');
    $('div#' + hash + '-btn').addClass('active');
    $('.page-tab').addClass('hidden');
    $('.' + hash).removeClass('hidden');
    $('.popup').addClass('hidden');
  });



  $('div.preview-video-btn').click(function(){

    var id_base = $(this).attr('class').replace('-btn', '');
    $('.' + id_base + '-popup').removeClass('hidden');

  });

  $('.popup-cancel-btn').click(function(){
    $('.popup').addClass('hidden');
  });



  $('.reviews-list-container tr').click(function() {
    $('.review-popup').removeClass('hidden');
  });

  $('.surveys-list-container tr').click(function() {
    $('.survey-popup').removeClass('hidden');
  });

  $('.reviews-tab-btn').click(function() {
    $('.reviews-list-container').removeClass('hidden');
    $('.surveys-list-container').addClass('hidden');
    $('.surveys-tab-btn').removeClass('selected');
    $(this).addClass('selected');
  });

  $('.surveys-tab-btn').click(function() {
    $('.reviews-list-container').addClass('hidden');
    $('.surveys-list-container').removeClass('hidden');
    $('.reviews-tab-btn').removeClass('selected');
    $(this).addClass('selected');
  });

  $('div.show-client-info').click(function() {
    if ($('div.popup-client-info').hasClass('hidden')) {
      $(this).html('<img src="/Content/Images/social/triangle-down.png">');
      $('div.popup-client-info').removeClass('hidden');
    } else {
      $(this).html('<img src="/Content/Images/social/triangle.png">');
      $('div.popup-client-info').addClass('hidden');
    }
  });

  if ($('div.popup select[name="status-update-dropdown"]').prop('value') == 'resolved') {
    $('.status-update-form-wrap').addClass('hidden');
  }

  $('div.popup select[name="status-update-dropdown"]').change(function(el) {
    if (this.value == "resolved") {
      $('.status-update-form-wrap').addClass('hidden');
    } else {
      $('.status-update-form-wrap').removeClass('hidden');
    }
  });

  $('button.client-popup-save').click(function() {
    alert('Client Data Saved Successfully!');
    $('.popup').addClass('hidden');
  });



//  $('button[name="new-communication"]').click(function() {
//    $('.customer-information-popup').removeClass('hidden');
//  });

  $('div.post-new button').click(function() {
    $('.popup.facebook-post-popup').removeClass('hidden');
  });

  $('.fb-btn.cancel').click(function() {
    $('.popup').addClass('hidden');
  });

  $('.report-tab h3').click(function() {
    if ($(this).parent().hasClass('closed')) {
      $('.report-tab').addClass('closed');
      $(this).parent().removeClass('closed');
    } else {
      $('.report-tab').addClass('closed');
    }
  });


//  $('button[name="take-new-survey').click(function () {
//    $('.popup.take-survey-popup').removeClass('hidden');
//  });


});