
<%@ Page Language="C#" AutoEventWireup="true" Title="SDM Details"
    Inherits="SDMDetails" Codebehind="SDMDetails.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="DownloadOptions" content="noopen">
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
        .mGrid
        {
            width: 100%;
            background-color: #fff; /* margin: 5px 0 10px 0;*/
            border: solid 1px #525252;
            border-collapse: collapse;
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

        .HiddenCol{display:none;}

    </style>
    <script type="text/javascript">
      
     



    </script>

    <link rel="stylesheet" href="boot.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Label ID="lblCurr" runat="server" 
             
            style="font-family: Calibri; font-size: small; color: #0000FF; font-weight: 700;" visible="false" 
             Text="Label"></asp:Label>
          <br />
          <asp:GridView ID="grdBEDMView" runat="server" AutoGenerateColumns="False" 
                                                    EmptyDataText="No records found"  OnRowCreated="grdBEDMView_RowCreated"
                                                    CssClass="mGrid">
                                                    <Columns>
                                                      
                                                     
                                                        <asp:TemplateField HeaderText="MCC" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMCC" Text='<%# Bind("txtMasterClientCode") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Native Currency" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNativeCurrency" Text='<%# Bind("txtNativeCurrency") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="SDM Mail Id" SortExpression="SkillType" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth1" Text='<%# Bind("txtSDMMailId") %>' 
                                                                     runat="server"></asp:label>
                                                                 
                                                            </ItemTemplate>
                                                            
                                                          
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                  
                                                        <asp:TemplateField HeaderText="Month1" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth1" Text='<%# Bind("fltSDMMonth1BE") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                          
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month2" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth2" Text='<%# Bind("fltSDMMonth2BE") %>'
                                                                   runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                           
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                           
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month3" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth3" Text='<%# Bind("fltSDMMonth3BE") %>'
                                                                   runat="server"></asp:label>
                                                            </ItemTemplate>
                                                           
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                           
                                                          
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="BETotal" SortExpression="TotalVol" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBKLeft" Text='<%# Bind("fltSDMQuarterBE") %>' runat="server"></asp:Label>
                                                              
                                                            </ItemTemplate>
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                          
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Booked Business (Like RTBR)" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="txtBKMonth1" Text='<%# Bind("fltBK1") %>'
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Extensions or deals already won" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="txtBKMonth2" Text='<%# Bind("fltBK2") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                         
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                         
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Proposals already submitted but still open" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="txtBKMonth3" Text='<%# Bind("fltBK3") %>' 
                                                                    runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Other opportunities WIP (Not submitted yet)" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="txtBKMonth4" Text='<%# Bind("fltBK4") %>' 
                                                                    runat="server"></asp:label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SDM Remarks" SortExpression="SkillType" ItemStyle-HorizontalAlign="left">
                                                            <ItemTemplate>
                                                                <asp:label ID="txtRemarks" Text='<%# Bind("txtSDMBERemarks") %>' 
                                                                    runat="server"></asp:label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                        </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                              <asp:GridView ID="gvrtbr" runat="server" AutoGenerateColumns="False"  OnRowCreated="gvrtbr_RowCreated"
                                                    EmptyDataText="No records found" ShowFooter="FALSE"
                                                    CssClass="mGrid">
                                                        <Columns>
                                                          <asp:TemplateField HeaderText="MCC" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMCC" Text='<%# Bind("MCC") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Native Currency" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNativeCurrency" Text='<%# Bind("NC") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
  <asp:TemplateField HeaderText="Month1" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblMonth1USD" Text='<%# Bind("M1RTBRNC") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                          
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month2" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblMonth2USD" Text='<%# Bind("M2RTBRNC") %>'
                                                                   runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                           
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                           
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month3" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblMonth3USD" Text='<%# Bind("M3RTBRNC") %>'
                                                                   runat="server"></asp:label>
                                                            </ItemTemplate>
                                                           
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                           
                                                          
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="BETotal" SortExpression="TotalVol" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalUSD" Text='<%# Bind("RTBRTotalNC") %>' runat="server"></asp:Label>
                                                              
                                                            </ItemTemplate>
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                          
                                                        </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Month1" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblMonth1NC" Text='<%# Bind("M1RTBRUSD") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                          
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month2" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblMonth2NC" Text='<%# Bind("M2RTBRUSD") %>'
                                                                   runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                           
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            
                                                           
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month3" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblMonth3NC" Text='<%# Bind("M3RTBRUSD") %>'
                                                                   runat="server"></asp:label>
                                                            </ItemTemplate>
                                                           
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                           
                                                          
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="BETotal" SortExpression="TotalVol" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalNC" Text='<%# Bind("RTBRTotalUSD") %>' runat="server"></asp:Label>
                                                              
                                                            </ItemTemplate>
                                                          
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                          
                                                        </asp:TemplateField>
                                                        </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                        </br>
                                                        <asp:GridView ID="grdBEDMViewVol" runat="server" AutoGenerateColumns="False" 
                                                    EmptyDataText="No records found"  OnRowCreated="grdBEDMViewVol_RowCreated"
                                                    CssClass="mGrid">
                                                        <Columns>
                                                          
                                                        <asp:TemplateField HeaderText="MCC" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMCC" Text='<%# Bind("txtMasterClientCode") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                       
                                                       
                                                        <asp:TemplateField HeaderText="Native Currency" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNativeCurrency" Text='<%# Bind("txtNativeCurrency") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="SDM Mail Id" SortExpression="SkillType" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth1" Text='<%# Bind("txtSDMMailId") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                          
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                           <asp:BoundField DataField="fltSDMMonth1onsite" HeaderText="DM1Onsite" ItemStyle-HorizontalAlign = "Center" />
                <asp:BoundField DataField="fltSDMMonth1offsite" HeaderText="DM1Offshore" ItemStyle-HorizontalAlign = "Center" />
                <asp:BoundField DataField="fltSDMMonth2onsite" HeaderText="DM2Onsite" ItemStyle-HorizontalAlign = "Center" />
                <asp:BoundField DataField="fltSDMMonth2offsite" HeaderText="DM2Offshore" ItemStyle-HorizontalAlign = "Center" />
                <asp:BoundField DataField="fltSDMMonth3onsite" HeaderText="DM3Onste" ItemStyle-HorizontalAlign = "Center" />
                <asp:BoundField DataField="fltSDMMonth3offsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Center" />
                 <asp:BoundField DataField="fltSDMTotalonsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Center" />
                  <asp:BoundField DataField="fltSDMTotaloffsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Center" />

                <asp:BoundField DataField="fltSDmTotalVolume" HeaderText="Total" ItemStyle-HorizontalAlign = "Center" />
                                                        </Columns>
                                                        
                                                        </asp:GridView>

                                                        
                                                        <br />
                                                       
                                                           <asp:GridView ID="gvAlcon" runat="server" AutoGenerateColumns="False"  OnRowCreated="gvAlcon_RowCreated"
                                                    EmptyDataText="No records found" ShowFooter="true"
                                                    CssClass="mGrid">
                                                        <Columns>
                                                          
                                                        <asp:TemplateField HeaderText="MCC" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMCC" Text='<%# Bind("Mcc") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                  
                                                  <asp:TemplateField HeaderText="Native Currency" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNativeCurrency" Text='<%# Bind("CurrencyCode") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            
                                                        </asp:TemplateField>
                                                         <asp:TemplateField  ControlStyle-CssClass="HiddenCol" FooterStyle-CssClass="HiddenCol" ItemStyle-CssClass="HiddenCol" HeaderStyle-CssClass="HiddenCol"   HeaderText="DM Mail Id" Visible="false" SortExpression="SkillType" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth1" Text='<%# Bind("DmMailID") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                          
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                           <asp:BoundField DataField="M1Onsite" HeaderText="DM1Onsite" 
                                                                ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M1Offsite" HeaderText="DM1Offshore" ItemStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M2Onsite" HeaderText="DM2Onsite" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M2Offsite" HeaderText="DM2Offshore" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M3Onsite" HeaderText="DM3Onste" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M3Offsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                 <asp:BoundField DataField="TotalOnsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                  <asp:BoundField DataField="TotalOffsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Right" >

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>

                <asp:BoundField DataField="TotalVol" HeaderText="Total" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                        
                                                                <FooterStyle HorizontalAlign="Right" BackColor="#CC0000" />
                                                        
                                                        </asp:GridView>
                                                        <br />





                                                        
                                                        <asp:GridView ID="gvPBS" runat="server" AutoGenerateColumns="False"  OnRowCreated="gvPBS_RowCreated"
                                                    EmptyDataText="No records found" ShowFooter="true"
                                                    CssClass="mGrid">
                                                        <Columns>
                                                          
                                                        <asp:TemplateField HeaderText="MCC" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMCC" Text='<%# Bind("Mcc") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                           
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                           
                                                        </asp:TemplateField>
                                                  
                                                  <asp:TemplateField HeaderText="Native Currency" SortExpression="SkillType" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNativeCurrency" Text='<%# Bind("CurrencyCode") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            
                                                        </asp:TemplateField> 
                                                         <asp:TemplateField ControlStyle-CssClass="HiddenCol" FooterStyle-CssClass="HiddenCol" ItemStyle-CssClass="HiddenCol" HeaderStyle-CssClass="HiddenCol"  HeaderText="DM Mail Id" Visible="false" SortExpression="SkillType" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:label ID="lblDMMonth1" Text='<%# Bind("DmMailID") %>' 
                                                                     runat="server"></asp:label>
                                                            </ItemTemplate>
                                                            
                                                          
                                                            <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                            </asp:TemplateField>
                                                           <asp:BoundField DataField="M1Onsite" HeaderText="DM1Onsite" 
                                                                ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M1Offsite" HeaderText="DM1Offshore" ItemStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M2Onsite" HeaderText="DM2Onsite" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M2Offsite" HeaderText="DM2Offshore" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M3Onsite" HeaderText="DM3Onste" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                <asp:BoundField DataField="M3Offsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                 <asp:BoundField DataField="TotalOnsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                  <asp:BoundField DataField="TotalOffsite" HeaderText="DM3OffShore" ItemStyle-HorizontalAlign = "Right" >

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>

                <asp:BoundField DataField="TotalVol" HeaderText="Total" ItemStyle-HorizontalAlign = "Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                        
                                                                <FooterStyle HorizontalAlign="Right" BackColor="#CC0000" />
                                                        
                                                        </asp:GridView>
    </div>
   
    </form>
</body>
</html>
