var waitingImage = '../../Content/Images/ajaxloadingindicator.gif';
var systemErrorMessage = 'An error has occured during this process, please wait a few seconds and try again';

function ShowSystemError() {
    ShowErrorMessage(systemErrorMessage);
}

function ShowWarningMessage(message, width) {
    var customedWidth = width == undefined ? 350 : width;
    ShowMessage(message, "WARNING", customedWidth, "#003399");
}

function ShowErrorMessage(message, width) {
    var customedWidth = width == undefined ? 350 : width;
    ShowMessage(message, "ERROR", customedWidth, "#D12C00");
}

function ShowInfoMessage(message, type) {
    var customedType = type == undefined ? "INFORMATION" : type;
    var content = $("<div style='width:" + 350 + "px;font-size:13px;text-align:center;'></div>");
    var title = $("<div style='background-color:#003399" + "" + ";color:White;font-weight:bold;padding: 5px 0;'>" + customedType + "</div>");
    var body = $("<div style='padding: 5px 0;'>" + message + "</div>");
    content.append(title);
    content.append(body);
    
    $.fancybox({
        content: content,
        'hideOnOverlayClick': false,
        'titlePosition': 'outside',
        'padding': 0,
        'closeBtn': false,
        'onComplete': function () {
            $("#fancybox-close").hide(); // hide close button
            setTimeout(function () { $.fancybox.close(); }, 3000);
        }
    });
}

function ShowMessage(message, type, width, color) {
    var content = $("<div style='width:" + width + "px;font-size:13px;text-align:center;'></div>");
    var title = $("<div style='background-color:" + color + ";color:White;font-weight:bold;padding: 5px 0;'>" + type + "</div>");
    var body = $("<div style='padding: 5px 0;'>" + message + "</div>");
    var buttonOk = $("<div style='margin: 10px 0; text-align:center;'><input type='button' style='width:50px; cursor:pointer; margin: 0 auto; padding: 0;' value='OK' /></div>");
    content.append(title);
    content.append(body);
    content.append(buttonOk);

    $.fancybox({
        content: content,
        'hideOnOverlayClick': false,
        'titlePosition': 'outside',
        'padding': 0,
        'closeBtn': false,
        'onComplete': function () {
            $("#fancybox-close").hide(); // hide close button
        }
    });
    $(buttonOk).click(function () { $.fancybox.close(); });
}

function blockUI(element, message) {
    $(element).blockUI({ message: '<div><div style="display:inline-block;width:100%;text-align:center;"><img src="' + waitingImage + '"/></div><div style="display:inline-block;width:100%;text-align:center;color:white;font-size:14px;">' + message + '.</div></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
}

function blockUI(element) {
    $(element).blockUI({ message: '<div style="display:inline-block;width:100%;text-align:center;"><img src="' + waitingImage + '"/></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
}

function blockUI() {
    $.blockUI({ message: '<div style="display:inline-block;width:100%;text-align:center;"><img src="' + waitingImage + '"/></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
}

function blockUIPopUp(element) {
    $(element).blockUI({ message: '<div style="display:inline-block;width:100%;text-align:center;"><img src="' + waitingImage + '"/></div>', css: { width: '200px', backgroundColor: 'none', border: 'none' } });
}

function blockUIPopUp() {
    $.blockUI({ message: '<div style="display:inline-block;width:100%;text-align:center;"><img src="' + waitingImage + '"/></div>', css: { width: '200px', backgroundColor: 'none', border: 'none' } });
}

function unblockUI(element) {
    $(element).unblockUI();
}

function unblockUI() {
    $.unblockUI();
}

function money_format(number, decimals, decPoint, thousandsSep) {
    return '$' + number_format(number, decimals, decPoint, thousandsSep);
}
function miles_format(number, decimals, decPoint, thousandsSep) {
    return number_format(number, decimals, decPoint, thousandsSep);
}
function number_format(number, decimals, decPoint, thousandsSep) {
    
    //   example 1: number_format(1234.56);
    //   returns 1: '1,235'
    //   example 2: number_format(1234.56, 2, ',', ' ');
    //   returns 2: '1 234,56'
    //   example 3: number_format(1234.5678, 2, '.', '');
    //   returns 3: '1234.57'
    //   example 4: number_format(67, 2, ',', '.');
    //   returns 4: '67,00'
    //   example 5: number_format(1000);
    //   returns 5: '1,000'
    //   example 6: number_format(67.311, 2);
    //   returns 6: '67.31'
    //   example 7: number_format(1000.55, 1);
    //   returns 7: '1,000.6'
    //   example 8: number_format(67000, 5, ',', '.');
    //   returns 8: '67.000,00000'
    //   example 9: number_format(0.9, 0);
    //   returns 9: '1'
    //  example 10: number_format('1.20', 2);
    //  returns 10: '1.20'
    //  example 11: number_format('1.20', 4);
    //  returns 11: '1.2000'
    //  example 12: number_format('1.2000', 3);
    //  returns 12: '1.200'
    //  example 13: number_format('1 000,50', 2, '.', ' ');
    //  returns 13: '100 050.00'
    //  example 14: number_format(1e-8, 8, '.', '');
    //  returns 14: '0.00000001'

    number = (number + '')
      .replace(/[^0-9+\-Ee.]/g, '');
    var n = !isFinite(+number) ? 0 : +number,
      prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
      sep = (typeof thousandsSep === 'undefined') ? ',' : thousandsSep,
      dec = (typeof decPoint === 'undefined') ? '.' : decPoint,
      s = '',
      toFixedFix = function (n, prec) {
          var k = Math.pow(10, prec);
          return '' + (Math.round(n * k) / k)
            .toFixed(prec);
      };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n))
      .split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '')
      .length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1)
          .join('0');
    }
    return s.join(dec);
}

String.prototype.format = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};

// forceNumeric() plug-in implementation
jQuery.fn.forceNumeric = function () {

    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.which || e.keyCode;

            if (!e.shiftKey && !e.altKey && !e.ctrlKey &&
                // numbers   
                 key >= 48 && key <= 57 ||
                // Numeric keypad
                 key >= 96 && key <= 105 ||
                // comma, period and minus, . on keypad
                key == 190 || key == 188 || key == 109 || key == 110 ||
                // Backspace and Tab and Enter
                key == 8 || key == 9 || key == 13 ||
                // Home and End
                key == 35 || key == 36 ||
                // left and right arrows
                key == 37 || key == 39 ||
                // Del and Ins
                key == 46 || key == 45)
                return true;

            return false;
        });
    });
}
