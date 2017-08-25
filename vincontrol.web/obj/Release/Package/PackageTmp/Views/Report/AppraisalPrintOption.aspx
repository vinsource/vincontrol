<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PrintOption</title>
</head>
<body>

    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>

    <script type='text/javascript'>
        $(document).ready(function() {
            $("#btnKPIPrint").click(function() {

                if ($("#30DaysChoice").attr('checked')) {
                    window.location = '<%= Url.Action("PrintAppraisalCarInfo", "PDF", new { NumberOfDay = 30 }) %>';
                }
                else if ($("#60DaysChoice").attr('checked')) {
                    window.location = '<%= Url.Action("PrintAppraisalCarInfo", "PDF", new { NumberOfDay = 60 }) %>';
                }
                else if ($("#90DaysChoice").attr('checked')) {
                    window.location = '<%= Url.Action("PrintAppraisalCarInfo", "PDF", new { NumberOfDay = 90 }) %>';
                }
            });
        });
    </script>

    <div style="text-align: center; background-color: blue; color: whitesmoke;font-size: 14px;font-family: Arial">
        <div style="font-weight: bold; margin-bottom: 10px;">
            Print appraisal during :
        </div>
        <input id="30DaysChoice" type="radio" name="VINDaysChoice" />
        Last 30 days
        <input id="60DaysChoice" type="radio" name="VINDaysChoice" checked="checked" />
        Last 60 days
        <input id="90DaysChoice" type="radio" name="VINDaysChoice" />
        Last 90 days
        <div style="margin-top: 5px; text-align: right;">
            <input type="button" name="KPT" value="Print" id="btnKPIPrint" /></div>
    </div>
</body>
</html>
