<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InventoryFormViewModel>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Inventory List
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
    <div id="recent">
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <% if (Model.CurrentOrSoldInventory)
                   {%>
                <div id="soldVehicles" class="right_header_btns invent_today_bucket_jump">
                    <%= Html.ActionLink("View Sold Vehicles", "ViewSoldInventory", "Inventory")%>
                </div>
                <% }
                   else
                   { %>
                <div id="B1" class="right_header_btns invent_today_bucket_jump">
                    <%= Html.ActionLink("View Current Vehicles", "ViewInventory", "Inventory")%>
                </div>
                <% } %>
                <% if ((bool)Session["IsEmployee"] == false)
                   { %>
                <div id="TodayBucketJump" class="right_header_btns invent_today_bucket_jump">
                    <%= Html.ActionLink("Today Bucket Jump", "TodayBucketJump", "Inventory")%>
                    &nbsp;&nbsp;
                </div>
                <div id="expressBucketJump" class="right_header_btns invent_bucket_jump">
                    <%= Html.ActionLink("Express Bucket Jump", "ExpressBucketJump", "Inventory")%>
                    &nbsp;&nbsp;
                </div>
                <% } %>
                <%if (!Model.CombineInventory && Model.CurrentOrSoldInventory)
                  {%>
                <div id="color0" class="right_header_btns invent_acars">
                    <%= Html.ActionLink("A Cars", "ACarInventory", "Inventory")%>
                </div>
                <div id="color1" class="right_header_btns invent_missiing_content">
                    <%= Html.ActionLink("Missing Content", "MissContentSmallInventory", "Inventory")%>
                </div>
                <div id="color2" class="right_header_btns invent_no_content">
                    <%= Html.ActionLink("No Content", "NoContentSmallInventory", "Inventory")%>
                </div>
                <% } %>
            </div>
        </div>
        <div id="right_content_nav" class="right_content_nav">
            <div class="right_content_nav_items nav_middle">
            </div>
            <div class="right_content_nav_items nav_vin_expended right_content_items_vin nav_long" style="margin-left: 65px;">
                VIN
            </div>
            <div class="right_content_nav_items nav_long">
                <%= Html.ActionLink("Stock", "SortSmallInventory", "Inventory", new { id = "Stock" }, null)%>
            </div>
            <div class="right_content_nav_items nav_middle">
                <%= Html.ActionLink("Year", "SortSmallInventory", "Inventory", new { id = "Year" }, null)%>
            </div>
            <div class="right_content_nav_items nav_long">
                <%= Html.ActionLink("Make", "SortSmallInventory", "Inventory", new { id = "Make" }, null)%>
            </div>
            <div class="right_content_nav_items nav_long">
                <%= Html.ActionLink("Model", "SortSmallInventory", "Inventory", new { id = "Model" }, null)%>
            </div>
            <div class="right_content_nav_items nav_long">
                <%= Html.ActionLink("Trim", "SortSmallInventory", "Inventory", new { id = "Trim" }, null)%>
            </div>
            <div class="right_content_nav_items nav_middle">
                <%= Html.ActionLink("Color", "SortSmallInventory", "Inventory", new { id = "Color" }, null)%>
            </div>
            <div class="right_content_nav_items right_content_items_owners nav_short">
                <%= Html.ActionLink("Owners", "SortSmallInventory", "Inventory", new { id = "Owners" }, null)%>
            </div>
            <div class="right_content_nav_items nav_short">
                <%= Html.ActionLink("Age", "SortSmallInventory", "Inventory", new { id = "Age" }, null)%>
            </div>
            <div class="right_content_nav_items right_content_items_maket nav_long">
                <%= Html.ActionLink("Market Data", "SortSmallInventory", "Inventory", new { id = "Age" }, null)%>
            </div>
            <div class="right_content_nav_items nav_middle">
                <%= Html.ActionLink("Miles", "SortSmallInventory", "Inventory", new { id = "Miles" }, null)%>
            </div>
            <div class="right_content_nav_items nav_long">
                <%= Html.ActionLink("Price", "SortSmallInventory", "Inventory", new { id = "Price" }, null)%>
            </div>
            <%if (!Model.CombineInventory)
              { %>
            <% if (Model.CurrentOrSoldInventory)
               {%>
            <div id="switch-view" title="Switch to Large Cells">
                <a href="<%= Url.Content("~/Inventory/ViewInventory") %>">
                    <img src='<%= Url.Content("~/Images/small-view.jpg") %>' />
                </a>
            </div>
            <% }
               else
               { %>
            <div id="switch-view" title="Switch to Large Cells">
                <a href="<%= Url.Content("~/Inventory/ViewSoldInventory") %>">
                    <img src='<%= Url.Content("~/Images/small-view.jpg") %>' />
                </a>
            </div>
            <% } %>
            <% } %>
        </div>
        <%if (Model.CurrentOrSoldInventory)
          {%>
        <%=Html.DynamicHtmlLabelForInventory("InventoryGrid")%>
        <% }
          else
          {%>
        <%= Html.DynamicHtmlLabelForInventory("InventoryGrid")%>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".sForm").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
            if ($("#IsEmployee").val() == 'True') {
                $("input[name='odometer']").attr('readonly', 'true');
                $("input[name='price']").attr('readonly', 'true');
            }
        });

        $("input[id^=IsFeatured_]").live('click', function () {
            var id = this.id.split('_')[1];
            var urlString = "/Inventory/UpdateIsFeatured/" + id;
            $.ajax({
                type: "POST",
                contentType: "text/hmtl; charset=utf-8",
                dataType: "html",
                url: urlString,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    //alert(result);
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                }
            });
        });

        function updateMileage(txtBox) {
            $.post('<%= Url.Content("~/Inventory/UpdateMileageFromInventoryPage") %>', { Mileage: txtBox.value, ListingId: txtBox.id }, function (data) {
            });
        }
        
        function updateSalePrice(txtBox) {
            $.post('<%= Url.Content("~/Inventory/UpdateSalePriceFromInventoryPage") %>', { SalePrice: txtBox.value, ListingId: txtBox.id }, function (data) {
            });
        }

        function updateReconStatus(txtBox) {
            $.post('<%= Url.Content("~/Inventory/UpdateReconStatusFromInventoryPage") %>', { Reconstatus: txtBox.checked, ListingId: txtBox.id }, function (data) {

            });
        }

        $('a:not(.iframe)').click(function (e) {
            if ($(this).attr('target') == '') {
                $('#elementID').removeClass('hideLoader');
            }
        });

        $("a.iframe").fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $('.rowOuter').each(function () {
            var status = $(this).children('.imageCell').children('.imageWrap').children('input').attr('value');
            //console.log(status);
            if (status == 0 || status == undefined) {
                //console.log('good status');
                $(this).removeClass('border');
            } else {
                //console.log('bad status');
                $(this).addClass('border');
                if (status == 1 || status == 2) {
                    //console.log('med status'); 
                    $(this).addClass('med');
                }
            }
        });
        $('#sold').hide('fast');
        var toggle = false;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server"></asp:Content>