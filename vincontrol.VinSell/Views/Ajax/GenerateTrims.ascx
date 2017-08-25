<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlTrim", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All Trims", new { @class = "trim multi-select", multiple = "multiple" })%>

<script type="text/javascript">
    var trims = '';
    var alltrims = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlTrim").multiselect({
                header: false
                ,click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    trims += ',' + (value);
                            });
                            alltrims = trims;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            trims = '';
                        }
                    } else {

                        if (ui.checked) {
                            trims += ',' + (ui.value);
                        }
                        else {
                            trims = trims.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (alltrims != '' && (alltrims.length != trims.length))
                            emptyOption.prop('checked', false);
                        else if (alltrims != '' && (alltrims.length == trims.length))
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
                    $("#SelectedTrim").val(trims);
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    trims = '';
                    $("#SelectedTrim").val(trims);
                },
                optgrouptoggle: function (event, ui) {

                }
            });
        });
    });
</script>