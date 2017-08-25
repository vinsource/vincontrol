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
    <% Html.BeginForm("AutoLoanCalculatorSummary", "Inventory", FormMethod.Post, new { id = "autoloanCalculatorForm", name = "autoloanCalculatorForm" }); %>
    <div class="kpi_print_title">
        Auto Loan Calculator
    </div>
    <div class="fsqRow smHeight4" style="height: 320px;">
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Vehicle Price $
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.VehiclePrice, new { maxlength=13, @class = "fsqInpuText loRequired", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.VehiclePrice)})%>
                </div>
            </div>
            <div class="loInvalidMessage">*This field is required.</div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Down Payment $
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.DownPayment, new { maxlength=13, @class = "fsqInpuText", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.DownPayment)})%>
                </div>
            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Trade In Value $
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.TradeInValue, new { maxlength=13, @class = "fsqInpuText", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.TradeInValue)})%>
                </div>
            </div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Sale Tax % (e.g 8.25)
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.SaleTax, new {maxlength=6, @class = "fsqInpuText", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.SaleTax)})%>
                </div>
            </div>
            <div class="loInvalidMessage">*Sale tax must be under 100%</div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Interest rate % (e.g 3.5)
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.InterestRate, new { maxlength=6, @class = "fsqInpuText", @style = "font-size: 16px" ,Value=String.Format("{0:N}",Model.InterestRate)})%>
                </div>
            </div>
            <div class="loInvalidMessage">*Interest rate must be under 100%</div>
        </div>
        <div class="loCalculateItems">
            <div class="sqrPaymentText">
                Loan Term(months)
            </div>
            <div class="sqrPayments">
                <div class="fsqInput fsqFloatLeft borderBottom">
                    <%=Html.TextBoxFor(x => x.Terms, new { maxlength=3, @class = "fsqInpuText loRequired", @style = "font-size: 16px"})%>
                </div>
            </div>
            <div class="loInvalidMessage">*This field is required</div>
        </div>

    </div>
    <div class="status_btn_holder">
        <button style="cursor: pointer" type="button" id="status_reset" class="btns_shadow">Reset</button>
        <button style="cursor: pointer" id="calculateBtn" type="submit" class="btns_shadow">Calculate</button>
    </div>
    <% Html.EndForm(); %>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type="text/javascript">

        $("#status_reset").live('click', function () {
            $('#VehiclePrice').val(0);
            $('#DownPayment').val(0);
            $('#TradeInValue').val(0);
            $('#SaleTax').val(10.00);
            $('#InterestRate').val(10.00);
            $('#Terms').val(60);
        });

        $(".fsqInpuText").keypress(function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 40 && charCode != 37 && charCode != 38 && charCode != 39 && charCode != 46) {
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
            }

            return true;
        });

        $("#SaleTax").focusout(function () {
            var value = $(this).val();
            if (parseFloat(value) > 100) {
                $(this).focus();
                $(this).parent().parent().parent().find(".loInvalidMessage").show();
            } else {
                $(this).parent().parent().parent().find(".loInvalidMessage").hide();
            }
        });
        $("#InterestRate").focusout(function () {
            var value = $(this).val();
            if (parseFloat(value) > 100) {
                $(this).focus();
                $(this).parent().parent().parent().find(".loInvalidMessage").show();
            } else {
                $(this).parent().parent().parent().find(".loInvalidMessage").hide();
            }
        });

        $(".loRequired").focusout(function () {
            var value = $(this).val();
            if (!value || parseFloat(value) == 0) {
                $(this).focus();
                $(this).parent().parent().parent().find(".loInvalidMessage").show();
            } else {
                $(this).parent().parent().parent().find(".loInvalidMessage").hide();
            }
        });

    </script>
</body>
</html>

