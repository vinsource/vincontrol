<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.EbayFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ebay Template</title>
    <!-- jQuery library - I get it from Google API's -->
       <%--<script src="http://apps.vincontrol.com/js/jquery-1.6.4.min.js" type="text/javascript"></script>
     <script src="http://apps.vincontrol.com/js/fancybox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <script src="http://apps.vincontrol.com/js/bxslider/jquery.bxslider.min.js" type="text/javascript"></script>
    <script src="http://apps.vincontrol.com/js/previewlisting.js" type="text/javascript"></script>--%>
  <%--  <link href="http://apps.vincontrol.com/js/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" media="screen" />--%>
   <%-- <link href="http://apps.vincontrol.com/js/bxslider/jquery.bxslider.css" rel="stylesheet" type="text/css" media="screen" />--%>
    <%--<link href="http://apps.vincontrol.com/Content/common.css" rel="stylesheet" type="text/css" />
    <link href="http://apps.vincontrol.com/Content/previewlisting.css" rel="stylesheet" type="text/css" />--%>
   <style type="text/css">
       
       body {
	font-family: Arial;
	/*height: 1000px;
	margin: 0px;*/
}
a {
	text-decoration: none;
}
#container_left_btns_holder a {
	text-decoration: none;
}

#container {
	background-color: #E8EAEF;
	min-height: 800px;
	width: 1280px;
	margin: 15px auto;
}
#container_left {
	background-color: white;
	height: 725px;
	width: 17%;
	float: left;
	box-shadow: 4px 0px 5px #888;
    padding-bottom: 20px;
}
#container_right {
	height: 725px;
	width: 83%;
	float: right;
}
.profile_popup {
	border: 10px solid #333;
	border-radius: 5px;
	padding: 15px;
	position: absolute;
	z-index: 100;
	background-color: white;
	display: none;
}
#top_line {
	height: 15px;
	background-color: #3366cc;
}
#container_left_logo {

	background-color: white;
	height: 85px;
}
#container_left_logo img {
	margin: 7px 17px;
	width: 180px;
}
#container_right_top {
	height: 85px;
}

/*
#container_right_top_search {
	background-color: #3366cc;
	float: right;
	width: 17%;
	height: 28px;
	text-align: center;
	font-size: 15px;
	color: #FFF;
	font-weight: bold;
}
*/
.mvp_cars_holder {
	float: right;
	margin-right: 10px;
	margin-top: 3px;
}


#container_right_btn_holder {
	background-color: #3366cc;
	height: 65px;
}

.kpi_market_equal_icon {
	background-image: url("/content/images/market_equal.gif");
}
.kpi_market_above_icon {
	background-image: url("/content/images/market_higher.gif");
}
.kpi_market_below_icon {
	background-image: url("/content/images/market_lower.gif");
}

#container_right_btns {
	position: relative;
	height: 55px;
	top: 10px;
	background-color: #3366cc;
}
#container_right_content {
	padding-bottom: 20px;
	min-height: 565px;
	width: 97%;
	margin: 0px auto;
	background-color: white;
	box-shadow: 4px 0px 5px #888;
}
.tab_active {
	background-color: #E8EAEF;
}

#container_left_btns_holder {

}
.container_left_btns:hover {
	background-color: #E8EAEF;
}
.container_left_btns {
	cursor: pointer;
	font-weight: bold;
	text-align: right;
	font-size: 22px;
	height: 25px;
	padding: 13px;
	border-bottom: 2px solid #808080;
	color: #414342;
}
.btns_shadow:hover {
	background-color: #333333;
}
.btns_shadow {
	box-shadow: 3px 2px 3px #333;
}

#container_left_search_input input {
	width: 182px;
	height: 20px;
}

.tenm {
	padding: .1em .3em .1em .3em;
	border-radius: 4px 4px 0px 0px;
	background: -moz-linear-gradient(left, rgba(136, 0, 0, 1) 56%, rgba(136, 0, 0, 0) 100%);
	background: -webkit-gradient(linear, left top, right top, color-stop(56%,rgba(136, 0, 0, 1)), color-stop(100%,rgba(136, 0, 0, 0)));
	background: -webkit-linear-gradient(left, rgba(136, 0, 0, 1) 56%,rgba(136, 0, 0, 0) 100%);
	background: -o-linear-gradient(left, rgba(136, 0, 0, 1) 56%,rgba(136, 0, 0, 0) 100%);
	background: -ms-linear-gradient(left, rgba(136, 0, 0, 1) 56%,rgba(136, 0, 0, 0) 100%);
	background: linear-gradient(to right, rgba(136, 0, 0, 1) 56%,rgba(136, 0, 0, 0) 100%);
	filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#880000', endColorstr='#00880000',GradientType=1 );
}
input[type="text"] {
	padding-left: 5px;
}

