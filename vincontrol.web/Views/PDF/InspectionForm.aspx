<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InspectionAppraisalViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>InspectionForm</title>
    <style type="text/css">
        body, html
        {
            margin: 0;
            padding: 0;
            font-family: Helvetica, Arial, sans-serif;
        }
        .clear
        {
            clear: both !important;
        }
        .no-right-border
        {
            border-right: none !important;
            margin-right: 1px;
        }
        div.wrapper
        {
            overflow: hidden;
            width: 1163px;
            height: 1500px;
            margin: 0 auto;
            border: 1px solid black;
            position: relative;
        }
        div.header
        {
            width: 100%;
            text-align: center;
        }
        div.col
        {
            float: left;
            clear: right;
            height: 100%;
            overflow: hidden;
        }
        div#col-one
        {
            width: 918px;
        }
        div#col-two
        {
            width: /*25%*/ 245px;
            border-top: 1px solid black;
            font-size: 1em;
        }
        div.info-box
        {
            width: 301px;
            float: left;
            clear: right;
            padding: 2px;
            font-size: 1em;
            border-bottom: 1px black solid;
            border-right: 1px black solid;
        }
        div.info-box.top
        {
            border-top: 1px black solid;
        }
        div.info-box.last
        {
            width: 913px;
        }
        ul.vin
        {
            margin: 0;
            padding: 0;
            list-style-type: none;
        }
        ul.vin li
        {
            width: 53px;
            height:20px;
            float: left;
            clear: right;
            border-bottom: 1px solid black;
            border-right: 1px solid black;
            text-align: center;
            padding-top: .5em;
            padding-bottom: .5em;
        }
        ul.list
        {
            margin: 0;
            padding: 0;
            list-style-type: none;
        }
        ul.list li.info-box
        {
            width: 179px /*32%*/;
            float: left;
            clear: right;
            padding: 2px;
            font-size: .75em;
            border-bottom: 1px black solid;
            border-right: 1px black solid;
        }
        ul.list li.info-box.top
        {
            border-top: 1px solid black;
        }
        ul.vehicle-equipment
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            height: 650px;
            width: 303px;
            overflow: hidden;
            float: left;
            clear: right;
            border-bottom: 1px solid black;
            border-right: solid 1px black;
        }
        ul.vehicle-equipment li
        {
            font-size: 1em;
            float: left;
            clear: left;
        }
        ul.vehicle-equipment li.header
        {
            font-weight: bold;
            font-style: italic;
            font-size: 1em;
        }
        div.walkaround img
        {
            width: 33%;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            float: left;
            clear: right;
        }
        ul.car-details
        {
            list-style-type: none;
            width: 100%;
            float: left;
            clear: right;
            font-size: 1em;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            padding-bottom: 4px;
            padding-left: 5px;
            min-height: 507px;
        }
        .small-size
        {
            font-size: 1em;
        }
        .relative-small-size
        {
            font-size: .4em;
        }
        .very-small-size
        {
            font-size: .3em;
        }
        .normal-size
        {
            font-size: 1em;
        }
        div.questionnaire
        {
            width: 488px;
           /* padding-top: 16px;*/
            font-size: 1em;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
           /* padding-left: 3px;*/
            float: left;
            clear: right;
            overflow: hidden;
            height: 450px;
        }
        div.questionnaire .header
        {
            font-size: 1.1em;
        }
        div.questionnaire span.lable
        {
            display: inline-block;
            width: 88%;
        }
        div.questionnaire span.answer
        {
            display: inline-block;
            border: 1px solid black;
            margin-top: 3px;
            margin-left: 4px;
            font-weight: bold;
            width: 17px;
            height: 17px;
            position: relative;
            top: 5px;
            padding-left: 3px;
        }
        div.questionnaire span.answer img
        {
            width: 15px;
            height: 15px;
        }
        
        div.listquestionnaire
        {
        	padding-left:7px;
        }
        div.recon
        {
            float: left;
            clear: right;
            width: 428px;
            overflow: hidden;
            position: relative;
            /*padding: 2px;*/
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            padding-bottom: 30px;
            /*padding-top: 10px;*/
            height: 420px;
            font-size: 1em;
        }
        div.recon .cost
        {
            display: inline-block;
            border-bottom: 1px solid black;
            width: 278px;
            height: 14px;
            margin-left: 5px;
            margin-right: 5px;
            margin-top: 5px;
            text-align: center;
        }
        
        div.listrecon
        {
        	padding-left:7px;
        }
        div.recon .total
        {
            margin-top: 0;
            border: 1px solid black;
            border-bottom: 2px solid black;
            height: 25px;
            position: relative;
            width: 278px;
            top: 6px;
        }
        div.signature
        {
            width: 915px;
            height: 100%;
            padding-left: 2px;
            border-right: 1px solid black;
        }
        div.signature small
        {
            font-size: .9em;
        }
        div.signature span
        {
            display: inline-block;
            margin-top: 1px;
            font-weight: bold;
            vertical-align: top;
        }
        div.relative
        {
            position: relative;
        }
        div.recon img
        {
            left: -1px;
        }
        img.img-header
        {
            position: absolute;
            top: 1px;
            left: 0px;
        }
        div.underline-answer
        {
            border-bottom-style: solid;
            border-bottom-width: thin;
            width: 280px;
        }
        .empty-space
        {
            height: 42px;
        }
        .title-header
        {
            font-size: 1.5em;
            height: 24px;
            vertical-align: bottom;
            background-color: #DDD;
            text-align: center;
            font-weight: bold;
            border-bottom-color: black;
            border-bottom-style: solid;
            border-bottom-width: 2px;
        }
        div.comments 
        {
        	padding-left:4px;
        }
        div.comments .line
        {
            display: inline-block;
            border-bottom: 1px solid black;
            width: 230px;
            height: 18px;
            margin-left: 4px;
            margin-right: 5px;
            margin-top: 5px;
        }
        
    </style>
