<%@ Page Title="SubCon Management - SubCon Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubConHome.aspx.cs" EnableEventValidation="false" ClientIDMode="Static" Inherits="BECodeProd.SubConHome" %>
   <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <meta name="DownloadOptions" content="noopen">
    <link href="Styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/Menu.js" type="text/javascript"></script>
    <script src="Scripts/BEHomeScripts.js" type="text/javascript"></script>
    <script src="Scripts/JQuery.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-datepicker
        {
            font-family: Calibri;
            font-size: 12px;
            margin-left: 10px;
        }
    </style>
    <style type="text/css">
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
            width:100px !important;
        }
        .clsMCC
        {
            width:20px;
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
            padding-left:4px;
            padding-right:4px
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
            padding-left:4px;
            padding-right:4px
        }
        .btn
        {
            font-family: Calibri; font-size: 9pt;
        }
        .mGrid
        {
         
            background-color: #fff; /* margin: 5px 0 10px 0;*/
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            font-size: 9pt;
         
        }
       .Label
        {
            font-family: Calibri;
            font-size: 9pt;
            background: none;
            
        }
    </style>
    <script type="text/javascript">

        $(function () {
            // document ready event - dom loaded completed.


            //  btnHide('Subcons - Existing')

        });

        function dpExcept() {
            debugger;
            $("[id$=dtpExpenseDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });

            $("[id$=dtpFieldDate1]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });

            $("[id$=dtpFieldDate2]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $("[id$=dtpFieldDate3]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $("[id$=dtpFieldDate4]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $("[id$=dtpFieldDate5]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $("[id$=dtpFieldDate6]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $("[id$=dtpFieldDate7]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $("[id$=dtpFieldDate8]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'

            });
            $('div.ui-datepicker').css({ 'font-size': '10px' }).css({ 'top': 'top' })
           ;
            $("#divHiddenFieldControls > img").hide();
        }

    </script>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
   <script type="text/javascript" src="http://ajax.cdnjs.com/ajax/libs/json2/20110223/json2.js"></script>


    <script type="text/javascript">

        var enity = {};
        var myArray = new Array;
        var ords = [];

        //        function btnHide(value) {
        //          
        //           

        //            if (value == "Subcons - Existing") {
        //            
        //                document.getElementById('<%= ctrl.ClientID%>').style.display = "none";
        //           }
        //            else {
        //                document.getElementById('<%= ctrl.ClientID%>').style.display = "block";

        //            }
        //        }

        function OnSuccess(response) {
            //alert(response);

            //window.location(response.d);

        }

        //        function ValidateMandatoryColumns(columns, dateControls) {

        //            var selectedQtr = document.getElementById('ddlQuarter').value;
        //            if (selectedQtr == "Q2'13") {
        //                alert('Q2-13 is not a valid quarter');
        //                document.getElementById('ddlQuarter').focus();
        //                return false;
        //            }

        //            var count = 0;
        //            var mandcolumns = columns.split(",");
        //            for (var j = 0; j < mandcolumns.length; j++) {
        //                var control = mandcolumns[j];
        //                var txtbox = document.getElementById(control);
        //                if (txtbox != null) {
        //                    if (txtbox.value == '') {
        //                        alert('Pls enter the mandatory fields');
        //                        txtbox.focus();
        //                        return false;
        //                    }
        //                    else if ((txtbox.value + '').toLowerCase() == 'select') {
        //                        alert('Pls enter the mandatory fields');
        //                        txtbox.focus();
        //                        return false;
        //                    }
        //                    else if ((txtbox.value + '').toLowerCase() == 'n/a') {
        //                        alert('Pls enter the mandatory fields');
        //                        txtbox.focus();
        //                        return false;
        //                    }
        //                    if (txtbox.value == '--Select--') {
        //                        alert('Pls enter the mandatory fields');
        //                        txtbox.focus();
        //                        return false;
        //                    }
        //                }
        //            }

        //            var count1 = 0;
        //            var _datecolumns = dateControls.split(",");
        //            for (var j = 0; j < _datecolumns.length; j++) {
        //                var control = _datecolumns[j];
        //                var txtbox = document.getElementById(control);
        //                if (txtbox != null) {
        //                    if ((txtbox.value + '').length > 0) {
        //                        var date = txtbox.value;
        //                        if (!validDate(date)) {
        //                            alert('Pls enter the valid date format [MM/dd/yyyy]');
        //                            txtbox.focus();
        //                            return false;
        //                        }
        //                        else {

        //                            // valid date. make sure the date selected is greater than yesterday..
        //                            var _today = new Date();
        //                            var m = _today.getMonth();
        //                            var d = _today.getDate();
        //                            var y = _today.getFullYear();
        //                            var today = new Date(y, m, d);

        //                            var selecteddate = GetValidDate(date);
        //                            if (selecteddate >= today) {
        //                                // true 
        //                            }
        //                            else {
        //                                alert('Date must not be less than today');
        //                                txtbox.focus();
        //                                return false;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        function validDate(text) {
            var date = Date.parse(text);
            if (isNaN(date)) { return false; }
            var comp = text.split('/');
            if (comp.length !== 3)
            { return false; }
            var m = parseInt(comp[0], 10);
            var d = parseInt(comp[1], 10);
            var y = parseInt(comp[2], 10);
            var date = new Date(y, m - 1, d);
            return (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d);
        }

        function GetValidDate(text) {
            var date = Date.parse(text);
            var comp = text.split('/');
            var m = parseInt(comp[0], 10);
            var d = parseInt(comp[1], 10);
            var y = parseInt(comp[2], 10);
            var date = new Date(y, m - 1, d);
            return date;
        }



        function PopUp(rowid) {


            var mcc = document.getElementById('<%= ddlMCC.ClientID%>');
            var mccvalue = mcc.options[mcc.selectedIndex].text;

            var Qtr = document.getElementById('<%= ddlQuarter.ClientID%>');
            var Qtrvalue = Qtr.options[Qtr.selectedIndex].text;

            var ExpenseType = document.getElementById('<%= ddlExpenseType.ClientID%>');
            var ExpenseTypevalue = ExpenseType.options[ExpenseType.selectedIndex].text;

            var CtrlValue = document.getElementById('<%= hdnCtrl.ClientID%>').value;
            var getCtrl = document.getElementById('<%= hdnCt.ClientID%>').value;

            var Ctrl = getCtrl.split(",");

            if (ExpenseTypevalue == "Subcons - Existing") {

                if (CtrlValue == "SAVE") {

                    var temp = rowid.parentNode.parentElement.cells;

                    var value = $('input', temp[33]).val();

                    var hdvalue = 0;

                    var grid = document.getElementById('MainContent_grdBEData');

                    var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;

                    var footerrow = grid.rows[grid.rows.length - 1];

                    var row2 = rowid.parentNode.parentNode;



                    gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;




                    var mandatorycol = document.getElementById('<%= hdnMandCol.ClientID%>').value;
                    var datecol = document.getElementById('<%= hdnDateCol.ClientID%>').value;



                    for (i = 0; i <= gridviewrowsLength; i++) {
                        var name = "MainContent_grdBEData_chkRow_" + i;
                        var hdnname = "MainContent_grdBEData_hdnfld_" + i;
                        var chk = document.getElementById(name);
                        var hdv = document.getElementById(hdnname).value


                        if (hdv == value) {

                            var count = 0;
                            var mancol = mandatorycol.split(",");

                            for (var j = 0; j < mancol.length; j++) {
                                var control = mancol[j];
                                var ctrlname = "MainContent_grdBEData_" + control + "_" + i;
                                var txtbox = document.getElementById(ctrlname);

                                if (txtbox != null) {

                                    if (txtbox.value == '') {
                                        alert('Pls enter the mandatory fields');
                                        txtbox.focus();
                                        return false;
                                    }
                                    else if ((txtbox.value + '').toLowerCase() == 'select') {
                                        alert('Pls enter the mandatory fields');
                                        txtbox.focus();
                                        return false;
                                    }
                                    else if ((txtbox.value + '').toLowerCase() == 'n/a') {
                                        alert('Pls enter the mandatory fields');
                                        txtbox.focus();
                                        return false;
                                    }
                                    if (txtbox.value == '--Select--') {
                                        alert('Pls enter the mandatory fields');
                                        txtbox.focus();
                                        return false;
                                    }

                                }
                            }

                            var count1 = 0;
                            var _datecolumns = datecol.split(",");

                            for (var j = 0; j < _datecolumns.length; j++) {
                                var control = _datecolumns[j];
                                var ctrlname = "MainContent_grdBEData_" + control + "_" + i;
                                var datebox = document.getElementById(ctrlname);

                                if (datebox != null) {

                                    if ((datebox.value + '').length > 0) {
                                        var txtboxdate = datebox.value;

                                        if (!validDate(txtboxdate)) {
                                            alert('Pls enter the valid date format [MM/dd/yyyy]');
                                            datebox.focus();
                                            return false;
                                        }
                                        else {

                                            // valid date. make sure the date selected is greater than yesterday..
                                            var _today = new Date();
                                            var m = _today.getMonth();
                                            var d = _today.getDate();
                                            var y = _today.getFullYear();
                                            var today = new Date(y, m, d);

                                            var selecteddate = GetValidDate(txtboxdate);

                                            if (selecteddate >= today) {
                                                // true 
                                            }
                                            else {

                                                alert('Date must not be less than today');
                                                datebox.focus();
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }



                            var columns = document.getElementById('<%= hdnCol.ClientID%>').value;

                            var name = "MainContent_grdBEData_chkRow_" + i;
                            var constcontrolprefix = "MainContent_grdBEData_";

                            // alert(columns);
                            var mandcolumns = columns.split(",");

                            var enitys = [];






                            for (var k = 2; k < mandcolumns.length; k++) {


                                var control = Ctrl[k];
                                var ctrlname = "MainContent_grdBEData_" + control + "_" + i;
                                var ControlVal = document.getElementById(ctrlname);

                                if (control != "") {

                                    if (ControlVal != null && ControlVal.type == "text") {


                                        enity[mandcolumns[k]] = $('input', temp[k + 2]).val();


                                    }

                                    if (ControlVal != null && ControlVal.type == "select-one") {

                                        var no = document.getElementById(ControlVal.id);
                                        var option = no.options[no.selectedIndex].text;

                                        enity[mandcolumns[k]] = $('select', temp[k + 2]).val();

                                    }

                                }
                            }
                            enity["intExpId"] = $('input', temp[33]).val();
                            enity["DMMailId"] = $('input', temp[3]).val();
                            enity["TotalAmt"] = $('input', temp[2]).val();
                            enity["FieldDate3"] = $('input', temp[10]).val();
                            enity["ClientCode"] = $('select', temp[11]).val();
                            enity["FieldList7"] = $('select', temp[7]).val();
                            enity["Fieldtxt8"] = $('input', temp[30]).val();

                            enitys.push(enity);
                            PageMethods.Data(enitys, OnSuccess);
                        }
                    }
                    alert("saved successfully!!!");

                    window.location.reload();
                    return false;
                }
                else {

                    EditViewPopup(rowid)
                    window.location.reload();
                    return false;
                }
            }
            else {

                EditViewPopup(rowid)
                window.location.reload();
                return false;


            }
        }



        function EditViewPopup(rowid) {

            var isView = '0';
            isView = rowid.innerText == 'VIEW' ? "1" : "0";
            var no = rowid.id.split('_')[3];
            var hndfldname = 'MainContent_grdBEData_hdnfld_' + no;
            var ctrl = document.getElementById(hndfldname);
            var expid = ctrl.value;
            var hdnvalue = document.getElementById('MainContent_hdnPass').value;
            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;

            var heighttt = document.getElementById('MainContent_hdnpopupHeight').value;


            window.open('SubConAddEdit.aspx?Mode=AddEdit&IsAddNew=0&Expid=' + expid + '&view=' + isView, 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=700, height=' + heighttt + ', menubar=no, scrollbars=no, resizable=no');
            return false;
        }

        function PopUpAddNew() {
            debugger;


            var left = (screen.width - 700) / 2;
            //var top = (screen.height - 300) / 2;

            var hdnvalue = '';
            //            var pu = document.getElementById('MainContent_ddlPU').value;
            //            var du = document.getElementById('MainContent_ddlDM').value;
            var exptype = document.getElementById('MainContent_ddlExpenseType').value;
            var qtr = document.getElementById('MainContent_ddlQuarter').value;

            exptype = exptype.replace("&", "%26");
            exptype = exptype.replace(" ", "%20");

            var hdnvalue = /*'&PU=' + pu + '&DU=' + du +*/'&ExpenseType=' + exptype + '&Quarter=' + qtr;

            var heighttt = document.getElementById('MainContent_hdnpopupHeight').value;

            window.open('SubConAddEdit.aspx?Mode=AddEdit&Expid=0&IsAddNew=1' + hdnvalue, 'ThisPopUp', 'left = ' + left + ', width=700, height=' + heighttt + ', menubar=no, scrollbars=no, resizable=no');

            return false;




        }

        //        function PopUpOtherExpenses() {

        //            var hdnvalue = '';  // document.getElementById('MainContent_hdnPass').value;
        //            var left = (screen.width - 700) / 2;
        //            var top = (screen.height - 300) / 2;
        //            // window.showModalDialog('SubConHomeSubConHome.aspx?Mode=AddEdit&Expid=0&IsAddNew=1' + hdnvalue + '', 'bow', 'dialogHeight:' + heighttt + '; dialogWidth:50;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');

        //            // window.open('AddOtherExpenses.aspx?' + hdnvalue, 'ThisPopUp', 'left = 150, top=50, width=300, height=150, menubar=no, scrollbars=yes, resizable=no');
        //            window.open('SubConHome.aspx?Mode=OtherExp' + hdnvalue, 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=700, height=90, menubar=no, scrollbars=no, resizable=no');

        //            // window.showModalDialog("SubConHome.aspx?Mode=OtherExp", 'bow', "dialogHeight:6; dialogWidth:42;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no");

        //            return false;
        //        } 

        function PopUpOtherExpenses() {
            debugger;
            // ClearMessage();
            var hdnvalue = '';
            var left = (screen.width - 700) / 2;
            //  var top = (screen.height - 300) / 2;
            if (typeof winPopup === 'undefined') {
                winPopup = window.open('SubConAddEdit.aspx?Mode=OtherExp' + hdnvalue, 'ThisPopUp', 'left = ' + left + ', width=700, height=90, menubar=no, scrollbars=no, resizable=no');
            }

            else {
                winPopup.focus();
                //                If window.focus stops working      
                // winPopup.close();               
                //   winPopup = window.open('SubConHome.aspx?Mode=OtherExp' + hdnvalue, 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=700, height=90, menubar=no, scrollbars=no, resizable=no');
            }
            return false;

        }

       

    </script>
    <script type="text/javascript">
        function ValidateCopyRow() {


            var count = 0; var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length - 1 == 0) {
                alert('not a valid operation');
                return false;
            }
            var id = 0;
            var copycontrol = document.getElementById('MainContent_hdnCopiedID');
            var csv = '';

            var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;
            var footerrow = grid.rows[grid.rows.length - 1];


            gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;

            for (i = 0; i < gridviewrowsLength; i++) {
                var name = "MainContent_grdBEData_chkRow_" + i;
                var hdnname = "MainContent_grdBEData_hdnfld_" + i;
                var chk = document.getElementById(name);
                if (chk != null) {
                    var isChecked = chk.checked;
                    if (isChecked) {
                        count = count + 1;
                        id = i;
                        var valu = document.getElementById(hdnname).value;
                        csv += valu + ',';
                    }

                }
            }

            copycontrol.value = csv;


            if (count > 0) {

                //                // don t go to this loop..
                //                var name = 'MainContent_grdBEData_hdnfld_' + id;
                //                var copyID = document.getElementById(name).value;
                //                var copycontrol = document.getElementById('MainContent_hdnCopiedID');
                //                copycontrol.value = copyID;
                return true;
            }
            else {
                if (count == 0) alert('Please select a row');
                else if (count > 100) alert('Please select single row'); // don go to this loop 
                return false;
            }



        }

    </script>
    <script type="text/javascript">
        function ColumnsReordering(columns) {
            var count = 0;
            var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length == 0) {
                alert('not a valid operation');
                return false;
            }
            for (i = 0; i < grid.rows.length - 1; i++) {


                var name = "MainContent_grdBEData_chkRow_" + i;
                var constcontrolprefix = "MainContent_grdBEData_";
                var chk = document.getElementById(name);
                if (chk != null) {
                    var isChecked = chk.checked;
                    if (isChecked) {
                        count = count + 1;

                        var mandcolumns = columns.split(",");

                        for (var j = 0; j < mandcolumns.length; j++) {
                            var control = constcontrolprefix + mandcolumns[j] + '_' + i;
                            var txtbox = document.getElementById(control);
                            if (txtbox != null) {


                                if (txtbox.value == '') {
                                    alert('Pls enter the mandatory fields');
                                    txtbox.focus();
                                    return false;
                                }
                            }



                        }
                    }
                }
            }
            if (count > 0)
                return true;
            else {
                if (count == 0)
                    alert('Please select a row');




                return false;
            }


        }
   
    </script>
      <script type="text/javascript">
          function mandatoryDropDown(columns) {
              var txtbox = document.getElementById(ddlFieldList3);

              alert('Pls enter the mandatory fields');
              txtbox.focus();
              return false;

          }
        </script>
    <script type="text/javascript">
        function ValidateCheckBoxes(columns) {
            var count = 0;
            var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length == 0) {
                alert('not a valid operation');
                return false;
            }
            var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;
            var footerrow = grid.rows[grid.rows.length - 1];


            gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;


            for (i = 0; i < gridviewrowsLength; i++) {

                var name = "MainContent_grdBEData_chkRow_" + i;
                var constcontrolprefix = "MainContent_grdBEData_";
                var chk = document.getElementById(name);
                if (chk != null) {
                    var isChecked = chk.checked;
                    if (isChecked) {
                        count = count + 1;

                        var mandcolumns = columns.split(",");

                        for (var j = 0; j < mandcolumns.length; j++) {
                            var control = constcontrolprefix + mandcolumns[j] + '_' + i;
                            var txtbox = document.getElementById(control);
                            if (txtbox != null) {


                                if (txtbox.value == '') {
                                    alert('Pls enter the mandatory fields');
                                    txtbox.focus();
                                    return false;
                                }
                            }



                        }
                    }
                }
            }
            if (count > 0)
                return true;
            else {
                if (count == 0)
                    alert('Please select a row');




                return false;
            }


        }
   
    </script>
    <script type="text/javascript">

        function ChkDataIsPresent() {
            var count = 0;
            var grid = document.getElementById('MainContent_grdBEData');
            if (grid == null) {
                alert('not a valid operation');
                return false;
            }
            else if (grid.rows.length - 1 == 0) {
                alert('not a valid operation');
                return false;
            }
            else
                return true;
        }

        function ValidateDeleteRow() {


            var count = 0; var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length - 1 == 0) {
                alert('not a valid operation');
                return false;
            }
            var id = 0;
            var copycontrol = document.getElementById('MainContent_hdnCopiedID');
            var csv = '';
            var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;
            var footerrow = grid.rows[grid.rows.length - 1];
            gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;

            for (i = 0; i < gridviewrowsLength; i++) {
                var name = "MainContent_grdBEData_chkRow_" + i;
                var hdnname = "MainContent_grdBEData_hdnfld_" + i;
                var chk = document.getElementById(name);
                if (chk != null) {
                    var isChecked = chk.checked;
                    if (isChecked) {
                        count = count + 1;
                        id = i;
                        var valu = document.getElementById(hdnname).value;
                        csv += valu + ',';
                    }

                }
            }

            copycontrol.value = csv;

            if (count == 0) {
                alert('Please select a row to delete');
                return false;
            }
            else {
                var Ok = confirm('Are you sure you want to delete the selected record(s)?');

                if (Ok) return true;
                else return false;

            }





        }

    </script>
  <%--  <script type="text/javascript">
        // created by karthik_mahalingam01 sept 15 2012 
        //  Purpopse: cascading drop down calling server side event in JS. to avoid post back :-)
        function PopulateDM(ctrl) {

            var puvalue = ctrl.value;
            var hdnflddmcsv = document.getElementById('MainContent_hdnfldDMCSV');
            var hdnfldddlDM = document.getElementById('MainContent_hdnfldddlDM');


            // clearing the ddl items 
            var ddl = document.getElementById('MainContent_ddlDM');
            ddl.options.length = 0;
            for (var j = 0; j < ddl.options.length; j++) {
                ddl.options.remove(j);
            } //

            var checkCount = 0;

            var csvItemarray = hdnflddmcsv.value.split('|');
            var i = 0;
            for (var i = 0; i < csvItemarray.length; i++) {

                var rowcsvarray = csvItemarray[i].split(',');
                if (rowcsvarray[0] == puvalue) {
                    var respectedDm = rowcsvarray[1];
                    checkCount++;
                    // updating the first item in ddl to the hidden field

                    if (checkCount == 1)
                        hdnfldddlDM.value = respectedDm;



                    var option = document.createElement("option");
                    option.text = respectedDm;
                    option.value = respectedDm;
                    try {
                        ddl.add(option, null); //Standard   
                    } catch (error) {
                        ddl.add(option); // IE only 
                    }

                }
            }

            //alert(hdnfldddlDM.value);   //testing purpose
        }


        function UpdateDMValue(ctrl) {
            var hdnfldddlDM = document.getElementById('MainContent_hdnfldddlDM');
            hdnfldddlDM.value = ctrl.value;
            //alert(hdnfldddlDM.value);  //testing purpose
        } 


    </script>--%>
  <%--  <script type="text/javascript">

        function UpdateSDMPhase1Data(_amount, _remainingPagesTotal) {

            var amount = parseFloat(_amount);
            var remainingPagesTotal = parseFloat(_remainingPagesTotal);

            var count = 0; var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length - 1 == 0) {
                alert('not a valid operation');
                return false;
            }

            var id = 0;
            var copycontrol = document.getElementById('MainContent_hdnfldSDMDHPhase1Data');
            var csv = '';

            var temptotal = 0;
            var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;
            var footerrow = grid.rows[grid.rows.length - 1];
            gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;

            //            var row = grid.rows[grid.rows.length - 1];
            for (i = 0; i < gridviewrowsLength; i++) {
                var name = "MainContent_grdBEData_chkRow_" + i;
                var hdnname = "MainContent_grdBEData_hdnfld_" + i;
                var sdmdhstatusname = 'MainContent_grdBEData_ddlSDMStatus_' + i;
                var txtApprovedamtname = 'MainContent_grdBEData_txtSDMApprovedAmount_' + i;

                // dh details
                var dhstatusname = 'MainContent_grdBEData_ddlDHStatus_' + i;
                var txtdhamountname = 'MainContent_grdBEData_txtDHApprovedAmount_' + i;

                // pna details
                var pnastatusname = 'MainContent_grdBEData_ddlPNAStatus_' + i;
                var txtpnaamountname = 'MainContent_grdBEData_txtPNAApprovedAmount_' + i;


                var priorityname = 'MainContent_grdBEData_ddlPriority_' + i;


                var chk = document.getElementById(name);
                // if (chk != null) {
                if (true) {
                    // var isChecked = chk.checked;
                    var isChecked;
                    isChecked = true;
                    if (isChecked) {
                        count = count + 1;
                        id = i;
                        var intexpid = document.getElementById(hdnname).value;
                        var sdmstatus = document.getElementById(sdmdhstatusname).value;
                        var approveamt = document.getElementById(txtApprovedamtname).value;

                        var dhstatus = document.getElementById(dhstatusname).value;
                        var dhamount = document.getElementById(txtdhamountname).value;

                        var pnastauts = document.getElementById(pnastatusname).value;
                        var pnaamount = document.getElementById(txtpnaamountname).value;



                        if (pnastauts == 'Frozen' || pnastauts == 'Not Approved') {
                            var temppnaamount = pnaamount == '' ? 0 : parseFloat(pnaamount);
                            temptotal += temppnaamount
                        }
                        else if (pnastauts == "On Hold") {
                            if (dhstatus == 'Approved' || dhstatus == 'Not Approved') {
                                var tempdhamount = dhamount == '' ? 0 : parseFloat(dhamount);
                                temptotal += tempdhamount;
                            }
                            else if (dhstatus == 'On Hold') {

                                if (sdmstatus == 'Approved') {
                                    var tempsdmamount = approveamt == '' ? 0 : parseFloat(approveamt);
                                    temptotal += tempsdmamount
                                }
                            }
                        }


                        if (approveamt == '') {
                            document.getElementById(txtApprovedamtname).focus();
                            alert('Pls enter the amount');
                            return false;
                        }
                        if (sdmstatus == '') {
                            alert('Pls select the status'); document.getElementById(sdmdhstatusname).focus(); return false;
                        }

                        var tempsdmamount = approveamt == '' ? 0 : parseFloat(approveamt);
                        if (sdmstatus == 'Approved' && tempsdmamount == 0) {
                            alert('Amount should be greater than 0');
                            document.getElementById(txtApprovedamtname).focus();
                            return false;
                        }


                        var priority = document.getElementById(priorityname).value;
                        csv += intexpid + '|' + sdmstatus + '|' + approveamt + '|' + priority + '&';
                    }

                }
            }

            temptotal += remainingPagesTotal;

            if (temptotal > amount) {
                alert('Amount exceeded the Budget limit');
                return false;
            }

            copycontrol.value = csv;

            if (count == 0)
                return false;
            else
                return true;

        }
    </script>--%>
   <%-- <script type="text/javascript">

        function UpdatePNAhase2Data(_amount, _remainingPagesTotal) {

            var remainingPagesTotal = parseFloat(_remainingPagesTotal);
            var amount = parseFloat(_amount);
            var count = 0; var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length - 1 == 0) {
                alert('not a valid operation');
                return false;
            }
            var temptotal = 0;
            var id = 0;
            var copycontrol = document.getElementById('MainContent_hdnfldPNAPhase2Data');
            var csv = '';
            var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;
            var footerrow = grid.rows[grid.rows.length - 1];
            gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;


            for (i = 0; i < gridviewrowsLength; i++) {
                var name = "MainContent_grdBEData_chkRow_" + i;
                var hdnname = "MainContent_grdBEData_hdnfld_" + i;
                var pnastatus = 'MainContent_grdBEData_ddlPNAStatus_' + i;
                var pnaApprovedAmtctrl = 'MainContent_grdBEData_txtPNAApprovedAmount_' + i;





                var chk = document.getElementById(name);
                // if (chk != null) {
                if (true) {
                    //var isChecked = chk.checked;
                    var isChecked;
                    isChecked = true;
                    if (isChecked) {
                        count = count + 1;
                        id = i;
                        var intexpid = document.getElementById(hdnname).value;
                        var pnastatus = document.getElementById(pnastatus).value;
                        var pnaapprovedamt = document.getElementById(pnaApprovedAmtctrl).value;

                        if (pnastatus == 'Frozen') {
                            var tempappamt = pnaapprovedamt == '' ? 0 : parseFloat(pnaapprovedamt);
                            temptotal += tempappamt
                        }

                        if (pnaapprovedamt == '') {
                            document.getElementById(pnaApprovedAmtctrl).focus();
                            alert('Pls enter the amount');
                            return false;
                        }


                        if (pnastatus == '') {
                            alert('Pls select the status'); document.getElementById(pnastatus).focus(); return false;
                        }

                        var tempappamt = pnaapprovedamt == '' ? 0 : parseFloat(pnaapprovedamt);
                        if (pnastatus == 'Frozen' && tempappamt == 0) {
                            alert('Amount should be greater than 0');
                            document.getElementById(pnaApprovedAmtctrl).focus();
                            return false;
                        }

                        csv += intexpid + '|' + pnastatus + '|' + pnaapprovedamt + '&';

                    }
                }
            }


            copycontrol.value = csv;

            temptotal += remainingPagesTotal;

            if (temptotal > amount) {
                alert('Amount exceeded the budget limit');
                return false;

                if (count == 0)
                    return false;
                else
                    return true;
            }
        }
    </script>--%>
   <%-- <script type="text/javascript">


        function UpdateDHMPhase1Data(amt, _remainingPagesTotal) {

            var remainingPagesTotal = parseFloat(_remainingPagesTotal);
            var amount = parseFloat(amt);
            var count = 0;
            var grid = document.getElementById('MainContent_grdBEData');
            if (grid.rows.length - 1 == 0) {
                alert('not a valid operation');
                return false;
            }

            var id = 0;
            var copycontrol = document.getElementById('MainContent_hdnfldDHPhase2Data');
            var csv = '';
            var temptotal = 0;
            var gridviewrowsLength = grid.rows.length == 13 ? 11 : grid.rows.length - 1;
            var footerrow = grid.rows[grid.rows.length - 1];

            gridviewrowsLength = footerrow.className == 'pgr' ? gridviewrowsLength - 1 : gridviewrowsLength;


            for (i = 0; i < gridviewrowsLength; i++) {
                var name = "MainContent_grdBEData_chkRow_" + i;
                var hdnname = "MainContent_grdBEData_hdnfld_" + i;
                var sdmdhstatusname = 'MainContent_grdBEData_ddlDHStatus_' + i;
                var txtApprovedamtname = 'MainContent_grdBEData_txtDHApprovedAmount_' + i;
                var priorityname = 'MainContent_grdBEData_ddlPriority_' + i;

                var pnastatusname = 'MainContent_grdBEData_ddlPNAStatus_' + i;
                var pnaamountname = 'MainContent_grdBEData_txtPNAApprovedAmount_' + i;


                var chk = document.getElementById(name);
                //if (chk != null) {
                if (true) {


                    //    var isChecked = chk.checked;
                    var isChecked;
                    isChecked = true;
                    if (isChecked) {
                        count = count + 1;
                        id = i;
                        var intexpid = document.getElementById(hdnname).value;
                        var sdmstatus = document.getElementById(sdmdhstatusname).value;
                        var approveamt = document.getElementById(txtApprovedamtname).value;

                        var pnastatus = document.getElementById(pnastatusname).value;
                        var pnaamount = document.getElementById(pnaamountname).value;

                        //                        if (sdmstatus == 'Approved') {
                        //                            var tempappamt = approveamt == '' ? 0 : parseFloat(approveamt);
                        //                            temptotal += tempappamt
                        //                        }

                        if (approveamt == '') {
                            document.getElementById(txtApprovedamtname).focus();
                            alert('Pls enter the amount');
                            return false;
                        }
                        if (sdmstatus == '') {
                            alert('Pls select the status'); document.getElementById(sdmdhstatusname).focus(); return false;
                        }

                        var tempappamt = approveamt == '' ? 0 : parseFloat(approveamt);
                        if (sdmstatus == 'Approved' && tempappamt == 0) {
                            alert('Amount should be greater than 0');
                            document.getElementById(txtApprovedamtname).focus();
                            return false;
                        }


                        if (pnastatus == 'Frozen' || pnastatus == 'Not Approved') {
                            var temppnaamount = pnaamount == '' ? 0 : parseFloat(pnaamount);
                            temptotal += temppnaamount
                        }
                        else if (pnastatus == 'On Hold') {
                            if (sdmstatus == 'Approved') {
                                var tempappamt = approveamt == '' ? 0 : parseFloat(approveamt);
                                temptotal += tempappamt
                            }
                        }




                        var priority = document.getElementById(priorityname).value;
                        csv += intexpid + '|' + sdmstatus + '|' + approveamt + '|' + priority + '&';
                    }
                }
            }

            temptotal += remainingPagesTotal;
            if (temptotal > amount) {
                alert('Amount exceeded the budget limit');
                return false;
            }

            copycontrol.value = csv;

            if (count == 0)
                return false;
            else
                return true;
        } 
        
      

    </script>--%>
    <script type="text/javascript">
        function ChangeOver(thisID, control, valu) {

            var id = thisID.id.split('_')[3];
            var textControlname = 'MainContent_grdBEData_' + control + '_' + id;
            var textControl = document.getElementById(textControlname);
            var selectedValue = thisID.value;
            if (selectedValue == 'Not Approved' || selectedValue == 'On Hold')
                textControl.value = 0;
        }

    </script>
    <script type="text/javascript">
        function PressfloatOnlyCustomLogic(evt, thisobj, controlID) {

            var id = thisobj.id.split('_')[3];
            var ddlControlname = 'MainContent_grdBEData_' + controlID + '_' + id;
            var ddl = document.getElementById(ddlControlname);
            var selectedValue = ddl.value;
            if (selectedValue == 'Not Approved' || selectedValue == 'On Hold')
                return false;

            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (evt.shiftKey == true)
                if (charCode > 47 && charCode < 61)
                    return false;

            var textboxValue = thisobj.value + "";

            if (charCode == 17 || charCode == 67)
                return true;
            if (charCode == 17 || charCode == 86)
                return true;
            if (charCode == 17 || charCode == 88)
                return true;

            if (charCode == 190 || charCode == 110) {
                var contains = textboxValue.indexOf(".") != -1;
                if (contains)
                    return false;
            }

            if (charCode == 37 || charCode == 39) return true;  // allow arrows

            if (charCode == 46) return true; //delete

            if (charCode == 190 || charCode == 110) return true; // period or dot


            if (charCode == 35 || charCode == 36) return true; // home, end 


            if (charCode == 8 || charCode == 9) return true; // backspace , tab 


            if (charCode > 47 && charCode < 58) return true; //0-9

            if (charCode > 95 && charCode < 106) return true; //0-9



            return false;
        }

 

    </script>
    <script type="text/javascript">

       

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var div = document.getElementById('MainContent_divgrid');
            if (div != null) {
                div.style.width = (window.screen.width - 160) + 'px';
                div.style.height = (window.screen.height - 420) + 'px'; // address bar, favorites, tool bar , status bar 
            }
        });
            
    </script>
    <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            overflow-y: auto;
            padding: 0 0 0 0;
            width:1300px !important;
            padding-left:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:RoundedCornersExtender ID="RoundedCornersExtender1" BorderColor="White" Radius="10"
                Corners="All" TargetControlID="pnlGrid" runat="server">
            </asp:RoundedCornersExtender>
            <asp:HiddenField ID="hdnfldddlDM" runat="server" />
            <asp:HiddenField ID="hdnfldDMCSV" runat="server" />
            <asp:HiddenField ID="hdnfldSDMDHPhase1Data" runat="server" />
            <asp:HiddenField ID="hdnfldPNAPhase2Data" runat="server" />
            <asp:HiddenField ID="hdnfldDHPhase2Data" runat="server" />
            <asp:HiddenField ID="hdnfld1DHallocated" runat="server" />
            <asp:HiddenField ID="hdnfld2shortlistableamount" runat="server" />
            <asp:HiddenField ID="hdnfld3askedamount" runat="server" />
            <asp:HiddenField ID="hdnfld4finalisedamount" runat="server" />
            <asp:HiddenField ID="hdnfldMessage" runat="server" />
            <div style="background-color: #adaba6">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <div id="maindiv" style="background-color: #adaba6">
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Panel ID="pnlGrid" Width="100%" runat="server" BackColor="white">
                                                <div style="margin:20px">
                                                    <table cellpadding="0" cellspacing="0" width="100%">


                                                        <tr id="trowFilter" runat="server">
                                                            <td colspan="2" style="border-color: #00FF00">
                                                                <table style="font-family: Calibri; font-size: 9pt;width:100%">
                                                                    <tr>
                                                                 
                                                                        
