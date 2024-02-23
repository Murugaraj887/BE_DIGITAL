<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/AdminSite.Master" CodeBehind="ExhangeRateUpdate.aspx.cs" Inherits="BECodeProd.ExhangeRateUpdate" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <table width="100%" >
    <tr>
    <td>
    <asp:DropDownList ID="ddlRates" runat="server" AutoPostBack="true" 
            onselectedindexchanged="ddlRates_SelectedIndexChanged">
       <asp:ListItem>--Select--</asp:ListItem>
         <asp:ListItem>Weekly</asp:ListItem>
         <asp:ListItem>Monthly Actuals</asp:ListItem>
        <asp:ListItem>Quarterly Rates</asp:ListItem>
         <asp:ListItem>Bench mark Rates</asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    <tr align="left">
    <td style="text-align: left">    
        <asp:CheckBoxList ID="ckMonths" runat="server" Visible="false" 
            style="text-align: left" >       
         <asp:ListItem>Month1
        </asp:ListItem>
         <asp:ListItem>Month2
        </asp:ListItem>
        <asp:ListItem>Month3
        </asp:ListItem>
        </asp:CheckBoxList>        
    </td>    
    </tr>
    <tr>
    <td>
    <asp:Button ID="btUpdate" runat="server" Text="Update" onclick="btUpdate_Click" 
            Visible="False" />
    </td>
    </tr>
    <tr>
    <td>
     <asp:FileUpload ID="fpUpload" runat="server"></asp:FileUpload>
    </td>
    </tr>
    <tr>
    <td>
     <asp:Button ID="btUpload" runat="server" Text="Upload" />
    </td>
    </tr>
    </table>

        
       
       
    </div>
 


</asp:Content>
