<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WhitmanEnterpriseMVC.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedYear, Model.YearsList) %>
