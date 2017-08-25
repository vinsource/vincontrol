<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.CustomeInfoModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mark As Sold</title>
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <% Html.BeginForm("MarkSold", "Appraisal", FormMethod.Post, new { id = "markSoldForm", name = "markSoldForm" }); %>
    <div class="profile_tab_holder profile_popup" id="profile_marksold_holder" style="display: block">
        <div class="profle_transfer_header">
            Mark As Sold
        </div>
        <div class="profile_marksold_title">
            Customer Information
        </div>
        <div class="profile_marksold_items_holder">
            <div class="profile_marksold_items">
                <div class="profile_marksole_key">
                    First Name:
                </div>
                <div class="profile_marksole_value">
                    <%=Html.EditorFor(x=>x.FirstName) %>
                </div>
            </div>
            <div class="profile_marksold_items">
                <div class="profile_marksole_key">
                    Last Name:
                </div>
                <div class="profile_marksole_value">
                    <%=Html.EditorFor(x=>x.LastName) %>
                </div>
            </div>
            <div class="profile_marksold_items">
                <div class="profile_marksole_key">
                    Country:
                </div>
                <div class="profile_marksole_value">
                    <%=Html.DropDownListFor(x => x.Country, Model.Countries, new { @class = "DDLSold" })%>
                </div>
            </div>
            <div class="profile_marksold_items">
                <div class="profile_marksole_key">
                    Street:
                </div>
                <div class="profile_marksole_value">
                    <%=Html.EditorFor(x=>x.Street) %>
                </div>
            </div>
            <div class="profile_marksold_items_three" style="width: 20%;">
                <div class="profile_marksole_key">
                    Zip:
                </div>
                <div class="profile_marksole_value">
                    <%=Html.EditorFor(x=>x.ZipCode) %>
                </div>
            </div>
            <div class="profile_marksold_items_three" style="width: 38%;">
                <div class="profile_marksole_key">
                    City:
                </div>
                <div class="profile_marksole_value">
                    <%=Html.EditorFor(x=>x.City) %>
                </div>
            </div>
            <div class="profile_marksold_items_three" style="width: 38%;">
                <div class="profile_marksole_key">
                    State/Province
                </div>
                <div class="profile_marksole_value">
                    <%=Html.DropDownListFor(x => x.State, Model.States, new { @class = "DDLSold" })%>
                </div>
            </div>
            <div class="profile_marksold_delete_holder">
                Delete Immediately
                <%=Html.CheckBoxFor(x=>x.DeleteImmediately) %>
                <%=Html.HiddenFor(x=>x.ListingId) %>
            </div>
        </div>
        <div class="profile_marksold_btns_holder">
            <div class="profile_marksold_btns" onclick="MarkSoldSubmit()">
                Mark Sold
            </div>
            <div class="profile_marksold_btns profile_marksold_btns_cancel" onclick="window.parent.UnCheckStatus('chk');parent.$.fancybox.close();">
                Cancel
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
    
    <%if (Model.SessionTimeOut)
      { %>
    <script type="text/javascript">
        parent.$.fancybox.close();
        ShowWarningMessage("Your session is timed out. Please login back again");
        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
        window.parent.location = actionUrl;
    </script>
    <% }%>
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
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

        function MarkSoldSubmit() {
            $("#markSoldForm").submit();
        }

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
            try {
                window.parent.UnCheckStatus('chk');
                parent.$.fancybox.close();

                if (result > 0) {
                    var actionUrl = "/Inventory/ViewISoldProfile?ListingID=" + result;
                    window.parent.location.href = actionUrl;
                }


            }
            catch (e) {
                actionUrl = '<%= Url.Action("ViewInventory", "Inventory") %>';
                window.parent.location.href = actionUrl;
            }
        }

    </script>
</body>
</html>
