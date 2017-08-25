<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.EbayFormViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Ebay Template</title>
<!-- jQuery library - I get it from Google API's -->    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<style type="text/css">
	#confirm div {
		display:block;
		overflow: hidden;}
		
	#confirm {
			text-align: center;
			color: white;
			width: 750px;
			margin: 0 auto;}
			
	#confirm-wrapper {
		text-align:left;
		background: #111;}
		
	#confirm-innerwrap {
		margin: 10px;
		background: #444;
		padding: 10px;}
		
	#confirm .btn {
		font-size: 1.2em;
		font-weight: bold;
		border: 0;
		background: #860000;
		color: white;
		padding: .5em .7em .5em .7em !important;}
		
	#confirm .btn:hover {
		background: #350001;}
		
	#confirm #pricing {
		width: 300px;
		text-align:center;
		display: inline-block;}
		
	#confirm #buttons {
		display: inline-block;
		margin-top: 8px;
		float: right;}	
		
		.hideLoader {display: none;}
		
</style>
</head>

<body>
 <script type="text/javascript" >
     $(document).ready(function() {


         $("#PostEbayForm").submit(function(event) {

            $('#elementID').removeClass('hideLoader');

             event.preventDefault();


             PostEbay(this);

         });

     });
     
     function PostEbay(form) {



         $.ajax({

             url: form.action,

             type: form.method,

             dataType: "json",

             data: $(form).serialize(),

             success: PostEbayClose

         });


     }

     function PostEbayClose(result) {
         var actionUrl;
         $('#elementID').addClass('hideLoader');
         if (result == "SessionTimeOut") {
             alert("Your session is timed out. Please login back again");
             actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
             window.parent.location = actionUrl;

         } else if (result == "Fail") {
             alert("There is an problem with ebay posting ads. Please contact 1-855-VINCONTRL for support.");
             actionUrl = '<%= Url.Action("ViewInventory", "Inventory"   ) %>';
             window.location.href = actionUrl;

         } else {

             alert("Thanks for using ebay posting intergration. Your ads is just posted on ebay successfully");
            
             
             actionUrl = '<%= Url.Action("ViewIProfile", "Inventory", new { ListingId = "PLACEHOLDER"  } ) %>';
             actionUrl = actionUrl.replace('PLACEHOLDER', $("#ListingId").val());

             newPopup(result);
             window.location.href = actionUrl;

         }

     }

     function newPopup(url) {
         return window.open(url, "Ebay from Vincontrol", 'height=768,width=1366,left=10,top=10,titlebar=no,toolbar=no,menubar=no,location=no,directories=no,status=no');
     }
 </script>
 <div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajax-loader1.gif")%>" alt="" />
</div>
	<div id="confirm">
	 <% Html.BeginForm("PostEbayAds", "Market", FormMethod.Post, new { id = "PostEbayForm", target = "_blank" }); %>
    	<h1>Confirm eBay Ad Posting</h1>
        <div id="confirm-wrapper">
        	<div id="confirm-innerwrap">
            	<div id="pricing">
                    Final eBay Posting Price:
                    <h1 style=" margin: 0;"><%=Model.TotalListingFee %></h1>
                    
                </div>
                <div id="buttons">
                	<input class="btn" type="submit" value="Complete Ad" />
                	<%=Html.ReportButtonEbayGroup() %>
                    <%=Html.HiddenFor(x => x.ListingId)%>
                                                         
        
                </div>
            </div>
        </div>
   <% Html.EndForm(); %>
    </div>
