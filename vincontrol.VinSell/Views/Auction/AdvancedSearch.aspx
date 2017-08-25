<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.AdvancedSearchViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VinSell | Advanced Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <div class="page-info">
            <span><br /></span><span><br /></span>
            <h3>Advanced Search</h3>
        </div>
        <div class="filter-box">
        </div>
        <form id="advancedSearchForm" method="post" action="<%= Url.Action("AdvancedSearch", "Auction") %>">
        <input type="hidden" id="SelectedMake" name="SelectedMake" value=""/>
        <input type="hidden" id="SelectedModel" name="SelectedModel" value=""/>
        <input type="hidden" id="SelectedTrim" name="SelectedTrim" value=""/>
        <input type="hidden" id="SelectedRegion" name="SelectedRegion" value=""/>
        <input type="hidden" id="SelectedState" name="SelectedState" value=""/>
        <input type="hidden" id="SelectedAuction" name="SelectedAuction" value=""/>
        <input type="hidden" id="SelectedSeller" name="SelectedSeller" value=""/>
        <input type="hidden" id="SelectedBodyStyle" name="SelectedBodyStyle" value=""/>
        <input type="hidden" id="SelectedExteriorColor" name="SelectedExteriorColor" value=""/>
        <div class="content">
            <div class="filter">
                <div class="adv-info">
                    The advanced search feature allows you to look for vehicles the fulfill specific
                    creteria. Please select options from the dropdown menu, and then click search to
                    view the results that match your selections. You may also enter in text to further
                    filter your results.
                </div>
                <div class="text-search">
                    <b>Text Search</b>
                    <input id="Text" name="Text" type="text" style="width: 100%;" placeholder="Enter Text Here" />
                </div>
                <div class="advanced-vehicle">
                    <%--<div class="adv-wrap">
                        <span class="carfax-lable">Carfax 1-Owner</span>
                        <input type="checkbox" name="HasCarfax1Owner" id="HasCarfax1Owner" />
                    </div>--%>
                    <br />
                    <div class="adv-wrap range-input">
                        <div class="label">Year Range</div>
                        <%: Html.DropDownList("SelectedYearFrom", Model.YearsFrom.ToSelectItemList(m => m.Value, m => m.Text, false), "Year", new { style = "width:93px;" })%>
                        - to -
                        <%: Html.DropDownList("SelectedYearTo", Model.YearsTo.ToSelectItemList(m => m.Value, m => m.Text, false), "Year", new { style = "width:93px;" })%>
                    </div>
                    <div class="adv-wrap">
                        <div class="label">By Make</div>
                        <div id="divMake">
                            <%: Html.DropDownList("ddlMake", Model.Makes.ToSelectItemList(m => m.Value, m => m.Text, false), "All Makes", new { @class = "make multi-select", multiple = "multiple" })%>
                        </div>
                    </div>
                    <br />
                    <div class="adv-wrap">
                        <div class="label">By Model</div>
                        <div id="divModel">
                        <%: Html.DropDownList("ddlModel", Model.Models.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "model multi-select", multiple = "multiple" })%>
                        </div>
                    </div>
                    <div class="adv-wrap">
                        <div class="label">By Trim</div>
                        <div id="divTrim">
                        <%: Html.DropDownList("ddlTrim", Model.Trims.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "trim multi-select", multiple = "multiple" })%>
                        </div>
                    </div>
                    <br />
                    <div class="adv-wrap range-input">
                        <div class="label">Mileage Range</div>
                        <input type="text" style="width: 93px;" id="MileageFrom" name="MileageFrom"/>
                        - to -
                        <input type="text" style="width: 93px;" id="MileageTo" name="MileageTo"/>
                    </div>
                    <div class="adv-wrap">
                        <div class="label">By Condition Rating</div>
                        <div class="cr">CR</div>
                        <%: Html.DropDownList("SelectedCr", Model.Crs.ToSelectItemList(m => m.Value, m => m.Text, false), "All", new { @class = "cr-data", style = "width: 185px" })%>                        
                    </div>
                    <br />
                    <div class="adv-wrap">
                        <div class="label">By MMR Value</div>
                        <%: Html.DropDownList("SelectedMmr", Model.Mmrs.ToSelectItemList(m => m.Value, m => m.Text, false), "All", new { @class = "mmr-value", style = "width: 223px" })%>
                    </div>
                    <div class="adv-wrap">
                        <div class="label">By Body Style</div>
                        <%: Html.DropDownList("ddlBodyStyle", Model.BodyStyles.ToSelectItemList(m => m.Value, m => m.Text, false), "All Body Styles", new { @class = "bodystyle multi-select", multiple = "multiple" })%>
                    </div>
                    <br />
                    <div class="adv-wrap">
                        <div class="label">Transmission</div>
                        <%: Html.DropDownList("SelectedTransmission", Model.Transmissions.ToSelectItemList(m => m.Value, m => m.Text, false), "All", new { @class = "transmission", style = "width: 223px" })%>                        
                    </div>
                    <div class="adv-wrap">
                        <div class="label">By Exterior Color</div>
                        <%: Html.DropDownList("ddlExteriorColor", Model.ExteriorColors.ToSelectItemList(m => m.Value, m => m.Text, false), "All Exterior Colors", new { @class = "exteriorcolor multi-select", multiple = "multiple" })%>                        
                    </div>
                </div>
                <div class="advanced-auction">
                    <div class="adv-wrap">
                        <div class="label">By Region</div>
                        <%: Html.DropDownList("ddlRegion", Model.Regions.ToSelectItemList(m => m.Value, m => m.Text, false), "All Regions", new { @class = "region multi-select", multiple = "multiple" })%>
                        
                    </div>
                    <br />
                    <div class="adv-wrap">
                        <div class="label">By State</div>
                        <div id="divState">
                        <%: Html.DropDownList("ddlState", Model.States.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "state multi-select", multiple = "multiple" })%>
                        </div>
                    </div>
                    <br />
                    <div class="adv-wrap">
                        <div class="label">By Auction</div>
                        <div id="divAuction">
                        <%: Html.DropDownList("ddlAuction", Model.Auctions.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "auction multi-select", multiple = "multiple" })%>                        
                        </div>
                    </div>
                    <br />
                    <div class="adv-wrap">
                        <div class="label">By Seller</div>
                        <div id="divSeller">
                        <%: Html.DropDownList("ddlSeller", Model.Sellers.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "seller multi-select", multiple = "multiple" })%>                                                
                        </div>
                    </div>
                    <br />
                    <div class="adv-wrap range-input">
                        <div class="label">By Sale Date Range</div>
                        <input type="text" style="width: 93px;" id="SaleDateFrom" name="SaleDateFrom"/>
                        - to -
                        <input type="text" style="width: 93px;" id="SaleDateTo" name="SaleDateTo"/>
                    </div>
                </div>                
                <button id="btnSearch" class="adv-search">Search</button>
            </div>
        </div>
        </form>
        <!-- end of inner wrap div-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/advanced.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/jquery.multiselect.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/jquery.multiselect.filter.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .error { background-color: Pink; border: 1px solid Red; }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/Scripts/jquery.multiselect.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var readyToSubmit = true;
        $(document).ready(function () {
            $('#SaleDateFrom').datepicker();
            $('#SaleDateTo').datepicker();
            $("#MileageFrom").numeric({ decimal: false, negative: false }, function () { });
            $("#MileageTo").numeric({ decimal: false, negative: false }, function () { });

            $('#btnSearch').click(function () {
                Validation();
                if (readyToSubmit) {
                    $('#MileageFrom').val($('#MileageFrom').val().replace(/\,/g, ''));
                    $('#MileageTo').val($('#MileageTo').val().replace(/\,/g, ''));
                    $('#HasCarfax1Owner').val($('#HasCarfax1Owner').is(':checked') ? 'True' : 'False');

                    $('#advancedSearchForm').submit();
                }
            });

            $(function () {
                $("select.multi-select").multiselect({ header: false });

                var regions = '';
                var allregions = '';
                $("select#ddlRegion").multiselect({
                    header: false
                    , click: function (event, ui) {
                        if (ui.value == '') {
                            if (ui.checked) {
                                $(this).multiselect("checkAll");
                                $.each($(this).val(), function (key, value) {
                                    if (value != '')
                                        regions += ',' + (value);
                                });
                                allregions = regions;
                            }
                            else {
                                $(this).multiselect("uncheckAll");
                                regions = '';
                            }
                        } else {

                            if (ui.checked) {
                                regions += ',' + (ui.value);
                            }
                            else {
                                regions = regions.replace(',' + (ui.value), '');
                            }

                            var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                            if (allregions != '' && (allregions.length != regions.length))
                                emptyOption.prop('checked', false);
                            else if (allregions != '' && (allregions.length == regions.length))
                                $(this).multiselect("checkAll");
                        }

                    },
                    beforeopen: function () {

                    },
                    open: function () {

                    },
                    beforeclose: function () {

                    },
                    close: function () {
                        $("#SelectedRegion").val(regions);
                        if (regions != '') {
                            $.ajax({
                                type: "GET",
                                dataType: "html",
                                url: '/Ajax/GenerateStates?regions=' + regions,
                                data: {},
                                cache: false,
                                traditional: true,
                                success: function (result) {
                                    $('#divState').html(result);
                                    $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select" multiple = "multiple"></select>');
                                    $("select#ddlAuction").multiselect({ header: false });
                                    $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                                    $("select#ddlSeller").multiselect({ header: false });
                                },
                                error: function (err) {
                                    $('#divState').html('<select id="ddlState" name="ddlState" class="state multi-select" multiple = "multiple"></select>');
                                    $("select#ddlState").multiselect({ header: false });
                                    $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select" multiple = "multiple"></select>');
                                    $("select#ddlAuction").multiselect({ header: false });
                                    $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                                    $("select#ddlSeller").multiselect({ header: false });
                                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                                }
                            });
                        } else {
                            $('#divState').html('<select id="ddlState" name="ddlState" class="state multi-select" multiple = "multiple"></select>');
                            $("select#ddlState").multiselect({ header: false });
                            $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select" multiple = "multiple"></select>');
                            $("select#ddlAuction").multiselect({ header: false });
                            $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                            $("select#ddlSeller").multiselect({ header: false });
                        }
                    },
                    checkAll: function () {

                    },
                    uncheckAll: function () {
                        regions = '';
                        $("#SelectedRegion").val(regions);
                    },
                    optgrouptoggle: function (event, ui) {

                    }
                });

                var makes = "";
                var allmakes = "";
                $("select#ddlMake").multiselect({
                    click: function (event, ui) {
                        if (ui.value == '') {
                            if (ui.checked) {
                                $(this).multiselect("checkAll");
                                $.each($(this).val(), function (key, value) {
                                    if (value != '')
                                        makes += ',' + (value);
                                });
                                allmakes = makes;
                            }
                            else {
                                $(this).multiselect("uncheckAll");
                                makes = '';
                            }
                        } else {

                            if (ui.checked) {
                                makes += ',' + (ui.value);
                            }
                            else {
                                makes = makes.replace(',' + (ui.value), '');
                            }

                            var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                            if (allmakes != '' && (allmakes.length != makes.length))
                                emptyOption.prop('checked', false);
                            else if(allmakes != '' && (allmakes.length == makes.length))
                                $(this).multiselect("checkAll");
                        }
                    },
                    beforeopen: function () {

                    },
                    open: function () {

                    },
                    beforeclose: function () {

                    },
                    close: function () {
                        $('#SelectedMake').val(makes);
                        if (makes != '') {
                            $.ajax({
                                type: "GET",
                                dataType: "html",
                                url: '/Ajax/GenerateModels?makes=' + makes,
                                data: {},
                                cache: false,
                                traditional: true,
                                success: function (result) {
                                    $('#divModel').html(result);
                                    $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                                    $("select#ddlTrim").multiselect({ header: false });
                                },
                                error: function (err) {
                                    $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                                    $("select#ddlModel").multiselect({ header: false });
                                    $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                                    $("select#ddlTrim").multiselect({ header: false });
                                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                                }
                            });
                        } else {
                            $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                            $("select#ddlModel").multiselect({ header: false });
                            $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                            $("select#ddlTrim").multiselect({ header: false });
                        }
                    },
                    checkAll: function () {

                    },
                    uncheckAll: function () {
                        makes = '';
                        $('#SelectedMake').val(makes);
                    },
                    optgrouptoggle: function (event, ui) {

                    }
                });

                var body = '';
                var allbody = '';
                $("select#ddlBodyStyle").multiselect({
                    click: function (event, ui) {
                        if (ui.value == '') {
                            if (ui.checked) {
                                $(this).multiselect("checkAll");
                                $.each($(this).val(), function (key, value) {
                                    if (value != '')
                                        body += ',' + (value);
                                });
                                allbody = body;
                            }
                            else {
                                $(this).multiselect("uncheckAll");
                                body = '';
                            }
                        } else {

                            if (ui.checked) {
                                body += ',' + (ui.value);
                            }
                            else {
                                body = body.replace(',' + (ui.value), '');
                            }

                            var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                            if (allbody != '' && (allbody.length != body.length))
                                emptyOption.prop('checked', false);
                            else if (allbody != '' && (allbody.length != body.length))
                                $(this).multiselect("checkAll");
                        }
                    },
                    beforeopen: function () {

                    },
                    open: function () {

                    },
                    beforeclose: function () {

                    },
                    close: function () {
                        $("#SelectedBodyStyle").val(body);
                    },
                    checkAll: function () {

                    },
                    uncheckAll: function () {
                        body = '';
                        $("#SelectedBodyStyle").val(body);
                    },
                    optgrouptoggle: function (event, ui) {

                    }
                });

                var color = '';
                var allcolors = '';
                $("select#ddlExteriorColor").multiselect({
                    click: function (event, ui) {
                        if (ui.value == '') {
                            if (ui.checked) {
                                $(this).multiselect("checkAll");
                                $.each($(this).val(), function (key, value) {
                                    if (value != '')
                                        color += ',' + (value);
                                });
                                allcolors = color;
                            }
                            else {
                                $(this).multiselect("uncheckAll");
                                color = '';
                            }
                        } else {

                            if (ui.checked) {
                                color += ',' + (ui.value);
                            }
                            else {
                                color = color.replace(',' + (ui.value), '');
                            }

                            var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                            if (allcolors != '' && (allcolors.length != color.length))
                                emptyOption.prop('checked', false);
                            else if (allcolors != '' && (allcolors.length == color.length))
                                $(this).multiselect("checkAll");
                        }
                    },
                    beforeopen: function () {

                    },
                    open: function () {

                    },
                    beforeclose: function () {

                    },
                    close: function () {
                        $("#SelectedExteriorColor").val(color);
                    },
                    checkAll: function () {

                    },
                    uncheckAll: function () {
                        color = '';
                        $("#SelectedExteriorColor").val(color);
                    },
                    optgrouptoggle: function (event, ui) {

                    }
                });
            });

            $('#SelectedYearFrom').change(function () {
                YearChanging();
            });

            $('#SelectedYearTo').change(function () {
                YearChanging();
            });
        });

        function YearChanging() {
            var from = $('#SelectedYearFrom').val();
            var to = $('#SelectedYearTo').val();
            if (from == '' && to == '') {
                $('#divMake').html('<select id="ddlMake" name="ddlMake" class="make multi-select" multiple = "multiple"></select>');
                $("select#ddlMake").multiselect({ header: false });
                $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                $("select#ddlModel").multiselect({ header: false });
                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                $("select#ddlTrim").multiselect({ header: false });
            } else {
                var years = (from == '') ? to : from;
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: '/Ajax/GenerateMakes?years=' + years,
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        $('#divMake').html(result);
                        $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                        $("select#ddlModel").multiselect({ header: false });
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                        $("select#ddlTrim").multiselect({ header: false });
                    },
                    error: function (err) {
                        $('#divMake').html('<select id="ddlMake" name="ddlMake" class="make multi-select" multiple = "multiple"></select>');
                        $("select#ddlMake").multiselect({ header: false });
                        $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                        $("select#ddlModel").multiselect({ header: false });
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                        $("select#ddlTrim").multiselect({ header: false });
                        jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    }
                });
            }
        }

        function Validation() {
            readyToSubmit = true;
            $("#SaleDateFrom").removeClass("error");
            $("#SaleDateTo").removeClass("error");
            $("#MileageFrom").removeClass("error");
            $("#MileageTo").removeClass("error");
            $("#SelectedYearFrom").removeClass("error");
            
            var startDate = new Date($('#SaleDateFrom').val());
            var endDate = new Date($('#SaleDateTo').val());
            if (
            $('#SaleDateTo').val() != "" &&
            ((endDate.getFullYear() <= startDate.getFullYear() && endDate.getMonth() <= startDate.getMonth() && endDate.getDate() < startDate.getDate()))             
            ) {
                $("#SaleDateTo").addClass("error");
                readyToSubmit = false;
            }

            $('#MileageFrom').val($('#MileageFrom').val().replace(/\,/g, ''));
            $('#MileageTo').val($('#MileageTo').val().replace(/\,/g, ''));
            if ($('#MileageFrom').val() != "" && $('#MileageTo').val() != "" && (parseInt($('#MileageTo').val(), 11) < parseInt($('#MileageFrom').val(), 11))) {
                $("#MileageTo").addClass("error");
                readyToSubmit = false;
                $("#MileageFrom").numeric({ decimal: false, negative: false }, function () { });
                $("#MileageTo").numeric({ decimal: false, negative: false }, function () { });
            }

            if ($('#SelectedYearFrom').val() != "" && $('#SelectedYearTo').val() != "" && (parseInt($('#SelectedYearTo').val(), 11) < parseInt($('#SelectedYearFrom').val(), 11))) {
                $("#SelectedYearTo").addClass("error");
                readyToSubmit = false;                
            }
        }
    </script>
</asp:Content>
