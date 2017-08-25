<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.KellyBlueBookViewModel>" %>
<style type="text/css">
    .disable
    {
        display: none;
    }
</style>
<% if (Model.TrimReportList.Count == 0)
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
<% if (Model.TrimReportList.Count == 1)
   {%>
<div class="karpowerTrim" id="Div1" <%= Model.TrimReportList.FirstOrDefault().TrimName.Equals(Model.TrimReportList.FirstOrDefault().TrimName) ? "" : "style='display:none'" %>>
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetKellyBlueBookSummaryByTrim", "Market", new { ListingId = Model.ListingId.ToString(), trimId = Model.TrimReportList.FirstOrDefault().TrimId}) %>">
                <%= Model.TrimReportList.FirstOrDefault().TrimName%></a>
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
                <%= Model.TrimReportList.FirstOrDefault().WholeSale.ToString("C0")%>
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
                <%= Model.TrimReportList.FirstOrDefault().MileageAdjustment.ToString("C0")%>
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
                <%= Model.TrimReportList.FirstOrDefault().BaseWholesale.ToString("C0")%>
            </div>
        </div>
    </div>
</div>
<% } %>
<%else
   {%>
<% if (Model.TrimReportList.Any())
   {%>
<div id="divMultipleKbbTrim">
    <% foreach (var item in Model.TrimReportList)
       {%>
    <div class="karpowerTrim" id="karpowerTrim_<%= item.TrimId %>" <%= item.TrimId.Equals(Model.TrimReportList.FirstOrDefault().TrimId) ? "" : "style='display:none'" %>>
        <div class="ptr_items_content_header">
            <div class="ptr_items_btns ptr_items_left_btn left-control" id="karpower-left-control_<%= item.TrimId %>">
            </div>
            <div class="ptr_items_textInfo">
                <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetKellyBlueBookSummaryByTrim", "Market", new { ListingId = Model.ListingId.ToString(), trimId = item.TrimId }) %>">
                    <%= item.TrimName%></a>
            </div>
            <div class="ptr_items_btns ptr_items_right_btn right-control" id="karpower-right-control_<%= item.TrimId %>">
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
                    <%= item.WholeSale.ToString("C0")%>
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

        $('#KBBCount').html(<%=Model.TrimReportList.Count %>);

        $("a.iframeKBB").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

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
