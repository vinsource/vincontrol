<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AdminBuyerGuideViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            height: 3330px;
            position: relative;
            left: 100px;
            font-size: 2.5em;
            overflow: hidden;
             padding-right: 40px
        }
        .print-wrap
        {
            margin: 75px;
        }
        .header
        {
            width: 100%;
            text-align: center;
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
            font-size: 4.5em;
            position: relative;
            top: -25px;
        }
        .checkbox
        {
            width: 150px;
            height: 150px;
            border: 5px solid black;
            float: left;
            margin-left: 75px;
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
    </style>
</head>
<body>
    
    <%if (Model.SelectedLanguage == 1) {%>
    <div class="wrapper" id="buyer-guide">
        <div class="print-wrap">
            <div class="header">
                <h1>Buyers Guide</h1>
                <h3>IMPORTANT: Spoken promises are difficult to enforce. Ask the dealer to put all promises in writing. Keep this form.</h3>
            </div>
            <div class="vehicle-info">
                <div class="item make">
                    <div class="input">
                        <%= Model.Make%>
                    </div>
                    Vehicle Make
                </div>
                <div class="item model">
                    <div class="input">
                        <%= Model.VehicleModel%>
                    </div>
                    Model
                </div>
                <div class="item year">
                    <div class="input">
                        <%= Model.Year %>
                    </div>
                    Year
                </div>
                <div class="item vin">
                    <div class="input">
                        <%= Model.Vin%>
                    </div>
                    VIN Number
                </div>
                <div class="item stock">
                    <div class="input">
                        <%= Model.StockNumber%>
                    </div>
                    Dealer Stock Number (Optional)
                </div>
            </div>
            <h3 class="warranty-header">
                Warranties for this vehicle:</h3>
            <div class="as-is">
                <div class="checkbox">
                    <%if (Model.IsAsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3>AS IS - NO WARRANTY</h3>
                <p>
                    YOU WILL PAY ALL COSTS FOR ANY REPAIRS. The dealer assumes no responsibility for
                    any repairs regardless of any oral statements about the vehicle.</p>
            </div>
            <div class="warranty">
                <div class="checkbox">
                    <%if (Model.IsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3>WARRANTY</h3>
                <div class="warranty-type">
                    <div class="sml-checkbox">
                        <%if (Model.IsFullWarranty)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    <p>
                        FULL</p>
                    <div class="sml-checkbox" id="limitedWarranty">
                        <%if (Model.IsLimitedWarranty)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    <p>
                        LIMITED WARRANTY - The dealer will pay
                        <%= Model.PercentageOfLabor%>
                        % of the labor and
                        <%= Model.PercentageOfPart%>
                        % of the parts for the covered systems that fail during the warranty period. Ask
                        the dealer for a copy of the warranty document for full explanation of warranty
                        coverage, exclusions, and the dealer's repair obligations. Under state law, 'implied
                        warranties' may give you even more rights.</p>
                </div>
                   <div class="warranty-info" style="height: 1150px; margin-top: 50px;">
                    <div class="coverage two-column">
                        <h4>SYSTEMS COVERED:</h4>
                        <%if (!(String.IsNullOrEmpty(Model.SystemCovered)) && !Model.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= Model.SystemCovered %></div>
                        <%}%>
                    </div>
                    <div class="coverage two-column">
                        <h4>DURATION:</h4>
                        <%if (!(String.IsNullOrEmpty(Model.Durations)) && !Model.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= Model.Durations %></div>
                        <%}%>
                    </div>
                    <%if (Model.IsMixed)
                      {%>
                    <div style="top: 0px; margin-bottom: 50px; width: 97%;">
                        <%= Model.SystemCoveredAndDurations %>
                    </div>
                    <%}%>
                </div>
                <%if (Model.IsPriorRental)
                  {%>
                <div style="float: right; display: inline-block; width: 100%;">
                    <div style="font-size: 1.7em; font-weight: bold; text-transform: uppercase; float: right">
                        <%= Model.PriorRental%></div>
                </div>
                <%}%>
                       <div class="contract" style="font-size: .85em">
                    <div class="sml-checkbox">
                        <%if (Model.IsServiceContract)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    SERVICE CONTRACT. A service contract is available at an extra charge on this vehicle.
                    Ask for details as to coverage, deductible, price and exclusions. If you buy a service
                    contract within 90 days of the tiem of sale, state law 'implied warranties' may
                    give you additional rights.
                    <br />
                    <br />
                    PRE PURCHASE INSPECTION. ASK THE DEALER IF YOU MAY HAVE THIS VEHICLE INSPECTED BY
                    YOUR MECHANIC EITHER ON OR OFF THE LOT.
                    <br />
                    <br />
                    SEE THE BACK OF THIS FORM for important additional information, including a list
                    of some major defects that may occur in used motor vehicles.
                </div>
            </div>
        </div>
    </div>
    <%}else if (Model.SelectedLanguage == 2) {%>
    <div class="wrapper" id="buyer-guide">
        <div class="print-wrap">
            <div class="header">
                <h1>GUÍA DEL COMPRADOR</h1>
                <h3>IMPORTANTE: Las Promesas verbales son difíciles de hacer cumplir.  Solicite al vendedor que ponga todas las promesas por escrito.  Conserve este formulario.</h3>
            </div>
            <div class="vehicle-info">
                <div class="item make">
                    <div class="input">
                        <%= Model.Make%>
                    </div>
                    MARCA DEL VEHÍCULO
                </div>
                <div class="item model">
                    <div class="input">
                        <%= Model.VehicleModel%>
                    </div>
                    MODELO
                </div>
                <div class="item year">
                    <div class="input">
                        <%= Model.Year %>
                    </div>
                    AÑO
                </div>
                <div class="item vin">
                    <div class="input">
                        <%= Model.Vin%>
                    </div>
                    NUMERO DE IDENTIFICACION
                </div>
                <div class="item stock">
                    <div class="input">
                        <%= Model.StockNumber%>
                    </div>
                    NÚMERO DE ABASTO DEL DISTRIBUIDOR (Opcional)
                </div>
            </div>
            <h3 class="warranty-header">
                GARANTÍAS PARA ESTE VEHÍCULO:</h3>
            <div class="as-is">
                <div class="checkbox">
                    <%if (Model.IsAsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3>COMO ESTÁ – SIN GARANTÍA</h3>
                <p>
                    USTED PAGA TODOS LOS GASTOS PARA UNA REPARACIÓN.  El concesionario no asume ninguna responsabilidad por cualquier reparación, independientemente de las declaraciones orales sobre el vehículo.</p>
            </div>
            <div class="warranty">
                <div class="checkbox">
                    <%if (Model.IsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3>GARANTÍA</h3>
                <div class="warranty-type">
                    <div class="sml-checkbox">
                        <%if (Model.IsFullWarranty)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    <p>
                        COMPLETA</p>
                    <div class="sml-checkbox" id="Div2">
                        <%if (Model.IsLimitedWarranty)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    <p>
                        LIMITADA - El vendedor pagará
                        <%= Model.PercentageOfLabor%>
                        % de la mano de obra y
                        <%= Model.PercentageOfPart%>
                        % de los repuestos de los sistemas cubiertos que dejen de funcionar durante el período
                        de garantía. Pida al vendedor una copia del documento de garantía donde se explican
                        detalladamente la cobertura de la garantía, exclusiones y las obligaciones que tiene
                        el vendedor de realizar reparaciones. Conforme a la ley estatal, las “garantías
                        implícitas” pueden darle a usted incluso más derechos.
                    </p>
                </div>
                    <div class="warranty-info" style="height: 1150px; margin-top: 50px;">
                    <div class="coverage two-column">
                        <h4>SISTEMAS CUBIERTOS POR LA GARANTÍA:</h4>
                        <%if (!(String.IsNullOrEmpty(Model.SystemCovered)) && !Model.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= Model.SystemCovered %></div>
                        <%}%>
                    </div>
                    <div class="coverage two-column">
                        <h4>DURACIÓN:</h4>
                        <%if (!(String.IsNullOrEmpty(Model.Durations)) && !Model.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= Model.Durations %></div>
                        <%}%>
                    </div>
                    <%if (Model.IsMixed)
                      {%>
                    <div style="top: 0px; margin-bottom: 50px; width: 97%;">
                        <%= Model.SystemCoveredAndDurations %>
                    </div>
                    <%}%>
                </div>
                <%if (Model.IsPriorRental)
                  {%>
                <div style="float: right; display: inline-block; width: 100%;">
                    <div style="font-size: 1.7em; font-weight: bold; text-transform: uppercase; float: right">
                        ANTES DE ALQUILER</div>
                </div>
                <%}%>
                        <div class="contract" style="font-size: .85em">
                    <div class="sml-checkbox">
                        <%if (Model.IsServiceContract)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    CONTRATO DE SERVICIO. Este vehículo tiene disponible un contrato de servicio a un precio adicional. Pida 
los detalles en cuanto a cobertura, deducible, precio y exclusiones. Si adquiere usted un contrato de servicio 
dentro de los 90 días del momento de la venta, las “garantías implícitas” de acuerdo a la ley del estado pueden 
concederle derechos adicionales.
                    <br />
                    <br />
                    INSPECCIÓN PREVIA A LA COMPRA: PREGUNTE AL VENDEDOR SI PUEDE USTED TRAER UN MECÁNICO 
PARA QUE INSPECCIONE EL AUTOMÓVIL O LLEVAR EL AUTOMÓVIL PARA QUE ESTE LO INSPECCIONE EN 
SU TALLER.
                    <br />
                    <br />
                    VéASE EL DORSO DE ESTE FORMULARIO donde se proporciona información adicional importante, 
incluyendo una lista de algunos de los principales defectos que pueden ocurrir en vehículos usados.
                </div>
            </div>
        </div>
    </div>
    <%}%>
</body>
</html>
