<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Window Sticker</title>
<script src="http://code.jquery.com/jquery-latest.js"></script>
<link href="<%=Url.Content("~/Sticker/style.css")%>" rel="stylesheet" type="text/css" />
</head>

<body>
<img src=<%=Url.Content("~/sticker/background.jpg")%> />
<div id="container" class="clear">
	
    <div id="header" class="clear">
        <div id="logo" class="column">
        	<p>  <%=Html.DynamicHtmlLabelForSticker("Logo")%></p>
        </div>
        
        <div id="carInfoTop" class="column">
        	<h1><%=Html.DynamicHtmlLabelForSticker("CarName") %></h1>
            <p>
            <span>
            <%=Html.DynamicHtmlLabelForSticker("Tranmission") %><br />
              <%=Html.DynamicHtmlLabelForSticker("ExteriorColor")%><br />
              </span>
            <span>
               <%=Html.DynamicHtmlLabelForSticker("InteriorColor")%><br />
               </span>
            <span>
           <%=Html.DynamicHtmlLabelForSticker("Stock")%><br />
              <%=Html.DynamicHtmlLabelForSticker("Vin")%><br /></span></p>
           <div class="clear"></div>
        </div>
    
    </div>

	<div id="main" class="clear">
        <div id="c1" class="column">
        	<div class="clear"></div>
            <div id="equipment" style="font-size:.9em; text-transform:uppercase; line-height: 1em; padding-top: .9em; padding-left:1.5em;">
            	<span style="font-weight:bold; font-size: 1.2em;">Equipment</span><br /><br />
                <ul id="Ul1"><li class=" coltopper"><span class="listHead">Mechanical</span></li>
               
                    <%=Html.DynamicHtmlLabelForSticker("MECHANICALLIST")%>
                    <li class="break"></li>
                    <li class=""><span class="listHead">Exterior</span></li>
                	<%=Html.DynamicHtmlLabelForSticker("EXTERIORLIST")%>
                    <li class="break"></li>
                    <li class=""><span class="listHead">Entertainment</span></li>
                	<%=Html.DynamicHtmlLabelForSticker("ENTERTAINMENTLIST")%>
                 <li class="break"></li>
                    <li class=""><span class="listHead">Interior</span></li>
                	<%=Html.DynamicHtmlLabelForSticker("INTERIORLIST")%>
                  <li class="break"></li>
                    <li class=""><span class="listHead">Safety</span></li>
                   <%=Html.DynamicHtmlLabelForSticker("SAFETYLIST")%>
            
       <div class="clear"></div>
        	</div>
            <div id="mpg" style="padding-top: .3em;">
           		<h3 style="text-align: center;">Fuel Economy/MPG</h3>
                    <div id="city" style="position: relative; left: 200px;">
                        City MPG
                        <h1 style="font-size: 5em; margin: 0; margin-left: -20px; padding: 0;"><%=Html.DynamicHtmlLabelForSticker("FuelEconomyCity")%></h1>
                     </div>
                    <div id="highway" style="position: relative; top: -120px; left: 900px;">
                        Highway MPG
                        <h1 style="font-size: 5em; margin: 0; padding: 0;"><%=Html.DynamicHtmlLabelForSticker("FuelEconomyHighWay")%></h1>
                    </div>
                
               
                <div class="clear"></div>
                <span style="position: absolute; top: 130px; left: 310px;">Actual Mileage will vary with options, driving conditions, driving habits and vehicle condition.</span>
        	</div>
            <div id="price1">
                <h1 style="font-size: 3.5em; text-align:center;">Sale Price <span id="salePrice1"><%=Html.DynamicHtmlLabelForSticker("SalePrice")%></span></h1>
        	</div>
            
        </div>
        <div id="c2" class="column">
        <div id="pInfo" style=" text-transform: uppercase; line-height: 1em;">
            	<h3>Price Information</h3>
                <ul style=" margin-top: -15px; font-size: .9em; width: 230px !important; overflow: hidden; height: 585px;">					<li class="break"></li>
                	<li class="listHead">Packages</li>
                	<%=Html.DynamicHtmlLabelForSticker("FactoryPackageOption")%>
                    <li class="break"></li>
                    <li class="listHead">Options</li>
                    <%=Html.DynamicHtmlLabelForSticker("FactoryOption")%>
                </ul>
        	</div>
          <div id="MSRP" style="padding-left: 1.5em; padding-top:26px; text-align:center;">
            	<h3 style="margin: 0; padding: 0;">Original MSRP</h3>
                <h1 style="margin: -2px; padding: 0;"><%=Html.DynamicHtmlLabelForSticker("MSRP")%></h1>
        	</div>
            <div id="price2" style="padding-left: 2em; padding-top: .9em; overflow: hidden; height:55px;">
            	 <%=Html.DynamicHtmlLabelForSticker("BarCode")%>
        	</div>
        </div>
        
    </div>

</div>
<script language="javascript" type="text/javascript">
    //window.print();
    var length = 0;
    var colNum = 0;
    //console.log(length);
    $('#equipment li').each(function(index) {
        if (length == (32) * colNum) {
            colNum++;
            $(this).addClass('coltopper');
        }
        $(this).addClass('column' + colNum);

        //console.log(length);
        console.log(index);
        if ($(this).hasClass('coltopper')) {
            if (length != 0) {
                $(this).css('margin-top', '-' + (32) + 'em');
            }
            //console.log('-'+(length-2)+'em');
        }
        length++;
    });
</script>
</body>
</html>
