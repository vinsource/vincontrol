<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Vincontrol.Web.Models.TradeinCustomerViewModel>>" %>
<%  bool flag = false;
    foreach (var item in Model)
    {%>
<tr class="apa_table_row">
    <td class="apa_condition">
        <img src="/Content/images/<%=item.Condition %>.png" />
    </td>
    <td class="apa_age">
        <%=item.Age %>
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
        <input type="text" class="sForm" value="<%=item.TradeInFairValue %>" id="inpSaveValue_<%=item.ID%>"
            readonly="readonly" />
    </td>
    <td class="apa_acv">
        <label>
            <input type="text" class="sForm apa_acv_input" value="<%=item.StrACV %>" id="ACV_<%=item.ID%>" />
            <input class="apa_acv_button" id="lnkSendEmail_<%=item.ID%>" type="button" value="Send" />
        </label>
    </td>
    <td class="apa_status">
        <select id="opttrade_<%=item.ID %>" class="apa_status_select">
            <option>Select... </option>
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
    </td>
</tr>
<% } %>
