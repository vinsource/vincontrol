<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.ContentViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GraphInfo</title>
    <style type="text/css">
        body
        {
            font-family: 'Trebuchet MS' , Arial, sans-serif !important;
        }
        #result
        {
            font-family: "Trebuchet MS" , Helvetica, sans-serif;
        }
        .hidden
        {
            display: none;
        }
        #rangeNav span
        {
            display: inline-block;
            width: 50px;
            text-align: center;
            padding: .3em .7em .3em .7em;
            background: #222222;
            color: white;
        }
        #rangeNav span.selected
        {
            font-weight: bold;
            background-color: #C80000;
        }
        #graphWrap
        {
            width: 300px;
            height: 143px;
            display: inline-block;
            float: left;
        }
        #carInfo
        {
            display: inline-block;
            padding: 10px;
            background-color: #c80000;
            color: #eeeeee;
            width: 200px;
            font-size: .7em;
        }
        #filter-wrap
        {
            background: #c80000;
            padding: .5em;
            color: white;
            width: 98%;
        }
        input[type="radio"]
        {
            background: transparent;
        }
        /*######################################################################*//*NEW CSS ##############################################################*//*######################################################################*/table#vehicle-list
        {
            font-size: .7em;
        }
        #vehicle-list td
        {
            padding: .3em .7em .3em .7em;
            border-bottom: 1px #bbbbbb solid;
        }
        #printable-list
        {
            display: block;
        }
        /*
        #vehicle-list tr:nth-child(1) td
        {
            font-weight: bold;
            color: black;
            border-bottom: #C80000 4px solid;
        }*/#vehicle-list tr.highlight td
        {
            background: green;
            color: #fff;
        }
        #tblVehicles
        {
            align: center;
        }
        /*######################################################################*//*NEW CSS ##############################################################*//*######################################################################*/</style>
</head>
<body>
    <div class="graph-title-bar" style="margin-bottom: 30px;">
        <h1 style="font-size: 30px; text-align: center;">
            List of Charted Vehicles
        </h1>
        <%--  <h2 style="text-align: center;">
            <% = Model.DealshipName %>
        </h2>--%>
    </div>
    <div>
        <%= Model.Text.Replace("id=\"tblVehicles\"", "id=\"tblVehicles\" align=\"center\"")%>
    </div>

    <%--<script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function(e) {
            $("#tblVehicles").attr("align", "center");
        });
    </script>--%>

</body>
</html>
