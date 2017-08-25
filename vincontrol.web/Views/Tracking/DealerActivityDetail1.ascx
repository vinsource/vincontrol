<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Vincontrol.Web.Models.DealershipActivityViewModel>>" %>
<%@ Import Namespace="Vincontrol.Web.HelperClass" %>
<%@ Import Namespace="vincontrol.Constant" %>

<script type="text/javascript">
    $(document).ready(function () {

         <%if ((short)ViewData["Type"] == Constanst.ActivityType.Inventory)
           {%>
        $(".activity_display").removeClass("activity_display");
        $("#inventory_tab").html(<%=Model.Count() %>);
        <%}%>
            
         <%if ((short)ViewData["Type"] == Constanst.ActivityType.Appraisal)
           {%>
        $(".activity_display").removeClass("activity_display");
        $("#appraisals_tab").html(<%=Model.Count() %>);
        <%}%>
       
         <%if ((short)ViewData["Type"] == Constanst.ActivityType.User)
           {%>
        $(".activity_display_none").addClass("activity_display");
        $("#user_tab").html(<%=Model.Count() %>);
        <%}%>
        
        <%if ((short)ViewData["Type"] == Constanst.ActivityType.ShareFlyer || (short)ViewData["Type"] == Constanst.ActivityType.ShareBrochure)
          {%>
        //         $(".activity_display_none").addClass("activity_display");
        $(".activity_display").removeClass("activity_display");

        $("#shareflyer_tab").html(<%=Model.Count() %>);
        <%}%>
        

        <% if (Model.Count() == 0 && (bool)ViewData["IsFirstime"])
           { %>
            $('#detailDealerActivity').hide();
            $('#detailDealerActivityError').show();
        <% } %>
        <% else
           {%>
            $('#detailDealerActivity').show();
            $('#detailDealerActivityError').hide();
        <%   } %>
    });
</script>

<% int i = 0;
%>
<% foreach (var item in Model)
   {
       i++;
%>
<% if (i % 2 == 1) %>
<%
   {
%>

<input type="hidden" id="hdType" value="<%=ViewData["Type"] %>" />
<div class="activity_list_items">
    <% }
   else
   { %>
    <div class="activity_list_items activity_list_odd">
        <%
   }
        %>
        <div class="activity_list_collumn">
            <div class="activity_list_first">
                <%= (string.IsNullOrEmpty(item.UserStamp) || string.IsNullOrEmpty(item.UserStamp.Trim())) ? "&nbsp;" : item.UserStamp%>
            </div>
        </div>
        <div class="activity_list_collumn activity_datestock">
            <div class="activity_list_second">
                <%= item.DateStamp.ToString("MM/dd/yyyy hh:mm:ss tt") %>
            </div>
        </div>

        <div class="activity_display_none">

            <div class="activity_list_collumn">
                <div class="activity_list_first">

                    <%= (string.IsNullOrEmpty(item.Vehicle) || string.IsNullOrEmpty(item.Vehicle.Trim())) ? "&nbsp;" : item.Vehicle%>
                </div>
            </div>
            <div class="activity_list_collumn activity_vin">
                <div class="activity_list_second">
                    <%= (string.IsNullOrEmpty(item.Vin) || string.IsNullOrEmpty(item.Vin.Trim())) ? "&nbsp;" : (!String.IsNullOrEmpty(item.Vin) && item.Vin.Length > 8 ? item.Vin.Substring(item.Vin.Length - 8, 8) : item.Vin)%>
                </div>
            </div>
            <div class="activity_list_collumn activity_stock">
                <div class="activity_list_second">
                    <%= (string.IsNullOrEmpty(item.Stock) || string.IsNullOrEmpty(item.Stock.Trim())) ? "&nbsp;" : item.Stock%>
                </div>
            </div>
        </div>


        <div class="activity_list_collumn">
            <div class="activity_list_first">
                <%= (string.IsNullOrEmpty(item.Activity) || string.IsNullOrEmpty(item.Activity.Trim())) ? "&nbsp;" : item.Activity%>
            </div>
        </div>

        <div class="activity_list_collumn activity_list_collumn_content">
            <div class="activity_list_second activity_list_content_collumn">
                <%= (string.IsNullOrEmpty(item.Title) || string.IsNullOrEmpty(item.Title.Trim())) ? "&nbsp;" : item.Title%>
            </div>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <% } %>
    <input type="hidden" id="hdStartYear" value="<%=ViewData["StartYear"] %>" />
    <input type="hidden" id="hdStartMonth" value="<%=ViewData["StartMonth"] %>" />
    <input type="hidden" id="hdStartDay" value="<%=ViewData["StartDay"] %>" />
