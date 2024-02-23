<%@ Page Title="Digital BE DATA - SDM" Language="C#" MasterPageFile="~/Site.Master" 
    EnableViewState="true" AutoEventWireup="true" codebehind="SDMView.aspx.cs" Inherits="SDMView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="DownloadOptions" content="noopen">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
    <script src="Scripts/JQuery.js" type="text/javascript"></script>
     <style type="text/css">
         
           .modalBackground {
    background-color: Black!important;
    filter: alpha(opacity=90)!important;
    opacity: 0.8!important;
}
          .progress
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        
        .progress1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
            display:none;
        }
        .center
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
            
        }
       .borderempty
        {
            border:0px!important;
            font-size:9pt!important;
            font-family:Calibri!important;
            color:Gray!important;
            text-align:right!important;
        }
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
    select::-ms-expand {
    display: none;
}

 
</style>


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
        .DisplayNone
        {
            display: none;
        }
        
           #MainContent_grdBESDMViewHorizontalRail
        {
            display:none !important;
       
        }
        #MainContent_grdBESDMViewHorizontalBar
        {
            display:none !important;
        }
        
           .wid
        {
        	width:100px !important;
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
            border: 1px solid red;
            background-color: #f8da92;
            padding: 1px 0px;
            cursor: pointer;
            cursor: hand;
            font-family: Calibri;
            font-size: 9pt;
            text-align: center;
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
            font-family:Calibri;
            font-size: 9pt;
            text-align: right;
            color:Black;
        }
        
        .footerBox
        {
            font-family: Calibri;
            font-size: 9pt;
            text-align: right;
            background-color:rgb(51, 51, 51);
            color:floralwhite;
        }
         .footerBox1
        {
            font-family: Calibri;
            font-size: 9pt;
            text-align: right;
            color:Gray;
        }
        .Label
        {
            font-family: Calibri;
            font-size: 9pt;
            background: none;
            border: 0;
        }
    </style>
   <script type="text/javascript">

       function validateUpload() {
           var value = document.getElementById('<%= fuUploader.ClientID %>').value;

           var filename = value.replace(/^.*[\\\/]/, '');


           var agree = confirm("The file to be uploaded is " + filename + ". Ensure to review the data before upload as the data uploaded will overwrite the latest values in the BE portal. Do you want to continue?")
           if (agree == true) {
               var flag = value != '';
               if (!flag) {
                   alert('Please select a file to upload');
               }
               else {
                   document.getElementById('MainContent_btnCancel').style.display = "none";
                   document.getElementById('MainContent_btnUpload').style.display = "none";
                   document.getElementById('MainContent_fuUploader').style.display = "none";
                   setTimeout(function () {
                       document.getElementById('ldimg').style.display = "block";

                   }, 50);


               }
               return flag;
           }
           else {
               document.getElementById('MainContent_btnCancel').click();
               return false;
           }
       }

       function validateFile() {
           var value = document.getElementById('<%= fuUploader.ClientID %>').value;
           var flag = value != '';
           if (!flag) {
               alert('Please select a file to upload');
               document.getElementById("MainContent_btnUpload").style.display = 'none';
           }
           else {
               document.getElementById("MainContent_btnUpload").style.display = 'inline-block';
           }
           return flag;
       }
       //        $(function () {

       //            var prefix = '#MainContent_grdBESDMView_';
       //            var borderredcss = '1px solid red';
       //            var bordergraycss = '1px solid gray';
       //            var html = $('#MainContent_hiddenMsg').val();
       //            var rowInfo = $('#MainContent_hiddenMsgIndex').val() + '';

       //            if (rowInfo != "") {
       //                var rows = rowInfo.split('|');
       //                for (var i = 0; i < rows.length; i++) {
       //                    debugger;
       //                    var row = rows[i] + '';
       //                    var temp = row.split('-');
       //                    var index = temp[0];
       //                    var values = temp[1].split(',');

       //                    var bordercss = borderredcss;
       //                    var left = (parseFloat(values[0]) + parseFloat(values[1]) + parseFloat(values[2]));
       //                    var right = (parseFloat(values[3]) + parseFloat(values[4]) + parseFloat(values[5]) + parseFloat(values[6]));
       //                    var flag = left.toFixed(1) == right.toFixed(1);
       //                    bordercss = flag ? bordergraycss : borderredcss;



       //                    $(prefix + 'txtDMMonth1_' + index).val(values[0]).css('border', bordercss);
       //                    $(prefix + 'txtDMMonth2_' + index).val(values[1]).css('border', bordercss);
       //                    $(prefix + 'txtDMMonth3_' + index).val(values[2]).css('border', bordercss);

       //                    $(prefix + 'txtBKMonth1_' + index).val(values[3]).css('border', bordercss);
       //                    $(prefix + 'txtBKMonth2_' + index).val(values[4]).css('border', bordercss);
       //                    $(prefix + 'txtBKMonth3_' + index).val(values[5]).css('border', bordercss);
       //                    $(prefix + 'txtBKMonth4_' + index).val(values[6]).css('border', bordercss);






       //                    $(prefix + 'txtVolOnMonth1_' + index).val(values[7]);
       //                    $(prefix + 'txtVolOffMonth1_' + index).val(values[8]);

       //                    $(prefix + 'txtVolOnMonth2_' + index).val(values[9]);
       //                    $(prefix + 'txtVolOffMonth2_' + index).val(values[10]);

       //                    $(prefix + 'txtVolOnMonth3_' + index).val(values[11]);
       //                    $(prefix + 'txtVolOffMonth3_' + index).val(values[12]);

       //                    $(prefix + 'txtVolsdmRemarks_' + index).val(values[13]);

       //                }
       //                $('#MainContent_lblmsg').html(html);
       //                $('#MainContent_hiddenMsg').val('')

       //            }

       //            CalculateOnLoadFooterTotal();
       //        });

       function PopUpBcktC(rowindex) {
           //debugger;
           var left = (screen.width - 700) / 2;
           var top = $(document).scrollTop();
           var i, CellValue, Row, MCC, NC, PU, Qtr, Year, qtryr, yr, yr1;
           var no = rowindex.id.split('_')[3];
           i = parseInt(no) + 2;
           var hndfMcc = 'MainContent_grdBESDMView_hdnfmcc_' + no;
           var hndfldname = 'MainContent_grdBESDMView_hdnfld_' + no;
           var hndme = 'MainContent_grdBESDMView_hdnd_' + no;

           var table = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           var BEID = document.getElementById(hndfldname).value;
           var MCC = document.getElementById(hndfMcc).value;
           var RF = document.getElementById(hndme).value;
           winpopupstatus = window.open('BucketCInfoSDM.aspx?ID=' + BEID + '&Type=C', 'pop', 'left = ' + left + ',width=800, height=450 , menubar=no, scrollbars=yes, resizable=no');
           if (!winpopupstatus.closed) {
               winpopupstatus.focus();

           }
           //           else if (winpopupstatus.closed) {
           //               window.location.reload();
           //           }
           CalculateOnLoadFooterTotal();
           makeTextBoxRed();
           PressNumberOnlyAndCalcBK(null);
           return false;
       }

       function PopUpBcktD(rowindex) {
           //debugger;
           var left = (screen.width - 700) / 2;
           var top = $(document).scrollTop();
           var i, CellValue, Row, MCC, NC, PU, Qtr, Year, qtryr, yr, yr1;
           var no = rowindex.id.split('_')[3];
           i = parseInt(no) + 2;
           var hndfMcc = 'MainContent_grdBESDMView_hdnfmcc_' + no;
           var hndfldname = 'MainContent_grdBESDMView_hdnfld_' + no;
           var hndme = 'MainContent_grdBESDMView_hdnd_' + no;

           var table = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           var BEID = document.getElementById(hndfldname).value;
           var MCC = document.getElementById(hndfMcc).value;
           var RF = document.getElementById(hndme).value;
           winpopupstatus = window.open('BucketDInfoSDM.aspx?ID=' + BEID + '&Type=D', 'pop', 'left = ' + left + ',width=800, height=450 , menubar=no, scrollbars=yes, resizable=no');
           if (!winpopupstatus.closed)
           { winpopupstatus.focus(); }
           CalculateOnLoadFooterTotal();
           makeTextBoxRed();
           PressNumberOnlyAndCalcBK(null);
           return false;
       }


       function loaddata(thisobj) {
         
           
         var btnsave2 = document.getElementById('MainContent_btnSave2');
           if (thisobj.value == '') { thisobj.value = '0.0'; return true; }
           var currenttext = thisobj.value;
           var isEror = parseFloat(currenttext) + '' == 'NaN';
           if (isEror) {

               alert('Please enter a valid number'); thisobj.focus();
           }
           else {
               var value = thisobj.value;
               value = parseFloat(value);
              
               var decimalplace = (thisobj.value.split('.')[1] || []).length;
               var strValue = thisobj.value + '';
               if (decimalplace == 1) {

                   value = value.toFixed(1);
               }
               if (strValue.length > 9 || decimalplace > 1 || value < 0) {


                   alert('Please enter a positive value less than 6 digits with 1 decimal values'); thisobj.focus();
                   return;
               }

           }
               var SDMMonth1ONtotal = 0.0;
               var SDMMonth2ONtotal = 0.0;
               var SDMMonth3ONtotal = 0.0;
               var SDMMonth1OFFtotal = 0.0;
               var SDMMonth2OFFtotal = 0.0;
               var SDMMonth3OFFtotal = 0.0;
               var totalOnsite = 0.0;
               var totalOFFsite = 0.0;
               var totalGrandtotal = 0.0;

           var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           for (i = 0; i < grid.rows.length - 4; i++) {



               var month1valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth1_' + i).value;
               var month2valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth2_' + i).value;
               var month3valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth3_' + i).value;

               var month1valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth1_' + i).value;
               var month2valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth2_' + i).value;
               var month3valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth3_' + i).value;

               month1valueON = month1valueON == "" ? 0 : month1valueON;
               month2valueON = month2valueON == "" ? 0 : month2valueON;
               month3valueON = month3valueON == "" ? 0 : month3valueON;

               month1valueOFF = month1valueOFF == "" ? 0 : month1valueOFF;
               month2valueOFF = month2valueOFF == "" ? 0 : month2valueOFF;
               month3valueOFF = month3valueOFF == "" ? 0 : month3valueOFF;


               var totalON = parseFloat(month1valueON) + parseFloat(month2valueON) + parseFloat(month3valueON);
               var totalOFF = parseFloat(month1valueOFF) + parseFloat(month2valueOFF) + parseFloat(month3valueOFF);
               var grandTotal = totalON + totalOFF;

               document.getElementById('MainContent_grdBESDMView_lblTotOn_' + i ).value = totalON.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotOff_' + i ).value = totalOFF.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotVol_' + i ).value = grandTotal.toFixed(1);

               document.getElementById('MainContent_grdBESDMView_lblTotOn_' + i).title = totalON.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotOff_' + i).title = totalOFF.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotVol_' + i).title = grandTotal.toFixed(1);
