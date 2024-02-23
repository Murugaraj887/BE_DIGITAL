<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True"
    CodeBehind="Reports.aspx.cs" Inherits="Reports" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Styles/css/style.css" />
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
            width: 300px;
            height: 170px;
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
        a:link, a:visited
        {
            color:Gray;
        }
        .text
        {
             padding-bottom:10px;
        }
        
        body {
  
    font-size: 12px!important;
    font-family: Calibri !important;
   
}
table {
    border-spacing: 0;
    border-collapse: collapse;
   
}
td, th {
   padding-left:10px !important;
    padding-right:5px !important;
}

.modal-dialog
{
    width:80%!important;
    }


        .modal-header
        {
        	padding-top:5px!important;
        	 background-color: rgb(51, 51, 51)!important;
        	 height:40px!important;
            border-top-right-radius:3px;
            border-top-left-radius:3px;
        }
        .modal-title
        {
        	color: floralwhite !important;
            font-family :Calibri!important;
        }
        .modal-body
        {
            
        }
        .close {
            color: floralwhite !important;
            font-size:1em;
        }
    </style>
   
    <%-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
   
    <script src="http://code.jquery.com/jquery-2.2.0.js"></script> 

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>   --%> 
  
    <link href="Styles/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-2.2.0.js" type="text/javascript"></script>
    <script src="Scripts/bootstrap.min.js" type="text/javascript"></script>
   
     <script type="text/javascript">
         function modal(clickedlink, text) {

             $(".modal-body").html("");

             $('<div align="center">')
            .attr('id', 'div1')
            .appendTo('.modal-body');

             $('<img>')
            .attr('id', 'loadmodal')                  // Creates the element
            .attr('src', 'img/loading-small.gif')
             .appendTo('#div1');

             $('#loadmodal').show();

             $('#myModalReport').on('shown.bs.modal', function () {
                 $(this).find('.modal-dialog').css({
                     width: '90%',
                     height: 'auto',
                     'max-height': '80%'

                 });
             });

             var link = clickedlink.textContent;


             if (link == "User Details" || link == "Client Code Portfolio Dump") {

                 if (text == "Client Code Portfolio Dump") {
                     document.getElementById('MainContent_btnCCP').click();
                 }
                 else if (text == "User Details") {
                     document.getElementById('MainContent_btnUserDetails').click();
                 }
             }
             else {



                 $('#modalhdng').html(link);

                 $('<iframe>')
            .attr('id', 'mdlifr')                  // Creates the element
    .attr('src', text) // Sets the attribute spry:region="myDs"
    .attr('height', 220)
    .attr('width', '100%')
     .attr('frameborder', 0)
     .attr('scrolling', 'no')
     .attr('style', 'display:none')
    .appendTo('.modal-body');

                 $('#myModalReport').modal('show');

                 window.setTimeout(function () { $('#loadmodal').hide(); $('#mdlifr').show(); }, 2000);

             }


         }
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <body>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="background-color: #adaba6">
                <%--<div style="height: 1px">
                </div>--%>
                <%--<div style="height: 2px">
                </div>--%>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" align="left" bgcolor="#C41502" style="width: 400px">
                            <asp:Menu ID="MenuAdmin" Orientation="Horizontal" runat="server" CssClass="menufont"
                                    DynamicHorizontalOffset="2"
                                    Font-Names="Calibri" Font-Size="11px" Font-Bold="true" style="float:left;margin-left:3px"
                                    StaticSubMenuIndent="10px" DynamicMenuItemStyle-Font-Size="11px" StaticPopOutImageUrl="~/Images/right.png" DynamicPopOutImageUrl="~/Images/right.png">
                                    <Items>
                                    </Items>

