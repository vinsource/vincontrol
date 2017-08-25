<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>PrintFourSquare</title>

    <style type="text/css">
        #fsquareContainer
        {
            width: 1100px;
            font-size: 13px;
            margin: 0px auto;
            background-color: #FAFAFA;
            font-weight: bold;
            color: #555;
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
                width: 58px;
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
</head>
<body>
    <div id="fsquareContainer">
   
        <div id="fsquareHeader">
         
            <div class="fsquareHeaderLeft">
                <div class="fsqRow">
                    <div class="fsqItem">
                        <div class="fsqLabel fsqFloatLeft">
                            DATE
                        </div>
                        <div class="fsqInput fsqFloatLeft">
                            <input class="fsqInpuText" type="text" disabled="disabled" />
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
                        <input class="fsqInpuText" type="text" disabled="disabled" />
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
                        <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Mileage %>" />
                    </div>
                </div>
                <div style="width: 44%;" class="fsqItem fsqFloatLeft">
                    <div class="fsqLabel fsqFloatLeft">
                        VIN
                    </div>
                    <div class="fsqInput fsqFloatLeft">
                        <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.Vin %>" />
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
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqItem fsqCheck">
                                <div class="fsqLabel fsqFloatLeft">
                                    NEW
                                </div>
                                <div class="fsqInput fsqFloatLeft">
                                    <input class="fsqInpuText" type="checkbox" />
                                </div>
                            </div>
                            <div class="fsqItem fsqCheck">
                                <div class="fsqLabel fsqFloatLeft">
                                    USED
                                </div>
                                <div class="fsqInput fsqFloatLeft">
                                    <input class="fsqInpuText" type="checkbox" />
                                </div>
                            </div>
                            <div class="fsqItem fsqCheck">
                                <div class="fsqLabel fsqFloatLeft">
                                    DEMO
                                </div>
                                <div class="fsqInput fsqFloatLeft">
                                    <input class="fsqInpuText" type="checkbox" />
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
                        </div>
                        <div class="fsqFloatLeft">
                            <div class="sqrPayments">
                                <div class="fsqFloatLeft">$</div>
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input class="fsqInpuText" type="text" disabled="disabled" />
                                </div>

                            </div>
                            <div class="sqrPayments">
                                <div class="fsqFloatLeft">$</div>
                                <div class="fsqInput fsqFloatLeft borderBottom">
                                    <input class="fsqInpuText" type="text" disabled="disabled" />
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="fsqRow smHeight5">
                        <div class="sqrPaymentText">
                            1/3 CASH DOWN
                        </div>
                        <div class="sqrPayments sqrPaymentRight">
                            <div class="fsqFloatLeft">$</div>
                            <div class="fsqFloatLeft fsqInput borderBottom">
                                <input class="fsqInpuText" type="text" disabled="disabled" value="<%=(Model.SalePrice*(decimal) 0.2).ToString("N") %>" />
                            </div>

                        </div>
                    </div>
                    <div class="fsqRow smHeight6">
                        <div class="sqrBottomText">
                            Salesmen cannot accept the offer or obligate seller in any manner whatsover. OFFER is NOT BINDING UNTIL ACCEPTED IN WRITING BY OFFICER OR SALES MANAGER OF SELLER
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
                                <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.BodyType %>" />
                            </div>

                        </div>
                        <div class="fsqItem fsqFloatLeft">
                            <div class="fsqRow">
                                <div class="fsqItem">
                                    <div class="fsqLabel fsqFloatLeft">
                                        TAB NO.
                                    </div>
                                    <div class="fsqInput fsqFloatLeft">
                                        <input class="fsqInpuText" type="text" disabled="disabled" />
                                    </div>
                                </div>
                            </div>
                            <div class="fsqRow">
                                <div class="fsqItem">
                                    <div class="fsqLabel fsqFloatLeft">
                                        YRS EXP.
                                    </div>
                                    <div class="fsqInput fsqFloatLeft">
                                        <input class="fsqInpuText" type="text" disabled="disabled" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div class="fsqRow smHeight2">
                        <div style="padding-left: 5px; padding-right: 5px;" class="fsqItem fsqFloatLeft">
                            <div class="fsqLabel">
                                SALE PRICE
                            </div>
                            <div class="fsqInput">
                                <div class="fsqFloatLeft">$</div>
                                <div class="fsqFloatLeft fsqInput borderBottom">
                                    <input class="fsqInpuText" type="text" disabled="disabled" value="<%=Model.SalePrice.ToString("N") %>" />
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
                        <div class="salePriceBig"><%=Model.SalePrice.ToString("C") %></div>
                    </div>
                    <div class="fsqRow smHeight5">
                        <div class="sqrPaymentText">
                            MONTHLY PAYMENTS
                        </div>
                        <div class="sqrPayments sqrPaymentRight">
                            <div class="fsqFloatLeft">$</div>
                            <div class="fsqFloatLeft fsqInput borderBottom">
                                <%
                                    var monthlyPayment = ((Model.SalePrice*(decimal) 0.8)*(decimal) (0.1/12))/(decimal) (1 - Math.Pow((1 + 0.1/12), -12));
                                     %>
                                <input class="fsqInpuText" type="text" disabled="disabled" value="<%=monthlyPayment.ToString("N") %>" />
                            </div>

                        </div>
                    </div>
                    <div class="fsqRow smHeight6">
                        <div class="sqrBottomText">
                            Please enter my offer, subject to you acceptance
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
