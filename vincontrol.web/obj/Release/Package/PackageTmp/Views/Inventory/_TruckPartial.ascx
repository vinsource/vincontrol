<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>
<%@ Import Namespace="vincontrol.DomainObject" %>

    <%--<% if (Model.IsTruck){%>--%>
    <div id="divTruckContainer" style="text-align: right; cursor: pointer; margin-left: 3px; float: right; display: none;">
    <img id="imgTruck" src="../../images/bt_truck.png" style="border: 0px;width:120px;height:20px;margin-top:2px;margin-right:5px;" />
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
    </div>
    <%--<%} %>--%>


