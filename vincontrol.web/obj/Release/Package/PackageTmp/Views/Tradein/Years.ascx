<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedYear, Model.YearsList) %>
