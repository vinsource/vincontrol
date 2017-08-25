<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CarFax Report</title>
    
</head>
<body>
    <input type="hidden" id="carfaxreport" name="carfaxreport" value="<%= (String)ViewData["CarFaxReport"] %>" />
    <div>
    <% if ((String)ViewData["CarFaxReport"] == String.Empty) {%> 
    No CARFAX report.
    <%}%>
    </div>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#carfaxreport').val() != '') window.location.href = $('#carfaxreport').val();
        });
    </script>
</body>
</html>
