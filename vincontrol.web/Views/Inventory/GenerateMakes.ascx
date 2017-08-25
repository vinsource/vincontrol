<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AdvancedSearchViewModel>" %>

<%= Html.DropDownListFor(m => m.SelectedMake, Model.Makes, "----", new Dictionary<string, object>() { {"style","width:105px"} }) %>