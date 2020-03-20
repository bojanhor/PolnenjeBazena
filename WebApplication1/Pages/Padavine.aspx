<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Padavine.aspx.cs" Inherits="WebApplication1.Padavine" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
     
    </style>
   


</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scriptmanager" runat="server"></asp:ScriptManager>
        <div id="TemplateClassID" class="TemplateClass" runat="server">

            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                     <asp:Chart ID="Chart1" runat="server" Width="2000px" Height="700px">
                <Series>
                    
                </Series>

                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">                       
                    </asp:ChartArea>
                    
                </ChartAreas>

            </asp:Chart>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID ="TimerUpdtPnl1" /> 
                </Triggers>
            </asp:UpdatePanel>
            <asp:Timer ID="TimerUpdtPnl1" Interval="5000" runat="server"></asp:Timer>
           
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                     <asp:Chart ID="Chart2" runat="server" Width="2000px" Height="700px">
                <Series>
                    
                </Series>

                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">                       
                    </asp:ChartArea>
                </ChartAreas>

            </asp:Chart>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID ="TimerUpdtPnl2" /> 
                </Triggers>
            </asp:UpdatePanel>
            <asp:Timer ID="TimerUpdtPnl2" Interval="4900" runat="server"></asp:Timer>
            
        </div>



    </form>
</body>
</html>
