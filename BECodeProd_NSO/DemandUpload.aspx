<%@ Page Title="Upload Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    codebehind="DemandUpload.aspx.cs" Inherits="DemandUpload" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript" src="Scripts/Utility.js"></script>
    <script type="text/javascript">
        function ValidateControl(contrl) {
        
            var upctrl = document.getElementById(contrl);
            var errorLabel = document.getElementById('MainContent_lblError');
            if (upctrl.value.length == 0) {
                errorLabel.innerText = 'Please browse a file for uploading';
                return false;
            }
            if (upctrl.value.indexOf("xlsx") > 0) {
                return true;
            }


            else {

                errorLabel.innerText = "Please upload Only .xlsx files ";

                return false;
            }

        }

        function validate() {
            //var userid = document.getElementById("<%=drpApplication.ClientID%>");
            var environment = document.getElementById('MainContent_drpApplication');
            if (environment.value == "-Select-") {

                alert("Please Select Environment")
                return false;
            }
            return true;
        }

        function PopUpMasterClientList() {


            var left = (screen.width - 400) / 2;
            var top = (screen.height - 300) / 2;

            var winpopupstatus = window.open('NewlyAddedMCC.aspx', 'ThisPopUp10', 'left = ' + left + ', top=' + top + ', width=300, height=200, menubar=no, scrollbars=yes, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }
            //return false;

        }

        function PopUpMasterClientListProd() {


            var left = (screen.width - 400) / 2;
            var top = (screen.height - 300) / 2;

            var winpopupstatus = window.open('NewAddedCustProd.aspx', 'ThisPopUp11', 'left = ' + left + ', top=' + top + ', width=300, height=200, menubar=no, scrollbars=yes, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }
            //return false;

        }


        function PopUpMasterClientAlcon() {

            var left = (screen.width - 400) / 2;
            var top = (screen.height - 300) / 2;

            var winpopupstatus = window.open('NewlyAddedAlconMCC.aspx', 'ThisPopUp12', 'left = ' + left + ', top=' + top + ', width=300, height=200, menubar=no, scrollbars=yes, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }
            //return false;

        }

        function PopUpMasterClientFinpulse() {

            var left = (screen.width - 400) / 2;
            var top = (screen.height - 300) / 2;

            var winpopupstatus = window.open('NewlyAddedFinpulseMCC.aspx', 'ThisPopUp13', 'left = ' + left + ', top=' + top + ', width=300, height=200, menubar=no, scrollbars=yes, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }
            //return false;

        }

        function PopUpMasterClientAlconProd() {

            var left = (screen.width - 400) / 2;
            var top = (screen.height - 300) / 2;

            var winpopupstatus = window.open('NewlyAddedAlconMCCProd.aspx', 'ThisPopUp14', 'left = ' + left + ', top=' + top + ', width=300, height=200, menubar=no, scrollbars=yes, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }
            //return false;

        }

        function PopUpMasterClientFinpulseProd() {

            var left = (screen.width - 400) / 2;
            var top = (screen.height - 300) / 2;

            var winpopupstatus = window.open('NewlyAddedFinpulseMCCProd.aspx', 'ThisPopUp15', 'left = ' + left + ', top=' + top + ', width=300, height=200, menubar=no, scrollbars=yes, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }
            //return false;

        }


        function ValidateQuarter() {

            var Ok = confirm('Are you sure correct quarter is selected?');

            if (Ok) return true;
            else return false;
        }

        function ValidateYear() {

            var Ok = confirm('Are you sure correct year is selected?');

            if (Ok) return true;
            else return false;
        }


        function PopUpUpdateVolume() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            var winpopupstatus = window.open('UpdateVolfromFinpulse.aspx', 'ThisPopUp18', 'left = ' + left + ', top=' + top + ', width=500, height=180 , menubar=no, scrollbars=no, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }

            // return false;
        }

        function PopUpUpdateVolumeProd() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            var winpopupstatus = window.open('UpdateVolfromFinpulseProd.aspx', 'ThisPopUp19', 'left = ' + left + ', top=' + top + ', width=500, height=180 , menubar=no, scrollbars=no, resizable=no');

            if (!winpopupstatus.closed)
            { winpopupstatus.focus(); }

            // return false;
        }


    </script>
     <style type="text/css">
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
        .mGrid
        {
            width: 100%;
            background-color: #fff; /* margin: 5px 0 10px 0;*/
            border: solid 1px #525252;
            border-collapse: collapse;
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
        .FormLabel
        {
            background-color: #f0f0ed;
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
        }
        .FormControls
        {
            background-color: White;
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
        }
        .TextBox
        {
            font-family: verdana;
            font-size: 9pt;
        }
    </style>
    <style type="text/css">
        #tbldev td
        {
            border: 1px solid #b12c1a;
        }
        #tblProd td
        {
            border: 1px solid #b12c1a;
        }
        
        
        .FormLabel
        {
            background-color: #f0f0ed;
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
        }
        .FormControls
        {
            background-color: White;
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-left: 20px">
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="false" Font-Size="Small"></asp:Label>
        <asp:Label ID="lblSuccess" runat="server" ForeColor="Green" Font-Bold="false" Font-Size="Small"></asp:Label><br />
       
    </div>
    <div>
     <asp:GridView ID="gvBeforeUpload" runat="server"  CssClass="mGrid" Visible="False" 
            OnRowCreated="gvBeforeUpload_RowCreated" HeaderStyle-BackColor="#c41502" 
            HeaderStyle-ForeColor="White">
        </asp:GridView>
        <br />
        <asp:GridView ID="gvAfterUplaod" runat="server"  CssClass="mGrid" Visible="false" OnRowCreated="gvAfterUplaod_RowCreated" HeaderStyle-BackColor="#c41502" HeaderStyle-ForeColor="White">
        </asp:GridView>
    
    </div>
    <br />
    <br />
    <div>
        <table>
            <tr>
                <td style="width: 30%">
                </td>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td class="FormControls">
                                <asp:Label ID="lblEnv" runat="server" Text=" Select the Environment: "></asp:Label>
                            </td>
                            <td class="FormControls">
                                <asp:DropDownList ID="drpApplication" Font-Names="verdana" runat="server" CssClass="TextBox">
                                    <asp:ListItem Text="-Select-" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Development"></asp:ListItem>
                                    <asp:ListItem Text="Production"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" Text=" GO " CssClass="button" runat="server" OnClick="btnSearch_Click"
                                    OnClientClick="return validate();" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 30%">
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" style="height: 438px;">
        <div id="divDev" runat="server" style="height: 438px;" visible="false">
           
               
                   
                        <table id="tbldev" style="width: 1100px; height: 140px;" cellpadding="2" cellspacing="1">
                            <tr>
                                <td align="center" bgcolor="#B12C1A" colspan="3">
                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Names="Calibri" ForeColor="White"
                                        Text="Data Upload (DEVELOPMENT)"></asp:Label>
                                </td>
                                <td align="center" bgcolor="#B12C1A" style="font-weight: bold; color: #FFFFFF">
                                    Download Sample
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="Label14" runat="server" Text="FINPULSE Load :" ForeColor="Black"></asp:Label>
                                </td>
                                <td class="FormControls" align="left">
                                    <asp:FileUpload ID="FinPulseUpload" runat="server" Height="24px" />
                                    <asp:DropDownList ID="drpYer" Font-Names="verdana" runat="server" 
                                        CssClass="TextBox" onselectedindexchanged="drpYer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <%--<asp:DropDownList ID="drpSU" Font-Names="verdana" runat="server" CssClass="TextBox">
                                    </asp:DropDownList>--%>
                                    &nbsp;
                                     <asp:DropDownList ID="ddlServiceline" Font-Names="verdana" runat="server" CssClass="TextBox">
                                         <asp:ListItem Value="0">ORC</asp:ListItem>
                                         <asp:ListItem Value="1">SAP</asp:ListItem>
                                         <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:LinkButton ID="lnkDmMailId" OnClick="lnkDmMailId_Click" runat="server">Update DM MailID</asp:LinkButton>--%>
                                    &nbsp;
                                      <asp:DropDownList ID="ddlMonth" Font-Names="verdana" runat="server" CssClass="TextBox">
                                      <asp:ListItem Value="0">00</asp:ListItem>
                                      <asp:ListItem Value="1">01</asp:ListItem>
                                      <asp:ListItem Value="2">02</asp:ListItem>
                                      <asp:ListItem Value="3">03</asp:ListItem>
                                      <asp:ListItem Value="4">04</asp:ListItem>
                                      <asp:ListItem Value="5">05</asp:ListItem>
                                      <asp:ListItem Value="6">06</asp:ListItem>
                                      <asp:ListItem Value="7">07</asp:ListItem> 
                                      <asp:ListItem Value="8">08</asp:ListItem>
                                      <asp:ListItem Value="9">09</asp:ListItem>
                                      <asp:ListItem Value="10">10</asp:ListItem>
                                      <asp:ListItem Value="11">11</asp:ListItem>
                                      <asp:ListItem Value="12">12</asp:ListItem>
                                         <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:LinkButton ID="lnkupdatefin" OnClientClick=" return PopUpUpdateVolume();" 
                                        runat="server" onclick="lnkupdatefin_Click">Update Volume</asp:LinkButton>--%>
                                </td>
                                <td class="FormControls">
                                    <asp:Button ID="btnfinpulUpload" runat="server" CssClass="button" Text=" Upload "
                                        OnClick="btnfinpulUpload_Click" />
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="ImgDownLoadFinPul" runat="server" Width="20" Height="20" ToolTip="Sample Excel for FinPulse"
                                        ImageUrl="~/Images/exportexcel.bmp" OnClick="ImgDownLoadFinPul_Click" />
                                    &nbsp;
                                </td>
                             
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="Label15" runat="server" Text="RTBR Load :"></asp:Label>
                                </td>
                                <td class="FormControls" align="left">
                                    <asp:FileUpload ID="RTBRUpload" Font-Names="verdana" runat="server" CssClass="TextBox" />
                                    <asp:DropDownList ID="ddlUpload" runat="server" Visible="false">                                    
                                        <asp:ListItem Value="0">ORC</asp:ListItem>
                                        <asp:ListItem Value="1">SAP</asp:ListItem>
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                               
                                <td>
                                <asp:Button ID="btnVerify" runat="server" CssClass="button" Text=" Verify " 
                                        onclick="btnVerify_Click"  /> &nbsp&nbsp
                                    <asp:Button ID="btnRtbrUpload" runat="server" CssClass="button" Text=" Upload " OnClick="btnRtbrUpload_Click" />
                                    
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="ImageButton6" runat="server" Width="20" Height="20" ToolTip="Sample Excel for RTBR"
                                        ImageUrl="~/Images/exportexcel.bmp" OnClick="ImageButton6_Click" />
                                    &nbsp;
                                </td>
                                      </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="Label1" runat="server" Text="PBS-ALCON Load :" Enabled="False"></asp:Label>
                                </td>
                                <td class="FormControls" align="left">
                                    <asp:FileUpload ID="AlconUpload" runat="server" Height="24px" />
                                     <asp:DropDownList ID="ddlSuAlcon" runat="server" Visible="false" >                                    
                                        <asp:ListItem Value="0">ORC</asp:ListItem>
                                        <asp:ListItem Value="1">SAP</asp:ListItem>
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnAlcon" runat="server" CssClass="button" Text=" Upload " OnClick="btnAlcon_Click" />
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="ImgbtnAlcon" runat="server" Width="20" Height="20" ToolTip="Sample Excel for ALCON"
                                        ImageUrl="~/Images/exportexcel.bmp" OnClick="ImgbtnAlcon_Click" />
                                    &nbsp;
                                </td>
                            </tr>
                          
                        </table>
                  
            <br />
        </div>
  
                  
            <br />
        </div>
  
</asp:Content>
