<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.KellyBlueBookViewModel>" %>
<% if (Model.Success)
   {
       foreach (
           var trimDetail in Model.TrimReportList.OrderBy(x => x.TrimId))
       {%>
<div id="kbb-row" class="mr-row">
    <div class="range-item label">
        <a id="kbbRow_<%= trimDetail.TrimId %>" class="iframe" target="_blank" style="font-size: .7em" title="<%=trimDetail.TrimName%>"
            href="<%=Url.Content("~/Market/GetKellyBlueBookSummaryByTrim?ListingId=")%><%=Model.ListingId%>&TrimId=<%=trimDetail.TrimId%> ">
            <%=trimDetail.TrimName%></a></div>
    <div class="low-wrap range-item low">
        <span class="bb-price">
            <%=trimDetail.BaseWholesale%></span>
    </div>
    <div class="mid-wrap range-item mid">
        <%
           if (trimDetail.MileageAdjustment >= 0)
           {%>
        <span class="bb-price">+
            <%=trimDetail.MileageAdjustment%></span>
        <%
                          }
                          else
                          {%>
        <span class="bb-price">
            <%=trimDetail.MileageAdjustment%></span>
        <%
                                    }%>
    </div>
    <div class="high-wrap range-item high">
        <span class="bb-price">=
            <%=trimDetail.WholeSale%></span>
    </div>
</div>
<%
       }%>
<%
   }
   else
   {%>
<div id="kbb-row" class="mr-row">
    There is no KBB value associated with this vehicle
</div>
<%}%>
<div>
    <a style="float: left; font-weight: normal; font-size: .9em;" href="<%= Url.Action("ResetKbbTrim", "Market", new { listingId = Model.ListingId }) %>">        
        Not a correct trim? Click here</a>
    <br />
<%--    <a id="kbbRow" class="iframe" target="_blank" style="float: right; font-weight: normal; font-size: .9em;"
        href="<%= Url.Action("GetKellyBlueBookSummary", "Market", new { listingId = Model.ListingId }) %>">
        View KBB Summary Report</a>--%>
    
    <br />
    <font size="1">* Total Value = Base WholeSale +/- Mileage Adjustment</font>
</div>
