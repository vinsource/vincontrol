<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AdminViewModel>" %>
<style type="text/css">
    .disable
    {
        display: none;
    }
</style>
<script type="text/javascript">

    $(document).ready(function () {
        $("#aDoneBuyerGuide").click(function () {

            if ($("#SelectedWarrantyType").val() == "") {
                ShowWarningMessage("Category is required.");
            } else if ($("#txtNewBuyerGuide").val() == "") {
                ShowWarningMessage('Buyer Guide Name is required.');
            } else {
                $.ajax({
                    type: "GET",
                    url: '/Admin/AddNewBuyerGuide?name=' + $("#txtNewBuyerGuide").val() + '&category=' + $("#SelectedWarrantyType").val(),
                    data: {},
                    beforeSend: function () {
                        $("#BuyerGuideContent").html("<div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");
                    },
                    success: function (results) {

                        $.ajax({
                            type: "POST",
                            contentType: "text/hmtl; charset=utf-8",
                            dataType: "html",
                            url: buyerGuideUrl,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $("#BuyerGuideContent").html(result);

                            },
                            error: function (err) {
                                console.log(err.status + " - " + err.statusText);
                            }
                        });

                    }
                });
            }
            ;
        });


        $("a[id^='aRemoveBuyerGuide_']").live('click', function () {
            var retVal = confirm("Do you want to remove this buyer guide ?");
            if (retVal == true) {
                $(this).parent().closest('.ac_bg_items').fadeOut(500);
                var id = this.id.split("_")[1];
                $.ajax({
                    type: "GET",
                    url: '/Admin/RemoveBuyerGuide/' + id,
                    data: {},
                    beforeSend: function () {
                        $("#BuyerGuideContent").html("<div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");
                    },
                    success: function (results) {

                        $.ajax({
                            type: "POST",
                            contentType: "text/hmtl; charset=utf-8",
                            dataType: "html",
                            url: buyerGuideUrl,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $("#BuyerGuideContent").html(result);

                            },
                            error: function (err) {
                                console.log(err.status + " - " + err.statusText);
                            }
                        });

                    }
                });
            }
        });


        $("a[id^='aDoneEditBuyerGuide_']").live('click', function () {
            var id = this.id.split("_")[1];
            if ($("#SelectedWarrantyTypeForEdit_" + id).val() == "") {
                ShowWarningMessage('Category is required.');
            }
            else if ($("#txtEditBuyerGuide_" + id).val() == "") {
                ShowWarningMessage('Buyer Guide Name is required.');
            } else {
                $.ajax({
                    type: "POST",
                    url: '/Admin/UpdateBuyerGuide?listingId=' + id + '&name=' + $("#txtEditBuyerGuide_" + id).val() + '&category=' + $("#SelectedWarrantyTypeForEdit_" + id).val(),
                    data: {},
                    success: function (results) {
                        if (results == 'Existing') {
                            ShowWarningMessage('This name is already in the system');
                        } else if (results == 'TimeOut') {
                            window.location.href = logOffURL;
                        } else if (results == 'Error') {
                            ShowWarningMessage('System error!');
                        } else if (results == 'True') {
                            var container = $("#txtEditBuyerGuide_" + id).parent().parent().parent();

                            container.find(".ac_bg_name").html($("#txtEditBuyerGuide_" + id).val());

                            container.find(".ac_bg_delete").show();
                            container.find(".ac_bg_name").show();
                            container.find(".ac_bg_edit_item").hide();
                        } else {
                            ShowWarningMessage(results);
                        }
                    }
                });
            }
        });

        $("a[id^='aEditBuyerGuide_']").live('click', function () {
            var id = this.id.split("_")[1];

            var container = $(this).parent().parent().parent();
            container.find(".ac_bg_delete").hide();
            container.find(".ac_bg_name").hide();
            container.find(".ac_bg_edit_item").show();
        });

        $("a[id^='aCancelEditBuyerGuide_']").live('click', function () {
            var id = this.id.split("_")[1];
            var container = $(this).parent().parent().parent();
            container.find(".ac_bg_delete").show();
            container.find(".ac_bg_name").show();
            container.find(".ac_bg_edit_item").hide();
        });

        $("#addNewBG").live("click", function () {
            $(".content_new_bg").fadeIn();
        });

        $(".bg_btns_cancel").live("click", function () {
            $(".content_new_bg").fadeOut();
        });

        $(".bg_btns_save").live("click", function () {
            $(".content_new_bg").fadeOut();
        });

    });
