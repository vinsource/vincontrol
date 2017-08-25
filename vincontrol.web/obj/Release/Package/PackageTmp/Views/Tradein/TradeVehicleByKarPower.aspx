<%@ Page Title="Trade In" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.TradeInVehicleModel>" %>

<!DOCTYPE html>
<html>
<head>
    <title>Trade-In Value</title>
    <link href="<%= Url.Content("~/Css/TradeIn/style.css") %>" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]>
	<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Css/TradeIn/ie-7.css") %>">
	<![endif]-->
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <%--<script src="http://code.jquery.com/jquery-latest.js"></script>--%>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
</head>
<body>
    <div id="container" class="step1">
        <div id="header">
            <div class="logo">
            </div>
            <div class="mask">
                <div class="text-wrap">
                    <h1>
                        Get Your Trade In Value!</h1>
                </div>
            </div>
            <div class="steps">
                <div id="step-1" class="step">
                    <img src="<%= Url.Content("~/images/on-step-1.png")%>" alt="step 1" /></div>
                <div id="step-2" class="step">
                    <img src="<%= Url.Content("~/images/step-2.png")%>" alt="step 2" /></div>
                <div id="step-3" class="step">
                    <img src="<%= Url.Content("~/images/step-3.png")%>" alt="step 3" /></div>
            </div>
        </div>
        <div class="slide-wrapper">
            <div class="info-wrap">
                <h3 class="description-header">
                    Thinking Of Trading In Your Vehicle?
                </h3>
                <div class="info-box">
                    <p class="description">
                        Input your VIN number or select from the dropdown choices, then fill out the rest
                        of the form and you will be on your way to knowing your vehicle's trade-in value!</p>
                </div>
            </div>
            <div id="vehicle-info" class="info-wrap">
                <h3 class="description-header">
                    Vehicle Information</h3>
                <% Html.BeginForm("TradeInWithOptions", "TradeIn", FormMethod.Post, new { id = "TradeVehicleForm", onsubmit = "return validateForm()" }); %>
                <div class="info-box">
                    <div class="error-wrap">
                        <p class="error" title="Click to Close">
                        </p>
                    </div>
                    <div class="row">
                        <div id="decode" class="row-cell">
                            <h3>
                                Vehicle Info</h3>
                            <%--<%= Html.TextBoxFor(x => x.Vin, new {title="VIN Number", placeholder="Enter Vin Number Here!"}) %>--%>
                            <input type="text" id="Vin" name="Vin" value="<%= Model.Vin %>"/>
                            
                            <%=Html.HiddenFor(x=>x.ValidVin) %>
                            <em>- or - </em><b>Select</b> <span id="divYears">
                                <%= Html.Partial("Years", Model) %></span> <span id="divMakes">
                                    <%= Html.Partial("Makes", Model) %></span> <span id="divModels">
                                        <%= Html.Partial("Models", Model) %></span> <span id="divTrims">
                                            <%= Html.Partial("Trims", Model) %></span>
                            <%= Html.HiddenFor(x=>x.Dealer) %>
                            <%= Html.HiddenFor(x=>x.SelectedMake) %>
                        </div>
                    </div>
                    <div class="row">
                        <div class="row-cell">
                            <h3>
                                Mileage</h3>
                            <%= Html.TextBoxFor(x => x.Mileage,new {title="Mileage" ,placeholder="Mileage!"}) %>
                        </div>
                        <div id="condition" class="row-cell">
                            <h3>
                                Condition</h3>
                            <div class="con_btn" title="Poor Condition" id="poor">
                                <%if (Model.Condition == "poor")
                                  { %>
                                <img src="<%= Url.Content("~/images/on-poor.jpg") %> " />
                                <% }
                                  else
                                  { %>
                                <img src="<%= Url.Content("~/images/poor.jpg") %> " />
                                <% } %>
                            </div>
                            <div class="con_btn" title="Fair Condition" id="fair">
                                <%if (Model.Condition == "fair")
                                  { %>
                                <img src="<%= Url.Content("~/images/on-fair.jpg")%> " />
                                <% }
                                  else
                                  {%>
                                <img src="<%= Url.Content("~/images/fair.jpg")%> " />
                                <% } %>
                            </div>
                            <div class="con_btn" title="Great Condition" id="great">
                                <%if (Model.Condition == "great")
                                  { %>
                                <img src="<%= Url.Content("~/images/on-great.jpg") %>" />
                                <% }
                                  else
                                  {
                                %>
                                <img src="<%= Url.Content("~/images/great.jpg") %>" />
                                <%    
                                   } %>
                            </div>
                            <%if (Model.Condition == "poor")
                              { %>
                            <input type="radio" name="condition" class="poor" value="poor" id="rbPoor" checked="checked" />
                            <% }
                              else
                              {%>
                            <input type="radio" name="condition" class="poor" value="poor" id="rbPoor"/>
                             <%     
                              }%>
                              
                              <%if (Model.Condition == "fair")
                                { %>
                            <input type="radio" name="condition" class="fair" value="fair" id="rbFair" checked="checked"/>
                            <% }
                                else
                                { %>
                            <input type="radio" name="condition" class="fair" value="fair" id="rbFair" />
                             <% } %>
                             
                              <%if (Model.Condition == "great")
                                { %>
                            <input type="radio" name="condition" class="great" value="great" id="rbGreat" checked="checked"/>
                            <% }
                                else
                                { %>
                            <input type="radio" name="condition" class="great" value="great" id="rbGreat" />
                               <% } %>

                            <%= Html.HiddenFor(x=>x.Condition) %>
                        </div>
                    </div>
                    <div id="get-value-btn" class="row">
                    <%if (Model.IsValidDealer) { %>
                        <img id="imgSubmit" title="Get Your Trade-In Value!" src="<%= Url.Content("~/images/get-trade-value-btn.jpg")%>" />
                    <% } %>
                    </div>
                </div>
                <%Html.EndForm(); %>
            </div>
        </div>
    </div>
    <script src="<%=Url.Content("~/js/trade-in.js")%>" type="text/javascript"></script>
