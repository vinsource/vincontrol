<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.TradeInManagement.LandingViewModel>" %>
<%@ Import Namespace="vincontrol.VinTrade.Handlers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
  <title>Trade In</title>
  
  <link href="<%=Url.Content("~/Content/LandingCSS/print.css")%>" rel="stylesheet" type="text/css" />
</head>
<body>
  <div class="container">

    <div class="main-content-container">
      <div class="main-content">
        <% if (Model.DealerInfo != null){%>
        <div class="content-box first">
          <div class="logo">
            <img src="<%=Url.Content("~/Content/Images/NewSiteImages/logo.png")%>">
          </div>
          <div class="dealer-info">
            <h3><%= Model.DealerInfo.DealershipName %></h3>
            <ul class="dealer-details">
              <li class="dealer-phone">
                <span class="dealer-detail-label">
                  Phone
                </span>
                <div class="dealer-detail-text">
                  <%= Model.DealerInfo.Phone %>
                </div>
              </li>
              <li class="dealer-fax">
                <span class="dealer-detail-label">
                  Fax
                </span>
                <div class="dealer-detail-text">
                  
                </div>
              </li>
              <li class="dealer-email">
                <span class="dealer-detail-label">
                  Email
                </span>
                <div class="dealer-detail-text">
                  <%= Model.DealerInfo.Email %>
                </div>
              </li>
              <li class="dealer-address">
                <span class="dealer-detail-label">
                  Address
                </span>
                <div class="dealer-detail-address-one">
           <%= Model.DealerInfo.Address %> <%=Model.DealerInfo.City %>, <%=Model.DealerInfo.State %> <%=Model.DealerInfo.ZipCode %>
                </div>
                <div class="dealer-detail-address-two">
                  
                </div>
              </li>
            </ul>
          </div>
        </div>
        <%}%>

        <% if (Model.VehicleInfo != null){%>
        <div class="content-box with-header" id="highlights">
          <h3><%= Model.VehicleInfo.SelectedYear %> <%= Model.VehicleInfo.SelectedMakeValue %> <%= Model.VehicleInfo.SelectedModelValue %> <%= Model.VehicleInfo.SelectedTrimValue %></h3>
          <div class="profile-img">
            <div class="profile-img-row">
              <div class="profile-img-cell">
                <img src="<%= Model.VehicleInfo.ImageList.Any() ? Model.VehicleInfo.ImageList[0] : String.Empty %>"/>
              </div>
            </div>
          </div>
          <div class="highlights">
            <div class="pricing">
              <div class="label">Internet Price</div>
              <div class="price"><%= Model.VehicleInfo.SalePrice.ToString("c0") %></div>
            </div>
            <h3>Highlights:</h3>
            <div class="highlight-boxes">
              <% if (Model.VehicleInfo.IsLowMiles) {%>
              <div class="highlight-item">Low Miles</div>
              <%}%>
              <% if (Model.VehicleInfo.IsLowPrice) {%>
              <div class="highlight-item">Low Price</div>
              <%}%>
              <% if (Model.VehicleInfo.IsFuelEfficient) {%>
              <div class="highlight-item">Fuel Efficient</div>
              <%}%>
              <% if (Model.VehicleInfo.IsPopularColor) {%>
              <div class="highlight-item">Popular Color</div>
              <%}%>
              <% if (Model.VehicleInfo.IsNewArrival) {%>
              <div class="highlight-item">New Arrival</div>
              <%}%>
              <% if (Model.VehicleInfo.IsCarfax1Owner) {%>
              <div class="highlight-item">Carfax 1-Owner</div>
              <%}%>
            </div>
          </div>
        </div>
        <%}%>

        <% if (Model.VehicleInfo != null && Model.VehicleInfo.CarFax != null) {%>
        <div class="content-box with-header" id="carfax">
          <h3>Carfax Vehicle History Report</h3>
          <% if (Model.VehicleInfo.CarFax.NumberofOwners == "1") {%>
          <div class="carfax-logo-and-button">
            <img src="<%=Url.Content("~/Content/Images/NewSiteImages/carfax-one-owner.png")%>" width="150">
          </div>
          <%}%>
          <div class="carfax-report">
            <div class="header">
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
				<div class="report-item-ico">
					<%if (tmp.Text.Contains("Prior Rental") || tmp.Text.Contains("Accident(s) / Damage Reported to CARFAX")){%>
                    <%}else{%>
                    <img alt="" src="<%=Url.Content("~/Content/Images/NewSiteImages/carfax-check.png")%>"/>
                    <%}%>
				</div>
				<div class="report-item-label">
					<%= tmp.Text %>
				</div>
			</div>
            <%} %>
          </div>
        </div>
        <%}%>

        <% if (Model.VehicleInfo != null) {%>
        <div class="content-box with-header" id="car-info">
          <h3>Vehicle Information</h3>
          <ul class="vehicle-details">
            <li id="stocknumber">
              <div class="vehicle-details-label">Stock:</div>
              <div class="vehicle-details-text"><%= Model.VehicleInfo.StockNumber %></div>
            </li>
            <li id="engine">
              <div class="vehicle-details-label">Engine:</div>
              <div class="vehicle-details-text"><%= Model.VehicleInfo.Engine %></div>
            </li>
            <li id="vin">
              <div class="vehicle-details-label">VIN:</div>
              <div class="vehicle-details-text"><%= Model.VehicleInfo.Vin %></div>
            </li>
            <li id="color">
              <div class="vehicle-details-label">Ext. Color:</div>
              <div class="vehicle-details-text"><%= Model.VehicleInfo.ExteriorColor %></div>
            </li>
            <li id="mileage">
              <div class="vehicle-details-label">Mileage:</div>
              <div class="vehicle-details-text"><%= Model.VehicleInfo.MileageNumber.ToString("0,0") %></div>
            </li>
            <li id="transmission">
              <div class="vehicle-details-label">Trans:</div>
              <div class="vehicle-details-text"><%= Model.VehicleInfo.Transmission %></div>
            </li>
          </ul>
          <div class="vehicle-detail description">
            <h3>Description</h3>
            <p><%= Model.VehicleInfo.Description %></p>
          </div>
          
        </div>
        <%}%>

        <div class="page-break"></div>

        <% if (Model.DealerInfo != null){%>
        <div class="content-box first">
          <div class="logo">
            <img src="<%=Url.Content("~/Content/Images/NewSiteImages/logo.png")%>">
          </div>
          <div class="dealer-info">
            <h3><%= Model.DealerInfo.DealershipName %></h3>
            <ul class="dealer-details">
              <li class="dealer-phone">
                <span class="dealer-detail-label">
                  Phone
                </span>
                <div class="dealer-detail-text">
                  <%= Model.DealerInfo.Phone %>
                </div>
              </li>
              <li class="dealer-fax">
                <span class="dealer-detail-label">
                  Fax
                </span>
                <div class="dealer-detail-text">
                  
                </div>
              </li>
              <li class="dealer-email">
                <span class="dealer-detail-label">
                  Email
                </span>
                <div class="dealer-detail-text">
                  <%= Model.DealerInfo.Email %>
                </div>
              </li>
              <li class="dealer-address">
                <span class="dealer-detail-label">
                  Address
                </span>
                <div class="dealer-detail-address-one">
                  <%= Model.DealerInfo.Address %> <%=Model.DealerInfo.City %>, <%=Model.DealerInfo.State %> <%=Model.DealerInfo.ZipCode %>
                </div>
                <div class="dealer-detail-address-two">
                  
                </div>
              </li>
            </ul>
          </div>
        </div>
        <%}%>

        <div class="content-box with-header dealer-info-box">
          <h3>Information</h3>
          <div class="about-dealership">
            <iframe width="900" height="350" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="http://maps.google.com/maps?q=<%= SessionHandler.TradeInDealer.Address %>,+<%= SessionHandler.TradeInDealer.State %>+<%= SessionHandler.TradeInDealer.ZipCode %>&amp;ie=UTF8&amp;hq=&amp;hnear=<%= SessionHandler.TradeInDealer.Address %>,+<%= SessionHandler.TradeInDealer.City %>,+<%= SessionHandler.TradeInDealer.State %>+<%= SessionHandler.TradeInDealer.ZipCode %>&amp;output=embed"></iframe>
            

            <div class="dealer-hours-of-operation">
              <h4>Hours of Operation</h4>
              <ul>
            	<li>Mon-Sat: 8:30am - 9:00pm</li>
                <li>Sun: 9:30am - 8:00pm</li>
              </ul>
            </div>

          </div>
        </div>
      </div>
    </div>
  </div>
  <script type="text/javascript">
      setTimeout(function () {
          window.print();
      },1000);
      
  </script>
</body>
</html>