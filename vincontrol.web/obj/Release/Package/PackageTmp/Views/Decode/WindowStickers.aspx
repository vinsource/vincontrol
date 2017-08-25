<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Window Sticker</title>
<link rel="stylesheet" type="text/css" href="style.css" />
<script language="javascript" type="text/javascript">
//window.print()
</script>
</head>

<body>

<div id="container" class="clear">
	<img src="background.jpg" />
    <div id="header" class="clear">
        <div id="logo" class="column">
        
        	<p>Logo/Dealership</p>
        
        </div>
        
        <div id="carInfoTop" class="column">
        	<p>Car Info</p>
        </div>
    
    </div>

	<div id="main" class="clear">
        <div id="c1" class="column">
        	
            <div id="equipment">
            	<p>Equipment</p>
        	</div>
            <div id="mpg">
           		<p>Fuel Economy/MPG</p>
        	</div>
            <div id="price1">
            	<p>Sale Price</p>
        	</div>
            
        </div>
        <div id="c2" class="column">
        <div id="pInfo">
            	<p>Price Information</p>
        	</div>
            <div id="MSRP">
            	<p>Total MSRP<br />
                   Sale Price</p>
        	</div>
            <div id="price2">
            	<p>Barcode</p>
        	</div>
        </div>
        
    </div>

</div>

</body>
</html>
