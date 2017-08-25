<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.CommonManagement.KarPowerViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>

<br />
<table border="1" cellpadding="2" cellspacing="2" width="750px" style="font-size:1.9em; color:White; border-color:White;">
    <tr>
        <td style="background-color:#0062A0"><%= Model.BaseWholesale %></td>
        <td style="background-color:#008000"><%= Model.MileageAdjustment %></td>
        <td style="background-color:#D02C00"><%= Model.Wholesale %></td>
    </tr>
</table>

<table border="0" cellpadding="2" cellspacing="2" width="750px" style="background-color:#E8CB95">
    <tr>
        <td>Year</td>
        <td><%= Model.SelectedYearId %></td>
        <td>Engine</td>
        <td><%= Html.DropDownListFor(m => m.SelectedEngineId, Model.Engines.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist", disabled = true })%></td>
    </tr>
    <tr>
        <td>Make</td>
        <td><%= Html.DropDownListFor(m => m.SelectedMakeId, Model.Makes.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist", disabled = true })%></td>
        <td>Transmission</td>
        <td>        
        <%= Html.DropDownListFor(m => m.SelectedTransmissionId, Model.Transmissions.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist" })%>
        </td>
    </tr>
    <tr>
        <td>Model</td>
        <td><%= Html.DropDownListFor(m => m.SelectedModelId, Model.Models.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist", disabled = true })%></td>
        <td>Drive Train</td>
        <td><%= Html.DropDownListFor(m => m.SelectedDriveTrainId, Model.DriveTrains.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist" })%></td>
    </tr>
    <tr>
        <td>Trim</td>
        <td>
        <% if (Model.IsMultipleTrims == false){%>
        <%=Html.DropDownListFor(m => m.SelectedTrimId, Model.Trims.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist", disabled = true })%>
        <%}else {%>
        <%=Html.DropDownListFor(m => m.SelectedTrimId, Model.Trims.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist" })%>
        <%}%>
        </td>
        <td></td>
        <td></td>
    </tr>
</table>
<br />

<%--<div><%= Model.OptionalEquipmentMarkup %></div>--%>
<div style="background-color:#EAEBEB; width:750px;">
    <div style="margin-bottom:5px;font-size:1.1em;">Optional Equipment</div>
    <table id="options" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
    <tbody>
        <tr>
        <%
            int i = 0; 
            foreach (var option in Model.OptionalEquipmentMarkupList)
            {
                i += 1;
        %>        
            <% if (!(option.Id.Equals(Model.SelectedDriveTrainId.ToString()) || option.Id.Equals(Model.SelectedEngineId.ToString()) || option.Id.Equals(Model.SelectedTransmissionId.ToString()))){%>
            <td>
                <span>
                    <input id="option_<%= option.Id %>" type="checkbox" name="option_<%= option.Id %>" <%= option.IsSelected ? "checked" : "" %>/>
                    <label><%= option.DisplayName %></label>
                </span>
            </td>
            <%} %>
        <%if (i % 4 == 0){%>
        </tr><tr>
        <%} %>
        <%}%>
        <% if (i % 4 != 0){%></tr><%}%>        
    </tbody>
    </table>
</div>

<% Html.BeginForm("PrintReport", "KarPower", FormMethod.Post, new { id = "data"}); %>

<input type="hidden" id="DownloadTokenValueId" name="DownloadTokenValueId"/>
<input type="hidden" id="baseWholesale" name="baseWholesale" value="<%= Model.BaseWholesale %>" />
<input type="hidden" id="wholesale" name="wholesale" value="<%= Model.Wholesale %>" />
<input type="hidden" id="SelectedOptionIds" name="SelectedOptionIds" value="<%= Model.SelectedOptionIds %>" />
<div style="width:750px; margin-top:5px; text-align: right">
    <font style="color:white;">Report:</font>&nbsp; <%= Html.DropDownListFor(m => m.SelectedReport, Model.Reports.ToSelectItemList(m => m.Value, m => m.Text, false), new { @class = "dropdownlist" })%> &nbsp;
    <input type="button" name="btnPrint" id="btnPrint" value="Print" />
    <input type="button" name="btnSave" id="btnSave" value="Save" <%= Model.HasVin ? "" : "style='display:none'" %> />
</div>

<% Html.EndForm(); %>