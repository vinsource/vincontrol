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
				<div id="step-1" class="step"><img src="<%= Url.Content("~/images/on-step-1.png")%>" alt="step 1"/></div>
				<div id="step-2" class="step"><img src="<%= Url.Content("~/images/step-2.png")%>" alt="step 2"/></div>
				<div id="step-3" class="step"><img src="<%= Url.Content("~/images/step-3.png")%>" alt="step 3"/></div>
			</div>
		</div>
		
		<div class="slide-wrapper">
			<div class="info-wrap">
				<h3 class="description-header">Select Your Vehicle's Trim</h3>
				<div id="trims" class="info-box">
					<ul>
					       <% foreach (var tmp in Model.SpecificKBBTrimList)
                       {
                            
  %>
                      <a href="<%=Url.Content("~/TradeIn/TradeInByTrim?VId=") %><%=tmp.VIN %>&Mileage=<%=tmp.Mileage %>&Condition=<%=Model.Condition %>"><%= tmp.Year.Value%> <%= tmp.Make.Value%> <%= tmp.Model.Value%> <%= tmp.Trim.Value%></a><br/>
                        
  <%
                       } %>
				    
					
					</ul>
				</div>
			</div>
		</div>
		<div class="controls">
		
			<a href="javascript: history.go(-1)" class="prev">< Previous Step</a>
		</div>
	</div>
	<script src="<%=Url.Content("~/js/trade-in.js")%>" type="text/javascript"></script>
</body>
</html>