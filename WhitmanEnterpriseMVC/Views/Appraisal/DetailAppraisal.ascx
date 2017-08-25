<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>
<div class="column" style="font-size: 0.9em;">
    <h3><%=Html.DynamicHtmlLabel("lblTitleWithoutTrim", "TitleWithoutTrim")%></h3>
    <h5><%=Model.Trim%></h5>
    <table>
        <tr>
            <td>VIN</td>
           <td>    <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.VinNumber, new { @class = "z-index", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.VinNumber, new { @class = "z-index" })%>
                    <%}%>
             </td>
            
        </tr>
        <tr>
            <td>Stock</td>
            <td><%=Html.TextBoxFor(x => x.StockNumber)%></td>
        </tr>
        <tr>
            <td>Date</td>
            <td><%=Html.TextBoxFor(x => x.AppraisalDate, new { @readonly = "readonly" })%></td>
        </tr>
        <tr>
            <td>Year</td>
         
            <td>    <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "small", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "small" })%>
                    <%}%>
             </td>
        </tr>
        <tr>
            <td>Make</td>
              <td>    <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.Make, new { @class = "z-index", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.Make, new { @class = "z-index" })%>
                    <%}%>
             </td>
        </tr>
        <tr>
            <td>Model</td>
            <td>    <% if (Model.VinDecodeSuccess) {%>
                         <%= Html.TextBoxFor(x => x.SelectedModel, new { @class = "z-index", @readonly = "readonly" })%>
                      
                    <%} else {%>
                        <%= Html.TextBoxFor(x => x.SelectedModel, new { @class = "z-index" })%>
                    <%}%>
             </td>
        </tr>
        <tr>
            <td>Trim</td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedTrimItem, Model.TrimList, new { @class = "z-index" })%>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <em style="font-size: .7em;">Other: </em>
                <%= Html.TextBoxFor(x => x.CusTrim, new { @style = "width: 70px !important;" })%>
            </td>
        </tr>
        <% if (Model.IsTruck) {%>
        <tr>
            <td>
                Truck Type
            </td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedTruckType, Model.TruckTypeList ??new List<SelectListItem>())%>
            </td>
        </tr>
        <tr>
            <td>
                Truck Class
            </td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedTruckClass, Model.TruckClassList ?? new List<SelectListItem>())%>
            </td>
        </tr>
        <tr>
            <td>
                Truck Category
            </td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedTruckCategory, Model.TruckCategoryList ?? new List<SelectListItem>())%>
            </td>
        </tr>
        <%}%>
        <tr>
            <td>Exterior Color</td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ExteriorColorList, new { id = "SelectedExteriorColor" })%>
                <input type="hidden" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue" value="<%=Model.SelectedExteriorColorValue %>" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <em style="font-size: .7em;">Other: </em>
                <%=Html.TextBoxFor(x=>x.CusExteriorColor, new { @style="width: 50px !important;"})%>
            </td>
        </tr>
        <tr>
            <td>Interior Color</td>
            <td><%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %></td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <em style="font-size: .7em;">Other: </em>
                <%=Html.TextBoxFor(x => x.CusInteriorColor, new { @style = "width: 50px !important;" })%>
            </td>
        </tr>
        <tr>
            <td>Vehicle Type</td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedVehicleType, Model.VehicleTypeList)%>
            </td>
        </tr>
        <tr>
            <td>Odometer</td>
            <td><%=Html.TextBoxFor(x=>x.Mileage)%></td>
        </tr>
        <tr>
            <td>Cylinders</td>
            <td><%=Html.DropDownListFor(x=>x.SelectedCylinder,Model.CylinderList )%></td>
        </tr>
        <tr>
            <td>Liters</td>
            <td><%=Html.DropDownListFor(x=>x.SelectedLiters,Model.LitersList )%></td>
        </tr>
        <tr>
            <td>Transmission</td>
            <td><%=Html.DropDownListFor(x => x.SelectedTranmission, Model.TranmissionList)%></td>
        </tr>
        <tr>
            <td>Doors</td>
            <td><%=Html.TextBoxFor(x=>x.Door, new { @class = "small", @readonly = "readonly" })%></td>
        </tr>
        <tr>
            <td>Style</td>
            <td><%=Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList)%></td>
        </tr>
        <tr>
            <td>Fuel</td>
            <td><%=Html.DropDownListFor(x => x.SelectedFuel, Model.FuelList)%></td>
        </tr>
        <tr>
            <td>Drive</td>
            <td><%=Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList)%></td>
        </tr>
        <tr>
            <td>Original MSRP</td>
            <td><%=Html.TextBoxFor(x=>x.MSRP, new { @readonly = "readonly" })%></td>
        </tr>
    </table>
</div>
<div class="column" id="column-two" style="font-size: 0.9em;">
    <input class="pad" type="submit" name="CancelAppraisal" value=" Cancel " id="CancelAppraisal" />
    <% if (Model.IsTruck) {%>
    <input class="pad" type="submit" name="SaveEditTruckAppraisal" value=" Save Changes > " id="SaveAppraisal" />
    <%} else {%>
    <input class="pad" type="submit" name="SaveEditAppraisal" value=" Save Changes > " id="SaveAppraisal" />
    <%}%>
    <br />
    <br />
    Packages:<br />
    <div class="scrollable-list">
        <%= Html.CheckBoxGroupPackage("txtFactoryPackageOption", Model.FactoryPackageOptions,Model.ExistPackages)%>
        <input type="hidden" id="hdnFirstSelectedTrim" />
        <input type="hidden" id="hdnFistSelectedOptions" />
        <input type="hidden" id="hdnFistSelectedPackages" />
    </div>
    <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
    <div class="clear">
        Options:
    </div>
    <div class="scrollable-list">
        <div id="optionals" class="" style="font-size: .9em;">
            <div class="clear">
            </div>
            <%= Html.CheckBoxGroupOption("txtNonInstalledOption", Model.FactoryNonInstalledOptions,Model.ExistOptions)%>
            <div class="clear">
            </div>
        </div>
    </div>
</div>
<div class="clear" id="img-title">
</div>
<div id="other-details" class="clear">
    <div id="description-box" class="column">
        <%=Html.TextAreaFor(x=>x.Notes,new {@cols=90, @rows=9})%>
    </div>
</div>
<%=Html.HiddenFor(x=>x.AppraisalGenerateId) %>
<%=Html.HiddenFor(x=>x.FuelEconomyCity) %>
<%=Html.HiddenFor(x=>x.FuelEconomyHighWay) %>
<%=Html.HiddenFor(x=>x.DefaultImageUrl) %>
<%=Html.HiddenFor(x => x.StandardInstalledOption)%>

