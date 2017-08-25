<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<List<Vincontrol.Web.Models.DealershipActivityViewModel>>" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Activity Tracking
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    <title>Dealer Activity</title>
    <link href="<%=Url.Content("~/Content/vinAdminMockupActive.css")%>" rel="stylesheet"
        type="text/css" />
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-ui.min.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/Content/jquery-ui.css")%>" rel="stylesheet" type="text/css" />
    <%--<link href="<%=Url.Content("~/Content/ui.theme.css")%>" rel="stylesheet" type="text/css" />--%>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <script type="text/javascript">

        var dealerActivityList = "/Tracking/GetAllDealerActivity";
        var pageIndex = 2;
        var pageSize = 50;
        var scrollHeight = 125;
        var hasActivityValue = true;
        var type = 2;
        var scrollHandler = function() {
            //console.log($('#detailDealerActivity').scrollTop(),scrollHeight);
            //if (hasActivityValue) {
            if (($('#detailDealerActivity').scrollTop() + $('#detailDealerActivity').innerHeight() >= $('#detailDealerActivity')[0].scrollHeight)&& hasActivityValue) {
                console.log(hasActivityValue);
                //scrollHeight += 125;
                //$('#divLoading').show();
                //var appraisalListUrl = "/Tracking/FilterDealerActivityNew?pageIndex=" + (pageIndex++) + "&pageSize=" + pageSize+"&type=" + type;
                $.ajax({
                    type: "GET",
                    contentType: "text/hmtl; charset=utf-8",
                    dataType: "html",
                    url: '/Tracking/FilterDealerActivityPaging',
                    data: { fromdate: $("#activity_date_From").val(), todate: $("#activity_date_To").val(), type: $('#hdType').val(), pageSize: pageSize, pageIndex: pageIndex++ },
                    cache: false,
                    traditional: true,
                    success: function(result) {
                        //$('#divLoading').hide();
                        //console.log(result);
                        hasActivityValue = $.trim(result) !== '';
                        if (hasActivityValue) {
                            $("#detailDealerActivity").append(result);
                        } else {
                            console.log(pageIndex);
                        }


                    },
                    error: function(err) {
                        //$('#divLoading').hide();
                        console.log(err.status + " - " + err.statusText);
                    }
                });
            }

        };
        
        function resetScrollValues()
        {
            pageIndex = 2;
            pageSize = 50;
            scrollHeight = 125;
            hasActivityValue = true;
        }
        $(document).ready(function () {
       
            $('#detailDealerActivity').scroll(scrollHandler);
            
            //$.ajax({
            //        type: "POST",
            //        contentType: "text/hmtl; charset=utf-8",
            //        dataType: "html",
            //        url: dealerActivityList,
            //        data: {},
            //        cache: false,
            //        traditional: true,
            //        success: function (result) {
            //            $("#detailDealerActivity").html(result);
            //            //$(".activity_date_From").val(getStartDate());
            //        },
            //        error: function (err) {
            //            console.log(err.status + " - " + err.statusText);
            //        }
            //    });
            
            $(".activity_date_To").val(todayDate(true));
            $(".activity_date_From").val(previousDate(true,30));
            $.ajax({
                type: "get",
                dataType: "html",
                url: '<%= Url.Action("FilterDealerActivityNew", "Tracking") %>',
                data: { fromdate: $(".activity_date_From").val(), todate: $(".activity_date_To").val(), type: $('#hdType').val() },
                cache: false,
                traditional: true,
                success: function(results) {
                    var idValue = $('#hdType').val();
                    if (idValue == <%=Constanst.ActivityType.Appraisal %>) {
                        $("#VehicleColumn").removeClass("activity_display");
                }
                    if (idValue == <%=Constanst.ActivityType.Inventory %>) {
                        $("#VehicleColumn").removeClass("activity_display");
                    }
            if (idValue == <%=Constanst.ActivityType.User %>) {
                $("#VehicleColumn").addClass("activity_display");
        }
                    if (idValue == <%=Constanst.ActivityType.ShareFlyer %>) {
                        $("#VehicleColumn").addClass("activity_display");
        }
                    
        //$('#detailDealerActivity').unbind('scroll');
        //$('#detailDealerActivity').scrollTop(0);
        $('#detailDealerActivity').html(results);
        //$('#detailDealerActivity').scroll(scrollHandler);
        },
        error: function(err) {
            console.log('System Error: ' + err.status + " - " + err.statusText);
        }
        });
        $("div[id^=tracking]").live('click', function() {
            $("#detailDealerActivity").html("         <div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");
            var idValue = this.id.split('_')[1];
            $("div.number_below").remove();

            $(".admin_top_btns_active").removeClass("admin_top_btns_active");
            $(this).addClass("admin_top_btns_active");
            $('#hdType').val(idValue);
            if (idValue == <%=Constanst.ActivityType.Appraisal %>) {
                    $("#VehicleColumn").removeClass("activity_display");
            $(this).append("<div class=\"number_below\" id=\"appraisals_tab\">0</div>");
        }
                if (idValue == <%=Constanst.ActivityType.Inventory %>) {
                    $("#VehicleColumn").removeClass("activity_display");
        $(this).append("<div class=\"number_below\" id=\"inventory_tab\">0</div>");
        }
        if (idValue == <%=Constanst.ActivityType.User %>) {
                        $("#VehicleColumn").addClass("activity_display");
        $(this).append("<div class=\"number_below\" id=\"user_tab\">0</div>");
        }
                
        if (idValue == <%=Constanst.ActivityType.ShareFlyer %>) {
                     $("#VehicleColumn").addClass("activity_display");
            $(this).append("<div class=\"number_below\" id=\"shareflyer_tab\">0</div>");
            }
            if (idValue == <%=Constanst.ActivityType.ShareBrochure %>) {
                        $("#VehicleColumn").addClass("activity_display");
                    $(this).append("<div class=\"number_below\" id=\"shareflyer_tab\">0</div>");
                    }
                

                    resetScrollValues();

                    dealerActivityList = "/Tracking/GetAllDealerActivity?type=" + idValue;

                    //$.ajax({
                    //    type: "POST",
                    //    contentType: "text/hmtl; charset=utf-8",
                    //    dataType: "html",
                    //    url: dealerActivityList,
                    //    data: {},
                    //    cache: false,
                    //    traditional: true,
                    //    success: function(result) {
                    //        $('#detailDealerActivity').unbind('scroll');
                    //        $('#detailDealerActivity').scrollTop(0);
                    //        $("#detailDealerActivity").html(result);
                    //        $('#detailDealerActivity').scroll(scrollHandler);
                    //        //$(".activity_date_From").val(getStartDate());
                    //        //$(".activity_date_To").val(todayDate(true));

                    //    },
                    //    error: function(err) {
                    //        console.log(err.status + " - " + err.statusText);
                    //    }
                    //});
                    ShowDataByDate($(".activity_date_From").val(), $(".activity_date_To").val());
                    });

                    $("#ddlMonths").live('change', function () {
                        FilterList($("#ddlMonths").val(), $("#ddlYears").val());
                    });

                    $("#ddlYears").live('change', function () {
                        FilterList($("#ddlMonths").val(), $("#ddlYears").val());
                    });

                    $("tr[id^='aDetailActivity_']").live('click', function () {
                        var id = this.id.split("_")[1];

                        window.parent.document.getElementById('ActivityId').value = id;
                        window.parent.document.getElementById('NeedToRedirectToDetailActivity').value = true;
                        parent.$.fancybox.close();
                    });

                    $(".activity_date_From").datepicker({ onSelect: function (dateText, inst) {
                        var toDate = $('.activity_date_To').val();
                        ShowDataByDate(dateText, toDate);
                        resetScrollValues();
                    }
                    });

                    $(".activity_date_To").datepicker({ onSelect: function (dateText, inst) {
                        var fromDate = $('.activity_date_From').val();
                        ShowDataByDate(fromDate, dateText);
                        resetScrollValues();
                    }
                    });

          


                    $(".calendar_logo_From").click(function () {
                        $(".activity_date_From").trigger("focus");
                    });

                    $(".calendar_logo_To").click(function () {
                        $(".activity_date_To").trigger("focus");
                    });
                    });
        
                    function previousDate(isFlash,noOfDate) {
                        var beforeDate = new Date();
                        var today = new Date();
                        today.setDate(beforeDate.getDate()-noOfDate);
                
                        var dd = today.getDate();
                        var mm = today.getMonth() + 1;
                        //January is 0!

                        var yyyy = today.getFullYear();
                        if (dd < 10) {
                            dd = '0' + dd;
                        }
                        if (mm < 10) {
                            mm = '0' + mm;
                        }

                        if (isFlash) {
                            today = mm + '/' + dd + '/' + yyyy;
                        } else {
                            today = yyyy + '-' + mm + '-' + dd;
                        }

                        return today;
                    }

                    function todayDate(isFlash) {
                        var today = new Date();
                        var dd = today.getDate();
                        var mm = today.getMonth() + 1;
                        //January is 0!

                        var yyyy = today.getFullYear();
                        if (dd < 10) {
                            dd = '0' + dd;
                        }
                        if (mm < 10) {
                            mm = '0' + mm;
                        }

                        if (isFlash) {
                            today = mm + '/' + dd + '/' + yyyy;
                        } else {
                            today = yyyy + '-' + mm + '-' + dd;
                        }

                        return today;
                    }
        
                    function getStartDate() {
                        var today = new Date();
                        var dd = $('#hdStartDay').val();
                        var mm = $('#hdStartMonth').val();
            
                        var yyyy = $('#hdStartYear').val();
                        if (dd < 10) {
                            dd = '0' + dd;
                        }
                        if (mm < 10) {
                            mm = '0' + mm;
                        }

                        today = mm + '/' + dd + '/' + yyyy;

                        return today;
                    }
                    function getVin(value) {
                        if (value != '' && value != null && value.length >= 8) {

                            return value.substring(value.length - 8, value.length);
                        } else
                            return value;
                    }
        
                    function FilterList(month, year) {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '<%= Url.Action("FilterDealerActivity", "Tracking") %>',
                            data: { month: month, year: year },
                            cache: false,
                            traditional: true,
                            success: function (results) {
                                $('#detailDealerActivity').html(results);

                            },
                            error: function (err) {
                                console.log('System Error: ' + err.status + " - " + err.statusText);
                            }
                        });
                    }

                    function ShowDataByDate(fromdate, todate) {
                        //if (fromdate > todate) {
                        //    alert('From Date can not be greater than To Date');
                        //}
                        $.ajax({
                            type: "get",
                            dataType: "html",
                            url: '<%= Url.Action("FilterDealerActivityNew", "Tracking") %>',
                            data: { fromdate: fromdate, todate: todate, type: $('#hdType').val() },
                            cache: false,
                            traditional: true,
                            success: function(results) {
                                var idValue = $('#hdType').val();
                                if (idValue == <%=Constanst.ActivityType.Appraisal %>) {
                        $("#VehicleColumn").removeClass("activity_display");
                }
                    if (idValue == <%=Constanst.ActivityType.Inventory %>) {
                        $("#VehicleColumn").removeClass("activity_display");
                    }
                        if (idValue == <%=Constanst.ActivityType.User %>) {
                            $("#VehicleColumn").addClass("activity_display");
                    }
                    if (idValue == <%=Constanst.ActivityType.ShareFlyer %>) {
            $("#VehicleColumn").addClass("activity_display");
        }
                    
        $('#detailDealerActivity').unbind('scroll');
        $('#detailDealerActivity').scrollTop(0);
        $('#detailDealerActivity').html(results);
        $('#detailDealerActivity').scroll(scrollHandler);
        },
        error: function(err) {
            console.log('System Error: ' + err.status + " - " + err.statusText);
        }
        });
        }
    </script>
    <div id="admin_top_btns_holder">
        <% ActivityUserRight userRight = SessionHandler.UserRight.Activity; %>

        <% if (userRight.Inventory == true) %>
        <% { %>
        <div class="admin_top_btns admin_top_btns_active" id="tracking_<%= Constanst.ActivityType.Inventory%>">
            Inventory
            <div class="number_below" id="inventory_tab">
            </div>
        </div>
        <% } %>

        <% if (userRight.Appraisals == true) %>
        <% { %>
        <div class="admin_top_btns" id="tracking_<%= Constanst.ActivityType.Appraisal%>">
            Appraisals
        </div>
        <% } %>

        <% if (userRight.ShareFlyers == true) %>
        <% { %>
        <div class="admin_top_btns" id="tracking_<%= Constanst.ActivityType.ShareFlyer%>">
            Share Flyers
        </div>
        <% } %>

        <% if (userRight.ShareBrochures == true) %>
        <% { %>
        <div class="admin_top_btns" id="tracking_<%= Constanst.ActivityType.ShareBrochure%>">
            Share Brochures
        </div>
        <% } %>

        <% if (userRight.Users == true) %>
        <% { %>
        <div class="admin_top_btns" id="tracking_<%= Constanst.ActivityType.User%>">
            Users
        </div>
        <% } %>
    </div>
    <input type="hidden" id="hdType" value="<%=ViewData["Type"] %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="admin_mockupActive_header">
                <div class="admin_mockupActive_header_left">
                    Activity Tracking
                </div>
                <div style="padding-top: 15px;">
                    <div class="calendar_logo_From">
                        <div class="ac_date_holder">
                            <div style="float: left; color: white">
                                From
                                <input disabled="disabled" type="text" class="activity_date_From" id="activity_date_From" />
                            </div>
                            <div style="float: left; padding: 1px 0px 0px 4px;">
                                <img alt="Calendar" src="/Content/images/vincontrol/icon-calendar.png">
                            </div>
                        </div>
                    </div>
                    <div class="calendar_logo_To">
                        <div class="ac_date_holder">
                            <div style="float: left; padding-left: 10px; color: white">
                                To
                                <input disabled="disabled" type="text" class="activity_date_To" id="activity_date_To" />
                            </div>
                            <div style="float: left; padding: 1px 0px 0px 4px;">
                                <img alt="Calendar" src="/Content/images/vincontrol/icon-calendar.png">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="container_right_content">
        <div class="admin_mockupActive_content">
            <div class="activity_list_header">
                <div class="activity_list_collumn" style="width: 13.4%;">
                    <div class="activity_list_first">
                        User
                    </div>
                </div>
                <div class="activity_list_collumn activity_datestock">
                    <div class="activity_list_second">
                        Date Stamp
                    </div>
                </div>

                <div id="VehicleColumn">
                    <div class="activity_list_collumn activity_vehicle">
                        <div class="activity_list_first">
                            Vehicle
                        </div>
                    </div>
                    <div class="activity_list_collumn activity_vin">
                        <div class="activity_list_second">
                            VIN
                        </div>
                    </div>
                    <div class="activity_list_collumn activity_stock">
                        <div class="activity_list_second">
                            Stock
                        </div>
                    </div>
                </div>

                <div class="activity_list_collumn">
                    <div class="activity_list_first">
                        Activity
                    </div>
                </div>

                <div class="activity_list_collumn activity_list_collumn_content">
                    <div class="activity_list_second activity_list_content_collumn">
                        Activity Content
                    </div>
                </div>
            </div>
            <div class="activity_list_content_holder" id="detailDealerActivity">
                <div class="data-content" align="center">
                    <img src="/content/images/ajaxloadingindicator.gif" />
                </div>
            </div>
            <div class="activity_list_content_holder" id="detailDealerActivityError" style="display: none; margin-left: 300px; padding-top: 10px;">
                There are no results which match your filter criteria.
            </div>
        </div>
    </div>
</asp:Content>
