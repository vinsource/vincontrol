<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AppraisalViewFormModel>" %>
<%@ Import Namespace="vincontrol.DomainObject" %>

<div id="divTruckContainer" class="column_one_items" style="text-align: center; cursor: pointer; margin-left: 3px; display: none;">
    <%--<% if (Model.VinDecodeSuccess && Model.IsTruck){%>--%>
    <img id="imgTruck" src="../../images/bt_truck.png" style="border: 0px;" />
    <div id="divTruck" class="column_one_items" style="display: none; width: 300px; height: 80px;">
        <div style="display: inline-block; width: 100%;">
            <label style="width: 120px;">Truck Type</label>
            <%=Html.DropDownListFor(x => x.SelectedTruckType, Model.TruckTypeList, "Select...")%>
        </div>
        <div style="display: inline-block; width: 100%;">
            <label style="width: 120px;">Truck Category</label>
            <% if (!String.IsNullOrEmpty(Model.SelectedTruckType)){ %>
            <%= Html.DropDownListFor(x => x.SelectedTruckCategoryId, Model.TruckCategoryList, "Select...") %>
            <% } else { %>
            <%= Html.DropDownListFor(x => x.SelectedTruckCategoryId, new List<ExtendedSelectListItem>(), "Select...") %>
            <% } %>
        </div>
        <div style="display: inline-block; width: 100%;">
            <label style="width: 120px;">Truck Class</label>
            <%=Html.DropDownListFor(x => x.SelectedTruckClassId, Model.TruckClassList, "Select...")%>
        </div>
    </div>
    <%--<%} %>--%>
</div>

