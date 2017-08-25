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
                <h1>
                    GUÍA DEL COMPRADOR</h1>
                <h3>
                    IMPORTANTE: Las Promesas verbales son difíciles de hacer cumplir. Solicite al vendedor
                    que ponga todas las promesas por escrito. Conserve este formulario.</h3>
            </div>
            <div class="vehicle-info">
                <div class="item make">
                    <div class="input">
                        <%= item.Make%>
                    </div>
                    MARCA DEL VEHÍCULO
                </div>
                <div class="item model">
                    <div class="input">
                        <%= item.VehicleModel%>
                    </div>
                    MODELO
                </div>
                <div class="item year">
                    <div class="input">
                        <%= item.Year%>
                    </div>
                    AÑO
                </div>
                <div class="item vin">
                    <div class="input">
                        <%= item.Vin%>
                    </div>
                    NUMERO DE IDENTIFICACION
                </div>
                 <div class="item stock" style="width: 900px">
                    <div class="input">
                        <%= item.StockNumber%>
                    </div>
                    NÚMERO DE ABASTO DEL DISTRIBUIDOR (Opcional)
                </div>
            </div>
            <h3 class="warranty-header">
                GARANTÍAS PARA ESTE VEHÍCULO:</h3>
            <div class="as-is">
                <div class="checkbox">
                    <%if (item.IsAsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3 style="font-size: 3.0em">COMO ESTÁ – SIN GARANTÍA</h3>
                <p>
                    USTED PAGA TODOS LOS GASTOS PARA UNA REPARACIÓN. El concesionario no asume ninguna
                    responsabilidad por cualquier reparación, independientemente de las declaraciones
                    orales sobre el vehículo.</p>
            </div>
            <div class="warranty">
                <div class="checkbox">
                    <%if (item.IsWarranty)
                      {%>
                    ✔
                    <%}%>
                </div>
                <h3 style="font-size: 3.0em">
                    GARANTÍA</h3>
                <div class="warranty-type" style="width: 100%;">
                    <div class="sml-checkbox">
                        <%if (item.IsFullWarranty)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    <p>
                        COMPLETA</p>
                    <div class="sml-checkbox" id="Div3">
                        <%if (item.IsLimitedWarranty)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    <p style="width: 1650px;">
                        LIMITADA - El vendedor pagará
                        <%= item.PercentageOfLabor%>
                        % de la mano de obra y
                        <%= item.PercentageOfPart%>
                        % de los repuestos de los sistemas cubiertos que dejen de funcionar durante el período
                        de garantía. Pida al vendedor una copia del documento de garantía donde se explican
                        detalladamente la cobertura de la garantía, exclusiones y las obligaciones que tiene
                        el vendedor de realizar reparaciones. Conforme a la ley estatal, las “garantías
                        implícitas” pueden darle a usted incluso más derechos.
                    </p>
                </div>
                  <%if (item.IsPriorRental)
                    {%>
               <div class="warranty-info" style="min-height: 920px; max-height: 1200px; margin-top: 50px;">
                    <%}
                    else
                    {%>
               <div class="warranty-info" style="min-height: 1000px; max-height: 1200px; margin-top: 50px;">     
                    <% } %>
                    <div class="coverage two-column">
                          <h4 style="font-size: 1.3em">
                            SISTEMAS CUBIERTOS POR LA GARANTÍA:</h4>
                        <%if (!(String.IsNullOrEmpty(item.SystemCovered)) && !item.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= item.SystemCovered%></div>
                        <%}%>
                    </div>
                    <div class="coverage two-column">
                          <h4 style="font-size: 1.3em">
                            DURACIÓN:</h4>
                        <%if (!(String.IsNullOrEmpty(item.Durations)) && !item.IsMixed)
                          {%>
                        <div style="margin-top: 10px;">
                            <%= item.Durations%></div>
                        <%}%>
                    </div>
                    <%if (item.IsMixed)
                      {%>
                    <div style="top: 0px; margin-bottom: 50px; width: 97%; display: inline-block;">
                        <%= item.SystemCoveredAndDurations%>
                    </div>
                    <%}%>
                </div>
                <%if (item.IsPriorRental)
                  {%>
                <div style="float: right; display: inline-block; width: 100%;">
                    <div style="font-size: 1.7em; font-weight: bold; text-transform: uppercase; float: right">
                        ANTES DE ALQUILER</div>
                </div>
                <%}%>
                    <div class="contract" style="font-size: .85em">
                    <div class="sml-checkbox">
                        <%if (item.IsServiceContract)
                          {%>
                        ✔
                        <%}%>
                    </div>
                    CONTRATO DE SERVICIO. Este vehículo tiene disponible un contrato de servicio a un
                    precio adicional. Pida los detalles en cuanto a cobertura, deducible, precio y exclusiones.
                    Si adquiere usted un contrato de servicio dentro de los 90 días del momento de la
                    venta, las “garantías implícitas” de acuerdo a la ley del estado pueden concederle
                    derechos adicionales.
                    <br />
                    <br />
                    INSPECCIÓN PREVIA A LA COMPRA: PREGUNTE AL VENDEDOR SI PUEDE USTED TRAER UN MECÁNICO
                    PARA QUE INSPECCIONE EL AUTOMÓVIL O LLEVAR EL AUTOMÓVIL PARA QUE ESTE LO INSPECCIONE
                    EN SU TALLER.
                    <br />
                    <br />
                    VéASE EL DORSO DE ESTE FORMULARIO donde se proporciona información adicional importante,
                    incluyendo una lista de algunos de los principales defectos que pueden ocurrir en
                    vehículos usados.
                </div>
            </div>
        </div>
    </div>
    <h1 class="break"></h1>
     <%
      }
          %>
    
 
</body>
</html>
