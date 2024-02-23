using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.Data;
using System.IO;
using BEData;


    public partial class GuidanceAndActuals : BasePage
    {
        BEDL objbe = new BEDL();
        public string fileName = "BEData.GuidanceAndActuals";
        Logger logger = new Logger();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            lblMsg.Visible = false;
            

            if (!Page.IsPostBack)
            {
                //onload
                string isValidEntry = Session["Login"] + "";
                if (!isValidEntry.Equals("1"))
                    Response.Redirect("UnAuthorised.aspx");

                ddlYear.Items.Clear();
               
                ddlYear.DataSource = objbe.FetchYearForMonthlyConversion();
                
                ddlYear.DataBind();
                ddlYear.Items.Insert(0, "--Select--");
                ddlMonth.Items.Clear();
                ddlMonth.Items.Add("--Select--");
                //ddlMonth.DataSource = objbe.FetchMonthYearForMonthlyConversion("Month");
                //ddlMonth.DataBind();
                //string month = ddlMonth.SelectedValue;
                //string year = ddlYear.SelectedValue;
                //grdCurrConv.DataSource = objbe.FetchMonthlyCurrencyConversion(month,year);
                //grdCurrConv.DataBind();
            }
            else
            {

            }
        }

       

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string month = ddlMonth.SelectedValue;
            string year = ddlYear.SelectedValue;

            grdCurrConv.DataSource = objbe.FetchMonthlyCurrencyConversion(month, year);
            grdCurrConv.DataBind();
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<MonthlyCurrencyConversion> lstUPdateItems = new List<MonthlyCurrencyConversion>();

                DataTable dt = new DataTable();

                foreach (GridViewRow row in grdCurrConv.Rows)
                {
                    if ((row.Cells[0].FindControl("chkRow") as CheckBox).Checked)
                    {

                        string nativeCurr = row.Cells[1].Text.Trim();
                        TextBox tbguid = (TextBox)row.FindControl("txtGuidanceConvRate");
                        double guidrate = double.Parse(tbguid.Text);
                        TextBox tbBenchmark = (TextBox)row.FindControl("txtBenchMarkRate");
                        double Benchrate = double.Parse(tbBenchmark.Text);

                        TextBox tbQtyAvgFA = (TextBox)row.FindControl("txtQtyAvgFARate");
                        double QtyAvgRate = double.Parse(tbQtyAvgFA.Text);
                        //TextBox txtActualConvRateMonth2 = (TextBox)row.FindControl("txtActualConvRateMonth2");
                        //double actualRateMonth2 = double.Parse(txtActualConvRateMonth2.Text);
                        //TextBox txtActualConvRateMonth3 = (TextBox)row.FindControl("txtActualConvRateMonth3");
                        //double actualRateMonth3 = double.Parse(txtActualConvRateMonth3.Text);

                        TextBox txtActualConvRateMonth1 = (TextBox)row.FindControl("txtActualConvRateMonth1");                        
                        double actualRateMonth1 = txtActualConvRateMonth1.Text.Length == 0 ? 0 : double.Parse(txtActualConvRateMonth1.Text);

                        TextBox txtActualConvRateMonth2 = (TextBox)row.FindControl("txtActualConvRateMonth2");
                        double actualRateMonth2 = txtActualConvRateMonth2.Text.Length == 0 ? 0 : double.Parse(txtActualConvRateMonth2.Text);
                        
                        TextBox txtActualConvRateMonth3 = (TextBox)row.FindControl("txtActualConvRateMonth3");
                        double actualRateMonth3 = txtActualConvRateMonth3.Text.Length == 0 ? 0 : double.Parse(txtActualConvRateMonth3.Text);
                        

                        string month = (row.FindControl("hdnfldMonth") as HiddenField ).Value;
                        string year = (row.FindControl("hdnfldYear") as HiddenField).Value;
                        
                        lstUPdateItems.Add(new MonthlyCurrencyConversion()
                        {
                            nativeCurrency=nativeCurr,
                            guidanceRate=guidrate,
                            BenchMarkrate = Benchrate,
                            QtyAvgFARate=QtyAvgRate,
                            actualRateMonth1=actualRateMonth1,
                            actualRateMonth2 = actualRateMonth2,
                            actualRateMonth3 = actualRateMonth3,
                            month=month,
                            year=year
                        });
                    }
                }


                string role = Session["Role"] + "";

                if (lstUPdateItems.Count > 0)
                {
                    
                    objbe.UpdateMonthlyCurrencyConversion(lstUPdateItems);


                    btnSearch_Click(null, null);

                    lblMsg.Text = "&nbsp &nbsp &nbsp &nbsp" + "Data saved successfully !";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;

                }
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }
        protected void btnSavepopup_click(object sender, EventArgs e)
        {
            string month = ddlpopUpMonth.SelectedValue;
            string year = ddlPopUpYear.SelectedValue;
            int ret = objbe.AddNewMonthlyConvRate(month, year);
            if (ret == -1)
            {
                lblMsg.Text = "&nbsp &nbsp &nbsp &nbsp" + "Data is present for the month !";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            else if (ret == 0)
            {
                lblMsg.Text = "&nbsp &nbsp &nbsp &nbsp" + "Data saved successfully !";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;

                ddlMonth.DataSource = objbe.FetchMonthYearForMonthlyConversion("Month");
                ddlMonth.DataBind();
                ddlYear.DataSource = objbe.FetchMonthYearForMonthlyConversion("Year");
                ddlYear.DataBind();
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(month));
                ddlYear.Items.FindByValue(year.ToString()).Selected = true;
                grdCurrConv.DataSource = objbe.FetchMonthlyCurrencyConversion(month, year);
                grdCurrConv.DataBind();
            }

        }

        protected void btnSavepopupAddNew_click(object sender, EventArgs e)
        {
            string currentQtr = DateUtility.GetQuarter("current");
            int tempyearnext = 0;
            string curyr = string.Empty;
            tempyearnext = Convert.ToInt32(currentQtr.Remove(0, 3)) + 2000 - 1;
            curyr = string.Format("{0}-{1}", tempyearnext, (tempyearnext - 2000 + 1));                       
            string Currency = txtCurrency.Text;
            string month = currentQtr.Remove(2, 3);
            int ret = objbe.AddNewMonthlyConvRateNew(month, curyr, Currency);
            if (ret == -1)
            {
                lblMsg.Text = "&nbsp &nbsp &nbsp &nbsp" + "Data is present for the month !";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            else if (ret == 0)
            {
                lblMsg.Text = "&nbsp &nbsp &nbsp &nbsp" + "Data saved successfully !";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;

                ddlMonth.DataSource = objbe.FetchMonthYearForMonthlyConversion("Month");
                ddlMonth.DataBind();
                ddlYear.DataSource = objbe.FetchMonthYearForMonthlyConversion("Year");
                ddlYear.DataBind();
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(month));
                ddlYear.Items.FindByValue(curyr.ToString()).Selected = true;
                grdCurrConv.DataSource = objbe.FetchMonthlyCurrencyConversion(month, curyr);
                grdCurrConv.DataBind();
            }

        }

        protected void grdCurrConvt_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            int year = DateTime.Today.Year;
            string month1 = string.Empty;
            string month2 = string.Empty;
            string month3 = string.Empty;


            DateTime todaydate = DateTime.Now;
            //int year = todaydate.Year - 2000;
            int nextyear = year + 1;
            if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
            {
                month1 = "Jan";
                month2 = "Feb";
                month3 = "Mar";
            }
            else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
            {
                month1 = "Apr";
                month2 = "May";
                month3 = "Jun";
            }
            else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
            {
                month1 = "Jul";
                month2 = "Aug";
                month3 = "Sep";
            }
            else
            {
                month1 = "Oct";
                month2 = "Nov";
                month3 = "Dec";
            }

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[3].Text = month1;
                    e.Row.Cells[4].Text = month2;
                    e.Row.Cells[5].Text = month3;
                }


                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {



                }
            }
            catch (Exception ex)
            {

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }




        protected void btnAddNew_Click1(object sender, EventArgs e)
        {
            //ddlPopUpYear.DataSource = objbe.GetPopUpYear();
            //ddlPopUpYear.DataTextField = "year";
            //ddlPopUpYear.DataValueField = "year";
            //ddlPopUpYear.DataBind();

            string currentQtr = DateUtility.GetQuarter("current");


            int tempyearnext = 0;
            string curyr = string.Empty;
            tempyearnext = Convert.ToInt32(currentQtr.Remove(0, 3)) + 2000 - 1;
            curyr = string.Format("{0}-{1}", tempyearnext, (tempyearnext - 2000 + 1));
            ddlPopUpYear.Items.Insert(0, curyr);
      
            ddlpopUpMonth.DataSource = objbe.FetchMonthYearForMonthlyConversion("Month");
            ddlpopUpMonth.DataTextField = "txtMonth";
            ddlpopUpMonth.DataValueField = "txtMonth";
            ddlpopUpMonth.DataBind();

            int year = DateTime.Today.Year;
            
            int mon = DateTime.Today.Month;
            mon = mon - 1;


            //if (mon == 12)
            //{
            //    year = year - 1;
                
            //    ddlPopUpYear.Items.FindByValue(year.ToString()).Selected = true;
            //}
            //else
            //{

            //    ddlPopUpYear.Items.FindByValue(year.ToString()).Selected = true;
            //}

            //string month = string.Empty;
            //switch (mon)
            //{
            //    case 1: month = "January";
            //        break;
            //    case 2: month = "Februray";
            //        break;
            //    case 3: month = "March";
            //        break;
            //    case 4: month = "April";
            //        break;
            //    case 5: month = "May";
            //        break;
            //    case 6: month = "June";
            //        break;
            //    case 7: month = "July";
            //        break;
            //    case 8: month = "August";
            //        break;
            //    case 9: month = "September";
            //        break;
            //    case 10: month = "October";
            //        break;
            //    case 11: month = "November";
            //        break;
            //    case 12: month = "December";
            //        break;
            //}

            //ddlpopUpMonth.SelectedIndex = ddlpopUpMonth.Items.IndexOf(ddlpopUpMonth.Items.FindByText(month));

        }

        protected void grdCurrConv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //int tempyear = Convert.ToInt32(ddlMonth.Text.Remove(0, 3)) + 2000 - 1;
            //string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

            string _CurrentQ = string.Empty;
            _CurrentQ = ddlMonth.Text.ToString();

            int year = DateTime.Today.Year;
            int prevyear = DateTime.Today.Year-1;
            string month1 = string.Empty;
            string month2 = string.Empty;
            string month3 = string.Empty;
            string guidance = string.Empty;
            string benchMark = string.Empty;
            string QtyAvgFA = string.Empty;
            string curyear = string.Empty;
            string preyear = string.Empty;
            curyear = Convert.ToString(year);

            preyear = Convert.ToString(prevyear);


            DateTime todaydate = DateTime.Now;
            //int year = todaydate.Year - 2000;
            int nextyear = year + 1;
            int nextfinyr = year + 2;
            string nextfin=string.Empty;
            string nextfinyear = string.Empty;
            string nexyr = string.Empty;
            nextfin=Convert.ToString(nextyear);
            nextfinyear = Convert.ToString(nextfinyr);
            nexyr = Convert.ToString(nextyear);

            //if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
            //{
            //    month1 = "Jan " + year;
            //    month2 = "Feb " + year;
            //    month3 = "Mar " + year;
            //}
            //else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
            //{
            //    month1 = "Apr " + year;
            //    month2 = "May " + year;
            //    month3 = "Jun " + year;
            //}
            //else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
            //{
            //    month1 = "Jul " + year;
            //    month2 = "Aug " + year;
            //    month3 = "Sep " + year;
            //}
            //else
            //{
            //    month1 = "Oct " + year;
            //    month2 = "Nov " + year;
            //    month3 = "Dec " + year;
            //}

            if (_CurrentQ=="Q4")
            {
                guidance = "Q4'" + ddlYear.Text.Substring(5, 2) + " Guidance";
                benchMark = "Q4'" + ddlYear.Text.Substring(5, 2) + " BenchMark";
                QtyAvgFA = "Q4'" + ddlYear.Text.Substring(5, 2) + " QtyAvgFA";
                month1 = "Jan-" + ddlYear.Text.Substring(5,2) + " Actuals";
                month2 = "Feb-" + ddlYear.Text.Substring(5, 2) + " Actuals";
                month3 = "Mar- " + ddlYear.Text.Substring(5, 2) + " Actuals";
            }
            else if (_CurrentQ == "Q1")
            {
                guidance = "Q1'" + ddlYear.Text.Substring(5, 2) + " Guidance";
                benchMark = "Q1'" + ddlYear.Text.Substring(5, 2) + " BenchMark";
                QtyAvgFA = "Q1'" + ddlYear.Text.Substring(5, 2) + " QtyAvgFA";
                month1 = "Apr-" + ddlYear.Text.Substring(2, 2) + " Actuals";
                month2 = "May-" + ddlYear.Text.Substring(2, 2) + " Actuals";
                month3 = "Jun-" + ddlYear.Text.Substring(2, 2) + " Actuals";
            }
            else if (_CurrentQ == "Q2")
            {
                guidance = "Q2'" + ddlYear.Text.Substring(5, 2) + " Guidance";
                benchMark = "Q2'" + ddlYear.Text.Substring(5, 2) + " BenchMark";
                QtyAvgFA = "Q2'" + ddlYear.Text.Substring(5, 2) + " QtyAvgFA";
                month1 = "Jul-" + ddlYear.Text.Substring(2, 2) + " Actuals";
                month2 = "Aug-" + ddlYear.Text.Substring(2, 2) + " Actuals";
                month3 = "Sep-" + ddlYear.Text.Substring(2, 2) + " Actuals";
            }
            else
            {
                guidance = "Q3'" + ddlYear.Text.Substring(5, 2) + " Guidance";
                benchMark = "Q3'" + ddlYear.Text.Substring(5, 2) + " BenchMark";
                QtyAvgFA = "Q3'" + ddlYear.Text.Substring(5, 2) + " QtyAvgFA";
                month1 = "Oct-" + ddlYear.Text.Substring(2, 2) + " Actuals";
                month2 = "Nov-" + ddlYear.Text.Substring(2, 2) + " Actuals";
                month3 = "Dec-" + ddlYear.Text.Substring(2, 2) + " Actuals";
            }

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[2].Text = guidance;
                    e.Row.Cells[3].Text = benchMark;
                    e.Row.Cells[4].Text = QtyAvgFA;
                    e.Row.Cells[5].Text = month1;
                    e.Row.Cells[6].Text = month2;
                    e.Row.Cells[7].Text = month3;
                }


                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {



                }
            }
            catch (Exception ex)
            {

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }

        }

        protected void btnDelete_Click1(object sender, EventArgs e)
        {
            try
            {
                List<MonthlyCurrencyConversion> lstUPdateItems = new List<MonthlyCurrencyConversion>();

                DataTable dt = new DataTable();

                foreach (GridViewRow row in grdCurrConv.Rows)
                {
                    if ((row.Cells[0].FindControl("chkRow") as CheckBox).Checked)
                    {

                        string nativeCurr = row.Cells[1].Text.Trim();                       
                        string month = (row.FindControl("hdnfldMonth") as HiddenField).Value;
                        string year = (row.FindControl("hdnfldYear") as HiddenField).Value;

                        lstUPdateItems.Add(new MonthlyCurrencyConversion()
                        {
                            nativeCurrency = nativeCurr,                            
                            month = month,
                            year = year
                        });
                    }
                }
                string role = Session["Role"] + "";
                if (lstUPdateItems.Count > 0)
                {
                    objbe.DeleteMonthlyCurrencyConversion(lstUPdateItems);
                    btnSearch_Click(null, null);
                    lblMsg.Text = "&nbsp &nbsp &nbsp &nbsp" + "Data has been deleted successfully !";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }

        protected void btnAddNewCurrency_Click1(object sender, EventArgs e)
        {
            string currentQtr = DateUtility.GetQuarter("current");
            int tempyearnext = 0;
            string curyr = string.Empty;
            tempyearnext = Convert.ToInt32(currentQtr.Remove(0, 3)) + 2000 - 1;
            curyr = string.Format("{0}-{1}", tempyearnext, (tempyearnext - 2000 + 1));            
            lblYear.Text = "Year:";
            lblYr.Text = curyr;
            
            string month = currentQtr.Remove(2, 3);
            lblQuarter.Text = "Quarter:";
            lblQtr.Text = month;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if (ddlYear.SelectedItem.Text == "--Select--")
            {
                ddlMonth.Items.Clear();
                ddlMonth.Items.Add("--Select--");
            }
            else
            {
                ddlMonth.DataSource = objbe.FetchMonthForMonthlyConversion(ddlYear.SelectedItem.Text);
                ddlMonth.DataBind();
            }
        }
    }