</body>
<script type="text/javascript">
    //    function TradeVehicleFormSubmit() {
    //        $("#TradeVehicleForm").submit();
    //    }

    function trimString(text) {
        return text.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
    }

    function validateForm() {
        var flag = true;
        var errorCount = 0;

        if ($("#SelectedTrim").val() == null && trimString($("#Vin").val()) == "") {
            $("#Vin").val("");
            error("Please select a vehicle or vehicle identification number before continuing", "37px");
            return false;
        }

        else if ($("#SelectedTrim").val() == "0" && trimString($("#Vin").val()) == "") {
            $("#Vin").val("");
            error("Please select a vehicle or vehicle identification number before continuing", "37px");
            return false;
        }

        else {
            var errorString = "Please correct all errors below : ";
            if (trimString($("#Mileage").val()) == "") {
                errorString += "<li>Mileage info is required</li>";
                flag = false;
                errorCount++;
            } else {
                var mileageInfo = trimString($("#Mileage").val());
                var numbermileage = Number(mileageInfo.replace(/[^0-9\.]+/g, ""));
                if (isNaN(numbermileage) || numbermileage > 1000000) {
                    errorString += "<li>Valid mileage info is required</li>";
                    flag = false;
                    errorCount++;
                }
            }

            if ($("#rbPoor").is(':checked') == false && $("#rbFair").is(':checked') == false && $("#rbGreat").is(':checked') == false) {
                errorString += "<li>Condition info is required</li>";
                flag = false;
                errorCount++;
            } else {
                if ($("#rbPoor").is(':checked'))
                    $("#Condition").val("Poor");
                else if ($("#rbFair").is(':checked'))
                    $("#Condition").val("Fair");
                else if ($("#rbGreat").is(':checked'))
                    $("#Condition").val("Great");

            }
        }
        if (flag == false) {
            if (errorCount == 1)
                error(errorString, "60px");
            else if (errorCount == 2)
                error(errorString, "90px");
            else
                error(errorString, "120px");
            return false;
        }

        return true;
    }

    $(document).ready(function () {

        //        if ($("#ValidVin").val() == "False")
        //            error("Invalid vin or no result matched. Please try again", "37px");

        $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

        //        $.ajax({
        //            type: "GET",
        //            url: "/TradeIn/GetYears",
        //            data: {},
        //            dataType: 'html',
        //            success: function (data) {
        //                $("#divYears").html(data);
        //            },
        //            error: function (xhr, ajaxOptions, thrownError) {
        //                alert(xhr.status + ' ' + thrownError);
        //            }
        //        });

        $("#imgSubmit").live('click', function () {
            if (validateForm()) {
                if (trimString($("#Vin").val()) == "") {
                    $("#TradeVehicleForm").submit();
                } else {
                    $.post('<%= Url.Content("~/TradeIn/VinDecode") %>', { vin: $("#Vin").val() }, function (result) {
                        if (result == "False") {
                            error("Invalid vin or no result matched. Please try again", "37px");
                        } else {
                            $("#TradeVehicleForm").submit();
                        }
                    });
                }

            }
        });

        $("#SelectedYear").live('change', function () {
            $("#SelectedMake").html("");
            $("#SelectedModel").html("");
            $("#SelectedTrim").html("");
            $("#SelectedMakeValue").val("");
            $("#SelectedModelValue").val("");
            $("#SelectedTrimValue").val("");

            if ($("#SelectedYear").val() != "Year..." && $("#SelectedYear").val() != "0") {

                $('#decode select').each(function () {
                    //console.log($(this).attr("id"));
                    $(this).prop('disabled', true);
                });


                $.ajax({
                    type: "GET",
                    url: "/TradeIn/GetMakes",
                    data: { yearId: $("#SelectedYear").val() },
                    dataType: 'html',
                    success: function (data) {
                        $("#divMakes").html(data);
                        EnableSelectList();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status + ' ' + thrownError);
                    }
                });

            }
        });

        $("#SelectedMake").live('change', function () {
            $("#SelectedModel").html("");
            $("#SelectedTrim").html("");
            $("#SelectedModelValue").val("");
            $("#SelectedTrimValue").val("");
            $("#SelectedMakeValue").val("");
            if ($("#SelectedMake").val() != "Make..." && $("#SelectedMake").val() != "0") {

                $('#decode select').each(function () {
                    //console.log($(this).attr("id"));
                    $(this).prop('disabled', true);
                });

                $.ajax({
                    type: "GET",
                    url: "/TradeIn/GetModels",
                    data: { yearId: $("#SelectedYear").val(), makeId: $("#SelectedMake").val() },
                    dataType: 'html',
                    success: function (data) {
                        $("#divModels").html(data);
                        EnableSelectList();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status + ' ' + thrownError);
                    }
                });

                $("#SelectedMakeValue").val($("#SelectedMake :selected").text());

            }
        });

        $("#SelectedModel").live('change', function () {
            $("#SelectedTrim").html("");
            $("#SelectedTrimValue").val("");
            $("#SelectedModelValue").val("");
            if ($("#SelectedModel").val() != "Model..." && $("#SelectedModel").val() != "0") {
                $('#decode select').each(function () {
                    //console.log($(this).attr("id"));
                    $(this).prop('disabled', true);
                });



                $.ajax({
                    type: "GET",
                    url: "/TradeIn/GetTrims",
                    data: { yearId: $("#SelectedYear").val(), makeId: $("#SelectedMake").val(), modelId: $("#SelectedModel").val() },
                    dataType: 'html',
                    success: function (data) {
                        $("#divTrims").html(data);
                        EnableSelectList();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status + ' ' + thrownError);
                    }
                });

                $("#SelectedModelValue").val($("#SelectedModel :selected").text());

            }
        });

        $("#SelectedTrim").live('change', function () {
            if ($("#SelectedTrim").val() != "Trim..." && $("#SelectedTrim").val() != "0") {


                $("#SelectedTrimValue").val($("#SelectedTrim :selected").text());
            }
        });
    });

    function EnableSelectList() {
        $("#SelectedYear").removeAttr('disabled');
        $("#SelectedMake").removeAttr('disabled');
        $("#SelectedModel").removeAttr('disabled');
        $("#SelectedTrim").removeAttr('disabled');
    }

</script>
</html>
