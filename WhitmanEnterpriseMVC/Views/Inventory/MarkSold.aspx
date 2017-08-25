<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.CustomeInfoModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mark As Sold</title>
    <link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.js"></script>
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            background: #222;
            color: #ddd;
        }
        body
        {
            width: 500px;
            margin: 0 auto;
        }
        #container
        {
            background: #333;
            padding: 1em;
        }
        h3, ul
        {
            margin: 0;
        }
        input[type="text"]
        {
            width: 200px;
        }
        span.label
        {
            display: block;
            width: 150px;
            float: left;
            clear: right;
        }
        input[type="submit"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 1.1em;
            font-weight: bold;
            padding: .5em;
            margin-top: -2em;
        }
        input[type="button"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 1.1em;
            font-weight: bold;
            padding: .5em;
            margin-top: -2em;
        }
        .short
        {
            width: 50px !important;
        }
        
        #spnZipError {
            color: red;
            font-size: 12px;
        }
       
    </style>
</head>
<body>
    <%if (Model.SessionTimeOut)
      { %>
    <script>
        parent.$.fancybox.close();
        alert("Your session is timed out. Please login back again");
        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
        window.parent.location = actionUrl;

</script>
    <% }%>
    <script type="text/javascript">

        $(document).ready(function () {

            //Setup the AJAX call
            $("#markSoldForm").submit(function (event) {
                event.preventDefault();
                SaveMarkSold(this);
            });

            $("input#State").hide();
            $("input#State").attr("disabled", "disabled");

            $("#Country").change(function () {
                if ($(this).val() != 'US') {
                    $("#spnZipError").text("");
                    $("select#State").hide();
                    $("select#State").attr("disabled", "disabled");
                    $("input#State").show();
                    $("input#State").removeAttr("disabled");
                    $("#lblState").html("State/Province");
                }
                else {
                    ValidateZipCode();
                    $("select#State").show();
                    $("select#State").removeAttr("disabled");
                    $("input#State").hide();
                    $("input#State").attr("disabled", "disabled");
                    $("#lblState").html("State");

                }
            });

            $("#ZipCode").keyup(function () {
                if ($("#Country").val() == 'US') {
                    ValidateZipCode();
                }
            }
            );
        });
        
        function ValidateZipCode() {
            var value = $("#ZipCode").val();
            if ((value.length == 5) && IsInt(value)) {
                $("#spnZipError").text("");
                AjaxUpdateCity(value);
            } else {
                $("#spnZipError").text("ZIP contains 5 numbers");
            }
        }

        function AjaxUpdateCity(zipCode) {
            $.ajax({
                url: '/Inventory/GetCityAndStateFromZip?zipCode=' + zipCode,
                cache: false,
                dataType: "json",
                type: "GET",
                data: {},
                success: function (result) {
                    $("#City").val(result.CityName);
                    $("select#State").val(result.StateAbbr);
                },
                error: function () {
                    $("#spnZipError").text("Cannot find City and State with this Zip.");
                }
            });
        }

        function IsInt(value) {
            if ((parseFloat(value) == parseInt(value)) && !isNaN(value)) {
                return true;
            } else {
                return false;
            }
        }

        function SaveMarkSold(form) {
            $.ajax({
                url: form.action,
                type: form.method,
                dataType: "json",
                data: $(form).serialize(),
                success: MarkSoldSave
            });
        }

        function MarkSoldSave(result) {

            parent.$.fancybox.close();
            var actionUrl = '<%= Url.Action("ViewInventory", "Inventory", new { dealershipId = "PLACEHOLDER"  } ) %>';
            actionUrl = actionUrl.replace('PLACEHOLDER', result);
            window.parent.location.href = actionUrl;
        }

    </script>
    <h1 style="margin-bottom: 0;">
        Mark As Sold</h1>
    <div id="container">
        <% Html.BeginForm("MarkSold", "Inventory", FormMethod.Post, new { id = "markSoldForm", name = "markSoldForm" }); %>
        <h3>
            Customer Information</h3>
        <div id="space" class="label">
            First Name:
        </div>
        <%=Html.EditorFor(x=>x.FirstName) %>
        <div class="label">
            Last Name:
        </div>
        <%=Html.EditorFor(x=>x.LastName) %>
        <div class="label">
            Country:
        </div>
        <%=Html.DropDownListFor(x=>x.Country,Model.Countries) %>
        <div class="label">
            Address:
        </div>
        <%=Html.EditorFor(x=>x.Address) %>
        <div class="label">
            ZIP:
        </div>
        <%=Html.EditorFor(x=>x.ZipCode) %>
         <span id="spnZipError"></span>
        <div class="label">
            City:
        </div>
        <%=Html.EditorFor(x=>x.City) %>
       
        <div class="label" id="lblState">
            State:
        </div>
        <%=Html.DropDownListFor(x=>x.State,Model.States) %>
        <%=Html.TextBoxFor(x => x.State)%>
        <br />
        <br />
        <div class="label">
            Delete Immediately
            <%=Html.CheckBoxFor(x=>x.DeleteImmediately) %></div>
        <%=Html.HiddenFor(x=>x.ListingId) %>
        <br />
        <input type="submit" name="submit" value="Mark Sold" />
        <input type="button" name="Cancel" onclick="parent.$.fancybox.close()" value="Cancel" />
        <% Html.EndForm(); %>
    </div>
</body>
</html>