//               grid.rows[i + 3].cells[20].innerText = totalON.toFixed(1);
//               grid.rows[i + 3].cells[21].innerText = totalOFF.toFixed(1);
//               grid.rows[i + 3].cells[22].innerText = grandTotal.toFixed(1);


               SDMMonth1ONtotal += parseFloat(month1valueON);
               SDMMonth2ONtotal += parseFloat(month2valueON);
               SDMMonth3ONtotal += parseFloat(month3valueON);
               SDMMonth1OFFtotal += parseFloat(month1valueOFF);
               SDMMonth2OFFtotal += parseFloat(month2valueOFF);
               SDMMonth3OFFtotal += parseFloat(month3valueOFF);


               totalOnsite += totalON;
               totalOFFsite += totalOFF;
               totalGrandtotal += grandTotal;
           }

           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').value = SDMMonth1ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').value = SDMMonth1OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').value = SDMMonth2ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').value = SDMMonth2OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').value = SDMMonth3ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').value = SDMMonth3OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOnTotal').value = totalOnsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOffTotal').value = totalOFFsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblFooterAlcon').value = totalGrandtotal.toFixed(1);

           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').title = SDMMonth1ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').title = SDMMonth1OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').title = SDMMonth2ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').title = SDMMonth2OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').title = SDMMonth3ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').title = SDMMonth3OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOnTotal').title = totalOnsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOffTotal').title = totalOFFsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblFooterAlcon').title = totalGrandtotal.toFixed(1);
       }


       function loaddatadhpna(thisobj) {
           
           var btnsave2 = document.getElementById('MainContent_btnSave2');
           if (thisobj.value == '') { thisobj.value = '0.0'; return true; }
           var currenttext = thisobj.value;
           var isEror = parseFloat(currenttext) + '' == 'NaN';
           if (isEror) {

               alert('Please enter a valid number'); thisobj.focus();
           }
           else {
               var value = thisobj.value;
               value = parseFloat(value);

               var decimalplace = (thisobj.value.split('.')[1] || []).length;
               var strValue = thisobj.value + '';
               if (decimalplace == 1) {

                   value = value.toFixed(1);
               }
               if (strValue.length > 9 || decimalplace > 1 ) {


                   alert('Please enter a positive value less than 6 digits with 1 decimal values'); thisobj.focus();
                   return;
               }

           }
           var SDMMonth1ONtotal = 0.0;
           var SDMMonth2ONtotal = 0.0;
           var SDMMonth3ONtotal = 0.0;
           var SDMMonth1OFFtotal = 0.0;
           var SDMMonth2OFFtotal = 0.0;
           var SDMMonth3OFFtotal = 0.0;
           var totalOnsite = 0.0;
           var totalOFFsite = 0.0;
           var totalGrandtotal = 0.0;

           var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           for (i = 0; i < grid.rows.length - 4; i++) {



               var month1valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth1_' + i).value;
               var month2valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth2_' + i).value;
               var month3valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth3_' + i).value;

               var month1valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth1_' + i).value;
               var month2valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth2_' + i).value;
               var month3valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth3_' + i).value;

               month1valueON = month1valueON == "" ? 0 : month1valueON;
               month2valueON = month2valueON == "" ? 0 : month2valueON;
               month3valueON = month3valueON == "" ? 0 : month3valueON;

               month1valueOFF = month1valueOFF == "" ? 0 : month1valueOFF;
               month2valueOFF = month2valueOFF == "" ? 0 : month2valueOFF;
               month3valueOFF = month3valueOFF == "" ? 0 : month3valueOFF;


               var totalON = parseFloat(month1valueON) + parseFloat(month2valueON) + parseFloat(month3valueON);
               var totalOFF = parseFloat(month1valueOFF) + parseFloat(month2valueOFF) + parseFloat(month3valueOFF);
               var grandTotal = totalON + totalOFF;

               document.getElementById('MainContent_grdBESDMView_lblTotOn_' + i).value = totalON.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotOff_' + i).value = totalOFF.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotVol_' + i).value = grandTotal.toFixed(1);

               document.getElementById('MainContent_grdBESDMView_lblTotOn_' + i).title = totalON.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotOff_' + i).title = totalOFF.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblTotVol_' + i).title = grandTotal.toFixed(1);
               //               grid.rows[i + 3].cells[20].innerText = totalON.toFixed(1);
               //               grid.rows[i + 3].cells[21].innerText = totalOFF.toFixed(1);
               //               grid.rows[i + 3].cells[22].innerText = grandTotal.toFixed(1);


               SDMMonth1ONtotal += parseFloat(month1valueON);
               SDMMonth2ONtotal += parseFloat(month2valueON);
               SDMMonth3ONtotal += parseFloat(month3valueON);
               SDMMonth1OFFtotal += parseFloat(month1valueOFF);
               SDMMonth2OFFtotal += parseFloat(month2valueOFF);
               SDMMonth3OFFtotal += parseFloat(month3valueOFF);


               totalOnsite += totalON;
               totalOFFsite += totalOFF;
               totalGrandtotal += grandTotal;
           }

           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').value = SDMMonth1ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').value = SDMMonth1OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').value = SDMMonth2ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').value = SDMMonth2OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').value = SDMMonth3ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').value = SDMMonth3OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOnTotal').value = totalOnsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOffTotal').value = totalOFFsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblFooterAlcon').value = totalGrandtotal.toFixed(1);

           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').title = SDMMonth1ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').title = SDMMonth1OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').title = SDMMonth2ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').title = SDMMonth2OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').title = SDMMonth3ONtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').title = SDMMonth3OFFtotal.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOnTotal').title = totalOnsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOffTotal').title = totalOFFsite.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblFooterAlcon').title = totalGrandtotal.toFixed(1);
       }

       var CalculateOnLoadFooterTotal = function (){  // calcualte footer total during OnLoad.

          
           var prefix = '#MainContent_grdBESDMView_';
           var rtbr = 0, dmBE = 0, DmVol = 0, MonthTotal = 0, Bk = 0, BKdiff = 0, diff = 0, BK1 = 0, BK2 = 0, BK3 = 0, BK4 = 0, M1 = 0, M2 = 0, M3 = 0, dmMonth1 = 0, dmMonth2 = 0, dmMonth3 = 0, TotBE = 0, bkMonth1 = 0, bkMonth2 = 0, bkMonth3 = 0, bkMonth4 = 0, BKTotal = 0, volmonth1on = 0, volmonth1off = 0, volmonth2on = 0, volmonth2off = 0, volmonth3on = 0, volmonth3off = 0, V1On = 0, V1Off = 0, V2On = 0, V2Off = 0, V3On = 0, V3Off = 0, Von = 0, Voff = 0, VTot = 0;
           var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           for (index = 0; index < grid.rows.length - 4; index++) {

         
               dmMonth1 += parseFloat($(prefix + 'txtDMMonth1_' + index).val());
               dmMonth2 += parseFloat($(prefix + 'txtDMMonth2_' + index).val());
               dmMonth3 += parseFloat($(prefix + 'txtDMMonth3_' + index).val());

               var lbldmbe = document.getElementById('MainContent_grdBESDMView_lnkbtnDMTotal_' + index).innerText;

               dmBE += parseFloat(lbldmbe);

               var lblRtbr = document.getElementById('MainContent_grdBESDMView_lnkbtnrtbr_' + index).innerText;

               rtbr += parseFloat(lblRtbr);

               var lblVoldm = document.getElementById('MainContent_grdBESDMView_lnkbtnDMVolTotal_' + index).innerText;
               DmVol += parseFloat(lblVoldm);

               //               var lblAlcon = document.getElementById('MainContent_grdBESDMView_lnkbtnAlconTotal_' + index).innerText;

               //               lblAlcon += parseFloat(lblAlcon);


               M1 = parseFloat($(prefix + 'txtDMMonth1_' + index).val());
               M2 = parseFloat($(prefix + 'txtDMMonth2_' + index).val());
               M3 = parseFloat($(prefix + 'txtDMMonth3_' + index).val());

               TotBE = M1 + M2 + M3;
               var lbl = document.getElementById('MainContent_grdBESDMView_lblBKLeft_' + index);
               lbl.innerText = TotBE.toFixed(1);
               bkMonth1 += parseFloat($(prefix + 'txtBKMonth1_' + index).val());
               bkMonth2 += parseFloat($(prefix + 'txtBKMonth2_' + index).val());
               bkMonth3 += parseFloat($(prefix + 'txtBKMonth3_' + index).val());
               bkMonth4 += parseFloat($(prefix + 'txtBKMonth4_' + index).val());



               BK1 = parseFloat($(prefix + 'txtBKMonth1_' + index).val());
               BK2 = parseFloat($(prefix + 'txtBKMonth2_' + index).val());
               BK3 = parseFloat($(prefix + 'txtBKMonth3_' + index).val());
               BK4 = parseFloat($(prefix + 'txtBKMonth4_' + index).val());
               BKTotal = BK1 + BK2 + BK3 + BK4
               diff = TotBE - BKTotal;
               var lblBK = document.getElementById('MainContent_grdBESDMView_lblBKRight_' + index);
               lblBK.innerText = diff.toFixed(1);

               volmonth1on += parseFloat($(prefix + 'txtVolOnMonth1_' + index).val());
               volmonth1off += parseFloat($(prefix + 'txtVolOffMonth1_' + index).val());

               volmonth2on += parseFloat($(prefix + 'txtVolOnMonth2_' + index).val());
               volmonth2off += parseFloat($(prefix + 'txtVolOffMonth2_' + index).val());

               volmonth3on += parseFloat($(prefix + 'txtVolOnMonth3_' + index).val());
               volmonth3off += parseFloat($(prefix + 'txtVolOffMonth3_' + index).val());

               V1On = parseFloat($(prefix + 'txtVolOnMonth1_' + index).val());
               V2On = parseFloat($(prefix + 'txtVolOnMonth2_' + index).val());
               V3On = parseFloat($(prefix + 'txtVolOnMonth3_' + index).val());


               V1Off = parseFloat($(prefix + 'txtVolOffMonth1_' + index).val());
               V2Off = parseFloat($(prefix + 'txtVolOffMonth2_' + index).val());
               V3Off = parseFloat($(prefix + 'txtVolOffMonth3_' + index).val());

               Von = V1On + V2On + V3On;
               Voff = V1Off + V2Off + V3Off;
               VTot = Voff + Von;
//               var lvlOn = document.getElementById('MainContent_grdBESDMView_lblTotOn_' + index);
//               var lblOff = document.getElementById('MainContent_grdBESDMView_lblTotOff_' + index);
//               var lblVolTot = document.getElementById('MainContent_grdBESDMView_lblTotVol_' + index);


               var lvlOn = 0, lblOff = 0, lblVolTot = 0;
               lvlOn = parseFloat($(prefix + 'lblTotOn_' + index).val());
               lblOff = parseFloat($(prefix + 'lblTotOff_' + index).val());
               lblVolTot = parseFloat($(prefix + 'lblTotVol_' + index).val());

//               lvlOn.innerText = Von;
//               lblOff.innerText = Voff;
//               lblVolTot.innerText = VTot;


           }
          
           MonthTotal = dmMonth1 + dmMonth2 + dmMonth3;
           MonthTotal = parseFloat(MonthTotal).toFixed(1);
           BK = bkMonth1 + bkMonth2 + bkMonth3 + bkMonth4;
           BK = parseFloat(BK).toFixed(1);
           BKdiff = MonthTotal - BK;
           BKdiff = parseFloat(BKdiff).toFixed(1);
           var text = MonthTotal + "(" + BKdiff + ")";

           var footerrow = grid.rows.length - 1;
           var voloonfooter = volmonth1on + volmonth2on + volmonth3on
           voloonfooter = parseFloat(voloonfooter).toFixed(1);

           var volofffooter = volmonth1off + volmonth2off + volmonth3off
           volofffooter = parseFloat(volofffooter).toFixed(1);

           var voltotfooter = volofffooter + voloonfooter;
           voltotfooter = parseFloat(voltotfooter).toFixed(1);

           document.getElementById('MainContent_grdBESDMView_lblmonth1').value = dmMonth1.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblmonth2').value = dmMonth2.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblmonth3').value = dmMonth3.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonthTotal').value = text;
           document.getElementById('MainContent_grdBESDMView_lblmonth1').title = dmMonth1.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblmonth2').title = dmMonth2.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblmonth3').title = dmMonth3.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonthTotal').title = text;

           //           grid.rows[footerrow].cells[3].innerText = dmMonth1.toFixed(1);
           //           grid.rows[footerrow].cells[4].innerText = dmMonth2.toFixed(1);
           //           grid.rows[footerrow].cells[5].innerText = dmMonth3.toFixed(1);
           //           grid.rows[footerrow].cells[6].innerText = text;
           document.getElementById('MainContent_grdBESDMView_lblBKmonth1').value = bkMonth1.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth2').value = bkMonth2.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth3').value = bkMonth3.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth4').value = bkMonth4.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblDMBETotal').value = dmBE.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblFooterRtbr').value = rtbr.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth1').title = bkMonth1.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth2').title = bkMonth2.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth3').title = bkMonth3.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth4').title = bkMonth4.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblDMBETotal').title = dmBE.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblFooterRtbr').title = rtbr.toFixed(1);

           //           grid.rows[footerrow].cells[7].innerText = bkMonth1.toFixed(1);
           //           grid.rows[footerrow].cells[8].innerText = bkMonth2.toFixed(1);
           //           grid.rows[footerrow].cells[9].innerText = bkMonth3.toFixed(1);
           //           grid.rows[footerrow].cells[10].innerText = bkMonth4.toFixed(1);
           //           grid.rows[footerrow].cells[11].innerText = dmBE.toFixed(1);
           //           grid.rows[footerrow].cells[12].innerText = rtbr.toFixed(1);

