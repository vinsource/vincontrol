<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Vincontrol.Web.Models.AdminBuyerGuideViewModel>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Buyer's Guide</title>
    <style type="text/css">
        html
        {
            font-family: Arial, sans-serif !important;
        }
        .wrapper
        {
            margin: 0 auto;
            width: 2600px;
            min-height: 3330px;
            position: relative;
            left: 100px;
            font-size: 2.3em;
            overflow: hidden;
            padding-right: 50px
        }
        .print-wrap
        {
            margin: 90px;
        }
        .header
        {
            width: 100%;
            text-align: center;
            margin-top: 50px;
        }
        .header h1
        {
            margin-bottom: 0;
            font-size: 4em;
            width: 100%;
            display: inline-block;
            border-bottom: 7px black solid;
            text-transform: uppercase;
        }
        .header h3
        {
            margin-top: 0;
            font-size: 1.1em;
        }
        .vehicle-info .item
        {
            display: inline-block;
            width: 24%;
            text-transform: uppercase;
            font-style: italic;
        }
        .vehicle-info .input
        {
            margin-top: 75px;
            border-bottom: 5px solid black;
            font-style: normal;
            font-size: 1.1em;
        }
        .warranty-header
        {
            text-transform: uppercase;
            margin-top: 75px;
            display: block;
            padding-bottom: 0;
            margin-bottom: 25px;
        }
        .as-is
        {
            padding-top: 75px;
            border-top: 7px solid black;
            border-bottom: 7px solid black;
        }
        .as-is h3, .warranty h3
        {
            margin: 0;
            padding: 0;
            display: inline-block;
            /*font-size: 4.5em;*/
            font-size: 140px;
            position: relative;
            /*top: -25px;*/
        }
        .checkbox
        {
            width: 150px;
            height: 150px;
            border: 5px solid black;
            float: left;
            /*margin-left: 75px;*/
            margin-right: 50px;
            text-align: center;
            font-size: 3.2em;
            font-weight: bold;
            padding: 0;
        }
        .warranty
        {
            padding-top: 75px;
        }
        .sml-checkbox
        {
            width: 50px;
            height: 50px;
            float: left;
            border: 5px solid black;
            font-weight: bold;
            text-align: center;
            margin-right: 20px;
        }
        .warranty-type p
        {
            margin-top: 0;
            margin-right: 55px;
            float: left;
            max-width: 85%;
        }
        .warranty-info
        {
            clear: both;
            overflow: hidden;
        }
        .warranty-info h4
        {
            margin-bottom: 10px;
            font-size: 1.4em;
        }
        .coverage, .duration
        {
            position: relative;
            top: -75px;
        }
        .warranty-info .two-column
        {
            padding: 0;
            float: left;
            width: 45%;
            margin-right: 5%;
            margin-left: 0;
            padding: 0;
        }
        .checkbox, .sml-checkbox
        {
            cursor: pointer;
        }
        .break { page-break-before: always; }
    </style>
