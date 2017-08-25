<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.AutoLoanCalculatorModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mark As Sold</title>
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/VinControl/popup.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
</head>
<body>
    <% Html.BeginForm("BackAutoLoanCalculator", "Inventory", FormMethod.Post, new { id = "autoloanCalculatorSummaryForm", name = "autoloanCalculatorSummaryForm" }); %>
    <div class="kpi_print_title">
        Auto Loan Summary
    </div>
    <div class="fsqRow smHeight4">
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Montly Payment $
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.MonthlyPayment, new { @class = "fsqInpuText",@readonly="true", @style = "font-size: 16px" , Value=String.Format("{0:N}",Model.MonthlyPayment)})%>
                </div>
            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Down Payment $
            </div>
            <div class="sqrPayments">

                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.DownPayment, new { @class = "fsqInpuText",@readonly="true", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.DownPayment)})%>
                </div>

            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Principal $
            </div>
            <div class="sqrPayments">

                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.Principal, new { @class = "fsqInpuText",@readonly="true", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.Principal)})%>
                </div>

            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Total Interest $
            </div>
            <div class="sqrPayments">

                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.TotalInterest, new { @class = "fsqInpuText",@readonly="true", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.TotalInterest)})%>
                </div>

            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Total To Pay $
            </div>
            <div class="sqrPayments">

                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.TotalToPay, new { @class = "fsqInpuText",@readonly="true", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.TotalToPay)})%>
                </div>

            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Final Cost $
            </div>
            <div class="sqrPayments">

                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.FinalCost, new { @class = "fsqInpuText",@readonly="true", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.FinalCost)})%>
                </div>

            </div>
        </div>

    </div>
    <div class="status_btn_holder">
        <button style="cursor: pointer" type="submit" class="btns_shadow">Back</button>


    </div>
    <%=Html.HiddenFor(x=>x.VehiclePrice) %>
    <%=Html.HiddenFor(x=>x.InterestRate) %>
    <%=Html.HiddenFor(x=>x.SaleTax) %>
    <%=Html.HiddenFor(x=>x.Terms) %>
    <%=Html.HiddenFor(x=>x.TradeInValue) %>
    <% Html.EndForm(); %>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
</body>
</html>