<%--                                                                        <td style="width: 25px; font-weight: bold;">
                                                                            Practice
                                                                        </td>
                                                                        <td class="FormControls">
                                                                            <asp:DropDownList ID="ddlPU" CssClass="FormControls" Width="100px" runat="server"
                                                                                onchange="PopulateDM(this);" Visible="False">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 25px; font-weight: bold;">
                                                                            DM
                                                                        </td>
                                                                        <td class="FormControls">
                                                                            <asp:DropDownList ID="ddlDM" runat="server" onchange="UpdateDMValue(this)" Width="180px"
                                                                                CssClass="FormControls" Visible="False">
                                                                            </asp:DropDownList>
                                                                        </td>--%>
                                                                        <div style="float:left;padding-left:10px">MCC</div>
                                                                        <div style="float:left;margin-left:10px"><asp:DropDownList ID="ddlMCC" runat="server" CssClass="btn"
                                                                                 Visible="True">
                                                                            </asp:DropDownList></div>
                                                                            <div style="float:left;margin-left:10px"> Type</div>

                                                                            <div style="float:left;margin-left:10px"> <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="btn">
                                                                            </asp:DropDownList></div>
                                                                            <div style="float:left;margin-left:10px">Quarter</div>
                                                                            <div style="float:left;margin-left:10px"> <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="btn">                                                                       
                                                                    
                                                                            </asp:DropDownList></div>

                                                                              <div style="float:left;margin-left:20px;margin-right:5px">   
                                                                             <asp:Button ID="btnSearch" runat="server" CssClass="button" 
                                                                                 Font-Underline="false" OnClick="btnSearch_Click" 
                                                                              Style="background-image: url(/Images/search.png); background-position: 2px; background-repeat: no-repeat" 
                                                                                  Text="&nbsp;&nbsp;&nbsp;&nbsp;Search " />&nbsp;
                                                                                 </div>
                                                                                 <div id="ctrl" runat="server" style="float:left">
                                                                           <div style="float:left">
                                                                          
                                                                             <asp:Button ID="btnAddNew" runat="server" CssClass="button" 
                                                                                 onclick="btnAddNew_Click1" OnClientClick=" return PopUpAddNew();" 
                                                                                  Style="background-image: url(/Images/add.gif); background-position: 2px; background-repeat: no-repeat" 
                                                                                 Text="&nbsp;&nbsp;&nbsp;Add New " 
                                                                                 ToolTip="To Add new budget requirement under the same SubCon type" /> &nbsp;
                                                             </div>
                                                                           <div style="float:left">   <asp:Button ID="btnCopyRow" runat="server" CssClass="button" 
                                                                                 OnClick="btnCopyRow_Click" OnClientClick="return ValidateCopyRow();" 
                                                                                 Style="background-image: url(/Images/copy.png); background-position: 2px; background-repeat: no-repeat" 
                                                                                 Text="&nbsp;&nbsp;&nbsp;Copy Row " /> &nbsp;</div>
                                                                           <div style="float:left">
                                                                             <asp:Button ID="btnDelete"  runat="server" Text="    Delete " CssClass="button"

                                                                              Style="background-image: url(/Images/delete.png); background-position: 2px;
                                                                    background-repeat: no-repeat"
                                                                    OnClientClick=" return ValidateDeleteRow();" OnClick="btnDelete_Click" />
                                                                                                         
                                                                            &nbsp;</div>
                                                                            </div>
                                                                            <div style="float:left;margin-left:20px"> <asp:ImageButton ID="lnkExportExcel" runat="server" Width="20" Height="20" OnClick="lnkExportExcel_Click"
                                                                    ToolTip="Export Expense Data" ImageUrl="~/Images/exportexcel.bmp" /></div>
                                                                       
                                                                       
                                                                        <td style="width: 450px; ">
                                                                       
                                                                        </td>
                                                                       
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                            <div style="float:left">
                                                                <asp:Button ID="btnSave" Style="background-image: url(/Images/save.png); background-position: 2px;
                                                                    background-repeat: no-repeat" runat="server" Text="      Save " CssClass="button"
                                                                    OnClick="btnSave_Click" /></div>
                                                             <div style="float:left;margin-left:5px">
                                                                <asp:Button ID="hypAddOtherExpenses" Style="background-image: url(/Images/addother.png); 
                                                                    background-position: 2px; background-repeat: no-repeat" Text="Add Other Expenses "
                                                                    CssClass="button" runat="server" 
                                                                    ToolTip="To Add budget requirement under other SubCon type" 
                                                                    onclick="hypAddOtherExpenses_Click" Visible="False"></asp:Button></div>

                                                                     <asp:HiddenField ID="hdnPass" runat="server" />
                                                                <asp:HiddenField ID="hdnpopupHeight" Value="300" runat="server" />
                                                                <asp:HiddenField ID="hdnCopiedID" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                      
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <div id="divgrid" class="GridDock" runat="server">
                                                                    <asp:GridView ID="grdBEData" runat="server" AutoGenerateColumns="False" 
                                                                        EmptyDataText="No records found" AlternatingRowStyle-CssClass="alt" 
                                                                        CssClass="mGrid" AllowPaging="True"
                                                                        PagerStyle-CssClass="pgr" 
                                                                        OnPageIndexChanging="grdBEData_PageIndexChanging" PageSize="11"
                                                                        OnRowDataBound="grdBEData_RowDataBound">
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkRow" runat="server" />
                                                                                </ItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    Select
                                                                                </HeaderTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                     <asp:LinkButton ID="hypEdit" Text="Save" CssClass="Label" OnClientClick=" return PopUp(this);"
                                                                                        runat="server" Font-Underline="false"></asp:LinkButton>
                                                                                                                                                                      
                                                                                </ItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    Edit
                                                                                </HeaderTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ClientCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<%--                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtClientCode" ToolTip='<%# Bind("ClientCode") %>' Text='<%# Bind("ClientCode") %>'
                                                                                         CssClass="Label AutoCompleteTextBox" Height="15" Width="140" Enabled="false"
                                                                                        runat="server"></asp:TextBox>
                                                                                 </ItemTemplate>--%>
                                                                                 <ItemTemplate>
                                                                                    <asp:DropDownList ID="ClientCode" CssClass="Label" Height="15" Width="40"
                                                                                        runat="server">
                                                                                    </asp:DropDownList>                                                                                
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center"/>
                                                                                <ItemStyle HorizontalAlign="Center"/>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ItemName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtItemName" Text='<%# Bind("ItemName") %>' ToolTip='<%# Bind("ItemName") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="NumberofItems" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtNumberofItems" Text='<%# Bind("NumberofItems") %>' ToolTip='<%# Bind("NumberofItems") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="UnitCost" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtUnitCost" Text='<%# Bind("UnitCost") %>' ToolTip='<%# Bind("UnitCost") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="JustificationRemarks" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtJustificationRemarks" Text='<%# Bind("JustificationRemarks") %>'
                                                                                        ToolTip='<%# Bind("JustificationRemarks") %>' 
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ProjOppCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtProjOppCode" Text='<%# Bind("ProjOppCode") %>' ToolTip='<%# Bind("ProjOppCode") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="BEUpside" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtBEUpside" Text='<%# Bind("BEUpside") %>' ToolTip='<%# Bind("BEUpside") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="BEDownside" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtBEDownside" Text='<%# Bind("BEDownside") %>' ToolTip='<%# Bind("BEDownside") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="CurrQtr" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCurrQtr" Text='<%# Bind("CurrQtr") %>' ToolTip='<%# Bind("CurrQtr") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FutQtrBE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFutQtrBE" Text='<%# Bind("FutQtrBE") %>' ToolTip='<%# Bind("FutQtrBE") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt1" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt1" Text='<%# Bind("Fieldtxt1") %>' ToolTip='<%# Bind("Fieldtxt1") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt2" Text='<%# Bind("Fieldtxt2") %>' ToolTip='<%# Bind("Fieldtxt2") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt3" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt3" Text='<%# Bind("Fieldtxt3") %>' ToolTip='<%# Bind("Fieldtxt3") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt4" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt4" Text='<%# Bind("Fieldtxt4") %>' ToolTip='<%# Bind("Fieldtxt4") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt5" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt5" Text='<%# Bind("Fieldtxt5") %>' ToolTip='<%# Bind("Fieldtxt5") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt6" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt6" Text='<%# Bind("Fieldtxt6") %>' ToolTip='<%# Bind("Fieldtxt6") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt7" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt7" Text='<%# Bind("Fieldtxt7") %>' ToolTip='<%# Bind("Fieldtxt7") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt8" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt8" Text='<%# Bind("Fieldtxt8") %>' ToolTip='<%# Bind("Fieldtxt8") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt9" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt9" Text='<%# Bind("Fieldtxt9") %>' ToolTip='<%# Bind("Fieldtxt9") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt10" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt10" Text='<%# Bind("Fieldtxt10") %>' ToolTip='<%# Bind("Fieldtxt10") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt11" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt11" Text='<%# Bind("Fieldtxt11") %>' ToolTip='<%# Bind("Fieldtxt11") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Fieldtxt12" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt12" Text='<%# Bind("Fieldtxt12") %>' ToolTip='<%# Bind("Fieldtxt12") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>

                                                                             <asp:TemplateField HeaderText="Fieldtxt13" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt13" Text='<%# Bind("Fieldtxt13") %>' ToolTip='<%# Bind("Fieldtxt13") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                          
                                                                              <asp:TemplateField HeaderText="Fieldtxt14" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt14" Text='<%# Bind("Fieldtxt14") %>' ToolTip='<%# Bind("Fieldtxt14") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                             <asp:TemplateField HeaderText="Fieldtxt15" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt15" Text='<%# Bind("Fieldtxt15") %>' ToolTip='<%# Bind("Fieldtxt15") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>

                                                                              <asp:TemplateField HeaderText="Fieldtxt16" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt16" Text='<%# Bind("Fieldtxt16") %>' ToolTip='<%# Bind("Fieldtxt16") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>

                                                                             <asp:TemplateField HeaderText="Fieldtxt17" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt17" Text='<%# Bind("Fieldtxt17") %>' ToolTip='<%# Bind("Fieldtxt17") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Fieldtxt18" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFieldtxt18" Text='<%# Bind("Fieldtxt18") %>' ToolTip='<%# Bind("Fieldtxt18") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="PUCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlPUCode" Text='<%# Bind("PUCode") %>' ToolTip='<%# Bind("PUCode") %>'
                                                                                         runat="server" CssClass="Label AutoCompleteTextBox">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="BUCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlBUCode" Text='<%# Bind("BUCode") %>' ToolTip='<%# Bind("BUCode") %>'
                                                                                         runat="server" CssClass="Label AutoCompleteTextBox">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="DUCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlDUCode" Text='<%# Bind("DUCode") %>' ToolTip='<%# Bind("DUCode") %>'
                                                                                         CssClass="Label AutoCompleteTextBox" runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ExpType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlExpType" Text='<%# Bind("ExpType") %>' ToolTip='<%# Bind("ExpType") %>'
                                                                                         CssClass="Label AutoCompleteTextBox" runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ExpCategory" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlExpCategory" Text='<%# Bind("ExpCategory") %>' ToolTip='<%# Bind("ExpCategory") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <%--Text='<%# Bind("Priority") %>' ToolTip='<%# Bind("Priority") %>'--%>
                                                                            <asp:TemplateField HeaderText="Priority" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlPriority" CssClass="Label AutoCompleteTextBox" 
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="IsCustomerRecoverable" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlIsCustomerRecoverable" Text='<%# Bind("IsCustomerRecoverable") %>'
                                                                                        ToolTip='<%# Bind("IsCustomerRecoverable") %>' 
                                                                                        CssClass="Label AutoCompleteTextBox" runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="IsBudgetedinPBS" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlIsBudgetedinPBS" Text='<%# Bind("IsBudgetedinPBS") %>' ToolTip='<%# Bind("IsBudgetedinPBS") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlStatus" Text='<%# Bind("Status") %>' ToolTip='<%# Bind("Status") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList1" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList1" Text='<%# Bind("FieldList1") %>' ToolTip='<%# Bind("FieldList1") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<%--                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList2" Text='<%# Bind("FieldList2") %>' ToolTip='<%# Bind("FieldList2") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>--%>
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlFieldList2" CssClass="Label AutoCompleteTextBox" 
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList3" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList3" Text='<%# Bind("FieldList3") %>' ToolTip='<%# Bind("FieldList3") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList4" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList4" Text='<%# Bind("FieldList4") %>' ToolTip='<%# Bind("FieldList4") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList5" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList5" Text='<%# Bind("FieldList5") %>' ToolTip='<%# Bind("FieldList5") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList6" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<%--                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList6" Text='<%# Bind("FieldList6") %>' ToolTip='<%# Bind("FieldList6") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>--%>
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlFieldList6" CssClass="Label AutoCompleteTextBox" 
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList7" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<%--                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList7" Text='<%# Bind("FieldList7") %>' ToolTip='<%# Bind("FieldList7") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>--%>
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlFieldList7" CssClass="Label AutoCompleteTextBox" 
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldList8" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList8" Text='<%# Bind("FieldList8") %>' ToolTip='<%# Bind("FieldList8") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                              <asp:TemplateField HeaderText="FieldList9" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <%--<ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList9" Text='<%# Bind("FieldList9") %>' ToolTip='<%# Bind("FieldList9") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>--%>
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlFieldList9" CssClass="Label AutoCompleteTextBox" 
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                             <asp:TemplateField HeaderText="FieldList10" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="ddlFieldList10" Text='<%# Bind("FieldList10") %>' ToolTip='<%# Bind("FieldList10") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ExpenseDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpExpenseDate" Text='<%# Bind("ExpenseDate", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("ExpenseDate") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate1" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate1" Text='<%# Bind("FieldDate1", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate1") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate2" Text='<%# Bind("FieldDate2", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate2") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate3" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate3" Text='<%# Bind("FieldDate3", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate3") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>

                                                                                        <asp:CalendarExtender ID="cal" runat="server" TargetControlID="dtpFieldDate3" ></asp:CalendarExtender>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate4" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate4" Text='<%# Bind("FieldDate4", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate4") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate5" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate5" Text='<%# Bind("FieldDate5", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate5") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate6" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate6" Text='<%# Bind("FieldDate6", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate6") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate7" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate7" Text='<%# Bind("FieldDate7", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate7") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="FieldDate8" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="dtpFieldDate8" Text='<%# Bind("FieldDate8", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("FieldDate8") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="CreatedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCreatedBy" Text='<%# Bind("CreatedBy") %>' ToolTip='<%# Bind("CreatedBy") %>'
                                                                                        CssClass="Label AutoCompleteTextBox" Height="15"  Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="CreatedOn" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCreatedOn" Text='<%# Bind("CreatedOn", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("CreatedOn") %>' CssClass="Label AutoCompleteTextBox" Height="15" 
                                                                                        Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ModifiedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtModifiedBy" Text='<%# Bind("ModifiedBy") %>' ToolTip='<%# Bind("ModifiedBy") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15" Width="140"
                                                                                        runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ModifiedOn" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtModifiedOn" Text='<%# Bind("ModifiedOn", "{0:MM/dd/yyyy}") %>'
                                                                                        ToolTip='<%# Bind("ModifiedOn") %>' CssClass="Label AutoCompleteTextBox" 
                                                                                        Height="15" Width="140" runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="SDMStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlSDMStatus" CssClass="Label AutoCompleteTextBox" Width="140" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="SDMApprovedAmount" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSDMApprovedAmount"  CssClass="AutoCompleteTextBox" onKeydown="return PressfloatOnly(event,this);"
                                                                                        Height="15" Width="140" runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="DHStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlDHStatus" CssClass="Label AutoCompleteTextBox" Width="140" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="DHApprovedAmount" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtDHApprovedAmount" onKeydown="return PressfloatOnly(event,this);"
                                                                                        Height="15" Width="140"  CssClass="AutoCompleteTextBox" runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="PNAStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlPNAStatus" CssClass="Label AutoCompleteTextBox" Width="140" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="PNAApprovedAmount" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtPNAApprovedAmount" CssClass="AutoCompleteTextBox" onKeydown="return PressfloatOnly(event,this);"
                                                                                        Height="15" Width="140" runat="server">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Ask (k$)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTotalAmount" Text='<%# Bind("TotalAmt") %>' ToolTip='<%# Bind("TotalAmt") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15"
                                                                                        Width="70" runat="server" ReadOnly="true">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="DM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtDM" Text='<%# Bind("DMMailId") %>' ToolTip='<%# Bind("DMMailId") %>'
                                                                                        CssClass="Label AutoCompleteTextBox"  Height="15"
                                                                                        Width="120" runat="server" ReadOnly="true">
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Width="65" HeaderText="">
                                                                                <ItemTemplate>
                                                                                    <asp:HiddenField ID="hdnfld" Value='<%# Bind("intExpId") %>' runat="server"></asp:HiddenField>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="65px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                            PageButtonCount="20" />
                                                                        <PagerStyle CssClass="pgr" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>


                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <asp:HiddenField ID="hdnCol" runat="server" />
            <asp:HiddenField ID="hdnMandCol" runat="server" />
            <asp:HiddenField ID="hdnDateCol" runat="server" />
            <asp:HiddenField ID="hdnCtrl" runat="server" />
            <asp:HiddenField ID="hdnCt" runat="server" />
        </ContentTemplate>
        <Triggers>
          
            <asp:PostBackTrigger ControlID="lnkExportExcel" />
        
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
