<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pending Appraisals</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet"
        type="text/css" media="all" />

    <%--<script src="http://code.jquery.com/jquery-latest.js"></script>--%>
        <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <style type="text/css">
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
            width: 565px; max-height: 800px; overflow-y:auto;
        }
        #table .mid
        {
            width: 82px;
        }
        
        #table .longer
        {
            width: 181px !important;
        }
        #table .longest
        {
            width: 205px !important;
        }
        #table div
        {
            margin-bottom: 0em;
        }
        #table .long
        {
            width: 130px;
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
        .hideLoader
        {
            display: none;
        }
        .imageCell
        {
            height: 26px;
            vertical-align: bottom;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function() {
//            var automaticallyRefreshPendingAppraisals = setInterval(function() {
//                $.post('<%= Url.Content("~/Appraisal/ListOfPendingAppraisals") %>', {}, function(data) {
//                    $('#table').html(data);
//                });
//            }, 20000);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="recent">
        <h3>
            Pending Appraisals
        </h3>
        <div id="table">
            <!-- entries go in rowOuter div -->
            <%= Html.Partial("ListOfPendingAppraisals", Model)%>
            <%--<%=Html.DynamicHtmlLabelAppraisal("txtAppraisalGridMiddle") %>--%>
            <!-- end of row div -->
        </div>
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
    <div id="c3" class="column">
       <%-- <% Html.BeginForm("SortBy", "Appraisal", FormMethod.Post, new { id = "SortByForm", name = "SortByForm" });%>
        <span id="sort">Sort By:
            <%=Html.DropDownListFor(x => x.SelectedSortSet, Model.SortSetList, new { @onChange = "  $('#elementID').removeClass('hideLoader');this.form.submit()" })%>
        </span>
        <br />
        <% Html.EndForm(); %>--%>
        <h3>
            Appraisal List
            <%-- <a class="iframe" href="<%=Url.Content("~/Report/AppraisalPrintOption")%>"
                style="background-color: #666; padding: 2px; font-size: 13px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;">
                Print </a>--%>
        </h3>
        <div class="scroll-pane">
            <ul>
                <%=Html.DynamicHtmlLabelAppraisal("txtPendingAppraisalGridSideBar") %>
            </ul>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            $(".sForm").numeric({ decimal: false, negative: false }, function() { alert("Positive integers only"); this.value = ""; this.focus(); });
        });

        $("a.iframe").fancybox({ 'width': 1000, 'height': 700 });

        function updateACV(txtBox) {
            console.log(txtBox.id);
            console.log(txtBox.value);
            $.post('<%= Url.Content("~/Appraisal/UpdateACV") %>', { AppraisalId: txtBox.id, ACV: txtBox.value }, function(data) {

            });
        }

    </script>

</asp:Content>
