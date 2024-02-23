<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="BEAdmin.aspx.cs" Inherits="BEAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
      <script type="text/javascript" src="NSO/jquery-1.10.2.js"></script>
    <style type="text/css">
        .info, .success, .warning, .error, .validation
        {
            border: 1px solid;
            margin: 10px 0px;
            padding: 5px 7px 5px 5px;
            background-repeat: no-repeat;
            background-position: 10px center;
            width: 200px;
        }
        .info
        {
            color: #00529B;
            background-color: #BDE5F8;
            background-image: url('~\Images\info.png');
        }
        .success
        {
            color: #4F8A10;
            background-color: #DFF2BF;
            background-image: url('~/Images/success.png');
        }
        .warning
        {
            color: #9F6000;
            background-color: #FEEFB3;
            background-image: url('~\Images\warning.png');
        }
        .error
        {
            color: #D8000C;
            background-color: #FFBABA;
            background-image: url('~\Images\error.png');
        }
    </style>
    <style type="text/css">
        .closebtn
        {
            cursor: pointer;
            cursor: hand;
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
        .TextBox
        {
            font-family: Calibri;
            font-size: 9pt;
        }
    </style>
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
            width: 300px;
            height: 195px;
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
         select::-ms-expand {
    display: none;
}
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
        .style1
        {
            width: 160px;
        }
        .style2
        {
            text-align: right;
            width: 86px;
        }
        .style3
        {
            width: 172px;
        }
        </style>
    <script language="javascript" type="text/javascript">
        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
        
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById('DivHeaderRow');
                //var DivMC = document.getElementById('MainContent_DivMainContent');
                var DivMC = document.getElementById('DivMainContent');
                var DivFR = document.getElementById('DivFooterRow');
                //*** Set divheaderRow Properties ****   
                DivHR.style.height = headerHeight + 'px';
                DivHR.style.width = (parseInt(width) - 16) + 'px';
                DivHR.style.position = 'relative';
                DivHR.style.top = '0px';
                DivHR.style.zIndex = '11';
                DivHR.style.verticalAlign = 'top';
                //*** Set divMainContent Properties ****   
                DivMC.style.width = (parseInt(width) - 16) + 'px'; // width + 'px';
                DivMC.style.height = height + 'px';
                DivMC.style.position = 'relative';
                DivMC.style.top = -headerHeight + 'px';
                DivMC.style.zIndex = '1';
                //*** Set divFooterRow Properties **** 
                DivFR.style.width = (parseInt(width) - 16) + 'px';
                DivFR.style.position = 'relative';
                DivFR.style.top = -headerHeight + 'px';
                DivFR.style.verticalAlign = 'top';
                DivFR.style.paddingtop = '2px';
                if (isFooter) {
                    var tblfr = tbl.cloneNode(true);
                    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                    var tblBody = document.createElement('tbody');
                    tblfr.style.width = '100%';
                    tblfr.cellSpacing = "0";
                    tblfr.border = "0px";
                    tblfr.rules = "none";
                    //*****In the case of Footer Row *******   
                    tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
                    tblfr.appendChild(tblBody);
                    DivFR.appendChild(tblfr);
                }
                //****Copy Header in divHeaderRow**** 
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }



    </script>
    <script type="text/javascript">
        function openWindowLink(link) {
            window.navigate(link);
        }
        function openWindow(type) {
            ClearSaveMessage();
            if (type == "MCOBEData") {
                window.open('MCOBEData.aspx');
            }
            else if (type == "RTBRData") {
                window.open('http://www.bing.com/');
            }
            else if (type == "Opportunity") {
                window.open('Opportunity.aspx');
            }
            else if (type == 'ReportBEBaseData') {
                window.open('ReportBEBaseData.aspx');
            }
            else if (type == 'BEReportForINPIPE') {
                window.open('ReportForINPIPEUpdate.aspx');
            }
        }
        function PopUpFreeze() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            window.open('AppFreeze.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=450, height=190 , menubar=no, scrollbars=no, resizable=no');



            //return false;
        }


        function PopUpDelegateUser() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            window.open('DelegateUser.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=400, height=150 , menubar=no, scrollbars=no, resizable=no');



            //        return false;
        }
         
    </script>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.21.custom/js/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.21.custom/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

         

 
    </script>

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="background-color:''">
                <div style="height: 1px">
                </div>
                <div style="height: 1px">
                    <%--<asp:LinkButton ID="Button1" runat="server" Visible="true" Text="" />--%>
                    <asp:LinkButton ID="Button1" runat="server" Text=""></asp:LinkButton>
                </div>
               <%-- <asp:RoundedCornersExtender ID="RoundedCornersExtender1" BorderColor="White" Radius="10"
                    Corners="All" TargetControlID="pnlGrid" runat="server">
                </asp:RoundedCornersExtender>--%>
                <table width="100%" style="height: 100%">
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlGrid" Width="900px" Height="625px" runat="server" 
                                BackColor="white">
                                <div>
                                <%--<div>
                                <table>
                                <tr>
                                <td>--%>
                                    <%-- <h1 style="color: Red">
                                        Development in progress.....</h1>--%>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblError" runat="server" Font-Size="Small" ForeColor="#FF3300"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:HyperLink ID="hypMaintenance" runat="server" NavigateUrl="~/MaintenancePageAdmin.aspx"
                                                    Text="Maintenance" Visible="false"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblSuccess" runat="server" Font-Size="Small" ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    
                                    <%--</td>
                                    </tr>
                                    </table>--%>
                                      <div style="margin-top:20px"><table width="100%"><tr valign="bottom" >
                                                        <td align="right" style="width: 100px">
                                                            User ID:
                                                        </td>
                                                        <td class="style1" align="left"  >
                                                            <asp:TextBox ID="txtUserID" CssClass="form-control1" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td align="left">                                                       
                                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-info btn-sm" Height="25" style="margin-left:10px;padding-top:2px!important;border:1px solid lightgray;" 
                                                         OnClick="btnSearch_Click" Text=" Search" />
                                                         &nbsp;&nbsp;
                                                            <asp:Button ID="btnReset" runat="server" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;"
                                                                OnClick="btnReset_Click" Text=" Reset " />
                                                        </td>
                                                       <%-- <td align="left" colspan="2">                                                       
                                                            &nbsp;</td>--%>
                                                       
                                                       </tr></table></div>
                                                         <div id="divForddlRole" runat="server" visible="false">
                                                     <table width=100%>
                                                     <tr  valign="bottom" >                                                    
                                                      <td align="right" style="width: 100px">
                                                      Role:
                                                      </td>
                                                      <td align="left" class="style1">                                                      
                                                      <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="True" CssClass="form-control"
                                                       OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" 
                                                      Width="150">
                                                      </asp:DropDownList>
                                                      </td>  
                                                      <td align="right" style="text-align: right" valign="middle">
                                                             
                                                            </td>
                                                            <td id="tdSU" runat="server" align="left"> 
                                                            <div style="float:left;margin-top:3px">   SU:</div>
                                                            <div id="divSu1" runat ="server"  style="float:left">                                                                                                                  
                                                            <asp:RadioButtonList ID="rdbSU" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                    onselectedindexchanged="rdbSU_SelectedIndexChanged">
                                                              <%--  <asp:ListItem Value="ORC" Selected="True">ORC</asp:ListItem>
                                                                <asp:ListItem Value="SAP">SAP</asp:ListItem> 
                                                                 <asp:ListItem Value="ECAS">ECAS</asp:ListItem>
                                                                <asp:ListItem Value="EAIS">EAIS</asp:ListItem> --%>
                                                                <asp:ListItem Value="All">All</asp:ListItem>                                                                
                                                            </asp:RadioButtonList>
                                                            </div>
                                                                 <div id="divSu2" runat ="server" style="display:none;float:left;">
                                                                <asp:CheckBoxList ID="chkSU" runat="server" RepeatDirection="Horizontal" 
                                                                    style="font-size: small">
                                                                    <asp:ListItem Value="ORC">ORC</asp:ListItem>
                                                                    <asp:ListItem Value="SAP">SAP</asp:ListItem>
                                                                    <asp:ListItem Value="ECAS">ECAS</asp:ListItem>
                                                                    <asp:ListItem Value="EAIS">EAIS</asp:ListItem>                                                                     
                                                                </asp:CheckBoxList>

                                                                </div>
                                                           </td>                                                  
                                                     </tr>
                                                     </table>
                                                     </div>
                                                     <div id="divInfo" runat="server" style="height: 15px" visible="true">
                                                    </div>
                                                     <div id="divDetails" runat="server" visible="false">
                                                    
                                                    <table width="100%" cellpadding="0" cellspacing="0" style="height: 27px">
                                                        <tr   valign="middle">
                                                            
                                                           <td align="right" style="width: 100px">
                                                                Type:
                                                            </td>
                                                            <td align="left" class="style3">       <asp:RadioButtonList ID="rdbType" runat="server" RepeatDirection="Horizontal" 
                                                                    Height="36px" Width="167px" style="font-size: small">
                                                                    <asp:ListItem Value="DM" Selected="True">DM</asp:ListItem>
                                                                    <asp:ListItem Value="SDM">SDM</asp:ListItem>
                                                                    <asp:ListItem Value="All">All</asp:ListItem>                                                                     
                                                                </asp:RadioButtonList>

                                                            
                                                           
                                                            </td>
                                                            <td class="style2">
                                                                IsEditable:
                                                            </td>  
                                                            <td  align="left">
                                                                <asp:RadioButtonList ID="rdbIsReadOnly" runat="server" 
                                                                    RepeatDirection="Horizontal" style="margin-left: 11px">
                                                                    <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="False">No</asp:ListItem>                                                                    
                                                                </asp:RadioButtonList>
                                                            </td>                                                     
                                                        </tr>
                                                        
                                                        </table>
                                                        </div>
                                                        <div id="Div1" runat="server" style="height: 15px">
                                                        
                                                        </div>
                                                         <div id="divForAccessLevel" runat="server" visible="false">
                                                        <table>
                                                        <tr  valign="middle">
                                                        <td align="right">
                                                        Access Level:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlAccessLevel" runat="server" CssClass="form-control" Width="70"
                                                                onselectedindexchanged="ddlAccessLevel_SelectedIndexChanged" AutoPostBack="True">
                                                                <asp:ListItem Value="Offering">Offering</asp:ListItem>
                                                                <asp:ListItem Value="MCC">MCC</asp:ListItem>
                                                                <asp:ListItem Value="SDM">SDM</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        </tr>
                                                        </table>
                                                        </div>


                                    <table>
                                        <tr  valign="bottom">
                                            <%--<td align="center">
                                               
                                                        </td>--%>
                                                        </tr>

                                                        <tr valign="top">
                                                        <td>
                                                        <div id="divForOfferings" runat="server" visible="false">
                                                        <table>
                                                         <tr valign="top">
                                                            <th>
                                                                ALL Offering List:
                                                            </th>
                                                            <th>
                                                            </th>
                                                            <th>
                                                                Offerings to be Added:
                                                            </th>
                                                        </tr>
                                                        <tr valign="top">                                                                                                                    
                                                   
                                                                   <td style="width: 280px;" align="center">
                                                                        <asp:ListBox ID="lstOffering" runat="server" CssClass="TextBox" Height="130px" 
                                                                            Rows="5" SelectionMode="Multiple" Width="250px"></asp:ListBox>
                                                                    </td>
                                                                    <td>
                                                                    <table>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnPUAddAll" runat="server" Text=" >>> " OnClick="btnPUAddAll_Click"
                                                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnPUAdd" runat="server" Text=" > " OnClick="btnPUAdd_Click" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;"
                                                                                Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnPURemove" runat="server" Text=" < " OnClick="btnPURemove_Click"
                                                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnPURemoveAll" runat="server" Text=" <<< " OnClick="btnPURemoveAll_Click"
                                                                               class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                        <asp:ListBox ID="lstOfferingDestination" runat="server" CssClass="TextBox" Height="130px" 
                                                                            Rows="5" SelectionMode="Multiple" Width="250px" 
                                                                            onselectedindexchanged="lstOfferingDestination_SelectedIndexChanged"></asp:ListBox>
                                                                    </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <%--<div style="height: 0px">
                                                    </div>--%>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                    </table>
                                                    <div runat="server" style="height: 15px">
                                                    </div>
                                                    <div id="divForMcc" runat="server" visible="false">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>
                                                                ALL Master Customer List:
                                                            </th>
                                                            <th>
                                                            </th>
                                                            <th>
                                                                MCCs to be Added:
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstMCCSource" Width="250px" SelectionMode="Single" runat="server"
                                                                    Height="130px" CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                            <td align="center" style="width: 50px;">
                                                                <table>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnMCCAddAll" runat="server" Text=" >>> " OnClick="btnMCCAddAll_Click"
                                                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMCCAdd" runat="server" Text=" > " OnClick="btnMCCAdd_Click" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small"
                                                                                Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMCCRemove" runat="server" Text=" < " OnClick="btnMCCRemove_Click"
                                                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnMCCRemoveAll" runat="server" Text=" <<< " OnClick="btnMCCRemoveAll_Click"
                                                                               class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstMCCDestination" Width="250px" Height="130px" runat="server" SelectionMode="Multiple"
                                                                    CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                    <div id="divForSDM" runat="server" visible="false">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>
                                                                ALL SDM List:
                                                            </th>
                                                            <th>
                                                            </th>
                                                            <th>
                                                                SDMs to be Added:
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstSDMSource" Width="250px" SelectionMode="Single" runat="server"
                                                                    Height="130px" CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                            <td align="center" style="width: 50px;">
                                                                <table>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnSDMAddAll" runat="server" Text=" >>> " OnClick="btnSDMAddAll_Click"
                                                                               class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSDMAdd" runat="server" Text=" > " OnClick="btnSDMAdd_Click" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small"
                                                                                Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSDMRemove" runat="server" Text=" < " OnClick="btnSDMRemove_Click"
                                                                               class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSDMRemoveAll" runat="server" Text=" <<< " OnClick="btnSDMRemoveAll_Click"
                                                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstSDMDestination" Width="250px" Height="130px" runat="server" SelectionMode="Multiple"
                                                                    CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                    <%--<div runat="server" style="height: 15px">
                                                    </div>--%>
                                                    </td>
                                                    </tr>
                                                    <%--<table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>
                                                                ALL Client Code List:
                                                            </th>
                                                            <th>
                                                            </th>
                                                            <th>
                                                                Client Codes to be Added:
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstClientCodeSource" Width="250px" SelectionMode="Single" runat="server"
                                                                    Height="100px" CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                            <td align="center" style="width: 50px;">
                                                                <table>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnClentCodeAll" runat="server" Text=" >>> " OnClick="btnClentCodeAll_Click"
                                                                                CssClass="button" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnClientAdd" runat="server" Text=" > " OnClick="btnClientAdd_Click" CssClass="button"
                                                                                Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnClientRemove" runat="server" Text=" < " OnClick="btnClientRemove_Click"
                                                                                CssClass="button" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnClientCodeRemoveAll" runat="server" Text=" <<< " OnClick="btnClientCodeRemoveAll_Click"
                                                                                CssClass="button" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstClientCodeDest" Width="250px" Height="100px" runat="server" SelectionMode="Multiple"
                                                                    CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>--%>
                                                    <tr>
                                                    <td>
                                                    <div id="divForReports" runat="server" visible="false">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>
                                                                ALL Reports List:
                                                            </th>
                                                            <th>
                                                            </th>
                                                            <th>
                                                                Reports to be Added:
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstReportList" Width="250px" SelectionMode="Single" runat="server" Visible="true" AutoPostBack="true"
                                                                    Height="100px" CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                            <td align="center" style="width: 50px;">
                                                                <table>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnAddAllReport" runat="server" Text=" >>> " class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px"
                                                                                OnClick="btnAddAllReport_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnReportAdd" runat="server" Text=" > " class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px"
                                                                                OnClick="btnReportAdd_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnRemove" runat="server" Text=" < " class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px"
                                                                                OnClick="btnRemove_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnRemoveAllReports" runat="server" Text=" <<< " OnClick="btnRemoveAllReports_Click"
                                                                                class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Width="40px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 280px;" align="center">
                                                                <asp:ListBox ID="lstReporttobeAdded" Width="250px" Height="100px" runat="server"
                                                                    SelectionMode="Multiple" CssClass="TextBox" Rows="5"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                    
                                                   <%-- <div id="Div2" runat="server" style="height: 15px">
                                                    </div>--%>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <div id="divForButtons" runat="server" visible="false">
                                                    <table cellpadding="0" cellspacing="0" style="height: 10px; width: 99%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btnSave" runat="server" Text=" Save " class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" OnClick="btnSave_Click"/>
                                                                &nbsp
                                                                <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text=" Delete "
                                                                    class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                </td>
                                                </tr>
                                            <%--</td>
                                        </tr>--%>
                                    </table>
                                <%--</div>--%>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <div style="height: 5px; width: 973px;">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
