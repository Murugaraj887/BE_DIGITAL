<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="SessionTimeOut" Codebehind="SessionTimeOut.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">




         closePopup();
         function closePopup() {

             var win = window.open("", "pop");
             win.moveTo(0, 0);
             win.blur();
             win.close();

             //              document.getElementById("ifr").contentWindow.document.write('popUpWindow = window.open("", "pop", "width=1,height=1");');
             //              document.getElementById("ifr").contentWindow.document.write('popUpWindow.close();');

         }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellspacing="0" cellpadding="0" align="center" width="100%" style="padding-right: 0px;
        padding-left: 0px; padding-bottom: 0px; padding-top: 0px; margin: 0px;">
        <tr valign="bottom" style="height: 50px">
            <td style="background-color:rgb(51, 51, 51); width: 80px" valign="middle" align="center">
            </td>
            <td colspan="2" style="background-color:rgb(51, 51, 51); width: 850px"
                valign="bottom">
                <%--<asp:HyperLink ID="hypSwitchUser" Font-Names="Calibri" Font-Size="11px" Font-Bold="True" ForeColor="#FFCB8B" NavigateUrl="~/Login.aspx?reset=true" runat="server">Switch User</asp:HyperLink>--%>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <table border="0" cellspacing="0" cellpadding="0" align="center" width="100%" style="height: 424px">
                    <tr>
                        <td style="width: 0px; height: 415px;">
                        </td>
                        <td style="width: 100%; height: 415px; background-color: White" valign="top">
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <h1 class="errorPanel">
                                           Session Expired
                                        </h1>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        
 Click<asp:HyperLink ID="HyperLink3" NavigateUrl="~/sessiontimeout.aspx" runat="server">here</asp:HyperLink>   to launch the application again. 
                                       
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="background-color:rgb(51, 51, 51); height: 21px" align="center"
                colspan="3">
                <table>
                    <tr>
                        <td style="width: 120px">
                        </td>
                        <td style="width: 650px" align="center">
                            <asp:Label ID="copyright" ForeColor="#FFCB8B" runat="server" Text=" Copyright © 2012 Infosys Limited. All rights reserved."></asp:Label>
                        </td>
                        <td style="width: 200px">
                            &nbsp;
                            <asp:Label ID="Label4" runat="server" Text="|" CssClass="WhiteHyperLink" Height="15px"></asp:Label>&nbsp;
                            <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="http://sparsh/V1/" Target="_blank"
                                CssClass="WhiteHyperLink" Font-Underline="false">Sparsh</asp:HyperLink>
                            <asp:Label ID="Label1" runat="server" Text="|" CssClass="WhiteHyperLink" Height="15px"></asp:Label>&nbsp;
                            <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="http://sparsh/v1/aspx/SparshWebapps.aspx"
                                Target="_blank" CssClass="WhiteHyperLink" Font-Underline="false">Webapps</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
