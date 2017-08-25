<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/TradeIn.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.TradeInManagement.TradeInVehicleModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trade In | Step 2
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeaderHolder" runat="server">
    <div class="tradeInHeader_steps">
    </div>
    <div class="tradeInHeader_step1Btn">
    </div>
    <div class="tradeInHeader_step2Btn">
    </div>
    <div class="tradeInHeader_step3Btn">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("TradeInCustomer", "Trade", FormMethod.Post, new { id = "TradeInOptionsForm", onsubmit = "return OptionsSelected()" }); %>
    <%=Html.HiddenFor(x=>x.SelectedOptions) %>
    
    <input id="DealerName" name="DealerName" type="hidden" value="<%= Model.DealerName %>" />
    
     <h1 style="color: darkblue"> Please Choose Your Vehicle's Addtional Options</h1>
    <div class="tstep2_items_list">
        <% foreach (var tmp in Model.OptionalEquipment){%>
        <div class="tstep2_items_holder">
            <div class="tstep2_items_text">
                <label>
                    <input id="<%=tmp.VehicleOptionId%>" type="checkbox" name="Options" value="<%=tmp.DisplayName%>"/>
                    <%=tmp.DisplayName%>
                </label>
            </div>
        </div>
        <% } %>        
        <div style="clear: both">
        </div>
    </div>
    <%Html.EndForm(); %>
    <a id="step3" class="tradeIn_links" href="javascript:;" onclick="javascript:TradeInOptionsFormSubmit();">
        <div class="tradeIn_step_goto">Go to Final Step</div>
    </a>
    <script type="text/javascript">
        $(".tradeInHeader_step1Btn").addClass("tradeInHeader_step1Btn_active");
        $(".tradeInHeader_step2Btn").addClass("tradeInHeader_step2Btn_active");

        $(".tstep2_items_text").find("input").change(function () {
            if ($(this).attr("checked")) {
                $(this).parent().parent().parent().css("background-color", "#6685C2");
            } else {
                $(this).parent().parent().parent().css("background-color", "#EEEEEE");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        
    });

    function TradeInOptionsFormSubmit() {
        $("#TradeInOptionsForm").submit();
    }

    function OptionsSelected() {

        var checks = $('input[type="checkbox"]');
        var itemoption = "";
        

        for (var i = 0; i < checks.length; i++) {
            if (checks[i].checked && checks[i].name == "Options") {
                itemoption += checks[i].value + ",";
              
            }

        }

        if (itemoption.indexOf(",") != -1) {
            itemoption = itemoption.substring(0, itemoption.length - 1);
        }

        $("#SelectedOptions").val(itemoption);
       
    }
</script>
</asp:Content>
