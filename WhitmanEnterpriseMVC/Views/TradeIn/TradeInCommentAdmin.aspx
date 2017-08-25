<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WhitmanEnterpriseMVC.Models.TradeinCommentViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Add Comments</h2>
    <form id="search" method="post" action="">
    <div id="result">
        <%= Html.Partial("TradeInComment", Model) %>
    </div>
    <p>
        &nbsp;</p>
    <table>
        <tr>
            <td>
                Name
            </td>
            <td>
                <input id="Name" name="Name" type="text">
            </td>
        </tr>
        <tr>
            <td>
                City
            </td>
            <td>
                <input id="City" name="City" type="text">
            </td>
        </tr>
        <tr>
            <td>
                State
            </td>
            <td>
                <input id="State" name="State" type="text">
            </td>
        </tr>
        <tr>
            <td>
                Content
            </td>
            <td>
                <input id="Content" name="Content" type="text">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input type="button" id="btnAdd" name="add-comment" value="Add" />
            </td>
        </tr>
    </table>
    </form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet"
        type="text/css" />

    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />

    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#btnAdd").click(function() {
                $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                $.ajax({
                    type: "POST",
                    //url: "/TradeIn/AddComment?city=1&state=1&comment=1",
                    url: "/TradeIn/AddComment",
                    data: { city: $("#City").val(), state: $("#State").val(), comment: $("#Content").val(), name: $("#Name").val() },
                    success: function(results) {
                        $("#result").html(results);
                         $.unblockUI();
                    }
                });
            });

            $("input[id^=btnSave]").live('click', function() {
                var idValue = this.id.split('_')[1];
                var cityValue = $("#txtCity_" + idValue).val();
                var stateValue = $("#txtState_" + idValue).val();
                var contentValue = $("#txtContent_" + idValue).val();
                var nameValue = $("#txtName_" + idValue).val();
                $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                $.ajax({
                    type: "POST",
                    url: "/TradeIn/SaveComment",
                    data: { city: cityValue, state: stateValue, comment: contentValue, id: idValue, name: nameValue },
                    success: function(results) {
                        $("#result").html(results);
                         $.unblockUI();
                    }
                });
            });

            $("input[id^=btnDelete]").live('click', function() {
                var id = this.id.split('_')[1];
                $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                $.ajax({
                    type: "POST",
                    url: "/TradeIn/DeleteComment/" + id,
                    data: {},
                    success: function(results) {
                        $("#result").html(results);
                         $.unblockUI();
                    }
                });
            });
        });
    </script>

    <style type="text/css">
        #notes
        {
            position: relative;
            top: -220px;
            left: 150px;
            width: 750px;
            color: white;
            z-index: 0;
        }
        #c2
        {
            width: 784px;
        }
        h4
        {
            margin-bottom: 0;
            margin-top: 0;
        }
        #sort
        {
            margin-top: -1.5em;
        }
        p
        {
            margin-top: 0;
        }
        #recent li
        {
            margin-left: -3px;
            margin-top: .3em;
            font-size: .9em;
        }
        #recent .sForm
        {
            margin: 0;
        }
        #recent .sForm[name="odometer"]
        {
        }
        img.thumb
        {
            float: left;
            clear: right;
        }
        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }
        #c2
        {
            border-right: none;
        }
        #table
        {
            width: 850px;
            padding: 0;
            margin: 0;
            font-size: .8em;
        }
        #table div
        {
            padding: 0;
            margin: 0;
        }
        #table .cell
        {
            width: 95px;
            height: 20px;
            overflow: hidden;
            margin-bottom: 3px;
            background: none;
        }
        #table .longest
        {
            width: 184px;
        }
        #table .long
        {
            width: 99px;
        }
        #table .short
        {
            width: 67px;
        }
        #table .shortest
        {
            width: 29px;
        }
        #table .infoCell
        {
            width: 674px;
        }
        #table .mid
        {
            width: 84px;
        }
        div.evenShorter
        {
            width: 15px !important;
            border: none;
        }
        #table div
        {
            margin-bottom: .5em;
        }
        .scroll-pane
        {
            height: 700px;
            overflow: auto;
            overflow-x: hidden;
        }
        #table
        {
            position: relative;
            left: .5em;
            font-size: 9.5pt;
            word-spacing: -1px;
            width: 750px;
        }
        #table .shorter
        {
            width: 55px;
        }
        #table .eC
        {
            width: 100px;
        }
        .rowOuter
        { !important;margin-bottom:.5em!important;padding:.5em!important;padding-right:0!important;padding-bottom:.1em!important;}
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
        #tableHeader .longest
        {
            width: 184px;
        }
        #tableHeader .longer
        {
            width: 137px;
        }
        #tableHeader .long
        {
            width: 99px;
        }
        #tableHeader .short
        {
            width: 65px;
        }
        #tableHeader .shortest
        {
            width: 29px;
        }
        #tableHeader .infoCell
        {
            width: 674px;
        }
        #tableHeader .mid
        {
            width: 85px;
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
        .hider
        {
            display: none;
        }
        .imageWrap .text
        {
            position: absolute;
            top: 8px; /* in conjunction with left property, decides the text position */
            left: 4px;
            width: 300px; /* optional, though better have one */
            color: Red;
        }
        .hideLoader
        {
            display: none;
        }
    </style>
</asp:Content>
