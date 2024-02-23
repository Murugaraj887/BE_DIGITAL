<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PBS_RRHmismatchreport.aspx.cs" Inherits="BECodeProd.PBS_RRHmismatchreport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

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
    </style>
     <script type="text/javascript">
         var myVar;
         function isvalidupload() {
             var btnNewProjectList = document.getElementById('btnNewProjectList');
             btnNewProjectList.style.visibility = 'hidden';
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
             clearTimeout(myVar);

         }

         function isvaliduploadClose() {
             var btnNewProjectList = document.getElementById('btnNewProjectList');
             btnNewProjectList.style.visibility = 'visible';

         }
    </script>

</head>
<body>
    <form id="form1" runat="server">
          
      <asp:ScriptManager ID="sm" runat ="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
    <ContentTemplate> <div align="center">
       <table class="style2">
           <tr>
           

           <td class="FormLabel">
               <asp:Label ID="lblServiceLine" runat="server" Text="Select ServiceLine: "></asp:Label></td>
           <td >
               <asp:DropDownList ID="ddlServiceLine" AppendDataBoundItems="true"  runat="server" 
              
              ><asp:ListItem Value="All">All</asp:ListItem>
               </asp:DropDownList>
           </td>
          
           </tr>

           <tr>
              <td colspan="3" align="left">
               

              </td>

           </tr>
           
            </table> </div>
            <div style="float:left;padding-left:80px;margin-right:10px">  
             <asp:Button ID="btnNewProjectList" runat="server" Text="Download To Excel" 
                        OnClick="btnNewProjectList_Click" OnClientClick="return isvalidupload()" CssClass="btn btn-success"
                         /></div>
          
             
              <div id="loading" align="left" runat="server" style="font-size: medium; font-weight: bold;
                                     visibility: hidden; color: #FF0000; font-family: Tahoma;float:left">
                                 <asp:Label ID="lbl" runat="server" Text="Downloading"></asp:Label>   <span style="width: 50px" id="dots"></span></div>
                                  
                                  <iframe id="iframe" runat ="server" style="display:none"></iframe>
               <asp:Label ID="lblError" runat="server" 
                   Text="No data available for this selection" Visible="False" ForeColor="Red"></asp:Label>
    
   </ContentTemplate>
    <Triggers>
    <asp:PostBackTrigger ControlID="btnNewProjectList" />
    </Triggers>
    </asp:UpdatePanel>
   
    </form>
</body>
</html>
