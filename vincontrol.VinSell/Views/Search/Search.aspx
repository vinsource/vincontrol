<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.AdvancedSearchViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VinSell | Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <div class="page-info">
            <span><br /></span><span><br /></span>
            <h3>Advanced Search</h3>
        </div>
        <div class="filter-box">
            <form id="form" method="post">
            <input type="hidden" id="SelectedMake" name="SelectedMake" value="<%= Model.SelectedMake %>"/>
            <input type="hidden" id="SelectedModel" name="SelectedModel" value="<%= Model.SelectedModel %>"/>
            <input type="hidden" id="SelectedTrim" name="SelectedTrim" value="<%= Model.SelectedTrim %>"/>
            <input type="hidden" id="SelectedRegion" name="SelectedRegion" value="<%= Model.SelectedRegion %>"/>
            <input type="hidden" id="SelectedState" name="SelectedState" value="<%= Model.SelectedState %>"/>
            <input type="hidden" id="SelectedAuction" name="SelectedAuction" value="<%= Model.SelectedAuction %>"/>
            <input type="hidden" id="SelectedSeller" name="SelectedSeller" value="<%= Model.SelectedSeller %>"/>
            <input type="hidden" id="SelectedBodyStyle" name="SelectedBodyStyle" value="<%= Model.SelectedBodyStyle %>"/>
            <input type="hidden" id="SelectedExteriorColor" name="SelectedExteriorColor" value="<%= Model.SelectedExteriorColor %>"/>
            <div class="filter">                
                <u>Vehicle</u>&nbsp;
                <%--<input type="checkbox" name="one-owner" <%= Model.HasCarfax1Owner ? "checked" : "" %>/>1-Owner--%>
                <%: Html.DropDownList("SelectedYearFrom", Model.YearsFrom.ToSelectItemList(m => m.Value, m => m.Text, m => m.Text == Model.SelectedYearFrom.ToString(), false), "Year", new { style = "width:63px;" })%>
                        - to -
                <%: Html.DropDownList("SelectedYearTo", Model.YearsTo.ToSelectItemList(m => m.Value, m => m.Text, m => m.Text == Model.SelectedYearTo.ToString(), false), "Year", new { style = "width:63px;" })%>
                
                <span id="divMake">                    
                    <%: Html.DropDownList("ddlMake", Model.Makes.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All Makes", new { @class = "make multi-select", style = "width:120px;", multiple = "multiple" })%>                    
                </span>
                
                <span id="divModel">                    
                    <% if (!String.IsNullOrEmpty(Model.SelectedMake)) {%>
                    <%: Html.DropDownList("ddlModel", Model.Models.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All Models", new { @class = "model multi-select", style = "width:120px;", multiple = "multiple" })%>
                    <%} else {%>
                    <%: Html.DropDownList("ddlModel", Model.Models.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), new { @class = "model multi-select", style = "width:120px;", multiple = "multiple" })%>
                    <%} %>
                </span>
                                
                <span id="divTrim">     
                <% if (!String.IsNullOrEmpty(Model.SelectedModel))
                   {%>               
                    <%: Html.DropDownList("ddlTrim", Model.Trims.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All Trims", new { @class = "trim multi-select", style = "width:120px;", multiple = "multiple" })%>
                <%}
                   else {%>
                    <%: Html.DropDownList("ddlTrim", Model.Trims.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), new { @class = "trim multi-select", style = "width:120px;", multiple = "multiple" })%>
                <%} %>
                </span>
                
                <%: Html.DropDownList("SelectedMmr", Model.Mmrs.ToSelectItemList(m => m.Value, m => m.Text, m => m.Text == Model.SelectedMmr.ToString(), false), "MMR", new { style = "width:60px;" })%>
                <%: Html.DropDownList("SelectedCr", Model.Crs.ToSelectItemList(m => m.Value, m => m.Text, m => m.Text == Model.SelectedCr.ToString(), false), "CR", new { @class = "cr", style = "width:50px;" })%>
                
                <br />
                <u>Auction</u>&nbsp;
                <%: Html.DropDownList("ddlRegion", Model.Regions.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All Regions", new { @class = "region multi-select", multiple = "multiple" })%>
                
                <span id="divState">
                <% if (!String.IsNullOrEmpty(Model.SelectedRegion)) {%>
                <%: Html.DropDownList("ddlState", Model.States.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All States", new { @class = "state multi-select", multiple = "multiple" })%>
                <%} else {%>
                <%: Html.DropDownList("ddlState", Model.States.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), new { @class = "state multi-select", multiple = "multiple" })%>
                <%} %>
                </span>
                
                <span id="divAuction">   
                <% if (!String.IsNullOrEmpty(Model.SelectedState)) {%>                 
                    <%: Html.DropDownList("ddlAuction", Model.Auctions.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All Auctions", new { @class = "auction multi-select", multiple = "multiple", style = "width:120px;" })%>                        
                <%} else {%>
                    <%: Html.DropDownList("ddlAuction", Model.Auctions.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), new { @class = "auction multi-select", multiple = "multiple", style = "width:120px;" })%>                        
                <%} %>
                </span>
                
                <span id="divSeller"> 
                <% if (!String.IsNullOrEmpty(Model.SelectedAuction)) {%>                    
                    <%: Html.DropDownList("ddlSeller", Model.Sellers.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), "All Sellers", new { @class = "seller multi-select", multiple = "multiple", style = "width:120px;" })%>                                                
                <%} else {%>
                    <%: Html.DropDownList("ddlSeller", Model.Sellers.ToSelectItemList(m => m.Value, m => m.Text, m => m.Selected, false), new { @class = "seller multi-select", multiple = "multiple", style = "width:120px;" })%>                                                
                <%} %>
                </span>
                
                <input type="text" style="width: 167px; height: 15px;" placeholder="Enter Text Here" value="<%= Model.Text %>" id="Text" name="Text" />
                <button id="btnSearch" type="button">Search</button>
            </div>
            </form>
        </div>
        <div id="data">
        
        </div>
        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
<link href="<%=Url.Content("~/Content/search.css")%>" rel="stylesheet" type="text/css" />
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
        
    var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';

    var selectedMake;
    var selectedModel;
    var selectedTrim;
    var selectedRegion;
    var selectedState;
    var selectedAuction;
    var selectedSeller;
    $(document).ready(function () {
        selectedMake = $('#SelectedMake').val();
        selectedModel = $('#SelectedModel').val();
        selectedTrim = $('#SelectedTrim').val();
        selectedRegion = $('#SelectedRegion').val();
        selectedState = $('#SelectedState').val();
        selectedAuction = $('#SelectedAuction').val();
        selectedSeller = $('#SelectedSeller').val();

        Submit();
        $('#btnSearch').click(function () {
            Validation();
            if (readyToSubmit) {
                $('#HasCarfax1Owner').val($('#HasCarfax1Owner').is(':checked') ? 'True' : 'False');

                Submit();
            }
        });

        $(function () {
            $("select.multi-select").multiselect({
                header: false, 
                minWidth: 120,
                create: function () { $(this).multiselect('widget').width(120); }
            });

            var regions = selectedRegion;
            var allregions = '';
            $("select#ddlRegion").multiselect({
                header: false
                , noneSelectedText: "Region"
                , minWidth: 120
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
                            url: '/Search/GenerateStates?regions=' + regions,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divState').html(result);
                                $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select" multiple = "multiple"></select>');
                                $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                            },
                            error: function (err) {
                                $('#divState').html('<select id="ddlState" name="ddlState" class="state multi-select" multiple = "multiple"></select>');
                                $("select#ddlState").multiselect({ header: false, noneSelectedText: "State", minWidth: 120 });
                                $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select" multiple = "multiple"></select>');
                                $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divState').html('<select id="ddlState" name="ddlState" class="state multi-select" multiple = "multiple"></select>');
                        $("select#ddlState").multiselect({ header: false, noneSelectedText: "State", minWidth: 120 });
                        $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select" multiple = "multiple"></select>');
                        $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                        $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                        $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
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

            var states = selectedState;
            var allstates = '';
            $("select#ddlState").multiselect({
                header: false,
                noneSelectedText: "State",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    states += ',' + (value);
                            });
                            allstates = states;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            states = '';
                        }
                    } else {

                        if (ui.checked) {
                            states += ',' + (ui.value);
                        }
                        else {
                            states = states.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allstates != '' && (allstates.length != states.length))
                            emptyOption.prop('checked', false);
                        else if (allstates != '' && (allstates.length == states.length))
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
                    $("#SelectedState").val(states);
                    if (states != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Search/GenerateAuctions?states=' + states,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divAuction').html(result);
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                            },
                            error: function (err) {
                                $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select"></select>');
                                $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select"></select>');
                        $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                        $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                        $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    states = '';
                    $("#SelectedState").val(states);
                },
                optgrouptoggle: function (event, ui) {

                }
            });

            var auctions = selectedAuction;
            var allauctions = '';
            $("select#ddlAuction").multiselect({
                header: false,
                noneSelectedText: "Auction",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    auctions += ',' + (value);
                            });
                            allauctions = auctions;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            auctions = '';
                        }
                    } else {

                        if (ui.checked) {
                            auctions += ',' + (ui.value);
                        }
                        else {
                            auctions = auctions.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allauctions != '' && (allauctions.length != auctions.length))
                            emptyOption.prop('checked', false);
                        else if (allauctions != '' && (allauctions.length == auctions.length))
                            $(this).multiselect("checkAll");
                    }
                },
                create: function () { $(this).multiselect('widget').width(120); },
                beforeopen: function () { },
                open: function () {

                },
                beforeclose: function () {

                },
                close: function () {
                    $("#SelectedAuction").val(auctions);
                    if (auctions != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Search/GenerateSellers?auctions=' + auctions,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divSeller').html(result);
                            },
                            error: function (err) {
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                        $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    auctions = '';
                    $("#SelectedAuction").val(auctions);
                },
                optgrouptoggle: function (event, ui) {

                }
            });

            var sellers = selectedSeller;
            var allsellers = '';
            $("select#ddlSeller").multiselect({
                header: false,
                noneSelectedText: "Seller",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    sellers += ',' + (value);
                            });
                            allsellers = sellers;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            sellers = '';
                        }
                    } else {

                        if (ui.checked) {
                            sellers += ',' + (ui.value);
                        }
                        else {
                            sellers = sellers.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allsellers != '' && (allsellers.length != sellers.length))
                            emptyOption.prop('checked', false);
                        else if (allsellers != '' && (allsellers.length == sellers.length))
                            $(this).multiselect("checkAll");
                    }
                },
                create: function () { $(this).multiselect('widget').width(120); },
                beforeopen: function () {

                },
                open: function () {

                },
                beforeclose: function () {

                },
                close: function () {
                    $("#SelectedSeller").val(sellers);
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    sellers = '';
                    $("#SelectedSeller").val(sellers);
                },
                optgrouptoggle: function (event, ui) {

                }
            });

            var makes = selectedMake;
            var allmakes = '';
            $("select#ddlMake").multiselect({
                header: false
                , noneSelectedText: "Make"
                , minWidth: 120
                , click: function (event, ui) {
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
                        else if (allmakes != '' && (allmakes.length == makes.length))
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
                            url: '/Search/GenerateModels?makes=' + makes,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divModel').html(result);
                                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                                $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                            },
                            error: function (err) {
                                $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                                $("select#ddlModel").multiselect({ header: false, noneSelectedText: "Model", minWidth: 120 });
                                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                                $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                        $("select#ddlModel").multiselect({ header: false, noneSelectedText: "Model", minWidth: 120 });
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                        $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
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

            var models = selectedModel;
            var allmodels = '';
            $("select#ddlModel").multiselect({
                header: false,
                noneSelectedText: "Model",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    models += ',' + (value);
                            });
                            allmodels = models;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            models = '';
                        }
                    } else {

                        if (ui.checked) {
                            models += ',' + (ui.value);
                        }
                        else {
                            models = models.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allmodels != '' && (allmodels.length != models.length))
                            emptyOption.prop('checked', false);
                        else if (allmodels != '' && (allmodels.length == models.length))
                            $(this).multiselect("checkAll");
                    }
                },
                create: function () { $(this).multiselect('widget').width(120); },
                beforeopen: function () {

                },
                open: function () {

                },
                beforeclose: function () {

                },
                close: function () {
                    $("#SelectedModel").val(models);
                    if (models != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Search/GenerateTrims?models=' + models,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divTrim').html(result);
                            },
                            error: function (err) {
                                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                                $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                        $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    models = '';
                    $("#SelectedModel").val(models);
                },
                optgrouptoggle: function (event, ui) {

                }
            });

            var trims = selectedTrim;
            var alltrims = '';
            $("select#ddlTrim").multiselect({
                header: false,
                noneSelectedText: "Trim",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    trims += ',' + (value);
                            });
                            alltrims = trims;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            trims = '';
                        }
                    } else {

                        if (ui.checked) {
                            trims += ',' + (ui.value);
                        }
                        else {
                            trims = trims.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (alltrims != '' && (alltrims.length != trims.length))
                            emptyOption.prop('checked', false);
                        else if (alltrims != '' && (alltrims.length == trims.length))
                            $(this).multiselect("checkAll");
                    }
                },
                create: function () { $(this).multiselect('widget').width(120); },
                beforeopen: function () {

                },
                open: function () {

                },
                beforeclose: function () {

                },
                close: function () {
                    $("#SelectedTrim").val(trims);
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    trims = '';
                    $("#SelectedTrim").val(trims);
                },
                optgrouptoggle: function (event, ui) {

                }
            });

            var body = '';
            var allbody = '';
            $("select#ddlBodyStyle").multiselect({
                header: false,
                noneSelectedText: "Body",
                minWidth: 120,
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
                        else if (allbody != '' && (allbody.length == body.length))
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
                header: false,
                noneSelectedText: "Color",
                minWidth: 120,
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

    function Submit() {
        blockUI(waitingImage);
        $.ajax({
            type: "POST",
            dataType: "html",
            url: '<%= Url.Action("Search","Search") %>',
            data: $("form").serialize(),
            success: function (result) {
                $('#data').html(result);
                unblockUI();
            },
            error: function (err) {
                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                unblockUI();
            }
        });
    }

    function YearChanging() {
        var from = $('#SelectedYearFrom').val();
        var to = $('#SelectedYearTo').val();
        if (from == '' && to == '') {
            $('#divMake').html('<select id="ddlMake" name="ddlMake" class="make multi-select" multiple = "multiple"></select>');
            $("select#ddlMake").multiselect({ header: false, noneSelectedText: "Make", minWidth: 120 });
            $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select"></select>');
            $("select#ddlModel").multiselect({ header: false, noneSelectedText: "Model", minWidth: 120 });
            $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
            $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
        } else {
            var years = (from == '') ? to : from;
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Search/GenerateMakes?years=' + years,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#divMake').html(result);
                    $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select"></select>');
                    $("select#ddlModel").multiselect({ header: false, noneSelectedText: "Model", minWidth: 120 });
                    $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                    $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                },
                error: function (err) {
                    $('#divMake').html('<select id="ddlMake" name="ddlMake" class="make multi-select" multiple = "multiple"></select>');
                    $("select#ddlMake").multiselect({ header: false, noneSelectedText: "Make", minWidth: 120 });
                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                }
            });
        }
    }

    function Validation() {
        readyToSubmit = true;
        $("#SelectedYearFrom").removeClass("error");

        if ($('#SelectedYearFrom').val() != "" && $('#SelectedYearTo').val() != "" && (parseInt($('#SelectedYearTo').val(), 11) < parseInt($('#SelectedYearFrom').val(), 11))) {
            $("#SelectedYearTo").addClass("error");
            readyToSubmit = false;
        }
    }
    </script>
</asp:Content>
