<%@ Page Language="C#" AutoEventWireup="true"
    Title="RTBR" Inherits="ReportRTBRFinpulse" Codebehind="ReportRTBRFinpulse.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <script src="Scripts/bootstrap-select.js" type="text/javascript"></script>
    <link href="Styles/css/bootstrap-select.css" rel="stylesheet" type="text/css" />


    
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
   table.mylist input {
      
    margin-right:3px;
      float: left;
   }
   table.mylist label {
     
      margin-top:3px;
      float: left;
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
            /*border-radius: 4px;*/
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            -moz-box-shadow: 5px 5px 8px #CCC;
            -webkit-box-shadow: 5px 5px 8px #CCC;
            /*box-shadow: 5px 5px 8px #CCC;*/
        }
        .style1
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
         .style2
        {
           
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 150px;
            text-align:center;
            height:25px;
            padding-top:5px;
            
        }
         .sty2
        {
            font-family: Verdana;
            color: #000000;
            font-size: 10px;
            font-weight: normal;
            width: 100px;
            text-align:center;
            height:25px;
            padding-top:5px;
            padding-left:10px;
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

        //        function isvalidupload() {
        //            //debugger;
        //            var btnReport = document.getElementById('btnreport');
        //            btnReport.style.visibility = 'hidden';
        //            document.getElementById('loading').style.visibility = 'visible';
        //            var d = $('#dots');
        //            (function loading() {
        //                setTimeout(function () {
        //                    draw = d.text().length >= 5 ? d.text('') : d.append('.');
        //                    loading();
        //                }, 300);
        //            })();
        //        }

        //       function isvaliduploadClose (){
        //            //debugger;
        //            var btnReport = document.getElementById('btnreport');
        //            btnReport.style.visibility = 'visible';
        //            document.getElementById('loading').style.visibility = 'hidden';
        //            alert('Hi');
        //        }


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
    <%--<script type="text/javascript">

        $(document).ready(function () {

            $(".selectpicker").selectpicker();

        });

</script>--%>
</head>
<body>
    <form id="form1" runat="server" >
    
     <asp:ScriptManager ID="sm" runat ="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
    <ContentTemplate>      
    <div align="center" >
        <table style="width:950px">
            <tr>
                <td align="center">
                    <%--<table style="border-color: #CCCCCC; background-color: #b4b4b4; border-width: 0"--%>
                    <asp:Label ID="lbldisplayRTBR" runat="server" Font-Names="Calibri" Font-Size="11px"
                        ForeColor="Red" Font-Bold="true" Text="RTBR last modified on : "></asp:Label>
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1" id="tblRTBR" runat="server">
                        <tr>
                            <td >
                               <div class="style1"> Practice:</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:15px">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlSU" runat="server" Font-Names="Calibri" Font-Size="11px" Width="100" Height="25"
                                                OnSelectedIndexChanged="ddlSU_SelectedIndexChanged" AutoPostBack="true" class="form-control"
                                                style="margin-left:5px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                           <%-- <div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">Dropdown Example
  <span class="caret"></span></button>
  <ul class="dropdown-menu">
    <li><a href="#">HTML</a></li>
    <li><a href="#">CSS</a></li>
    <li><a href="#">JavaScript</a></li>
  </ul>
</div>--%>


                            <td>
                             <div class="sty2">  Offering:</div>                              
                            </td>

                            <td class="FormControls">
                                <table style="width:20px">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlNSO" runat="server" Font-Names="Calibri" Font-Size="11px" Width="70" Height="25"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlNSO_SelectedIndexChanged" class="form-control"
                                                style="margin-left:5px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                             </td> 
                                                      
                            <td >                              
                                 <div class="style2">  Master Client Code:</div>
                            </td>
                            <td class="FormControls">
                                <table >
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlMCC" runat="server" Font-Names="Calibri" Font-Size="11px" 
                                            width="160" Height="25" class="form-control" style="margin-left:5px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td >
                                
                                <div class="style1">  Financial Year:&nbsp;</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:50px">
                                    <tr>
                                        <td>
                                            <%-- <asp:DropDownList ID = "ddlyear" runat = "server"></asp:DropDownList>--%>
                                            <asp:RadioButtonList ID="rdlYear" runat="server" CssClass="mylist">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                             
                                 <div class="style1">    Currency:&nbsp;</div>
                            </td>
                            <td class="FormControls">
                                <table style="width:100px" align="left">
                                    <tr align="left" >
                                        <td>
                                            <%--<asp:RadioButtonList ID="rdlRTBR" runat="server" Width="129px" RepeatDirection="Vertical" CssClass="mylist">
                                                <asp:ListItem Selected="True" Text="USD" Value="USD"></asp:ListItem>
                                                <asp:ListItem Text="Native Currency" Value="NC"></asp:ListItem>
                                                <asp:ListItem Text="Both" Value="Both"></asp:ListItem>--%>
                                            <asp:RadioButtonList ID="rdlRTBR" runat="server" Width="140px" RepeatDirection="Vertical" CssClass="mylist">
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
            
                      </td>
                      </tr>

                      </table>
                </div>     

                       

                               
                                 
                    <div style ="width:130px;margin:0px auto">
                    <div style ="margin-right:10px" >
                     <asp:Button ID="btnreport" Text="Generate Report "  runat="server" CssClass="btn btn-success"
                     OnClick="btnreport_Click" OnClientClick="return isvalidupload()" >
                     </asp:Button>
                     <%--<asp:LinkButton ID="btnreport" Text="Generate Report "  runat="server" CssClass="btn btn-success"
                     OnClick="btnreport_Click" OnClientClick="return isvalidupload()">
                     <span class="glyphicon glyphicon-print"></span>   
                     </asp:LinkButton>--%>
                    </div>
                         <div id="loading"  runat="server" style="font-size: medium; font-weight: bold;
                                     visibility: hidden; color: #FF0000; font-family: Calibri;width:600px;margin-left:15px">                                
                                 <asp:Label ID="lbl" runat="server"  Text="Downloading"></asp:Label>   <span style="width: 50px" id="dots"></span>
                                 </div>
                                
                    </div>

                       
                   <%-- <table>
                    <tr>
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
                    </tr>
                    </table>--%>
        
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
