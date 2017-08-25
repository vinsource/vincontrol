<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AdvisorListViewModel>" %>

<% if (Model != null && Model.AdvisorList != null && Model.AdvisorList.Count > 0) %>
<% { %>
            <table id="ap_advisor_table">
                <tr class="apa_table_header">
                    <th class="apa_condition">
                        Cond.
                    </th>
                    <th class="apa_age">
                        Age
                    </th>
                    <th class="apa_year">
                        Year
                    </th>
                    <th class="apa_make">
                        Make
                    </th>
                    <th class="apa_model">
                        Model
                    </th>
                    <th class="apa_trim">
                        Trim
                    </th>
                    <th class="apa_client">
                        Client
                    </th>
                    <th class="apa_email">
                        Email
                    </th>
                    <th class="apa_phone">
                        Phone
                    </th>
                    <th class="apa_value">
                        Value
                    </th>
                    <th class="apa_acv">
                        ACV
                    </th>
                    <th class="apa_status">
                        Status
                    </th>
                </tr>
        <% foreach (var tmp in Model.AdvisorList)
            { %>
                <tr class="apa_table_header">
                    <td class="apa_condition">
                        <%= tmp.Condition %>
                    </td>
                    <td class="apa_age">
                        <%= tmp.Age %>
                    </td>
                    <td class="apa_year">
                        <%= tmp.Year %>
                    </td>
                    <td class="apa_make">
                        <%= tmp.Make %>
                    </td>
                    <td class="apa_model">
                        <%= tmp.Model %>
                    </td>
                    <td class="apa_trim">
                        <%= tmp.StrTrim %>
                    </td>
                    <td class="apa_client">
                        <%= tmp.ClientContact %>
                    </td>
                    <td class="apa_email">
                        <%= tmp.Email %>
                    </td>
                    <td class="apa_phone">
                        <%= tmp.Phone %>
                    </td>
                    <td class="apa_value">
                        <%= tmp.TradeInFairValue %>
                    </td>
                    <td class="apa_acv">
                        <%= tmp.StrACV %>
                    </td>
                    <td class="apa_status">
                        <%= tmp.TradeInStatus %>
                    </td>
                </tr>
        <% } %>
            </table>
<% } %>
<% else %>
<% { %>
        <div style="text-align: center; padding-top: 10px;" >
            There are no results which match your filter criteria.
        </div>    
<% } %>