<!-- START OF TEMPLATE -->
    <div id="template">
    
    
    
        <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <html xmlns="http://www.w3.org/1999/xhtml">
        <head>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"></script>
		<script src="http://bxslider.com/sites/default/files/jquery.bxSlider.min.js" type="text/javascript"></script>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>Ebay Title Goes Here</title>
        <style type="text/css">
			body {
				width: 100%; 
				background: #222222; 
				font-family:
					"Trebuchet MS", 
					Arial, 
					Helvetica, 
					sans-serif;}
				
			ul.unstyled {
				list-style-type: none; 
				margin: 0; 
				padding: 0;}	
			
			p {
				padding: 0 15px 0 15px;}
			
			h3.header{
				padding-bottom: 8px;}
			
			div {
				display: block;}
				
			.header {
				background: #900508; 
				color: white; 
				padding: 
					.2em 
					.5em 
					.2em 
					.5em; 
				margin-bottom: 0;
				margin-top: 0;}
			
			.pad {
				margin: 10px;
				overflow:hidden;
				background: #ddd;
				margin-bottom: 20px;
				position: relative;
				padding-bottom: 35px;}
			
			#description .pad,
			#summary .pad {
				padding: 0;}
				
			.pad .return {
				width: 150px !important;
				position: absolute;
				bottom: 5px;
				right: 5px;}
			
			#title .header {
				color: white;
				font-size: 1.5em;}
			
			.header.bg {
				background: url('http://vincontrol.com/alpha/ebayimages/header-repeat.jpg') bottom left repeat-x;}
				
			#outerwrap {
				width: 770px; 
				margin: 0 auto; 
				background: #000;
				overflow:hidden;
				margin-top: 50px;
				margin-bottom: 50px;}
				
			#innerwrap {
				overflow: hidden;
				margin: 15px; 
				background: #444;
				padding-bottom: 10px;}
			
			
			#title {
				border-bottom: 
					2px 
					#000000 
					solid;}
				
			#dealer-banner {
				color: white;
				width: 100%; 
				background: #000;
				overflow: hidden;}
				
			#dealer-banner img {
				max-width: 350px;
				}
			
			#dealer-banner-phone {
				text-align:right;
				float: right;
				padding-right: 20px;}
			
			#dealer-banner-phone h5 {
				margin-bottom: 0;}
				
			#dealer-banner-phone h1 {
				margin-top: 0;}
			
			#topnav {
				height: 30px !important;}
				
			#topnav, 
			#topnav-innerwrap {
				height: 100%;
				overflow:hidden;
				margin: 0; 
				padding: 0; 
				width: 100% !important;
				background:
					url('http://vincontrol.com/alpha/ebayimages/navbg.jpg') 
					bottom 
					left 
					repeat-x;}
				
			#topnav ul {
				height: 100%;
				margin: 0; 
				padding: 0;}
				
			#topnav li {
				height: 22px; 
				margin: 0; 
				padding: 0; 
				padding-top: 1px;
				list-style-type:none; 
				float: left; 
				clear: right;
				border-right: 
					1px 
					#333 
					solid;
				border-left: 
					1px 
					#000 
					solid;}
			
			#topnav li.end {
				border-right: none;}
				
			#topnav li.start {
				border-left: none;}
				
			#topnav li a {
				height: 100%;
				color: #ffffff;
				text-decoration:none; 
				margin: 
					.2em 
					.5em 
					.2em 
					.5em;}
				
			#topnav li a:hover {
				color: #900508;}
			
			
			#topnav #facebook-ico img{
				height: 25px;
				position: relative;
				left: 15px;}	
			
			#summary {
				margin-top: 20px;}
			
			#summary #img {
				width: 450px;
				height: 300px;
				overflow:hidden;
				float: left;
				display: inline-block;}
			
			#summary #img img {
				width: 100%;}
				
			#information {
				font-size: .9em;
				display: inline-block;
				width: 270px;}
				
			#information ul {
				margin: 15px;}
			
			a.btn {
				font-size:1.1em;
				text-align: center;
				text-decoration:none;
				color: white;
				background: 
					url('http://vincontrol.com/alpha/ebayimages/navbg.jpg') 
					top 
					left 
					repeat-x;
				border-radius: 4px;
				padding: 
					.2em 
					.5em 
					.2em 
					.5em;
				margin: 5px;
				display: block;
				color: white;
				}
					
			a.btn:hover {
				background: 
					url('http://vincontrol.com/alpha/ebayimages/header-repeat.jpg') 
					top 
					left 
					repeat-x;}
			
			#packages {
				font-size: .9em;}
				
			#packages-innerwrap h3 {
				margin-left: 15px; 
				margin-bottom: 0;}
				
			#packages ul {
				margin-top: 0; 
				width: 250px;}
			
			#packages #option-list,
			#packages #package-list {
				display: inline-block;}
				
			#packages #option-list {
				margin-left: 10px; 
				float: left;}
			
			#images-innerwrap {padding-bottom: 50px;}
				
			#images .imagelisting-wrap {
				width: 720px;
				height: 500px !important;}
				
			#warranty p,	
			#terms p{
				font-size: .8em;}
				
			#brand {
				color: #888;
				text-align:center;
				padding: 2em;}
			#brand a {
				font-size: 1.5em;
				font-weight: bold;
				text-decoration:none;}
				
			#carfaxWrap {
				width: 400px;
				margin: 0 auto;
				margin-bottom: 30px;}
				
			table, 
			td, 
			tr {
				table-layout:fixed; 
				overflow: hidden; 
				padding: 0 !important; 
				margin: 0 !important;}
				
            table.image, 
			table.image td {
				overflow: hidden; 
				width:100% !important; 
				height:100% !important; 
				background: #ddd ;}
				
            table.image img {
				display: block; 
				margin: 0 auto; 
				max-width: 98% !important;
				max-height: 98% !important; 
				margin-left: auto;
				margin-right: auto;}​
			a img {border: none;}	
			
			.image-small-wrap {
				width: 120px;
				height: 90px;
				display: inline-block;
				float: left;
			}

			.image-small-wrap {
				text-align: center;
				margin: 0;
				padding: 0;
				margin-bottom: 40px;
			}

			.image-small-wrap a {
				text-decoration: none;
				color: #960000;
				font-weight: bold;
				font-size: .8em;
				margin: 2px;
				margin-top: 0;
			}

			table.image {
				margin-right: 5px;
			} 

			a.bx-next {
				background: #111;
				padding: 1px 4px 3px 4px;
				text-decoration: none;
				color: white;
				font-weight: bold;
				position: absolute;
				top: 30px;
				right: -45px;
			}

			a.bx-next:hover,
			a.bx-prev:hover {
				color: red;
			}

			a.bx-prev {
				background: #111;
				padding: 1px 4px 3px 4px;
				text-decoration: none;
				color: white;
				font-weight: bold;
				position: absolute;
				top: 30px;
				left: -45px;
			}

			.bx-wrapper {
				width: 600px !important;
				margin: 0 auto;
			}
		</style>
        </head>
        
        <body>
            <div id="outerwrap" >
                <div id="innerwrap">
                	<div id="top" name="top"></div>
                	
                    
                    <!-- DEALER BANNER -->
                    <div id="dealer-banner">
                        <img alt="dealer-info-goes-here" src=<%=Model.Dealer.LogoUrl %> />
                        <div id="dealer-banner-phone">
                        	<h5> <%=Model.Dealer.ContactPerson %> </h5>
                            <h1> <%=Model.Dealer.Phone %> </h1>
                        </div>
                    </div>
                    
                    <!-- TITLE -->
                    <div id="title">
                        <h1 class="header"><%=Model.SellerProvidedTitle%></h1>
                    </div>
                    
                    <!-- TOPNAV -->
                    <div id="topnav">
                        <div id="topnav-innerwrap">
                            <ul id="nav">
                                <li class="start"><a href="#description">Description</a></li>
                                <li><a href="#packages">Packages &amp; Options</a></li>
                                <li><a href="#carfax">Carfax</a></li>
                                <li><a href="#images">Images</a></li>
                                <li><a href="#dealership">Dealer</a></li>
                                <li><a href="#warranty">Warranty</a></li>
                                <li><a href="#terms">Terms &amp; Conditions</a></li>
                                <li id="facebook-ico" class="end" style="margin:0 !important;"><a style="margin:0 !important;" href=<%=Model.Dealer.FacebookUrl %>><img border="0" alt="dealer-info-goes-here-facebook" src="http://vincontrol.com/alpha/cListThemes/dealerImages/facebook.jpg" /></a></li>
                            </ul>
                        </div>
                    </div>
                    
                    <!-- CAR SUMMARY -->
                    
                    <div id="summary">
                    	<h3 class="header bg"><%=Model.VehicleInfo.ModelYear %> <%=Model.VehicleInfo.Make %> <%=Model.VehicleInfo.Model %> <%=Model.VehicleInfo.Trim %> - <%=Model.Dealer.ContactPerson %> <%=Model.Dealer.Phone %></h3>
                    	<div id="summary-innerwrap" class="pad">
                        	<div id="img">
                            	<table class="image" style="vertical-align: middle;">
                                <tr><td style="vertical-align: middle;"><img alt="dealer-info-plus-car-info-here" src=<%=Model.VehicleInfo.SinglePhoto %> /></td></tr>
                            </table>
                            </div>
                            <div id="information">
                            	<ul class="unstyled">
                                	<li><b>VIN:</b> <%=Model.VehicleInfo.Vin %></li>
                                	<li><b>Stock:</b> <%=Model.VehicleInfo.StockNumber %></li>
                                	<li><b>Mileage:</b> <%=Model.VehicleInfo.Mileage %></li>
                                	<li><b>Ext. Color:</b> <%=Model.VehicleInfo.ExteriorColor %></li>
                                	<li><b>Int. Color:</b> <%=Model.VehicleInfo.InteriorColor %></li>
                                	<li><b>Trans:</b> <%=Model.VehicleInfo.Tranmission %></li>
                                	<li><b>Engine:</b>  <%=Model.VehicleInfo.Engine %></li>
                                </ul>
                                <a class="btn" href=<%=Model.Dealer.EbayInventoryUrl %>>View Our Inventory on eBay!</a>
                                <a class="btn" href=<%=Model.Dealer.CreditUrl %>>Financing</a>
                                <a class="btn" href=<%=Model.Dealer.WebSiteUrl %>>Visit Us Online!</a>
                                <a class="btn" href=<%=Model.Dealer.ContactUsUrl %>>Contact Us</a>
                            </div>
                        </div>
                    </div>
                    
                    
                    <!-- DESCRIPTION -->
                    
                    <div id="description" name="description">
                    	<h3 class="header bg">Description</h3>
                    	<div id="description-innerwrap" class="pad">
                        	<p><%=Model.VehicleInfo.Description %></p>
                        </div>
                    </div>
                    
                    
                    <!-- PACKAGES & OPTIONS -->
                    <%    int count = Model.VehicleInfo.StandardOptions.Count;
                        if(count>0){ %>
                    <div id="packages" name="packages">
                    	<h3 class="header bg">Standard Options</h3>
                    	
                        <div id="packages-innerwrap" class="pad">
                        	<div id="option-list">
                               
                                <ul>
                                <%
                                
                                    
                                    int index = count / 2;
                                    foreach (string tmp in Model.VehicleInfo.StandardOptions.Take(index))
                                  { %>
                                    <li><%=tmp %></li>
                                  
                                    <%} %>
                                </ul>
                            </div>
                            <div id="package-list">
                               
                                <ul>
                                     <%foreach (string tmp in Model.VehicleInfo.StandardOptions.Skip(index).Take(Math.Min(index,count-index)))
                                  { %>
                                    <li><%=tmp %></li>
                                  
                                    <%} %>
                                </ul>
                            </div>
                            <a href="#top" class="btn return">Back to top</a>
                        </div>
                        
                    </div>
                       <%} %>
                    <%   if (Model.VehicleInfo.ExistOptions!=null &&(Model.VehicleInfo.ExistOptions.Count != 0 || Model.VehicleInfo.ExistPackages.Count != 0))
                         { %>
                    <div id="packages" name="packages">
                    	<h3 class="header bg">Additional Options &amp; Packages</h3>
                    	
                        <div id="packages-innerwrap" class="pad">
                        	<div id="option-list">
                                <h3>Options</h3>
                                <ul>
                                      <%foreach (string tmp in Model.VehicleInfo.ExistOptions)
                                  { %>
                                    <li><%=tmp %></li>
                                  
                                    <%} %>
                                </ul>
                            </div>
                            <div id="package-list">
                                <h3>Packages</h3>
                                <ul>
                                         <%foreach (string tmp in Model.VehicleInfo.ExistPackages)
                                  { %>
                                    <li><%=tmp %></li>
                                  
                                    <%} %>
                                </ul>
                            </div>
                            <a href="#top" class="btn return">Back to top</a>
                        </div>
                        
                    </div>
                       <%} %>
                    <!-- CARFAX -->
                    
                    <div id="carfax" name="carfax">
                    	<h3 class="header bg">Carfax Vehicle History Report</h3>
                    	<div id="carfax-innerwrap" class="pad">
                            <a href="#top" class="btn return">Back to top</a>
                        	<div id="carfaxWrap" style="position: relative; ">
                            
                                <a target="_blank" href="http://www.carfax.com/VehicleHistory/p/Report.cfx?vin=<%=Model.VehicleInfo.Vin%>">
                                	<img alt="carfax logo" title="Click for full report" style="display: inline-block; float: left; margin-right: 5px;" src="http://www.carfaxonline.com/phoenix/img/cfx_logo_bw.jpg" />
                                </a>
                                
                                <h2> Vehicle History Report</h2>
                        
                            <a class="btn" target="_blank" id="carfaxBTN" href="http://www.carfax.com/VehicleHistory/p/Report.cfx?vin=<%=Model.VehicleInfo.Vin%>">Click Here For Full Report</a>
                            
                                <div style=" width: 100%; font-size: .8em; opacity: .7; text-align:center !important;">
                                    Not all accidents or other issues are reported to CARFAX. The number of owners is estimated. See the full CARFAX Report for additional information and glossary of terms. 23.Feb.2012 14:43:00
                                </div>
                            </div>
    	
                        </div>
                    </div>
                    
                    
                    
                    <!-- IMAGES -->
                    
                    <div id="images" name="images">
                    	<h3 class="header bg">Images - <%=Model.VehicleInfo.ModelYear %> <%=Model.VehicleInfo.Make %> <%=Model.VehicleInfo.Model %> <%=Model.VehicleInfo.Trim %></h3>
                    	
                        <div id="images-innerwrap" class="pad">
                            <a href="#top" class="btn return">Back to top</a>
                            <%foreach(string imgSrc in Model.VehicleInfo.UploadPhotosURL){ %>
                            <div class="imagelisting-wrap">
                            	<table class="image" style="vertical-align: middle;">
                                	<tr><td style="vertical-align: middle;"><img alt="dealer-info-plus-car-info-here" src=<%=imgSrc %> /></td></tr>
                            	</table>
                            </div>
                            <%} %>
                           
                        </div>
                        
                    </div>
                    
                     <!-- OTHER EBAY VEHICLES --> 

                        <div id="otherebay" align="center">
                    	<h3 class="header bg">Other Cars On eBay</h3>
                    	<div id="otherebay-innerwrap" class="pad">
                    		<a href="#top" class="btn return">Back to Top</a>
                    		<div id="slider1">
	                    		<div>
                                    <%foreach (WhitmanEnterpriseMVC.Models.PostEbayAds ad in Model.PostEbayList)
                                      {%>
	                    			<div class="image-small-wrap">
		                    			<table class="image" style="vertical-align: middle;">
		                                	<tr><td style="vertical-align: middle;">	<a href="<%=ad.ebayAdURL %>" target="_blank"><img alt="dealer-info-plus-car-info-here" src="<%=ad.ebayThumbNailPic %>" /></a></td></tr>
		                            	</table>
		                            	<a href="<%=ad.ebayAdURL %>" target="_blank"><%=ad.Title %></a>
		                            </div>

		                          
                                    <%} %>
	                    		</div>
	                    	</div>
                    	</div>
                    </div>
                    
                    <!-- DEALER INFO -->
                    
                    <div id="dealership" name="dealership">
                    	<h3 class="header bg">About Our Dealership</h3>
                    	<div id="dealership-innerwrap" class="pad">
                            <a href="#top" class="btn return">Back to top</a>
                        	<p><%=Model.Dealer.DealerInfo %></p>
                        </div>
                    </div>
                    
                    
                    <!-- WARRANTY INFO -->
                    
                    <div id="warranty" name="warranty">
                    	<h3 class="header bg">Warranty Information</h3>
                    	<div id="warranty-innerwrap" class="pad">
                            <a href="#top" class="btn return">Back to top</a>
                        	<p><%=Model.Dealer.DealerWarranty %></p>
                        </div>
                    </div>
                    
                    <!-- TERMS & CONDITIONS -->
                    
                    <div id="terms" name="terms">
                    	<h3 class="header bg">Terms &amp; Conditions</h3>
                    	<div id="terms-innerwrap" class="pad">
                            <a href="#top" class="btn return">Back to top</a>
                        	<p><%=Model.Dealer.TermConditon %></p>

                        </div>
                        <div id="dealer-address" style="text-align:center; color:white;"><%=Model.VehicleInfo.DealershipAddress %></div>
                    </div>
                    
                </div>
                <div id="brand">
					Powered by <a href="" style="color: white;">V<span style="color: red;">I</span>N</a> | Vehicle Inventory Network, LLC.
				        
				          <%--  <textarea  class="z-index" id="Textarea1" name="description" cols="55" rows="15" ><%=Session["MakeANdModel"]%></textarea>
				            <textarea  class="z-index" id="Textarea2" name="description" cols="55" rows="15" ><%=Session["XML"]%></textarea>--%>
				           
				</div>
            </div>
    
        </body>
        </html>
        
</div>

<!-- END OF TEMPLATE -->
       
        


</body>
</html>