//           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').innerText = volmonth1on.toFixed(1);
//           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').innerText = volmonth1off.toFixed(1);
//           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').innerText = volmonth2on.toFixed(1);
//           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').innerText = volmonth2off.toFixed(1);
//           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').innerText = volmonth3on.toFixed(1);
//           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').innerText = volmonth3off.toFixed(1);
//           document.getElementById('MainContent_grdBESDMView_lblOnTotal').innerText = voloonfooter;
//           document.getElementById('MainContent_grdBESDMView_lblOffTotal').innerText = voltotfooter;

           document.getElementById('MainContent_grdBESDMView_lblDMVolTotal').value = DmVol.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').title = volmonth1on.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').title = volmonth1off.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').title = volmonth2on.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').title = volmonth2off.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').title = volmonth3on.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').title = volmonth3off.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblOnTotal').title = voloonfooter;
           document.getElementById('MainContent_grdBESDMView_lblOffTotal').title = voltotfooter;

           document.getElementById('MainContent_grdBESDMView_lblDMVolTotal').title = DmVol.toFixed(1);

           //           grid.rows[footerrow].cells[13].innerText = volmonth1on.toFixed(1);
           //           grid.rows[footerrow].cells[14].innerText = volmonth1off.toFixed(1);
           //           grid.rows[footerrow].cells[15].innerText = volmonth2on.toFixed(1);
           //           grid.rows[footerrow].cells[16].innerText = volmonth2off.toFixed(1);
           //           grid.rows[footerrow].cells[17].innerText = volmonth3on.toFixed(1);
           //           grid.rows[footerrow].cells[18].innerText = volmonth3off.toFixed(1);
           //           grid.rows[footerrow].cells[19].innerText = voloonfooter;
           //           grid.rows[footerrow].cells[20].innerText = volofffooter;
           //           grid.rows[footerrow].cells[21].innerText = voltotfooter;
           //           grid.rows[footerrow].cells[23].innerText = DmVol.toFixed(1);

       }
        


    </script>
    

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
            width: 450px;
            height: 250px;
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
        
        .web_dialogpopup
        {
            position: relative;
            width: 650px;
            height: 200px;
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
        .web_dialogpopupRevenue
        {
            position: fixed;
            width: 650px;
            height: 200px;
            top: 50%;
            left: 38%;
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
         .align_Left
        {
            text-align:left;
        }
        .mGrid
        {
            background-color: #fff; /* margin: 5px 0 10px 0;*/
            border: solid 0px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            font-size: 9pt;
        }
        body
        {
            font-family:Calibri!important;
        }
    </style>

    <style type="text/css">
        .modalBackground
        {
            background-color: #fff;
            border-bottom-style: none;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
       .modalPopup {
    background-color: #FFFFFF;
    border-width: 3px;
    border-style: solid;
    border-color: black;
    padding-top: 10px;
    padding-left: 10px;
    width: 300px;
    height: 140px;
}

        .style2
        {
            width: 106%;
        }
        

    </style>

   <script type="text/javascript">

       function HeaderClick(CheckBox) {
           //debugger;
           var TargetBaseControl =
       document.getElementById('<%= this.grdBESDMView.ClientID %>');
           var TargetChildControl = "chkRow";

           //Get all the control of the type INPUT in the base control.
           var Inputs = TargetBaseControl.getElementsByTagName("input");

           //Checked/Unchecked all the checkBoxes in side the GridView.
           for (var n = 0; n < Inputs.length; ++n)
               if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                   Inputs[n].checked = CheckBox.checked;


       }


       //            var OpenPopUpDetailsPage = function (query) {
       //                var left = (screen.width - 700) / 2;
       //                //var top = (screen.height - 900) / 2;
       //                var top = 0;
       //                newwindow = window.open(query + '', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=800, height=200 , menubar=no, scrollbars=yes, resizable=no');
       //                if (window.focus) { newwindow.focus() }

       //                if (!newwindow.closed) { newwindow.focus() }
       //                return false;

       //            }





       function PressNumberOnlyAndCalcVol(thisobj) {
           //debugger;
          
           var btnsave2 = document.getElementById('MainContent_btnSave2');
           if (thisobj.value == '') { thisobj.value = '0.0'; return true; }
           var currenttext = thisobj.value;
           var isEror = parseFloat(currenttext) + '' == 'NaN';
           if (isEror) {

               alert('Please enter a valid number'); thisobj.focus();
           }
           else {
               var value = thisobj.value;
               value = parseFloat(value);

               var decimalplace = (thisobj.value.split('.')[1] || []).length;
               var strValue = thisobj.value + '';
               if (decimalplace == 1) {

                   value = value.toFixed(1);
               }
               if (strValue.length > 9 || decimalplace > 1 || value < 0) {


                   alert('Please enter a positive value less than 6 digits with 1 decimal values'); thisobj.focus();
                   return;
               }


               var SDMMonth1ONtotal = 0.0;
               var SDMMonth2ONtotal = 0.0;
               var SDMMonth3ONtotal = 0.0;
               var SDMMonth1OFFtotal = 0.0;
               var SDMMonth2OFFtotal = 0.0;
               var SDMMonth3OFFtotal = 0.0;
               var totalOnsite = 0.0;
               var totalOFFsite = 0.0;
               var totalGrandtotal = 0.0;


               var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');
               for (i = 0; i < grid.rows.length - 4; i++) {



                   var month1valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth1_' + i).value;
                   var month2valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth2_' + i).value;
                   var month3valueON = document.getElementById('MainContent_grdBESDMView_txtVolOnMonth3_' + i).value;

                   var month1valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth1_' + i).value;
                   var month2valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth2_' + i).value;
                   var month3valueOFF = document.getElementById('MainContent_grdBESDMView_txtVolOffMonth3_' + i).value;

                   month1valueON = month1valueON == "" ? 0 : month1valueON;
                   month2valueON = month2valueON == "" ? 0 : month2valueON;
                   month3valueON = month3valueON == "" ? 0 : month3valueON;

                   month1valueOFF = month1valueOFF == "" ? 0 : month1valueOFF;
                   month2valueOFF = month2valueOFF == "" ? 0 : month2valueOFF;
                   month3valueOFF = month3valueOFF == "" ? 0 : month3valueOFF;


                   var totalON = parseFloat(month1valueON) + parseFloat(month2valueON) + parseFloat(month3valueON);
                   var totalOFF = parseFloat(month1valueOFF) + parseFloat(month2valueOFF) + parseFloat(month3valueOFF);
                   var grandTotal = totalON + totalOFF;


//                   grid.rows[i + 3].cells[20].innerText = totalON.toFixed(1);
//                   grid.rows[i + 3].cells[21].innerText = totalOFF.toFixed(1);
//                   grid.rows[i + 3].cells[22].innerText = grandTotal.toFixed(1);

                   document.getElementById('MainContent_grdBEDMView_lblTotOn_' + i).value = totalON.toFixed(1); ;
                   document.getElementById('MainContent_grdBEDMView_lblTotOff_' + i).value = totalOFF.toFixed(1);
                   document.getElementById('MainContent_grdBEDMView_lblTotVol_' + i).value = grandTotal.toFixed(1);


                   SDMMonth1ONtotal += parseFloat(month1valueON);
                   SDMMonth2ONtotal += parseFloat(month2valueON);
                   SDMMonth3ONtotal += parseFloat(month3valueON);
                   SDMMonth1OFFtotal += parseFloat(month1valueOFF);
                   SDMMonth2OFFtotal += parseFloat(month2valueOFF);
                   SDMMonth3OFFtotal += parseFloat(month3valueOFF);


                   totalOnsite += totalON;
                   totalOFFsite += totalOFF;
                   totalGrandtotal += grandTotal;

               }
               var footerrow = grid.rows.length - 1;
               document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').value = SDMMonth1ONtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').value = SDMMonth1OFFtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').value = SDMMonth2ONtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').value = SDMMonth2OFFtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').value = SDMMonth3ONtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').value = SDMMonth3OFFtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblOnTotal').value = totalOnsite.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblOffTotal').value = totalOFFsite.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblFooterAlcon').value = totalGrandtotal.toFixed(1);

               document.getElementById('MainContent_grdBESDMView_lblVolOnmonth1').title = SDMMonth1ONtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOffmonth1').title = SDMMonth1OFFtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOnmonth2').title = SDMMonth2ONtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOffmonth2').title = SDMMonth2OFFtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOnmonth3').title = SDMMonth3ONtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblVolOffmonth3').title = SDMMonth3OFFtotal.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblOnTotal').title = totalOnsite.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblOffTotal').title = totalOFFsite.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblFooterAlcon').title = totalGrandtotal.toFixed(1);