</script>
<div class="bg_add_holder">
    <div class="admin_content_info_title">
        Buyer's Guide
        <img src="<%= Url.Content("~/Content/images/vincontrol/quote.jpg") %>" title=" Add, edit, and delete your own Buyer’s Guide templates. Each template will become a choice in the Warranty drop down menu on a car’s Edit Profile page.">
        <div class="ac_window_addNew_holder">
            <div id="addNewBG" class="btns_shadow ac_inventory_btns">
                Add Buyer's Guide
            </div>
        </div>
    </div>
    <div class="content_new_bg">
        <div class="content_view_title">
            Buyer's Guide Information</div>
        <div class="bg_edit_content">
            <div class="bg_add_content">
                <div>
                    <div style="width: 100px; float: left">
                        Category:
                    </div>
                    <%= Html.DropDownListFor(m => m.SelectedWarrantyType, Model.BasicWarrantyTypes,
                                                      "-- Select category --",new {style="width:180px;",id="SelectedWarrantyType"})%>
                </div>
                <div>
                    <div style="width: 100px; float: left">
                        Name:
                    </div>
                    <input type="text" id="txtNewBuyerGuide" name="txtNewBuyerGuide" style="width: 180px" />
                </div>
            </div>
        </div>
        <div class="bg_edit_btns">
            <div class="btns_shadow bg_btns_save">
                <a id="aDoneBuyerGuide" href="javascript:;">Add</a>
            </div>
            <div class="btns_shadow bg_btns_cancel">
                <a id="aCancelBuyerGuide" href="javascript:;">Cancel</a></div>
        </div>
    </div>
</div>
<div class="ac_bg_content">
    <% if (Model.WarrantyTypes != null && Model.WarrantyTypes.Any())
       {
           var count = 0;
    %>
    <% foreach (var item in Model.WarrantyTypes)
       { %>
    <div class="ac_bg_items <%=count%2==0?String.Empty:"ac_bg_items_odd" %>">
        <div class="ac_bg_content_left">
            <div class="ac_bg_delete">
                <% if (item.DealerId > 0)
                   { %>
                <a id="aRemoveBuyerGuide_<%= item.Id %>" href="javascript:;" title="Delete">
                    <img src='<%= Url.Content("~/Content/images/vincontrol/delete_bg.jpg") %>' /></a>
                <% }
                   else
                   { %>
                &nbsp;
                <% } %>
            </div>
            <div class="ac_bg_name">
                <%= item.Name %>
            </div>
            <div class="ac_bg_edit_item" style="display: none">
                <select id="SelectedWarrantyTypeForEdit_<%= item.Id %>" style="width: 150px; display: inline-block;">
                    <option value="">-- Select category --</option>
                    <option value="1" <%=(item.CategoryId==1)?"selected=\"selected\"":String.Empty %>>As
                        Is</option>
                    <option value="2" <%=(item.CategoryId==2)?"selected=\"selected\"":String.Empty %>>Manufacturer
                        Warranty</option>
                    <option value="3" <%=(item.CategoryId==3)?"selected=\"selected\"":String.Empty %>>Dealer
                        Warranty</option>
                    <option value="4" <%=(item.CategoryId==4)?"selected=\"selected\"":String.Empty %>>Manufacturer
                        Certified</option>
                    <option value="5" <%=(item.CategoryId==5)?"selected=\"selected\"":String.Empty %>>Other Warranty</option>
                    <option value="6" <%=(item.CategoryId==6)?"selected=\"selected\"":String.Empty %>>Service Contract</option>
                </select>
                <input type="text" id="txtEditBuyerGuide_<%= item.Id %>" name="txtEditBuyerGuide__<%= item.Id %>"
                    value=" <%= item.Name %>" />
                <a id="aDoneEditBuyerGuide_<%= item.Id %>" href="javascript:;" class="bg_edit_done_bg">
                    Done</a> <a id="aCancelEditBuyerGuide_<%= item.Id %>" href="javascript:;" class="bg_edit_done_bg">
                        Cancel</a>
                <br />
            </div>
        </div>
        <div class="ac_bg_content_right">
            <div class="ac_bg_edit">
                <% if (item.DealerId > 0)
                   { %>
                <a id="aEditBuyerGuide_<%= item.Id %>" href="javascript:;" title="Edit">(edit) </a>
                <% }
                   else
                   { %>
                &nbsp;
                <% } %>
            </div>
            <div class="ac_bg_eng">
                <a class="iframe" href="<%= item.EnglishVersionUrl %>" id="English_<%= item.Id %>">English
                </a>
            </div>
            <div class="ac_bg_separated">
                |
            </div>
            <div class="ac_bg_spanish">
                <a class="iframe" href="<%= item.SpanishVersionUrl %>" id="Spanish_<%= item.Id %>">Spanish</a>
            </div>
        </div>
    </div>
    <% count++;
       } %>
    <% } %>
</div>
