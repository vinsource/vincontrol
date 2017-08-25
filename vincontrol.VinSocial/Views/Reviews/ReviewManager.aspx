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
                Reviews - Manager Dashboard</h3>
        </div>
        <div class="filter-box">
            <div class="sub-nav">
                <div class="sub-nav-btn active" id="dashboard-tab-btn">
                    Dashboard
                </div>
                <div class="sub-nav-btn" id="team-tab-btn">
                    Teams
                </div>
                <div class="sub-nav-btn" id="send-survey-tab-btn">
                    Send Survey
                </div>
                <div class="sub-nav-btn" id="survey-list-tab-btn">
                    Surveys
                </div>
                <div class="sub-nav-btn" id="reviews-list-tab-btn">
                    Reviews
                </div>
            </div>
        </div>
        <div class="content">
            <%=Html.Partial("~/Views/Reviews/Manager/Dashboard.ascx")%>
            <%=Html.Partial("~/Views/Reviews/Manager/Team.ascx")%>
            <%=Html.Partial("~/Views/Reviews/Manager/Surveys.ascx")%>
            <%=Html.Partial("~/Views/Reviews/Manager/SurveysList.ascx")%>
            <%=Html.Partial("~/Views/Reviews/Manager/ReviewsList.ascx")%>
        </div>
    </div>
    <!-- end of inner wrap div-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/social/reviews.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        $("#nav").find(".reviews").addClass("active");
    </script>
</asp:Content>
