<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<WhitmanEnterpriseMVC.Models.SmallKarPowerViewModel>>" %>

<% if (Model.Any())
   {%>
<% foreach (var data in Model)
   {%>
<div id="kbb-row" class="mr-row" <%= data.IsSelected ? "" : "style='display:none'" %>>
    <div class="range-item label">
        <a id="kbbRow_<%= data.SelectedTrimId %>" class="iframe" target="_blank" style="font-size: .7em" title="<%=data.SelectedTrimName%>" href="<%= Url.Action("GetSingleKarPowerSummaryForAppraisal", "Market", new { appraisalId = (string)ViewData["LISTINGID"], trimId = data.SelectedTrimId }) %>">
            <%=data.SelectedTrimName%></a></div>
    <div class="low-wrap range-item low">
        <span class="bb-price">
            <%=data.BaseWholesale%></span>
    </div>
    <div class="mid-wrap range-item mid">
        <span class="bb-price">
            <%=data.MileageAdjustment%></span>
    </div>
    <div class="high-wrap range-item high">
        <span class="bb-price">
            <%=data.Wholesale%></span>
    </div>
</div>
<%}%>
<%}
   else
   {%>
<div id="kbb-row" class="mr-row">
    There is no KBB value associated with this vehicle
</div>
<%}%>
<div>
    <a id="aLoadFullKBBTrims" style="float: left; font-weight: normal; font-size: .9em;" href="javascript:;">        
        Not a correct trim? Click here</a>
    <br />
  <%--  <a id="kbbRow" class="iframe" target="_blank" style="float: right; font-weight: normal; font-size: .9em;"
        href="<%= Url.Action("GetKarPowerSummaryForAppraisal", "Market", new { appraisalId = (string)ViewData["LISTINGID"] }) %>">
        View KBB Summary Report</a>--%>
    
    <br />
    <font size="1">* Total Value = Base WholeSale +/- Mileage Adjustment</font>
</div>
