<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<vincontrol.DomainObject.ExtendedSelectListItem>>" %>
<%@ Import Namespace="vincontrol.VinSell.Extensions" %>
<%: Html.DropDownList("ddlSeller", Model.ToSelectItemList(m => m.Value, m => m.Text, false), "All Sellers", new { @class = "seller multi-select", multiple = "multiple", style="width:120px;" })%>

<script type="text/javascript">
    var sellers = selectedSeller;
    var allsellers = '';
    $(document).ready(function () {
        $(function () {
            $("select#ddlSeller").multiselect({
                header: false,
                noneSelectedText: "Seller",
                minWidth: 120,
                click: function (event, ui) {
                    if (ui.value == '') {
                        if (ui.checked) {
                            $(this).multiselect("checkAll");
                            $.each($(this).val(), function (key, value) {
                                if (value != '')
                                    sellers += ',' + (value);
                            });
                            allsellers = sellers;
                        }
                        else {
                            $(this).multiselect("uncheckAll");
                            sellers = '';
                        }
                    } else {

                        if (ui.checked) {
                            sellers += ',' + (ui.value);
                        }
                        else {
                            sellers = sellers.replace(',' + (ui.value), '');
                        }

                        var emptyOption = $(this).multiselect("widget").find("input:checked[value='']");
                        if (allsellers != '' && (allsellers.length != sellers.length))
                            emptyOption.prop('checked', false);
                        else if (allsellers != '' && (allsellers.length == sellers.length))
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
                    $("#SelectedSeller").val(sellers);
                },
                checkAll: function () {

                },
                uncheckAll: function () {
                    sellers = '';
                    $("#SelectedSeller").val(sellers);
                },
                optgrouptoggle: function (event, ui) {

                }
            });
        });
    });
</script>