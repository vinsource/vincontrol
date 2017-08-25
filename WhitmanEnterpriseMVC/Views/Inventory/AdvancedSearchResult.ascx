<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<WhitmanEnterpriseMVC.Models.CarInfoFormViewModel>>" %>
<% if(Model.Count == 0) {%>
There is no results found.
<%}else{%>

<% if((int)TempData["HasMultipleCategory"] == 1)
{%>

<a id="aAll" href="javascript:;" style=""><%= Model.Count %> Vehicle(s) Found</a> (
<%if ((int)TempData["NumberOfNewInventory"] == 0) {%>
    <%= (int)TempData["NumberOfNewInventory"] %> New
<%}else {%>
    <a id="aNew" href="javascript:;" style="color: Red;"><%= (int)TempData["NumberOfNewInventory"] %> New</a>
<%} %>
,&nbsp;
<%if ((int)TempData["NumberOfUsedInventory"] == 0) {%>
    <%= (int)TempData["NumberOfUsedInventory"] %> Used
<%}else {%>
    <a id="aUsed" href="javascript:;" style="color: Red;"><%= (int)TempData["NumberOfUsedInventory"] %> Used</a>
<%} %>
,&nbsp;
<%if ((int)TempData["NumberOfSoldInventory"] == 0) {%>
    <%= (int)TempData["NumberOfSoldInventory"] %> Sold
<%}else {%>
    <a id="aSold" href="javascript:;" style="color: Red;"><%= (int)TempData["NumberOfSoldInventory"] %> Sold</a>
<%} %>
,&nbsp;
<%if ((int)TempData["NumberOfWholesaleInventory"] == 0) {%>
    <%= (int)TempData["NumberOfWholesaleInventory"] %> Wholesale
<%}else {%>
    <a id="aWholesale" href="javascript:;" style="color: Red;"><%= (int)TempData["NumberOfWholesaleInventory"] %> Wholesale</a>
<%} %>
,&nbsp;
<%if ((int)TempData["NumberOfAppraisalInventory"] == 0) {%>
    <%= (int)TempData["NumberOfAppraisalInventory"] %> Appraisals
<%}else {%>
    <a id="aAppraisal" href="javascript:;" style="color: Red;"><%= (int)TempData["NumberOfAppraisalInventory"] %> Appraisals</a>
<%} %>
)
    
<%}else{%>
    <%= Model.Count %> Vehicle(s) Found
<%} %>


<div id="tableHeader">
    <div class="rowOuter">
        <div class="innerRow1 clear">
            <div class="cell start column">
                <a href="javascript:;" id="a_Year">Year</a>
            </div>
            <div class="long cell column">
                <a href="javascript:;" id="a_Make">Make</a>
            </div>
            <div class="mid cell column">
                <a href="javascript:;" id="a_Model">Model</a>
            </div>
            <div class=" cell column">
                <a href="javascript:;" id="a_Trim">Trim</a>
            </div>
            <div class="mid cell column">
                <a href="javascript:;" id="a_Stock">Stock</a>
            </div>
            <div class="mid cell column">
                <a href="javascript:;" id="a_Age">Age</a>
            </div>
            <div class="shorter cell column">
                <a href="javascript:;" id="a_Mile">Miles</a>
            </div>
            <div class="shorter cell column">
                <a href="javascript:;" id="a_Price">Price</a>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
</div>
<%= Html.DynamicHtmlLabelForAdvancedSearch("InventoryGrid")%>
<script type="text/javascript">
    $("a.iframe").fancybox({ 'width': 1010, 'height': 700 });
</script>
<%} %>

