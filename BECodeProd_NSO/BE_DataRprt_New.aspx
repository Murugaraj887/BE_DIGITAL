<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BE_DataRprt_New.aspx.cs" Inherits="BECodeProd.BE_DataRprt_New" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/BEHomeScripts.js" type="text/javascript"></script>
    <link href="Styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/BEHomeScripts.js" type="text/javascript"></script>
    <link href="datepickercss/ui-lightness/jquery-ui-1.8.14.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="datepickerjs/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery.ui.core.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="Calendar_files/common.js" type="text/javascript"></script>
    <script src="Scripts/ol/IMStatus.js" type="text/javascript"></script>
    <script src="Scripts/JQuery.js" type="text/javascript"></script>
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
            font-family: Tahoma;
            font-size: 8pt;
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
            font-family: Tahoma;
            font-size: 8pt;
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
            font-size: 8pt;
        }
    </style>
    <style type="text/css">
        a.tooltip
        {
            outline: none;
        }
        a.tooltip strong
        {
            line-height: 30px;
        }
        a.tooltip:hover
        {
            text-decoration: none;
        }
        a.tooltip span
        {
            z-index: 10;
            display: none;
            padding: 14px 20px;
            margin-top: -30px;
            margin-left: 28px;
            width: 240px;
            line-height: 16px;
        }
        a.tooltip:hover span
        {
            display: inline;
            position: absolute;
            color: #111;
            border: 1px solid #DCA;
            background: #fffAF0;
        }
        .callout
        {
            z-index: 20;
            position: absolute;
            top: 30px;
            border: 0;
            left: -12px;
        }
        
        /*CSS3 extras*/
        a.tooltip span
        {
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            -moz-box-shadow: 5px 5px 8px #CCC;
            -webkit-box-shadow: 5px 5px 8px #CCC;
            box-shadow: 5px 5px 8px #CCC;
        }
        .style4
        {
            width: 183px;
            padding-left:20px;
        }
           .style1
        {
            
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 84px;
            text-align:center;
            height:25px;
            padding-top:10px;
            
        }
         </style>
    <script type="text/javascript">

        //        $(document).ready(function () {

        //            var flagctrl = document.getElementById('hdnfldFlag');
        //            if (flagctrl != null) {
        //                var flag = flagctrl.value;
        //                if (flag == '1') {

        //                    document.getElementById('btnhidden').click(); ;
        //                }

        //            }

        //        });
        var myVar;

        function isvalidupload() {

            var btnReport = document.getElementById('btnreport');
            btnReport.style.visibility = 'hidden';
            document.getElementById('loading').style.visibility = 'visible';
            var a = document.getElementById('lbl').innerHTML;
            var d = $('#dots');
            if (a == "Downloaded") {
                document.getElementById('lbl').innerHTML = 'Downloading';
            }
            (function loading() {

                myVar = setTimeout(function () {

                    draw = d.text().length >= 5 ? d.text('') : d.append('.');
                    loading();
                }, 300);

            })();

        }

        function myStopFunction() {
            clearTimeout(myVar);

        }

        function isvaliduploadClose() {
            var btnReport = document.getElementById('btnreport');
            btnReport.style.visibility = 'visible';

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="sm" runat ="server" AsyncPostBackTimeOut="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
     <table width="100%">
            <tr>
                <td align="left">
                    <%--<table style="border-color: #CCCCCC; background-color: #b4b4b4; border-width: 0"--%>
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1">
                        <tr>
                            <%--<td class="FormControls">
                            </td>--%>

                            <td >                               
                                <div class="style1"> SU:&nbsp;</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:30px">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlSU" runat="server" Font-Names="Tahoma" Font-Size="11px"
                                                 OnSelectedIndexChanged="ddlSU_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                           <%-- <td class="FormLabel">
                                DH:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlDH" runat="server" Font-Names="Tahoma" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlDH_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                            <td>                               
                                <div class="style1">   Current QTR:&nbsp;</div>
                            </td>
                            
                            <td class="FormControls">
                                <table style="width:30px">
                                    <tr>
                                        <td>
                                        <asp:UpdatePanel ID="update" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlQtr" runat="server" Font-Names="Tahoma" Font-Size="11px"
                                                OnSelectedIndexChanged="ddlQtr_SelectedIndexChanged" 
                                                Enabled="False">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td >                                
                                 <div class="style1">   Current Year:&nbsp;</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:30px">
                                    <tr>
                                        <td>
                                          <asp:UpdatePanel ID="GG" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Tahoma" Font-Size="11px"
                                               OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" 
                                                Style="height: 19px" Enabled="False">
                                            </asp:DropDownList>
                                             </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </td>
                            <td style="font-family: Verdana;font-size: 10px" class="style4">
                                &nbsp;</td>
                            <%--<td class="FormLabel">
                                Type:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlfetchingType" runat="server" Font-Names="Tahoma" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlfetchingType_SelectedIndexChanged" Style="height: 19px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                        </tr>
                    </table>
                    <%--<table>
                        <tr>
                            <td>
                            </td>
                            <td class="FormLabel">
                                BE Report:&nbsp;
                            </td>
                            <td class="style1">
                                <asp:RadioButtonList ID="rdbtnlst" runat="server" Font-Names="Tahoma" Font-Size="11px"
                                    Width="233px" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdbtnlst_SelectedIndexChanged">
                                    <asp:ListItem Selected="True">As Of Today</asp:ListItem>
                                    <asp:ListItem>As Of Last Week</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDate" runat="server" Font-Names="Tahoma" 
                                    Font-Size="11px" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                   --%>
                    <%--<table>
                        <tr>
                            <td>
                            </td>
                            <td class="FormLabel">
                                <asp:CheckBox ID="chkbxOnOff" runat="server" Text="Want Onsite/Offshore Split for Volumes in BE Details Sheet?">
                                </asp:CheckBox>
                            </td>
                        </tr>
                    </table>--%>
                    <table width="600px">
                       
                    </table>
                </td>
            </tr>
           <%-- <tr>
                <td align="left">
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" Font-Size="Smaller" ForeColor="Green" runat="server" Font-Names="Verdana"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
        </table>
                               <div style ="width:300px;margin:0px auto">
                    <div style ="float:left;margin-right:10px">
                     <asp:Button ID="btnreport" Text="Generate Report " CssClass="button" runat="server"
                                    OnClick="btnreport_Click" OnClientClick="return isvalidupload()"></asp:Button>
                    </div>
                    

                                      <div id="loading"  runat="server" style="font-size: medium; font-weight: bold;
                                     visibility: hidden; color: #FF0000; font-family: Tahoma;float:left">                                
                                 <asp:Label ID="lbl" runat="server"  Text="Downloading"></asp:Label>   <span style="width: 50px" id="dots"></span>
                                 </div>
                    </div>
    </ContentTemplate>
    
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>  
    <iframe id="iframe" runat ="server" style="display:none"></iframe>
    </ContentTemplate>
     <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnreport" EventName="Click"/>   
    </Triggers>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>