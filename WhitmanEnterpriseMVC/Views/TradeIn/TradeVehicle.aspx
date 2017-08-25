<%@ Page Title="Trade In" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.TradeInVehicleModel>" %>

<!DOCTYPE html>
<html>
<head>
	<title>Trade-In Value</title>
	<link href="<%= Url.Content("~/Css/TradeIn/style.css") %>" rel="stylesheet" type="text/css" />

<!--[if lte IE 7]>
	<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Css/TradeIn/ie-7.css") %>">
	<![endif]-->
<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

<%--<script src="http://code.jquery.com/jquery-latest.js"></script>--%>
<script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
</head>
<body>
	
	
	
	<div id="container" class="step1">

		<div id="header">

			<div class="logo"> </div>

			<div class="mask">
				<div class="text-wrap">
					<h1>Get Your Trade In Value!</h1>
				</div>
			</div>
			<div class="steps">
				<div id="step-1" class="step"><img src="<%= Url.Content("~/images/on-step-1.png")%>" alt="step 1"/></div>
				<div id="step-2" class="step"><img src="<%= Url.Content("~/images/step-2.png")%>" alt="step 2"/></div>
				<div id="step-3" class="step"><img src="<%= Url.Content("~/images/step-3.png")%>" alt="step 3"/></div>
			</div>
		</div>
		
		<div class="slide-wrapper">
			<div class="info-wrap">
				<h3 class="description-header">Thinking Of Trading In Your Vehicle? </h3>
				<div class="info-box">
				    
					<p class="description">Input your VIN number or select from the dropdown choices, then fill out the rest of the form and you will be on your way to knowing your vehicle's trade-in value!</p>
				</div>
			</div>

			<div  id="vehicle-info" class="info-wrap">
				<h3 class="description-header">Vehicle Information</h3>
				  <% Html.BeginForm("TradeInWithOptions", "TradeIn", FormMethod.Post, new { id = "TradeVehicleForm", onsubmit = "return validateForm()" }); %>
				<div class="info-box">
				      <div class="error-wrap">
						<p class="error" title="Click to Close"></p>
					</div>
					<div class="row">
							<div id="decode" class="row-cell">
							<h3>Vehicle Info</h3>
							<%= Html.TextBoxFor(x => x.Vin,new {title="VIN Number" ,placeholder="Enter Vin Number Here!"}) %>
							<%=Html.HiddenFor(x=>x.ValidVin) %>
							<em> - or - </em>
							<b>Select</b>
						    <%= Html.DropDownListFor(x => x.SelectedYear, Model.YearsList) %>
					        <%= Html.DropDownListFor(x => x.SelectedMake, Model.MakesList) %>
					        <%= Html.DropDownListFor(x => x.SelectedModel, Model.ModelsList) %>
					        <%= Html.DropDownListFor(x => x.SelectedTrim, Model.TrimsList) %>
					        
					         <%=Html.HiddenFor(x=>x.Dealer) %>
						</div>
					</div>
					<div class="row">
						<div class="row-cell">
							<h3>Mileage</h3>
							<%= Html.TextBoxFor(x => x.Mileage,new {title="Mileage" ,placeholder="Mileage!"}) %>
							
						</div>
						<div id="condition" class="row-cell">
							<h3>Condition</h3>
							<div class="con_btn" title="Poor Condition" id="poor"><img src="<%= Url.Content("~/images/poor.jpg")%> " /></div>
							<div class="con_btn" title="Fair Condition" id="fair"><img src="<%= Url.Content("~/images/fair.jpg")%> " /></div>
							<div class="con_btn" title="Great Condition" id="great"><img src="<%= Url.Content("~/images/great.jpg")%>" /></div>
							<input type="radio" name="condition" class="poor" value="poor" id="rbPoor" />
							<input type="radio" name="condition" class="fair" value="fair" id="rbFair" />
							<input type="radio" name="condition" class="great" value="great" id="rbGreat"/>
							<%=Html.HiddenFor(x=>x.Condition) %>
						</div>
					</div>
					<div id="get-value-btn" class="row">
						<img title="Get Your Trade-In Value!" onclick="javascript:TradeVehicleFormSubmit();" src="<%= Url.Content("~/images/get-trade-value-btn.jpg")%>"  />
					</div>
				</div>
				   <%Html.EndForm(); %>
			</div>
		</div>
	</div>
		<script src="<%=Url.Content("~/js/trade-in.js")%>" type="text/javascript"></script>
