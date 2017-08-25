<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AppraisalViewFormModel>" %>
<script type="text/javascript">
    function checkOtherExteriorColor(field, rules, i, options) {
        if ($("#SelectedExteriorColor").val() == "Other Colors" && field.val().length == 0) {
            rules.push('required');
        }
    }

    function checkOtherInteriorColor(field, rules, i, options) {
        if ($("#SelectedInteriorColor").val() == "Other Colors" && field.val().length == 0) {
            rules.push('required');
        }
    }
</script>
<div class="column">
    <table>
        <tr>
            <td>VIN</td>
            <td><%=Html.TextBoxFor(x => x.VinNumber, new { @readonly = "readonly" })%></td>
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
            <td><%=Html.TextBoxFor(x => x.ModelYear, new { @readonly = "readonly" })%></td>
        </tr>
        <tr>
            <td>Make</td>
            <td><%=Html.TextBoxFor(x => x.Make, new { @readonly = "readonly" })%></td>
        </tr>
        <tr>
            <td>Model</td>
            <td><%=Html.TextBoxFor(x => x.AppraisalModel)%></td>
        </tr>
        <tr>
            <td>Trim</td>
            <td><%=Html.DropDownListFor(x=>x.SelectedTrim, Model.TrimList) %></td>
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
                <%=Html.DropDownListFor(x => x.SelectedTruckType, Model.TruckTypeList)%>
            </td>
        </tr>
        <tr>
            <td>
                Truck Class
            </td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedTruckClass, Model.TruckClassList)%>
            </td>
        </tr>
        <tr>
            <td>
                Truck Category
            </td>
            <td>
                <%=Html.DropDownListFor(x => x.SelectedTruckCategory, Model.TruckCategoryList)%>
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
                <%--<%=Html.TextBoxFor(x=>x.CusExteriorColor, new { @style="width: 50px !important;"})%>--%>
                <input type="text" name="CusExteriorColor" id="CusExteriorColor" style="width: 50px !important;" data-validation-engine="validate[funcCall[checkOtherExteriorColor]" data-errormessage-value-missing="Other exterior color is required!" />
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
                <%--<%=Html.TextBoxFor(x => x.CusInteriorColor, new { @style = "width: 50px !important;" })%>--%>
                <input type="text" name="CusInteriorColor" id="CusInteriorColor" style="width: 50px !important;" data-validation-engine="validate[funcCall[checkOtherInteriorColor]" data-errormessage-value-missing="Other interior color is required!" />
            </td>
        </tr>
        <tr>
            <td>Odometer</td>
            <td>
            <%--<%=Html.TextBoxFor(x=>x.Mileage)%>--%>
            <input type="text" name="Mileage" id="Mileage" data-validation-placeholder="0" data-validation-engine="validate[required]" data-errormessage-value-missing="Odometer is required!" />
            </td>
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
            <td>
            <%--<%=Html.DropDownListFor(x => x.SelectedTranmission, Model.TranmissionList)%>--%>
            <%= Html.CusDropDown("SelectedTranmission","DDLStyle", string.Empty, Model.TranmissionList,"validate[required]","Transmission is required!") %>
            <%--<select name="SelectedTranmission" id="SelectedTranmission" data-validation-engine="validate[required]" data-errormessage-value-missing="Transmission is required!">
                <% foreach (var item in Model.TranmissionList){%>
                <option value="<%= item.Value %>"><%= item.Text %></option>
                             
                <%}%>

            </select>--%>
            </td>
        </tr>
        <tr>
            <td>Doors</td>
            <td><%=Html.TextBoxFor(x=>x.Door, new { @readonly = "readonly" })%></td>
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
<%=Html.HiddenFor(x=>x.AppraisalGenerateId) %>
<%=Html.HiddenFor(x=>x.FuelEconomyCity) %>
<%=Html.HiddenFor(x=>x.FuelEconomyHighWay) %>
<%=Html.HiddenFor(x=>x.DefaultImageUrl) %>
<%=Html.HiddenFor(x => x.StandardInstalledOption)%>
<div class="column">
    <div id="AppraisalType" class="" style="font-size: .9em;">
        <input id="submit" class="pad" type="submit" name="submit" value="Complete Appraisal >" id="submitBTN" />        
        <span>&nbsp;</span>
    </div>
    <div>
        Packages:</div>
    <%= Html.CheckBoxGroupPackage("txtFactoryPackageOption", Model.FactoryPackageOptions)%>
    <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
    <div class="clear">
        Options:
    </div>
    <div id="optionals" class="" style="font-size: .9em;">
        <div class="clear">
        </div>
        <%= Html.CheckBoxGroupOption("txtNonInstalledOption", Model.FactoryNonInstalledOptions)%>
    </div>
    <div class="clear">
    </div>
</div>
<div id="notes" class="clear">
    Notes:
    <br />
    <%=Html.TextAreaFor(x=>x.Notes,new {@cols=87, @rows=3})%>
    <%=Html.HiddenFor(x=>x.ChromeModelId) %>
    <%=Html.HiddenFor(x=>x.ChromeStyleId) %>
    <p class="top">
    </p>
    <p class="bot">
    </p>
</div>
