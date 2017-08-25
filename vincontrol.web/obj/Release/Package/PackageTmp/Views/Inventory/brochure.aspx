<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<%@ Import Namespace="Vincontrol.Web.HelperClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
        .ep_container
        {
            width: 612px;
            font-family: arial narrow;
            height: 792px;
            margin: 0px auto;
        }

        .ep_header
        {
            background-color: #039;
            padding: 5px;
        }

            .ep_header img
            {
                display: block;
                margin: 0px auto;
            }

        .ep_header_line
        {
            height: 15px;
            background-color: #3266CC;
        }

        .ep_content_holder
        {
            clear: both;
        }

        .ep_car_info
        {
            padding: 15px;
        }

        .ep_car_img
        {
            width: 150px;
            float: left;
        }

            .ep_car_img img
            {
                width: 150px;
            }

        .ep_car_value
        {
            padding: 20px 0px;
            width: 420px;
            float: right;
        }

            .ep_car_value > div
            {
                font-size: 28px;
                text-align: center;
                font-weight: bold;
            }

        .ep_car_ysa
        {
            padding: 10px;
        }

        .ep_content_title
        {
            font-size: 25px;
            margin-bottom: 2px;
            border-bottom: 3px solid #039;
        }

        .ep_content_info
        {
            width: 85%;
            float: right;
            border-top: 3px solid #3266CC;
            padding: 20px 0px;
        }

        .ep_ysa_items
        {
            font-size: 19px;
            padding-bottom: 10px;
            font-weight: bold;
        }

        .ep_ysa_items_new
        {
            font-size: 19px;
            padding-bottom: 10px;
            font-weight: bold;
            word-wrap: break-word;
        }

        .ep_dealership_info
        {
            width: 45%;
            float: left;
        }

        .ep_dealership_map_holder
        {
            float: right;
            border: 3px solid gray;
            border-radius: 4px;
            border-right: 0px !important;
            width: 230px;
            height: 230px;
            padding: 3px;
        }

        .ep_dealership_map
        {
            background-color: #039;
            padding: 12px;
        }

        .ep_dealership_phone
        {
            font-size: 25px;
            font-weight: bold;
            margin-top: 30px;
        }

        .ep_footer
        {
            text-align: center;
            background-color: #039;
            height: 25px;
            padding: 3px;
            font-size: 20px;
            color: #FFF;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class="ep_container">
        <div class="ep_header">
            <img src="<%= Model.Dealer.Setting.LogoUrl %>" />
        </div>
        <div class="ep_header_line">
        </div>
        <div class="ep_content_holder">
            <div class="ep_car_info">
                <div class="ep_car_img">
                    <img src="<%= Model.SinglePhoto %>" />
                </div>
                <div class="ep_car_value">
                    <div class="ep_car_ymmt">
                        <%= Model.ModelYear %> <%= Model.Make %> <%= Model.Model %> <%= Model.Trim %>
                    </div>
                    <div class="ep_car_price">
                        <% if (Model.SalePrice > 0) %>
                        <%
                           { %>
                            Starting at: <%= Model.SalePrice.ToString("C0") %>
                        <% } %>
                        <%else
                           { %>
                            Contact us for the price
                        <% } %>
                    </div>
                </div>
                <div style="clear: both"></div>
            </div>
            <div class="ep_car_ysa">
                <div class="ep_content_title">
                    Your Sales Associate:
                </div>
                <div class="ep_content_info">
                    <div class="ep_ysa_items">
                        <%= Model.CurrentUser.Name %>
                    </div>
                    <div class="ep_ysa_items">
                        <%= Model.CurrentUser.CellPhone%>
                    </div>
                    <div class="ep_ysa_items">
                        <%= Model.CurrentUser.Email %>
                    </div>
                </div>
                <div style="clear: both"></div>
            </div>

            <div class="ep_car_ysa">
                <div class="ep_content_title">
                    Dealership:
                </div>
                <div class="ep_content_info">
                    <div class="ep_dealership_info">
                        <div class="ep_ysa_items">
                            <%= Model.Dealer.Name %>
                        </div>
                        <div class="ep_ysa_items">
                            <%= Model.Dealer.Address %>
                            <%= Model.Dealer.City %>, <%= Model.Dealer.State %>
                        </div>
                        <div class="ep_dealership_phone">
                            <%= Model.Dealer.Phone%>
                        </div>
                        <div class="ep_ysa_items_new">
                            <%= Model.Dealer.Email %>
                        </div>
                    </div>
                    <div class="ep_dealership_map_holder">
                        <div class="ep_dealership_map">
                            <img border="0" alt="Points of Interest in Lower Manhattan"
                                src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=205x206&maptype=roadmap&markers=color:red%7Clabel:S%7A%7C<%= Model.Dealer.Lattitude %>,<%= Model.Dealer.Longtitude %>&sensor=false">
                        </div>
                    </div>
                    <div style="clear: both"></div>
                </div>
                <div style="clear: both"></div>
            </div>
        </div>

        <div class="ep_footer">
            Powered by VINControl
        </div>
    </div>
</body>
</html>
