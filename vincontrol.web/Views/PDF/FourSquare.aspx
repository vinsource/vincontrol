<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>PrintFourSquare</title>

    <style type="text/css">
        #fsquareContainer
        {
            width: 1100px;
            font-size: 14px;
            margin: 0px auto;
            background-color: #FAFAFA;
            font-weight: bold;
            color: #555;
        }
   .fsquareLogo
        {
			
            background-color: white;
 margin-left: 250px
        }

  

        .fsquareHeaderLeft
        {
            float: right;
            width: 40%;
        }

        .fsqRow
        {
            min-height: 30px;
            clear: both;
            border-bottom: 2px solid #CDCDCD;
            padding-bottom: 4px;
            margin-top: 5px;
        }

        .fsqFloatLeft
        {
            margin-right: 5px;
            float: left;
        }

        .fsqLabel
        {
            
        }

            .fsqLabel.fsqFloatLeft
            {
                width: 64px;
            }

        #fsquareInfo
        {
            clear: both;
            margin-top: 30px;
        }

        .clear
        {
            clear: both;
        }

        .fsqInpuText[type="text"]
        {
            margin-top: 0px;
            width: 100%;
            padding-left: 7px;
            border: none;
            background-color: #FAFAFA;
            font-size: 15px
        }

        .fsqInpuText[type="text"].fsqFont
        {
          font-size: 30px
        }

        .fsquareInfoLeft
        {
            float: left;
            width: 50%;
            border-right: 2px solid #808080;
        }

        .fsqInput.fsqFloatLeft
        {
            width: 65%;
        }

        .fsqCheck .fsqInput
        {
            width: 27px !important;
        }

        .fsqCheck .fsqLabel
        {
            width: 40px !important;
        }

        div, input
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box; /* Firefox */
        }

        .smHeight1
        {
            height: 85px;
        }

        .sqrPayments
        {
            height: 30px;
        }

        .smHeight2
        {
            height: 71px;
        }

        .sqrPaymentText
        {
            font-size: 22px;
            font-weight: bold;
            text-align: center;
        }

        .sqrPaymentRight
        {
            float: right;
            margin-right: 60px;
            margin-top: 20px;
        }
        .salePriceBig {
            padding: 15px;
            font-size: 50px;
        }
        .borderBottom
        {
            border-bottom: 2px solid #808080;
        }

        .smHeight3
        {
            height: 52px;
        }

        .smHeight4
        {
            height: 200px;
        }

        .smHeight5
        {
            height: 300px;
        }

        .smHeight6
        {
            height: 150px;
        }

        .sqrSignLabel
        {
            float: left;
        }

        .sqrBottomText
        {
            padding: 10px 30px 0px;
            height: 70px;
            font-size: 15px;
            font-weight: normal;
        }

        .sqrBottomSign
        {
            padding: 0px 30px;
            height: 30px;
        }

        .sqrSignInput
        {
            float: left;
            width: 200px;
            border-bottom: 2px solid #808080;
            margin-top: 10px;
            margin-left: 10px;
        }

        .textCentered
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function showDate() {
            var currentDate = new Date();
            var day = currentDate.getDate();
            var month = currentDate.getMonth() + 1;
            var year = currentDate.getFullYear();
            document.getElementById("fsqDate").value =  day + "/" + month + "/" + year ;
         
            
        }


    </script>
