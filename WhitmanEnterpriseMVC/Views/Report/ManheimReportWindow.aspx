<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<List<WhitmanEnterpriseMVC.Models.ManheimTransactionViewModel>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Manheim Report</title>
    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {

            // add parser through the tablesorter addParser method 
            $.tablesorter.addParser({
                // set a unique id 
                id: 'price',
                is: function(s) {
                    // return false so this parser is not auto detected 
                    return false;
                },
                format: function(s) {
                    // format your data for normalization 
                    return s.replace('$', '').replace(/,/g, '');
                },
                // set type, either numeric or text 
                type: 'numeric'
            });

            $("table#manheimTransaction").tablesorter({
                // prevent first column from being sortable
                headers: {
                    //0: { sorter: false },
                    1: { sorter: 'price' }, // miles
                    2: { sorter: 'price', sortInitialOrder: 'asc'}// prices
                }                                
            });

            $("#ddlRegion").change(function() {
                $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
                var year = $("#Year").val();
                var make = $("#Make").val();
                var model = $("#Model").val();
                var trim = $("#Trim").val();
                var region = $("#ddlRegion").val();
                $.ajax({
                    type: "POST",
                    url: "/Report/ManheimReportDetail?year=" + year + "&make=" + make + "&model=" + model + "&trim=" + trim + "&region=" + region,
                    data: {},
                    success: function(results) {
                        $("#result").html(results);
                         $.unblockUI();
                        //$("table#manheimTransaction").trigger("update");
                        $("table#manheimTransaction").tablesorter({                            
                            headers: {                             
                                1: { sorter: 'price' }, // miles
                                2: { sorter: 'price', sortInitialOrder: 'asc'}// prices
                            }                            
                        });
                    }
                });
            });
        });
    </script>
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            background: #222;
            color: #ddd;
        }
        body
        {
            width: 980px;
            margin: 0 auto;
        }
        #container
        {
            background: #333;
            padding: 1em;
        }
        h3, ul
        {
            margin: 0;
        }
        input[type="text"]
        {
            width: 30px;
        }
        span.label
        {
            display: block;
            width: 150px;
            float: left;
            clear: right;
        }
        input[type="submit"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 1.1em;
            font-weight: bold;
            padding: .5em;
            float: right;
            margin-top: -2em;
        }
        .short
        {
            width: 50px !important;
        }
        .submit
        {
            background: none repeat scroll 0 0 #860000;
            border: medium none #000000;
            color: #FFFFFF;
            cursor: pointer;
            display: inline-block;
            font-size: 14px;
            font-weight: normal;
            padding: 2px 8px;
            width: 100px;
            text-align: center;
            padding: 4px 2px;
        }
    </style>
</head>

<body>
    <div id="container">
        <form id="manheimReportForm" method="post" action="">
        <input type="hidden" id="Year" name="Year" value="<%= (string)ViewData["ManheimYear"] %>" />
        <input type="hidden" id="Make" name="Make" value="<%= (string)ViewData["ManheimMake"] %>" />
        <input type="hidden" id="Model" name="Model" value="<%= (string)ViewData["ManheimModel"] %>" />
        <input type="hidden" id="Trim" name="Trim" value="<%= (string)ViewData["ManheimTrim"] %>" />
        <br />        
        <div>
        <strong class="reportHeader">REPORTED WHOLESALE AUCTION SALES - With Exact Matches</strong>
        <div style="margin-right: 5px; float: right;">
            <select id="ddlRegion" name="ddlRegion">
                <option value="NA">National</option>
                <option value="SE">South East</option>
                <option value="NE">North East</option>
                <option value="MW">Mid West</option>
                <option value="SW">South West</option>
                <option value="WC">West Coast</option>
            </select>
        </div>
        </div>
        <hr />       
        </form>
        
        <div id="result"><%= Html.Partial("ManheimReportDetail", Model) %></div>
    </div> 

</body>
</html>