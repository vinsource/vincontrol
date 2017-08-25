<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.ContentViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GraphInfo</title>
    <link href="<%= Url.Content("~/Css/validationEngine.jquery.css") %>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            font-family: 'Trebuchet MS', Arial, sans-serif !important;
        }

        #result
        {
            font-family: "Trebuchet MS", Helvetica, sans-serif;
        }

        .hidden
        {
            display: none;
        }

        #rangeNav span
        {
            display: inline-block;
            width: 50px;
            text-align: center;
            padding: .3em .7em .3em .7em;
            background: #222222;
            color: white;
        }

            #rangeNav span.selected
            {
                font-weight: bold;
                background-color: #C80000;
            }

        #graphWrap
        {
            width: 300px;
            height: 143px;
            display: inline-block;
            float: left;
        }

        #carInfo
        {
            display: inline-block;
            padding: 10px;
            background-color: #c80000;
            color: #eeeeee;
            width: 200px;
            font-size: .7em;
        }

        #filter-wrap
        {
            background: #c80000;
            padding: .5em;
            color: white;
            width: 98%;
        }

        input[type="radio"]
        {
            background: transparent;
        }
        
        table#vehicle-list
        {
            font-size: .7em;
        }

        #vehicle-list td
        {
            padding: .3em .7em .3em .7em;
            border-bottom: 1px #bbbbbb solid;
        }

        #printable-list
        {
            display: block;
        }
        /*
        #vehicle-list tr:nth-child(1) td
        {
            font-weight: bold;
            color: black;
            border-bottom: #C80000 4px solid;
        }*/ #vehicle-list tr.highlight td
        {
            background: green;
            color: #fff;
        }

        #tblVehicles
        {
            align: center;
        }

        .formError .formErrorContent
        {
            width: 100px !important;
        }
        
    </style>
</head>
<body>
    <form id="formDistance">
        <div class="graph-title-bar" style="margin-bottom: 30px; padding-top: 20px;">
            <div style="float: left; padding-bottom: 5px;">
                <label style="font-size: 15px; color: #3366cc; font-weight: bold">Distance Search</label>
            </div>
            <div style="clear: both; border-top: 1px solid gray; padding-top: 15px; padding-left: 40px; text-align: center; font-size: 11px;">
                <div style="float: left">
                    <input type="text" id="txtFrom" style="width: 100px;" autocomplete="off" maxlength="5" data-validation-engine="validate[required]" data-errormessage-value-missing="this is required!" />
                </div>
                <div style="float: left; padding: 5px;">
                    to
                </div>
                <div style="float: left">
                    <input type="text" id="txtTo" style="width: 100px;" autocomplete="off" maxlength="5" data-validation-engine="validate[funcCall[checkDistanceRange],[required]" data-errormessage-value-missing="this is required!" />
                </div>
                <div style="float: left; padding: 5px;">
                    <input type="submit" id="btnSearch" value="Search" onclick="Search();" style="display: none;" />
                    <img src="../../Content/images/vincontrol/search.png" height="16" onclick="SubmitSearch();" />
                </div>
            </div>
        </div>
    </form>

    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/jquery.validationEngine-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/jquery.validationEngine.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            $("#txtTo").numeric({ decimal: false, negative: false }, function () {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#txtFrom").numeric({ decimal: false, negative: false }, function () {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            jQuery("#formDistance").validationEngine();
        });

        function checkDistanceRange(field, rules, i, options) {
            if ($("#txtFrom").val() != '' && $("#txtTo").val() != '') {
                if (parseInt($("#txtFrom").val().replace(',', '')) > parseInt($("#txtTo").val().replace(',', ''))) {
                    return "to distance must be greater than from distance";
                }
                else {
                    if (parseInt($("#txtTo").val().replace(',', '')) > 3000) {
                        return "to distance must be <=3000";
                    }
                }
            }
        }

        function Search() {
            if ($("#txtFrom").val() != '' && $("#txtTo").val() != '') {
                if (parseInt($("#txtFrom").val().replace(',', '')) > parseInt($("#txtTo").val().replace(',', ''))) {
                    return false;
                }
                else {
                    if (parseInt($("#txtTo").val().replace(',', '')) > 3000) {
                        return false;
                    }
                }
                var from = $('#txtFrom').val().replace(',', '');
                var to = $('#txtTo').val().replace(',', '');
                parent.$.fancybox.close();
                window.parent.MoreClick();
                window.parent.RedrawChartNew(from, to);
            }
        }

        function SubmitSearch() {
            $('#btnSearch').click();
            //$("#formDistance").Submit();
        }
    </script>
</body>
</html>
