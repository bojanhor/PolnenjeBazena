<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="WebApplication1.Pages.PageEditor" validateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" spellcheck="false">

        <asp:ScriptManager ID="Scriptmanager" runat="server"></asp:ScriptManager>

        <div id="TemplateClassID" class="TemplateClass" runat="server">

                      

        <div style= "position:absolute;top:10%;left:1%;height:85%;width:97%;">
            <textarea id="Editor" runat="server" style="position:absolute;top:0%;left:0%;height:100%;width:100%"></textarea>
        </div>
            </div>
    </form>
</body>
</html>
