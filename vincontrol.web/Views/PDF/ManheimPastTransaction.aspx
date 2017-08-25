<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.ManheimReport>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Manheim Past Transaction Report</title>
    <style type="text/css">
        html {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            background: #e5eaf0;
            color: #000;  
            font-size: 13px;          
        }
        body {
            
            margin: 0 auto;
        }
        table.tablesorter {
            font-family: arial;
            background-color: #CDCDCD;
            /*margin:10px 0pt 15px;*/
            font-size: 8pt;
            width: 100%;
            text-align: left;
        }
            table.tablesorter thead tr th, table.tablesorter tfoot tr th {
                background-color: #e6EEEE;
                border: 1px solid #FFF;
                font-size: 8pt;
                padding: 4px;
            }

            table.tablesorter thead tr .header {
                background-image: url(images/bg.gif);
                background-repeat: no-repeat;
                background-position: center right;
                cursor: pointer;
            }

            table.tablesorter tbody td {
                color: #3D3D3D;
                padding: 4px;
                background-color: #FFF;
                vertical-align: top;
            }

            table.tablesorter tbody tr.odd td {
                background-color: #F0F0F6;
            }

            table.tablesorter thead tr .headerSortUp {
                background-image: url(images/asc.gif);
            }

            table.tablesorter thead tr .headerSortDown {
                background-image: url(images/desc.gif);
            }

            table.tablesorter thead tr .headerSortDown, table.tablesorter thead tr .headerSortUp {
                background-color: #8dbdd8;
            }

        .popup_title {
            background-color: #3366cc;
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 10px;
            /*padding: 5px 10px;*/
            color: white;
            text-align: left !important;
        }
    </style>
</head>
<body>
    <div id="container">

        <div>
        </div>
        <hr />

        <div>

            <div>
                <table id="manheimTransaction" class="tablesorter" style="display: inline-block; width: 755px;">
                    <thead style="cursor: pointer;">
                        <tr>
                            <th align="left" width="60px">Sale date
                            </th>
                            <th align="left" width="60px">Price
                            </th>
                            <th align="left" width="60px">Odo.
                            </th>
                            <th align="left" width="80px">Vin
                            </th>
                            <th align="left" width="70px">Engine
                            </th>
                            <th align="left" width="70px">Color
                            </th>
                            <th align="left" width="140px">Auction
                            </th>
                            <th align="left" width="100px">Region
                            </th>
                        </tr>
                    </thead>
                    <tbody id="result">
                        <%if (Model.ManheimTransactions.Any())
                          {%>
                        <% foreach (var item in Model.ManheimTransactions)
                           {%>
                        <tr>

                            <td><%= item.SaleDate %>
                            </td>
                            <td align="left">
                                <%= item.Price %>
                            </td>
                            <td align="left">
                                <%= item.Odometer %>
                            </td>

                            <td>
                                <%= item.Vin %>
                            </td>
                            <td align="left">
                                <%= item.Engine %>
                            </td>
                            <td>
                                <%= item.Color %>
                            </td>
                            <td>
                                <%= item.Auction %>
                            </td>
                            <td>
                                <%= item.Region %>
                            </td>
                        </tr>
                        <% } %>
                        <%}
                          else
                          {%><%} %>
                    </tbody>
                </table>
            </div>
        </div>

    </div>
</body>
</html>
