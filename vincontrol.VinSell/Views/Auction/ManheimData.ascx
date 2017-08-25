<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.ManheimWholesaleViewModel>>" %>
<% if (Model.Count == 0) {%> 
<div class="subheader-nav"></div>
<div class="data-content">No data</div>
<%} else {%>

<% if (Model.Count == 1) {%> 
<div class="manheimTrim">
<div class="subheader-nav">
    <div class="subnav-text"><a style="color: white;text-underline-style: thick;" href="javascript:displayManheim()"><%= Model.FirstOrDefault().TrimName%></a></div>    
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Above</span><img src='<%= Url.Content("~/Content/Images/market-high.jpg") %>'>
            <%= Model.FirstOrDefault().HighestPrice%></li>
        <li><span class="label">Average</span><img src='<%= Url.Content("~/Content/Images/market-mid.jpg") %>'>
            <%= Model.FirstOrDefault().AveragePrice%></li>
        <li><span class="label">Below</span><img src='<%= Url.Content("~/Content/Images/market-low.jpg") %>'>
            <%= Model.FirstOrDefault().LowestPrice%></li>
    </ul>
</div>
</div>
<%} else {%> 
<% foreach (var item in Model){%>
<div class="manheimTrim" id="manheimTrim_<%= item.TrimServiceId %>" <%= item.TrimName.Equals(Model.FirstOrDefault().TrimName) ? "" : "style='display:none'" %>>
<div class="subheader-nav">
    <div class="subnav-text"><a style="color: white;text-underline-color: blue"  href="javascript:displayManheim()"><%= item.TrimName%></a></div>
    <div class="left-control" id="manheim-left-control_<%= item.TrimServiceId %>">
        <img src='<%= Url.Content("~/Content/Images/data-box-left.jpg") %>'></div>
    <div class="right-control" id="manheim-right-control_<%= item.TrimServiceId %>">
        <img src='<%= Url.Content("~/Content/Images/data-box-right.jpg") %>'></div>
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Above</span><img src='<%= Url.Content("~/Content/Images/market-high.jpg") %>'>
            <%= item.HighestPrice%></li>
        <li><span class="label">Average</span><img src='<%= Url.Content("~/Content/Images/market-mid.jpg") %>'>
            <%= item.AveragePrice%></li>
        <li><span class="label">Below</span><img src='<%= Url.Content("~/Content/Images/market-low.jpg") %>'>
            <%= item.LowestPrice%></li>
    </ul>
</div>
</div>      
<%}%>
<%}%>

<%}%>

<script type="text/javascript">

    $(document).ready(function () {
        $(".manheimTrim .left-control").click(function () {
            var id = this.id.split('_')[1];

            if ($('#manheimTrim_' + id).prev().length > 0) {
                $(".manheimTrim").hide();
                $('#manheimTrim_' + id).prev().show();
            }
            else {
                //$('#manheimTrim_' + id).show();
                $(this).hide();
            }
        });

        $(".manheimTrim .right-control").click(function () {
            var id = this.id.split('_')[1];

            if ($('#manheimTrim_' + id).next().length > 0) {
                $(".manheimTrim").hide();
                $('#manheimTrim_' + id).next().show();
            }
            else {
                //$('#manheimTrim_' + id).show();
                $(this).hide();
            }
        });
    });
</script>