</head>
<body>
 
    <%foreach (var item in Model)
      {
          %>
          
    <div class="wrapper" id="buyer-guide">
        <div class="print-wrap">
            <div class="header">
               
                <h1>Buyers Guide</h1>
                <h3>IMPORTANT: Spoken promises are difficult to enforce. Ask the dealer to put all promises in writing. Keep this form.</h3>
            </div>
            <div class="vehicle-info">
                <div class="item make">
                    <div class="input">
                        <%= item.Make%>
                    </div>
                    Vehicle Make
                </div>
                <div class="item model">
                    <div class="input">
                        <%= item.VehicleModel%>
                    </div>
                    Model
                </div>
                <div class="item year">
                    <div class="input">
                        <%= item.Year%>
                    </div>
                    Year
                </div>
                <div class="item vin">
                    <div class="input">
                        <%= item.Vin%>
                    </div>
                    VIN Number
                </div>
                <%--<div class="item stock" style="width: 900px">
                    <div class="input">
                        <%= item.StockNumber%>
                    </div>
                    Dealer Stock Number (Optional)
                </div>--%>
            </div>
            <h3 class="warranty-header">
                Warranties for this vehicle:</h3>
            <div class="as-is" style="border-bottom: none;">
                <div class="checkbox" style="margin-left: 0px;">
                    <%if (item.IsAsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3>AS IS - NO DEALER WARRANTY</h3>
                <p style="margin-left: 200px;">THE DEALER DOES NOT PROVIDE A WARRANTY FOR ANY REPAIRS AFTER SALE.</p>
                <hr style="border-style: dashed; margin-left: 200px;" />
            </div>
            <div class="warranty">
                <div class="checkbox" style="margin-left: 0px;">
                    <%if (item.IsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3>DEALER WARRANTY</h3>
                <div class="warranty-type" style="margin-top: 20px;">
                    <div style="display: inline-block; margin-left: 100px;">
                        <div class="sml-checkbox">
                            <%if (item.IsFullWarranty)
                                {%>
                        ✔
                        <%}%>
                        </div>
                        <p style="margin-left: 40px;">FULL WARRANTY.</p>
                    </div>
                    <div style="display: inline-block; margin-left: 100px;">
                        <div class="sml-checkbox" id="limitedWarranty">
                            <%if (item.IsLimitedWarranty)
                                {%>
                        ✔
                        <%}%>
                        </div>
                        <p style="max-width: 90%; margin-left: 40px;">
                            LIMITED WARRANTY - The dealer will pay
                        <%= item.PercentageOfLabor%>
                        % of the labor and
                        <%= item.PercentageOfPart%>
                        % of the parts for the covered systems that fail during the warranty period. Ask
                        the dealer for a copy of the warranty document for full explanation of warranty
                        coverage, exclusions, and the dealer's repair obligations. Under state law, 'implied
                        warranties' may give you even more rights.
                        </p>
                    </div>                    
                </div>
                  <%--<div class="warranty-info" style="min-height: 1150px;  max-height: 1250px; margin-top: 50px;">--%>
                <div class="warranty-info" style="min-height: 650px;  max-height: 750px; margin-top: 50px;">    
                    <div class="coverage two-column">
                        <h4 style="font-size: 1.3em">
                            SYSTEMS COVERED:</h4>
                        <%if (!(String.IsNullOrEmpty(item.SystemCovered)) && !item.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= item.SystemCovered %></div>
                        <%}%>
                    </div>
                    <div class="coverage two-column">
                        <h4 style="font-size: 1.3em">
                            DURATION:</h4>
                        <%if (!(String.IsNullOrEmpty(item.Durations)) && !item.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= item.Durations %></div>
                        <%}%>
                    </div>
                    <%if (item.IsMixed)
                      {%>
                    <div style="top: 0px; margin-bottom: 50px; width: 97%;">
                        <%= item.SystemCoveredAndDurations %>
                    </div>
                    <%}%>

                </div>
                <%if (item.IsPriorRental)
                  {%>
                <div style="float: right; display: inline-block; width: 100%;">
                    <div style="font-size: 1.7em; font-weight: bold; text-transform: uppercase; float: right">
                        <%= item.PriorRental%></div>
                </div>
                <%}%>
                     <%if (item.IsBrandedTitle)
                  {%>
                <div style="float: right; display: inline-block; width: 100%;">
                    <div style="font-size: 1.7em; font-weight: bold; text-transform: uppercase; float: right">
                        Branded Title</div>
                </div>
                <%}%>
                
                <div>
                    <div class="warranty-header">NON-DEALER WARRANTIES FOR THIS VEHICLE:</div>
                    <hr style="border: 4px solid;" />
                    <div class="contract" style="font-size: .85em; display: inline-block; margin-bottom: 10px; width: 100%;">
                        <div class="sml-checkbox">
                            <%if (item.IsManufacturerWarranty)
                          {%>
                        ✔
                        <%}%>
                        </div>
                        MANUFACTURER’S WARRANTY STILL APPLIES.  The manufacturer’s original warranty has not expired on some components of the vehicle.
                    </div>
                    <div class="contract" style="font-size: .85em; display: inline-block; margin-bottom: 10px; width: 100%;">
                        <div class="sml-checkbox">
                            <%if (item.IsManufacturerUsedVehicleWarranty)
                          {%>
                        ✔
                        <%}%>
                        </div>
                        MANUFACTURER’S USED VEHICLE WARRANTY APPLIES.
                    </div>
                    <div class="contract" style="font-size: .85em; display: inline-block; margin-bottom: 10px; width: 100%;">
                        <div class="sml-checkbox">
                            <%if (item.IsOtherWarranty)
                          {%>
                        ✔
                        <%}%>
                        </div>
                        OTHER USED VEHICLE WARRANTY APPLIES.
                    </div>
                    <div class="contract" style="font-size: .85em">
                        Ask the dealer for a copy of the warranty document and an explanation of warranty coverage, exclusions, and repair obligations.
                    </div>
                </div>
                <hr style="border: 2px solid;"/>
                <div class="contract" style="font-size: .85em">
                    <div class="sml-checkbox">
                        <%if (item.IsServiceContract)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    SERVICE CONTRACT.  service contract on this vehicle is available for an extra charge.  Ask for details about coverage, deductible, price, and exclusions.  If you buy a service contract within 90 days of your purchase of this vehicle, implied warranties under your state’s laws may give you additional rights.                    
                </div>
                <hr style="border: 4px solid;"/>
            <p style="font-weight: 700; font-size: 30px; display: inline-block;">ASK THE DEALER IF YOUR MECHANIC CAN INSPECT THE VEHICLE ON OR OFF THE LOT.</p>
                <div>
                    <font style="font-weight: 700; font-size: 30px;">OBTAIN A VEHICLE HISTORY REPORT AND CHECK FOR OPEN SAFETY RECALLS.</font> For information on how to obtain a vehicle history report, visit ftc.gov/usedcars. To check for open safety recalls, visit safercar.gov. You will need the vehicle identification number (VIN) shown above to make the best use of the resources on these sites.
                </div>
                <p style="font-weight: 700; font-size: 30px; display: inline-block;">SEE OTHER SIDE for important additional information, including a list of major defects that may occur in used motor vehicles.</p>
            </div>
        </div>
    </div>
    <h1 class="break"></h1>
     <%
      }
          %>
    
 
</body>
</html>
