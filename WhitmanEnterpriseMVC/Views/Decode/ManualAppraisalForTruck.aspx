<%@ Page Title="New Appraisal"  Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<title>New Appraisal</title>
<link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=Url.Content("~/Content/ui.datepicker.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/Scripts/jquery-1.6.2.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

<%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>

<script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

<script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>

<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<style type="text/css">
	#noteBox {width: 735px; margin-bottom: 1em;}
	#c2 {width: 785px; border-right: none; overflow: hidden; height: 100%;}
	h4 {margin-bottom: 0; margin-top: 0;}
	select { width: 135px;}
	p {  padding: 1em; border-bottom: 1px solid #101010; border-top: 1px #777 solid; margin: 0;}
p.top {border-top: none; padding: 0; margin-top: .5em;}
p.bot {border-bottom: none; padding: 0;}
p {margin-top: 0; }
body {background: url('../images/cBgRepeatW.png') top center repeat-y;}
#noteList {padding: 1em;}
#submit {float: right;}
#optionals {width: 410px; margin-top: 0; margin-bottom: 0; overflow: hidden;}
#c2 {padding-top: 0;}
.hider{display: none;}
input[name="options"] {margin-bottom: 0; margin-left: 0;}
textarea[name="description"] {width: 420px;}
#optionals ul{margin-top: .3em;}
.scroll-pane {height: 100%; overflow:auto; overflow-x: hidden;}
#notes {height: 200px; margin-bottom: 6em;}
input[name="completeApp"] {float: right;}

#Packages {max-height: 175px; width: 400px; overflow: hidden; overflow-y: scroll;}
#Options {max-height: 300px; width: 400px; overflow: hidden; overflow-y: scroll;}
.hideLoader {display: none;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            <div id="topNav">
                <input class="btn" type="button" name="inventory" value=" Car " id="btnCar" />
                <input class="btn onadmin" type="button" name="notifications" value=" Commercial Truck " id="btnTruck" />
               
                </div>
            <div class="column">
                   <% Html.BeginForm("ViewProfileByManualForTruck", "Appraisal", FormMethod.Post, new { id = "viewProfileForm",onsubmit="return validateForm()" }); %>
                        <table>
                            <tr>
                            	<td>VIN</td>
                                <td><%=Html.TextBoxFor(x=>x.VinNumber) %></td>
                         	</tr> 
                         
                              <tr>
                            	<td>Date</td>
                                <td><%=Html.TextBoxFor(x=>x.AppraisalDate, new { @readonly = "readonly" }) %> </td>
                         	</tr>
                            <tr>
                            	<td>Year</td>
                                <td><%=Html.TextBoxFor(x => x.SelectedModelYear)%></td>
                         	</tr>
                           <tr>
                            	<td>Make</td>
                            <td><%=Html.TextBoxFor(x => x.SelectedMake)%></td>
                                
                         	</tr>
                            <tr>
                            	<td>Model</td>
                            <td><%=Html.TextBoxFor(x => x.SelectedModel)%></td>
                                
                         	</tr>
                            <tr>
                            	<td>Trim</td>
                            	<td><%=Html.TextBoxFor(x => x.SelectedTrim)%></td>
                            
                         	</tr>
                         	<tr>
                            	<td>Truck Type</td>
                            	<td><%=Html.DropDownListFor(x => x.SelectedTruckType, Model.TruckTypeList)%></td>
                                
                         	</tr>
                         	<tr>
                            	<td>Truck Class</td>
                            	<td><%=Html.DropDownListFor(x => x.SelectedTruckClass, Model.TruckClassList)%></td>
                                
                         	</tr>
                         	<tr>
                            	<td>Truck Category</td>
                            	<td><%=Html.DropDownListFor(x => x.SelectedTruckCategory, Model.TruckCategoryList)%></td>
                                
                         	</tr>
                            <tr>
                            	<td>Exterior Color</td>
                                <td>
                               <%=Html.TextBoxFor(x => x.ExteriorColor)%>
                              
                                </td>
                         	</tr>
                         	
                            <tr>
                            	<td>Interior Color</td>
                                <td>
                               <%=Html.TextBoxFor(x => x.SelectedInteriorColor)%>
                              
                                </td>
                             
                         	</tr>
                         	
                            <tr>
                            	<td>Odometer</td>
                            	<td><%=Html.TextBoxFor(x=>x.Mileage)%></td>
                                
                         	</tr>
                            <tr>
                            	<td>Cylinders</td>
                            	 <td><%=Html.TextBoxFor(x => x.SelectedCylinder)%></td>
                            		
                               
                         	</tr>
                            <tr>
                            	<td>Liters</td>
                            	 <td><%=Html.TextBoxFor(x => x.SelectedLiters)%></td>
                            	
                         	</tr>
                            <tr>
                            	<td>Transmission</td>
                            	<td><%=Html.TextBoxFor(x => x.SelectedTranmission)%></td>
                           
                            </tr>
                            <tr>
                            	<td>Doors</td>
                             	<td><%=Html.TextBoxFor(x=>x.Door)%></td>
                         	</tr>
                          <tr>
                            	<td>Style</td>
                            	<td><%=Html.TextBoxFor(x => x.SelectedBodyType)%></td>
                            		
                                
                         	</tr>
                            <tr>
                            	<td>Fuel</td>
                            	<td><%=Html.TextBoxFor(x => x.SelectedFuel)%></td>
                           
                                
                         	</tr> 
                            <tr>
                            	<td>Drive</td>
                            		<td><%=Html.TextBoxFor(x => x.SelectedDriveTrain)%></td>
                           
                           
                                
                         	</tr> 
                            <tr>
                            	<td>Original MSRP</td>
                            	<td><%=Html.TextBoxFor(x=>x.MSRP)%></td>
                                
                         	</tr>
                         
                            <tr>
                                    <td style="font-size:.8em;" colspan="2">You <em>must</em> select an appraisal type:</td>
                                </tr>
                                <tr>
                                    <td style="font-size:.8em;" colspan="2">
                                  <%=Html.RadioButtonFor(x=>x.AppraisalType,"Customer") %> Customer
                                  <br />
                                  <%=Html.RadioButtonFor(x=>x.AppraisalType,"Auction") %> Auction
                                </td>
                                </tr>
                           </table>             
               </div>
               	<div class="column">
               	<input id="submit" class="pad" type="submit" name="submit" value="Complete Appraisal >" id ="submitBTN" /><br /><br />
               		
                      
                   	
				
                    
                 </div>
               
                 
            	
            
                    
                        <div id="notes" class="clear">
                        Notes: <br />
                             <%=Html.TextAreaFor(x=>x.Notes,new {@cols=87, @rows=3})%>
                              <%=Html.HiddenFor(x=>x.ChromeModelId) %>
                            <%=Html.HiddenFor(x=>x.ChromeStyleId) %>
                            <% Html.EndForm(); %>
                            <p class="top"></p>
                            
                            
                           <p class="bot"></p></div>
             

<script type="text/javascript">
  function validateForm() {

        var x = document.forms["viewProfileForm"];

        var flag = true;
        if ($("#VinNumberEx").length > 0) {
            $("#VinNumberEx").remove();
        }
        if ($("#SelectedModelYearEx").length > 0) {
            $("#SelectedModelYearEx").remove();
        }
        if ($("#SelectedModelYearWrongFormatEx").length > 0) {
            $("#SelectedModelYearWrongFormatEx").remove();
        }

        if ($("#SelectedMakeEx").length > 0) {
            $("#SelectedMakeEx").remove();
        }

        if ($("#SelectedModelEx").length > 0) {
            $("#SelectedModelEx").remove();
        }
        if ($("#CusErrorApp").length > 0) {
            $("#CusErrorApp").remove();
        }
        

        if ($("#VinNumber").val() == "") {
            $("#VinNumber").parent().append("<strong id='VinNumberEx'><font color='Red'>Required</font></strong>");
            flag = false;
        }

        if ($("#SelectedModelYear").val() == "") {
            $("#SelectedModelYear").parent().append("<strong id='SelectedModelYearEx'><font color='Red'>Required</font></strong>");
            flag = false;
        }
        else {
            if (!IsNumeric($("#SelectedModelYear").val())) {
                $("#SelectedModelYear").parent().append("<strong id='SelectedModelYearWrongFormatEx'><font color='Red'>Positive number only</font></strong>");
                flag = false;
            }
        }
        if ($("#SelectedMake").val() == "") {
            $("#SelectedMake").parent().append("<strong id='SelectedMakeEx'><font color='Red'>Required</font></strong>");
            flag = false;
        }
        if ($("#SelectedModel").val() == "") {
            $("#SelectedModel").parent().append("<strong id='SelectedModelEx'><font color='Red'>Required</font></strong>");
            flag = false;
        }
        
//        if (x.AppraisalType[0].checked == false && x.AppraisalType[1].checked == false) {
//            $("#AppraisalType").parent().parent().parent().append("<strong id='CusErrorApp'><font color='Red'>Required</font></strong>");
//            flag = false;
//        }

        if (flag == false)
            return false;
        else
            $('#elementID').removeClass('hideLoader');
    }


    $("#btnCar").click(function() {


        $('#elementID').removeClass('hideLoader');

        var actionUrl = '<%= Url.Action("DecodeProcessingManual", "Decode") %>';

        window.location = actionUrl;


    });

    $("#btnTruck").click(function() {


        $('#elementID').removeClass('hideLoader');



        var actionUrl = '<%= Url.Action("DecodeProcessingTruckManual", "Decode") %>';

        window.location = actionUrl;


    });
    </script>
    </asp:Content>