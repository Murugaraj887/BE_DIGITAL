<%@ Page Language="C#"  EnableViewState="true"AutoEventWireup="true" CodeBehind="SubConAddEdit.aspx.cs" EnableEventValidation="false" Title="SubCon Management - SubCon Add new / Edit Page " Inherits="SubConAddEdit" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv='refresh' content='900;url=/SessionTimeOut.aspx' />
    <link href="Styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/BEHomeScripts.js" type="text/javascript"></script>
    
  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />

 

    <%-- <link href="datepickercss/ui-lightness/jquery-ui-1.8.14.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="datepickerjs/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery.ui.core.js" type="text/javascript"></script>
    <script src="datepickerjs/jquery.ui.datepicker.js" type="text/javascript"></script>--%>
    <script src="Calendar_files/common.js" type="text/javascript"></script>
    <%--<style type="text/css">
        .ui-datepicker
        {
            font-family: Calibri;
            font-size: 9px;
            margin-left: 10px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".DatepickerInput").datepicker({ dateFormat: 'mm/dd/yy' });
        });
    </script>--%>
    <style type="text/css">
        .Label
        {
            font-family: Calibri;
            font-size: 9pt;
            background: none;
            border: 0;
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
            text-align: left;
        }
        .TextBox
        {
            font-family: Verdana;
            font-size: 9pt;
        }
    </style>
    <script type="text/javascript">



        function dpExcept() {
            debugger;

            $("[id$=dtpExpenseDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'
      , beforeShow: function (input, inst) {
          var rect = input.getBoundingClientRect();
          setTimeout(function () {
              inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
          }, 0);
      }
            });

            $("[id$=dtpFieldDate1]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

            });


            $("[id$=dtpFieldDate2]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

            });

