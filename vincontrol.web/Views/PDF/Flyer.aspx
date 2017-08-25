<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<%@ Import Namespace="vincontrol.Constant" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">    
    <title>Flyer</title>
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/flyers.css")%>" />
    <style type="text/css">
        body {
	        font-family: Arial;
	        margin: 0px;
        }

        .flyer-container {
	        width: 1100px;
	        margin: 0px auto;
	        padding-bottom: 30px;
        }
        .flyer_content_holder {

        }

        .bottom_line {
	        border-bottom: 2.5px solid gray;
        }
        .top_line {
	        border-top: 2.5px solid #333;
	        margin-top: 2px;
        }

        .flyer_header_right {
	        float: right;
	        width: 45%;
	        text-align: right;
	        padding-right: 10px;
	        padding-top: 25px;
        }

        .flyer_header_left {
	        font-size: 26px;
	        color: #555;
	        float: left;
	        width: 53%;
	        font-weight: bold;
	        padding: 24px 5px;
            overflow: hidden;
text-overflow: ellipsis;
white-space: nowrap;
        }
        .flyer_header {
	        height: 80px;
	        background-color: #F8F8F8;
        }

        .flyer_header_name {
	        display: block;
	        font-size: 19px;
	        font-weight: bold;
	        color: #555;
        }
        .flyer_content_left {
	        float: left;
	        width: 40%;
        }
        .flyer_title {
	        font-size: 22px;
	        height: 35px;
	        font-weight: bold;
	        color: #777;
        }

        .flyer_content_right {
	        padding: 5px;
	        float: right;
	        width: 58%;
	        position: relative;
        }
        .flyer_dealer_address_bottom{
	        position: absolute;
	        bottom: 0px;
	        text-align: center;
	        font-weight:bold;
	        font-size: 17px;
	        color: #555555;
        }
        .center_line {
	        /*border-left: 2.5px solid #333;*/
        }
        .flyer_cardes {
	        line-height: 21px;
	        font-size: 16px;
	        padding-left: 28px;
            height: 318px;
            overflow: hidden;
        }
        .flyer_content_holder {
	        padding-top: 5px;
        }

        .flyer_mainImg {

        }
        .flyer_content_right > div {
	        padding: 20px;
        }

        .flyer_mainImg > img {
	        width: 96%;
        }
        .flyer_carfax_info ul {
	        list-style-type: none;
	        padding-left: 20px;
	        margin: 10px 0px;
        }

        .flyer_carfax_info ul li {
	        font-size: 13px;
	        padding-bottom: 5px;
        }

        .flyer_carfax_logo {
	        margin-top: 10px;
        }
        .flyer_list_imgs {

        }

        .flyer_list_imgs_item {
	        width: 31%;
	        float: left;
	        margin-right: 5px;
	        margin-top: 2px;
        }
        .flyer_list_imgs_item > img {
	        width: 100%;
        }

        .flyer_ymm {
	        font-size: 30px;
	        font-weight: bold;
	        color: #555;
        }
        .flyer_dealer_address {
	        line-height: 26px;
	        font-size: 19px !important;
        }
        .flyer_carinfo {

        }
        .flyer_carinfo ul {
	        list-style-type: none;
	        padding-left: 20px;
        }
        .flyer_carinfo ul li {
	        font-size: 14px;
	        margin-bottom: 5px;
        }

        .flyer_carinfo nobr {
	        font-size: 15px;
	        font-weight: bold;
	        color: #444;
        }
        .flyer_noadditional {
	        padding: 15px;
        }
        .flyer_noadditional label {
	        display: block;
	        text-align: center;
	        font-size: 28px;
	        color: #555;
        }

        .flyer_noadditional ul {
	        list-style-type: none;
	        margin: 0px 10px;
	        padding-left: 15px;
        }

        .flyer_noadditional li {
	        font-size: 13px;
	        margin-bottom: 5px;
	        color: #444444;
        }

        .flyer_map_holder {
	        margin-top: 5px;
        }

        .flyer_map_holder small a {
	        text-align: center !important;
	        display: block;
	        padding-top: 5px;
	        font-size: 22px;
	        color: #333;
	        text-decoration: none;
        }

        .flyer_cardes { text-align: justify; }
    </style>

    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
            $(document).ready(function () {
                setInterval(function () {
                    //adjustHeight();
                }, 1000);
            });

            function adjustHeight() {
                var leftHeight = $(".flyer_content_left").css("height");
                var rightHeigh = $(".flyer_content_right").css("height");
                if (leftHeight > rightHeigh) {
                    $(".flyer_content_right").css("height", leftHeight);
                }
            }
    </script>
