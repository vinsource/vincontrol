<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<VINControl.Craigslist.CreditCardInfo>" %>

<form id="creditForm">
<input type="hidden" id="ListingId" name="ListingId" value="<%= Model.ListingId %>"/>
<input type="hidden" id="LocationUrl" name="LocationUrl" value="<%= Model.LocationUrl %>"/>
<input type="hidden" id="CryptedStepCheck" name="CryptedStepCheck" value="<%= Model.CryptedStepCheck %>"/>
<div style="float: left;">
    <table width="100%" border="0" style="font-size: 14px;">
        <tr bgcolor="#eeeeee">
            <th align="left" colspan="2"><b>Item(s) for Purchase</b></th>
            <th align="right"><b>Price</b></th>
        </tr>
        <tr>
            <td nowrap colspan="2"><%= ViewData["PostingTitle"] %></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;&nbsp;</td>
            <td><i>cars &amp; trucks - by dealer</i></td>
            <td align='right'>$5.00</td>
        </tr>

        <tr>
            <td align="right" nowrap colspan="2">&nbsp;<b>Total To Be Charged:</b>&nbsp;&nbsp;&nbsp;</td>
            <td align="right"><b>$5.00</b></td>
        </tr>
    </table>

    <hr>
    
    <fieldset id="ccinfo" style="font-size: 14px;">
        <legend><b>Please enter your Credit Card information:</b></legend>
        <table>
            <tr>
                <td align="right"><b><span style="color: green">Card Number:</span></b></td>
                <td>
                    <input type="text" size="20" maxlength="20" autocomplete="off" name="CardNumber" id="CardNumber" value="">
                </td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Verification Number:</span></b></td>
                <td>
                    <input type="text" size="4" maxlength="4" id="VerificationNumber" name="VerificationNumber" value="">
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><small>(We accept American Express, MasterCard, and Visa)</small></td>
            </tr>
            <tr>
                <td align="right"><b>Expiration <span style="color: green">Month</span> / <span style="color: green">Year:</span></b></td>
                <td>
                    <select name="ExpirationMonth" id="ExpirationMonth">
                        <option value="Month" selected>
                        Month<option value="01">
                        01
                            <option value="02">
                        02
                            <option value="03">
                        03
                            <option value="04">
                        04
                            <option value="05">
                        05
                            <option value="06">
                        06
                            <option value="07">
                        07
                            <option value="08">
                        08
                            <option value="09">
                        09
                            <option value="10">
                        10
                            <option value="11">
                        11
                            <option value="12">
                        12
                    </select>
                    /
                        <select name="ExpirationYear" id="ExpirationYear">
                            <option value="Year" selected>
                            Year<option value="2014">
                            2014
                            <option value="2015">
                            2015
                            <option value="2016">
                            2016
                            <option value="2017">
                            2017
                            <option value="2018">
                            2018
                            <option value="2019">
                            2019
                            <option value="2020">
                            2020
                            <option value="2021">
                            2021
                            <option value="2022">
                            2022
                            <option value="2023">
                            2023
                            <option value="2024">
                            2024
                        </select></td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Card Name, First:</span></b></td>
                <td>
                    <input type="text" size="20" maxlength="80" id="FirstName" name="FirstName" value="">
                    <b><span style="color: green">Last:</span></b>
                    <input type="text" size="20" maxlength="80" id="LastName" name="LastName" value=""></td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Card Address:</span></b></td>
                <td>
                    <input type="text" size="30" maxlength="80" id="Address" name="Address" value="">
                    <%--<i>(must match address on card statement exactly)</i>--%>
                </td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">City:</span></b></td>
                <td>
                    <input type="text" size="15" maxlength="80" id="City" name="City" value="">
                    <b><span style="color: green" id="ccst">State:</span></b>
                    <input type="text" size="2" maxlength="2" id="State" name="State" value=""></td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Zip/Postal Code:</span></b></td>
                <td>
                    <input type="text" size="5" maxlength="20" id="Postal" name="Postal" value="">
                </td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Country:</span></b></td>
                <td>
                    <label>
                        <input type="radio" id="Country" name="Country" value="US" checked="checked">US</label>
                </td>
            </tr>
        </table>
    </fieldset>

    <table style="font-size: 14px;">
        <tr>
            <td colspan="2"><b>Who is the best contact person if we have questions about your payment?</b></td>
        </tr>
        <tr>
            <td align="right"><b><span style="color: green">Contact Name:</span></b></td>
            <td>
                <input type="text" size="20" maxlength="80" id="ContactName" name='ContactName' value=""></td>
        </tr>
        <tr>
            <td align="right"><b><span style="color: green">Contact Phone Number:</span></b></td>
            <td>
                <input type="text" size="16" maxlength="80" id="ContactPhone" name='ContactPhone' value=""></td>
        </tr>
        <tr>
            <td align="right"><b><span style="color: green">Contact Email Address:</span></b></td>
            <td>
                <input type="text" size="30" maxlength="80" id="ContactEmail" name='ContactEmail' value="<%= Model.ContactEmail %>"></td>
        </tr>
    </table>
    <hr>

    <div id="b" style="float:right;font-size:14px;">
        <input type="hidden" name='cryptedStepCheck' value="">
        <input type="button" id="btnPurchase" value="Purchase" style="float:right;padding: 6px 10px; cursor: pointer;margin-top:-5px;"/>
        &nbsp;<em>(Please click ONLY ONCE. This step may take up to 60 seconds.)</em>
    </div>
</div>
</form>
