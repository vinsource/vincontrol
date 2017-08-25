<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.ManheimReport>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Manheim Transaction Report</title>
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
        
        <div class="popup_title">
        <% if (!Model.IsAuction) {%>
        <div style="display: inline-block;width:100%;">
            <strong class="reportHeader"><%= Model.Region %></strong><br />
            <strong class="reportHeader">REPORTED WHOLESALE AUCTION SALES - With Exact Matches</strong>            
        </div>
        <%} %>
        </div>
                        
        <div>
            <% if (!Model.IsAuction) {%>
            <div style="float:left;width:130px;display:inline-block;border:1px solid black;padding:4px;">
                <div>
                High Price:	<b><%= Model.HighestPrice %></b><br />
                Low Price: <b><%= Model.LowestPrice %></b><br />
                Avg Price: <b><%= Model.AveragePrice %></b><br />
                Avg Odo: <b><%= Model.AverageOdometer %></b><br />
                # of Vehicles: <b><%= Model.NumberOfTransactions %></b>
                </div>                
            </div>
            <%} %>
            <div>
            <table id="manheimTransaction" class="tablesorter" style="display:inline-block;width:605px;">
                <thead style="cursor: pointer;">
                    <tr>
                        <th align="left" width="80px">Sale date
                        </th>
                        <th align="left" width="100px">Auction
                        </th>
                        <th align="left" width="75px">Odometer
                        </th>
                        <th align="left" width="75px">Price
                        </th>
                        <th align="left" width="50px">Engine
                        </th>
                        <th align="left" width="50px">Cond
                        </th>
                        <th align="left" width="75px">Color
                        </th>
                        <th align="left" width="75px">In Sample
                        </th>
                    </tr>
                </thead>
                <tbody id="result">
                    <%if (Model.ManheimTransactions.Any(i => i.Price != "0" && i.Odometer != "0"))
                      {%>
                    <% foreach (var item in Model.ManheimTransactions.Where(i => i.Price != "0" && i.Odometer != "0"))
                       {%>
                    <tr>                        
                        <td>
                            <%= item.SaleDate %>
                        </td>
                        <td>
                            <%= item.Auction %>
                        </td>
                        <td>
                            <%= item.Odometer %>
                        </td>
                        <td>
                            <%= item.Price %>
                        </td>
                        <td>
                            <%= item.Engine %>
                        </td>
                        <td>
                            <%= item.Cond %>
                        </td>
                        <td>
                            <%= item.Color %>
                        </td>
                        <td>
                            <%= item.Sample %>
                        </td>
                    </tr>
                    <% } %>
                    <%}
                      else
                      {%>
                    <%} %>
                </tbody>
            </table>        
            </div>
        </div>
        
    </div>
</body>
</html>