</body>


  <script type="text/javascript">

      function TradeVehicleFormSubmit() {
          $("#TradeVehicleForm").submit();
      }
      function trimString(text) {
          return text.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
      }
      function validateForm() {
            var flag = true;
            var errorCount = 0;
           
            if ($("#SelectedTrim").val() == null && trimString($("#Vin").val()) == "") {
                $("#Vin").val("");
                error("Please select a vehicle or vehicle identification number before continuing", "37px");
                return false;
            }

            else if ($("#SelectedTrim").val() == "0****Trim..." && trimString($("#Vin").val()) == "") {
            $("#Vin").val("");
                error("Please select a vehicle or vehicle identification number before continuing", "37px");
                return false;
            }

            else {
                var errorString = "Please correct all errors below : ";
                if (trimString($("#Mileage").val()) == "") {
                    errorString += "<li>Mileage info is required</li>";
                    flag = false;
                    errorCount++;
                } else {
                    var mileageInfo = trimString($("#Mileage").val());
                    var numbermileage = Number(mileageInfo.replace(/[^0-9\.]+/g, ""));
                    if (numbermileage > 1000000) {
                        errorString += "<li>Valid mileage info is required</li>";
                        flag = false;
                        errorCount++;
                    }
                }

                if ($("#rbPoor").is(':checked') == false && $("#rbFair").is(':checked') == false && $("#rbGreat").is(':checked') == false) {
                    errorString += "<li>Condition info is required</li>";
                    flag = false;
                    errorCount++;
                } else {
                    if($("#rbPoor").is(':checked'))
                        $("#Condition").val("Poor");
                    else if ($("#rbFair").is(':checked'))
                        $("#Condition").val("Fair");
                    else if ($("#rbGreat").is(':checked'))
                        $("#Condition").val("Great");
  
                }
            }
          if (flag == false) {
              if (errorCount == 1)
                  error(errorString, "60px");
              else if (errorCount == 2)
                  error(errorString, "90px");
              else
                  error(errorString, "120px");
              return false;
          }
          return true;
          
      }

      $(document).ready(function() {
          var Make = $("#SelectedMake");
          var Year = $("#SelectedYear");
          var Model = $("#SelectedModel");
          var Trim = $("#SelectedTrim");

          
          
          if ($("#ValidVin").val() == "False")
              error("Invalid vin or no result matched. Please try again", "37px");

          $("#Mileage").numeric({ decimal: false, negative: false }, function() { alert("Positive integers only"); this.value = ""; this.focus(); });


          Year.change(function() {


          if (Year.val() != "Year...") {
                  $('#decode select').each(function() {
                      //console.log($(this).attr("id"));
                      $(this).prop('disabled', true);
                  });

                  Make.html("");
                  Model.html("");
                  Trim.html("");
                  $.post('<%= Url.Content("~/TradeIn/YearAjaxPost") %>', { YearId: Year.val() }, function(data) {

                      var items = "<option value='" + 0 + "****" + "Make..." + "'>" + "Make..." + "</option>";

                      $.each(data, function(i, data) {
                          items += "<option value='" + data.Id + "'>" + data.Value + "</option>";
                      });
                      Make.html(items);
                      Year.removeAttr('disabled');
                      Make.removeAttr('disabled');
                      Model.removeAttr('disabled');
                      Trim.removeAttr('disabled');

                  });
              }
          });


          Make.change(function() {

          if (Make.val() != "0****Make...") {
                  $('#decode select').each(function() {
                      //console.log($(this).attr("id"));
                      $(this).prop('disabled', true);
                  });
                  Model.html("");
                  Trim.html("");


                  $.post('<%= Url.Content("~/TradeIn/MakeAjaxPost") %>', { YearId: Year.val(), MakeId: Make.val() }, function(data) {

                      var items = "<option value='" + 0 + "****" + "Model..." + "'>" + "Model..." + "</option>";
                      $.each(data, function(i, data) {
                          items += "<option value='" + data.Id + "'>" + data.Value + "</option>";
                      });
                      Model.html(items);
                      Year.removeAttr('disabled');
                      Make.removeAttr('disabled');
                      Model.removeAttr('disabled');
                      Trim.removeAttr('disabled');

                  });
              }
          });

          Model.change(function() {


          if (Model.val() != "0****Model...") {
                  $('#decode select').each(function() {
                      //console.log($(this).attr("id"));
                      $(this).prop('disabled', true);
                  });
                  Trim.html("");

                  $.post('<%= Url.Content("~/TradeIn/ModelAjaxPost") %>', { YearId: Year.val(), ModelId: Model.val() }, function(data) {

                      var items = "<option value='" + 0 + "****" + "Trim..." + "'>" + "Trim..." + "</option>";
                      $.each(data, function(i, data) {
                          items += "<option value='" + data.VehicleId + "'>" + data.DisplayName + "</option>";
                      });
                      Trim.html(items);
                      Year.removeAttr('disabled');
                      Make.removeAttr('disabled');
                      Model.removeAttr('disabled');
                      Trim.removeAttr('disabled');

                  });
              }
          });



      });

   

  </script>
</html>