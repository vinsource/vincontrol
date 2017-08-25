<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlModel", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All Model", new { @class = "model multi-select", style = "width:120px;", multiple = "multiple" })%>

<script type="text/javascript">
    var models = selectedModel;
    var allmodels = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlModel").multiselect({
                header: false,
                noneSelectedText: "Model",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    models += ',' + (value);
                            });
                            allmodels = models;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            models = '';
                        }
                    } else {

                        if (ui.checked) {
                            models += ',' + (ui.value);
                        }
                        else {
                            models = models.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allmodels != '' && (allmodels.length != models.length))
                            emptyOption.prop('checked', false);
                        else if (allmodels != '' && (allmodels.length == models.length))
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
                    $("#SelectedModel").val(models);
                    if (models != '') {
                        $.ajax({
                            type: "GET",
                            dataType: "html",
                            url: '/Search/GenerateTrims?models=' + models,
                            data: {},
                            cache: false,
                            traditional: true,
                            success: function (result) {
                                $('#divTrim').html(result);
                            },
                            error: function (err) {
                                $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                                $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                                jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                            }
                        });
                    } else {
                        $('#divTrim').html('<select id="ddlTrim" name="ddlTrim" class="trim multi-select"></select>');
                        $("select#ddlTrim").multiselect({ header: false, noneSelectedText: "Trim", minWidth: 120 });
                    }
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    models = '';
                    $("#SelectedModel").val(models);
                },
                optgrouptoggle: function (event, ui) {

                }
            });
        });
    });
</script>