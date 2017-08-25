<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.AdminManagement.AdminViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VinSell | Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="inner-wrap">
        <div class="page-info">
            <span><br /></span><span><br /></span>
            <h3>Admin</h3>
        </div>
        <div class="filter-box">
            <div id="container_right_btns">
			<div class="btns_shadow admin_save_change" id="btnSave">
				Save Changes
			</div>
		</div>
        </div>
        <form id="form">
        <input type="hidden" id="DealershipId" name="DealershipId" value="<%= Model.DealershipId %>" />
        <input type="hidden" id="hdnOldCarFax" />
        <input type="hidden" id="hdnOldManheim" />
        <input type="hidden" id="hdnOldKBB" />
        <input type="hidden" id="hdnOldBB" />
        <div id="data">
            <div id="list-render-0" class="content">
                <div class="admin_3rdparty_header">
                    Third Party Site Login Credentials</div>
                <div class="admin_3rdparty_title">
                    This area gives you the ability to access 3rd party web resources from VIN Control.
                    It also provides additional information for your appraisals and profile pages. All
                    information stored here is secure and will not be shared.
                </div>
                <div class="admin_3rdparty_items_holder">
                    <div class="admin_3rdparty_items">
                        <div class="admin_3rdparty_items_text">
                            CarFax
                        </div>
                        <div class="admin_3rdparty_items_name">
                            <input type="text" id="CarFax" name="CarFax" value="<%= Model.CarFax %>" />
                        </div>
                        <div class="admin_3rdparty_items_pass">
                            <input type="password" id="CarFaxPassword" name="CarFaxPassword" value="<%= Model.CarFaxPassword %>" />
                            <%=Html.HiddenFor(x => x.CarFaxPasswordChanged)%>
                        </div>
                    </div>
                    <div class="admin_3rdparty_items">
                        <div class="admin_3rdparty_items_text">
                            Manheim
                        </div>
                        <div class="admin_3rdparty_items_name">
                            <input type="text" id="Manheim" name="Manheim" value="<%= Model.Manheim %>" />
                        </div>
                        <div class="admin_3rdparty_items_pass">
                            <input type="password" id="ManheimPassword" name="ManheimPassword" value="<%= Model.ManheimPassword %>" />
                            <%=Html.HiddenFor(x => x.ManheimPasswordChanged)%>
                        </div>
                    </div>
                    <div class="admin_3rdparty_items">
                        <div class="admin_3rdparty_items_text">
                            Kelly Blue Book
                        </div>
                        <div class="admin_3rdparty_items_name">
                            <input type="text" id="KellyBlueBook" name="KellyBlueBook" value="<%= Model.KellyBlueBook %>" />
                        </div>
                        <div class="admin_3rdparty_items_pass">
                            <input type="password" id="KellyPassword" name="KellyPassword" value="<%= Model.KellyPassword %>" />
                            <%=Html.HiddenFor(x => x.KellyPasswordChanged)%>
                        </div>
                    </div>
                    <div class="admin_3rdparty_items">
                        <div class="admin_3rdparty_items_text">
                            Black Book Online
                        </div>
                        <div class="admin_3rdparty_items_name">
                            <input type="text" id="BlackBook" name="BlackBook" value="<%= Model.BlackBook %>" />
                        </div>
                        <div class="admin_3rdparty_items_pass">
                            <input type="password" id="BlackBookPassword" name="BlackBookPassword" value="<%= Model.BlackBookPassword %>" />
                            <%=Html.HiddenFor(x => x.BlackBookPasswordChanged)%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </form>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
<link href="<%=Url.Content("~/Content/admin.css")%>" rel="stylesheet" type="text/css" />
<style type="text/css">
    .admin_save_change {
        cursor: pointer;        
        bottom: 2px;        
        background-color: #5C5C5C;
        color: #FFF;
        text-align: center;
        width: 175px;
        font-size: 19px;
        padding: 2px 5px 3px;
        font-weight: bold;
        margin-top: 33px; 
        margin-left: 0;       
        position: relative;
        float: left;
        }
</style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script type="text/javascript">
    var waitingImage = '<%= Url.Content("~/Content/Images/ajax-loader1.gif") %>';
    $(document).ready(function () {
        $('#CarFaxPassword').focus(function () {
            $("#hdnOldCarFax").val($(this).val());
            $(this).val("");
        }).blur(function (defaultValue) {
            if ($(this).val() == "") {
                $(this).val($("#hdnOldCarFax").val());
            }
            else {
                $("#CarFaxPasswordChanged").val('True');
            }
        });

        $('#ManheimPassword').focus(function () {
            $("#hdnOldManheim").val($(this).val());
            $(this).val("");
        }).blur(function (defaultValue) {
            if ($(this).val() == "") {
                $(this).val($("#hdnOldManheim").val());
            }
            else {
                $("#ManheimPasswordChanged").val('True');
            }
        });

        $('#KellyPassword').focus(function () {
            $("#hdnOldKBB").val($(this).val());
            $(this).val("");
        }).blur(function (defaultValue) {
            if ($(this).val() == "") {
                $(this).val($("#hdnOldKBB").val());
            }
            else {
                $("#KellyPasswordChanged").val('True');
            }
        });

        $('#BlackBookPassword').focus(function () {
            $("#hdnOldBB").val($(this).val());
            $(this).val("");
        }).blur(function (defaultValue) {
            if ($(this).val() == "") {
                $(this).val($("#hdnOldBB").val());
            }
            else {
                $("#BlackBookPasswordChanged").val('True');
            }
        });

        $("#btnSave").click(function () {
            blockUI(waitingImage);
            $.ajax({
                type: "POST",
                url: "/Admin/SaveSetting",
                data: $("form").serialize(),
                success: function (results) {
                    if (results == 'Error') {
                        jAlert('System Error: ', 'Warning!');
                        return false;
                    }

                    unblockUI();
                },
                error: function (err) {
                    jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    unblockUI();
                }
            });
        });
    });
</script>
</asp:Content>
