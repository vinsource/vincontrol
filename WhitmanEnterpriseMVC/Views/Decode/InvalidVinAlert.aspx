
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"  style="background: #111 !important;">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Mark As Sold</title>
<link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />

<script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
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
<script type="text/javascript">
    $(document).ready(function () {
        $('#btnContinue').click(function () {
            //document.forms[0].submit();
            // Set a flag to know if we need to reload profile page after closing the Chart                
            window.parent.document.getElementById('NeedToContinueWithBlankAppraisal').value = true;            
            parent.$.fancybox.close();            
        });

        $('#params').submit(function () {
            //parent.$.fancybox.close();                        
        });
    });
</script>

<h2>Invalid Vin Alert</h2>
    <div id="container">
        <div id="space" class="label">
            This Vin
            <%= ViewData["Vin"]%>
            is not valid or unavailable
        </div>
        <div class="label">
            What would you like to do?</div>
        <br />
        <%--<% Html.BeginForm("Action", "Decode", FormMethod.Post, new { id = "params" }); %>--%>
        <input type="hidden" id="Vin" name="Vin" value="<%= ViewData["Vin"] %>" /><br />
        <%--<input type="submit" name="BlankAppraisal" value="Continue Appraisal" />--%>
        <input type="button" name="BlankAppraisal" value="Continue Appraisal" id="btnContinue" />
        <%--<% Html.EndForm(); %>--%>
        <!--<input type="button" name="Cancel" onClick="parent.$.fancybox.close()" value="Cancel"  />-->
    </div>
</body>
</html>