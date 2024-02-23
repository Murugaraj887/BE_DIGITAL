<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewPortfolio.aspx.cs"
    Inherits="AddNewPortfolio" Title="Add New ClientCode" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
    <style type="text/css">
        .web_dialog_overlay
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .70;
            filter: alpha(opacity=70);
            -moz-opacity: .70;
            z-index: 101;
        }
        
        .web_dialog_title
        {
            border-bottom: solid 2px #c41502;
            background-color: #c41502;
            padding: 4px;
            color: White;
            font-family: Calibri;
            font-weight: bold;
        }
        .web_dialog_title a
        {
            color: Black;
            text-decoration: none;
            font-family: Verdana;
            font-weight: bold;
        }
        .align_right
        {
            text-align: right;
        }
    </style>
    <style type="text/css">
        .modalBackground
        {
            background-color: #fff;
            border-bottom-style: none;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            border-bottom-width: thin;
            font-family: Calibri;
            font-size: 9pt;
        }
        .button
        {
            border: 1px solid red;
            background-color: #f8da92;
            padding: 1px 0px;
            cursor: pointer;
            cursor: hand;
            font-family: Calibri;
            font-size: 9pt;
            width: 44px;
        }
        .button:hover
        {
            border-style: solid;
            background-color: #c41502;
            border-color: Black;
            color: White;
            border-width: 1px;
            padding: 1px 0px;
            cursor: pointer;
            cursor: hand;
            font-family: Calibri;
            font-size: 9pt;
        }
        .txtFont
        {
            font-family: Calibri;
            font-size: 9pt;
        }
        .style6
        {
            width: 92px;
        }
        .style7
        {
            width: 119px;
        }
        .style10
        {
            width: 207px;
        }
        .style11
        {
            width: 200px;
        }
        .style12
        {
            width: 238px;
        }
    </style>
    <script type="text/javascript">
        function PoPUPSave() {
            return true;
        }
        function ValidPopUpSave() {


            var idMCC = 'txtpopupMCC';
            //var idGCR = 'MainContent_txtPopupGCR';
            var idCC = 'txtpopupClientCode';

            var controlMCC = document.getElementById(idMCC);
            //var controlGCR = document.getElementById(idGCR);
            var controlCC = document.getElementById(idCC);

            var valueMCC = controlMCC.value + '';
            //var valueGCR = controlGCR.value + '';
            var valueCC = controlCC.value + '';
            valueCC = valueCC.trim();
            valueMCC = valueMCC.trim();

            if (valueMCC == '') { alert('Pls enter the Master Customer Code'); controlMCC.focus(); return false; }
            // if (valueGCR == '') { alert('Pls enter the Guidance conv rate.'); controlGCR.focus(); return false; }
            if (valueCC == '') { alert('Pls enter the Client Code.'); controlCC.focus(); return false; }
            return true;
        }


        function NCtoUpper(evt, thisobj)
        { thisobj.value = (thisobj.value + '').toUpperCase(); }


        function Reset() {
            var textBoxes = document.getElementsByTagName("input");
            for (var i = 0; i < textBoxes.length; i++)
                if (textBoxes[i].type == "text")
                    textBoxes[i].value = '';
    }
    </script>
    <%--<script type="text/javascript">
        function Reset() {
            var textBoxes = document.getElementsByTagName("input");
            for (var i = 0; i < textBoxes.length; i++)
                if (textBoxes[i].type == "text")
                    textBoxes[i].value = '';
        }
    </script>--%>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmgr" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" BorderWidth="2px" BackColor="White">
            <div id="dialog" class="web_dialog">
                <table id="tblTitlePopUP" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="1" class="web_dialog_title">
                            Client Code Portfolio - Add new
                        </td>
                    </tr>
                </table>
                <table id="boxcontent" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" style="text-align: center; height: 1px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblpopupInfo" runat="server" Text="" CssClass="txtFont" ForeColor="#FF3300"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style10">
                            Master Customer Code :
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="txtpopupMCC" runat="server" CssClass="txtFont" Width="100" onKeyUp="NCtoUpper(event,this);"
                                MaxLength="50" CausesValidation="True"></asp:TextBox>
                        </td>
                        <td align="right" class="style7">
                            Master Customer Name :
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="txtpopupMCName" runat="server" CssClass="txtFont" Width="100" >
                           </asp:TextBox>
                        </td>
                        <td align="right" class="style6">
                            Client Code :
                        </td>
                        <td>
                            <asp:TextBox ID="txtpopupClientCode" runat="server" CssClass="txtFont" Width="100"
                                onKeyUp="NCtoUpper(event,this);" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style10">
                            Client Name :
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="txtpopupClientName" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                        </td>
                        <td align="right" class="style7">
                            Portfolio :
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="txtpopupPortfolio" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                        </td>
                        <td align="right" class="style6">
                            Division :
                        </td>
                        <td>
                            <asp:TextBox ID="txtpopupDivision" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                     <td align="right" class="style10">
                            Service Line:
                        </td>
                        <td align="left" class="style11">
                           
                        <asp:DropDownList ID="ddlSU" AutoPostBack=true
                                runat="server" onselectedindexchanged="ddlSU_SelectedIndexChanged">
                                <asp:ListItem>--Select--</asp:ListItem>
                                <asp:ListItem>ORC</asp:ListItem>
                                <asp:ListItem>SAP</asp:ListItem>

                            </asp:DropDownList>
                        </td>


                        
                        <td align="right" class="style7">
                            RH Mail Id :
                        </td>
                        <td class="style12">
                            <asp:TextBox ID="txtpopupRHMailid" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                        </td>
                        <td align="right" class="style6">
                            SDM Mail Id :
                        </td>
                        <td>
                            <asp:TextBox ID="txtpopupSDM" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style10">
                            DH Mail Id :
                        </td>
                        <td class="style11">
                          <asp:DropDownList ID="ddlDH" runat="server" CssClass="txtFont" DataTextField="txtDHMailId" Enabled="false"
                                DataValueField="txtDHMailId">
                            </asp:DropDownList>
                        </td>
                        <td align="right" class="style7">
                            SOH Mail Id&nbsp;
                        </td>
                        <td class="style12">
                           <asp:DropDownList ID="ddlSoh" runat="server" CssClass="txtFont" DataTextField="txtBITSCSIHMailId"  Enabled="false"
                                DataValueField="txtBITSCSIHMailId">
                            </asp:DropDownList>
                        </td>
                        <td align="right" class="style6">
                            UH Mail Id :
                        </td>
                        <td>
                           <asp:DropDownList ID="ddlUh" runat="server" CssClass="txtFont" DataTextField="txtUHMailId"  Enabled="false"
                                DataValueField="txtUHMailId">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style10">
                            FA Portfolio:
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="txtFaPortfolio" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                        </td>
                        <td align="right" class="style7">
                            PU :
                        </td>
                        <td class="style12">
                            <asp:DropDownList ID="ddlPopupPu" runat="server" CssClass="txtFont" DataTextField="txtPU"  Enabled="false"
                                DataValueField="txtPU">
                            </asp:DropDownList>
                        </td>
                        <td align="right" class="style6">
                             MCC isActive?
                        </td>
                          <td align="left">
                            <asp:DropDownList ID="ddlisActive" runat="server" CssClass="txtFont" DataTextField="isActive"
                                DataValueField="isActive">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                         <tr>
                    <td align="right" class="style10">
                        MCO Mail Id
                    </td>
                                       <td class="style11">
                     <asp:TextBox ID="TXTMCONAME" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                    </td>
                       <td align="right" class="style7">
                            Vertical :
                        </td>
                        <td class="style12">
                          <asp:DropDownList ID="ddlVertical" runat="server" CssClass="txtFont" DataTextField="txtVertical" Enabled="false"
                                DataValueField="txtVertical">
                            </asp:DropDownList>
                        </td>
                               <td align="right" class="style6">
                                   Unit:
                        </td>
                        <td align="left">
                            <asp:Label ID="lblUnit" runat="server" text="EAS"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                        </td>
                        <td class="style11">
                        </td>
                        <td colspan="2" style="text-align: center">
                            <asp:Button ID="btnSavepopup" runat="server" Text=" Save " OnClick="btnSavepopup_click"
                                OnClientClick="return ValidPopUpSave(); " CssClass="button" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text=" Reset " CssClass="button" 
                                onclick="btnCancel_Click" />
                        </td>
                        <td class="style6">
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
