<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlAuction", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All Auctions", new { @class = "auction multi-select", multiple = "multiple" })%>

<script type="text/javascript">
    var auctions = '';
    var allauctions = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlAuction").multiselect({
                header: false
                , click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    auctions += ',' + (value);
                            });
                            allauctions = auctions;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            auctions = '';
                        }
                    } else {

                        if (ui.checked) {
                            auctions += ',' + (ui.value);
                        }
                        else {
                            auctions = auctions.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allauctions != '' && (allauctions.length != auctions.length))
                            emptyOption.prop('checked', false);
                        else if (allauctions != '' && (allauctions.length == auctions.length))
                            $(this).multiselect("checkAll");
                    }
                },
                beforeopen: function () {

                },
                open: function () {

                },
                beforeclose: function () {

                },
                close: function () {
                    $("#SelectedAuction").val(auctions);
                    if (auctions != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Ajax/GenerateSellers?auctions=' + auctions,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divSeller').html(result);
                            },
                            error: function (err) {
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                                $("select#ddlSeller").multiselect({ header: false });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select" multiple = "multiple"></select>');
                        $("select#ddlSeller").multiselect({ header: false });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    auctions = '';
                    $("#SelectedAuction").val(auctions);
                },
                optgrouptoggle: function (event, ui) {

                }
            });
        });
    });
</script>