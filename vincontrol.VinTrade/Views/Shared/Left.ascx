<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="vincontrol.VinTrade.Handlers" %>

<div class="left-bar">
    <div class="fixed-right-menu">
        <div class="fixed-btn" id="Div1">
            <a href="javascript:newPopup('<%= Url.Action("PrintTradeIn", "Inventory")%>',1200,800)"><img alt="" src="<%=Url.Content("~/Content/Images/NewSiteImages/print-btn.png")%>" width="65"/></a>
        </div>
    </div>
    <div class="logo">
        <h1>
            Trade-In</h1>
    </div>

    <% if (SessionHandler.TradeInDealer != null) {%>
    <input type="hidden" id="vin" name="vin" value="<%= SessionHandler.LandingInfo.VehicleInfo.Vin %>" />
    <input type="hidden" id="dealerName" name="dealerName" value="<%= SessionHandler.TradeInDealer.DealershipName.Replace(" ","") %>" />
    <div class="dealer-info">
        <h3><%= SessionHandler.TradeInDealer.DealershipName %></h3>
        <ul class="dealer-details">
            <li class="dealer-phone"><span class="dealer-detail-label" style="float:left;">Phone </span>
                <div class="dealer-detail-text">
                    <%= SessionHandler.TradeInDealer.Phone%>
                </div>
            </li>
         
            <li class="dealer-email"><span class="dealer-detail-label" style="float:left;">Email </span>
                <div class="dealer-detail-text">
                    <%= SessionHandler.TradeInDealer.Email%>
                </div>
            </li>
            <li class="dealer-address"><span class="dealer-detail-label" style="vertical-align:top;float:left;">Address </span>
                <div class="dealer-detail-address-one" style="width:170px;">
                    
                         <%=SessionHandler.TradeInDealer.Address%> <%=SessionHandler.TradeInDealer.City%>, <%=SessionHandler.TradeInDealer.State%> <%=SessionHandler.TradeInDealer.ZipCode%>
                </div>
                <div class="dealer-detail-address-two">
                    
                </div>
            </li>
        </ul>
    </div>
    <%}%>

    <div class="navigation">
        <button class="green-btn">
            Call Now! <%= SessionHandler.TradeInDealer != null ? SessionHandler.TradeInDealer.Phone : string.Empty %>
        </button>
        <a class="make_offer" href="javascript:;">
            <button class="popup" id="make-offer-btn">
                Make Offer
            </button>
        </a>
        <a class="more-info" href="javascript:;">
            <button class="popup" id="more-info-btn">
                Get More Info
            </button>
        </a>                   
        <button class="link" id="value-trade-btn">
            Value Trade
        </button>
        <button class="link" id="financing-btn">
            Financing
        </button>
        <a class="test_drive" href="javascript:;">
            <button class="popup" id="test-drive-btn">
                Test Drive
            </button>
        </a>
        <a href="javascript:void(0);">
            <button class="blue-btn" id="back-top-btn">
                Back to Top
            </button>
        </a>
    </div>
    <div class="price-drop-alerts">
        <div class="price-drop-wrapper">
            <h2>
                Price Drop Alerts!</h2>
            <div style="height:40px;"><input type="text" style="width:150px;" id="pdaFirstName" name="pdaFirstName"/>
            <span style="display:block;">First Name</span>
            <br/>
            </div>
            <div style="height:40px;"><input type="text" style="width:150px;" id="pdaLastName" name="pdaLastName"/>
            <span style="display:block;">Last Name</span>
            <br/>
            </div>
            <div style="height:40px;">
            <input type="text" style="width:150px;" class="mask_phone" id="pdaPhone" name="pdaPhone"/>
            <span style="display:block;">Phone</span>
            <br/>
            </div>
            <div style="height:40px;">
            <input type="text" style="width:150px;" id="pdaEmail" name="pdaEmail"/>
            <span style="display:block;">Email</span>
            <br/>
            </div>
            <div>I would like updates via:
            <br/>
            <input type="radio" name="pdaRadio" id="pdaRadio_2"/>
            Text
            <input type="radio" name="pdaRadio" checked="checked" id="pdaRadio_1"/>
            Email
            <br/>
            </div>
            <button id="price-alert-btn">
                Submit
            </button>
        </div>
    </div>    
</div>