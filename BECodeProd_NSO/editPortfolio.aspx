<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editPortfolio.aspx.cs"
    Title="Add edit portfolio" Inherits="editPortfolio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
    <script src="Scripts/JQUERY.js" type="text/javascript"></script>
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
           
            background-color: black;
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
        .button
        {
            border-style: solid;
            background-color: #f8da92;
            padding: 1px 0px;
            border-color: red;
            border-width: 1px;
            cursor: pointer;
            cursor: hand;
            font-family: Calibri;
            font-size: 9pt;
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
       
        
        .txtFont
        {
            font-family: Calibri;
            font-size: 9pt;
        }
        .style1
        {
            width: 202px;
        }
        .style2
        {
            width: 121px;
        }
        .style3
        {
            width: 152px;
        }
        .style4
        {
            width: 118px;
        }
    </style>
    <script type="text/javascript">
        function Reset() {
            var textBoxes = document.getElementsByTagName("input");
            for (var i = 0; i < textBoxes.length; i++)
                if (textBoxes[i].type == "text")
                    textBoxes[i].value = '';
        }
    </script>
    <script type="text/javascript">
        function PoPUPSave() {
            return true;
        }
        function ValidPopUpSave() {

            var idMCC = 'txtMasterclientcode';
            //var idGCR = 'MainContent_txtPopupGCR';
            var idCC = 'txtClientCode';

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

    </script>
    <title></title>
    <link rel="stylesheet" href="boot.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="pnlEdit" runat="server" CssClass="modalPopup" BorderWidth="2px" BackColor="White">
            <asp:HiddenField ID="hdfnldID" runat="server" />
            <div id="Div1" class="web_dialog">
                <table id="Table1" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="1" class="web_dialog_title">
                            Edit - Client Code Portfolio
                        </td>
                    </tr>
                </table>
                <table id="Table2" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="2" style="text-align: center; height: 1px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Text="" CssClass="TextBox" ForeColor="#FF3300"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Master Customer Code :
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="txtMasterclientcode" runat="server" Width="180px" onKeyUp="NCtoUpper(event,this);"
                                CssClass="txtFont"></asp:TextBox>
                        </td>
                        <td align="right" class="style4">
                            Master Customer Name :
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtMcname" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right">
                            Client Code :
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientCode" runat="server" CssClass="txtFont" Width="180px" onKeyUp="NCtoUpper(event,this);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Client Name :
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="txtClientName" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right" class="style4">
                            Portfolio :
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtPortfolio" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right">
                            Division :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDivision" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Vertical :
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="txtVertical" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right" class="style4">
                            RH Mail Id :
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtRH" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right">
                            SDM Mail Id :
                        </td>
                        <td>
                            <asp:TextBox ID="txtSDM" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            DH Mail Id :
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="txtDH" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right" class="style4">
                            SOH Mail Id :
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtBITSCSI" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right">
                            UH Mail Id :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUH" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                    <td align="right">
                     FA Portfolio :
                    </td>
                                       <td class="style2">
                     <asp:TextBox ID="txtFaportfolio" runat="server" CssClass="txtFont" Width="180px"></asp:TextBox>
                    </td>
                        <td align="right" class="style4">
                            PU:
                        </td>
                        <td align="left" class="style1">
                            <asp:DropDownList ID="ddlPU" runat="server" CssClass="txtFont" DataTextField="txtPU"
                                DataValueField="txtPU">
                            </asp:DropDownList>
                        </td>


                               <td align="right">
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
                    <td align="right">
                        MCO Mail Id
                    </td>
                                       <td class="style2">
                     <asp:TextBox ID="TXTMCONAME" runat="server" CssClass="txtFont" Width="100"></asp:TextBox>
                    </td>
                        <td align="right" class="style4">
                            Service Line:
                        </td>
                        <td align="left" class="style1">
                           
                            <asp:Label ID="lblSU" runat="server"></asp:Label>
                        </td>


                               <td align="right">
                                   Unit
                        </td>
                        <td align="left">
                            <asp:Label ID="lblUnit" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: center">
                            <asp:Button ID="btnEditSave" runat="server" Text=" Save " class="btn btn-info btn-sm" Height="25" style="margin-left:10px;padding-top:2px!important;border:1px solid lightgray;"  OnClick="btnEditSave_Click"
                                OnClientClick="return ValidPopUpSave();"  />
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
