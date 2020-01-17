<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Pages.PageDefault" %>

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
            
            <div>
                <asp:UpdatePanel ID="TemperaturePanel" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
                    <ContentTemplate>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Timer ID="Timer2" runat="server" Interval="1000" OnTick="Timer1_Tick">
                </asp:Timer>
            </div>

        </div>



    </form>
</body>
</html>
