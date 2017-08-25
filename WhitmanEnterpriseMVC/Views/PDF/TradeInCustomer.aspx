<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WhitmanEnterpriseMVC.Models.TradeinCustomerViewModel>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>TradeInCustomer</title>
     <style type="text/css">
        table#vehicle-list
        {
            font-size: .7em;
        }
        #vehicle-list td
        {
            padding: .3em .7em .3em .7em;
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
</head>
<body>
 <div class="graph-title-bar">
        <h1 style="font-size:40px; text-align:center;">
            Trade in Customer Report
        </h1>
    </div>
    <div id="printable-list">
        <div id="vehicle-list" style="font-size: 12pt">  
    
    <table>
    <thead align="left" style="display: table-header-group">
        <tr>
          
            <td>
                First Name
            </td>
            <td>
                Last Name
            </td>
            <td>
                Email
            </td>
            <td class="padding-right">
                Phone
            </td>
            <td class="padding-right">
                Mileage
            </td>
            <td class="padding-right">
                Date
            </td>
            <td class="padding-right">
                Year
            </td>
            <td>
                Make
            </td>
            <td>
                Model
            </td>
            <td>
                Condition
            </td>
            <td>
                Status
            </td>
           
        </tr>
</thead>
    <% foreach (var item in Model) { %>
    
        <tr>
           
            <td>
                <%= Html.Encode(item.FirstName) %>
            </td>
            <td>
                <%= Html.Encode(item.LastName) %>
            </td>
            <td>
                <%= Html.Encode(item.Email) %>
            </td>
            <td class="padding-right">
                <%= Html.Encode(item.Phone) %>
            </td>
            <td class="padding-right">
                <%= Html.Encode(item.MileageAdjustment) %>
            </td>
            <td class="padding-right">
                <%= Html.Encode(item.Date) %>
            </td>
            <td class="padding-right">
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
                <%= Html.Encode(item.TradeInStatus) %>
            </td>
           
        </tr>
    
    <% } %>

    </table>

   </div>
   </div>
</body>
</html>

