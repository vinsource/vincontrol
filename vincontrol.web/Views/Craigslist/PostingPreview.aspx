<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Craigslist</title>
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.alerts.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/jquery.alerts.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .error
        {
            border-color: red;
        }
        #progressBarHolder{
			/*width: 600px;*/
			margin: 0px auto;
		}
        .prBars_phase1
        {
            background-image: url("../../images/1_on.png");
        }
        .prBars_phase2
        {
            width: 176px !important;
            background-image: url("../../images/2_on.png");
        }
        .prBars_phase3
        {
            width: 176px !important;
            background-image: url("../../images/3_on.png");
        }
        .prBars_phase4
        {
            background-image: url("../../images/4_on.png");
        }
        .prBars_phase1_active
        {
            background-image: url("../../images/1.png");
        }
        .prBars_phase2_active
        {
            width: 176px !important;
            background-image: url("../../images/2.png");
        }
        .prBars_phase3_active
        {
            width: 176px !important;
            background-image: url("../../images/3.png");
        }
        .prBars_phase4_active
        {
            background-image: url("../../images/4.png");
        }
        #progressBarHolder > div
        {
            width: 172px;
            height: 72px;
            float: left;
            margin-left: -24px;
            background-repeat: no-repeat;
            /*cursor: pointer;*/
        }
    </style>
    <script type="text/javascript">
        var listingId = '<%= ViewData["LISTINGID"]%>';
        
    </script>
    <script src="<%=Url.Content("~/js/VinControl/craigslist.js")%>" type="text/javascript"></script>
</head>
<body>
    <form id="postingForm" style="display: inline-block; padding-top: 5px;">
    
        <div id="progressBarHolder">
            <div class="prBars_phase1 prBars_phase1_active" id="prBars_phase1">
            </div>
            <div class="prBars_phase2" id="prBars_phase2">
            </div>
            <div class="prBars_phase3" id="prBars_phase3">
            </div>
            <div class="prBars_phase4" id="prBars_phase4">
            </div>
        </div>
        <div id="content" style="display: inline-block;margin: 0 auto;">
            <%--<img id="waitingImage" src="../../Content/Images/ajaxloadingindicator.gif" style="border: 0;position:absolute; top: 280px; left: 280px; display: none;"/>--%>
        </div>
    </form>
</body>
</html>
