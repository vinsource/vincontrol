<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.CommonManagement.KarPowerViewModel>" %>
<br />
<table border="1" cellpadding="2" cellspacing="2" width="750px" style="font-size: 1.9em;
    color: White; border-color: White;">
    <tr>
        <td style="background-color: #0062A0">
            <%= Model.BaseWholesale %>
        </td>
        <td style="background-color: #008000">
            <%= Model.MileageAdjustment %>
        </td>
        <td style="background-color: #D02C00">
            <%= Model.Wholesale %>
        </td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="2" width="750px" style="background-color: #E8CB95">
    <tr>
        <td>
            Year
        </td>
        <td>
            <%= Model.SelectedYearId %>
        </td>
        <td>
            Engine
        </td>
        <td>
            <%= Html.DropDownListFor(m => m.SelectedEngineId, Model.Engines, new { @class = "dropdownlist" })%>
        </td>
    </tr>
    <tr>
        <td>
            Make
        </td>
        <td>
            <%= Html.DropDownListFor(m => m.SelectedMakeId, Model.Makes, new { @class = "dropdownlist", disabled = true })%>
        </td>
        <td>
            Transmission
        </td>
        <td>
            <%= Html.DropDownListFor(m => m.SelectedTransmissionId, Model.Transmissions, new { @class = "dropdownlist" })%>
        </td>
    </tr>
    <tr>
        <td>
            Model
        </td>
        <td>
            <% if (!Model.IsMultipleModels) { %>
            <%= Html.DropDownListFor(m => m.SelectedModelId, Model.Models, new {@class = "dropdownlist", disabled = true}) %>
            <% } else { %>
            <%= Html.DropDownListFor(m => m.SelectedModelId, Model.Models, new {@class = "dropdownlist"}) %><% } %>
        </td>
        <td>
            Drive Train
        </td>
        <td>
            <%= Html.DropDownListFor(m => m.SelectedDriveTrainId, Model.DriveTrains, new { @class = "dropdownlist" })%>
        </td>
    </tr>
    <tr>
        <td>
            Trim
        </td>
        <td>
            <% if (!Model.IsMultipleTrims)
               {%>
            <%=Html.DropDownListFor(m => m.SelectedTrimId, Model.Trims, new { @class = "dropdownlist", disabled = true })%>
            <%}
               else
               {%>
            <%=Html.DropDownListFor(m => m.SelectedTrimId, Model.Trims, new {@class = "dropdownlist"})%>
            <%}%>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
</table>
<br />
<%--<div><%= Model.OptionalEquipmentMarkup %></div>--%>
<div style="background-color: #EAEBEB; width: 750px;">
    <div style="margin-bottom: 5px; font-size: 1.1em;">
        Optional Equipment</div>
    <table id="options" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
        <tbody>
            <tr>
                <%
                    int i = 0;
                    foreach (var option in Model.OptionalEquipmentMarkupList)
                    {
                        i += 1;
                %>
                <% if (!(option.Id.Equals(Model.SelectedDriveTrainId.ToString()) || option.Id.Equals(Model.SelectedEngineId.ToString()) || option.Id.Equals(Model.SelectedTransmissionId.ToString())))
                   {%>
                <% if (!string.IsNullOrEmpty(option.DisplayName)) %>
                <%
                   { %>
                <td>
                    <span>
                        <input id="option_<%= option.Id %>" type="checkbox" name="option_<%= option.Id %>"
                            <%= option.IsSelected ? "checked" : "" %> />
                        <label>
                            <%= option.DisplayName %></label>
                    </span>
                </td>
                <% } %>
                <%} %>
                <%if (i % 4 == 0)
                  {%>
            </tr>
            <tr>
                <%} %>
                <%}%>
                <% if (i % 4 != 0)
                   {%></tr>
            <%}%>
        </tbody>
    </table>
</div>
<% Html.BeginForm("PrintReport", "Market", FormMethod.Post, new { id = "data" }); %>
<%--<form id="data" method="post" action="">--%>
<input type="hidden" id="DownloadTokenValueId" name="DownloadTokenValueId" />
<input type="hidden" id="baseWholesale" name="baseWholesale" value="<%= Model.BaseWholesale %>" />
<input type="hidden" id="wholesale" name="wholesale" value="<%= Model.Wholesale %>" />
<input type="hidden" id="Type" name="Type" value="<%= Model.Type %>" />
<input type="hidden" id="BaseWholesale" name="BaseWholesale" value="<%= Model.BaseWholesale %>" />
<input type="hidden" id="Wholesale" name="Wholesale" value="<%= Model.Wholesale %>" />
<input type="hidden" id="SelectedOptionIds" name="SelectedOptionIds" value="<%= Model.SelectedOptionIds %>" />
<div style="width: 750px; margin-top: 5px; text-align: right">
    <font style="color: white;">Report:</font>&nbsp;
    <%= Html.DropDownListFor(m => m.SelectedReport, Model.Reports, new { @class = "dropdownlist" })%>
    &nbsp;
    <input type="button" name="btnPrint" id="btnPrint" value="Print" />
    <%--<input type="submit" name="btnSubmit" id="btnSubmit" value="Print" />--%>
    <input type="button" name="btnSave" id="btnSave" value="Save" <%= Model.HasVin ? "" : "style='display:none'" %> />
</div>
<%--</form>--%>
<% Html.EndForm(); %>