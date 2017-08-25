<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedTrim, Model.TrimsList) %>
<%= Html.HiddenFor(x=>x.SelectedTrimValue) %>