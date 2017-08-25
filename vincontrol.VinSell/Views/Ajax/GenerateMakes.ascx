<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlMake", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All Makes", new { @class = "make multi-select", multiple = "multiple" })%>

<script type="text/javascript">
    var makes = '';
    var allmakes = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlMake").multiselect({
                header: false
                , click: function (event, ui) {
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
                            url: '/Ajax/GenerateModels?makes=' + makes,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divModel').html(result);
                            },
                            error: function (err) {
                                $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                                $("select#ddlModel").multiselect({ header: false });
                                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                                $("select#ddlTrim").multiselect({ header: false });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divModel').html('<select id="ddlModel" name="ddlModel" class="model multi-select" multiple = "multiple"></select>');
                        $("select#ddlModel").multiselect({ header: false });
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select" multiple = "multiple"></select>');
                        $("select#ddlTrim").multiselect({ header: false });
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