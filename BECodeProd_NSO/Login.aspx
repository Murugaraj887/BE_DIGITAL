<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" Codebehind="Login.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%-- <link href="Styles/Site.css" rel="stylesheet" type="text/css" />--%><script
        src="Scripts/JQuery.js" type="text/javascript"></script>
    <title>Digital BE - Login</title>
    <style type="text/css">
        body
        {
            background: #b6b7bc;
            font-size: 9pt;
            font-family: "Calibri";
            margin: 0px;
            padding: 0px;
            color: #696969;
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
        .TextBox
        {
            font-family: Calibri;
            font-size: 9pt;
        }
        
        a:link
        {
            font-family: Calibri;
            font-size: 9pt;
            font-weight: bold;
            text-decoration: none;
            color: Blue;
            background-color: transparent;
        }
        
        .menuMain
        {
            color: black; /*background-image : url(images/image003.png);*/
        }
        .menuTop
        {
            color: black;
            font-family: Calibri;
            font-size: 11px;
            font-weight: bolder;
        }
        
        .style2
        {
            height: 23px;
        }
          select::-ms-expand {
    display: none;
}
          

    </style>
   <script src="Select2/JScriptSelect2.js" type="text/javascript"></script>
    <link href="Select2/select2.css" rel="stylesheet" type="text/css" />
   
    <script type="text/javascript">

        $(document).ready(function () {
            $("#ddlEmpList").select2({
                selectOnClose: true
            }
            ).on("change", function () {

                setTimeout(function () {
                    $('#btnLogin').focus();
                });
            });
        });


    </script>

    <script type="text/javascript">
        function DoAnchorWala(ddl) {

            var selectedValue = ddl.value + '';
            var hnd = document.getElementById('hndValue');
            var strUser = ddl.options[ddl.selectedIndex].text;
            hnd.value = strUser;

            var trRole = document.getElementById('TRRole');
            var trdel = document.getElementById('TRDel');
            if (selectedValue == '1_0') {

                trRole.style.visibility = "visible";
                trdel.style.visibility = "hidden";
            }
            if (selectedValue == '0_0') {
                trRole.style.visibility = "hidden";
                trdel.style.visibility = "hidden";
            }
            if (selectedValue == '0_1') {
                trRole.style.visibility = "hidden";
                trdel.style.visibility = "visible";
            }

            if (selectedValue == '1_1') {
                trRole.style.visibility = "visible";
                trdel.style.visibility = "visible";

            }

        }

        function changeenteras(thisctrl) {

            var selected = $(':checked', $('#rdbDel'))[0].value;

            if (selected == 'Delegated')
            // document.getElementById('TRRole').style.visibility = "hidden";
                $('#TRRole').hide();
            else
            // document.getElementById('TRRole').style.visibility = "visible";
                $('#TRRole').show();

        }
    </script>
    <link rel="stylesheet" href="boot.css"/>
  
</head>
<body>
    <form id="form1" runat="server">
      
   <%-- <asp:RoundedCornersExtender ID="RoundedCornersExtender1" BorderColor="White" Radius="10"
        Corners="All" TargetControlID="pnlGrid" runat="server">
    </asp:RoundedCornersExtender>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div  style="border: 1px solid;border-radius:8px;background-color:White;margin:0px auto;width:450px;margin-top:200px;padding-left:30px" >
   <div align="center"> <h2 style="color:brown">Login to Digital BE Application</h2> 
    <div style="margin-bottom:10px"><span style="float:left;margin-top:5px;margin-right:10px;font-size:small">Login as: </span> 
    <asp:DropDownList ID="ddlEmpList"  style="float:left" runat="server" Font-Size="10pt"
                                    onchange="DoAnchorWala(this);" Width="300px">
                                </asp:DropDownList>
                                <div style="clear:both">
                                <asp:Button ID="btnLogin" runat="server" Text=" Login " class="btn btn-info btn-sm" Height="25" style=" padding-top:3px; margin-top:10px!important;border:1px solid lightgray;font-size:small" OnClick="btnLogin_Click" />
                                <asp:HiddenField ID="hndValue" Value="" runat="server" /></div>
                                </div> 
   
   </div>

    

      


    </div>
   
    </form>
</body>
</html>
