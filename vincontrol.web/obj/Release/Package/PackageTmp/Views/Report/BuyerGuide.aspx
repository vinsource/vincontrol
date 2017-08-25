<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.BuyerGuideViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Buyer's Guide</title>
<style type="text/css">
html {font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;}
div {padding-right: 0px;}
#wrapper {position: relative; width: 1000px; margin: 0 auto; padding: 0;}
#wrapperSpanish {position: relative; width: 1000px; margin: 0 auto; padding: 0;}
.column {float: left; clear: right;}
.noSelect {display: none;}
span, p {position: absolute; top: 0; left: 0;}
#make {top: 160px; left:105px;}
#model {top: 160px; left: 295px;}
#year {top: 160px; left: 430px;}
#vin {top: 160px; left: 560px;}
#stockNumber {top: 235px; left: 105px;}
#asIs {top: 324px; left: 100px; font-size: 3.4em;}
#fullLimit {top: 464px; left: 100px; font-size: 3.4em;}
#full {top: 535px; left: 92px;}
#limited {top: 535px; left: 195px;}
#labor {top: 542px; left: 524px;}
#parts {top: 542px; left: 691px;}
#contract {top: 1100px; left: 87px; font-size: 1.2em;}
#systems { width: 800px; top: 633px; left: 100px; line-height:21px; font-size: .9em;}
#duration { width: 300px; top: 633px; left: 535px; line-height:21px; font-size: .9em;}


#makeSpanish{top: 160px; left:105px;}
#modelSpanish{top: 160px; left: 295px;}
#yearSpanish  {top: 160px; left: 430px;}
#vinSpanish {top: 160px; left: 560px;}
#stockNumberSpanish{top: 235px; left: 105px;}
#systemsSpanish  { width: 800px; top: 633px; left: 100px; line-height:21px; font-size: .9em;}
#asIsSpanish {top: 324px; left: 100px; font-size: 3.4em;}
#fullLimitSpanish{top: 464px; left: 100px; font-size: 3.4em;}
#fullSpanish{top: 535px; left: 92px;}
#limitedSpanish {top: 535px; left: 195px;}
#laborSpanish {top: 538px; left: 524px;}
#partsSpanish {top: 538px; left: 811px;}
#contractSpanish {top: 1100px; left: 87px; font-size: 1.2em;}


#makeSpanishBi{top: 160px; left:105px;}
#modelSpanishBi{top: 160px; left: 295px;}
#yearSpanishBi  {top: 160px; left: 430px;}
#vinSpanishBi {top: 160px; left: 560px;}
#stockNumberSpanishBi{top: 235px; left: 105px;}
#systemsSpanishBi  { width: 800px; top: 633px; left: 100px; line-height:21px; font-size: .9em;}
#asIsSpanishBi {top: 324px; left: 100px; font-size: 3.4em;}
#fullLimitSpanishBi{top: 464px; left: 100px; font-size: 3.4em;}
#fullSpanishBi{top: 535px; left: 92px;}
#limitedSpanishBi {top: 535px; left: 195px;}
#laborSpanishBi {top: 538px; left: 524px;}
#partsSpanishBi {top: 538px; left: 811px;}
#contractSpanishBi {top: 1100px; left: 87px; font-size: 1.2em;}
</style>
<style type='text/css' media='print'>

	
	#printLink {display : none}

</style>
</head>


<body  onload="window.print()">
      <input type="button" value="Print Buyer Guide" id="printLink" onclick="window.print()" />

    <%if(Model.SelectedLanguage==0)
  {%>
  <div id="wrapper">

        <%=Html.DynamicHtmlControlForBuyerGuide("BackGroundImage")%>
      
    	<span id="make"><%=Model.Make %></span>
        <span id="model"><%=Model.Model %></span>
        <span id="year"><%=Model.Year %></span>
        <span id="vin"><%=Model.Vin %></span>
        <span id="stockNumber"><%=Model.StockNumber %></span>
    	<%=Html.DynamicHtmlControlForBuyerGuide("Warranty")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("ServiceContract")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("WarrantyFullLimit")%>
        <%=Html.DynamicHtmlControlForBuyerGuide("WarrantyInfo")%>
</div>
<% } %>
<%if(Model.SelectedLanguage==1)
  {%>
<div id="wrapperSpanish">
        <%=Html.DynamicHtmlControlForBuyerGuide("BackGroundImageInSpanish")%>
        <span id="makeSpanish"><%=Model.Make %></span>
        <span id="modelSpanish"><%=Model.Model %></span>
        <span id="yearSpanish"><%=Model.Year %></span>
        <span id="vinSpanish"><%=Model.Vin %></span>
        <span id="stockNumberSpanish"><%=Model.StockNumber %></span>
    	<%=Html.DynamicHtmlControlForBuyerGuide("WarrantySpanish")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("ServiceContractSpanish")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("WarrantyFullLimitSpanish")%>
        <%=Html.DynamicHtmlControlForBuyerGuide("WarrantyInfoSpanish")%>
</div>
<% } %>



    <%if(Model.SelectedLanguage==2)
  {%>
    <div id="wrapper">
        <%=Html.DynamicHtmlControlForBuyerGuide("BackGroundImage")%>
      
    	<span id="make"><%=Model.Make %></span>
        <span id="model"><%=Model.Model %></span>
        <span id="year"><%=Model.Year %></span>
        <span id="vin"><%=Model.Vin %></span>
        <span id="stockNumber"><%=Model.StockNumber %></span>
    	<%=Html.DynamicHtmlControlForBuyerGuide("Warranty")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("ServiceContract")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("WarrantyFullLimit")%>
        <%=Html.DynamicHtmlControlForBuyerGuide("WarrantyInfo")%>
</div>
<div id="wrapperSpanish">
        <%=Html.DynamicHtmlControlForBuyerGuide("BackGroundImageInSpanish")%>
        <span id="makeSpanishBi"><%=Model.Make %></span>
        <span id="modelSpanishBi"><%=Model.Model %></span>
        <span id="yearSpanishBi"><%=Model.Year %></span>
        <span id="vinSpanishBi"><%=Model.Vin %></span>
        <span id="stockNumberSpanishBi"><%=Model.StockNumber %></span>
    	<%=Html.DynamicHtmlControlForBuyerGuide("WarrantySpanishBi")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("ServiceContractSpanishBi")%>
    	<%=Html.DynamicHtmlControlForBuyerGuide("WarrantyFullLimitSpanishBi")%>
        <%=Html.DynamicHtmlControlForBuyerGuide("WarrantyInfoSpanishBi")%>
</div>
<% } %>

</body>
</html>
