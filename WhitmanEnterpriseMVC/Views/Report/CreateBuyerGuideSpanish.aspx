<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AdminBuyerGuideViewModel>"  ValidateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Buyer's Guide</title>
    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ckeditor/ckeditor.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ckeditor/adapters/jquery.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {

             $.unblockUI();

            CKEDITOR.replace('SystemCovered',
            {
                height: 300,
                width: 450
            });

            CKEDITOR.replace('Durations',
            {
                height: 300,
                width: 450
            });

            CKEDITOR.replace('SystemCoveredAndDurations',
            {
                height: 300,
                width: 945
            });

            if ($("#IsAsWarranty").val() == "True")
                $("#checkboxWarrantyType").html('✔');

            if ($("#IsWarranty").val() == "True")
                $("#warranty").html('✔');

            if ($("#IsFullWarranty").val() == "True")
                $("#fullWarranty").html('✔');

            if ($("#IsLimitedWarranty").val() == "True")
                $("#limitedWarranty").html('✔');

            if ($("#IsServiceContract").val() == "True")
                $("#serviceContract").html('✔');

            if ($("#IsMixed").val() == "True") {
                $("#combineSystemCoveredAndDurations").html('✔');
                $("#areaSystemCovered").slideUp("slow");
                $("#areaDurations").slideUp("slow");
                $("#areaSystemCoveredAndDurations").slideDown("slow");
            }

            $("span#btnSave").click(function() {
                $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajax-loader1.gif") %>"  /></div>', css: { width: '350px', backgroundColor: 'none', border: 'none'} });

                //                $.ajax({
                //                    type: "POST",
                //                    url: "/Report/CreateBuyerGuide",
                //                    data: $("form").serialize(),
                //                    success: function(results) {
                //                         $.unblockUI();
                //                        alert(results);
                //                    }
                //                });
                $("#IsPreview").val('False');
                $("form").submit();
            });

            $("span#btnPreview").click(function() {
                $("#IsPreview").val('True');
                $("form").submit();
            });

            $("div#checkboxWarrantyType").click(function() {
                var isAswarranty = $("#IsAsWarranty").val();
                if (isAswarranty == "" || isAswarranty == "False") {
                    $("#checkboxWarrantyType").html('✔');
                    $("#IsAsWarranty").val('True');
                }
                else {
                    $("#checkboxWarrantyType").html('');
                    $("#IsAsWarranty").val('False');
                }
            });

            $("div#warranty").click(function() {
                var isWarranty = $("#IsWarranty").val();
                if (isWarranty == "" || isWarranty == "False") {
                    $("#warranty").html('✔');
                    $("#IsWarranty").val('True');
                }
                else {
                    $("#warranty").html('');
                    $("#IsWarranty").val('False');
                }
            });

            $("div#fullWarranty").click(function() {
                var isFullWarranty = $("#IsFullWarranty").val();
                if (isFullWarranty == "" || isFullWarranty == "False") {
                    $("#fullWarranty").html('✔');
                    $("#IsFullWarranty").val('True');
                }
                else {
                    $("#fullWarranty").html('');
                    $("#IsFullWarranty").val('False');
                }
            });

            $("div#limitedWarranty").click(function() {
                var isLimitedWarranty = $("#IsLimitedWarranty").val();
                if (isLimitedWarranty == "" || isLimitedWarranty == "False") {
                    $("#limitedWarranty").html('✔');
                    $("#IsLimitedWarranty").val('True');
                }
                else {
                    $("#limitedWarranty").html('');
                    $("#IsLimitedWarranty").val('False');
                }
            });

            $("div#serviceContract").click(function() {
                var isServiceContract = $("#IsServiceContract").val();
                if (isServiceContract == "" || isServiceContract == "False") {
                    $("#serviceContract").html('✔');
                    $("#IsServiceContract").val('True');
                }
                else {
                    $("#serviceContract").html('');
                    $("#IsServiceContract").val('False');
                }
            });

            $("div#combineSystemCoveredAndDurations").click(function() {
                var isMixed = $("#IsMixed").val();
                if (isMixed == "" || isMixed == "False") {
                    $("#combineSystemCoveredAndDurations").html('✔');
                    $("#IsMixed").val('True');
                    $("#areaSystemCovered").slideUp("slow");
                    $("#areaDurations").slideUp("slow");
                    $("#areaSystemCoveredAndDurations").slideDown("slow");
                }
                else {
                    $("#combineSystemCoveredAndDurations").html('');
                    $("#IsMixed").val('False');
                    $("#areaSystemCovered").slideDown("slow");
                    $("#areaDurations").slideDown("slow");
                    $("#areaSystemCoveredAndDurations").slideUp("slow");
                }
            });

            if ($("#Message").val() != "")
                alert($("#Message").val());

        });
