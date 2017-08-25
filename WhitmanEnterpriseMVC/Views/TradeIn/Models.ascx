<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedModel, Model.ModelsList) %>
<%= Html.HiddenFor(x=>x.SelectedModelValue) %>