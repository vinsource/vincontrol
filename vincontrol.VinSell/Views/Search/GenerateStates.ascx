<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlState", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All States", new { @class = "state multi-select", multiple = "multiple" })%>

<script type="text/javascript">
    var states = selectedState;
    var allstates = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlState").multiselect({
                header: false,
                noneSelectedText: "State",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    states += ',' + (value);
                            });
                            allstates = states;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            states = '';
                        }
                    } else {

                        if (ui.checked) {
                            states += ',' + (ui.value);
                        }
                        else {
                            states = states.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allstates != '' && (allstates.length != states.length))
                            emptyOption.prop('checked', false);
                        else if (allstates != '' && (allstates.length == states.length))
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
                    $("#SelectedState").val(states);
                    if (states != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Search/GenerateAuctions?states=' + states,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divAuction').html(result);
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                            },
                            error: function (err) {
                                $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select"></select>');
                                $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                                $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                                $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divAuction').html('<select id="ddlAuction" name="ddlAuction" class="auction multi-select"></select>');
                        $("select#ddlAuction").multiselect({ header: false, noneSelectedText: "Auction", minWidth: 120 });
                        $('#divSeller').html('<select id="ddlSeller" name="ddlSeller" class="seller multi-select"></select>');
                        $("select#ddlSeller").multiselect({ header: false, noneSelectedText: "Seller", minWidth: 120 });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    states = '';
                    $("#SelectedState").val(states);
                },
                optgrouptoggle: function (event, ui) {

                }
            });
        });
    });
</script>