</head>
<body>
    <div class="wrapper">
        <div class="header">
            <h1>
                Used Vehicle Inspection Form</h1>
        </div>
        <div id="col-one" class="col">
            <div>
                <div class="info-box top">
                    <strong>Salesperson</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.AppraisalBy %></span></div>
                <div class="info-box top">
                    <strong>Date</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.AppraisalDate %></span></div>
                <div class="info-box top">
                    <strong>Time</strong><span class="small-size">
                        <%=Model.AppraisalInfo.AppraisalTime %></span></div>
            </div>
            <div>
                <div class="info-box">
                    <strong>Customer</strong> <span class="small-size">
                        <%=Model.CustomerInfo.FirstName +" "+ Model.CustomerInfo.LastName %></span></div>
                <div class="info-box">
                    <strong>Email</strong><span class="small-size">
                        <%=Model.CustomerInfo.Email%></span></div>
                <div class="info-box">
                    <strong>Phone</strong><span class="normal-size">
                        <%=Model.CustomerInfo.Phone%></span></div>
            </div>
            <div>
               
                <div class="info-box last">
                    <strong>Address</strong> <span class="small-size">
                        <%=Model.CustomerInfo.Street + ", " + Model.CustomerInfo.City + ", " + Model.CustomerInfo.State + ", " + Model.CustomerInfo.Zip%></span></div>
            </div>
            <div>
                <ul class="vin">
                    <% if (String.IsNullOrEmpty(Model.AppraisalInfo.VinNumber))
                       { %>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <li></li>
                    <%}
                       else
                       { %>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[0]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[1]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[2]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[3]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[4]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[5]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[6]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[7]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[8]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[9]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[10]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[11]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[12]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[13]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[14]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[15]%></li>
                    <li>
                        <%=Model.AppraisalInfo.VinNumber[16]%></li>
                    <%} %>
                </ul>
            </div>
            <div>
                <div class="info-box no-right-border">
                    <strong>Year</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.Year %></span></div>
                <div class="info-box no-right-border ">
                    <strong>Make</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.Make %></span></div>
                <div class="info-box">
                    <strong>Model</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.Model %></span></div>
            </div>
            <div>
                <div class="info-box no-right-border">
                    <strong>Ext. Color</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.ExteriorColor %></span></div>
                <div class="info-box no-right-border">
                    <strong>Int. Color</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.InteriorColor %></span></div>
                <div class="info-box">
                    <strong>Mileage</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.Odometer %></span></div>
            </div>
            <div>
                <div class="info-box no-right-border">
                    <strong>Transmission</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.Transmission %></span></div>
                <div class="info-box no-right-border">
                    <strong>Engine Type</strong><span class="small-size">
                        <%=Model.AppraisalInfo.EngineType %></span></div>
                <div class="info-box">
                    <strong>Drive Type</strong> <span class="small-size">
                        <%=Model.AppraisalInfo.DriveType %></span></div>
            </div>
            <div>
                <div class="info-box no-right-border">
                    <strong>Current Monthly</strong>
                </div>
                <div class="info-box no-right-border">
                    <strong>Desired Monthly</strong>
                </div>
                <div class="info-box">
                    <strong>Pay Off</strong>
                </div>
            </div>
            <div>
                <ul class="vehicle-equipment no-right-border" style="padding-right: 1px; padding-left:2px;">
                    <li class="header">Standard Options</li>
                    <%foreach (var item in Model.AppraisalInfo.StandardOptions.Take(15))
                      { %>
                    <li>
                        <%=item %></li>
                    <%} %>
                </ul>
                <ul class="vehicle-equipment no-right-border" style="padding-right: 1px;padding-left:2px;">
                    <li>
                        &nbsp;</li>
                    <%foreach (var item in Model.AppraisalInfo.StandardOptions.Skip(14))
                      { %>
                    <li>
                        <%=item %></li>
                    <%} %>
                </ul>
                <ul class="vehicle-equipment" style="padding-left:2px;">
                    <li class="header">Additional Packages</li>
                    <% foreach (var item in Model.AppraisalInfo.Packages)
                       { %>
                    <li>
                        <%=item %></li>
                    <%} %>
                    <li class="header">Additional Options</li>
                    <%foreach (var item in Model.AppraisalInfo.Options)
                      { %>
                    <li>
                        <%=item %></li>
                    <%} %>
                </ul>
            </div>
            <div class="clear">
            </div>
            <div class="questionnaire relative" >
                <%--<img class="img-header" src="<%= Url.Content("~/images")%>/questionnaire-header.jpg">--%>
                <div class="title-header" style=" padding-left:3px; border-right-color:Black; border-right-width:1px; border-right-style:solid;">
                    Customer Questionnaire</div>
                <br />
                <div class="listquestionnaire">
                <% foreach (var item in Model.AppraisalAnswer)
                   {
                       switch (item.QuestionType)
                       {
                           case 4:
                           case 5:                       
                %>
                <span class="lable">
                    <%= item.Order %>.
                    <%= item.Question %></span><br />
                <div class="underline-answer">
                    <%if (!String.IsNullOrEmpty(item.Answer))
                      { %>
                    <%= item.Answer %>
                    <%}
                      else
                      { %>
                    &nbsp;
                    <%} %>
                </div>
                <%-- _____________________________<br />--%>
                <%          break;
                           case 1:
                           case 2:
                           case 3:
                %>
                <span class="lable">
                    <%= item.Order %>.
                    <%=item.Question %></span>
                <%if (item.Answer.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                  { %>
                <span class="answer">
                    <%--<img src="<%= Url.Content("~/images")%>/checkmark.jpg" />--%>
                    ✔ </span>
                <%}
                  else
                  {%>
                <span class="answer">
                    <%--<img src="<%= Url.Content("~/images")%>/checkmark.jpg" style="visibility:hidden" />--%>
                </span>
                <%} %>
                <br />
                <% if (item.QuestionType == 2 || item.QuestionType == 3)
                   {%>
                List:
                <%=  item.Comment%>
                <br />
                <%}
                   break;
                       }
                   }%>
                   </div>
                <div class="empty-space">
                    &nbsp;</div>
                <%--   <span class="lable">1. How many original keys missing?</span><br />
                _____________________________<br />
                <span class="lable">2. When was the last major service?</span><br />
                _____________________________<br />
                <span class="lable">3. When was the last oil change?</span><br />
                _____________________________<br />
                <span class="lable">4. Are you the original owner? </span><span class="answer">
                    <img src="<%= Url.Content("~/images")%>/checkmark.jpg"></span>
                <br />
                <span class="lable">5. Scheduled maintenance records? </span><span class="answer">
                    <img src="<%= Url.Content("~/images")%>/checkmark.jpg"></span>
                <br />
                <span class="lable">6. Has the vehicle been smoked in? </span><span class="answer">
                    <img src="<%= Url.Content("~/images")%>/checkmark.jpg"></span>
                <br />
                <span class="lable">7. Transmission shifts properly?</span><span class="answer"><img
                    src="<%= Url.Content("~/images")%>/checkmark.jpg"></span>
                <br />
                <span class="lable">8. Air conditioning blows cold air?</span><span class="answer"><img
                    src="<%= Url.Content("~/images")%>/checkmark.jpg"></span>
                <br />
                <span class="lable">9. Extended Warranty?</span><span class="answer"></span>
                <br />
                <span class="lable">10. Complete books and manuals?</span><span class="answer"></span>
                <br />
                <span class="lable">11. Any equipement to be removed?</span><span class="answer"></span>
                <br />
                List Equipement:
                <br />
                <br />
                <br />--%>
            </div>
            <div class="recon relative">
             <div class="title-header" >
             Preliminary Recon Cost
             </div>
             <div class="listrecon">
                <%--<img class="img-header" src="<%= Url.Content("~/images")%>/prelim-header.jpg">--%><br />
                <%-- <%=Model.PreliminaryReconCostObject.FrontBumperCount %> x 
                <span class="cost">
                    <%=Model.PreliminaryReconCostObject.MechanicalCost.ToString("C0")  %>
                </span> = --%><span class="cost">
                    <%=Model.PreliminaryReconCostObject.MechanicalSubTotal.ToString("C0")  %>
                </span>Mechanical<br />
                <%--<%=Model.PreliminaryReconCostObject.FrontBumperCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.FrontBumperCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.FrontBumperSubTotal.ToString("C0") %></span>Front Bumper<br />
                <%--<%=Model.PreliminaryReconCostObject.RearBumperCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.RearBumperCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.RearBumperSubTotal.ToString("C0") %></span>Rear Bumper<br />
                <%--<%=Model.PreliminaryReconCostObject.GlassCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.GlassCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.GlassSubTotal.ToString("C0") %></span>Glass<br />
                <%--<%=Model.PreliminaryReconCostObject.TireCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.TireCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.TireSubTotal.ToString("C0") %></span>Tires<br />
                <%--<%=Model.PreliminaryReconCostObject.FrontEndCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.FrontEndCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.FrontEndSubTotal.ToString("C0") %></span>Front End<br />
                <%--<%=Model.PreliminaryReconCostObject.RearEndCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.RearEndCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.RearEndSubTotal.ToString("C0") %></span>Rear End<br />
                <%--<%=Model.PreliminaryReconCostObject.DriverSideCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.DriverSideCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.DriverSideSubTotal.ToString("C0") %></span>Driver Side<br />
                <%--<%=Model.PreliminaryReconCostObject.PassengerSideCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.PassengerSideCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.PassengerSideSubTotal.ToString("C0") %></span>Passenger Side<br />
                <%--<%=Model.PreliminaryReconCostObject.InteriorCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.InteriorCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.InteriorSubTotal.ToString("C0") %></span>Interior<br />
                <%--<%=Model.PreliminaryReconCostObject.LightBulbCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.LightBulbCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.LightBulbSubTotal.ToString("C0") %></span>Lights/Bulbs<br />
                <%--<%=Model.PreliminaryReconCostObject.OtherCount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.OtherCost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.OtherSubTotal.ToString("C0") %></span>Other<br />
                <%--<%=Model.PreliminaryReconCostObject.LMACount %> x <span class="cost"><%=Model.PreliminaryReconCostObject.LMACost.ToString("C0") %></span> = --%><span class="cost"><%=Model.PreliminaryReconCostObject.LMASubTotal.ToString("C0") %></span>LMA<br />
                <span class="cost total"><%=Model.PreliminaryReconCostObject.Total.ToString("C0") %></span>Total<br />
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="signature">
                <small>Signature</small><br />
                <span style="width: 50%;">Appraiser</span><span style="width: 10%;">Customer</span>
                <%--<img style="height: 20px;" src="<%= Url.Action("RenderSignature","PDF", new { id = Model.AppraisalInfo.AppraisalId }) %>" />--%>
                <img style="height: 80px;" src="<%= Model.AppraisalInfo.SignatureUrl %>" />
            </div>
        </div>
        <div id="col-two" class="col">
            <div style="display: inline-block; width: 194px; height: 335px;">
                <img style="width: 100%" src="<%= Url.Action("RenderWalkaroundPhoto","PDF", new { id = Model.AppraisalInfo.AppraisalId }) %>" />
            </div>
            <ul class="car-details no-right-border">
                <%--<%if (Model.WalkaroundObject != null)
                  {
                      foreach (var item in Model.WalkaroundObject)
                      {
                          %>
                        <li>
                            <%=item.Name%>:<%=item.ItemCount%> damage(s)</li>
                        <%}
                  } %>--%>
                <%if (Model.WalkaroundInfo != null)
                  {
                      foreach (var item in Model.WalkaroundInfo)
                      {
                          %>
                <li>
                    <%--<%=item.Order%>.--%><%=item.Note%></li>
                <%}
                  } %>
            </ul>
            <div class="comments">
                <strong>Comments</strong>
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br /><br />
            <b>ACV : <%=Model.AppraisalInfo.ACV.HasValue ? Model.AppraisalInfo.ACV.Value.ToString("C0") : "" %></b>
            <%--
            <span class="line"></span><br />
            <span class="line"></span><br />
            <span class="line"></span><br />            --%>
            </div>
            </>
        </div>
</body>
</html>
