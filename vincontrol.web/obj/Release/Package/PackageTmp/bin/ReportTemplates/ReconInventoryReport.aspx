<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReconInventoryReport.aspx.cs" Inherits="Vincontrol.Web.ReportTemplates.ReconInventoryReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recon Inventory Report</title>
</head>
<body>
    
    <form id="form2" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Button ID="btnPrint" runat="server" Font-Size="Large" 
            onclick="btnPrint_Click" Text="Print" Width="110px" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="876px" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="823px">
            <LocalReport ReportPath="ReportTemplates\ReconInventoryReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="VinSource3" 
                        Name="VincontrolReportSource" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="VinSource3" runat="server" 
            SelectMethod="GetVinControlReconVehicles" 
            TypeName="Vincontrol.Web.ReportModel.VinControlReport">
            <SelectParameters>
                <asp:SessionParameter Name="dealerId" SessionField="dealerId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </div>
    </form>
  
</body>
</html>
