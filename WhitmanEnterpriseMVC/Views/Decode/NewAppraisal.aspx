<%@ Page Title="New Appraisal" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>New Appraisal</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/ui.datepicker.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <%--<script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>--%>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>

    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <style type="text/css">
        #noteBox
        {
            width: 735px;
            margin-bottom: 1em;
        }
        #c2
        {
            width: 785px;
            border-right: none;
            overflow: hidden;
            height: 100%;
        }
        h4
        {
            margin-bottom: 0;
            margin-top: 0;
        }
        select
        {
            width: 135px;
        }
        p
        {
            padding: 1em;
            border-bottom: 1px solid #101010;
            border-top: 1px #777 solid;
            margin: 0;
        }
        p.top
        {
            border-top: none;
            padding: 0;
            margin-top: .5em;
        }
        p.bot
        {
            border-bottom: none;
            padding: 0;
        }
        p
        {
            margin-top: 0;
        }
        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }
        #noteList
        {
            padding: 1em;
        }
        #submit
        {
            float: right;
        }
        #optionals
        {
            width: 450px;
            margin-top: 0;
            margin-bottom: 0;
            overflow: hidden;
        }
        #c2
        {
            padding-top: 0;
        }
        .hider
        {
            display: none;
        }
        input[name="options"]
        {
            margin-bottom: 0;
            margin-left: 0;
        }
        textarea[name="description"]
        {
            width: 420px;
        }
        #optionals ul
        {
            margin-top: .3em;
        }
        .scroll-pane
        {
            height: 100%;
            overflow: auto;
            overflow-x: hidden;
        }
        #notes
        {
            height: 200px;
            margin-bottom: 6em;
        }
        input[name="completeApp"]
        {
            float: right;
        }
        #Packages
        {
            max-height: 175px;
            width: 450px;
            overflow: hidden;
            overflow-y: scroll;
            font-size: .9em;
            background-color:#111111;
        }
        #Options
        {
            max-height: 300px;
            width: 450px;
            overflow: hidden;
            overflow-y: scroll;
            font-size: .9em;
            background-color:#111111;
        }
        .hideLoader
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("ViewProfileByVin", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", onsubmit = "return validateForm()" }); %>
    <div id="divDetailAppraisal"><%= Html.Partial("DetailAppraisal", Model) %></div>
    <% Html.EndForm(); %>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
            
            if ($("#SelectedExteriorColorValue").val() == '' && $("#SelectedExteriorColor :selected").text()) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            }
            
            $("#SelectedExteriorColor").live('change', function (e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            });

            $("#SelectedTrim").live('change', function () {
                
                var name = $("#SelectedTrim option:selected").text();
                if (name != '') {
                    $('#elementID').removeClass('hideLoader');
                    var id = $("#SelectedTrim").val().split('|')[0];
                    var style = $("#SelectedTrim").val().split('|')[1];
                    $.ajax({
                        type: "GET",
                        url: "/Decode/GetVehicleInformationFromStyleId?vin=" + $('#VinNumber').val() + "&styleId=" + id + "&isTruck=False&styleName=" + style + "&cusStyle=" + $("#CusTrim").val(),
                        data: {},
                        success: function (results) {
                            $("#divDetailAppraisal").html(results);
                            $('#elementID').addClass('hideLoader');
                            $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
                        },
                        error: function (err) {
                            alert('Server Error: ' + err.status + " - " + err.statusText);
                            $('#elementID').addClass('hideLoader');
                        }
                    });
                }
            });

        });

        function changeMSRP(checkbox) {

            if (checkbox.checked) {

                var optionPrice = checkbox.value;
                optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
                var total = Number(number1) + Number(number2);

            }
            else {

                var optionPrice = checkbox.value;
                optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
                var total = Number(number1) - Number(number2);
            }


            $("#MSRP").val(formatDollar(total));



        }

        function validateForm() {

            var x = document.forms["viewProfileForm"];

            var flag = true;

            if ($("#CusErrorEx").length > 0) {
                $("#CusErrorEx").remove();
            }

            if ($("#CusErrorIn").length > 0) {
                $("#CusErrorIn").remove();
            }

            if ($("#CusErrorApp").length > 0) {
                $("#CusErrorApp").remove();
            }

            if ($("#CusErrorTransmission").length > 0) {
                $("#CusErrorTransmission").parent().remove();
            }

            if ($("#CusErrorMileage").length > 0) {
                $("#CusErrorMileage").parent().remove();
            }

            if ($("#SelectedExteriorColor").val() == "Other Colors" && $("#CusExteriorColor").val() == "") {
                $("#CusExteriorColor").parent().append("<strong id='CusErrorEx'><font color='Red'>Required</font></strong>");
                flag = false;
            }

            if ($("#SelectedInteriorColor").val() == "Other Colors" && $("#CusInteriorColor").val() == "") {
                $("#CusInteriorColor").parent().append("<strong id='CusErrorIn'><font color='Red'>Required</font></strong>");
                flag = false;
            }

            if ($("#SelectedTranmission").val() == "") {
                $("#SelectedTranmission").parent().append("<div><strong id='CusErrorTransmission'><font color='Red'>Required</font></strong></div>");
                flag = false;
            }

            if ($("#Mileage").val() == "") {
                $("#Mileage").parent().append("<div><strong id='CusErrorMileage'><font color='Red'>Required</font></strong></div>");
                flag = false;
            }

            if (flag == false)
                return false;
            else
                $('#elementID').removeClass('hideLoader');

            var result = $("#Packages input[name='txtFactoryPackageOption']:checked");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('title') !== undefined) {
                    descriptionArray.push($(result[i]).attr('title'));
                }
            }
            if (result.length > 0) {
                $("#SelectedPackagesDescription").val(JSON.stringify(descriptionArray));
            }

        }   

</script>
</asp:Content>
