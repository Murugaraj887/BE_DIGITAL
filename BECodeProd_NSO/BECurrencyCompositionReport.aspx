<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BECurrencyCompositionReport.aspx.cs" Inherits="BECodeProd.BECurrencyCompositionReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
         .style1
        {
           
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 50px;
            text-align:center;
            height:25px;
            padding-top:5px;
            
        }
         .style2
        {
           
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 100px;
            text-align:center;
            height:25px;
            padding-top:5px;
            
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
         .style1
        {
            
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 70px;
            text-align:center;
            height:25px;
            padding-top:5px;
            
        }
         .style2
        {
           
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 130px;
            text-align:center;
            height:25px;
            padding-top:5px;
            
        }
    </style>
    <script type="text/javascript">




        var myVar;
        function isvalidupload() {
            //debugger;
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
    <form id="form2" runat="server">
    
    <asp:ScriptManager ID="sm" runat ="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div align="center" style="margin-top:5px">
        <table width="100%">
            <tr>
                <td align="center">
                    
                    
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1";>
                        <tr>
                            <td class="FormControls">
                            </td>
                            <td >
                               <div class="style1" style="margin-left:5px"> Practice:</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:30px">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlSU" runat="server" Font-Names="Calibri" Font-Size="11px" class="form-control" Width="78" Height="25"
                                                 style="margin-left:5px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                           <td>
                             <div class="style1" style="margin-left:5px">  BE-Qtr:</div>                              
                            </td>

                            <td class="FormControls">
                                <table style="width:20px">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlBEQtr" runat="server" Font-Names="Calibri" Font-Size="11px" style="margin-left:5px"
                                                   class="form-control" Width="70" Height="25">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                             </td> 
                            <td >
                                
                                 <div class="style2" style="margin-left:5px">  Actuals-Qtr:</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:50px">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlActualsQtr" runat="server" Font-Names="Calibri" Font-Size="11px" 
                                            class="form-control" Width="78" Height="25" style="margin-left:5px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>                                          
                    </table>
                </td>
            </tr>
           
        </table>

        <table >
                            <tr>
                            <td>
                            <span class="FormLabel">
                               Variance Range (BE - Actuals) (%):
                               </span>
                            </td>
                            <td class="FormControls" style="padding-left:5px;padding-right:5px">Above</td>
                            <td class="FormControls">
                              
                              <div style="margin-top:12px">
                              <asp:TextBox ID="txtAbove" runat="server" Width="25" style="margin-top:5px" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label Id="txtAbove_BoundControl" runat="server"></asp:Label>
                              </div>
                                            
                                       
                            </td>
                            <td class="FormControls" style="padding-left:5px;padding-right:5px">  Below </td>
                            <td class="FormControls">
                                <div style="margin-top:12px">
                                            <asp:TextBox ID="txtBelow" runat="server" Width="25" style="margin-top:5px" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label Id="txtBelow_BoundControl" runat="server"></asp:Label>
                                       
                                    </div>
                                </td>
                            </tr>                 
</table>

 </div>



                         <div style ="width:120px;margin:0px auto">
                                <div style ="float:left;margin-right:10px;margin-top:5px">
                                <asp:Button ID="btnreport" Text="Generate Report " CssClass="btn btn-success" runat="server"
                                    OnClick="btnreport_Click" OnClientClick="return isvalidupload()"></asp:Button>
                                </div>
                    

                                      <div id="loading"  runat="server" style="font-size: medium; font-weight: bold;
                                     visibility: hidden; color: #FF0000; font-family: Calibri;float:left;margin-right:17px">                                
                                 <asp:Label ID="lbl" runat="server"  Text="Downloading"></asp:Label>   <span style="width: 50px" id="dots"></span>
                                 </div>
                    </div>
                  

<div style="clear:both" align="center">
  <span style="font-size:smaller;color:Red;">Note : Ranges are inclusive for both Above and Below. Default value of Zero shall provide the complete report</span></div>
                   <asp:SliderExtender ID="SliderExtender2" runat="server"
         behaviorId="txtAbove"  
    TargetControlID="txtAbove"
    BoundControlID="txtAbove_BoundControl"
    Orientation="Horizontal"
    EnableHandleAnimation="true" 
    maximum="30"
    minimum="0"/>

    <asp:SliderExtender ID="SliderExtender1" runat="server"
         behaviorId="txtBelow"
    TargetControlID="txtBelow"
    BoundControlID="txtBelow_BoundControl"
    Orientation="Horizontal"
    EnableHandleAnimation="true" 
    minimum="-30"
    maximum="0"/>


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
   
    </form>
</body>


</html>
