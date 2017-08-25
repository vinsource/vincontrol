<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Vincontrol.Web.Models.TradeinCustomerViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Trade In Customers
        <input type="button" id="btnPrint" value="Print" /></h2>
    <div id="printable-list">
        <div id="vehicle-list">
            <table>
                <tr>
                    <th>Name
                    </th>
                    <%--<th>
                Email
            </th>--%>
                    <th>Phone
                    </th>
                    <th>Mileage
                    </th>
                    <th>Date
                    </th>
                    <th>Year
                    </th>
                    <th>Make
                    </th>
                    <th>Model
                    </th>
                    <th>Condition
                    </th>
                    <th>Status
                    </th>
                    <th></th>
                </tr>
                <% foreach (var item in Model)
                   { %>
                <tr>
                    <td>
                        <%= Html.Encode(item.FirstName) %>
                        <%= Html.Encode(item.LastName) %>
                    </td>
                    <%-- <td>
                <%= Html.Encode(item.Email) %>
            </td>--%>
                    <td>
                        <%= Html.Encode(item.Phone) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.MileageAdjustment) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Date) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Year) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Make) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Model) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Condition) %>
                    </td>
                    <td>
                        <select id="opt_<%=item.ID %>">
                            <option value="Empty"></option>
                            <%if (item.TradeInStatus == "Done")
                              { %>
                            <option value="Done" selected="selected">Done</option>
                            <%}
                              else
                              { %>
                            <option value="Done">Done</option>
                            <%} %>
                            <%if (item.TradeInStatus == "Pending")
                              { %>
                            <option value="Pending" selected="selected">Pending</option>
                            <%}
                              else
                              { %>
                            <option value="Pending">Pending</option>
                            <%} %>
                            <%if (item.TradeInStatus == "Dead")
                              { %>
                            <option value="Dead" selected="selected">Dead</option>
                            <%}
                              else
                              { %>
                            <option value="Dead">Dead</option>
                            <%} %>
                        </select>
                    </td>
                    <td>
                        <input type="button" id="btnSave_<%= item.ID%>" name="save-comment" value="Save" />
                    </td>
                </tr>
                <% } %>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id^=btnSave]").click(function () {
                var idValue = this.id.split('_')[1];
                var value = $("#opt_" + idValue).val();
                $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>"  /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
                $.ajax({
                    type: "POST",
                    url: "/TradeIn/SaveTradeInStatus",
                    data: { status: value, id: idValue },
                    success: function (results) {
                        $.unblockUI();
                    }
                });
            });

            $("#btnPrint").click(function () {
                window.location = '<%= Url.Action("PrintTradeInCustomer", "PDF") %>';
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #notes
        {
            position: relative;
            top: -220px;
            left: 150px;
            width: 750px;
            color: white;
            z-index: 0;
        }

        #c2
        {
            width: 784px;
        }

        h4
        {
            margin-bottom: 0;
            margin-top: 0;
        }

        #sort
        {
            margin-top: -1.5em;
        }

        p
        {
            margin-top: 0;
        }

        #recent li
        {
            margin-left: -3px;
            margin-top: .3em;
            font-size: .9em;
        }

        #recent .sForm
        {
            margin: 0;
        }

            #recent .sForm[name="odometer"]
            {
            }

        img.thumb
        {
            float: left;
            clear: right;
        }

        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }

        #c2
        {
            border-right: none;
        }

        #table
        {
            width: 850px;
            padding: 0;
            margin: 0;
            font-size: .8em;
        }

            #table div
            {
                padding: 0;
                margin: 0;
            }

            #table .cell
            {
                width: 95px;
                height: 20px;
                overflow: hidden;
                margin-bottom: 3px;
                background: none;
            }

            #table .longest
            {
                width: 184px;
            }

            #table .long
            {
                width: 99px;
            }

            #table .short
            {
                width: 67px;
            }

            #table .shortest
            {
                width: 29px;
            }

            #table .infoCell
            {
                width: 674px;
            }

            #table .mid
            {
                width: 84px;
            }

        div.evenShorter
        {
            width: 15px !important;
            border: none;
        }

        #table div
        {
            margin-bottom: .5em;
        }

        .scroll-pane
        {
            height: 700px;
            overflow: auto;
            overflow-x: hidden;
        }

        #table
        {
            position: relative;
            left: .5em;
            font-size: 9.5pt;
            word-spacing: -1px;
            width: 750px;
        }

            #table .shorter
            {
                width: 55px;
            }

            #table .eC
            {
                width: 100px;
            }

        .rowOuter
        {
            margin-bottom: .5em !important;
            padding: .5em !important;
            padding-right: 0 !important;
            padding-bottom: .1em !important;
        }

        .light
        {
            background: #555;
        }

        .dark
        {
            background: #111;
        }

        #tableHeader .cell
        {
            border: none;
            border-bottom: 5px solid #990000;
            height: 20px !important;
            width: 97px;
            margin: 0;
            padding: 0;
            font-weight: bold;
            margin-right: 3px;
        }

        #tableHeader .longest
        {
            width: 184px;
        }

        #tableHeader .longer
        {
            width: 137px;
        }

        #tableHeader .long
        {
            width: 99px;
        }

        #tableHeader .short
        {
            width: 65px;
        }

        #tableHeader .shortest
        {
            width: 29px;
        }

        #tableHeader .infoCell
        {
            width: 674px;
        }

        #tableHeader .mid
        {
            width: 85px;
        }

        #tableHeader .shorter
        {
            width: 57px;
        }

        .cell
        {
            text-align: center;
            border-left: 2px solid #860000;
        }

        .noBorder
        {
            border: none !important;
        }

        #tableHeader .rowOuter
        {
            border: none !important;
        }

        #tableHeader .start
        {
            margin-left: 50px;
            width: 89px;
        }

        .dark .marketSection
        {
            background: #460000 !important;
        }

        .light .marketSection
        {
            background: #460000 !important;
        }

        .border
        {
            border: 3px red solid;
        }

        .med
        {
            border: 3px yellow solid;
        }

        input[name="price"]
        {
            background: green !important;
        }

        .hider
        {
            display: none;
        }

        .imageWrap .text
        {
            position: absolute;
            top: 8px; /* in conjunction with left property, decides the text position */
            left: 4px;
            width: 300px; /* optional, though better have one */
            color: Red;
        }

        .hideLoader
        {
            display: none;
        }

        #vehicle-list td, #vehicle-list th
        {
            padding: .1em .4em .1em .4em;
            border-bottom: .1em #bbbbbb solid;
        }

        #printable-list
        {
            display: block;
        }

        #vehicle-list thead tr td
        {
            font-weight: bold;
            color: black;
            border-bottom: #C80000 4px solid;
        }

        .graph-title-bar a
        {
            display: none;
        }

        .padding-right
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>