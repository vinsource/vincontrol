<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<vincontrol.Application.ViewModels.CommonManagement.ManheimWholesaleViewModel>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>VinSell | Manheim</title>
    <link href="<%=Url.Content("~/Scripts/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Content/cupertino/jquery-ui-1.8.14.custom.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Scripts/jquery.alerts.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/style.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/auction.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/charts.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body { background: #6770f0; }
        .mr-row div, .mr-row h3, .mr-row span
        {
            display: inline;
        }
        .mr-row
        {
            position: relative;
            overflow: hidden;
            /*width: 541px !important;*/
            margin-top: 8px !important;
        }
        .mr-row
        {
            background: #860000;
            border: 4px solid #860000;
            white-space: nowrap;
            display: inline-block;
            /*width: 560px !important;*/
            min-height: 25px;
            color: White;
            text-align: right;
        }
        .mr-row .range-item
        {
            display: inline-block;
            width: 122px;
            padding: 3px 4px 3px 4px;
            background: #555;
            margin-right: 0px;
        }
        .range-item.label
        {
            width: 120px !important;
        }
        .mr-row h3
        {
            background: #111;
            padding: 4px 5px 4px 5px !important;
            overflow: hidden;
            border-bottom: 3px #111 solid;
            cursor: pointer;
        }
        .mr-on
        {
            background: #860000 !important;
            border-bottom: 3px #860000 solid;
        }
        .high, .mid, .high, .low-wrap, .mid-wrap, .high-wrap
        {
            padding-left: 10px;
            font-weight: bold;
        }
        .high
        {
            background: #d02c00 !important;
        }
        .mid
        {
            background: #008000 !important;
        }
        .low
        {
            background: #0062A0 !important;
        }
    </style>

    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.alerts.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.easing-1.3.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.mousewheel-3.0.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/util.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/accounting.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
        var year = '';
        var makeId = '';
        var modelId = '';
        var trimId = '';
        $(document).ready(function () {
            $(document).ajaxError(function (event, request, settings, error) {
                //alert(request.status + " " + request.statusText);
            });

            $('#Trim').live('change', function () {
                $('div[id^="manheim_"]').hide();
                $('#transaction').html('');
                if ($('#Trim').val() != '') {
                    trimId = $('#Trim').val();
                    $('div[id^="manheim_' + trimId + '"]').show();
                    year = $('input[id^="hdnManheim_' + trimId + '"]').val().split('_')[0];
                    makeId = $('input[id^="hdnManheim_' + trimId + '"]').val().split('_')[1];
                    modelId = $('input[id^="hdnManheim_' + trimId + '"]').val().split('_')[2];
                    blockUI(waitingImage);
                    $.ajax({
                        type: "GET",
                        dataType: "html",
                        url: '/Manheim/ManheimTransaction?year=' + year + "&make=" + makeId + "&model=" + modelId + "&trim=" + trimId,
                        data: {},
                        cache: false,
                        traditional: true,
                        success: function (result) {
                            $('#transaction').html(result);
                            unblockUI();
                        },
                        error: function (err) {
                            unblockUI();
                        }
                    });
                } else {
                }
            });
        });

    </script>
</head>
<body>
    <div>
        <div style="float:left;display:inline-block;width:auto;">
            <%: Html.DropDownList("Trim", Model.ToSelectItemList(m => (m.TrimServiceId), m => m.TrimName, false), "Trim", new { @class = "trim", style = "width:auto;padding:5px;font-size:1em;margin-top:10px;" })%>
        </div>
        <% if (Model != null && Model.Count > 0)
           {%>
        <% foreach (var manheimWholesale in Model)
           {%>
        <input type="hidden" id="hdnManheim_<%= manheimWholesale.TrimServiceId %>" value="<%= manheimWholesale.Year %>_<%= manheimWholesale.MakeServiceId %>_<%= manheimWholesale.ModelServiceId %>_<%= manheimWholesale.TrimServiceId %>" />
        <div id="manheim_<%= manheimWholesale.TrimServiceId %>" class="mr-row" style="display:none;">
            <div class="low-wrap range-item low">
                <span class="bb-price">
                    <%= manheimWholesale.LowestPrice %></span>
            </div>
            <div class="mid-wrap range-item mid">
                <span class="bb-price">
                    <%= manheimWholesale.AveragePrice %></span>
            </div>
            <div class="high-wrap range-item high">
                <span class="bb-price">
                    <%= manheimWholesale.HighestPrice %></span>
            </div>
        </div>
        <%} %>
        <%} %>
    </div>
    <div id="transaction">
    </div>
</body>
</html>
