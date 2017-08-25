<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedMake, Model.MakesList) %>
<%= Html.HiddenFor(x=>x.SelectedMakeValue) %>
