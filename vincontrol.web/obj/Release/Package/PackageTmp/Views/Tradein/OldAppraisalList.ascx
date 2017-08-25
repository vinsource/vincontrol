<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Vincontrol.Web.Models.TradeinCustomerViewModel>>" %>
<%  bool flag = false;
    foreach (var item in Model)
    {%>
<tr class="apa_table_row">
    <td class="apa_condition">
        <a href="<%=Url.Content("~/Appraisal/ViewProfileForCustomerAppraisal?CustomerId=")%><%=item.ID%>">
        <img src="/Content/images/quote_green.png" />
        </a>
    </td>
    <td class="apa_year">
        <%=item.Year %>
    </td>
    <td class="apa_make">
        <%=item.ShortMake%>
    </td>
    <td class="apa_model">
        <%=item.Model %>
    </td>
    <td class="apa_trim">
        <%=item.StrTrim %>
    </td>
    <td class="apa_client">
        <%=item.StrClientName %>
    </td>
    <td class="apa_email">
        <%=item.ClientContact %>
    </td>
    <td class="apa_phone">
        <%=item.Phone %>
    </td>
    <td class="apa_value">
        <input type="text" class="sForm" value="<%=item.TradeInFairValue %>" id="inpSaveValue_<%=item.ID%>" />
    </td>
    <td class="apa_acv">
        <label>
            <input type="text" class="sForm" value="<%=item.StrACV %>" id="inpSaveCost_<%=item.ID%>" />
            <input class="apa_acv_button" type="button" value="Send" />
        </label>
    </td>
    <td class="apa_status">
        <select class="apa_status_select">
            <option>Select... </option>
        </select>
    </td>
</tr>
<% } %>
