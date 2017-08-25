<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InspectionFormCostModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CustomerInfo</title>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <% Html.BeginForm("UpdateInSpectionFormInfo", "Appraisal", FormMethod.Post, new { id = "InspectionInfoForm", name = "InspectionInfoForm" }); %>
    <%=Html.HiddenFor(x=>x.AppraisalId) %>
    <div class="customerInfo_popup_holder profile_popup">
        <div class="customerInfo_popup_header">
            Inspection Form Information
        </div>
        <div class="inspectionFormInfo_popup_content">
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        Mechanical:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="Mechanical" name="Mechanical" type="text" value="<%=Model.Mechanical %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        FrontBumper:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="FrontBumper" name="FrontBumper" type="text" value="<%=Model.FrontBumper %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        RearBumper:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="RearBumper" name="RearBumper" type="text" value="<%=Model.RearBumper %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        Glass:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="Glass" name="Glass" type="text" value="<%=Model.Glass %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        Tires:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="Tires" name="Tires" type="text" value="<%=Model.Tires %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        FrontEnd:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="FrontEnd" name="FrontEnd" type="text" value="<%=Model.FrontEnd %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        RearEnd:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="RearEnd" name="RearEnd" type="text" value="<%=Model.RearEnd %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        DriverSide:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="DriverSide" name="DriverSide" type="text" value="<%=Model.DriverSide %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        PassengerSide:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="PassengerSide" name="PassengerSide" type="text" value="<%=Model.PassengerSide %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        Interior:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="Interior" name="Interior" type="text" value="<%=Model.Interior %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        LightsBulbs:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="LightsBulbs" name="LightsBulbs" type="text" value="<%=Model.LightsBulbs %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        Other:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="Other" name="Other" type="text" value="<%=Model.Other %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        LMA:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input" id="LMA" name="LMA" type="text" value="<%=Model.LMA %>" maxlength="12">
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="inspectionFormInfo_popup_items">
                <div class="inspectionFormInfo_Left">
                    <label>
                        Total:
                    </label>
                </div>
                <div class="inspectionFormInfo_Right">
                    <input class="customerInfo_input_total" disabled="disabled" id="Total" name="Total" type="text" value="">
                </div>
            </div>
            <div style="clear: both"></div>

        </div>
        <div class="customerInfo_popup_btns">
            <div class="btns_shadow" id="inspectionFormInfo_save" onclick="SaveInspectionFormInfoSubmit();">
                Save
            </div>
            <div class="btns_shadow" id="inspectionFormInfo_cancel">
                <a onclick="parent.$.fancybox.close()">Cancel</a>
            </div>
        </div>
    </div>
    <% Html.EndForm(); %>
    
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function formatDollar(amount) {
            if (amount == 0)
                return amount;

            amount = amount.toString().replace(/^0+/, '');
            amount += '';
            x = amount.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }

        function checkSalePrice(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "Price should <= $100,000,000";
            }
        }

        $(document).ready(function () {
            $("input.customerInfo_input").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

            $('input.customerInfo_input').each(function () {
                if ($(this).val() != "") {
                    $(this).val(formatDollar(Number($(this).val().replace(/[^0-9\.]+/g, ""))));
                }
            });

            var total = 0;
            $('input.customerInfo_input').each(function () {
                if ($(this).val() != "") {
                    total += parseInt($(this).val().replace(/[^0-9\.]+/g, ""));
                }
            });
            $('#Total').val(formatDollar(total));

            $('input.customerInfo_input').blur(function () {
                total = 0;
                $('input.customerInfo_input').each(function () {
                    if ($(this).val() != "") {
                        total += parseInt($(this).val().replace(/[^0-9\.]+/g, ""));
                    }
                });
                $('#Total').val(formatDollar(total));
            });


            //Setup the AJAX call
            $("#InspectionInfoForm").submit(function (event) {
                $("#elementID").removeClass("hideLoader");
                event.preventDefault();
                SaveInspectionFormInfo(this);
            });
        });

        function SaveInspectionFormInfo(form) {
            blockUI();
            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: SaveInspectionFomrInfoClose
            });
        }

        function SaveInspectionFomrInfoClose(result) {
            if (result == "Success") {
                unblockUI();
                parent.$.fancybox.close();
            }

            else if (result == "DuplicateStock") {
                $("#elementID").addClass("hideLoader");
                $("#StockExist").removeClass("hideLoader");
            }
        }
        function SaveInspectionFormInfoSubmit() {
            $("#InspectionInfoForm").submit();
        }

    </script>
</body>
</html>
