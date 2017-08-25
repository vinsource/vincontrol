<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.BuyerGuideViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Buyer's Guide</title>
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function SubmitPDF(selectedLanguageID) {
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
            $.post('<%= Url.Content("~/Report/ViewBuyerGuideinPdfNew") %>', { selectedLanguage: selectedLanguageID, listingID:<%=Model.ListingId %> }, function(data) {
                //window.location = data.url;
                //window.open(data.url);
                window.parent.PopupBuyerGuideWindow(data.url);
            });
        }

    </script>
</head>
<body>
    <div id="profile_bg_holder" class="profile_tab_holder" style="display: block;">
        <div id="container_right_btn_holder">
            <div id="container_right_btns" style="float: left">
                <% foreach (var language in Model.Languages)
                {%>
                    <div class="btns_shadow profile_bg_btns bg_enlish_print" onclick="SubmitPDF(<%=language.Value %>)">
                        <%=language.Text %>
                    </div>
                <% } %>
            </div>
        </div>
        <div id="container_right_content" style="height: 600px; padding-top: 10px;">
            <div class="profile_bg_content">
                <div class="profile_bg_content_left">
                    <img src="/content/images/bGuideBG.png">
                </div>
                <div class="profile_bg_content_right">
                    <img src="/content/images/bGuideBG-spanish-resize.png">
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
    <%--<h1 style="margin-bottom: 0;">
        Buyer's Guide</h1>
    <div id="container">
        <% Html.BeginForm("ViewBuyerGuideinPdf", "Report", FormMethod.Post); %>
        <input type="hidden" name="warrantyInfo" />
        <h3>
            Select a language to print</h3>
        <%=Html.DropDownListFor(x=>x.SelectedLanguage,Model.Languages) %>
        <%=Html.HiddenFor(x=>x.ListingId) %>
        <input type="submit" name="submit" value="Print Guide" />
        <% Html.EndForm(); %>
        <img alt="" src="../images/bGuideBG.png" width="200" />
        <img alt="" src="../images/bGuideBG-spanish-resize.png" width="200" />
    </div>--%>
</body>
</html>
