<%@ Page Title="" MasterPageFile="~/Views/Shared/Site.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=Model.ModelYear %>
        <%=Model.Make %>
        <%=Model.SelectedModel %></title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.js" type="text/javascript" language="javascript"></script>
    <!-- styles needed by jScrollPane -->
    <link type="text/css" href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>"
        rel="stylesheet" media="all" />
    <!-- latest jQuery direct from google's CDN -->
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <!-- Fancybox Plugin -->
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <style type="text/css">
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
        #warranty-info, #pricing
        {
            width: 206px;
            padding-left: 0;
        }
        #warranty-info .column, #pricing .column
        {
            padding-left: 0;
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
        div.scrollable-list
        {
            max-height: 150px;
            overflow: hidden;
            overflow-y: scroll;
            background: #111111;
        }
        input.small
        {
            width: 75px !important;
        }
        #column-two
        {
            height: 510px;
        }
        #c2 h3, #c2 h5
        {
            margin: 0;
            padding: 0;
        }
        #c2 h5
        {
            margin-bottom: 20px;
        }
        #pricing td.label
        {
            display: block;
            width: 140px !important;
        }
        input#edit-images
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 96%;
        }
        input#btndescription
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 100%;
        }
        #description-box
        {
            width: 48%;
            padding: 0;
            padding-top: 15px;
        }
        #other
        {
            background: #333333;
            overflow: hidden;
            width: 726px !important;
            position: relative;
            padding: 0;
            margin-left: 10px;
            padding-bottom: 5px;
        }
        #images
        {
            width: 48%;
            margin-left: 2%;
            padding: 0;
            padding-top: 15px;
        }
        #img-title
        {
            margin-left: 15px;
        }
        #images img
        {
            margin-left: 1%;
            margin-top: 3px;
            padding: 0;
            display: inline-block;
            width: 5%;
        }
        #description-input
        {
            width: 98%;
            height: 100px;
            resize: none;
            margin-left: 1px;
        }
        #fancybox-content
        {
            background: #111111;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

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
</script>
    <% Html.BeginForm("Action", "Appraisal", FormMethod.Post, new { id = "editiProfileForm", name = "editiProfileForm", onsubmit = "return validateForm()"}); %>
    <div id="divDetailAppraisal"><%= Html.Partial("DetailAppraisal", Model) %></div>    
    <% Html.EndForm(); %>
    <script type="text/javascript">

        $("#CancelIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });

        $("#SaveIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });
        
        $("a.iframe").fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.smalliframe").fancybox();
        
        $(document).ready(function () {
            $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

            if ($("#SelectedExteriorColorValue").val() == '' && $("#SelectedExteriorColor :selected").text()) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            }

            $("#SelectedExteriorColor").live('change', function (e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            });

            $("#SelectedVehicleType").live('change',function (e) {

                $('#elementID').removeClass('hideLoader');

                var VehicleType = $("#SelectedVehicleType").val();

                if (VehicleType == 'Car') {
                    var actionUrl = '<%= Url.Action("EditAppraisal", "Appraisal", new { appraisalId = "PLACEHOLDER" } ) %>';

                    actionUrl = actionUrl.replace('PLACEHOLDER', $("#AppraisalGenerateId").val());

                    window.location = actionUrl;
                }

                $('#elementID').addClass('hideLoader');
                
            });

            getSelectedOption();
            getSelectedPakage();

            $("#SelectedTrimItem").live('change', function () {

                var name = $("#SelectedTrimItem option:selected").text();
                if (name != '') {
                    $('#elementID').removeClass('hideLoader');
                    var id = $("#SelectedTrimItem").val().split('|')[0];
                    var style = $("#SelectedTrimItem").val().split('|')[1];
                    $.ajax({
                        type: "GET",
                        url: "/Appraisal/GetVehicleInformationFromStyleId?appraisalId=" + $('#AppraisalGenerateId').val() + "&vin=" + $('#VinNumber').val() + "&styleId=" + id + "&isTruck=True&styleName=" + style + "&cusStyle=" + $("#CusTrim").val(),
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

        function validateForm() {
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
                $("#ErrorCusEx").remove()
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

        function getSelectedPakage() {
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {
                    descriptionArray.push($(result[i]).attr('value'));
                }
            }
            if (result.length > 0) {
                $("#hdnFistSelectedPackages").val(JSON.stringify(descriptionArray));
            }
        }

        function getSelectedOption() {
            var result = $("#optionals input[name='txtNonInstalledOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {
                    descriptionArray.push($(result[i]).attr('value'));
                }
            }
            if (result.length > 0) {
                $("#hdnFistSelectedOptions").val(JSON.stringify(descriptionArray));
            }
        }
                        
</script>
</asp:Content>