#admin_top_btns_holder {
	float: left;
	clear: both;
	height: 55px;
	padding-left: 10px;
}

.admin_top_btns {
	position: relative;
	cursor: pointer;
	margin: 28px 5px 5px 5px;
	float: left;
	background-color: white;
	padding: 2px 14px;
	box-shadow: 4px 0px 5px #888;
	font-size: 20px;
}

.admin_top_btns > a{
    color: black;
}

.admin_top_btns_active {
	padding-top: 8px !important;
	height: 46px !important;
	margin-top: 0px !important;
}

.admin_notifications_title {
	clear: both;
	font-size: 20px;
	padding: 5px 10px;
	font-weight: bold;
}

#opacity-layer {
	display: none;
	top: 0px;
	width: 100%;
	height: 100%;
	position: absolute;
	background-color: #2a558c;
	opacity: 0.9;
	z-index: 2;
}

/* bucket jump popup */
#profile_bucketjump_holder {
	padding: 0px;
	width: 900px;
	height: 450px;
	background-color: #EEEEEE;
}
.bucketjump_popup_content {
	height: 400px;
	overflow: auto;
}
.bucketjump_popup_header {
	position: relative;
	height: 25px;
	font-size: 20px;
	font-weight: bold;
	background-color: #3366cc;
	padding: 10px;
}
.popup_circle_close:hover {
	opacity: 1;
	color: #FFF;
	font-weight: bold;
}
.popup_circle_close {
	opacity: 0.5;
	cursor: pointer;
	background-color: #000;
	position: absolute;
	top: -18px;
	right: -19px;
	font-size: 14px;
	color: white;
	font-family: times;
	padding: 5px 10px;
	border-radius: 15px;
}
.bucketjump_popup_collumn {
	float: left;
}
.bucketjump_popup_content_items {
	clear: both;
	height: 16px;
	padding: 5px 10px;
}
.bucketjump_popup_content_items > div {
	font-size: 13px;
}
.bucketjump_popup_content_header {
	padding: 10px 10px;
	background-color: #A4A4A4;
}
.bucketjump_popup_content_header > div {
	font-size: 17px;
	font-weight: bold;
}
.bucketjump_popup_collumn_userstamp {
	width: 15%;
}
.bucketjump_popup_collumn_datestamp {
	width: 20%;
}
.bucketjump_popup_collumn_price {
	width: 25%;
}
.bucketjump_popup_collumn_download {
	width: 10%;
}
.bucketjump_popup_download_btns {
	width: 75px;
	border-radius: 10px;
	margin-top: -4px;
	background-color: #D3202B;
	color: white;
	padding: 4px 10px;
	font-size: 12px;
	cursor: pointer;
}
.bucketjump_popup_download_btns nobr {
	font-weight: bold;
	margin-left: 3px;
}
.bucketjump_popup_odd {
	background-color: #D4D4D4;
}

#profile_pricetracking_holder {
	padding: 0px;
}
.pricetracking_time_select_holder label {
	font-size: 13px;
}
.price_tracking_print {
	width: 90px;
	background-color: #5C5C5C;
	color: #FFF;
	margin: 0px auto;
	text-align: center;
	padding: 5px;
	margin-top: 15px;
	font-weight: bold;
	cursor: pointer;
}
/* end */
/* market expended */
.market_expended_popup_holder {
	width: 1000px;
	height: 600px;
}

