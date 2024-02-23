<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoPage.aspx.cs" Inherits="BECodeProd.InfoPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<style>
  .mGrid
        {
            width: 100%;
            background-color: #fff; /* margin: 5px 0 10px 0;*/
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            font-size: 9pt;
        }

</style>
   
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="gvInfo" runat="server" CellPadding="4" ForeColor="#333333" CssClass="mGrid" 
            GridLines="None">
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
    
    </div>
    </form>    
</body>
</html>
