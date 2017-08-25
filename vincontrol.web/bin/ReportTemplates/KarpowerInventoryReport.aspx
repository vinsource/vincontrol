<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KarpowerInventoryReport.aspx.cs" Inherits="Vincontrol.Web.ReportTemplates.KarpowerInventoryReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Karpower Inventory Report</title>
</head>
<body>
    <form id="form2" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="876px" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="823px">
            <LocalReport ReportPath="ReportTemplates\KarPowerInventoryReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="VinCertified" 
                        Name="VincontrolReportSource" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="VinCertified" runat="server" 
            SelectMethod="GetKarPowerVehicles" 
            TypeName="Vincontrol.Web.ReportModel.VinControlReport">
            <SelectParameters>
                <asp:SessionParameter Name="dealerId" SessionField="dealerId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </div>
    </form>
  
</body>
</html>
