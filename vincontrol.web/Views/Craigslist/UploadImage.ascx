<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<VINControl.Craigslist.PostingPreview>" %>

<% if (!string.IsNullOrEmpty(Model.Warning)) {%>
<div style="display: inline-block; width: 100%; color: red; font-weight: bold; padding-bottom: 10px;">
    <%= Model.Warning %>
</div>
<%}%>

<% if (Model.Post != null) {%>
<input type="hidden" id="listingId" name="listingId" value="<%= (string)ViewData["ListingId"] %>"/>
<input type="hidden" id="locationUrl" name="locationUrl" value="<%= (string)ViewData["LocationUrl"] %>"/>
<input type="hidden" id="cryptedStepCheck" name="cryptedStepCheck" value="<%= (string)ViewData["CryptedStepCheck"] %>"/>
<input type="hidden" id="postingTitle" name="postingTitle" value="<%= (string)ViewData["PostingTitle"] %>"/>
<div><%= Model.Post.Location %> > <%= Model.Post.SubLocation %> > <%= Model.Post.Type %> > <%= Model.Post.Category %></div>
<div>
    <h2><%= Model.Post.Title %> - <%= Model.Post.SalePrice.ToString("c0")%> (<%= Model.Post.SpecificLocation %>)</h2>
</div>
<div style="display: inline-block; width: 100%;">
    <div style="float: left; width: 400px;">
        <% foreach (var item in Model.Post.Images)
           {%>
        <img src="<%= item %>" style="width: 88px; height: 88px" />
        <%}%>
    </div>
    <div style="">
        <div style="display: inline-block; padding: 2px; margin-bottom: 2px; border: 1px solid #808080"><b><%= Model.Post.Year %> <%= Model.Post.Make %> <%= Model.Post.Model %></b></div>
        <div style="display: inline-block; padding: 2px; margin-bottom: 2px; border: 1px solid #808080">Odometer: <%= Model.Post.Odometer %></div>
        <div style="display: inline-block; padding: 2px; margin-bottom: 2px; border: 1px solid #808080">VIN: <%= Model.Post.Vin %></div>
        <div style="display: inline-block; padding: 2px; margin-bottom: 2px; border: 1px solid #808080"><%= Model.Post.Transmission %></div>
    </div>
</div>
<div><%= Model.Post.Body %></div>
<div>
    <ul>
        <li>Location: <%= Model.Post.SpecificLocation %></li>
        <li><%= Model.Post.Note %></li>
    </ul>
</div>
<% if (string.IsNullOrEmpty(Model.Warning)) {%>
<div style="display: inline-block; width: 100%; float: right">
    <input type="button" id="btnContinue" value="Continue" style="float: right; padding: 6px 10px; cursor: pointer;" />
</div>
<%}%>
<%}%>


