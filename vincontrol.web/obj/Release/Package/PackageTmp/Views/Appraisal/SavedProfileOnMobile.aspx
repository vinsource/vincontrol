<%@ Page Title="Appraisal Profile" MasterPageFile="~/Views/Shared/NewSite.Master"
    Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.ModelYear %>
    <%=Model.Make %>
    <%=Model.AppraisalModel%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">

    <div id="admin_top_btns_holder">
        <a href="<%=Url.Action("EditAppraisal","Appraisal",new{AppraisalId=Model.AppraisalID}) %>"
            style="color: black">
            <div id="appraisals_edit_tab" class="admin_top_btns">
                Edit Appraisal
            </div>
        </a><a class="iframe iframeStatus" href="<%=Url.Action("OpenStatus", "Appraisal", new { appraisalID = Model.AppraisalID }) %>"
            style="color: black">
            <div id="profile_status_tab" class="pf_has_popup admin_top_btns">
                Status
            </div>
        </a>
        <div id="appraisals_customerinfo_tab" class="admin_top_btns">
            <a class="iframe iframeCustomerInfo" href="<%=Url.Action("ViewCustomerInfo","Appraisal",new{appraisalId=Model.AppraisalID}) %>">Customer Information</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
    <input type="hidden" id="NeedToReloadPage" name="NeedToReloadPage" value="false" />
    <% Html.BeginForm("Action", "Appraisal", FormMethod.Post, new { id = "FormAppraisalDetail", name = "FormAppraisalDetail" }); %>
    <input type="hidden" id="StockNumber" name="StockNumber" />
    <input type="hidden" id="InventoryStatusCodeId" name="InventoryStatusCodeId" />
    <div class="profile_tab_holder" id="profile_edit_holder" style="display: block">
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <div class="profile_vh_info">
                    <%=  Html.DynamicHtmlLabel("txtTitle", "Title")%>
                </div>
            </div>
        </div>
        <div id="container_right_content">
            <div class="profile_top_container">
                <div class="profile_top_left_holder">
                    <div class="pt_left_carsInfo">
                        <div class="pt_left_carsInfo_left">
                            <div class="pt_left_carsInfo_img">
                                <%=Html.DynamicHtmlLabel("txtCarImage", "CarImage")%>
                                <%=Html.HiddenFor(x => x.AppraisalGenerateId)%>
                            </div>
                        </div>
                        <div class="pt_left_carsInfo_right">
                            <ul>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelVin", "Vin")%></li>
                                <%=Html.HiddenFor(i=>i.VinNumber) %>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelStock", "Stock")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelYear", "Year")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelMake", "Make")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelModel", "Model")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelTrim", "Trim")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelExteriorColor", "ExteriorColor")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelInteriorColor", "InteriorColor")%></li>
                                <%--<li>Age: 55 days </li>--%>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelTranmission", "Tranmission")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelOdometer", "Odometer")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelCylinders", "Cylinders")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelLitters", "Litters")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelDoors", "Doors")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelFuelType", "FuelType")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelMSRP", "MSRP")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelDriveType", "DriveType")%></li>
                                <li>Date:
                                    <%=Model.DateOfAppraisal %>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="profile_top_items_holder_appraisal">
                        <div class="profile_top_items_appraisal" style="width: 150px;">
                            <div class="profile_top_items_text_appraisal">
                                ACV
                            </div>
                            <div class="btns_shadow profile_top_items_btn">
                                Save
                            </div>
                            <%=Html.TextBoxFor(x => x.ACV, new { @class = "profile_top_items_input_appraisal" })%>
                        </div>
                        <div class="profile_top_items" style="width: 50px;">
                            <div class="control_appraisals_print">
                                <a class="pad_tab iframe iframePrint" title="Print" href="<%= Url.Content("~/PDF/PrintAppraisal/" +Model.AppraisalID) %>">
                                    <img src="/Content/images/icon-printBlue.png" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="pt_left_charts_holder">
                        <div class="ptl_charts_title">
                            <a id="A1" href="<%= Url.Action("NavigateToNationwide", "Chart", new { ListingId = Model.AppraisalGenerateId, isCarsCom = false }) %>">
                                <div class="btns_shadow ptl_charts_btn expend_market">
                                    <input type="button" style="margin-left: 0; border: none; background: none; color: white; padding-top: 3px; font-weight: bold; cursor: pointer"
                                        id="graphButton" name="toggleGraph"
                                        value="Expand" />
                                    <%--<%=Html.ExpandChartButton()%>--%>
                                </div>
                            </a>
                            <div class="ptl_charts_text">
                                Within (100 miles) from your location
                            </div>
                        </div>
                        <div class="ptl_chart_holder">
                            <div class="ptl_charts_left">
                                <div class="ptl_charts" id="graphWrap">
                                    <div id="placeholder" style="height: 100%; width: 100%; padding-left: 8px;" align="center">
                                        <img src="/content/images/ajaxloadingindicator.gif" />
                                    </div>
                                </div>
                                <div style="clear: both; border-top: solid 2px gray; height: 60px;">
                                    <div class="ptl_charts_ranking">
                                        <div class="ptl_chart_rangking_text">
                                            <div class="market_ranking_key">
                                                Market Ranking
                                            </div>
                                        </div>
                                        <div class="ptl_chart_avg">
                                            <nobr style="color: #458C00"><label id="txtAvgDays">
                                            0</label></nobr>
                                            Avg Days
                                        </div>
                                    </div>
                                    <div style="float: left; width: 100%; padding-left: 30px;">
                                        <div class="ptl_charts_right_items" style="margin-top: 0px;">
                                            <div class="ptl_charts_right_icons kpi_market_above_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #D12C00">
                                                <label id="txtMaxPrice">
                                                    0</label>
                                            </div>
                                        </div>
                                        <div class="ptl_charts_right_items">
                                            <div class="ptl_charts_right_icons kpi_market_equal_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #458C00">
                                                <label id="txtAveragePrice">
                                                    0</label>
                                            </div>
                                        </div>
                                        <div class="ptl_charts_right_items">
                                            <div class="ptl_charts_right_icons kpi_market_below_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #0062A1">
                                                <label id="txtMinPrice">
                                                    0</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="ptl_charts_right">
                                <div class="market_ranking_value">
                                    <label id="txtRanking">
                                        0
                                    </label>
                                    out of
                                    <label id="txtCarsOnMarket">
                                        0</label>
                                </div>
                                <div id="divTrims" style="display: inline-block; text-overflow: ellipsis; white-space: nowrap; overflow: hidden; width: 160px; background-color: #222; color: white; padding-left: 17px; font-size: 13px;">
                                    &nbsp;
                                </div>
                                <div id="trim-filter" style="padding: 5px; height: 155px; overflow-y: auto; font-size: 13px;">
                                </div>
                                <div class="btns_shadow profile_top_items_btn" id="SaveSelectionSmallChart" style="margin: 10px 5px;">
                                    Save
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="ptl_carinfo_des">
                        <%=  Html.DynamicHtmlLabel("txtDescription", "Notes")%>
                    </div>
                    <%--<div class="control_apprailsals_bottom_holder">
                        <div class="control_apprailsals_bottom_btns">
                            <div class="btns_shadow ca_bottom_add_wholdsales">
                                <input type="submit" class="pad" id="AddToWholeSale" name="AddToWholeSale" value="Add to WholeSale" />
                            </div>
                            <div class="btns_shadow ca_bottom_add_inventory">
                                <input type="submit" class="pad" id="AddToInventory" name="AddToInventory" value="Add to Inventory" />
                            </div>
                            <div class="btns_shadow ca_bottom_add_recon">
                                <input type="submit" class="pad" id="AddToRecon" name="AddToRecon" value="Add to Recon" />
                            </div>
                        </div>
                    </div>--%>
                </div>
                <div class="profile_top_right_holder">
                    <div class="ptr_items">
                        <div class="ptr_items_top" style="position: relative;">
                            <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.VinNumber%>')">
                                <img style="display: inline-flexbox; float: left;" src='<%=Url.Content("~/Images/carfax-large.jpg")%>'
                                    alt="carfax logo" /></a>
                            <div style="position: absolute; top: 10px; left: 170px; width: 140px;">
                                <a style="display: inline-block; cursor: pointer; height: 17px; font-size: 1.0em; font-weight: bold;"
                                    title="DMV Desk" onclick="window.open('https://secure.dmvdesk.com/dmvdesk/', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                                    <span style="position: relative; top: 0px;">DMV Desk</span> </a><a style="display: inline-block; cursor: pointer; height: 17px; font-size: 1.0em; font-weight: bold; padding-left: 5px; border-left: 1px solid;"
                                        title="KSR" onclick="window.open('https://www.dmvlink.com/online/default.asp', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                                        <span style="position: relative; top: 0px;">KSR</span> </a>
                            </div>
                        </div>
                        <div class="ptr_items_content">
                            <%if (Model.CarFax.Success)
                              {%>
                            <div class="ptr_items_content_header">
                                <div class="ptr_cafax_owners">
                                    <nobr class="ptr_cafax_number">
									    <%if (Model.CarFax.NumberofOwners.Equals("0"))
               { %>
                                            -
                                        <%}
               else if (Model.CarFax.NumberofOwners.Equals("1"))
               { %>
                                            <%=Model.CarFax.NumberofOwners%>
                                        <%}
               else
               {%>
                                            <%=Model.CarFax.NumberofOwners%>
                                        <%} %>
								    </nobr>
                                    Owner(s)
                                </div>
                                <div class="ptr_cafax_service_repords">
                                    <nobr class="ptr_cafax_number">
									    <%=Model.CarFax.ServiceRecords %>
								    </nobr>
                                    Service Reports
                                </div>
                            </div>
                            <%} %>
                            <div id="history-report" style="clear: both; float: left; width: 100%;">
                                <ul>
                                    <%foreach (var tmp in Model.CarFax.ReportList)
                                      {  %>
                                    <%if (tmp.Text.Contains("Prior Rental") || tmp.Text.Contains("Accident(s) / Damage Reported to CARFAX"))
                                      { %>
                                    <li style="background-color: red">
                                        <% }
                                      else
                                      {
                                        %>
                                        <li>
                                            <%
                                      } %>
                                            <%=tmp.Text %>
                                            <img class="c-fax-img" src="<%=tmp.Image %>" height="10px" width="10px" />
                                        </li>
                                        <%} %>
                                </ul>
                                <a style="margin-left: 25px; color: black; font-weight: bold" href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.VinNumber%>')">View Full Report</a>
                            </div>
                        </div>
                    </div>
                    <div class="ptr_items_Appraisal">
                        <div class="ptr_items_top" id="controlappraisals_manheim">
                            <a title="MMR Auction Pricing" href="JavaScript:newPopup('<%=Url.Content("~/Market/OpenManaheimLoginWindow?Vin=")%><%=Model.VinNumber%>')">
                                <div class="ptr_items_top_title_holder">
                                    <div class="ptr_items_top_title_logo">

                                        <img src="/content/images/logo_manheim.gif" />
                                    </div>
                                    <div class="ptr_items_top_title_text">
                                        Manheim
                                    </div>
                                </div>
                            </a>
                            <div class="ptr_item_top_circle circleBase" id="ManheimCount">
                                0
                            </div>
                        </div>
                        <div class="ptr_items_content_Appraisal">
                            <div class="ptr_items_content_list partialContents" data-url="<%= Url.Action("ManheimDataOnMobile", "Appraisal", new { listingId = Model.AppraisalGenerateId }) %>">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="ptr_items_Appraisal">
                        <div class="ptr_items_top" id="controlappraisals_kbb">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo">
                                    <img src="/Content/images/kbb-logo-alpha.png" />
                                </div>
                                <div class="ptr_items_top_title_text">
                                    KBB
                                </div>
                            </div>
                            <div class="ptr_item_top_circle circleBase" id="KBBCount">
                                0
                            </div>
                        </div>
                        <a style="float: right; font-weight: normal; font-size: .9em; display: block; margin-top: -18px;"
                            href="<%= Url.Action("ResetKbbApraisalTrim", "Market", new { AppraisalID = Model.AppraisalGenerateId }) %>">Not a correct trim? Click here</a>
                        <div class="ptr_items_content_Appraisal">
                            <div id="divPartialKbb" class="ptr_items_content_list partialContents" data-url="<%= Url.Action("KarPowerDataOnMobile", "Appraisal", new { listingId = Model.AppraisalGenerateId }) %>">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.symbols.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.functions.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/google_map_graph_plotter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/ChartSmall.js")%>" type="text/javascript"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/d3/d3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.d3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/return-large.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/func.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/chart.js")%>"></script>
    <script type="text/javascript">
        function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
        // ############################### //
        // Chart Data Set and Draw Section //
        // ############################### //
        // check url for GET
        var window_url = window.location.search;

        // default graph to unexpanded draw size
        var expand = expanded(window_url);

        // set chart dimensions
        if (expand) {
            chart_dimensions = ["700px", "500px"];
            $('#graphButton').css('display', 'none');
        } else {
            chart_dimensions = ["400px", "183px"];
        }

        // grab graph div element and click element
        var gwrap = $('#graphWrap');
        gwrap.css('width', chart_dimensions[0]);
        gwrap.css('height', chart_dimensions[1]);

        // load default options & trims
        // we already filtered the results in code behind, so we don't need to do on the js code
        //var default_option = ($('#Options').val() == '' || $('#Options').val() == '0') ? [0] : $('#Options').val().split(',');
        //var default_trim = ($('#Trims').val() == '' || $('#Trims').val() == '0') ? [0] : $('#Trims').val().split(',');
        var default_option = [0];
        var default_trim = [0];

        var ListingId = $('#AppraisalGenerateId').val();

        // create ajax post url
        //var requestUrl = '<%= Url.Action("GetMarketDataWithin100MilesRadius", "chart", new { appraisalId = "PLACEHOLDER" } ) %>';
        
        //var requestNationwideUrl = '<%= Url.Action("GetMarketDataByListingNationwideWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        var waitingImage = '<%= Url.Content("~/images/ajaxloadingindicator.gif") %>';

        var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange:{min:0,max:100}, isSoldView:false, isSmallChart:false };
        var $dCar = {};
        var $selectedCar = {};
        // set y change check
        var newY = false;

        // create current filterred list of car
        var $currentFilterredList = [];

        var openChart = false;
        var dataTrims = null;
        ChartInfo.isSmallChart = true;

        var UrlPaths = UrlPaths || { requestNationwideUrl: "" };
        UrlPaths.requestNationwideUrl = '<%= Url.Action("GetMarketDataByListingNationwideWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        UrlPaths.requestNationwideUrl = UrlPaths.requestNationwideUrl.replace("PLACEHOLDER", ListingId);

    </script>
    <script language="javascript" type="text/javascript">

        function showPopUpStock(inventoryStatus) {
            $('#InventoryStatusCodeId').val(inventoryStatus);
            var markSoldUrl = '/Appraisal/PopUpStock';
            $("<a href=" + markSoldUrl + "></a>").fancybox({
                height: 115,
                width: 455,
                padding:0,
                margin:0,
                overlayShow: true,
                showCloseButton: true,
                enableEscapeButton: true,
                type: 'iframe'
            }).click();
        }
        
        function showPopUpViewDetail(url,message) {
            var popUpUrl = '/Appraisal/PopUpViewDetail/?url=' + url+'&message='+message;
            $("<a href=" + popUpUrl + "></a>").fancybox({
                height: 150,
                width: 455,
                padding:0,
                margin:0,
                overlayShow: true,
                showCloseButton: true,
                enableEscapeButton: true,
                type: 'iframe'
            }).click();
        }
        
        function openMarkSoldAppraisalIframe(appraisalId) {
            var markSoldUrl = '/Appraisal/ViewMarkSold?appraisalID=' + appraisalId;
            $("<a href=" + markSoldUrl + "></a>").fancybox({
                height: 306,
                width: 455,
                overlayShow: true,
                showCloseButton: true,
                enableEscapeButton: true,
                type: 'iframe'
            }).click();
        }

        function SubmitAddInventory() {
            if($('#InventoryStatusCodeId').val()==1) {

            }
            else if($('#InventoryStatusCodeId').val()==3) {
                $('#FormAppraisalDetail').attr('action','AddToWholeSale');
                $('#FormAppraisalDetail').attr('method','Post');
                $('#FormAppraisalDetail').submit();   
            }
            else
            {
                $('#FormAppraisalDetail').attr('action','AddToInventory');
                $('#FormAppraisalDetail').attr('method','Post');
                $('#FormAppraisalDetail').submit();   
            }
        }
        
        function UnCheckStatus(className) {
  
        }
        
        function SubmitStockNumber(stock) {
            if($('#InventoryStatusCodeId').val()==1) {

            } 
            else if($('#InventoryStatusCodeId').val()==3) {
                $('#StockNumber').val(stock);
                $('#FormAppraisalDetail').attr('action','AddToWholeSale');
                $('#FormAppraisalDetail').attr('method','Post');
                $('#FormAppraisalDetail').submit();
            } 
            else
            {
                $('#StockNumber').val(stock);
                $('#FormAppraisalDetail').attr('action','AddToInventory');
                $('#FormAppraisalDetail').attr('method','Post');
                $('#FormAppraisalDetail').submit();
            }
        }
    
        $(document).ready(function (e) {
              
            $("#ACV").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
              
            $("#ACV").val(formatDollar(Number($("#ACV").val().replace(/[^0-9\.]+/g, ""))));

            $(".partialContents").each(function (index, item) {
                var url = $(item).data("url");
                if (url && url.length > 0) {
                    $(item).load(url);
                }
            });

            $("#aLoadFullKBBTrims").live('click', function () {
                $('.mr-row').each(function (index) {
                    //$(this).removeClass('disable');
                    //$(this).addClass('mr-row');
                    $(this).show();
                });
            });

            //$("a.iframe").live('click', function () {
            $("a.iframe").fancybox({
                'width': 1000,
                'height': 700,
                'hideOnOverlayClick': false,
                'centerOnScroll': true,
                'onCleanup': function () {
                    // reload page when closing Chart screen
                    if (openChart && $("#NeedToReloadPage").val() == 'true') {
                        blockUI();
                        openChart == false;
                        window.location.href = '/Appraisal/ViewProfileForAppraisal?AppraisalId=' + ListingId;
                    }
                }
            });
            //});
              
            $("a.iframeCustomerInfo").fancybox({ 'width': 330, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });
              
            $("a.iframePrint").fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });
              
            $("a.iframeStatus").fancybox({ 'margin': 0, 'padding': 0, 'width': 500, 'height': 260, 'hideOnOverlayClick': false, 'centerOnScroll': true });
            //});

            // fire click event to open Manheim Transaction fancybox popup
            $("a[id^='manheimRow']").live('mouseover focus', function (e) {
                $(this).fancybox({
                    'width': 1000,
                    'height': 700,
                    'hideOnOverlayClick': false,
                    'centerOnScroll': true,
                    'onCleanup': function () {
                    }
                });
            });

            // fire click event to open KBB Summary fancybox popup
            $("a[id^='kbbRow']").live('mouseover focus', function (e) {
                $(this).fancybox({
                    'width': 1000,
                    'height': 700,
                    'hideOnOverlayClick': false,
                    'centerOnScroll': true,
                    'onCleanup': function () {
                        // reload KBB section
                        if ($("#NeedToReloadPage").val() == 'true') {
                            $("#divPartialKbb").html('<div id=\"kbb-row\" class=\"mr-row\">Loading...</div>');
                            $.ajax({
                                type: "GET",
                                url: "/Appraisal/KarPowerData?listingId=" + '<%= Model.AppraisalGenerateId %>',
                                data: {},
                                dataType: 'html',
                                success: function (data) {
                                    $("#divPartialKbb").html(data);
                                    $("#NeedToReloadPage").val('false');
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    unblockUI();
                                }
                            });
                        }
                    }
                });
            });
        });

        $("#AddToInventory").click(function() {

            $('#elementID').removeClass('hideLoader');
        });

        $("#AddToRecon").click(function () {

            $('#elementID').removeClass('hideLoader');
        });

        function newPopup(url) {
            var popupWindow = window.open(
                url, 'popUpWindow', 'height=900,width=1000,left=500,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
        }

        $('#ACV').blur(function () {

            var acv = $("#ACV").val();

            $.post('<%= Url.Content("~/Appraisal/UpdateAcv") %>', { appraisalId:<%=Model.AppraisalID %>,  acv: acv }, function (data) {
             
            });

        });

        $('#CustomerFirstName').blur(function () {
           
            var customerFirstName = $("#CustomerFirstName").val();

            $.post('<%= Url.Content("~/Appraisal/UpdateCustomerFirstName") %>', { appraisalId: <%=Model.AppraisalID %>, customerFirstName: customerFirstName }, function (data) {
              
              });

          });

          $('#CustomerLastName').blur(function () {
           
              var customerLastName = $("#CustomerLastName").val();

              $.post('<%= Url.Content("~/Appraisal/UpdateCustomerLastName") %>', { appraisalId: <%=Model.AppraisalID %>, customerLastName: customerLastName }, function (data) {
              
             });

         });

         $('#CustomerAddress').blur(function () {
       
             var customerAddress = $("#CustomerAddress").val();

             $.post('<%= Url.Content("~/Appraisal/UpdateCustomerAddress") %>', { appraisalId: <%=Model.AppraisalID %>, customerAddress: customerAddress }, function (data) {
           
             });

         });

         $('#CustomerCity').blur(function () {
        
             var customerCity = $("#CustomerCity").val();

             $.post('<%= Url.Content("~/Appraisal/UpdateCustomerCity") %>', { appraisalId: <%=Model.AppraisalID %>, customerCity: customerCity }, function (data) {
           
             });

         });

         $('#CustomerState').blur(function () {
         
             var customerState = $("#CustomerState").val();

             $.post('<%= Url.Content("~/Appraisal/UpdateCustomerState") %>', { appraisalId: <%=Model.AppraisalID %>, customerState: customerState }, function (data) {
         
             });

         });


         $('#CustomerZipCode').blur(function () {
           
             var customerZipCode = $("#CustomerZipCode").val();

             $.post('<%= Url.Content("~/Appraisal/UpdateCustomerZipCode") %>', { appraisalId: <%=Model.AppraisalID %>, customerZipCode: customerZipCode }, function (data) {
              
             });

         });

         $('#CustomerEmail').blur(function () {
             
             var customerEmail = $("#CustomerEmail").val();

             $.post('<%= Url.Content("~/Appraisal/UpdateCustomerEmail") %>', { appraisalId: <%=Model.AppraisalID %>, customerEmail: customerEmail }, function (data) {
                 
             });

         });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ddcl-trim-filter
        {
            z-index: 100;
        }

        .ui-dropdownchecklist-text
        {
            text-overflow: ellipsis;
        }

        div.ui-dropdownchecklist
        {
            height: 250px !important;
            width: 176px !important;
            left: 956px !important;
            top: 500px !important;
            color: gray !important;
            margin-left: 2px;
            padding-top: 4px;
        }

        div.ui-dropdownchecklist-dropcontainer
        {
            height: 168px;
            overflow-y: auto;
            overflow-x: hidden;
            background-color: white !important;
            border: 1px solid #cdcdcd !important;
        }

        span.ui-dropdownchecklist-selector
        {
            width: 170px !important;
        }

        .ptl_charts_right
        {
            text-align: left !important;
        }

        .grid .tick
        {
            stroke: rgba(0,0,0,.3);
        }

        .axis path,
        .axis line
        {
            fill: none;
            stroke: black;
            shape-rendering: crispEdges;
        }

        .axis text
        {
            font-family: sans-serif;
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("../Inventory/_TemplateCarFaxData")  %>
    <%=Html.Partial("../Inventory/_TemplateKarPowerData")  %>
    <%=Html.Partial("../Inventory/_TemplateManheimData")  %>
    <%=Html.Partial("../Inventory/_TemplateAuctionData")  %>
</asp:Content>
