<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BEReports_Sales.aspx.cs" Inherits="BECodeProd.BEReports_Sales" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<meta name="DownloadOptions" content="noopen">--%>

     <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
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

                    document.getElementById('btnhidden').click(); ;
                }

            }

        });

        function alertMsg() {

            alert("Didn't generate the latest reports....");

        }

    </script>

      <script src="Select2/JScriptSelect2.js" type="text/javascript"></script>
    <link href="Select2/select2.css" rel="stylesheet" type="text/css" />
   
    <script type="text/javascript">

        $(document).ready(function () {
            $("#ddlRegion").select2({
                selectOnClose: true
            }
            )
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

 select::-ms-expand {
    display: none;
        
    </style>
    <script type="text/javascript">

        var myVar;
        function isvalidupload() {
            var btnreport = document.getElementById('btnGenerate');
            btnreport.style.visibility = 'hidden';

            var btnreport1 = document.getElementById('btnDownload');
            btnreport1.style.visibility = 'hidden';

            var btnreport2 = document.getElementById('btnDownloadBE');
            btnreport2.style.visibility = 'hidden';

            document.getElementById('loading').style.visibility = 'visible';
            var a = document.getElementById('lbl').innerHTML;
            var d = $('#dots');
            if (a == "Generated" || a == "") {
                document.getElementById('lbl').innerHTML = 'Generating';
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
            var btnreport = document.getElementById('btnGenerate');
            btnreport.style.visibility = 'visible';

            var btnreport1 = document.getElementById('btnDownload');
            btnreport1.style.visibility = 'visible';

            var btnreport2 = document.getElementById('btnDownloadBE');
            btnreport2.style.visibility = 'visible';
        }

        
    </script>
</head>
<body>
    <form id="form1" runat="server">

        
   <asp:ScriptManager ID="sm" runat ="server" AsyncPostBackTimeOut="1000">
    </asp:ScriptManager>
     <div><asp:Label ID="lblasondate" runat="server" Text="Pipeline reports generated as on:    " ForeColor="Red" Font-Names="calibri" ></asp:Label></div>
     <div><asp:Label ID="lblBE" runat="server" Text="BE reports generated as on:    " ForeColor="Red" Font-Names="calibri" ></asp:Label></div>
    <div style="float:left">
    
    
     <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
    <ContentTemplate>   
    
    <div >
    <table>
    <tr>
    <td>
    <div style ="width:50px;margin:0px auto">
    
    </td>
    
    <td>
   
    <div style="float:left;margin-top:10px;margin-right:20px"> <span style="font-family:Calibri;font-size:medium;padding-top:5px">Region :</span> <asp:DropDownList style="font-family:Calibri;font-size:small" runat="server" ID="ddlRegion" Font-Size="11px" Font-Names="Calibri" Width="150px">
    <asp:ListItem Text="AMERICAS" Selected="True">AMERICAS</asp:ListItem>
    
    </asp:DropDownList></div>
                   <div style="float:left;margin-right:10px;margin-top:7px">

                    

                                <asp:Button ID="btnGenerate" Text="Generate BE" Width="160" CssClass="btn btn-success" runat="server"
                                    OnClick="btnBEreportSales_Click" OnClientClick="return isvalidupload()"></asp:Button> 

                                </div>     

                                  <div style="float:left;margin-right:10px;margin-top:7px">

                                <asp:Button ID="btnDownloadBE" Text="Download BE" Width="160" CssClass="btn btn-success" runat="server"
                                    OnClick="btn_BE_SalesDownload_Click" ></asp:Button> 

                                </div>     

                                 <div id="loading" align="left" runat="server" style="font-size: medium; font-weight: bold;
                                     visibility: hidden; color: #FF0000; font-family: Calibri;margin-top:10px">                                
                                 <asp:Label ID="lbl" runat="server"  Text="Generating"></asp:Label>   <span style="width: 50px" id="dots"></span>
                                 </div>
                                </td>
                                <td>
                            
                                            </td>
                                       </tr>
                                       <tr>                                             
                              
                                 </tr>

                                 <tr>
                                  <td style="text-align: center; height: 100%;" align="center">
                                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="600px"
                                            align="center" DocumentMapCollapsed="True" InteractivityPostBackMode="AlwaysSynchronous"
                                            ProcessingMode="Remote" ShowExportControls="False" SizeToReportContent="True"
                                            InternalBorderStyle="None" InternalBorderWidth="10px" ToolBarItemBorderStyle="None"
                                            ToolBarItemBorderWidth="" ShowParameterPrompts="False" />
                                    </td>
                                 </tr>
                           </table>
                           <iframe id="iframe" runat ="server" style="display:none"></iframe>
    </div>
     </ContentTemplate>      
  <Triggers>
    <asp:PostBackTrigger ControlID="btnGenerate"  />   
    <asp:PostBackTrigger ControlID="btnDownloadBE" />
    </Triggers>
   </asp:UpdatePanel>

    
    </div>
    <div style="float:left;padding-left:100px;margin-top:7px">
                                 <asp:Button ID="btnDownload" Text="Download BE + Pipeline Sales" Width="220" CssClass="btn btn-success" runat="server"
                                    OnClick="btn_BE_Pipeline_SalesDownload_Click" ></asp:Button> 
                                    </div> 
    </form>
</body>
</html>
