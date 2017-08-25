<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<WhitmanEnterpriseMVC.Models.TradeinCustomerViewModel>>" %>
<%  bool flag = false;
    foreach (var item in Model)
    {%>
<div class="rowOuter  <%if (flag == false){%> dark <%flag = true;} else{%>light<% flag = false;} %>">
    <%
        string carName = item.Year + " " + item.Make + " " + item.Model;
    %>
    <div class="imageCell column">
        <%--  <%=WhitmanEnterpriseMVC.HTMLExtension.ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", car.DefaultImageUrl, "", new { AppraisalId = car.AppraisalID }, null, new { width = 47, height = 47 }) + Environment.NewLine);
 %>--%>
      <a  href="<%=Url.Content("~/Appraisal/ViewProfileForCustomerAppraisal?CustomerId=")%><%=item.ID%>">     <img class="imageCell" src="<%= Url.Content("~/content/images/vin-trade-icon.gif") %>" alt="" /></a>
    </div>
    <div class="infoCell column">
        <div class="innerRow1 clear">
            <div class="cell long noBorder  column">
                    <a  href="<%=Url.Content("~/Appraisal/ViewProfileForCustomerAppraisal?CustomerId=")%><%=item.ID%>">                
                    <%=item.Year%></a>
                <%--ViewProfileForAppraisal action--%>
            </div>
            <div class="cell column">
                    <a  href="<%=Url.Content("~/Appraisal/ViewProfileForCustomerAppraisal?CustomerId=")%><%=item.ID%>">
                
                   
                    <%=item.Make%></a>
            </div>
            <div class="cell column" style="width: 110px;">
                    <a  href="<%=Url.Content("~/Appraisal/ViewProfileForCustomerAppraisal?CustomerId=")%><%=item.ID%>">
                
                            
                    <%=item.Model%></a>
            </div>
            <div class="cell short column">
                <%=item.Condition%>
                <%--  <a href =""> <%=item.stockNumber %></a>--%>
            </div>
            <div class="cell short column">
                <%= item.Date%>
            </div>
            <div class="cell short column">
                <%-- <%= item.TradeInStatus%>--%>
                <select id="opttrade_<%=item.ID %>">
                    <option value="Empty"></option>
                    <%if (item.TradeInStatus == "Done")
                      { %>
                    <option value="Done" selected="selected">Done</option>
                    <%}
                      else
                      { %>
                    <option value="Done">Done</option>
                    <%} %>
                    <%if (item.TradeInStatus == "Pending")
                      { %>
                    <option value="Pending" selected="selected">Pending</option>
                    <%}
                      else
                      { %>
                    <option value="Pending">Pending</option>
                    <%} %>
                    <%if (item.TradeInStatus == "Dead")
                      { %>
                    <option value="Dead" selected="selected">Dead</option>
                    <%}
                      else
                      { %>
                    <option value="Dead">Dead</option>
                    <%} %>
                </select>
            </div>
            <div class="cell shortest column" style="font-size: 1.6em; font-weight: bold;">
                Value
            </div>
        </div>
        <div class="innerRow2 clear">
            <div class="cell long noBorder marketSection column">
                <%=string.Format("{0} {1}", item.FirstName, item.LastName) %>
            </div>
            <div class="cell longest marketSection column">
                <%=item.Email %>
            </div>
            <div class="cell short marketSection column">
                <%= item.Phone %>
            </div>
            <div class="cell short marketSection column">
                <a href="<%=Url.Content("~/PDF/PrintCustomerEmail/")%><%=item.ID%>">Lead </a>
            </div>
            <div class="cell short column">
                <%--  <a target="_blank" href="/Market/OpenManaheimLoginWindow?Vin=<%=item.Date%>">MMR</a>
                / <a class="iframe" href="/Market/GetKellyBlueBookSummaryAppraisal?AppraisalId=<%=item.ID%>">
                    KBB </a>--%>
                     <a href="javascript:;" id="lnkSendEmail_<%=item.ID%>"> Resend </a>
                    
                   
            </div>
            <div class="cell shortest column">
                <%--   int ACV = 0;

                        bool ACVFlag = Int32.TryParse(car.ACV, out ACV);

                        if (ACVFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.AppraisalID + "\" name=\"Acv\" class=\"sForm\" onblur=\"javascript:updateACV(this);\" value=\"" + ACV.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.AppraisalID + "\" name=\"Acv\" class=\"sForm\" onblur=\"javascript:updateACV(this);\" value=\"" + car.ACV + "\" />" + Environment.NewLine);
--%>
                <input type="text" class="sForm" value="<%=item.TradeInFairValue %>" id="inpSaveCost_<%=item.ID%>"> />
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<% } %>
