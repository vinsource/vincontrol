<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.VariantCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Set Price Variance</h2>
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
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnsubmit").click(function () {
                var value = $("#Variance").val();
                $.blockUI({ message: '<div><img src="/images/ajaxloadingindicator.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
                $.ajax({
                    type: "POST",
                    url: "/TradeIn/SaveVarianceCost?cost=" + value,
                    data: {},
                    success: function (results) {
                        $.unblockUI();
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>