<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.Chart.CarFaxDetailViewModel>" %>
<%@ Import Namespace="vincontrol.Application.ViewModels.CommonManagement" %>
<script type="text/javascript">
    function newPopup(url) {
        var popupWindow = window.open(
                url, 'popUpWindow', 'height=900,width=1000,left=500,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
    }
</script>
<div id="carfax" style="overflow: hidden;">
    <div id="carfax-header" style="display: block; width: 100%;">
        <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.Vin%>')">
            <img style="display: inline-flexbox; float: left;" src='<%=Url.Content("~/Content/carfax/carfax-large.jpg")%>'
                alt="carfax logo" /></a>
        <h3 style="">
            Summary</h3>
    </div>
    <%if (Model.CarFax.Success)
      {%>
    <div id="summary-wrapper">
        <div id="owners" class="carfax-wrapper">
            <%if (Model.CarFax.NumberofOwners.Equals("0"))
              { %>
            <div class="number">
                -</div>
            <%}
              else if (Model.CarFax.NumberofOwners.Equals("1"))
              { %>
            <div class="oneowner">
                <%=Model.CarFax.NumberofOwners%></div>
            <%}
              else
              {%>
            <div class="number">
                <%=Model.CarFax.NumberofOwners%></div>
            <%} %>
            <div class="text">
                Owner(s)</div>
        </div>
        <div id="reports" class="carfax-wrapper">
            <div class="number">
                <%=Model.CarFax.ServiceRecords%></div>
            <div class="text">
                Service Reports</div>
        </div>
    </div>
    <%} %>
    <div id="report-wrapper" style="width: 100%;">
        <div id="history-report" style="clear: both; float: left; width: 100%;">
            <ul>
                <%foreach (CarFaxWindowSticker tmp in Model.CarFax.ReportList)
                  {  %>
                <%if (tmp.Text.Contains("Prior Rental") || tmp.Text.Contains("Accident(s) / Damage Reported to CARFAX"))
                  { %>
                <li style="background-color: red">
                    <% }
                  else
                  {
                    %>
                    <li>
                        <%
                                } %>
                        <%=tmp.Text %>
                        <img class="c-fax-img" src="<%=tmp.Image %>" />
                    </li>
                    <%} %>
            </ul>
            <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.Vin%>')">
                View Full Report</a>
        </div>
    </div>
</div>
