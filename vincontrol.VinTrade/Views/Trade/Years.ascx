<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.Application.ViewModels.TradeInManagement.TradeInVehicleModel>" %>

<%= Html.DropDownListFor(x => x.SelectedYear, Model.YearsList.Select(i => new SelectListItem() { Selected = i.Selected, Text = i.Text, Value = i.Value }))%>
