<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AdminViewModel>" %>
<style type="text/css">
    .disable
    {
        display: none;
    }
</style>
 <script type="text/javascript">
     $(document).ready(function () {
         disclaimerManageEvents();

     });
     function disclaimerManageEvents() {



         $(".admin_rebate_manufactory").live("keydown", function(event) {
             // Allow: backspace, delete, tab, escape, and enter
             if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                 // Allow: Ctrl+A
                 (event.keyCode == 65 && event.ctrlKey === true) ||
                 // Allow: home, end, left, right
                 (event.keyCode >= 35 && event.keyCode <= 39)) {
                 // let it happen, don't do anything
                 return;
             } else {
                 // Ensure that it is a number and stop the keypress
                 if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                     event.preventDefault();
                 }
             }
         });
         
       
         $("div[id^=rdm_view]").click(function () {
             $(".rebate_view_disclaimer").hide();
             $(".rebate_edit_disclaimer").hide();
             $(this).parent().parent().find(".rebate_view_disclaimer").show();
             console.log($(this).parent().parent().find(".rebate_view_disclaimer").offset().top);
             console.log($('.admin_rebates_table').scrollTop());
             if ($(this).parent().parent().find(".rebate_view_disclaimer").offset().top > 700) {
                 $('.admin_rebates_table').scrollTop($('.admin_rebates_table').scrollTop() + 100);
             }
         });


         $("div[id^=rdm_clear]").live("click", function () {
             if (confirm("Do you want to clear this disclaimer?")) {
                 $.post('/Admin/SetDisclaimerContent', { rebateId: $(this).parent().attr("rebateId"), content: '' }, function (data) {
                 });
                 $(this).parent().parent().find(".rebate_view_disclaimer").find(".rebate_disclaimer_content").html("<p></p>");
             }
             $(".rebate_edit_disclaimer").hide();
         });

         $(".admin_rebate_expiration_list").live("change", function () {
             $.post('/Admin/UpdateRebateExpirationDate', { rebateId: $(this).parent().attr("rebateId"), date: $(this).val() }, function (data) {
                 
            });
         });

         $(".admin_rebate_create_list").live("change", function () {
             $.post('/Admin/UpdateRebateCreateDate', { rebateId: $(this).parent().attr("rebateId"), date: $(this).val() }, function (data) {

             });
         });

         $(".admin_rebate_manufactory").live("change", function () {
             $.post('/Admin/UpdateRebateAmount', { rebateId: $(this).parent().parent().attr("rebateId"), rebateAmount: $(this).val() }, function (data) {

             });
         });
         
         $("div[id^=rdm_edit]").live('click', function () {
             $(".rebate_view_disclaimer").hide();
            $(".rebate_edit_disclaimer").hide();
             var html = $(this).parent().parent().find(".rebate_view_disclaimer").find(".rebate_disclaimer_content").children("p").html();
             $(this).parent().parent().find(".rebate_edit_disclaimer").find("textarea").val(html);
             $(this).parent().parent().find(".rebate_edit_disclaimer").fadeIn();
             if ($(this).parent().parent().find(".rebate_edit_disclaimer").offset().top > 700) {
                 $('.admin_rebates_table').scrollTop($('.admin_rebates_table').scrollTop() + 200);
             }

             //$("div[id^=rdm_view]").unbind("mouseenter");
             //$("div[id^=rdm_view]").unbind("mouseleave");
         });

         $(".rde_btns_cancel").live("click", function () {
             $(".rebate_edit_disclaimer").fadeOut();
             $(".rebate_view_disclaimer").fadeOut();
             //$("div[id^=rdm_view]").mouseenter(function () {
             //    $(this).parent().parent().find(".rebate_view_disclaimer").show();
             //}).mouseleave(function () {
             //    $(this).parent().parent().find(".rebate_view_disclaimer").hide();
             //});
             $("div[id^=rdm_view]").click(function () {
                 $(".rebate_view_disclaimer").hide();
                 $(".rebate_edit_disclaimer").hide();
                 $(this).parent().parent().find(".rebate_view_disclaimer").show();
                 if ($(this).parent().parent().find(".rebate_view_disclaimer").offset().top > 700) {
                     $('.admin_rebates_table').scrollTop($('.admin_rebates_table').scrollTop() + 100);
                 }
             });
         });

         $(".rde_btns_save").live("click", function () {
             var value = $(this).parent().parent().find("textarea").val();
             $.post('/Admin/SetDisclaimerContent', { rebateId: $(this).parent().parent().parent().attr("rebateId"), content: value }, function (data) {
             });
             $(this).parent().parent().parent().find(".rebate_disclaimer_content").html("<p>" + value + "</p>");
             $(".rebate_edit_disclaimer").fadeOut();
         });
     }
     
   
 </script>

    <% foreach (var tmp in Model.RebateList)
       { %>
                            <div class="admin_rebates_items" rebateid="<%= tmp.UniqueId %>" style="margin-left: 6px;">
                                <input type="text" readonly="readonly" class="admin_rebate_select readonly" value="<%= tmp.Year %>">
                                <input type="text" readonly="readonly" class="admin_rebate_select readonly" value="<%= tmp.Make %>">
                                <input type="text" readonly="readonly" class="admin_rebate_select readonly" value="<%= tmp.Model %>">
                                <input type="text" readonly="readonly" class="admin_rebate_select readonly" value="<%= tmp.Trim %>">
                                <input type="text" readonly="readonly" class="admin_rebate_select readonly" value="<%= tmp.BodyType %>"
                                    style="width: 12%;">
                                <form name="SaveMBForm" class="MBForm">
                                    <input type="text" class="admin_rebate_manufactory" data-validation-engine="validate[funcCall[checkMB]]" value="<%= tmp.RebateAmount %>">
                                </form>
                                <input type="text" class="admin_rebate_create admin_rebate_create_list" value="<%= tmp.CreateDate.ToShortDateString() %>">
                                <input type="text" class="admin_rebate_expiration admin_rebate_expiration_list" value="<%= tmp.ExpirationDate.ToShortDateString() %>">
                                <div class="rebate_disclaimer_manage" rebateid="<%= tmp.UniqueId %>">
                                    <div id="rdm_view_<%= tmp.UniqueId %>" >
                                        View |</div>
                                    <div id="rdm_edit<%= tmp.UniqueId %>">
                                        Edit |</div>
                                    <div id="rdm_clear<%= tmp.UniqueId %>">
                                        Clear</div>
                                </div>
                                <div class="rebate_view_disclaimer">
                                    <div class="rebate_disclaimer_title">
                                        Disclaimer</div>
                                    <div class="rebate_disclaimer_content">
                                        <p>
                                            <%= tmp.Disclaimer %></p>
                                    </div>
                                    <div class="rebate_disclaimer_edit_btns">
                                        <div class="btns_shadow rde_btns_cancel">
                                            Cancel</div>
                                    </div>
                                </div>
                                <div class="rebate_edit_disclaimer">
                                    <div class="rebate_disclaimer_title">
                                        Disclaimer</div>
                                    <div class="rebate_disclaimer_edit_content">
                                        <textarea>this is test disclaimer.this is test disclaimer.this is test disclaimer.this is test disclaimer.this is test disclaimer.</textarea>
                                    </div>
                                    <div class="rebate_disclaimer_edit_btns">
                                        <div class="btns_shadow rde_btns_save">
                                            Save</div>
                                        <div class="btns_shadow rde_btns_cancel">
                                            Cancel</div>
                                    </div>
                                </div>
                                <div class="btns_shadow admin_rebates_delete_btn" id="btnDeleteManuRebate" rebateid="<%= tmp.UniqueId %>"
                                    onclick="javascript:deleteRebate(this);">
                                    Delete
                                </div>
                            </div>
                            <% } %>
