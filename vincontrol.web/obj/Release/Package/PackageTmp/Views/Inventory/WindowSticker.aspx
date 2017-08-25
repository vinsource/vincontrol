<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>VINControl - WindowSticker</title>
    <link rel="shortcut icon" media="all" type="image/x-icon" href="<%= Url.Content("~/Content/images/vincontrol/icon.ico") %>" />
    <style type="text/css">
        html, body
        {
            height: 100%;
            width: 100%;
            margin: 0px;
            position: fixed;
        }

        body
        {
            text-align: center;
        }

        #outer
        {
            height: 100%;
            width: 100%;
            display: table;
            vertical-align: middle;
        }

        #container
        {
            text-align: center;
            position: relative;
            vertical-align: middle;
            display: table-cell;
        }

        #inner
        {
            width: auto;
            height: auto;
            text-align: center;
            margin-left: auto;
            margin-right: auto;
        }

        #WSForm
        {
            border: 1px solid blue;
        }

        .formTitle
        {
            background-color: #3366cc;
            color: white;
            font-weight: bold;
            font-size: 30px;
            padding: 10px 10px;
            margin-bottom: 10px;
            text-align: center;
        }

        .confirmButton
        {
            background-color: #3366cc;
            color: white;
            border: 0;
            float: right;
            margin: 30px 10px 20px 30px;
            padding: 10px 10px;
            width: 120px;
        }

        .cancelButton
        {
            float: right;
            border: 0;
            margin: 30px 30px 20px 0px;
            background-color: #414342;
            color: white;
            padding: 10px 40px;
            width: 120px;
        }

        .printOptions
        {
            text-align: left;
            padding: 5px;
            margin-left: 30px;
        }

        .bgOptions
        {
            float: right;
            width: 180px;
            height: 200px;
        }

        .templateImage
        {
            width: 150px;
        }
    </style>
    
    <link href="<%= Url.Content("~/Content/Admin.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Css/validationEngine.jquery.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/jquery-ui.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/jquery.alerts.css")%>" rel="stylesheet" type="text/css" media="screen" />
</head>
<body>
    <div id="outer">
        <div id="container">
            <div id="inner">
                <form id="WSForm">
                    <div class="formTitle">Window Sticker</div>
                    <div style="overflow: hidden">
                        <div class="printOptions">
                            <input type="radio" id="rdNoBackground" value="rdNoBackground" name="printOptions" checked="checked" />
                            <label for="rdNoBackground">Print without background</label>
                        </div>
                        <div class="printOptions">
                            <input type="radio" id="rdBackground" value="rdBackground" name="printOptions" />
                            <label for="rdBackground">Print with background</label>
                            <div id="bgOptions" style="margin-top: 5px;">
                                <div class="bgOptions">
                                    <div>
                                        <input type="radio" name="bgOptions" />
                                        <label></label>
                                    </div>
                                    <div>
                                        <img class="templateImage" src="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: right"></div>
                        <input class="cancelButton" type="button" value="Cancel" id="btnCancel" />
                        <input class="confirmButton" type="button" value="Print" id="btnConfirm" />
                    </div>
                </form>
            </div>
        </div>
    </div>
    
    <script src="<%= Url.Content("~/Scripts/jquery-1.6.4.min.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/jquery.validationEngine-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/jquery.validationEngine.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.alerts.js")%>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: "/Inventory/GetWSTemplate",
                data: {},
                success: function (results) {
                    var options = "";

                    results.reverse().forEach(function (opt) {
                        options = options + '<div class="bgOptions">';
                        options = options + '<div>';
                        options = options + '<input type="radio" id="' + opt.templateId + '" value="' + opt.templateId + '" name="bgOptions" />';
                        options = options + '<label for="' + opt.templateId + '">' + opt.templateName + '</label>';
                        options = options + '</div>';
                        options = options + '<div>';
                        options = options + '<img class="templateImage" src="' + opt.templateUrl + '" />';
                        options = options + '</div>';
                        options = options + '</div>';
                    });

                    $("#bgOptions").html(options);

                    $("input[type=radio]").change(function () {
                        if ($(this).parent().parent().hasClass("bgOptions")) {
                            $("#rdNoBackground").prop("checked", false);
                            $("#rdBackground").prop("checked", true);
                        }
                        else
                            if (this.id == "rdBackground") {
                                $("#bgOptions").find("input:last").prop("checked", true);
                            }
                            else {
                                $(".bgOptions").find("input").prop("checked", false);
                            }
                    });

                    $(".templateImage").click(function (event) {
                        var radio = $(this).parent().parent().find("input");

                        if (radio) {
                            radio.prop("checked", true);
                            radio.change();
                        }
                    });
                }
            });

            $("#btnCancel").click(function (event) {
                //$("#WSForm").find(":checked").prop("checked", false);
                parent.$.fancybox.close();
            });

            $("#btnConfirm").click(function (event) {
                var listingId = '<%= ViewData["listingId"] %>';
                    var usingTemplate = $("#rdBackground").is(':checked');

                    if (usingTemplate) {
                        var selects = $("#bgOptions").find(":checked");

                        if (selects.length > 0) {
                            templateId = selects[0].id;
                        }

                        window.parent.printWSWithTemplate(listingId, templateId);
                    }
                    else {
                        window.parent.printWSWithoutTemplate(listingId);
                    }

                    //parent.$.fancybox.close();
                });
            });
    </script>
</body>
</html>
