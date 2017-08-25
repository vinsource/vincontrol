<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>View or create new appraisal</title>
    <link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <!-- the jScrollPane script -->
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            color: white;
        }
        #container
        {
            background: white;
            
            color: #333;
            text-align: center;
        }
        
        
        h3, ul
        {
            margin: 0;
        }
        input[type="text"]
        {
            width: 200px;
        }
        span.label
        {
            display: block;
            width: 150px;
            float: left;
            clear: right;
        }
        #btnContinue:hover {
            opacity: 0.8;
        }
        #btnContinue
        {
            cursor: pointer;
            border: 0;
            color: white;
            font-size: 0.8em;
            font-weight: bold;
            padding: .5em;
            clear: both;
            margin-top: 15px;
            background-color: #2a558c;
            float: right;
        }
        input[type="button"]
        {
            background: #3366cc;
            border: 0;
            color: white;
            font-size: 0.8em;
            font-weight: bold;
            padding: .5em;
            margin-top: -2em;
        }
        #fancybox-content
        {
            background: #3366cc !important;
            text-align: center;
        }
        .invalid_vin_title
        {
            margin: 10px;
            text-align: center;
            color: #800;
            border-bottom: 2px solid gray;
        }
        .short
        {
            width: 50px !important;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnCreateNew').click(function () {
                //document.forms[0].submit();
                // Set a flag to know if we need to reload profile page after closing the Chart                
                window.parent.document.getElementById('hdIsCreateNew').value = true;
                parent.$.fancybox.close();
            });
            
            $('#btnViewDetail').click(function () {
                //document.forms[0].submit();
                // Set a flag to know if we need to reload profile page after closing the Chart                
                window.parent.document.getElementById('hdIsCreateNew').value = false;
                parent.$.fancybox.close();
            });
        });
</script>
    <h2 class="invalid_vin_title">
        This vin is already existed</h2>
    <div id="container">
        <div id="space" class="label">
            This Vin
            <%= ViewData["Vin"]%>
            was appraised on <%=ViewData["AppraisalDate"] %>.
        </div>
        <div class="label" style="padding-bottom:9px; ">
            What would you like to do?</div>
        
        
        <input type="hidden" id="Vin" name="Vin" value="<%= ViewData["Vin"] %>" />
        
        <input type="button" name="BlankAppraisal" value="View Detail" id="btnViewDetail"/>
        <input type="button" name="BlankAppraisal" value="Create New" id="btnCreateNew"/>
        
        
    </div>
</body>
</html>
