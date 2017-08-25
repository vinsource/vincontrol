<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.SmallKarPowerViewModel>>" %>
<style type="text/css">
    .disable
    {
        display: none;
    }
</style>
<% if (Model.Count == 0)
   {%>
<div class="manheimTrim">
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            No Data
        </div>
    </div>
</div>
<%}
   else %>
<% if (Model.Count == 1)
   {%>
<div class="karpowerTrim" id="Div1" <%= Model.FirstOrDefault().SelectedTrimName.Equals(Model.FirstOrDefault().SelectedTrimName) ? "" : "style='display:none'" %>>
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetSingleKarPowerSummary", "Market", new { listingId = ViewData["LISTINGID"], trimId = Model.FirstOrDefault().SelectedTrimId }) %>">
                <%= Model.FirstOrDefault().SelectedTrimName%></a>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Lending Value
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_above_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #D12C00">
                <%= Model.FirstOrDefault().Wholesale.ToString("C0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Mileage Adj.
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_equal_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #458C00">
                <%= Model.FirstOrDefault().MileageAdjustment.ToString("C0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Blue Book Value
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_below_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #0062A1">
                <%= Model.FirstOrDefault().BaseWholesale.ToString("C0")%>
            </div>
        </div>
    </div>
</div>
<%}
   else if (Model.Count(i => i.IsSelected) == 1)
   {%>
<div class="karpowerTrim" id="karpowerTrim_<%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimId %>">
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetSingleKarPowerSummary", "Market", new { listingId = ViewData["LISTINGID"], trimId = Model.FirstOrDefault(i => i.IsSelected).SelectedTrimId }) %>">
                <%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimName%></a>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Lending Value
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_above_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #D12C00">
                <%= Model.FirstOrDefault(i => i.IsSelected).Wholesale.ToString("C0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Mileage Adj.
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_equal_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #458C00">
                <%= Model.FirstOrDefault(i => i.IsSelected).MileageAdjustment.ToString("C0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Blue Book Value
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_below_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #0062A1">
                <%= Model.FirstOrDefault(i => i.IsSelected).BaseWholesale.ToString("C0")%>
            </div>
        </div>
    </div>
</div>
<% } %>
<%else
   {%>
<% if (Model.Any())
   {%>
<div id="divMultipleKbbTrim">
    <% foreach (var item in Model)
       {%>
    <div class="karpowerTrim" id="karpowerTrim_<%= item.SelectedTrimId %>" <%= item.SelectedTrimName.Equals(Model.FirstOrDefault().SelectedTrimName) ? "" : "style='display:none'" %>>
        <div class="ptr_items_content_header">
            <div class="ptr_items_btns ptr_items_left_btn left-control" id="karpower-left-control_<%= item.SelectedTrimId %>">
            </div>
            <div class="ptr_items_textInfo">
                <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetSingleKarPowerSummary", "Market", new { listingId = ViewData["LISTINGID"], trimId = item.SelectedTrimId }) %>">
                    <%= item.SelectedTrimName%></a>
            </div>
            <div class="ptr_items_btns ptr_items_right_btn right-control" id="karpower-right-control_<%= item.SelectedTrimId %>">
            </div>
        </div>
        <div class="ptr_items_content_items">
            <div class="ptr_items_content_key">
                Lending Value
            </div>
            <div class="ptr_items_content_value">
                <div class="ptri_content_value_icons kpi_market_above_icon">
                </div>
                <div class="ptri_content_value_text" style="color: #D12C00">
                    <%= item.Wholesale.ToString("C0")%>
                </div>
            </div>
        </div>
        <div class="ptr_items_content_items">
            <div class="ptr_items_content_key">
                Mileage Adj.
            </div>
            <div class="ptr_items_content_value">
                <div class="ptri_content_value_icons kpi_market_equal_icon">
                </div>
                <div class="ptri_content_value_text" style="color: #458C00">
                    <%= item.MileageAdjustment.ToString("C0")%>
                </div>
            </div>
        </div>
        <div class="ptr_items_content_items">
            <div class="ptr_items_content_key">
                Blue Book Value
            </div>
            <div class="ptr_items_content_value">
                <div class="ptri_content_value_icons kpi_market_below_icon">
                </div>
                <div class="ptri_content_value_text" style="color: #0062A1">
                    <%= item.BaseWholesale.ToString("C0")%>
                </div>
            </div>
        </div>
    </div>
    <%}%>
</div>
<%}%>
<%}%>
<script type="text/javascript">

    $(document).ready(function () {

        $('#KBBCount').html(<%=Model.Count %>);

        $("a.iframeKBB").fancybox({ 'width': 790, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        var numberOfTrims = '<%= Model.Count %>';
        if (numberOfTrims <= 1) {
            $('#clickNotCorrectKbbTrim').hide();
        } else {
            $('#clickNotCorrectKbbTrim').click(function () {
                $('#divMultipleKbbTrim').show();
                $('#divSingleKbbTrim').hide();
            });
        }

        $('#kbbTrims').html('KBB(' + '<%= Model.Count %>' + ')');
        $(".karpowerTrim .left-control").click(function () {
            var id = this.id.split('_')[1];
            console.log($('#divMultipleKbbTrim #karpowerTrim_' + id));
            if ($('#divMultipleKbbTrim #karpowerTrim_' + id).prev().length > 0) {
                $(".karpowerTrim").hide();
                $('#divMultipleKbbTrim #karpowerTrim_' + id).prev().show();
            } else {
                //$('#karpowerTrim_' + id).show();
                $(this).hide();
            }
        });

        $(".karpowerTrim .right-control").click(function () {
            var id = this.id.split('_')[1];
            console.log($('#divMultipleKbbTrim #karpowerTrim_' + id));
            if ($('#divMultipleKbbTrim #karpowerTrim_' + id).next().length > 0) {
                $(".karpowerTrim").hide();
                $('#divMultipleKbbTrim #karpowerTrim_' + id).next().show();
            } else {
                //$('#karpowerTrim_' + id).show();
                $(this).hide();
            }
        });
    });
</script>
