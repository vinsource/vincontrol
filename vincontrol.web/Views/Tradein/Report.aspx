<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    VIN Advisor
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="inventory_top_btns_holder">
        <% AppraisalsUserRight userRight = SessionHandler.UserRight.Appraisals; %>

        <% if (userRight.Recent == true) %>
        <% { %>
        <div class="admin_top_btns" id="appraisal_recent_tab">
            <a href="<%=Url.Action("ViewAppraisal","Appraisal") %>">Recent</a>
        </div>
        <% } %>

        <% if (userRight.Pending == true) %>
        <% { %>
        <div class="admin_top_btns" id="appraisal_pending_tab">
            <a href="<%=Url.Action("ViewPendingAppraisal","Appraisal") %>">Pending</a>
        </div>
        <% } %>

        <% if (userRight.Advisor == true) %>
        <% { %>
        <div class="admin_top_btns appraisal_tradein_tab_active" id="appraisal_tradein_tab">
            <a href="<%=Url.Content("~/Tradein/Report")%>" style="font-size: 0px;">
                <img src="/content/images/VINAdvisor-LongNew.png" />
            </a>
        </div>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="appraisal_time_btns_holder">
                <div>
                    <div class="calendar_logo_To">
                        <div class="ac_date_holder">
                            <div style="float: right; padding: 1px 0px 0px 4px;">
                                <img alt="Calendar" src="/Content/images/vincontrol/icon-calendar.png">
                            </div>
                            <div style="float: right; margin-left: 5px; color: white">
                                To
                                <input disabled="disabled" type="text" class="appraisal_date_To" id="appraisal_date_To" />
                            </div>
                        </div>
                    </div>
                    <div class="calendar_logo_From">
                        <div class="ac_date_holder">
                            <div style="float: right; padding: 1px 0px 0px 4px;">
                                <img alt="Calendar" src="/Content/images/vincontrol/icon-calendar.png">
                            </div>
                            <div style="float: right; color: white; margin-left: 5px;">
                                From
                                <input disabled="disabled" type="text" class="appraisal_date_From" id="appraisal_date_From" />
                            </div>
                        </div>
                    </div>
                </div>
                <a onclick="updateDate(2)">
                    <div class="appraisal_time_btns">
                        Last Week
                    </div>
                </a>
                <a onclick="updateDate(1)">
                    <div class="appraisal_time_btns">
                        Today
                    </div>
                </a>
                <a onclick="updateDate(4)">
                    <div id="closedLeads" class="appraisal_time_btns">
                        Closed Leads
                    </div>
                </a>
            </div>
        </div>
    </div>
    <div class="ap_advisor_holder_v2" id="displayAdvisorList">
    </div>
    <input type="hidden" id="Hidden1" value="Date,Des" />
    <input type="hidden" id="Hidden2" value="Date,Des" />
    <input type="hidden" id="Hidden3" value="Date,Des" />
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $(".appraisal_date_To").val(todayDate(true));
            $(".appraisal_date_From").val(previousDate(true, 30));
            showAdvisorData(3);

            $(".appraisal_date_From").datepicker({
                //minDate: previousDate(true, 180),
                onSelect: function (dateText, inst) {
                    showAdvisorData(3);
                }
            });

            $(".appraisal_date_To").datepicker({
                //minDate: previousDate(true, 180),
                onSelect: function (dateText, inst) {
                    showAdvisorData(3);
                }
            });

            $(".calendar_logo_From").click(function () {
                $(".appraisal_date_From").trigger("focus");
            });

            $(".calendar_logo_To").click(function () {
                $(".appraisal_date_To").trigger("focus");
            });


            $(".sForm").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        });

        $("select[id^=opttrade]").change(function () {
            var idValue = this.id.split('_')[1];
            var value = $(this).val();
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $.ajax({
                type: "POST",
                url: '<%= Url.Content("~/TradeIn/SaveTradeInStatus") %>',
                data: { status: value, id: idValue },
                success: function (results) {
                    $.unblockUI();
                }
            });
        });

        $("input[id^=lnkSendEmail]").click(function () {
            var idValue = this.id.split('_')[1];
            var ACV = $(this).val();
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>"  /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $.ajax({
                type: "POST",
                url: '<%= Url.Content("~/TradeIn/ResendEmailContent") %>',
                data: { id: idValue, acv: ACV },
                success: function (results) {
                    $.unblockUI();

                    $("<a href=" + '<%= Url.Content("~/TradeIn/SuccesfulMessage?content=' + results + '") %>' + "></a>").fancybox({
                        overlayShow: true,
                        showCloseButton: true,
                        enableEscapeButton: true,
                        autoScale: true,
                        autoDimensions: false,
                        width: 500,
                        height: 50
                    }).click();
                }
            });
        });

        $("a.iframe").fancybox({ 'width': 1000, 'height': 400 });
        function updateACV(txtBox) {
            console.log(txtBox.id);
            console.log(txtBox.value);
            $.post('<%= Url.Content("~/Appraisal/UpdateACV") %>', { AppraisalId: txtBox.id, ACV: txtBox.value }, function (data) {

            });
        }
        var usingClosedLeads = false;

        function updateDate(type) {
            if (type == 1) {
                $(".appraisal_date_To").val(todayDate(true));
                $(".appraisal_date_From").val(todayDate(true));
                $("#closedLeads").css("background-color", "");
                usingClosedLeads = false;
            }
            else if (type == 2) {
                $(".appraisal_date_To").val(todayDate(true));
                $(".appraisal_date_From").val(previousDate(true, 7));
                $("#closedLeads").css("background-color", "");
                usingClosedLeads = false;
            }
            else if (type == 4) {
                $(".appraisal_date_To").val(todayDate(true));
                $(".appraisal_date_From").val(previousDate(true, 180));
                $("#closedLeads").css("background-color", "#003399");
                usingClosedLeads = true;
            }

            showAdvisorData(type);
        }

        function showAdvisorData(type) {
            if (usingClosedLeads == true) {
                type = 4;
            }

            var url = "/TradeIn/LoadReportList?type=" + type + "&fromDate=" + $(".appraisal_date_From").val() + "&toDate=" + $(".appraisal_date_To").val();
            $("#displayAdvisorList").html("<div class='data-content' align='center'><img  src='/content/images/ajaxloadingindicator.gif' /></div>");
            $.ajax({
                type: "POST",
                contentType: "text/hmtl; charset=utf-8",
                dataType: "html",
                url: url,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $("#displayAdvisorList").html(result);
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                }
            });
        }

        function previousDate(isFlash, noOfDate) {
            var beforeDate = new Date();
            var today = new Date();
            today.setDate(beforeDate.getDate() - noOfDate);

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
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/jquery-ui.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/appraisals.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/VinControl/NewAdvisor.css")%>" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>