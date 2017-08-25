<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.MarketFilterSource>" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<%--<link href="/js/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" media="screen">
<link href="/js/ui.dropdownchecklist.standalone.css" rel="stylesheet" type="text/css" media="screen">--%>
<link href="/Content/VinControl/chart.css" rel="stylesheet" type="text/css">
<link href="/Content/VinControl/tempMarket.css" rel="stylesheet" type="text/css">
<link href="/Content/inventory.css" rel="stylesheet" type="text/css">
<%--<link href="<%=Url.Content("~/Css/validationEngine.jquery.css")%>" rel="stylesheet" type="text/css" />--%>
<style type="text/css">
    /*######################################################################*/
    /*PRINT CSS ##############################################################*/
    /*######################################################################*/

    .centerImg
    {
        height: 25px;
    }

    .centerImgHolder
    {
        height: 25px;
        display: table;
        margin: 0px auto;
    }

    #divListContent
    {
        position: relative;
        padding-top: 28px;
    }

    #tblVehicles td:nth-child(1)
    {
        width: 10px;
    }

    #tblVehicles th:nth-child(1)
    {
        width: 35px;
    }

    #tblVehicles td:nth-child(2)
    {
        width: 30px;
    }

    #tblVehicles th:nth-child(2)
    {
        width: 30px;
    }

    #tblVehicles td:nth-child(3)
    {
        width: 25px;
    }

    #tblVehicles th:nth-child(3)
    {
        width: 40px;
    }

    #tblVehicles td:nth-child(4)
    {
        width: 50px;
    }

    #tblVehicles th:nth-child(4)
    {
        width: 50px;
    }

    #tblVehicles td:nth-child(5)
    {
        width: 40px;
    }

    #tblVehicles th:nth-child(5)
    {
        width: 65px;
    }

    #tblVehicles td:nth-child(6)
    {
        width: 30px;
    }

    #tblVehicles th:nth-child(6)
    {
        width: 30px;
    }

    #tblVehicles td:nth-child(7)
    {
        width: 30px;
    }

    #tblVehicles th:nth-child(7)
    {
        width: 75px;
    }

    #tblVehicles td:nth-child(8)
    {
        width: 60px;
    }

    #tblVehicles th:nth-child(8)
    {
        width: 70px;
    }

    #tblVehicles td:nth-child(9)
    {
        width: 40px;
    }

    #tblVehicles th:nth-child(9)
    {
        width: 60px;
    }

    #tblVehicles td:nth-child(10)
    {
        width: 40px;
    }

    #tblVehicles th:nth-child(10)
    {
        width: 60px;
    }

    #vehicle-list table
    {
        width: 785px;
        overflow: hidden;
        font-size: 1.1em;
    }

    thead tr
    {
        position: absolute;
        top: 0px;
        left: 0px;
        width: 100%;
        z-index: 0;
        padding: 3px 0px;
        background-color: gray;
    }

    #vehicle-list table tbody tr td
    {
        text-align: center;
    }

    #vehicle-list table thead tr th
    {
        text-align: center;
    }

    #vehicle-list td
    {
        padding: .3em .7em .3em .7em;
        border-bottom: 1px #bbbbbb solid;
    }

    #vehicle-list tr:nth-child(2n + 2) td
    {
        background: #cccccc;
    }

    #vehicle-list tr.highlight td
    {
        background: green;
        color: #fff;
    }
    /*#vehicle-list tr:nth-child(1) td
        {
            font-weight: bold;
            color: white;
            background: #222222;
        }*/
    #graph-title-bar
    {
        position: relative;
        height: 50px !important;
        max-height: 50px !important;
        overflow: hidden;
        width: 99%;
        display: block;
    }

        #graph-title-bar h2
        {
            display: inline-block;
            padding-bottom: 0;
            margin-bottom: 0;
        }

        #graph-title-bar a
        {
            margin-left: 20px;
            padding: .2em .5em .2em .5em;
            background: #3366cc;
            color: white;
            position: relative;
            top: -3px;
            font-size: .9em;
            cursor: pointer;
        }

            #graph-title-bar a:hover
            {
                background: #3366cc;
            }

    @media print
    {
        #result
        {
            display: none;
        }

        #printable-list
        {
            display: block;
        }
        /*#vehicle-list tr:nth-child(1) td
            {
                font-weight: bold;
                color: black;
                border-bottom: #C80000 4px solid;
            }*/
        #graph-title-bar a
        {
            display: none;
        }
    }

    .blue
    {
        color: blue;
    }

    .red
    {
        color: red;
    }

    .green
    {
        color: green;
    }

    #placeholder
    {
    }

    .market-info
    {
        background: #dddddd;
        width: 200px;
        padding-left: 20px;
        display: inline-block;
        padding-bottom: 10px;
    }

    .highlightselected td
    {
        background: #3366cc !important;
        color: white !important;
        position: relative;
        left: 2px;
        box-shadow: 2px 2px 2px #333333;
        border: none !important;
        -moz-box-shadow: 2px 2px 2px #000000;
        border: none !important;
    }

    a.pad_tab
    {
        display: inline-block;
        background: #111111;
        font-size: 0.9em /*1.1em*/;
        color: white;
        font-weight: bold;
    }

        a.pad_tab:hover
        {
            color: red;
            font-weight: bold;
            cursor: pointer;
        }

    a.pad_tab
    {
        color: white;
        text-decoration: none;
        font-weight: bold;
    }

    .ui-dropdownchecklist-selector
    {
        width: 106px !important;
        background-image: url('/Content/vincontrol/110.png') !important;
        padding: 3px 2px 2px 2px !important;
    }

    .ui-dropdownchecklist-text
    {
        width: 91px !important;
    }

    .ui-dropdownchecklist
    {
        padding-top: 3px;
    }

    .ui-dropdownchecklist-item
    {
        text-align: left;
    }

    .grid .tick
    {
        stroke: rgba(0,0,0,.3);
    }

    .axis path, .axis line
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

    .mp_v2_content_right
    {
        display: none;
    }

    .mp_v2_header
    {
        display: none;
    }

    .mp_v2_cl_header
    {
        display: none;
    }

    .mp_v2_content_holder
    {
        display: none;
    }
