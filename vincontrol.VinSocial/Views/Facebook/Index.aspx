<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VinSocial | Facebook
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <!--
						<div class="post-box-fb">
						<textarea name="fb-input">What's new?</textarea>
						<div class="controls">
						<div class="fb-tag"><img src="/Content/Images/social/tag.gif" alt=""></div>
						<div class="fb-photo"><img src="/Content/Images/social/photo.gif" alt=""></div>
						<div class="fb-mood"><img src="/Content/Images/social/feeling.gif" alt=""></div>
						<div class="fb-type">Public</div>
						<div class="fb-post-btn">Post</div>
						</div>
						</div>
						-->
        <div class="page-info">
            <span>
                <br>
            </span><span>
                <br>
            </span>
            <h3>
                Facebook</h3>
        </div>
        <div class="filter-box">
            <div class="sub-nav">
                <div class="sub-nav-btn active" id="dashboard-tab-btn">
                    Dashboard
                </div>
                <div class="sub-nav-btn" id="fb-stats-tab-btn">
                    Statistics
                </div>
            </div>
        </div>
        <div class="content">
            <!-- facebook script -->
            <div id="fb-root">
            </div>
            <script>
                // likebox
                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id))
                        return;
                    js = d.createElement(s);
                    js.id = id;
                    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=83806641846";
                    fjs.parentNode.insertBefore(js, fjs);
                } (document, 'script', 'facebook-jssdk'));
                // follows
                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id))
                        return;
                    js = d.createElement(s);
                    js.id = id;
                    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=83806641846";
                    fjs.parentNode.insertBefore(js, fjs);
                } (document, 'script', 'facebook-jssdk'));

            </script>
            <div class="dashboard-tab page-tab">
                <div id="calendar">
                    <div class="prev-month">
                    </div>
                    <div class="next-month">
                    </div>
                    <div class="facebook-stats-box">
                        <div class="followers">
                            <div class="label">
                                Followers
                            </div>
                            <div class="number">
                                1,234,456
                            </div>
                        </div>
                        <div class="likes">
                            <div class="label">
                                Likes
                            </div>
                            <div class="number">
                                1,234,456
                            </div>
                        </div>
                    </div>
                    <div class="calendar-draw-div">
                    </div>
                </div>
                <div class="post-new">
                    <img src="/Content/Images/social/fb-logo.png" width="25">
                    <button>
                        New Post
                    </button>
                </div>
                <div class="facebook-post-box">
                    <div class="stats-box">
                        <div class="date-header">
                            Aug 12, 2013
                        </div>
                        <div class="post-schedule">
                            <ul>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                                <li>
                                    <div class="post-time">
                                        10:30am
                                    </div>
                                    <div class="post-description">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing eli...
                                    </div>
                                    <div class="post-delete">
                                        Delete
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div id="facebook-recent-activity">
                    <img src="/Content/Images/social/activity-feed-placeholder.jpg">
                </div>
                <script type="text/javascript" src="/Scripts/social/calendar.js"></script>
                <script type="text/javascript">
                    $(document).ready(function () {

                        // Set current month and year,
                        // 0 = January, 11 = December
                        var calendar_config = {
                            month: 7,
                            year: 2013
                        };

                        $(".calendar-draw-div").calendarWidget(calendar_config);

                        $('.next-month').click(function (event) {
                            calendar_config.month++;
                            if (calendar_config.month > 11) {
                                calendar_config.year++;
                                calendar_config.month = 0;
                            }
                            console.log(calendar_config);
                            $(".calendar-draw-div").calendarWidget(calendar_config);
                        });

                        $('.prev-month').click(function (event) {
                            calendar_config.month--;
                            if (calendar_config.month < 0) {
                                calendar_config.month = 11;
                                calendar_config.year--;
                            }
                            console.log(calendar_config);
                            $(".calendar-draw-div").calendarWidget(calendar_config);
                        });

                        $('#calendar td').click(function (event) {
                            $('#calendar td').removeClass('cal-selected');
                            $(this).addClass('cal-selected');
                        });

                    });
                </script>
            </div>
            <div class="fb-stats-tab page-tab hidden">
                <div class="fb-mumbers-breakdown">
                    <div class="fb-numbers-box">
                        <div class="top-label">
                            Likes
                            <div class="fb-numbers-change">
                                (<span class="positive">+5%</span>)
                            </div>
                        </div>
                        <div class="fb-number">
                            9,999,999,999
                        </div>
                    </div>
                    <div class="fb-numbers-box">
                        <div class="top-label">
                            People Talking About This
                            <div class="fb-numbers-change">
                                (<span class="positive">+5%</span>)
                            </div>
                        </div>
                        <div class="fb-number">
                            9,999,999,999
                        </div>
                    </div>
                    <div class="fb-numbers-box">
                        <div class="top-label">
                            Reach
                            <div class="fb-numbers-change">
                                (<span class="positive">+5%</span>)
                            </div>
                        </div>
                        <div class="fb-number">
                            9,999,999,999
                        </div>
                    </div>
                    <div class="fb-numbers-box">
                        <div class="top-label">
                            Friends of Fans
                            <div class="fb-numbers-change">
                                (<span class="positive">+5%</span>)
                            </div>
                        </div>
                        <div class="fb-number">
                            9,999,999,999
                        </div>
                    </div>
                </div>
                <div class="fb-graph-canvas">
                    <img src="/Content/Images/social/placeholder-graph.jpg" width="886">
                </div>
                <div class="fb-posts-items">
                    <div class="scrollableContainer">
                        <div class="scrollingArea">
                            <table class="list" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th width="60">
                                        </th>
                                        <th width="475">
                                            Post
                                        </th>
                                        <th width="105">
                                            Total Reach
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fb-post-type" width="60">
                                            <span class="ico">ICO</span>
                                        </td>
                                        <td class="fb-post-name" width="475">
                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod.
                                        </td>
                                        <td class="fb-total-reach" width="85">
                                            50
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="post-data-box">
                    <p>
                        Select a post to the left and see more specific data for it.
                    </p>
                    <p>
                        You can also sort the list by clicking the headers.
                    </p>
                </div>
            </div>
        </div>
        <div class="popup facebook-post-popup hidden">
            <div class="popup-wrap">
                <div class="fb-popup-controls">
                    <div class="label">
                        Date:
                    </div>
                    <input type="text">
                    <div class="label">
                        Time:
                    </div>
                    <select>
                        <option>12:00</option>
                        <option>01:00</option>
                        <option>02:00</option>
                        <option>03:00</option>
                        <option>04:00</option>
                        <option>05:00</option>
                        <option>06:00</option>
                        <option>07:00</option>
                        <option>08:00</option>
                        <option>09:00</option>
                        <option>10:00</option>
                        <option>11:00</option>
                        <option>12:00</option>
                        <option>01:00</option>
                        <option>02:00</option>
                        <option>03:00</option>
                        <option>04:00</option>
                        <option>05:00</option>
                        <option>06:00</option>
                        <option>07:00</option>
                        <option>08:00</option>
                        <option>09:00</option>
                        <option>10:00</option>
                        <option>11:00</option>
                    </select>
                    <select>
                        <option>AM</option>
                        <option>PM</option>
                    </select>
                </div>
                <div class="fb-popup-postbox">
                    <textarea placeholder="Enter Your Post Here!"></textarea>
                    <div class="fb-controls">
                        <div class="fb-btn tag">
                        </div>
                        <div class="fb-btn photo">
                        </div>
                        <div class="fb-btn emotion">
                        </div>
                        <div class="fb-btn place">
                        </div>
                        <div class="fb-btn save right">
                            Save
                        </div>
                        <div class="fb-btn cancel right">
                            Cancel
                        </div>
                        <div class="fb-dropdown restriction right">
                            <select>
                                <option>Private</option>
                                <option>Public</option>
                                <option>Friends</option>
                                <option>Friends and no aquaintences</option>
                                <option>Custom</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- end of inner wrap div-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
<link href="<%=Url.Content("~/Content/social/facebook.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        $("#nav").find(".facebook").addClass("active");
    </script>
</asp:Content>
