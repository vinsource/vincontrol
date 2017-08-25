<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.VariantCodeViewModel>" %>  
   
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <%-- <% using (Html.BeginForm()) {%>--%>
   
            <label for="Variance">
                Variance:</label>
            <%= Html.TextBox("Variance", Model.Variance) %>
            <%= Html.ValidationMessage("Variance", "*") %>
      
            <input type="button" id="btnsubmit" value="Save" />
            <label style="color:red; display:none;" id="lbVarianceValidate"> Number only </label>
       
   