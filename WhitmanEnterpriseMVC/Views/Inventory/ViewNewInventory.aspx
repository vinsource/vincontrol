<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.InventoryFormViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
<link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>
<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<!-- the jScrollPane script -->

<style type="text/css">
#notes {position: relative; top: -220px; left: 150px; width: 750px; color: white; z-index: 0;}
#c2 {width: 784px;}
h4 {margin-bottom: 0; margin-top: 0;}
#sort {margin-top: -1.5em;}
p {margin-top: 0; }
#recent li {margin-left: -3px; margin-top: .3em; font-size: .9em;}
#recent .sForm {margin: 0;}
#recent .sForm[name="odometer"] {}
img.thumb {float: left; clear: right;}

body {background: url('../images/cBgRepeatW.png') top center repeat-y;}


#c2 {border-right: none;}
#table { width: 850px; padding: 0; margin: 0; font-size: .8em;}
#table div { padding: 0;  margin: 0;}
#table .cell {width: 95px; height:20px; 
overflow: hidden; 
margin-bottom: 3px;background: none;}
#table .longest {width: 184px;}
#table .long {width: 99px;}
#table .short {width: 67px;}
#table .shortest {width: 29px;}
#table .infoCell {width: 674px;}
#table .mid {width: 84px;}
div.evenShorter{width: 15px !important; border: none;}
#table div {margin-bottom: .5em;}
.scroll-pane { height: 700px; overflow: auto; overflow-x: hidden; }
#table {position: relative; left: .5em; font-size: 9.5pt; word-spacing: -1px; width: 750px;}
#table .shorter {width: 55px;}
#table .eC {width: 100px;}
.rowOuter {!important; margin-bottom: .5em !important; padding: .5em !important; padding-right:0 !important; padding-bottom: .1em !important;}
.light {background: #555;}
.dark {background: #111;}
#tableHeader .cell {border: none;
	border-bottom: 5px solid #990000;
	height: 20px !important;  
	width: 97px;
	 margin:0;
	  padding: 0; 
	  font-weight:bold;
	  margin-right: 3px;}
#tableHeader .longest {width: 184px;}
#tableHeader .longer{width: 137px;}
#tableHeader .long {width: 99px;}
#tableHeader .short {width: 65px;}
#tableHeader .shortest {width: 29px;}
#tableHeader .infoCell {width: 674px;}
#tableHeader .mid {width: 85px;}
#tableHeader .shorter {width: 57px;}
.cell{text-align: center; border-left: 2px solid #860000;}
.noBorder {border: none !important;}
#tableHeader .rowOuter {border: none !important;}
#tableHeader .start {margin-left: 50px; width: 89px;}
.dark .marketSection {background: #460000 !important;}
.light .marketSection {background: #460000 !important;}
.border {border: 3px red solid;}
.med {border: 3px yellow solid;}
.acar { border: 3px green solid; }
input[name="price"] {background: green !important; }
.hider {display: none;}

.imageWrap .text {
	position:absolute ;
	top:8px; /* in conjunction with left property, decides the text position */
	left:4px;
	width:300px; /* optional, though better have one */
color:Red;
}
.hideLoader {display: none;}


</style>
	<title>Inventory</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />

    
               <div id="recent">
               <div id="other" style="float:right; margin-top:-25px;">
               <%if (Model.CurrentOrSoldInventory)
                     {%>
                <b id="soldVehicles" style="float:right; cursor: pointer;"><%= Html.ActionLink("View Sold Vehicles", "ViewFullNewSoldInventory", "Inventory")%></b>
                <% }
                     else
                     { %>
                <b id="B1" style="float:right; cursor: pointer;"><%= Html.ActionLink("View Current Vehicles", "ViewFullNewInventory", "Inventory")%></b>
                <% } %>
                  <% if ((bool) Session["IsEmployee"] == false)
                             { %>
                    <b id="TodayBucketJump" style="float: left; cursor: pointer;">
                <%= Html.ActionLink("Today Bucket Jump", "TodayBucketJump", "Inventory")%> &nbsp;&nbsp;
            </b>
                <b id="expressBucketJump" style="float: right; cursor: pointer;">
                    <%= Html.ActionLink("Express Bucket Jump", "ExpressBucketJump", "Inventory")%> &nbsp;&nbsp;
                </b>
                  <% } %>
                <div class="clear" style="font-size: .75em; float:right;">
                <div id="color0" style="margin-top: 2px; margin-right: .5em; float: left; clear: right;
                    padding: .2em; border: 1px solid green">
                    <%= Html.ActionLink("A Cars", "ACarNewInventory", "Inventory")%></div>
                <div id="color1" style="margin-top: 2px; margin-right: .5em; float:left; clear:right; padding:.2em; border: 1px solid yellow">     	<%= Html.ActionLink("Missing Content", "MissContentNewInventory", "Inventory")%></div>
                <div id="color2" style="margin-top: 2px; margin-right: .5em; padding:.2em; float:left; clear:right; border: 1px solid red">     	<%= Html.ActionLink("No Content", "NoContentNewInventory", "Inventory")%></div></div>
                </div>
                <h3 id="invenHeader">Number of Cars = <%=Model.CarsList.Count().ToString()%></h3>
                <div id="tableHeader">
                   		<div class="rowOuter">
                        	<div class="innerRow1 clear">
                            	<div class="cell start column">
                                	<%= Html.ActionLink("Year", "SortInventory", "Inventory", new { id = "Year" }, null)%>
                                </div>
                                <div class="long cell column">
                                 	<%= Html.ActionLink("Make", "SortInventory", "Inventory", new { id = "Make" }, null)%>
                                </div>
                                <div class="mid cell column">
                                <%= Html.ActionLink("Model", "SortInventory", "Inventory", new { id = "Model" }, null)%>
                                </div>
                                <div class=" cell column">
                               <%= Html.ActionLink("Trim", "SortInventory", "Inventory", new { id = "Trim" }, null)%>
                                </div>
                                <div class="mid cell column">
                                 <%= Html.ActionLink("Stock", "SortInventory", "Inventory", new { id = "Stock" }, null)%>
                                </div>
                                <div class="mid cell column">
                                   <%= Html.ActionLink("Age", "SortInventory", "Inventory", new { id = "Age" }, null)%>
                                </div>
                                <div class="shorter cell column">
                                   <%= Html.ActionLink("Miles", "SortInventory", "Inventory", new { id = "Miles" }, null)%>
                                </div>
                               <div class="shorter cell column">
                                  <%= Html.ActionLink("Price", "SortInventory", "Inventory", new { id = "Price" }, null)%>
                                </div>
                                  <%if (Model.CurrentOrSoldInventory)
                     {%>
                   <div id="switch-view" title="Switch to Small Cells">
                                 <a   href="<%=Url.Content("~/Inventory/ViewNewInventory")%>">
                                    <img src=<%=Url.Content("~/Images/large-view.jpg")%> />
                                </a>
                            </div>

                <% }
                     else
                     { %>
                 <div id="switch-view" title="Switch to Small Cells">
                                 <a   href="<%=Url.Content("~/Inventory/ViewNewSoldInventory")%>">
                                    <img src=<%=Url.Content("~/Images/large-view.jpg")%> />
                                </a>
                            </div>
                <% } %>
                            </div>
                            <div class="clear"></div>
                        </div>
                   </div>
                   
                   
                      <%=Html.DynamicHtmlLabelForInventory("InventoryGrid")%>
                      
                    
                     
                     
                </div>
             <%--<script src="<%=Url.Content("~/js/fieldValidation.js")%>" type="text/javascript"></script>--%>
<script language="javascript" type="text/javascript">

    $(document).ready(function() {
    $(".sForm").numeric({ decimal: false, negative: false }, function() { alert("Positive integers only"); this.value = ""; this.focus(); });
    if ($("#IsEmployee").val() == 'True') {
        $("input[name='odometer']").attr('readonly', 'true');
        $("input[name='price']").attr('readonly', 'true');
    }
    });

    function updateMileage(txtBox) {


        $.post('<%= Url.Content("~/Inventory/UpdateMileageFromInventoryPage") %>', { Mileage: txtBox.value, ListingId: txtBox.id }, function(data) {


        });
    }


    function updateSalePrice(txtBox) {
      
        $.post('<%= Url.Content("~/Inventory/UpdateSalePriceFromInventoryPage") %>', { SalePrice: txtBox.value, ListingId: txtBox.id }, function(data) {



        });

    }
    
    function updateReconStatus(txtBox) {
      
        $.post('<%= Url.Content("~/Inventory/UpdateReconStatusFromInventoryPage") %>', { Reconstatus: txtBox.checked, ListingId: txtBox.id }, function(data) {



        });

    }

    $('a:not(.iframe)').click(function(e) {
        if ($(this).attr('target') == '')
            $('#elementID').removeClass('hideLoader');

    });

    $("a.iframe").fancybox({ 'width': 1010, 'height': 700 });


    $('.rowOuter').each(function() {
        var status = $(this).children('.imageCell').children('.imageWrap').children('input').attr('value');
        console.log(status);
        if (status == 0 || status == undefined) {
            //console.log('good status');
            $(this).removeClass('border');
        } else {
            //console.log('bad status');
        $(this).addClass('border');
            if (status == 1 || status==2) { console.log('med status'); $(this).addClass('med'); }
        }
    });
    $('#sold').hide('fast');
    var toggle = false;
//    $('#soldVehicles').click(function() {
//        if (toggle == false) {
//            toggle = true;
//            document.getElementById('soldVehicles').innerHTML = 'View Current Inventory';
//            document.getElementById('invenHeader').innerHTML = "Sold Inventory";
//        } else {
//            toggle = false;
//            document.getElementById('soldVehicles').innerHTML = 'View Sold Inventory';
//            document.getElementById('invenHeader').innerHTML = "Current Inventory";
//        }
//        $('#sold').slideToggle('slow');
//        $('#inven').slideToggle('slow');
//    });

  
    </script>
    
</asp:Content>
