<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlMake", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All Make", new { @class = "make multi-select", multiple = "multiple", style = "width:120px;" })%>

<script type="text/javascript">
    var makes = selectedMake;
    var allmakes = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlMake").multiselect({
                header: false,
                noneSelectedText: "Make",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    makes += ',' + (value);
                            });
                            allmakes = makes;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            makes = '';
                        }
                    } else {

                        if (ui.checked) {
                            makes += ',' + (ui.value);
                        }
                        else {
                            makes = makes.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allmakes != '' && (allmakes.length != makes.length))
                            emptyOption.prop('checked', false);
                        else if (allmakes != '' && (allmakes.length == makes.length))
                            $(this).multiselect("checkAll");
                    }
                },
                create: function () { $(this).multiselect('widget').width(120); },
                beforeopen: function () {

                },
                open: function () {

                },
                beforeclose: function () {

                },
                close: function () {
                    $('#SelectedMake').val(makes);
                    if (makes != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Search/GenerateModels?makes=' + makes,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divModel').html(result);
                            },
                            error: function (err) {
                                $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select"></select>');
                                $("select#ddlModel").multiselect({ header: false, noneSelectedText: "Model", minWidth: 120 });
                                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                                $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select"></select>');
                        $("select#ddlModel").multiselect({ header: false, noneSelectedText: "Model", minWidth: 120 });
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                        $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    makes = '';
                    $('#SelectedMake').val(makes);
                },
                optgrouptoggle: function (event, ui) {

                }
            });
        });
    });
</script>