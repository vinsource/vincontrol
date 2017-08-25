<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <div class="page-info">
        </div>
        <div class="filter-box">
            <div class="admin_tabs_holder">
                <div class="admin_tab" id="admin_tab_users">
                    Users
                </div>
                <div class="admin_tab" id="admin_tab_teams">
                    Teams
                </div>
                <div class="admin_tab" id="admin_tab_3rd">
                    3rd Party Info
                </div>
                <div class="admin_tab" id="admin_tab_videosetting">
                    Video Settings
                </div>
                <div class="admin_tab" id="admin_tab_notifications">
                    Notifications
                </div>
            </div>
        </div>
        <div class="content">
            <%=Html.Partial("~/Views/Admin/Teams.ascx")%>
            <%=Html.Partial("~/Views/Admin/Notifications.ascx")%>
            <%=Html.Partial("~/Views/Admin/ThirdParty.ascx")%>
            <%=Html.Partial("~/Views/Admin/Users.ascx")%>
            <%=Html.Partial("~/Views/Admin/VideoSettings.ascx")%>
            
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/social/admin.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/Scripts/social/admin.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $("#nav").find(".admin").addClass("active");
    </script>
</asp:Content>
