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
            <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetSingleKarPowerSummaryForAppraisal", "Market", new { appraisalId = ViewData["LISTINGID"], trimId = Model.FirstOrDefault().SelectedTrimId }) %>">
                <%= Model.FirstOrDefault().SelectedTrimName%></a>
        </div>
    </div>
     <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Base Price
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_below_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #0062A1">
                <%= Model.FirstOrDefault().BaseWholesale.ToString("C0")%>
            </div>
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

</div>
<%}
   else if (Model.Count(i => i.IsSelected) == 1)
   {%>
<div class="karpowerTrim" id="karpowerTrim_<%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimId %>">
    <div class="ptr_items_content_header">
        <div class="ptr_items_textInfo">
            <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetSingleKarPowerSummaryForAppraisal", "Market", new { appraisalId = ViewData["LISTINGID"], trimId = Model.FirstOrDefault(i => i.IsSelected).SelectedTrimId }) %>">
                <%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimName%></a>
        </div>
    </div>
    <div class="ptr_items_content_items">
        <div class="ptr_items_content_key">
            Base Price
        </div>
        <div class="ptr_items_content_value">
            <div class="ptri_content_value_icons kpi_market_below_icon">
            </div>
            <div class="ptri_content_value_text" style="color: #0062A1">
                <%= Model.FirstOrDefault(i => i.IsSelected).BaseWholesale.ToString("C0")%>
            </div>
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
    
</div>
<% } %>
<%else
   {%>
<% if (Model.Any())
   {%>
<div id="divMultipleKbbTrim">
    <div class="karpowerTrim">
        <div class="ptr_items_content_header">
            <%--<div class="ptr_items_textInfo">
                <a style="color: black; text-underline: none" class="iframe iframeKBB" href="<%= Url.Action("GetSingleKarPowerSummaryForAppraisal", "Market", new { appraisalId = (string)ViewData["LISTINGID"], trimId = item.SelectedTrimId }) %>">
                    <%= item.SelectedTrimName%></a>
            </div>--%>
            <div class="ptr_items_textInfo">
                <select id="DDLKBB" onchange="ViewDetail();">
                    <% foreach (var item in Model)
                       {%>
                        <option value="<%=string.Format("{0}|{1}|{2}|{3}",item.Wholesale.ToString("C0"),item.MileageAdjustment.ToString("C0"),item.BaseWholesale.ToString("C0"),item.SelectedTrimId) %>"><%= item.SelectedTrimName%></option>
                    <%}%>
                </select>
            </div>
            <div style="padding-top: 3px;cursor:pointer" title="View detail">
                <a id="lnkDetail" style="color: black; text-underline: none" class="iframe iframeKBB">
                    <img src="../../Content/images/vincontrol/iconEye.png" height="15" /></a>
            </div>
        </div>
        <div class="ptr_items_content_items">
            <div class="ptr_items_content_key">
                Base Price
            </div>
            <div class="ptr_items_content_value">
                <div class="ptri_content_value_icons kpi_market_below_icon">
                </div>
                <div class="ptri_content_value_text" style="color: #0062A1" id="divBaseWholesale">
                    0
                </div>
            </div>
        </div>
        <div class="ptr_items_content_items">
            <div class="ptr_items_content_key">
                Lending Value
            </div>
            <div class="ptr_items_content_value">
                <div class="ptri_content_value_icons kpi_market_above_icon">
                </div>
                <div class="ptri_content_value_text" style="color: #D12C00" id="divWholeSale">
                    0
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
                <div class="ptri_content_value_text" style="color: #458C00" id="divMileageAdjustment">
                    0
                </div>
            </div>
        </div>
    
    </div>
</div>
<%}%>
<%}%>
<input type="hidden" value="0" id="hdValue" />
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
        ViewDetail();

        $('#DDLKBB').change(function (e) {
            ViewDetailChange($("#DDLKBB option:selected").val());
        });
    });

    function ViewDetail() {
        try {
            var value = $("#DDLKBB option:selected").val();
            var arr = value.split('|');
            $('#divWholeSale').html(arr[0]);
            $('#divMileageAdjustment').html(arr[1]);
            $('#divBaseWholesale').html(arr[2]);
            var urldetail = '<%= Url.Action("GetSingleKarPowerSummaryForAppraisal", "Market", new { appraisalId = ViewData["LISTINGID"], trimId = "_trimID" }) %>';
            urldetail = urldetail.replace('_trimID', arr[3]);
            $('#lnkDetail').attr('href', urldetail);
        }
        catch (e)
        { }
    }

    function ViewDetailChange(value) {
        try {
            var arr = value.split('|');
            $('#divWholeSale').html(arr[0]);
            $('#divMileageAdjustment').html(arr[1]);
            $('#divBaseWholesale').html(arr[2]);
            var urldetail = '<%= Url.Action("GetSingleKarPowerSummaryForAppraisal", "Market", new { appraisalId = ViewData["LISTINGID"], trimId = "_trimID" }) %>';
            urldetail = urldetail.replace('_trimID', arr[3]);
            $('#lnkDetail').attr('href', urldetail);
        }
        catch (e)
        { }
    }
</script>
