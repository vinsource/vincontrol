<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/TradeIn.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.TradeInManagement.TradeInVehicleModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trade In | Step 1
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeaderHolder" runat="server">
     <script src="<%=Url.Content("~/Scripts/Landing/tradein3step.js")%>" type="text/javascript"></script>
    <div class="tradeInHeader_steps">
    </div>
    <div class="tradeInHeader_step1Btn">
    </div>
    <div class="tradeInHeader_step2Btn">
    </div>
    <div class="tradeInHeader_step3Btn">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("TradeInWithOptions", "Trade", FormMethod.Post, new { id = "TradeVehicleForm", onsubmit = "return validateForm()" }); %>
    <input id="DealerName" name="DealerName" type="hidden" value="<%= Model.DealerName %>" />
    <div id="tradeIn_step1_holder">
        <div class="tradeIn_step1_top">
            <div class="tstep1_top_title">
                Thinking Of Trading In Your Vehicle?
            </div>
            <div class="tstep1_top_content">
                Select from the dropdown choices, then fill out the rest
                of the form and you will be on your way to knowing your vehicle's trade-in value!
            </div>
        </div>
        <div class="tradeIn_step1_bottom">
            <div class="tstep1_top_title">
                Vehicle Information
            </div>
            <div class="tstep1_content_holder">
                <div class="tstep1_vehicle_information">
                    <div class="tstep1_VH_img">
                        <img src="<%=Url.Content("~/Content/Images/TradeInStep/img.png")%>">
                    </div>
                    <div class="tstep1_vh_info_holder" id="decode">
                        <div class="error-wrap">
                            <p class="error" title="Click to Close">
                            </p>
                        </div>
                       <%-- <div class="tstep1_vh_info_items">
                            <label>Vehicle VIN</label>
                            <input type="text" id="Vin" name="Vin" placeholder="VIN Number" value="<%= Model.Vin %>" />
                        </div>--%>
                       <%-- <div class="tstep1_or">
                            - Or -</div>--%>
                        <div class="tstep1_vh_info_items">
                            <label>Year</label>
                            <span id="divYears"><%= Html.Partial("Years", Model) %></span>
                        </div>
                        <div class="tstep1_vh_info_items">
                            <label>Make</label>
                            <span id="divMakes"><%= Html.Partial("Makes", Model) %></span>
                        </div>
                        <div class="tstep1_vh_info_items">
                            <label>Model</label>
                            <span id="divModels"><%= Html.Partial("Models", Model) %></span>
                        </div>
                        <div class="tstep1_vh_info_items">
                            <label>Trim</label>
                            <span id="divTrims"><%= Html.Partial("Trims", Model) %></span>
                        </div>
                        <div class="tstep1_vh_info_items">
                            <label>Mileage</label>
                            <%= Html.TextBoxFor(x => x.Mileage, new {title="Mileage", placeholder="Mileage"}) %>
                        </div>
                        <div class="tstep1_vh_info_items">
                            <label>Condition</label>
                            <select id="Condition" name="Condition">
                                <option value="Poor">Poor</option>
                                <option value="Fair" selected="true">Fair</option>
                                <option value="Great">Great</option>
                            </select>
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>
                    <a id="step2" class="tradeIn_links" href="javascript:;">
                        <div class="tradeIn_step_goto">Go to Step 2</div>
                    </a>
                    
                </div>
            </div>
        </div>
    </div>
    <%Html.EndForm(); %>
    <script type="text/javascript">
        $(".tradeInHeader_step1Btn").addClass("tradeInHeader_step1Btn_active");
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">

</asp:Content>
