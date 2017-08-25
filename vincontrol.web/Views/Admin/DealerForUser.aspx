<%@ Page Title="Choose dealer for user" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.AccountManagement.DealerGroupViewModel>" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Default Dealership Login</title>
    
    <link href="<%=Url.Content("~/Content/VinControl/popup.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/VinControl/choosedealer.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/jquery.alerts.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.alerts.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
</head>
<body>
  
    <div class="adduser_popup_holder">
        <div class="kpi_print_title" style="margin-bottom: 0px;">
            Choose Dealerships
        </div>
        <div class="adduser_popup_events">
            <label class="adduser_popup_selectall">
                <input type="checkbox" class="adduser_selectall" id="SelectAllCheckbox" />
                Select All
            </label>
            <label class="adduser_popup_default">
                Default</label>
        </div>
        <div class="adduser_popup_list_holder">
            <%
                int i = 0;
                var dealerListId = (List<int>)ViewData["DealerList"];
                var defaultLoginId = (int) ViewData["UserDefaultLogin"];
                foreach (var tmp in Model.DealerList)
                {
                    i++;
            %>
            <div class="adduser_list_items <%=(i%2==0)?"adduser_list_items_odd": "adduser_list_items_even"%> ">
                <label class="adduser_list_select">
                    <input type="checkbox" name="DealerSelect"  value="<%=tmp.DealershipId %>" <%= ((dealerListId != null) && dealerListId.Contains(tmp.DealershipId)) ? "checked=\"checked\"" : String.Empty  %>/>
                    <%= tmp.DealershipName %>
                </label>
                <label class="adduser_popup_default">
                    <input type="radio" name="DefaultLogin" value="<%= tmp.DealershipId %>" <%=tmp.DealershipId == defaultLoginId? "checked=\"checked\"":String.Empty%> />
              </label>
            </div>
            <% } %>
            
            <%
                if (Model.DealershipGroupName != null)
                { %>
            
               <div class="adduser_list_items <%= (i%2 == 0) ? "adduser_list_items_odd" : "adduser_list_items_even" %> ">
                <label class="adduser_list_select">
                   <%-- <input type="checkbox" />--%>
                    <%= Model.DealershipGroupName%>
                </label>
                <label class="adduser_popup_default">
                    <% if (vincontrol.Constant.Constanst.DealerGroupConst == defaultLoginId)
                       { %>
                    <input type="radio" name="DefaultLogin" value="<%= vincontrol.Constant.Constanst.DealerGroupConst %>" checked="checked" />
                    <% }
                       else
                       { %>
                    <input type="radio" name="DefaultLogin" value="<%= vincontrol.Constant.Constanst.DealerGroupConst %>" />
                    <% } %>
                    

                </label>
            </div>
            <% } %>
            <div style="clear: both">
            </div>
        </div>
        <div class="btns_shadow adduser_popup_btns" id="btnAddUserInGroup">
          <%=((string)ViewData["Mode"]).Equals("New")?"Add":"Edit" %> User</div>
          <input  id="viewMode" type="hidden" value="<%=ViewData["Mode"] %>"/>
    </div>
</body>
<script type="text/javascript">
    var paraUserId = <%=ViewData["UserId"] %>;
    var paraRoleId = <%=ViewData["RoleId"] %>;
    $(document).ready(function () {
        var count = 0;
        
        $('input[name=DealerSelect]').each(function() {
            if ($(this).is(':checked')) {
                count++;
            }
        });
        if ($('input[name=DealerSelect]').length == count) {
            $('#SelectAllCheckbox').prop('checked', true);
        } else {
            $('#SelectAllCheckbox').prop('checked', false);
        }
        
        $('input[name=DealerSelect]').click(function() {
            count = 0;
            $('input[name=DealerSelect]').each(function() {
                if ($(this).is(':checked')) {
                    count++;
                } else {
                    count--;
                }
            });
            if ($('input[name=DealerSelect]').length == count) {
                $('#SelectAllCheckbox').prop('checked', true);
            } else {
                $('#SelectAllCheckbox').prop('checked', false);
            }
        });

        $("#SelectAllCheckbox").click(function (event) {
            var checks = $('input[name="DealerSelect"]');

            if ($(this).is(':checked')) {

                for (i in checks) {
                    checks[i].checked = true;

                }

            } else {

                for (i in checks) {

                    checks[i].checked = false;

                }
            }
        });

        $("#btnAddUserInGroup").click(function (event) {

            if ($("#CusErrorIn").length > 0) {
                $("#CusErrorIn").remove();
            }
            var checks = $('input[name="DealerSelect"]');
            var radios = $('input[name="DefaultLogin"]');
            var flag = false;
            var selectDealer = "";

            var defaultLogin = "";
            for (i in radios) {
                if (radios[i].checked && radios[i].value != undefined) {
                    defaultLogin = radios[i].value;
                }
            }

            for (i in checks) {
                if (checks[i].checked && checks[i].value != undefined) {
                    flag = true;
                    selectDealer += checks[i].value + ",";
                }
            }


            if (flag == false) {
                jAlert("Please choose at least one dealership", "Warning");
            } else {

                if ((selectDealer.indexOf(defaultLogin) == -1) && (defaultLogin.toString() != '<%=vincontrol.Constant.Constanst.DealerGroupConst%>')) {
                    jAlert("Default dealership is not in the list of dealeships you choose for this user", "Warning");
                } else {

                    if ($("#viewMode").val() == 'Edit') {
                        var editData = {
                            userId: paraUserId,
                            RoleId: paraRoleId,
                            DealerList: selectDealer,
                            DefaultLogin: defaultLogin
                        };

                        $.post('<%= Url.Content("~/Admin/EditUserRight") %>', editData,
                            function(user) {
                                console.log(user);
                                $('#elementID').addClass('hideLoader');
                                var actionUrl;
                                if (user.SessionTimeOut == "TimeOut") {
                                    jAlert("Your session is timed out. Please login back again", "Warning");
                                    actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                                    window.parent.location = actionUrl;
                                } else {
                                    parent.$.fancybox.close();
                                    parent.getUpdatedUserList();

                                }
                            });
                    } else {
                        var info = {
                            Name: window.parent.$("#NewName").val(),
                            UserName: window.parent.$("#NewUsername").val(),
                            Password: window.parent.$("#NewPassword").val(),
                            Email: window.parent.$("#NewEmail").val(),
                            CellPhone: window.parent.$("#NewPhone").val(),
                            RoleId: window.parent.$("#UserLevel").val(),
                            DealerList: selectDealer,
                            DefaultLogin: defaultLogin
                        };

                        $.post('<%= Url.Content("~/Admin/AddUser") %>', info,function(user) {
                            
                                $('#elementID').addClass('hideLoader');
                                var actionUrl;
                                if (user.SessionTimeOut == "TimeOut") {
                                    ShowTimeOutMessage();
                                    actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                                    window.parent.location = actionUrl;
                                
                                } else {
                                    parent.$("#NewName").val("Name");
                                    parent.$("#NewUsername").val("UserName");
                                    parent.$("#NewPassword").val("Password");
                                    parent.$("#NewEmail").val("Email");
                                    parent.$("#NewPhone").val("Cell#");
                                    parent.getUpdatedUserList();
                                    parent.$.fancybox.close();
                                   
                                }
                            });
                    }


                }
            }
        });
    });


</script>
</html>