.marketexpend_popup_content {
	height: 560px;
	overflow: auto;
	padding: 5px;
}
.mkexpend_viewmap_holder {
	margin-bottom: 15px;
}
.mkexpend_viewmap {
	font-size: 13px;
	background-color: #5C5C5C;
	color: #FFF;
	padding: 5px;
	cursor: pointer;
	text-align: center;
	width: 70px;
	font-weight: bold;
}
.mkexpend_vhInfo_holder {
	font-size: 20px;
	margin-bottom: 10px;
}
.mkexpend_checksource_holder {

}
.mkexpend_checksource_items {
	float: left;
	margin-right: 10px;
}
.mkexpend_checksource_items label {
	font-size: 13px;
}
.mkexpend_checksource_items a {
	color: #00F;
	text-decoration: underline;
	font-size: 14px;
}
.number_state_holder {
	clear: both;
}
.number_state_holder > div {
	font-weight: bold;
	text-align: center;
	font-size: 13px;
	color: white;
	padding: 5px;
	width: 55px;
	cursor: pointer;
	float: left;
	margin-right: 4px;
}
.mkexpend_number {
	background-color: #3366cc;
}
.mkexpend_state {
	background-color: black;
}

.mkexpend_select_trim_holder {
	background-color: #3366cc;
	padding: 7px;
	clear: both;
	height: 22px;
}

.mkexpend_select_trim_items {
	float: left;
	margin-right: 5px;
}

.mkexpend_select_trim_items label {
	font-size: 13px;
	color: white;
}
.mkexpend_save {
	background-color: #5C5C5C;
	text-align: center;
	font-size: 13px;
	color: white;
	padding: 2px;
	cursor: pointer;
}
.mkexpend_select_trim_items a {
	color: white;
	font-size: 13px;
	text-decoration: underline;
}
.mkexpend_charts_content_holder {
	padding-top: 15px;
}
.mkexpend_charts_content_left {
	float: left;
	width: 715px;
}
.mkexpend_charts_holder {

}

.mkexpend_charts_holder img {
	width: 715px;
}

.mkexpend_charts_content_right {
	float: right;
	width: 250px;
	position: relative;
}
.mkexpend_list_holder {
	margin-top: 20px;
	clear: both;
}
.mkexpend_list_title {
	height: 30px;
}
.mkexpend_list_title_text {
	font-size: 20px;
	float: left;
}
.mkexpend_list_title_print {
	float: left;
	font-size: 13px;
	margin-left: 20px;
	color: #FFF;
	background-color: #3366cc;
	width: 80px;
	padding: 5px;
	cursor: pointer;
	font-weight: bold;
	text-align: center;
}
.mkexpend_list_content {
	clear: both;
}
.mkexpend_list_header {
	height: 23px;
	background-color: #000;
}
.mkexpend_list_header div {
	color: white;
}
.mkexpend_list_collumn {
	width: 27px;
	font-size: 11px;
	text-align: center;
	float: left;
	padding: 4px 10px;
}
.mkexpend_list_items {
	height: 35px;
	clear: both;
}

.mkexpend_list_items div {
	color: black;
}
.mkexpend_item_odd {
	background-color: #CDCDCD;
}

.mkexpend_list_model {
	width: 50px;
}
.mkexpend_list_dc {
	width: 30px;
}
.mkexpend_list_seller {
	width: 115px;
}
.mkexpend_list_number {
	width: 15px;
}

.mkexpend_item_active {
	background-color: #008001 !important;
}
.mkexpend_item_active div {
	color: white !important;
}

.mkexpend_right_holder {
	margin-left: 50px;
	width: 200px;
	height: 360px;
	background-color: #EEEEEE;
}
.mkexpend_right_vhInfo_holder ul {
	padding: 5px;
}
.mkexpend_right_vhInfo_holder div, .mkexpend_right_vhInfo_holder ul li {
	list-style: none;
	font-size: 14px;
	color: #FFF;
	color: white;
}
.mkexpend_right_vhInfo_holder {
	padding: 5px;
	background-color: #3366cc;
}
.mkexpend_right_vhInfo_title {
	color: #FFF;
	font-size: 18px;
	font-weight: bold;
	display: block;
	padding: 10px 0px;
}

.mkexpend_right_price_holder {
	padding-left: 8px;
}

.mkexpend_right_price_holder div {
	float: left;
	font-size: 14px;
}

.mkexpend_price_range_lower {
	color: blue;
}
.mkexpend_price_range_medium {
	color: green;
}

.mkexpend_price_title {
	text-align: center;
	margin-top: 20px;
	font-size: 15px;
	font-weight: bold;
}

