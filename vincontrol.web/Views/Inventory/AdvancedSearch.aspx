<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AdvancedSearchViewModel>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
Advanced search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubMenu" runat="server">
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <form id="search" method="post" action="">
        <input type="hidden" id="DealershipId" name="DealershipId" />
        <input type="hidden" id="SortField" name="SortField" />
        <input type="hidden" id="PageIndex" name="PageIndex" />
        <input type="hidden" id="PageSize" name="PageSize" />
        
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <div class="inventory_fixed_title">
                    Advanced Search
                </div>
            </div>
        </div>
        <div id="container_right_content">
            <div class="advancedsearch_container">
                <div class="advancedsearch_title">
                    <label>
                        Search VinControl
                    </label>
                    <p>
                        Here you can search your entire inventory for vehicles. Using the fields below you
                    can pull a list of specific makes, models, years, cars of similar color or description
                    or with similar options, etc.
                    </p>
                    <p>
                        Please choose your options and click "Submit search" button to find your entire inventory.
                        </p>
                </div>
                <div class="advancedsearch_holder">
                    <div class="advancedsearch_ymm_holder">
                        <div class="advancedsearch_items">
                            <div class="advancedsearch_items_text">
                                Year
                            </div>
                            <div class="advancedsearch_items_input">
                                <%= Html.DropDownListFor(m => m.SelectedYear, Model.Years, "----") %>
                            </div>
                        </div>
                        <div class="advancedsearch_items">
                            <div class="advancedsearch_items_text">
                                Make
                            </div>
                            <div class="advancedsearch_items_input" id="makes">
                                <%= Html.Partial("GenerateMakes", Model) %>
                            </div>
                        </div>
                        <div class="advancedsearch_items">
                            <div class="advancedsearch_items_text">
                                Model
                            </div>
                            <div class="advancedsearch_items_input" id="models">
                                <%= Html.Partial("GenerateModels", Model) %>
                            </div>
                        </div>
                    </div>
                    <div class="advancedsearch_items_text_category">
                        <div class="advancedsearch_items">
                            <div class="advancedsearch_items_text">
                                Text
                            </div>
                            <div class="advancedsearch_items_input">
                                <%= Html.TextBoxFor(m => m.Text, new Dictionary<string, object>(){ {"class","subtext"}, {"size","12"}, {"title","Searches entire inventory for text given. Ex - typing 'green' searches inventory for any vehicles that contain the text 'green'"} } ) %>
                            </div>
                        </div>
                        <div class="advancedsearch_items">
                            <div class="advancedsearch_items_text">
                                Category
                            </div>
                            <div class="advancedsearch_items_input">
                                <%= Html.DropDownListFor(m => m.SelectedCategory, Model.Categories, new Dictionary<string, object>() { {"title","Filter based on inventory category such as Appraisal, New, Used, etc."} }) %>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="btns_shadow advancedsearch_btn" id="btnAdvancedSearch">
                    Submit search >
                </div>
                <div id="divResult">
                    <div style="float: left;">
                        <div id="aAll"></div>
                        <div style="margin-left: 10px; font-size: 12px;">
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aUsed" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aNew" class="lnkCategory"></a>
                            </div>
                            <% if (Vincontrol.Web.Handlers.SessionHandler.IsEmployee == false) %>
                            <% { %>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aLoaner" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aAuction" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aRecon" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aWholesale" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aTradeNotClear" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aSold" class="lnkCategory"></a>
                            </div>
                            <% } %>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aRecent" class="lnkCategory"></a>
                            </div>
                            <div class="itemHeader">
                                <img src="" />
                                <a id="aPending" class="lnkCategory"></a>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="right_content_nav" class="right_content_nav" style="display: none">
                    <div class="right_content_nav_items nav_middle">
                    </div>
                    <div class="right_content_nav_items nav_short" style="width: 40px!important">
                        <a href="javascript:;" id="a_Age">Age</a>
                    </div>
                    <div class="right_content_nav_items right_content_items_maket nav_long" style="width: 78px!important;cursor: none !important">
                        Rank
                    </div>
                    <div class="right_content_nav_items nav_vin_expended right_content_items_vin nav_long" style="width: 54px!important;cursor: none !important">
                        Vin
                    </div>
                    <div class="right_content_nav_items nav_long">
                        <a href="javascript:;" id="a_Stock">Stock</a>
                    </div>
                    <div class="right_content_nav_items nav_middle">
                        <a href="javascript:;" id="a_Year">Year</a>
                    </div>
                    <div class="right_content_nav_items nav_long">
                        <a href="javascript:;" id="a_Make">Make</a>
                    </div>
                    <div class="right_content_nav_items nav_long">
                        <a href="javascript:;" id="a_Model">Model</a>
                    </div>
                    <div class="right_content_nav_items nav_long">
                        <a href="javascript:;" id="a_Trim">Trim</a>
                    </div>
                    <div class="right_content_nav_items nav_middle" style="cursor: none !important">
                        Color
                    </div>
                    <div class="right_content_nav_items nav_long">
                        <a href="javascript:;" id="a_Miles">Odometer</a>
                    </div>
                    <div class="right_content_nav_items nav_long">
                        <a href="javascript:;" id="a_Price">Price</a>
                    </div>
                    <div class="right_content_nav_items right_content_items_owners nav_short" style="width: 102px!important;cursor: none !important">
                        Owners
                    </div>
                </div>
                <div class="advancedsearch_result_holder" id="contentResult">
                    <div class="vin_listVehicle_holder" id="result">
                        <div id="divLoading" class="contain_list list_even" style="text-align: center; display: none;">
                            <img src="../../Content/images/ajaxloadingindicator.gif" style="border: 0;" />
                        </div>
                    </div>
                </div>
                <div id="divNoData" style="display:none;">
                    <label class="no_data">There are no results which match your search criteria.</label>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        function updateMileage(txtBox) {
            var img = $(txtBox).parent().find('.imgLoadingAdvSearch');
            img.show();

            $.post('<%= Url.Content("~/Inventory/UpdateMileageFromInventoryPage") %>', { Mileage: txtBox.value, ListingId: txtBox.id }, function (data) {
                img.hide();
            });
        }

        function updateSalePrice(txtBox) {
            var inventoryStatus = $(txtBox).attr('inventorystatuscodeid');
            //Default Inventory
            var vehicleStatus = 2;
            //Case Sold
            if (inventoryStatus == 4) {
                vehicleStatus = 3;
            }
                //Case Appraisal
            else if (inventoryStatus == 3) {
                vehicleStatus = 1;
            }

            var img = $(txtBox).parent().find('.imgLoadingAdvSearch');
            img.show();

            $.post('<%= Url.Content("~/Inventory/UpdateSalePriceFromInventoryPage") %>', { SalePrice: txtBox.value, ListingId: txtBox.id, vehicleStatusCodeId: vehicleStatus }, function (data) {
                img.hide();
            });
        }

        var totalResult = 0;
        var loadedResult = 0;
        var pageIndex = 1;
        var pageSize = 10;
        var scrollHeight = 50;

        function updateHeaderImage(header, isActive) {
            var img = $(header).parent().find("img");

            if (isActive) {
                img.attr("src", "../../Content/images/vincontrol/AdvSearch/advActive.png");
            }
            else {
                img.attr("src", "../../Content/images/vincontrol/AdvSearch/advNormal.png");
            }
        }

        function replaceNumber(currentText, number) {
            var newText = currentText.replace(/[0-9]/g, '');
            newText = number + newText;
            return newText;
        }

        $(document).ready(function () {
            $('#Text').keypress(function (e) {
                if (e.which == 13) {
                    $('#btnAdvancedSearch').click();
                }
            });

            if ($("#IsEmployee").val() == 'True') {
                $("#divResult").removeClass("divResultAdmin");
                $("#divResult").addClass("divResultEmp");

                $("#result").removeClass("divVehicleAdmin");
                $("#result").addClass("divVehicleEmp");
            }
            else {
                $("#divResult").removeClass("divResultEmp");
                $("#divResult").addClass("divResultAdmin");

                $("#result").removeClass("divVehicleEmp");
                $("#result").addClass("divVehicleAdmin");
            }

            $("#btnAdvancedSearch").click(function () {
                blockUI();
                $("#DealershipId").val($("#SelectedDealership").val());
                //$("#SelectedCategory").val("All");
                ResetScrollHeight();
                $.ajax({
                    type: "POST",
                    url: "/Inventory/AdvancedSearchResult",
                    data: $("form").serialize(),
                    success: function (results) {
                        console.log(results.count);
                        console.log(results);
                        totalResult = results.count;
                        loadedResult = pageSize;

                        if (results.count > 0) {
                            $('#divNoData').hide();
                            $('#contentResult').show();
                            $("#result").html(results.content);
                            $('#divResult').show();
                            $('#right_content_nav').show();

                            $("a[id^='a']").each(function () {
                                $(this).removeClass('lnkCategoryActive');
                            });

                            $('#aAll').text(results.count + ' Vehicle(s) found');

                            $('#aUsed').text(results.NumberOfUsedInventory + ' Used');
                            updateHeaderImage($('#aUsed'), results.NumberOfUsedInventory != 0);
                            if (results.NumberOfUsedInventory == 0) {
                                $('#aUsed').removeClass('lnkCategory');
                                $('#aUsed').unbind('click');
                            } else {
                                $('#aUsed').addClass('lnkCategory');
                                $('#aUsed').bind('click');
                            }

                            $('#aNew').text(results.NumberOfNewInventory + ' New');
                            updateHeaderImage($('#aNew'), results.NumberOfNewInventory != 0);
                            if (results.NumberOfNewInventory == 0) {
                                $('#aNew').removeClass('lnkCategory');
                                $('#aNew').unbind('click');
                            } else {
                                $('#aNew').addClass('lnkCategory');
                                $('#aNew').bind('click');
                            }

                            $('#aLoaner').text(results.NumberOfLoanerInventory + ' Loaner');
                            updateHeaderImage($('#aLoaner'), results.NumberOfLoanerInventory != 0);
                            if (results.NumberOfLoanerInventory == 0) {
                                $('#aLoaner').removeClass('lnkCategory');
                                $('#aLoaner').unbind('click');
                            }
                            else {
                                $('#aLoaner').addClass('lnkCategory');
                                $('#aLoaner').bind('click');
                            }

                            $('#aAuction').text(results.NumberOfAuctionInventory + ' Auction');
                            updateHeaderImage($('#aAuction'), results.NumberOfAuctionInventory != 0);
                            if (results.NumberOfAuctionInventory == 0) {
                                $('#aAuction').removeClass('lnkCategory');
                                $('#aAuction').unbind('click');
                            }
                            else {
                                $('#aAuction').addClass('lnkCategory');
                                $('#aAuction').bind('click');
                            }

                            $('#aRecon').text(results.NumberOfReconInventory + ' Recon');
                            updateHeaderImage($('#aRecon'), results.NumberOfReconInventory != 0);
                            if (results.NumberOfReconInventory == 0) {
                                $('#aRecon').removeClass('lnkCategory');
                                $('#aRecon').unbind('click');

                            } else {
                                $('#aRecon').addClass('lnkCategory');
                                $('#aRecon').bind('click');
                            }

                            $('#aWholesale').text(results.NumberOfWholesaleInventory + ' Wholesale');
                            updateHeaderImage($('#aWholesale'), results.NumberOfWholesaleInventory != 0);
                            if (results.NumberOfWholesaleInventory == 0) {
                                $('#aWholesale').removeClass('lnkCategory');
                                $('#aWholesale').unbind('click');

                            } else {
                                $('#aWholesale').addClass('lnkCategory');
                                $('#aWholesale').bind('click');
                            }

                            $('#aTradeNotClear').text(results.NumberOfTradeNotClearInventory + ' Trade Not Clear');
                            updateHeaderImage($('#aTradeNotClear'), results.NumberOfTradeNotClearInventory != 0);
                            if (results.NumberOfTradeNotClearInventory == 0) {
                                $('#aTradeNotClear').removeClass('lnkCategory');
                                $('#aTradeNotClear').unbind('click');
                            }
                            else {
                                $('#aTradeNotClear').addClass('lnkCategory');
                                $('#aTradeNotClear').bind('click');
                            }

                            $('#aSold').text(results.NumberOfSoldInventory + ' Sold');
                            updateHeaderImage($('#aSold'), results.NumberOfSoldInventory != 0);
                            if (results.NumberOfSoldInventory == 0) {
                                $('#aSold').removeClass('lnkCategory');
                                $('#aSold').unbind('click');
                            }
                            else {
                                $('#aSold').addClass('lnkCategory');
                                $('#aSold').bind('click');
                            }

                            $('#aRecent').text(results.NumberOfRecentAppraisal + ' Recent appraisal');
                            updateHeaderImage($('#aRecent'), results.NumberOfRecentAppraisal != 0);
                            if (results.NumberOfRecentAppraisal == 0) {
                                $('#aRecent').removeClass('lnkCategory');
                                $('#aRecent').unbind('click');
                            }
                            else {
                                $('#aRecent').addClass('lnkCategory');
                                $('#aRecent').bind('click');
                            }

                            $('#aPending').text(results.NumberOfPendingAppraisal + ' Pending appraisal');
                            updateHeaderImage($('#aPending'), results.NumberOfPendingAppraisal != 0);
                            if (results.NumberOfPendingAppraisal == 0) {
                                $('#aPending').removeClass('lnkCategory');
                                $('#aPending').unbind('click');
                            }
                            else {
                                $('#aPending').addClass('lnkCategory');
                                $('#aPending').bind('click');
                            }

                            $("a.iframe").fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                        }
                        else {
                            $('#contentResult').hide();
                            $('#divResult').hide();
                            $('#right_content_nav').hide();
                            $('#divNoData').show();
                        }
                        unblockUI();
                    }
                });
            });

            $('#btnAdvancedSearch').trigger('click');

            $("a[id^='a']").live('click', function () {
                if ($(this).hasClass('lnkCategory')) {
                    ResetScrollHeight();
                    //var column = this.id.split("_")[1];
                    var column = this.id.substring(1, this.id.length);
                    console.log(column);
                    blockUI();
                    $("#DealershipId").val($("#SelectedDealership").val());
                    $("#SortField").val(column);
                    $("#SelectedCategory").val(column);
                    $("#SelectedCategory option[text=" + column + "]").attr("selected", "selected");
                    $("a[id^='a']").each(function () {
                        $(this).removeClass('lnkCategoryActive');
                    });
                    $(this).addClass('lnkCategoryActive');

                    var currentHeader = $(this);

                    $.ajax({
                        type: "POST",
                        url: "/Inventory/AdvancedSearchResult",
                        data: $("form").serialize(),
                        success: function (results) {
                            //$('.vin_listVehicle_holder').scrollTop(100);
                            totalResult = results.count;
                            loadedResult = pageSize;
                            $("#result").html(results.content);
                            $(currentHeader).text(replaceNumber($(currentHeader).text(), results.count));
                            unblockUI();
                        }
                    });
                }
            });

            $('.vin_listVehicle_holder').scroll(function () {
                //alert($('.vin_listVehicle_holder').scrollTop());
                if ($('.vin_listVehicle_holder').scrollTop() >= scrollHeight && loadedResult < totalResult) {
                    scrollHeight += 50;
                    $('#divLoading').show();

                    $('#PageIndex').val(++pageIndex);
                    $('#PageSize').val(pageSize);
                    var url = "/Inventory/AdvancedSearchResult";
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: $("form").serialize(),
                        success: function (result) {
                            $('#divLoading').hide();
                            $("#result").append(result.content != '' && result.content != undefined ? result.content : result);
                            loadedResult = loadedResult + pageSize;
                        },
                        error: function (err) {
                            $('#divLoading').hide();
                            console.log(err.status + " - " + err.statusText);
                        }
                    });
                }
            });

            $("#SelectedYear").live('change', function () {
                $.ajax({
                    type: "POST",
                    url: "/Inventory/GenerateMakes",
                    data: $("form").serialize(),
                    success: function (results) {
                        $("#makes").html(results);
                        if ($('#SelectedMake').val() == "")
                            $("#models").html("<select id='SelectedModel' name='SelectedModel' style='width:105px'><option value=''>----</option></select>");
                        else {
                            $.ajax({
                                type: "POST",
                                url: "/Inventory/GenerateModels",
                                data: $("form").serialize(),
                                success: function (results) {
                                    $("#models").html(results);

                                }
                            });
                        }
                    }
                });
            });

            $("#SelectedMake").live('change', function () {
                $.ajax({
                    type: "POST",
                    url: "/Inventory/GenerateModels",
                    data: $("form").serialize(),
                    success: function (results) {
                        $("#models").html(results);

                    }
                });
            });

            $('img[id^="btnCraigslist_"]').live('click', function () {
                var id = this.id.split('_')[1];
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: "/Craigslist/AuthenticationChecking",
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        if (result == 302) {
                            $.ajax({
                                type: "GET",
                                dataType: "html",
                                url: "/Craigslist/LocationChecking",
                                data: {},
                                cache: false,
                                traditional: true,
                                success: function (result) {
                                    if (result == false)
                                        ShowWarningMessage("Please choose State/City/Location in Admin section.");
                                    else
                                        var url = '/Craigslist/GoToPostingPreviewPage?listingId=' + id;
                                    $.fancybox({
                                        href: url,
                                        'width': 800,
                                        'height': 700,
                                        'hideOnOverlayClick': false,
                                        'centerOnScroll': true,
                                        'scrolling': 'yes',
                                        'onCleanup': function () {
                                        },
                                        isLoaded: function () {

                                        },
                                        onClosed: function () {

                                        }
                                    });
                                }
                            });

                        } else {
                            ShowWarningMessage("Please enter valid Email/Password in Admin section.");
                        }
                    },
                    error: function (err) {

                    }
                });
            });

        });

        function ResetScrollHeight() {
            pageIndex = 1;
            pageSize = 10;
            scrollHeight = 50;
            $('#PageIndex').val(pageIndex);
            $('#PageSize').val(pageSize);
        }

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/advancedsearch.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #aAll
        {
            font-weight: bold;
            color: #023399;
            margin-left: 10px;
            font-size: 15px;
        }

        .itemHeader
        {
            float: left;
            margin-right: 30px;
            padding-top: 5px;
        }

        .vin_listVehicle_holder
        {
            padding-left: 0px !important;
        }

        .divVehicleAdmin
        {
            height: 265px !important;
        }

        .divVehicleEmp
        {
            height: 285px !important;
        }

        #container_right_content
        {
            padding-bottom: 5px !important;
        }

        .nav_long
        {
            width: 65px !important;
        }

        #right_content_nav
        {
            margin-left: 0px !important;
        }

        #divResult
        {
            padding-top: 5px; 
            display: none; 
            border: 1px solid black;
            margin-top: 40px; 
            margin-bottom: 10px;
        }

        .divResultAdmin
        {
            height: 65px; 
        }

        .divResultEmp
        {
            height: 45px; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>