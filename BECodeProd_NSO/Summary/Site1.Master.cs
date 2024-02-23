using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData;
using System.Configuration;

namespace BECodeProd.Summary
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private BEDL service = new BEDL();
        public string fileName = "BEData.SiteMaster";
        Logger logger = new Logger();
        string accessDenied = "alert('Access denied'); return false;";
        protected void Page_Load(object sender, EventArgs e)
        {

            //try
            {
                string status = service.CheckStatus();

                if (status != "Active")
                {
                    //Response.Redirect("Process.htm");
                }

                if (!Page.IsPostBack)
                {


                    string userId = Session["userid"].ToString();
                    string Role = Session["Role"].ToString();
                    if (Role == "Anchor")
                    {
                        //if (Convert.ToInt32(Session["RadioButtonSelected"].ToString()) == 0)
                        //{
                        //    //lblWelcome.Text = "( " + Session["Role"] + " -'DM')";

                        //    lblWelcome.Text = "( Account Anchor)";
                        //}
                        //else
                        //{
                        //    lblWelcome.Text = "( " + Session["Role"] + " -'SDM')";
                        //}

                        lblWelcome.Text = "( Account Anchor)";
                    }
                    else
                    {
                        lblWelcome.Text = "( " + Session["Role"] + " )";
                    }

                    if (Session["LoginRole"].ToString() == "Admin" || Session["LoginRole"].ToString() == "PnA")
                    {
                        hypSwitchUser.Visible = true;
                        lnkUplaod.Visible = false;
                        lnkbtnBEAdmin.Visible = true;

                    }
                    else if (Session["LoginRole"].ToString() == "UH" || Session["LoginRole"].ToString() == "SOH" || Session["LoginRole"].ToString() == "DH")
                    {
                        hypSwitchUser.Visible = false;
                        lnkbtnBEAdmin.Visible = false;
                    }
                    else
                    {
                        hypSwitchUser.Visible = false;
                        lnkUplaod.Visible = false;
                        lnkbtnBEAdmin.Visible = false;

                    }



                }

            }


        }


        protected void hypSignOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Login.aspx");
        }

        protected void hypAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("../BEAdmin.aspx");
        }

        protected void hypSwitchUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Login.aspx");
        }

        protected void btnDemand_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://BLRKEC72629S:9876/Login.aspx?site=demand");
        }

        protected void btnVisa_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://BLRKEC72629S:9876/Login.aspx?site=demand");
        }
        protected void btnExpense_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://BLRKEC72629S:7777/Login.aspx?site=expense");
        }
        protected void btnBE_Click(object sender, EventArgs e)
        {
            Response.Redirect("../SDMView.aspx");
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            string Report = ConfigurationManager.AppSettings["Report"].ToString();
            Response.Redirect(Report);
            //Response.Redirect("/beapp/Summary/BeReportSummary.aspx?#menu1");
            //Response.Redirect("/Summary/BeReportSummary.aspx?#menu1");
           
          
        }
        protected void btnSummary_Click(object sender, EventArgs e)
        {
            string Summary = ConfigurationManager.AppSettings["Summary"].ToString();
            Response.Redirect(Summary);
            //Response.Redirect("/Summary/BeReportSummary.aspx?#home");
           // Response.Redirect("/beapp/Summary/BeReportSummary.aspx?#home");

        }
        protected void btnSubcon_Click(object sender, EventArgs e)
        {
            Response.Redirect("../SubConHome.aspx");
        }

        protected void btnBE_Click1(object sender, EventArgs e)
        {
            Response.Redirect("../Login.aspx");
        }

        protected void lnkUplaod_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://nebula:1212/Login.aspx");
        }

        protected void lnkbtnMCCDMSDM_Click(object sender, EventArgs e)
        {
            Response.Redirect("../MCCDMSDMChange.aspx");
        }

        protected void lnkbtnBEAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("../BEAdmin.aspx");
        }

        protected void lnkBtnFAQ_Click(object sender, EventArgs e)
        {

        }

        //protected void ddlUpload_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Response.Redirect("DemandUpload.aspx");
        //}
    }

    public static class StringExtension
    {
        public static string ToLowerTrim(this string s)
        {
            string returnValue = s + "";
            returnValue = returnValue.Trim().ToLower();
            return returnValue;
        }
    }
}

