<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VinSell | Timeout
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="Action" name="Action" value="" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
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
                    window.location.href = '<%= Session["RedirectUrl"] %>';
                }
            },
            afterShow: function () {
                $(".fancybox-close").hide(); // hide close button                
            }
        });
    });
</script>
</asp:Content>
