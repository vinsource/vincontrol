<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AdminViewModel>" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Admin</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />

    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/Content/uploadify.css")%>" rel="stylesheet" type="text/css" />

    <script src="<%=Url.Content("~/Scripts/jquery.uploadify.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/Scripts/jquery.uploadify.min.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

    <%--<script src="<%=Url.Content("~/js/ckeditor/ckeditor.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/ckeditor/adapters/jquery.js")%>" type="text/javascript"></script>--%>
    <style type="text/css">
        #c2
        {
            width: 784px;
            border-right: none;
        }
        h4
        {
            margin-bottom: 0;
            color: white;
        }
        table
        {
            width: 750px;
            font-size: .8em;
            overflow: visible;
        }
        tr.l
        {
            background: #222;
            margin: 0;
        }
        tr.d
        {
            padding-top: .2em;
            padding-bottom: .3em;
            margin: 0;
        }
        .listHeader
        {
            background: black;
            text-align: center;
        }
        .none
        {
            float: right;
            color: #999;
            margin-right: .3em;
        }
        .hider
        {
            display: none;
        }
        .fit
        {
            width: 100px;
        }
        #userRights input
        {
            width: 100%;
            min-width: 67px;
        }
        #name
        {
            width: 200px;
        }
        #Username
        {
            width: 100px;
        }
        tr.space td
        {
            height: 10px;
            border-top: #ddd solid 5px;
        }
        input.save
        {
            background: #680000 !important;
            border-bottom: 5px solid #680000 !important;
        }
        input.save:hover
        {
            border-bottom: 5px solid #680000 !important;
        }
        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }
        .subtext
        {
            font-size: .8em;
            margin-top: 0;
            padding: .5em;
            background: black;
        }
        td.label
        {
            width: 120px;
        }
        #ebayCraigs textarea
        {
            width: 95%;
            height: 200px;
        }
        #WindowStickerBuyerGuide textarea
        {
            width: 95%;
            height: 200px;
        }
        .hideLoader
        {
            display: none;
        }
        #discounts td
        {
            padding: 4px !important;
            text-align: center;
        }
        #discounts select, #discounts input
        {
            max-width: 85px !important;
        }
        #discounts a
        {
            display: block;
            padding: .3px;
            font-weight: bold;
            background: #555555;
            text-align: center;
        }
        #this-id
        {
            background: #222222;
            color: white;
            width: 400px;
            height: 250px;
            text-align: center;
        }
        #this-id textarea
        {
            width: 350px;
            height: 150px;
        }
        #this-id h3
        {
            margin-top: 0;
            padding-top: 20px;
        }
        .discount-wrap
        {
            height: 400px;
            overflow: hidden;
            overflow-y: scroll;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">
        var basicsWarrantyTypes = <%= Html.ToJson(Model.BasicWarrantyTypes)%>;

        $("#InventoryTab").attr("class", "");
        $("#AppraisalTab").attr("class", "");
        $("#KPITab").attr("class", "");
        $("#AdminTab").attr("class", "on");
        $("#ReportTab").attr("class", "");
        $(document).ready(function () {
            $("#aCancelBuyerGuide").click(function () {
                if ($("#divNewBuyerGuide").is(":visible")) {
                    $("#txtNewBuyerGuide").val('');
                    $("#divNewBuyerGuide").slideUp("slow");
                }
            });

            $("#aAddBuyerGuide").click(function () {
                if ($("#divNewBuyerGuide").is(":hidden")) {
                    $("#divNewBuyerGuide").slideToggle("slow");
                    $("#txtNewBuyerGuide").focus();
                }
            });

            $("#aDoneBuyerGuide").click(function () {
                if ($("#SelectedWarrantyType").val() == "") {
                    alert("Category is required.");
                }
                else if ($("#txtNewBuyerGuide").val() == "") {
                    alert('Buyer Guide Name is required.');
                } else {

                    $.ajax({
                        type: "GET",
                        url: '/Admin/AddNewBuyerGuide?name=' + $("#txtNewBuyerGuide").val() + '&category=' + $("#SelectedWarrantyType").val(),
                        data: {},
                        success: function (results) {
                            if (results == 'Existing') {
                                alert('This name is already in the system');
                            } else if (results == 'TimeOut') {
                                window.location.href = '<%= Url.Action("LogOff", "Account") %>';
                            } else if (results == 'Error') {
                                alert('System error!');
                            } else {
                                var str = "<span id=subBuyerGuide_" + results + ">";
                                str += "<span id=spanBuyerGuideName_" + results + ">" + $("#txtNewBuyerGuide").val() + "</span>";

                                str += "<select id=SelectedWarrantyTypeForEdit_" + results + " style='width:150px;display:none;'>";
                                str += "<option value=''>-- Select category --</option>";

                                $.each(basicsWarrantyTypes, function (index, value) {
                                    str += value.Value == $("#SelectedWarrantyType").val() ? "<option value=" + value.Value + " selected='true'>" + value.Text + "</option>" : "<option value=" + value.Value + ">" + value.Text + "</option>";
                                });
                                                              
                                str += "</select>";

                                str += "&nbsp;<input type=text id=txtEditBuyerGuide_" + results + " name=txtEditBuyerGuide_" + results + " style='padding:2px;margin:5px 0;width:200px;display:none;' value='" + $("#txtNewBuyerGuide").val() + "' />";
                                str += "&nbsp;&nbsp;&nbsp;&nbsp;<a class=iframe href=/Report/CreateBuyerGuide?type=" + results + " id=English_" + results + " style='font-weight: normal; font-size: 13px; color: red'>(English) </a>";
                                str += "<a class=iframe href=/Report/CreateBuyerGuideSpanish?type=" + results + " id=Spanish_" + results + " style='font-weight: normal; font-size: 13px; color: royalblue'>(Spanish) </a>";
                                str += "<a id=aEditBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;'>Edit</a>&nbsp;";
                                str += "<a id=aRemoveBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;'>Remove</a>&nbsp;";
                                str += "<a id=aDoneEditBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;display:none;'>Done</a>&nbsp;";
                                str += "<a id=aCancelEditBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;display:none;'>Cancel</a>";
                                str += "<br/>";
                                str += "</span>";

                                $("#txtNewBuyerGuide").val('');
                                $('#SelectedWarrantyType option:selected').remove();
                                $("#divNewBuyerGuide").slideUp("slow");

                                $('#spanNewBuyerGuide').append(str);
                            }
                        }
                    });
                };
            });

            $("a[id^='aRemoveBuyerGuide_']").live('click', function () {
                var retVal = confirm("Do you want to remove this buyer guide ?");
                if (retVal == true) {
                    var id = this.id.split("_")[1];
                    $.ajax({
                        type: "GET",
                        url: '/Admin/RemoveBuyerGuide/' + id,
                        data: {},
                        success: function (results) {
                            if (results == 'True') {
                                //$("#subBuyerGuide_" + id).slideUp("slow");
                                $("#subBuyerGuide_" + id).hide();
                            }
                            else {
                                alert(results);
                            }
                        }
                    });
                }
            });

            $("a[id^='aDoneEditBuyerGuide_']").live('click', function () {
                var id = this.id.split("_")[1];
                if ($("#SelectedWarrantyTypeForEdit_" + id).val() == "") {
                    alert('Category is required.');
                }
                else if ($("#txtEditBuyerGuide_" + id).val() == "") {
                    alert('Buyer Guide Name is required.');
                } else {
                    $.ajax({
                        type: "GET",
                        url: '/Admin/UpdateBuyerGuide?listingId=' + id + '&name=' + $("#txtEditBuyerGuide_" + id).val() + '&category=' + $("#SelectedWarrantyTypeForEdit_" + id).val(),
                        data: {},
                        success: function (results) {
                            if (results == 'Existing') {
                                alert('This name is already in the system');
                            } else if (results == 'TimeOut') {
                                window.location.href = '<%= Url.Action("LogOff", "Account") %>';
                            } else if (results == 'Error') {
                                alert('System error!');
                            } else if (results == 'True') {
                                $("#aDoneEditBuyerGuide_" + id).hide();
                                $("#aCancelEditBuyerGuide_" + id).hide();
                                $("#txtEditBuyerGuide_" + id).hide();
                                $("#SelectedWarrantyTypeForEdit_" + id).hide();
                                $("#aEditBuyerGuide_" + id).show();
                                $("#aRemoveBuyerGuide_" + id).show();
                                $("#spanBuyerGuideName_" + id).html($("#txtEditBuyerGuide_" + id).val());
                                $("#spanBuyerGuideName_" + id).show();
                            } else {
                                alert(results);
                            }
                        }
                    });
                }
            });

            $("a[id^='aEditBuyerGuide_']").live('click', function () {
                var id = this.id.split("_")[1];
                $("#aDoneEditBuyerGuide_" + id).show();
                $("#aCancelEditBuyerGuide_" + id).show();                
                $("#txtEditBuyerGuide_" + id).show();
                $("#txtEditBuyerGuide_" + id).focus();
                $("#SelectedWarrantyTypeForEdit_" + id).show();
                $("#aEditBuyerGuide_" + id).hide();
                $("#aRemoveBuyerGuide_" + id).hide();
                $("#spanBuyerGuideName_" + id).hide();

            });

            $("a[id^='aCancelEditBuyerGuide_']").live('click', function () {
                var id = this.id.split("_")[1];
                $("#aDoneEditBuyerGuide_" + id).hide();
                $("#aCancelEditBuyerGuide_" + id).hide();
                //$("#txtEditBuyerGuide_" + id).val($("#spanBuyerGuideName_" + id).html());
                $("#txtEditBuyerGuide_" + id).hide();                
                $("#SelectedWarrantyTypeForEdit_" + id).hide();                
                $("#aEditBuyerGuide_" + id).show();
                $("#aRemoveBuyerGuide_" + id).show();
                $("#spanBuyerGuideName_" + id).show();
            });

            $("#txtNewBuyerGuide").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();                    
                    if ($("#SelectedWarrantyType").val() == "") {
                        alert('Category is required.');
                    }
                    else if ($("#txtNewBuyerGuide").val() == "") {
                        alert('Buyer Guide Name is required.');
                    } else {

                        $.ajax({
                            type: "GET",
                            url: '/Admin/AddNewBuyerGuide?name=' + $("#txtNewBuyerGuide").val() + '&category=' + $("#SelectedWarrantyType").val(),
                            data: {},
                            success: function (results) {
                                if (results == 'Existing') {
                                    alert('This name is already in the system');
                                } else if (results == 'TimeOut') {
                                    window.location.href = '<%= Url.Action("LogOff", "Account") %>';
                                } else if (results == 'Error') {
                                    alert('System error!');
                                } else {
                                    var str = "<span id=subBuyerGuide_" + results + ">";
                                    str += "<span id=spanBuyerGuideName_" + results + ">" + $("#txtNewBuyerGuide").val() + "</span>";

                                    str += "<select id=SelectedWarrantyTypeForEdit_" + results + " style='width:150px;display:none;'>";
                                    str += "<option value=''>-- Select category --</option>";

                                    $.each(basicsWarrantyTypes, function (index, value) {
                                        str += value.Value == $("#SelectedWarrantyType").val() ? "<option value=" + value.Value + " selected='true'>" + value.Text + "</option>" : "<option value=" + value.Value + ">" + value.Text + "</option>";
                                    });
                                    
                                    str += "</select>";
                                    
                                    str += "&nbsp;<input type=text id=txtEditBuyerGuide_" + results + " name=txtEditBuyerGuide_" + results + " style='padding:2px;margin:5px 0;width:200px;display:none;' value='" + $("#txtNewBuyerGuide").val() + "' />";
                                    
                                    str += "&nbsp;&nbsp;&nbsp;&nbsp;<a class=iframe href=/Report/CreateBuyerGuide?type=" + results + " id=English_" + results + " style='font-weight: normal; font-size: 13px; color: red'>(English) </a>";
                                    str += "<a class=iframe href=/Report/CreateBuyerGuideSpanish?type=" + results + " id=Spanish_" + results + " style='font-weight: normal; font-size: 13px; color: royalblue'>(Spanish) </a>";
                                    str += "<a id=aEditBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;'>Edit</a>&nbsp;";
                                    str += "<a id=aRemoveBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;'>Remove</a>&nbsp;";
                                    str += "<a id=aDoneEditBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;display:none;'>Done</a>&nbsp;";
                                    str += "<a id=aCancelEditBuyerGuide_" + results + " href=javascript:; style='background:#680000 !important;color:White;padding:0 5px;display:none;'>Cancel</a>";
                                    str += "<br/>";
                                    str += "</span>";

                                    $("#txtNewBuyerGuide").val('');
                                    $('#SelectedWarrantyType option:selected').remove();
                                    $("#divNewBuyerGuide").slideUp("slow");

                                    $('#spanNewBuyerGuide').append(str);
                                }
                            }
                        });
                    };
                }
            });

            $("input[id^='txtEditBuyerGuide_']").live('keypress', function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    var id = this.id.split("_")[1];
                    if ($("#SelectedWarrantyTypeForEdit_" + id).val() == "") {
                        alert('Category is required.');
                    }
                    else if ($("#txtEditBuyerGuide_" + id).val() == "") {
                        alert('Buyer Guide Name is required.');
                    } else {
                        $.ajax({
                            type: "GET",
                            url: '/Admin/UpdateBuyerGuide?listingId=' + id + '&name=' + $("#txtEditBuyerGuide_" + id).val() + '&category=' + $("#SelectedWarrantyTypeForEdit_" + id).val(),
                            data: {},
                            success: function (results) {
                                if (results == 'Existing') {
                                    alert('This name is already in the system');
                                } else if (results == 'TimeOut') {
                                    window.location.href = '<%= Url.Action("LogOff", "Account") %>';
                                } else if (results == 'Error') {
                                    alert('System error!');
                                } else if (results == 'True') {
                                    $("#aDoneEditBuyerGuide_" + id).hide();
                                    $("#aCancelEditBuyerGuide_" + id).hide();
                                    $("#txtEditBuyerGuide_" + id).hide();
                                    $("#SelectedWarrantyTypeForEdit_" + id).hide();
                                    $("#aEditBuyerGuide_" + id).show();
                                    $("#aRemoveBuyerGuide_" + id).show();
                                    $("#spanBuyerGuideName_" + id).html($("#txtEditBuyerGuide_" + id).val());
                                    $("#spanBuyerGuideName_" + id).show();
                                } else {
                                    alert(results);
                                }
                            }
                        });
                    }
                }
            });

        });
    </script>

    <h3>
        Admin Control Panel</h3>
    <div id="topNav">
        <%if (Model.LandingPage.Equals("Default"))
          {%>
        <input class="btn onadmin" type="button" name="inventory" value=" Content " />        
        <%if ((bool)Session["HasAdminRight"])
          {%>
        <input class="btn" type="button" name="notifications" value=" Notifications " />
        <input class="btn" type="button" name="userRIghts" value=" User Rights " />
        <%}%>
        <input class="btn" type="button" name="wholesale" value=" Wholesale " />
        <%if ((bool)Session["HasAdminRight"])
          {%>
        <input class="btn" type="button" name="dealerInfo" value=" Dealer Info " />
        <%}%>
        <input class="btn" type="button" name="priceInfo" value=" Price/Website " />
        <input class="btn" type="button" name="tradeIn" value=" Trade in " />
        <%--<input class="btn" type="button" name="buyerGuide" value=" Buyer Guide " />--%>
        <% Html.BeginForm("UpdateSetting", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }); %>
        <input class="save" type="submit" name="UpdateSetting" value="Save Changes" />
        <% } %>
        <%if (Model.LandingPage.Equals("UserRights"))
          {%>
        <input class="btn" type="button" name="inventory" value=" Content " />        
        <%if ((bool)Session["HasAdminRight"])
          {%>
        <input class="btn" type="button" name="notifications" value=" Notifications " />
        <input class="btn onadmin" type="button" name="userRIghts" value=" User Rights " />
        <%}%>
        <input class="btn" type="button" name="wholesale" value=" Wholesale " />
        <%if ((bool)Session["HasAdminRight"])
          {%>
        <input class="btn" type="button" name="dealerInfo" value=" Dealer Info " />
        <%}%>
        <input class="btn" type="button" name="priceInfo" value=" Price/Website " />
        <input class="btn" type="button" name="tradeIn" value=" Trade in " />
        <%--<input class="btn" type="button" name="buyerGuide" value=" Buyer Guide " />--%>
        <% Html.BeginForm("UpdateSetting", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }); %>
        <input class="save" type="submit" name="UpdateSetting" value="Save Changes" />
        <% } %>
    </div>
    <div id="tradeIn" class="hider">
        <div id="cost_result">
            <h4>
                Trade In Price Variance
                <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                    width: auto !important; padding: .1em;" type="button" name="tradeinpricevarianceBTN"
                    value=" ? " /></h4>
            <p id="tradeinpricevarianceInfo" class="subtext hider">
                In this section you can manipulate the trade in price variance</p>
            <%Html.RenderPartial("VarianceCost", Model.VarianceCost); %>
        </div>
        <h4>
            Manage Comments
            <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                width: auto !important; padding: .1em;" type="button" name="managecommentsBTN"
                value=" ? " /></h4>
        <p id="managecommentsInfo" class="subtext hider">
            In this section you can manipulate the trade in price variance</p>
        <div id="result">
            <%Html.RenderPartial("TradeInComment", Model.Comments); %>
        </div>
        <h4>
            Add Comment
            <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                width: auto !important; padding: .1em;" type="button" name="addcommentBTN" value=" ? " /></h4>
        <p id="addcommentInfo" class="subtext hider">
            In this section you can manipulate the trade in price variance</p>
        <table>
            <tr>
                <td>
                    Name
                </td>
                <td>
                    <input id="Name" name="Name" type="text">
                </td>
            </tr>
            <tr>
                <td>
                    City
                </td>
                <td>
                    <input id="City" name="City" type="text">
                </td>
            </tr>
            <tr>
                <td>
                    State
                </td>
                <td>
                    <input id="State" name="State" type="text">
                </td>
            </tr>
            <tr>
                <td>
                    Content
                </td>
                <td>                   
                    <textarea rows="6" name="Content" id="Content" cols="30"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" id="btnAdd" name="add-comment" value="Add" />
                </td>
            </tr>
        </table>
    </div>
    <div id="priceInfo" class="hider">
        <h4>
            Sold
            <input style="background: #860000; font-weight: bold; padding: .1em;" type="button"
                name="soldActionBTN" value=" ? " /></h4>
        <p id="P3" class="subtext hider">
            This changes how marking a vehicle as sold is handled. You can set it to either
            delete it immediately from your inventory or have it remain in inventory for a few
            days in case changes need to be made.</p>
        Select Sold Action:
        <%=Html.DropDownListFor(x=>x.SoldAction,Model.SoldActionList) %>
        <h4>
            Window Sticker/Buyer Guide Misc Information
            <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                width: auto !important; padding: .1em;" type="button" name="windowstickerbuyerguideBTN"
                value=" ? " /></h4>
        <p id="P1" class="subtext hider">
            In this section you can add your miscellaneous information which will be repeated
            across template such as Window Sticker/BuyerGuide.</p>
        <!--<table id="WindowStickerBuyerGuide" style="width:100%;">
                    <tr><td>Manufacturer Warranty</td><td>Dealer Certified</td><td>Manufacturer Certified</td></tr>
                   <td><%=Html.TextAreaFor(x=>x.ManufacturerWarranty) %></td>
                    <td><%=Html.TextAreaFor(x=>x.DealerCertified) %></td>
                    <td><%=Html.TextAreaFor(x=>x.ManufacturerCertified) %></td></tr>
                    <tr><td></td><td>Dealer Certified Duration</td><td>Manufacturer Certified Duration</td></tr>
                     <tr><td></td>
                    <td><%=Html.TextAreaFor(x => x.DealerCertifiedDuration)%></td>
                    <td><%=Html.TextAreaFor(x => x.ManufacturerCertifiedDuration)%></td></tr>
                    </table>-->
        <table id="WindowStickerWebsite" style="width: 100%;">
            <tr>
                <td>
                    <h4>
                        Price</h4>
                </td>
                <td>
                    <h4>
                        Website</h4>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Html.DynamicHtmlLabelAdmin("RetailPriceWSNotification")%><%=Html.TextBoxFor(x=>x.RetailPriceWSNotificationText) %>
                    (Retail Price)<br />
                    <%=Html.DynamicHtmlLabelAdmin("DealerDiscountWSNotification")%><%=Html.TextBoxFor(x=>x.DealerDiscountWSNotificationText) %>
                    (Dealer Discount)<br />
                    <%=Html.DynamicHtmlLabelAdmin("ManufacturerReabateNotification")%><%=Html.TextBoxFor(x=>x.ManufacturerReabteWsNotificationText) %>
                    (Manufacturer Rebate)<br />
                    <%=Html.DynamicHtmlLabelAdmin("SalePriceNotification")%><%=Html.TextBoxFor(x=>x.SalePriceWsNotificationText) %>
                    (Sale Price)<br />
                </td>
                <td>
                    <input type="checkbox" name="RetailPrice">Retail Price<br />
                    <input type="checkbox" name="DealerDiscount">Dealer Discount<br />
                    <input type="checkbox" name="ManufacturerRebate">Manufacturer Rebate<br />
                    <input type="checkbox" name="SalePrice">Sale Price<br />
                </td>
            </tr>
        </table>
        <h3>
            Discounts/Rebates
            <input style="background: #860000; font-weight: bold; padding: .1em;" type="button"
                name="discountsAndRebatesBTN" value=" ? " /></h3>
        <p id="discountsAndRebatesInfo" class="subtext hider" style="font-size: .9em;">
            Here you can manage your rebate information pertaining to your new vehicles. By
            simply choosing the information about the car/truck, pick its body style and then
            add the discount number along with the disclaimer. This information will be sent
            out in any data feeds, so if a website supports the displaying of these deals it
            should appear there as well (you can toggle this on and off above).</p>
        <div class="discount-wrap">
            <table id="discounts" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="listHeader">
                        Year
                    </td>
                    <td class="listHeader">
                        Make
                    </td>
                    <td class="listHeader">
                        Model
                    </td>
                    <td class="listHeader">
                        Trim
                    </td>
                    <td class="listHeader">
                        Body Type
                    </td>
                    <td class="listHeader" title="Manufacturer Rebate">
                        Manufacturer Rebate
                    </td>
                    <td class="listHeader" title="Click to Add a Disclaimer">
                        Disclaimer
                    </td>
                    <td class="listHeader">
                    </td>
                </tr>
                <tr>
                    <td class="listHeader">
                        <%= Html.DropDownListFor(x => x.SelectedYear, Model.YearsList) %>
                    </td>
                    <td class="listHeader">
                        <%= Html.DropDownListFor(x => x.SelectedMake, Model.MakesList) %>
                    </td>
                    <td class="listHeader">
                        <%= Html.DropDownListFor(x => x.SelectedModel, Model.ModelsList) %>
                    </td>
                    <td class="listHeader">
                        <%= Html.DropDownListFor(x => x.SelectedTrim, Model.TrimsList) %>
                    </td>
                    <td class="listHeader">
                        <%= Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList)%>
                    </td>
                    <td class="listHeader" title="Manufacturer Rebate">
                        <input id="rebateamount" type="text" />
                    </td>
                    <td class="listHeader" title="Click to Add a Disclaimer">
                        <textarea cols="18" id="disclaimerrebate" name="DealerWarranty" rows="2"></textarea>
                    </td>
                    <td class="listHeader">
                        <input type="button" id="btnAddManuRebate" value="Add">
                    </td>
                </tr>
                <% foreach (var tmp in Model.RebateList)
                   {
                                        %>
                <tr>
                    <td class="listHeader">
                        <%=tmp.Year %>
                    </td>
                    <td class="listHeader">
                        <%=tmp.Make %>
                    </td>
                    <td class="listHeader">
                        <%=tmp.Model %>
                    </td>
                    <td class="listHeader">
                        <%=tmp.Trim %>
                    </td>
                    <td class="listHeader">
                        <%=tmp.BodyType %>
                    </td>
                    <td class="listHeader" title="Manufacturer Rebate">
                        <input id="<%=tmp.UniqueId %>" onblur="javascript:updateRebateAmount(this);" type="text"
                            value="<%=tmp.RebateAmount %>" />
                    </td>
                    <td class="listHeader" title="Click to Add a Disclaimer">
                        <textarea id="<%=tmp.UniqueId %>" cols="18" name="DealerWarranty" rows="2" onblur="javascript:updateRebateDisclaimer(this);"><%=tmp.Disclaimer %></textarea>
                    </td>
                    <td class="listHeader">
                        <%= Html.ActionLink("Report", "ViewRebateReportByTrim", "Report", new{@RebateId= tmp.UniqueId }, new { @target = "_blank" })%>
                    </td>
                    <td class="listHeader">
                        <input type="button" id="<%=tmp.UniqueId %>" value="Delete" onclick="javascript:deleteRebate(this);">
                    </td>
                </tr>
                <%
                    } %>
            </table>
        </div>
        <div class="hider">
            <div id="this-id">
                <h3>
                    Disclaimer for - Item Name</h3>
                <textarea></textarea><br />
                <input type="submit" value="Save" />
            </div>
        </div>
    </div>
    <div id="dealerInfo" class="hider">
        <h4>
            Dealer Info</h4>
        <table>
            <tr>
                <td>
                    Dealer Name
                </td>
                <td>
                    <%= Html.Label(Model.DealershipName)%>
                </td>
            </tr>
            <tr>
                <td>
                    Dealer Address
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.Address, new { @readonly = true })%><%=Html.TextBoxFor(x => x.City, new { @readonly = true })%><%=Html.TextBoxFor(x => x.State, new { @readonly = true })%><%=Html.TextBoxFor(x => x.ZipCode, new { @readonly = true })%>
                </td>
            </tr>
            <tr>
                <td>
                    Main Phone #
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.Phone)%>
                </td>
            </tr>
            <tr>
                <td>
                    Main Email Address
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.Email)%>
                </td>
            </tr>
            <tr>
                <td>
                    Upload Your logo
                </td>
                <td>
                    <input type="file" name="dealerLogo" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Override Stock Image<%=Html.DynamicHtmlLabelAdmin("OverrideStockImage")%><input style="background: #860000;
                        font-weight: bold; padding: .1em;" type="button" name="defaultStockImageBTN"
                        value=" ? " />
                    <p id="defaultStockImageInfo" class="subtext hider" style="font-size: .9em;">
                        When checked, this is the stock image that will be displayed and sent to third party
                        lead providers such as AutoTrader and Cars.com. When no images have been uploaded
                        to the vehicle profile for any given vehicle in the inventory this is the image
                        that will be shown.</p>
                </td>
            </tr>
            <tr>
                <td>
                    <img id="DefaultStockImage" height="150" width="200" src="<%=Model.DefaultStockImageURL %>" /><input
                        type="button" id="DeleteStock" title="Delete This Image" value="X" />
                </td>
                <td>
                    Upload Your Own:
                    <input type="file" id="FileUpload" runat="server" multiple="false" />
                    <br />
                    <%=Html.HiddenFor(x=>x.DealershipId) %>
                    <em style="font-size: .9em;">Uploaded files cannot exceed 500kb and can be up to 500px
                        wide and 500px tall.</em>
                </td>
            </tr>
        </table>
    </div>
    <%if (Model.LandingPage.Equals("Default"))
      {%>
    <div id="contentBTN">
        <% }
      else
      {%>
        <div id="contentBTN" class="hider">
            <% }%>
            <h4>
                Inventory
                <input style="background: #860000; font-weight: bold; padding: .1em;" type="button"
                    name="inventorySortBTN" value=" ? " /></h4>
            <p id="inventorySortInfo" class="subtext hider">
                This sets the default sorting order for the inventory screen. Changing this will
                change what vehicles are displayed based on your selection in the above dropdown
                box.</p>
            Default Inventory Sorting:
            <%=Html.DropDownListFor(x=>x.SortSet,Model.SortSetList) %>            
            <div>
                <h4>
                    Buyer Guides
                    <input style="background: #860000; font-weight: bold; padding: .1em;" type="button"
                        name="buyerGuideBTN" value=" ? " /></h4>
                <p id="buyerguideinfo" class="subtext hider">
                    This feature helps you to create templates for diffrent kinds of buyer guide.</p>
                <div style="font-size: 13px">
                    <% if (Model.WarrantyTypes != null && Model.WarrantyTypes.Any())
                       {%>
                    <span id="spanNewBuyerGuide">
                        <% foreach (var item in Model.WarrantyTypes)
                           {%>
                        <span id="subBuyerGuide_<%= item.Id %>"><span id="spanBuyerGuideName_<%= item.Id %>">
                            <%= item.Name %></span>
                            <% if (item.DealerId > 0)
                               {%>                            
                            <select id="SelectedWarrantyTypeForEdit_<%= item.Id %>" style="width:150px;display:none;">
                                <option value="">-- Select category --</option>
                                <%
                                   if (Model.BasicWarrantyTypes != null) { 
                                    foreach (var type in Model.BasicWarrantyTypes){%>
                                <option value="<%= type.Value %>" <%= type.Value.Equals(item.CategoryId.ToString()) ? "selected" : "" %>><%= type.Text %></option>
                                <%}
                                   }%>
                            </select>
                            <input type="text" id="txtEditBuyerGuide_<%= item.Id %>" name="txtEditBuyerGuide_<%= item.Id %>"
                                style="padding: 2px; margin: 5px 0; width: 200px; display: none;" value="<%= item.Name %>" />
                            <%}%>
                            &nbsp;&nbsp; <a class="iframe" href="<%= item.EnglishVersionUrl %>" id="English_<%= item.Id %>"
                                style='font-weight: normal; font-size: 13px; color: red'>(English) </a><a class="iframe"
                                    href="<%= item.SpanishVersionUrl %>" id="Spanish_<%= item.Id %>" style='font-weight: normal;
                                    font-size: 13px; color: royalblue'>(Spanish) </a>
                            <% if (item.DealerId > 0)
                               {%>
                            <a id="aEditBuyerGuide_<%= item.Id %>" href="javascript:;" style='background: #680000 !important;
                                color: White; padding: 0 5px;'>Edit</a> 
                            <a id="aRemoveBuyerGuide_<%= item.Id %>" href="javascript:;" style='background: #680000 !important; color: White; padding: 0 5px;'>
                                    Remove</a> 
                            <a id="aDoneEditBuyerGuide_<%= item.Id %>" href="javascript:;" style='background: #680000 !important; color: White; padding: 0 5px; display: none;'>Done</a> 
                            <a id="aCancelEditBuyerGuide_<%= item.Id %>" href="javascript:;" style='background: #680000 !important; color: White; padding: 0 5px;
                                            display: none;'>Cancel</a>
                            <%}%>
                            <br />
                        </span>
                        <%}%>
                    </span>
                    <%}%>
                    <div id="divNewBuyerGuide" style="display: none;">
                        <%= Html.DropDownListFor(m => m.SelectedWarrantyType, Model.BasicWarrantyTypes, "-- Select category --", new Dictionary<string, object>() { { "style", "width:150px" }, { "id", "SelectedWarrantyType" } })%>
                        <input type="text" id="txtNewBuyerGuide" name="txtNewBuyerGuide" style="padding: 2px;
                            margin: 5px 0; width: 200px;" />
                        &nbsp;&nbsp; <a id="aDoneBuyerGuide" href="javascript:;" style="background: #680000 !important;
                            color: White; padding: 0 5px;">Done</a> <a id="aCancelBuyerGuide" href="javascript:;"
                                style="background: #680000 !important; color: White; padding: 0 5px;">Cancel</a>
                    </div>
                    <a id="aAddBuyerGuide" href="javascript:;" style="background: #680000 !important;
                        color: White; padding: 0 5px;">Add New Buyer Guide</a>
                </div>
            </div>
            <h4>
                Third Party Site Login Credentials
                <input style="background: #860000; font-weight: bold; padding: .1em;" type="button"
                    name="loginCredentialsBTN" value=" ? " /></h4>
            <p id="loginCredentialsInfo" class="subtext hider">
                This area gives you the ability to access 3rd party web resources from VIN Control.
                It also provides additional information for your appraisals and profile pages. All
                information stored here is secure and will not be shared.</p>
            <table>
                <tr>
                    <td class="label">
                        CarFax:
                    </td>
                    <td>
                        <%=Html.EditorFor(x => x.CarFax)%>
                        <%=Html.EditorFor(x => x.CarFaxPassword)%>
                        <%=Html.HiddenFor(x=>x.CarFaxPasswordChanged) %>
                        <input type="hidden" id="hdnOldCarFax" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        Manheim:
                    </td>
                    <td>
                        <%=Html.EditorFor(x => x.Manheim)%>
                        <%=Html.EditorFor(x => x.ManheimPassword)%>
                        <%=Html.HiddenFor(x => x.ManheimPasswordChanged)%>
                        <input type="hidden" id="hdnOldManheim" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        Kelly Blue Book:
                    </td>
                    <td>
                        <%=Html.EditorFor(x => x.KellyBlueBook)%>
                        <%=Html.EditorFor(x => x.KellyPassword)%>
                        <%=Html.HiddenFor(x => x.KellyPasswordChanged)%>
                        <input type="hidden" id="hdnOldKBB" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        Black Book Online:
                    </td>
                    <td>
                        <%=Html.EditorFor(x => x.BlackBook)%>
                        <%=Html.EditorFor(x => x.BlackBookPassword)%>
                        <%=Html.HiddenFor(x => x.BlackBookPasswordChanged)%>
                        <input type="hidden" id="hdnOldBB" />
                    </td>
                </tr>
            </table>
             <h4>
                Auto Description
                <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                    width: auto !important; padding: .1em;" type="button" name="autoDescriptionBTN"
                    value=" ? " /></h4>
            <p id="autoDescriptionInfo" class="subtext hider">
                In this section you can enable and disable auto description service</p>
            <div>
                <%=Html.CheckBoxFor(x=>x.EnableAutoDescription) %>
                <span style="font-size: .8em; margin-left: 3px;">Enable Auto Description</span>
            </div>
            <h4>
                Ebay/Craigslist Misc Information
                <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                    width: auto !important; padding: .1em;" type="button" name="ebayCraigsBTN" value=" ? " /></h4>
            <p id="ebayCraigsInfo" class="subtext hider">
                In this section you can add your miscellaneous information which will be repeated
                across templates such as Ebay and Craigslist.</p>
            <span style="font-size: .8em; margin-left: 3px;">Dealer Contact Info:</span>
            <input type="text" name="name" value="Name" />
            <input type="text" name="phone" value="Phone" />
            <input type="text" nam="email" value="Email" /><br />
            <table id="ebayCraigs" style="width: 100%;">
                <tr>
                    <td>
                        Dealer Warranty
                    </td>
                    <td>
                        Shipping Details
                    </td>
                    <td>
                        Terms and Conditions
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Html.TextAreaFor(x=>x.DealerWarrantyInfo) %>
                    </td>
                    <td>
                        <%=Html.TextAreaFor(x => x.ShippingInfo)%>
                    </td>
                    <td>
                        <%=Html.TextAreaFor(x=>x.TermConditon) %>
                    </td>
            </table>
            <h4>
                Description
                <input style="background: #860000; font-weight: bold; min-width: 0px !important;
                    width: auto !important; padding: .1em;" type="button" name="ebayCraigsBTN" value=" ? " /></h4>
            <p id="P2" class="subtext hider">
                In this section you can add your miscellaneous information which will be repeated
                across templates such as Ebay and Craigslist.</p>
            <table id="descriptInfo" style="width: 100%;">
                <tr>
                    <td>
                        Starting Sentence
                    </td>
                    <td>
                        Ending Sentence
                    </td>
                    <td>
                        Auction Sentence
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Html.TextAreaFor(x => x.StartSentence, new { @cols = 30, @rows = 6 })%>
                    </td>
                    <td>
                        <%=Html.TextAreaFor(x => x.EndSentence, new { @cols = 30, @rows = 6 })%>
                    </td>
                    <td>
                        <%=Html.TextAreaFor(x => x.AuctionSentence, new { @cols = 25, @rows = 6 })%>
                    </td>
            </table>
        </div>
        <div id="wholesale" class="hider">
            <h4>
                Wholesale<input style="background: #860000; font-weight: bold; min-width: 0px !important;
                    width: auto !important; padding: .1em;" type="button" name="wholesaleBTN" value=" ? " /></h4>
            <p id="wholesaleInfo" class="subtext hider">
                Here you can set filters for the vehicles your dealership is looking to purchase
                on the wholesale market. By entering a year (range of years), make and model you
                can prioritize these cars in the Wanted Cars section of the Wheolesale page on this
                site.</p>
            <table>
                <tr>
                    <td class="listHeader">
                    </td>
                    <td class="listHeader">
                        Year Range
                    </td>
                    <td class="listHeader">
                        Make
                    </td>
                    <td class="listHeader">
                        Model
                    </td>
                    <td class="listHeader">
                    </td>
                </tr>
                <tr>
                    <td>
                        Add New:
                    </td>
                    <td>
                        <input type="text" name="year" value="example: 2009-2011" />
                    </td>
                    <td>
                        <select name="make">
                            <option value="">Select Make </option>
                        </select>
                    </td>
                    <td>
                        <select name="model">
                            <option value="">Select Model </option>
                        </select>
                    </td>
                    <td>
                        <input type="submit" name="submit" value="Add" />
                    </td>
                </tr>
                <tr>
                    <td class="none">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr class="l">
                    <td class="none">
                        1.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
                <tr>
                    <td class="none">
                        2.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
                <tr class="l">
                    <td class="none">
                        3.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
                <tr>
                    <td class="none">
                        4.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
                <tr class="l">
                    <td class="none">
                        5.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
                <tr>
                    <td class="none">
                        6.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
                <tr class="l">
                    <td class="none">
                        7.
                    </td>
                    <td>
                        Year (range)
                    </td>
                    <td>
                        Make
                    </td>
                    <td>
                        Model
                    </td>
                    <td>
                        <input type="submit" name="delete" value="Delete" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="notifications" class="hider">
            <h4>
                Notifications<input style="background: #860000; font-weight: bold; padding: .1em;"
                    type="button" name="notificationsBTN" value=" ? " /></h4>
            <p id="notificationsInfo" class="subtext hider">
                This section allows you to manage what notifications are sent to both email and
                phone as well as who gets what notifications. By clicking on "Manage Users" you
                can select the individuals who will recieve the notices.</p>
            <table>
                <tr>
                    <td class="listHeader">
                        Notification Type
                    </td>
                    <td class="listHeader">
                        Select
                    </td>
                    <td class="listHeader">
                        Users
                    </td>
                </tr>
                <tr>
                    <td>
                        Appraisals
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("AppraisalNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="aUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="aUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("ApprasialUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Wholesale
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("WholeSaleNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="wUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="wUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("WholeSaleUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Add to Inventory
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("InventoryNotfication")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="iUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="iUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("InventoryUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        24H Appraisal Alert
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("TwentyFourHourNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="24hUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="24hUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("24HUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Note Notifications
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("NoteNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="nUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="nUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("NoteUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Price Changes
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("PriceChangeNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="pUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="pUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("PriceUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ageing/Bucket Jump
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("AgeingBucketJumpNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="agingUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="agingUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("AgeingBucketJumpUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Bucket Jump Report
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("BucketJumpReportNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="bucketJumpUserList" value="Manage Users" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="bucketJumpUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("BucketJumpReportUsersNotificationList")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Market Price Range
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("MarketPriceRangeChangeNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="marketPriceRangeUserList" value="Manage Users" />
                    </td>
                </tr>
             
                <tr>
                    <td colspan="3">
                        <div id="marketPriceRangeUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("MarketPriceRangeNotificationList")%>
                        </div>
                    </td>
                </tr>
                
                    <tr>
                    <td>
                        Image Upload
                    </td>
                    <td>
                        <%=Html.DynamicHtmlLabelAdmin("ImageUploadNotification")%>
                    </td>
                    <td class="fit">
                        <input type="button" name="imageUploadUserList" value="Manage Users" />
                    </td>
                </tr>
                
                
                
                <tr>
                    <td colspan="3">
                        <div id="imageuploadUserList" class="hider">
                            <%=Html.DynamicHtmlLabelAdmin("ImageUploadNotificationList")%>
                        </div>
                    </td>
                </tr>
            </table>
            <h4>
                Ageing/BucketJump<input style="background: #860000; font-weight: bold; padding: .1em;"
                    type="button" name="AgeingNotificationBTN" value=" ? " /></h4>
            <p id="AgeingNotificationInfo" class="subtext hider">
                This section allows you to modify ageing/bucket jump for the inventory.</p>
            <table>
                <tr>
                    <td>
                        First Time Range
                    </td>
                    <td>
                        <%=Html.TextBoxFor(x=>x.FirstRange) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Second Time Range
                    </td>
                    <td>
                        <%=Html.TextBoxFor(x=>x.SecondRange) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Interval
                    </td>
                    <td>
                        <%=Html.DropDownListFor(x=>x.SelectedInterval,Model.IntervalList)%>
                    </td>
                </tr>
            </table>
        </div>
        <% Html.EndForm(); %>
        <%if (Model.LandingPage.Equals("UserRights"))
          {%>
        <div id="userRights">
            <% }
          else
          {%>
            <div id="userRights" class="hider">
                <% }%>
                <div style="width:100%;display:block;">
                <h4>User Rights
                <input style="background: #860000; font-weight: bold; min-width: 0px !important; width: auto !important; padding: .1em;" type="button" name="userRightsBTN" value=" ? " />
                </h4>
                <p id="userRightsInfo" class="subtext hider">
                    This is the user management panel. You can add new users, remove old users or change
                    their privelages/account information. To add a new user simply fill out the top
                    row of the form and click add on the right when finished. To manage older accounts
                    just edit the info and hit the save bar at the bottom of the list.</p>
                <table id="UserRoleTable">
                    <tr>
                        <td class="listHeader" id="headerName">
                            Name
                        </td>
                        <td class="listHeader" id="headerUsername">
                            Username
                        </td>
                        <td class="listHeader" id="headerPassword">
                            Password
                        </td>
                        <td class="listHeader" id="headerEmail">
                            Email
                        </td>
                        <td class="listHeader" id="headerCell">
                            Cell #
                        </td>
                        <td class="listHeader" id="headerUserType">
                            Usertype
                        </td>
                        <td class="listHeader">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="text" id="NewName" name="NewName" value="Name" onfocus="javascript:clearText(this)" />
                        </td>
                        <td>
                            <input type="text" id="NewUsername" name="NewUsername" value="Username" onfocus="javascript:clearText(this)" />
                        </td>
                        <td>
                            <input type="password" id="NewPassword" name="NewPassword" value="Password" onfocus="javascript:clearText(this)" />
                        </td>
                        <td>
                            <input type="text" id="NewEmail" name="NewEmail" value="Email" onfocus="javascript:clearText(this)" />
                        </td>
                        <td>
                            <input type="text" id="NewPhone" name="NewPhone" value="Cell#" onfocus="javascript:clearText(this)" />
                        </td>
                        <td>
                            <select name="UserLevel" id="UserLevel">
                                <option>Manager</option>
                                <option>Employee</option>
                                <option>Admin</option>
                            </select>
                        </td>
                        <%=Html.HiddenFor(x=>x.MutipleDealer) %>
                        <td>
                            <input type="button" name="submit" id="btnAddUser" value=" Add User " />
                        </td>
                    </tr>
                    <tr class="space">
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%=Html.DynamicHtmlLabelAdmin("UsersList")%>
                </table>
                </div>
                <div style="width:100%;display:block;">
                <h4>Permissions
                <input style="background: #860000; font-weight: bold; min-width: 0px !important; width: auto !important; padding: .1em;" type="button" name="userRightsBTN" value=" ? " />
                </h4>
                <%if(Model.ButtonPermissions!=null&& Model.ButtonPermissions.Any()) {%>
                <%--<table>
                <% foreach (var group in Model.ButtonPermissions){%>
                <tr>
                    <td style="background-color:#860000"><%= group.GroupName %></td>
                    <% foreach (var button in group.Buttons){%>
                    <td><span><input type="checkbox" id="chkButtonPermisison_<%= group.GroupId %>_<%= button.ButtonId %>" <%= button.CanSee ? "checked" : "" %> style="float:left;display:inline-block;width:20px;min-width:20px;" /></span> <span><%= button.ButtonName %></span></td>
                    <%}%>
                </tr>
                <%}%>                
                </table>--%>
                <div>
                <% foreach (var group in Model.ButtonPermissions){%>
                <div style="display:inline-block; width:110%;">
                    <div style="background-color:#860000;float:left;display:inline-block;width:70px;padding:2px 4px;"><%= group.GroupName %></div>
                    <div style="float:left;display:inline-block;margin-left:10px;width:700px;font-size:0.85em;">
                    <% foreach (var button in group.Buttons){%>
                    <% if ((SessionHandler.CanViewBucketJumpReport != null && SessionHandler.CanViewBucketJumpReport.Value && button.ButtonName.Equals(WhitmanEnterpriseMVC.HelperClass.Constanst.ProfileButton.BucketJumpTracking))
                           || (SessionHandler.CanViewBucketJumpReport != null && !button.ButtonName.Equals(WhitmanEnterpriseMVC.HelperClass.Constanst.ProfileButton.BucketJumpTracking))
                           ) {%>
                    <div style="float:left;display:inline-block;width:auto;">
                    <span>
                    <input type="checkbox" id="chkButtonPermisison_<%= group.GroupId %>_<%= button.ButtonId %>" <%= button.CanSee ? "checked" : "" %> style="float:left;display:inline-block;width:20px;min-width:20px;" />
                    </span> 
                    <span><%= button.ButtonName %></span>
                    </div>
                    <%}%>
                    <%}%>
                    </div>
                </div>
                <%}%>                
                </div>
                <%}%>
                </div>
            </div>
            <div id="buyerGuide_backup" class="hider">
                <div style="margin-top: 20px;">
                    <%--<div style="display:inline-block; width: 750px;">
                    <div style="float:left;margin-right: 30px;"><%= Html.TextAreaFor(m => m.BuyerGuide1, new { cols = "110" })%></div>
                    <div style="float:left;"><%= Html.TextAreaFor(m => m.BuyerGuide2, new { cols = "110" })%></div>
                    </div>
                    <div style="display:inline-block; width: 750px; margin-top: 20px;">
                    <div style="float:left; margin-right: 30px;"><%= Html.TextAreaFor(m => m.BuyerGuide3, new { cols = "110" })%></div>
                    <div style="float:left;"><%= Html.TextAreaFor(m => m.BuyerGuide4, new { cols = "110" })%></div>
                    </div>--%>
                    <div style="display: inline-block; width: 750px;">
                        <div>
                            <a href="javascript:;" id="aBuyerGuide1">Buyer Guide 1</a></div>
                        <div id="divBuyerGuide1" style="display: none;">
                            <%= Html.TextAreaFor(m => m.BuyerGuide1, new { cols = "110" })%></div>
                        <div>
                            <a href="javascript:;" id="aBuyerGuide2">Buyer Guide 2</a></div>
                        <div id="divBuyerGuide2" style="display: none;">
                            <%= Html.TextAreaFor(m => m.BuyerGuide2, new { cols = "110" })%></div>
                        <div>
                            <a href="javascript:;" id="aBuyerGuide3">Buyer Guide 3</a></div>
                        <div id="divBuyerGuide3" style="display: none;">
                            <%= Html.TextAreaFor(m => m.BuyerGuide3, new { cols = "110" })%></div>
                        <div>
                            <a href="javascript:;" id="aBuyerGuide4">Buyer Guide 4</a></div>
                        <div id="divBuyerGuide4" style="display: none;">
                            <%= Html.TextAreaFor(m => m.BuyerGuide4, new { cols = "110" })%></div>
                    </div>
                </div>
            </div>
            <!-- JAVASCRIPT FOR BUTTONS -->

            <script language="javascript">

                $(window).load(
                        function () {
                            $("#EnableAutoDescription").click(function () {
                                if ('<%=Model.AutoDescriptionSubscribe %>'.toLowerCase() == 'false') {
                                    if ($(this).is(':checked')) {
                                        alert('In order to enable auto description, please contact 1.855.VIN.CTRL.');
                                        $(this).attr('checked', false);
                                    }
                                }
                            });
                            var defaultStockImage = "";
                            $("#<%=FileUpload.ClientID%>").fileUpload({
                                'uploader': '<%= Url.Content("~/Scripts/uploader.swf") %>',
                                'cancelImg': '<%= Url.Content("~/content/images/cancel.png") %>',
                                'script': '<%= Url.Content("~/Handlers/StockImageUpload.ashx") %>',
                                'folder': '<%= Url.Content("~/UploadImages") %>',
                                'fileDesc': 'Image Files',
                                'fileExt': '*.jpg;*.jpeg;*.gif;*.png',
                                'queueSizeLimit': 40,
                                'sizeLimit': 10240000,
                                'buttonText': 'Upload Files',
                                'displayData': 'percentage',
                                'scriptData': { 'DealerId': $("#DealershipId").val() },
                                'multi': true,
                                'auto': true,
                                'simUploadLimit': 1,
                                'onQueueFull': function (event, queueSizeLimit) {
                                    $('#dialog').html("Sorry You Are only allowed to upload a maximuim of (40) images at a time!");
                                    return false;
                                },
                                'onProgress': function (event, ID, fileObj, data) {
                                    $("#progressbar").progressbar("value", 50);
                                },

                                'onError': function (a, b, c, d) {
                                    if (d.status == 404)
                                        alert("Could not find upload script. Use a path relative to: " + "<?= getcwd() ?>");
                                    else if (d.type === "HTTP")
                                        alert("error " + d.type + ": " + d.status);
                                    else if (d.type === "File Size")
                                        alert(c.name + " " + d.type + " Limit: " + Math.round(d.sizeLimit / 1024) + "KB");
                                    else
                                        alert("error " + d.type + ": " + d.text);
                                },

                                'onComplete': function (event, ID, fileObj, response, data) {

                                    defaultStockImage = response.toString();
                                    $("#DefaultStockImage").attr("src", defaultStockImage);


                                },
                                'onAllComplete': function (event, data) {

                                    $.post('<%= Url.Content("~/Admin/UpdateDefaultStockImage") %>', { DefaultStockImageUrl: defaultStockImage }, function (data) {

                                    });

                                }
                            });


                        });



                function updatePass(txtBox) {



                    $.post('<%= Url.Content("~/Admin/UpdatePassword") %>', { Pass: txtBox.value, Username: txtBox.id }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }

                    });

                }

                function updateEmail(txtBox) {
                    $.post('<%= Url.Content("~/Admin/UpdateEmail") %>', { Email: txtBox.value, Username: txtBox.id }, function(data) {
                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }

                    });
                }
                function updatePhone(txtBox) {
                    $.post('<%= Url.Content("~/Admin/UpdateCellPhone") %>', { CellPhone: txtBox.value, Username: txtBox.id }, function(data) {
                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }
                    });
                }


                function changeRole(selectBox) {

                    $.post('<%= Url.Content("~/Admin/ChangeRole") %>', { Role: selectBox.value, Username: selectBox.id }, function(data) {
                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }
                    });
                }

                function appraisalNotify(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 0 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
               

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("AppraisalCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("AppraisalCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }



                }

                function appraisalNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 0 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }



                function WholesaleNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 1 }, function(data) {
                        console.log(data);
                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                  

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("WholeSaleCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("WholeSaleCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }

                }



                function wholeSaleNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 1 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }





                function InventoryNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 2 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
         

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("InventoryCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("InventoryCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }

                }






                function inventoryNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 2 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }








                function TwentyFourHourNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 3 }, function(data) {
                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }

                    });

                 

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("24HCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("24HCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }

                }




                function twentyfourhourNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 3 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }






                function NoteNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 4 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("NoteCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("NoteCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }

                   
                }



                function noteNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 4 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }

                    });
                }





                function PriceNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 5 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                   
                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("PriceCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("PriceCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }

                }

                function MarketPriceRangeNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 7 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("MarketPriceRangeCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("MarketPriceRangeCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }


                    

                }

                function priceNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 5 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function marketPriceRangeNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 7 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function AgeNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 6 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("AgeCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("AgeCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }

               

                }

                function BucketJumpReportNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 8 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("BucketJumpCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("BucketJumpCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }


                            }

                        }
                    }


                 
                }


                function ImageUploadNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 9 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                    var checks = $('input[type="checkbox"]');
                   
                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                              if (checks[i].id.indexOf("ImageUploadCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }
                                
                            }


                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("ImageUploadCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }
                              

                            }

                        }
                    }

                  
                }

                function imageUploadNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 9 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function ageNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 6 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function bucketJumpReportNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, Username: checkbox.id, Notificationkind: 8 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function retailPriceWindowStickerNotify(checkbox) {

                    $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: 1 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function dealerDiscountWindowStickerNotify(checkbox) {

                    $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: 2 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function manufacturerRebateWindowStickerNotify(checkbox) {

                    $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: 3 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }

                function salePriceWindowStickerNotify(checkbox) {

                    $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: 4 }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }


                    });
                }


                function OverideStockImage(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateOverideStockImage") %>', { OverideStockImage: checkbox.checked }, function(data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }

                    });
                }


                $(document).ready(function () {
                    var Make = $("#SelectedMake");
                    var Year = $("#SelectedYear");
                    var Model = $("#SelectedModel");
                    var Trim = $("#SelectedTrim");
                    var BodyType = $("#SelectedBodyType");

                    Year.change(function () {

                        if (Year.val() != "Year") {
                            //alert(Year.val());
                            Make.html("");
                            Model.html("");
                            Trim.html("");
                            $.post('<%= Url.Content("~/Admin/YearAjaxPost") %>', { YearId: Year.val() }, function (data) {
                                //console.log(data);
                                var itemsMake = "<option value='" + 0 + "****" + "Make..." + "'>" + "Make..." + "</option>";

                                var itemsModel = "<option value='" + 0 + "****" + "Model..." + "'>" + "Model..." + "</option>";

                                var makeList = data.MakeList;

                                var modelList = data.ModelList;



                                if (makeList != null) {
                                    if (makeList.length == 1) {
                                        for (var i = 0; i < makeList.length; i++) {
                                            itemsMake = "<option value='" + makeList[i] + "'>" + makeList[i] + "</option>";
                                        }
                                    } else {
                                        for (i = 0; i < makeList.length; i++) {
                                            itemsMake += "<option value='" + makeList[i] + "'>" + makeList[i] + "</option>";

                                        }
                                    }
                                }
                                if (modelList != null) {
                                    for (var i = 0; i < modelList.length; i++) {
                                        itemsModel += "<option value='" + modelList[i] + "'>" + modelList[i] + "</option>";
                                    }

                                }

                                Make.html(itemsMake);
                                Model.html(itemsModel);
                                //                                        Year.removeAttr('disabled');
                                //                                        Make.removeAttr('disabled');
                                //                                        Model.removeAttr('disabled');
                                //                                        Trim.removeAttr('disabled');

                            });
                        }
                    });


                    Model.change(function () {

                        if (Model.val() != "0****Model...") {

                            Trim.html("");

                            $.post('<%= Url.Content("~/Admin/ModelAjaxPost") %>', { YearId: Year.val(), MakeId: Make.val(), ModelId: Model.val() }, function (data) {

                                var itemTrims = "<option value='" + 0 + "****" + "Trim..." + "'>" + "Trim..." + "</option>";

                                itemTrims += "<option value='All Trims'>" + "All Trims" + "</option>";

                                var itemBodyTypes = "<option value='" + 0 + "****" + "Body..." + "'>" + "Body..." + "</option>";

                                var trimList = data.TrimList;

                                var bodyTypeList = data.BodyTypeList;



                                if (trimList != null) {

                                    if (trimList.length == 1) {
                                        for (var i = 0; i < trimList.length; i++) {
                                            if (trimList[i] == "")
                                                itemTrims = "<option value='" + trimList[i] + "'>Unspecified</option>";
                                            else
                                                itemTrims = "<option value='" + trimList[i] + "'>" + trimList[i] + "</option>";
                                        }
                                    } else {
                                        for (i = 0; i < trimList.length; i++) {
                                            if (trimList[i] == "")
                                                itemTrims += "<option value='" + trimList[i] + "'>Unspecified</option>";
                                            else
                                                itemTrims += "<option value='" + trimList[i] + "'>" + trimList[i] + "</option>";

                                        }
                                    }

                                }
                                if (bodyTypeList != null) {

                                    if (bodyTypeList.length == 1) {
                                        for (var i = 0; i < bodyTypeList.length; i++) {
                                            itemBodyTypes = "<option value='" + bodyTypeList[i] + "'>" + bodyTypeList[i] + "</option>";
                                        }
                                    } else {
                                        for (i = 0; i < bodyTypeList.length; i++) {
                                            itemBodyTypes += "<option value='" + bodyTypeList[i] + "'>" + bodyTypeList[i] + "</option>";

                                        }
                                    }


                                }

                                Trim.html(itemTrims);
                                BodyType.html(itemBodyTypes);

                            });
                        }

                    });

                    $("#btnAddManuRebate").click(function (event) {

                        if (Trim.val() != null && Trim.val() != "0****Trim...") {
                            if (BodyType.val() != null && BodyType.val() != "0****Body...") {

                                $('#elementID').removeClass('hideLoader');
                                $.post('<%= Url.Content("~/Admin/ApplyReabte") %>', { YearId: Year.val(), MakeId: Make.val(), ModelId: Model.val(), TrimId: Trim.val(), BodyType: BodyType.val(), RebateAmount: $("#rebateamount").val(), Disclaimer: $("#disclaimerrebate").val() }, function (data) {

                                    if (data != "Error") {
                                        for (var i = 0; i < data.length; i++) {
                                            var sb = new StringBuilder();
                                            var actionUrl = '<%= Url.Action("ViewRebateReportByTrim", "Report", new { RebateId = "PLACEHOLDER"  } ) %>';
                                            actionUrl = actionUrl.replace('PLACEHOLDER', data[i].UniqueId);
                                            sb.appendLine("<tr>");
                                            sb.appendLine("   <td>" + Year.val() + "</td>");
                                            sb.appendLine("  <td>" + Make.val() + "</td>");
                                            sb.appendLine(" <td>" + Model.val() + "</td>");
                                            sb.appendLine("  <td>" + data[i].Trim + "</td>");
                                            sb.appendLine(" <td>" + BodyType.val() + "</td>");
                                            sb.appendLine("<td title='Manufaturer Rebate'><input type='text' value='" + $("#rebateamount").val() + "'/></td>");
                                            sb.appendLine("  <td title='Click to Add a Disclaimer'><textarea cols='18' name='DealerWarranty' rows='2'>" + $("#disclaimerrebate").val() + "</textarea></td>");
                                            sb.appendLine(" <td><a href='" + actionUrl + "' target='_blank'>Report</a></td>");
                                            sb.appendLine(" <td><input type='button' value='Delete' id='" + data[i].UniqueId + "' onclick='javascript:deleteRebate(this);' /></td>");
                                            sb.appendLine("</tr>");
                                            $("#discounts").append(sb.toString());
                                        }
                                        Year.val("Year");
                                        Make.html("");
                                        Model.html("");
                                        Trim.html("");
                                        BodyType.html("");
                                        $("#rebateamount").val("");
                                        $("#disclaimerrebate").val("");

                                    } else {
                                        alert("Your session has timed out. Please login back again");
                                        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                                        window.parent.location = actionUrl;

                                    }
                                    $('#elementID').addClass('hideLoader');
                                });

                            }
                            else {
                                alert("Please select a body type");
                            }
                        }
                        else {
                            alert("Please select a trim");
                        }

                    });

                    $("#rebateamount").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

                    $("#FirstRange").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

                    $("#btnAddUser").click(function (event) {

                        if ($("#CusErrorIn").length > 0) {
                            $("#CusErrorIn").remove();
                        }

                        var flag = true;
                        var errorString = "<strong id='CusErrorIn'><font color='Red'  size='2' >There are some following errors : <br><ul>";

                        if ($("#NewName").val() == "Name" || $("#NewName").val() == "") {
                            errorString += "<li>Name is required</li>";

                        }

                        if ($("#NewUsername").val() == "Username" || $("#NewUsername").val() == "") {
                            errorString += "<li>Username is required</li>";

                        }

                        if ($("#NewPassword").val() == "Password" || $("#NewPassword").val() == "") {
                            errorString += "<li>Password is required</li>";

                        }

                        if ($("#NewEmail").val() == "Email" || $("#NewEmail").val() == "") {
                            errorString += "<li>Email is required</li>";

                        }

                        if ($("#NewPhone").val() == "Cell#" || $("#NewPhone").val() == "") {
                            errorString += "<li>Cellphone is required</li>";

                        }

                        if (errorString != "<strong id='CusErrorIn'><font color='Red'  size='2' >There are some following errors : <br><ul>") {
                            errorString += "</ul></font></strong>";
                            $("#userRights").prepend(errorString);
                            flag = false;

                        } else {

                            $.post('<%= Url.Content("~/Admin/CheckUserExist") %>', { UserName: $("#NewUsername").val(), DealerId: '<%=SessionHandler.DealerId %>' }, function (user) {


                                $('#elementID').addClass('hideLoader');
                                if (user.SessionTimeOut == "TimeOut") {
                                    flag = false;
                                    alert("Your session is timed out. Please login back again");
                                    var actionLogOutUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                                    window.parent.location = actionLogOutUrl;
                                } else if (user.IsUserExist == "Exist") {
                                    flag = false;
                                    errorString += "<li>This username already existed</li>";
                                    errorString += "</ul></font></strong>";
                                    $("#userRights").prepend(errorString);


                                }


                                if (flag) {


                                    if ($('#MutipleDealer').val() == "True") {
                                        var actionUrl = '<%= Url.Action("ChoooseDealerForUser", "Admin" ) %>';


                                        $("<a href=" + actionUrl + "></a>").fancybox({
                                            overlayShow: true,
                                            showCloseButton: true,
                                            enableEscapeButton: true,
                                            width: 600,
                                            height: 500,
                                            onClosed: function () {
                                                window.location.reload(true);
                                            }
                                        }).click();

                                    } else {

                                        $('#elementID').removeClass('hideLoader');
                                        $.post('<%= Url.Content("~/Admin/AddSingleUser") %>', { Name: $("#NewName").val(), UserName: $("#NewUsername").val(), Password: $("#NewPassword").val(), Email: $("#NewEmail").val(), CellPhone: $("#NewPhone").val(), UserLevel: $("#UserLevel").val() }, function (user) {
                                            $('#elementID').addClass('hideLoader');
                                            if (user.SessionTimeOut == "TimeOut") {
                                                alert("Your session is timed out. Please login back again");
                                                var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                                                window.parent.location = actionUrl;
                                            } else if (user.IsUserExist == "Exist") {
                                                alert("Username existed. Please choose another username");

                                            } else {

                                                var item = "";

                                                var rowCount = $('#UserRoleTable tr').length;
                                                if (rowCount % 2 == 0)
                                                    item += "<tr>";
                                                else
                                                    item += "<tr class=\"l\">";

                                                item += "<td>" + user.Name + "</td>" + "<td>" + user.UserName + "</td>" + "<td><input type=\"password\" id=\"" + user.UserName + "\" name=\"pass\" value=\"" + user.PassWord + "\" onblur=\"javascript:updatePass(this);\"/></td>";
                                                item += "<td><input type=\"text\" id=\"" + user.UserName + "\" name=\"email\" value=\"" + user.Email + "\" onblur=\"javascript:updateEmail(this);\"/></td>";
                                                item += "<td><input type=\"text\" id=\"" + user.UserName + "\" name=\"phone\"  value=\"" + user.Cellphone + "\" onblur=\"javascript:updatePhone(this);\"/></td>";
                                                item += "<td><select  id=\"" + user.UserName + "\"  name=\"userlevel\" onchange=\"javascript:changeRole(this);\">";

                                                if (user.Role == "Manager") {
                                                    item += "<option selected=\"selected\" value=\"Manager\" >Manager</option>";
                                                    item += "<option value=\"Employee\" >Employee</option>";
                                                    item += "<option value=\"Admin\" >Admin</option>";
                                                } else if (user.Role == "Admin") {
                                                    item += "<option selected=\"selected\" value=\"Admin\" >Admin</option>";
                                                    item += "<option value=\"Manager\" >Manager</option>";
                                                    item += "<option value=\"Employee\" >Employee</option>";
                                                } else {
                                                    item += "<option selected=\"selected\" value=\"Employee\" >Employee</option>";
                                                    item += "<option value=\"Admin\" >Admin</option>";
                                                    item += "<option value=\"Manager\" >Manager</option>";
                                                }
                                                item += "</select>";

                                                item += "<br/></td>";
                                                item += "<td><input class=\"deleteBTN\" type=\"button\" id=\"" + user.UserName + "\" name=\"delete\" value=\"Delete\" onclick=\"javascript:deleteUser(this);\" /></td>";

                                                item += "</tr>";

                                                $("#UserRoleTable").last().append(item);
                                                $("#aUserList").append("<ul class=\"column\"> <li id=\"Appraisal" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");
                                                $("#wUserList").append("<ul class=\"column\"> <li id=\"WholeSale" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");
                                                $("#iUserList").append("<ul class=\"column\"> <li id=\"Inventory" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");
                                                $("#24hUserList").append("<ul class=\"column\"> <li id=\"24H" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");
                                                $("#nUserList").append("<ul class=\"column\"> <li id=\"Note" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");
                                                $("#pUserList").append("<ul class=\"column\"> <li id=\"Price" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");
                                                $("#marketPriceRangeUserList").append("<ul class=\"column\"> <li id=\"MarketPriceRange" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" /> " + user.UserName + " </li>" + "</ul>");

                                                $("#NewName").val("Name");
                                                $("#NewUsername").val("UserName");
                                                $("#NewPassword").val("Password");
                                                $("#NewEmail").val("Email");
                                                $("#NewPhone").val("Cell#");


                                            }
                                        });

                                    }

                                }

                            });
                        }

                    });

                    $('input[id^=chkButtonPermisison_]').click(function () {
                        var groupId = this.id.split('_')[1];
                        var buttonId = this.id.split('_')[2];
                        var canSee = $(this).is(':checked') ? 'True' : 'False';
                        $.ajax({
                            type: "GET",
                            url: "/Admin/UpdateButtonPermission?groupId=" + groupId + "&buttonId=" + buttonId + "&canSee=" + canSee,
                            data: {},
                            success: function (results) {

                            }
                        });
                    });


                });



                function deleteUser(user) {


                    var answer = confirm("Are you sure you want to delete selected user?");

                    if (answer) {
                        $('#elementID').removeClass('hideLoader');
                        $.post('<%= Url.Content("~/Admin/DeleteUser") %>', { Username: user.id }, function(data) {

                            document.getElementById('UserRoleTable').deleteRow(user.parentNode.parentNode.rowIndex);

                            $("#Appraisal" + user.id).remove();
                            $("#WholeSale" + user.id).remove();
                            $("#Inventory" + user.id).remove();
                            $("#24H" + user.id).remove();
                            $("#Note" + user.id).remove();
                            $("#Price" + user.id).remove();

                            $('#elementID').addClass('hideLoader');

                        });
                    }
                }

                function deleteRebate(rebate) {


                    var answer = confirm("Are you sure you want to delete this rebate?");

                    if (answer) {
                        $('#elementID').removeClass('hideLoader');



                        $.post('<%= Url.Content("~/Admin/DeleteRebate") %>', { rebateId: rebate.id }, function(data) {

                            document.getElementById('discounts').deleteRow(rebate.parentNode.parentNode.rowIndex);

                        });

                        $('#elementID').addClass('hideLoader');
                    }
                }

                function editUser(user) {


                    $('#elementID').removeClass('hideLoader');
                    var actionUrl = '<%= Url.Action("EditUserForDefaultLogin", "Admin",new { username = "PLACEHOLDER" } ) %>';
                    actionUrl = actionUrl.replace('PLACEHOLDER', user.id);

                    $("<a href=" + actionUrl + "></a>").fancybox({
                        overlayShow: true,
                        showCloseButton: true,
                        enableEscapeButton: true,
                        width: 600,
                        height: 500,
                        onClosed: function() {
                            actionUrl = '<%= Url.Action("AdminSecurityLanding", "Admin",new { LandingPage = "PLACEHOLDER" } ) %>';
                            actionUrl = actionUrl.replace('PLACEHOLDER', 'UserRights');
                            window.location = actionUrl;
                        }
                    }).click();


                }


                $('#DeleteStock').click(function() {

                    console.log("Delete");
                    $('#elementID').removeClass('hideLoader');

                    $.post('<%= Url.Content("~/Admin/DeleteStockImage") %>', function(data) {
                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session is timed out. Please login back again");
                            var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                            window.parent.location = actionUrl;
                        }
                        else {
                            $('#DefaultStockImage').attr("src", "");
                        }
                        $('#elementID').addClass('hideLoader');

                    });
                });


                $('input[name="tradeinpricevarianceBTN"]').click(function() {
                    $("#tradeinpricevarianceInfo").slideToggle("slow");

                    $("#managecommentsInfo").slideUp("slow");
                    $("#addcommentInfo").slideUp("slow");
                });

                $('input[name="managecommentsBTN"]').click(function() {
                    $("#managecommentsInfo").slideToggle("slow");

                    $("#tradeinpricevarianceInfo").slideUp("slow");
                    $("#addcommentInfo").slideUp("slow");
                });

                $('input[name="addcommentBTN"]').click(function() {
                    $("#addcommentInfo").slideToggle("slow");

                    $("#tradeinpricevarianceInfo").slideUp("slow");
                    $("#managecommentsInfo").slideUp("slow");
                });


                $('input[name="ebayCraigsBTN"]').click(function() {
                    $("#ebayCraigsInfo").slideToggle("slow");
                    $("#inventorySortInfo").slideUp("slow");
                    $("#loginCredentialsInfo").slideUp("slow");
                    $("#soldActionInfo").slideUp("slow");
                    $("#discountsAndRebatesInfo").slideUp("slow");
                    $("#buyerguideinfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideUp("slow");
                });

                $('input[name="autoDescriptionBTN"]').click(function () {
                    $("#ebayCraigsInfo").slideUp("slow");
                    $("#inventorySortInfo").slideUp("slow");
                    $("#loginCredentialsInfo").slideUp("slow");
                    $("#soldActionInfo").slideUp("slow");
                    $("#discountsAndRebatesInfo").slideUp("slow");
                    $("#buyerguideinfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideToggle("slow");

                });

                $('input[name="inventorySortBTN"]').click(function() {
                    $("#inventorySortInfo").slideToggle("slow");
                    $("#ebayCraigsInfo").slideUp("slow");
                    $("#loginCredentialsInfo").slideUp("slow");
                    $("#soldActionInfo").slideUp("slow");
                    $("#discountsAndRebatesInfo").slideUp("slow");
                    $("#buyerguideinfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideUp("slow");
                });

                $('input[name="loginCredentialsBTN"]').click(function() {
                    $("#loginCredentialsInfo").slideToggle("slow");
                    $("#ebayCraigsInfo").slideUp("slow");
                    $("#inventorySortInfo").slideUp("slow");
                    $("#soldActionInfo").slideUp("slow");
                    $("#discountsAndRebatesInfo").slideUp("slow");
                    $("#buyerguideinfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideUp("slow");
                });
                $('input[name="soldActionBTN"]').click(function() {
                    $("#soldActionInfo").slideToggle("slow");
                    $("#ebayCraigsInfo").slideUp("slow");
                    $("#inventorySortInfo").slideUp("slow");
                    $("#loginCredentialsInfo").slideUp("slow");
                    $("#discountsAndRebatesInfo").slideUp("slow");
                    $("#buyerguideinfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideUp("slow");

                });
                $('input[name="buyerGuideBTN"]').click(function() {
                    $("#buyerguideinfo").slideToggle("slow");
                    $("#soldActionInfo").slideUp("slow");
                    $("#ebayCraigsInfo").slideUp("slow");
                    $("#inventorySortInfo").slideUp("slow");
                    $("#loginCredentialsInfo").slideUp("slow");
                    $("#discountsAndRebatesInfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideUp("slow");
                });
                $('input[name="discountsAndRebatesBTN"]').click(function() {
                    $("#discountsAndRebatesInfo").slideToggle("slow");
                    $("#soldActionInfo").slideUp("slow");
                    $("#ebayCraigsInfo").slideUp("slow");
                    $("#inventorySortInfo").slideUp("slow");
                    $("#loginCredentialsInfo").slideUp("slow");
                    $("#buyerguideinfo").slideUp("slow");
                    $("#autoDescriptionInfo").slideUp("slow");
                });

                $('input[name="wholesaleBTN"]').click(function() {
                    $("#wholesaleInfo").slideToggle("slow");

                });
                $('input[name="notificationsBTN"]').click(function() {
                    $("#notificationsInfo").slideToggle("slow");

                });
                $('input[name="AgeingNotificationBTN"]').click(function() {
                    $("#AgeingNotificationInfo").slideToggle("slow");

                });
                $('input[name="userRightsBTN"]').click(function() {
                    $("#userRightsInfo").slideToggle("slow");

                });


                $('input[name="inventory"]').click(function() {
                    $("#contentBTN").slideDown("slow");
                    $(this).addClass("onadmin");
                    $('#topNav input[type="button"]').not(this).removeClass('on');
                    $("#notifications").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $("#wholesale").slideUp("slow");
                    $("#dealerInfo").slideUp("slow");
                    $("#priceInfo").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideUp("slow");
                });

                $('input[name="dealerInfo"]').click(function() {
                    $("#contentBTN").slideUp("slow");
                    $(this).addClass("onadmin");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                    $("#notifications").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $("#wholesale").slideUp("slow");
                    $("#dealerInfo").slideDown("slow");
                    $("#priceInfo").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideUp("slow");
                });


                $('input[name="wholesale"]').click(function() {
                    $("#wholesale").slideDown("slow");
                    $("#contentBTN").slideUp("slow");
                    $("#notifications").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $(this).addClass("onadmin");
                    $("#dealerInfo").slideUp("slow");
                    $("#priceInfo").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideUp("slow");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                });

                $('input[name="notifications"]').click(function() {
                    $("#notifications").slideDown("slow");
                    $("#wholesale").slideUp("slow");
                    $("#contentBTN").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $(this).addClass("onadmin");
                    $("#dealerInfo").slideUp("slow");
                    $("#priceInfo").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideUp("slow");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                });
                $('input[name="userRIghts"]').click(function() {
                    $("#userRights").slideDown("slow");
                    $("#wholesale").slideUp("slow");
                    $("#notifications").slideUp("slow");
                    $("#contentBTN").slideUp("slow");
                    $(this).addClass("onadmin");
                    $("#dealerInfo").slideUp("slow");
                    $("#priceInfo").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideUp("slow");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                });

                $('input[name="priceInfo"]').click(function() {
                    $("#priceInfo").slideDown("slow");
                    $("#wholesale").slideUp("slow");
                    $("#notifications").slideUp("slow");
                    $("#contentBTN").slideUp("slow");
                    $(this).addClass("onadmin");
                    $("#dealerInfo").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideUp("slow");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                });

                $('input[name="buyerGuide"]').click(function() {
                    $("#priceInfo").slideUp("slow");
                    $("#wholesale").slideUp("slow");
                    $("#notifications").slideUp("slow");
                    $("#contentBTN").slideUp("slow");
                    $(this).addClass("onadmin");
                    $("#dealerInfo").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $("#buyerGuide").slideDown("slow");
                    $("#tradeIn").slideUp("slow");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                });

                $('input[name="tradeIn"]').click(function() {
                    $("#priceInfo").slideUp("slow");
                    $("#wholesale").slideUp("slow");
                    $("#notifications").slideUp("slow");
                    $("#contentBTN").slideUp("slow");
                    $(this).addClass("onadmin");
                    $("#dealerInfo").slideUp("slow");
                    $("#userRights").slideUp("slow");
                    $("#buyerGuide").slideUp("slow");
                    $("#tradeIn").slideDown("slow");
                    $('#topNav input[type="button"]').not(this).removeClass('onadmin');
                });

                $('input[name="aUserList"]').click(function() {
                    $("#aUserList").slideToggle("slow");

                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#iUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");
                });

                $('input[name="pUserList"]').click(function() {
                    $("#pUserList").slideToggle("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#iUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");
                });

                $('input[name="marketPriceRangeUserList"]').click(function() {
                    $("#marketPriceRangeUserList").slideToggle("slow");
                    $("#pUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#iUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");
                });

                $('input[name="nUserList"]').click(function() {
                    $("#nUserList").slideToggle("slow");

                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#iUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");
                });

                $('input[name="24hUserList"]').click(function() {
                    $("#24hUserList").slideToggle("slow");

                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#iUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                });

                $('input[name="iUserList"]').click(function() {
                    $("#iUserList").slideToggle("slow");

                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");

                });

                $('input[name="agingUserList"]').click(function() {
                    $("#agingUserList").slideToggle("slow");

                    $("#iUserList").slideUp("slow");

                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");
                });

                $('input[name="bucketJumpUserList"]').click(function () {

                    $("#bucketJumpUserList").slideToggle("slow");
                    $("#iUserList").slideUp("slow");
                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#imageuploadUserList").slideUp("slow");
                });


                $('input[name="imageUploadUserList"]').click(function () {

                    $("#imageuploadUserList").slideToggle("slow");
                    $("#iUserList").slideUp("slow");
                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                    $("#wUserList").slideUp("slow");
                    $("#agingUserList").slideUp("slow");
                    $("#bucketJumpUserList").slideUp("slow");
                });


                $('input[name="defaultStockImageBTN"]').click(function() {
                    $("#defaultStockImageInfo").slideToggle("slow");
                });

                $('input[name="wUserList"]').click(function() {
                    $("#wUserList").slideToggle("slow");

                    $("#pUserList").slideUp("slow");
                    $("#marketPriceRangeUserList").slideUp("slow");
                    $("#nUserList").slideUp("slow");
                    $("#24hUserList").slideUp("slow");
                    $("#iUserList").slideUp("slow");
                    $("#aUserList").slideUp("slow");
                });

                $('#clListBTN').click(function() {
                    $('#clList').slideToggle('slow');
                    $('.thirdPartList').slideToggle('slow');
                });

                $('#aBuyerGuide1').click(function() {
                    $('#divBuyerGuide4').hide();
                    $('#divBuyerGuide2').hide();
                    $('#divBuyerGuide3').hide();
                    $('#divBuyerGuide1').slideToggle('slow');

                });

                $('#aBuyerGuide2').click(function() {
                    $('#divBuyerGuide1').hide();
                    $('#divBuyerGuide4').hide();
                    $('#divBuyerGuide3').hide();
                    $('#divBuyerGuide2').slideToggle('slow');

                });

                $('#aBuyerGuide3').click(function() {
                    $('#divBuyerGuide1').hide();
                    $('#divBuyerGuide2').hide();
                    $('#divBuyerGuide4').hide();
                    $('#divBuyerGuide3').slideToggle('slow');

                });

                $('#aBuyerGuide4').click(function() {
                    $('#divBuyerGuide1').hide();
                    $('#divBuyerGuide2').hide();
                    $('#divBuyerGuide3').hide();
                    $('#divBuyerGuide4').slideToggle('slow');

                });

                function HideBuyerGuideDivs() {
                    $('#divBuyerGuide1').hide();
                    $('#divBuyerGuide2').hide();
                    $('#divBuyerGuide3').hide();
                    $('#divBuyerGuide4').hide();
                }

                function StringBuilder() {
                    var strings = [];

                    this.append = function(string) {
                        string = verify(string);
                        if (string.length > 0) strings[strings.length] = string;
                    };

                    this.appendLine = function(string) {
                        string = verify(string);
                        if (this.isEmpty()) {
                            if (string.length > 0) strings[strings.length] = string;
                            else return;
                        } else strings[strings.length] = string.length > 0 ? "\r\n" + string : "\r\n";
                    };

                    this.clear = function() { strings = []; };

                    this.isEmpty = function() { return strings.length == 0; };

                    this.toString = function() { return strings.join(""); };

                    var verify = function(string) {
                        if (!defined(string)) return "";
                        if (getType(string) != getType(new String())) return String(string);
                        return string;
                    };

                    var defined = function(el) {
                        // Changed per Ryan O'Hara's comment:
                        return el != null && typeof (el) != "undefined";
                    };

                    var getType = function(instance) {
                        if (!defined(instance.constructor)) throw Error("Unexpected object type");
                        var type = String(instance.constructor).match(/function\s+(\w+)/);

                        return defined(type) ? type[1] : "undefined";
                    };
                }

                ;

                function updateRebateAmount(txtBox) {


                    $.post('<%= Url.Content("~/Admin/UpdateRebateAmount") %>', { RebateAmount: txtBox.value, RebateId: txtBox.id }, function(data) {


                    });
                }


                function updateRebateDisclaimer(txtBox) {

                    $.post('<%= Url.Content("~/Admin/UpdateRebateDisclaimer") %>', { RebateDisclaimer: txtBox.value, RebateId: txtBox.id }, function(data) {



                    });

                }
                $('a:not(.iframe)').click(function(e) {
                    if ($(this).attr('target') == '')
                        $('#elementID').removeClass('hideLoader');

                });

                $('a#aAsIs').click(function(e) {
                    $(this).fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                });
                                
                $("a.iframe").fancybox({ 'width': 800, 'height': 600, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                $("a.fancybox").fancybox();

                $("a[id^='English_']").live('mouseover focus', function (e) {
                    $(this).fancybox({
                        'width': 1000,
                        'height': 700,
                        'hideOnOverlayClick': false,
                        'centerOnScroll': true,
                        'onCleanup': function () {
                        }
                    });
                });

                $("a[id^='Spanish_']").live('mouseover focus', function (e) {
                    $(this).fancybox({
                        'width': 1000,
                        'height': 700,
                        'hideOnOverlayClick': false,
                        'centerOnScroll': true,
                        'onCleanup': function () {
                        }
                    });
                });

                $(document).ready(function() {
                    $("#btnAdd").click(function() {
                        $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                        $.ajax({
                            type: "POST",
                            //url: "/TradeIn/AddComment?city=1&state=1&comment=1",
                            url: "/TradeIn/AddComment",
                            data: { city: $("#City").val(), state: $("#State").val(), comment: $("#Content").val(), name: $("#Name").val() },
                            success: function(results) {
                                $("#result").html(results);
                                 $.unblockUI();
                            }
                        });
                    });

                    $("#Variance").blur(function() {
                        if ($("#Variance").val() == '') {
                            $("#Variance").val(0);
                        }
                        if (!IsNumeric($("#Variance").val())) {
                            $("#lbVarianceValidate").show();
                        }
                        else {
                            $("#lbVarianceValidate").hide();
                        }
                    });

                    $("input[id^=btnSave]").live('click', function() {
                        var idValue = this.id.split('_')[1];
                        var cityValue = $("#txtCity_" + idValue).val();
                        var stateValue = $("#txtState_" + idValue).val();
                        var contentValue = $("#txtContent_" + idValue).val();
                        var nameValue = $("#txtName_" + idValue).val();
                        $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                        $.ajax({
                            type: "POST",
                            url: "/TradeIn/SaveComment",
                            data: { city: cityValue, state: stateValue, comment: contentValue, id: idValue, name: nameValue },
                            success: function(results) {
                                $("#result").html(results);
                                 $.unblockUI();
                            }
                        });
                    });

                    $("input[id^=btnDelete]").live('click', function() {
                        var id = this.id.split('_')[1];
                        $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                        $.ajax({
                            type: "POST",
                            url: "/TradeIn/DeleteComment/" + id,
                            data: {},
                            success: function(results) {
                                $("#result").html(results);
                                 $.unblockUI();
                            }
                        });
                    });



             
                    $('#CarFaxPassword').focus(function() {
                        $("#hdnOldCarFax").val($(this).val());
                        $(this).val("");
                    }).blur(function(defaultValue) {
                        if ($(this).val() == "") {
                            $(this).val($("#hdnOldCarFax").val());
                        }
                        else {
                            $("#CarFaxPasswordChanged").val('True');
                        }
                    });

                    $('#ManheimPassword').focus(function() {
                        $("#hdnOldManheim").val($(this).val());
                        $(this).val("");
                    }).blur(function(defaultValue) {
                        if ($(this).val() == "") {
                            $(this).val($("#hdnOldManheim").val());
                        }
                        else {
                            $("#ManheimPasswordChanged").val('True');
                        }
                    });

                    $('#KellyPassword').focus(function() {
                        $("#hdnOldKBB").val($(this).val());
                        $(this).val("");
                    }).blur(function(defaultValue) {
                        if ($(this).val() == "") {
                            $(this).val($("#hdnOldKBB").val());
                        }
                        else {
                            $("#KellyPasswordChanged").val('True');
                        }
                    });

                    $('#BlackBookPassword').focus(function() {
                        $("#hdnOldBB").val($(this).val());
                        $(this).val("");
                    }).blur(function(defaultValue) {
                        if ($(this).val() == "") {
                            $(this).val($("#hdnOldBB").val());
                        }
                        else {
                            $("#BlackBookPasswordChanged").val('True');
                        }
                    });

                    $("#btnsubmit").live('click', function() {

                        if (IsNumeric($("#Variance").val())) {
                            $("#lbVarianceValidate").hide();

                            var value = $("#Variance").val();
                            $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                            $.ajax({
                                type: "POST",
                                url: "/Admin/SaveVarianceCost?cost=" + value,
                                data: {},
                                success: function(results) {
                                    $("#cost_result").html(results);
                                     $.unblockUI();
                                }
                            });
                        }
                        else {
                            $("#lbVarianceValidate").show();
                        }
                    });
                });
        
    </script>
</asp:Content>