</style>

<div id="result" class="market_popup_holder_v2">
    <div class="market_search_holder" style="display: inline-block; width: 100%; margin-top: -5px;">
        <div class="market_search_title">Market Search</div>
        <form id="FormSearch">
            <div class="market_search_select_holder">
                <div class="market_search_select_items">
                    <div class="market_search_select_title">Year Range</div>
                    <select id="market_search_yearfrom" data-validation-placeholder="0" data-validation-engine="validate[required,funcCall[checkYearRange]]"
                        data-errormessage-value-missing="Year Range is required!">
                        <%if (Model != null && Model.Year != null)
                          {
                              foreach (var item in Model.Year)
                              {%>
                        <option value="<%=item.Value %>" <%=(item.Value == Model.SeletedYearFrom) ? "selected" : "" %>><%=item.Text %></option>
                        <% }
                            } %>
                    </select>
                </div>
                <div class="market_search_text_separated">to</div>
                <div class="market_search_select_items">
                    <select id="market_search_yearto" data-validation-placeholder="0" data-validation-engine="validate[required,funcCall[checkYearRange]]"
                        data-errormessage-value-missing="Year Range is required!">
                        <%if (Model != null && Model.Year != null)
                          {
                              foreach (var item in Model.Year)
                              {%>
                        <option value="<%=item.Value %>" <%=(item.Value == Model.SeletedYearTo) ? "selected" : "" %>><%=item.Text %></option>
                        <% }
                            } %>
                    </select>

                </div>
                <div class="market_search_select_items">
                    <div class="market_search_select_title">Make</div>
                    <div id="divMake">
                        <select id="market_search_make" data-validation-placeholder="0" data-validation-engine="validate[funcCall[checkSelectedMake]]">
                            <% if (Model != null && Model.Makes != null)
                               {
                                   foreach (var item in Model.Makes)
                                   { %>
                            <option value="<%= item.Value %>" <%= (item.Value == Model.SelectedMakeId) ? "selected" : "" %>><%= item.Text %></option>
                            <% }
                                    }
                               else
                               { %>
                            <option value="-1">---</option>
                            <% } %>
                        </select>
                    </div>
                </div>
                <div class="market_search_select_items">
                    <div class="market_search_select_title">Model</div>
                    <div id="divModel">
                        <select id="market_search_model" data-validation-placeholder="0" data-validation-engine="validate[funcCall[checkSelectedModel]]">
                            <% if (Model != null && Model.Models != null)
                               {
                                   foreach (var item in Model.Models)
                                   { %>
                            <option value="<%= item.Value %>" <%= (item.Value == Model.SeletedModelId) ? "selected" : "" %>><%= item.Text %></option>
                            <% }
                                    }
                               else
                               { %>
                            <option value="-1">---</option>
                            <% } %>
                        </select>
                    </div>
                </div>
                <div class="market_search_select_items" id="searchTrimHolder" style="display: none">
                    <div class="market_search_select_title">Trim</div>
                    <div id="divTrim">
                        <select id="market_search_trim" data-validation-placeholder="0" data-validation-engine="validate[funcCall[checkSelectedTrim]]">
                            <option value="-1">---</option>
                        </select>
                    </div>
                </div>
                <div class="market_search_select_items" style="display: none">
                    <div class="market_search_select_title">Options</div>
                    <select id="market_search_options">
                        <option>---</option>
                    </select>
                </div>
                <div class="market_search_btn" id="btnGlobalMarketSearch">Search</div>
            </div>
        </form>
    </div>
    <div class="mp_v2_header">
        <div class="mp_v2_header">
            <div class="mp_v2_hl_nummeric" id="rangeNav">
                <span class="mp_v2_hl_items selected" id="100">100 </span><span class="mp_v2_hl_items" id="250">250 </span><span class="mp_v2_hl_items" id="500">500 </span><span class="mp_v2_hl_items" id="nation">Nation </span><%--<span class="mp_v2_hl_items" id="viewGoogleMapLink" onclick="ViewMap();">Map</span> <span class="mp_v2_hl_items" id="viewLinkChart" style="display: none" onclick="ViewChart();">Chart</span>--%>
                <div class="mp_v2_hl_items" id="divMore">
                    <a class="iframe iframeMore" href="/Chart/MoreDistance" id="moredistance" style="position: relative">...</a>
                </div>

            </div>
            <div class="market_chart_list_chart">
                <div class="market_chart_list_v2 market_list_chart_active">
                    List
                </div>

                <div class="market_chart_chart_v2">
                    Chart
                </div>
                <div class="market_chart_map_v2" onclick="ViewMap();">
                    Map
                </div>
            </div>

        </div>

    </div>
