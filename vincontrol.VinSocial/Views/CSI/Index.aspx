<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <div class="page-info">
            <span>
                <br>
            </span><span>
                <br>
            </span>
            <h3>
                CSI</h3>
        </div>
        <div class="filter-box">
            <div class="sub-nav">
                <div class="sub-nav-btn active" id="overview-tab-btn">
                    Overview
                </div>
                <div class="sub-nav-btn" id="customer-profile-tab-btn">
                    Customer Profile
                </div>
            </div>
        </div>
        <div class="content">
            
            <%=Html.Partial("~/Views/CSI/Overview.ascx")%>
            <%=Html.Partial("~/Views/CSI/Customer.ascx")%>
        </div>
        <!-- end of inner wrap div-->
    </div>
    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/social/csi.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script src="<%=Url.Content("~/Scripts/social/csi.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $("#nav").find(".csi").addClass("active");
    </script>
</asp:Content>
