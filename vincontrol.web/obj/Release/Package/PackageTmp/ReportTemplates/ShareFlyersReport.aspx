<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShareFlyersReport.aspx.cs"
    Inherits="Vincontrol.Web.ReportTemplates.ShareFlyersReport" %>

<%@ Import Namespace="System.Security.Policy" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Share Flyers Report</title>
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
        <asp:Button ID="btnPrint" runat="server" Font-Size="Large" OnClick="btnPrint_Click"
            Text="Print" Width="110px" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            Height="876px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="823px" BorderStyle="Solid">
            <LocalReport ReportPath="ReportTemplates\ShareFlyersReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SharedFlyersHistory" Name="SharedFlyersHistory" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="SharedFlyersHistory" runat="server" SelectMethod="GetSharedFlyersHistory"
            TypeName="Vincontrol.Web.ReportModel.VinControlReport">
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
