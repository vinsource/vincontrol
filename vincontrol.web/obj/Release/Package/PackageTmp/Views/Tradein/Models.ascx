<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedModel, Model.ModelsList) %>
<%= Html.HiddenFor(x=>x.SelectedModelValue) %>