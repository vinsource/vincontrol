<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InventoryFormViewModel>" %>

<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Reports
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="add_user_holder profile_popup">
    </div>
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="report_fixed_title">
                Reporting Dashboard
            </div>
        </div>
    </div>
    <div id="container_right_content">
        <% ReportsUserRight userRight = SessionHandler.UserRight.Reports; %>

        <% if (userRight.Inventory == true) %>
        <% { %>
        <div class="reports_items">
            <div class="reports_items_header reports_inventory report_expended">
                <div class="reports_items_header_icon reports_items_expended">
                </div>
                <div class="reports_items_header_text">
                    Inventory Reports
                </div>
                <div class="reports_items_header_bottom">
                </div>
            </div>
            <div class="reports_items_content" style="display: block;">
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Pre-Owned Inventory Report", "ViewPreOwnedInventoryReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewPreOwnedInventoryReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                    </div>
                    <%-- <div class="report_content_view">
                        <img src="/Content/images/vincontrol/Report/Pre-owned%20inventory.PNG" />
                    </div>--%>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Pre-Owned Inventory Report Template 2", "ViewPreOwnedInventoryReportTemplate2", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewPreOwnedInventoryReportTemplate2", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%-- <div class="report_content_view">
                            <img src="/Content/images/vincontrol/Report/pre-owned inventory template 2.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("New Inventory Report", "ViewNewInventoryReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewNewInventoryReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--   <div class="report_content_view_left">
                            <img src="/Content/images/vincontrol/Report/new.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Recon Inventory Report", "ViewReconInventoryReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewReconInventoryReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>

                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Certified Pre-Owned Inventory Report", "ViewCertifiedInventoryReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewCertifiedInventoryReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>

                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Manheim Inventory Report", "ViewManheimInventoryReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewManheimInventoryReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>

                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Price Range Report", "PriceRangeReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("PriceRangeReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>

                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Aging Report", "AgingReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("AgingReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>

                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <% } %>

        <% if (userRight.Appraisal == true) %>
        <% { %>
        <div class="reports_items">
            <div class="reports_items_header">
                <div class="reports_items_header_icon">
                </div>
                <div class="reports_items_header_text">
                    Appraisal Reports
                </div>
                <div class="reports_items_header_bottom">
                </div>
            </div>
            <div class="reports_items_content">
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <a class="iframe" href="<%=Url.Content("~/Report/AppraisalPrintOption")%>">Appraisal
                            Report (Pdf)</a>
                    </div>
                    <div class="ric_part_title">
                        <a class="iframe" href="<%= Url.Action("PrintSummaryAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>" name="btnSummary" id="btnSummary">Summary</a> - <a class="iframe" name="btnDetail"
                            href="<%= Url.Action("PrintDetailedAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>" id="btnDetail">Detail</a>
                    </div>
                    <div class="ric_part_icon">
                        <a class="iframe" href="<%=Url.Action("AppraisalPrintOption", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>

                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <% } %>

        <% if (userRight.BucketJump == true) %>
        <% { %>
        <div class="reports_items">
            <div class="reports_items_header">
                <div class="reports_items_header_icon">
                </div>
                <div class="reports_items_header_text">
                    Bucket Jump Reports
                </div>
                <div class="reports_items_header_bottom">
                </div>
            </div>
            <div class="reports_items_content">
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Today Bucket Jump Report", "ViewTodayBucketJumpReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewTodayBucketJumpReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--<div class="report_content_view">
                            <img src="/Content/images/vincontrol/Report/todays bucket jump.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Next 7 Days Bucket Jump Report", "ViewNext7DaysBucketJumpReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewNext7DaysBucketJumpReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--   <div class="report_content_view">
                            <img src="/Content/images/vincontrol/Report/next 7 days bucket jump.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <% } %>

        <% if (userRight.Print == true) %>
        <% { %>
        <div class="reports_items">
            <div class="reports_items_header">
                <div class="reports_items_header_icon">
                </div>
                <div class="reports_items_header_text">
                    Print Reports
                </div>
                <div class="reports_items_header_bottom">
                </div>
            </div>
            <div class="reports_items_content">
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Karpower Report", "ViewKarpowerReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewKarpowerReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--  <div class="report_content_view">
                            <img src="/Content/images/vincontrol/Report/karpower.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Window Sticker Report", "ViewFullSticker", "PDF",null, new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewFullSticker", "PDF") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--   <div class="report_content_view">
                            <img src="/Content/images/vincontrol/Report/window sticker.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <a href="javascript:;">Buyer Guide Report </a>
                    </div>
                    <div class="ric_part_title">
                        <%= Html.ActionLink("  (English) ", "ViewFullInventoryBuyerGuide", "Report", new { @class="iframe" })%>
                        <%= Html.ActionLink("  (Spanish) ", "ViewFullInventoryBuyerGuideInSpanish", "Report", new { @class="iframe" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a class="iframe" href="<%=Url.Action("ViewFullInventoryBuyerGuide", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--<div class="report_content_view_left">
                            <img src="/Content/images/vincontrol/Report/buyer guide.PNG" />
                        </div>--%>
                    </div>
                </div>
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Profit Margin Report", "KBBWholesale", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("KBBWholesale", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--  <div class="report_content_view_middle">
                            <img src="/Content/images/vincontrol/Report/profit margin.PNG" />
                        </div>--%>
                    </div>
                </div>

                <div style="clear: both">
                </div>
            </div>
        </div>
        <% } %>

        <% if (userRight.Tracking == true) %>
        <% { %>
        <div class="reports_items">
            <div class="reports_items_header">
                <div class="reports_items_header_icon">
                </div>
                <div class="reports_items_header_text">
                    Tracking Reports
                </div>
                <div class="reports_items_header_bottom">
                </div>
            </div>
            <div class="reports_items_content">
                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Price Change Report", "ViewPriceChangeReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewPriceChangeReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--<div class="report_content_view_middle">
                            <img src="/Content/images/vincontrol/Report/price change.PNG" />
                        </div>--%>
                    </div>
                </div>

                <div class="reports_items_content_part">
                    <div class="ric_part_title">
                        <%= Html.ActionLink("Share Flyers Report", "ViewShareFlyersReport", "Report", new { @target = "_blank" })%>
                    </div>
                    <div class="ric_part_icon">
                        <a target="_blank" href="<%=Url.Action("ViewShareFlyersReport", "Report") %>">
                            <img src="/images/list_report_icon.jpg" />
                        </a>
                        <%--<div class="report_content_view_middle">
                            <img src="/Content/images/vincontrol/Report/price change.PNG" />
                        </div>--%>
                    </div>
                </div>

                <div style="clear: both">
                </div>
            </div>
        </div>
        <% } %>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/Scripts/jquery.uploadify.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.uploadify.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/report.js")%>" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(".reports_items_header").click(function () {
                if ($(this).hasClass("report_expended")) {
                    $(this).removeClass("report_expended").addClass("report_minimize");
                    $(this).parent().find(".reports_items_content").slideUp();
                    $(this).find(".reports_items_header_icon").removeClass("reports_items_expended");
                } else {
                    $(".report_expended").trigger("click");
                    $(this).removeClass("report_minimize").addClass("report_expended");
                    $(this).parent().find(".reports_items_content").slideDown();
                    $(this).find(".reports_items_header_icon").addClass("reports_items_expended");
                }

            });

            //$(".reports_inventory").trigger("click");

            //$("#btnSummary").click(function () {
            //window.location = '<%= Url.Action("PrintSummaryAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>';
            //window.open('<%= Url.Action("PrintSummaryAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>');
            //});

            //$("#btnDetail").click(function () {
            //window.location = '<%= Url.Action("PrintDetailedAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>';
            //window.open('<%= Url.Action("PrintDetailedAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>');
            //});

            $("a.iframe").fancybox({ 'width': 900, 'height': 600, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/ui.theme.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/jquery-ui.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/VinControl/report.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ric_part_title a
        {
            color: Black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>