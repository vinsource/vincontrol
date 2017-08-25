<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.ManheimWholesaleViewModel>>" %>
<% if (Model.Count == 0)
   {%>
<div class="manheimTrim">
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            No Data
        </div>
    </div>
</div>
<%}%>
<% if (Model.Count == 1)
   {%>
<div class="manheimTrim">
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline-color: blue" class="iframe iframeManHeim" href="<%=Url.Content("~/Report/ManheimTransactionDetail?year=")%><%=Model.FirstOrDefault().Year%>&make=<%=Model.FirstOrDefault().MakeServiceId%>&model=<%=Model.FirstOrDefault().ModelServiceId%>&trim=<%=Model.FirstOrDefault().TrimServiceId%>">
                <%= Model.FirstOrDefault().TrimName%></a>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Above
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_above_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #D12C00">
                <%= Model.FirstOrDefault().HighestPrice.ToString("c0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Average
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_equal_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #458C00">
                <%= Model.FirstOrDefault().AveragePrice.ToString("c0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Below
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_below_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #0062A1">
                <%= Model.FirstOrDefault().LowestPrice.ToString("c0")%>
            </div>
        </div>
    </div>
</div>
<%}%>
<% else
   {%>
<% foreach (var item in Model)
   {%>
<div class="manheimTrim" id="manheimTrim_<%= item.TrimServiceId %>" <%= item.TrimName.Equals(Model.FirstOrDefault().TrimName) ? "" : "style='display:none'" %>>
    <div class="ptr_items_content_header">
        <div class="ptr_items_btns ptr_items_left_btn left-control" id="manheim-left-control_<%= item.TrimServiceId %>">
        </div>
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeManHeim" href="<%=Url.Content("~/Report/ManheimTransactionDetail?year=")%><%=item.Year%>&make=<%=item.MakeServiceId%>&model=<%=item.ModelServiceId%>&trim=<%=item.TrimServiceId%>">
                <%= item.TrimName%></a>
        </div>
        <div class="ptr_items_btns ptr_items_right_btn right-control" id="manheim-right-control_<%= item.TrimServiceId %>">
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Above
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_above_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #D12C00">
                <%= item.HighestPrice.ToString("c0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Average
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_equal_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #458C00">
                <%= item.AveragePrice.ToString("c0")%>
            </div>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Below
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_below_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #0062A1">
                <%= item.LowestPrice.ToString("c0")%>
            </div>
        </div>
    </div>
</div>
<%} %>
<%} %>
<script type="text/javascript">

    $(document).ready(function () {

        $('#ManheimCount').html(<%=Model.Count %>);
        $("a.iframeManHeim").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        $(".manheimTrim .left-control").click(function () {
            var id = this.id.split('_')[1];

            if ($('#manheimTrim_' + id).prev().length > 0) {
                $(".manheimTrim").hide();
                $('#manheimTrim_' + id).prev().show();
            }
            else {
                //$('#manheimTrim_' + id).show();
                $(this).hide();
            }
        });

        $(".manheimTrim .right-control").click(function () {
            var id = this.id.split('_')[1];
            if ($('#manheimTrim_' + id).next().length > 0) {
                $(".manheimTrim").hide();
                $('#manheimTrim_' + id).next().show();
            }
            else {
                //$('#manheimTrim_' + id).show();
                $(this).hide();
            }
        });
    });
</script>
