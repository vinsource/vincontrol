<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.InventoryFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Key Performance Index</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/jScroll/style/jquery.jscrollpane.css")%>" rel="stylesheet"
        type="text/css" media="all" />

    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.mousewheel.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/jScroll/script/jquery.jscrollpane.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <style type="text/css">
        #figures
        {
            width: 600px;
            height: 250px;
            padding: 0;
        }
        #figures h3
        {
            margin-bottom: .6em;
        }
        #c3 li
        {
            width: 200px;
        }
        #c3 ul.info
        {
            height: 100px;
            margin-bottom: 1em;
        }
        p
        {
            padding: 1em;
            border-bottom: 1px solid #101010;
            border-top: 1px #777 solid;
            margin: 0;
        }
        p.top
        {
            border-top: none;
            padding: 0;
            margin-top: .5em;
        }
        p.bot
        {
            border-bottom: none;
            padding: 0;
        }
        #figures.column
        {
            margin: 1em;
            width: 250px;
            background: #000;
        }
        .colorBar
        {
            background: black;
            width: 255px;
            padding: .4em;
            height: 24px;
            margin-bottom: 6px;
            font-weight: bold;
        }
        .colorBar div
        {
            float: left;
            clear: right;
            margin: 0;
        }
        /* padding: .4em; margin-bottom: .5em; font-weight: bold;  */#red .symbol
        {
            background: url(../images/aboveLRG.jpg) top left no-repeat;
            width: 45px;
            height: 17px;
            margin-top: .1em;
        }
        #green .symbol
        {
            background: url(../images/atLRG.jpg) top left no-repeat;
            width: 45px;
            height: 17px;
            margin-top: .2em;
        }
        #blue .symbol
        {
            background: url(../images/belowLRG.jpg) top left no-repeat;
            width: 45px;
            height: 17px;
            margin-top: .3em;
        }
        .text
        {
            width: 120px;
        }
        .symbol
        {
            width: 45px;
        }
        #market
        {
            margin: 0;
            padding: 0;
            width: 260px;
        }
        #age
        {
            margin: 0;
            padding: 0;
            position: relative;
            left: 15px;
        }
        table
        {
            text-align: center;
            margin-top: -3px;
            width: 275px;
        }
        table td
        {
            padding: .2em;
        }
        tr.topCell td
        {
            background: #000;
        }
        .itemValue
        {
            margin-top: 0em;
        }
        #buttons table
        {
            background: #333;
            margin: 0 auto;
            position: relative;
        }
        #buttons td
        {
            font-size: .9em;
            color: white;
            text-align: center;
            width: 200px !important;
            background: #444;
            border: 1px solid #999;
            border-top: 1px solid #222;
            border-left: 1px solid #111;
        }
        .scroll-pane
        {
            height: 100%;
            overflow: auto;
            overflow-x: hidden;
        }
        #activity
        {
            /*height: 200px;*/
            margin-top: -2em;
        }
        .high
        {
            font-weight: bold;
            background: green !important;
            font-size: 1.4em !important;
        }
        .mid
        {
            font-weight: bold;
            background: #F60 !important;
            font-size: 1.4em !important;
        }
        .low
        {
            font-weight: bold;
            background: #d02c00 !important;
            font-size: 1.4em !important;
        }
        a.aDetailActivity :hover {color: Red;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <script type="text/javascript" >
     $("#InventoryTab").attr("class", "");
     $("#AppraisalTab").attr("class", "");
     $("#KPITab").attr("class", "on");
     $("#AdminTab").attr("class", "");
     $("#ReportTab").attr("class", "");
    
 </script>
   <input type="hidden" id="NeedToRedirectToDetailActivity" name="NeedToRedirectToDetailActivity" />
   <input type="hidden" id="ActivityId" name="ActivityId" />
   <div id="topNav">
                <input class="btn" type="button" name="preownedKPI" value=" Pre-Owned " id="btnPreowned" />
                <input class="btn onadmin" type="button" name="newKPI" value=" New " id="btnNew" />
               
                </div>
            <h3>Key Performance Index</h3>
            <div id="gauges">
            	<div class="gauges" id="iHealth">
                </div>
                <div id="buttons">
                	<table id="graphButtons">
                        <tr>
                            <td>Pictures</td>
                            <td>Descriptions</td>
                            <td>Price</td>                        	
                        </tr>
                        <tr>
                            <%=Html.DynamicHtmlLabelForKPI("txtPercentPics", "NPercentPics")%>
                            <%=Html.DynamicHtmlLabelForKPI("txtPercentDescriptions", "NPercentDescriptions")%>
                            <%=Html.DynamicHtmlLabelForKPI("txtPercentSalePrice", "NPercentSalePrice")%>                        	
                        </tr>
                    </table>

                </div>
            </div>
            <div id="figures">
            	
                <div id="age" class="column">
                    <%=Html.DynamicHtmlLabelForKPI("txtHiddenContentGauge", "NHiddenContentGauge")%>
                <h3>Inventory Age</h3>
                	<table>
                    	<tr class="topCell">
                        	<td>Days</td>
                            <td># of Cars</td>
                            <td>% of Inventory</td>
                        </tr>
                        <tr class="greenL">
                        	<td>0-15</td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txt15Inventory", "N0-15InInventory")%></td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "NPercent0-15InInventory")%></td>
                        </tr>
                        <tr class="greenD">
                        	<td>16-30</td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txt30Inventory", "N16-30InInventory")%></td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txtPercent30Inventory", "NPercent16-30InInventory")%></td>
                        </tr>
                        <tr  class="blue">
                        	<td>31-60</td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txt60Inventory", "N31-60InInventory")%></td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txtPercent60Inventory", "NPercent31-60InInventory")%></td>
                        </tr>
                        <tr class="orange">
                        	<td>61-90</td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txt90Inventory", "N61-90InInventory")%></td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txtPercent90Inventory", "NPercent61-90InInventory")%></td>
                        </tr>
                        <tr class="red">
                        	<td>90+</td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txtOverInventory", "N90OverInInventory")%></td>
                            <td><%=Html.DynamicHtmlLabelForKPI("txtPercentOverInventory", "NPercent90OverInInventory")%></td>
                        </tr>
                   </table>
                </div>
            </div>
             <div id="activity" class="clear"><br />
                
                <div style="margin-top: 10px;">
                <% if (Model.DealershipActivities != null && Model.DealershipActivities.Count() > 0) {%>
                <h3>Dealership Activity</h3>
                <% foreach (var item in Model.DealershipActivities)
                   { %>
                <a class="aDetailActivity" href="<%= Url.Action("RedirectToDetailActivity", "Market", new { id = item.Id}) %>" style="color:#ddd; font-weight:normal;">
                <div style="font-size:0.9em;margin-bottom:5px;">
                    <div style="float: left; width: 110px;font-style:italic;overflow:hidden;"><%= item.UserStamp %>:</div><div style="width: 100%;font-style:italic;margin-left:130px;"><%= item.Title %></div>
                    <div style="margin-left:130px;"><%= item.DateStamp.ToString("MM/dd/yyyy hh:mm:ss tt")%></div>
                </div>
                </a>
                <% } %>
                <a style="display:inline-block;" id="aViewMoreActivity" href="javascript:;" title="View All Activities">View More</a>
                <%}%>
                
                </div>
                	<div class="scroll-pane">
              			<p class="top"></p>
                        <p class="bot"></p>
                    </div>
                </div>
            </div>
                  <div id="c3" class="column">
            	<span id="sort">   
            	
            	<% Html.BeginForm("SortBy", "Market", FormMethod.Post);%>
             
               Sort By: <%=Html.DropDownListFor(x => x.SelectedSortSet, Model.SortSetList, new { @onChange = "  $('#elementID').removeClass('hideLoader');this.form.submit()" })%>
                <% Html.EndForm();%>
                 </span><br  />
            	  <h3>
            List of Cars <a class="iframe" href=<%=Url.Content("~/Report/PrintOption?Condition=New")%> style="background-color: #666;
                padding: 2px; font-size: 13px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;">
                Print </a>
        </h3>
                        <div class="scroll-pane">
            	<ul>
                	<%=Html.DynamicHtmlLabelForKPI("txtGridKPISideBar","KPISideBar")%>
                </ul>
                 </div>
            </div>
            </div>
            
     
    
