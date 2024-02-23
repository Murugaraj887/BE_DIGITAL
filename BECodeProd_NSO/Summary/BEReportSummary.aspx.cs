using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Configuration;

namespace BECodeProd.Summary
{
    public partial class BEReportSummary : BasePage
    {
        BEDL service = new BEDL();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (!IsPostBack)
            {
               
                string cmd = "select distinct txtRole from beuseraccess_nso where txtUserId='" + Session["UserID"].ToString() + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmd);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                string cmd1 = "select distinct txtDMorSDM from beuseraccess_nso where txtUserId='" + Session["UserID"].ToString() + "'";
                DataSet ds1 = new DataSet();
                ds1 = service.GetDataSet(cmd1);
                DataTable dt1 = new DataTable();
                dt1 = ds1.Tables[0];

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();//Web Address of your report server (ex: http://rserver/reportserver (http://rserver/reportserver)) 

                string ServerURL = ConfigurationManager.AppSettings["ServerURL"].ToString();

                ReportViewer1.ServerReport.ReportServerUrl = new Uri(ServerURL); // Report Server URL

                if (dt.Rows[0][0].ToString() == "DM" || dt.Rows[0][0].ToString() == "Anchor")
                {

                     
                    //ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["SummaryreportDM"].ToString();
                }
                else
                {
                     
                    //ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["Summaryreport"].ToString();
                }

                


                //ReportParameter[] param = new ReportParameter[1];
                //param[0] = new ReportParameter("User", Session["UserID"].ToString(), true);
                //ReportViewer1.ServerReport.SetParameters(param);

              

            }
        }
    }
}