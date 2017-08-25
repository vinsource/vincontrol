<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<WhitmanEnterpriseMVC.Models.DealershipActivityViewModel>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dealer Activity</title>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            background: #222;
            color: #ddd;
        }
        body
        {
            width: 980px;
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
            width: 30px;
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
            float: right;
            margin-top: -2em;
        }
        .short
        {
            width: 50px !important;
        }
        .submit
        {
            background: none repeat scroll 0 0 #860000;
            border: medium none #000000;
            color: #FFFFFF;
            cursor: pointer;
            display: inline-block;
            font-size: 14px;
            font-weight: normal;
            padding: 2px 8px;
            width: 100px;
            text-align: center;
            padding: 4px 2px;
        }
        .aDetailActivity :hover { color: Red; }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#ddlMonths").live('change', function () {
                FilterList($("#ddlMonths").val(), $("#ddlYears").val());
            });

            $("#ddlYears").live('change', function () {
                FilterList($("#ddlMonths").val(), $("#ddlYears").val());
            });

            $("tr[id^='aDetailActivity_']").live('click', function () {
                var id = this.id.split("_")[1];

                window.parent.document.getElementById('ActivityId').value = id;
                window.parent.document.getElementById('NeedToRedirectToDetailActivity').value = true;
                parent.$.fancybox.close();
            });
        });

        function FilterList(month, year) {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: '<%= Url.Action("FilterDealerActivity", "Market") %>',
                data: { month: month, year: year },
                cache: false,
                traditional: true,
                success: function (results) {
                    $('#detailDealerActivity').html(results);

                },
                error: function (err) {
                    alert('System Error: ' + err.status + " - " + err.statusText);
                }
            });
        }
    </script>
</head>
<body>
    <div id="container" style="width: 950px;">
    <div style="display:block;width:100%;">
        <select id="ddlMonths">
            <option value="0">All Months</option>
            <option value="1">January</option>
            <option value="2">February</option>
            <option value="3">March</option>
            <option value="4">April</option>
            <option value="5">May</option>
            <option value="6">June</option>
            <option value="7">July</option>
            <option value="8">August</option>
            <option value="9">September</option>
            <option value="10">October</option>
            <option value="11">November</option>
            <option value="12">December</option>
        </select>
        <select id="ddlYears">
            <option value="0">All Years</option>
            <% for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 3; i--){%>
            <option value="<%= i %>"><%= i %></option>
            <%}%>
        </select>
    </div>
    <table id="manheimTransaction" width="100%" class="reportText">
        <thead style="cursor: pointer;">
            <tr>
                <th align="left" width="60%">
                    Action
                </th>
                <th align="left" width="20%">
                    User Stamp
                </th>
                <th align="right" width="20%">
                    Date Stamp
                </th>                
            </tr>
        </thead>
        <tbody id="detailDealerActivity">
            <%= Html.Partial("DealerActivityDetail", Model)%>
        </tbody>
    </table>
    </div>
</body>
</html>
