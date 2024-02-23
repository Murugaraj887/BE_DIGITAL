<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true"
    CodeBehind="ExchangeRates.aspx.cs" Inherits="BECodeProd.ExchangeRates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="GENERATOR" content="MSHTML 10.00.9200.17609" />
    <style type="text/css">
         .container
        {
            margin-left:115px!important;
        }
        .style2
        {
            width: 1040px;
        }
        select::-ms-expand
        {
            display: none;
        }
        
        .nav-tabs > .active > a, .nav-tabs > .active > a:hover
        {
            outline: 0;
        }
    </style>
    <link rel="stylesheet" href="Styles/css/style.css" />
    <style>
        .nav-tabs
        {
            border-bottom: 2px solid #DDD;
        }
        .nav-tabs > li.active > a, .nav-tabs > li.active > a:focus, .nav-tabs > li.active > a:hover
        {
            border-width: 0;
        }
        .nav-tabs > li > a
        {
            border: none;
            color: #666;
        }
        .nav-tabs > li.active > a, .nav-tabs > li > a:hover
        {
            border: none;
            color: gray !important;
            background: transparent;
        }
        .nav-tabs > li > a::after
        {
            content: "";
            background: #4285F4;
            height: 2px;
            position: absolute;
            width: 100%;
            left: 0px;
            bottom: -1px;
            transition: all 250ms ease 0s;
            transform: scale(0);
        }
        .nav-tabs > li.active > a::after, .nav-tabs > li:hover > a::after
        {
            transform: scale(1);
        }
        .tab-nav > li > a::after
        {
            background: #21527d none repeat scroll 0% 0%;
            color: #fff;
        }
        .tab-pane
        {
            padding: 15px 0;
        }
        .tab-content
        {
            padding: 20px;
        }
        
        .card
        {
            background: #FFF none repeat scroll 0% 0%;
            box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.3);
            margin-bottom: 30px;
        }
        body
        {
            background: #EDECEC;
            padding: 50px;
        }
    </style>
    <link href="content/bootstrap-wysihtml5.css" rel="stylesheet" />
    <link href="content/wysiwyg-color.css" rel="stylesheet" />
    <link href="content/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="content/github.css" rel="stylesheet" />
    <link href="content/prettify.css" rel="stylesheet" />
    <script src="content/jquery-1.10.2.js"></script>
    <script src="content/bootstrap.min.js" type="text/javascript"></script>
    <script src="content/wysihtml5-0.3.0.js"></script>
    <script src="content/bootstrap-wysihtml5.js"></script>
    <script src="content/highlight.pack.js"></script>
    <script src="content/prettify.js"></script>
    <script type="text/javascript">

        function modalPop() {
        
            $('#myModalReport').on('shown.bs.modalexc', function () {
               
                $(this).find('.modalexc-dialog').css({
                    width: '50%',
                    height: 'auto',
                    'max-height': '80%'

                });
            });

            GetInstruction();
           
            $('#modalexchdng').html($('#<%=ddlType.ClientID %> option:selected').text());

            setTimeout(function () {
                $('#myModalReport').modal('show');
                $('#myModalReport').show();
            }, 200);

        }

        function SaveInstruction() {
           
            var html = $(".textarea1").val() + '';
            html = html.replace(/\\/g, "\\\\");
            html = html.replace(/"/g, "“");

            $.ajax({ url: "WebService.asmx/SaveInstruction",
                contentType: "application/json; charset=utf-8",
                type: 'post',
                data: '{instruction: "' + html + '",instructionId:"' + InstructionId + '" }',
                success: function (result) {
                    alert('Instruction updated/saved successfully');
                }
            });
        }

        function Admin() {
           
            $.ajax({ url: "WebService.asmx/EnableEdit",
                contentType: "application/json; charset=utf-8",
                type: 'post',
                data: '{}',
                success: function (result) {
                    var s = result.d;
                    if (s == "YES") {
                        $("#pop").css("display", "block");
                    }
                }
            });
        }


        function GetInstruction() {
           
            var value = $('#<%=ddlType.ClientID %> option:selected').text()
            if (value == "Daily") {
                value = "BE-ExchangeRates";
            }
            else if (value == "<--Select-->") {
                $("#divInstruction").css("display", "none");
            }
            else {
                value = "Exchange Rates";
            }
            if (value != "" && value != "<--Select-->") {

                $.ajax({ url: "WebService.asmx/GetInstruction",
                    contentType: "application/json; charset=utf-8",
                    type: 'post',
                    data: '{ApplicationName: "' + value + '" }',
                    success: function (result) {

                        var InstructValue = JSON.parse(result.d)[0].InstructionText;
                        InstructValue = InstructValue.replace(/“/g, "\"");


                        InstructionId = JSON.parse(result.d)[0].sortorder;
                        $('.textarea1').data("wysihtml5").editor.setValue(InstructValue);
                        //                    $('.textarea').data("wysihtml5").editor.setValue(InstructValue);
                        $('.textarea').html(InstructValue);
                    }
                });
            }
            Admin();
        }
        var InstructionId;
    </script>
    <style type="text/css" media="screen">
        .modalexc
        {
            position: fixed;
            top: 50%;
            left: 25% !important;
            z-index: 1050;
            overflow: auto;
            width: 1200px !important;
        }
        .modalexc.fade
        {
            -webkit-transition: opacity .3s linear, top .3s ease-out;
            -moz-transition: opacity .3s linear, top .3s ease-out;
            -ms-transition: opacity .3s linear, top .3s ease-out;
            -o-transition: opacity .3s linear, top .3s ease-out;
            transition: opacity .3s linear, top .3s ease-out;
            top: -25%;
        }
        
        .btn.jumbo
        {
            font-size: 20px;
            font-weight: normal;
            margin-right: 10px;
            -webkit-border-radius: 6px;
            -moz-border-radius: 6px;
            border-radius: 6px;
        }
        
        
        .container, .navbar-fixed-top .container, .navbar-fixed-bottom .container
        {
            padding: 0px !important;
        }
        
        .modalexc-dialog
        {
            width: 1200px !important;
        }
        .modalexc
        {
        }
        
        .modalexc-header
        {
            padding-top: 5px !important;
            background-color: gray !important;
            height: 40px !important;
            border-top-right-radius: 3px;
            border-top-left-radius: 3px;
        }
        
        
        
        .modalexc-body
        {
            width: 1200px !important;
            padding: 0px !important;
        }
        
        .modalexc-title
        {
            color: floralwhite !important;
            font-family: Calibri !important;
        }
        .close
        {
            color: White !important;
            background-color: transparent !important;
            font-size: 1em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 300px; margin: 0px auto; font-family: Calibri; font-size: x-large;
        margin-top: 10px">
        Push Exchange Rates
    </div>
    <div style="margin-left:50px;">
    <a style="color:Maroon" href="http://nebula/Apps/ExchnageRateValidationApp/"> Validation </a>
    </div>
    <div style="margin: 20px; margin-left: 20px">
        <div class="container">
            <div class="row">
                <div class="col-md-14">
                    <div role="tabpanel" class="tab-pane active" style="display: none" id="home">
                        <div style="max-height: 100%; width: 500px; margin: 0px auto; padding-top: 40px">
                            <div id="div1" runat="server" style="max-height: 100%; width: 500px; margin: 0px auto overflow: auto">
                                <asp:Panel ID="pnlDownload" runat="server">
                                    <div style="float: left; font-family: Calibri; font-size: medium">
                                        Year
                                    </div>
                                    <div style="float: left; margin-left: 5px">
                                        <asp:DropDownList ID="ddlYear" runat="server" AppendDataBoundItems="True" CssClass="form-control"
                                            Width="100" Height="25" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            <asp:ListItem>-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left; margin-left: 5px; font-family: Calibri; font-size: medium">
                                        Quarter
                                    </div>
                                    <div style="float: left; margin-left: 5px">
                                        <asp:DropDownList ID="ddlQtr" runat="server" AutoPostBack="True" Height="25" CssClass="form-control"
                                            Width="100" OnSelectedIndexChanged="ddlQtr_SelectedIndexChanged">
                                            <asp:ListItem>-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left; margin-left: 15px;">
                                        <asp:Button ID="dwldTemp" runat="server" OnClick="dwldTemp_Click" class="btn btn-info btn-sm"
                                            Height="25" Style="background-image: url(/Images/download.png); background-position: 2px;
                                            background-repeat: no-repeat; padding-top: 2px!important; border: 1px solid lightgray;"
                                            Text="&nbsp;&nbsp;&nbsp;Download to Excel" /></div>
                                    <br />
                                </asp:Panel>
                            </div>
                        </div>
                        <div style="min-height: 130px; max-height: 100%; margin-left: 40px; width: 800px;
                            margin: 0px auto; margin-top: 10px">
                            <asp:GridView ID="gvData" runat="server" EmptyDataText="Sorry, No Data Found" OnPageIndexChanging="gvData_PageIndexChanging"
                                ForeColor="Gray" Font-Size="Small">
                                <PagerSettings FirstPageText="&lt;&lt;" LastPageText="&gt;&gt;" Mode="NumericFirstLast"
                                    NextPageText="&gt;" PageButtonCount="3" PreviousPageText="&lt;" />
                                <PagerStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="profile" >
                        <asp:UpdatePanel ID="upUpload" runat="server">
                            <ContentTemplate>
                                <div style="height: 85px; padding-top: 40px;">
                                    <div style="float: left;">
                                        <div style="float: left; font-family: Calibri; font-size: medium">
                                            Update Type</div>
                                        <div style="float: left; margin-left: 5px">
                                            <asp:DropDownList ID="ddlType" Height="25" runat="server" AppendDataBoundItems="True"
                                                CssClass="form-control" Width="100" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                <asp:ListItem Value="0"><--Select--></asp:ListItem>
                                                <asp:ListItem>Daily</asp:ListItem>
                                                <asp:ListItem>Weekly</asp:ListItem>
                                                <asp:ListItem>Monthly</asp:ListItem>
                                                <asp:ListItem>Quarterly</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div style="float: left; margin-left: 5px; font-family: Calibri; font-size: medium">
                                            Year</div>
                                        <div style="float: left; margin-left: 5px">
                                            <asp:DropDownList ID="ddlYear1" Height="25" runat="server" AppendDataBoundItems="True"
                                                CssClass="form-control" Width="100" AutoPostBack="True" OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged">
                                                <asp:ListItem>-Select-</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div style="float: left; margin-left: 5px; font-family: Calibri; font-size: medium">
                                            Quarter</div>
                                        <div style="float: left; margin-left: 5px">
                                            <asp:DropDownList ID="ddlQtr1" Height="25" runat="server" AppendDataBoundItems="True"
                                                CssClass="form-control" Width="100" AutoPostBack="True" OnSelectedIndexChanged="ddlQtr1_SelectedIndexChanged">
                                                <asp:ListItem>-Select-</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div style="float: left; margin-left: 5px">
                                            <asp:CheckBoxList ID="cbQuarter1" runat="server" Style="font-size: medium; font-family: Calibri"
                                                RepeatDirection="Horizontal" RepeatLayout="Flow" Visible="False">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <div style="float: left; margin-top: 3px; margin-left: 20px">
                                        <asp:FileUpload ID="fuUpload" runat="server" />
                                    </div>
                                    <div style="padding-top: 20px; clear: both; padding-left: 100px">
                                        <div style="float: left; margin-left: 5px; font-family: Calibri; font-size: medium">
                                            Current Timestamp</div>
                                        <div style="float: left; margin-left: 5px">
                                            <asp:TextBox ID="txtdate" runat="server" Width="208px"></asp:TextBox>
                                        </div>
                                        <div style="float: left; margin-left: 20px">
                                            <asp:Button ID="btnUpload" runat="server" class="btn btn-info btn-sm" Height="25"
                                                Style="background-image: url(/Images/upload.png); background-position: 2px; background-repeat: no-repeat;
                                                padding-top: 2px!important; border: 1px solid lightgray;" OnClick="btnUpload_Click"
                                                Text="&nbsp;&nbsp;&nbsp;&nbsp;Upload " />
                                            <asp:Label ID="lblError" runat="server"></asp:Label>
                                            <asp:Label ID="lblSuccess" runat="server"></asp:Label></div>
                                          <div style="float: left; margin-left: 20px">
                                              <asp:CheckBox ID="cbxExcp" 
                                                  Text="click here to enable the Upload on Exceptional cases"   runat="server" 
                                                  AutoPostBack="true" />
                                          </div>   

                                    </div>
                                </div>
                                <div id="divInstruction" runat="server" style="height: 55%; margin-top: 10px" visible="false"
                                    align="center">
                                    <table id="Table1" style="width: 1150px;" cellpadding="2" cellspacing="1">
                                        <tr>
                                            <td align="center" bgcolor="darkcyan" colspan="3">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Tahoma" ForeColor="White"
                                                    Text="Instructions             "></asp:Label>
                                            </td>
                                            <td bgcolor="darkcyan" align="right" style="width: 50px">
                                                <a id="pop" type="button" onclick="modalPop()" href="#" style="color: White; display: none">
                                                    &nbsp;edit&nbsp;&nbsp; </a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <div style="text-align: left!important; border: 1px solid gray; width: 100%; padding-left: 20px;
                                                    padding-right: 15px" class="textarea">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:TextBox ID="txtinstruction" Enabled="false" runat="server" Height="249px" TextMode="MultiLine"
                                        Width="1093px" Visible="false"></asp:TextBox>
                                </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlYear1" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlQtr1" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalReport" class="modalexc" role="dialog" style="display: none;" data-backdrop="static"
        data-keyboard="false">
        <div class="modalexc-dialog">
            <!-- Modal content-->
            <div class="modalexc-content">
                <div class="modalexc-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h2 id="modalexchdng" class="modalexc-title">
                        Editor</h2>
                </div>
                <div class="modalexc-body" style="margin-top: 0px!important; margin-left: 0px!important">
                    <textarea class="textarea1" style="width: 99%; height: 300px; line-height: 18px;
                        font-size: 14px;" placeholder="Enter text ...">Enter text ...</textarea>
                    <script>



                        $('.textarea1').wysihtml5({
                            "stylesheets": ["content/wysiwyg-color.css", "content/github.css"],
                            "color": true,
                            "size": 'small',
                            "html": true,
                            "format-code": true
                        });



                        function pageLoad() {
                            GetInstruction();
                        }

     
       
        
                    </script>
                </div>
                <div class="modalexc-footer">
                    <div style="text-align: center; margin: 0px auto">
                        </br>
                        <button class="btn btn-success" onclick="SaveInstruction()">
                            Save</button></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
