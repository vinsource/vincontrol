<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="904px" Width="1877px">
        <LocalReport ReportPath="ReportTemplates\MultipleInventoryReportTemplate.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                    Name="VinControlVehicleReport" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        SelectMethod="GetVinControlVehiclesInMultipleStore" 
        TypeName="WhitmanEnterpriseMVC.ReportModel.VinControlReport">
        <SelectParameters>
            <asp:SessionParameter Name="NewUsed" SessionField="NewUsed" Type="String" />
            <asp:SessionParameter Name="dealerGroupId" SessionField="dealerGroupId" 
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </form>
</body>
</html>
