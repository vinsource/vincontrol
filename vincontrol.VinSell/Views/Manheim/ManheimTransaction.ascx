<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.ManheimTransactionViewModel>>" %>

<div>
    <form id="manheimReportForm" method="post" action="">    
    <br />
    <div>
        <strong class="reportHeader">REPORTED WHOLESALE AUCTION SALES - With Exact Matches</strong>
        <div style="margin-right: 5px; float: right;">
            <select id="ddlRegion" name="ddlRegion">
                <option value="NA">National</option>
                <option value="SE">South East</option>
                <option value="NE">North East</option>
                <option value="MW">Mid West</option>
                <option value="SW">South West</option>
                <option value="WC">West Coast</option>
            </select>
        </div>
    </div>
    <hr />
    </form>
    <div id="result">
        <%= Html.Partial("ManheimTransactionDetail", Model) %></div>
</div>

<script type="text/javascript">
    $(document).ready(function () {

        // add parser through the tablesorter addParser method 
        $.tablesorter.addParser({
            // set a unique id 
            id: 'price',
            is: function (s) {
                // return false so this parser is not auto detected 
                return false;
            },
            format: function (s) {
                // format your data for normalization 
                return s.replace('$', '').replace(/,/g, '');
            },
            // set type, either numeric or text 
            type: 'numeric'
        });

        $("table#manheimTransaction").tablesorter({
            // prevent first column from being sortable
            headers: {
                //0: { sorter: false },
                1: { sorter: 'price' }, // miles
                2: { sorter: 'price', sortInitialOrder: 'asc'}// prices
            }
        });

        $("#ddlRegion").change(function () {
            $.blockUI({ message: '<div><img src="/images/ajax-loader1.gif" /></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
            blockUI(waitingImage);
            $.ajax({
                type: "POST",
                url: "/Manheim/ManheimTransactionDetail?year=" + year + "&make=" + makeId + "&model=" + modelId + "&trim=" + trimId + "&region=" + $("#ddlRegion").val(),
                data: {},
                success: function (results) {
                    $("#result").html(results);
                    unblockUI();
                    //$("table#manheimTransaction").trigger("update");
                    $("table#manheimTransaction").tablesorter({
                        headers: {
                            1: { sorter: 'price' }, // miles
                            2: { sorter: 'price', sortInitialOrder: 'asc'}// prices
                        }
                    });
                },
                error: function (err) {
                    unblockUI();
                }
            });
        });
    });
</script>