.mkexpend_price_range_higher {
	color: red;
}
.mkexpend_right_price_holder nobr {
	float: left;
	margin: -2px 5px;
}
/* end */

.number_below {
	padding-top: 3px;
	margin-left: -14px;
	width: 100%;
	background-color: #BEBEBE;
	height: 20px;
	position: absolute;
	bottom: 0px;
	color: #3366cc;
	text-align: center;
	font-size: 14px;
	font-weight: bold;
}

.menu_gear_holder {
	background-position-x: -5px;
	background-position-y: 16px;
	background-repeat: no-repeat;
	height: 328px;
	background-image: url("/Content/images/MenuGear.png");
}

/* new*/
.logout_holder:hover {
	background-color: #003399;
}
.logout_holder {
	cursor: pointer;
	margin-right: 10px;
	float: right;
	background-color: #3366cc;
	width: 100px;
	padding: 3px;
	text-align: center;
	font-weight: bold;
	font-size: 13px;
	margin-top: 3px;
	border: 1px solid #FFF;
	color: #FFF;
}
.mvp_cars_holder select {
	color: #CDCDCD;
	border: 0px;
	background-color: #003399;
	font-weight: bold;
	height: 20px !important;
    width: 260px;
    margin-top:3px;
}

.ptr_items_textInfo select {
	color: white;
	border: 0px;
	background-color: #3366cc;
	font-weight: bold;
	height: 20px !important;
    margin-top:1px;
}
.cl_appraisal_btns_items:hover {
	background-color: #003399;
}
.cl_appraisal_btns_items {
	cursor: pointer;
	margin-right: 9px;
	border: 1px solid #FFF;
	color: #FFF;
	float: left;
	background-color: #3366cc;
	width: 90px;
	text-align: center;
	font-size: 13px;
	font-weight: bold;
	padding: 2px 0px;
	box-sizing: border-box;
	-moz-box-sizing: border-box; /* Firefox */
}
.cl_search_btns_items:hover {
	background-color: #3366cc;
}
.cl_search_btns_items {
	cursor: pointer;
	margin-right: 9px;
	border: 1px solid #FFF;
	color: #FFF;
	float: left;
	background-color: #003399;
	width: 90px;
	text-align: center;
	font-size: 13px;
	font-weight: bold;
	padding: 2px 0px;
	box-sizing: border-box;
	-moz-box-sizing: border-box; /* Firefox */
}
.container_left_advanceSearch:hover {
	background-color: #3366cc;
}
#container_right_top_MVP {
	float: left;
	width: 63%;
	height: 28px;
	background-color: #003399;
}
.container_right_logout_mpv {
	float: right;
	width: 37%;
	background-color: #003399;
	height: 28px;
}
#container_left_search_btns {
	padding: 0px 0px 0px 14px;
	height: 20px;
}
.container_left_advanceSearch {
	padding: 2px;
	margin: 12px 14px;
	background-color: #003399;
	text-align: center;
	cursor: pointer;
	font-weight: bold;
	font-size: 13px;
	color: #FFF;
	border: 1px solid #FFF;
}
#container_left_search_input {
	padding: 13px;
	padding-bottom: 8px !important;
}

#container_left_seach {
	height: 110px;
	background-color: #003399;
}
/* end new */

/*Liem Add*/
.cl_search_btns_items > a{
	color: #222222;
    background-color: #eeeeee;
	
}

.cl_search_btns_items_LogOut{
	background-color: #EEEEEE;
    color: #222222;
    cursor: pointer;
    float: right;
    font-size: 17px;
    font-weight: bold;
    margin-right: 10px;
    margin-top: 3px;
    padding: 2px;
    text-align: center;
    width: 100px;
}
.bucketjump_popup_download_btns >a {
    color: white;
}
/*End Liem Add*/
        #brand {
				color: #003399;
				text-align:center;
				padding: 2em;
				}
			#brand a {
				font-size: 1.5em;
				font-weight: bold;
				text-decoration:none;}
body {
	font-family: Arial;
	width: 1280px;
	margin: 0px auto;
	height: 800px;
}

.previewListingHeader {
	height: 100px;
	background-color: #181D67;
}

.previewListingLine {

	height: 10px;
	background-color: #3B5998;
}

