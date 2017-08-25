<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WhitmanEnterpriseMVC.Models.BucketJumpHistory>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Bucket Jump Tracking</title>
    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
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
    <% if (Model != null && Model.Any()) {%>
    <table id="manheimTransaction" width="100%" class="reportText">
    <thead style="cursor: pointer;">
    <tr>
        <th align="left" width="15%">
            User Stamp
        </th>
        <th align="left" width="20%">
            Date Stamp
        </th>       
        <th align="right" width="35%">
            Price Before Bucket Jump
        </th>
        <th align="right" width="35%">
            Price After Bucket Jump
        </th>
        <th align="right" width="20%">
            Download
        </th>
    </tr>
    </thead>
    <tbody>
    <% foreach (var item in Model){%>
    <tr>
        <td align="left"><%= item.UserStamp %></td>
        <td align="left"><%= item.DateStamp.ToString("MM/dd/yyy hh:mm:ss") %></td>
        <td align="right"><%= item.SalePrice.Equals(0) ? "" : item.SalePrice.ToString("c0")%></td>
        <td align="right"><%= item.RetailPrice.Equals(0) ? "" : item.RetailPrice.ToString("c0")%></td>                
        <td align="right"><a href="<%= Url.Action("DownloadBucketJumpReport", "Inventory", new { name = item.AttachFile}) %>" id="aDownloadReport_<%= item.Id %>"><img src="../../images/pdf-icon.png" style="border:0" alt="Download Report" /></a></td>
    </tr>
    <%}%>
    
    </tbody>
    </table>
    <%} else {%>
    There is no record.
    <%}%>
    </div>
</body>
</html>
