<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Advanced.aspx.cs" Inherits="WebApplication1.Pages.PageAdvanced" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <style>
        .Table {
            position: absolute;
            border-collapse: collapse;
        }


        .theader {
            background-color: #C61720;
            font-weight: bold;
            color: white;
            font-size: large;
            height: 20%;
        }

        .TableHeadTxt {
            font-size: 1.0vw;
            padding: 2% 2%;
        }


        .row {
            border-bottom: 0.05vh solid lightgrey;
        }

            .row:last-of-type {
                border-bottom: 0.5vh solid #C61720;
            }

        .LeftCells {
            font-weight: bold;
            font-size: 1.0vw;
            padding-left: 2%;
        }

        .Cells {
            padding-left: 2%;
            font-size: 1.0vw;
        }

        .TableWhole {
            background-color: rgba(255,255,255,0.6)
        }

        .CellsPic {
            vertical-align: top;
        }
    </style>
        
</head>
<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="Scriptmanager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatablePanel" UpdateMode="Conditional" runat="server">

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer" EventName="Tick" />
            </Triggers>

            <ContentTemplate>
                                
                <div id="TemplateClassID" class="TemplateClass" runat="server">

                    <div class="frame">
                        <div class="control">
                            <span class="e-select" aria-disabled="false"><span class="e-icon e-datetime" aria-label="select"></span></span>
                        </div>
                    </div>


                    <div class="Table" id="Table_div" style="width: 32%; height: 50%; top: 20%; left: 2%;">



                        <asp:Table ID="Table_Settings" runat="server" CellSpacing="0" Width="100%" Height="100%" CssClass="TableWhole">

                            <asp:TableHeaderRow CssClass="theader">
                                <asp:TableCell ID="TableCell0" Width="20%" runat="server" Text="" CssClass="TableHeadTxt"></asp:TableCell>
                                <asp:TableCell ID="TableCellDevName" Width="25%" runat="server" Text="Name" CssClass="TableHeadTxt"></asp:TableCell>
                                <asp:TableCell ID="TableCell0_1" Width="27%" runat="server" Text="" CssClass="TableHeadTxt"></asp:TableCell>
                                <asp:TableCell ID="TableCell10" HorizontalAlign="Center" runat="server" Text="Connection status" CssClass="TableHeadTxt"></asp:TableCell>

                            </asp:TableHeaderRow>

                            <asp:TableRow CssClass="row">
                                <asp:TableCell ID="cellLogo1" HorizontalAlign="Left" runat="server" Text="" CssClass="LeftCells"></asp:TableCell>
                                <asp:TableCell ID="TableCellDevName1" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="cellLogo_watchdogVal1" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="TableCell11" HorizontalAlign="Center" runat="server" Text="" CssClass="CellsPic">                                    
                                </asp:TableCell>

                            </asp:TableRow>
                            <asp:TableRow CssClass="row">
                                <asp:TableCell ID="cellLogo2" HorizontalAlign="Left" runat="server" Text="" CssClass="LeftCells"></asp:TableCell>
                                <asp:TableCell ID="TableCellDevName2" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="cellLogo_watchdogVal2" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="TableCell12" HorizontalAlign="Center" runat="server" Text="" CssClass="CellsPic"></asp:TableCell>

                            </asp:TableRow>
                            <asp:TableRow CssClass="row">
                                <asp:TableCell ID="cellLogo3" HorizontalAlign="Left" runat="server" Text="" CssClass="LeftCells"></asp:TableCell>
                                <asp:TableCell ID="TableCellDevName3" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="cellLogo_watchdogVal3" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="TableCell13" HorizontalAlign="Center" runat="server" Text="" CssClass="CellsPic"></asp:TableCell>

                            </asp:TableRow>
                            <asp:TableRow CssClass="row">
                                <asp:TableCell ID="cellLogo4" HorizontalAlign="Left" runat="server" Text="" CssClass="LeftCells"></asp:TableCell>
                                <asp:TableCell ID="TableCellDevName4" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="cellLogo_watchdogVal4" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="TableCell14" HorizontalAlign="Center" runat="server" Text="" CssClass="CellsPic"></asp:TableCell>

                            </asp:TableRow>
                            <asp:TableRow CssClass="row">
                                <asp:TableCell ID="cellLogo5" HorizontalAlign="Left" runat="server" Text="" CssClass="LeftCells"></asp:TableCell>
                                <asp:TableCell ID="TableCellDevName5" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="cellLogo_watchdogVal5" runat="server" Text="" CssClass="Cells"></asp:TableCell>
                                <asp:TableCell ID="TableCell15" HorizontalAlign="Center" runat="server" Text="" CssClass="CellsPic"></asp:TableCell>

                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </div>

            
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:Timer ID="Timer" runat="server">

        </asp:Timer>


    </form>
</body>
</html>

<%--var t = document.getElementById('TextBoxDebugID'); t.scrollTop = t.scrollHeight;--%>
