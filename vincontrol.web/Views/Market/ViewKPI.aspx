<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InventoryFormViewModel>" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<%@ Import Namespace="Vincontrol.Web.HelperClass" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
Key Performance Index
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    
    <div id="admin_top_btns_holder">
        <% var userRight = SessionHandler.UserRight.KPI; %>

        <% if (userRight.PreOwned == true) %>
        <% { %>
        <div id="btnPreowned" class="admin_top_btns admin_top_btns_active">
            Pre-Owned
            <div class="number_below" id="inventory_used_tab_number">
                <%if (Model.SubSetList != null) %>
                <%
                  {
                %>
                <%= Model.SubSetList.Count%>
                <% }
                  else
                  {%>
                0
                <% } %></div>
        </div>
        <% } %>

        <% if (userRight.New == true) %>
        <% { %>
        <div id="btnNew" class="admin_top_btns">
            New
        </div>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.DynamicHtmlLabelForKPI("txtHiddenInventoryGauge", "HiddenInventoryGauge")%>
    <%=Html.DynamicHtmlLabelForKPI("txtHiddenContentGauge", "HiddenContentGauge")%>
    <div class="add_user_holder profile_popup">
    </div>
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="kpi_fixed_title">
                Key Performance Index
            </div>
        </div>
    </div>
    <div id="container_right_content">
        <div class="kpi_tab_holder" id="kpi_new_holder" style="height: 575px;">
            <div class="kpi_top_holder">
                <div class="kpi_top_items">
                    <div class="kpi_top_charts_holder" style="padding-left: 70px">
                        <div class="gauges" id="iHealthInventory">
                        </div>
                    </div>
                    <div class="kpi_top_charts_text">
                        Inventory Vs Market
                    </div>
                    <div class="kpi_pdp_holder">
                        <div class="kpi_pdp_items">
                            <div class="kpi_pdp_items_key">
                                Above Market
                            </div>
                            <a href="javascript:void(0)" id="kpiData_4">
                                <img src="/Content/images/market_higher.gif" style="float:left;width:15px !important;height:27px !important;margin-top:5px;"/>
                                <div class="kpi_pdp_items_value kpi_pdp_price_above">
                                    <%=Html.DynamicHtmlLabelForKPI("txtPercentAboveMarket", "PercentAboveMarket")%>
                                    (<%=Html.DynamicHtmlLabelForKPI("txtAboveMarket", "AboveMarket")%>)
                                </div>
                            </a>
                        </div>
                        <div class="kpi_pdp_items">
                            <div class="kpi_pdp_items_key">
                                At Market
                            </div>
                            <a href="javascript:void(0)" id="kpiData_5">
                                <img src="/Content/images/market_equal.gif" style="float:left;width:15px !important;height:27px !important;margin-top:5px;"/>
                                <div class="kpi_pdp_items_value kpi_pdp_price_average">
                                    <%=Html.DynamicHtmlLabelForKPI("txtPercentAverageMarket", "PercentAverageMarket")%>
                                    (<%=Html.DynamicHtmlLabelForKPI("txtAverageMarket", "AverageMarket")%>)
                                </div>
                            </a>
                        </div>
                        <div class="kpi_pdp_items">
                            <div class="kpi_pdp_items_key">
                                Below Market
                            </div>
                            <a href="javascript:void(0)" id="kpiData_6">
                                <img src="/Content/images/market_lower.gif" style="float:left;width:15px !important;height:27px !important;margin-top:5px;"/>
                                <div class="kpi_pdp_items_value kpi_pdp_price_below">
                                    <%=Html.DynamicHtmlLabelForKPI("txtPercentBelowMarket", "PercentBelowMarket")%>
                                    (<%=Html.DynamicHtmlLabelForKPI("txtBelowMarket", "BelowMarket")%>)
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="kpi_top_items">
                    <div class="kpi_content_right_top">
                        <div class="admin_notifications_title">
                            Inventory Age
                        </div>
                        <div class="kpi_inventory_age_holder">
                            <div class="kpi_inventory_age_items kpi_inventory_age_header">
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_days">
                                    Days
                                </div>
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_cars">
                                    # of Cars
                                </div>
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_percent">
                                    % of Inventory
                                </div>
                            </div>
                            <div class="kpi_inventory_age_items kpi_inventory_age_items_1">
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_days">
                                    0 - 15
                                </div>
                                <a href="javascript:void(0)" id="kpiData_7">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_cars">
                                        <%=Html.DynamicHtmlLabelForKPI("txt15Inventory", "0-15InInventory")%>
                                    </div>
                                </a><a href="javascript:void(0)" id="kpiData_8">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_percent">
                                        <%=Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory")%>
                                    </div>
                                </a>
                            </div>
                            <div class="kpi_inventory_age_items kpi_inventory_age_items_2">
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_days">
                                    16 - 30
                                </div>
                                <a href="javascript:void(0)" id="kpiData_9">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_cars">
                                        <%=Html.DynamicHtmlLabelForKPI("txt30Inventory", "16-30InInventory")%>
                                    </div>
                                </a><a href="javascript:void(0)" id="kpiData_10">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_percent">
                                        <%=Html.DynamicHtmlLabelForKPI("txtPercent30Inventory", "Percent16-30InInventory")%>
                                    </div>
                                </a>
                            </div>
                            <div class="kpi_inventory_age_items kpi_inventory_age_items_3">
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_days">
                                    31 - 60
                                </div>
                                <a href="javascript:void(0)" id="kpiData_11">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_cars">
                                        <%=Html.DynamicHtmlLabelForKPI("txt60Inventory", "31-60InInventory")%>
                                    </div>
                                </a><a href="javascript:void(0)" id="kpiData_12">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_percent">
                                        <%=Html.DynamicHtmlLabelForKPI("txtPercent60Inventory", "Percent31-60InInventory")%>
                                    </div>
                                </a>
                            </div>
                            <div class="kpi_inventory_age_items kpi_inventory_age_items_4">
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_days">
                                    61 - 90
                                </div>
                                <a href="javascript:void(0)" id="kpiData_13">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_cars">
                                        <%=Html.DynamicHtmlLabelForKPI("txt90Inventory", "61-90InInventory")%>
                                    </div>
                                </a><a href="javascript:void(0)" id="kpiData_14">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_percent">
                                        <%=Html.DynamicHtmlLabelForKPI("txtPercent90Inventory", "Percent61-90InInventory")%>
                                    </div>
                                </a>
                            </div>
                            <div class="kpi_inventory_age_items kpi_inventory_age_items_5">
                                <div class="kpi_inventory_age_collumn kpi_inventory_age_days">
                                    90+
                                </div>
                                <a href="javascript:void(0)" id="kpiData_15">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_cars">
                                        <%=Html.DynamicHtmlLabelForKPI("txtOverInventory", "90OverInInventory")%>
                                    </div>
                                </a><a href="javascript:void(0)" id="kpiData_16">
                                    <div class="kpi_inventory_age_collumn kpi_inventory_age_percent">
                                        <%=Html.DynamicHtmlLabelForKPI("txtPercentOverInventory", "Percent90OverInInventory")%>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="kpi_top_items">
                    <div class="kpi_top_charts_holder" style="padding-left: 70px">
                        <%--<img src="/images/kpi_content_chart.jpg" />--%>
                        <div class="gauges" id="iHealthContent">
                        </div>
                    </div>
                    <div class="kpi_top_charts_text">
                        Content
                    </div>
                    <div class="kpi_pdp_holder">
                        <div class="kpi_pdp_items">
                            <div class="kpi_pdp_items_key">
                                Pictures
                            </div>
                            <a href="javascript:void(0)" id="kpiData_1">
                                <div class="kpi_pdp_items_value" id="content_Pictures">
                                    <%=Html.DynamicHtmlLabelForKPI("txtPercentPics", "PercentPics")%>
                                </div>
                            </a>
                        </div>
                        <div class="kpi_pdp_items">
                            <div class="kpi_pdp_items_key">
                                Descriptions
                            </div>
                            <a href="javascript:void(0)" id="kpiData_3">
                                <div class="kpi_pdp_items_value" id="content_Descriptions">
                                    <%=Html.DynamicHtmlLabelForKPI("txtPercentDescriptions", "PercentDescriptions")%>
                                </div>
                            </a>
                        </div>
                        <div class="kpi_pdp_items">
                            <div class="kpi_pdp_items_key">
                                Price
                            </div>
                            <a href="javascript:void(0)" id="kpiData_2">
                                <div class="kpi_pdp_items_value" id="content_Price">
                                    <%=Html.DynamicHtmlLabelForKPI("txtPercentSalePrice", "PercentSalePrice")%>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="kpi_list_holder">
                <div class="kpi_list_header">
                    <div class="kpi_list_collum kpi_list_img">
                        <a class="iframeCommon" href="<%=Url.Action("PrintOption","Report",new {Condition=1}) %>"
                            id="kpiPrint">
                            <div id="addNewBG" class="btns_shadow kpi_print_btns">
                                Print
                            </div>
                        </a>
                    </div>
                    <a href="javascript:void(0);">
                        <div class="kpi_list_collum" style="width: 55px;padding-left: 20px;" id="kpisort_9" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="age">
                            Age
                            <img class="imgSort" id="imgSortAge" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a>
                    <a href="javascript:void(0);">
                        <div class="kpi_list_collum kpi_list_marketData" style="width: 57px;" id="kpisort_10" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="market">
                            Rank
                            <img class="imgSort" id="imgSortMarket" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a>
                    <a href="javascript:void(0);">
                        <div class="kpi_list_collum kpi_list_vin" id="kpisort_1" style="width: 60px !important;" onmouseover="this.style.background='gray';" onmouseout="this.style.background='white';" value="vin">
                            VIN
                            <img class="imgSort" id="imgSortVin" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum" style="width: 83px;margin-left: 8px;" id="kpisort_2" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="stock">
                            Stock
                            <img class="imgSort" id="imgSortStock" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum" style="width: 60px" id="kpisort_3" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="year">
                            Year
                            <img class="imgSort" id="imgSortYear" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum_mediumHeader" id="kpisort_4" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="make">
                            Make
                            <img class="imgSort" id="imgSortMake" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum_longHeader" id="kpisort_5" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="model">
                            Model
                            <img class="imgSort" id="imgSortModel" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum_mediumHeader" id="kpisort_6" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="trim">
                            Trim
                            <img class="imgSort" id="imgSortTrim" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum_longHeader" style="width: 90px" id="kpisort_7" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="color">
                            Color
                            <img class="imgSort" id="imgSortColor" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum" style="width: 70px" id="kpisort_11" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="miles">
                            Odometer
                            <img class="imgSort" id="imgSortMiles" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a><a href="javascript:void(0);">
                        <div class="kpi_list_collum" id="kpisort_12" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="price">
                            Price
                            <img class="imgSort" id="imgSortPrice" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a>
                    <a href="javascript:void(0);">
                        <div class="kpi_list_collum" style="width: 110px" id="kpisort_8" onmouseover="this.style.background='gray';"
                            onmouseout="this.style.background='white';" value="owners">
                            Owners
                            <img class="imgSort" id="imgSortOwners" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                        </div>
                    </a>
                 
                </div>
                <div class="kpi_list_container" id="kpicontanier">
                    <div class="data-content" align="center">
                        <img src="/content/images/ajaxloadingindicator.gif" /></div>
                </div>
            </div>
        </div>
    </div>
    
    <input type="hidden" id="NeedToRedirectToDetailActivity" name="NeedToRedirectToDetailActivity" />
    <input type="hidden" id="ActivityId" name="ActivityId" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/underscore.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.uploadify.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.uploadify.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/kpi.js")%>" type="text/javascript"></script>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <script type='text/javascript'>
        var inventoryObj = [];
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';
        var viewInfo = { sortFieldName: 'make', isUp: true };

        $("a.iframeCommon").fancybox({ 'margin': 0, 'padding': 0, 'width': 400, 'height': 180, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        $("#btnPreowned").click(function () {

            $('#elementID').removeClass('hideLoader');

            var actionUrl = '<%= Url.Action("ViewKpi", "Market") %>';

            window.location = actionUrl;


        });

        $("#btnNew").click(function () {


            $('#elementID').removeClass('hideLoader');


            var actionUrl = '<%= Url.Action("ViewNewKpi", "Market") %>';

            window.location = actionUrl;


        });
        $("#btnPreowned").click(function () {

            $('#elementID').removeClass('hideLoader');

            var actionUrl = '<%= Url.Action("ViewKpi", "Market") %>';

            window.location = actionUrl;


        });

        $("#btnNew").click(function () {


            $('#elementID').removeClass('hideLoader');


            var actionUrl = '<%= Url.Action("ViewNewKpi", "Market") %>';

            window.location = actionUrl;


        });
        var name = "Content";
        var value = 20;
        google.load('visualization', '1', { packages: ['gauge'] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var percentageInventory = $("#txtHiddenInventoryGauge").val();
            var percentageContent = $("#txtHiddenContentGauge").val();
            var numberInventory = parseInt(percentageInventory);
            var numberContent = parseInt(percentageContent);
            var dataInventory = google.visualization.arrayToDataTable([
                ['Label', 'Value'],
                ['Inventory', numberInventory]
            ]);
            var dataContent = google.visualization.arrayToDataTable([
                ['Label', 'Value'],
                ['Content', numberContent]
            ]);

            var options = {
                width: 565,
                height: 200,
                greenFrom: 66,
                greenTo: 100,
                redFrom: 0,
                redTo: 33,
                yellowFrom: 33,
                yellowTo: 66,
                minorTicks: 15
            };
            var chartInventory = new google.visualization.Gauge(document.getElementById('iHealthInventory'));
            chartInventory.draw(dataInventory, options);

            var chartContent = new google.visualization.Gauge(document.getElementById('iHealthContent'));
            chartContent.draw(dataContent, options);
        }

        $('a:not(.iframe)').click(function (e) {
            if ($(this).attr('target') == '')
                $('#elementID').removeClass('hideLoader');

        });
        var kpiListUrl = "/Market/GetKpiList";

        $(document).ready(function () {

            grid = new InventoryGrid(viewInfo);
            grid.bindSorting();
            grid.loadGrid();

            setContentColors();
            setDefaultSort();
        });

        function setDefaultSort() {
            $('#imgSortMake').css('display', 'inline');
            $('#imgSortMake').attr('src', '../Content/images/vincontrol/up.png');
        }

        function getValueFromPercentString(percentString) {
            return parseInt(percentString.substr(0, percentString.length - 1));

        }

        function setContentColors() {
            setContentColorBasedOnPercentage($("#content_Pictures"));
            setContentColorBasedOnPercentage($("#content_Descriptions"));
            setContentColorBasedOnPercentage($("#content_Price"));
        }

        function setContentColorBasedOnPercentage(item) {
            if (item[0] != null && item[0].innerText && item[0].innerText != "") {
                var percent = getValueFromPercentString(item[0].innerText);

                if (percent > 90) {
                    $(item).addClass("kpi_pdp_price_average");
                }
                else {
                    $(item).addClass("kpi_pdp_price_above");
                }

            }
        }

        $("a[id^=kpiData]").live('click', function () {
            $("#kpicontanier").html("         <div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");
            var idValue = this.id.split('_')[1];
            kpiListUrl = "/Market/ViewConditionKpiJson?Condition=" + idValue;
            grid.loadGrid(kpiListUrl);

        });

        function PopupBuyerGuideWindow(actionUrl) {

            $("<a href=" + actionUrl + "></a>").fancybox({
                height: 915,
                width: 1000,
                overlayShow: true,
                showCloseButton: true,
                enableEscapeButton: true,
                type: 'iframe'
            }).click();
        }


    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/kpi.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .disable
    {
        display: none;
    }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("_TemplateKPI")%>
</asp:Content>