.previewListingContainer {
	padding-bottom: 30px;
	width: 70%;
	margin: 0px auto;
}

.container_header {
	height: 230px;
	border-radius: 5px;
	background-color: #242424;
	margin-top: 5px;
}

.container_header_top {
	height: 200px;
}

.container_header_bottom {
	height: 30px;
	background-color: black;
}

.container_header_top_dealerInfo {
	height: 170px;
}

.container_header_top_navi {
	height: 30px;
	width: 65%;
	margin: 0px auto;
}

.previewListingHeader_price {
	float: left;
	padding-top: 10px;
	width: 55%;
}

.previewListingHeader_btns {
	float: right;
	width: 45%;
	padding-top: 30px;
}

.previewListingHeader_price label {
	display: block;
	text-align: center;
	color: #FFF;
	font-size: 33px;
	font-weight: bold;
}

.previewListingHeader_btns_items {
	cursor: pointer;
	border-radius: 4px;
	float: left;
	background-color: #3B5998;
	width: 110px;
	text-align: center;
	font-size: 17px;
	padding: 10px;
	color: #FFF;
	margin-right: 5px;
	font-weight: bold;
}

.previewListingHeader_btns_items a{
	color: #FFF;
}

.container_header_dealerInfo_logo {
	float: left;
	width: 50%;
}

.container_header_dealerInfo_text {
	float: right;
	width: 45%;
	padding-top: 7px;
	padding-right: 7px;
}

.container_header_dealerInfo_text > div {
	text-align: right;
	color: white;
}
.container_header_dealerInfo_logo img {
	width: 320px;
	margin: 16px 55px;
}

.container_header_phone {
	font-size: 23px;
	font-weight: bold;
}

.container_header_fax_email {
	font-size: 18px;
}
.container_header_address {
	font-size: 18px;
}

.container_header_top_navi a {
	text-decoration: none;
}

.top_navi_items {
	float: left;
	color: #FFF;
	background-color: #000;
	text-align: center;
	padding: 4px 10px 10px;
	margin-right: 3px;
	font-size: 14px;
}

.navi_area_holder {
	background-color: #242424;
	margin-top: 5px;
	height: 44px;
	padding: 8px;
	border-radius: 5px;
}

.navi_area_items {
	cursor: pointer;
	float: left;
	text-align: center;
	font-size: 18px;
	margin-left: 5px;
	padding: 10px 12px;
	background-color: #E1E1E1;
}

.backtotop {
	font-size: 13px;
	color: #FFF;
	background-color: #242424;
	padding-left: 50px;
	height: 20px;
	cursor: pointer;
	padding-top: 5px;
	font-weight: bold;
}
.image_slider_vhInfo {
	height: 30px;
	background-color: #242424;
	padding-top: 9px;
}

.image_slider_vhInfo_text {
	float: left;
	color: white;
	font-size: 17px;
	padding-left: 30px;
}
.image_slider_vhInfo nobr {
	font-weight: bold;
}
.image_slider_vhInfo_ymm {
	text-align: right;
	float: right;
	color: white;
	font-size: 17px;
	padding-right: 30px;
}

.bx-wrapper .bx-pager {
	bottom: -95px;
}

.bx-wrapper .bx-pager a {
	border: solid #ccc 1px;
	display: block;
	margin: 0 5px;
	padding: 3px;
}

.bx-wrapper .bx-pager a:hover, .bx-wrapper .bx-pager a.active {
	border: solid #5280DD 1px;
}

#bx-pager {
	text-align: center;
	overflow: hidden;
}
#bx-pager img {
	max-width: 100px;
	max-height: 100px;
}
.bx-pager-item {
	width: 100px;
	height: 100px;
}

.bx-wrapper {
	margin-bottom: 5px !important;
}

.bx-wrapper .img-box,
.bx-wrapper .img-cell {
	width: 896px !important;
	height: 500px !important;
}

#bx-pager .img-box,
#bx-pager .img-cell {
	width: 100px !important;
	height: 100px !important;
}

#bx-pager .img-box {
	float: left;
	margin: 3px 6px;
}

.img-box {
	display: table;
	vertical-align: middle;
}

.img-row {
	display: table-row;
	vertical-align: middle;
}

.img-cell {
	display: table-cell;
	width: 100%;
	height: 100%;
	vertical-align: middle;
}

