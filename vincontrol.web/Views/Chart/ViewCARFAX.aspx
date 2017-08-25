<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.VINViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ViewCARFAX</title>
    <style type="text/css">
        #CARFAXresult #carfax .number, #CARFAXresult #carfax .text
        {
            font-size: .95em !important;
            display: inline-block;
            overflow: hidden;
            padding: .1em .4em .1em .4em;
            height: 100%;
            border: 4px #111 solid;
        }

        #CARFAXresult #carfax .oneowner, #CARFAXresult #carfax .text
        {
            font-size: .95em !important;
            display: inline-block;
            overflow: hidden;
            padding: .1em .4em .1em .4em;
            height: 100%;
            border: 4px #111 solid;
        }

        #CARFAXresult #carfax .text
        {
            margin-left: 0;
            background: #111;
        }

        #CARFAXresult #carfax .number
        {
            margin-right: 0;
            background: #c80000;
            overflow: hidden;
            color: white;
            font-weight: bold;
        }

        #CARFAXresult #carfax .oneowner
        {
            margin-right: 0;
            background: green;
            overflow: hidden;
            color: white;
            font-weight: bold;
        }

        #CARFAXresult #carfax .carfax-wrapper
        {
            overflow: hidden;
            margin-top: 10px;
            display: inline;
            width: 225px;
        }

        #carfax-header, #report-wrapper, #summary-wrapper
        {
            display: block;
        }

        #carfax-header
        {
            position: relative;
            top: -8px;
        }

        #summary-wrapper
        {
            position: absolute;
            right: 0;
            top: 0;
        }

        #carfax-header img
        {
            margin-right: 5px;
        }

        #report-wrapper img
        {
            float: right;
            margin-right: 10px;
            margin-top: 3px;
        }

        #carfax-header h3
        {
            margin-top: 3px;
        }

        #report-wrapper ul, #report-wrapper li
        {
            margin: 0;
            padding: 0;
        }

        #report-wrapper ul
        {
            margin-top: 5px;
        }

        #report-wrapper li
        {
            width: 100%;
            background: #111;
            margin-top: 4px;
            padding: .2em .5em .2em .5em;
        }

        #CARFAXresult #carfax
        {
            position: relative;
            margin-bottom: 8px !important;
        }

        #history-report ul li
        {
            color: white;
        }

        #owners .text
        {
            color: white;
        }

        #reports .text
        {
            color: white;
        }
    </style>
</head>
<body>
    <div style="margin-bottom: 20px;">
        VIN:
        <input id="CARFAXVin" type="text" value="<%=Model.VINNumber %>" />
        <input type="button" id="btnCARFAXSearch" value="Search" />
    </div>
    <div id="CARFAXresult">
    </div>
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function (e) {

            $("#btnCARFAXSearch").click(function () {
                
                var vin = $("#CARFAXVin").val();
                $.ajax({
                    type: "GET",
                    url: "/Chart/CARFAXDetail?vin=" + vin,
                    data: {},
                    success: function (results) {
                        $("#CARFAXresult").html(results);
                        //                         $.unblockUI();
                    }
                });
            });

            if ($("#CARFAXVin").val() != '') {
                $("#btnCARFAXSearch").click();
            }
        });
    </script>
</body>
</html>
