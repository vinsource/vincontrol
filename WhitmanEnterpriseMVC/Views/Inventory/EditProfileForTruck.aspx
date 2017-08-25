<%@ Page Title="" MasterPageFile="~/Views/Shared/Site.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.CarInfoFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=Model.ModelYear %>
        <%=Model.Make %>
        <%=Model.Model %></title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.js" type="text/javascript" language="javascript"></script>
    <!-- styles needed by jScrollPane -->
    <link type="text/css" href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>"
        rel="stylesheet" media="all" />
    <!-- latest jQuery direct from google's CDN -->
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <!-- Fancybox Plugin -->
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
           <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <style type="text/css">
        #c2
        {
            width: 785px;
            border-right: none;
            overflow: hidden;
            height: 100%;
        }
        h4
        {
            margin-bottom: 0;
            margin-top: 0;
        }
        select
        {
            width: 135px;
        }
        p
        {
            padding: 1em;
            border-bottom: 1px solid #101010;
            border-top: 1px #777 solid;
            margin: 0;
        }
        p.top
        {
            border-top: none;
            padding: 0;
            margin-top: .5em;
        }
        p.bot
        {
            border-bottom: none;
            padding: 0;
        }
        p
        {
            margin-top: 0;
        }
        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }
        #noteList
        {
            padding: 1em;
        }
        #submit
        {
            float: right;
        }
        #optionals
        {
            width: 410px;
            margin-top: 0;
            margin-bottom: 0;
            overflow: hidden;
        }
        #warranty-info, #pricing
        {
            width: 206px;
            padding-left: 0;
        }
        #warranty-info .column, #pricing .column
        {
            padding-left: 0;
        }
        #c2
        {
            padding-top: 0;
        }
        .hider
        {
            display: none;
        }
        input[name="options"]
        {
            margin-bottom: 0;
            margin-left: 0;
        }
        textarea[name="description"]
        {
            width: 420px;
        }
        #optionals ul
        {
            margin-top: .3em;
        }
        .scroll-pane
        {
            height: 100%;
            overflow: auto;
            overflow-x: hidden;
        }
        #notes
        {
            height: 200px;
            margin-bottom: 6em;
        }
        input[name="completeApp"]
        {
            float: right;
        }
        div.scrollable-list
        {
            max-height: 150px;
            overflow: hidden;
            overflow-y: scroll;
            background: #111111;
        }
        input.small
        {
            width: 75px !important;
        }
        #column-two
        {
            height: 510px;
            width: 455px;
        }
        #c2 h3, #c2 h5
        {
            margin: 0;
            padding: 0;
        }
        #c2 h5
        {
            margin-bottom: 20px;
        }
        #pricing td.label
        {
            display: block;
            width: 140px !important;
        }
        input#edit-images
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 96%;
        }
        input#btndescription
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 100%;
        }
        input#btnauctiondescription
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 100%;
        }
           input#btnLoanerDescription
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 100%;
        }
        input#btnGenerateAutoDescription
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 100%;
        }
        #description-box
        {
            width: 48%;
            padding: 0;
            padding-top: 15px;
        }
        #other
        {
            background: #333333;
            overflow: hidden;
            width: 726px !important;
            position: relative;
            padding: 0;
            margin-left: 10px;
            padding-bottom: 5px;
        }
        #images
        {
            width: 48%;
            margin-left: 2%;
            padding: 0;
            padding-top: 15px;
        }
        #img-title
        {
            margin-left: 15px;
        }
        #images img
        {
            margin-left: 1%;
            margin-top: 3px;
            padding: 0;
            display: inline-block;
            width: 5%;
        }
        #description-input
        {
            width: 98%;
            height: 100px;
            resize: none;
            margin-left: 1px;
        }
        #fancybox-content
        {
            background: #111111;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function changeMSRP(checkbox) {

            if (checkbox.checked) {

                var optionPrice = checkbox.value;
                optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
                var total = Number(number1) + Number(number2);

            }
            else {

                var optionPrice = checkbox.value;
                optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
                var total = Number(number1) - Number(number2);
            }


            $("#MSRP").val(formatDollar(total));



        }
    </script>
   <% Html.BeginForm("Action", "Inventory", FormMethod.Post, new { id = "editiProfileForm", name = "editiProfileForm", onsubmit = "return validateForm()" }); %>
    <input type="hidden" id="EnableAutoDescriptionSetting" name="EnableAutoDescriptionSetting" value="<%= Model.EnableAutoDescriptionSetting %>" />
    <div class="column" style="font-size: 0.9em; margin:0; padding:0.5em 0;">
         <h3>
              <%=Html.DynamicHtmlLabel("lblMarketTitle", "MarketRangeEditProfile")%>
            <%=Html.DynamicHtmlLabel("lblTitleWithoutTrim", "TitleWithoutTrim")%>
           
        </h3>
        <h5>
            <%=Model.Trim%></h5>
        <table>
            <tr>
                <td>
                    VIN
                </td>
                <td>
                  <% if (Model.VinDecodeSuccess) {%>
                        <%= Html.TextBoxFor(x => x.Vin, new { @class = "z-index", @readonly = "readonly" })%>
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.Vin, new { @class = "z-index" })%>
                    <%}%>
                </td>
            </tr>
            <tr>
                <td>
                    Stock #
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.StockNumber, new { @class = "small" })%>
                </td>
            </tr>
            <tr>
                <td>
                     Date in Stock
                </td>
                <td>
              <%= Html.TextBoxFor(x => x.DateInStock, new { @class = "z-index", @readonly = "readonly" })%>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Year
                </td>
                <td>
                     <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "small", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "small" })%>
                    <%}%>
                </td>
            </tr>
            <tr>
                <td>
                    Make
                </td>
                <td>
                        <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.Make, new { @class = "z-index", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.Make, new { @class = "z-index" })%>
                    <%}%>
                </td>
            </tr>
            <tr>
                <td>
                    Model
                </td>
                <td>
                      <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.VehicleModel, new { @class = "z-index", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.VehicleModel, new { @class = "z-index" })%>
                    <%}%>
                </td>
            </tr>
            <tr>
                <td>
                    Trim
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedTrimItem, Model.EditTrimList, new { @class = "z-index", id = "SelectedTrim" })%>
                    <%-- <td><%= Html.TextBoxFor(x => x.Trim, new { @class = "z-index" })%></td>--%>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusTrim, new { @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Exterior Color
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ChromeExteriorColorList, new { id = "SelectedExteriorColor" })%>
                     <%=Html.HiddenFor(x => x.SelectedExteriorColorValue, Model.SelectedExteriorColorValue)%>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusExteriorColor, new { @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Interior Color
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.ChromeInteriorColorList) %>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusInteriorColor, new {  @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Vehicle Type
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedVehicleType, Model.VehicleTypeList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Odometer
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Mileage, new { @class = "small" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Cylinders
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Cylinder, new { @class = "small" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Liters
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Litters, new { @class = "small" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Transmission
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedTranmission, Model.ChromeTranmissionList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Doors
                </td>
                
                  <td>
                       <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.Door, new { @class = "small", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.Door, new { @class = "small" })%>
                    <%}%>
                </td>
            </tr>
            <tr>
                <td>
                    Style
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.BodyType, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Fuel
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Fuel, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Drive
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedDriveTrain, Model.ChromeDriveTrainList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Truck Type
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedTruckType, Model.TruckTypeList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Truck Class
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedTruckClass, Model.TruckClassList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Truck Category
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedTruckCategory, Model.TruckCategoryList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Original MSRP
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.MSRP, new { @class = "z-index", @readonly = "readonly" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Additional Title Info
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Title, new { @class = "z-index" })%>
                </td>
            </tr>
             <input type="hidden" id="hdnFirstSelectedTrim"/>
            <input type="hidden" id="hdnFistSelectedOptions"/>
            <input type="hidden" id="hdnFistSelectedPackages"/>
            <%=Html.HiddenFor(x=>x.ListingId) %>
            <%=Html.HiddenFor(x=>x.AfterSelectedOptions) %>
            <%=Html.HiddenFor(x=>x.AfterSelectedPackage) %>
        </table>
    </div>
    <div class="column" id="column-two" style="font-size: 0.9em;">
      
            <input class="pad" type="submit" name="CancelIProfile" value=" Cancel " id="CancelIProfile" />
             <% if ((bool) Session["IsEmployee"] == false)
                             { %>
            <input class="pad" type="submit" name="SaveITruckProfile" value=" Save Changes "
                id="SaveITruckProfile" />
                 <% } %>
            <a class="iframe" href="/Inventory/OpenSilverlightUploadWindow?ListingId=<%=Model.ListingId %>">
                <input class="pad" type="button" name="ManageImages" value="Manage Images" id="ManageImages" />
            </a>
        
            <a class="iframe" href="<%= Url.Content("~/Inventory/GetImageLinks") %>/<%=Model.ListingId %>">
                <input class="pad" type="button" name="ShowPopupImages" value="Show Images" id="ShowPopupImages" />
            </a>
      
        <br />
        <br />
        Packages:<br />
        <div class="scrollable-list">
            <% if (Model.ChromeFactoryPackageOptions.Any())
               { %>
            <%= Html.CheckBoxGroupPackage("txtFactoryPackageOption", Model.ChromeFactoryPackageOptions, Model.ExistPackages) %>
            <% }
               else
               { %>
            <%= Html.CheckBoxGroupPackageByYear("txtFactoryPackageOption")%>
            <% } %>
         
        </div>
        <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
        <div class="clear">
            Options:
        </div>
        <div class="scrollable-list">
            <div id="optionals" class="" style="font-size: .9em;">
                <div class="clear">
                </div>
                <% if (Model.ChromeFactoryNonInstalledOptions.Any())
                   { %>
                <%= Html.CheckBoxGroupOption("txtNonInstalledOption", Model.ChromeFactoryNonInstalledOptions,Model.ExistOptions)%>
                <% }
                   else
                   { %>
                <%= Html.CheckBoxGroupOptionByYear("txtNonInstalledOption")%>
                <% } %>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="warranty-info" class="clear" style="font-size: 99%">
            <div class="column">
                Warranty Type:<br />
                <%--<%=  Html.DynamicHtmlControlForIprofile("txtWarranty", "Warranty")%>--%>
                <select id="ddlWarrantyType" onchange="javascript:warrantyInfoUpdate(this);">
                    <option value="0"></option>
                    <% if (Model.WarrantyTypes.Count() > 0) {%>
                        <% foreach (var item in Model.WarrantyTypes){%>
                            <option value="<%= item.Id %>" <%= (item.Id == Model.WarrantyInfo) ? "selected" : "" %>><%= item.Name %></option>
                        <%}%>
                    <%}%>
                </select>
                <br />
                Certified
                <%= Html.CheckBoxFor(x=>x.IsCertified)%><br />
                Prior Rental?
                <%=  Html.DynamicHtmlControlForIprofile("txtPriorRental", "EditPriorRental")%><br />
                Dealer Demonstrator?<br />
                <%=  Html.DynamicHtmlControlForIprofile("txtDealerDemo", "EditDealerDemo")%><br />
                Unwind?
                <%=  Html.DynamicHtmlControlForIprofile("txtUnwind", "EditUnwind")%><br />
                A Car&nbsp;
                <input type="checkbox" id="ACar" name="ACar" <%= Model.ACar ? "checked" : "" %> /><br />
                    RAV(Branded Title) 
                <%= Html.CheckBoxFor(x=>x.BrandedTitle)%>
            </div>
        </div>
        <div id="pricing" class="clear column" style="font-size: 99%">
            Pricing Information:<br />
            <table>
                <tr>
                    <td class="label">
                        Retail Price
                    </td>
                    <td>
                        <%= Html.TextBoxFor(x => x.RetailPrice, new { @class = "small" })%>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        Dealer Discount
                    </td>
                    <td>
                        <%= Html.TextBoxFor(x => x.DealerDiscount, new { @class = "small" })%>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        MR Rebate
                    </td>
                    <td>
                        <%= Html.TextBoxFor(x => x.ManufacturerRebate, new { @class = "small" })%>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        WS Price
                    </td>
                    <td>
                        <%= Html.TextBoxFor(x => x.WindowStickerPrice, new { @class = "small" })%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="clear" id="img-title">
    </div>
    <div id="other-details" class="clear">
        <div id="description-box" class="column">
            <table width="100%">
                <tr>
                    <td>
                        <a id="description-link" class="iframe" href="<%=Url.Content("~/Inventory/EditDescription?ListingId=" + Model.ListingId) %>">
                            <input type="button" id="btndescription" value="Edit Description" title="Click to Edit Description"
                                class="pad" />
                        </a>
                    </td>
                    <td>
                        <input type="button" id="btnauctiondescription" value="Auction Description" title="Click to Plug Auction Description"
                            class="pad" />
                    </td>
                    <td>
                        <input type="button" id="btnLoanerDescription" value="Loaner Description" title="Click to Plug Loaner Description"
                            class="pad" />
                    </td>
                     <td>
                        <input type="button" id="btnGenerateAutoDescription" value="Generate Auto Description" title="Click to Generate Auto Description"
                            class="pad" />
                    </td>
                </tr>
            </table>
            <div id="divEditDescription"><input type="checkbox" id="chbManual" <%= Model.IsManual ? "checked" : "" %>/> Edit description manually</div>            
            <textarea cols="90" <%= Model.EnableAutoDescriptionSetting ? (Model.IsManual ? "" : "disabled") : "" %> id="Description" name="Description" rows="9"><%= Model.Description %></textarea>
        </div>
       
    </div>
    <div style="height: 10px" class="clear">
        &nbsp;</div>
    <div>
      
    </div>
    <% Html.EndForm(); %>
    <script type="text/javascript">
        $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

        $("#btnauctiondescription").click(function () {

            $.post('<%= Url.Content("~/Inventory/GetDealerAuctionDescription") %>', {}, function (data) {
                if (data == "SessionTimeOut") {

                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.location = actionUrl;
                } else {
                    document.editiProfileForm.Description.value = data;

                }
            });
        });


        $("#btnLoanerDescription").click(function () {

            $.post('<%= Url.Content("~/Inventory/GetDealerLoanerDescription") %>', {}, function (data) {
                if (data == "SessionTimeOut") {

                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.location = actionUrl;
                } else {
                    document.editiProfileForm.Description.value = data;

                }
            });
        });

        $("#CancelIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });
        $("#SaveIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });

        $("#ACar").click(function () {
            if ($('#ACar').is(':checked')) {
                $('#ACar').val('True');
            } else {
                $('#ACar').val('False');
            }
        });

        $("#BrandedTitle").click(function () {
            if ($('#BrandedTitle').is(':checked')) {
                $('#BrandedTitle').val('True');
            } else {
                $('#BrandedTitle').val('False');
            }
        });
        $("a.iframe").fancybox({ 'width': 1010, 'height': 700, centerOnScroll: true });

        $("a.smalliframe").fancybox();

        function getSelectedPakage() {
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {
                    descriptionArray.push($(result[i]).attr('value'));
                }
            }
            if (result.length > 0) {
                $("#hdnFistSelectedPackages").val(JSON.stringify(descriptionArray));
            }
        }

        function getSelectedOption() {
            var result = $("#optionals input[name='txtNonInstalledOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {
                    descriptionArray.push($(result[i]).attr('value'));
                }
            }
            if (result.length > 0) {
                $("#hdnFistSelectedOptions").val(JSON.stringify(descriptionArray));
            }
        }

        $(document).ready(function () {
            if ($("#EnableAutoDescriptionSetting").val() == 'False') {
                $("#btnGenerateAutoDescription").hide();
                $("#divEditDescription").hide();
            }

            $("#btnGenerateAutoDescription").click(function () {
                //$.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });

                $.ajax({
                    type: "GET",
                    url: "/Inventory/GenerateAutoDescription?listingId=" + '<%=Model.ListingId %>',
                    data: {},
                    success: function (results) {
                        if (results == 'Failed') {
                            alert("You need to uncheck \'Edit description manually\' to use this function");
                            return false;
                        }
                        else {
                            $("#Description").val(results);
                            $("#Description").attr('disabled', 'disabled');
                        }
                         $.unblockUI();
                    }
                });
            });

            $("#chbManual").click(function (e) {
                if ($(this).is(':checked')) {
                    var manual = confirm('If you edit description manually, the description won\'t be updated automatically. Are you sure to continue?');
                    if (manual) {
                        $("#Description").removeAttr('disabled');
                        $.ajax({
                            type: "GET",
                            url: "/Inventory/UpdateAutoDescriptionStatus?listingId=" + '<%=Model.ListingId %>' + "&status=false",
                            data: {},
                            success: function () {

                            }
                        });

                    } else {
                        $("#chbManual").attr('checked', false);
                    }
                } else {
                    var auto = confirm('If you let the system create description automatically, all the description you typed in will be lost. Are you sure to continue?');
                    if (auto) {
                        $("#Description").attr('disabled', 'disabled');
                        $.ajax({
                            type: "GET",
                            url: "/Inventory/UpdateAutoDescriptionStatus?listingId=" + '<%=Model.ListingId %>' + "&status=true",
                            data: {},
                            success: function () {

                            }
                        });
                    } else {
                        $("#chbManual").attr('checked', true);
                    }
                }
            });

            $("#ShowImages").click(function (e) {
                $.post('<%= Url.Content("~/Inventory/GetImages") %>', { id: '<%=Model.ListingId %>' }, function (data) {
                    $("#photos").html('<ul id="listImages">' + data + '</ul>');
                    var count = document.getElementById('listImages').getElementsByTagName('li').length;
                    var currentheight = 55 * (count / 3);
                    document.getElementById('listImages').style.height = currentheight + 'px';
                });
            });

            $("a.image").live('mouseover', function () {
                $(this).fancybox();
                //                    pressed++;
                //                    var t = setTimeout("pressed=0", 500);
                //                    if (pressed == 2) {
                //                        pressed = 0;
                //                        $(this).fancybox();
                //                        return false;
                //                    }
            });

            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            var trimValue = $("#SelectedTrim").val();
            if (trimValue) {
                $("#hdnFirstSelectedTrim").val(trimValue.substring(0, trimValue.indexOf("|")));
            }
            getSelectedOption();
            getSelectedPakage();


            $("#SelectedExteriorColor").change(function (e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            });
            $("#SelectedVehicleType").change(function (e) {


                $('#elementID').removeClass('hideLoader');

                var VehicleType = $("#SelectedVehicleType").val();

                if (VehicleType == 'Car') {
                    var actionUrl = '<%= Url.Action("EditIProfile", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';

                    actionUrl = actionUrl.replace('PLACEHOLDER', $("#ListingId").val());

                    window.location = actionUrl;
                }

                $('#elementID').addClass('hideLoader');


            });

            var Trim = $("#SelectedTrim");
            Trim.change(function () {

                var index = Trim.val().indexOf("|");
                var id = Trim.val().substring(0, index);
                $('#elementID').removeClass('hideLoader');
                $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id }, function (data) {


                    $("#Packages li").children().each(
                  function () {

                      $(this).val("");
                      $(this).removeAttr("checked");
                      $(this).attr("class", "z-index hider");
                      $(this).next("label:first").text("");
                      $(this).next("label:first").attr("class", "z-index hider");

                  }
                );

                    $("#Options li").children().each(
                  function () {

                      $(this).val("");
                      $(this).removeAttr("checked");
                      $(this).attr("class", "z-index hider");
                      $(this).next("label:first").text("");
                      $(this).next("label:first").attr("class", "z-index hider");

                  }
                );
                    var exitems = "";
                    var initems = "";
                    var MSRP = "";
                    var cylinders = "";
                    var fuel = "";
                    var litters = "";
                    var trans = "";
                    var wheeldrive = "";
                    var bodytype = "";
                    var stockPhoto = "";
                    var vehicleoptions = new Array();

                    var vehiclepackages = new Array();
                    var exteriorFlag = true;
                    var doors = "";
                    var selectedTransmission = "";
                    $.each(data, function (i, data) {
                        var text;
                        if (data.toString() == "Automatic") {
                            selectedTransmission = "Automatic";
                        }
                        else if (data.toString() == "Manual") {
                            selectedTransmission = "Manual";
                        }
                        else if (data.toString().indexOf("BodyType") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            bodytype += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Optional") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            var result = text.split("*");
                            text = result[0];
                            var optionPrice = text.substring(text.lastIndexOf(" ") + 1);

                            var option = new Object();
                            option.price = optionPrice;
                            option.name = text;
                            option.description = result[1];
                            vehicleoptions[vehicleoptions.length] = option;
                        }
                        else if (data.toString().indexOf("Package") != -1) {
                            text = data.toString().substring(0, data.toString().length - 7);
                            var result = text.split("*");
                            text = result[0];
                            var packagePrice = text.substring(text.lastIndexOf(" ") + 1);

                            var option = new Object();
                            option.price = packagePrice;
                            option.name = text;
                            option.description = result[1];
                            vehiclepackages[vehiclepackages.length] = option;
                        }
                        else if (data.toString().indexOf("Fuel") != -1) {
                            text = data.toString().substring(0, data.toString().length - 4);
                            fuel += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Litter") != -1) {
                            text = data.toString().substring(0, data.toString().length - 6);
                            litters += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Cylinder") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            cylinders += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("MSRP") != -1) {
                            MSRP = data.toString().substring(0, data.toString().length - 4);
                            MSRP = Number(MSRP);
                            MSRP = formatDollar(MSRP);
                        }
                        else if (data.toString().indexOf("Exterior") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            var textList = text.split("|");
                            if (exteriorFlag) {
                                $("#SelectedExteriorColorValue").val(textList[0]);
                                exteriorFlag = false;
                            }

                            exitems += "<option value='" + textList[1] + "'>" + textList[0] + "</option>";
                        }
                        else if (data.toString().indexOf("Interior") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            initems += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("PassengerDoors") != -1) {
                            text = data.toString().substring(0, data.toString().length - 14);
                            doors += text;
                        }
                        else if (data.toString().indexOf("WheelDrive") != -1) {
                            text = data.toString().substring(0, data.toString().length - 10);
                            wheeldrive += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("DefaultImage") != -1) {
                            text = data.toString().substring(0, data.toString().length - 12);
                            stockPhoto = text;
                        }
                    });

                    if (selectedTransmission == "Automatic") {
                        trans += "<option value='Automatic' selected='selected'>Automatic</option>";
                    } else {
                        trans += "<option value='Automatic'>Automatic</option>";
                    }

                    if (selectedTransmission == "Manual") {
                        trans += "<option value='Manual' selected='selected'>Manual</option>";
                    } else {
                        trans += "<option value='Manual'>Manual</option>";
                    }
                    trans += "<option value='Shiftable Automatic Transmission'>Shiftable Automatic Transmission</option>";

                    exitems += "<option value='Other Colors'>Other Colors</option>";

                    initems += "<option value='Other Colors'>Other Colors</option>";

                    $("#SelectedExteriorColor").html(exitems);
                    $("#SelectedInteriorColor").html(initems);
                    $("#SelectedCylinder").html(cylinders);
                    $("#SelectedFuel").html(fuel);
                    $("#SelectedLiters").html(litters);
                    $("#SelectedTranmission").html(trans);
                    $("#SelectedDriveTrain").html(wheeldrive);
                    $("#SelectedBodyType").html(bodytype);
                    $("#MSRP").val(MSRP);

                    $("#Door").val(doors);
                    $("#ChromeStyleId").val(id);
                    $("#DefaultImageUrl").val(stockPhoto);

                    var indexPackages = 0;

                    var indexOptions = 0;

                    var packageContent = "";
                    $("#Packages").html("");

                    packageContent += "<ul class='options'>";

                    for (var i = 0; i < vehiclepackages.length; i++) {
                        packageContent += '<li>';
                        packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this);' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "' title='" + vehiclepackages[i].description + "'>";
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>" + vehiclepackages[i].name + "</label>";
                        packageContent += "<br class='z-index' price='" + vehiclepackages[i].price + "'>";
                        packageContent += '</li>';
                    }

                    packageContent += "</ul>";

                    $("#Packages").html(packageContent);

                    $("#Options").html("");
                    var optionContent = "";

                    optionContent += "<ul class='options'>";

                    for (var i = 0; i < vehicleoptions.length; i++) {
                        optionContent += '<li>';
                        optionContent += "<input type='checkbox' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this)' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                        optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>" + vehicleoptions[i].name + "</label>";
                        optionContent += '<br>';
                        optionContent += '</li>';
                    }

                    optionContent += "</ul>";


                    $("#Options").html(optionContent);


                    $('#elementID').addClass('hideLoader');




                });
            });
        });

        function validateForm() {
            var flag = true;

            if ($("#CusErrorEx").length > 0) {
                $("#CusErrorEx").remove();
            }

            if ($("#CusErrorIn").length > 0) {
                $("#CusErrorIn").remove();
            }

            if ($("#CusErrorApp").length > 0) {
                $("#CusErrorApp").remove();
            }

            if ($("#CusErrorTransmission").length > 0) {
                $("#CusErrorTransmission").parent().remove();
            }

            if ($("#CusErrorMileage").length > 0) {
                $("#CusErrorMileage").parent().remove();
            }

            if ($("#SelectedTranmission").val() == "") {
                $("#SelectedTranmission").parent().append("<div><strong id='CusErrorTransmission'><font color='Red'>Required</font></strong></div>");
                flag = false;
            }

            if ($("#Mileage").val() == "") {
                $("#Mileage").parent().append("<div><strong id='CusErrorMileage'><font color='Red'>Required</font></strong></div>");
                flag = false;
            }

            if (flag == false)
                return false;
            else
                $('#elementID').removeClass('hideLoader');

            $("#Description").removeAttr('disabled');
            var result = $("#Packages input[name='txtFactoryPackageOption']:checked");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('title') !== undefined) {
                    descriptionArray.push($(result[i]).attr('title'));
                }
            }
            if (result.length > 0) {
                $("#SelectedPackagesDescription").val(JSON.stringify(descriptionArray));
            }
            getAfterSelectedPakage();
            getAfterSelectedOption();
        }

        function getAfterSelectedPakage() {
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            //var descriptionArray = new Array();
            var builderstring = '';
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {

                    var indexS = $(result[i]).attr('value').indexOf('$');
                    builderstring += $(result[i]).attr('value').substring(0, indexS) + ',';

                }
            }
            if (result.length > 0) {
                $("#AfterSelectedPackage").val(builderstring);
              
            }

        }

        function getAfterSelectedOption() {
            var result = $("#optionals input[name='txtNonInstalledOption']");
            var builderstring = '';
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {

                    var indexS = $(result[i]).attr('value').indexOf('$');
                    builderstring += $(result[i]).attr('value').substring(0, indexS) + ',';

                }
            }
            if (result.length > 0) {
                $("#AfterSelectedOptions").val(builderstring);
             
            }

        }

        function warrantyInfoUpdate(checkbox) {
            console.log(checkbox.value);
            if (checkbox.value != '0') {
                $.post('<%= Url.Content("~/Inventory/UpdateWarrantyInfo") %>', { WarrantyInfo: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                    if (data.SessionTimeOut == "TimeOut") {
                        alert("Your session has timed out. Please login back again");
                        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                        window.parent.location = actionUrl;
                    }


                });
            }
        }

        function priorRentalUpdate(checkbox) {
            console.log(checkbox.value);
            $.post('<%= Url.Content("~/Inventory/PriorRentalUpdate") %>', { PriorRental: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }


            });
        }

        function dealerDemoUpdate(checkbox) {
            //console.log(checkbox.value);
            $.post('<%= Url.Content("~/Inventory/DealerDemoUpdate") %>', { DealerDemo: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }


            });
        }

        function unwindUpdate(checkbox) {
            // console.log(checkbox.value);
            $.post('<%= Url.Content("~/Inventory/UnwindUpdate") %>', { Unwind: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }


            });
        }
                        
    </script>
</asp:Content>
