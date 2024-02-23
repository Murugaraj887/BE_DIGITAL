﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportClientCategorization.aspx.cs"
    Inherits="BECodeProd.ReportClientCategorization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

     <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


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
    <script type="text/javascript">
        $(document).ready(function () {

            var flagctrl = document.getElementById('hdnfldFlag');
            if (flagctrl != null) {
                var flag = flagctrl.value;
                if (flag == '1') {

                    document.getElementById('btnhidden').click();;
                }

            }

        });

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
            width:150px;
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
            padding-top:2px;
            
        }
        .TextBox
        {
            font-family: verdana;
            font-size: 9pt;
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
        
         .form-control {
     display: block;
    width: 100%;
     background: url('Images/down.png') no-repeat right;
    padding: 5px 5px 5px 5px;
    font-size: 10px;
    line-height: 1.4285;
    color: #555;
    
   
    border: 1px solid #ccc;
    border-radius: 4px;
    -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
    box-shadow: inset 0px 1px 1px rgba(0,0,0,0.075);
    -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
    -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
    transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
}
          .form-control1 {
     display: block;
    width: 100%;
   
    font-size: 10px;
    line-height: 1.4285;
    color: #555;
    
   
    border: 1px solid #ccc;
    border-radius: 4px;
    -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
    box-shadow: inset 0px 1px 1px rgba(0,0,0,0.075);
    -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
    -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
    transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
}

 select::-ms-expand {
    display: none;
    </style>
    <script type="text/javascript">

        var myVar;
        function isvalidupload() {
            var btnreport = document.getElementById('btnreport');
            btnreport.style.visibility = 'hidden';
            document.getElementById('loading').style.visibility = 'visible';
            var a = document.getElementById('lbl').innerHTML;
            var d = $('#dots');
            if (a == "Downloaded" || a == "") {
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
            //debugger;
            clearTimeout(myVar);
        }

        function Message() {
            alert("No data found");
        }

        function isvaliduploadClose() {
            //debugger;
            var btnreport = document.getElementById('btnreport');
            btnreport.style.visibility = 'visible';
        }
    </script>
</head>
<body>
   <form id="form1" runat="server">
    
        <asp:ScriptManager ID="sm" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div align="center" style="margin-top:5px;padding-left:100px">
            <div style=" margin-left:5px; margin-right:50px">
        
        <div style="margin: 3px; float: left;" class="FormControls">
                            ServiceLine: </div>
            <div style="float:left">
                                        <asp:DropDownList ID="ddlServiceLine" runat="server" Font-Names="Calibri" Font-Size="11px"
                                            AutoPostBack="True" style="margin-left:5px;float:left"
                                            Height="25" class="form-control" width="78">
                                        </asp:DropDownList></div>
                   <div style="margin: 3px 3px 3px 10px; float: left;" class="FormControls">             
                           Fin Year: </div>
                    <div style="float:left">    
                                        <asp:DropDownList ID="ddFinYr" runat="server" Font-Names="Calibri" Font-Size="11px"
                                            AutoPostBack="True" style="margin-left:5px"
                                            Height="25" class="form-control" width="78" OnSelectedIndexChanged="ddlDuration_SelectedIndexChanged">
                                        </asp:DropDownList></div>
                   <div style="margin: 3px 3px 3px 10px; float: left;" class="FormControls">                
                      
                            Duration: </div>
                     <div style="float:left">
                                        <asp:DropDownList ID="ddlDuration" runat="server" Font-Names="Calibri" Font-Size="11px"
                                            AutoPostBack="True" style="margin-left:5px"
                                            Height="25" class="form-control" width="78" OnSelectedIndexChanged="ddlDuration_SelectedIndexChanged">
                                            <asp:ListItem>LTM</asp:ListItem>
                                            <asp:ListItem>YTD</asp:ListItem>
                                            <asp:ListItem>QTD</asp:ListItem>
                                            <asp:ListItem>Quarter</asp:ListItem>
                                        </asp:DropDownList></div>
                          <div style="margin: 3px 3px 3px 10px; float: left;" class="FormControls">         
                            Quarter: </div>
                       <div style="float:left">
                                        <asp:DropDownList ID="ddlQuarter" runat="server" Font-Names="Calibri" Font-Size="11px"
                                            AutoPostBack="True" style="margin-left:5px" 
                                            Height="25" class="form-control" width="78" Enabled="false">
                                            <asp:ListItem Value="NA">NA</asp:ListItem>
                                        </asp:DropDownList></div>
                             <div style="margin: 3px 3px 3px 5px; float: left;" title="Accounts with revenue above 0.05 Mn for the selected period are only considered in this report" 
                                 class="FormControls">     
                            &nbsp; Minimum Cut-off for the Revenue Slab is 0.05 Mn </div>
               </div>
                </div>
                <div style="width:100px;margin:0px auto">
                     <div style ="float:left;margin-right:10px;padding-top:10px">
                                <asp:Button ID="btnreport" style="margin-top:15px;" Text="Generate Report " CssClass="btn btn-success" runat="server"
                                    OnClick="btnreport_Click" OnClientClick="return isvalidupload()"></asp:Button>
                        </div>    
                                <div id="loading" align="left" runat="server" style="font-size: medium; font-weight: bold;
                                    visibility: hidden; color: #FF0000; font-family: Calibri;">
                                    <asp:Label ID="lbl" runat="server" Text="Downloading"></asp:Label>
                                    <span style="width: 50px" id="dots"></span>
                                </div>
                            
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <iframe id="iframe" runat="server" style="display: none"></iframe>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnreport" />
            </Triggers>
        </asp:UpdatePanel>
   
    </form>
</body>
</html>