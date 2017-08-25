<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.VariantCodeViewModel>" %>


    <h2>
        Trade In Price Variance</h2>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <%-- <% using (Html.BeginForm()) {%>--%>
    <fieldset>
        <legend>Variance</legend>
        <p>
            <label for="Variance">
                Variance:</label>
            <%= Html.TextBox("Variance", Model.Variance) %>
            <%= Html.ValidationMessage("Variance", "*") %>
        </p>
        <p>
            <input type="button" id="btnsubmit" value="Save" />
        </p>
    </fieldset>
    <%--<% } %>--%>
   

     