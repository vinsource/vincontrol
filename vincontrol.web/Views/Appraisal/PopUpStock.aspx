<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Option</title>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
        }

        .kpi_print_options
        {
            margin-top: 20px;
        }

            .kpi_print_options > img
            {
                cursor: pointer;
                margin: 10px 20px;
            }

        .kpi_print_title
        {
            background-color: #3366cc;
            font-size: 21px;
            font-weight: bold;
            margin-bottom: 10px;
            padding: 5px 10px;
            text-align: left !important;
        }


        .app_popup_btns:hover
        {
            background-color: #444;
        }

        .app_popup_btns
        {
            background-color: #5C5C5C;
            border: medium none;
            color: #FFFFFF;
            cursor: pointer;
            height: 20px;
            margin-left: 5px;
            width: 76px;
        }

        .btns_shadow
        {
            box-shadow: 3px 2px 3px #333333;
        }

        #txtStock
        {
            width: 80px;
        }
    </style>
</head>

<body>
    
    <input type="hidden" id="hdAppraisalID" value="<%=ViewData["appraisalID"] %>" />
    <div style="text-align: center">
        <div class="kpi_print_title">Input Stock Number </div>
        <div class="kpi_print_options">
            Stock number:
            <input type="text" id="txtStock" />
            <input class="btns_shadow app_popup_btns" type="button" id="btnOk" value="Ok" onclick="SubmitStock();" />
            <input class="btns_shadow app_popup_btns" type="button" id="Button1" value="Skip" onclick="Skip();" />
            <input class="btns_shadow app_popup_btns" type="button" id="btnCancel" value="Cancel" onclick="Cancel();" />
        </div>
    </div>
    
    <script src="<%=Url.Content("~/js/jquery-1.7.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type='text/javascript'>
        function Skip() {
            parent.$.fancybox.close();
            window.parent.SubmitAddInventory($('#hdAppraisalID').val());
        }

        function SubmitStock() {
            $.ajax({
                type: 'POST',
                url: '<%=Url.Action("CheckStockNumberExist","Inventory") %>',
                data: { stock: $('#txtStock').val() },
                async: false,
                success: function (result) {
                    if (result.success == true) {
                        window.parent.UnCheckStatus('chk' + $('#hdAppraisalID').val());
                        parent.$.fancybox.close();
                        window.parent.SubmitStockNumber($('#txtStock').val(), $('#hdAppraisalID').val());
                    }
                    else {
                        alert('This stock already existed, please enter another stock.');
                    }
                }
            });
        }

        function Cancel() {
            window.parent.UnCheckStatus('chk' + $('#hdAppraisalID').val());
            parent.$.fancybox.close();
        }
    </script>
</body>
</html>
