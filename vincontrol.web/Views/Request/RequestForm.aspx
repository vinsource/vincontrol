<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.ClientInfoModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
 <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
  <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
  
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<style>
.multi-line { height:8em; width:40em; }
</style>
<title>Request Form</title>
</head>

<body>
 <script>
     $(document).ready(function() {
         $("#datepicker").datepicker();
     });
  </script>
<table align="center">
<tr><td colspan="2"><div id="logo"><img width="100%" height="200" src="http://vincontrol.com/alpha/cListThemes/dealerImages/logoLong.jpg" /></div></td></tr>
 <% Html.BeginForm("SubmitClientRequest", "Request", FormMethod.Post ); %>
<tr><td colspan="2"><h3><%= Html.DisplayTextFor(x=>x.Stock) %> - <%= Html.DisplayTextFor(x => x.Year)%>   <%= Html.DisplayTextFor(x => x.Make)%>    <%= Html.DisplayTextFor(x => x.Model)%>   <%= Html.DisplayTextFor(x => x.Trim)%>  </h3></td></tr>
<tr><td><table style="float:left; clear:right; margin-right: 1em;">
  
    <tr><td>First Name:</td> <td> <%=Html.EditorFor(x=>x.FirstName) %></td><tr>
    <tr><td>Last Name:</td> <td> <%=Html.EditorFor(x=>x.LastName) %></td><tr>
    <tr><td>Email:</td> <td><%=Html.EditorFor(x=>x.Email) %></td><tr>
    <tr><td>Phone:</td> <td><%=Html.EditorFor(x=>x.Phone) %></td><tr>
     
    <tr><td><%=Html.HiddenFor(x=>x.Trim) %></td><tr>
    <tr><td><input id="Model" name="Model" type="hidden" value="<%=Model.Model %>" /></td><tr>
    <tr><td><%=Html.HiddenFor(x=>x.Year) %></td><tr>
    <tr><td><%=Html.HiddenFor(x=>x.Make) %></td><tr>
    <tr><td><%=Html.HiddenFor(x=>x.Vin) %></td><tr>
    <tr><td><%=Html.HiddenFor(x=>x.Stock) %></td><tr>
    <tr><td><%=Html.HiddenFor(x => x.DealerID)%></td><tr>
    
    
    <tr><td colspan="2">Comments:<br /><%=Html.EditorFor(x=>x.Comment) %></td></tr>
    <tr><td colspan="2"><input style="float: right;" type="submit" name="submit" value="Send" /></td></tr>
    <tr><td colspan="2"><font size="5"><%=Html.DisplayTextFor(x => x.Submit)%></font></td></tr>
    
</table></td><td valign="top">
<table>
	<tr><td colspan="2">If you would like to schedule a test drive:</td><tr>
	<tr><td>Date: </td> <td><%=Html.EditorFor(x=>x.Date) %></td><tr>
    <tr><td>Time:</td> <td><%=Html.EditorFor(x=>x.Time) %></td><tr>
 <% Html.EndForm(); %>
</td></tr>
</table>
</body>

</html>