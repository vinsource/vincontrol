<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.VariantCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Set Price Variance</h2>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <%-- <% using (Html.BeginForm()) {%>--%>
    <fieldset>
        <legend>Variance</legend>
        <p>
            <label for="Variance">
                Variance:</label>
            <%= Html.TextBox("Variance", Model.Variance) %>
            <%= Html.ValidationMessage("Variance", "*") %>
        </p>
        <p>
            <input type="button" id="btnsubmit" value="Save" />
        </p>
    </fieldset>
    <%--<% } %>--%>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet"
        type="text/css" />

    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />           

    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#btnsubmit").click(function() {
                var value = $("#Variance").val();
                $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                $.ajax({
                    type: "POST",
                    url: "/TradeIn/SaveVarianceCost?cost=" + value,
                    data: {},
                    success: function(results) {
                         $.unblockUI();
                    }
                });
            });
        });
    </script>

</asp:Content>
