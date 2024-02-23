<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="ClientCodePortfolioScreen0.aspx.cs" Inherits="ClientCodePortfolioScreen0" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--  <script src="Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>--%>
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
        .web_dialog
        {
            position: fixed;
            width: 55%;
            height: 40%;
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
        
        .hdnfldDisplay
        {
            display: none;
        }
        
        .txtFont
        {
            font-family: Calibri;
            font-size: 9pt;
        }
        .TextBox
        {
            font-family: Calibri;
            font-size: small;
        }
        .button
        {}
       
          
    </style>
    <script type="text/javascript">
        function PoPUPSave() {
            return true;
        }
        function ValidPopUpSave() {

            var idMCC = 'MainContent_txtpopupMCC';
            //var idGCR = 'MainContent_txtPopupGCR';
            var idCC = 'MainContent_txtpopupClientCode';

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
        
    </script>
    <script language="javascript" type="text/javascript">
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
            window.open('AppFreeze.aspx', 'pop', 'left = ' + left + ', top=' + top + ', width=450, height=190 , menubar=no, scrollbars=no, resizable=no');



            //return false;
        }

        function ClearSaveMessage() {

            var control = document.getElementById('MainContent_lblMsg');
            if (control != null)
                control.outerText = '';
        }

        function HeaderClick(CheckBox) {
            //            ClearSaveMessage();
            //Get target base & child control.
            var TargetBaseControl = document.getElementById('<%= this.grdClientCode.ClientID %>');
            var TargetChildControl = "chkRow";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;


        }
    </script>
    <script type="text/javascript">
        function PopUp(rowid) {
            debugger;
            var no = rowid.id.split('_')[3];
            var hndfldname = 'MainContent_grdClientCode_hdnfldID_' + no;
            var pkey = document.getElementById(hndfldname).value;
            var left = (screen.width - 900) / 2;
          ///  var top = (screen.height - 300) / 2;
            window.open('editPortfolio.aspx?&MCCID=' + pkey, 'pop', 'left = ' + left + ', width=1000, height=275, menubar=no, scrollbars=no, resizable=no');


        }

        function PopUpCopy() {



            var grid = document.getElementById('<%= this.grdClientCode.ClientID %>');
            //            var TargetChildControl = "chkRow";

            //            //Get all the control of the type INPUT in the base control.
            //            var Inputs = TargetBaseControl.getElementsByTagName("input");

            var hiddenno = 0;
            var prefix = 'MainContent_grdClientCode_';
            var icount = 0;
            for (var i = 0; i < grid.rows.length; i++) {
                var checkbox = document.getElementById(prefix + 'chkRow_' + i);
                if (checkbox != null) {
                    var ischecked = checkbox.checked;
                    if (ischecked) {
                        icount++;

                        var hiddennoctrl = document.getElementById(prefix + 'hdnfldID_' + i);
                        if (hiddennoctrl != null)
                            hiddenno = hiddennoctrl.value;
                    }
                }
            }
            if (icount == 0) {
                alert('Please select a row');
                return false;
            }

            if (icount > 1) {
                alert('Please select one row');
                return false;
            }




            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            window.open('CopyRowCCP.aspx?&MCCID=' + hiddenno, 'pop', 'left = ' + left + ', top=' + top + ', width=700, height=260, menubar=no, scrollbars=no, resizable=no');

            return false;

        }
    </script>
    <script type="text/javascript">
        function PopUpAddNew() {

            //  var no = rowid.id.split('_')[3];
            //var hndfldname = 'MainContent_grdClientCode_hdnfldID_' + no;
            // var pkey = document.getElementById(hndfldname).value;

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            window.open('AddNewPortfolio.aspx?', 'pop', 'left = ' + left + ', top=' + top + ', width=700, height=260, menubar=no, scrollbars=no, resizable=no');


        }

        function PopUpDelegateUser() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            window.open('DelegateUser.aspx', 'pop', 'left = ' + left + ', top=' + top + ', width=400, height=150 , menubar=no, scrollbars=no, resizable=no');



            //return false;
        }
    </script>

    <style>
     select::-ms-expand {
    display: none;
}
    </style>

    <link rel="stylesheet" href="boot.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="background-color: #adaba6">
                <%-- <div style="background-color: #c41502; height: 18px;">
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
                                                <asp:MenuItem Text="E-Mail Alert Settings" Value="Delegation" NavigateUrl="~/BEMailAlertSettings.aspx" />
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
                <%-- <asp:ModalPopupExtender BackgroundCssClass="modalBackground" DropShadow="false" X="200"
                    Y="300" CancelControlID="btnClose" runat="server" PopupControlID="Panel1" ID="ModalPopupExtender1" 
                    OkControlID="btnCancel" PopupDragHandleControlID="Panel1" Drag="true" TargetControlID="btnAddNew" 
                    OnInit="btnAddNew_Click1" />--%>
                <%--<asp:ModalPopupExtender BackgroundCssClass="modalBackground" DropShadow="false" X="200"
                    Y="300" CancelControlID="btnEditClose" runat="server" PopupControlID="pnlEdit"
                    ID="ModalPopupExtender2" OkControlID="btnEditCancel" PopupDragHandleControlID="pnlEdit"
                    Drag="true" TargetControlID="lbtnEdit" />--%>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlGrid" Width="100%" Height="500px" runat="server" BackColor="white"
                                CssClass="txtFont">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="right" style="width: 10px">
                                        </td>
                                        <td align="right" style="width: 50px">
                                            PU :
                                        </td>
                                        <td align="left" style="width: 100px">
                                            <asp:DropDownList ID="ddlPu" runat="server" DataTextField="txtPu" DataValueField="txtPu"
                                                 CssClass="form-control" OnSelectedIndexChanged="ddlPu_SelectedIndexChanged" AutoPostBack="True"
                                                Width="120">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" style="width: 75px">
                                            SDM :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlSDM" runat="server" DataTextField="txtSDMMailid" DataValueField="txtSDMMailid"
                                                 CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSDM_SelectedIndexChanged"
                                                Width="120">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" style="width: 300px">
                                            Master Customer Code:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlMcc" runat="server" DataTextField="txtMCC" DataValueField="txtMCC"
                                                 CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlMcc_SelectedIndexChanged"
                                                Width="120">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" style="width: 100px">
                                            <asp:Button ID="btnSearch" Text=" Search" runat="server" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" OnClick="btnSearch_Click" />
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                        <td style="width: 100px" align="right">
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control1">
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:ImageButton ID="lnkSearch" runat="server" Width="15" Height="15" ToolTip="Search Master Client Code"
                                                ImageUrl="~/Images/search.png" OnClick="lnkSearch_Click" />
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 300px" align="left">
                                            <asp:Label ID="lblMsg" runat="server" Text="Label" Visible="false" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center">
                                            <div id="divgrid" runat="server" style="height: 410px; width: 100%; overflow: auto">
                                                <asp:GridView ID="grdClientCode" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found"
                                                    CssClass="mGrid" Width="100%" AllowPaging="True" OnPageIndexChanging="grdClientCode_PageIndexChanging"
                                                    PageSize="30" PagerSettings-Position="TopAndBottom" 
                                                    PagerSettings-Mode="NumericFirstLast" onselectedindexchanged="grdClientCode_SelectedIndexChanged" 
                                                   >
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <asp:Image ID="img" runat="server" ImageUrl="~/Images/old.gif" />
                                                                <asp:CheckBox ID="chkRow" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemStyle Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hypEdit" Text="edit" NavigateUrl="#" onclick="PopUp(this); return false;"
                                                                    runat="server"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="txtMasterClientCode" HeaderText="Master Client Code" ItemStyle-Width="100px">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtMasterCustomerName" HeaderText="Master Customer Name"
                                                            ItemStyle-Width="100px">
                                                            <ItemStyle  Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtClientCode" HeaderText="Client Code" ItemStyle-Width="100px">
                                                            <ItemStyle  Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtClientName" HeaderText="Client Name" ItemStyle-Width="100px">
                                                            <ItemStyle    Wrap="false"/>
                                                        </asp:BoundField>
                                                     
                                                        <asp:BoundField DataField="txtVertical" HeaderText="Vertical" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                      
                                                        <asp:BoundField DataField="txtSDMMailId" HeaderText="SDM Mail Id" ItemStyle-Width="100px">
                                                            <ItemStyle  Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtDHMailId" HeaderText="DH Mail Id" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtBITSCSIHMailId" HeaderText="BITSCSIH Mail Id" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtUHMailId" HeaderText="UH Mail Id" ItemStyle-Width="100px">
                                                            <ItemStyle    Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtPU" HeaderText="Practice" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtUpdatedBy" HeaderText="Updated By" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dtUpdatedDate" HeaderText="Updated Date" ItemStyle-Width="100px"
                                                            DataFormatString="{0:dd-MMM-yy}">
                                                            <ItemStyle  Wrap="false"/>
                                                        </asp:BoundField>
                                                      
                                                         <asp:BoundField DataField="isActive" HeaderText="isActive" ItemStyle-Width="100px">
                                                            <ItemStyle    Wrap="false"/>
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="txtServiceline" HeaderText="Service Line" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="txtunit" HeaderText="Unit" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false" />
                                                        </asp:BoundField>
                                                           <asp:BoundField DataField="txtPortfolio" HeaderText="Portfolio" ItemStyle-Width="100px">
                                                            <ItemStyle    Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="txtDivision" HeaderText="Division" ItemStyle-Width="100px">
                                                            <ItemStyle  Wrap="false" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="txtRHMailId" HeaderText="RH Mail Id" ItemStyle-Width="100px">
                                                            <ItemStyle    Wrap="false"/>
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="txtFAPortfolio" HeaderText="FA Portfolio" ItemStyle-Width="100px">
                                                            <ItemStyle   Wrap="false"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField ItemStyle-CssClass="hdnfldDisplay" HeaderStyle-CssClass="hdnfldDisplay">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnfldID" Value='<%# Bind("intMccId") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="hdnfldDisplay" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0" style="margin-top: 0px">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnCopy" runat="server" OnClientClick="return PopUpCopy();" Text=" Copy Row "
                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="70px" Visible="false" />
                                            &nbsp; &nbsp;
                                            <asp:Button ID="btnAddNew" runat="server" Text=" Add New " class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small"
                                                Width="67px" Visible="true"
                                                OnClientClick="PopUpAddNew(); return false;" OnClick="btnAddNew_Click" />
                                            &nbsp; &nbsp;
                                            <asp:Button ID="btnDelete" runat="server" Text=" Delete " class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" OnClick="btnDelete_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <div style="height: 7px">
                </div>
                <%--</div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