</div>

<div class="mp_v2_content_holder">
    <div class="market_chart_container">
        <div class="mp_v2_content_left">
            <div class="mp_v2_cl_header">
                <div class="mp_v2_cl_items mp_v2_cl_trim">
                    <label>
                        Trims</label>
                    <select id="trim-filter" multiple="" style="height: 20px; display: none;">
                    </select>

                </div>
                <div class="mp_v2_cl_items mp_v2_cl_trim">
                    <label>Body Type</label>
                    <select id="style-filter" multiple="multiple" style="height: 20px;">
                        <option>Any Style</option>
                    </select>

                </div>
                <div class="mp_v2_cl_items mp_v2_cl_fi">
                    <div class="mp_v2_cl_fi_all_new" style="background-color: red!important; color: white!important">
                        <input type="radio" id="radioAll" checked="checked" name="webSource" key="all" class="tab_radio_All_car" /><label
                            for="radioAll">All</label>
                    </div>
                    <div class="mp_v2_cl_fi_carscom mp_v2_cl_btns" style="background-color: blue!important; color: white!important">
                        <input type="radio" id="radioCarsCom" name="webSource" key="carscom" class="tab_radio_car" /><label
                            for="radioCarsCom"><%--<img src="/Content/images/carscom.png" height="18px" />--%>Carscom</label>
                    </div>
                    <div class="mp_v2_cl_fi_at mp_v2_cl_btns" style="background-color: black!important; color: white!important">
                        <input type="radio" id="radioAutoTrader" name="webSource" key="autotrader" class="tab_radio_car" /><label
                            for="radioAutoTrader"><%--<img src="/Content/images/autotrader.png" height="18px" />--%>Autotrader</label>
                    </div>
                    <div class="mp_v2_cl_fi_at mp_v2_cl_btns" style="background-color: whitesmoke!important; color: gray!important">
                        <input type="radio" id="radioCarMax" name="webSource" key="carmax" class="tab_radio_car" /><label
                            for="radioCarMax"><%--<img src="/Content/images/carmax-logo.png" height="18px" />--%>Carmax</label>
                    </div>
                    <div class="mp_v2_cl_fi_at mp_v2_cl_btns" style="background-color: green!important; color: white!important; width: 100px; padding-right: 15px;">
                        <input type="radio" id="radioCommercialTruck" name="webSource" key="commercialtruck" class="tab_radio_car" /><label
                            for="radioCommercialTruck">CommercialTruck</label>
                    </div>
                </div>
                <div style="float: left; margin-top: 14px; width: 50px;">
                    <div class="mp_v2_cl_fi_at mp_v2_cl_btns" style="background-color: #039!important; color: white!important; width: 40px; padding-left: 3px;">
                        <input type="radio" id="radioFilter" class="tab_radio_car" /><label
                            for="radioFilter">Filter</label>
                    </div>
                    <div id="extraFilter" style="position: absolute; width: 230px; background-color: #039!important; padding-bottom: 5px; z-index: 1000; display: none;">
                        <div class="mp_v2_cl_items mp_v2_cl_fi">
                            <div class="mp_v2_cl_fi_all_new">
                                <input type="radio" id="all" checked="checked" name="dealertype" key="all" class="tab_radio_All" /><label
                                    for="all">All</label>
                            </div>
                            <div class="mp_v2_cl_itemsmp_v2_cl_fi_franchise mp_v2_cl_btns">
                                <input type="radio" id="franchise" name="dealertype" key="franchise" class="tab_radio" /><label
                                    for="franchise">Franchise</label>
                            </div>
                            <div class="mp_v2_cl_fi_independant mp_v2_cl_btns">
                                <input type="radio" id="independant" name="dealertype" key="independant" class="tab_radio" /><label
                                    for="independant">independent</label>
                            </div>
                        </div>
                        <div class="mp_v2_cl_items mp_v2_cl_fi">
                            <div class="mp_v2_cl_fi_all_new">
                                <input type="radio" id="rdbAllCertified" checked="checked" name="CertifiedType" key="allCertified" class="tab_radio_All" /><label
                                    for="rdbAllCertified">All</label>
                            </div>
                            <div class="mp_v2_cl_itemsmp_v2_cl_fi_franchise mp_v2_cl_btns">
                                <input type="radio" id="rdbCertified" name="CertifiedType" key="Certified" class="tab_radio" /><label
                                    for="rdbCertified">Certified</label>
                            </div>
                            <div class="mp_v2_cl_fi_independant mp_v2_cl_btns">
                                <input type="radio" id="rdbUnCertified" name="CertifiedType" key="UnCertified" class="tab_radio" /><label
                                    for="rdbUnCertified">UnCertified</label>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div style="clear: both"></div>
            <div class="mp_v2_scroll_holder">
                <div id="divMap" style="display: none;">
                    <div id="google-maps" style="height: 450px; width: 99%">
                    </div>
                </div>
                <div class="mp_v2_chart_holder" id="divChart" style="width: 100%; display: none">
                    <div id="graphWrap" style="width: 785px; height: 500px; background-image: none;">
                        <div id="placeholder" style="height: 100%; width: 100%;">
                        </div>
                    </div>
                </div>
                <div class="market_list_container" id="divListContent">
                    <div id="vehicle-list" style="font-size: .6em; overflow-y: auto; height: 466px; padding: 5px;">
                        <table id="tblVehicles" cellspacing="0" style="display: none;">
                            <thead style="background-color: gray; color: #fff; height: 20px; cursor: pointer;">
                                <tr>
                                    <th>#
                                    </th>
                                    <th align="center">Year
                                    </th>
                                    <th align="center">Make
                                    </th>
                                    <th align="left">Model
                                    </th>
                                    <th align="left">Trim
                                    </th>
                                    <th align="left">Body
                                    </th>
                                    <th align="left">Dist
                                    </th>
                                    <th>Seller
                                    </th>
                                    <th align="center">Miles
                                    </th>
                                    <th align="center">Price
                                    </th>
                                    <th align="center">Age
                                    </th>
                                    <th>
                                        <img src="/Content/images/carscom.png" height="18px" />
                                    </th>
                                    <th>
                                        <img src="/Content/images/autotrader.png" height="18px" />
                                    </th>
                                    <th>
                                        <img src="/Content/images/carfax.png" height="18px" />
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div id="NoContent" style="width: 785px; height: 500px; background-image: url('/images/No-Data-big.jpg'); display: none">
            </div>
        </div>
        <div class="mp_v2_content_right">
            <div class="mp_v2_right_header">
                Selected Vehicle
                <input id="ListingId" name="ListingId" type="hidden" value="103633">
            </div>
            <div class="mp_v2_right_carinfo" id="divNoCarInfo" style="padding: 15px; text-align: center">
                No Vehicle selected
            </div>
            <div class="mp_v2_right_carinfo" id="carInfo" style="display: none">
                <div class="mp_v2_rc_ymm">
                    <span id="car"></span>
                    <br />
                    <span>Age:
                            <label id="lblAge"></label>
                    </span>
                </div>
                <div class="mp_v2_rc_img">
                    <span id="car-thumb"></span>
                    <br />
                    <div class="centerImg">
                        <div class="centerImgHolder">
                            <div id="CarsComLink" style="float: left;"></div>
                            <div id="AutoTraderLink" style="float: left; padding-left: 10px"></div>
                            <div id="CarmaxLink" style="float: left; padding-left: 10px"></div>
                            <div id="CommercialTruckLink" style="float: left; padding-left: 10px"></div>
                        </div>
                    </div>
                </div>
                <div class="mp_v2_rc_comparison_holder" style="clear: both">
                    <div class="mp_v2_cp_items" style="font-size: 14px; padding-left: 25px;">
                        <label style="font-weight: bold">
                            Vin:</label>
                        <span id="sp_vin"></span>
                    </div>
                    <div class="mp_v2_cp_items">
                        <div class="mp_v2_cp_items_left">
                            <label class="mpv2_cp_items_title">
                                Miles</label>
                            <label class="mpv2_cp_items_value">
                                <span id="miles"></span>
                            </label>
                        </div>
                        <div class="mp_v2_cp_items_left">
                        </div>
                    </div>
                    <div class="mp_v2_cp_items">
                        <div class="mp_v2_cp_items_left">
                            <label class="mpv2_cp_items_title">
                                Price</label>
                            <label class="mpv2_cp_items_value">
                                $<span id="price"></span></label>
                        </div>
                        <div class="mp_v2_cp_items_left">
                        </div>
                    </div>
                </div>
                <div class="mpv2_rc_dealer_info">
                    <label class="mpv2_rc_dealer_name">
                        <span id="seller"></span>
                    </label>
                    <div class="mpv2_rc_dealer_address">
                        <span id="address"></span>
                    </div>
                </div>
                <div class="mpv2_rc_distance">
                    Distance - <span id="distance"></span>
                </div>
            </div>

            <div class="mpv2_rc_market_info" style="display: none">
                <div class="mpv2_rc_mi_items">
                    <div class="mpv2_rcmi_items_left">
                        <label style="padding-left: 14px;">
                            <img src="/Content/images/vincontrol/red.png">
                            <label style="padding-left: 9px;">
                                All</label>
                        </label>
                    </div>
                </div>
                <div class="mpv2_rc_mi_items">
                    <div class="mpv2_rcmi_items_left">
                        <label style="padding-left: 14px;">
                            <img src="/Content/images/vincontrol/black.png">
                            <label style="padding-left: 9px;">
                                Auto Trader</label>
                        </label>
                    </div>
                </div>
                <div class="mpv2_rc_mi_items">
                    <div class="mpv2_rcmi_items_left">
                        <label style="padding-left: 14px;">
                            <img src="/Content/images/vincontrol/blue.png">
                            <label style="padding-left: 9px;">
                                CarsCom</label>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>