.bxslider li img {
	max-width: 896px;
	max-height: 500px;
	margin: 0 auto;
}

.bxslider {
	margin: 0px;
}

.img_slider_holder {
	margin-top: 5px;
}

.vehicleInfo_area {
	margin-top: 10px;
}

.vehicleInfo_area_content {
	background-color: #E1E1E1;
	height: 160px;
}

.vehicleInfo_area_content ul {
	margin: 0px;
	padding: 15px 50px;
	height: 70px;
}
.vehicleInfo_area_key {
	font-weight: bold;
}
.vehicleInfo_area_value {

}
.vehicleInfo_area_content ul li {
	font-size: 14px;
	list-style: none;
	width: 50%;
	float: left;
}

.vehicleInfo_area_cafax {
	clear: both;
	margin: 0px 50px;
}

.vehicleInfo_area_cafax_logo {
	cursor: pointer;
	float: left;
	width: 23%;
	margin-left: 40px;
}

.vehicleInfo_area_cafax_text {
	float: left;
	width: 50%;
	font-size: 12px;
}

.des_area_content {
	font-size: 14px;
	padding: 15px 50px;
	background-color: #E1E1E1;
}

.descriptions_area {
	margin-top: 10px;
}

.standard_options_area {
	margin-top: 10px;
}

.standard_option_area_content ul {
	list-style: disc;
	margin: 0px;
	padding-top: 5px;
}

.standard_option_area_content ul li {
	float: left;
	width: 50%;
	font-size: 14px;
}

.standard_option_area_content {
	background-color: #E1E1E1;
	padding-bottom: 20px;
}

.dealerInfo_area {
	margin-top: 10px;
}

.dealerInfo_area_content {
	padding-top: 10px;
	padding-bottom: 10px;
}

.warranty_area {
	margin-top: 10px;
}

.term_conditons_area {
	margin-top: 10px;
}

.dealerInfo_area_text {
	width: 300px;
	float: left;
	height: 430px;
}

.dealerInfo_area_map {
	width: 560px;
	float: right;
	height: 400px;
}

.dealerInfo_area_one {

}

.dealerInfo_area_one_dealername {
	font-weight: bold;
	font-size: 20px;
}

.dealerInfo_area_one_address {
	font-size: 16px;
	line-height: 22px;
	padding: 0px 10px;
}

.largemap {

}
.dealerInfo_area_two {
	margin-top: 30px;
}

.dealerInfo_area_two_name {
	font-weight: bold;
	font-size: 20px;
}

.dealerInfo_area_two_phone {
	font-size: 20px;
}

.dealerInfo_area_two_fax {
	font-size: 14px;
}

.dealerInfo_area_two_email {
	font-size: 14px;
}

.dealerInfo_area_btns {
	padding: 42px;
}

.dealerInfo_area_btns > div {
	width: 200px;
	margin-top: 10px;
	border-radius: 6px;
	background-color: #e1e1e1;
	color: #FFF;
	text-align: center;
	padding: 15px;
	font-weight: bold;
	font-size: 15px;
	cursor: pointer;
}

.dealerInfo_area_btns  a:visited, .dealerInfo_area_btns  a {
    color: #000;
}
.dealerInfo_area_btns_tous {

}

.dealerInfo_area_btns_tofriend {

}

.dealerInfo_area_text div {
	text-align: center;
}

.bx-viewport {
	background: #111 !important;
}


   </style>
