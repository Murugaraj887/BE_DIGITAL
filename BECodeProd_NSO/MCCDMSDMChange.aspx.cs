using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.OleDb;
using BEData.BusinessEntity;
using BEData;


    public partial class MCCDMSDMChange : BasePage
    {
        Logger logger = new Logger();
        public string fileName = "BEData.MCCDMSDMChange.cs";
        BEDL objbe = new BEDL();
        int mon = DateTime.Now.Month;
        string curqtr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (Page.IsPostBack)
            {
                string type = ddlChange.SelectedValue;
                string year = ddlYear.SelectedValue;
                string qtr = ddlQtr.SelectedValue;
                DataTable dt = new DataTable();
                //grdBEDU.DataBind();
                dt = objbe.GetWeeklyDatesMccDmSdm(qtr, year, "Weekly", DateTime.Now, DateTime.Now);
               

                //if (txtChange.Text == "")
                //{
                //    dt = new DataTable();
                //}
                //else
                //{
                //    dt = objbe.GetWeeklyDatesMccDmSdm(qtr, year, "Weekly", DateTime.Now, DateTime.Now);
                //    ModalPopupExtender1.TargetControlID = "btnChange2";
                //}

                if (dt.Rows.Count == 0)
                {
                    //if (type == "Currency")
                    //{
                    //    btnChange2.Visible = true;
                    //    btnChange.Visible = false;
                    //}
                    //else
                    //{
                        btnChange.Visible = true;
                        btnChange2.Visible = false;
                        btnChange2.Enabled = false;
                    //}

                }
                else
                {
                    if (type == "Currency")
                    {
                       
                        btnChange2.Visible = true;
                        btnChange.Visible = false;
                        btnChange2.Enabled = true;
                        //return;
                    }
                    else
                    {
                        btnChange.Visible = true;
                        btnChange2.Visible = false;
                        btnChange2.Enabled = false;
                    }
                }

            }

            else
            {
              

                tble1.Visible = false;
                tble2.Visible = false;
                tbl3.Visible = false;
                ddlChange.Items.Insert(0, "---Select---");

                string type = ddlChange.SelectedValue;


                // weekly 
                ddlQtrWeekly.DataTextField = "txtQuarter";
                ddlQtrWeekly.DataValueField = "txtQuarter";
                ddlQtrWeekly.DataSource = objbe.GetQuarterYearWeekly("Quarter", "");
                ddlQtrWeekly.DataBind();
                ddlQtrWeekly.Items.Insert(0, "--Select--");



                string qtrweek = ddlQtrWeekly.SelectedValue;
                ddlYearWeekly.DataTextField = "txtYear";
                ddlYearWeekly.DataValueField = "txtYear";
                ddlYearWeekly.DataSource = objbe.GetQuarterYearWeekly("Year", qtrweek);
                ddlYearWeekly.DataBind();
                ddlYearWeekly.Items.Insert(0, "--Select--");

                string yr = ddlYearWeekly.SelectedValue;
                // List<string> lstDelDateSum = objbe.GetWeeklyDateSum(yr, qtr);
                ddlDelDate.DataSource = objbe.GetWeeklyDateSum(yr, qtrweek); ;
                ddlDelDate.DataBind();

                for (int i = 0; i < ddlDelDate.Items.Count; i++)
                {
                    if (ddlDelDate.Items[i].Value != null && ddlDelDate.Items[i].Text != "")
                    {
                        ddlDelDate.Items[0].Selected = true;
                    }
                }

                ddlUpDate.DataSource = objbe.GetWeeklyDates();
                ddlUpDate.DataBind();
                ddlUpDate.Items.Insert(0, "--Select--");

                ddlWeeklyDate.DataSource = objbe.GetWeeklyDates();
                ddlWeeklyDate.DataBind();
                ddlWeeklyDate.Items.Insert(0, "--Select--");
               

                txtDate.Attributes.Add("onKeydown", "return PressReadOnly(event,this)");

                ddlTrendQuarter.DataTextField = "txtqtr";
                ddlTrendQuarter.DataValueField = "txtqtr";
                ddlTrendQuarter.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
                ddlTrendQuarter.DataBind();
                ddlTrendQuarter.Items.Insert(0, "--Select--");

                string trendqtr = ddlTrendQuarter.SelectedValue.Trim();

                ddlTrendYear.DataTextField = "txtyear";
                ddlTrendYear.DataValueField = "txtyear";
                ddlTrendYear.DataSource = objbe.GetBEReportQtrYear("Year", trendqtr);
                ddlTrendYear.DataBind();
                ddlTrendYear.Items.Insert(0, "--Select--");

                int val = 1;
                lblInfo.Visible = false;
                ddlFromFinYear.DataSource = objbe.GetCopyDataFinancialYear(val);
                ddlFromFinYear.DataBind();
                ddlFromFinYear.Items.Insert(0, "-Select-");

                ddlFromQuarter.Items.Insert(0, "-Select-");
              

                ddlToFinYear.DataSource = objbe.GetCopyDataFinancialYear(val);
                ddlToFinYear.DataBind();
                ddlToFinYear.Items.Insert(0, "-Select-");

                ddlToQuarter.Items.Insert(0, "-Select-");
            
                

                if (type == "Currency")
                {

                    btnChange2.Visible = true;
                    btnChange.Visible = false;
                }
                else
                {
                    btnChange.Visible = true;
                    btnChange2.Visible = false;
                    ModalPopupExtender1.Hide();
                }

            }

        }

        protected void ddlDMSDM_SelectedIndexChanged(object sender, EventArgs e)
        {

          

            string type = ddlChange.SelectedValue;
            if (type == "Master Client Code")
            {
                ddlDMSDM.Enabled = false;

            }

            else
            {

                ddlDMSDM.Enabled = true;
            }
            //else if (type == "SDM")
            //{



            tble1.Visible = true;
            tble2.Visible = false;
            tbl3.Visible = false;
            string dmsdm = ddlDMSDM.SelectedValue;

          



        }

        protected void ddlPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";

            tble2.Visible = false;
            tbl3.Visible = false;
            string type = ddlChange.SelectedValue;

            string pu = ddlPU.SelectedValue;
          

            ddlQtr.DataTextField = "txtQtr";
            ddlQtr.DataValueField = "txtQtr";
            ddlQtr.DataSource = objbe.GetQuarterYearDineshReport("Quarter", "0");
            ddlQtr.DataBind();


            ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            string qtr = ddlQtr.SelectedValue;


            ddlYear.DataTextField = "txtYear";
            ddlYear.DataValueField = "txtYear";
            ddlYear.DataSource = objbe.GetQuarterYearDineshReport("Year", qtr);
            ddlYear.DataBind();

            string year = ddlYear.SelectedValue;
            ddlMcc.DataTextField = "txtMCC";
            ddlMcc.DataValueField = "txtMCC";
            ddlMcc.DataSource = objbe.GetMCC_MccDMSDMChange(pu, qtr, year);
            ddlMcc.DataBind();
            ddlMcc.Items.Insert(0, "---Select---");

        }

        protected void ddlChange_SelectedIndexChanged(object sender, EventArgs e)
        {


            txtChange.Text = "";
            txtCurrent.Text = "";
            lblMessage.Visible = false;
            tble2.Visible = false;
            tbl3.Visible = false;

            lblMsg.Visible = false;
            lblInfo.Visible = false;
            rbtnAction.DataBind();
            rbtnAction.SelectedIndex = rbtnAction.Items.IndexOf(rbtnAction.Items.FindByValue("Update"));
            ddlUpDate.Enabled = true;
            txtDate.Enabled = true;
            img1.Visible = true;
            
            ddlWeeklyDate.Enabled = false;
            ddlQtrWeekly.Enabled = false;
            ddlYearWeekly.Enabled = false;
            ddlDelDate.Enabled = false;
            ddlTrendYear.Enabled = false;
            ddlTrendQuarter.Enabled = false;
            txtDate.Text = "";
            ddlUpDate.Text = "--Select--";
            ddlTrendQuarter.Text = "--Select--";
            ddlTrendYear.Text = "--Select--";
            ddlQtrWeekly.Text = "--Select--";
            ddlYearWeekly.Text = "--Select--";
            ddlWeeklyDate.Text = "--Select--";
            ddlDelDate.DataSource = objbe.GetWeeklyDateSum("", "");
            ddlDelDate.DataBind();
            ddlFromFinYear.Text = "-Select-";
            ddlFromQuarter.Text = "-Select-";
            ddlToFinYear.Text = "-Select-";
            ddlToQuarter.Text = "-Select-";
            
            //ddlPU.Items.Insert(0, "---Select---");
            //ddlMcc.Items.Insert(0, "---Select---");

            string type = ddlChange.SelectedValue;

            if (ddlChange.SelectedIndex == 0)
            {
                txtChange.Text = "";
                txtCurrent.Text = "";
                lblMessage.Visible = false;
                tble2.Visible = false;
                tbl3.Visible = false;
                tble1.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Invalid Selection');</script>", false);
                return;
            }
            if (type == "Master Client Code")
            {
                ddlDMSDM.Enabled = false;
            }
            else
            {
                ddlDMSDM.Enabled = true;
            }

            tble1.Visible = true;


            ddlPU.DataTextField = "txtPU";
            ddlPU.DataValueField = "txtPU";
            ddlPU.DataSource = objbe.GetPU_MccDMSDMChange();
            ddlPU.DataBind();
            ddlPU.Items.Insert(0, "---Select---");

            string pu = ddlPU.SelectedValue;
           

            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";

            ddlQtr.DataTextField = "txtQtr";
            ddlQtr.DataValueField = "txtQtr";
            ddlQtr.DataSource = objbe.GetQuarterYearDineshReport("Quarter", "0");
            ddlQtr.DataBind();

            ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            string qtr = ddlQtr.SelectedValue;


            ddlYear.DataTextField = "txtYear";
            ddlYear.DataValueField = "txtYear";
            ddlYear.DataSource = objbe.GetQuarterYearDineshReport("Year", qtr);
            ddlYear.DataBind();

            string year = ddlYear.SelectedValue;
            ddlMcc.DataTextField = "txtMCC";
            ddlMcc.DataValueField = "txtMCC";
            ddlMcc.DataSource = objbe.GetMCC_MccDMSDMChange(pu,qtr,year);
            ddlMcc.DataBind();
            ddlMcc.Items.Insert(0, "---Select---");

            string mcc = ddlMcc.SelectedValue;
            ddlDMSDM.DataTextField = "txtDMSDM";
            ddlDMSDM.DataValueField = "txtDMSDM";
          //  ddlDMSDM.DataSource = objbe.GetDMSDM_MccDMSDMChange(type, pu, mcc,qtr,year);
            ddlDMSDM.DataBind();
            ddlDMSDM.Items.Insert(0, "---Select---");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            txtChange.Text = "";
            txtCurrent.Text = "";
            string type = ddlChange.SelectedValue;
            int i = ddlPU.SelectedIndex;
            int j = ddlMcc.SelectedIndex;




            if (type == "Master Client Code")
            {
                if (ddlPU.SelectedIndex < 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select PU');</script>", false);
                    return;
                }

                else if (ddlMcc.SelectedIndex < 1)
                {

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select Master Client Code');</script>", false);
                    return;
                }
            }
            else
            {

                if (ddlPU.SelectedIndex < 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select PU');</script>", false);
                    return;
                }

                else if (ddlMcc.SelectedIndex < 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select Master Client Code');</script>", false);
                    return;

                }

                else if (ddlDMSDM.SelectedIndex < 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select DM/SDM/Currency');</script>", false);
                    return;

                }
            }

            string pu = ddlPU.SelectedValue;
            string mcc = ddlMcc.SelectedValue;
            string dmsdm = ddlDMSDM.SelectedValue;
            string qtr = ddlQtr.SelectedValue;
            string year = ddlYear.SelectedValue;
            string current = "";
            tble2.Visible = true;
            tbl3.Visible = true;
            //if(type=="Master Client Code")

            //string qtr = ddlQtr.SelectedValue;
            //string year = ddlYear.SelectedValue;
            grdBEDU.DataSource = objbe.GetWeeklyDatesMccDmSdm(qtr, year, "Weekly", DateTime.Now, DateTime.Now);
            grdBEDU.DataBind();

            foreach (GridViewRow row in grdBEDU.Rows)
            {
                DateTime date = Convert.ToDateTime(row.Cells[0].Text);
                DropDownList ddldates = (DropDownList)row.FindControl("ddlDailyDates");
                ddldates.DataTextField = "dtdate";
                ddldates.DataValueField = "dtdate";
                ddldates.DataSource = objbe.GetWeeklyDatesMccDmSdm(qtr, year, "Daily", date, DateTime.Now);
                ddldates.DataBind();
            }
            objbe.GetCurrentField(type, dmsdm, pu, mcc, qtr, year, out current);

            if (current == string.Empty)
            {
                tble2.Visible = false;
                tbl3.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('NO REcORDS EXIST');</script>", false);
                return;

            }

            else
                txtCurrent.Text = current;


        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            int ret = 0; ;

            string type = ddlChange.SelectedValue;

            string pu = ddlPU.SelectedValue;
            string mcc = ddlMcc.SelectedValue;
            string dmsdm = ddlDMSDM.SelectedValue;
            string qtr = ddlQtr.SelectedValue;
            string year = ddlYear.SelectedValue;

           
            tble2.Visible = true;
            tbl3.Visible = true;




            ret = objbe.UpdateMccdmsdmChange(type, dmsdm, pu, mcc, qtr, year, txtChange.Text,DateTime.Now,DateTime.Now);
            if (type == "Currency" && ret == 0)
            {
                ModalPopupExtender1.Hide();
            }

            if (ret == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Updated Successfully!!!";


            }

            tble1.Visible = false;
            tble2.Visible = false;
            tbl3.Visible = false;
            ddlChange.DataBind();
            ddlChange.SelectedIndex = ddlChange.Items.IndexOf(ddlChange.Items.FindByValue("---Select---"));


        }

        protected void ddlQtr_SelectedIndexChanged(object sender, EventArgs e)
        {
            tble2.Visible = false;
            tbl3.Visible = false;
            string qtr = ddlQtr.SelectedValue;
            string pu = ddlPU.SelectedValue;

            ddlYear.DataTextField = "txtYear";
            ddlYear.DataValueField = "txtYear";
            ddlYear.DataSource = objbe.GetQuarterYearDineshReport("Year", qtr);
            ddlYear.DataBind();

            string year = ddlYear.SelectedValue;
            ddlMcc.DataTextField = "txtMCC";
            ddlMcc.DataValueField = "txtMCC";
            ddlMcc.DataSource = objbe.GetMCC_MccDMSDMChange(pu,qtr,year);
            ddlMcc.DataBind();
            ddlMcc.Items.Insert(0, "---Select---");

            string mcc = ddlMcc.SelectedValue;

       
        }

        protected void ddlMcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";

            tble2.Visible = false;
            tbl3.Visible = false;
            string type = ddlChange.SelectedValue;
            string pu = ddlPU.SelectedValue;
            string mcc = ddlMcc.SelectedValue;
            string qtr = ddlQtr.SelectedValue;
                string year = ddlYear.SelectedValue;
            ddlDMSDM.DataTextField = "txtDMSDM";
            ddlDMSDM.DataValueField = "txtDMSDM";
           // ddlDMSDM.DataSource = objbe.GetDMSDM_MccDMSDMChange(type, pu, mcc,qtr,year);
            ddlDMSDM.DataBind();
            ddlDMSDM.Items.Insert(0, "---Select---");


          

        }

        protected void rbtnAction_SelectedIndexChanged(object sender, EventArgs e)
        {

            tble1.Visible = false;
            tble2.Visible = false;
            tbl3.Visible = false;
            lblMessage.Visible = false;
            lblInfo.Visible = false;
            ddlChange.DataBind();
            ddlChange.SelectedIndex = ddlChange.Items.IndexOf(ddlChange.Items.FindByValue("---Select---"));
            ddlFromFinYear.Text = "-Select-";
            ddlFromQuarter.Text = "-Select-";
            ddlToFinYear.Text = "-Select-";
            ddlToQuarter.Text = "-Select-";

            string typeWeekly = rbtnAction.SelectedValue;

            if (typeWeekly == "Update")
            {
                txtDate.Text = "";
                ddlTrendYear.Text = "--Select--";
                ddlTrendQuarter.Text = "--Select--";
                ddlQtrWeekly.Text = "--Select--";
                ddlYearWeekly.Text = "--Select--";
                ddlWeeklyDate.Text = "--Select--";
                ddlDelDate.DataSource = objbe.GetWeeklyDateSum("", "");
                ddlDelDate.DataBind();
                ddlUpDate.Enabled = true;
                img1.Visible = true;
                txtDate.Enabled = true;
                ddlDelDate.Enabled = false;
                ddlWeeklyDate.Enabled = false;
                ddlQtrWeekly.Enabled = false;
                ddlYearWeekly.Enabled = false;

                ddlUpDate.DataSource = objbe.GetWeeklyDates();
                ddlUpDate.DataBind();
                ddlUpDate.Items.Insert(0, "--Select--");
                lblMsg.Visible = false;
                ddlTrendQuarter.Enabled = false;
                ddlTrendYear.Enabled = false;
            }
            else if (typeWeekly == "Delete")
            {
                txtDate.Text = "";
                ddlUpDate.Text = "--Select--";
                ddlTrendYear.Text = "--Select--";
                ddlTrendQuarter.Text = "--Select--";
                ddlQtrWeekly.Text = "--Select--";
                ddlYearWeekly.Text = "--Select--";
                ddlDelDate.DataSource = objbe.GetWeeklyDateSum("", "");
                ddlDelDate.DataBind();


                ddlUpDate.Enabled = false;
                img1.Visible = false;
                txtDate.Enabled = false;
                ddlDelDate.Enabled = false;
                ddlQtrWeekly.Enabled = false;
                ddlYearWeekly.Enabled = false;
                ddlWeeklyDate.Enabled = true;

                ddlWeeklyDate.DataSource = objbe.GetWeeklyDates();
                ddlWeeklyDate.DataBind();
                ddlWeeklyDate.Items.Insert(0, "--Select--");
                lblMsg.Visible = false;
                ddlTrendQuarter.Enabled = false;
                ddlTrendYear.Enabled = false;
            }

            else if (typeWeekly == "Execute" )
            {
                txtDate.Text = "";
                ddlUpDate.Text = "--Select--";
                ddlTrendYear.Text = "--Select--";
                ddlTrendQuarter.Text = "--Select--";
                ddlQtrWeekly.Text = "--Select--";
                ddlYearWeekly.Text = "--Select--";
                ddlWeeklyDate.Text = "--Select--";
                ddlDelDate.DataSource = objbe.GetWeeklyDateSum("", "");
                ddlDelDate.DataBind();
                ddlUpDate.Enabled = false;
                img1.Visible = false;
                txtDate.Enabled = false;
                ddlDelDate.Enabled = false;
                lblMsg.Visible = false;
                ddlWeeklyDate.Enabled = false;
                ddlQtrWeekly.Enabled = false;
                ddlYearWeekly.Enabled = false;
                ddlTrendQuarter.Enabled = false;
                ddlTrendYear.Enabled = false;

            }


            else if (typeWeekly == "Trends")
            {
                txtDate.Text = "";
                ddlUpDate.Text = "--Select--";
                ddlTrendYear.Text = "--Select--";
                ddlTrendQuarter.Text = "--Select--";
                ddlQtrWeekly.Text = "--Select--";
                ddlYearWeekly.Text = "--Select--";
                ddlWeeklyDate.Text = "--Select--";
                ddlDelDate.DataSource = objbe.GetWeeklyDateSum("", "");
                ddlDelDate.DataBind();
                ddlUpDate.Enabled = false;
                img1.Visible = false;
                txtDate.Enabled = false;
                ddlDelDate.Enabled = false;
                lblMsg.Visible = false;
                ddlWeeklyDate.Enabled = false;
                ddlQtrWeekly.Enabled = false;
                ddlYearWeekly.Enabled = false;
                ddlTrendQuarter.Enabled = true;
                ddlTrendYear.Enabled = true;

            }
            else if (typeWeekly == "DeletebyQtr")
            {
                txtDate.Text = "";
                ddlUpDate.Text = "--Select--";
                ddlWeeklyDate.Text = "--Select--";
                ddlTrendYear.Text = "--Select--";
                ddlTrendQuarter.Text = "--Select--";
                ddlUpDate.Enabled = false;
                img1.Visible = false;
                txtDate.Enabled = false;
                ddlDelDate.Enabled = true;
                lblMsg.Visible = false;
                ddlWeeklyDate.Enabled = false;
                ddlQtrWeekly.Enabled = true;
                ddlYearWeekly.Enabled = true;
                ddlTrendQuarter.Enabled = false;
                ddlTrendYear.Enabled = false;


            }



        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string typeweekly = rbtnAction.SelectedValue;
         
            if (typeweekly == "Execute")
            {
                int ret = objbe.WeeklyDatesDelUpdate("", "", typeweekly, "", "");
                if (ret == 0)
                {
                    lblMsg.Text = "Executed Successfully!!";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                    lblMsg.Font.Bold = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error Occured!!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    
                }
            }


            if (typeweekly == "Trends")
            {

                //if (ddlTrendQuarter.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select the Quarter');</script>", false);

                //    return;
                //}
                //if (ddlTrendYear.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select the Year');</script>", false);

                //    return;
                //}
                int ret = objbe.WeeklyDatesDelUpdate("", "", typeweekly, ddlTrendQuarter.SelectedValue,ddlTrendYear.SelectedValue);
                if (ret == 0)
                {
                    lblMsg.Text = "Executed Successfully!!";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                    lblMsg.Font.Bold = true;
                }
                else
                {
                    lblMsg.Text = "Error Occured!!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
                }
            }

            if (typeweekly == "Delete")
            {
                string weeklydate = ddlWeeklyDate.SelectedValue;

                if (ddlWeeklyDate.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select Date to be Deleted');</script>", false);

                    return;
                }


                int ret = objbe.WeeklyDatesDelUpdate(weeklydate, "", typeweekly, "", "");
                if (ret == 0)
                {
                    lblMsg.Text = "Deleted Successfully!!";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                }



                else
                {
                    lblMsg.Text = "Error Occured!!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
                }



                ddlWeeklyDate.DataSource = objbe.GetWeeklyDates();
                ddlWeeklyDate.DataBind();
                ddlWeeklyDate.Items.Insert(0, "--Select--");
            }

            if (typeweekly == "Update")
            {
                string frmupdate = ddlUpDate.SelectedValue;
                string toupdate = txtDate.Text;

                if (ddlUpDate.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select From Date');</script>", false);

                    return;
                }

                if (txtDate.Text == null || txtDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select To date ');</script>", false);

                    return;
                }
                //IWin32Window obj = (IWin32Window)MessageBox.Show((IWin32Window)this, "Are You sure You want to Update?", "Alert", MessageBoxButtons.OKCancel);




                int ret = objbe.WeeklyDatesDelUpdate(frmupdate, toupdate, typeweekly, "", "");
                if (ret == 0)
                {

                    lblMsg.Text = "Updated Successfully!!";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Error Occured!!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
                }



                ddlUpDate.DataSource = objbe.GetWeeklyDates();
                ddlUpDate.DataBind();
                ddlUpDate.Items.Insert(0, "--Select--");
                txtDate.Text = "";
            }

            if (typeweekly == "DeletebyQtr")
            {
                string qtr = ddlQtrWeekly.SelectedValue;
                string yr = ddlYearWeekly.SelectedValue;

                //string frmupdate = ddlDelDate.SelectedValue.Substring(0, 11);


                int[] SDMMailId = ddlDelDate.GetSelectedIndices();
                List<string> lstMailId = new List<string>();
                for (int i = 0; i < ddlDelDate.Items.Count; i++)
                {
                    if (SDMMailId.Contains(i))
                        lstMailId.Add(ddlDelDate.Items[i].Value.Substring(0, 11));
                }


                if (ddlQtrWeekly.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select The Quarter');</script>", false);

                    return;
                }

                if (ddlYearWeekly.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select The Year');</script>", false);

                    return;
                }

                if (ddlDelDate.SelectedIndex < 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Select Atleast 1 Date');</script>", false);

                    return;
                }
                int ret = -1;
                foreach (string item in lstMailId)
                {

                    ret = objbe.WeeklyDatesDelUpdate(item, "", typeweekly, qtr, yr);
                }
                if (ret == 0)
                {

                    lblMsg.Text = "Deleted Successfully!!";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Error Occured!!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
                }








                ddlQtrWeekly.DataTextField = "txtQuarter";
                ddlQtrWeekly.DataValueField = "txtQuarter";
                ddlQtrWeekly.DataSource = objbe.GetQuarterYearWeekly("Quarter", "");
                ddlQtrWeekly.DataBind();
                ddlQtrWeekly.Items.Insert(0, "--Select--");
                string qtrwe = ddlQtrWeekly.SelectedValue;

                ddlYearWeekly.DataTextField = "txtYear";
                ddlYearWeekly.DataValueField = "txtYear";
                ddlYearWeekly.DataSource = objbe.GetQuarterYearWeekly("Year", qtrwe);
                ddlYearWeekly.DataBind();
                ddlYearWeekly.Items.Insert(0, "--Select--");

                string yrwe = ddlYearWeekly.SelectedValue;

                ddlDelDate.DataSource = objbe.GetWeeklyDateSum(yrwe, qtrwe);
                ddlDelDate.DataBind();

                for (int i = 0; i < ddlDelDate.Items.Count; i++)
                {
                    if (ddlDelDate.Items[i].Value != null && ddlDelDate.Items[i].Text != "")
                    {
                        ddlDelDate.Items[0].Selected = true;
                    }
                }
            }

        }

        protected void ddlQtrWeekly_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            string qtr = ddlQtrWeekly.SelectedValue;
            ddlYearWeekly.DataTextField = "txtYear";
            ddlYearWeekly.DataValueField = "txtYear";
            ddlYearWeekly.DataSource = objbe.GetQuarterYearWeekly("Year", qtr);
            ddlYearWeekly.DataBind();
            ddlYearWeekly.Items.Insert(0, "--Select--");

            string yr = ddlYearWeekly.SelectedValue;

            ddlDelDate.DataSource = objbe.GetWeeklyDateSum(yr, qtr);
            ddlDelDate.DataBind();
            for (int i = 0; i < ddlDelDate.Items.Count; i++)
            {
                if (ddlDelDate.Items[i].Value != null && ddlDelDate.Items[i].Text != "")
                {
                    ddlDelDate.Items[0].Selected = true;
                }
            }
        }

        protected void ddlYearWeekly_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qtr = ddlQtrWeekly.SelectedValue;
            string yr = ddlYearWeekly.SelectedValue;

            ddlDelDate.DataSource = objbe.GetWeeklyDateSum(yr, qtr);
            ddlDelDate.DataBind();

            for (int i = 0; i < ddlDelDate.Items.Count; i++)
            {
                if (ddlDelDate.Items[i].Value != null && ddlDelDate.Items[i].Text != "")
                {
                    ddlDelDate.Items[0].Selected = true;
                }
            }
        }


        protected void ddlFromFinYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int val = 1;
            lblInfo.Visible = false;
            lblMessage.Visible = false;
            lblMsg.Visible = false;
            rbtnAction.DataBind();
            rbtnAction.SelectedIndex = rbtnAction.Items.IndexOf(rbtnAction.Items.FindByValue("Update"));
            ddlUpDate.Enabled = true;
            txtDate.Enabled = true;
            img1.Visible = true;

            ddlTrendYear.Enabled = false;
                ddlTrendQuarter.Enabled=false;
            ddlWeeklyDate.Enabled = false;
            ddlQtrWeekly.Enabled = false;
            ddlYearWeekly.Enabled = false;
            ddlDelDate.Enabled = false;
            txtDate.Text = "";
            ddlUpDate.Text = "--Select--";
            ddlTrendQuarter.Text = "--Select--";
            ddlTrendYear.Text = "--Select--";
            ddlQtrWeekly.Text = "--Select--";
            ddlYearWeekly.Text = "--Select--";
            ddlWeeklyDate.Text = "--Select--";
            ddlDelDate.DataSource = objbe.GetWeeklyDateSum("", "");
            ddlDelDate.DataBind();

            tble1.Visible = false;
            tble2.Visible = false;
            tbl3.Visible = false;
            ddlChange.DataBind();
            ddlChange.SelectedIndex = ddlChange.Items.IndexOf(ddlChange.Items.FindByValue("---Select---"));


            string fromYear = ddlFromFinYear.SelectedItem.ToString();
            ddlFromQuarter.DataSource = objbe.GetCopyDataQuarter(fromYear, val);
            ddlFromQuarter.DataBind();
            ddlFromQuarter.Items.Insert(0, "-Select-");
            ddlToFinYear.Items.Clear();
            ddlToFinYear.DataSource = objbe.GetCopyDataFinancialYear(val);
            ddlToFinYear.DataBind();
            ddlToFinYear.Items.Insert(0, "-Select-");
            ddlToQuarter.Items.Clear();
            ddlToQuarter.Items.Insert(0, "-Select-");
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            string fromFinYear = ddlFromFinYear.SelectedItem.ToString();
            string fromQuarter = ddlFromQuarter.SelectedItem.ToString();
            string toFinYear = ddlToFinYear.SelectedItem.ToString();
            string toQuarter = ddlToQuarter.SelectedItem.ToString();

            int count = objbe.GetCopyData(fromQuarter, fromFinYear, toQuarter, toFinYear);
            if (count > 0)
            {
                //lblInfo.Text = "Data has been copied successfully for: " + count + "rows.";
                lblInfo.Text = "Data has been copied successfully for: " + count + "rows.";
                lblInfo.ForeColor = System.Drawing.Color.Green;
                lblInfo.Visible = true;
                lblInfo.Font.Bold = true;
            }
            else
            {
                lblInfo.Text = "Data already exists.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Visible = true;
                lblInfo.Font.Bold = true;
            }
        }

        protected void ddlToFinYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfo.Visible = false;
            string fromYear = ddlFromFinYear.SelectedItem.ToString();
            string toYear = ddlToFinYear.SelectedItem.ToString();
            string fromQuarter = ddlFromQuarter.SelectedItem.ToString().Trim();
            //string quarter = null;
            //string fromYearCheck = fromYear.Remove(4, 3);
            //DateTime todaysYear = DateTime.Now;
            // int year = todaysYear.Year;


            //DateTime todaysMonth = DateTime.Now;
            //int month = todaysMonth.Month;

            //if (month == 4 || month == 5 || month == 6)
            //{
            //    quarter = "Q1";
            //}
            //else if (month == 7 || month == 8 || month == 9)
            //{
            //    quarter = "Q2";
            //}
            //else if (month == 10 || month == 11 || month == 12)
            //{
            //    quarter = "Q3";
            //}
            //else
            //{
            //    quarter = "Q4";
            //   year = year - 1;
            //}
            if (fromYear == toYear)
            {
                if (fromQuarter.Equals("Q1"))
                {
                    ddlToQuarter.Items.Clear();
                    ddlToQuarter.Items.Insert(0, "-Select-");
                    ddlToQuarter.Items.Insert(1, "Q2");
                    ddlToQuarter.Items.Insert(2, "Q3");
                    ddlToQuarter.Items.Insert(3, "Q4");
                    //ddlToQuarter.Items.Insert(4, "Q4");
                }
                else if (fromQuarter.Equals("Q2"))
                {
                    ddlToQuarter.Items.Clear();
                    ddlToQuarter.Items.Insert(0, "-Select-");
                    ddlToQuarter.Items.Insert(1, "Q3");
                    ddlToQuarter.Items.Insert(2, "Q4");
                    //ddlToQuarter.Items.Insert(3, "Q4");
                }
                else if (fromQuarter.Equals("Q3"))
                {
                    ddlToQuarter.Items.Clear();
                    ddlToQuarter.Items.Insert(0, "-Select-");
                    ddlToQuarter.Items.Insert(1, "Q4");
                    //ddlToQuarter.Items.Insert(2, "Q4");
                }
                else
                {
                    ddlToQuarter.Items.Clear();
                    ddlToQuarter.Items.Insert(0, "-Select-");
                    ddlToQuarter.Items.Insert(1, "Q4");
                }
            }
            else
            {
                int val = 1;
                ddlToQuarter.Items.Clear();
                ddlToQuarter.DataSource = objbe.GetCopyDataQuarter(toYear, val);
                ddlToQuarter.DataBind();
                ddlToQuarter.Items.Insert(0, "-Select-");
            }
        }

        protected void ddlFromQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fromYear = ddlFromFinYear.SelectedItem.ToString();
            string fromQuarter = ddlFromQuarter.SelectedItem.ToString().Trim();
            int no = 0;
            if (fromQuarter == "Q4")
            {
                no = 0;
                ddlToFinYear.Items.Clear();
                ddlToQuarter.Items.Clear();
                ddlToFinYear.DataSource = objbe.GetCopyDataFutureFinancialYear(fromYear, no);
                ddlToFinYear.DataBind();
                ddlToFinYear.Items.Insert(0, "-Select-");
                ddlToQuarter.Items.Insert(0, "-Select-");
            }
            else
            {
                no = 1;
                ddlToFinYear.Items.Clear();
                ddlToQuarter.Items.Clear();

                ddlToFinYear.DataSource = objbe.GetCopyDataFutureFinancialYear(fromYear, no);
                ddlToFinYear.DataBind();
                ddlToFinYear.Items.Insert(0, "-Select-");
                ddlToQuarter.Items.Insert(0, "-Select-");
            }
        }

        protected void btnhidden_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ddlUpDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            tble1.Visible = false;
            tble2.Visible = false;
            tbl3.Visible = false;
            lblMsg.Visible = false;
            ddlChange.DataBind();
            ddlChange.SelectedIndex = ddlChange.Items.IndexOf(ddlChange.Items.FindByValue("---Select---"));
            ddlFromFinYear.Text = "-Select-";
            ddlFromQuarter.Text = "-Select-";
            ddlToFinYear.Text = "-Select-";
            ddlToQuarter.Text = "-Select-";
        }

        protected void btnHiddenCopy_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnhiddenUpdate_Click(object sender, ImageClickEventArgs e)
        {

        }

        //protected void btnChange1_Click(object sender, EventArgs e)
        //{
        //    string qtr = ddlQtr.SelectedValue;
        //    string year = ddlYear.SelectedValue;
        //    grdBEDU.DataSource = objbe.GetWeeklyDatesMccDmSdm(qtr, year, "Weekly", DateTime.Now, DateTime.Now); ;
        //    //objbe.GetWeeklyDatesMccDmSdm("Q4", "2012-13", "Weekly", DateTime.Now, DateTime.Now);
        //    grdBEDU.DataBind();

        //    foreach (GridViewRow row in grdBEDU.Rows)
        //    {
        //        DateTime date = Convert.ToDateTime(row.Cells[0].Text);
        //        DropDownList ddldates = (DropDownList)row.FindControl("ddlDailyDates");
        //        ddldates.DataTextField = "dtdate";
        //        ddldates.DataValueField = "dtdate";
        //        ddldates.DataSource = objbe.GetWeeklyDatesMccDmSdm(qtr, year, "Daily", date, DateTime.Now);
        //        ddldates.DataBind();
        //    }

        //    //var flag = Session["SaveFlag"];
        //    //if (flag=="true")
        //    //{
        //    //    btnChange_Click(sender, e);
        //    //}

        //    //else
        //    //{
        //    //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Update the Misplaced Dates');</script>", false);

        //    //    return;
        //    //}
        //}
        protected void btnSave_Click(object sender, EventArgs e)
        {

            //string cur = 
            //string pu = Request.QueryString["Pu"];
            //string mcc = Request.QueryString["Mcc"];
            //string change = Request.QueryString["Change"];
            //string qtr = Request.QueryString["Qtr"];
            //string year = Request.QueryString["Year"];
            ModalPopupExtender1.Show();
            string type = ddlChange.SelectedValue;
            string pu = ddlPU.SelectedValue;
            string mcc = ddlMcc.SelectedValue;
            string dmsdm = ddlDMSDM.SelectedValue;
            string qtr = ddlQtr.SelectedValue;
            string year = ddlYear.SelectedValue;

            if (type == "Currency")
            {

                foreach (GridViewRow row in grdBEDU.Rows)
                {
                    DateTime date = Convert.ToDateTime(row.Cells[0].Text);
                    DropDownList ddldates = (DropDownList)row.FindControl("ddlDailyDates");

                    DateTime dailydate = Convert.ToDateTime(ddldates.SelectedValue);

                    objbe.UpdateMccdmsdmChange("UpdateCurrency", dmsdm, pu, mcc, qtr, year, txtChange.Text, date, dailydate);


                }
                btnChange2_Click1(sender, e);
            }

            Session["SaveFlag"] = "true";
            // Response.Write(@" <script type=""text/javascript""> alert('Updated successfully !'); window.opener.document.getElementById('MainContent_btnChange').click(); window.close();   </script>");
        }

        protected void btnChange2_Click1(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            //btnSave_Click(sender, e);

            int ret = 0; ;
            string type = ddlChange.SelectedValue;
            string pu = ddlPU.SelectedValue;
            string mcc = ddlMcc.SelectedValue;
            string dmsdm = ddlDMSDM.SelectedValue;
            string qtr = ddlQtr.SelectedValue;
            string year = ddlYear.SelectedValue;

          
            tble2.Visible = true;
            tbl3.Visible = true;




            ret = objbe.UpdateMccdmsdmChange(type, dmsdm, pu, mcc, qtr, year, txtChange.Text,DateTime.Now,DateTime.Now);
            if (type == "Currency" && ret == 0)
            {
                ModalPopupExtender1.Hide();
            }

            if (ret == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Updated Successfully!!!";


            }

            tble1.Visible = false;
            tble2.Visible = false;
            tbl3.Visible = false;
            ddlChange.DataBind();
            ddlChange.SelectedIndex = ddlChange.Items.IndexOf(ddlChange.Items.FindByValue("---Select---"));
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qtr = ddlQtr.SelectedValue;
            string year = ddlYear.SelectedValue;
            string pu = ddlPU.SelectedValue;
            ddlMcc.DataTextField = "txtMCC";
            ddlMcc.DataValueField = "txtMCC";
            ddlMcc.DataSource = objbe.GetMCC_MccDMSDMChange(pu, qtr, year);
            ddlMcc.DataBind();
            ddlMcc.Items.Insert(0, "---Select---");

        }

        protected void ddlTrendQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            string trendqtr = ddlTrendQuarter.SelectedValue.Trim();

            ddlTrendYear.DataTextField = "txtyear";
            ddlTrendYear.DataValueField = "txtyear";
            ddlTrendYear.DataSource = objbe.GetBEReportQtrYear("Year", trendqtr);
            ddlTrendYear.DataBind();
            ddlTrendYear.Items.Insert(0, "--Select--");

        }

        protected void ddlWeeklyDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
        }

    }
