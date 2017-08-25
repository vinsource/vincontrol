<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VinSocial | Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
<div class="inner-wrap">

						<div class="page-info">
							<span>
								<br>
							</span>
							<span>
								<br>
							</span>
							<h3>Video</h3>
						</div>

						<div class="filter-box">
							<div class="sub-nav">
								<div class="sub-nav-btn active" id="dashboard-tab-btn">
									Dashboard
								</div>
								<div class="sub-nav-btn" id="videos-tab-btn">
									Videos
								</div>
							</div>
						</div>

						<div class="content">
							<div class="dashboard-tab page-tab">

								<div class="top-videos">
									<h3>Top Videos</h3>
									<div class="view-all-videos">
										View All
									</div>
									<ul class="video-list">
										<li>
											<div class="video-thumb">
												<img src="<%=Url.Content("~/Content/Images/social/sample-video-thumb.jpg")%>">
											</div>
											<div class="video-title">
												2013 Mitsubishi Lancer Evolution GTS
											</div>
											<div class="video-viewcount">
												555,555,555
											</div>
										</li>
										<li>
											<div class="video-thumb">
												<img src="<%=Url.Content("~/Content/Images/social/sample-video-thumb.jpg")%>">
											</div>
											<div class="video-title">
												2013 Mitsubishi Lancer Evolution GTS
											</div>
											<div class="video-viewcount">
												555,555,555
											</div>
										</li>
										<li>
											<div class="video-thumb">
												<img src="<%=Url.Content("~/Content/Images/social/sample-video-thumb.jpg")%>">
											</div>
											<div class="video-title">
												2013 Mitsubishi Lancer Evolution GTS
											</div>
											<div class="video-viewcount">
												555,555,555
											</div>
										</li>
										<li>
											<div class="video-thumb">
												<img src="<%=Url.Content("~/Content/Images/social/sample-video-thumb.jpg")%>">
											</div>
											<div class="video-title">
												2013 Mitsubishi Lancer Evolution GTS
											</div>
											<div class="video-viewcount">
												555,555,555
											</div>
										</li>
									</ul>
								</div>

								<div class="video-views-box">
									<h3>Views</h3>
									<div class="views-graph-canvas">
										<img src="<%=Url.Content("~/Content/Images/social/graph-sample.jpg")%>">
									</div>
									<div class="breakdown-buttons">
										<div class="breakdown-btn video-likes">
											<span class="title">Likes</span>
											<div class="number positive">
												555,555,555
											</div>
										</div>
										<div class="breakdown-btn video-dislikes">
											<span class="title">Dislikes</span>
											<div class="number negative">
												555,555,555
											</div>
										</div>
										<div class="breakdown-btn video-comments">
											<span class="title">Comments</span>
											<div class="number">
												555,555,555
											</div>
										</div>
										<div class="breakdown-btn vide-shares">
											<span class="title">Shares</span>
											<div class="number">
												555,555,555
											</div>
										</div>
										<div class="breakdown-btn video-favorites">
											<span class="title">Favorites</span>
											<div class="number">
												555,555,555
											</div>
										</div>
									</div>
								</div>

								<div class="video-management">
									<div class="generate-video">
										<div class="search-inventory">
											<span class="search-inventory-label">Search</span>
											<input type="text">
										</div>
										<h3>Generate a Video</h3>
										<div class="inventory-listing">
											<span>Select the vehicle(s)</span>
											<div class="inventory-list-wrap">
												<ul>

													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>
													<li>
														<div class="vehicle-list-cell stock">
															12345J
														</div>
														<div class="vehicle-list-cell year">
															2013
														</div>
														<div class="vehicle-list-cell make">
															Mitsubishi
														</div>
														<div class="vehicle-list-cell model">
															Lancer
														</div>
														<div class="vehicle-list-cell trim">
															Evolution GTS
														</div>
														<div class="vehicle-list-cell vin">
															VIN: 12345678901234567
														</div>
														<div class="vehicle-list-cell photo-count">
															Photos: 100
														</div>
													</li>

												</ul>
											</div>
										</div>
									</div>
									<div class="video-upload-and-share">
										<div class="or-icon">
											OR
										</div>
										<div class="upload-video">
											<h3>Upload a Video</h3>
											<input type="file">
										</div>
										<div class="share-online">
											<h3>Share it online!</h3>
											<div class="social-site-selection">
												<span>Select the sites to post the video to and start sharing!</span>
												<ul class="social-sites-listing">
													<li><img src="<%=Url.Content("~/Content/Images/social/youtube-sml.png")%>">
														<br>
														<input type="checkbox">
													</li>
													<li><img src="<%=Url.Content("~/Content/Images/social/facebook-sml.png")%>">
														<br>
														<input type="checkbox">
													</li>
													<li><img src="<%=Url.Content("~/Content/Images/social/twitter-sml.png")%>">
														<br>
														<input type="checkbox">
													</li>
												</ul>
												<div class="preview-video-btn">
													Preview &amp; Submit
												</div>
											</div>
										</div>
									</div>
								</div>

							</div>

							<div class="videos-tab page-tab hidden">
								<div class="scrollableContainer">
									<div class="scrollingArea">

										<table class="list" cellspacing="0">
											<thead>
												<tr>
													<th width="300">Title</th>
													<th width="80">Views</th>
													<th width="80">Watched <small>(Minutes)</small></th>
													<th width="80">Likes</th>
													<th width="80">Dislikes</th>
													<th width="80">Comments</th>
													<th width="100">Favorites</th>
												</tr>
											</thead>
											<tbody>

												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>
												<tr>
													<td class="video-title" width="300"><span class="ico">Random Video Title Goes Right Here</span></td>
													<td class="video-views" width="80">25k</td>
													<td class="video-watch-time" width="80">1.1m</td>
													<td class="video-likes" width="80">88k</td>
													<td class="video-dislikes" width="80">15k</td>
													<td class="video-comments" width="80">34k</td>
													<td class="video-favorites" width="80">74k</td>
												</tr>

											</tbody>
										</table>

									</div>
								</div>
							</div>

							<div class="popup hidden preview-video-popup">

								<div class="popup-wrap">
									<h3>Preview Video</h3>
									<iframe width="550" height="315" src="//www.youtube.com/embed/QZNeXL8H8JE" frameborder="0" allowfullscreen></iframe>
									<div class="popup-keywords">
										<b>Keywords:</b> Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod
										tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
										quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
										consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse
										cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non
										proident, sunt in culpa qui officia deserunt mollit anim id est laborum. <a href="">...Read More</a>
									</div>
									<div class="popup-btn popup-cancel-btn">
										Cancel
									</div>
									<div class="popup-btn popup-share-btn">
										Share!
									</div>
								</div>

							</div>

						</div><!-- end of inner wrap div-->
					</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
</asp:Content>
