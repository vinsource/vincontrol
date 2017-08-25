<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PriceRangeReport.aspx.cs"
    Inherits="WhitmanEnterpriseMVC.ReportTemplates.PriceRangeReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
       .refineLst {
           width: 232px;
           margin-bottom: 10px;
       }
        .refineLst .pane
        {
            background-color: #d1d1d1;
            padding: 3px 5px;
            border-radius: 4px;
            height: auto;
            color: #999;
            font-size: 14px;
            display: block;
        }
        
        .refineLst ul li
        {
            padding: 2px;
            position: relative;
            list-style: none;
        }
        
        .refineLst ul.priceRange li
        {
            padding-bottom: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="refineLst">
            <div id="fltr-prange" style="">
                <h4 id="hdr-prange" class="pane-hdr">
                    Price Range</h4>
                <div id="pane-priceRange" class="pane priceRange" title="available_vPriceRange">
                    <ul class="priceRange">
                        <li >
                            <asp:LinkButton ID="btn0" runat="server" 
                                ToolTip="Vehicles Priced Under $10,001" onclick="btn0_Click">Under $10,000</asp:LinkButton>
                           (<asp:Label ID="lbl0" runat="server" Text="0"></asp:Label>)
                        </li>
                        <li><asp:LinkButton ID="btn1000" runat="server" 
                                ToolTip="Vehicles Priced Between $10,000 &amp; $20,000" onclick="btn1000_Click">$10,000 - $20,000</asp:LinkButton>
                           (<asp:Label ID="lbl1000" runat="server" Text="0"></asp:Label>)
                        
                        </li>
                        <li ><asp:LinkButton ID="btn2000" runat="server" 
                                ToolTip="Vehicles Priced Between $20,000 &amp; $30,000" onclick="btn2000_Click">$20,000 - $30,000</asp:LinkButton>
                           (<asp:Label ID="lbl2000" runat="server" Text="0"></asp:Label>)
                        </li>
                        <li ><asp:LinkButton ID="btn3000" runat="server" 
                                ToolTip="Vehicles Priced Between $30,000 &amp; $40,000" onclick="btn3000_Click">$30,000 - $40,000</asp:LinkButton>
                           (<asp:Label ID="lbl3000" runat="server" Text="0"></asp:Label>)
                        </li>
                        <li ><asp:LinkButton ID="btn4000" runat="server" 
                                ToolTip="Vehicles Priced Between $40,000 &amp; $50,000" onclick="btn4000_Click">$40,000 - $50,000</asp:LinkButton>
                           (<asp:Label ID="lbl4000" runat="server" Text="0"></asp:Label>)
                        </li>
                        <li ><asp:LinkButton ID="btn5000" runat="server" 
                                ToolTip="Vehicles Priced Between $50,000 &amp; $70,000" onclick="btn5000_Click">$50,000 - $70,000</asp:LinkButton>
                           (<asp:Label ID="lbl5000" runat="server" Text="0"></asp:Label>)
                        </li>
                        <li><asp:LinkButton ID="btn7000" runat="server" 
                                ToolTip="Vehicles Priced Above $70,000" onclick="btn7000_Click">Above $70,000</asp:LinkButton>
                           (<asp:Label ID="lbl7000" runat="server" Text="0"></asp:Label>)
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <asp:Button ID="btnPrint" runat="server" Font-Size="Large" OnClick="btnPrint_Click"
            Text="Print" Width="110px" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
            Height="876px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="823px">
            <LocalReport ReportPath="ReportTemplates\PriceRangeReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="VinObject1" Name="VincontrolReportSource" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="VinObject1" runat="server" SelectMethod="GetVinControlUsedVehicles"
            TypeName="WhitmanEnterpriseMVC.ReportModel.VinControlReport">
            <SelectParameters>
                <asp:SessionParameter Name="dealerId" SessionField="dealerId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
