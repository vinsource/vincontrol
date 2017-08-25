<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.PriceChangeViewModel>" %>
<%@ Import Namespace="WhitmanEnterpriseMVC.Controllers" %>
<div id="container">
    <% if (Model != null && Model.PriceChangeHistory != null && Model.PriceChangeHistory.Any())
       {%>
    <div style="max-height: 280px; overflow-y:auto; margin-bottom: 20px;">
        <table id="manheimTransaction" width="100%" class="reportText">
            <thead style="cursor: pointer;">
                <tr>
                    <th align="left" width="30%">
                        User Stamp
                    </th>
                    <th align="left" width="30%">
                        Date Stamp
                    </th>
                    <th align="right" width="20%">
                        Previous Price
                    </th>
                    <th align="right" width="20%">
                        Sale Price
                    </th>
                    <%--<th align="center" width="25%">
            Download
        </th>--%>
                </tr>
            </thead>
            <tbody>
                <% foreach (var item in Model.PriceChangeHistory.OrderByDescending(i=>i.DateStamp).Take(10))
                   {%>
                <tr>
                    <td align="left">
                        <%= item.UserStamp %>
                    </td>
                    <td align="left">
                        <%= item.DateStamp.ToString("MM/dd/yyy hh:mm:ss") %>
                    </td>
                    <td align="right">
                        <%= item.OldSalePrice.Equals(0) ? "" : item.OldSalePrice.ToString("c0")%>
                    </td>
                    <td align="right">
                        <%= item.NewSalePrice.Equals(0) ? "" : item.NewSalePrice.ToString("c0") %>
                    </td>
                    <%--<td align="center"><a href="javascript:;"><img src="../../images/pdf-icon.png" style="border:0" alt="Download Report" /></a></td>--%>
                </tr>
                <%}%>
            </tbody>
        </table>
    </div>
    
    <%}
       else
       {%>
    There is no change.
    <%}%>
    <br />
    <img src="/Inventory/RenderPriceChart?itemId=<%=Model.Id %>&chartTimeType=<%=Model.ChartType%>&inventoryStatus=<%=Model.InventoryStatus%>" />

</div>