$("[id$=dtpFieldDate3]").datepicker({
    showOn: 'button',
    buttonImageOnly: true,
    buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

});
$("[id$=dtpFieldDate4]").datepicker({
    showOn: 'button',
    buttonImageOnly: true,
    buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

});
$("[id$=dtpFieldDate5]").datepicker({
    showOn: 'button',
    buttonImageOnly: true,
    buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

});
$("[id$=dtpFieldDate6]").datepicker({
    showOn: 'button',
    buttonImageOnly: true,
    buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

});
$("[id$=dtpFieldDate7]").datepicker({
    showOn: 'button',
    buttonImageOnly: true,
    buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

});
$("[id$=dtpFieldDate8]").datepicker({
    showOn: 'button',
    buttonImageOnly: true,
    buttonImage: 'Images/calendar.gif'
 , beforeShow: function (input, inst) {
     var rect = input.getBoundingClientRect();
     setTimeout(function () {
         inst.dpDiv.css({ top: rect.top - 160, left: rect.left + 0 });
     }, 0);
 }

});

            $('div.ui-datepicker').css({ 'font-size': '10px' }).css({ 'position': 'absolute' });
            $("#divHiddenFieldControls > img").hide();
        }

    </script>
    <script type="text/javascript">

        function UpdateReleventHiddenField(ctrl, hdnname) {

            var value = ctrl.value;
            var update = document.getElementById(hdnname);
            if (update != null)
                update.value = value;
        }

        function ValidateDigits(ctrl, digits) {
            var value = ctrl.value;
            if (digits > 0) {
                if (value.length > digits) {
                    alert('Please enter only ' + digits + ' digits');
                    ctrl.focus();
                }
            }
            else {

            }


        }

        function UpdateReleventHiddenFieldForVisa(ctrl, hdnname) {
            // updating  the country value ...
            var country = ctrl.value;
            var update = document.getElementById(hdnname);
            if (update != null)
                update.value = country;

            var ddl = document.getElementById('ddlFieldList2'); // Important ::: very careful in hardcoding the control names. pls refer markup or Expensecolumns table in DB..
            var hdnfldddl = document.getElementById('hdnfldddlFieldList2');
            var hdnflddmcsv = document.getElementById('hdnfldVisaWalaCSV').value;





            // clearing the ddl items 
            ddl.options.length = 0;
            for (var j = 0; j < ddl.options.length; j++) {
                ddl.options.remove(j);
            } //


            var csvItemarray = hdnflddmcsv.split('|');
            var i = 0;
            var checkCount = 0;
            for (var i = 0; i < csvItemarray.length; i++) {

                var rowcsvarray = csvItemarray[i].split(',');
                if (rowcsvarray[0] == country) {
                    var respectedVisaType = rowcsvarray[1];
                    checkCount++;
                    // updating the first item in ddl to the hidden field

                    if (checkCount == 1)
                        hdnfldddl.value = respectedVisaType;

                    var option = document.createElement("option");
                    option.text = respectedVisaType;
                    option.value = respectedVisaType;
                    option.title = respectedVisaType;
                    try {
                        ddl.add(option, null); //Standard   
                    } catch (error) {
                        ddl.add(option); // IE only 
                    }
                }
            }


            //            if (dmsary.length > 0)
            //                hdnfldddl.value = dmsary[0];

            //            for (var i = 0; i < dmsary.length; i++) {
            //                var valu = dmsary[i];
            //                var option = document.createElement("option");
            //                option.text = valu;
            //                option.value = valu;
            //                try {
            //                    ddl.add(option, null); //Standard   
            //                } catch (error) {
            //                    ddl.add(option); // IE only 
            //                }
            //            }


        }


         
    </script>
    <script type="text/javascript">
        function ValidateMandatoryColumns(columns, dateControls) {

            var selectedQtr = document.getElementById('ddlQuarter').value;
            if (selectedQtr == "Q2'13") {
                alert('Q2-13 is not a valid quarter');
                document.getElementById('ddlQuarter').focus();
                return false;
            }

            var count = 0;
            var mandcolumns = columns.split(",");
            for (var j = 0; j < mandcolumns.length; j++) {
                var control = mandcolumns[j];
                var txtbox = document.getElementById(control);
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
            var _datecolumns = dateControls.split(",");
            for (var j = 0; j < _datecolumns.length; j++) {
                var control = _datecolumns[j];
                var txtbox = document.getElementById(control);
                if (txtbox != null) {
                    if ((txtbox.value + '').length > 0) {
                        var date = txtbox.value;
                        if (!validDate(date)) {
                            alert('Pls enter the valid date format [MM/dd/yyyy]');
                            txtbox.focus();
                            return false;
                        }
                        else {

                            // valid date. make sure the date selected is greater than yesterday..
                            var _today = new Date();
                            var m = _today.getMonth();
                            var d = _today.getDate();
                            var y = _today.getFullYear();
                            var today = new Date(y, m, d);

                            var selecteddate = GetValidDate(date);
                            if (selecteddate >= today) {
                                // true 
                            }
                            else {
                                alert('Date must not be less than today');
                                txtbox.focus();
                                return false;
                            }
                        }
                    }
                }
            }
        }
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
   


    </script>
    <script type="text/javascript">
        function isValidDate(controls) {
        }

        function isValidClick() {
            var selectedQtr = document.getElementById('ddlQuarter').value;
            if (selectedQtr == "Q2'13") {
                alert('Q2-13 requirement is closed.Pls enter for other quarters.');
                document.getElementById('ddlQuarter').focus();
                return false;
            }
            var ddlpu = document.getElementById('ddlPU');
            var ddldm = document.getElementById('ddlDM');
            if (ddlpu != null) {
                if ((ddlpu.value + '') == '') {
                    alert('pls enter the pu value.');
                    ddlpu.focus();
                    return false;
                }
            }
            else
                return true;
            if (ddldm != null) {
                if ((ddldm.value + '') == '') {
                    alert('pls enter the pu value.');
                    ddldm.focus();
                    return false;
                }
            }
            else
                return true;

            return true;



        }
    </script>
    <script type="text/javascript">
        function DOTHIS() {

            // window.showModalDialog('test2.aspx');
            tempDate = window.showModalDialog("test2.aspx", 'bow', "dialogHeight:11; dialogWidth:14;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no");


            return false;
        }
    </script>
    <script type="text/javascript">
        function ShowCurrentTime() {
            PageMethods.GetCurrentTime(document.getElementById("<%=txtUserName.ClientID%>").value, OnSuccess);

        }
        function OnSuccess(response, userContext, methodName) {
            alert(response);
        }
    </script>
    <script type="text/javascript">
        // created by karthik_mahalingam01 sept 15 2012 
        //  Purpopse: cascading drop down calling server side event in JS. to avoid post back :-)
        function PopulateDM(ctrl) {

            var puvalue = ctrl.value;
            var hdnflddmcsv = document.getElementById('hdnfldDMCSV');
            var hdnfldddlDM = document.getElementById('hdnfldddlDM');


            // clearing the ddl items 
            var ddl = document.getElementById('ddlDM');
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
                    option.title = respectedDm;
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
            var hdnfldddlDM = document.getElementById('hdnfldddlDM');
            hdnfldddlDM.value = ctrl.value;
            //alert(hdnfldddlDM.value);  //testing purpose
        }

        



    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%--  <div id="divmain" runat="server">
    </div>--%>
    <div id="DivAddEditInfo" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div id="divtest" runat="server" visible="False">
            Your Name :
            <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
            <input id="btnGetTime" type="button" value="Show Current Time" onclick="ShowCurrentTime()" />
        </div>
        <%--<table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center" style="border-color: #00FF00">
                    <table border="2">
                        <tr>
                            <td align="center" runat="server" class="FormControls" id="tdinfo">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>--%>
    </div>
    <div id="Divminddetails" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <%--<table style="border-color: #CCCCCC; background-color: #b4b4b4; border-width: 0"--%>
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1">
                        <tr>
                            <td class="FormLabel">
                                Practice</td>
                            <td class="FormControls">
                                <asp:DropDownList ID="ddlPU" CssClass="TextBox" Width="150px" runat="server" onchange="PopulateDM(this);">
                                </asp:DropDownList>
                            </td>
                            <td class="FormLabel">
                                DM
                            </td>
                            <td class="FormControls">
                                <asp:DropDownList ID="ddlDM" runat="server" Width="150px" onchange="UpdateDMValue(this);"
                                    CssClass="TextBox">
                                </asp:DropDownList>
                            </td>
                            <%-- <td class="FormControls">
                                <asp:Image ID="imgadd" runat="server" Height="15" Width="15" ImageUrl="~/Images/add.gif" />
                            </td>--%>
                            <td class="FormControls">
                                <asp:Button ID="hypadd" Height="20px" Text="    Create " Style="background-image: url(/Images/add.gif);
                                    background-position: 2px; background-repeat: no-repeat" CssClass="button" runat="server"
                                    Font-Underline="false" OnClick="hypadd_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                SubCon Type
                            </td>
                            <td class="FormControls">
                                <asp:DropDownList ID="ddlExpenseType" runat="server" Width="265px" CssClass="TextBox">
                                </asp:DropDownList>
                                <%--  <asp:Button ID="btn" runat="server" Text="test" OnClientClick="return DOTHIS(); " />--%>
                            </td>
                            <td class="FormLabel">
                                Quarter
                            </td>
                            <td class="FormControls">
                                <asp:DropDownList ID="ddlQuarter" runat="server" Width="150px" CssClass="TextBox">
                                    <%-- <asp:ListItem Text="Current" Selected="True"></asp:ListItem>
                                     <asp:ListItem Text="Next"></asp:ListItem> --%>
                                </asp:DropDownList>
                            </td>
                            <%-- <td class="FormControls">
                                <asp:Image ID="Image2" runat="server" Height="15" Width="15" ImageUrl="~/Images/refresh.jpg" />
                            </td>--%>
                            <td class="FormControls" align="left">
                                <asp:Button ID="hypRefresh" Height="20px" Text="     Reset " Style="background-image: url(/Images/reset.png);
                                    background-repeat: no-repeat; background-position: 2px" CssClass="button" runat="server"
                                    Font-Underline="false" OnClick="hypRefresh_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:PlaceHolder ID="DynamicControlsHolder" EnableViewState="true" runat="server">
                </asp:PlaceHolder>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnSave" Height="22px" runat="server" Text="      Save " Style="background-image: url(/Images/save.png);
                    background-repeat: no-repeat; background-position: 2px" CssClass="button" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <div id="divHiddenFieldControls" runat="server">
        <asp:HiddenField ID="hdnfldtxtNumberofItems" runat="server" />
        <asp:HiddenField ID="hdnfldtxtProjOppCode" runat="server" />
        <asp:HiddenField ID="hdnfldtxtJustificationRemarks" runat="server" />
        <asp:HiddenField ID="hdnfldtxtUnitCost" runat="server" />
        <asp:HiddenField ID="hdnfldddlClientCode" runat="server" />
        <asp:HiddenField ID="hdnfldtxtItemName" runat="server" />
        <asp:HiddenField ID="hdnfldtxtBEUpside" runat="server" />
        <asp:HiddenField ID="hdnfldtxtBEDownside" runat="server" />
        <asp:HiddenField ID="hdnfldtxtCurrQtr" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFutQtrBE" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate1" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate2" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate3" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate4" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate5" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate6" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate7" runat="server" />
        <asp:HiddenField ID="hdnflddtpFieldDate8" runat="server" />
        <asp:HiddenField ID="hdnfldtxtCreatedBy" runat="server" />
        <asp:HiddenField ID="hdnfldtxtCreatedOn" runat="server" />
        <asp:HiddenField ID="hdnfldtxtModifiedBy" runat="server" />
        <asp:HiddenField ID="hdnfldtxtModifiedOn" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt1" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt2" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt3" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt4" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt5" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt6" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt7" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt8" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt9" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt10" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt11" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt12" runat="server" />
          <asp:HiddenField ID="hdnfldtxtFieldtxt13" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt14" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt15" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt16" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt17" runat="server" />
        <asp:HiddenField ID="hdnfldtxtFieldtxt18" runat="server" />
        <asp:HiddenField ID="hdnfldddlStatus" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList1" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList2" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList3" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList4" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList5" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList6" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList7" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList8" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList9" runat="server" />
        <asp:HiddenField ID="hdnfldddlFieldList10" runat="server" />
        <asp:HiddenField ID="hdnfldddlExpType" runat="server" />
        <asp:HiddenField ID="hdnfldddlExpCategory" runat="server" />
        <asp:HiddenField ID="hdnfldddlPriority" runat="server" />
        <asp:HiddenField ID="hdnflddtpExpenseDate" runat="server" />
        <asp:HiddenField ID="hdnfldddlIsCustomerRecoverable" runat="server" />
        <asp:HiddenField ID="hdnfldddlIsBudgetedinPBS" runat="server" />
    </div>
    <asp:HiddenField ID="hdnfldddlDM" runat="server" />
    <asp:HiddenField ID="hdnfldDMCSV" runat="server" />
    <asp:HiddenField ID="hdnfldKey" runat="server" />
    <asp:HiddenField ID="hdnfldVisaWalaCSV" runat="server" />
    </form>
</body>
</html>
