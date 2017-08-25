<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.EbayFormViewModel>" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="vincontrol.Helper" %>
<%@ Import Namespace="Vincontrol.Web.HelperClass" %>

<!DOCTYPE html>
<html>
	<head>
		<title>Ebay Post</title>

		<style type="text/css">
			body {
				margin: 0px;
				padding: 0px;
				font-family: Helvetica
			}

			.ebay_container {
				width: 980px;
				margin: 0px auto;
			}
			.ebay_top {
				width: 980px;
				height: 128px;
				background-image: url("http://apps.vincontrol.com/images/top-banner.png");
			}
		    .ebay_top_logo
		    {
		       float: left;
			    margin-left: 60px;
		    }

			.ebay_top_logo img {
				width: 80%;
				padding: 12px;
			}

			.ebay_top_info {
				width: 55%;
				float: right;
				height: 100%;
				text-align: right;
				padding: 17px;
				color: #FFF;
				font-weight: bold;
				line-height: 26px;
			}

			.ebay_content_holder {
				background-color: #000000;
				padding: 25px;
			}

			.ebay_content_left {
				width: 263px;
				float: left;
			}

			.ebay_content_right {
				width: 630px;
				float: right;
				padding: 5px;
			}
			div {
				box-sizing: border-box;
				-moz-box-sizing: border-box; /* Firefox */
			}
			.clear {
				clear: both;
			}
			.ebay_content_left .ebay_content_items {
				border: 1px solid gray;
				padding: 0px 0px 4px;
				margin-bottom: 15px;
			}
			.ebay_cl_info_items {
				padding: 3px 4px 0px;
				color: #FFF;
			}
			.ebay_cl_info_key {
				background-color: #1A1A1A;
				font-size: 12px;
				font-weight: bold;
				padding: 4px;
			}
			.ebay_content_left .ebay_content_title {
				height: 40px;
				background-image: url("http://apps.vincontrol.com/images/bg-title.png");
				color: #FF8400;
				padding: 10px;
				font-size: 17px;
				font-weight: bold;
			}
			.ebay_cl_info_value {
				font-size: 12px;
				color: #DDD;
				padding: 5px 4px 1px;
			}
			.ebay_clc_form {
				width: 90%;
				margin: 0px auto;
				border: 1px solid #DDD;
				margin-top: 10px;
			}
			.ebay_clcf_title {
				text-align: center;
				color: #FFF;
				font-size: 12px;
				font-weight: bold;
				padding: 8px;
				border-bottom: 1px solid #DDD;
			}
			.ebay_clcf_input_items {
				height: 30px;
				margin-top: 2px;
			}
			.ebay_clcf_ii_key {
				color: #FFF;
				font-size: 12px;
				float: left;
				padding: 8px;
			}
			.ebay_clcf_ii_val {
				float: right;
				width: 130px;
			}
			.ebay_clcf_input_holder {
				border-bottom: 1px solid #DDD;
			}
			.ebay_clcf_mp_title {
				font-size: 17px;
				color: #008000;
				font-weight: bold;
				text-align: center;
				padding: 5px;
			}
			.ebay_clcf_monthlyPayment {
				padding-bottom: 10px;
				height: 62px;
			}
			.ebay_cl_carfax img {
				display: block;
				margin: 10px auto;
				cursor: pointer;
			}
			.ebay_clcf_mp_value {
				text-align: center;
				color: #FFF;
				font-size: 17px;
				font-weight: bold;
			}
			#ebay_cal_monthylyPayemnt {
				width: 90%;
				color: #FFF;
				background-color: #008000;
			}
			.bx-controls-direction {
				display: none;
			}
			.inputCal {
				width: 80%;
				float: right;
				margin-right: 4px;
				border-radius: 4px;
			}
			.ebay_clcf_ii_small {
				float: left;
				color: #FFF;
				font-size: 12px;
				padding: 5px 3px;
			}
			.ebay_content_title {
				padding: 12px 20px;
				color: #FF8400;
				height: 40px;
				background-image: url("http://apps.vincontrol.com/images/bg-title-2.png");
				font-size: 16px;
				font-weight: bold;
			}
			.ebay_cr_imgs {
				height: 430px;
			}
			.ebay_cr_imgs_left {
				width: 100px;
				border: 1px solid #808080;
				height: 100%;
				float: left;
			}
			.ebay_cr_imgs_right {
				width: 515px;
				float: right;
				height: 100%;
				border: 1px solid #808080;
				background-size: 100% !important;
			}
			.ebay_cr_btns_items {
				width: 100px;
				height: 30px;
				float: right;
				margin-right: 20px;
				margin-top: 10px;
				cursor: pointer;
			}
			.ebay_cr_btns_items:hover {
				opacity: 0.8;
			}
			.ebay_crb_prev {
				background-image: url("http://apps.vincontrol.com/images/bt_prev.png");
			}
			.ebay_crb_next {
				background-image: url("http://apps.vincontrol.com/images/bt_next.png");
			}
			.ebay_cr_btns {
				clear: both;
				height: 65px;
			}
			.ebay_content_items {
				margin-bottom: 15px;
				clear: both;
			}
			.ebay_rcoi_img img {
				width: 95%;
				display: block;
				margin: 0px auto;
			}
			.ebay_rco_items {
				width: 23%;
				height: 150px;
				float: left;
				background-color: #1A1A1A;
				margin: 5px;
			}
			.ebay_cr_do {
				font-size: 12px;
				padding: 10px;
				color: #FFF;
				line-height: 22px;
				background-color: #1A1A1A;
			}
			.ebay_cr_do ul li {
				float: left;
				width: 48%;
			}
			.ebay_content_photos {
				padding: 15px;
				background-color: #1A1A1A;
			}
			.ebay_cr_do ul {
				margin-left: 15px !important;
			}
			.ebay_content_photos_items img {
				width: 100%;
			}
			.ebay_rcoi_ymm {
				color: #FFF;
				overflow: hidden;
				text-overflow: ellipsis;
				font-size: 12px;
				padding-top: 3px;
				text-align: center;
			}
			.ebay_rcoi_ymm a {
				color: #FFF;
			}
			.ebay_cril_topNextPre {
				cursor: pointer;
				height: 20px;
			}
			.ebay_cril_items {
				cursor: pointer;
			}
			.ebay_cril_topNextPre:hover {
				opacity: 0.8;
			}
			.ebay_cril_topNextPre img {
				display: block;
				margin: 10px auto;
			}
			.ebay_cril_items img {
				width: 90%;
				display: block;
				margin: 3px auto;
			}

			.ebay_cr_imgs_right img {
				display: block;
				width: 97%;
				margin: 10px auto;
			}

			.ebay_cril_topImgs {
				height: 366px;
				overflow: auto;
			}
			.bx-wrapper .bx-viewport {
				background-color: transparent !important;
				left: 0px !important;
			}
		    .clickApplyFinance
		    {
		        background-color: #555;
		        width: 244px;
		        padding: 11px;
		        text-align: center;
		        font-size: 18px;
		        margin-top: 30px;
		        cursor: pointer;
		        border-radius: 4px;
                  color: white
		    }
		</style>

	</head>
	<body>
		<div class="ebay_container">
			<div class="ebay_top">
				<div class="ebay_top_logo">
						<img height="120px" width="30px" src="<%= Model.Dealer.DealerSetting.LogoUrl %>" />
				</div>
				<div class="ebay_top_info">
					<div class="ebay_ti_items">
						Call  <%= Model.Dealer.DealerSetting.EbayContactInfoName %>: <%= Model.Dealer.DealerSetting.EbayContactInfoPhone %>
					</div>
					<div class="ebay_ti_items">
					  <%=Model.Dealer.DealerSetting.EbayContactInfoEmail%>
					</div>
					<div class="ebay_ti_items">
						  <%=Model.Dealer.DealershipAddress %> <%=Model.Dealer.City %>, <%=Model.Dealer.State %> <%=Model.Dealer.ZipCode %>
					</div>
				</div>
			</div>
			<div class="ebay_content_holder">
				<div class="ebay_content_left">
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							VEHICLE INFORMATION
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								VIN
							</div>
							<div class="ebay_cl_info_value">
									<%=Model.VehicleInfo.Vin %>
							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								CONDITION
							</div>
							<div class="ebay_cl_info_value">
									<% if (Model.VehicleInfo.Condition == Constanst.ConditionStatus.Used)
									   {%>
									       Used
									   <%}else{
									       %>
                                            New
                                <%
									   }%>

							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								MILEAGE
							</div>
							<div class="ebay_cl_info_value">
								<%=Model.VehicleInfo.Mileage %> Mi.
							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								Transmission
							</div>
							<div class="ebay_cl_info_value">
								<%=Model.VehicleInfo.Tranmission %>
							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								ENGINE
							</div>
							<div class="ebay_cl_info_value">
									<%=Model.VehicleInfo.Cylinder %> Cylinders
							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								EXTERIOR COLOR
							</div>
							<div class="ebay_cl_info_value">
									<%=Model.VehicleInfo.ExteriorColor %>
							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								INTERIOR COLOR
							</div>
							<div class="ebay_cl_info_value">
									<%=Model.VehicleInfo.InteriorColor %>
							</div>
						</div>
						<div class="ebay_cl_info_items">
							<div class="ebay_cl_info_key">
								STOCK
							</div>
							<div class="ebay_cl_info_value">
									<%=Model.VehicleInfo.Stock %>
							</div>
						</div>

					</div>
					<%--<div class="ebay_content_items">
						<div class="ebay_content_title">
							LOAN CALCULATOR
						</div>
						<div class="ebay_cl_cal">
							<div class="ebay_clc_form">
								<div class="ebay_clcf_title">
									VEHICLE PRICE CALCULATOR
								</div>
								<div class="ebay_clcf_input_holder">
									<div class="ebay_clcf_input_items">
										<div class="ebay_clcf_ii_key">
											Current Price:
										</div>
										<div class="ebay_clcf_ii_val">
											<div class="ebay_clcf_ii_small">
												$
											</div>
											<input onblur="validateCalculateForm()" value="<%=Model.VehicleInfo.SalePrice %>" class="inputCal" type="text" id="ebay_cal_currentPrice" />
										</div>
									</div>
									<div class="ebay_clcf_input_items">
										<div class="ebay_clcf_ii_key">
											Downpayment:
										</div>
										<div class="ebay_clcf_ii_val">
											<div class="ebay_clcf_ii_small">
												$
											</div>
											<input onblur="validateCalculateForm()" value="<%=Model.VehicleInfo.SalePrice/10 %>" class="inputCal" type="text" id="ebay_cal_downpayment" />
										</div>
									</div>
									<div class="ebay_clcf_input_items">
										<div class="ebay_clcf_ii_key">
											Total Price:
										</div>
										<div class="ebay_clcf_ii_val">
											<div class="ebay_clcf_ii_small">
												$
											</div>
											<input disabled="disabled" class="inputCal" type="text" id="ebay_cal_total" />
										</div>
									</div>
									<div class="ebay_clcf_input_items">
										<div class="ebay_clcf_ii_key">
											Interest Rate:
										</div>
										<div class="ebay_clcf_ii_val">
											<div class="ebay_clcf_ii_small">
												$
											</div>
											<input onblur="validateCalculateForm()" value="7" class="inputCal" type="text" id="ebay_cal_rate" />
										</div>
									</div>
									<div class="ebay_clcf_input_items">
										<div class="ebay_clcf_ii_key">
											Loan Term:
										</div>
										<div class="ebay_clcf_ii_val">
											<div class="ebay_clcf_ii_small">
												$
											</div>
											<input onblur="validateCalculateForm()" value="36" class="inputCal" type="text" id="ebay_cal_loanTerm" />
										</div>
									</div>
								</div>
								<div class="ebay_clcf_monthlyPayment">
									<div class="ebay_clcf_mp_title">
										Monthly Payment (in USD)
									</div>
									
									<input disabled="disabled" style="text-align: center" class="inputCal" type="text" id="ebay_cal_monthylyPayemnt" />
								</div>
							</div>
						</div>
					</div>--%>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							VEHICLE HISTORY REPORT
						</div>
						<div class="ebay_cl_carfax">
							<a target="_blank" href="http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.Dealer.DealerSetting.CarFax %>&vin=<%=Model.VehicleInfo.Vin %>">
							<img src="http://apps.vincontrol.com/images/carfax.png" />
                                </a>
						</div>
					</div>
				</div>
				<div class="ebay_content_right">
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							  <%=Model.VehicleInfo.OrginalName %>
						</div>
						<div class="ebay_cr_imgs">
							<div class="ebay_cr_imgs_left">
								<div id="photo_list_up" onmousedown="mouse_on=1;scroll_photo_list_up();" onmouseup="scroll_photo_list_exit();" class="ebay_cril_topNextPre ebay_cril_topNext">
									<img src="http://apps.vincontrol.com/images/arrow.png" />
								</div>
								<div class="ebay_cril_topImgs" id="photo_list">
								        <% int index = 0;
                   if (Model.VehicleInfo.ThumbnailPhotosUrl!=null){
                    %>
                    <%foreach (string imgSrc in Model.VehicleInfo.ThumbnailPhotosUrl)
                      { %>
                   <div onclick="display_photo(<%=index %>);" class="ebay_cril_items">
										  <img src="<%=imgSrc %>">
									</div>
                    <% index++; %>
                    <%} %>
                    <%} %>

                                    

									
								

								</div>
								<div onmousedown="mouse_on=1;scroll_photo_list_down();"  onmouseup="scroll_photo_list_exit();" class="ebay_cril_topNextPre ebay_cril_topPre"><img src="http://apps.vincontrol.com/images/arrow2.png" />
								</div>
							</div>
							<div class="ebay_cr_imgs_right" id="main_picture">

							</div>
						</div>
						<div class="ebay_cr_btns">
							<div onclick="display_next_photo();" class="ebay_cr_btns_items ebay_crb_next"></div>
							<div onclick="display_previous_photo();" class="ebay_cr_btns_items ebay_crb_prev"></div>
						</div>

					</div>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Description
						</div>
						<div class="ebay_cr_do">
							  <%=Model.VehicleInfo.Description %>
						</div>

					</div>
                     <%   if (Model.VehicleInfo.StandardOptions != null )
             {%>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Standard Options
						</div>
						<div class="ebay_cr_do">
							<ul style="width: 100% !important; font-size: .9em; list-style-type: disc !important; padding-left: 1em !important;" id="content_standarOption">
							   <%foreach (string tmp in Model.VehicleInfo.StandardOptions)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%} %>
								<div class="clear"></div>
							</ul>
						</div>

					</div>
                      <% } %>
                       <%   if (Model.VehicleInfo.ExistOptions != null )
             {%>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Additional Options
						</div>
						<div class="ebay_cr_do">
							<ul style="width: 100% !important; font-size: .9em; list-style-type: disc !important; padding-left: 1em !important;" id="content_additonalOption">

								 <%foreach (string tmp in Model.VehicleInfo.ExistOptions)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%} %>
								<div class="clear"></div>
							</ul>
						</div>

					</div>
                      <% } %>
                      <%   if (Model.VehicleInfo.ExistPackages != null )
             {%>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Packages
						</div>
						<div class="ebay_cr_do">
							<ul style="width: 100% !important; font-size: .9em; list-style-type: disc !important; padding-left: 1em !important;" id="content_packages">
								 <%foreach (string tmp in Model.VehicleInfo.ExistPackages)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%} %>
								<div class="clear"></div>
							</ul>
						</div>

					</div>
                       <% } %>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Photos
						</div>
						<div class="ebay_content_photos">
						     <% foreach (string imgSrc in Model.VehicleInfo.UploadPhotosUrl)
						        { %>
							<div class="ebay_content_photos_items">
							  <img src="<%=imgSrc %>">
							</div>
                            <% } %>
						
						</div>
					</div>
                     <div class="ebay_content_items">
						<div class="ebay_content_title">
							Secure Online Credit Application Form
						</div>
						<div class="ebay_cr_do">
							 Low interest car loans are available for customers with existing loans. You're just a step away from approved car financing.<br/>
                             <a target="_blank" href="<%=Model.Dealer.DealerSetting.CreditUrl %>"><div id="NCA_Credit" class="clickApplyFinance">CLICK HERE TO APPLY</div> </a>
						</div>

					</div>
                      <%   if (Model.PostEbayList.Any())
             {%>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Other Listings By This Seller
						</div>
						<div class="ebay_rc_otherListings">
                             <%foreach (var tmp in Model.PostEbayList)
                      { %>
                            	<a href="<%=tmp.EbayAdUrl %>">
                            	<div class="ebay_rco_items">
								<div class="ebay_rcoi_img">
								  <img src="<%=tmp.EbayThumbNailPic %>">
								</div>
								<div class="ebay_rcoi_ymm">
									<%=tmp.Title %> 
								</div>
                                    <div class="ebay_rcoi_ymm">
									<%=tmp.SalePrice.ToString("c0") %> 
								</div>
							</div>
                             </a>
                            
                              <% } %>
						
							
						</div>
					</div>
                        <% } %>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Warranty Information
						</div>
						<div class="ebay_cr_do">
							    <%=Model.Dealer.DealerSetting.DealerWarranty %>
						</div>

					</div>
					<div class="ebay_content_items">
						<div class="ebay_content_title">
							Term &amp; Conditions
						</div>
						<div class="ebay_cr_do">
							<%=Model.Dealer.DealerSetting.TermConditon%>
						</div>

					</div>
                    <div class="ebay_content_items">
						<div class="ebay_content_title">
								Powered by Vincontrol, LLC.
						</div>
						

					</div>
				</div>
				<div class="clear"></div>
			</div>
		</div>
		<script type="text/javascript">
		    var photo_list = document.getElementById('photo_list');
		    var mouse_on = 0;
		    var photoIndexTracker = 0;
		    //validateCalculateForm();
		    var pictures = new Array();
		 

		    <%
		        var indexPic = 0;
      foreach (string imgSrc in Model.VehicleInfo.UploadPhotosUrl)
						        { 
             %>
		    pictures[<%=indexPic%>] = "<%=imgSrc %>";
		   
              <% 
                                    indexPic = indexPic + 1;
  } %>
		 

		  

		    function scroll_photo_list_up() {
		        if (mouse_on == 1) {
		            if (photo_list.scrollTop > 0) {
		                photo_list.scrollTop -= 2;
		                setTimeout("scroll_photo_list_up()", 1);
		            } else {
		                mouse_on = 0;
		                photo_list.scrollTop = 0;
		            }
		        }
		    }

		    function scroll_photo_list_down() {
		        if (mouse_on == 1) {
		            if ((photo_list.scrollHeight - photo_list.scrollTop) > 0) {
		                photo_list.scrollTop += 2;
		                setTimeout("scroll_photo_list_down()", 1);
		            } else {
		                mouse_on = 0;
		                photo_list.scrollTop = photo_list.scrollHeight;
		            }
		        }
		    }

		    function scroll_photo_list_exit() {
		        mouse_on = 0;
		    }

		    function display_previous_photo() {
		        var photoIndex = photoIndexTracker - 1;
		        photoIndex = photoIndex < 0 ? (pictures.length - 1) : photoIndex;
		        display_photo(photoIndex);
		    }

		    function display_next_photo() {
		        var photoIndex = photoIndexTracker + 1;
		        photoIndex = photoIndex > (pictures.length - 1) ? 0 : photoIndex;
		        display_photo(photoIndex);
		    }

		    var width;
		    function display_photo(photoIndex) {
		        photoIndexTracker = photoIndex;

		        var img = new Image();

		        img.onload = function () {
		            var main_picture = document.getElementById('main_picture');
		            if (!(main_picture) || main_picture.tagName == 'IMG') {
		                var aspect = this.width / this.height;
		                width = width || document.getElementById('main_picture_img').getAttribute('width');
		                var height = width / aspect;

		                main_picture = document.createElement('div');
		                main_picture.setAttribute('id', 'main_picture');
		                main_picture.style.height = height + 'px';
		                main_picture.style.width = width + 'px';
		                main_picture.style.border = '1px solid #999';
		                document.getElementById('main_picture_img').parentNode.replaceChild(main_picture, document.getElementById('main_picture_img'));
		                var photo_list = document.getElementById('photo_list');
		                var photo_list_up = document.getElementById('photo_list_up');
		                if (photo_list) {
		                    photo_list.style.height = height - photo_list_up.offsetHeight + 3 + 'px';
		                }
		            }
		            //set img height
		            main_picture.style.background = 'url(' + this.src + ') no-repeat center center';
		        };
		        img.src = pictures[photoIndex];
		    }

		    function imageholderclass() {
		        this.over = new Array();
		        this.down = new Array();
		        this.src = new Array();
		        this.store = store;

		        function store(src, down, over) {
		            var AL = this.src.length;
		            this.src[AL] = new Image();
		            this.src[AL].src = src;
		            this.over[AL] = new Image();
		            this.over[AL].src = over;
		            this.down[AL] = new Image();
		            this.down[AL].src = down;
		        }

		    }

		    var ih = new imageholderclass();
		    var mouseisdown = 0;

		    function preloader(t) {
		        for (i = 0; i < t.length; i++) {
		            if (t[i].getAttribute('srcover') || t[i].getAttribute('srcdown')) {

		                storeimages(t[i]);
		                var checker = '';
		                checker = (t[i].getAttribute('srcover')) ? checker + 'A' : checker + '';
		                checker = (t[i].getAttribute('srcdown')) ? checker + 'B' : checker + '';

		                switch (checker) {
		                    case 'A':
		                        mouseover(t[i]);
		                        mouseout(t[i]);
		                        break;
		                    case 'B':
		                        mousedown(t[i]);
		                        mouseup2(t[i]);
		                        break;
		                    case 'AB':
		                        mouseover(t[i]);
		                        mouseout(t[i]);
		                        mousedown(t[i]);
		                        mouseup(t[i]);
		                        break;
		                    default:
		                        return;
		                }

		                if (t[i].src) {
		                    t[i].setAttribute("oldsrc", t[i].src);
		                }
		            }
		        }
		    }

		    function mouseup(t) {
		        var newmouseup;
		        if (t.onmouseup) {
		            t.oldmouseup = t.onmouseup;
		            newmouseup = function () {
		                mouseisdown = 0;
		                this.src = this.getAttribute("srcover");
		                this.oldmouseup();
		            };
		        } else {
		            newmouseup = function () {
		                mouseisdown = 0;
		                this.src = this.getAttribute("srcover");
		            };
		        }
		        t.onmouseup = newmouseup;
		    }

		    function mouseup2(t) {
		        var newmouseup;
		        if (t.onmouseup) {
		            t.oldmouseup = t.onmouseup;
		            newmouseup = function () {
		                mouseisdown = 0;
		                this.src = this.getAttribute("oldsrc");
		                this.oldmouseup();
		            };
		        } else {
		            newmouseup = function () {
		                mouseisdown = 0;
		                this.src = this.getAttribute("oldsrc");
		            };
		        }
		        t.onmouseup = newmouseup;
		    }

		    function mousedown(t) {
		        var newmousedown;
		        if (t.onmousedown) {
		            t.oldmousedown = t.onmousedown;
		            newmousedown = function () {
		                if (mouseisdown == 0) {
		                    this.src = this.getAttribute("srcdown");
		                    this.oldmousedown();
		                }
		            };
		        } else {
		            newmousedown = function () {
		                if (mouseisdown == 0) {
		                    this.src = this.getAttribute("srcdown");
		                }
		            };
		        }
		        t.onmousedown = newmousedown;
		    }

		    function mouseover(t) {
		        var newmouseover;
		        if (t.onmouseover) {
		            t.oldmouseover = t.onmouseover;
		            newmouseover = function () {
		                this.src = this.getAttribute("srcover");
		                this.oldmouseover();
		            };
		        } else {
		            newmouseover = function () {
		                this.src = this.getAttribute("srcover");
		            };
		        }
		        t.onmouseover = newmouseover;
		    }

		    function mouseout(t) {
		        var newmouseout;
		        if (t.onmouseout) {
		            t.oldmouseout = t.onmouseout;
		            newmouseout = function () {
		                this.src = this.getAttribute("oldsrc");
		                this.oldmouseout();
		            };
		        } else {
		            newmouseout = function () {
		                this.src = this.getAttribute("oldsrc");
		            };
		        }
		        t.onmouseout = newmouseout;
		    }

		    function storeimages(t) {
		        var s = (t.getAttribute('src')) ? t.getAttribute('src') : '';
		        var d = (t.getAttribute('srcdown')) ? t.getAttribute('srcdown') : '';
		        var o = (t.getAttribute('srcover')) ? t.getAttribute('srcover') : '';
		        ih.store(s, d, o);
		    }

		    function preloadimgsrc() {
		        display_photo(0);
		        if (!document.getElementById)
		            return;
		        var it = document.getElementsByTagName('IMG');
		        var it2 = document.getElementsByTagName('INPUT');
		        preloader(it);
		        preloader(it2);
		    }

		    if (window.addEventListener) {
		        window.addEventListener("load", preloadimgsrc, false);
		    } else {
		        if (window.attachEvent) {
		            window.attachEvent("onload", preloadimgsrc);
		        } else {
		            if (document.getElementById) {
		                window.onload = preloadimgsrc;
		            }
		        }
		    }

		    function validateCalculateForm() {

		        var check = true;
		        var vhPrice = parseFloat(document.getElementById("ebay_cal_currentPrice").value);
		        var dowPayment = parseFloat(document.getElementById("ebay_cal_downpayment").value);
		        var interestRate = parseFloat(document.getElementById("ebay_cal_rate").value);
		        var interestRateEl = document.getElementById("ebay_cal_rate");
		        var totalPrice = document.getElementById("ebay_cal_total");
		        totalPrice.value = vhPrice - dowPayment;

		        var loanTerm = parseFloat(document.getElementById("ebay_cal_loanTerm").value);
		        var loanTermEl = document.getElementById("ebay_cal_loanTerm");
		        interestRate = interestRate / 100;
		        if (interestRate == 0) {
		            if (check) {
		                interestRateEl.focus();
		                check = false;
		            }


		            interestRateEl.setAttribute("title", "*Please enter a value greater than 0 ");
		        } else {

		            interestRateEl.setAttribute("title", "");
		        }

		        if (loanTerm <= 0 || loanTerm > 480) {
		            if (check) {
		                loanTermEl.focus();
		                check = false;
		            }

		            loanTermEl.setAttribute("title", "*Please enter an integer between 1 and 480.");

		        } else {

		            loanTermEl.setAttribute("title", "");
		        }

		        if (check) {
		            var monthlyPayment = PaymentCalculate(vhPrice, interestRate, loanTerm, dowPayment);
		            var monthlyPaymentEl = document.getElementById("ebay_cal_monthylyPayemnt");
		            monthlyPaymentEl.value = monthlyPayment;
		        }

		    }

		    function checkKey(event) {
		        // Allow: backspace, delete, tab, escape, and enter
		        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
		            // Allow: Ctrl+A
				(event.keyCode == 65 && event.ctrlKey === true) ||
		            // Allow: home, end, left, right
				(event.keyCode >= 35 && event.keyCode <= 39)) {
		            // let it happen, don't do anything
		            return;
		        } else {
		            // Ensure that it is a number and stop the keypress
		            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
		                event.preventDefault();
		            }
		        }
		    }

		    function PaymentCalculate(p, r, m, d) {
		        // R: rate 0.07
		        // P : price
		        // M: months
		        // D : downpayment
		        var result = ((p - d) * (r / 12)) / (1 - Math.pow((1 + r / 12), -m));
		        return result.toFixed(2);
		    }
		</script>
	</body>
</html>
