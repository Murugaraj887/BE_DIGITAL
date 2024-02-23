<%@ Page Language="C#" Title="Revenue Momentum" AutoEventWireup="true" CodeBehind="RevenueMomentum.aspx.cs"
    Inherits="RevenueMomentum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<meta name="DownloadOptions" content="noopen">--%>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <link href="Styles/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/bootstrap.min.js" type="text/javascript"></script>
    <link href="Styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/BEHomeScripts.js" type="text/javascript"></script>
    <link href="Scripts/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            LoadMulti();
        });

        function pageLoad() {
            LoadMulti();
        }

        function LoadMulti() {
            console.log(new Date().toDateString());
            var sl = document.getElementById('hdnSL').value;
            if (sl != '' || sl != undefined) {
                var items = sl.split(',');
                var ddlSL = document.getElementById('ddlSL');
                for (var i = 0; i < ddlSL.options.length; i++) {
                    if (items.indexOf(ddlSL.options[i].text) != -1)
                        ddlSL.options[i].selected = true;
                }
            }



            $('#ddlSL').multiselect({
                includeSelectAllOption: true,
                allSelectedText: "All",
                nonSelectedText: 'Select'
            });

            $('button').addClass('btn-xs')
        }


    </script>
    <script type="text/javascript">


        $(document).ready(function () {

            var flagctrl = document.getElementById('hdnfldFlag');
            if (flagctrl != null) {
                var flag = flagctrl.value;
                if (flag == '1') {

                    document.getElementById('btnhidden').click(); ;
                }

            }

        }); 

       

    </script>
    <style type="text/css">
        button
        {
            background-color: White;
            border: 1px solid lightgray;
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
        .FormLabel
        {
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
        
        button span
        {
            font-size: 10px;
        }
        
        ul li
        {
            font-size: 10px;
        }
        
        button1
        {
            height: 25px;
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
        
        .form-control
        {
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
        
        select::-ms-expand
        {
            display: none;</style>
    <script type="text/javascript">

        var myVar;
        function isvalidupload() {
         var hdnSL  = document.getElementById('hdnSL') ;
         var ddlSL = document.getElementById('ddlSL');
         var items = [];
            for (var i = 0; i < ddlSL.options.length; i++)
                if (ddlSL.options[i].selected) {
                    items.push(ddlSL.options[i].text);
                }
                var csv = items.join(',');
                hdnSL.value = csv;






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

        function isvaliduploadClose() {
            //debugger;
            var btnreport = document.getElementById('btnreport');
            btnreport.style.visibility = 'visible';
        }

       
    </script>
</head>
<body>
    <form id="form1" runat="server" width="400px">
    <asp:ScriptManager ID="sm" runat="server" AsyncPostBackTimeout="1000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div align="center" style="margin-top: 5px">
                <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                    cellspacing="1">
                    <tr>
                        <td class="FormControls">
                        </td>
                        <td class="FormLabel">
                            Service Line:
                        </td>
                        <td class="FormControls">
                            <table width="100%">
                                <tr>
                                    <td>
                                      
                                        <div style="width:80px;">
                                        <asp:ListBox ID="ddlSL" runat="server" Font-Names="Calibri" Font-Size="9px" AutoPostBack="false"
                                            Style="margin-left: 5px" Height="25" Width="70" SelectionMode="Multiple"></asp:ListBox>
                                            </div>
                                      
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="FormLabel" style="padding-left: 5px">
                            Quarter:
                        </td>
                        <td class="FormControls">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlQuarter" runat="server" Font-Names="Calibri" Font-Size="11px"
                                            AutoPostBack="false" Style="margin-left: 5px" Height="23" class="form-control"
                                            Width="70">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="FormLabel" style="padding-left: 5px">
                            BE FrozenDate:
                        </td>
                        <td class="FormControls">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlBEWeeKDate" runat="server" Font-Names="Calibri" Font-Size="11px"
                                            AutoPostBack="True" Style="margin-left: 5px" Height="23" class="form-control"
                                            Width="150">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 130px; margin: 0px auto; padding-top: 10px">
                <div style="float: left; margin-right: 10px">
                    <asp:Button ID="btnreport" Text="Generate Report " CssClass="btn btn-success" runat="server"
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
    <asp:HiddenField ID="hdnSL" runat="server" Value="" />
    </form>
</body>
</html>