</head>
<body>
    <% if (String.IsNullOrEmpty(Model.SinglePhoto))
       {
           Model.SinglePhoto = "http://vincontrol.com/alpha/no-images.jpg";
       } %>
    <% if (String.IsNullOrEmpty(Model.SinglePhoto)) {%>
    <div class="flyer-container"></div>
    <%} else {%>
    <!-- PAGE 1 -->
    <div class="flyer-container">
		<div class="flyer_header bottom_line">
			<div class="flyer_header_left">
				<%= SessionHandler.Dealer.DealershipName %>
			</div>
			<div class="flyer_header_right">
				<label class="flyer_header_name"> <%= SessionHandler.CurrentUser.FullName %> </label>
				<label class="flyer_header_name"><%= String.IsNullOrEmpty(SessionHandler.CurrentUser.Cellphone) ? "" : String.Format("{0:(###) ###-####}", Convert.ToDecimal(SessionHandler.CurrentUser.CellPhoneWithNumberOnly))%></label>
			</div>
		</div>
		<div class="top_line flyer_content_holder">
			<div class="flyer_content_left">
				<div class="flyer_mainImg">
					<img src="<%= Model.SinglePhoto %>" />
				</div>
				<div class="flyer_carfax_holder">
					<div class="flyer_carfax_logo">
						<img src="http://www.carfax.com/media/img/cfx/homepage/carfax-logo.png" style="width:157px;height:30px;" />
					</div>
					<div class="flyer_carfax_info">
						<ul>
							<li>
								<%= Model.CarFaxOwner %> Owner(s)
							</li>
								
                            <% foreach (var item in Model.CarFax.ReportList){%>
                            <li>
								<%= item.Text %>
							</li>
                            <%}%>
                                
						</ul>
					</div>
				</div>
                <% if (Model.ThumbnailPhotosUrl != null)
                   {
                       if (Model.CarThumbnailUrl.Split(',').ToArray().Any()){
                         %><div class="flyer_list_imgs">
					        <% foreach (var item in Model.CarThumbnailUrl.Split(',').ToArray().Skip(0).Take(15)){%>
                            <% if (!String.IsNullOrEmpty(item)) {%>
                                <div class="flyer_list_imgs_item">
						            <img src="<%= item %>" style="width:136px;height:102px;"/>
					            </div>
                            <%}%>						  
                            <%}%>                        
				        </div>
                    <%}
                   }
                      
                %>
				
			</div>
			<div class="center_line flyer_content_right">
				<div class="flyer_carinfo_holder bottom_line">
					<div class="flyer_ymm">
						<%= Model.ModelYear %> <%= Model.Make %> <%= Model.Model %> <%= Model.Trim %>
					</div>
					<div class="flyer_carinfo">
						<ul>
							<li>
								<nobr>
									Vin:
								</nobr>
								<%= Model.Vin %>
							</li>
							<li>
								<nobr>
									Stock:
								</nobr>
								<%= Model.Stock %>
							</li>
							<li>
								<nobr>
									Odometer:
								</nobr>
								<%= Model.Mileage.ToString("0,0") %>
							</li>
							<li>
								<nobr>
									Ext. Color:
								</nobr>
								<%= Model.ExteriorColor %>
							</li>
							<li>
								<nobr>
									Int. Color:
								</nobr>
								<%= Model.InteriorColor %>
							</li>
							<li>
								<nobr>
									Trans:
								</nobr>
								<%= Model.Tranmission %>
							</li>
							<li>
								<nobr>
									Engine:
								</nobr>
								<%= Model.Engine %>
							</li>
						</ul>
					</div>
				</div>
				<div class="flyer_carinfo_holder bottom_line top_line" style="min-height:300px;">
					<div class="flyer_title">
						Description
					</div>
					<div class="flyer_cardes">
						<%= Model.Description %>
					</div>
				</div>				
                <div class="flyer_carinfo_holder bottom_line top_line" style="min-height:500px;">
                    <% if (!String.IsNullOrEmpty(Model.CarsOptions) && Model.CarsOptions.Split(',').Any()) {%>
                    <div class="flyer_title">
						Additional Options and Packages
					</div>
                    <div class="flyer_noadditional">
                        <ul style="display:inline-block;margin-left:0px;padding-left:5px;">
                            <% foreach (var item in Model.CarsOptions.Split(',').Skip(0).Take(20)){%>
                                <li style="float:left;margin-right:20px;width:250px;"><%= item %></li>
                            <%}%>
                        </ul>
                    </div>
                    <%}%>

					<% if (String.IsNullOrEmpty(Model.CarsOptions) || !Model.CarsOptions.Split(',').Any()) {%>
                    <div class="flyer_noadditional">
						<label>Call <%= SessionHandler.CurrentUser.FullName %> at <%= SessionHandler.Dealer.DealershipName %></label>
						<label>today for more information:</label>
                        <label><%= String.IsNullOrEmpty(SessionHandler.CurrentUser.Cellphone) ? "" : String.Format("{0:(###) ###-####}", Convert.ToDecimal(SessionHandler.CurrentUser.CellPhoneWithNumberOnly))%></label>
					</div>
                    <%}%>
				</div>
				<div class="flyer_carinfo_holder top_line">
						
					<div class="flyer_noadditional">
						<label><%= SessionHandler.Dealer.DealershipName %></label>
						<label class="flyer_dealer_address"><%= SessionHandler.Dealer.Address %></label>
						<label class="flyer_dealer_address"><%= SessionHandler.Dealer.City %>, <%= SessionHandler.Dealer.State %></label>
					</div>
				</div>
			</div>
			<div style="clear: both"></div>
		</div>
	</div>

    <!-- PAGE 2 -->
    <div class="flyer-container">
		<div class="flyer_header bottom_line">
			<div class="flyer_header_left">
				<%= SessionHandler.Dealer.DealershipName %>
			</div>
			<div class="flyer_header_right">
				<label class="flyer_header_name"> <%= SessionHandler.CurrentUser.FullName %> </label>
				<label class="flyer_header_name"><%= String.IsNullOrEmpty(SessionHandler.CurrentUser.Cellphone) ? "" : String.Format("{0:(###) ###-####}", Convert.ToDecimal(SessionHandler.CurrentUser.CellPhoneWithNumberOnly))%></label>
			</div>
		</div>
		<div class="top_line flyer_content_holder">
			<div class="flyer_content_left">
				<div class="flyer_mainImg">
					<img src="<%= Model.SinglePhoto %>" />
				</div>
                <% if (Model.CarThumbnailUrl != null)
                   {
                      if (Model.CarThumbnailUrl.Split(',').ToArray().Count() > 15){%>
                    <div class="flyer_list_imgs">
					    <% foreach (var item in Model.CarThumbnailUrl.Split(',').ToArray().Skip(15).Take(15)){%>
                        <% if (!String.IsNullOrEmpty(item)) {%>
                        <div class="flyer_list_imgs_item">
						    <img src="<%= item %>" style="width:136px;height:102px;"/>
					    </div>
                        <%}%>
                        <%}%>                        
				    </div>
                    <%}
                   }%>
				    
                <div id="mapWrap" style="float: left;margin-top:10px;">
                    <img border="0" alt="Points of Interest in Lower Manhattan" 
                                src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=420x400&maptype=roadmap&markers=color:red%7Clabel:S%7A%7C<%= SessionHandler.Dealer.Latitude %>,<%= SessionHandler.Dealer.Longtitude %>&sensor=false">
                    <br />                    
                </div>
			</div>
			<div class="center_line flyer_content_right">
				<div class="flyer_carinfo_holder bottom_line">
					<div class="flyer_ymm">
						<%= Model.ModelYear %> <%= Model.Make %> <%= Model.Model %> <%= Model.Trim %>
					</div>
					<div class="flyer_carinfo">
						<ul>
							<li>
								<nobr>
									Vin:
								</nobr>
								<%= Model.Vin %>
							</li>
							<li>
								<nobr>
									Stock:
								</nobr>
								<%= Model.Stock %>
							</li>
							<li>
								<nobr>
									Odometer:
								</nobr>
								<%= Model.Mileage.ToString("0,0") %>
							</li>
							<li>
								<nobr>
									Ext. Color:
								</nobr>
								<%= Model.ExteriorColor %>
							</li>
							<li>
								<nobr>
									Int. Color:
								</nobr>
								<%= Model.InteriorColor %>
							</li>
							<li>
								<nobr>
									Trans:
								</nobr>
								<%= Model.Tranmission %>
							</li>
							<li>
								<nobr>
									Engine:
								</nobr>
								<%= Model.Engine %>
							</li>
						</ul>
					</div>
				</div>				
				<div class="flyer_carinfo_holder top_line" style="min-height:520px;">
					<div class="flyer_title">
						Standard Options
					</div>
                    <% if (Model.StandardOptions != null && Model.StandardOptions.Any() && Model.Condition == Constanst.ConditionStatus.Used)
                        {%>
                    <div class="flyer_noadditional">
                        <ul style="display:inline-block;margin-left:0px;padding-left:5px;">
                            <% foreach (var item in Model.StandardOptions.Skip(0).Take(56)){%>
                                <li style="float: left; margin-right: 20px; width: 250px;"><%= item %></li>
                            <%}%>
                        </ul>
                    </div>
                    <%}%>
                </div>                
				<div class="flyer_carinfo_holder top_line">
						
					<div class="flyer_noadditional">
						<label class="flyer_dealer_address"><%= SessionHandler.Dealer.Address %> <%= SessionHandler.Dealer.City %>, <%= SessionHandler.Dealer.State %></label>
						
					</div>
				</div>
			</div>
			<div style="clear: both"></div>
		</div>
	</div>
    <%}%>
</body>
</html>