</head>
<body onload="showDate()">
      <img class="fsquareLogo" src="<% =SessionHandler.Dealer.Logo %>" alt=""/>
 
   
          
 
    <div id="fsquareContainer">
       
        <div id="fsquareHeader">
            <div class="fsquareHeaderLeft">
                <div class="fsqRow">
                    <div class="fsqItem">
                        <div class="fsqLabel fsqFloatLeft">
                            DATE
                        </div>
                        <div class="fsqInput fsqFloatLeft">
                            <input class="fsqInpuText" id="fsqDate" type="text" disabled="disabled" />
                        </div>
                    </div>
                </div>

                <div class="fsqRow">
                    <div class="fsqItem">
                        <div class="fsqLabel fsqFloatLeft">
                            SOURCE
                        </div>
                        <div class="fsqInput fsqFloatLeft">
                            <input class="fsqInpuText" type="text" disabled="disabled" />
                        </div>
                    </div>
                </div>

            </div>
            <div class="clear"></div>
        </div>
        <div id="fsquareInfo">
            <div class="fsqRow">
                <div style="width: 40%" class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        NAME
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" />
                    </div>
                </div>
                <div style="width: 34%" class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft" style="width: 90px;">
                        SALESPERSON
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" value="<%=SessionHandler.CurrentUser.Name %>" />
                    </div>
                </div>
                <div class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        MGR.
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" />
                    </div>
                </div>
            </div>

            <div class="fsqRow">
                <div style="width: 55%;" class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        ADDRESS
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" />
                    </div>
                </div>
                <div class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        PHONE
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" />
                    </div>
                </div>
                <div class="fsqItem fsqFloatLeft">
                    <div class="fsqItem">
                        <div class="fsqLabel fsqFloatLeft">
                            H
                        </div>
                        <div class="fsqInput fsqFloatLeft">
                            <input class="fsqInpuText" type="text" disabled="disabled" />
                        </div>
                    </div>
                    <div class="fsqItem">
                        <div class="fsqLabel fsqFloatLeft">
                            W
                        </div>
                        <div class="fsqInput fsqFloatLeft">
                            <input class="fsqInpuText" type="text" disabled="disabled" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>

            <div class="fsqRow">
                <div class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        MILEAGE
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Mileage.ToString("n0") %>" />
                    </div>
                </div>
                <div style="width: 44%;" class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft" style="width: 30px;">
                        VIN
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Vin %>" />
                    </div>
                </div>
            <%--    <div class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        LIC.NO
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" />
                    </div>
                </div>--%>
            </div>
            <div class="fsqLeftRightHolder">
                <div class="fsquareInfoLeft">
                    <div class="fsqRow smHeight1">
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel">
                                STOCK NO.
                            </div>
                            <div class="fsqInput">
                                <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Stock %>" />
                            </div>
                        </div>
                         <%
                                bool checkedUsed = false, checkedNew = false, checkedDemo = false;
                                if (Model.Condition == Constanst.ConditionStatus.New)
                                {
                                    checkedNew = true;
                                }
                                else
                                {
                                    if (Model.Condition == Constanst.ConditionStatus.Used)
                                    {
                                        checkedUsed = true;
                                    }
                                    
                                }

                                if (Model.DealerDemo == true)
                                {
                                    checkedDemo = true;
                                }
                                 %>
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqItem fsqCheck">
                                <div class="fsqLabel fsqFloatLeft">
                                    NEW
                                </div>
                                <div class="fsqInput fsqFloatLeft">
                                    <% if (checkedNew == true)
                                       {
                                            %>
                                                <input class="fsqInpuText" checked="checked" type="checkbox"/>
                                            <%
                                       }
                                       else
                                       {    
                                           %><input type="checkbox" class="fsqInpuText" /><%
                                       }%>
                                    
                                </div>
                            </div>
                           
                            <div class="fsqItem fsqCheck">
                                <div class="fsqLabel fsqFloatLeft">
                                    USED
                                </div>
                                <div class="fsqInput fsqFloatLeft">
                                    <% if (checkedUsed == true)
                                       {
                                           %><input type="checkbox" class="fsqInpuText" checked="checked"/><%
                                       }
                                       else
                                       {
                                           %><input type="checkbox" class="fsqInpuText"/><%
                                       }%>
                                    
                                </div>
                            </div>
                            <div class="fsqItem fsqCheck">
                                <div class="fsqLabel fsqFloatLeft">
                                    DEMO
                                </div>
                                <div class="fsqInput fsqFloatLeft">
                                    <% if (checkedDemo == true)
                                       {
                                           %><input type="checkbox" class="fsqInpuText" checked="checked"/><%
                                       }
                                       else
                                       {
                                           %><input type="checkbox" class="fsqInpuText"/><%
                                       } %>
                                    
                                </div>
                            </div>
                        </div>
                        <div style="width: 17%" class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel textCentered">
                                YEAR
                            </div>
                            <div class="fsqInput">
                                <input class="fsqInpuText textCentered" type="text" disabled="disabled" value="<%=Model.ModelYear %>" />
                            </div>
                        </div>
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel textCentered">
                                MAKE
                            </div>
                            <div class="fsqInput">
                                <input class="fsqInpuText textCentered" type="text" disabled="disabled" value="<%=Model.Make %>" />
                            </div>
                        </div>

                        <div class="clear"></div>
                    </div>
                    <div class="fsqRow smHeight2">
                        <div class="fsqRow" style="min-height: 15px">
                            <div class="fsqLabel" style="text-align: center">
                                TRADE-IN INFORMATION
                            </div>
                        </div>
                        <div class="fsqRow">
                            <div style="width: 20%" class="fsqItem fsqFloatLeft">
                                <div class="fsqLabel">
                                    YEAR
                                </div>
                                <div class="fsqInput">
                                    <input class="fsqInpuText" type="text" disabled="disabled" />
                                </div>
                            </div>
                            <div style="width: 33%" class="fsqItem fsqFloatLeft">
                                <div class="fsqLabel">
                                    MAKE
                                </div>
                                <div class="fsqInput">
                                    <input class="fsqInpuText" type="text" disabled="disabled" />
                                </div>
                            </div>
                            <div style="width: 15%" class="fsqItem fsqFloatLeft">
                                <div class="fsqLabel">
                                    CYL.
                                </div>
                                <div class="fsqInput">
                                    <input class="fsqInpuText" type="text" disabled="disabled" />
                                </div>
                            </div>
                            <div style="width: 28%" class="fsqItem fsqFloatLeft">
                                <div class="fsqLabel">
                                    BODY STYLE
                                </div>
                                <div class="fsqInput">
                                    <input class="fsqInpuText" type="text" disabled="disabled" />
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="fsqRow smHeight3">
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel fsqFloatLeft">
                                MILEAGE
                            </div>
                            <div class="fsqInput fsqFloatLeft">
                                <input class="fsqInpuText" type="text" disabled="disabled" />
                            </div>
                        </div>
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel fsqFloatLeft">
                                LIC.NO
                            </div>
                            <div class="fsqInput fsqFloatLeft">
                                <input class="fsqInpuText" type="text" disabled="disabled" />
                            </div>
                        </div>

                    </div>
                    <div class="fsqRow smHeight4">
                        <div class="fsqFloatLeft">
                            <div class="fsqInput borderBottom">
                                <input class="fsqInpuText" type="text" disabled="disabled" />
                            </div>
                            <div class="sqrnumber">
                                NUMBER
                            </div>
                        </div>
                        <div class="fsqFloatLeft">

                            <div class="sqrPaymentText">
                                PAYMENTS OF
                            </div>
                             <div class="sqrPaymentText" style="font-size: 18px; padding-top: 30px">
                                PAY OFF AMOUNT
                            </div>
                        </div>
                        <div class="fsqFloatLeft">
                            <div class="sqrPayments">
                                <div class="fsqFloatLeft">$</div>
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input style="font-size: 16px" class="fsqInpuText" type="text" disabled="disabled" />
                                </div>

                            </div>
                            <div class="sqrPayments" style="padding-top: 20px">
                                <div class="fsqFloatLeft">$</div>
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input style="font-size: 16px;" class="fsqInpuText" type="text" disabled="disabled" />
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="fsqRow smHeight5">
                        <div class="sqrPaymentText">
                           20% CASH DOWN
                        </div>
                        <div class="sqrPayments sqrPaymentRight">
                            
                            <div class="fsqFloatLeft fsqInput borderBottom">
                                <input style="font-size: 30px;text-align: center;" class="fsqInpuText" type="text" disabled="disabled" value="$<%=(Model.SalePrice*(decimal) 0.2).ToString("N") %>" />
                            </div>

                        </div>
                    </div>
                    <div class="fsqRow smHeight6">
                        <div class="sqrBottomText">
                            Salesmen cannot accept the offer or obligate seller in any manner whatsover. OFFER IS NOT BINDING UNTIL ACCEPTED IN WRITING BY OFFICER OR SALES MANAGER OF SELLER
                        </div>
                        <div class="sqrBottomSign">
                            <div class="sqrSignLabel">
                                X
                            </div>
                            <div class="sqrSignInput"></div>
                        </div>
                    </div>
                </div>
                <div class="fsquareInfoLeft">
                    <div class="fsqRow smHeight1">
                        <div style="padding-left: 5px; padding-right: 5px;" class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel">
                                BODY STYLE
                            </div>
                            <div class="fsqInput">
                                <input class="fsqInpuText fsqFont" type="text" disabled="disabled" value="<%=Model.BodyType %>" />
                            </div>

                        </div>
            
                        <div class="clear"></div>
                    </div>

                    <div class="fsqRow smHeight2">
                        <div style="padding-left: 5px; padding-right: 5px;" class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel">
                                LIST PRICE
                            </div>
                            <div class="fsqInput">
                                <div class="fsqFloatLeft">$</div>
                                <div class="fsqFloatLeft fsqInput borderBottom">
                                    <input style="font-size: 16px" class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.RetailPrice.ToString("N") %>" />
                                </div>
                                
                            </div>

                        </div>

                        <div class="clear"></div>
                    </div>
                    <div class="fsqRow smHeight3">
                        <div style="padding-left: 5px; padding-right: 5px;" class="fsqItem">
                            <div class="fsqLabel">
                                ADDITIONAL ACCESSORIES
                            </div>
                            <div class="fsqInput">
                                <input class="fsqInpuText" type="text" disabled="disabled" />
                            </div>

                        </div>

                        <div class="clear"></div>
                    </div>
                 
                    <div class="fsqRow smHeight4">
                          <div class="fsqLabel" style="padding-left: 5px; padding-top: 5px ">
                                INTERNET PRICE
                            </div>
                        <div class="salePriceBig"><%=Model.SalePrice.ToString("C") %></div>
                         <div class="clear"></div>
                        
                    </div>
                           
                    
                    <div class="fsqRow smHeight5">
                        <div class="sqrPaymentText">
                            MONTHLY PAYMENTS
                        </div>
                        <div class="sqrPayments sqrPaymentRight">
                                 <div class="fsqFloatLeft">

                            <div class="sqrPaymentText" style="font-size: 25px">
                                60 Months
                            </div>
                             <div class="sqrPaymentText" style="font-size: 20px; padding-top: 30px">
                                48 Months
                            </div>
                            <div class="sqrPaymentText" style="font-size: 20px; padding-top: 30px">
                                36 Months
                            </div>
                        </div>
                        <div class="fsqFloatLeft">
                            <div class="sqrPayments">
                               
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input style="font-size: 25px;text-align: center" class="fsqInpuText" type="text" disabled="disabled" value="$<%=Model.Monthsof60Payment  %>"/>
                                </div>

                            </div>
                            <div class="sqrPayments" style="padding-top: 20px">
                              
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input style="font-size: 16px;text-align: center" class="fsqInpuText" type="text" disabled="disabled" value="$<%=Model.Monthsof48Payment  %>" />
                                </div>

                            </div>
                                <div class="sqrPayments" style="padding-top: 40px">
                                
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input style="font-size: 16px;text-align: center" class="fsqInpuText" type="text" disabled="disabled" value="$<%=Model.Monthsof36Payment  %>"/>
                                </div>

                            </div>
                        </div>

                           <%-- <div class="fsqFloatLeft">60 Month</div>
                            <div class="fsqFloatLeft fsqInput borderBottom">
                                <input style="font-size: 16px;" class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Monthsof60Payment  %>" />
                            </div>
                              <div class="fsqFloatLeft">48 Month</div>
                            <div class="fsqFloatLeft fsqInput borderBottom">
                                <input style="font-size: 16px;" class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Monthsof48Payment  %>" />
                            </div>
                              <div class="fsqFloatLeft">36 Month</div>
                            <div class="fsqFloatLeft fsqInput borderBottom">
                                <input style="font-size: 16px;" class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Monthsof36Payment  %>" />
                            </div>--%>

                        </div>
                    </div>
                    <div class="fsqRow smHeight6">
                        <div class="sqrBottomText">
                            Please enter my offer, subject to your acceptance
                        </div>
                        <div class="sqrBottomSign">
                            <div class="sqrSignLabel">
                                BUYER'S
									<br />
                                SIGNATURE X
                            </div>
                            <div class="sqrSignInput"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>

        </div>
    </div>
</body>
    
</html>
