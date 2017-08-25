<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PriceTrackingReport.aspx.cs"
    Inherits="Vincontrol.Web.ReportTemplates.PriceTrackingReport" %>

<%@ Import Namespace="System.Security.Policy" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Price Tracking Report</title>
    <style>
        #filter span
        {
            font-weight: bold;
            width: 100px;
            display: inline-block;
            background-color: sienna;
            padding: 2px;
            margin: 2px;
        }
        
        #filter select, #filter input
        {
            width: 200px;
        }
        
        #filter input
        {
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="filter">
            <span>Stock#</span>
            <asp:TextBox ID="txtStock" runat="server"></asp:TextBox>
            &nbsp;<br />
            <div>
                <span>VIN</span>
                <asp:TextBox ID="VINNumberTextBox" runat="server"></asp:TextBox></div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <span>Year</span>
                        <asp:DropDownList ID="YearDropDownList" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <span>Make</span>
                        <asp:DropDownList ID="MakeDropDownList" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <span>Model</span>
                        <asp:DropDownList ID="ModelDropDownList" runat="server">
                        </asp:DropDownList>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <span>Month</span>
                <asp:DropDownList ID="MonthDropDownList" runat="server">
                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <asp:Button ID="btnRest" runat="server" Font-Size="Large" 
            onclick="btnRest_Click" Text="Reset" Width="60px" />
        <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click" Text="Filter"
            Width="52px" Font-Size="Large" />
        <asp:Button ID="btnPrint" runat="server" Font-Size="Large" OnClick="btnPrint_Click"
            Text="Print" Width="110px" />
        &nbsp;<rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" Height="876px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="823px" BorderStyle="Solid">
            <LocalReport ReportPath="ReportTemplates\PriceTrackingReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="HistoryChanged" Name="HistoryChanged" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="HistoryChanged" runat="server" SelectMethod="GetHistoryChanged"
            TypeName="Vincontrol.Web.ReportModel.VinControlReport">
            <SelectParameters>
                <asp:SessionParameter Name="dealerId" SessionField="dealerId" Type="Int32" />
                <asp:SessionParameter Name="month" SessionField="month" Type="Int32" />
                <asp:SessionParameter Name="year" SessionField="year" Type="Int32" />
                <asp:SessionParameter Name="make" SessionField="make" Type="String" />
                <asp:SessionParameter Name="model" SessionField="model" Type="String" />
                <asp:SessionParameter Name="stock" SessionField="stock" Type="String" />
                <asp:SessionParameter Name="VIN" SessionField="VIN" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
