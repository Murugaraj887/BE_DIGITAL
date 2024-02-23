<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="GuidanceAndActuals.aspx.cs" Inherits="GuidanceAndActuals" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
    <style type="text/css">
        .DisplayNone
        {
            display: none;
        }
        
        
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
        .web_dialog
        {
            position: fixed;
            width: 300px;
           
            top: 50%;
            left: 50%;
            margin-left: -190px;
            margin-top: -100px;
            background-color: white;
            border: 2px solid #c41502;
            padding: 0px;
            z-index: 102;
            font-family: Calibri;
            font-size: 9pt;
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
        .style2
        {
            width: 18%;
        }
    </style>
    <script type="text/javascript">
        function PoPUPSave() {
            return true;
        }
        function ValidPopUpSave() {

            var idNV = 'MainContent_ddlpopUpMonth';
            //var idGCR = 'MainContent_txtPopupGCR';
            var idCCR = 'MainContent_ddlPopUpYear';

            var controlNV = document.getElementById(idNV);
            //var controlGCR = document.getElementById(idGCR);
            var controlCCR = document.getElementById(idCCR);

            var valueNV = controlNV.value + '';
            //var valueGCR = controlGCR.value + '';
            var valueCCR = controlCCR.value + '';
            if (valueNV == '') { alert('Pls enter the Native Currency.'); controlNV.focus(); return false; }
            // if (valueGCR == '') { alert('Pls enter the Guidance conv rate.'); controlGCR.focus(); return false; }
            if (valueCCR == '') { alert('Pls enter the Current conv rate.'); controlCCR.focus(); return false; }
            return true;
        }
        
    </script>
    <script language="javascript" type="text/javascript">

        function PressTextOnly(evt, thisobj) {


            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 37 || charCode == 39) return true;  // allow arrows
            if (charCode == 46) return true; //delete  
            if (charCode == 35 || charCode == 36) return true; // home, end 
            if (charCode == 8 || charCode == 9) return true; // backspace , tab



            if (charCode > 64 && charCode < 91)
                return true;
            else
                return false;

            if (evt.shiftKey == true)
                if (charCode > 64 && charCode < 91)
                    return true;
                else
                    return false;

            return false;

        }

        function NCtoUpper(evt, thisobj)
        { thisobj.value = (thisobj.value + '').toUpperCase(); }

        function PressfloatOnly(evt, thisobj) {



            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (evt.shiftKey == true)
                if (charCode > 47 && charCode < 61)
                    return false;

            var textboxValue = thisobj.value + "";

            if (charCode == 17 || charCode == 67)
                return true;
            if (charCode == 17 || charCode == 86)
                return true;
            if (charCode == 17 || charCode == 88)
                return true;

            if (charCode == 190 || charCode == 110) {
                var contains = textboxValue.indexOf(".") != -1;
                if (contains)
                    return false;
            }

            if (charCode == 37 || charCode == 39) return true;  // allow arrows

            if (charCode == 46) return true; //delete

            if (charCode == 190 || charCode == 110) return true; // period or dot


            if (charCode == 35 || charCode == 36) return true; // home, end 


            if (charCode == 8 || charCode == 9) return true; // backspace , tab


            //            var temp = parseFloat(textboxValue);
            //            temp = temp.toFixed(2);
            //            if (temp > 99999999999.99) {
            //                
            //            return false; }




            if (charCode > 47 && charCode < 58) return true; //0-9

            if (charCode > 95 && charCode < 106) return true; //0-9



            return false;
        }

        function PressNumberOnly(evt, thisobj) {
            ClearSaveMessage();
            var charCode = (evt.which) ? evt.which : event.keyCode


            // avoid shift key operations 

            if (evt.shiftKey == true)
                if (charCode > 47 && charCode < 61)
                    return false;

            ////////////



            var textboxValue = thisobj.value + "";

            if (charCode == 17 || charCode == 67)
                return true;
            if (charCode == 17 || charCode == 86)
                return true;
            if (charCode == 17 || charCode == 88)
                return true;

            if (charCode == 190 || charCode == 110) {
                var contains = textboxValue.indexOf(".") != -1;
                if (contains)
                    return false;
            }

            if (charCode == 37 || charCode == 39)  // allow arrows
                return true;


            if (charCode == 46)
                return true;
            if (charCode == 190 || charCode == 110)
                return true;

            if (charCode > 47 && charCode < 58)
                return true;

            if (charCode > 95 && charCode < 106)
                return true;

            if (charCode == 8 || charCode == 9) return true;

            return false;
        }

        function isNum(elem, msg) {
            var numericExpression = /^[0-9]+$/;
            if (elem.value.match(numericExpression)) {
                return true;
            }
            else {
                alert(msg);
                elem.focus();
                return false;
            }

            function CheckFloat() {

                var value = document.getElementById("MainContent_grdCurrConv_txtGuidanceConvRate").value;
                if (isNaN(parseFloat(value))) {
                    alert("Enter Numeric values");
                    return false;
                }

                else return true;

            }
        }
        function openWindowLink(link) {
            window.navigate(link);
        }
        function openNewWindow(link) {
            window.open(link);
        }
        function PopUpFreeze() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            window.open('AppFreeze.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=450, height=190 , menubar=no, scrollbars=no, resizable=no');



            //return false;
        }

        function ClearSaveMessage() {

            var control = document.getElementById('MainContent_lblMsg');
            if (control != null)
                control.outerText = '';
        }

        function HeaderClick(CheckBox) {
            debugger;
            ClearSaveMessage();
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.grdCurrConv.ClientID %>');
            var TargetChildControl = "chkRow";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;


        }
        function PopUpDelegateUser() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            window.open('DelegateUser.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=400, height=150 , menubar=no, scrollbars=no, resizable=no');



            // return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="background-color: #adaba6">
                <%--<div style="background-color: #c41502; height: 18px;">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <asp:Menu ID="MenuAdmin" Orientation="Vertical" runat="server" BackColor="#f8df9c"
                                    StaticMenuStyle-HorizontalPadding="20px" DynamicHorizontalOffset="2" Font-Names="Calibri"
                                    Font-Size="11px" ForeColor="#c41502" Font-Bold="true" StaticSubMenuIndent="10px">
                                    <Items>
                                      
                                        <asp:MenuItem Text=" Admin " Value="Admin" NavigateUrl="~/BEAdmin.aspx">
                                            <asp:MenuItem Text="Freezing and Delegation" Value="freeze">
                                                <asp:MenuItem Text="Application Freeze" Value="App Freeze" NavigateUrl="javascript:PopUpFreeze();" />
                                                <asp:MenuItem Text="Monthly Freeze" Value="Monthly Freeze" NavigateUrl="~/MasterSetting.aspx" />
                                                <asp:MenuItem Text="Delegation" Value="Delegation" NavigateUrl="~/DelegatePage.aspx" />
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="Master Data" Value="Data">
                                                <asp:MenuItem Text="Client Code Portfolio" Value="Client Code Portfolio" NavigateUrl="~/ClientCodePortfolioScreen0.aspx" />
                                                <asp:MenuItem Text="Portfolio" Value="Portfolio" NavigateUrl="~/BEPortfolioAdmin.aspx" />
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="Exchange Rates" Value="Exchange Rate" NavigateUrl="~/ExchangeRate.aspx">
                                                <asp:MenuItem Text=" Daily Conversion" Value=" Daily Conv." NavigateUrl="~/ConvRateScreen.aspx" />
                                                <asp:MenuItem Text="Monthly Conversion" Value="Monthly Conv." NavigateUrl="~/GuidanceAndActuals.aspx" />
                                                <asp:MenuItem Text="Push Exchange Rates" Value="Exchange Rate" NavigateUrl="~/ExchangeRate.aspx" />
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="Maintenance" Value="Maintain">
                                                <asp:MenuItem Text="Audit Log" Value="Audit">
                                                    <asp:MenuItem Text="View" Value="Audit" NavigateUrl="~/AuditLog.aspx" />
                                                    <asp:MenuItem Text="Delete" Value="Delete" NavigateUrl="~/AuditLogDelete.aspx" />
                                                </asp:MenuItem>
                                                <asp:MenuItem Text="DM SDM Map" Value="DM SDM Map" NavigateUrl="~/DmSdmMap.aspx" />
                                                <asp:MenuItem Text="Deletion/Updation Of Data" Value="Update" NavigateUrl="~/MCCDMSDMChange.aspx" />
                                                <asp:MenuItem Text="User" Value="User" NavigateUrl="~/BEAdmin.aspx" />
                                            </asp:MenuItem>
                                         
                                        </asp:MenuItem>
                                    </Items>
                                    <StaticSelectedStyle BackColor="#c41502" />
                                    <StaticMenuItemStyle HorizontalPadding="20px" VerticalPadding="2px" />
                                    <DynamicHoverStyle BackColor="#c41502" Font-Bold="False" ForeColor="White" />
                                    <DynamicItemTemplate>
                                        <%# Eval("Text") %>
                                    </DynamicItemTemplate>
                                    <DynamicMenuStyle BackColor="#f8df9c" />
                                    <DynamicSelectedStyle BackColor="#1C5E55" />
                                    <DynamicMenuItemStyle HorizontalPadding="15px" VerticalPadding="2px" />
                                    <StaticHoverStyle BackColor="#c41502" Font-Bold="False" ForeColor="White" />
                                    <StaticItemTemplate>
                                        <%# Eval("Text") %>
                                    </StaticItemTemplate>
                                </asp:Menu>
                            </td>
                            <td style="width: 60px" align="right" bgcolor="#C41502">
                                <asp:HyperLink ID="HyperLink1" Font-Bold="true" Visible="true" ForeColor="#F8DF9C"
                                    runat="server" NavigateUrl="~/BEHome.aspx" Font-Underline="True">Revenue &nbsp</asp:HyperLink>
                            </td>
                            <td style="width: 60px" align="right" bgcolor="#C41502">
                                <asp:HyperLink ID="hypVol" Font-Bold="true" Visible="true" ForeColor="#F8DF9C" runat="server"
                                    NavigateUrl="~/BEVolume.aspx" Font-Underline="True">Volume &nbsp</asp:HyperLink>
                            </td>
                            <td style="width: 60px" align="right" bgcolor="#C41502">
                                <asp:HyperLink ID="hypbacktoreports" Font-Bold="true" ForeColor="#F8DF9C" runat="server"
                                    Visible="true" NavigateUrl="~/Reports.aspx" Font-Underline="true">Reports </asp:HyperLink>
                                &nbsp
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <asp:RoundedCornersExtender ID="RoundedCornersExtender1" BorderColor="White" Radius="10"
                    Corners="All" TargetControlID="pnlGrid" runat="server">
                </asp:RoundedCornersExtender>
                <asp:ModalPopupExtender BackgroundCssClass="modalBackground" DropShadow="false" X="440"
                    Y="500" CancelControlID="btnClose" runat="server" PopupControlID="Panel1" ID="ModalPopupExtender1"
                    OkControlID="btnCancel" PopupDragHandleControlID="Panel1" Drag="true" TargetControlID="btnAddNew"
                    OnInit="btnAddNew_Click1" />
                <asp:ModalPopupExtender BackgroundCssClass="modalBackground" DropShadow="false" X="440"
                    Y="700" CancelControlID="btnClose1" runat="server" PopupControlID="Panel2" ID="ModalPopupExtender2"
                    OkControlID="btnCancel" PopupDragHandleControlID="Panel2" Drag="true" TargetControlID="btnAddNewCurrency"
                    OnInit="btnAddNewCurrency_Click1" />
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlGrid" Width="100%" Height="450px" runat="server" BackColor="white">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr valign="top">
                                        <td align="center">
                                            <td align="left" class="style2">
                                                <asp:Label ID="lblMsg" runat="server" Text="Label" Visible="false"></asp:Label>
                                            </td>
                                            
                                            <td align="right">
                                                Select Year: &nbsp
                                            </td>
                                            <td align="left" style="width: 40px">
                                                <asp:DropDownList ID="ddlYear" runat="server" DataTextField="txtYear" DataValueField="txtYear"
                                                    CssClass="TextBox" AutoPostBack="True" 
                                                    onselectedindexchanged="ddlYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                Select Quarter: &nbsp
                                            </td>
                                            <td align="left" style="width: 40px">
                                                <asp:DropDownList ID="ddlMonth" runat="server" DataTextField="txtMonth" DataValueField="txtMonth"
                                                    CssClass="TextBox" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            
                                            <td align="left">
                                                <asp:Button ID="Button1" Text=" Search " runat="server" CssClass="button" OnClick="btnSearch_Click" />
                                            </td>
                                            <td style="width: 30%">
                                            </td>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center">
                                            <div id="divgrid" runat="server" style="height: 410px; width: 100%; overflow: auto">
                                                <asp:GridView ID="grdCurrConv" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found"
                                                    CssClass="mGrid" OnRowDataBound="grdCurrConv_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%--<asp:Image ID="img" runat="server" ImageUrl="~/Images/old.gif" />--%>
                                                                <asp:CheckBox ID="chkRow" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="txtNativeCurrency" HeaderText="Currency"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Guidance Rate" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtGuidanceConvRate" runat="server" CssClass="TextBox" Text='<%# Bind("fltRunningAvgRate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="BenchMark Rate" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBenchMarkRate" runat="server" CssClass="TextBox" Text='<%# Bind("fltBenchmarkRate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="QtyAvgFA Rate" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQtyAvgFARate" runat="server" CssClass="TextBox" Text='<%# Bind("fltQtyAvgFARate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--                                                        <asp:TemplateField HeaderText="Actual Rate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtActualConvRate" runat="server" CssClass="TextBox" Text='<%# Bind("fltActualConvRate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Actual Rate" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtActualConvRateMonth1" runat="server" CssClass="TextBox" Text='<%# Bind("fltMonth1ActRate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Actual Rate" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtActualConvRateMonth2" runat="server" CssClass="TextBox" Text='<%# Bind("fltMonth2ActRate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Actual Rate" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtActualConvRateMonth3" runat="server" CssClass="TextBox" Text='<%# Bind("fltMonth3ActRate") %>'
                                                                    Width="100" onKeyDown="return PressfloatOnly(event,this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%-- <asp:BoundField DataField="Month" HeaderText="Month" ItemStyle-Width="50px" Visible="false">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Year" HeaderText="Year" ItemStyle-Width="50px" Visible="false">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnfldMonth" Value='<%# Bind("Month") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnfldYear" Value='<%# Bind("Year") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0" style="margin-top: 0px">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnAddNew" runat="server" Text=" Copy Data to Next Quarter " CssClass="button" OnClick="btnAddNew_Click1" visible="false"/>
                                            &nbsp;
                                            <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="button" OnClick="btnSave_Click" />
                                            &nbsp;
                                            <asp:Button ID="btnDelete" runat="server" Text=" Delete " CssClass="button" OnClick="btnDelete_Click1" />
                                            &nbsp;
                                            <asp:Button ID="btnAddNewCurrency" runat="server" Text=" Add New Currency " CssClass="button" OnClick="btnAddNewCurrency_Click1" />
                                        </td>
                                    </tr>
                                </table>                                
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <div style="height: 7px">
                </div>
            </div>
            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;
                left: 480px" BorderWidth="2px" BackColor="White">
                <asp:UpdatePanel ID="updatepnlpopup" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div id="dialog" class="web_dialog">
                            <table id="tblTitlePopUP" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="1" class="web_dialog_title">
                                        Monthly Currency Conversion Rate - Add new
                                    </td>
                                    <td class="web_dialog_title align_right">
                                        <asp:Image ID="btnClose" Width="25" CssClass="closebtn" ToolTip="Close this PopUp"
                                            Height="25" runat="server" ImageUrl="~/Images/close.png" />
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
                                        <asp:Label ID="lblpopupInfo" runat="server" Text="" CssClass="TextBox" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                <td>From:</td><td></td><td></td><td>To:</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Month :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlpopUpMonth" runat="server" CssClass="TextBox">
                                            <asp:ListItem>January</asp:ListItem>
                                            <asp:ListItem>February</asp:ListItem>
                                            <asp:ListItem>March</asp:ListItem>
                                            <asp:ListItem>April</asp:ListItem>
                                            <asp:ListItem>May</asp:ListItem>
                                            <asp:ListItem>June</asp:ListItem>
                                            <asp:ListItem>July</asp:ListItem>
                                            <asp:ListItem>August</asp:ListItem>
                                            <asp:ListItem>September</asp:ListItem>
                                            <asp:ListItem>October</asp:ListItem>
                                            <asp:ListItem>November</asp:ListItem>
                                            <asp:ListItem>December</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        Month :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlpopUpMonthTo" runat="server" CssClass="TextBox">
                                            <asp:ListItem>January</asp:ListItem>
                                            <asp:ListItem>February</asp:ListItem>
                                            <asp:ListItem>March</asp:ListItem>
                                            <asp:ListItem>April</asp:ListItem>
                                            <asp:ListItem>May</asp:ListItem>
                                            <asp:ListItem>June</asp:ListItem>
                                            <asp:ListItem>July</asp:ListItem>
                                            <asp:ListItem>August</asp:ListItem>
                                            <asp:ListItem>September</asp:ListItem>
                                            <asp:ListItem>October</asp:ListItem>
                                            <asp:ListItem>November</asp:ListItem>
                                            <asp:ListItem>December</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Year :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPopUpYear" runat="server" CssClass="TextBox">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        Year :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPopUpYearTo" runat="server" CssClass="TextBox">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center">
                                        <asp:Button ID="btnSavepopup" runat="server" OnClientClick="return ValidPopUpSave();"
                                            OnClick="btnSavepopup_click" Text=" Save " CssClass="button" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text=" Cancel " CssClass="button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
             <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none; 
                left: 480px" BorderWidth="2px" BackColor="White" Height="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div id="Div1" class="web_dialog">
                            <table id="Table1" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td colspan="1" class="web_dialog_title">
                                        Monthly Currency Conversion Rate - Add new
                                    </td>
                                    <td class="web_dialog_title align_right">
                                        <asp:Image ID="btnClose1" Width="25" CssClass="closebtn" ToolTip="Close this PopUp"
                                            Height="25" runat="server" ImageUrl="~/Images/close.png" />
                                    </td>
                                </tr>
                            </table>
                            <table id="Table2" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                            
                                <tr>
                                
                                <td align="right">
                                    <asp:Label ID="lblQuarter" runat="server" Text="">:</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblQtr" runat="server" Text=""></asp:Label>
                                </td>
                                </tr>
                                <tr>
                                <td align="right">
                                    <asp:Label ID="lblYear" runat="server" Text="">:</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblYr" runat="server" Text=""></asp:Label>
                                </td>
                               
                                </tr>
                                
                                <tr>
                                    <td colspan="2" style="text-align: center; height: 1px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label1" runat="server" Text="" CssClass="TextBox" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        CurrencyCode :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCurrency" runat="server"></asp:TextBox>
                                    </td>                                    
                                </tr>                               
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <asp:Button ID="Button2" runat="server" OnClientClick="return ValidPopUpSave();"
                                            OnClick="btnSavepopupAddNew_click" Text=" Save " CssClass="button" />
                                        &nbsp;
                                        <asp:Button ID="Button3" runat="server" Text=" Cancel " CssClass="button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
