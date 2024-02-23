<%@ Page Language="C#"  AutoEventWireup="true" Inherits="AddMasterCustomer" Codebehind="AddMasterCustomer.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%-- <link href="Styles/Site.css" rel="stylesheet" type="text/css" />--%><script
        src="Scripts/JQuery.js" type="text/javascript"></script>
        
 <link rel="stylesheet" href="boot.css"/>
    <title></title>
    <style type="text/css">
        body
        {
            background: #b6b7bc;
            font-size: 9pt;
            font-family: "Calibri";
            margin: 0px;
            padding: 0px;
            color: #696969;
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
        
        a:link
        {
            font-family: Calibri;
            font-size: 9pt;
            font-weight: bold;
            text-decoration: none;
            color: Blue;
            background-color: transparent;
        }
        
        .menuMain
        {
            color: black; /*background-image : url(images/image003.png);*/
        }
        .menuTop
        {
            color: black;
            font-family: Calibri;
            font-size: 11px;
            font-weight: bolder;
        }
        
        .style2
        {
            height: 23px;
        }
        .style3
        {
            color: #FF0000;
        }
        
         select::-ms-expand {
    display: none;
}
        
        </style>
    <script type="text/javascript">
       



    </script>
    <script type="text/javascript">
        function DoAnchorWala(ddl) {

            var selectedValue = ddl.value + '';
            var hnd = document.getElementById('hndValue');
            var strUser = ddl.options[ddl.selectedIndex].text;
            hnd.value = strUser;

            var trRole = document.getElementById('TRRole');
            var trdel = document.getElementById('TRDel');
            if (selectedValue == '1_0') {

                trRole.style.visibility = "visible";
                trdel.style.visibility = "hidden";
            }
            if (selectedValue == '0_0') {
                trRole.style.visibility = "hidden";
                trdel.style.visibility = "hidden";
            }
            if (selectedValue == '0_1') {
                trRole.style.visibility = "hidden";
                trdel.style.visibility = "visible";
            }

            if (selectedValue == '1_1') {
                trRole.style.visibility = "visible";
                trdel.style.visibility = "visible";

            }

        }

        function changeenteras(thisctrl) {

            var selected = $(':checked', $('#rdbDel'))[0].value;

            if (selected == 'Delegated')
            // document.getElementById('TRRole').style.visibility = "hidden";
                $('#TRRole').hide();
            else
            // document.getElementById('TRRole').style.visibility = "visible";
                $('#TRRole').show();

        }

        //        function OpenPopUp() {
        //            window.open("AnchorLogin.aspx");
        //        }

        function closePopup() {

            var win = window.open("", "pop");
            //win.moveTo(0, 0);
            win.blur();
            win.close();

            //              document.getElementById("ifr").contentWindow.document.write('popUpWindow = window.open("", "pop", "width=1,height=1");');
            //              document.getElementById("ifr").contentWindow.document.write('popUpWindow.close();');

        }

    </script>
</head>
<body style="background-color:White">
    <form id="form1" runat="server" target="_parent">
   
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <div id="divMain" runat="server">
      <table align="left"  bgcolor="White" style="padding-left:10px">
                         <tr>
                            <td align="center" colspan="2" class="style2" bgcolor="rgb(51, 51, 51)" style="font-family: Tahoma;
                                font-size: small; font-weight: bold; color: #FFFFFF;">
                                Include Master Customer to BE Application
                            </td>
                        </tr>
                        <tr >
                            <td colspan="2" font-size: "small">
                              
                                    <asp:Label ID="lblmsg" runat="server" Text="Label" Visible="False" 
                                        Font-Size="X-Small" style="font-size: small"></asp:Label>
                              
                            </td>
                        </tr>
                        
                      
                        
                          <tr runat="server" id="TRRole">
                            <td style="width: 200px; font-family: Calibri; font-size: small;" align="left">
                                &nbsp;&nbsp;<sup style="color: #FF0000">*</sup>Service Line:
                            </td>
                              <td style="width: 150px">
                                <asp:DropDownList ID="ddlServiceLine"  CssClass="form-control" Height="25" runat="server" style="font-family: Calibri; font-size: small;"  
                                       AutoPostBack="true"
                                    Width="150px" onselectedindexchanged="ddlServiceLine_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </td>          
                        </tr>
                      
                         
                         <tr runat="server" id="TR2">
                            <td style="width: 200px; font-family: Calibri; font-size: small;" align="left">
                                &nbsp;&nbsp;<sup style="color: #FF0000">*</sup>Master Customer Code:
                            </td>
                            <td style="width: 150px">
                                <asp:DropDownList ID="ddlMasterCustomerCode"  CssClass="form-control" Height="25" runat="server"  
                                    style="font-family: Calibri; font-size: small;"  AutoPostBack="true"
                                     Width="150px" 
                                     onselectedindexchanged="ddlMasterCustomerCode_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">New Account</asp:ListItem>
                                </asp:DropDownList>
                            </td>        
                        </tr>
             <tr runat="server" id="TR6">
                            <td style="width: 200px; font-family: Calibri; font-size: small;" align="left">
                                &nbsp;&nbsp;<sup style="color: #FF0000">*</sup>New Service Offering:
                            </td>
                              <td style="width: 150px">
                                <asp:DropDownList ID="ddlNSO"  CssClass="form-control" Height="25" runat="server" style="font-family: Calibri; font-size: small;"  
                                       AutoPostBack="true"  OnSelectedIndexChanged="ddlNSO_SelectedIndexChanged"
                                    Width="150px" >
                                </asp:DropDownList>
                            </td>          
                        </tr>
                            <%--<tr runat="server" id="TR1">
                            <td style="width: 200px; font-family: Calibri; font-size: small;" align="left">
                                &nbsp;&nbsp;<sup style="color: #FF0000">*</sup>PU:
                            </td>
                            <td style="width: 150px">
                                <asp:DropDownList ID="ddlPU"  CssClass="form-control" Height="25" runat="server"  
                                    style="font-family: Calibri; font-size: small;"  AutoPostBack="true"
                                     Width="150px" 
                                     onselectedindexchanged="ddlPU_SelectedIndexChanged" >
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                
                                </asp:DropDownList>
                            </td>        
                        </tr>--%>
                         <tr runat="server" id="TR3">
                            <td style="width: 200px; font-family: Calibri; font-size: small;" align="left">
                                &nbsp;&nbsp;<span class="style3"><sup>*</sup></span>Native Currency:
                            </td>
                              <td style="width: 150px">
                                <asp:DropDownList ID="ddlNativeCurrency"  CssClass="form-control" Height="25" runat="server" 
                                       style="font-family: Calibri; font-size: small;"  AutoPostBack="true"
                                     Width="150px" onselectedindexchanged="ddlNativeCurrency_SelectedIndexChanged" 
                                     >
                                </asp:DropDownList>
                            </td>          
                        </tr>
                         <tr runat="server" id="TR4">
                            <td style="width: 200px; font-family: Calibri; font-size: small;" align="left">
                                &nbsp;&nbsp;<sup style="color: #FF0000">*</sup>Quarter:
                            </td>
                              
                              <td style="width: 150px">
                                <asp:DropDownList ID="ddlQuarter"  CssClass="form-control" Height="25" runat="server" 
                                       style="font-family: Calibri; font-size: small;"  AutoPostBack="true"
                                     Width="150px" onselectedindexchanged="ddlQuarter_SelectedIndexChanged" >
                                </asp:DropDownList>
                                  </td>          
                        </tr>
                         <tr runat="server" id="TR5" >       
                             <td style="width: 200px; font-size: x-small;" align="left">
                                 &nbsp;&nbsp;   <sup style="color: #FF0000">*</sup><asp:Label ID="lblDMorSDM" runat="server" Text="" 
                                     style="font-size: small"></asp:Label>
                                 :</td>
                            <td style="width: 150px" >
                                <asp:DropDownList ID="ddlSDMorDM"  CssClass="form-control" Height="25" runat="server" 
                                     style="font-family: Calibri; font-size: small;"  AutoPostBack="true"
                                     Width="150px" onselectedindexchanged="ddlSDMorDM_SelectedIndexChanged" >
                                </asp:DropDownList>
                                 <asp:Label ID="lbl1" runat="server" Text="" style="font-size: small"></asp:Label>
                            </td>            
                        </tr>
                       <%-- <tr>
                        <td colspan=2>
                         <asp:HiddenField ID="HiddenField2" Value="" runat="server" />
                           <asp:HiddenField ID="HiddenField1" Value="" runat="server" />
                        </td>
                        </tr>--%>
                        <tr>
                           
                            <td align="center"   colspan="2">
                             <asp:Button ID="Button1" runat="server" Text=" Add " class="btn btn-info btn-sm" Height="25" style="margin-left:10px;padding-top:2px!important;border:1px solid lightgray;" 
 OnClick="btnAdd_Click" ToolTip=""/>
                               
                                <asp:Button ID="btnCancel" runat="server" Text=" Cancel " class="btn btn-info btn-sm" Height="25" style="margin-left:10px;padding-top:2px!important;border:1px solid lightgray;" 
 
                                  OnClientClick="closePopup(); return false" 
                                    ToolTip=""/>
                             
                            </td>
                        </tr>
                       
                    </table>
            </div>
    </ContentTemplate>
    <Triggers>
    </Triggers>
    </asp:UpdatePanel>

    
                 
    </form>
</body>


</html>
