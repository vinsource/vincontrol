<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.InventoryFormViewModel>" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Reports</title>
      <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />

    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <style type="text/css">
        #c2 input
        {
            width: 50px;
        }
        .hider
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript">
        $("#InventoryTab").attr("class", "");
        $("#AppraisalTab").attr("class", "");
        $("#KPITab").attr("class", "");
        $("#AdminTab").attr("class", "");
        $("#ReportTab").attr("class", "on");
    
    </script>

    <h3>
        Reports</h3>
   <%-- View Reports:
    <select id="rSelect" name="week">
        <option>Today</option>
    </select>--%>
    <span id="range" class="hider">Range:
        <input type="text" name="from" />-<input type="text" name="to" /></span>
    <ul>
      <%--  <%if (SessionHandler.Single)
          {%>--%>
        <li>
            <%= Html.ActionLink("Pre-Owned Inventory Report", "ViewPreOwnedInventoryReport", "Report", new { @target = "_blank" })%></li>
        <li>
            <%= Html.ActionLink("Pre-Owned Inventory Report Template 2", "ViewPreOwnedInventoryReportTemplate2", "Report", new { @target = "_blank" })%></li>
        <li>
            <%= Html.ActionLink("New Inventory Report", "ViewNewInventoryReport", "Report", new { @target = "_blank" })%></li>
        <li>
            <%= Html.ActionLink("Recon Inventory Report", "ViewReconInventoryReport", "Report", new { @target = "_blank" })%></li>
        <li>
            <%= Html.ActionLink("Certified Pre-Owned Inventory Report", "ViewCertifiedInventoryReport", "Report", new { @target = "_blank" })%></li>
    <%--    <%}
          else
          { %>
        <li>
            <%= Html.ActionLink("Pre-Owned Inventory Report", "ViewPreOwnedMultipleInventoryReport", "Report", new { @target = "_blank" })%></li>
        <li>
            <%= Html.ActionLink("New Inventory Report", "ViewNewMultipleInventoryReport", "Report", new { @target = "_blank" })%></li>
        <li>
            <%= Html.ActionLink("Recon Inventory Report", "ViewReconMultipleInventoryReport", "Report", new { @target = "_blank" })%></li>
        <%} %>--%>
        <li>
            <%= Html.ActionLink("Manheim Inventory Report", "ViewManheimInventoryReport", "Report", new { @target = "_blank" })%>
        </li>
        <li><a class="iframe" href="<%=Url.Content("~/Report/AppraisalPrintOption")%>">Appraisal
            Report (Pdf)</a> - <a href="javascript:;" name="btnSummary" id="btnSummary">Summary</a> - <a name="btnDetail"
              href="javascript:;"  id="btnDetail">Detail</a></li>
        <li>
            <%= Html.ActionLink("Today Bucket Jump Report", "ViewTodayBucketJumpReport", "Report", new { @target = "_blank" })%>
        </li>
        <li>
            <%= Html.ActionLink("Next 7 Days Bucket Jump Report", "ViewNext7DaysBucketJumpReport", "Report", new { @target = "_blank" })%>
        </li>
        <li>
            <%= Html.ActionLink("Karpower Report", "ViewKarpowerReport", "Report", new { @target = "_blank" })%>
        </li>
        <li>
            <%= Html.ActionLink("Window Sticker Report", "ViewFullSticker", "Report", new { @target = "_blank" })%>
        </li>
          <li>
            <%= Html.ActionLink("Buyer Guide Report", "ViewFullInventoryBuyerGuide", "Report", new { @target = "_blank" })%>
            <%= Html.ActionLink(" (English) ", "ViewFullInventoryBuyerGuide", "Report", new { @target = "_blank" })%>
            <%= Html.ActionLink(" (Spanish) ", "ViewFullInventoryBuyerGuideInSpanish", "Report", new { @target = "_blank" })%>
        </li>
          <li>
            <%= Html.ActionLink("Price Change Report", "ViewPriceChangeReport", "Report", new { @target = "_blank" })%>
        </li>   
         <li>
            <%= Html.ActionLink("Profit Margin Report", "KBBWholesale", "Report", new { @target = "_blank" })%>
        </li>   
         <li>
            <%= Html.ActionLink("Price Range Report", "PriceRangeReport", "Report", new { @target = "_blank" })%>
        </li>           
    </ul>

    <script type="text/javascript" language="javascript">
        $('#rSelect').click(function() {
            if (this.selectedIndex == 4) {
                // console.log('dateRange selected');
                $('#range').removeClass('hider');
            } else {
                if (!$('#range').hasClass('hider')) {
                    $('#range').addClass('hider');
                }
            }
        });

        $("#btnSummary").click(function() {
        window.location = '<%= Url.Action("PrintSummaryAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>';
        });

        $("#btnDetail").click(function() {
        window.location = '<%= Url.Action("PrintDetailedAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>';
        });

    $("a.iframe").fancybox({ 'width': 500, 'height': 350, 'hideOnOverlayClick': false, 'centerOnScroll': true });
  
    </script>

</asp:Content>
