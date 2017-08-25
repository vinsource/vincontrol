<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.Vinsocial.ViewModels.ReviewManagement.DealerReviewViewModel>" %>
<% if (Model == null || Model.UserReviews == null) {%> 
<div class="data-content">No data</div>
<%} else {%>
<div class="data-content">
   
        <% foreach (var tmp in Model.UserReviews)
           {
  %>

             <div class="dealership_review review_items_odd">
                    
                    <p class="dealership_review_content">
                       <%=tmp.Comment %>
                    </p>
                    <label class="dealership_review_name">
                        <%=tmp.Author %></label>
                </div>
  

  <%         } %>

    
</div>
<%}%>
