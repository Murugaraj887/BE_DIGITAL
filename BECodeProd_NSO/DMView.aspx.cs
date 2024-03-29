﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.OleDb;
//using Office = Microsoft.Office.Core;

using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Collections.Specialized;
using BEData;

using System.Web.UI.HtmlControls;
using System.Net;
using System.Globalization;
using Microsoft.Office.Core;
using VBIDE = Microsoft.Vbe.Interop;
using ExcelFordownload = Microsoft.Office.Interop.Excel;

    public partial class DMView : BasePage
    {

        
        FindQuarter fq = new FindQuarter();
        private BEDL service = new BEDL();
        public DateTime dateTime = DateTime.Today;
        public string fileName = "BEData.dmView";
        Logger logger = new Logger();
        public string yearForddl = string.Empty;
        public static int indexSelected;
        public int iCountRev = 0;
        public int iCountVol = 0;
        BEMonthlyFreeze be = new BEMonthlyFreeze();
        DataTable dtExcelData = new DataTable();
        DataTable dtExcel2Data = new DataTable();
        DataTable dtExcel3Data = new DataTable();
        static int Day;


        string PhysicalPath_Macro = "";
        string PhysicalPath_DownloadFiles = "";
        string PhysicalPath_Template = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            PhysicalPath_DownloadFiles = Server.MapPath("ExcelOperations\\DownloadFiles");
            PhysicalPath_Template = Server.MapPath("ExcelOperations\\Template");
            PhysicalPath_Macro = Server.MapPath("ExcelOperations\\Macro");


            string user = HttpContext.Current.User.Identity.Name;
            string[] userids = user.Split('\\');
            if (userids.Length == 2)
            {
                user = userids[1];
            }

            string userid = Session["userid"].ToString();
            string role = service.GetUserRole(userid);
            string LoggedRole = service.GetUserRole(userid);
            //return;
            //  TextBox1.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");

            // try
            {
                //Menu Menumain = (Menu)Master.FindControl("Menu_MainOptions");

                //Menumain.Items[0].Selected = true;
                if (!Page.IsPostBack)
                {
                    Page.Form.Attributes.Add("enctype", "multipart/form-data");
                    Session["RadioButtonSelected"] = 0;
                    string isValidEntry = Session["Login"] + "";
                    if (!isValidEntry.Equals("1"))
                        Response.Redirect("UnAuthorised.aspx");

                    string machineUser = string.Empty;
                    string[] machineUsers = HttpContext.Current.User.Identity.Name.Split('\\');
                    if (machineUsers.Length == 2)
                        machineUser = machineUsers[1];


                    if (machineUsers.Length == 2)
                        machineUser = machineUsers[1];

                    //TextBox1.Visible = false;

                    //TextBox1.Text = dateTime.ToShortDateString();

                     Day = service.FreezingPreviousMonthBE();

                     hdFreeze.Value = Day.ToString();
                  
                    LoadComboBox(userid);

                    if (role != "Admin" || LoggedRole != "Admin")
                    {
                        //btnSearch_Click(null, null);
                        string script = "$(document).ready(function () { $('[id*=btnSearch]').click(); });";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "load", script, true);
                    }
                    else
                    {
                        int tempyear = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
                        string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));
                        Session["Year"] = year;

                        string currqtr = ddlQuarter.SelectedValue.ToString();
                        Session["currqtr"] = currqtr;

                        string quarter = ddlQuarter.Text.Remove(2);
                        Session["quarter"] = quarter;
                    }
                    Session["A"] = Session["DM"];
                    //string customerCode = ddlCustomerCodePopup.Text.Trim();

                    // new july 18
                    //if (!service.CanAbleTOAddNewCode(userid, customerCode))
                    //{
                    //    lblpopupInfo.Text = "You dont have access to this client code. pls contact Anchor.";

                    //}
                }
                //else
                //{
                //    btnSave_DM.Enabled = true;

                //}

            }


            if (Session["UserID"].ToString().ToLower() == Session["LoggedInUserID"].ToString().ToLower())
            {
                btnSave2.Enabled = true;
                btnAddMasterCustomer.Enabled = true;
                btnZeroBE.Enabled = true;
                //bulk.Visible = true;
            }
            else
            {
                btnSave2.Enabled = false;
                btnAddMasterCustomer.Enabled = false;
                btnZeroBE.Enabled = false;
                //bulk.Visible = false;
            }

            if (role.ToLower().Trim() == "admin")
            {
                btnSave2.Enabled = true;
                btnAddMasterCustomer.Enabled = true;
                btnZeroBE.Enabled = true;
                //bulk.Visible = true;
            }



            if (hdrefress.Value == "1")
            {
                btnSearch_Click(null, null);
              
                hdrefress.Value = "0";
            }
            //catch (Exception ex)
            //{

            //    if ((ex.Message + "").Contains("Thread was being aborted."))
            //        logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //    else
            //    {
            //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //        throw ex;
            //    }
            //}
        }

        System.Collections.ArrayList lst = new System.Collections.ArrayList();
        protected void gvUserInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label ddl = (Label)e.Row.FindControl("lblMCC");
                HiddenField hdMcc = (HiddenField)e.Row.FindControl("hdnfmcc");
                Label lblNativeCurrency = (Label)e.Row.FindControl("lblNativeCurrency");
                HyperLink lnkbtnDMTotal = (HyperLink)e.Row.FindControl("lnkbtnDMTotal");
                HyperLink lnkbtnDMVolTotal = (HyperLink)e.Row.FindControl("lnkbtnDMVolTotal");
                //string[] MCC = hdMcc.Value.Split('-');
                string[] MCC = ddl.Text.Split('-');

                if (MCC.Length > 2)
                {
                    if (lst.Contains(MCC[2].ToString().ToLower().TrimEnd().TrimStart() + "-" + MCC[0].ToString().ToLower().TrimEnd().TrimStart() + "-" + lblNativeCurrency.Text.ToLower().TrimEnd().TrimStart()))
                    {
                        lnkbtnDMTotal.Text = "";
                        lnkbtnDMVolTotal.Text = "";

                    }
                    else
                    {
                        e.Row.Cells[12].BackColor = Color.Gray;
                        e.Row.Cells[24].BackColor = Color.Gray;
                        lnkbtnDMTotal.ForeColor = Color.White;
                        lnkbtnDMVolTotal.ForeColor = Color.White;
                        lst.Add(MCC[2].ToString().ToLower().TrimEnd().TrimStart() + "-" + MCC[0].ToString().ToLower().TrimEnd().TrimStart() + "-" + lblNativeCurrency.Text.ToLower().TrimEnd().TrimStart());
                    }
                }

                else
                {
                    if (lst.Contains(MCC[1].ToString().ToLower().TrimEnd().TrimStart() + "-" + MCC[0].ToString().ToLower().TrimEnd().TrimStart() + "-" + lblNativeCurrency.Text.ToLower().TrimEnd().TrimStart()))
                    {
                        lnkbtnDMTotal.Text = "";
                        lnkbtnDMVolTotal.Text = "";

                    }
                    else
                    {
                        e.Row.Cells[12].BackColor = Color.Gray;
                        e.Row.Cells[24].BackColor = Color.Gray;
                        lnkbtnDMTotal.ForeColor = Color.White;
                        lnkbtnDMVolTotal.ForeColor = Color.White;
                        lst.Add(MCC[1].ToString().ToLower().TrimEnd().TrimStart() + "-" + MCC[0].ToString().ToLower().TrimEnd().TrimStart() + "-" + lblNativeCurrency.Text.ToLower().TrimEnd().TrimStart());
                    }
                }

                System.Text.StringBuilder text = new System.Text.StringBuilder();
                string mcc = ddl.Text;

                string[] array = mcc.Split('_');

                foreach (string a in array)
                {
                    text.Append(a + " ");
                }

                ddl.Text = text.ToString();
               
                ddl.Text = ddl.Text.Trim(' ');

                //HyperLink link = (HyperLink)e.Row.FindControl("lnkbtnDMCompetencyVolTotal");

                //if (link.Text == "")
                //{
                //    link.Text = "0.0";
                //}

                TextBox lblTotVol = (TextBox)e.Row.FindControl("lblTotVol");
                //HiddenField hdn = (HiddenField)e.Row.FindControl("hdnd");
                //if (Convert.ToDecimal(lblTotVol.Text) < Convert.ToDecimal(link.Text))
                //{
                //    hdn.Value = "1";
                //    //link.ForeColor = Color.Red;
                //}
                //else
                //{
                //    hdn.Value = "0";
                //}


                TextBox month1 = (TextBox)e.Row.FindControl("txtDMMonth1");
                TextBox month2 = (TextBox)e.Row.FindControl("txtDMMonth2");
                TextBox month3 = (TextBox)e.Row.FindControl("txtDMMonth3");
                TextBox VolOnMonth1 = (TextBox)e.Row.FindControl("txtVolOnMonth1");
                TextBox VolOffMonth1 = (TextBox)e.Row.FindControl("txtVolOffMonth1");
                TextBox VolOnMonth2 = (TextBox)e.Row.FindControl("txtVolOnMonth2");
                TextBox VolOffMonth2 = (TextBox)e.Row.FindControl("txtVolOffMonth2");
                TextBox VolOnMonth3 = (TextBox)e.Row.FindControl("txtVolOnMonth3");
                TextBox VolOffMonth3 = (TextBox)e.Row.FindControl("txtVolOffMonth3");
                string currentQtr = fq.GetQuarter("current");
                
                string QtrSelected = ViewState["Qtr"].ToString();

                string PrevQtr = fq.GetQuarter("prev");
                int Day = Convert.ToInt32(hdFreeze.Value);
                int CurrentDay = DateTime.Now.Day;
                string Month = System.DateTime.Now.Month.ToString();
                if (QtrSelected == currentQtr)
                {
                    if (Month == "2" || Month == "5" || Month == "8" || Month == "11")
                    {
                        if (CurrentDay > Day)
                        {
                            month1.CssClass = "borderempty TextBox";
                            VolOnMonth1.CssClass = "borderempty TextBox";
                            VolOffMonth1.CssClass = "borderempty TextBox";
                            month1.ReadOnly = true;
                            VolOnMonth1.ReadOnly = true;
                            VolOffMonth1.ReadOnly = true;
                        }

                    }
                    else if (Month == "3" || Month == "6" || Month == "9" || Month == "12")
                    {
                        month1.CssClass = "borderempty TextBox";
                        VolOnMonth1.CssClass = "borderempty TextBox";
                        VolOffMonth1.CssClass = "borderempty TextBox";
                        month1.ReadOnly = true;
                        VolOnMonth1.ReadOnly = true;
                        VolOffMonth1.ReadOnly = true;

                        if (CurrentDay > Day)
                        {
                            month2.CssClass = "borderempty TextBox";
                            VolOnMonth2.CssClass = "borderempty TextBox";
                            VolOffMonth2.CssClass = "borderempty TextBox";
                            month2.ReadOnly = true;
                            VolOnMonth2.ReadOnly = true;
                            VolOffMonth2.ReadOnly = true;
                        }
                    }
                }
                else if (QtrSelected == PrevQtr)
                {
                    month1.CssClass = "borderempty TextBox";
                    VolOnMonth1.CssClass = "borderempty TextBox";
                    VolOffMonth1.CssClass = "borderempty TextBox";
                    month1.ReadOnly = true;
                    VolOnMonth1.ReadOnly = true;
                    VolOffMonth1.ReadOnly = true;
                    month2.CssClass = "borderempty TextBox";
                    VolOnMonth2.CssClass = "borderempty TextBox";
                    VolOffMonth2.CssClass = "borderempty TextBox";
                    month2.ReadOnly = true;
                    VolOnMonth2.ReadOnly = true;
                    VolOffMonth2.ReadOnly = true;

                    if (Month == "1" || Month == "4" || Month == "7" || Month == "10")
                    {
                        if (CurrentDay > Day)
                        {
                            month3.CssClass = "borderempty TextBox";
                            VolOnMonth3.CssClass = "borderempty TextBox";
                            VolOffMonth3.CssClass = "borderempty TextBox";
                            month3.ReadOnly = true;
                            VolOnMonth3.ReadOnly = true;
                            VolOffMonth3.ReadOnly = true;
                        }
                    }
                    else
                    {
                        month3.CssClass = "borderempty TextBox";
                        VolOnMonth3.CssClass = "borderempty TextBox";
                        VolOffMonth3.CssClass = "borderempty TextBox";
                        month3.ReadOnly = true;
                        VolOnMonth3.ReadOnly = true;
                        VolOffMonth3.ReadOnly = true;
                    }

                }

            }

        }

        public void RefreshCOmbo()
        {
            try
            {
                string nso = ddlNSO.Text;
                string userID = Session["UserID"] + "";
                string quarter = ddlQuarter.Text.Remove(2);
                //string year = dateTime.Year + "-" + (dateTime.Year + 1 - 2000);
                string role = Session["Role"] + "";
                //   string currency = ddlCurrency.Text;

                if (nso.ToLowerTrim() == "all")
                {
                    ddlNSO.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { nso += k + ","; });
                    nso = nso.Replace("ALLALL,", string.Empty);
                    nso = nso.Trim().TrimEnd(',').TrimStart(',');
                }

                List<string> lstCustomerCode = new List<string>();
                lstCustomerCode = service.GetCustomerCodeDropDown(userID, nso);

                if (lstCustomerCode.Count > 0)
                {
                    ddlCustomerCode.DataSource = lstCustomerCode.ToNotNull();
                    ddlCustomerCode.DataBind();
                }
                else
                {
                    ddlCustomerCode.Items.Clear();
                }
                ddlCustomerCode.Items.Insert(0, "ALL");


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



        static List<DUPUCCMap> lstMapping = new List<DUPUCCMap>();
        private void LoadComboBox(string userID)
        {
            try
            {
                string role = Session["Role"] + "";

                



                string roleForAnchorChecking = service.GetUserRole(userID);
                //lstMapping = service.GetMapping(userID);

            DataSet ds = service.GetMapping_1(userID);
            var mapping = service.GetNSOCodeDescMapping();
                Func<string, string> funcGetNSODesc = (code) =>
                {
                    var item = mapping.FirstOrDefault(k => k.NSOCode == code);
                    if (item != null)
                        return item.NSODesc;
                    return code;
                };

             

           // if (lstMapping.Count() > 0)
                {
                //ddlNSO.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                ddlNSO.DataSource = ds.Tables[0].DefaultView;
                //ddlNSO.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                ddlNSO.DataTextField = "NSO";
                ddlNSO.DataValueField = "NSOCode";
                ddlNSO.DataBind();
            }
                ddlNSO.Items.Insert(0, "ALL");

                ddlNSO.SelectedIndex = 0;

                foreach (ListItem item in ddlNSO.Items) 
                    item.Attributes["title"] = funcGetNSODesc(item.Text);
                 

                //if (lstMapping.Count() > 0)
                //{
                //    ddlNSOpopup.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                //    ddlNSOpopup.DataBind();
                //}
                //ddlCustomerCodePopup.DataSource = service.GetCustomerCode(); // lstMapping.Select(k => k.CustomerCode).Distinct().ToList();
                //ddlCustomerCodePopup.DataBind();
                List<string> custcodes = new List<string>();

                //if (ddlNSOpopup.Items.Count > 0)
                //    custcodes = service.GetCustomerCodeForBEtype("Regular", ddlNSOpopup.Items[0].Text, userID);

                //if (custcodes.Count > 0)
                //{
                //    ddlCustomerCodePopup.DataSource = service.GetCustomerCodeForBEtype("Regular", ddlNSOpopup.Items[0].Text, userID);
                //    //ddlCustomerCodePopup.DataBind();

                //    ddlCustomerCodePopup.DataBind();
                //}

                BindQuarter();
                indexSelected = ddlQuarter.SelectedIndex;


            



                //ddlQuarterPopUp.Items.Insert(0, currentQtr);
                //ddlQuarterPopUp.Items.Insert(1, nextQtr);
                //ddlQuarterPopUp.Items.Insert(2, nextQtrPlus1);
                //ddlQuarterPopUp.Text = currentQtr;

                string nso = ddlNSO.Text;
                //string quarter = GetCurrentQuarter();
                string quarter = ddlQuarter.Text.Remove(2);
                string year = dateTime.Year + "-" + (dateTime.Year + 1 - 2000);
                // string currency = ddlCurrency.Text;

             
                //DataTable dt = new DataTable();
                //DataSet combtable = new DataSet();
                //if (role.ToLower() == "dm" || role.ToLower() == "others")
                //{
                //    combtable = service.GetDMBEData(pu, "All", userID, quarter, year, currency);

                //}
                //else
                //    combtable = service.GetNotDMBEData(pu, "All", userID, quarter, year, currency, "Screen");

                //dt = combtable.Tables[0];
                //List<string> lstCustomerCode = dt.Rows.OfType<DataRow>().Select(k => k["CustomerCode"] + "").Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();
                //ddlCustomerCode.DataSource = null;
                //if (lstCustomerCode.Count() > 0)
                //{
                //    ddlCustomerCode.DataSource = lstCustomerCode;
                //    ddlCustomerCode.DataBind();
                //}
                //ddlCustomerCode.Items.Insert(0, "ALL");
                //if (pu.ToLowerTrim() == "all")
                //{
                //    ddlNSO.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
                //    pu = pu.Replace("ALLALL,", string.Empty);
                //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
                //}

                List<string> lstCustomerCode = new List<string>();
                lstCustomerCode = service.GetCustomerCodeDropDown(userID, nso);

                if (lstCustomerCode.Count > 0)
                {
                    ddlCustomerCode.DataSource = lstCustomerCode;
                    ddlCustomerCode.DataBind();
                }
                ddlCustomerCode.Items.Insert(0, "ALL");

                //ddlCurrencypopup.DataSource = service.GetCurrency(ddlNSOpopup.Text);

                //ddlCurrencypopup.DataBind();

                //lbldmsdmemail.Text = userID.ToString();

                //if (roleForAnchorChecking.ToLowerTrim() == "anchor")
                //{
                //    lbldmsdmemail.Visible = false;
                //    ddlDMpopup.Visible = true;

                //    ddlDMpopup.DataSource = service.GetDMMailList(userID, ddlNSOpopup.Text, role);//, ddlCurrencypopup.Text);
                //    ddlDMpopup.DataBind();
                //}
                //else
                //{
                //    lbldmsdmemail.Visible = true;
                //    ddlDMpopup.Visible = false;
                //}

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

        private void BindQuarter()
        {
            ddlQuarter.Items.Clear();
            string PrevQtr = DateUtility.GetQuarter("prev");
            Session["PreviousQuarter"] = PrevQtr;
            //string PrevQtr = DateUtility.GetQuarter("prev");
            string currentQtr = DateUtility.GetQuarter("current");
            string nextQtr = DateUtility.GetQuarter("next");
            string nextQtrPlus1 = DateUtility.GetQuarter("next1");
            ddlQuarter.Text = currentQtr;


            ddlQuarter.Items.Insert(0, PrevQtr);
            ddlQuarter.Items.Insert(1, currentQtr);
            ddlQuarter.Items.Insert(2, nextQtr);
            ddlQuarter.Items.Insert(3, nextQtrPlus1);

            ddlQuarter.Text = currentQtr;
          
        }

        private void BindQuarter1()
        {
            string value = ddlQuarter.Text;
            ddlQuarter.Items.Clear();
            string PrevQtr = DateUtility.GetQuarter("prev");
            Session["PreviousQuarter"] = PrevQtr;
            //string PrevQtr = DateUtility.GetQuarter("prev");
            string currentQtr = DateUtility.GetQuarter("current");
            string nextQtr = DateUtility.GetQuarter("next");
            string nextQtrPlus1 = DateUtility.GetQuarter("next1");
            //ddlQuarter.Text = currentQtr;
            
            ddlQuarter.Items.Insert(0, PrevQtr);
            ddlQuarter.Items.Insert(1, currentQtr);
            ddlQuarter.Items.Insert(2, nextQtr);
            ddlQuarter.Items.Insert(3, nextQtrPlus1);

            ddlQuarter.Text = value;

        }



        public string GetCurrentQuarter()
        {
            int year = DateTime.Today.Year;
            DateTime todaydate = dateTime;
            string strcurrent = "";

            int nxtyr;

            if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                nxtyr = year - 1;
            else
                nxtyr = year;

            string qtr = ddlQuarter.Text.Remove(2);
            //if (qtr == "Q3")
            //{
            //    TextBox1.Text = "10/12/" + year;
            //}
            //else if (qtr == "Q2")
            //{
            //    TextBox1.Text = "7/30/" + year;
            //}
            //else if (qtr == "Q1")
            //{
            //    TextBox1.Text = "5/9/" + year;
            //}
            //else if (qtr == "Q4")
            //{
            //    TextBox1.Text = "1/11/" + nxtyr;
            //}



            //dateTime = Convert.ToDateTime(TextBox1.Text);



            if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)

                strcurrent = "Q4";
            else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                strcurrent = "Q1";
            else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                strcurrent = "Q2";
            else
                strcurrent = "Q3";

            return strcurrent;


        }

        public string GetNextQuarter()
        {



            string strNext = "";
            DateTime todaydate = dateTime;
            if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                strNext = "Q1";
            else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                strNext = "Q2";
            else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                strNext = "Q3";
            else
                strNext = "Q4";
            return strNext;


        }

        public string GetPreviousQuarter()
        {


            string strNext = "";
            DateTime todaydate = dateTime;
            if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                strNext = "Q3";
            else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                strNext = "Q4";
            else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                strNext = "Q1";
            else
                strNext = "Q2";
            return strNext;


        }


         

        protected void grdBEDMView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            base.ValidateSession();


            string QtrYrSelected = ViewState["Qtr"].ToString();
            string YrSelected = QtrYrSelected.Substring(3, 2);
            int yr = Convert.ToInt32(YrSelected);


            string user = HttpContext.Current.User.Identity.Name;
            string[] userids = user.Split('\\');
            if (userids.Length == 2)
            {
                user = userids[1];
            }


            string Role = "";   
            string LoggedUserIdRole = "";  



            //try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {

                      Role =  service.GetUserRole(user);
                      LoggedUserIdRole = service.GetUserRole(Session["UserID"].ToString());



                    int year = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
                    DateTime todaydate = dateTime;
                    int nxtyr;
                    //if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                    //    nxtyr = year - 1;
                    //else
                    //    nxtyr = year;

                    // hdnqtr.Value = ddlQuarter.SelectedIndex + "";
                    string qtr = Session["quarter"] + "";
                    


                    GridView objGridView = (GridView)sender;


                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell objtablecell = new TableCell();

                    //4 made to 3 as DMMailid is removed
                    AddMergedCells(objgridviewrow, objtablecell, 3, "", "#c41502");
                    AddMergedCells(objgridviewrow, objtablecell, 10, "Revenue BE ( in NC '000)", "#c41502");

                    AddMergedCells(objgridviewrow, objtablecell, 11, "Volume BE ( in person months)", "#c41502");

                    //AddMergedCells(objgridviewrow, objtablecell, 1, "", "#c41502");

                    //if (ddlCurrency.Text.ToLowerTrim() == "usd")
                    //    AddMergedCells(objgridviewrow, objtablecell, 4, "RTBR/FinPulse (KUSD) ", "#c41502");
                    //else
                    //    AddMergedCells(objgridviewrow, objtablecell, 4, "RTBR/FinPulse ('000 NC) ", "#c41502");

                    //4 made to 2 as two columns DM modified on and Dm remarks is removed


                    objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);

                    var row = e.Row;

                    int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;

                    // currentYear = currentYear - 2000;
                    //  string _CurrentQ = string.Empty;
                    //string _NextQ = string.Empty;
                    currentYear = currentYear - 2000;
                    //dm--dmmailid column is removed
                    string _CurrentQ = string.Empty;
                    _CurrentQ = Session["currqtr"] + "";
                    string currentQuarter = Session["quarter"] + "";

                    // Month1 / Month2 / Month3
                    string _month1 = string.Empty;
                    string _month2 = string.Empty;
                    string _month3 = string.Empty;
                    string mon1 = string.Empty;
                    string mon2 = string.Empty;
                    string mon3 = string.Empty;
                    if (currentQuarter == "Q4")
                    {
                        //currentYear = currentYear;
                        _month1 = "Jan";
                        _month2 = "Feb";
                        _month3 = "Mar";
                        mon1 = " " + _month1 + "'" + yr + " ";
                        mon2 = " " + _month2 + "'" + yr + " ";
                        mon3 = " " + _month3 + "'" + yr + " ";

                    }
                    else if (currentQuarter == "Q1")
                    {
                        _month1 = "Apr";
                        _month2 = "May";
                        _month3 = "Jun";
                        mon1 = " " + _month1 + "'" + (yr - 1) + " ";
                        mon2 = " " + _month2 + "'" + (yr - 1) + " ";
                        mon3 = " " + _month3 + "'" + (yr - 1) + " ";
                    }
                    else if (currentQuarter == "Q2")
                    {
                        _month1 = "Jul";
                        _month2 = "Aug";
                        _month3 = "Sep";
                        mon1 = " " + _month1 + "'" + (yr - 1) + " ";
                        mon2 = " " + _month2 + "'" + (yr - 1) + " ";
                        mon3 = " " + _month3 + "'" + (yr - 1) + " ";
                    }
                    else
                    {
                        _month1 = "Oct";
                        _month2 = "Nov";
                        _month3 = "Dec";
                        mon1 = " " + _month1 + "'" + (yr - 1) + " ";
                        mon2 = " " + _month2 + "'" + (yr - 1) + " ";
                        mon3 = " " + _month3 + "'" + (yr - 1) + " ";
                    }

                   


                    GridViewRow objgridviewrow2 = new GridViewRow(2, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    AddMergedCells(objgridviewrow2, objtablecell, 3, "", "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 4, "BE Projections(x)", "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 4, "**Additional Inputs for the qtr" + "\r\n" + " (y)", "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 2, "", "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 2, mon1, "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 2, mon2, "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 2, mon3, "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 2, "Total", "#c41502");
                    AddMergedCells(objgridviewrow2, objtablecell, 3, "", "#c41502");
                    //AddMergedCells(objgridviewrow2, objtablecell, 1, "", "#c41502");

                    if (currentQuarter == "Q4")
                    {
                        row.Cells[4].Text = " " + _month1 + "'" + (yr) + " ";
                        row.Cells[5].Text = " " + _month2 + "'" + (yr) + " ";
                        row.Cells[6].Text = " " + _month3 + "'" + (yr) + " ";
                    }
                    else if (currentQuarter == "Q1")
                    {
                        row.Cells[4].Text = " " + _month1 + "'" + (yr - 1) + " ";
                        row.Cells[5].Text = " " + _month2 + "'" + (yr - 1) + " ";
                        row.Cells[6].Text = " " + _month3 + "'" + (yr - 1) + " ";
                    }
                    else if (currentQuarter == "Q2")
                    {
                        row.Cells[4].Text = " " + _month1 + "'" + (yr - 1) + " ";
                        row.Cells[5].Text = " " + _month2 + "'" + (yr - 1) + " ";
                        row.Cells[6].Text = " " + _month3 + "'" + (yr - 1) + " ";
                    }
                    else if (currentQuarter == "Q3")
                    {
                        row.Cells[4].Text = " " + _month1 + "'" + (yr - 1) + " ";
                        row.Cells[5].Text = " " + _month2 + "'" + (yr - 1) + " ";
                        row.Cells[6].Text = " " + _month3 + "'" + (yr - 1) + " ";
                    }



                    //dm:dm mailid is removed reduced by 1

                    row.Cells[7].Text = "" + _CurrentQ + "";
                    row.Cells[12].Text = "SDM " + _CurrentQ + "";
                    row.Cells[22].Text = "" + _CurrentQ + "";
                    row.Cells[24].Text = "SDM " + _CurrentQ + "";
                    //sdm:dm mailid is removed reduced by 1
                    //row.Cells[9].Text = " " + _month1 + "'" + currentYear + " ";
                    //row.Cells[10].Text = " " + _month2 + "'" + currentYear + " ";
                    //row.Cells[11].Text = " " + _month3 + "'" + currentYear + " ";

                    //dm mailid is removed reduced by 1
                    string constt = "";
                    
                    objGridView.Controls[0].Controls.AddAt(1, objgridviewrow2);
                    CheckBox chkBxHeader = e.Row.Cells[0].FindControl("chkBxHeader") as CheckBox;

                    //bool isFreezed = false;
                    //if (isFreezed)
                    //{
                    //    chkBxHeader.Enabled = false;
                    //    lblInfoVol.Text = "&nbsp &nbsp  &nbsp Application Freezed !";
                    //    lblInfoVol.ForeColor = System.Drawing.Color.Green;
                    //}

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtDMMonth1 = (TextBox)e.Row.FindControl("txtDMMonth1");
                    TextBox txtDMMonth2 = (TextBox)e.Row.FindControl("txtDMMonth2");
                    TextBox txtDMMonth3 = (TextBox)e.Row.FindControl("txtDMMonth3");

                    TextBox txtBKMonth1 = (TextBox)e.Row.FindControl("txtBKMonth1");
                    TextBox txtBKMonth2 = (TextBox)e.Row.FindControl("txtBKMonth2");
                    TextBox txtBKMonth3 = (TextBox)e.Row.FindControl("txtBKMonth3");
                    TextBox txtBKMonth4 = (TextBox)e.Row.FindControl("txtBKMonth4");

                    TextBox txtOnM1 = (TextBox)e.Row.FindControl("txtVolOnMonth1");
                    TextBox txtOnM2 = (TextBox)e.Row.FindControl("txtVolOnMonth2");
                    TextBox txtOnM3 = (TextBox)e.Row.FindControl("txtVolOnMonth3");
                    TextBox txtOffM1 = (TextBox)e.Row.FindControl("txtVolOffMonth1");
                    TextBox txtOffM2 = (TextBox)e.Row.FindControl("txtVolOffMonth2");
                    TextBox txtOffM3 = (TextBox)e.Row.FindControl("txtVolOffMonth3");

                    HyperLink lnkbtnDMTotal = (HyperLink)e.Row.FindControl("lnkbtnDMTotal");
                    HyperLink lnkbtnDMVolTotal = (HyperLink)e.Row.FindControl("lnkbtnDMVolTotal");
                    lnkbtnDMTotal.Attributes.Add("onclick", "PopUpDMBE(this)");
                    lnkbtnDMVolTotal.Attributes.Add("onclick", "PopUpDMBE(this)");

                    ////changes KD
                   // HyperLink lnkbtnBcktC = (HyperLink)e.Row.FindControl("lnkbtnBcktC");
                   
                   // HyperLink lnkbtnBcktD = (HyperLink)e.Row.FindControl("lnkbtnBcktD");
                   // lnkbtnBcktC.Attributes.Add("onclick", "PopUpBcktC(this)");
                    //lnkbtnBcktD.Attributes.Add("onclick", "PopUpBcktD(this)");


                    foreach (TextBox txt in new TextBox[] { txtOnM1, txtOnM2, txtOnM3, txtOffM1, txtOffM2, txtOffM3 })
                    {
                        txt.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");
                        //txt.Attributes.Add("onblur", "loaddata(this)");

                       
                        if (Role == "Admin")
                        {
                            if (LoggedUserIdRole == "DH" || LoggedUserIdRole == "PnA")
                            {
                                txt.Attributes.Add("onKeydown", "return PressNegative(event,this)");
                                txt.Attributes.Add("onblur", "loaddataAdmin(this)");
                            }
                            else
                            {
                                txt.Attributes.Add("onblur", "loaddata(this)");
                            }
                        }
                        else if (Role == "DH" || Role == "PnA")
                        {
                            if (Session["UserID"].ToString() == user)
                            {
                                txt.Attributes.Add("onKeydown", "return PressNegative(event,this)");
                                txt.Attributes.Add("onblur", "loaddataAdmin(this)");
                            }
                            else
                            {
                                txt.Attributes.Add("onblur", "loaddata(this)");
                            }
                        }
                        else
                        {
                            txt.Attributes.Add("onblur", "loaddata(this)");
                        }
                        
                    }

                    //foreach (TextBox txt in new TextBox[] { txtOnM1, txtOnM2, txtOnM3, txtOffM1, txtOffM2, txtOffM3 })
                    //{
                    //    txt.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");
                    //    txt.Attributes.Add("onblur", "PressNumberOnlyAndCalcVol(this)");
                    //}


                    foreach (TextBox txt in new TextBox[] { txtBKMonth1, txtBKMonth2, txtBKMonth3, txtBKMonth4 })
                    {
                        txt.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");
                        txt.Attributes.Add("onblur", "PressNumberOnlyAndCalcBK(this)");
                        txt.Attributes.Add("onKeydown", "return PressNegative(event,this)");
                    }




                  
                    txtDMMonth1.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");
                    txtDMMonth2.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");
                    txtDMMonth3.Attributes.Add("onKeydown", "return PressNumberOnly(event,this)");
                    txtDMMonth1.Attributes.Add("onKeydown", "return PressNegative(event,this)");
                    txtDMMonth2.Attributes.Add("onKeydown", "return PressNegative(event,this)");
                    txtDMMonth3.Attributes.Add("onKeydown", "return PressNegative(event,this)");
                    txtBKMonth1.Attributes.Add("onKeydown", "return PressNegative(event,this)");


                  

                    txtDMMonth1.Attributes.Add("onblur", "PressNumberOnlyAndCalc( this)");
                    txtDMMonth2.Attributes.Add("onblur", "PressNumberOnlyAndCalc( this)");
                    txtDMMonth3.Attributes.Add("onblur", "PressNumberOnlyAndCalc( this)");

                    //var row = e.Row.DataItem as DataRowView;
                    //if (row != null)
                    //{
                    //    string BEid = row.Row.ItemArray[0] + "";
                    //    string qtr = row.Row.ItemArray[3] + "";
                    //    string queryString = string.Format("SDMDetails.aspx?BEID={0}&qtr={1}", BEid, qtr);

                    //    lnkbtnDMTotal.Attributes.Add("onclick", "OpenPopUpDetailsPage('" + queryString + "'); return false");
                    //    lnkbtnDMTotal.Attributes.Add("href", "#");


                    //    string queryStringVol = string.Format("SDMDetails.aspx?BEID={0}&qtr={1}", BEid, qtr);

                    //    lnkbtnDMVolTotal.Attributes.Add("onclick", "OpenPopUpDetailsPage('" + queryStringVol + "'); return false");

                    //    lnkbtnDMVolTotal.Attributes.Add("href", "#");






                    //}


                  
                    ////object DMMonth1 = DataBinder.Eval(e.Row.DataItem, "DMMonth1");
                    ////object DMMonth2 = DataBinder.Eval(e.Row.DataItem, "DMMonth2");
                    ////object DMMonth3 = DataBinder.Eval(e.Row.DataItem, "DMMonth3");
                    ////object DMRem = DataBinder.Eval(e.Row.DataItem, "Remarks");

                    ////object SDMMonth1 = DataBinder.Eval(e.Row.DataItem, "SDMMonth1");
                    ////object SDMMonth2 = DataBinder.Eval(e.Row.DataItem, "SDMMonth2");
                    ////object SDMMonth3 = DataBinder.Eval(e.Row.DataItem, "SDMMonth3");
                    ////object SDMrem = DataBinder.Eval(e.Row.DataItem, "SDMRemarks");

                    string role = Session["Role"] + "";

                    int currentMonth = DateTime.Now.Month; //DateTime.Now.Month;
                    //  int year = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
                    // int currentMonth = ddlQuarter.Text

                    // enable for next two quarters 
                    int ddlQtrIndex = ddlQuarter.SelectedIndex;
                    System.Drawing.Color disableColor = System.Drawing.Color.FromName("#f2f1ef");
                    //fltSDMMonth1BE.Enabled = false;
                    //fltSDMMonth1BE.BorderStyle = BorderStyle.None;
                    //fltSDMMonth1BE.Style.Add("border-width", "0px");
                    //fltSDMMonth2BE.Enabled = false;
                    //fltSDMMonth2BE.BorderStyle = BorderStyle.None;
                    //fltSDMMonth2BE.Style.Add("border-width", "0px");
                    //fltSDMMonth3BE.Enabled = false;
                    //fltSDMMonth3BE.BorderStyle = BorderStyle.None;
                    //fltSDMMonth3BE.Style.Add("border-width", "0px");

                    int tempyear = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
                    string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

                    //string year = "2012-13";
                    string quarter = ddlQuarter.Text.Remove(2);

                    string currentQuarter = GetCurrentQuarter();

                    int currentMnth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;

                    currentYear = currentYear - 2000;

                    //CopyActuals(QtrSelected, currentQuarter, currentMnth, currentYear);
                   


                    //List<BEMonthlyFreeze> lstMonthlyfreeze = new List<BEMonthlyFreeze>();
                    //lstMonthlyfreeze = service.GetMonthlyFreezeDetails();
                    //lstMonthlyfreeze = lstMonthlyfreeze.Where(k => k.Year == year).Where(k => k.Quarter == quarter).ToList();
                    //be = lstMonthlyfreeze[0];


                    //if (be.Month1 == true)
                    //{
                    //    txtSDMMonth1.Enabled = false;
                    //    txtSDMMonth1.CssClass = "TextBoxasLabel";
                    //    //txtSDMMonth1.BorderStyle = BorderStyle.None;
                    //    //txtSDMMonth1.Style.Add("border-width", "0px");
                    //}
                    //else
                    //    txtSDMMonth1.CssClass = "TextBox";
                    //if (be.Month2 == true)
                    //{
                    //    txtSDMMonth2.Enabled = false;
                    //    txtSDMMonth2.CssClass = "TextBoxasLabel";
                    //    //txtSDMMonth2.BorderStyle = BorderStyle.None;
                    //    //txtSDMMonth2.Style.Add("border-width", "0px");

                    //}
                    //else
                    //    txtSDMMonth2.CssClass = "TextBox";
                    //if (be.Month3 == true)
                    //{
                    //    txtSDMMonth3.Enabled = false;
                    //    txtSDMMonth3.CssClass = "TextBoxasLabel";
                    //    //txtSDMMonth3.BorderStyle = BorderStyle.None;
                    //    //txtSDMMonth3.Style.Add("border-width", "0px");

                    //}
                    //else
                    //    txtSDMMonth3.CssClass = "TextBox";


                    //if (Session[Constants.IsFreezed] == "1")
                    //{



                    //    txtSDMMonth1.Enabled = false;
                    //    txtSDMMonth2.Enabled = false;
                    //    txtSDMMonth3.Enabled = false;
                    //    txtSDMRem.Enabled = false;
                    //    //txtSDMNext.Enabled = false;
                    //    //  }

                    CheckBox chkRow = e.Row.Cells[0].FindControl("chkRow") as CheckBox;





                }





            }

          
            if (e.Row.RowType == DataControlRowType.Footer)
            {


                //actual added so 17 becums 21 -remarks added so becum 22
                //3 cols removed:21 bcums 18 so 19
                for (int i = 0; i < 25; i++)
                {
                    e.Row.Cells[i].CssClass = "FooterTotal";
                    // e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[i].ForeColor = System.Drawing.Color.FromName("#ffcb8b");
                    //e.Row.Cells[i].BackColor = System.Drawing.Color.FromName("#CC0000");
                    //e.Row.Cells[i].Attributes.Add("class", "footerBox");
                    e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
                }

               

                ((TextBox)e.Row.FindControl("lblmonth1")).Text = DMMonth1_total + "";
                ((TextBox)e.Row.FindControl("lblmonth2")).Text = DMMonth2_total + "";
                ((TextBox)e.Row.FindControl("lblmonth3")).Text = DMMonth3_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonthTotal")).Text = DMQCur_total + " (" + Diff + ")";
                ((TextBox)e.Row.FindControl("lblBKmonth1")).Text = BK1_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonth2")).Text = BK2_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonth3")).Text = BK3_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonth4")).Text = BK4_total + "";

                ((TextBox)e.Row.FindControl("lblDMBETotal")).Text = SDMRevBE.ToString() + "";
                ((TextBox)e.Row.FindControl("lblFooterRtbr")).Text = rtbrFinPulse + "";

                ((TextBox)e.Row.FindControl("lblVolOnmonth1")).Text = VolOn1 + "";
                ((TextBox)e.Row.FindControl("lblVolOffmonth1")).Text = VolOff1 + "";
                ((TextBox)e.Row.FindControl("lblVolOnmonth2")).Text = VolOn2 + "";
                ((TextBox)e.Row.FindControl("lblVolOffmonth2")).Text = VolOff2 + "";
                ((TextBox)e.Row.FindControl("lblVolOnmonth3")).Text = VolOn3 + "";
                ((TextBox)e.Row.FindControl("lblVolOffmonth3")).Text = VolOff3 + "";

                ((TextBox)e.Row.FindControl("lblOnTotal")).Text = VolOnTotal + "";
                ((TextBox)e.Row.FindControl("lblOffTotal")).Text = VolOffTotal + "";
                ((TextBox)e.Row.FindControl("lblFooterAlcon")).Text = VolTotal + "";
                ((TextBox)e.Row.FindControl("lblFooterVolsdm")).Text = SDMVolBE.ToString() + "";
               // ((TextBox)e.Row.FindControl("lblCompetencyDM")).Text = ComVolBE + "";

                ////////

                ((TextBox)e.Row.FindControl("lblmonth1")).ToolTip = DMMonth1_total + "";
                ((TextBox)e.Row.FindControl("lblmonth2")).ToolTip = DMMonth2_total + "";
                ((TextBox)e.Row.FindControl("lblmonth3")).ToolTip = DMMonth3_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonthTotal")).ToolTip = DMQCur_total + "(" + Diff + ")";
                ((TextBox)e.Row.FindControl("lblBKmonth1")).ToolTip = BK1_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonth2")).ToolTip = BK2_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonth3")).ToolTip = BK3_total + "";
                ((TextBox)e.Row.FindControl("lblBKmonth4")).ToolTip = BK4_total + "";

                ((TextBox)e.Row.FindControl("lblDMBETotal")).ToolTip = SDMRevBE + "";
                ((TextBox)e.Row.FindControl("lblFooterRtbr")).ToolTip = rtbrFinPulse + "";

                ((TextBox)e.Row.FindControl("lblVolOnmonth1")).ToolTip = VolOn1 + "";
                ((TextBox)e.Row.FindControl("lblVolOffmonth1")).ToolTip = VolOff1 + "";
                ((TextBox)e.Row.FindControl("lblVolOnmonth2")).ToolTip = VolOn2 + "";
                ((TextBox)e.Row.FindControl("lblVolOffmonth2")).ToolTip = VolOff2 + "";
                ((TextBox)e.Row.FindControl("lblVolOnmonth3")).ToolTip = VolOn3 + "";
                ((TextBox)e.Row.FindControl("lblVolOffmonth3")).ToolTip = VolOff3 + "";

                ((TextBox)e.Row.FindControl("lblOnTotal")).ToolTip = VolOnTotal + "";
                ((TextBox)e.Row.FindControl("lblOffTotal")).ToolTip = VolOffTotal + "";
                ((TextBox)e.Row.FindControl("lblFooterAlcon")).ToolTip = VolTotal + "";
                ((TextBox)e.Row.FindControl("lblFooterVolsdm")).ToolTip = SDMVolBE + "";
                //((TextBox)e.Row.FindControl("lblCompetencyDM")).ToolTip = ComVolBE + "";


                //e.Row.Cells[3].Text = DMMonth1_total + "";
                //e.Row.Cells[4].Text = DMMonth2_total + "";
                //e.Row.Cells[5].Text = DMMonth3_total + "";
                //e.Row.Cells[6].Text = DMQCur_total + "(" + Diff + ")";
                ////e.Row.Cells[10].Text = DMQNext_total + ""; //TODO
                ////e.Row.Cells[5].Text = DMQPrev_total + "";
                //e.Row.Cells[7].Text = BK1_total + "";
                //e.Row.Cells[8].Text = BK2_total + "";
                //e.Row.Cells[9].Text = BK3_total + "";
                //e.Row.Cells[10].Text = BK4_total + "";
                //e.Row.Cells[11].Text = SDMRevBE + "";
                //e.Row.Cells[12].Text = rtbrFinPulse + "";
                //e.Row.Cells[13].Text = VolOn1 + "";
                //e.Row.Cells[14].Text = VolOff1 + "";
                //e.Row.Cells[15].Text = VolOn2 + "";
                //e.Row.Cells[16].Text = VolOff2 + "";
                //e.Row.Cells[17].Text = VolOn3 + "";
                //e.Row.Cells[18].Text = VolOff3 + "";
                //e.Row.Cells[19].Text = VolOnTotal + "";
                //e.Row.Cells[20].Text = VolOffTotal + "";
                //e.Row.Cells[21].Text = VolTotal + "";
                //e.Row.Cells[23].Text = SDMVolBE + "";


            }


            //catch (Exception ex)
            //{


            //    if ((ex.Message + "").Contains("Thread was being aborted."))
            //        logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //    else
            //    {
            //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //        throw ex;
            //    }
            //}
        }

       
   

        private static void MakeTextBoxasLabel(TextBox txtDMRem)
        {

            txtDMRem.CssClass = "Label";
            txtDMRem.Attributes.Add("onKeydown", "return PressReadOnly(event,this)");

        }


        private decimal GetCellControlValue(GridViewRowEventArgs e, int position)
        {
            decimal returnValue = default(decimal);
            var cell = e.Row.Cells[position];
            if (cell.Text.Length == 0)
            {
                if (cell.Controls.Count > 0)
                {
                    var textbox = cell.Controls.OfType<TextBox>().SingleOrDefault();
                    var lable = cell.Controls.OfType<Label>().SingleOrDefault();
                    if (textbox == null)
                    {
                        if (lable == null)
                            returnValue = default(decimal);
                        else
                            returnValue = Convert.ToDecimal(lable.Text.Trim().Length == 0 ? "0" : lable.Text.Trim());
                    }
                    else
                    {
                        returnValue = Convert.ToDecimal(textbox.Text.Trim().Length == 0 ? "0" : textbox.Text.Trim());
                    }
                }
            }
            return returnValue;

        }


        protected void AddMergedCells(GridViewRow objgridviewrow,
    TableCell objtablecell, int colspan, string celltext, string backcolor)
        {



            try
            {
                objtablecell = new TableCell();
                objtablecell.Text = celltext;
                objtablecell.Font.Bold = true;
                objtablecell.ColumnSpan = colspan;
                //objtablecell.Style.Add("background-color", backcolor);
                //objtablecell.Style.Add("border-bottom-color", "#878484");// "#c41502");

                objtablecell.HorizontalAlign = HorizontalAlign.Center;
                // objtablecell.BorderColor = System.Drawing.Color.FromName("#c41502");//("#525252");
               // objtablecell.BorderColor = System.Drawing.Color.DarkSlateGray;
                //objtablecell.ForeColor = System.Drawing.Color.FromName("#ffcb8b");
                objtablecell.Attributes.Add("class", "GridHeader");
                objgridviewrow.Cells.Add(objtablecell);
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



        protected void ddlNSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.ValidateSession();

            try
            {
                string nso = ddlNSO.Text;
                string userID = Session["UserID"] + "";
                //if (pu.ToLower() == "all")
                //{
                //    ddlCustomerCode.DataSource = lstMapping.Select(k => k.CustomerCode).Distinct().ToList();
                //    ddlCustomerCode.DataBind();
                //}
                //else
                //{
                //    ddlCustomerCode.DataSource = lstMapping.Where(k => k.PU == pu).Select(k => k.CustomerCode).ToList();
                //    ddlCustomerCode.DataBind();
                //}
                //ddlCustomerCode.Items.Insert(0, "ALL");

                //if (pu.ToLowerTrim() == "all")
                //{
                //    ddlNSO.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
                //    pu = pu.Replace("ALLALL,", string.Empty);
                //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
                //}

                List<string> lstCustomerCode = new List<string>();
                lstCustomerCode = service.GetCustomerCodeDropDown(userID, nso);

                if (lstCustomerCode.Count > 0)
                {
                    ddlCustomerCode.DataSource = lstCustomerCode;
                    ddlCustomerCode.DataBind();
                }
                ddlCustomerCode.Items.Insert(0, "ALL");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "loadinggifClose", "loadinggifClose()", true);
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

        protected void ddlNSOpopup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                // ddlCurrencypopup.DataSource = service.GetCurrency(ddlNSOpopup.Text);
                //ddlCurrencypopup.DataTextField = "txtNativeCurrency";
                //  ddlCurrencypopup.DataBind();

                string userID = Session["UserID"] + "";
                string role = Session["Role"] + "";
                //  ddlDMpopup.DataSource = service.GetDMMailList(userID, ddlNSOpopup.Text, role);//, ddlCustomerCodePopup.Text, ddlNSOpopup.Text, ddlCurrencypopup.Text);
                //  ddlDUpopup.DataTextField = "txtNativeCurrency";
                //  ddlDMpopup.DataBind();
                // lbldmsdmemail.Text = userID;

                // string be = ddlBEType.SelectedItem.Text.Trim();
                //string pu = ddlNSOpopup.SelectedItem.Text.Trim();
                //ddlCustomerCodePopup.Items.Clear();

                //ddlCustomerCodePopup.Items.Clear();
                //ddlCustomerCodePopup.DataSource = service.GetCustomerCodeForBEtype("", pu, userID);
                //ddlCustomerCodePopup.DataBind();

                // ModalPopupExtender1.Show();
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

  
        decimal DMMonth1_total = default(decimal);
        decimal DMMonth2_total = default(decimal);
        decimal DMMonth3_total = default(decimal);
        decimal SDMRevBE = default(decimal);

        decimal SDMVolBE = default(decimal);
        decimal ComVolBE = default(decimal);

        decimal DMQCur_total = default(decimal);
        //decimal DMQNext_total = default(decimal);
        decimal BK1_total = default(decimal);

        decimal BK2_total = default(decimal);
        decimal BK3_total = default(decimal);
        decimal BK4_total = default(decimal);
        decimal BK_total = default(decimal);
        decimal Diff = default(decimal);
        decimal VolOn1 = default(decimal);
        decimal VolOn2 = default(decimal);
        decimal VolOn3 = default(decimal);
        decimal VolOff1 = default(decimal);
        decimal VolOff2 = default(decimal);
        decimal VolOff3 = default(decimal);
        decimal VolOnTotal = default(decimal);
        decimal VolOffTotal = default(decimal);
        decimal VolTotal = default(decimal);
        decimal rtbrFinPulse = default(decimal);


        private decimal GetDecimalCellValue(DataRow row, string columnName)
        {
            decimal returnValue = default(decimal);

            string value = (row[columnName] + "").Length == 0 ? "0" : row[columnName] + "";
            returnValue = Convert.ToDecimal(value);


            return returnValue;
        }

        bool ismonth1star = false;
        bool ismonth2star = false;
        bool ismonth3star = false;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            base.ValidateSession();

            Day = service.FreezingPreviousMonthBE();

            hdFreeze.Value = Day.ToString();

            Session["CurrentQuarter"] = GetCurrentQuarter();
            int CurrentDay = DateTime.Now.Day;

            ViewState["Qtr"] = ddlQuarter.Text;
            
            string yesorno = null;
            string queryQtr = null;
            string quarter = null;
            string Queryyear = null;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);
            if (!IsPostBack)
            {
                yesorno = Request.QueryString["Yes"];
                queryQtr = Request.QueryString["Qtr"];
                Queryyear = Request.QueryString["Year"];
                Session["quarter"] = queryQtr;
                Session["FyYear"] = Queryyear;
            }
            if (yesorno == null)
            {
                lblmsg1.Visible = false;
                quarter = ddlQuarter.Text.Remove(2).Trim();
                Session["quarter"] = quarter;

            }
            else
            {
                lblmsg1.Visible = true;
                lblmsg1.Text = "Record(s) have been added";
                lblmsg1.ForeColor = Color.Green;
                string ddlvalue = Session["quarter"].ToString() + "'" + Session["FyYear"].ToString();
                ddlQuarter.SelectedValue = ddlvalue;
               
                NameValueCollection filtered = new NameValueCollection(Request.QueryString);
                filtered.Remove("Yes");
                filtered.Remove("Qtr");
                filtered.Remove("Year");

            }
           
            {
                string userID = Session["userid"].ToString();
                // LoadComboBox(userID);
                string nso = ddlNSO.SelectedValue;
                string customerCOde = ddlCustomerCode.Text;
                string role = Session["Role"] + "";

                string isreadonly = "SELECT [txtisReadOnly] FROM [BEUserAccess_NSO] where txtUserId='" + userID + "'";
                DataSet dsreadonly = service.GetDataSet(isreadonly);
                DataTable dtreadonly = dsreadonly.Tables[0];
                string check = dtreadonly.Rows[0][0].ToString();

                if (role == "DM" || role == "Admin" || (role == "Anchor" && check == "True"))
                {
                    btnAddMasterCustomer.Visible = true;
                    ImgDownloadToExcel.Visible = true;
                    btnSave2.Visible = true;
                    lblmsg.Visible = false;
                    btnZeroBE.Visible = true;
                    lblmsg3.Visible = false;
                    //btnSave2.Visible = true;
                    bulk.Visible = true;
                  
                }
                else if ((role == "Anchor" && check == "False") || role == "PnA")
                {
                    btnAddMasterCustomer.Visible = false;
                    //btnSave.Visible = true;
                    //  btnCopy.Visible = false;
                    ImgDownloadToExcel.Visible = true;
                    //btncopydata.Visible = true;
                    btnSave2.Visible = false;
                    lblmsg.Visible = false;
                    btnZeroBE.Visible = false;
                    lblmsg3.Visible = false;
                    //btnSave2.Visible = false;
                    
                    bulk.Visible = false;
                    // btncopydata.Visible = false;

                }

                //if (Session["Role"].ToString() == "Admin" || Session["Role"].ToString() == "PnA")
                //{

                //    role = "Anchor";
                //}

            if (Session["Role"].ToString() == "Admin")
            {

                role = "Admin";
            }

            if (Session["Role"].ToString() == "PnA")
            {

                role = "PnA";
            }




            string Mrole = service.GetUserRole(Session["LoggedInUserID"].ToString().ToLower());

                if (Mrole == "PnA")
                {
                    if (Session["UserID"].ToString().ToLower() == Session["LoggedInUserID"].ToString().ToLower())
                    {
                        btnSave2.Visible = true;
                    }
                    else
                    {
                        btnSave2.Visible = false;
                    }
                }

                quarter = ddlQuarter.Text.Remove(2);
                Session["quarter"] = quarter;
                // Session.Add(quarter, "qtr");
                string currqtr = ddlQuarter.SelectedValue.ToString();
                Session["currqtr"] = currqtr;
                int tempyear = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
                string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));
                yearForddl = year;
                Session["Year"] = year;
                string M1 = "";
                string M2 = "";
                string M3 = "";
                if (quarter == "Q1")
                {
                    M1 = "Apr";
                    M2 = "May";
                    M3 = "Jun";

                }
                if (quarter == "Q2")
                {
                    M1 = "Jul";
                    M2 = "Aug";
                    M3 = "Sep";

                }
                if (quarter == "Q3")
                {
                    M1 = "Oct";
                    M2 = "Nov";
                    M3 = "Dec";

                }
                if (quarter == "Q4")
                {
                    M1 = "Jan";
                    M2 = "Feb";
                    M3 = "Mar";

                }
                string cmdM1 = "SELECT top 1 * FROM [BEPortalConfig] where [Year]='" + year + "' and [Quarter]='" + quarter + "' and [Month]='" + M1 + "' and [code] like 'Finpulse%' ";
                DataSet dsM1 = service.GetDataSet(cmdM1);
                DataTable dtM1 = dsM1.Tables[0];

                if (dtM1.Rows.Count > 0)
                {
                   // btnM1Actual.Enabled = true;
                }

                string cmdM2 = "SELECT top 1 * FROM [BEPortalConfig] where [Year]='" + year + "' and [Quarter]='" + quarter + "' and [Month]='" + M2 + "' and [code] like 'Finpulse%' ";
                DataSet dsM2 = service.GetDataSet(cmdM2);
                DataTable dtM2 = dsM2.Tables[0];

                if (dtM2.Rows.Count > 0)
                {
                   // btnM2Actual.Enabled = true;
                }
                string cmdM3 = "SELECT top 1 * FROM [BEPortalConfig] where [Year]='" + year + "' and [Quarter]='" + quarter + "' and [Month]='" + M3 + "'  and [code] like 'Finpulse%' ";
                DataSet dsM3 = service.GetDataSet(cmdM3);
                DataTable dtM3 = dsM3.Tables[0];

                if (dtM3.Rows.Count > 0)
                {
                   // btnM3Actual.Enabled = true;
                }

                DataTable dt = new DataTable();

                DataSet combtable = new DataSet();

                //if (currqtr == "Q1'16")
                //{
                //    btnM1Actual.Visible = true;
                //    btnM2Actual.Visible = true;
                //}
                //else
                //{
                //    btnM1Actual.Visible = false;
                //    btnM2Actual.Visible = false;
                //}

                if (quarter != Session["CurrentQuarter"] + "")
                {
                    
                }
               
                combtable = service.GetDMBEData(nso, customerCOde, userID, quarter, year, role);
                dt = combtable.Tables[0];

                string[] selectedColumns = new[] { "txtMasterClientCode", "txtNativeCurrency" };
                DataTable competencydata = new DataView(dt).ToTable(false, selectedColumns);
                Session["DM"] = competencydata;

                if (dt.Rows.Count == 0)
                {
                    //  btnAddMasterCustomer.Visible = false;
                    ImgDownloadToExcel.Visible = false;
                   // btnSave.Visible = false;
                    btnSave2.Visible = false;
                    btnZeroBE.Visible = false;
                    ImgDownloadToExcel.Visible = false;
                     
                    bulk.Visible = false;
                }
                if ((ddlQuarter.SelectedValue == Session["PreviousQuarter"] + ""))
                {
                    // btnAddMasterCustomer.Visible = false;

                    //btnSave.Visible = false;
                    string Month = System.DateTime.Now.Month.ToString();
                   
                    if (Month == "1" || Month == "4" || Month == "7" || Month == "10")
                    {
                        if (CurrentDay > Day)
                        {
                            btnSave2.Visible = false;
                            bulk.Visible = false;
                        }
                        else
                        {
                            btnSave2.Visible = true;
                            bulk.Visible = true;
                        }
                    }
                    else
                    {
                        btnSave2.Visible = false;
                        bulk.Visible = false;
                    }
                    // btnCopy.Visible = false;
                    ImgDownloadToExcel.Visible = true;
                    // btncopydata.Visible = false;
                    btnZeroBE.Visible = false;
                    
                    btnAddMasterCustomer.Visible = false;
                    
                }

                System.Collections.ArrayList list = new System.Collections.ArrayList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string MasterCustomerCode = dt.Rows[i]["txtMasterClientCode"].ToString();
                    string NativeCurrency = dt.Rows[i]["txtNativeCurrency"].ToString();

                    string[] MCC = MasterCustomerCode.Split('-');

                    if (list.Contains(MCC[0].ToString().ToLower() + "-" + NativeCurrency.ToLower()))
                    {
                        
                    }
                    else
                    {
                        SDMRevBE += Convert.ToDecimal(dt.Rows[i]["SDMRevenueBE"].ToString());
                        SDMVolBE += Convert.ToDecimal(dt.Rows[i]["SDMVolumeBE"].ToString());

                        list.Add(MCC[0].ToString().ToLower() + "-" + NativeCurrency.ToLower());
                    }


                    hdSDMRevenueBE.Value = SDMRevBE.ToString();
                    hdSDMVolumeBE.Value = SDMVolBE.ToString();
                  
                    //if (dt.Rows[i]["QuarterVol"].ToString() != "")
                    //{
                    //    ComVolBE += Convert.ToDecimal(dt.Rows[i]["QuarterVol"].ToString());
                    //}
                }

                foreach (DataRow row in dt.Rows)
                {


                    DMMonth1_total += GetDecimalCellValue(row, "fltDMMonth1BE");
                    DMMonth2_total += GetDecimalCellValue(row, "fltDMMonth2BE");
                    DMMonth3_total += GetDecimalCellValue(row, "fltDMMonth3BE");
                    DMQCur_total += GetDecimalCellValue(row, "fltDMQuarterBE");
                    BK1_total += GetDecimalCellValue(row, "fltBK1");
                    BK2_total += GetDecimalCellValue(row, "fltBK2");
                    BK3_total += GetDecimalCellValue(row, "fltBK3");
                    BK4_total += GetDecimalCellValue(row, "fltBK4");
                    Diff += GetDecimalCellValue(row, "fltBKTotal");
                    VolOn1 += GetDecimalCellValue(row, "fltDMMonth1onsite");
                    VolOn2 += GetDecimalCellValue(row, "fltDMMonth2onsite");
                    VolOn3 += GetDecimalCellValue(row, "fltDMMonth3onsite");
                    VolOff1 += GetDecimalCellValue(row, "fltDMMonth1offsite");
                    VolOff2 += GetDecimalCellValue(row, "fltDMMonth2offsite");
                    VolOff3 += GetDecimalCellValue(row, "fltDMMonth3offsite");

                    VolOnTotal += GetDecimalCellValue(row, "fltDMTotalonsite");
                    VolOffTotal += GetDecimalCellValue(row, "fltDMTotaloffsite");
                    VolTotal += GetDecimalCellValue(row, "fltDMQuarterVol");
                    rtbrFinPulse += GetDecimalCellValue(row, "RTBRFinPulse");
                }

              


                ApplyToolTipToMCC(dt);
                Session["data"] = dt;
                grdBEDMView.DataSource = dt;
                grdBEDMView.DataBind();

                // RefreshCOmbo();
                ddlCustomerCode.SelectedIndex = ddlCustomerCode.Items.IndexOf(new ListItem() { Text = customerCOde });

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "loadinggifClose", "loadinggifClose()", true);

            }

            // New logic  - 11May2019
            if (Session["Role"] + "".ToLower() == "pna")
            {
                ImgDownloadToExcel.Visible = true;
            }


            
        }

        private void ApplyToolTipToMCC(DataTable dt)
        {
            var mapping = service.GetNSOCodeDescMapping();
            Func<string, string> funcGetNSODesc = (code) =>
            {
                var item = mapping.FirstOrDefault(k => k.NSOCode == code);
                if (item != null)
                    return item.NSODesc;
                return code;
            };


            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                   //txtMasterCustomerName
                    string mcc = row["txtMasterCustomerName"] + "";
                    string nso = row["NewOffering"] + "";
                    row["txtMasterCustomerName"] = mcc + " - " + funcGetNSODesc(nso);

                }
            }
        }


        protected void ImgDownloadToExcel_Click(object sender, ImageClickEventArgs e)
        {
            string userID = Session["userid"].ToString();
            // LoadComboBox(userID);
            string nso = ddlNSO.Text;
            string customerCOde = ddlCustomerCode.Text;
            string role = Session["Role"] + "";
            string quarter = ddlQuarter.Text.Remove(2);

            int tempyear = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
            string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));
            yearForddl = year;

            DataTable dt = new DataTable();

            DataSet combtable = new DataSet();


            string currentQuarter = Session["quarter"] + "";
            //if (Session["Role"].ToString() == "Admin" || Session["Role"].ToString() == "PnA")
            //{

            //    role = "Anchor";
            //}

            if (Session["Role"].ToString() == "Admin" )
            {

                role = "Admin";
            }

            if (Session["Role"].ToString() == "PnA")
            {

                role = "PnA";
            }
             
            combtable = service.GetDMBEDataExcel(nso, customerCOde, userID, quarter, year, role);
            DownloadExcel(combtable);
        }

        private void DownloadExcel(DataSet ds)
        {
            string role = Session["Role"] + "";
            string userID = Session["userid"].ToString();
            int currentYear = dateTime.Year; //DateTime.Now.Year;
            currentYear = currentYear - 2000;

            string _CurrentQ = string.Empty;
            _CurrentQ = Session["currqtr"] + "";
            string YrSelected = _CurrentQ.Substring(3, 2);
            int yr = Convert.ToInt32(YrSelected);


            string currentQuarter = Session["quarter"] + "";

            string _month1 = string.Empty;
            string _month2 = string.Empty;
            string _month3 = string.Empty;
            if (currentQuarter == "Q4")
            {
                _month1 = "Jan";
                _month2 = "Feb";
                _month3 = "Mar";

                EXCEL(ds, role, userID, yr, _CurrentQ, currentQuarter, _month1, _month2, _month3);

            }
            else if (currentQuarter == "Q1")
            {
                _month1 = "Apr";
                _month2 = "May";
                _month3 = "Jun";
                yr = yr - 1;
                EXCEL(ds, role, userID, yr, _CurrentQ, currentQuarter, _month1, _month2, _month3);

            }
            else if (currentQuarter == "Q2")
            {
                _month1 = "Jul";
                _month2 = "Aug";
                _month3 = "Sep";
                yr = yr - 1;
                EXCEL(ds, role, userID, yr, _CurrentQ, currentQuarter, _month1, _month2, _month3);

            }
            else
            {
                _month1 = "Oct";
                _month2 = "Nov";
                _month3 = "Dec";
                // currentYear = currentYear - 1;
                yr = yr - 1;
                EXCEL(ds, role, userID, yr, _CurrentQ, currentQuarter, _month1, _month2, _month3);

            }



        }

        private void EXCEL(DataSet ds, string role, string userID, int currentYear, string _CurrentQ, string currentQuarter, string _month1, string _month2, string _month3)
        {
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.Columns["MasterClientCode"].ColumnName = "MasterClientCode";
            dt.Columns["DMMailId"].ColumnName = "DMMailId";
            // dt.Columns["SDMMailId"].ColumnName = "SDMMailId";
            dt.Columns["NativeCurrency"].ColumnName = "NativeCurrency";
            dt.Columns["M1ExchangeRate"].ColumnName = _month1 + " ExchangeRate";
            dt.Columns["M2ExchangeRate"].ColumnName = _month2 + " ExchangeRate";
            dt.Columns["M3ExchangeRate"].ColumnName = _month3 + " ExchangeRate";

            dt.Columns["Quarter"].ColumnName = "Quarter";
            dt.Columns["FYYR"].ColumnName = "Financial Year";

            //BE
            dt.Columns["Month1BE"].ColumnName = _month1 + "'" + currentYear + " Rev BE(NC)";
            dt.Columns["Month2BE"].ColumnName = _month2 + "'" + currentYear + " Rev BE(NC)";
            dt.Columns["Month3BE"].ColumnName = _month3 + "'" + currentYear + " Rev BE(NC)";
            dt.Columns["TotalBE"].ColumnName = _CurrentQ + " BE(NC)";
            dt.Columns["A"].ColumnName = "Booked Business (Like RTBR) (NC)";
            dt.Columns["B"].ColumnName = "Extensions or deals already won (NC)";
            dt.Columns["C"].ColumnName = "Proposals already submitted but still open (NC)";
            dt.Columns["D"].ColumnName = "Other opportunities WIP (Not submitted yet) (NC)";
            dt.Columns["Month1BE(NC)"].ColumnName = _month1 + "'" + currentYear + " Rev BE (USD)";
            dt.Columns["Month2BE(NC)"].ColumnName = _month2 + "'" + currentYear + " Rev BE(USD)";
            dt.Columns["Month3BE(NC)"].ColumnName = _month3 + "'" + currentYear + " Rev BE(USD)";
            dt.Columns["TotalBE(NC)"].ColumnName = _CurrentQ + " BE(USD)";
            dt.Columns["A(NC)"].ColumnName = "Booked Business (Like RTBR) (USD)";
            dt.Columns["B(NC)"].ColumnName = "Extensions or deals already won (USD)";
            dt.Columns["C(NC)"].ColumnName = "Proposals already submitted but still open (USD)";
            dt.Columns["D(NC)"].ColumnName = "Other opportunities WIP (Not submitted yet) (USD)";
            dt.Columns["SDMRevenueBE"].ColumnName = "SDMRevenueBE (NC)";
            dt.Columns["SDMRevenueBE(USD)"].ColumnName = "SDMRevenueBE (USD)";


            dt.Columns["RTBRM1NC"].ColumnName = "RTBR(NC) " + _month1 + "'" + currentYear;
            dt.Columns["RTBRM2NC"].ColumnName = "RTBR(NC) " + _month2 + "'" + currentYear;
            dt.Columns["RTBRM3NC"].ColumnName = "RTBR(NC) " + _month3 + "'" + currentYear;
            dt.Columns["FPM1NC"].ColumnName = "FinPulse(NC) " + _month1 + "'" + currentYear;
            dt.Columns["FPM2NC"].ColumnName = "FinPulse(NC) " + _month2 + "'" + currentYear;
            dt.Columns["FPM3NC"].ColumnName = "FinPulse(NC) " + _month3 + "'" + currentYear;

            dt.Columns["RTBR/FinPulse"].ColumnName = "RTBR/FinPulse(NC)";

            dt.Columns["RTBRM1USD"].ColumnName = "RTBR(USD) " + _month1 + "'" + currentYear;
            dt.Columns["RTBRM2USD"].ColumnName = "RTBR(USD) " + _month2 + "'" + currentYear;
            dt.Columns["RTBRM3USD"].ColumnName = "RTBR(USD) " + _month3 + "'" + currentYear;
            dt.Columns["FPM1USD"].ColumnName = "FinPulse(USD) " + _month1 + "'" + currentYear;
            dt.Columns["FPM2USD"].ColumnName = "FinPulse(USD) " + _month2 + "'" + currentYear;
            dt.Columns["FPM3USD"].ColumnName = "FinPulse(USD) " + _month3 + "'" + currentYear;


            dt.Columns["RTBR/FinPulse(USD)"].ColumnName = "RTBR/FinPulse(USD)";

            dt.Columns["PBSM1On"].ColumnName = "PBS " + _month1 + "'" + currentYear + " Onsite";
            dt.Columns["PBSM1Off"].ColumnName = "PBS " + _month1 + "'" + currentYear + " Offsite";
            dt.Columns["PBSM2On"].ColumnName = "PBS " + _month2 + "'" + currentYear + " Onsite";
            dt.Columns["PBSM2Off"].ColumnName = "PBS " + _month2 + "'" + currentYear + " Offsite";
            dt.Columns["PBSM3On"].ColumnName = "PBS " + _month3 + "'" + currentYear + " Onsite";
            dt.Columns["PBSM3Off"].ColumnName = "PBS " + _month3 + "'" + currentYear + " Offsite";
            dt.Columns["PBSOnTot"].ColumnName = "PBS Total Onsite";
            dt.Columns["PBSOffTot"].ColumnName = "PBS Total Offsite";

            dt.Columns["BilledM1On"].ColumnName = "Billed " + _month1 + "'" + currentYear + " Onsite";
            dt.Columns["BilledM1Off"].ColumnName = "Billed " + _month1 + "'" + currentYear + " Offsite";
            dt.Columns["BilledM2On"].ColumnName = "Billed " + _month2 + "'" + currentYear + " Onsite";
            dt.Columns["BilledM2Off"].ColumnName = "Billed " + _month2 + "'" + currentYear + " Offsite";
            dt.Columns["BilledM3On"].ColumnName = "Billed " + _month3 + "'" + currentYear + " Onsite";
            dt.Columns["BilledM3Off"].ColumnName = "Billed " + _month3 + "'" + currentYear + " Offsite";
            dt.Columns["BilledOnTot"].ColumnName = "Billed Total Onsite";
            dt.Columns["BilledOffTot"].ColumnName = "Billed Total Offsite";
            dt.Columns["BilledTot"].ColumnName = "Billed Total";

            dt.Columns["AlconM1On"].ColumnName = "Alcon " + _month1 + "'" + currentYear + " Onsite";
            dt.Columns["AlconM1Off"].ColumnName = "Alcon " + _month1 + "'" + currentYear + " Offsite";
            dt.Columns["AlconM2On"].ColumnName = "Alcon " + _month2 + "'" + currentYear + " Onsite";
            dt.Columns["AlconM2Off"].ColumnName = "Alcon " + _month2 + "'" + currentYear + " Offsite";
            dt.Columns["AlconM3On"].ColumnName = "Alcon " + _month3 + "'" + currentYear + " Onsite";
            dt.Columns["AlconM3Off"].ColumnName = "Alcon " + _month3 + "'" + currentYear + " Offsite";
            dt.Columns["AlconOnTot"].ColumnName = "Alcon Total Onsite";
            dt.Columns["AlconOffTot"].ColumnName = "Alcon Total Offsite";

            dt.Columns["M1On"].ColumnName = "BE Vol " + _month1 + "'" + currentYear + " (ON)";
            dt.Columns["M1Off"].ColumnName = "BE Vol " + _month1 + "'" + currentYear + " (OFF)";
            dt.Columns["M2On"].ColumnName = "BE Vol " + _month2 + "'" + currentYear + " (ON)";
            dt.Columns["M2Off"].ColumnName = "BE Vol " + _month2 + "'" + currentYear + " (OFF)";
            dt.Columns["M3On"].ColumnName = "BE Vol " + _month3 + "'" + currentYear + " (ON)";
            dt.Columns["M3Off"].ColumnName = "BE Vol " + _month3 + "'" + currentYear + " (OFF)";

            dt.Columns["PBSM1"].ColumnName = "PBS " + _month1;
            dt.Columns["PBSM2"].ColumnName = "PBS " + _month2;
            dt.Columns["PBSM3"].ColumnName = "PBS " + _month3;
            dt.Columns["PBSQtr"].ColumnName = "PBS " + _CurrentQ;


            dt.Columns["AlconM1"].ColumnName = "Alcon " + _month1;
            dt.Columns["AlconM2"].ColumnName = "Alcon " + _month2;
            dt.Columns["AlconM3"].ColumnName = "Alcon " + _month3;
            dt.Columns["AlconQtr"].ColumnName = "Alcon " + _CurrentQ;


            dt.Columns["BilledM1"].ColumnName = "Billed Month " + _month1;
            dt.Columns["BilledM2"].ColumnName = "Billed Month " + _month2;
            dt.Columns["BilledM3"].ColumnName = "Billed Month " + _month3;

            dt.Columns["fltDMMonth1Total"].ColumnName = "BE Vol " + _month1 + "'" + currentYear + "";
            dt.Columns["fltDMMonth2Total"].ColumnName = "BE Vol " + _month2 + "'" + currentYear + "";
            dt.Columns["fltDMMonth3Total"].ColumnName = "BE Vol " + _month3 + "'" + currentYear + "";

            dt.Columns["On"].ColumnName = "BE Vol " + _CurrentQ + " (ON)";
            dt.Columns["Off"].ColumnName = "BE Vol " + _CurrentQ + " (OFF)";
            dt.Columns["TotalVol"].ColumnName = "Total BE Vol " + _CurrentQ;
            dt.Columns["SDMVolumeBE"].ColumnName = "SDMVolumeBE(" + _CurrentQ + ")";

            // dt.Columns["BilledMonths/Alcon Effort"].ColumnName = "BilledMonths/Alcon Effort**";
            dt.Columns["Modified On"].ColumnName = "DM Modified On";
            dt.Columns["Remarks"].ColumnName = "DM Reasons for Rev & Vol changes";
            dt.Columns["newOffering"].ColumnName = "New Offering";
            //dt.Columns["ServiceLine"].ColumnName = "ServiceLine";
            DataSet dsreturn = new DataSet();
            dsreturn.Tables.Add(dt.Copy());
            //dsreturn.Tables.Add(ds.Tables[1].Copy());
            gvDMExcel.Visible = true;
            gvDMExcel.DataSource = dsreturn.Tables[0];
            gvDMExcel.DataBind();
            try
            {
                string Filename1 = currentQuarter + "_" + role + "_" + "Digital_BEData_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                string Sheetname = currentQuarter + "_" + role + "_" + "Digital_BEData_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm");
                DataTable dt0 = new DataTable();
                dt0 = ds.Tables[0];
                var tblProjectDownload0 = dt0;
                string folder = PhysicalPath_DownloadFiles;
                var MyDir = new DirectoryInfo( folder);

                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == Filename1) != null)
                    System.IO.File.Delete(MyDir.FullName + Filename1);

                string pathAndFile = GetPathAndFileName(PhysicalPath_DownloadFiles, Filename1);
                FileInfo file = new FileInfo(pathAndFile);
               
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;
                ExcelWorksheet ws1;
                int rowcountSheet0 = tblProjectDownload0.Rows.Count;
                int colcountSheet0 = tblProjectDownload0.Columns.Count;
                if (tblProjectDownload0 == null || tblProjectDownload0.Rows.Count == 0)
                {
                    Session["key"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"script", "<script language=JavaScript>alert('No Data to download!');</script>",true);
                    return;
                }
                else
                {
                    string user = HttpContext.Current.User.Identity.Name;
                    string[] userids = user.Split('\\');
                    if (userids.Length == 2)
                    {
                        user = userids[1];
                    }

                    ws = pck.Workbook.Worksheets.Add(Sheetname);
                    ws.Cells["A1"].LoadFromDataTable(tblProjectDownload0, true);
                    var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

               
                    pck.SaveAs(file);
                    pck.Dispose();
                    ReleaseObject(pck);
                  
                    ReleaseObject(ws);
                    GenerateReport(Filename1);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Error encountered. Please try again later.";
                Session.Add("Msg", lblmsg.Text);
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
            gvDMExcel.Visible = false;
        }


        void GenerateReport(string fname)
        {


            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            VBIDE.VBComponent oModule;
            //try
            {

                string pathAndFile = GetPathAndFileName(PhysicalPath_DownloadFiles, fname);

                String sCode;
                Object oMissing = System.Reflection.Missing.Value;
                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();

                oBook = oExcel.Workbooks.
                    Open(pathAndFile, 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
             

                oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
                sCode = "sub Macro()\r\n" +
                    GetMacroText("ExcelDownload.txt") + 
                     
                        "\nend sub";
                oModule.CodeModule.AddFromString(sCode);
                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "Macro" });
                //Adding permission to excel file//
             
                //oBook.Permission.Enabled = true;
                //oBook.Permission.RemoveAll();
                //string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                //DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                //DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                //UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                
                //userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////

                oBook.Save();
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                GC.Collect();


                downloadExcel(fname);

            }

        }

        

        public void ReleaseObject(object o)
        {
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch (Exception) { }
            finally { o = null; }
        }

        private void downloadExcel(string fileName)
        {
            Session["Key"] = fileName;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "loadinggifClose", "loadinggifClose()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "gridviewScroll", "gridviewScroll();", true);
            iframe.Attributes.Add("src", "Download.aspx");
          
        }
        protected void btnZeroBE_Click(object sender, EventArgs e)
        {
            try
            {
                int rowcount = 0;
                int norecord = 0;
                for (int i = 0; i < grdBEDMView.Rows.Count; i++)
                {
                    if (((CheckBox)grdBEDMView.Rows[i].FindControl("chkRow")).Checked == true)
                    {
                        norecord++;
                        int beID = Convert.ToInt32(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnfld") as HiddenField).Value);
                        decimal M1BE = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth1")).Text.ToString());
                        decimal M2BE = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth2")).Text.ToString());
                        decimal M3BE = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth3")).Text.ToString());
                        decimal BK1 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth1")).Text.ToString());
                        decimal BK2 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth2")).Text.ToString());
                        decimal BK3 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth3")).Text.ToString());
                        decimal BK4 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth4")).Text.ToString());
                        decimal VolOn1 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth1")).Text.ToString());
                        decimal VolOn2 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth2")).Text.ToString());
                        decimal VolOn3 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth3")).Text.ToString());
                        decimal VolOff1 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth1")).Text.ToString());
                        decimal VolOff2 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth2")).Text.ToString());
                        decimal VolOff3 = Convert.ToDecimal(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth3")).Text.ToString());
                        decimal BilledFinpulse = Convert.ToDecimal(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnBilledFinpulse") as HiddenField).Value);
                        decimal EffortMonths = Convert.ToDecimal(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnEffortMonths") as HiddenField).Value);
                        decimal AlconEffort = Convert.ToDecimal(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnAlconEffort") as HiddenField).Value);
                        decimal PBSEffort = Convert.ToDecimal(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnPBSEffort") as HiddenField).Value);

                        // string remarks = ((TextBox)grdBEDMView.Rows[i].FindControl("txtVoldmRemarks")).Text.ToString();
                        decimal actulartbr;
                        if (((HyperLink)grdBEDMView.Rows[i].FindControl("lnkbtnrtbr")).Text.ToString() == "")
                        {
                            actulartbr = default(decimal);
                        }
                        else
                        {
                            actulartbr = Convert.ToDecimal(((HyperLink)grdBEDMView.Rows[i].FindControl("lnkbtnrtbr")).Text.ToString());
                        }
                        if (M1BE.Equals(0) && M2BE.Equals(0) && M3BE.Equals(0) && BK1.Equals(0) && BK2.Equals(0) 
                            && BK3.Equals(0) && BK4.Equals(0) && VolOff1.Equals(0) && VolOff2.Equals(0) && VolOff3.Equals(0)
                            && VolOn1.Equals(0) && VolOn2.Equals(0) && VolOn3.Equals(0) && actulartbr.Equals(0) && BilledFinpulse.Equals(0)
                            && EffortMonths.Equals(0) && AlconEffort.Equals(0) && PBSEffort.Equals(0))
                        {
                            string cmdtext = "delete from EAS_BEData_DM_NSO where intBEId=" + beID + "";
                            DataSet combtable = new DataSet();

                            combtable = service.GetDataSet(cmdtext);
                            rowcount++;
                        }

                    }


                }
                if (norecord == 0)
                {
                    //btnSearch_Click(null, null);
                    lblmsg1.Visible = true;
                    lblmsg1.ForeColor = Color.Red;
                    lblmsg1.Text = "Please select some record(s) for deleting ";
                
                }
                else
                {
                    if (rowcount > 0)
                    {



                        btnSearch_Click(null, null);
                        lblmsg1.Visible = true;
                        lblmsg1.ForeColor = Color.Green;
                        lblmsg1.Text = rowcount + " Zero BE/Projections/Actuals/Alcon/PBS record(s) have been deleted ";
                    }
                    else
                    {


                        //btnSearch_Click(null, null);
                        lblmsg1.Visible = true;
                        lblmsg1.ForeColor = Color.Red;
                        lblmsg1.Text = "Only Zero BE/Projections/Actuals/Alcon/PBS record(s) can be deleted";
                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "gridviewScroll", "gridviewScroll();", true);
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }


        protected void lnkbtnDMTotal_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_DM_Click(object sender, EventArgs e)
        {

            int nochange = 0;
            int rowcount = 0;
            int row = 0;
            lblmsg.Text = "";
            lblmsg3.Text = "";

            string CurrentQtrBool = string.Empty;
            string MonthType = string.Empty;

            GetCurrentQtrAndMonthType(ref CurrentQtrBool, ref MonthType);

            string username = System.IO.Path.GetFileName(User.Identity.Name.ToString().ToUpper());
            string date = DateTime.Now.ToString();
            try
            {
                for (int i = 0; i < grdBEDMView.Rows.Count; i++)
                {

                    string mastercustcode = ((Label)grdBEDMView.Rows[i].FindControl("lblMCC")).Text.ToString();
                    string NC = ((Label)grdBEDMView.Rows[i].FindControl("lblNativeCurrency")).Text.ToString();
                    int beID = Convert.ToInt32(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnfld") as HiddenField).Value);
                    decimal M1BE = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth1")).Text.ToString());
                    decimal M2BE = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth2")).Text.ToString());
                    decimal M3BE = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth3")).Text.ToString());
                    decimal BK1 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth1")).Text.ToString());
                    decimal BK2 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth2")).Text.ToString());
                    //decimal BK3 =  Decimal.Parse(((HyperLink)grdBESDMView.Rows[i].FindControl("lnkbtnBcktC")).Text.ToString());
                    //decimal BK4 =  Decimal.Parse(((HyperLink)grdBESDMView.Rows[i].FindControl("lnkbtnBcktD")).Text.ToString());
                    decimal BK3 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth3")).Text.ToString());
                    decimal BK4 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth4")).Text.ToString());
                    decimal VolOn1 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth1")).Text.ToString());
                    decimal VolOn2 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth2")).Text.ToString());
                    decimal VolOn3 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth3")).Text.ToString());
                    decimal VolOff1 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth1")).Text.ToString());
                    decimal VolOff2 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth2")).Text.ToString());
                    decimal VolOff3 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth3")).Text.ToString());
                    string remarks = ((TextBox)grdBEDMView.Rows[i].FindControl("txtVolsdmRemarks")).Text.ToString();


               

                    string data = "select * from [EAS_BEData_DM_NSO] where [intBEId]=" + beID + "";
                    DataSet ds = service.GetDataSet(data);
                    DataTable dt1 = ds.Tables[0];

                    decimal M1BE_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth1BE"].ToString(), NumberStyles.Any);
                    decimal M2BE_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth2BE"].ToString(), NumberStyles.Any);
                    decimal M3BE_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth3BE"].ToString(), NumberStyles.Any);
                    decimal BK1_BK = Decimal.Parse(dt1.Rows[0]["fltBK1"].ToString(), NumberStyles.Any);
                    decimal BK2_BK = Decimal.Parse(dt1.Rows[0]["fltBK2"].ToString(), NumberStyles.Any);
                    decimal BK3_BK = Decimal.Parse(dt1.Rows[0]["fltBK3"].ToString(), NumberStyles.Any);
                    decimal BK4_BK = Decimal.Parse(dt1.Rows[0]["fltBK4"].ToString(), NumberStyles.Any);
                    decimal VolOn1_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth1onsite"].ToString(), NumberStyles.Any);
                    decimal VolOn2_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth2onsite"].ToString(), NumberStyles.Any);
                    decimal VolOn3_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth3onsite"].ToString(), NumberStyles.Any);
                    decimal VolOff1_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth1offsite"].ToString(), NumberStyles.Any);
                    decimal VolOff2_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth2offsite"].ToString(), NumberStyles.Any);
                    decimal VolOff3_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth3offsite"].ToString(), NumberStyles.Any);
                    string remarks_BK = dt1.Rows[0]["txtDMBERemarks"].ToString();

                    if (M1BE == M1BE_BK && M2BE == M2BE_BK && M3BE == M3BE_BK && BK1 == BK1_BK && BK2 == BK2_BK && BK3 == BK3_BK && BK4 == BK4_BK && VolOff1 == VolOff1_BK && VolOff2 == VolOff2_BK && VolOff3 == VolOff3_BK && VolOn1 == VolOn1_BK && VolOn2 == VolOn2_BK && VolOn3 == VolOn3_BK && remarks == remarks_BK)
                    {
                        nochange++;
                    }
                    else
                    { 
                        string cmdtext = "exec [EAS_spBEUpdateData_DM_NSO] " + beID + "," + M1BE + "," + M2BE + "," + M3BE + "," + BK1 + "," + BK2 + "," + BK3 + "," + BK4 + "," + VolOn1 + "," + VolOn2 + "," + VolOn3 + "," + VolOff1 + "," + VolOff2 + "," + VolOff3 + ",'" + remarks + "','" + username + "','" + date + "','" + MonthType + "','" + CurrentQtrBool + "'";
                        DataSet combtable = new DataSet();
                        combtable = service.GetDataSet(cmdtext);
                    }
                }
                if (nochange == grdBEDMView.Rows.Count)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);

                    lblmsg1.Visible = true;
                    lblmsg1.Text = " No changes made.";
                    lblmsg1.ForeColor = Color.Green;
                    lblmsg1.Visible = true;

                }
                else
                {

                    btnSearch_Click(null, null);
                  
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);

                    lblmsg1.Visible = true;
                    lblmsg1.Text = "Data saved successfully";
                    lblmsg1.ForeColor = Color.Green;
                }
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "gridviewScroll", "gridviewScroll();", true);
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

        private void GetCurrentQtrAndMonthType(ref string CurentQtrBool, ref string MonthType)
        {
            string currentQtr = fq.GetQuarter("current");
            string QtrSelected = ViewState["Qtr"].ToString();
            if (QtrSelected == currentQtr)
            {
                string Month = System.DateTime.Now.Month.ToString();
                CurentQtrBool = "True";


                if (Month == "1" || Month == "4" || Month == "7" || Month == "10")
                {
                    MonthType = "First";

                }
                else if (Month == "2" || Month == "5" || Month == "8" || Month == "11")
                {
                    MonthType = "Second";

                }
                else if (Month == "3" || Month == "6" || Month == "9" || Month == "12")
                {
                    MonthType = "Third";
                }
            }
            else
            {
                CurentQtrBool = "False";
            }
        }

        //protected void btnSave_DM_Click(object sender, EventArgs e)
        //{

        //    int nochange = 0;
        //    int rowcount = 0;
        //    int row = 0;
        //    lblmsg.Text = "";
        //    lblmsg3.Text = "";
        //    string username = System.IO.Path.GetFileName(User.Identity.Name.ToString().ToUpper());
        //    string date = DateTime.Now.ToString();
        //    try
        //    {
        //        for (int i = 0; i < grdBEDMView.Rows.Count; i++)
        //        {




        //            string mastercustcode = ((Label)grdBEDMView.Rows[i].FindControl("lblMCC")).Text.ToString();
        //            string NC = ((Label)grdBEDMView.Rows[i].FindControl("lblNativeCurrency")).Text.ToString();
        //            int beID = Convert.ToInt32(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnfld") as HiddenField).Value);
        //            decimal M1BE = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth1")).Text.ToString()), 1);
        //            decimal M2BE = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth2")).Text.ToString()), 1);
        //            decimal M3BE = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth3")).Text.ToString()), 1);
        //            decimal BK1 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth1")).Text.ToString()), 1);
        //            decimal BK2 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth2")).Text.ToString()), 1);
        //            //decimal BK3 = Math.Round( Decimal.Parse(((HyperLink)grdBESDMView.Rows[i].FindControl("lnkbtnBcktC")).Text.ToString());
        //            //decimal BK4 = Math.Round( Decimal.Parse(((HyperLink)grdBESDMView.Rows[i].FindControl("lnkbtnBcktD")).Text.ToString());
        //            decimal BK3 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth3")).Text.ToString()), 1);
        //            decimal BK4 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth4")).Text.ToString()), 1);
        //            decimal VolOn1 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth1")).Text.ToString()), 1);
        //            decimal VolOn2 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth2")).Text.ToString()), 1);
        //            decimal VolOn3 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth3")).Text.ToString()), 1);
        //            decimal VolOff1 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth1")).Text.ToString()), 1);
        //            decimal VolOff2 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth2")).Text.ToString()), 1);
        //            decimal VolOff3 = Math.Round(Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth3")).Text.ToString()), 1);
        //            string remarks = ((TextBox)grdBEDMView.Rows[i].FindControl("txtVolsdmRemarks")).Text.ToString();




        //            string data = "select * from [EAS_BEData_DM_NSO] where [intBEId]=" + beID + "";
        //            DataSet ds = service.GetDataSet(data);
        //            DataTable dt1 = ds.Tables[0];

        //            decimal M1BE_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth1BE"].ToString(), NumberStyles.Any), 1);
        //            decimal M2BE_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth2BE"].ToString(), NumberStyles.Any), 1);
        //            decimal M3BE_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth3BE"].ToString(), NumberStyles.Any), 1);
        //            decimal BK1_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltBK1"].ToString(), NumberStyles.Any), 1);
        //            decimal BK2_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltBK2"].ToString(), NumberStyles.Any), 1);
        //            decimal BK3_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltBK3"].ToString(), NumberStyles.Any), 1);
        //            decimal BK4_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltBK4"].ToString(), NumberStyles.Any), 1);
        //            decimal VolOn1_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth1onsite"].ToString(), NumberStyles.Any), 1);
        //            decimal VolOn2_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth2onsite"].ToString(), NumberStyles.Any), 1);
        //            decimal VolOn3_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth3onsite"].ToString(), NumberStyles.Any), 1);
        //            decimal VolOff1_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth1offsite"].ToString(), NumberStyles.Any), 1);
        //            decimal VolOff2_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth2offsite"].ToString(), NumberStyles.Any), 1);
        //            decimal VolOff3_BK = Math.Round(Decimal.Parse(dt1.Rows[0]["fltDMMonth3offsite"].ToString(), NumberStyles.Any), 1);
        //            string remarks_BK = dt1.Rows[0]["txtDMBERemarks"].ToString();



        //            if (M1BE == M1BE_BK && M2BE == M2BE_BK && M3BE == M3BE_BK && BK1 == BK1_BK && BK2 == BK2_BK && BK3 == BK3_BK && BK4 == BK4_BK && VolOff1 == VolOff1_BK && VolOff2 == VolOff2_BK && VolOff3 == VolOff3_BK && VolOn1 == VolOn1_BK && VolOn2 == VolOn2_BK && VolOn3 == VolOn3_BK && remarks == remarks_BK)
        //            {
        //                nochange++;

        //            }
        //            else
        //            {


        //                string cmdtext = "exec [EAS_spBEUpdateData_DM] " + beID + "," + M1BE + "," + M2BE + "," + M3BE + "," + BK1 + "," + BK2 + "," + BK3 + "," + BK4 + "," + VolOn1 + "," + VolOn2 + "," + VolOn3 + "," + VolOff1 + "," + VolOff2 + "," + VolOff3 + ",'" + remarks + "','" + username + "','" + date + "'";
        //                DataSet combtable = new DataSet();
        //                combtable = service.GetDataSet(cmdtext);
        //            }
        //        }
        //        if (nochange == grdBEDMView.Rows.Count)
        //        {
        //            ClientScript.RegisterStartupScript(this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);
        //            ClientScript.RegisterStartupScript(this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);

        //            lblmsg1.Visible = true;
        //            lblmsg1.Text = " No changes made.";
        //            lblmsg1.ForeColor = Color.Green;
        //            lblmsg1.Visible = true;

        //        }
        //        else
        //        {

        //            btnSearch_Click(null, null);
        //            ClientScript.RegisterStartupScript(this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);

        //            lblmsg1.Visible = true;
        //            lblmsg1.Text = "Data saved successfully";
        //            lblmsg1.ForeColor = Color.Green;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }

        //}

        private decimal GetCellValue(DataRow currentRow, string columnName)
        {
            decimal returnValue = default(decimal);
            try
            {

                string value = (currentRow[columnName] + "").Trim();
                returnValue = value.Length == 0 ? default(decimal) : Convert.ToDecimal(value);

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
            return returnValue;
        }


        private void Alert(string message)
        {

            try
            {
                Page page = HttpContext.Current.CurrentHandler as Page;

                // string script = string.Format("alert('{0}');", message);

                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {

                    // page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);

                    page.RegisterClientScriptBlock("alert", "<script type=\"text/javascript\">alert('" + message + "');</script>");

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



        private decimal GetDecimalValueofThisCell(GridViewRow row, int cellIndex)
        {
            decimal value = default(decimal);
            try
            {

                bool isDataBound = row.Cells[cellIndex].Controls.Count == 0;
                if (isDataBound)
                {
                    var text = row.Cells[cellIndex].Text.Trim().Replace("&nbsp;", "");
                    value = text.Length == 0 ? default(decimal) : Convert.ToDecimal(text);
                }
                else
                {
                    foreach (Control ctrl in row.Cells[cellIndex].Controls)
                    {
                        if (ctrl is TextBox)
                        {
                            string text = (ctrl as TextBox).Text.Trim().Replace("&nbsp;", "");
                            value = text.Trim().Length == 0 ? default(decimal) : Convert.ToDecimal(text);
                        }
                        if (ctrl is Label)
                        {
                            string text = (ctrl as Label).Text.Trim().Replace("&nbsp;", "");
                            value = text.Trim().Length == 0 ? default(decimal) : Convert.ToDecimal(text);
                        }
                    }

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
            return value;
        }

       // Application app = new Application();

        protected void PopUp(string msg)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "777", "alert('" + msg + "');", true);
        }

      



        //protected void lnkbtnDMTotal_Click(object sender, EventArgs e)
        //{
        //    lblSDMMailID.Visible = false;
        //    LinkButton lnk = sender as LinkButton;
        //    GridViewRow grdrow = lnk.Parent.Parent as GridViewRow;

        //    List<DMDetailsPopUp> dm = new List<DMDetailsPopUp>();
        //    //int Beid = Convert.ToInt32(grdrow.Cells[23].Text + "");
        //    string beID = (grdrow.FindControl("hdnfld") as HiddenField).Value + "";
        //    string role = Session["Role"] + "";
        //    string sdmtotal = string.Empty;


        //    if (role.ToLower() == "dm" || role.ToLower() == "others")
        //    {
        //        dm = service.GetBEPopUpDMValuesRevforDMView(beID);
        //        //sdmtotal = service.GetBEPopUpDMValuesRevforSDMTotalView(beID);
        //        DataTable dt = new DataTable();
        //        dt = service.GetBEPopUpDMValuesRevforSDMTotalView(beID);
        //        sdmtotal = dt.Rows[0][0].ToString();
        //        //sdmMailID = dt.Rows[0][1].ToString();
        //    }
        //    else
        //    {
        //        dm = service.GetBEPopUpDMValuesRevforSDMView(beID);
        //        sdmtotal = grdrow.Cells[11].Text + "";
        //    }

        //    lblSDMTotal.Text = "Total SDM BE : " + sdmtotal;

        //    if (dm.Count > 0)
        //    {



        //        int year = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
        //        int nxtyr;

        //        DateTime dt = DateTime.Today;
        //        string qtr = ddlQuarter.Text.Remove(2);
        //        if (qtr == "Q3")
        //        {
        //            TextBox1.Text = "10/12/" + year;
        //        }
        //        else if (qtr == "Q2")
        //        {
        //            TextBox1.Text = "7/30/" + year;
        //        }
        //        else if (qtr == "Q1")
        //        {
        //            TextBox1.Text = "5/9/" + year;
        //        }
        //        else if (qtr == "Q4")
        //        {
        //            nxtyr = year + 1;
        //            TextBox1.Text = "1/11/" + nxtyr;
        //        }

        //        dt = Convert.ToDateTime(TextBox1.Text);


        //        //DateTime dt = DateTime.Today;
        //        int currentMonth = dateTime.Month;
        //        int currentYear = dt.Year;
        //        int previousYear = currentYear - 1;
        //        int nextYear = currentYear + 1;
        //        string _PreviousQ = string.Empty;
        //        string _CurrentQ = string.Empty;
        //        string _NextQ = string.Empty;
        //        //string currentQuarter = GetCurrentQ();
        //        string currentQuarter = ddlQuarter.Text.Remove(2);
        //        currentYear = currentYear - 2000;

        //        string previousQuarter = GetPreviousQuarter();
        //        string nextQuarter = GetNextQuarter();

        //        string _month1 = string.Empty;
        //        string _month2 = string.Empty;
        //        string _month3 = string.Empty;
        //        if (currentQuarter == "Q4")
        //        {
        //            _month1 = "Jan";
        //            _month2 = "Feb";
        //            _month3 = "Mar";
        //        }
        //        else if (currentQuarter == "Q1")
        //        {
        //            _month1 = "Apr";
        //            _month2 = "May";
        //            _month3 = "Jun";
        //        }
        //        else if (currentQuarter == "Q2")
        //        {
        //            _month1 = "Jul";
        //            _month2 = "Aug";
        //            _month3 = "Sep";
        //        }
        //        else
        //        {
        //            _month1 = "Oct";
        //            _month2 = "Nov";
        //            _month3 = "Dec";
        //        }
        //        //dm
        //        string mon1 = " " + _month1 + "'" + currentYear + " ";
        //        string mon2 = " " + _month2 + "'" + currentYear + " ";
        //        string mon3 = " " + _month3 + "'" + currentYear + " ";



        //        var month1total = dm.Where(k => (k.DMMonth1 + "").Trim().Length > 0).Select(k => k.DMMonth1).Sum(k => Convert.ToDouble(k));
        //        var month1tota2 = dm.Where(k => (k.DMMonth2 + "").Trim().Length > 0).Select(k => k.DMMonth2).Sum(k => Convert.ToDouble(k));
        //        var month1tota3 = dm.Where(k => (k.DMMonth3 + "").Trim().Length > 0).Select(k => k.DMMonth3).Sum(k => Convert.ToDouble(k));

        //        var month1totalall = dm.Where(k => (k.total + "").Trim().Length > 0).Select(k => k.total).Sum(k => Convert.ToDouble(k));



        //    }

        //}

        protected void grdBEDMView_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            //ModalPopupExtender2.Enabled = true;
            //ModalPopupExtender2.Show();
            btnSearch_Click(null, null);
          
        }

        
        public class MenuValue1
        {
            public string Level1 { get; set; }
            public string Level2 { get; set; }
            public string Level3 { get; set; }
            public string Level4 { get; set; }
        }
        public class MenuAttributes1
        {
            public string key { get; set; }
            public string Text { get; set; }
            public string URL { get; set; }
        }



        public decimal SDMBE { get; set; }

        protected void btnAddMasterCustomer_Click1(object sender, EventArgs e)
        {
            lblmsg.Visible = false;
            lblmsg1.Visible = false;
            lblmsg3.Visible = false;
            int nochange = 0;
            for (int i = 0; i < grdBEDMView.Rows.Count; i++)
            {


                string NC = ((Label)grdBEDMView.Rows[i].FindControl("lblNativeCurrency")).Text.ToString();
                string mastercustcode = ((Label)grdBEDMView.Rows[i].FindControl("lblMCC")).Text.ToString();
                int beID = Convert.ToInt32(((HiddenField)grdBEDMView.Rows[i].FindControl("hdnfld") as HiddenField).Value);
                decimal M1BE = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth1")).Text.ToString(), NumberStyles.Any);
                decimal M2BE = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth2")).Text.ToString(), NumberStyles.Any);
                decimal M3BE = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtDMMonth3")).Text.ToString(), NumberStyles.Any);
                decimal BK1 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth1")).Text.ToString(), NumberStyles.Any);
                decimal BK2 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth2")).Text.ToString(), NumberStyles.Any);
                //decimal BK3 = Convert.ToDecimal(((HyperLink)grdBEDMView.Rows[i].FindControl("lnkbtnBcktC")).Text.ToString());
                decimal BK3 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth3")).Text.ToString(), NumberStyles.Any);
                //decimal BK4 = Convert.ToDecimal(((HyperLink)grdBEDMView.Rows[i].FindControl("lnkbtnBcktD")).Text.ToString());
                decimal BK4 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtBKMonth4")).Text.ToString(), NumberStyles.Any);
                decimal VolOn1 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth1")).Text.ToString(), NumberStyles.Any);
                decimal VolOn2 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth2")).Text.ToString(), NumberStyles.Any);
                decimal VolOn3 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOnMonth3")).Text.ToString(), NumberStyles.Any);
                decimal VolOff1 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth1")).Text.ToString(), NumberStyles.Any);
                decimal VolOff2 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth2")).Text.ToString(), NumberStyles.Any);
                decimal VolOff3 = Decimal.Parse(((TextBox)grdBEDMView.Rows[i].FindControl("txtVolOffMonth3")).Text.ToString(), NumberStyles.Any);

                string remarks;
                if (((TextBox)grdBEDMView.Rows[i].FindControl("txtVolsdmRemarks")).Text.ToString() != "")
                {
                    remarks = ((TextBox)grdBEDMView.Rows[i].FindControl("txtVolsdmRemarks")).Text.ToString();
                }
                else
                {
                    remarks = "";
                }

                string data = "select * from [EAS_BEData_DM_NSO] where [intBEId]=" + beID + "";
                DataSet ds = service.GetDataSet(data);
                DataTable dt1 = ds.Tables[0];
                decimal M1BE_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth1BE"].ToString(), NumberStyles.Any);
                decimal M2BE_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth2BE"].ToString(), NumberStyles.Any);
                decimal M3BE_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth3BE"].ToString(), NumberStyles.Any);
                decimal BK1_BK = Decimal.Parse(dt1.Rows[0]["fltBK1"].ToString(), NumberStyles.Any);
                decimal BK2_BK = Decimal.Parse(dt1.Rows[0]["fltBK2"].ToString(), NumberStyles.Any);
                decimal BK3_BK = Decimal.Parse(dt1.Rows[0]["fltBK3"].ToString(), NumberStyles.Any);
                decimal BK4_BK = Decimal.Parse(dt1.Rows[0]["fltBK4"].ToString(), NumberStyles.Any);
                decimal VolOn1_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth1onsite"].ToString(), NumberStyles.Any);
                decimal VolOn2_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth2onsite"].ToString(), NumberStyles.Any);
                decimal VolOn3_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth3onsite"].ToString(), NumberStyles.Any);
                decimal VolOff1_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth1offsite"].ToString(), NumberStyles.Any);
                decimal VolOff2_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth2offsite"].ToString(), NumberStyles.Any);
                decimal VolOff3_BK = Decimal.Parse(dt1.Rows[0]["fltDMMonth3offsite"].ToString(), NumberStyles.Any);
                string remarks_BK = dt1.Rows[0]["txtDMBERemarks"].ToString();

                if (M1BE == M1BE_BK && M2BE == M2BE_BK && M3BE == M3BE_BK && BK1 == BK1_BK && BK2 == BK2_BK && BK3 == BK3_BK && BK4 == BK4_BK && VolOff1 == VolOff1_BK && VolOff2 == VolOff2_BK && VolOff3 == VolOff3_BK && VolOn1 == VolOn1_BK && VolOn2 == VolOn2_BK && VolOn3 == VolOn3_BK && remarks == remarks_BK)
                {

                    nochange++;
                }
            }
            if (grdBEDMView.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddCustomerPopUp", "AddCustomerPopUpNOChange();", true);
            }
            else
            {


                if (grdBEDMView.Rows.Count == nochange)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddCustomerPopUp", "AddCustomerPopUpNOChange();", true);

                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddCustomerPopUp", "AddCustomerPopUpChange();", true);
                }
            }

            //Response.Redirect("SDMView.aspx");
        }

        
        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{

        //    string url = "InfoPage.aspx";
        //    string s = "window.open('" + url + "', 'popup_window', 'left=350,width=400, height=200, menubar=no, scrollbars=no, resizable=no');";

        //    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        //    ClientScript.RegisterStartupScript(this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);
        //    ClientScript.RegisterStartupScript(this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);
        //}


        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            //Response.Write(@" <script type=""text/javascript""> window.open('InfoPage.aspx', 'popup_window', 'left=350,width=400, height=250, menubar=no, scrollbars=no, resizable=no'); </script>");
            //Response.Write(@" <script type=""text/javascript""> window.focus() </script>");



            string url = "InfoPage.aspx";
            string s = " debugger;if (typeof winPopup == 'undefined'){ winPopup= window.open('" + url + "', 'popup_window', 'left=350,width=400, height=200, menubar=no, scrollbars=no, resizable=no'); } else { alert('IN');winPopup.close();alert('closed'); }";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", s, true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);

            //ClientScript.RegisterStartupScript(this.GetType(), "makeTextBoxRed", "makeTextBoxRed();", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "CalculateOnLoadFooterTotal", "CalculateOnLoadFooterTotal();", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "InfoPage", "InfoPage();", true);
        }

        protected void lbBulkUpdate_Click(object sender, EventArgs e)
        {
            Day = service.FreezingPreviousMonthBE();
            hdFreeze.Value = Day.ToString();

            string Machineuserid = HttpContext.Current.User.Identity.Name;
            string[] userids = Machineuserid.Split('\\');
            if (userids.Length == 2)
            {
                Machineuserid = userids[1];
            }

            string userID = Session["userid"].ToString();
            //Machineuserid = userID;
            string pu = ddlNSO.Text;
            string customerCOde = ddlCustomerCode.Text;
            string role = Session["Role"] + "";
            string quarter = ddlQuarter.Text.Remove(2);

            int tempyear = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
            string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));
            yearForddl = year;

            DataTable dt = new DataTable();

            DataSet combtable = new DataSet();


            string currentQuarter = Session["quarter"] + "";
            if (Session["Role"].ToString() == "Admin")
            {

                role = "Anchor";
            }

            //TODO :  On PHase 2 
            combtable = service.GetDMBEDataExcel_bulk_DM(pu, customerCOde, userID, quarter, year, role, Machineuserid);
            string templatename = "BEBulkUpdate.xlsx";
            if (combtable.Tables[0].Rows.Count == 0)
            {
                PopUp("No data to download. Please check if you have access to at least one MCC.");
            }
            else
            {
                DownloadExcel_download(combtable, templatename);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "loadinggifClose", "loadinggifClose()", true);
        }

        private void DownloadExcel_download(DataSet ds, string templatename)
        {
            string role = Session["Role"] + "";
            string userID = Session["userid"].ToString();

            string str = "current";
            string currentQuarter = DateUtility.GetQuarter(str);
            currentQuarter = currentQuarter.Substring(0, 2) + currentQuarter.Substring(3, 2);
            string Selectedquarter = ddlQuarter.Text.Substring(0, 2) + ddlQuarter.Text.Substring(3, 2);

            //string dd1 = ddlQuarter.Text;
            //string dd2 = ddlQuarter.SelectedItem.Text;

            //string Selectedquarter = ddlQuarter.Text.Remove(2);



            int countofds = ds.Tables[0].Rows.Count;

            int a = Convert.ToInt32(ddlQuarter.SelectedItem.Text.ToString().Remove(0, 3)) - 1 + 2000;

            string finyr = Convert.ToString(a) + '-' + ddlQuarter.SelectedItem.Text.ToString().Remove(0, 3);

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            try
            {
                string name = Selectedquarter.Remove(2, 2).ToString() + '_' + finyr + '_' + DateTime.Now.ToString("ddMMMyyyy");

                //dateTime = Convert.ToDateTime("07/23/2017");
                //Selectedquarter = "Q1" + ddlQuarter.Text.Substring(3, 2);
                //name = Selectedquarter.Remove(2, 2).ToString() + '_' + finyr + '_' + dateTime.ToString("ddMMMyyyy");

                Microsoft.Office.Interop.Excel.Application oExcel;
                Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
                Excel.Worksheet xlSheet;
                Microsoft.Vbe.Interop.VBComponent oModule;
                string fileAndPath = GetPathAndFileName(PhysicalPath_DownloadFiles, templatename);
               
                Object oMissing = System.Reflection.Missing.Value;
                oExcel = new Microsoft.Office.Interop.Excel.Application();
                oBook = oExcel.Workbooks.
                    Open(fileAndPath, 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
                xlSheet = oBook.Worksheets.get_Item(2);
                xlSheet.Name = name;
                Microsoft.Office.Interop.Excel.Worksheet excelSubcOnReport = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item(name);
                xlSheet.Unprotect("DM123+");
                FillExcelSheet(dt, excelSubcOnReport);

                //dt.Columns.Add(userID,typeof(string));
                int CurrMonth = dateTime.Month;
                int Month1 = 0;
                int Month2 = 0;
                int Month3 = 0;

                if (Selectedquarter.Contains("Q1"))
                {
                    Month1 = 04;
                    Month2 = 05;
                    Month3 = 06;
                }
                else if (Selectedquarter.Contains("Q2"))
                {
                    Month1 = 07;
                    Month2 = 08;
                    Month3 = 09;
                }
                else if (Selectedquarter.Contains("Q3"))
                {
                    Month1 = 10;
                    Month2 = 11;
                    Month3 = 12;
                }
                else if (Selectedquarter.Contains("Q4"))
                {
                    Month1 = 01;
                    Month2 = 02;
                    Month3 = 03;
                }

                string CurrQtr = currentQuarter.Substring(1, 3);
                string SelectQtr = Selectedquarter.Substring(1, 3);

                int cDay = dateTime.Day;
                int a1 = Convert.ToInt32(stringReverseString1(CurrQtr));
                int b1 = Convert.ToInt32(stringReverseString1(SelectQtr));

                if (a1 > b1)
                {
                    if (CurrMonth == 1 || CurrMonth == 4 || CurrMonth == 7 || CurrMonth == 10)
                    {
                        if (cDay <= Day)
                        {
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 11], (object)xlSheet.Cells[countofds + 1, 11])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 11], (object)xlSheet.Cells[countofds + 1, 11])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 20], (object)xlSheet.Cells[countofds + 1, 21])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 20], (object)xlSheet.Cells[countofds + 1, 21])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                    }
                }
                else if (currentQuarter == Selectedquarter)
                {
                    if (CurrMonth == Month1)
                    {
                        ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 9], (object)xlSheet.Cells[countofds + 1, 11])).Locked = false;
                        ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 9], (object)xlSheet.Cells[countofds + 1, 11])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                        ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 16], (object)xlSheet.Cells[countofds + 1, 21])).Locked = false;
                        ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 16], (object)xlSheet.Cells[countofds + 1, 21])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                    }
                    else if (CurrMonth == Month2)
                    {
                        if (cDay <= Day)
                        {
                            //1st month
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 9], (object)xlSheet.Cells[countofds + 1, 11])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 9], (object)xlSheet.Cells[countofds + 1, 11])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 16], (object)xlSheet.Cells[countofds + 1, 21])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 16], (object)xlSheet.Cells[countofds + 1, 21])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                        else
                        {
                            //2nd month
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 10], (object)xlSheet.Cells[countofds + 1, 11])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 10], (object)xlSheet.Cells[countofds + 1, 11])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 18], (object)xlSheet.Cells[countofds + 1, 21])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 18], (object)xlSheet.Cells[countofds + 1, 21])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                    }
                    else if (CurrMonth == Month3)
                    {
                        if (cDay <= Day)
                        {
                            //2nd month
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 10], (object)xlSheet.Cells[countofds + 1, 11])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 10], (object)xlSheet.Cells[countofds + 1, 11])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 18], (object)xlSheet.Cells[countofds + 1, 21])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 18], (object)xlSheet.Cells[countofds + 1, 21])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                        else
                        {
                            //3rd month
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 11], (object)xlSheet.Cells[countofds + 1, 11])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 11], (object)xlSheet.Cells[countofds + 1, 11])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 20], (object)xlSheet.Cells[countofds + 1, 21])).Locked = false;
                            ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 20], (object)xlSheet.Cells[countofds + 1, 21])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(stringReverseString1(CurrQtr)) <= Convert.ToInt32(stringReverseString1(SelectQtr)))
                    {
                        ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 9], (object)xlSheet.Cells[countofds + 1, 22])).Locked = false;
                        ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 9], (object)xlSheet.Cells[countofds + 1, 22])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    }
                }

                ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 12], (object)xlSheet.Cells[countofds + 1, 15])).Locked = false;
                ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 12], (object)xlSheet.Cells[countofds + 1, 15])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 22], (object)xlSheet.Cells[countofds + 1, 22])).Locked = false;
                ((Microsoft.Office.Interop.Excel.Range)xlSheet.get_Range((object)xlSheet.Cells[2, 22], (object)xlSheet.Cells[countofds + 1, 22])).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);


                xlSheet.Protect("DM123+", oMissing, oMissing, oMissing, oMissing, false, false, false, false, false, false, false, false, true, true, false);

                //oBook.Password = "DM123+";
                //oBook.SaveAs("spreadsheet.xls");

                //Workbook workbook = new Workbook();
                //workbook.LoadFromFile(@"C:\Documents\TestFile.xlsx");

                oBook.Author = userID;
                oBook.Protect("excel@123", true, false);
                string filename = "DM_BEBulkUpdate_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";
                Session["BulkExcel"] = filename;
                String excelFile1 = GetPathAndFileName(PhysicalPath_DownloadFiles, filename); 
               
                if (new DirectoryInfo(PhysicalPath_DownloadFiles).GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                {
                    System.IO.File.Delete(excelFile1);
                }

                oBook.SaveAs(excelFile1);
                oBook.Close(false);
                oExcel.Quit();

                //oExcel = null;
                //oModule = null;
                //oBook = null;
                //WRss = null;
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                GC.Collect();

            
                if (new DirectoryInfo(PhysicalPath_DownloadFiles).GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "gridviewScroll", "gridviewScroll();", true);
                    iframeexcel.Attributes.Add("src", "DMExcelDownload.aspx?Key=BulkData");

                }


            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Error encountered. Please try again later.";
                Session.Add("Msg", lblmsg.Text);
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
            gvDMExcel.Visible = false;


        }

        public static string stringReverseString1(string str)
        {
            //char[] chars = str.ToCharArray();
            //char[] result = new char[chars.Length];
            //for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--)
            //{
            //    result[i] = chars[j];
            //}
            //return new string(result);

            string qtr = str.Substring(0, 1);
            string year = str.Substring(1);
            return year + qtr;
        }

        public static void FillExcelSheet(DataTable dt, Microsoft.Office.Interop.Excel.Worksheet excel)
        {
            int rowsExcelConsolidated = 0;
            try
            {
                int rows = dt.Rows.Count;
                int columns = dt.Columns.Count;
                int r = 0; int c = 0; int d = 0;
                object[,] DataArray = new object[rows + 1, columns + 1];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    excel.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;
                }

                for (c = 0; c <= columns - 1; c++)
                {
                    DataArray[r, d] = dt.Columns[c].ColumnName;
                    for (r = 0; r <= rows - 1; r++)
                    {
                        DataArray[r, d] = dt.Rows[r][c];

                    } //end row loop
                    d++;
                } //end

                Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[2 + rowsExcelConsolidated, 1];
                Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[1 + rowsExcelConsolidated + dt.Rows.Count, dt.Columns.Count];
                Microsoft.Office.Interop.Excel.Range range_excel = excel.get_Range(c1, c2);

                //Fill Array in Excel
                range_excel.Value2 = DataArray;

                range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                range_excel.Interior.Pattern = Microsoft.Office.Interop.Excel.XlPattern.xlPatternSolid;
                range_excel.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                excel.Columns.AutoFit();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            base.ValidateSession();

            Day = service.FreezingPreviousMonthBE();
            hdFreeze.Value = Day.ToString();

            string date = DateTime.Now.ToString("ddMMMyyyy");
           
            string fyyr = "";
            string strYear = "";//date.Substring(7, 2);
            string strMonth = "";
            DateTime cDate = Convert.ToDateTime(date);
            if (cDate.Day <= Day)
            {
                strMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month - 1).Substring(0,3);
            }
            else
            {
                strMonth = date.Substring(2, 3);
            }
            string strQuarter = "";
            if (strMonth == "Jan" || strMonth == "Feb" || strMonth == "Mar")
            {
                strQuarter = "Q4";
                strYear = (Convert.ToDateTime(date).Year - 1).ToString() + (Convert.ToDateTime(date).Year).ToString().Substring(2);
                fyyr = (Convert.ToDateTime(date).Year - 1).ToString() + "-" + (Convert.ToDateTime(date).Year).ToString().Substring(2);
            }
            else if (strMonth == "Apr" || strMonth == "May" || strMonth == "Jun")
            {
                strQuarter = "Q1";
                strYear = (Convert.ToDateTime(date).Year).ToString() + (Convert.ToDateTime(date).Year + 1).ToString().Substring(2);
                fyyr = (Convert.ToDateTime(date).Year).ToString() + "-" + (Convert.ToDateTime(date).Year + 1).ToString().Substring(2);

            }
            else if (strMonth == "Jul" || strMonth == "Aug" || strMonth == "Sep")
            {
                strQuarter = "Q2";
                strYear = (Convert.ToDateTime(date).Year).ToString() + (Convert.ToDateTime(date).Year + 1).ToString().Substring(2);
                fyyr = (Convert.ToDateTime(date).Year).ToString() + "-" + (Convert.ToDateTime(date).Year + 1).ToString().Substring(2);
            }
            else if (strMonth == "Oct" || strMonth == "Nov" || strMonth == "Dec")
            {
                strQuarter = "Q3";
                strYear = (Convert.ToDateTime(date).Year).ToString() + (Convert.ToDateTime(date).Year + 1).ToString().Substring(2);
                fyyr = (Convert.ToDateTime(date).Year).ToString() + "-" + (Convert.ToDateTime(date).Year + 1).ToString().Substring(2);
            }


            int prvYear = Convert.ToInt32(strYear);
            if (fuUploader.HasFile)
            {
                if (fuUploader.PostedFile.ContentLength != 0)
                {
                    string fileExtension = Path.GetExtension(fuUploader.FileName);
                    if (fileExtension == ".xlsx")
                    {
                        string fileName = Path.GetFileName(fuUploader.PostedFile.FileName);
                        if (fileName.Contains("BEBulkUpdate") == true)
                        {
                            try
                            {
                                string path = string.Empty;
                                if (fuUploader.HasFile)
                                {

                                    path = string.Concat(Server.MapPath("~/UploadOperations/" + fuUploader.FileName));

                                    fuUploader.SaveAs(path);
                                    // Connection String to Excel Workbook
                                    string conString = string.Empty;

                                    string extension = Path.GetExtension(fuUploader.PostedFile.FileName);
                                    if (extension == null)
                                    {
                                        PopUp("Please Upload a file!!");
                                    }
                                    //else if (extension == ".xlsx")
                                    //{
                                    //    PopUp("Please Upload a Excel 97-2003 file!!");


                                    //}
                                    else //if (extension == ".xls")
                                    {
                                        switch (extension)
                                        {

                                            case ".xls": //Excel 97-03

                                                //conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=yes;IMEX=1;\"";
                                                break;

                                            case ".xlsx": //Excel 07 or higher

                                                //conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=yes;IMEX=1;\"";
                                                break;


                                        }

                                        conString = string.Format(conString, path);

                                        if (extension == ".xls" || extension == ".xlsx")
                                        {
                                            int boolean = checkSheetPasswrd(Session["Userid"].ToString(), path, 2);
                                            using (OleDbConnection excel_con = new OleDbConnection(conString))
                                            {

                                                excel_con.Open();

                                                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                                                DataTable worksheets = excel_con.GetSchema("Tables");
                                                string w = worksheets.Columns["TABLE_NAME"].ToString();
                                                List<string> lstsheetNames = new List<string>();
                                                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                                                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

                                                //string s = lstsheetNames[0].ToString().Replace(@"'", string.Empty);

                                                string s = "";

                                                foreach (string sheet in lstsheetNames)
                                                {
                                                    if (sheet.Contains("Q"))
                                                    {
                                                        s = sheet.ToString().Replace(@"'", string.Empty);
                                                    }
                                                }


                                                string CurrQtr = "n";

                                                if (boolean == 1)
                                                {
                                                    string FileQuarter = s.Substring(0, 2);
                                                    string spQuarter = FileQuarter;

                                                    string FileYear = s.Substring(3, 7).Replace(@"-", string.Empty);
                                                    string fileFY = s.Substring(3, 7);
                                                    string DwldDate = s.Substring(11, 9);

                                                  


                                                    //string FileMonth = s.Substring(13, 3);
                                                    //int finYear = Convert.ToInt32(Year);
                                                    string FileMonth = "";
                                                    
                                                    DateTime Filedate = Convert.ToDateTime(DwldDate);
                                                    
                                                    if (Filedate.Day <= Day)
                                                    {
                                                        FileMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month - 1).Substring(0, 3);
                                                    }
                                                    else
                                                    {
                                                        FileMonth = s.Substring(13, 3);
                                                    }
                                                    //string strQuarter = "";
                                                    if (FileMonth == "Jan" || FileMonth == "Feb" || FileMonth == "Mar")
                                                    {
                                                        FileQuarter = "Q4";
                                                        FileYear = (Filedate.Year - 1).ToString() + (Filedate.Year).ToString().Substring(2);
                                                        //fyyr = (Filedate.Year - 1).ToString() + "-" + (Filedate.Year).ToString().Substring(2);
                                                    }
                                                    else if (FileMonth == "Apr" || FileMonth == "May" || FileMonth == "Jun")
                                                    {
                                                        FileQuarter = "Q1";
                                                        FileYear = (Filedate.Year).ToString() + (Filedate.Year + 1).ToString().Substring(2);
                                                        //fyyr = (Filedate.Year).ToString() + "-" + (Filedate.Year + 1).ToString().Substring(2);

                                                    }
                                                    else if (FileMonth == "Jul" || FileMonth == "Aug" || FileMonth == "Sep")
                                                    {
                                                        FileQuarter = "Q2";
                                                        FileYear = (Filedate.Year).ToString() + (Filedate.Year + 1).ToString().Substring(2);
                                                        //fyyr = (Filedate.Year).ToString() + "-" + (Filedate.Year + 1).ToString().Substring(2);
                                                    }
                                                    else if (FileMonth == "Oct" || FileMonth == "Nov" || FileMonth == "Dec")
                                                    {
                                                        FileQuarter = "Q3";
                                                        FileYear = (Filedate.Year).ToString() + (Filedate.Year + 1).ToString().Substring(2);
                                                        //fyyr = (Filedate.Year).ToString() + "-" + (Filedate.Year + 1).ToString().Substring(2);
                                                    }
                                                    int FilefinYear = Convert.ToInt32(FileYear);
                                                    if (FilefinYear >= prvYear)
                                                    {
                                                        if (FilefinYear == prvYear)
                                                        {
                                                            if (Convert.ToInt32(FileQuarter.Substring(1, 1)) >= Convert.ToInt32(strQuarter.Substring(1, 1)))
                                                            {
                                                                if (Convert.ToInt32(FileQuarter.Substring(1, 1)) == Convert.ToInt32(strQuarter.Substring(1, 1)))
                                                                {
                                                                    if (strMonth == FileMonth)
                                                                    {

                                                                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT *  FROM [" + s + "]", excel_con))
                                                                        {
                                                                            oda.Fill(dtExcel2Data);
                                                                            if (Convert.ToInt32(String.Concat(FilefinYear, FileQuarter.Substring(1, 1))) == Convert.ToInt32(String.Concat(FilefinYear, spQuarter.Substring(1, 1))))
                                                                            {
                                                                                CurrQtr = "y";
                                                                            }
                                                                            else
                                                                            {
                                                                                CurrQtr = "n";
                                                                            }
                                                                            //dtExcel2Data.Columns[0].ColumnName = "Item #";
                                                                        }
                                                                        excel_con.Close();

                                                                    }
                                                                    else
                                                                    {
                                                                        excel_con.Close();
                                                                        PopUp("BE Err ID 1: Kindly download the latest template and upload again as month roll over happened between download date and upload date. Only current and future updates are allowed.");
                                                                        return;
                                                                    }
                                                                }
                                                                else
                                                                {

                                                                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT *  FROM [" + s + "]", excel_con))
                                                                    {
                                                                        oda.Fill(dtExcel2Data);
                                                                    }
                                                                    excel_con.Close();

                                                                }
                                                            }
                                                            else
                                                            {
                                                                excel_con.Close();
                                                                PopUp("BE Err ID 2: Kindly download the latest template and upload again as quarter roll over happened between download date and upload date. Only current and future updates are allowed.");
                                                                return;
                                                            }
                                                        }
                                                        else
                                                        {

                                                            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT *  FROM [" + s + "]", excel_con))
                                                            {
                                                                oda.Fill(dtExcel2Data);
                                                            }
                                                            excel_con.Close();

                                                        }

                                                        excel_con.Close();
                                                        if (dtExcel2Data != null && dtExcel2Data.Rows.Count != 0)
                                                        {
                                                            string id = "exec [EAS_SP_Fetch_BEData_DM_ImportExcel_upload] 'ALL','" + spQuarter + "','ALL','" + Session["Userid"].ToString() + "','" + fileFY + "','" + Session["Role"].ToString() + "'";
                                                            DataSet dsid = service.GetDataSet(id);
                                                            DataTable dtid = dsid.Tables[0];
                                                            int x = 0;



                                                            string[] XLid = dtExcel2Data.AsEnumerable().Select(r => r[0].ToString()).ToList().ToArray();

                                                            dtExcel2Data.Columns.Remove(dtExcel2Data.Columns["DownloadedBy"]);
                                                            dtExcel2Data.Columns.Remove(dtExcel2Data.Columns["DownloadedOn"]);
                                                            if (dtExcel2Data.Rows.Count <= dtid.Rows.Count)
                                                            {

                                                                for (int a = 0; a < dtExcel2Data.Rows.Count; a++)
                                                                {
                                                                    for (int b = 0; b < dtid.Rows.Count; b++)
                                                                    {

                                                                        if (dtExcel2Data.Rows[a]["ID"].ToString() == dtid.Rows[b]["ID"].ToString())
                                                                        {
                                                                            x = x + 1;
                                                                            XLid = XLid.Where(val => val != dtExcel2Data.Rows[a]["ID"].ToString()).ToArray();
                                                                        }

                                                                    }
                                                                }

                                                                List<string> items = XLid.Select(n => Convert.ToString(n)).ToList();
                                                                for (int i = 8; i <= 20; i++)
                                                                {
                                                                    for (int j = 0; j < dtExcel2Data.Rows.Count; j++)
                                                                    {
                                                                        float number;
                                                                        if (float.TryParse(dtExcel2Data.Rows[j][i].ToString(), out number) == false || dtExcel2Data.Rows[j][i].ToString() == "")
                                                                        {
                                                                            string es = dtExcel2Data.Rows[j][i].ToString();

                                                                            string itemToAdd = dtExcel2Data.Rows[j]["ID"].ToString();
                                                                            if (!items.Contains(itemToAdd))
                                                                            {
                                                                                items.Add(itemToAdd);
                                                                                x = x - 1;
                                                                            }
                                                                            // XLid = XLid.Where(val => val != dtExcel2Data.Rows[j]["ID"].ToString()).ToArray();
                                                                        }
                                                                    }
                                                                }
                                                                XLid = items.ToArray();


                                                                DataTable dterr = new DataTable();
                                                                dterr.Columns.AddRange(new DataColumn[4] { new DataColumn("ID", typeof(int)), new DataColumn("MCC", typeof(string)), new DataColumn("NC", typeof(string)), new DataColumn("MailId", typeof(string)) });
                                                                for (int y = 0; y < XLid.Length; y++)
                                                                {
                                                                    for (int a = 0; a < dtExcel2Data.Rows.Count; a++)
                                                                    {
                                                                        DataRow dr = dtExcel2Data.Rows[a];
                                                                        if (dr["ID"].ToString() == "" || dr["ID"].ToString() == null)
                                                                        {
                                                                            //dtExcel2Data.Rows.Remove(dr);
                                                                            PopUp("File is corrupted ! Kindly contact Srinivas_manjunath for ORC and patel_jignesh for SAP with this error.");
                                                                            return;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (dr["ID"].ToString() == XLid[y].ToString())
                                                                            {
                                                                                dterr.Rows.Add(new string[] { dtExcel2Data.Rows[a]["ID"].ToString(), dtExcel2Data.Rows[a]["MasterClientCode"].ToString(), dtExcel2Data.Rows[a]["NativeCurrency"].ToString(), dtExcel2Data.Rows[a]["DMMailId"].ToString() });
                                                                                //dr.Delete();
                                                                                dtExcel2Data.Rows.Remove(dr);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                string BEID = string.Join(",", XLid.ToArray());

                                                                if (x == (dtExcel2Data.Rows.Count + XLid.Length))
                                                                {

                                                                    lblFinalMessage.Text = "DATA Uploaded & Updated succesfully as on " + date + " !!. Kindly refresh your search criteria to check the updates.";
                                                                    GVErrorMsg.Visible = false;
                                                                }
                                                                else
                                                                {

                                                                    lblFinalMessage.Text = "Insufficient access to below records or incorrect Value entered :";
                                                                    GVErrorMsg.Visible = true;
                                                                    GVErrorMsg.DataSource = dterr;
                                                                    GVErrorMsg.DataBind();
                                                                }



                                                                try
                                                                {
                                                                    service.uploadDM(Session["Userid"].ToString(), CurrQtr, dtExcel2Data);
                                                                    lblProcessedno.Text = "Number of Processed Records : " + (dtExcel2Data.Rows.Count + XLid.Length);
                                                                    lblSuccessno.Text = "Number of Successes : " + x;
                                                                    lblFailureno.Text = "Number of Failures : " + XLid.Length;
                                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "gifClose", "gifClose()", true);
                                                                    Modal2.Show();
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    PopUp("DATA not Uploaded due to internal issue. " + ex.Message);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PopUp("Kindly download latest file again and upload!!! ");
                                                                return;
                                                            }

                                                        }
                                                        else
                                                        {
                                                            PopUp("Data not uploaded due to some internal error !!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        PopUp("BE Err ID 3: Kindly download the latest template and upload again as year roll over happened between download date and upload date. Only current and future updates are allowed.");
                                                        return;
                                                    }

                                                }
                                                else if (boolean == -1)
                                                {
                                                    PopUp("Incorrect template uploaded. Please ensure only DM template is uploaded in the DM BE screen.");
                                                    return;
                                                }
                                                else
                                                {
                                                    PopUp("Uploaded file appeared to be tampered with. Kindly ensure to upload the file that you have downloaded and not the file that was downloaded by someone else.");
                                                    return;
                                                }
                                                //excel_con.Close();
                                            }

                                        }
                                    }
                                    //PopUp("Uploaded Successfully!!!");

                                }
                                else
                                {
                                    PopUp("Incorrect file uploaded. Please ensure to not change anything other than the Revenue/Vol numbers that are highlighted in YELLOW.");
                                    return;
                                }

                            }
                            catch (Exception ex)
                            {
                                PopUp(ex.Message);
                                if ((ex.Message + "").Contains("Thread was being aborted."))
                                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                                else
                                {
                                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                                    throw ex;
                                }
                            }
                        }
                        else
                        {
                            PopUp("Incorrect file uploaded. Please ensure to not change anything other than the Revenue/Vol numbers that are highlighted in YELLOW .Please Upload BEBulkUpdate excel file!!");
                            return;
                        }
                    }
                    else
                    {
                        PopUp("Incorrect file uploaded. Please ensure to not change anything other than the Revenue/Vol numbers that are highlighted in YELLOW .Please Upload an excel file with .xlsx format !!");
                        return;
                    }
                }
                else
                {
                    PopUp("Please verify the file to be loaded");
                    return;
                }
            }
            else
            {
                PopUp("Please Upload an excel file !!");
                return;
            }
            if (Session["Role"].ToString().ToLower() != "admin")
            {
                btnSearch_Click(null, null);

            }
        }

        private static int checkSheetPasswrd(string userID, string path, int s)
        {
            string folderadress = path;
            //folderadress = HttpContext.Current.Server.MapPath(folderadress);
            Microsoft.Office.Interop.Excel.Application WRExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks WRwbs = null;
            //Microsoft.Office.Interop.Excel.Workbook WRwb = new Microsoft.Office.Interop.Excel.Workbook();
            object oMissing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook WRwb = WRExcel.Workbooks.Add(oMissing);
            Microsoft.Office.Interop.Excel._Worksheet WRws = null;
            WRExcel.Visible = false;
            WRwbs = WRExcel.Workbooks;

            WRwb = WRwbs.Open(folderadress, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
            Microsoft.Office.Interop.Excel.Sheets WRss = null;
            WRss = WRwb.Sheets;
            //s = s.TrimEnd('$');
            string author = WRwb.Author;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item(s);
            bool eas = WRwb.ProtectStructure;
            if (eas)
            {
                try
                {
                    WRwb.Unprotect("excel@123");
                    excelSheet1.Unprotect("DM123+");


                    //var hiddenRange = ((Microsoft.Office.Interop.Excel.Range)excelSheet1.get_Range((object)excelSheet1.Cells[2, 23], (object)excelSheet1.Cells[2, 23]));
                    //hiddenRange.EntireColumn.Hidden = false;

                    author = ((Microsoft.Office.Interop.Excel.Range)excelSheet1.get_Range((object)excelSheet1.Cells[2, 23], (object)excelSheet1.Cells[2, 23])).Value;
                    //author = "Ali_Shaik";
                    if (author.ToLower() != userID.ToLower())
                    {
                        WRwb.Close(false);
                        WRExcel.Quit();
                        WRExcel = null;
                        WRwb = null;
                        return 0;
                    }
                    WRwb.Close(false);
                    WRExcel.Quit();
                    WRExcel = null;
                    WRwb = null;
                    GC.Collect();
                    // Unprotect suceeded:
                    return 1;
                }
                catch
                {
                    try
                    {
                        excelSheet1.Unprotect("SDM123+");
                        WRwb.Close(false);
                        WRExcel.Quit();
                        WRExcel = null;
                        WRwb = null;
                        GC.Collect();
                        // Unprotect failed:
                        return -1;
                    }
                    catch
                    {
                        WRwb.Close(false);
                        WRExcel.Quit();
                        WRExcel = null;
                        WRwb = null;
                        GC.Collect();
                        // Unprotect failed:
                        return 0;
                    }
                }
            }
            else
            {
                WRwb.Close(false);
                WRExcel.Quit();
                WRExcel = null;
                WRwb = null;
                GC.Collect();
                // Unprotect failed:
                return 0;
            }


        }
        private string GetMacroText(string fileName)
        {
            string pathandFile = Path.Combine(PhysicalPath_Macro, fileName);
            string text = System.IO.File.ReadAllText(pathandFile);
            return text;
        }

        private string GetPathAndFileName(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }

    }
