<%@ Page Title="Trade In" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.TradeInVehicleModel>" %>

<!DOCTYPE html>
<html>
<head>
	<title>Trade-In Value</title>
	<link href="<%=Url.Content("~/Css/TradeIn/style.css")%>" rel="stylesheet" type="text/css" />
	<%--<script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>--%>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
</head>
<body>
	
	
<div id="container">

		<div id="header">

			<div class="logo"></div>

			<div class="mask">
				<div class="text-wrap">
					<h1>Get Your Trade In Value!</h1>
				</div>
			</div>
			<div class="steps">
			    <div id="step-1"  class="step"><img src="<%= Url.Content("~/images/step-1.png")%>" alt="step 1"/></div>
				<div id="step-2" class="step"><img src="<%= Url.Content("~/images/on-step-2.png")%>" alt="step 2"/></div>
				<div id="step-3"class="step"><img src="<%= Url.Content("~/images/step-3.png")%>" alt="step 3"/></div>
			
			</div>
		</div>
		
		<div class="slide-wrapper">
 <% Html.BeginForm("TradeInCustomerByKarPower", "TradeIn", FormMethod.Post, new { id = "TradeInOptionsForm", onsubmit = "return OptionsSelected()" }); %>
				<div class="info-wrap">
				<h3 class="description-header">Select Your Vehicle's Options</h3>
				
					<p style="font-size: 1.8em; animation-direction: normal;margin-left: 200px;color: green; "><%=Model.SelectedYear %> <%=Model.SelectedMakeValue %> <%=Model.SelectedModelValue %> <%=Model.SelectedTrimValue %></p>
					
					
					<div id="options" class="info-box">
					<ul>
				
				    <% foreach (var tmp in Model.OptionalEquipment)
                       {
  %>
                        <li><input id="<%=tmp.VehicleOptionId%>" name="Options" value="<%=tmp.VehicleOptionId%>" type="checkbox" <%= tmp.IsSelected ? "checked" : "" %> /><%=tmp.DisplayName%></li>
                      
  <%
                       } %>
				    
				
               	</ul>
				</div>
				<%=Html.HiddenFor(x=>x.SelectedOptions) %>
                <%=Html.HiddenFor(x=>x.EncryptVehicleId) %>
                <%=Html.HiddenFor(x=>x.VehicleId) %>
                <%=Html.HiddenFor(x=>x.Vin) %>
                <%=Html.HiddenFor(x=>x.SelectedYear) %>
                <%=Html.HiddenFor(x=>x.SelectedMake) %>
                <%=Html.HiddenFor(x => x.SelectedMakeValue)%>
                <%=Html.HiddenFor(x=>x.SelectedModel) %>
                <%=Html.HiddenFor(x => x.SelectedModelValue)%>
                <%=Html.HiddenFor(x=>x.SelectedTrim) %>
                <%=Html.HiddenFor(x => x.SelectedTrimValue)%>
                <%=Html.HiddenFor(x=>x.Mileage) %>
                <%=Html.HiddenFor(x=>x.Condition) %>
			
	        </div>
		</div>
				<div class="controls">
				     <a class="next" onclick="javascript:TradeInOptionsFormSubmit();">Next Step ></a>
				     	<a href="javascript: history.go(-1)" class="prev">< Previous Step</a>
					<%--<a href="/trade-in/" class="prev">< Previous Step</a>--%>
				</div>
			</div>
			<%Html.EndForm(); %>
	
	<script src="<%=Url.Content("~/js/trade-in.js")%>" type="text/javascript"></script>
</body>
<script type="text/javascript">
    function TradeInOptionsFormSubmit() {
        $("#TradeInOptionsForm").submit();
    }
    function OptionsSelected() {

        var checks = $('input[type="checkbox"]');
        var itemoption = "";

        for (i in checks) {
            if (checks[i].checked && checks[i].name == "Options") {
                itemoption += checks[i].id + ",";
            }

        }

        if (itemoption.indexOf(",") != -1) {
            itemoption = itemoption.substring(0, itemoption.length - 1);
        }

        $("#SelectedOptions").val(itemoption);

    }
    </script>


</html>