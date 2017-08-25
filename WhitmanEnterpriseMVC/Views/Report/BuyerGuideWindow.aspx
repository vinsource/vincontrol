<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.BuyerGuideViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Buyer's Guide</title>
<script src="http://code.jquery.com/jquery-latest.js"></script>
<style type="text/css">
html {font-family:"Trebuchet MS", Arial, Helvetica, sans-serif; background: #222; color: #ddd;}
body {width: 500px; margin: 0 auto;}
#container {background: #333;  padding: 1em;}
h3, ul {margin: 0;}
input[type="text"] {width: 30px;}
span.label {display: block; width: 150px; float: left; clear: right;}
input[type="submit"] {background: #680000; border: 0; color: white; font-size: 1.1em; font-weight: bold; padding: .5em; float: right; margin-top: -2em;}
.short {width: 50px !important;}
</style>
</head>

<body>
<h1 style="margin-bottom: 0;">Buyer's Guide</h1>
<div id="container">
 <% Html.BeginForm("ViewBuyerGuideinHtml", "Report", FormMethod.Post); %>
<input type="hidden" name="warrantyInfo"  />
<h3>Warranty Info</h3>
<ul class="warranty">
	<li>Bumper-to-Bumper: <%=Model.WarrantyBumperYear %>/<%=Model.WarrantyBumperMiles %></li>
	<li>Drivetrain: <%=Model.WarrantyDrivetrainYear %>/<%=Model.WarrantyRoadsideMiles%></li>
	<li>Corrosion: <%=Model.WarrantyWearAndTearYear %>/<%=Model.WarrantyWearAndTearMiles%></li>
	<li>Roadside: <%=Model.WarrantyRoadsideYear %>/<%=Model.WarrantyRoadsideMiles%></li>
</ul>
<h3 style="margin-top: 1em;">Warranty</h3>
Select a Warranty Type: 
<%=Html.DropDownListFor(x=>x.SelectedWarranty,Model.Warranties) %>

<div id="limited">
<h3 style="margin-top: 1em;">Limited Warranty</h3>
The dealer will pay:<br  />
<span class="short label">Parts: </span>
<%=Html.EditorFor(x=>x.PartsPercentage) %> % 
<br  />
<span class="short label">Labor: </span>
<%=Html.EditorFor(x=>x.LaborPercentage) %> %
</div>
<h3 style="margin-top: 1em;">Coverage</h3>
<span class="label">Bumper to Bumper:</span>
<%=Html.EditorFor(x=>x.Bumper) %> 
    <%=Html.DropDownListFor(x=>x.SelectedDurationForBumper,Model.Duration) %><br  />
<span class="label">Drivetrain:</span> 
<%=Html.EditorFor(x=>x.Drivetrain) %> 
    <%=Html.DropDownListFor(x=>x.SelectedDurationForDriveTrain,Model.Duration) %><br  />
<span class="label">Corrosion:</span> 
<%=Html.EditorFor(x=>x.Wear) %> 
   <%=Html.DropDownListFor(x=>x.SelectedDurationForWear,Model.Duration) %><br  />
<span class="label">Roadside:</span> 
<%=Html.EditorFor(x=>x.Roadside) %> 
   <%=Html.DropDownListFor(x=>x.SelectedDurationForRoadSide,Model.Duration) %><br  />
    <%=Html.HiddenFor(x=>x.ListingId) %>
<input type="submit" name="submit" onClick="parent.close()" value="Print Guide"  />
 <% Html.EndForm(); %>
</div>
<script type="text/javascript" language="javascript">
    $("#SelectedWarranty").change(function() {
     
        $thisval = $('#SelectedWarranty>option:selected').val();
        if ($thisval == 'Full') {
            $("#PartsPercentage").val("100");
            $("#LaborPercentage").val("100");
        }
        else {
            $("#PartsPercentage").val("");
            $("#LaborPercentage").val("");
        }


    });
</script>
</body>
</html>