<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.CarInfoFormViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Transfer Vehicle</title>
<link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
<script src="http://code.jquery.com/jquery-latest.js"></script>
<style type="text/css">
	input, textarea {margin-bottom: 1em;}
	#transfer-container {background: #333; width: 100%; padding: 1em; margin: 0 auto;}
	body {text-align: center; background: #222; padding: 0; color: #eee; font-family:"Trebuchet MS", Arial, Helvetica, sans-serif; width: 350px; margin: 0 auto}
	h1, h2, h3, h4, h5, h6 {color: #fff; margin-left: 1em;}
	h4 {margin: 0; margin-left: 1em;}
	input[type="submit"] {background: #680000; border: 0; color: white; font-size: 1.1em; font-weight: bold; padding: .5em;  margin-top: -2em;}
    input[type="button"] {background: #680000; border: 0; color: white; font-size: 1.1em; font-weight: bold; padding: .5em; margin-top: -2em;}
	
	.hideLoader {display: none;}
</style>

<body>
<%if (Model.SessionTimeOut)
  { %>
 <script>
     parent.$.fancybox.close();
     alert("Your session is timed out. Please login back again");
     var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
     window.parent.location = actionUrl;

</script>
<% }%>
<script type="text/javascript">
 
    $(document).ready(function() {

        //Setup the AJAX call
        $("#SwitchDealerForm").submit(function (event) {

    $("#elementID").removeClass("hideLoader");
            event.preventDefault();

            SwitchDealer(this);


        });

    });


    function SwitchDealer(form) {

       

        $.ajax({

            url: form.action,

            type: form.method,

            dataType: "json",

            data: $(form).serialize(),

            success: SwitchDealerClose

        });


    }

    function SwitchDealerClose(result) {

        if (result == "Success") {
            parent.$.fancybox.close();

            var actionUrl = '<%= Url.Action("ViewInventory", "Inventory" ) %>';

            window.parent.location.href = actionUrl;
        }
       

    }

    </script>
<div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajax-loader1.gif")%>" alt="" />
</div>
<h2>Switch Dealership</h2>
<div id="transfer-container">
<% %>
	<% Html.BeginForm("SwitchDealership", "Switch", FormMethod.Post, new { id = "SwitchDealerForm", name = "SwitchDealerForm" }); %>
<span style="font-size: 1.1em; margin-top: -10px;"></span><br />
<span>Current Location: </span><span><%=Model.DealershipName%></span><br /><br />
 <%=Html.DropDownListFor(x=>x.SelectedDealerTransfer,Model.TransferDealerGroup,new {@width=200}) %>
<br />
<br />
<br />
<input type="submit" name="submit"  value="Switch" /> 
<input type="button" name="Cancel" onClick="parent.$.fancybox.close()" value="Cancel"  />
<% Html.EndForm(); %>
</div>
</body>