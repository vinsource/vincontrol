<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.TradeInManagement.LandingViewModel>" %>
<%@ Import Namespace="iTextSharp.text.pdf.qrcode" %>
<%@ Import Namespace="vincontrol.Data.Model" %>
<%@ Import Namespace="vincontrol.VinTrade.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trade In | Landing Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-content-container">
				<div class="main-content">
                    <% if (Model.VehicleInfo != null){%>
					<div class="content-box with-header" id="highlights">
						<h3><%= Model.VehicleInfo.SelectedYear %> <%= Model.VehicleInfo.SelectedMakeValue %> <%= Model.VehicleInfo.SelectedModelValue %> <%= Model.VehicleInfo.SelectedTrimValue %></h3>
						<div class="profile-img">
							<div class="profile-img-row">
								<div class="profile-img-cell">
									<img alt="" src="<%= (Model.VehicleInfo.ImageList != null && Model.VehicleInfo.ImageList.Any()) ? Model.VehicleInfo.ImageList[0] : "http://vincontrol.com/alpha/no-images.jpg" %>">
								</div>
							</div>
						</div>
						<div class="highlights">
							<div class="pricing">
								<div class="label">
									Internet Price
								</div>
								<div class="price">
									<%= Model.VehicleInfo.SalePrice.ToString("c0") %>
								</div>
							</div>
							<div class="highlight-boxes">
								<% if (Model.VehicleInfo.IsLowMiles) {%>
                                <div class="highlight-item">
									Low Miles
								</div>
                                <%}%>
                                <% if (Model.VehicleInfo.IsLowPrice) {%>
								<div class="highlight-item">
									Low Price
								</div>
                                <%}%>
                                <% if (Model.VehicleInfo.IsFuelEfficient) {%>
								<div class="highlight-item">
									Fuel Efficient
								</div>
                                <%}%>
                                <% if (Model.VehicleInfo.IsPopularColor) {%>
								<div class="highlight-item">
									Popular Color
								</div>
                                <%}%>
                                <% if (Model.VehicleInfo.IsNewArrival) {%>
								<div class="highlight-item">
									New Arrival
								</div>
                                <%}%>
								<% if (Model.VehicleInfo.IsCarfax1Owner) {%>
                                <div class="highlight-item">
									Carfax 1-Owner
								</div>
                                <%}%>
							</div>
						</div>
					</div>
                    <%}%>

                    <% if (Model.VehicleInfo != null && Model.VehicleInfo.CarFax != null) {%>
					<div class="content-box with-header" id="carfax">
						<h3>Carfax Vehicle History Report</h3>
						<div class="carfax-logo-and-button">
							<button style="cursor:pointer" onclick="javascript:newPopup('http://www.carfax.com/VehicleHistory/p/Report.cfx?partner=VIT_0&vin=<%=Model.VehicleInfo.Vin%>')" id="#carfax-btn">
								See Full Report!
							</button>
							<% if (Model.VehicleInfo.CarFax.NumberofOwners == "1") {%>
                            <img alt="" src="<%=Url.Content("~/Content/Images/NewSiteImages/carfax-one-owner.png")%>" width="60" />
                            <%} else {%><img alt="" src="<%=Url.Content("~/Content/Images/carfax.jpg")%>" width="170px" /><%}%>
						</div>
						<div class="carfax-report">
							<div class="header">
								Vehicle Report
								<div class="service-records">
									<div class="label">
										Service Records
									</div>
									<div class="services-count">
										<%= Model.VehicleInfo.CarFax.ServiceRecords %>
									</div>
								</div>
								<div class="owner-number">
									<div class="label">
										Owners
									</div>
									<div class="owner-count">
										<%= Model.VehicleInfo.CarFax.NumberofOwners %>
									</div>
								</div>
							</div>
							<%foreach (var tmp in Model.VehicleInfo.CarFax.ReportList){ %>
                            <div class="carfax-report-item">
								
								<div class="report-item-label">
									<%= tmp.Text %>
								</div>
							</div>
                            <%} %>
						</div>
					</div>
                    <%}%>

                    <% if (Model.VehicleInfo != null && Model.VehicleInfo.ImageList != null && Model.VehicleInfo.ImageList.Any()) {%>
					<div class="content-box" id="vehicle-images">
						<div id="slider">
							<ul class="bxslider">	
                                <%
                                    var run = 0; var page = 0;
                                    var sum = Model.VehicleInfo.ImageList.Count();
                                    do
                                    {
                                        var list = Model.VehicleInfo.ImageList.Skip(page*4).Take(4).ToList();
                                        %>
                                        <li>
                                        <%
                                        foreach (var tmp in list)
                                        {%>
                                        
									        <div class="thumb-wrap">
										        <div class="thumb-row">
											        <div class="thumb-cell">
												        <img alt="" src="<%= tmp %>"/>
											        </div>
										        </div>
									        </div>								
                                        
                                        <%}%>
                                        </li>
                                        <%
                                        run += list.Count;
                                        page++;  
                                    } while (sum > run);
                                %>                                
							</ul>
						</div>
					</div>
                    <%} %>

                    <% if (Model.VehicleInfo != null) {%>
					<div class="content-box with-header" id="vehicle-info">
						<h3>Vehicle Information</h3>
						<ul class="vehicle-details">
							<li id="stocknumber">
								<div class="vehicle-details-label">
									Stock:
								</div>
								<div class="vehicle-details-text">
									<%= Model.VehicleInfo.StockNumber %>
								</div>
							</li>
							<li id="engine">
								<div class="vehicle-details-label">
									Engine:
								</div>
								<div class="vehicle-details-text">
									<%= Model.VehicleInfo.Engine %>
								</div>
							</li>
							<li id="vin">
								<div class="vehicle-details-label">
									VIN:
								</div>
								<div class="vehicle-details-text">
									<%= Model.VehicleInfo.Vin %>
								</div>
							</li>
							<li id="color">
								<div class="vehicle-details-label">
									Ext. Color
								</div>
								<div class="vehicle-details-text">
									<%= Model.VehicleInfo.ExteriorColor %>
								</div>
							</li>
							<li id="mileage">
								<div class="vehicle-details-label">
									Mileage:
								</div>
								<div class="vehicle-details-text">
									<%= Model.VehicleInfo.MileageNumber.ToString("0,0") %>
								</div>
							</li>
							<li id="transmission">
								<div class="vehicle-details-label">
									Trans:
								</div>
								<div class="vehicle-details-text">
									<%= Model.VehicleInfo.Transmission %>
								</div>
							</li>
						</ul>
						<div class="vehicle-detail description">
							<h3>Description</h3>
							<p>
								<%= Model.VehicleInfo.Description %>
							</p>
						</div>
                        <% if (Model.VehicleInfo != null && Model.VehicleInfo.Packages != null && Model.VehicleInfo.Packages.Any() && Model.VehicleInfo.Options != null && Model.VehicleInfo.Options.Any())
                           { %>
						<div class="options-and-packages">
							<div class="options-and-packages-wrap">
								<h3>Options</h3>
								<div class="scrollable-list">
									<ul>
                                        <% foreach (var tmp in Model.VehicleInfo.Options)
                                           { %>
										<li>
											<%= tmp %>
										</li>
										<% } %>
									</ul>
								</div>
							</div>
							<div class="options-and-packages-wrap">
								<h3>Packages</h3>
								<div class="scrollable-list" id="packages">
									<ul>
										<% foreach (var tmp in Model.VehicleInfo.Packages)
										   { %>
                                        <li>
											<%= tmp %>
										</li>
										<% } %>
									</ul>
								</div>
							</div>
						</div>
                        <% } %>
					</div>
                    <%} %>

					<% if (Model.DealerReview != null && Model.DealerReview.UserReviews != null && Model.DealerReview.UserReviews.Any()) {%>
                    <div class="content-box with-header reviews" id="reviews">
						<% if (Model.DealerReview.OverallScore == 0) {%>
                        <div class="overall-rating">
							<div class="star none"></div>
							<div class="star none"></div>
							<div class="star none"></div>
							<div class="star none"></div>
							<div class="star none"></div>
						</div>
                        <%} else if(Model.DealerReview.OverallScore ==4){%>
                        <div class="overall-rating">
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star none"></div>
						</div>
                             <%} else if(Model.DealerReview.OverallScore >4 &&Model.DealerReview.OverallScore <5 ){%>
                        <div class="overall-rating">
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star half"></div>
						</div>
                           <%} else 
                             { %>
                        <div class="overall-rating">
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
							<div class="star full"></div>
						</div>
                        <% } %>

						<h3>Dealership Reviews</h3>
						<div class="scrollable-list">
							<ul class="dealer reviews-list">
								
                                <%foreach (var item in Model.DealerReview.UserReviews)
                                  {
                                      if (item.Rating == 4)
                                      {
                                %>
                                <li>
                                    
									<div class="rating">
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star none"></div>
									</div>
									<p class="review-item">
										<%= item.Comment %>
									</p>
									<div class="review-user-name">
										- <%= item.Author %>
									</div>
								</li>  
                                <% }else if (item.Rating > 4 && item.Rating < 5)
                                   { %>
                                 <li>
                                    
									<div class="rating">
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star half"></div>
									</div>
									<p class="review-item">
										<%= item.Comment %>
									</p>
									<div class="review-user-name">
										- <%= item.Author %>
									</div>
								</li>  
                                <% }
                                   else
                                   { %>
                                <li>
                                    
									<div class="rating">
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
										<div class="star full"></div>
									</div>
									<p class="review-item">
										<%= item.Comment %>
									</p>
									<div class="review-user-name">
										- <%= item.Author %>
									</div>
								</li>  
                                
                                <% } %>

                                <%}%>                               
								
							</ul>
						</div>
					</div>
                    <%}%>				

					<div class="content-box with-header dealer-info-box" id="dealer-info">
						<h3><%= Model.DealerInfo.DealershipName %></h3>
						<div class="about-dealership">
							<iframe width="300" height="350" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="http://maps.google.com/maps?q=<%= SessionHandler.TradeInDealer.Address.Replace("#111","") %>,+<%= SessionHandler.TradeInDealer.State %>+<%= SessionHandler.TradeInDealer.ZipCode %>&amp;ie=UTF8&amp;hq=&amp;hnear=<%= SessionHandler.TradeInDealer.Address.Replace("#111","") %>,+<%= SessionHandler.TradeInDealer.City %>,+<%= SessionHandler.TradeInDealer.State %>+<%= SessionHandler.TradeInDealer.ZipCode %>&amp;output=embed"></iframe>
							<div class="dealer-hours-of-operation">
								<h4>Hours of Operation</h4>
								<ul>
								    <% foreach (var tmp in SessionHandler.TradeInDealer.DealerHours)
								       {
								           %>
                                    <li><%=tmp.Day %>: <%=tmp.Hours %></li>

								       <%} %>
									
								</ul>

							
							</div>

						</div>
					</div>
				</div>
			</div>
    <%= Html.Partial("MakeOffer") %>
    <%= Html.Partial("TestDrive") %>
    <%= Html.Partial("MoreInfo") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script type="text/javascript">
    var insertCustomerInfo_api = '<%= Url.Action("InsertCustomerInfo","Inventory") %>';
    var insertPriceAlert_api = '<%= Url.Action("InsertPriceAlert","Inventory") %>';
    var dealerId = '<%= SessionHandler.TradeInDealer.DealershipId %>';
    var dealerEmail = '<%= SessionHandler.TradeInDealer.Email %>';
    var vinNumber = '<%= Model.VehicleInfo.Vin %>';
    var year = '<%= Model.VehicleInfo.SelectedYear %>';
    var make = '<%= Model.VehicleInfo.SelectedMakeValue %>';
    var model = '<%= Model.VehicleInfo.SelectedModelValue %>';
    var trim = '<%= Model.VehicleInfo.SelectedTrimValue %>';
    var stockNumber = '<%= Model.VehicleInfo.StockNumber %>';
    var isSolded = 'False';
    var vehicleId = '<%= Model.VehicleInfo.VehicleId %>';
</script>
</asp:Content>
