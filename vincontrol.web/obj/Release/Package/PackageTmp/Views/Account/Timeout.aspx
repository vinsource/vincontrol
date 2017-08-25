<%@ Page Title="VINControl - Login" Language="C#" MasterPageFile="~/Views/Shared/TimeOut.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="Action" name="Action" value="" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    jQuery(document).ready(function () {
        if ($.browser.msie && parseInt($.browser.version, 10) === 8) {
            $.fancybox({
                href: '<%= Url.Action("LogOffForTimeOut", "Account") %>',
                 'type': 'iframe',
                 'width': 550,
                 'height': 400,
                 'scrolling': 'no',
                 'hideOnOverlayClick': false,
                 'onCleanup': function () {
                 },
                 onClosed: function () {

                     if ($("#Action").val() == 'Cancel') {
                         window.location.href = '<%= Url.Action("Index", "Home") %>';
                } else {
                    window.location.href = '<%= String.IsNullOrEmpty(SessionHandler.RedirectUrl) || SessionHandler.RedirectUrl.ToLower().Contains("account/timeout") ? "/Inventory/ViewInventory" : SessionHandler.RedirectUrl %>'; //'/Account/AfterLoggingOn?role=' + $("#Action").val();
                }

            },
                 afterShow: function () {
                     $(".fancybox-close").hide(); // hide close button                
                 }
             });
        } else {
            $.fancybox({
                href: '<%= Url.Action("LogOffForTimeOut", "Account") %>',
                 'type': 'iframe',
                 'width': 500,
                 'height': 300,
                 'scrolling': 'no',
                 'hideOnOverlayClick': false,
                 'onCleanup': function () {
                 },
                 onClosed: function () {

                     if ($("#Action").val() == 'Cancel') {
                         window.location.href = '<%= Url.Action("Index", "Home") %>';
                } else {
                    window.location.href = '<%= String.IsNullOrEmpty(SessionHandler.RedirectUrl) || SessionHandler.RedirectUrl.ToLower().Contains("account/timeout") ? "/Inventory/ViewInventory" : SessionHandler.RedirectUrl %>'; //'/Account/AfterLoggingOn?role=' + $("#Action").val();
                }

            },
                 afterShow: function () {
                     $(".fancybox-close").hide(); // hide close button                
                 }
             });
        }
       
    });
</script>
</asp:Content>