</script>    
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/buy-guide.css")%>" />
    <style type="text/css">
    .checkbox, .sml-checkbox { cursor: pointer; }
    .submit {
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
        padding: 8px 2px;
    }
    </style>
</head>
<body>
<form id="buyerGuide" method="post" action="">
<input type="hidden" name="IsPreview" id="IsPreview" value="<%= Model.IsPreview %>" />
<input type="hidden" name="Message" id="Message" value="<%= Model.Message %>" />
<input type="hidden" name="WarrantyType" id="WarrantyType" value="<%= Model.WarrantyType %>" />
<input type="hidden" name="IsAsWarranty" id="IsAsWarranty" value="<%= Model.IsAsWarranty %>" />
<input type="hidden" name="IsManufacturerWarranty" id="IsManufacturerWarranty" value="<%= Model.IsManufacturerWarranty %>" />
<input type="hidden" name="IsDealerCertified" id="IsDealerCertified" value="<%= Model.IsDealerCertified %>" />
<input type="hidden" name="IsManufacturerCertified" id="IsManufacturerCertified" value="<%= Model.IsManufacturerCertified %>" />
<input type="hidden" name="IsWarranty" id="IsWarranty" value="<%= Model.IsWarranty %>" />
<input type="hidden" name="IsFullWarranty" id="IsFullWarranty" value="<%= Model.IsFullWarranty %>" />
<input type="hidden" name="IsLimitedWarranty" id="IsLimitedWarranty" value="<%= Model.IsLimitedWarranty %>" />
<input type="hidden" name="IsServiceContract" id="IsServiceContract" value="<%= Model.IsServiceContract %>" />
<input type="hidden" name="IsMixed" id="IsMixed" value="<%= Model.IsMixed %>" />
<input type="hidden" name="SelectedLanguage" id="SelectedLanguage" value="<%= Model.SelectedLanguage %>" />

