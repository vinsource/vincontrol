
function error(string, vHeight) {
	$('p.error').html(string);
	//console.log($('p.error'));
	$('.error-wrap').animate({ height: vHeight }, 250);
	$('.error').animate({opacity: 1}, 600);
}

$(document).ready(function () {

    var page = window.location.pathname;
    var container = $('.slide-wrapper');
    var header = $('#header');
    var content_boxes = [];
    var mask = $('.mask');
    var header_text = $('#header h1');
    var steps = $('.steps');

    // get each info box element and append to array
    $(".info-wrap").each(function () {
        content_boxes.push($(this));
    });

    // next button
    $("a.next").click(function (e) {
        e.preventDefault();
        var url = this.href;

        //		container.animate({right: 500, opacity: 0}, 500, function(){
        //			//location.href = url;
        //		});
    });

    // prev button
    $("a.prev").click(function (e) {
        e.preventDefault();
        var url = this.href;
        container.animate({ left: 500, opacity: 0 }, 500, function () {
            location.href = url;
        });
    });

    $('.step1 .logo').animate({ opacity: 1 }, 250);

    // container animation
    container.animate({ opacity: 1 }, 500);

    mask.animate({ width: '120%' }, 1000, function () {
        header_text.animate({ opacity: 1 }, 400);

        for (var i = 0; i < content_boxes.length; i++) {
            content_boxes[i].delay(i * 200).animate({ opacity: 1 }, 200);
        }
    });

    steps.animate({ opacity: 1 }, 200, function () {
        $('#step-1').animate({ opacity: 1 }, 200, function () {
            $(this).animate({ opacity: .8 }, 100, function () {
                $(this).animate({ opacity: 1 }, 100);
            });
            $('#step-2').animate({ opacity: 1 }, 200, function () {
                $(this).animate({ opacity: .8 }, 100, function () {
                    $(this).animate({ opacity: 1 }, 100);
                });
                $('#step-3').animate({ opacity: 1 }, 200, function () {
                    $(this).animate({ opacity: .8 }, 100, function () {
                        $(this).animate({ opacity: 1 }, 100);
                    });
                });
            })
        });
    });

    $('#condition img').click(function () {
        var id = this.parentNode.id;
        var radio = $('.' + id);
        radio.prop('checked', true);
        $('#Condition').val(this.parentNode.id);
        $('#condition img').each(function () {
            if (this.parentNode.id === id) {
                if ($(this).attr('src') != '../images/on-' + id + '.jpg') {
                    $(this).attr('src', '../images/on-' + id + '.jpg');
                }
            } else {
                $(this).attr('src', '../images/' + this.parentNode.id + '.jpg');
            }
        });
    });

    //    $('#decode select').change(function() {
    //        $('#decode select').each(function() {
    //            //console.log($(this).attr("id"));
    //            $(this).prop('disabled', true);
    //        });
    //    });

    $('p.error').click(function () {
        $(this).animate({ opacity: 0 }, 100);
        $(this).parent().animate({ height: 0 }, 200);
    });
    $('#trims li').click(function () {
        var li = $(this);
        li.children('input').prop('checked', true);
        $('#trims li').each(function () {
            console.log(this);
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
                $(this).children('input').prop('checked', false);
            }
        });
        li.addClass('selected');

    });

    $('#options li').click(function () {
        var li = $(this);
        if (li.hasClass('selected')) {
            li.removeClass('selected');
            li.children('input').prop('checked', false);
        } else {
            li.children('input').prop('checked', true);
            li.addClass('selected');
        }

    });

});