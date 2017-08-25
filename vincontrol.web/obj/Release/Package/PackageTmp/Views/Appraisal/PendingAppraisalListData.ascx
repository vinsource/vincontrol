<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AppraisalListViewModel>" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<style type="text/css">
    .disable
    {
        display: none;
    }
</style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".sForm").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
            $(".appraisals_number_below").html(<%=Model.UnlimitedAppraisals.Count() %>);
            $(".SalePriceForm").validationEngine({ promptPosition: "bottomLeft", scroll: false });
        });
        function checkSalePrice(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "Price should <= $100,000,000";
            }
        }

        function checkMiles(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 2000000) {
                return "Miles should <= 2,000,000";
            }
        }
    </script>


<div class="vin_listVehicle_holder">
   <% var index = 0; %>
       <% foreach (var tmp in Model.UnlimitedAppraisals)
          {
              if (index%2 == 0)
              {
       %>
<div class="contain_list list_even">
      <% }
              else
              { %>
      <div class="contain_list">
      <% } %>
           <a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>" >
            </a><div class="right_content_items right_content_items_img"><a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
                </a><a href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
                        <img alt="" height="61" src="<%= String.IsNullOrEmpty(tmp.DefaultImageUrl)?tmp.PhotoUrl : tmp.DefaultImageUrl %>" width="65">
                    </a>
            </div>

        <a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
            <div class="right_content_items right_content_items_vin nav_long">
                <% if (tmp.VinNumber != null)
                   {
                		 %><%= tmp.VinNumber.Length < 8 ? tmp.VinNumber : tmp.VinNumber.Substring(tmp.VinNumber.Length - 8, 8) %><%
                   }%>
            </div></a>

        <a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
            <div class="right_content_items nav_middle">
             <%= tmp.ModelYear %>
            </div></a>
        <a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
            <div class="right_content_items nav_long">
                  <%= !String.IsNullOrEmpty(tmp.Make) && tmp.Make.Length > 8 ? tmp.Make.Substring(0, 8) + "..." : tmp.Make%>
            </div></a>
        <a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
            <div class="right_content_items nav_long">
                   <%= !String.IsNullOrEmpty(tmp.AppraisalModel) && tmp.AppraisalModel.Length > 7 ? tmp.AppraisalModel.Substring(0, 7) + "..." : tmp.AppraisalModel%>
            </div></a>
        <a class="vin_editProfile" href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {AppraisalId = tmp.AppraisalID}) %>">
            <div class="right_content_items nav_long">
                    <%= !String.IsNullOrEmpty(tmp.Trim) && tmp.Trim.Length > 7 ? tmp.Trim.Substring(0, 7) + "..." : tmp.Trim%>
            </div></a>
        <div class="right_content_items nav_middle">
                 <%= !String.IsNullOrEmpty(tmp.ExteriorColor) && tmp.ExteriorColor.Length > 5 ? tmp.ExteriorColor.Substring(0, 5) + "..." : tmp.ExteriorColor%>
        </div>
        <div class="right_content_items right_content_items_owners nav_short">
            <%= tmp.StrCarFaxOwner %> 
        </div>
        <div class="right_content_items right_content_items_client">
              <%= tmp.StrClientName %> 
        </div>
        <div class="right_content_items right_content_items_appraiser nav_long">
             <%= tmp.AppraisalBy%> 
        </div>
        <form name="SaveSalePriceForm" class="SalePriceForm">
            <div class="right_content_items nav_middle_Appraisal vehicle_miles_appraisal divValue">
                <input type="text" id="<%= tmp.AppraisalID %>" name="odometer" class="sForm" value="<%=tmp.Mileage.ToString("#,##0") %>" data-validation-engine="validate[funcCall[checkMiles]]">
                <img class="imgLoadingPendingAppraisal" src="../Content/images/ajaxloadingindicator.gif" height="15px"/>
            </div>
            <div class="right_content_items nav_long vehicle_price divValue">
                <input type="text" id="<%= tmp.AppraisalID %>" name="Acv" class="sForm" value="<%=tmp.ACV.HasValue ? tmp.ACV.Value.ToString("#,##0") : "" %>" data-validation-engine="validate[funcCall[checkSalePrice]]">
                <img class="imgLoadingPendingAppraisal" src="../Content/images/ajaxloadingindicator.gif" height="15px"/>
            </div>
        </form>
        <div class="invent_expanded_date_holder invent_expanded" style="display: block;">
                <label class="invent_expanded_date">
                <%=tmp.DateOfAppraisal%>
                </label>
            </div>
        <% if (SessionHandler.IsEmployee == false) %>
        <% { %>
        <div class="invent_expanded_action_holder invent_expanded" style="display: block;">
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Recon_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>   ',<%=tmp.AppraisalID %>,4)">
                    Recon
                </label>
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Inventory_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>    ',<%=tmp.AppraisalID %>,2)">
                    Inventory
                </label>
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Wholesale_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>    ',<%=tmp.AppraisalID %>,3)">
                    Wholesale
                </label>
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Auction_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>    ',<%=tmp.AppraisalID %>,5)">
                    Auction
                </label>
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Loaner_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>    ',<%=tmp.AppraisalID %>,6)">
                    Loaner
                </label>
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Trade_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>    ',<%=tmp.AppraisalID %>,7)">
                    Trade Not Clear</label>
                <label class="invent_expanded_recon">
                    <input type="checkbox" id="Sold_chk<%=tmp.AppraisalID %>" class="chk chk<%=tmp.AppraisalID %>" onclick="ChangeStatus('<%=tmp.VinNumber %>    ',<%=tmp.AppraisalID %>,1)">
                    Sold
                </label>
                <% if (tmp.InventoryId != 0)
                   {
                       %>
                         <label class="invent_expanded_recon">
                            <a href="<%=Url.Action("ViewIProfile", "Inventory", new {ListingId = tmp.InventoryId}) %>"><div class="btns_shadow loAppSeeInventory">See in Inventory</div></a>
                        </label>
                        <%
                   } %>
                
            </div>
        <% } %>
    </div>
    <%
        index++;
          } %>
</div>
</div>
