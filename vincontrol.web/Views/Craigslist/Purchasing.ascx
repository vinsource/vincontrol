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
                    <input type="text" size="20" maxlength="20" autocomplete="off" name="CardNumber" id="CardNumber" value="<%= Model.CardNumber %>">
                </td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Verification Number:</span></b></td>
                <td>
                    <input type="text" size="4" maxlength="4" id="VerificationNumber" name="VerificationNumber" value="<%= Model.VerificationNumber %>">
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
                        <option value="Month" <%= Model.ExpirationMonth == 0 ? "selected" : "" %>>
                        Month<option value="01" <%= Model.ExpirationMonth == 1 ? "selected" : "" %>>
                        01
                            <option value="02" <%= Model.ExpirationMonth == 2 ? "selected" : "" %>>
                        02
                            <option value="03" <%= Model.ExpirationMonth == 3 ? "selected" : "" %>>
                        03
                            <option value="04" <%= Model.ExpirationMonth == 4 ? "selected" : "" %>>
                        04
                            <option value="05" <%= Model.ExpirationMonth == 5 ? "selected" : "" %>>
                        05
                            <option value="06" <%= Model.ExpirationMonth == 6 ? "selected" : "" %>>
                        06
                            <option value="07" <%= Model.ExpirationMonth == 7 ? "selected" : "" %>>
                        07
                            <option value="08" <%= Model.ExpirationMonth == 8 ? "selected" : "" %>>
                        08
                            <option value="09" <%= Model.ExpirationMonth == 9 ? "selected" : "" %>>
                        09
                            <option value="10" <%= Model.ExpirationMonth == 10 ? "selected" : "" %>>
                        10
                            <option value="11" <%= Model.ExpirationMonth == 11 ? "selected" : "" %>>
                        11
                            <option value="12" <%= Model.ExpirationMonth == 12 ? "selected" : "" %>>
                        12
                    </select>
                    /
                        <select name="ExpirationYear" id="ExpirationYear">
                            <option value="Year" <%= Model.ExpirationYear == 0 ? "selected" : "" %>>
                            Year
                            
                            <option value="2017" <%= Model.ExpirationYear == 2017 ? "selected" : "" %>>
                            2017
                            <option value="2018" <%= Model.ExpirationYear == 2018 ? "selected" : "" %>>
                            2018
                            <option value="2019" <%= Model.ExpirationYear == 2019 ? "selected" : "" %>>
                            2019
                            <option value="2020" <%= Model.ExpirationYear == 2020 ? "selected" : "" %>>
                            2020
                            <option value="2021" <%= Model.ExpirationYear == 2021 ? "selected" : "" %>>
                            2021
                            <option value="2022" <%= Model.ExpirationYear == 2022 ? "selected" : "" %>>
                            2022
                            <option value="2023" <%= Model.ExpirationYear == 2023 ? "selected" : "" %>>
                            2023
                            <option value="2024" <%= Model.ExpirationYear == 2024 ? "selected" : "" %>>
                            2024
                            <option value="2025" <%= Model.ExpirationYear == 2025 ? "selected" : "" %>>
                            2025
                            <option value="2026" <%= Model.ExpirationYear == 2026 ? "selected" : "" %>>
                            2026
                            <option value="2027" <%= Model.ExpirationYear == 2027 ? "selected" : "" %>>
                            2027
                        </select></td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Card Name, First:</span></b></td>
                <td>
                    <input type="text" size="20" maxlength="80" id="FirstName" name="FirstName" value="<%= Model.FirstName %>">
                    <b><span style="color: green">Last:</span></b>
                    <input type="text" size="20" maxlength="80" id="LastName" name="LastName" value="<%= Model.LastName %>"></td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Card Address:</span></b></td>
                <td>
                    <input type="text" size="30" maxlength="80" id="Address" name="Address" value="<%= Model.Address %>">
                    <%--<i>(must match address on card statement exactly)</i>--%>
                </td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">City:</span></b></td>
                <td>
                    <input type="text" size="15" maxlength="80" id="City" name="City" value="<%= Model.City %>">
                    <b><span style="color: green" id="ccst">State:</span></b>
                    <input type="text" size="2" maxlength="2" id="State" name="State" value="<%= Model.State %>"></td>
            </tr>
            <tr>
                <td align="right"><b><span style="color: green">Zip/Postal Code:</span></b></td>
                <td>
                    <input type="text" size="5" maxlength="20" id="Postal" name="Postal" value="<%= Model.Postal %>">
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
                <input type="text" size="20" maxlength="80" id="ContactName" name='ContactName' value="<%= Model.ContactName %>"></td>
        </tr>
        <tr>
            <td align="right"><b><span style="color: green">Contact Phone Number:</span></b></td>
            <td>
                <input type="text" size="16" maxlength="80" id="ContactPhone" name='ContactPhone' value="<%= Model.ContactPhone %>"></td>
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
