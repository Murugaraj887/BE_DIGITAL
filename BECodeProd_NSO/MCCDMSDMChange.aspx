<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="MCCDMSDMChange.aspx.cs" Inherits="MCCDMSDMChange" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
    >
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
            width: 55%;
            height: 40%;
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
    </style>
    <style type="text/css">
        .hdnfldDisplay
        {
            display: none;
        }
        
        .txtFont
        {
            font-family: Calibri;
            font-size: 9pt;
        }
    </style>
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
        .label
        {
            border-style: solid;
            background-color: #f8da92;
            padding: 1px 0px;
            border-color: red;
            border-width: 1px;
            font-family: Calibri;
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
    </style>
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
            position: fixed;
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
    </style>
    <style type="text/css">
        .modalBackground
        {
            background-color: #fff;
            border-bottom-style: none;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            border-bottom-width: thin;
            font-family: Calibri;
            font-size: 9pt;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            var flagctrl = document.getElementById('MainContent_hdnfldFlag');
            if (flagctrl != null) {
                var flag = flagctrl.value;
                if (flag == '1') {

                    document.getElementById('MainContent_btnhidden').click(); ;
                }

            }

        });

        function isvalidupload() {



      
            //            var ctrlqtr = document.getElementById('MainContent_ddlQtr');
            //            var ctrlyr = document.getElementById('MainContent_ddlYear');
            //            var ctrlpu = document.getElementById('MainContent_ddlPu');
            //            var ctrlmcc = document.getElementById('MainContent_ddlMcc');
            //            var ctrlcur = document.getElementById('MainContent_ddlDMSDM');
            var ctrlchange = document.getElementById('MainContent_txtChange');

            var ctrlddlChange = document.getElementById('MainContent_ddlChange');
            var valddlChange = ctrlddlChange.value + '';
            valddlChange.trim();
            var txtChange = ctrlchange.value + '';
            txtChange.trim();


            if (valddlChange == "Master Client Code") {

                if (txtChange == "") {

                    alert('Enter the Master Client Code');
                    return false;
                }
            }

            if (valddlChange == "SDM") {

                if (txtChange == "") {

                    alert('Enter the SDM MailId');
                    return false;
                }
            }

            if (valddlChange == "DM") {

                if (txtChange == "") {

                    alert('Enter the DM MailId');
                    return false;
                }
            }

            if (valddlChange == "Currency") {


                if (txtChange == "") {

                    alert('Enter the Currency');
                    //                    
                    //                    var mb = $find('modalpopupBehaviour');
                    //                    mb.hide();


                    return false;
                }


                var btnchange = document.getElementById('MainContent_btnChange2');
                if (btnchange != null) {
                    $find("<%=ModalPopupExtender1.BehaviorID%>").show();
                    var btnReport = document.getElementById('MainContent_btnChange2');
                    btnReport.style.visibility = 'hidden';
                    document.getElementById('MainContent_loadingUpdate').style.visibility = 'visible';
                    var d = $('#dotsUpdate');
                    (function loadingUpdate() {
                        setTimeout(function () {
                            draw = d.text().length >= 5 ? d.text('') : d.append('.');
                            loadingUpdate();
                        }, 300);
                    })();

                    return false;
                }






            }

            var btnReport = document.getElementById('MainContent_btnChange');
            btnReport.style.visibility = 'hidden';
            document.getElementById('MainContent_loadingUpdate').style.visibility = 'visible';
            var d = $('#dotsUpdate');
            (function loadingUpdate() {
                setTimeout(function () {
                    draw = d.text().length >= 5 ? d.text('') : d.append('.');
                    loadingUpdate();
                }, 300);
            })();

        }

        function showPopup() {
            $find("<%=ModalPopupExtender1.BehaviorID%>").show();
        }


        function PressReadOnly(evt, thisobj) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            //            if (charCode == 37 || charCode == 39) return true; // allow arrows 

            //            if (charCode == 35 || charCode == 36) return true; // home, end

            if (charCode == 9) return true; // backspace , tab 



            alert('Select a date from the calender');

            return false;

        }

    </script>
    <script type="text/javascript">
        function AreuSure() {
        
            var update = document.getElementById('MainContent_rbtnAction_0');
            var deletee = document.getElementById('MainContent_rbtnAction_1');
            var deletebyQtr = document.getElementById('MainContent_rbtnAction_2');
            var exec = document.getElementById('MainContent_rbtnAction_3');
            var exectrends = document.getElementById('MainContent_rbtnAction_4');

            if (deletee.checked) {
                var ddlfromdate = document.getElementById('MainContent_ddlWeeklyDate').value;
                if (ddlfromdate == "--Select--") {
                    alert("Select a date");
                    return false;
                }
            }
            if (update.checked) {
                var ddlupdate = document.getElementById('MainContent_ddlupdate').value;
                if (ddlupdate == "--Select--") {
                    alert("Select a date ");
                    return false;
                }

                var text = document.getElementById('MainContent_txtDate').value;
                if (text == "") {
                    alert("Select a Date from the calender");
                    return false;
                }
            }

            if (deletebyQtr.checked) {
                var ddlQtrWeekly = document.getElementById('MainContent_ddlQtrWeekly').value;
                if (ddlQtrWeekly == "--Select--") {
                    alert("Select Quarter");
                    return false;
                }

                var ddlYearWeekly = document.getElementById('MainContent_ddlYearWeekly').value;
                if (ddlYearWeekly == "--Select--") {
                    alert("Select Year");
                    return false;
                }

                //                var ddlDelDate = document.getElementById('MainContent_ddlDelDate').value;
                //                if (ddlDelDate == "--Select--") {
                //                    alert("Select a Date");
                //                    return false;
                //                }
            }
            if (update.checked) {
                var isok = confirm('Are you sure you want to Update ?')
                //                if (isok)
                //                    return true;
                //                else
                //                    return false;
            }
            if (deletee.checked) {
                var isok = confirm('Are you sure you want to Delete ?')
                //                if (isok)
                //                    return true;
                //                else
                //                    return false;
            }
            if (deletebyQtr.checked) {
                var isok = confirm('Are you sure you want to Delete by Quarter ?')
                //                if (isok)
                //                    return true;
                //                else
                //                    return false;
            }

            if (exec.checked) {
                var isok = true;
            }

            if (exectrends.checked) {

               var ddlQtr = document.getElementById('MainContent_ddlTrendQuarter').value;
                if (ddlQtr == "--Select--") {
                    alert("Select Quarter");
                    return false;
                }

                var ddlYear = document.getElementById('MainContent_ddlTrendYear').value;
                if (ddlYear == "--Select--") {
                    alert("Select Year");
                    return false;
                }

                var isok = true;
            }

            if (isok) {

                var btnReport = document.getElementById('MainContent_btnGo');
                btnReport.style.visibility = 'hidden';
                document.getElementById('MainContent_loading').style.visibility = 'visible';
                var d = $('#dots');
                (function loadingpls() {
                    setTimeout(function () {
                        draw = d.text().length >= 5 ? d.text('') : d.append('.');
                        loadingpls();
                    }, 300);
                })();
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        function IsValidCopy() {
          

            if (true) {

                var btnReport = document.getElementById('MainContent_btnCopy');
                btnReport.style.visibility = 'hidden';
                document.getElementById('MainContent_loadingCopy').style.visibility = 'visible';
                var d = $('#dotsCopy');
                (function loading() {
                    setTimeout(function () {
                        draw = d.text().length >= 5 ? d.text('') : d.append('.');
                        loading();
                    }, 300);
                })();
                //                loading();
                return true;
            }
            var lblinfo = 'MainContent_lblInfo';
            var info = document.getElementById(lblinfo);

            if (info != null) {
                info.style.display = 'none';
            }

            var fromyear = document.getElementById('MainContent_ddlFromFinYear').value;
            var fromqtr = document.getElementById('MainContent_ddlFromQuarter').value;
            var toyear = document.getElementById('MainContent_ddlToFinYear').value;
            var toqtr = document.getElementById('MainContent_ddlToQuarter').value;
            var select = '-Select-';
            if (fromyear == select || toyear == select || fromqtr == select || toqtr == select)
            { alert('Please select the values from  drop down list'); return false; }

            var intfromqtr = parseInt(fromqtr.replace('Q', ''))
            var inttoqtr = parseInt(toqtr.replace('Q', ''))

            var intfromyear = GetSumYear(fromyear)
            var inttoyear = GetSumYear(toyear)

            if (fromyear == toyear) {
                if (intfromqtr == inttoqtr) {
                    alert('From Quarter and Year cannot be the same as To Quarter and Year'); return false;
                }
                //                else if (GetCurrentQtr() == toqtr) {
                //                alert('To Quarter should be greater than current quarter ');return false;
                //                }
                else if (intfromqtr > inttoqtr) {
                    alert('To Quarter should be greater than From quarter '); return false;
                }
                else
                    return true;
            }
            else if (intfromyear > inttoyear) {
                alert('To Quarter and Year should be greater than From Quarter and Year'); return false;
            }
            else
                return true;



        }

        //        function ResetAll() {
        //         
        //            var upddlChange = document.getElementById('MainContent_ddlChange');
        //            var ddlFromFinYear = document.getElementById('MainContent_ddlFromFinYear');
        //           
        //           

        //        }


        function GetSumYear(year) {
            var ary = year.split('-');
            return parseInt(ary[0]);
        }

        //        function GetCurrentQtr() {
        //            var currentQuarter = 'Q';
        //            var today = new Date();
        //            var todaymonth = today.getMonth();
        //            todaymonth--;

        //            if (todaymonth == 1 || todaymonth == 2 || todaymonth == 3) {
        //                todayquarternumber = 4;

        //            }
        //            else if (todaymonth == 4 || todaymonth == 5 || todaymonth == 6) {
        //                todayquarternumber = 1;

        //            }
        //            else if (todaymonth == 7 || todaymonth == 8 || todaymonth == 9) {
        //                todayquarternumber = 2;

        //            }
        //            else {
        //                todayquarternumber = 3;

        //            }
        //            return currentQuarter + todayquarternumber;

        //        }

    </script>
    <script type="text/javascript">
        function PopUpDates() {

            var ctrlqtr = document.getElementById('MainContent_ddlQtr');
            var ctrlyr = document.getElementById('MainContent_ddlYear');
            var ctrlpu = document.getElementById('MainContent_ddlPu');
            var ctrlmcc = document.getElementById('MainContent_ddlMcc');
            var ctrlcur = document.getElementById('MainContent_ddlDMSDM');
            var ctrlchange = document.getElementById('MainContent_txtChange');

            var txtChange = ctrlchange.value + '';
            txtChange.trim();

            if (txtChange == '') {

                alert('Enter the Currency');
                return false;
            }
            var qtr = ctrlqtr.value + '';
            var year = ctrlyr.value + '';
            var pu = ctrlpu.value + '';
            var mcc = ctrlmcc.value + '';
            var cur = ctrlcur.value + '';

            qtr.trim();
            year.trim();
            pu.trim();
            mcc.trim();
            cur.trim();
            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            window.open('PopupDates.aspx?&Qtr=' + qtr + '&Year=' + year + '&Pu=' + pu + '&Mcc=' + mcc + '&Cur=' + cur + '&Change=' + txtChange, 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=700, height=260, menubar=no, scrollbars=no, resizable=no');


        }
    
    </script>
    <script type="text/javascript">

        function PopUpFreeze() {

            var left = (screen.width - 700) / 2;
            var top = (screen.height - 300) / 2;
            // window.showModalDialog('AppFreeze.aspx', 'bow', 'dialogHeight:10; dialogWidth:15;  center:yes;toolbar: false;status: 0;scroll:0;unadorned:0;help:no');
            window.open('AppFreeze.aspx', 'ThisPopUp', 'left = ' + left + ', top=' + top + ', width=450, height=190 , menubar=no, scrollbars=no, resizable=no');



            //return false;
        }

        function ValidateQuarter() {

            var Ok = confirm('Are you sure correct quarter is selected?');

            if (Ok) return true;

            else return false;

        }


    </script>
    <style type="text/css">
        .style1
        {
            width: 81px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:ScriptManager AsyncPostBackTimeout="360000" runat="server" />--%>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="background-color: #adaba6">
                <div style="height: 2px">
                </div>
                 <%--  <div style="background-color: #c41502; height: 18px;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <asp:Menu ID="MenuAdmin" Orientation="Vertical" runat="server" BackColor="#f8df9c"
                                                StaticMenuStyle-HorizontalPadding="20px" DynamicHorizontalOffset="2" Font-Names="Calibri"
                                                Font-Size="11px" ForeColor="#c41502" Font-Bold="true" StaticSubMenuIndent="10px">
                                                <Items>
                                                   
                                                    <asp:MenuItem Text=" Admin " Value="Admin" NavigateUrl="~/BEAdmin.aspx">
                               
                                <asp:MenuItem Text="Freezing and Delegation" Value="freeze">
                                    <asp:MenuItem Text="Application Freeze" Value="App Freeze" NavigateUrl="javascript:PopUpFreeze();" />
                                    <asp:MenuItem Text="Monthly Freeze" Value="Monthly Freeze" NavigateUrl="~/MasterSetting.aspx" />
                                    <asp:MenuItem Text="Delegation" Value="Delegation" NavigateUrl="~/DelegatePage.aspx" />
                                </asp:MenuItem>
                                <asp:MenuItem Text="Master Data" Value="Data">
                                    <asp:MenuItem Text="Client Code Portfolio" Value="Client Code Portfolio" NavigateUrl="~/ClientCodePortfolioScreen0.aspx" />
                                    <asp:MenuItem Text="Portfolio" Value="Portfolio" NavigateUrl="~/BEPortfolioAdmin.aspx" />
                                </asp:MenuItem>
                                <asp:MenuItem Text="Exchange Rates" Value="Exchange Rate" NavigateUrl="~/ExchangeRate.aspx">
                                    <asp:MenuItem Text=" Daily Conversion" Value=" Daily Conv." NavigateUrl="~/ConvRateScreen.aspx" />
                                    <asp:MenuItem Text="Monthly Conversion" Value="Monthly Conv." NavigateUrl="~/GuidanceAndActuals.aspx" />
                                    <asp:MenuItem Text="Push Exchange Rates" Value="Exchange Rate" NavigateUrl="~/ExchangeRate.aspx" />
                                </asp:MenuItem>
                                <asp:MenuItem Text="Maintenance" Value="Maintain">
                                 <asp:MenuItem Text="Audit Log" Value="Audit">
                                    <asp:MenuItem Text="View" Value="Audit" NavigateUrl="~/AuditLog.aspx" />
                                    <asp:MenuItem Text="Delete" Value="Delete" NavigateUrl="~/AuditLogDelete.aspx" />
                                </asp:MenuItem>
                                    <asp:MenuItem Text="DM SDM Map" Value="DM SDM Map" NavigateUrl="~/DmSdmMap.aspx" />
                                  <asp:MenuItem Text="Deletion/Updation Of Data" Value="Update" NavigateUrl="~/MCCDMSDMChange.aspx" />
                                    <asp:MenuItem Text="User" Value="User" NavigateUrl="~/BEAdmin.aspx" />
                                </asp:MenuItem>
                              
                            </asp:MenuItem>
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
                                        <td style="width: 60px" align="right" bgcolor="#C41502">
                                            <asp:HyperLink ID="HyperLink1" Font-Bold="true" Visible="true" ForeColor="#F8DF9C"
                                                runat="server" NavigateUrl="~/BEHome.aspx" Font-Underline="True">Revenue &nbsp</asp:HyperLink>
                                        </td>
                                        <td style="width: 60px" align="right" bgcolor="#C41502">
                                            <asp:HyperLink ID="hypVol" Font-Bold="true" Visible="true" ForeColor="#F8DF9C" runat="server"
                                                NavigateUrl="~/BEVolume.aspx" Font-Underline="True">Volume &nbsp</asp:HyperLink>
                                        </td>
                                        <td style="width: 60px" align="right" bgcolor="#C41502">
                                            <asp:HyperLink ID="hypbacktoreports" Font-Bold="true" ForeColor="#F8DF9C" runat="server"
                                                Visible="true" NavigateUrl="~/Reports.aspx" Font-Underline="true">Reports </asp:HyperLink>
                                            &nbsp
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                <asp:ModalPopupExtender BackgroundCssClass="modalBackground" DropShadow="false" X="440"
                    Y="400" CancelControlID="btnClose" runat="server" PopupControlID="pnlpopup" ID="ModalPopupExtender1"
                    PopupDragHandleControlID="pnlpopup" Drag="true" TargetControlID="hdnfldFlag"
                    BehaviorID="ModalPopupBehaviour" />
                <asp:Panel ID="pnlGrid" Width="100%" Height="180px" runat="server" BackColor="white"
                    CssClass="txtFont">
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" Font-Size="Smaller" ForeColor="Green" runat="server" Font-Names="Verdana"
                                    Font-Bold="true" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHeading" runat="server" CssClass="label" Font-Bold="true" Font-Size="Small"
                                    Text="  Updation Of Data   " Width="100%"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1">
                        <tr>
                            <td class="FormControls">
                            </td>
                            <td class="FormLabel">
                                Change Of:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlChange" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlChange_SelectedIndexChanged">
                                                <asp:ListItem>Master Client Code</asp:ListItem>
                                                <asp:ListItem>SDM</asp:ListItem>
                                                <asp:ListItem>DM</asp:ListItem>
                                                <asp:ListItem>Currency</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1" id="tble1" runat="server">
                        <tr>
                            <td>
                            </td>
                            <td class="FormLabel">
                                PU:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlPU" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlPU_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="FormLabel">
                                Quarter:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlQtr" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlQtr_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="FormLabel">
                                Year:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="FormLabel">
                                Master Client Code:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlMcc" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlMcc_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="FormLabel">
                                DM/SDM/Currency:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlDMSDM" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlDMSDM_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="FormControls">
                                <asp:Button ID="btnSearch" Text="Search" CssClass="button" runat="server" Width="60px"
                                    OnClick="btnSearch_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <table style="border-color: White; background-color: White; border-width: 0" cellpadding="2"
                        cellspacing="1" id="tbl3" runat="server">
                        <tr>
                            <td>
                            </td>
                            <td class="FormLabel">
                                Current:&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCurrent" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                Font-Bold="true" ReadOnly="true" Enabled="false" ForeColor="Black">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="FormLabel">
                                Change To :&nbsp;
                            </td>
                            <td class="FormControls">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtChange" runat="server" Font-Names="Calibri" Font-Size="11px">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" id="tble2" runat="server">
                        <tr>
                            <td colspan="5" align="center">
                                <asp:Button ID="btnChange" Text="UPDATE" CssClass="button" runat="server" Width="60px"
                                    OnClick="btnChange_Click" OnClientClick=" return isvalidupload();"></asp:Button>
                                <%--<asp:Button ID="btnChange1" runat="server" Height="5px"></asp:Button>--%>
                                <asp:Button ID="btnChange2" Text="UPDATE" CssClass="button" runat="server" Width="60px"
                                    OnClientClick=" return isvalidupload();" OnClick="btnChange2_Click1" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <div id="loadingUpdate" align="left" runat="server" style="font-size: small; font-weight: bold;
                                    padding-left: 250px; visibility: hidden; color: #FF0000; font-family: Calibri">
                                    Please Wait<span style="width: 50px" id="dotsUpdate"></span></div>
                                <asp:ImageButton ID="btnhiddenUpdate" Height="2px" Text="Generate Report " ImageUrl="~/Images/white.png"
                                    runat="server" OnClick="btnhiddenUpdate_Click"></asp:ImageButton>
                                <%--<asp:HiddenField ID="hdnfldFlag2" runat="server" />--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel1" Width="100%" Height="205px" runat="server" BackColor="white"
                    CssClass="txtFont">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblBack" runat="server" CssClass="label" Font-Bold="true" Font-Size="Small"
                                    Text="   Weekly Tables Delete/Update/Run    " Width="100%"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" Font-Size="Smaller" ForeColor="Green"
                            Font-Bold="true" Font-Names="Verdana"></asp:Label>
                    </table>
                    <table align="left">
                        <tr valign="top">
                            <td class="FormLabel">
                                <asp:RadioButtonList ID="rbtnAction" runat="server" OnSelectedIndexChanged="rbtnAction_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    
                                    <asp:ListItem Value="Update" >Update the Weekly Frozen Date</asp:ListItem>
                                    <asp:ListItem Value="Delete">Delete Weekly Data By Date</asp:ListItem>
                                    <asp:ListItem Value="DeletebyQtr">Delete Weekly Data By Quarter</asp:ListItem>
                                    <asp:ListItem Value="Execute" Selected="True">Run the Weekly Freeze Stored Procedure </asp:ListItem>
                                    <asp:ListItem Value="Trends">Run the Daily Trends Load Stored Procedure </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                            </td>
                            <td valign="top">
                                <table>
                                    <tr valign="bottom" align="left">
                                        <td align="left" valign="bottom" class="FormLabel">
                                            From Date
                                        </td>
                                        <td class="FormControls">
                                            <asp:DropDownList ID="ddlUpDate" runat="server" Enabled="true" Font-Names="Calibri"
                                                Font-Size="11px" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlUpDate_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" class="FormLabel">
                                            To Date:
                                        </td>
                                        <td class="FormControls">
                                            <asp:TextBox ID="txtDate" runat="server" Enabled="true" Font-Names="Calibri" Font-Size="11px"
                                                Width="70px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Image ID="img1" runat="server" onmouseover="this.style.cursor='hand'" Visible="true"
                                                onclick="fnBEShowCalendarFrmDate('txtDate')" ImageUrl="~/Images/calendar.gif" />
                                        </td>
                                </table>
                                <table align="center">
                                    <tr valign="middle">
                                        <td align="left" class="FormControls">
                                            <asp:DropDownList ID="ddlWeeklyDate" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True"  Width="100px"
                                                Enabled="false" 
                                                onselectedindexchanged="ddlWeeklyDate_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="left" class="FormControls">
                                            <asp:DropDownList ID="ddlQtrWeekly" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlQtrWeekly_SelectedIndexChanged"
                                                Width="80px" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="FormControls">
                                            <asp:DropDownList ID="ddlYearWeekly" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" Width="80px" Enabled="false" OnSelectedIndexChanged="ddlYearWeekly_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="FormControls" valign="bottom">
                                            <asp:ListBox ID="ddlDelDate" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                Width="160px" Enabled="false" SelectionMode="Multiple" Rows="3" CssClass="TextBox">
                                            </asp:ListBox>
                                        </td>
                                    </tr>

                                    

                                    <tr>
                                    <td valign="middle">
                                    <asp:DropDownList ID="ddlTrendQuarter" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlTrendQuarter_SelectedIndexChanged"
                                                Width="80px" Enabled="false">
                                            </asp:DropDownList>


                                    </td>
                                    
                                    
                                     <td class="FormControls">
                                            <asp:DropDownList ID="ddlTrendYear" runat="server" Font-Names="Calibri" Font-Size="11px"
                                                AutoPostBack="True" Width="80px" Enabled="false" >
                                            </asp:DropDownList>
                                       
                                    </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        </tr>
                        <tr valign="bottom">
                            <td>
                            </td>
                            <td>
                            </td>
                            <td valign="bottom" align="left">
                                <asp:Button ID="btnGo" Text="GO" CssClass="button" runat="server" Width="50px" Visible="true"
                                    OnClientClick=" return AreuSure();" OnClick="btnGo_Click" Height="20px"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <div id="loading" align="left" runat="server" style="font-size: small; font-weight: bold;
                                    padding-left: 250px; visibility: hidden; color: #FF0000; font-family: Calibri">
                                    Please Wait<span style="width: 50px" id="dots"></span></div>
                                <asp:ImageButton ID="btnhidden" Height="2px" Text="Generate Report " ImageUrl="~/Images/white.png"
                                    runat="server" OnClick="btnhidden_Click"></asp:ImageButton>
                                <asp:HiddenField ID="hdnfldFlag" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCopyData" Width="100%" Height="150px" runat="server" BackColor="White"
                    CssClass="txtFont">
                    <table width="100%">
                        <tr>
                            <td align="left" width="15%">
                                <asp:Label ID="lblQuarterlyData" runat="server" Width="15%" Text="   Copy Quarterly Data    "
                                    Font-Bold="true" Font-Size="Small" CssClass="label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblInfo" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td width="19%">
                            </td>
                            <td align="center" class="FormLabel" style="width: 250px">
                                From
                            </td>
                            <td>
                            </td>
                            <td align="center" class="FormLabel" style="width: 250px">
                                To
                            </td>
                            <td width="25%">
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="right" style="width: 50%">
                                <table>
                                    <tr>
                                        <td width="175px">
                                        </td>
                                        <td class="FormLabel" width="80px">
                                            Financial Year
                                        </td>
                                        <td width="30px" class="FormControls">
                                            <asp:DropDownList ID="ddlFromFinYear" runat="server" Font-Size="11px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlFromFinYear_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="30px" align="right" class="FormLabel">
                                            Quarter
                                        </td>
                                        <td width="30px" class="FormControls">
                                            <asp:DropDownList ID="ddlFromQuarter" runat="server" Font-Size="11px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlFromQuarter_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" style="width: 50%">
                                <table>
                                    <tr>
                                        <td class="FormLabel" width="80px">
                                            Financial Year
                                        </td>
                                        <td width="30px" class="FormControls">
                                            <asp:DropDownList ID="ddlToFinYear" runat="server" Font-Size="11px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlToFinYear_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="30px" align="right" class="FormLabel">
                                            Quarter
                                        </td>
                                        <td width="30px" class="FormControls">
                                            <asp:DropDownList ID="ddlToQuarter" runat="server" Font-Size="11px" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="11%">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <%-- <tr>
                            <td>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCopy" runat="server" OnClientClick=" return IsValidCopy();" Text=" Copy Data "
                                    CssClass="button" OnClick="btnCopy_Click" />
                                <%-- </td>
                              <td colspan="5">--%>
                                <div id="loadingCopy" align="left" runat="server" style="font-size: small; font-weight: bold;
                                    padding-left: 250px; visibility: hidden; color: #FF0000; font-family: Calibri">
                                    Please Wait<span style="width: 50px" id="dotsCopy"></span></div>
                                <asp:ImageButton ID="btnHiddenCopy" Height="2px" Text="Generate Report " ImageUrl="~/Images/white.png"
                                    runat="server" OnClick="btnHiddenCopy_Click"></asp:ImageButton>
                                <asp:HiddenField ID="hdnfldFlag1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopup" Style="display: none;
                    left: 433px" BorderWidth="2px" BackColor="White">
                    <asp:UpdatePanel ID="updatepnlpopup" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div id="dialog" class="web_dialog">
                                <table id="tblTitlePopUP" style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="1" class="web_dialog_title">
                                            Map the Missing Dates
                                        </td>
                                        <%--<td class="web_dialog_title align_right">
                                            <asp:Image ID="btnClose" onclick="return closepopup();" Width="25" CssClass="closebtn"
                                                ToolTip="Close this PopUp" Height="25" runat="server" ImageUrl="~/Images/close.png" />
                                        </td>--%>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center">
                                            <div id="divgrid" runat="server" style="height: 180px; width: 100%; overflow: auto">
                                                <asp:GridView ID="grdBEDU" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                                                    AlternatingRowStyle-CssClass="alt" CssClass="mGrid" EmptyDataText="No records found">
                                                    <Columns>
                                                        <asp:BoundField DataField="dtDate" HeaderText="Weekly Dates" ItemStyle-Width="200px">
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Daily Currency Dates
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlDailyDates" runat="server" Font-Names="Calibri" Font-Size="11px">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr align="center">
                                        <td>
                                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="button" OnClick="btnSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <%--  <div id="divAddNewWarning">
                    do u want save or do wat ever want.... (custom message).....?
                    <asp:Button ID="Button2" OnClientClick="return false;" runat="server" Text="yesssss" />
                                <asp:Button ID="btnNO"  runat="server" Text="No never" OnClientClick="return ShowDivAddNew();" />
                    </div>--%>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <div style="height: 7px">
                </div>
                <%--</div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
