<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/TimeOut.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="Action" name="Action" value="" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    jQuery(document).ready(function () {
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
                    window.location.href = '<%= SessionHandler.RedirectUrl %>'; //'/Account/AfterLoggingOn?role=' + $("#Action").val();
                }
            },
            afterShow: function () {
                $(".fancybox-close").hide(); // hide close button                
            }
        });
    });
</script>
</asp:Content>