<StaticMenuStyle HorizontalPadding="20px"  ></StaticMenuStyle>
                                  
                                    <StaticMenuItemStyle HorizontalPadding="10px" Font-Size="11px" CssClass="menufont"  VerticalPadding="2px" />
                                    
                                    <DynamicHoverStyle  CssClass="menufont" Font-Size="11px"/>
                                    <DynamicItemTemplate>
                                        <%# Eval("Text") %>
                                    </DynamicItemTemplate>
                                    <DynamicMenuStyle CssClass="menufont" Font-Size="11px"/>
                                    <DynamicSelectedStyle CssClass="menufont" Font-Size="11px"/>
                                    <DynamicMenuItemStyle CssClass="menufont"  Font-Size="11px" HorizontalPadding="15px" VerticalPadding="2px" />
                                    <StaticHoverStyle  CssClass="menufont" Font-Size="11px"/>
                                    <StaticItemTemplate>
                                        <%# Eval("Text") %>
                                    </StaticItemTemplate>
                                </asp:Menu>
                        </td>
                        </tr>
                        <tr>
                   <%--     <td style="width: 50px" align="right" bgcolor="#C41502">
                            <asp:HyperLink ID="hypbacktoreports" ForeColor="#F8DF9C" runat="server" NavigateUrl="~/BEHome.aspx" Visible="false"
                                Font-Underline="True">Revenue </asp:HyperLink>&nbsp
                        </td>
                        <td style="width: 50px" align="right" bgcolor="#C41502">
                            <asp:HyperLink ID="HyperLink1" ForeColor="#F8DF9C" runat="server" NavigateUrl="~/BEVolume.aspx" Visible="false"
                                Font-Underline="True">Volume</asp:HyperLink>
                            &nbsp
                        </td>--%>
                    </tr>
                </table>
                <aspajax:RoundedCornersExtender ID="RoundedCornersExtender1" BorderColor="White"
                    Radius="10" Corners="All" TargetControlID="pnlGrid" runat="server">
                </aspajax:RoundedCornersExtender>
                <table width="100%" cellspacing="0" cellpadding="0" style="margin-top:5px">
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlGrid" Width="950px" Height="430px" runat="server" BackColor="white">
                                <div style="height: 10px;padding-top:10px">
                                </div>
                                <table width="100%">
                                    <tr>
                                        <td align="center" style="vertical-align: top; width: 50%">
                                         
                                            <table width="100%" id="tblReport" runat="server">
                                                <tr>
                                                    <td align="left" style="color: White; width: 50%; font: bold 11pt Calibri; background-color:rgb(51, 51, 51)">
                                                        &nbsp;  Revenue Reports
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="vertical-align: top">
                                                    &nbsp;
                                         
                                                        <asp:BulletedList ID="bulletRevenue" runat="Server" DisplayMode="HyperLink" BulletImageUrl="~/Images/bulleticon.jpg"
                                                            ForeColor="Gray" Style="margin-bottom: 0px; margin-top: 0px;height:150px" 
                                                            >
                                                        </asp:BulletedList>
                                                    </td>
                                                </tr>
                                            </table>
                                             &nbsp;
                                              &nbsp;
                                        </td>
                                        <td align="center" style="vertical-align: top; width: 50%">
                                            <table width="100%" id="tblVariance" runat="server">
                                                <tr>
                                                    <td align="left" style="color: White; width: 50%; font: bold 11pt Calibri; background-color: rgb(51, 51, 51)">
                                                        &nbsp;  Volume Reports
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="vertical-align: top">
                                                     &nbsp;
                                                     <asp:BulletedList ID="bulletVolume" runat="Server"  DisplayMode="HyperLink" BulletImageUrl="~/Images/bulleticon.jpg"
                                                            ForeColor="Gray" Style="margin-bottom: 0px; margin-top: 0px;height:150px">
                                                        </asp:BulletedList>
                                                    </td>
                                                </tr>
                                            </table>
                                             &nbsp;
                                              &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="vertical-align: top; width: 50%">
                                            <table width="100%" id="tblMisc" runat="server">
                                                <tr>
                                                    <td align="left" style="color: White; width: 50%; font: bold 11pt Calibri; background-color: rgb(51, 51, 51)">
                                                        &nbsp;  Miscellaneous Reports
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="vertical-align: top">
                                                      &nbsp;
                                                        <asp:BulletedList ID="bulletMisc" runat="Server"  DisplayMode="HyperLink" BulletImageUrl="~/Images/bulleticon.jpg"
                                                            ForeColor="Gray" Style="margin-bottom: 0px; margin-top: 0px;height:100px" Target="">
                                                        </asp:BulletedList>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>

                                  

                                        <td align="center" style="vertical-align: middle; width: 50%">
                                            <table width="100%" id="tblAdmin" runat="server">
                                                <tr>
                                                    <td align="left" style="color: White; width: 50%; font: bold 11pt Calibri; background-color: rgb(51, 51, 51)">
                                                        &nbsp;  Admin Reports
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="vertical-align: top">
                                                      &nbsp;
                                                        <asp:BulletedList ID="bulletAdmin"  runat="Server" DisplayMode="HyperLink" BulletImageUrl="~/Images/bulleticon.jpg"
                                                            ForeColor="Gray" Style="margin-bottom: 0px; margin-top: 0px;height:100px">
                                                        </asp:BulletedList>
                                                    </td>
                                                </tr>
                                            </table>
                                         
                                        </td>
                                        
                                    </tr>

                                          <%--check--%>

                                   <%-- <tr>
                                       <td align="center" style="vertical-align: top; width: 50%">
                                            <table width="100%" id="Table1" runat="server">
                                                <tr>
                                                    <td align="left" style="color: White; width: 50%; font: bold 9pt Calibri; background-color: #C41502">
                                                        &nbsp; trial tab
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="vertical-align: top">
                                                        <asp:BulletedList ID="bulletTrial" runat="Server" DisplayMode="HyperLink" BulletImageUrl="~/Images/bulleticon.jpg"
                                                            ForeColor="Blue" Style="margin-bottom: 0px; margin-top: 0px">
                                                        </asp:BulletedList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>--%>
                                    <%--<tr>
                                    <td>
                                       
                                    </td>
                                    <td>
                                    <table align="left">
                                    <tr>
                                    <td >
                                        <asp:BulletedList ID="BulletedList1" runat="server"  DisplayMode="HyperLink" ForeColor="Blue" Style="margin-bottom: 0px; margin-top: 0px"
                                            onclick="RevenueMomemtum_Click1" >
                                            <asp:ListItem Text="Revenue Momemtum"></asp:ListItem>
                                        </asp:BulletedList>
                                            <asp:LinkButton ID="RevenueMomemtum1" runat="server" 
                                                onclick="RevenueMomemtum_Click1">RevenueMomemtum</asp:LinkButton>
                                    </td>
                                    </tr>
                                    </table>
                                    </td>
                                    </tr>--%>
                                    <asp:Button ID="btnCCP" runat="server" Text="" style="display:none" OnClick="btnCCP_Click"
                                        BorderStyle="None" Width="0px" Height="0" />
                                   
                                    <asp:Button ID="btnUserDetails" runat="server" Text="" style="display:none" OnClick="btnUserDetails_Click"
                                        BorderStyle="None" Width="0px" Height="0" />

