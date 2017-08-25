<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedMake, Model.MakesList) %>
<%= Html.HiddenFor(x=>x.SelectedMakeValue) %>
