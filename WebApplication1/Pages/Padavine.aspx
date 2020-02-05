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

                        
            <asp:Chart ID="Chart1" runat="server" Width="2000px" Height="700px">
                <Series>
                    
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>

        </div>



    </form>
</body>
</html>
