<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Vincontrol.Web.Models.BucketJumpHistory>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bucket Jump Tracking</title>
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="profile_tab_holder profile_popup" id="profile_bucketjump_holder" style="display: block">
        <div class="bucketjump_popup_header">
            Bucket Jump Report Tracking
        </div>
        <% if (Model != null && Model.Any())
           {%>
        <div class="bucketjump_popup_content">
            <div class="bucketjump_popup_content_items bucketjump_popup_content_header">
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_userstamp">
                    User Stamp
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_datestamp">
                    Date Stamp
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_price">
                    Price Before Bucket Jump
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_price">
                    Price After Bucket Jump
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_download">
                    Download
                </div>
            </div>
            <% foreach (var item in Model)
               {%>
            <div class="bucketjump_popup_content_items">
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_userstamp">
                    <%= item.UserStamp %>
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_datestamp">
                    <%= item.DateStamp.ToString("MM/dd/yyy hh:mm:ss") %>
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_price">
                    <%= item.SalePrice.Equals(0) ? "&nbsp; " : item.SalePrice.ToString("c0")%>
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_price">
                    <%= item.RetailPrice.Equals(0) ? "&nbsp; " : item.RetailPrice.ToString("c0")%>
                </div>
                <div class="bucketjump_popup_collumn bucketjump_popup_collumn_download">
                    <a href="<%= Url.Action("DownloadBucketJumpReport", "Inventory", new { name = item.AttachFile}) %>"
                        id="aDownloadReport_<%= item.Id %>">
                        <div class="bucketjump_popup_download_btns">
                            View PDF
                            <nobr> >></nobr>
                        </div>
                    </a>
                </div>
            </div>
            <%}%>
        </div>
        <%}
           else
           {%>
        There is no record.
        <%}%>
    </div>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
</body>
</html>
