<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PrintOption</title>
</head>
<body>

    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>

    <script type='text/javascript'>
        $(document).ready(function() {
            $("#btnKPIPrint").click(function() {

                if ($("#weekChoice").attr('checked')) {
                    window.location = '<%= Url.Action("PrintTradeInCustomer", "PDF", new { period = "week" }) %>';

                }
                else {
                    window.location = '<%= Url.Action("PrintTradeInCustomer", "PDF", new { period = "month" }) %>';

                }

            });
        });
    </script>

    <div style="text-align: center">
        <div style="font-weight: bold; margin-bottom: 10px;">
            Please select document type to print:
        </div>
        <input id="weekChoice" type="radio" name="choice" />
        <span style="margin-right: 30px;">This Week Report </span>
        <input id="monthChoice" type="radio" name="choice" checked="checked" />
        This Month Report
        <div style="margin-top: 5px; text-align: right;">
            <input type="button" name="KPT" value="Print" id="btnKPIPrint" /></div>
    </div>
</body>
</html>
