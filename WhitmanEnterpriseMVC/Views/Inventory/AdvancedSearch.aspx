<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AdvancedSearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <div id="filter-form-wrapper">
        <h3>
            Search VinControl
            <br />
            <small class="subtext">Here you can search your entire inventory for vehicles. Using
                the fields below you can pull a list of specific makes, models, years, cars of similar
                color or description or with similar options, etc. </small>
        </h3>
        <form id="search" method="post" action="">
        <input type="hidden" id="DealershipId" name="DealershipId" />
        <input type="hidden" id="SortField" name="SortField" />
        <div style="float: right;">
        <div title="Filter results by Year, Make and/or Model" style="float: left;">
            Year:
            <%= Html.DropDownListFor(m => m.SelectedYear, Model.Years, "----") %>
            Make:
            <%--<%= Html.DropDownListFor(m => m.SelectedMake, Model.Makes, "----", new { id = "SelectedMake" }) %>--%>
            <span id="makes">
            <%= Html.Partial("GenerateMakes", Model) %>
            </span>
            Model:
            <span id="models">
            <%= Html.Partial("GenerateModels", Model) %>
            </span>
        </div>
        | Text:
        <%= Html.TextBoxFor(m => m.Text, new Dictionary<string, object>(){ {"class","subtext"}, {"size","12"}, {"title","Searches entire inventory for text given. Ex - typing 'green' searches inventory for any vehicles that contain the text 'green'"} } ) %>        
        | Category:
        <%= Html.DropDownListFor(m => m.SelectedCategory, Model.Categories, new Dictionary<string, object>() { {"title","Filter based on inventory category such as Appraisal, New, Used, etc."} }) %>        
        </div>
        <div style="float: right; margin-top: 5px;">
            <%--<input type="button" id="btnAdvancedSearch" name="search-submit" value="Submit Search" />--%>
            <span id="btnAdvancedSearch" class="search-submit">Submit Search ></span>
        </div>
        </form>        
        <div id="result"></div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
<link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>
<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
<script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#btnAdvancedSearch").click(function () {
            $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $("#DealershipId").val($("#SelectedDealership").val());
            $.ajax({
                type: "POST",
                url: "/Inventory/AdvancedSearchResult",
                data: $("form").serialize(),
                success: function (results) {
                    $("#result").html(results);
                     $.unblockUI();
                }
            });
        });

        $("a[id^='a_']").live('click', function () {
            var column = this.id.split("_")[1];
            //$.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $("#DealershipId").val($("#SelectedDealership").val());
            $("#SortField").val(column);
            $.ajax({
                type: "POST",
                url: "/Inventory/AdvancedSearchResult",
                data: $("form").serialize(),
                success: function (results) {
                    $("#result").html(results);
                     $.unblockUI();
                }
            });
        });

        $("#SelectedYear").live('change', function () {
            $.ajax({
                type: "POST",
                url: "/Inventory/GenerateMakes",
                data: $("form").serialize(),
                success: function (results) {
                    $("#makes").html(results);
                    if ($('#SelectedMake').val() == "")
                        $("#models").html("<select id='SelectedModel' name='SelectedModel' style='width:105px'><option value=''>----</option></select>");
                    else {
                        $.ajax({
                            type: "POST",
                            url: "/Inventory/GenerateModels",
                            data: $("form").serialize(),
                            success: function (results) {
                                $("#models").html(results);

                            }
                        });
                    }
                }
            });
        });

        $("#SelectedMake").live('change', function () {
            $.ajax({
                type: "POST",
                url: "/Inventory/GenerateModels",
                data: $("form").serialize(),
                success: function (results) {
                    $("#models").html(results);

                }
            });
        });

        $("#aNew").live('click', function () {
            HideCategory();
            $(".New").show();
        });

        $("#aUsed").live('click', function () {
            HideCategory();
            $(".Used").show();
        });

        $("#aSold").live('click', function () {
            HideCategory();
            $(".Sold").show();
        });

        $("#aWholesale").live('click', function () {
            HideCategory();
            $(".Wholesale").show();
        });

        $("#aAppraisal").live('click', function () {
            HideCategory();
            $(".Appraisals").show();
        });

        $("#aAll").live('click', function () {
            ShowCategory();
        });

        $("#Text").keypress(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                if ($("#Text").val() != "") {
                    $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                    $("#DealershipId").val($("#SelectedDealership").val());
                    $.ajax({
                        type: "POST",
                        url: "/Inventory/AdvancedSearchResult",
                        data: $("form").serialize(),
                        success: function (results) {
                            $("#result").html(results);
                             $.unblockUI();
                        }
                    });
                }
            }
        });
    });

    function HideCategory() {
        $(".New").hide();
        $(".Used").hide();
        $(".Sold").hide();
        $(".Wholesale").hide();
        $(".Appraisals").hide();
    }

    function ShowCategory() {
        $(".New").show();
        $(".Used").show();
        $(".Sold").show();
        $(".Wholesale").show();
        $(".Appraisals").show();
    }