<div id="divNoContent" style="width: 600px; margin: 0 auto; padding: 10px; padding-top: 200px;">
    <div>
        Please choose your options and click "Search" button to see Market Chart.
    </div>
    <div style="padding: 30px 0px 0px 80px;">
        <img alt="" src="/Content/images/vincontrol/FadeLogo.png" />
    </div>
</div>

<input type="hidden" id="hdIsList" value="1" />
<input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= Session["IsEmployee"] %>" />
<input type="hidden" id="IsCertified" name="IsCertified" value="" />

<script src="<%=Url.Content("~/Scripts/jquery-1.8.3.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Google/jsapi.js")%>" type="text/javascript"></script>
<script src="/js/extension.js" type="text/javascript"></script>
<script src="/js/excanvas.compiled.js" type="text/javascript"></script>
<script src="/js/Chart/GridView.js" type="text/javascript"></script>
<script src="/js/Chart/SideBar.js" type="text/javascript"></script>
<script src="/js/Chart/jquery.flot.functions.js" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/VinControl/NewMarket.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jsrender.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/MarketMapping.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/VINGoogleMap.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/google_map_graph_plotter.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/LoadFilterAndEvents.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/MarketSearchInit.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/MatchTrim.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Google/markerclusterer_compiled.js")%>" type="text/javascript"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/d3/d3.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.d3.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/return-large.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/func.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/chart.js")%>"></script>

