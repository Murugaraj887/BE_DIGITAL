<%@ Page Title="" Language="C#" MasterPageFile="~/Summary/Site1.Master" AutoEventWireup="true" CodeBehind="BEReportSummary.aspx.cs" Inherits="BECodeProd.Summary.BEReportSummary" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=8,IE=9,IE=10" />
    <title>Digital Reports.</title>
    <%--<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"/>
  <script  type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>--%>

  <link href="../Styles/bootstrap.min.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery_1.12.2.min.js" type="text/javascript"></script>
  <script src="../Scripts/bootstrap.min.js" type="text/javascript"></script>

 
   
  <style type="text/css">
        iframe, object, embed {
        width: 100%;
        height:100%;
        display: block !important;
  
        }
        .nav-tabs>li>a{
 
}
.tab-content{
    
   
    padding:5px
}


 .nav-tabs > li.active > a,
       
        .nav-tabs > li.active > a:focus{
            color: #fff;
            background-color: lightsteelblue;  
        } 

.nav-tabs > li > a:hover{
   
    color:#fff;
}

.nav-tabs > li > a {padding-top:1px !important; padding-bottom:1px !important;}
.nav {min-height:1px !important}

#summary:focus
{
    outline:0px!important;
}
#reports:focus
{
    outline:0px!important;
}

#summary,#reports
{
    margin-left:5px;
     font-size:smaller;
}

#summary:hover
{
   
    color:Black;
    background-color: lightsteelblue;  
}
#reports:hover
{
    color:Black;
     background-color: lightsteelblue; 
}

</style>
 <script type='text/javascript'>

     $(function () {

         var iFrames = $('iframe');
         var hx = $(window).height() - 70;



         iFrames.load(function () {
             this.style.height = 800 + 'px';

         });


     });
     $(function () {

         $(window).resize(function () {
             var newhght = $(window).height() - 70;
             $('iframe').css({ height: newhght + 'px' });
         });
     });
     
     $(function () {

         var hash = document.location.hash;
         if (hash) {
             $('.nav-tabs a[href="' + hash + '"]').tab('show');

         }


     });
    </script>


  
    <script>

        function FixSafari() {
            var element = document.getElementById("ctl00_MainContent_ReportViewer1_ctl03");
            if (element) {
                element.style.overflow = "visible";  //default overflow value
            }
        }


        if (window.addEventListener) // W3C standard
        {
            window.addEventListener('load', FixSafari, false); // NB **not** 'onload'
        }
        else if (window.attachEvent) // Microsoft
        {
            window.attachEvent('onload', FixSafari);
        }
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div style="margin-top:5px">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>--%>
       
        <ul class="nav nav-tabs" style="border-style:none !important">
       <%-- <li  class="active"><a id="summary" data-toggle="tab" href="#home">BE Summary</a></li>--%>
        <li ><a id="reports" data-toggle="tab" href="#menu1">Digital BE Reports</a></li>
        </ul>
    </div>
    <label id="userid" style="display:none" runat="server"></label>
    
    <div class="tab-content" style="border-style:none !important">
        <div id="home" class="tab-pane fade ">
            <%--<iframe id="iframeSSRS"  src="http://nebula:1212/Reports/Pages/Report.aspx?ItemPath=%2fDashboardReportsTest%2fBEReportsSummary" frameborder="0" style="border-style:none !important" scrolling="no"></iframe>--%>
          
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" >
            </rsweb:ReportViewer>
           
        </div>
       
        
        <div id="menu1" class="tab-pane fade in active">
            <iframe src="../Reports.aspx" frameborder="0" style="border-style:none !important" scrolling="no"></iframe>
       </div>
        
       
    </div>
 
</asp:Content>
