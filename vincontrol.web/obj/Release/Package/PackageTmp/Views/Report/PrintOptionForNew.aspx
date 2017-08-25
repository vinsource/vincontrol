<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PrintOption</title>
</head>
<style>
    body {
        margin: 0px;
        padding: 0px;
    }
    .kpi_print_options {
    
}
.kpi_print_options > img 
{
    cursor: pointer;
    margin: 10px 20px;
}
.kpi_print_title {
    background-color: #3366cc;
    font-size: 21px;
    font-weight: bold;
    
    margin-bottom: 10px;
    padding: 5px 10px;
    text-align: left !important;
}
</style>
<body>

       <script src="<%=Url.Content("~/js/jquery-1.7.2.min.js")%>" type="text/javascript"></script>
       <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
      <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
         <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type='text/javascript'>
        $(document).ready(function () {
            $("#btnKPIPrint").click(function () {

                if ($("#pdfChoice").attr('checked')) {
                    $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                    var actionUrl = '<%= Url.Action("ViewKPIReport", "Market", new { reportType = Vincontrol.Web.HelperClass.ReportType.Pdf }) %>';
                    window.parent.PopupBuyerGuideWindow(actionUrl);


                } else {
                    window.location = '<%= Url.Action("ViewKPIReport", "Market", new { reportType = Vincontrol.Web.HelperClass.ReportType.Excel }) %>';
                }

            });
        });

        function ViewPDF() {
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            var actionUrl = '<%= Url.Action("ViewKPIReport", "Market", new { reportType = Vincontrol.Web.HelperClass.ReportType.Pdf }) %>';
            window.parent.PopupBuyerGuideWindow(actionUrl);
        }

        function ViewExcel() {
            window.location = '<%= Url.Action("ViewKPIReport", "Market", new { reportType = Vincontrol.Web.HelperClass.ReportType.Excel }) %>';
        }

        function PopupBuyerGuideWindow(actionUrl) {
            $("<a href=" + actionUrl + "></a>").fancybox({
                height: 915,
                width: 1000,
                overlayShow: true,
                showCloseButton: true,
                enableEscapeButton: true,
                type: 'iframe'
            }).click();
        }
    </script>

    <%--<div style="text-align:center">
        <div style=" font-weight:bold; margin-bottom:10px;"> Please select document type to print: </div>
        <input id="pdfChoice" type="radio" name="choice" checked="checked" />
        <span style=" margin-right:30px;">
        Pdf
        </span>
        <input id="excelChoice" type="radio" name="choice"  />
        Excel
        <div style=" margin-top:5px; text-align:right;">         
            <input type="button" name="KPT" value="Print" id="btnKPIPrint" /></div>
    </div>--%>
    <div style="text-align:center">
        <div class="kpi_print_title"> Select Export Format </div>
        <div class="kpi_print_options">
            <img alt="" src="/Content/images/print-pdf.png" onclick="ViewPDF();" />
            <img alt="" src="/Content/images/print-excel.png" onclick="ViewExcel();" />   
        </div>
    </div>
</body>
</html>
