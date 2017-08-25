<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="background: #003399 !important;">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mark As Sold</title>
    <link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            background: #003399;
        }

        #container
        {
            background: white;
            padding: 1em;
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
            color: black;
            text-align: center;
        }

        input[type="submit"]
        {
            background: #3366cc;
            border: 0;
            color: white;
            font-size: 0.8em;
            font-weight: bold;
            padding: .5em;
            margin-top: -2em;
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
            background: #003399 !important;
            text-align: center;
        }

        .short
        {
            width: 50px !important;
        }
    </style>
    
</head>
<body style="background: #003399 !important;">

    <h2 style="color: white!important; text-align: center">Previous Sold Vehicle Alert</h2>
    <div id="container" style="text-align: center">
        <div id="space" class="label" style="text-align: center">
            This
            <%= ViewData["Year"]%>
            <%=ViewData["Make"]%>
            <%=ViewData["Model"]%>
            <%=ViewData["Trim"]%>
            was sold on
            <%=ViewData["DateInStock"]%>
        </div>
        <div class="label" style="text-align: center">
            What would you like to do?
        </div>
        <br />
        <%--<% Html.BeginForm("Action", "Inventory", FormMethod.Post); %>--%>
        <input type="hidden" id="ListingId" name="ListingId" value="<%= ViewData["ListingId"] %>" />
        <input type="hidden" id="Vin" name="Vin" value="<%= ViewData["Vin"] %>" />
        <input type="submit" name="MarkUnSoldFromVinDecode" value="Mark UnSold" onclick="MarkUnSoldFromVinDecodeNew();" />
        <input type="submit" name="NewAppraisal" value="New Appraisal" onclick="NewAppraisalFromSoldAlert();" />
        <%--<% Html.EndForm(); %>--%>
        <!--<input type="button" name="Cancel" onClick="parent.$.fancybox.close()" value="Cancel"  />-->
    </div>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function MarkUnSoldFromVinDecodeNew() {
            var listingId = $('#ListingId').val();
            parent.$.fancybox.close();
            window.parent.MarkUnSoldFromVinDecodeNew(listingId);
        }
        function NewAppraisalFromSoldAlert() {
            var vin = $('#Vin').val();
            parent.$.fancybox.close();
            window.parent.NewAppraisalFromSoldAlert(vin);
        }
    </script>
</body>
</html>
