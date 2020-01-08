<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Razsvetljava.aspx.cs" Inherits="WebApplication1.Pages.Razsvetljava" %>

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
                <asp:Panel ID="MasterPaneSettings" runat="server">

                    <asp:UpdatePanel ID="UpdatePanelSettings" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        </ContentTemplate>
                        
                        
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="LuciPanel" updatemode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>

                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick">
                </asp:Timer>
            </div>



        </div>
    </form>
</body>
</html>
