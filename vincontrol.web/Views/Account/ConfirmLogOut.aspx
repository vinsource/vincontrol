<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mark As Sold</title>
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
        input:hover
        {
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
            background-color: #2a558c;
            float: right;
        }
        input[type="button"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 0.8em;
            font-weight: bold;
            padding: .5em;
            cursor: pointer;
            
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
            $('#btnContinue').click(function () {
                window.parent.SubmitLogOut();
            });
        });
</script>
    <h2 class="invalid_vin_title">
        Logout Confirm</h2>
    <div id="container">
        <div id="space" class="label" style="width: 100%;height: 30px;">
            Are you sure to logout this page?
        </div>
        <div style="height: 40px;">
            <div style="float: left; width: 47%;">
                <input type="button" name="BlankAppraisal" value="Okay" id="btnContinue" />
            </div>
            <div style="float: left; margin-left: 10px;">
                <input type="button" name="BlankAppraisal" value="Cancel" id="btnCancel" onclick="parent.$.fancybox.close()" />
            </div>
        </div>
    </div>
</body>
</html>