<script type='text/javascript' src='https://www.google.com/jsapi'></script>
<script type='text/javascript' >


    $("#btnKPIPrint").click(function () {

        var actionUrl = '<%= Url.Action("ViewKPIReport", "Market") %>';
        window.location = actionUrl;


    });

    $("#btnPreowned").click(function () {

        $('#elementID').removeClass('hideLoader');

        var actionUrl = '<%= Url.Action("ViewKPI", "Market") %>';

        window.location = actionUrl;


    });

    $("#btnNew").click(function () {


        $('#elementID').removeClass('hideLoader');


        var actionUrl = '<%= Url.Action("DecodeProcessingTruckManual", "Decode") %>';

        window.location = actionUrl;


    });
                             
</script>
            
						<script type='text/javascript'>

						    var name = "Content";
						    var value = 20;
						 
						</script>
     <script type='text/javascript' src='https://www.google.com/jsapi'></script>
					                        
                         <script type='text/javascript'>

                             google.load('visualization', '1', { packages: ['gauge'] });
                             google.setOnLoadCallback(drawChart);
                             function drawChart() {
                                 //                                 var percentageInventory = document.getElementById("txtHiddenInventoryGauge").value;
                                 //                                 var percentageContent = document.getElementById("txtHiddenContentGauge").value;
                                 //                                 var numberInventory = parseInt(percentageInventory);
                                 //                                 var numberContent = parseInt(percentageContent);

                                 var data = google.visualization.arrayToDataTable([
                            ['Label', 'Value'],
                            ['Invnetory', 100],
                            ['Content', 100],
                            ['Website', 100]
        ]);

                                 var options = {
                                     width: 565,
                                     height: 200,
                                     greenFrom: 66,
                                     greenTo: 100,
                                     redFrom: 0,
                                     redTo: 33,
                                     yellowFrom: 33,
                                     yellowTo: 66,
                                     minorTicks: 15
                                 };
                                 var chart = new google.visualization.Gauge(document.getElementById('iHealth'));
                                 chart.draw(data, options);

                             }



                             $(function () {
                                 $('.scroll-pane').jScrollPane();
                             });
                             $('a:not(.iframe)').click(function (e) {
                                 if ($(this).attr('target') == '')
                                     $('#elementID').removeClass('hideLoader');

                             });
                             $(document).ready(function () {
                                 $("a.iframe").fancybox({ 'width': 500, 'height': 350, 'hideOnOverlayClick': false, 'centerOnScroll': true });

                                 $('#aViewMoreActivity').click(function (e) {
                                     $.fancybox({
                                         href: '<%= Url.Action("ViewAllDealerActivity", "Market") %>',
                                         'type': 'iframe',
                                         'width': 1000,
                                         'height': 450,
                                         'hideOnOverlayClick': false,
                                         'centerOnScroll': true,
                                         'onCleanup': function () {
                                             $('#elementID').addClass('hideLoader');
                                         },
                                         onClosed: function () {
                                             if ($('#NeedToRedirectToDetailActivity').val() == 'true') {
                                                 window.location.href = '<%= Url.Action("RedirectToDetailActivity", "Market")%>' + '/' + $('#ActivityId').val();
                                             }
                                             $('#elementID').addClass('hideLoader');
                                         }
                                     });
                                 });
                             });
        



                         </script>


</asp:Content>

