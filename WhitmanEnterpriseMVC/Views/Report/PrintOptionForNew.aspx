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
     
       if($("#pdfChoice").attr('checked'))
       {
            window.location = '<%= Url.Action("ViewNewKPIReport", "Market", new { reportType = WhitmanEnterpriseMVC.HelperClass.ReportType.Pdf }) %>';
        }
        else{
            window.location = '<%= Url.Action("ViewNewKPIReport", "Market", new { reportType = WhitmanEnterpriseMVC.HelperClass.ReportType.Excel }) %>';
        
        }    
        });
    });
    </script>

    <div style="text-align:center">
        <div style=" font-weight:bold; margin-bottom:10px;"> Please select document type to print: </div>
        <input id="pdfChoice" type="radio" name="choice" />
        <span style=" margin-right:30px;">
        Pdf
        </span>
        <input id="excelChoice" type="radio" name="choice" checked="checked" />
        Excel
        <div style=" margin-top:5px; text-align:right;">         
            <input type="button" name="KPT" value="Print" id="btnKPIPrint" /></div>
    </div>
</body>
</html>