</head>
<body>
  
    <!-- START OF TEMPLATE -->
    <div class="previewListingContainer" id="template">
        <div class="container_header">
            <div class="container_header_top">
                <div class="container_header_top_dealerInfo">
                    <div class="container_header_dealerInfo_logo">
                        
                        <img src="<%=Model.Dealer.DealerSetting.LogoUrl %>" />
                   
                    </div>
                    <div class="container_header_dealerInfo_text">
                        <div class="container_header_phone">
                            Call <%=Model.Dealer.DealerSetting.EbayContactInfoName %>:
                            <%=Model.Dealer.DealerSetting.EbayContactInfoPhone %>
                        </div>
                        <div class="container_header_fax_email">
                            <%=Model.Dealer.DealerSetting.EbayContactInfoEmail%>                   
                        </div>
                        <div class="container_header_address">
                            <%=Model.Dealer.DealershipAddress %>
                        </div>
                        <div class="container_header_address">
                            <%=Model.Dealer.City %>, <%=Model.Dealer.State %> <%=Model.Dealer.ZipCode %>
                        </div>
                    </div>
                </div>
                <div class="container_header_top_navi">
                    <a href="<%=Model.Dealer.EbayInventoryUrl %>">
                        <div class="top_navi_items">
                            View Our Inventory on eBay!
                        </div>
                    </a><a href="<%=Model.Dealer.DealerSetting.CreditUrl %>">
                        <div class="top_navi_items">
                            Financing
                        </div>
                    </a><a href="<%=Model.Dealer.DealerSetting.WebSiteUrl %>">
                        <div class="top_navi_items">
                            Visit Us Online!
                        </div>
                    </a><a href="mailto:<%=Model.Dealer.Email %>">
                        <div class="top_navi_items">
                            Email Us
                        </div>
                    </a><a href="<%=Model.Dealer.DealerSetting.ContactUsUrl %>">
                        <div style="border: none" class="top_navi_items">
                            Contact Us
                        </div>
                    </a>
                </div>
            </div>
            <div class="container_header_bottom">
            </div>
        </div>
        <div class="navi_area_holder">
            <div class="navi_area_items" id="tab_pictures" style="border-radius: 5px 0px 0px 5px;">
                Pictures
            </div>
            <div class="navi_area_items" id="tab_vehicle_information">
                Vehicle Information
            </div>
            <div class="navi_area_items" id="tab_descriptions">
                Description
            </div>
            <div class="navi_area_items" id="tab_packages_options">
                Package &amp; Options
            </div>
            <div class="navi_area_items" id="tab_warranty">
                Warranty
            </div>
            <div class="navi_area_items" id="tab_terms_conditions" style="border-radius: 0px 5px 5px 0px;">
                Terms &amp; Conditions
            </div>
        </div>
        <div id="pictures_area" class="img_slider_holder">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text"> Pictures </nobr>
                <nobr class="image_slider_vhInfo_ymm"><%=Model.VehicleInfo.ModelYear %> <%=Model.VehicleInfo.Make %> <%=Model.VehicleInfo.Model %> <%=Model.VehicleInfo.Trim %></nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="img_slider_content_holder">
                <div class="bx-wrapper" style="max-width: 100%;">
                    <div class="bx-viewport" style="overflow: hidden; position: relative; height: 500px;
                                                                                                                                                                                                                                                                                                                                                                                                                                                       width: 100%;">
                        <ul class="bxslider" style="width: 515%; position: relative; transition-duration: 0s;padding: 0px;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           transform: translate3d(-936px, 0px, 0px);">
                            <%foreach (string imgSrc in Model.VehicleInfo.UploadPhotosUrl)
                              { %>
                            <li class="bx-clone" style="list-style: none outside none; position: relative; text-align: center;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            width: 896px;">
                                <img src="<%=imgSrc %>">
                            </li>
                            <%} %>
                        </ul>
                    </div>
                </div>
                <div id="bx-pager">
                    <% int index = 0; %>
                    <%foreach (string imgSrc in Model.VehicleInfo.UploadPhotosUrl)
                      { %>
                    <a class="active" href="" data-slide-index="<%=index %>">
                        <div class="img-box">
                                 <div class="img-row">
                                               <div class="img-cell">
                        <img src="<%=imgSrc %>">
                        </div>
                        </div>
                        </div>
                    </a>
                    <% index++; %>
                    <%} %>
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="vehicleInfo_area" id="vehicle_information">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Vehicle Information
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="vehicleInfo_area_content">
                <ul>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								VIN:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Vin %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Stock:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Stock %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Mileage:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Mileage %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Ext. Color:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.ExteriorColor %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Int. Color:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.InteriorColor %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Trans:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Tranmission %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Engine:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Engine %>
							</nobr>
                    </li>
                </ul>
                <div class="vehicleInfo_area_cafax">
                    <div class="vehicleInfo_area_cafax_logo">
                        <a target="_blank" href="http://www.carfax.com/VehicleHistory/p/Report.cfx?vin=<%=Model.VehicleInfo.Vin%>">
                            <img src="http://apps.vincontrol.com/content/images/carfax-large.jpg" title="Click for full report" />
                        </a>
                    </div>
                    <div class="vehicleInfo_area_cafax_text">
                        *Not all accidents or other issues are reported to CARFAX. The number of owners
                        is estimated. See the full CARFAX Report for additional information and glossary
                        of terms. 23.Feb.2012 14:43:00
                    </div>
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="descriptions_area" id="descriptions_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Description
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="des_area_content">
                <%=Model.VehicleInfo.Description %>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
           <%   if (Model.VehicleInfo.StandardOptions != null)
             {%>
        <div class="standard_options_area" id="standard_options_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Standard Options
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="standard_option_area_content">
                <ul>
                    <%foreach (string tmp in Model.VehicleInfo.StandardOptions)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%} %>
                </ul>
                <div style="clear: both">
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
     <% } %>
     <%   if (Model.VehicleInfo.ExistOptions != null || Model.VehicleInfo.ExistPackages!=null)
             {%>
        <div class="standard_options_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
					Additional Options &amp; Packages
				</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="standard_option_area_content">
                <ul>
                    
                    <%
                 if (Model.VehicleInfo.ExistOptions != null) 
                 {
                     foreach (string tmp in Model.VehicleInfo.ExistOptions)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%}
                      } %>
                    <%
                 if (Model.VehicleInfo.ExistPackages != null)
                 {
                     foreach (string tmp in Model.VehicleInfo.ExistPackages)
                     { %>
                    <li>
                        <%= tmp %></li>
                    <% }
                 } %>
                </ul>
                <div style="clear: both">
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <% } %>
        <div class="dealerInfo_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Dealer Info
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="dealerInfo_area_content">
                <div class="dealerInfo_area_text">
                    <div class="dealerInfo_area_one">
                        <div class="dealerInfo_area_one_dealername">
                            <%=Model.Dealer.DealershipName %>
                        </div>
                        <div class="dealerInfo_area_one_address">
                            <%=Model.Dealer.DealershipAddress %>
                        </div>
                        <div class="dealerInfo_area_one_address">
                            <%=Model.Dealer.City %>, <%=Model.Dealer.State %>, <%=Model.Dealer.ZipCode %>
                        </div>
                        <div class="largemap">
                            <small><a target="_blank" href="https://maps.google.com/maps?q=<%=Model.Dealer.DealershipAddress %>&ie=UTF8&hq=&hnear=<%=Model.Dealer.DealershipAddress %>&gl=us&t=m&z=14&iwloc=near&vpsrc=0&output=embed"
                                style="color: #0000FF; text-align: left">Click for Driving Directions</a></small>
                        </div>
                    </div>
                    <div class="dealerInfo_area_two">
                        <div class="dealerInfo_area_two_name">
                            Ask For: <%=Model.Dealer.DealerSetting.EbayContactInfoName %>
                        </div>
                        <div class="dealerInfo_area_two_phone">
                            <%=Model.Dealer.DealerSetting.EbayContactInfoPhone %>
                        </div>
                        <div class="dealerInfo_area_two_email">
                            <%=Model.Dealer.DealerSetting.EbayContactInfoEmail %>
                        </div>
                    </div>
                    <div class="dealerInfo_area_btns">
                         <div class="dealerInfo_area_btns_tous">
                        <a href="<%=Model.Dealer.DealerSetting.WebSiteUrl %>">
                            Send us an E-mail
                        
                        </a>
                        </div>
                          
                        <div class="dealerInfo_area_btns_tofriend">
                             <a href="<%=Model.Dealer.DealerSetting.WebSiteUrl %>">
                            Email this to a Friend
                            </a>
                        </div>
                        
                    </div>
                </div>
                <div class="dealerInfo_area_map">
                   <%=Model.Dealer.DealerSetting.DealerInfo %>
                    <br />
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="warranty_area" id="warranty_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Warranty Information
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="des_area_content">
                <%=Model.Dealer.DealerSetting.DealerWarranty %>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="term_conditons_area" id="term_conditions_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Term &amp; Conditions
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="des_area_content">
                <%=Model.Dealer.DealerSetting.TermConditon%>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div id="brand">
					Powered by Vincontrol, LLC.
				        
				      
				</div>
    </div>
    <!-- END OF TEMPLATE -->
</body>
</html>