//               grid.rows[footerrow].cells[13].innerText = SDMMonth1ONtotal.toFixed(1);
//               grid.rows[footerrow].cells[14].innerText = SDMMonth1OFFtotal.toFixed(1);
//               grid.rows[footerrow].cells[15].innerText = SDMMonth2ONtotal.toFixed(1);
//               grid.rows[footerrow].cells[16].innerText = SDMMonth2OFFtotal.toFixed(1);
//               grid.rows[footerrow].cells[17].innerText = SDMMonth3ONtotal.toFixed(1);
//               grid.rows[footerrow].cells[18].innerText = SDMMonth3OFFtotal.toFixed(1);

//               grid.rows[footerrow].cells[19].innerText = totalOnsite.toFixed(1);
//               grid.rows[footerrow].cells[20].innerText = totalOFFsite.toFixed(1);
//               grid.rows[footerrow].cells[21].innerText = totalGrandtotal.toFixed(1);

           }
       }


       function PressNumberOnlyAndCalcBK(thisobj) {

           //  ClearSaveMessage();
           if (thisobj == null) {
               caldifference();
           }
           else {
               var btnsave2 = document.getElementById('MainContent_btnSave2');
               if (thisobj.value == '') { thisobj.value = '0.0'; return true; }
               var currenttext = thisobj.value;
               var isEror = parseFloat(currenttext) + '' == 'NaN';
               if (isEror) {


                   alert('Please enter a valid number'); thisobj.focus();

               }
               else {
                   var decimalplace = (thisobj.value.split('.')[1] || []).length;
                   var value = thisobj.value;
                   value = parseFloat(value);


                   var strValue = value + '';
                   if (decimalplace == 1) {

                       value = value.toFixed(1);
                   }

                   var strValue = value + '';
                   if (strValue.length > 9 || decimalplace > 1) {



                       alert('Please enter value less than 6 digits with 1 decimal values'); thisobj.focus();
                       return;
                   }
                   caldifference();
               //               grid.rows[footerrow].cells[6].innerText = FooterTot;
               //               grid.rows[footerrow].cells[7].innerText = sdmmonth1total.toFixed(1);
               //               grid.rows[footerrow].cells[8].innerText = sdmmonth2total.toFixed(1);
               //               grid.rows[footerrow].cells[9].innerText = sdmmonth3total.toFixed(1);
               //               grid.rows[footerrow].cells[10].innerText = sdmmonth4total.toFixed(1);


               //            if (totaldmmonth != totalBKFooter) {
               //              
               //                btnsave.disabled = true;
               //                btnsave2.disabled = true;

               //            }
               //            else {
               //                btnsave.disabled = false;
               //                btnsave2.disabled = false;
               //            }
           }
       }


       function caldifference()
       {


           var sdmmonth1total = 0;
           var sdmmonth2total = 0;
           var sdmmonth3total = 0;
           var sdmmonth4total = 0;
           var avgBK = 0;
           var totaldmmonth = 0;
           var totalBKFooter = 0;
           var diff = 0;
           var avg = 0;
           var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');

           for (i = 0; i < grid.rows.length - 4; i++) {

               var month1value = document.getElementById('MainContent_grdBESDMView_txtBKMonth1_' + i).value;
               var month2value = document.getElementById('MainContent_grdBESDMView_txtBKMonth2_' + i).value;
//               var month3value = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktC_' + i).innerText;
               //               var month4value = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktD_' + i).innerText;
               var month3value = document.getElementById('MainContent_grdBESDMView_txtBKMonth3_' + i).value;
               var month4value = document.getElementById('MainContent_grdBESDMView_txtBKMonth4_' + i).value;
               month1value = month1value == "" ? 0 : month1value;
               month2value = month2value == "" ? 0 : month2value;
               month3value = month3value == "" ? 0 : month3value;
               month4value = month4value == "" ? 0 : month4value;

               sdmmonth1total += parseFloat(month1value);
               sdmmonth2total += parseFloat(month2value);
               sdmmonth3total += parseFloat(month3value);
               sdmmonth4total += parseFloat(month4value);
               var M1 = document.getElementById('MainContent_grdBESDMView_txtDMMonth1_' + i).value;
               var M2 = document.getElementById('MainContent_grdBESDMView_txtDMMonth2_' + i).value;
               var M3 = document.getElementById('MainContent_grdBESDMView_txtDMMonth3_' + i).value;





               M1 = M1 == "" ? 0 : M1;
               M2 = M2 == "" ? 0 : M2;
               M3 = M3 == "" ? 0 : M3;



               var totalMonth = parseFloat(M1) + parseFloat(M2) + parseFloat(M3);
               var avgMonth = parseFloat(totalMonth).toFixed(1);
               totaldmmonth += parseFloat(avgMonth);



               var totalBK = parseFloat(month1value) + parseFloat(month2value) + parseFloat(month3value) + parseFloat(month4value);
               var avgBK = parseFloat(totalBK).toFixed(1);
               totalBKFooter += parseFloat(avgBK);


               var fName = document.getElementById('MainContent_grdBESDMView_txtBKMonth1_' + i)
               var fName1 = document.getElementById('MainContent_grdBESDMView_txtBKMonth2_' + i)
//               var fName2 = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktC_' + i)
               //               var fName3 = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktD_' + i)
               var fName2 = document.getElementById('MainContent_grdBESDMView_txtBKMonth3_' + i)
               var fName3 = document.getElementById('MainContent_grdBESDMView_txtBKMonth4_' + i)
               var fName4 = document.getElementById('MainContent_grdBESDMView_txtDMMonth1_' + i)
               var fName5 = document.getElementById('MainContent_grdBESDMView_txtDMMonth2_' + i)
               var fName6 = document.getElementById('MainContent_grdBESDMView_txtDMMonth3_' + i)
               //  fName.style.border = "1px solid red";

               fName.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";
               fName1.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";
               fName2.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";
               fName3.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";
//               fName2.style.color = avgMonth == avgBK ? "#034af3" : "red";
//               fName3.style.color = avgMonth == avgBK ? "#034af3" : "red";
               fName4.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";
               fName5.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";
               fName6.style.border = avgMonth == avgBK ? "1px solid gray" : "1px solid red";

               var Right1 = document.getElementById('MainContent_grdBESDMView_lblBKRight_' + i);
               var Left1 = document.getElementById('MainContent_grdBESDMView_lblBKLeft_' + i);
               Left1.innerText = parseFloat(avgMonth).toFixed(1);
               diff = avgMonth - avgBK;
               diff = diff.toFixed(1);
               Right1.innerText = parseFloat(diff).toFixed(1);

           }


           totaldmmonth = parseFloat(totaldmmonth).toFixed(1);
           var footerrow = grid.rows.length - 1;
           totalBKFooter = parseFloat(totalBKFooter);
           diff = totaldmmonth - totalBKFooter;
           diff = diff.toFixed(1)
           var FooterTot = totaldmmonth + "(" + diff + ")";
           document.getElementById('MainContent_grdBESDMView_lblBKmonthTotal').value = FooterTot;
           document.getElementById('MainContent_grdBESDMView_lblBKmonth1').value = sdmmonth1total.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth2').value = sdmmonth2total.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth3').value = sdmmonth3total.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth4').value = sdmmonth4total.toFixed(1);

           document.getElementById('MainContent_grdBESDMView_lblBKmonthTotal').title = FooterTot;
           document.getElementById('MainContent_grdBESDMView_lblBKmonth1').title = sdmmonth1total.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth2').title = sdmmonth2total.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth3').title = sdmmonth3total.toFixed(1);
           document.getElementById('MainContent_grdBESDMView_lblBKmonth4').title = sdmmonth4total.toFixed(1);

       }
       }

       function makeTextBoxRed() {

          
           var diff = 0;
           var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');

           for (i = 0; i < grid.rows.length - 4; i++) {

               var month1value = document.getElementById('MainContent_grdBESDMView_txtDMMonth1_' + i).value;
               var month2value = document.getElementById('MainContent_grdBESDMView_txtDMMonth2_' + i).value;
               var month3value = document.getElementById('MainContent_grdBESDMView_txtDMMonth3_' + i).value;
               month1value = month1value == "" ? 0 : month1value;
               month2value = month2value == "" ? 0 : month2value;
               month3value = month3value == "" ? 0 : month3value;



               var total = parseFloat(month1value) + parseFloat(month2value) + parseFloat(month3value);
               var avg = total.toFixed(1); /// 3;
               //19/12:12 changed to 11



               var Bk1 = document.getElementById('MainContent_grdBESDMView_txtBKMonth1_' + i).value;
               var Bk2 = document.getElementById('MainContent_grdBESDMView_txtBKMonth2_' + i).value;
//               var Bk3 = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktC_' + i).innerText;
               //               var Bk4 = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktD_' + i).innerText;
               var Bk3 = document.getElementById('MainContent_grdBESDMView_txtBKMonth3_' + i).value;
               var Bk4 = document.getElementById('MainContent_grdBESDMView_txtBKMonth4_' + i).value;
               Bk1 = Bk1 == "" ? 0 : Bk1;
               Bk2 = Bk2 == "" ? 0 : Bk2;
               Bk3 = Bk3 == "" ? 0 : Bk3;
               Bk4 = Bk4 == "" ? 0 : Bk4;


               var totalBK = parseFloat(Bk1) + parseFloat(Bk2) + parseFloat(Bk3) + +parseFloat(Bk4);
               var avgBK = totalBK.toFixed(1);




               var fName = document.getElementById('MainContent_grdBESDMView_txtBKMonth1_' + i)
               var fName1 = document.getElementById('MainContent_grdBESDMView_txtBKMonth2_' + i)
//               var fName2 = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktC_' + i)
               //               var fName3 = document.getElementById('MainContent_grdBESDMView_lnkbtnBcktD_' + i)
               var fName2 = document.getElementById('MainContent_grdBESDMView_txtBKMonth3_' + i)
               var fName3 = document.getElementById('MainContent_grdBESDMView_txtBKMonth4_' + i)
               var fName4 = document.getElementById('MainContent_grdBESDMView_txtDMMonth1_' + i)
               var fName5 = document.getElementById('MainContent_grdBESDMView_txtDMMonth2_' + i)
               var fName6 = document.getElementById('MainContent_grdBESDMView_txtDMMonth3_' + i)
               //  fName.style.border = "1px solid red";

               fName.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
               fName1.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
               fName2.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
               fName3.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
//               fName2.style.color = avg == avgBK ? "#034af3" : "red";
//               fName3.style.color = avg == avgBK ? "#034af3" : "red";
               fName4.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
               fName5.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
               fName6.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";

           }

        
       }

       function PressNumberOnlyAndCalc(thisobj) {

           //  ClearSaveMessage();
           if (thisobj.value == '') { thisobj.value = '0.0'; return true; }



       
           var btnsave2 = document.getElementById('MainContent_btnSave2');
           var currenttext = thisobj.value;
           var isEror = parseFloat(currenttext) + '' == 'NaN';
           if (isEror) {

               alert('Please enter a valid number'); thisobj.focus();

           }
           else {
               var decimalplace = (thisobj.value.split('.')[1] || []).length;
               var value = thisobj.value;
               value = parseFloat(value);


               var strValue = value + '';
               if (decimalplace == 1) {

                   value = value.toFixed(1);
               }

               var strValue = value + '';
               if (strValue.length > 9 || decimalplace > 1) {



                   alert('Please enter value less than 6 digits with 1 decimal values'); thisobj.focus();
                   return;
               }


               var sdmmonth1total = 0;
               var sdmmonth2total = 0;
               var sdmmonth3total = 0;

               var totaldmmonth = 0;
               var totalBKFooter = 0;
               var diff = 0;
               var grid = document.getElementById('<%= this.grdBESDMView.ClientID %>');

               for (i = 0; i < grid.rows.length - 4; i++) {

                   var month1value = document.getElementById('MainContent_grdBESDMView_txtDMMonth1_' + i).value;
                   var month2value = document.getElementById('MainContent_grdBESDMView_txtDMMonth2_' + i).value;
                   var month3value = document.getElementById('MainContent_grdBESDMView_txtDMMonth3_' + i).value;
                   month1value = month1value == "" ? 0 : month1value;
                   month2value = month2value == "" ? 0 : month2value;
                   month3value = month3value == "" ? 0 : month3value;

                   sdmmonth1total += parseFloat(month1value);
                   sdmmonth2total += parseFloat(month2value);
                   sdmmonth3total += parseFloat(month3value);

                   var total = parseFloat(month1value) + parseFloat(month2value) + parseFloat(month3value);
                   var avg = parseFloat(total); /// 3;
                   //19/12:12 changed to 11


                   totaldmmonth += parseFloat(avg);

                   var Bk1 = document.getElementById('MainContent_grdBESDMView_txtBKMonth1_' + i).value;
                   var Bk2 = document.getElementById('MainContent_grdBESDMView_txtBKMonth2_' + i).value;
                   var Bk3 = document.getElementById('MainContent_grdBESDMView_txtBKMonth3_' + i).value;
                   var Bk4 = document.getElementById('MainContent_grdBESDMView_txtBKMonth4_' + i).value;
                   Bk1 = Bk1 == "" ? 0 : Bk1;
                   Bk2 = Bk2 == "" ? 0 : Bk2;
                   Bk3 = Bk3 == "" ? 0 : Bk3;
                   Bk4 = Bk4 == "" ? 0 : Bk4;


                   var totalBK = parseFloat(Bk1) + parseFloat(Bk2) + parseFloat(Bk3) + +parseFloat(Bk4);


                   var avgBK = parseFloat(totalBK).toFixed(1);
                   totalBKFooter += parseFloat(avgBK);

                   var fName = document.getElementById('MainContent_grdBESDMView_txtBKMonth1_' + i)
                   var fName1 = document.getElementById('MainContent_grdBESDMView_txtBKMonth2_' + i)
                   var fName2 = document.getElementById('MainContent_grdBESDMView_txtBKMonth3_' + i)
                   var fName3 = document.getElementById('MainContent_grdBESDMView_txtBKMonth4_' + i)
                   var fName4 = document.getElementById('MainContent_grdBESDMView_txtDMMonth1_' + i)
                   var fName5 = document.getElementById('MainContent_grdBESDMView_txtDMMonth2_' + i)
                   var fName6 = document.getElementById('MainContent_grdBESDMView_txtDMMonth3_' + i)
                   //  fName.style.border = "1px solid red";

                   fName.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
                   fName1.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
                   fName2.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
                   fName3.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
                   fName4.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
                   fName5.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";
                   fName6.style.border = avg == avgBK ? "1px solid gray" : "1px solid red";

                   var Right1 = document.getElementById('MainContent_grdBESDMView_lblBKRight_' + i);
                   var Left1 = document.getElementById('MainContent_grdBESDMView_lblBKLeft_' + i);
                   Left1.innerText = parseFloat(avg).toFixed(1);
                   diff = avg - avgBK;
                   diff = diff.toFixed(1);
                   Right1.innerText = parseFloat(diff).toFixed(1);




               }

               totaldmmonth = parseFloat(totaldmmonth).toFixed(1);
               var footerrow = grid.rows.length - 1;
               totalBKFooter = parseFloat(totalBKFooter);
               totalBKFooter = totalBKFooter.toFixed(1)
               diff = totaldmmonth - totalBKFooter;
               diff = diff.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblmonth1').value = sdmmonth1total.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblmonth2').value = sdmmonth2total.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblmonth3').value = sdmmonth3total.toFixed(1);
               var FooterTot = totaldmmonth + "(" + diff + ")";
               document.getElementById('MainContent_grdBESDMView_lblBKmonthTotal').value = FooterTot;

               document.getElementById('MainContent_grdBESDMView_lblmonth1').title = sdmmonth1total.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblmonth2').title = sdmmonth2total.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblmonth3').title = sdmmonth3total.toFixed(1);
               document.getElementById('MainContent_grdBESDMView_lblBKmonthTotal').title = FooterTot;
