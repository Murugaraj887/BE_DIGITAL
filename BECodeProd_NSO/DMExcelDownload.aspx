<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DMExcelDownload.aspx.cs" Inherits="BECodeProd.DMExcelDownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:GridView ID="gvDMExcel" runat="server" ShowFooter="True"
                                                    EmptyDataText="No records found" 
                                                    CssClass="mGrid" Width="100%" 
         CellPadding="4" ForeColor="#333333" GridLines="None" Font-Names="Calibri" 
                Font-Size="Small">
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