<div class="wrapper" id="buyer-guide" style="margin-left:-110px;">
    <div style="margin-top: 5px; margin-bottom: 10px;">
        <span id="btnPreview" class="submit">Preview</span> <span id="btnSave" class="submit">
            Save Changes</span>
    </div>
  <div class="print-wrap">
    <div class="header">
      <h1>GUÍA DEL COMPRADOR</h1>
      <h3>IMPORTANTE: Las Promesas verbales son difíciles de hacer cumplir. Solicite al vendedor
                    que ponga todas las promesas por escrito. Conserve este formulario.</h3>
    </div>
    <div class="vehicle-info">
      <div class="item make">
        <div class="input">        
        <%= Html.TextBoxFor(m => m.Make, new { style = "border-bottom:none; width: 220px;" })%>
        </div>MARCA DEL VEHÍCULO
      </div>
      <div class="item model">
        <div class="input">
        <%= Html.TextBoxFor(m => m.VehicleModel, new { style = "border-bottom:none; width: 220px;" })%>        
        </div>MODELO
      </div>
      <div class="item year">
        <div class="input">
        <%= Html.TextBoxFor(m => m.Year, new { style = "border-bottom:none; width: 220px;" })%>
        </div>AÑO
      </div>
      <div class="item vin">
        <div class="input">
        <%= Html.TextBoxFor(m => m.Vin, new { style = "border-bottom:none; width: 220px;" })%>
        </div>NUMERO DE IDENTIFICACION
      </div>
      <div class="item stock">
        <div class="input">
        <%= Html.TextBoxFor(m => m.StockNumber, new { style = "border-bottom:none; width: 220px;" })%>
        </div>NÚMERO DE ABASTO DEL DISTRIBUIDOR (Opcional)
      </div>
    </div>
    <h3 style="font-size:0.5em;margin-bottom:10px;" class="warranty-header">GARANTÍAS PARA ESTE VEHÍCULO:</h3>
    <div class="as-is">
      <div class="checkbox" id="checkboxWarrantyType">      
      </div>
      <h3>COMO ESTÁ – SIN GARANTÍA</h3>
      <p style="margin-top:0px; font-size:0.4em"> USTED PAGA TODOS LOS GASTOS PARA UNA REPARACIÓN. El concesionario no asume ninguna
                    responsabilidad por cualquier reparación, independientemente de las declaraciones
                    orales sobre el vehículo.</p>
    </div>
    <div class="warranty">
      <div class="checkbox" id="warranty"></div>
      <h3>GARANTÍA</h3>
      <div class="warranty-type">
        <div class="sml-checkbox" id="fullWarranty"></div> <p style="font-size:0.3em">COMPLETA</p>
        <div class="sml-checkbox" id="limitedWarranty"></div> <p style="margin-top:0px; font-size:0.3em"> LIMITADA - El vendedor pagará
        <%= Html.TextBoxFor(m => m.PercentageOfLabor, new { style = "border-bottom:none; width: 50px;" })%>
        % de la mano de obra y 
        <%= Html.TextBoxFor(m => m.PercentageOfPart, new { style = "border-bottom:none; width: 50px;" })%>
        % de los repuestos de los sistemas cubiertos que dejen de funcionar durante el período
                        de garantía. Pida al vendedor una copia del documento de garantía donde se explican
                        detalladamente la cobertura de la garantía, exclusiones y las obligaciones que tiene
                        el vendedor de realizar reparaciones. Conforme a la ley estatal, las “garantías
                        implícitas” pueden darle a usted incluso más derechos.</p>
      </div>
      <div class="warranty-info">
        <div style="font-size: 0.4em; margin-top: 60px;" class="coverage two-column">
          <p>
          </p><h4>SISTEMAS CUBIERTOS POR LA GARANTÍA:</h4>          
          <div style="display: inline-block; height: 30px; width: 100%;">
          <div class="sml-checkbox" style="font-size:0.8em; width:20px; height: 20px;" id="combineSystemCoveredAndDurations"></div> 
          <p style="font-size:0.8em; margin-top: 5px;">COMBINE SYSTEMS COVERED & DURATIONS</p>
          </div>
          <div id="areaSystemCovered">
          <%= Html.TextAreaFor(m => m.SystemCovered, new { cols = "50" })%>
          </div>
          <%--<p></p>--%>
        </div>
        <div style="margin-top:60px; font-size:0.4em" class="duration two-column">
          <p>
          </p><h4>DURACIÓN:</h4>
          <div style="display: inline-block; height: 30px; width: 100%;"></div>
          <div id="areaDurations">
          <%= Html.TextAreaFor(m => m.Durations, new { cols = "50" })%>
          </div>
          <%--<p></p>--%>
        </div>
          
      </div>
        <div style="margin-top: -60px; margin-bottom: 50px; font-size: 0.4em; display: none;" id="areaSystemCoveredAndDurations">
            <%= Html.TextAreaFor(m => m.SystemCoveredAndDurations, new { cols = "120" })%>
        </div>
      <div style="float: right; display:inline-block; width: 100%;">
      <%= Html.TextBoxFor(m => m.PriorRental, new { style = "border-bottom:none; width: 240px; font-size:0.7em; text-transform:uppercase; float: right" })%>
      </div>
      <div style="margin-top:0px; font-size:0.4em" class="contract" >
        <div style="font-size:1.2em" class="sml-checkbox" id="serviceContract"></div> CONTRATO DE SERVICIO. Este vehículo tiene disponible un contrato de servicio a un
                    precio adicional. Pida los detalles en cuanto a cobertura, deducible, precio y exclusiones.
                    Si adquiere usted un contrato de servicio dentro de los 90 días del momento de la
                    venta, las “garantías implícitas” de acuerdo a la ley del estado pueden concederle
                    derechos adicionales.
        <br/><br/>
        INSPECCIÓN PREVIA A LA COMPRA: PREGUNTE AL VENDEDOR SI PUEDE USTED TRAER UN MECÁNICO
                    PARA QUE INSPECCIONE EL AUTOMÓVIL O LLEVAR EL AUTOMÓVIL PARA QUE ESTE LO INSPECCIONE
                    EN SU TALLER.
        <br/><br/>
        VéASE EL DORSO DE ESTE FORMULARIO donde se proporciona información adicional importante,
                    incluyendo una lista de algunos de los principales defectos que pueden ocurrir en
                    vehículos usados.
      </div>
    </div>
  </div>
</div>
</form>

</body></html>
