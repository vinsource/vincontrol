<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/TradeIn.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.TradeInManagement.TradeInVehicleModel>" %>
<%@ Import Namespace="vincontrol.VinTrade.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trade In | Vehicle Value
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .tradeInHeader
        {
            background-image: url("/Content/images/TradeInStep/header.png");
        }
    </style>
    <div class="tradeinVehicleValue">
        <div class="tvehiclevalue">
            <div class="tstep1_top_title">
                Your Vehicle's Trade-in Value
            </div>
            <div class="tvehiclevalue_content">
                <div class="tvehiclevalue_tradein_left">
                    <div class="tvehicle_tradein_title">
                        Market Value</div>
                    <div class="tvehicle_tradein_img">
                        <div class="tvehicle_tradein_lowprice">
                            Call Dealer
                        </div>
                        <div class="tvehicle_tradein_equalprice">
                            <%if(Model.TradeInFairPrice==0)
                                      { %>
                                    Call Dealer
                                    <% }
                                      else
                                      {
                                    %>
                                    <%=((int)Model.TradeInFairPrice).ToString("n0")%>
                                    <%} %>
                        </div>
                        <div class="tvehicle_tradein_hightprice">
                            <%if(Model.TradeInGoodPrice==0)
                                      { %>
                                    Call Dealer
                                    <% }
                                      else
                                      {
                                    %>
                                      <%=((int)Model.TradeInGoodPrice).ToString("n0")%>
                                    <%} %>
                        </div>
                    </div>
                    <div class="tvehicle_tradein_smalltext">
                        * Above numbers are ESTIMATED TRADE-IN VALUES, dealer offer may vary. *</div>
                </div>
                <div class="tvehiclevalue_tradein_right">
                    <div class="tvehicle_tradein_title">
                        Your Vehicle</div>
                    <div class="tvehicle_tradein_info_holder">
                        Year: <%=Model.SelectedYear %>
                    </div>
                    <div class="tvehicle_tradein_info_holder">
                        Make: <%=Model.SelectedMakeValue %>
                    </div>
                    <div class="tvehicle_tradein_info_holder">
                        Model: <%=Model.SelectedModelValue %>
                    </div>
                    <div class="tvehicle_tradein_info_holder">
                        Trim: <%=Model.SelectedTrimValue %>
                    </div>
                    <div class="tvehicle_tradein_info_holder">
                        Mileage: <%=Model.Mileage%>
                    </div>
                    <div class="tvehicle_tradein_info_holder">
                        Condition: <%=Model.Condition %>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <div class="tvehiclevalue">
            <div class="tstep1_top_title">
                Vehicle's Market Comparison
            </div>
            <div class="tvehiclevalue_content">
                <div class="vh_comparison_top">
                    <div class="vh_comparison_top_header">
                        <div class="vh_comparison_top_items">
                        </div>
                        <div class="vh_comparison_top_items">
                            Price
                        </div>
                        <div class="vh_comparison_top_items">
                            Mileage
                        </div>
                        <div class="vh_comparison_top_items">
                            Location
                        </div>
                    </div>
                    <div class="vh_comparison_top_row">
                        <div class="vh_comparison_top_items">
                            Your Vehicle
                        </div>
                        <div class="vh_comparison_top_items">
                                <%if(Model.TradeInFairPrice==0)
                                      { %>
                                    Call Dealer
                                    <% }
                                      else
                                      {
                                    %>
                                    <%=((int)Model.TradeInFairPrice).ToString("n0")%>
                                    <%} %>
                        </div>
                        <div class="vh_comparison_top_items">
                                  <%=(Model.MileageNumber).ToString("n0")%>
                        </div>
                        <div class="vh_comparison_top_items" style="font-size: 15px">
                               <%= SessionHandler.TradeInDealer.DealershipName%>
                        </div>
                    </div>
                </div>
                <div class="vh_comparison_tooltip">
                    The prices below are RETAIL prices and WILL BE HIGHER than the Trade-in Values given
                    for your vehicle.</div>
                <div class="vh_comparison_others">
                    <div class="vh_comparison_others_items">
                        <div class="vh_comparison_others_img">
                            <div class="vh_comparison_others_title">
                                Highest On Market</div>
                            <div class="vh_comparison_others_img_holder">
                                <% if (!String.IsNullOrEmpty(Model.AboveThumnailUrl) && !Model.AboveThumnailUrl.Contains("blank_dot"))
                                   { %>
                                <img src=" <%= Model.AboveThumnailUrl %>" style="max-height:100%;height:auto;width:100%;max-width:100%;margin:auto;display:inline;">
                                <% }
                                   else
                                   { %>
                                  <img src="<%=Url.Content("~/Content/Images/NoPhotoAvailable.jpg")%>" style="max-height:100%;height:auto;width:100%;max-width:100%;margin:auto;display:inline;">
                                <% } %>
                               
                            </div>
                        </div>
                        <div class="vh_comparison_others_info">      <%=((int)Model.MarketHighestPrice).ToString("n0")%></div>
                        <div class="vh_comparison_others_info">
                    <%=(Model.MarketHighestMileage).ToString("n0")%></div>
                        <div class="vh_comparison_others_info">
                            Nationwide</div>
                    </div>
                    <div class="vh_comparison_others_items">
                        <div class="vh_comparison_others_img">
                            <div class="vh_comparison_others_title">
                                Middle On Market</div>
                            <div class="vh_comparison_others_img_holder">
                                <img src="<%=Url.Content("~/Content/Images/NoPhotoAvailable.jpg")%>" style="max-height:100%;height:auto;width:100%;max-width:100%;margin:auto;display:inline;">
                            </div>
                        </div>
                       <div class="vh_comparison_others_info"><%=((int)Model.MarketAveragePrice).ToString("n0")%></div>
                         <div class="vh_comparison_others_info">
                      <%=(Model.MarketAverageMileage).ToString("n0")%></div>
                        <div class="vh_comparison_others_info">
                            Nationwide</div>
                    </div>
                <div class="vh_comparison_others_items">
                        <div class="vh_comparison_others_img">
                            <div class="vh_comparison_others_title">
                                Lowest On Market</div>
                            <div class="vh_comparison_others_img_holder">
                                  <img src=" <%=Model.BelowThumbnailUrl%>" style="max-height:100%;height:auto;width:100%;max-width:100%;margin:auto;display:inline;">
                            </div>
                        </div>
                       <div class="vh_comparison_others_info"><%=((int)Model.MarketLowestPrice).ToString("n0")%></div>
                         <div class="vh_comparison_others_info">
                  <%=(Model.MarketLowestMileage).ToString("n0")%></div>
                        <div class="vh_comparison_others_info">
                            Nationwide</div>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
            </div>
        </div>
        <div class="tvehiclevalue">
           
            <div class="tstep1_top_title">
                Dealership Reviews
                <nobr class="dealership_review_vh"><%= SessionHandler.TradeInDealer.DealershipName%></nobr>
            </div>
            
            <div class="tvehiclevalue_content" >
                <div class="partialContents" data-url="<%= Url.Action("ReviewsData", "Trade", new { Model.DealerId }) %>">
                    <div class="data-content">Loading...</div>
                </div>
            </div>
        </div>

        <div class="tvehiclevalue_btns">
          <%--  <div class="tvehicle_btns_print">
                Print
            </div>--%>

            <div class="tradeIn_step_goto">
               <%= Html.ActionLink("Appraise Another Vehicle", "StepOneDecode", "Trade", new { @style = "font-color:red" })%>
            </div> 
        </div>
       <div class="tbacktotop">
            Top page</div>
    </div>
    <script type="text/javascript">
        $(".tbacktotop").click(function () {
            $("html, body").animate({ scrollTop: $('.tradeInHeader').offset().top }, 1000);
        });

        $(document).ready(function () {
            $(".partialContents").each(function (index, item) {
                var url = $(item).data("url");
                if (url && url.length > 0) {
                    $(item).load(url);
                }
            });
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
</asp:Content>
