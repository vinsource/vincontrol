﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewMultipleInventoryReport.aspx.cs" Inherits="Vincontrol.Web.ReportTemplates.MultpleNewInventoryReport" %><%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Multiple Inventory Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="876px" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="823px">
            <LocalReport ReportPath="ReportTemplates\NewMultipleInventoryReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="VinObject1" 
                        Name="VincontrolReportSource" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="VinObject1" runat="server" 
            SelectMethod="GetVinControlNewVehiclesMultipleStore" 
            TypeName="Vincontrol.Web.ReportModel.VinControlReport">
            <SelectParameters>
                <asp:SessionParameter Name="dealerGroup" SessionField="dealerGroupId" 
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
