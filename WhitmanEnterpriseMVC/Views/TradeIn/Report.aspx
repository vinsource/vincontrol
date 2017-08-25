<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WhitmanEnterpriseMVC.Models.TradeinCustomerViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Recent Appraisals</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet"
        type="text/css" media="all" />

    <%--<script src="http://code.jquery.com/jquery-latest.js"></script>--%>
        <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <style type="text/css">
        body
        {
            background: url("../images/cBgRepeatW.png") repeat-y scroll center top transparent;
        }
        #c2
        {
            border-right: medium none;
        }
        #c2
        {
            width: 784px;
        }
        #c2
        {
            border-left: 1px solid #666666;
            border-top: 1px solid #888888;
            height: 698px;
        }
        .scroll-pane
        {
            height: 700px;
            overflow-x: hidden;
            overflow-y: auto;
        }
        #recent
        {
        }
        #recent li
        {
            margin-left: -3px;
            margin-top: .3em;
            font-size: .9em;
        }
        img.thumb
        {
            margin-left: -20px;
            float: left;
            clear: right;
            width: 47px;
            height: 47px;
        }
        img.mThumb
        {
            width: 97px;
            height: 97px;
        }
        #c3 li
        {
            width: 200px;
        }
        #c3 ul.info
        {
            height: 100px;
            margin-bottom: 1em;
        }
        ul
        {
            margin-top: 1em;
        }
        p
        {
            padding: 1em;
            border-bottom: 1px solid #101010;
            border-top: 1px #777 solid;
            margin: 0;
        }
        p.top
        {
            border-top: none;
            padding: 0;
            margin-top: .5em;
        }
        p.bot
        {
            border-bottom: none;
            padding: 0;
        }
        input.sForm
        {
            background: green;
            border: 0;
            color: white;
            width: 100%;
        }
        #table
        {
            width: 720px;
        }
        #table .mid, div .mid, #tableHeader .mid
        {
            width: 82px;
        }
        #table .long, #tableHeader .long.cell
        {
            width: 140px;
        }
        #table .longest, div .longest, #tableHeader .longest
        {
            width: 209px !important;
        }
        #table div
        {
            margin-bottom: 0em;
        }
        ul.l
        {
            padding-top: .2em;
            padding-bottom: .3em;
            margin: 0;
        }
        .hover
        {
            display: none;
        }
        .scroll-pane
        {
            height: 100%;
            overflow: auto;
            overflow-x: hidden;
        }
        #activity
        {
            height: 400px;
        }
        .rowOuter
        {
            margin-bottom: .5em !important;
            padding: .5em !important;
            padding-right: 0 !important;
            padding-bottom: .1em !important;
        }
        .light
        {
            background: #555;
        }
        .dark
        {
            background: #111;
        }
        #tableHeader .cell
        {
            border: none;
            border-bottom: 5px solid #990000;
            height: 20px !important;
            width: 97px;
            margin: 0;
            padding: 0;
            font-weight: bold;
            margin-right: 3px;
        }
        #tableHeader .longer
        {
            width: 137px;
        }
        #tableHeader .short
        {
            width: 76px;
        }
        #tableHeader .shortest
        {
            width: 29px;
        }
        #tableHeader .infoCell
        {
            width: 674px;
        }
        #tableHeader .shorter
        {
            width: 57px;
        }
        .cell
        {
            text-align: center;
            border-left: 2px solid #860000;
        }
        .noBorder
        {
            border: none !important;
        }
        #tableHeader .rowOuter
        {
            border: none !important;
        }
        #tableHeader .start
        {
            margin-left: 50px;
            width: 89px;
        }
        .dark .marketSection
        {
            background: #460000 !important;
        }
        .light .marketSection
        {
            background: #460000 !important;
        }
        .border
        {
            border: 3px red solid;
        }
        .med
        {
            border: 3px yellow solid;
        }
        input[name="price"]
        {
            background: green !important;
        }
        .hideLoader
        {
            display: none;
        }
        .imageCell
        {
            height: 26px;
            vertical-align: bottom;
        }
        #tableHeader .cell
        {
            padding: 0.5px;
        }
        #tableHeader .startCell
        {
            width: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <a href="<%=Url.Content("~/Appraisal/ViewAppraisal")%>">Recent Appraisals </a>
        <%--<input type="button" id="btnPrint" value="Print" />--%>
        <img class="imageCell" src="<%= Url.Content("~/content/images/vin-trade-in-long.gif") %>"
            alt="" />
        <a class="iframe" href="<%=Url.Content("~/TradeIn/ReportOptions")%>" style="background-color: #666;
            padding: 2px; font-size: 13px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;">
            Print </a>
    </h3>
    <div id="tableHeader">
        <div class="innerRow1 clear">
            <div class="cell startCell column">
            </div>
            <div class="long cell column">
                <a id="lnkTrade_Year" href="javascript:;" order="desc">Year </a>
            </div>
            <div class="cell column">
                <a id="lnkTrade_Make" href="javascript:;" order="desc">Make </a>
            </div>
            <div class="cell column" style="width: 110px">
                <a id="lnkTrade_Model" href="javascript:;" order="desc">Model </a>
            </div>
            <div class="short cell column">
                <a id="lnkTrade_Condition" href="javascript:;" order="desc">Condition </a>
            </div>
            <div class="short cell column">
                <a id="lnkTrade_Date" href="javascript:;" order="desc">Date </a>
            </div>
            <div class="short cell column">
                <a id="lnkTrade_Status" href="javascript:;" order="desc">Status </a>
            </div>
            <div class="shorter cell column">
                <a id="lnkTrade_Price" href="javascript:;" order="desc">Price </a>
            </div>
        </div>
    </div>
    <div>
        <input type="hidden" id="hdnCost" value="Date,Des" />
        <input type="hidden" id="hdnSortField" value="Date,Des" />
    </div>
    <div style="clear: both">
    </div>
    <div class="scroll-pane">
        <div id="recent">
            <div id="table">
                <!-- entries go in rowOuter div -->
                <%Html.RenderPartial("AppraisalList", Model); %>
            </div>
            <!-- end of row div -->
        </div>
    </div>
    <%--<div id="activity">
        <br />
        <h3>
            Dealership Activity</h3>
        <p class="top">
        </p>
        <p>
            Coming Soon</p>
        <p class="bot">
        </p>
    </div>--%>
    </div>

    <script type="text/javascript">

        $(document).ready(function() {

            $(".sForm").numeric({ decimal: false, negative: false }, function() { alert("Positive integers only"); this.value = ""; this.focus(); });
        });
        //    $('a').click(function(e) {
        //    if ($(this).attr('target') != '_blank') {
        //            $('#elementID').removeClass('hideLoader');
        //        }

        //    });

        $("select[id^=opttrade]").change(function() {
            var idValue = this.id.split('_')[1];
            var value = $(this).val();
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajax-loader1.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $.ajax({
                type: "POST",
                url: '<%= Url.Content("~/TradeIn/SaveTradeInStatus") %>',
                data: { status: value, id: idValue },
                success: function(results) {
                     $.unblockUI();
                }
            });
        });




        $("input[id^=inpSaveCost]").focus(function() {
            $("#hdnCost").val($(this).val());

        }).blur(function(defaultValue) {
            if ($(this).val() == $("#hdnCost").val()) {
                $("#hdnCost").val("");
            }
            else {
                var idValue = this.id.split('_')[1];
                var value = $(this).val();
                $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajax-loader1.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                $.ajax({
                    type: "POST",
                    url: '<%= Url.Content("~/TradeIn/SaveCost") %>',
                    data: { cost: value, id: idValue },
                    success: function(results) {
                         $.unblockUI();
                    }
                });
            }
        });

        $("a[id^=lnkSendEmail]").click(function() {
            var idValue = this.id.split('_')[1];

            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajax-loader1.gif") %>"  /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $.ajax({
                type: "POST",
                url: '<%= Url.Content("~/TradeIn/ResendEmailContent") %>',
                data: { id: idValue },
                success: function(results) {
                     $.unblockUI();

                    $("<a href=" + '<%= Url.Content("~/TradeIn/SuccesfulMessage?content=' + results + '") %>' + "></a>").fancybox({
                        overlayShow: true,
                        showCloseButton: true,
                        enableEscapeButton: true,
                        autoScale: true,
                        autoDimensions: false,
                        width: 500,
                        height: 50
                    }).click();
                }
            });
        });

        $("a[id^=lnkTrade]").click(function() {
            var idValue = this.id.split('_')[1];
            var sortOrder = $(this).attr("order");
            if (sortOrder == "asc") {
                $(this).attr("order", "desc");
            }
            else if (sortOrder == "desc") {
                $(this).attr("order", "asc");
            }
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajax-loader1.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $.ajax({
                type: "POST",
                url: '<%= Url.Content("~/TradeIn/GetPartialTradeinList") %>',
                data: { sort: idValue, sortOrder: sortOrder },
                success: function(results) {
                     $.unblockUI();
                    $("#table").html(results);
                }
            });
        });

        $("#btnPrint").click(function() {
            window.location = '<%= Url.Action("PrintTradeInCustomer", "PDF") %>';
        });

        $("a.iframe").fancybox({ 'width': 1000, 'height': 400 });
        function updateACV(txtBox) {
            console.log(txtBox.id);
            console.log(txtBox.value);
            $.post('<%= Url.Content("~/Appraisal/UpdateACV") %>', { AppraisalId: txtBox.id, ACV: txtBox.value }, function(data) {

            });
        }

    </script>

</asp:Content>