<%--                                    <asp:Button ID="btnNewProjectList" runat="server" Text="" Visible="true" OnClick="btnNewProjectList_Click"
                                        BorderStyle="None" Width="0px" Height="0" />--%>
<%--                                    <asp:Button ID="btnUserDetails" runat="server" Text="" Visible="true" OnClick="btnUserDetails_Click"
                                        BorderStyle="None" Width="0px" Height="0" />--%>
                                   <%-- <asp:Button ID="btnRevenueMomentum" runat="server" Text="" Visible="true" OnClick="RevenueMomemtum_Click1"
                                        BorderStyle="None" Width="0px" Height="0" />--%>
                                    <%--  <asp:Button ID="btnInpipe" runat="server" Text="" Visible="true" OnClick="btnInpipe_Click"
                                        BorderStyle="None" Width="0px" Height="0"/>
                                    <asp:Button ID="btnAllParam" runat="server" Text="" Visible="true" OnClick="btnAllParam_Click"
                                      BorderStyle="None" Width="0px" Height="0" />
                                    <asp:Button ID="btnComparison" runat="server" Text="" Visible="true" OnClick="btnComparison_Click"
                                      BorderStyle="None" Width="0px" Height="0" />--%>
                                    <%--<asp:Button ID="btnBEReport" runat="server" Text="" Visible="true" OnClick="btnBEReport_Click"
                                      BorderStyle="None" Width="0px" Height="0" />--%>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <div style="height: 15px">
                </div>
            </div>
                  <%--<asp:Button ID="btnUserDetails" runat="server" Text="Button" onclick="btnUserDetails_Click" />--%>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCCP" />
          <asp:PostBackTrigger ControlID="btnUserDetails" />
<%--          <asp:PostBackTrigger ControlID="btnNewProjectList" />--%>
            <%--<asp:PostBackTrigger ControlID="btnAllParam" />
            <asp:PostBackTrigger ControlID="btnComparison" />--%>
            <%--<asp:PostBackTrigger ControlID="btnBEReport" />--%>
        </Triggers>

    </asp:UpdatePanel>
    <asp:HiddenField id="RevMomEAS" runat="server"></asp:HiddenField>

      <div id="myModalReport" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
         <%-- <h4 id="modalhdng" style="text-align: center; padding-top: 10px; color: black">
                Modal Dialog
            </h4>--%>
        <h4 id="modalhdng" class="modal-title">Report Name</h4>
      </div>
      <div class="modal-body" style="margin-top:0px!important;margin-left:0px!important">
          
       
      </div>
      <div class="modal-footer">
        <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
      </div>
    </div>

  </div>
</div>

    </body>
</asp:Content>