<script type="text/javascript">
    var dealerLongtitude = <%=SessionHandler.Dealer.Longtitude%>;
    var dealerLatitude = <%=SessionHandler.Dealer.Latitude%>;
    var _defaultTrims = [];
    var filterList;
    $(document).ready(function () {
        $('.mp_v2_content_right').hide();
        $('.mp_v2_cl_header').hide();
        $('.mp_v2_content_holder').hide();
        $(".market_chart_chart_v2").click(function () {
            $.blockUI({ message: '<div><img src="' + loadingImg + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
            $('#divChart').show();
            $('#divMap').hide();
            $(".market_list_container").hide();
            //$(".mp_v2_scroll_holder").show();
            $(this).addClass("market_list_chart_active");
            $(".market_chart_list_v2").removeClass("market_list_chart_active");
            $(".market_chart_map_v2").removeClass("market_list_chart_active");
            $('#hdIsList').val(0);
                
            $('#carInfo').hide();
            $('#divNoCarInfo').show();
            console.log("set seletect point");
            resetAndSetSelectedPoint(ChartInfo.selectedId);
            $.unblockUI();
        });
        $(".market_chart_list_v2").click(function () {
            $.blockUI({ message: '<div><img src="' + loadingImg + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
            $('#divChart').hide();
            $('#divMap').hide();
            $(".market_list_container").show();
            //$(".mp_v2_scroll_holder").hide();
            $(this).addClass("market_list_chart_active");
            $(".market_chart_chart_v2").removeClass("market_list_chart_active");
            $(".market_chart_map_v2").removeClass("market_list_chart_active");
            $('#hdIsList').val(1);
               
                
            $('#carInfo').hide();
            $('#divNoCarInfo').show();

            var chartGridView = new VINControl.Chart.GridView();

            if(filterList==null)
                chartGridView.pop_list($data, null,ChartInfo.selectedId);
            else
                chartGridView.pop_list(filterList, null,ChartInfo.selectedId);
            $.unblockUI();
                
        });
    });
</script>
<script type="text/javascript">
    var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';
    var map;
    var zoomLevel;
    var markers;
    var markerCluster;
    function ViewMap() {
        $.blockUI({ message: '<div><img src="' + loadingImg + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
        $('#divMap').show();
        $('#divChart').hide();
            
        $(".market_list_container").hide();
        $(".mp_v2_content_left").show();
        $(".market_chart_map_v2").addClass("market_list_chart_active");
        $(".market_chart_chart_v2").removeClass("market_list_chart_active");
        $(".market_chart_list_v2").removeClass("market_list_chart_active");
        $('#hdIsList').val(0);
            
        $('#carInfo').hide();
        $('#divNoCarInfo').show();
        //$('#viewLinkChart').show();
        //$('#viewGoogleMapLink').hide();
            
        if (typeof (VINControl) != "undefined") {
            $('#google-maps').show();
            var zoomLevel = VINControl.GoogleMap.GetZoomLevel(dealerLongtitude, dealerLatitude);

            if (typeof google !== 'undefined' && typeof google.maps !== 'undefined') {
                var map = new google.maps.Map(document.getElementById('google-maps'), {
                    zoom: zoomLevel.zoom,
                    center: new google.maps.LatLng(zoomLevel.latitude, zoomLevel.longtitude),
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                });

                var objconvertHelper = new VINControl.GoogleMap.ConvertHelper(map);
                var markers = objconvertHelper.convertToMarkerPoints($currentFilterredList);
                    
                var markerCluster = new MarkerClusterer(map, markers);
                //            google.maps.event.trigger(map, 'resize');
                google.maps.event.addListener(markerCluster, 'clusterclick', function (cluster) {
                    var testMarker = cluster.getMarkers();
                    // your code here
                    pop_smalllist(testMarker);
                });


            }
        }
        $.unblockUI();
    }

    function RedrawChartNew(fRangeFrom, fRangeTo)
    {
        $.blockUI({ message: '<div><div style="display:inline-block;width:auto;text-align:center;"><img src="' + '/images/ajaxloadingindicator.gif' + '" style="display:inline-block;width:100%;text-align:center;"/></div></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        obj = drawChart($data,  {min:fRangeFrom, max:fRangeTo }, ChartInfo.$filter, $dCar, expand, default_trim, dataTrims);
        $('#divMore a').text(fRangeFrom + ' - ' + fRangeTo);
        $.unblockUI();
    }

    function MoreClick()
    {
        $('#rangeNav span').each(function(){
            $(this).removeClass('selected');
        });
        $('#divMore').addClass('mp_v2_hl_items_active');
        $("#moreImage").attr("src","/Content/images/vincontrol/Chart/dot_on.png");
    }

    var isEmployee = ($("#IsEmployee").val());
    // ############################### //
    // Chart Data Set and Draw Section //
    // ############################### //
    function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
    // check url for GET
    // check url for GET
    var window_url = window.location.search;

    // default graph to unexpanded draw size
    var expand = true;

    // set chart dimensions
    if (expand) {
        chart_dimensions = ["785px", "500px"];
        $('#expand').css('display', 'none');
    } else {
        chart_dimensions = ["300px", "143px"];
    }

    // grab graph div element and click element
    var gwrap = $('#graphWrap');
    gwrap.css('width', chart_dimensions[0]);
    gwrap.css('height', chart_dimensions[1]);

    // load default options & trims
    var default_option;
    var default_trim;
    var default_bodystyle =  [0];

    var ListingId = $('#ListingId').val();

    // create ajax post url
    var UrlPaths = UrlPaths || { requestNationwideUrl: "" };
    UrlPaths.requestNationwideUrl = '<%= Url.Action("GetMarketDataByListingNationwideWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        var requestsoldInfoUrl='';
        var updateSalePrice = '<%= Url.Content("~/Inventory/UpdateSalePrice") %>';
        var logOff = '<%= Url.Action("LogOff", "Account" ) %>';
    var detailUrl = '<%= Url.Action("ViewIProfile", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';
    var waitingImage = '<%= Url.Content("~/images/ajaxloadingindicator.gif") %>';
    var chartType = <%=Constanst.VehicleStatus.Inventory %>;

    // create filter
    var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange:{min:0,max:100}, isSoldView:false, isSmallChart:false };
    ChartInfo.$filter = {};
    var dataTrims = null;
      
    // get chart range
    //var fRange = {min:0,max:100}; // default
    //var fRange = 100; // default

    var $dCar = {};
    var $selectedCar = {};
    // set y change check
    var newY = false;

    // create current filterred list of car
    var $currentFilterredList = [];

    $(document).ready(function(e) {
        $("a.iframeCustomerInfo").fancybox({ 'width': 330, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        $("a.iframeStatus").fancybox({ 'margin': 0, 'padding': 0, 'width': 500, 'height': 260, 'hideOnOverlayClick': false, 'centerOnScroll': true });
            
        $("a.iframeCommon").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeTransfer").fancybox({ 'width': 350, 'height': 257, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeMarkSold").fancybox({ 'width': 455, 'height': 306, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeWholeSale").fancybox({ 'width': 360, 'height': 127, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeTrackingPrice").fancybox({ 'width': 870, 'height': 509, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeBucketJump").fancybox({ 'width': 929, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeCustomerInfo").fancybox({ 'width': 455, 'height': 230, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeMore").fancybox({ 'width': 370, 'height': 100, 'hideOnOverlayClick': false, 'centerOnScroll': true });
          
    });
</script>
