<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.BuyerGuideViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<%@ Import Namespace="Vincontrol.Web.Models" %>
<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.Year %>
    <%=Model.Make %>
    <%=Model.Model %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    
    <% Html.RenderPartial("InventoryTopMenu", new CarStatusViewModel() { InventoryStatus = Model.InventoryStatus, ListingId = Model.ListingId, Type = Model.Type }); %>
    
    <input type="hidden" id="hdDetailType" value="<%=ViewData["detailType"] %>" />
    <input type="hidden" id="NumberOfWSTemplate" name="NumberOfWSTemplate" value="<%= (int) SessionHandler.NumberOfWSTemplate %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="profile_bg_holder" class="profile_tab_holder" style="display: block;">
        <div id="container_right_btn_holder">
            <div id="container_right_btns" style="float: left">
                <% foreach (var language in Model.Languages)
                   {%>
                <div class="btns_shadow profile_bg_btns bg_enlish_print" onclick="SubmitPDF(<%=language.Value %>)">
                    <%=language.Text %>
                </div>
                <% } %>
            </div>
        </div>
        <div id="container_right_content" style="height: 600px; padding-top: 10px;">
            <div class="profile_bg_content" style="width: 90%; margin: 0 auto;">
                <div class="profile_bg_content_left">
                    <img src="/content/images/bGuideBG.png">
                </div>
                <div class="profile_bg_content_right">
                    <img src="/content/images/bGuideBG-spanish-resize.png">
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/VinControl/windowsticker.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function SubmitPDF(selectedLanguageID) {
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $.post('<%= Url.Content("~/Report/ViewBuyerGuideinPdfNew") %>', { selectedLanguage: selectedLanguageID, listingID:<%=Model.ListingId %> }, function(data) {
                //window.location = data.url;
                $("<a href=" + data.url + "></a>").fancybox({
                    height: 915,
                    width: 1000,
                    overlayShow: true,
                    showCloseButton: true,
                    enableEscapeButton: true,
                    type: 'iframe'
                }).click();
                $.unblockUI();
            });
        }

    </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            var detailType = $('#hdDetailType').val();
            $('div.admin_top_btns').each(function () {
                $(this).removeClass('admin_top_btns_active');
                if ($(this).attr('id') == detailType) {
                    $(this).addClass('admin_top_btns_active');
                }
            });

            $('a:not(.iframe)').click(function (e) {
                if ($(this).attr('target') == '') {
                    $('#elementID').removeClass('hideLoader');
                }

            });

            $("a.iframeCommon").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeTransfer").fancybox({ 'width': 350, 'height': 257, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeMarkSold").fancybox({ 'width': 455, 'height': 306, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeWholeSale").fancybox({ 'width': 360, 'height': 127, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeTrackingPrice").fancybox({ 'width': 870, 'height': 509, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeBucketJump").fancybox({ 'width': 929, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeStatus").fancybox({ 'margin': 0, 'padding': 0, 'width': 500, 'height': 230, 'hideOnOverlayClick': false, 'centerOnScroll': true });
            
            $("div[id='postCL_Tab']").click(function (e) {
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: "/Craigslist/AuthenticationChecking",
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        if (result == 302) {
                            $.ajax({
                                type: "GET",
                                dataType: "html",
                                url: "/Craigslist/LocationChecking",
                                data: {},
                                cache: false,
                                traditional: true,
                                success: function (result) {
                                    if (result == false)
                                        //jAlert("Please choose State/City/Location in Admin section.", "Warning");
                                        alert("Please choose State/City/Location in Admin section.");
                                    else
                                        //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
                                        var url = '/Craigslist/GoToPostingPreviewPage?listingId=' + '<%= Model.ListingId %>';
                                    $.fancybox({
                                        href: url,
                                        'width': 800,
                                        'height': 700,
                                        'hideOnOverlayClick': false,
                                        'centerOnScroll': true,
                                        'scrolling': 'yes',
                                        'onCleanup': function () {
                                        },
                                        isLoaded: function () {
                                            //$.unblockUI();
                                        },
                                        onClosed: function () {

                                        }
                                    });
                                }
                            });
                            
                        } else {
                            //jAlert("Please enter valid Email/Password in Admin section.", "Warning");
                            alert("Please enter valid Email/Password in Admin section.");
                        }                        
                    },
                    error: function (err) {
                        
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>