//               grid.rows[footerrow].cells[3].innerText = sdmmonth1total.toFixed(1);
//               grid.rows[footerrow].cells[4].innerText = sdmmonth2total.toFixed(1);
//               grid.rows[footerrow].cells[5].innerText = sdmmonth3total.toFixed(1);
//               grid.rows[footerrow].cells[6].innerText = FooterTot;

           }


           //            if (totalBKFooter != totaldmmonth) {
           //                
           //                btnsave.disabled = true;
           //                btnsave2.disabled = true;

           //            }
           //            else {
           //                btnsave.disabled = false;
           //                btnsave2.disabled = false;
           //            }


       }



       function PressNegative(evt, thisobj) {
           var charCode = (evt.which) ? evt.which : event.keyCode

           if (charCode = 45)
               return true;

       }


       function PressNumberOnly(evt, thisobj) {


           var charCode = (evt.which) ? evt.which : event.keyCode




           if (charCode == 13) return false;


           if (evt.shiftKey == true)
               if ((charCode > 47 && charCode < 61))
                   return false;





           var textboxValue = thisobj.value + "";




           if (evt.ctrlKey == true) {
               if (charCode == 67 || charCode == 86 || charCode == 88)
                   return true;
           }


           if (charCode == 190 || charCode == 110) {
               var contains = textboxValue.indexOf(".") != -1;
               if (contains)
                   return false;
           }

           if (charCode == 37 || charCode == 40)  // allow arrows
               return true;



           if (charCode == 46)
               return true;
           if (charCode == 190 || charCode == 110)
               return true;

           if (charCode > 47 && charCode < 58)
               return true;

           if (charCode > 95 && charCode < 106)
               return true;

           if (charCode == 8 || charCode == 9) return true;


           return false;
       }
       var winpopupstatus;
       var winpopupstatus1 = null;


       function AddCustomerPopUpChange() {
           //debugger;

           var Ok = confirm('Changes made to the BE data are not yet saved.\nClick “Cancel” if you would like to stay on the current page.\nClick “Ok” if you would like to ignore the changes and go to “Add Customer”');

           if (!Ok) return false;
           else {

               var left = (screen.width - 700) / 2;

               if (winpopupstatus != null)
               { winpopupstatus.close(); }
               if (winpopupstatus1 != null)
               { winpopupstatus1.close(); }

               winpopupstatus1 = window.open('AddMasterCustomer.aspx', 'pop', 'left = ' + left + ',width=355, height=290, menubar=no, scrollbars=no, resizable=no');
               winpopupstatus1.focus();
               winpopupstatus1.onbeforeunload = function () {
                   document.getElementById('MainContent_btnSearch').click();
               }

               document.onmousedown = parent_disable;
               document.onkeyup = parent_disable;
               document.onmousemove = parent_disable;

              // gridviewScroll();
               return false;
           }
       }
       function AddCustomerPopUpNOChange() {
           //debugger;




           var left = (screen.width - 700) / 2;

           if (winpopupstatus != null)
           { winpopupstatus.close(); }
           if (winpopupstatus1 != null)
           { winpopupstatus1.close(); }

          
           winpopupstatus1 = window.open('AddMasterCustomer.aspx', 'pop', 'left = ' + left + ',width=330, height=280, menubar=no, scrollbars=no, resizable=no');
          
           winpopupstatus1.focus();
            winpopupstatus1.onbeforeunload = function () {
                   document.getElementById('MainContent_btnSearch').click();
               }
           document.onmousedown = parent_disable;
           document.onkeyup = parent_disable;
           document.onmousemove = parent_disable;


           return false;
       }

       function popclose() {
           winpopupstatus1.close();
       }

       function parent_disable() {
           if (winpopupstatus1 && !winpopupstatus1.closed)
               winpopupstatus1.focus();
       }
       //        function ValidateAddCustomer() {

       //            var Ok = confirm('Unsaved data will be lost. Do you wish to continue?');

       //            if (Ok) return true;
       //            else return false;
       //        }
       //        

       function PopUpDMBE(rowindex) {



           //debugger;
           var left = (screen.width - 700) / 2;
           var top = $(document).scrollTop();


           // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
           //window.open('BETrendsReport.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=700, height=140 , menubar=no, scrollbars=no, resizable=no');
           //debugger;
           var i, CellValue, Row, MCC, NC, PU, Qtr, Year, qtryr, yr, yr1;
           var no = rowindex.id.split('_')[3];
           i = parseInt(no) + 2;
           var hndfldname = 'MainContent_grdBESDMView_hdnfld_' + no;
           var table = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           var BEID = document.getElementById(hndfldname).value;

           if (winpopupstatus != null)
           { winpopupstatus.close(); }
           if (winpopupstatus1 != null)
           { winpopupstatus1.close(); }

           winpopupstatus = window.open('DMDetails.aspx?ID=' + BEID, 'pop', 'left = ' + left + ',width=800, height=450 , menubar=no, scrollbars=yes, resizable=no');

           if (!winpopupstatus.closed)
           { winpopupstatus.focus(); }
           CalculateOnLoadFooterTotal();
           makeTextBoxRed();
           return false;
       }





       function PopUpDMVolumeBE(rowindex) {




           var left = (screen.width - 700) / 2;
           var top = $(document).scrollTop();


           // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
           //window.open('BETrendsReport.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=700, height=140 , menubar=no, scrollbars=no, resizable=no');

           var i, CellValue, Row, MCC, NC, PU, Qtr, Year, qtryr, yr, yr1;
           var no = rowindex.id.split('_')[3];
           i = parseInt(no) + 2;
       
           var hndfldname = 'MainContent_grdBESDMView_hdnfld_' + no;
          

           var table = document.getElementById('<%= this.grdBESDMView.ClientID %>');
           var BEID = document.getElementById(hndfldname).value;
          
          
           winpopupstatus = window.open('DMVolume.aspx?ID=' + BEID + '&Type=SDM', 'pop', 'left = ' + left + ',width=900, height=450 , menubar=no, scrollbars=yes, resizable=no');

           if (!winpopupstatus.closed)
           { winpopupstatus.focus(); }
           CalculateOnLoadFooterTotal();
           makeTextBoxRed();
           return false;
       }



        
           

  


    </script>
   
    <script type="text/javascript">
        function PostBackParentWindow() {

            var refress = document.getElementById('<%= this.hdrefress.ClientID %>');
            refress.value = "1";

            __doPostBack(null, null);
        }
        function ParentWindow() {

            var refress = document.getElementById('<%= this.hdrefress.ClientID %>');
            refress.value = "0";


        }


        function gifClose() {

            setTimeout(function () {
                document.getElementById('ldimg').style.display = "none";
            });


        }

        function loadinggif() {


            setTimeout(function () {
                document.getElementById('uploadgif').style.display = "none";
                document.getElementById('loadgif').style.display = "block";
            });


        }

        function loadinggifClose() {


            setTimeout(function () {
                document.getElementById('uploadgif').style.display = "block";
                document.getElementById('loadgif').style.display = "none";
            });
            gridviewScroll();

            DisableAll();

        }


        function DisableAll() {
            
            $("input[type='text']").each(function () {
                this.style.border = '0px'
                this.onkeydown = null;
                this.onblur = null;
                this.click = null;
            });

            $('[id*="btnSave2"]').hide();
            $('[id*="btnUpload"]').hide();
            $('[id*="btnCopy"]').hide();
            $('[id*="btnZeroBE"]').hide();
            $('[id*="btnAddMasterCustomer"]').hide();
        }

        $(function () { DisableAll(); });
      
         
       

    </script>



 <link rel="stylesheet" href="boot.css"/>

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >

