<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.BuyerGuideViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Buyer's Guide</title>
<script src="http://code.jquery.com/jquery-latest.js"></script>
<style type="text/css">
html {font-family:"Trebuchet MS", Arial, Helvetica, sans-serif; background: #222; color: #ddd;}
body {width: 500px; margin: 0 auto;}
#container {background: #333;  padding: 1em;}
h3, ul {margin: 0;}
input[type="text"] {width: 30px;}
span.label {display: block; width: 150px; float: left; clear: right;}
input[type="submit"] {background: #680000; border: 0; color: white; font-size: 1.1em; font-weight: bold; padding: .5em; float: right; margin-top: -2em;}
.short {width: 50px !important;}
</style>
</head>

<body>
<h1 style="margin-bottom: 0;">Buyer's Guide</h1>
<div id="container">
 <% Html.BeginForm("ViewBuyerGuideinPdf", "Report", FormMethod.Post); %>
<input type="hidden" name="warrantyInfo"  />
<h3>Select a language to print</h3>

	 <%=Html.DropDownListFor(x=>x.SelectedLanguage,Model.Languages) %>
     <%=Html.HiddenFor(x=>x.ListingId) %>
     <input type="submit" name="submit" value="Print Guide"  />
 <% Html.EndForm(); %>
    <img alt="" src="../images/bGuideBG.png"  width="200" />
    <img alt="" src="../images/bGuideBG-spanish-resize.png" width="200"/>
    </div>

  

</body>
</html>