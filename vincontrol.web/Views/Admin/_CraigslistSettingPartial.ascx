<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AdminViewModel>" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#craigslistState").live('change', function () {
            var state = $(this).val();
            $("#craigslistLocation").html('<option value="0">-- Choose location --</option>');
            $("select[id^='craigslistCity_']").hide();
            $("#craigslistCity_" + state).show();
        });
        LoadSubLocationList("<%= Model.DealerCraigslistSetting.CityUrl %>");
        $("select[id^='craigslistCity_']").live('change', function () {
            var city = $(this).val();
            LoadSubLocationList(city);
        });
    });


    function LoadSubLocationList(city) {
        if (city == '0') {
            $("#craigslistLocation").html('<option value="0">-- Choose location --</option>');
        } else {
            $("#craigslistLocation").html('<option value="0">loading...</option>');
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Craigslist/GetSubLocationList?location=" + city,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    var locationId = '<%= Model.DealerCraigslistSetting.LocationId %>';
                        var str = '<option value="0">-- Choose location --</option>';
                        $.each(result.data, function (i, item) {
                            str += '<option value="' + item.Value + '"' + (locationId == item.Value ? ' selected' : '') + '>' + item.Name + '</option>';
                        });
                        $("#craigslistLocation").html(str);
                    },
                    error: function (err) {
                        $("#craigslistLocation").html('<option value="0">-- Choose location --</option>');
                    }
                });

                }
            }
</script>

<div style="float: left;">

    <select id="craigslistState" style="width: 160px;">
        <option value="0">-- Choose state --</option>
        <% foreach (var state in Model.DealerCraigslistSetting.States.OrderBy(i => i.Name))
           {%>
        <option value="<%= state.Name %>" <%= (Model.DealerCraigslistSetting.State != null && Model.DealerCraigslistSetting.State.Equals(state.Name)) ? "selected" : "" %>><%= state.Name %></option>
        <%}%>
    </select>
</div>
<div style="float: left;">

    <select id="craigslistCity_0" <%= String.IsNullOrEmpty(Model.DealerCraigslistSetting.State) ? "style='width: 160px;'" : "style='display: none;'" %>>
        <option value="0">-- Choose city --</option>
    </select>
    <% foreach (var state in Model.DealerCraigslistSetting.States)
       {%>
    <select id="craigslistCity_<%= state.Name %>" <%= (Model.DealerCraigslistSetting.State != null && Model.DealerCraigslistSetting.State.Equals(state.Name)) ? "style='width: 160px;'" : "style='display: none;width: 160px;'" %>>
        <option value="0">-- Choose city --</option>
        <% foreach (var city in state.Cities)
           {%>
        <option value="<%= city.Url %>" <%= Model.DealerCraigslistSetting.CityUrl.Equals(city.Url) ? "selected" : "" %>><%= city.Name %></option>
        <%}%>
    </select>
    <%}%>
</div>
<div style="float: left;">

    <select id="craigslistLocation" style="width: 160px;">
        <option value="0">-- Choose location --</option>
    </select>
</div>
<div style="float: left;">
    &nbsp;Specific Location &nbsp;<%= Html.EditorFor(x => x.DealerCraigslistSetting.SpecificLocation) %>
</div>
