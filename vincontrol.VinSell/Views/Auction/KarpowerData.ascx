<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.SmallKarPowerViewModel>>" %>

<% if (Model.Count == 0) {%> 
<div class="subheader-nav"></div>
<div class="data-content">No data</div>
<%} else {%>

<% if (Model.Count == 1) {%> 
<div class="karpowerTrim">
<div class="subheader-nav">
    <div class="subnav-text" id="karpower_header_<%= Model.FirstOrDefault().SelectedTrimId %>" style="cursor:pointer;"><a style="color: white;text-underline-style: thick;" href="javascript:displayKBB(<%= Model.FirstOrDefault().SelectedTrimId %>)"><%= Model.FirstOrDefault().SelectedTrimName%></a></div>    
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Lending Average</span> <%= Model.FirstOrDefault().BaseWholesale %></li>
        <li><span class="label">Mileage Adjustement</span> <%= Model.FirstOrDefault().MileageAdjustment %></li>
        <li><span class="label">Blue Book Value</span> <%= Model.FirstOrDefault().Wholesale %></li>
    </ul>
</div>
</div>
<%} else if (Model.Count(i => i.IsSelected) == 1) {%> 
<div id="divSingleKbbTrim">
<div class="karpowerTrim">
<div class="subheader-nav">
    <div class="subnav-text" id="karpowerTrim_<%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimId %>" style="cursor:pointer;"><a style="color: white;text-underline-style: thick;" href="javascript:displayKBB(<%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimId %>)"><%= Model.FirstOrDefault(i => i.IsSelected).SelectedTrimName%></a></div>    
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Lending Average</span> <%= Model.FirstOrDefault(i => i.IsSelected).BaseWholesale%></li>
        <li><span class="label">Mileage Adjustement</span> <%= Model.FirstOrDefault(i => i.IsSelected).MileageAdjustment%></li>
        <li><span class="label">Blue Book Value</span> <%= Model.FirstOrDefault(i => i.IsSelected).Wholesale%></li>
    </ul>
</div>
</div>
</div>
<div id="divMultipleKbbTrim" style="display:none;">
<% foreach (var item in Model){%>
<div class="karpowerTrim" id="karpowerTrim_<%= item.SelectedTrimId %>" <%= item.SelectedTrimName.Equals(Model.FirstOrDefault().SelectedTrimName) ? "" : "style='display:none'" %>>
<div class="subheader-nav">
    <div class="subnav-text" id="karpower_header_<%= item.SelectedTrimId %>" style="cursor:pointer;"><a style="color: white;text-underline-style: thick;"  href="javascript:displayKBB(<%= item.SelectedTrimId %>)"><%= item.SelectedTrimName%></a></div>
    <div class="left-control" id="karpower-left-control_<%= item.SelectedTrimId %>">
        <img src='<%= Url.Content("~/Content/Images/data-box-left.jpg") %>'></div>
    <div class="right-control" id="karpower-right-control_<%= item.SelectedTrimId %>">
        <img src='<%= Url.Content("~/Content/Images/data-box-right.jpg") %>'></div>
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Lending Average</span> <%= item.BaseWholesale%></li>
        <li><span class="label">Mileage Adjustement</span> <%= item.MileageAdjustment%></li>
        <li><span class="label">Blue Book Value</span> <%= item.Wholesale%></li>
    </ul>
</div>
</div>      
<%}%>
</div>
<%} else {%>
<div id="divMultipleKbbTrim"> 
<% foreach (var item in Model){%>
<div class="karpowerTrim" id="karpowerTrim_<%= item.SelectedTrimId %>" <%= item.SelectedTrimName.Equals(Model.FirstOrDefault().SelectedTrimName) ? "" : "style='display:none'" %>>
<div class="subheader-nav">
    <div class="subnav-text" id="karpower_header_<%= item.SelectedTrimId %>" style="cursor:pointer;"><a style="color: white;text-underline-style: thick;" href="javascript:displayKBB(<%= item.SelectedTrimId %>)"><%= item.SelectedTrimName%></a></div>
    <div class="left-control" id="karpower-left-control_<%= item.SelectedTrimId %>">
        <img src='<%= Url.Content("~/Content/Images/data-box-left.jpg") %>'></div>
    <div class="right-control" id="karpower-right-control_<%= item.SelectedTrimId %>">
        <img src='<%= Url.Content("~/Content/Images/data-box-right.jpg") %>'></div>
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Lending Average</span> <%= item.BaseWholesale%></li>
        <li><span class="label">Mileage Adjustement</span> <%= item.MileageAdjustment%></li>
        <li><span class="label">Blue Book Value</span> <%= item.Wholesale%></li>
    </ul>
</div>
</div>      
<%}%>
</div>
<%}%>

<%}%>

<script type="text/javascript">

    $(document).ready(function () {
        var numberOfTrims = '<%= Model.Count %>';
        if (numberOfTrims <= 1) {
            $('#clickNotCorrectKbbTrim').hide();
        } else {
            $('#clickNotCorrectKbbTrim').click(function () {
                $('#divMultipleKbbTrim').show();
                $('#divSingleKbbTrim').hide();
            });
        }

        $('#kbbTrims').html('KBB(' + '<%= Model.Count %>' + ')');
        $(".karpowerTrim .left-control").click(function () {
            var id = this.id.split('_')[1];
            console.log($('#divMultipleKbbTrim #karpowerTrim_' + id));
            if ($('#divMultipleKbbTrim #karpowerTrim_' + id).prev().length > 0) {
                $(".karpowerTrim").hide();
                $('#divMultipleKbbTrim #karpowerTrim_' + id).prev().show();
            } else {
                //$('#karpowerTrim_' + id).show();
                $(this).hide();
            }
        });

        $(".karpowerTrim .right-control").click(function () {
            var id = this.id.split('_')[1];
            console.log($('#divMultipleKbbTrim #karpowerTrim_' + id));
            if ($('#divMultipleKbbTrim #karpowerTrim_' + id).next().length > 0) {
                $(".karpowerTrim").hide();
                $('#divMultipleKbbTrim #karpowerTrim_' + id).next().show();
            } else {
                //$('#karpowerTrim_' + id).show();
                $(this).hide();
            }
        });
    });
</script>