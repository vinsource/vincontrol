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
<%else
   { %>
<% if (Model.Count == 1)
   {%>
<div class="manheimTrim">
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeManHeim" href="<%=Url.Content("~/Report/ManheimTransactionDetail?year=")%><%=Model.FirstOrDefault().Year%>&make=<%=Model.FirstOrDefault().MakeServiceId%>&model=<%=Model.FirstOrDefault().ModelServiceId%>&trim=<%=Model.FirstOrDefault().TrimServiceId%>">
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
                <%= Model.FirstOrDefault().HighestPrice.ToString("C0")%>
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
                <%= Model.FirstOrDefault().AveragePrice.ToString("C0")%>
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
                <%= Model.FirstOrDefault().LowestPrice.ToString("C0")%>
            </div>
        </div>
    </div>
</div>
<%}%>
<% else
   {%>
<div class="manheimTrim">
    <div class="ptr_items_content_header">
        <%--<div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeManHeim" href="<%=Url.Content("~/Report/ManheimTransactionDetail?year=")%><%=item.Year%>&make=<%=item.MakeServiceId%>&model=<%=item.ModelServiceId%>&trim=<%=item.TrimServiceId%>">
                <%= item.TrimName%></a>
        </div>--%>
        <div class="ptr_items_textInfo">
            <select id="DDLManHeim">
                <% foreach (var item in Model)
                   {%>
                <option value="<%=string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",item.HighestPrice.ToString("C0"),item.AveragePrice.ToString("C0"),item.LowestPrice.ToString("C0"),item.Year,item.MakeServiceId,item.ModelServiceId,item.TrimServiceId) %>"><%= item.TrimName%></option>
                <%}%>
            </select>
        </div>
        <div style="padding-top: 3px; cursor: pointer" title="View detail">
            <a id="lnkDetail" style="color: black; text-underline: none" class="iframe iframeManHeim">
                <img src="../../Content/images/vincontrol/iconEye.png" height="15" /></a>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Above
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_above_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #D12C00" id="divHighestPriceManHeim">
                0
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
            <div class="ptri_content_value_text" style="color: #458C00" id="divAveragePriceManHeim">
                0
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
            <div class="ptri_content_value_text" style="color: #0062A1" id="divLowestPriceManHeim">
                0
            </div>
        </div>
    </div>
</div>
<%} %>
<%} %>
<script type="text/javascript">
    function ViewDetail() {
        try {
            var value = $("#DDLManHeim option:selected").val();
            console.log(value);
            var arr = value.split('|');
            $('#divHighestPriceManHeim').html(arr[0]);
            $('#divAveragePriceManHeim').html(arr[1]);
            $('#divLowestPriceManHeim').html(arr[2]);
            var year = arr[3];
            var make = arr[4];
            var model = arr[5];
            var trim = arr[6];
            var urldetail = '<%=Url.Content("~/Report/ManheimTransactionDetail?year=_Year&make=_Make&model=_Model&trim=_Trim")%>';
            urldetail = urldetail.replace('_Year', year).replace('_Make', make).replace('_Model', model).replace('_Trim', trim);
            $('#lnkDetail').attr('href', urldetail);
        }
        catch (e) {

        }
    }

    function ViewDetailChange(value) {
        try {
            var arr = value.split('|');
            $('#divHighestPriceManHeim').html(arr[0]);
            $('#divAveragePriceManHeim').html(arr[1]);
            $('#divLowestPriceManHeim').html(arr[2]);
            var year = arr[3];
            var make = arr[4];
            var model = arr[5];
            var trim = arr[6];
            var urldetail = '<%=Url.Content("~/Report/ManheimTransactionDetail?year=_Year&make=_Make&model=_Model&trim=_Trim")%>';
            urldetail = urldetail.replace('_Year', year).replace('_Make', make).replace('_Model', model).replace('_Trim', trim);
            $('#lnkDetail').attr('href', urldetail);
        }
        catch (e) {

        }
    }
    $(document).ready(function () {

        $('#ManheimCount').html(<%=Model.Count %>);
        $("a.iframeManHeim").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        ViewDetail();

        $('#DDLManHeim').change(function () {
            ViewDetailChange($("#DDLManHeim option:selected").val());
        });
    });
</script>
