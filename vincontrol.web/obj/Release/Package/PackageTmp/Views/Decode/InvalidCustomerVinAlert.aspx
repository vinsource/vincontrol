
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"  style="background: #111 !important;">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Mark As Sold</title>
<link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />

<script src="http://code.jquery.com/jquery-latest.js"></script>
<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<!-- the jScrollPane script -->
<style type="text/css">
html {font-family:"Trebuchet MS", Arial, Helvetica, sans-serif; background: #222; color: #ddd;}
#container {background: #333;  padding: 1em;}
h3, ul {margin: 0;}
input[type="text"] {width: 200px;}
span.label {display: block; width: 150px; float: left; clear: right;}
input[type="submit"] {background: #680000; border: 0; color: white; font-size: 0.8em; font-weight: bold; padding: .5em;  margin-top: -2em;}
input[type="button"] {background: #680000; border: 0; color: white; font-size: 0.8em; font-weight: bold; padding: .5em; margin-top: -2em;}
#fancybox-content {background: #111 !important;text-align:center}
.short {width: 50px !important;}
</style>
</head>
<body style="background: #111 !important;">
<script>

</script>

<h2>Invalid Vin Alert</h2>
<div id="container">
	
<div id="space" class="label">This Vin <%= ViewData["Vin"]%> is not valid or unavailable </div> 

</body>
</html>