using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BECodeProd
{
    public partial class ExhangeRateUpdate : BasePage
    {
        private BEDL service = new BEDL();
        public DateTime dateTime = DateTime.Today;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

           
        }

        protected void ddlRates_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
            int currentYear = dateTime.Year; //DateTime.Now.Year;


            currentYear = currentYear - 2000;
            string _CurrentQ = string.Empty;
            //string _NextQ = string.Empty;
            //  ddlQuarter.SelectedIndex = 1;
           // _CurrentQ = Session["currqtr"] + "";
            //string currentQuarter = Session["quarter"] + "";
            //dm--dmmailid column is removed
            string currentQuarter = "Q2";


            // Month1 / Month2 / Month3
            string _month1 = string.Empty;
            string _month2 = string.Empty;
            string _month3 = string.Empty;
            if (currentQuarter == "Q4")
            {
                _month1 = "Jan";
                _month2 = "Feb";
                _month3 = "Mar";
            }
            else if (currentQuarter == "Q1")
            {
                _month1 = "Apr";
                _month2 = "May";
                _month3 = "Jun";
            }
            else if (currentQuarter == "Q2")
            {
                _month1 = "Jul";
                _month2 = "Aug";
                _month3 = "Sep";
            }
            else
            {
                _month1 = "Oct";
                _month2 = "Nov";
                _month3 = "Dec";
            }
            if (ddlRates.SelectedValue == "Weekly")
            {
                ckMonths.Visible = true;
                btUpdate.Visible = true;
                ckMonths.Items.Clear();
                ckMonths.Items.Add(_month1);
                ckMonths.Items.Add(_month2);
                ckMonths.Items.Add(_month3);
            }

            string cmd = "SELECT distinct [MONTH] as Month  FROM [BEPortalConfig] where [quarter]='" + currentQuarter + "' and (Month is Not Null or Month != 'NA')";

            DataSet ds = service.GetDataSet(cmd);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < ckMonths.Items.Count; i++)
                {

                    ckMonths.Items[i].Selected = true;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Month"].ToString() != "NA")
                    {
                        ckMonths.Items.FindByValue(dt.Rows[i]["Month"].ToString()).Selected = false;
                    }

                }

            }
            
        }

       

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < ckMonths.Items.Count; i++)
            {
                if (ckMonths.Items[i].Selected == true)
                {
                    string display = "Do you want to edit?";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true) ;
                }

            }
                        
        }

    }
}