</script>

<style type="text/css">
#notes {position: relative; top: -220px; left: 150px; width: 750px; color: white; z-index: 0;}
#c2 {width: 784px;}
h4 {margin-bottom: 0; margin-top: 0;}
#sort {margin-top: -1.5em;}
p {margin-top: 0; }
#recent li {margin-left: -3px; margin-top: .3em; font-size: .9em;}
#recent .sForm {margin: 0;}
#recent .sForm[name="odometer"] {}
img.thumb {float: left; clear: right;}

body {background: url('../images/cBgRepeatW.png') top center repeat-y;}


#c2 {border-right: none;}
#table { width: 850px; padding: 0; margin: 0; font-size: .8em;}
#table div { padding: 0;  margin: 0;}
#table .cell {width: 95px; height:20px; 
overflow: hidden; 
margin-bottom: 3px;background: none;}
#table .longest {width: 184px;}
#table .long {width: 99px;}
#table .short {width: 67px;}
#table .shortest {width: 29px;}
#table .infoCell {width: 674px;}
#table .mid {width: 84px;}
div.evenShorter{width: 15px !important; border: none;}
#table div {margin-bottom: .5em;}
.scroll-pane { height: 600px; overflow: auto; overflow-x: hidden; }
#table {position: relative; left: .5em; font-size: 9.5pt; word-spacing: -1px; width: 750px;}
#table .shorter {width: 55px;}
#table .eC {width: 100px;}
.rowOuter {!important; margin-bottom: .5em !important; padding: .5em !important; padding-right:0 !important; padding-bottom: .1em !important;}
.light {background: #555;}
.dark {background: #111;}
#tableHeader .cell {border: none;
	border-bottom: 5px solid #990000;
	height: 20px !important;  
	width: 97px;
	 margin:0;
	  padding: 0; 
	  font-weight:bold;
	  margin-right: 3px;}
#tableHeader .longest {width: 184px;}
#tableHeader .longer{width: 137px;}
#tableHeader .long {width: 99px;}
#tableHeader .short {width: 65px;}
#tableHeader .shortest {width: 29px;}
#tableHeader .infoCell {width: 674px;}
#tableHeader .mid {width: 85px;}
#tableHeader .shorter {width: 57px;}
.cell{text-align: center; border-left: 2px solid #860000;}
.noBorder {border: none !important;}
#tableHeader .rowOuter {border: none !important;}
#tableHeader .start {margin-left: 50px; width: 89px;}
.dark .marketSection {background: #460000 !important;}
.light .marketSection {background: #460000 !important;}
.border {border: 3px red solid;}
.med {border: 3px yellow solid;}
input[name="price"] {background: green !important; }
.hider {display: none;}

.imageWrap .text {
	position:absolute ;
	top:8px; /* in conjunction with left property, decides the text position */
	left:4px;
	width:300px; /* optional, though better have one */
color:Red;
}
.hideLoader {display: none;}

html {
      background: #333;
      color: #eee;
      font-family: 'Trebuchet MS', Arial, sans-serif;
    }
    #filter-form-wrapper .subtext {
      font-size: .8em;
      font-weight: normal;
    }

    #filter-form-wrapper form {
      background: #880000;
      border-top: 1px white solid;
      border-bottom: 1px white solid;      
      display: inline-block;
      padding: 5px 5px 5px 0;
    }

    #filter-form-wrapper a {
      /*color: red;*/
      text-decoration: none;
    }
    
    .search-submit {
        background: none repeat scroll 0 0 #666666;
    border: medium none #000000;
    color: #FFFFFF;
    cursor: pointer;
    display: block;
    font-size: 14px;
    font-weight: normal;
    padding: 2px 8px;
    }
    
    .search-submit:hover {
    background: none repeat scroll 0 0 #666666;
    border: medium none #000000;    
    color: #ffffff;
    cursor: pointer;}
</style>
	<title>Search VinControl</title>
</asp:Content>