<body>

 <div id="loadgif" class="progress1">
                        <div class="center">
                             <img alt="" src="Images/load.gif" height="100" width="75"/>
                        </div>
                    </div>

                 

    <div align="center"  >  
          
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2" valign="top" align="left" bgcolor="#C41502" style="width: 400px">
                <asp:Menu ID="MenuAdmin" Orientation="Vertical" runat="server" BackColor="#f8df9c"
                    Height="18px" StaticMenuStyle-HorizontalPadding="20px" DynamicHorizontalOffset="2"
                    Font-Names="Calibri" Font-Size="11px" ForeColor="#c41502" Font-Bold="true" StaticSubMenuIndent="10px">
                    <Items>
                    </Items>
                    <StaticSelectedStyle BackColor="#c41502" />
                    <StaticMenuItemStyle HorizontalPadding="20px" VerticalPadding="2px" />
                    <DynamicHoverStyle BackColor="#c41502" Font-Bold="False" ForeColor="White" />
                    <DynamicItemTemplate>
                        <%# Eval("Text") %>
                    </DynamicItemTemplate>
                    <DynamicMenuStyle BackColor="#f8df9c" />
                    <DynamicSelectedStyle BackColor="#1C5E55" />
                    <DynamicMenuItemStyle HorizontalPadding="15px" VerticalPadding="2px" />
                    <StaticHoverStyle BackColor="#c41502" Font-Bold="False" ForeColor="White" />
                    <StaticItemTemplate>
                        <%# Eval("Text") %>
                    </StaticItemTemplate>
                </asp:Menu>
            </td>
        </tr>
        </table>
      <asp:UpdatePanel ID="upSetSession" runat="server" >
                   <ContentTemplate>
    <table>
        <tr>
            <td align="left" colspan="2">
                <asp:Label ID="lblmsg1" Text="" runat="server" ForeColor="Red" Height="16px"
                    Font-Size="9pt" Font-Bold="true"></asp:Label>
                <br />
                <asp:Label ID="lblmsg" runat="server" Text="" Font-Size="9pt"></asp:Label>
                <%--<asp:HiddenField ID="hiddenMsg" runat="server" />
                <asp:HiddenField ID="hiddenMsgIndex" runat="server" />--%>
                <asp:Label ID="lblmsg3" Text=" " runat="server" ForeColor="Red" Height="16px" Font-Size="9pt" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
    <tr>
    <td>
    
            
    </td>
    </tr>
        <tr>
            <td>
            <div style="margin:3px;float:left;margin-left:10px">
             
                      <div style="float:left;margin-top:3px">Offering:&nbsp</div>  
                        <div style="float:left"><asp:DropDownList ID="ddlNSO" Width="80" Height="25" onchange="loadinggif()" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlNSO_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList></div>
                       <div style="float:left;margin-top:3px"> &nbsp&nbsp Master Customer Code: &nbsp</div>
                      <div style="float:left">  <asp:DropDownList ID="ddlCustomerCode" Width="150" Height="25" runat="server" CssClass="form-control">
                        </asp:DropDownList></div>
                      <div style="float:left;margin-top:3px">  &nbsp&nbsp Qtr:&nbsp</div>
                      <div style="float:left;">  <asp:DropDownList ID="ddlQuarter" Width="60" Height="25" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        &nbsp</div>

                          <div style="float:left;margin-left:20px;margin-top:1px">
                <asp:Button ID="btnSearch" OnClientClick="loadinggif()" runat="server" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small" Visible="true" OnClick="btnSearch_Click"
                    Text=" Search " /> &nbsp; 
                <asp:Button ID="btnSave2" runat="server"  class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small;display:none" OnClick="btnSave_Click"
                    Text=" Save " Visible="false" />
                &nbsp; 
                <asp:Button ID="btnCopy" runat="server"  class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small;display:none" OnClick="btnCopyDMsData_Click" Enabled="true"
                    Text=" Copy DM's Data " Visible="true" />
                &nbsp; 
                
                <asp:Button ID="btnAddMasterCustomer" runat="server"  class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small;display:none"
                    Text=" Include Master Customer " Visible="true" 
                   OnClientClick="AddCustomerPopUpNOChange();return false;"
                   />
                    &nbsp;  
                    <asp:Button ID="btnZeroBE" runat="server" class="btn btn-info btn-sm" Height="25" style="padding-top:2px!important;border:1px solid lightgray;font-size:small;display:none" OnClick="btnZeroBE_Click" Enabled="true"
                    Text=" Delete Zero BE " Visible="false" />&nbsp; &nbsp; 
                    </div>

                    
                       <div style="float:left;margin-top:3px"><asp:ImageButton ID="ImgDownloadToExcel" runat="server" Width="25" Height="25" ToolTip="Download To Excel"
                                        ImageUrl="~/Images/excel.png" OnClick="ImgDownloadToExcel_Click" /></div>
                                             <div style="float:left;"><img style="display: none;height:25px;width:25px" alt="loading.." id="imgloadinggif" src="Images/ExcelLoading.gif"/></div>
                                         
                   <div id="bulk" runat="server" visible="false" style="top: 0px; height: 25px !important; margin-top: 0px;
                            margin-left: 10px; float: left;">
                          <%--  <div style="height: 12.5px  !important; margin-top: 0px;">
                                <asp:Label ID="lblMsgBulk" runat="server" Font-Names="Calibri" Font-Size="10pt" Font-Bold="true"
                                    Text="Updates Can be uploaded Using Excel template:"></asp:Label>
                            </div>--%>
                                <div style="height: 20px  !important; margin-top: 0px;margin-left:15px;float:left">
                        <div style="float:left;height:10px">
                          <asp:Label ID="lblBulk" runat="server" Text="Bulk BE Update"  Font-Names="Calibri" Font-Size="10pt" style="padding-left:10px"></asp:Label> </div>
                            <div style="float:left;clear:both;height:10px"> <asp:LinkButton ID="lbBulkUpdate" runat="server"  Font-Names="Calibri"
                                Font-Size="10pt" OnClick="lbBulkUpdate_Click" Enabled="false" ToolTip="Please ensure correct Quarter is selected from the dropdown">Download</asp:LinkButton>
                                <%-- /--%>
                                 <asp:LinkButton ID="lbUpload" runat="server" Font-Names="Calibri" Font-Size="10pt">Upload</asp:LinkButton></div>
                                  <iframe id="iframeexcel" runat="server" style="display: none"></iframe>
                               <%--   <asp:Button ID="hdUpload" runat="server" OnClick="btnUpload_Click" />--%>
                                    </div>
                            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Height="32px"
                                Width="470px" Style="display: none">
                                 <img id="ldimg" src="Images/squares.gif" style="display:none;text-align:center"/>
                                <span><strong>

                                    <asp:FileUpload ID="fuUploader" runat="server" Font-Names="Verdana" Style="width: 276px !important;
                                        height: 23px; padding-top: 2px!important; font-size: small" OnChange="return validateFile()"/>
                                    &nbsp;<asp:Button ID="btnUpload" runat="server" class="btn btn-info btn-sm" Height="25" Enabled="false"
                                        Font-Names="Verdana" OnClientClick="return validateUpload()"  OnClick="btnUpload_Click" Text="Upload" 
                                        Style="display:none; padding-top: 2px!important;
                                        border: 1px solid lightgray; margin-bottom: 4px; font-size: small" />
                                    &nbsp;</strong>
                           <asp:Button ID="btnCancel" runat="server" Font-Names="Verdana" Text="Close"
                                        class="btn btn-danger btn-sm" Height="25" Style="padding-top: 2px!important;
                                        margin-bottom: 4px; border: 1px solid lightgray; font-size: small; font-weight: bold" />
                                    &nbsp;&nbsp;</span><br />
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="MPE" runat="server" TargetControlID="lbUpload" PopupControlID="Panel1"
                                BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel" />
                            <asp:Button ID="btnNone" runat="server" Style="display: none" />
                            <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="center" Width="500px"
                                Height="300px" Style="display: none">
                                <div style="padding-right: 15px!important; float: right">
                                    <asp:ImageButton ID="btnmsgcls" runat="server" Width="20" Height="20" ToolTip="Close"
                                        ImageUrl="~/Images/close.gif" />
                                </div>
                                <div>
                                    <asp:Label ID="lblProcessedno" runat="server" Height="25" Style="width: 100%; padding-top: 2px!important;
                                        margin-bottom: 4px; border: none; font-size: small; font-weight: bold"></asp:Label>
                                </div>
                                <div style="padding-left: 194px!important; float: left">
                                    <asp:Label ID="lblSuccessno" runat="server" Height="25" Style="width: 100%; padding-top: 2px!important;
                                        margin-bottom: 4px; border: none; font-size: small; font-weight: bold"></asp:Label>
                                </div>
                                <div style="padding-left: 208px!important; float: left">
                                    <asp:Label ID="lblFailureno" runat="server" Height="25" Style="width: 100%; padding-top: 2px!important;
                                        margin-bottom: 4px; border: none; font-size: small; font-weight: bold"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblFinalMessage" runat="server" Height="25" Style="width: 100%; padding-top: 2px!important;
                                        padding-bottom: 10px!important; margin-bottom: 14px; border: none; font-size: small;
                                        font-weight: bold"></asp:Label>
                                </div>
                               <div style="margin-right:10px;overflow-y:scroll;height:125px" >
                                    <asp:GridView ID="GVErrorMsg" AutoGenerateColumns="true" Style="width: 100%" HeaderStyle-BackColor="#333333"
                                        HeaderStyle-ForeColor="White" Visible="false" runat="server">
                                    </asp:GridView>
                                </div>
                                <%--<div>
                            <asp:Button ID="btnmsgcls" runat="server" Font-Names="Verdana" Text="Close"
                                    class="btn btn-danger btn-sm" Height="25" Style="padding-top: 2px!important;
                                    margin-bottom: 4px; border: 1px solid lightgray; font-size: small; font-weight: bold" />
                            </div>--%>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="Modal2" runat="server" TargetControlID="btnNone" PopupControlID="Panel2"
                                BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnmsgcls" />
                        </div>
                   
                </div>
              
                       
                   

                   
                                       
                                     
            </td>
         
        </tr>
    </table>
   
                     

   
         
            
              
                <asp:Label ID="Label5" runat="server" Style="font-size: 11pt; color:#58738d; font-family: Calibri" Text="(**) A - Booked Business (Like RTBR)"></asp:Label>
                &nbsp &nbsp
                <asp:Label ID="Label2" runat="server" Style="font-size: 11pt; color:#58738d; font-family: Calibri"
                    Text="B – Extensions or deals already won"></asp:Label>
                &nbsp &nbsp
                <asp:Label ID="Label3" runat="server" Style="font-size: 11pt; color:#58738d; font-family: Calibri"
                    Text=" C – Proposals already submitted but still open"></asp:Label>
                &nbsp &nbsp
                <asp:Label ID="Label4" runat="server" Style="font-size: 11pt; color:#58738d; font-family: Calibri"
                    Text=" D – Other opportunities WIP (Not submitted yet)"></asp:Label>
          
                         </div>

                       
                       <div align="center" style="padding-left:30px;">
                          <div align="left">  

              <asp:GridView ID="grdBESDMView" runat="server" AutoGenerateColumns="False" ShowFooter="True" EmptyDataText="No records found" OnRowCommand="grdBESDMView_RowCommand" OnRowCreated="grdBESDMView_RowCreated" OnRowDataBound="gvUserInfo_RowDataBound"
                    CssClass="mGrid" >
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="DisplayNone"
                            HeaderStyle-CssClass="DisplayNone" FooterStyle-CssClass="DisplayNone">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnfld" Value='<%# Bind("ID") %>' runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnBilledFinpulse" Value='<%# Bind("BilledFinpulse") %>' runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnEffortMonths" Value='<%# Bind("fltEffortMonths") %>' runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnAlconEffort" Value='<%# Bind("AlconEffort") %>' runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnPBSEffort" Value='<%# Bind("PBSEffort") %>' runat="server"></asp:HiddenField>
                            </ItemTemplate>
                            <FooterStyle CssClass="DisplayNone" />
                            <HeaderStyle CssClass="DisplayNone" HorizontalAlign="Center" />
                            <ItemStyle CssClass="DisplayNone" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                            
                           
                             <asp:CheckBox ID="chkRow" runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                         
                                <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server" ToolTip="Selection for copy DM data" />
                            </HeaderTemplate>
                            <FooterTemplate>
                           
                                <asp:Label ID="total" runat="server" Text="Total" ForeColor="White" Font-Bold="True" CssClass="align_Left" ></asp:Label>
                              
                            </FooterTemplate>
                         <HeaderStyle Wrap="true" Width="20"/>
                      

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MCC-Offering" SortExpression="SkillType" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                              <div class="wid">
                            <asp:HiddenField ID="hdnfmcc" Value='<%# Bind("txtMasterClientCode") %>' runat="server"></asp:HiddenField>
                                <asp:Label ID="lblMCC" runat="server" Text='<%#Eval("txtMasterClientCode")+ "-" + Eval("NewOffering")%>' ToolTip='<%# Bind("txtMasterCustomerName") %>'></asp:Label>
                                   </div>
                            </ItemTemplate>
                               
                         <HeaderStyle Wrap="false" Width="100"/>
                       
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NC" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                           <div style="width:30px">
                                <asp:Label ID="lblNativeCurrency" Text='<%# Bind("txtNativeCurrency") %>' runat="server"></asp:Label>
                               </div>
                            </ItemTemplate>
                          <FooterTemplate>
                            <asp:TextBox ID="lblmonth1" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                         </FooterTemplate>
                          <HeaderStyle  Width="30"/>
                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Month1" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">

                             <FooterTemplate>
                             <asp:TextBox ID="lblmonth2" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="38"/>
                            
                            </FooterTemplate>
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtDMMonth1" Text='<%# Bind("fltSDMMonth1BE") %>' Height="15" Width="38"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>

                         <HeaderStyle  Width="45"/>
                         
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Month2" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtDMMonth2" Text='<%# Bind("fltSDMMonth2BE") %>' Height="15" Width="38"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                               <FooterTemplate>
                               <asp:TextBox ID="lblmonth3" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="38"/>
                            </FooterTemplate>
                                 <HeaderStyle  Width="45"/>
                        
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Month3" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtDMMonth3" Text='<%# Bind("fltSDMMonth3BE") %>' Height="15" Width="38"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                            <FooterTemplate>
                              <asp:TextBox ID="lblBKmonthTotal" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="60"/>
                                                            
                                                            </FooterTemplate>
                                 <HeaderStyle  Width="45"/>
                       
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="BETotal" SortExpression="TotalVol" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:70px;">
                                <asp:Label ID="lblBKLeft" runat="server" Text='<%# Bind("fltSDMQuarterBE") %>'></asp:Label>
                                (<asp:Label ID="lblBKRight" runat="server" Text='<%# Bind("fltBKTotal") %>'></asp:Label>)
                                </div>
                            </ItemTemplate>
                         <FooterTemplate>
                                <asp:TextBox ID="lblBKmonth1" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                               
                            </FooterTemplate>
                            <HeaderStyle  Width="70"/>
                          
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="A" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:47px">
                                <asp:TextBox ID="txtBKMonth1" Text='<%# Bind("fltBK1") %>' Height="15" Width="38"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                            <FooterTemplate>
                              <asp:TextBox ID="lblBKmonth2" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="38"/>
                                                            
                                                            </FooterTemplate>
                                 <HeaderStyle  Width="48"/>
                        
                        </asp:TemplateField>
                        
                       
                        <asp:TemplateField HeaderText="B" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:47px">
                                <asp:TextBox ID="txtBKMonth2" Text='<%# Bind("fltBK2") %>' Height="15" Width="40"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                          <FooterTemplate>
                             <asp:TextBox ID="lblBKmonth3" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                           </FooterTemplate>
                             <HeaderStyle  Width="48" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="C" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                           

                                    
                                   
                                  <div style="width:46px">

                                     <asp:TextBox ID="txtBKMonth3" Text='<%# Bind("fltBK3") %>' Height="15" Width="40"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                              
                                     
                              
                                   
                            </div>
                            </ItemTemplate>
                            <FooterTemplate>
                             <asp:TextBox ID="lblBKmonth4" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                            
                            </FooterTemplate>
                              <HeaderStyle  Width="46" />
                           
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="D" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                           
                                      <div style="width:48px">

                                     <asp:TextBox ID="txtBKMonth4" Text='<%# Bind("fltBK4") %>' Height="15" Width="40"
                                    CssClass="TextBox" runat="server"></asp:TextBox>
                                              
                                     </div>
                                   
                            </ItemTemplate>
                          <FooterTemplate>
                             <asp:TextBox ID="lblDMBETotal" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                             </FooterTemplate>
                             <HeaderStyle  Width="45" />
                           
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DMBE" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:55px">
                                <asp:HyperLink ID="lnkbtnDMTotal" runat="server" Text='<%# Bind("DMRevenueBE") %>'
                                     NavigateUrl="#" onclick="PopUpDMBE(this); return false;"></asp:HyperLink>
                                      </div>
                             
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:TextBox ID="lblFooterRtbr" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                         </FooterTemplate>
                            <HeaderStyle  Width="55" />
                           
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="(Actuals + RTBR)(NC)" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                            Visible="true">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:HyperLink ID="lnkbtnrtbr" runat="server" Text='<%# Bind("RTBRFinPulse") %>'
                                     NavigateUrl="#" onclick="PopUpDMBE(this); return false;" ></asp:HyperLink>
                                     </div>
                            </ItemTemplate>
                           <FooterTemplate>
                             <asp:TextBox ID="lblVolOnmonth1" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                          
                            </FooterTemplate>
                         
                        <HeaderStyle Wrap="true" Width="45" />
                           
                        </asp:TemplateField>
                       
                        <asp:TemplateField HeaderText="On" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:44px">
                                <asp:TextBox ID="txtVolOnMonth1" Text='<%# Bind("fltSDMMonth1onsite") %>' Height="15"
                                    Width="40" CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                             <FooterTemplate>
                             <asp:TextBox ID="lblVolOffmonth1" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                               
                            </FooterTemplate>
                             <HeaderStyle  Width="45" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtVolOffMonth1" Text='<%# Bind("fltSDMMonth1offsite") %>' Height="15"
                                    Width="40" CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:TextBox ID="lblVolOnmonth2" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                               
                            </FooterTemplate>
                             <HeaderStyle Width="45" />
                           
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="On" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtVolOnMonth2" Text='<%# Bind("fltSDMMonth2onsite") %>' Height="15"
                                    Width="40" CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                           <FooterTemplate>
                              <asp:TextBox ID="lblVolOffmonth2" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                             </FooterTemplate>
                               <HeaderStyle Width="45" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtVolOffMonth2" Text='<%# Bind("fltSDMMonth2offsite") %>' Height="15"
                                    Width="40" CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                             <FooterTemplate>
                              <asp:TextBox ID="lblVolOnmonth3" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                              
                            </FooterTemplate>
                           <HeaderStyle Width="45" />
                             
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="On" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtVolOnMonth3" Text='<%# Bind("fltSDMMonth3onsite") %>' Height="15"
                                    Width="40" CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                             <FooterTemplate>
                            <asp:TextBox ID="lblVolOffmonth3" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                             
                            </FooterTemplate>
                             <HeaderStyle  Width="45" />
                             
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:45px">
                                <asp:TextBox ID="txtVolOffMonth3" Text='<%# Bind("fltSDMMonth3offsite") %>' Height="15"
                                    Width="40" CssClass="TextBox" runat="server"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
                           <FooterTemplate>
                                 
                            <asp:TextBox ID="lblOnTotal" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="40"/>
                          
                            </FooterTemplate>
                             <HeaderStyle  Width="45" />
                           
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="On" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                             <div style="width:45px">
                                 <asp:TextBox ID="lblTotOn" ReadOnly ="true" Text='<%# Bind("fltSDMTotalonsite") %>' ToolTip='<%# Bind("fltSDMTotalonsite") %>' CssClass="footerBox1" runat="server" BorderWidth="0"  Width="40"/>
                               </div>
                            </ItemTemplate>
                               <FooterTemplate>
                                         
                            <asp:TextBox ID="lblOffTotal" ReadOnly ="true" CssClass="footerBox" runat="server" BorderWidth="0"  Width="38"/>
                            
                            </FooterTemplate>
                             <HeaderStyle Width="45" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                             <div style="width:40px">
                              <asp:TextBox ID="lblTotOff" ReadOnly ="true" Text='<%# Bind("fltSDMTotaloffsite") %>' ToolTip='<%# Bind("fltSDMTotaloffsite") %>'  CssClass="footerBox1" runat="server" BorderWidth="0"  Width="37"/>
                          </div>
                            </ItemTemplate>
                              <FooterTemplate>
                                     
                             <asp:TextBox ID="lblFooterAlcon" ReadOnly ="true" CssClass="footerBox" BorderWidth="0"  runat="server" Width="38"/>
                           
                             </FooterTemplate>
                          <HeaderStyle  Width="40" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TotalVol" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                              <div style="width:42px">
                               <asp:TextBox ID="lblTotVol" ReadOnly ="true" Text='<%# Bind("Q116") %>' ToolTip='<%# Bind("Q116") %>'  CssClass="footerBox1" runat="server" BorderWidth="0"  Width="40"/>
                             </div>
                            </ItemTemplate>
                                <HeaderStyle  Width="42" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SDM Reasons for Rev & Vol changes"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:185px">
                                <asp:TextBox ID="txtVolsdmRemarks" Text='<%# Bind("txtSDMBERemarks") %>' ToolTip='<%# Bind("txtSDMBERemarks") %>'
                                    Height="15" Width="182" runat="server" MaxLength="180" Font-Size="X-Small"></asp:TextBox>
                                    </div>
                            </ItemTemplate>
  <FooterTemplate>
                                   
                             <asp:TextBox ID="lblDMVolTotal"  CssClass="footerBox" BorderWidth="0"  runat="server" Width="38"/>
                        
                             </FooterTemplate>
                                    <HeaderStyle Wrap="true" Width="85" />        
                                   
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DMVolBE" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <div style="width:54px">
                                  <asp:HyperLink ID="lnkbtnDMVolTotal" runat="server" Text='<%# Bind("DMVolumeBE") %>'
                                     NavigateUrl="#" onclick="PopUpDMBE(this); return false;"></asp:HyperLink>
                                      <%--</div>--%>
                            </ItemTemplate>
                            <FooterTemplate>
                           <%-- <asp:TextBox ID="lblCompetencyDM" runat ="server" ReadOnly ="true" Text="1" CssClass="footerBox" BorderWidth="0" Width="40"></asp:TextBox>
                           --%>   
                            </FooterTemplate>
                                   <HeaderStyle Wrap="false" Width="54" />
                           
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="(Alcon)*" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                            Visible="false">
                            <ItemTemplate>
                            <div style="width:50px">
                                <asp:Label ID="lnkbtnAlconTotal" runat="server" Font-Underline="true" Text='<%# Bind("Alcon") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                           
                              <HeaderStyle  Width="50" />
                            
                        </asp:TemplateField>

                        <%-- <asp:TemplateField HeaderText="     " SortExpression="SkillType"   ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                            <div style="width:60px" class="removethiscol">
                                 <asp:HiddenField ID="hdnd" runat="server"  />
                               <asp:HyperLink ID="lnkbtnDMCompetencyVolTotal" runat="server"  Text='<%# Bind("QuarterVol") %>' 
                                     NavigateUrl="#" onclick="PopUpDMVolumeBE(this); return false;"></asp:HyperLink>
                                                        </div>         
                                      
                            </ItemTemplate>
                          
                                   <HeaderStyle Wrap="true" Width="60" />
                                   <FooterStyle BorderColor="White" />
                          
                        </asp:TemplateField>--%>
                   
                    </Columns>
                </asp:GridView>

               
                          </div> 
                           </div>
             
                  <asp:HiddenField ID="hdFreeze" runat="server" />
                     <asp:HiddenField ID="hdrefress" runat="server" />

                       <asp:GridView ID="gvSDMExcel" runat="server" ShowFooter="True" EmptyDataText="No records found"
        CssClass="mGrid" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
        Font-Names="Calibri" Font-Size="Small">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>
    <iframe id="iframe" runat="server" style="display:none"></iframe>
     </ContentTemplate>
                    <Triggers>
                      

                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlNSO" EventName="SelectedIndexChanged" />
                        <asp:PostBackTrigger ControlID="btnUpload" />
                    </Triggers>
                </asp:UpdatePanel>
  <script type="text/javascript" src="Scripts/jquery.min.js"></script> 
  <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script> 
  <%--<script type="text/javascript" src="Scripts/gridviewScroll.min.js"></script>--%>
        <script type="text/javascript" src="Scripts/gridviewScroll.js"></script>

  <script type="text/javascript">
      $(document).ready(function () {
        gridviewScroll();

      });

  

      function gridviewScroll() {
          console.log('calling -gridviewScroll()')

          var gridViewScroll = new GridViewScroll({
              elementID: 'MainContent_grdBESDMView',
              width: 1325, // Integer or String(Percentage)
              height : 370, // Integer or String(Percentage)
              freezeColumn : true, // Boolean
              freezeFooter : false, // Boolean
              freezeColumnCssClass : "", // String
              freezeFooterCssClass : "", // String
              freezeHeaderRowCount : 3, // Integer
              freezeColumnCount : 0 // Integer             
              });
          gridViewScroll.enhance();

          <%-- return false;
          $('#<%=grdBESDMView.ClientID%>').gridviewScroll({
              width: 1370,
              height: 370,
              headerrowcount: 3,
              IsInUpdatePanel: true
          });--%>


      } 
</script>
            <asp:HiddenField ID="hdnflag" runat="server" />
   
   
  

 
    
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="upSetSession">
                <ProgressTemplate>
               
                    <div id="uploadgif" class="progress">
                        <div class="center">
                             <img alt="" src="Images/load.gif" height="100" width="75"/>
                        </div>
                    </div>

                </ProgressTemplate>
            </asp:UpdateProgress>  

         </div>  
 </body>
</asp:Content>
