using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using Excel = Microsoft.Office.Interop.Excel;
using VBIDE = Microsoft.Vbe.Interop;
using BEData;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Office.Core;

    class Bullets { public string Name { get; set; } public string Value { get; set; } }



    public partial class ReportRTBRFinpulse : BasePage
    {


        List<string> lstRTBRMapping = new List<string>();
        List<string> lstFinMapping = new List<string>();
        Logger logger = new Logger();
        public string fileName = "BEData.ReportRTBRFinpulse.cs";
        BEDL objbe = new BEDL();
        //string userID = "sridevi_srirangan";

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            string userID = Session["UserID"] + "";

            //ddlPU.Visible = false;
            if (Page.IsPostBack)
            {
                
            }
            else
            {

                //onload
                //string isValidEntry = Session["Login"] + "";
                //if (!isValidEntry.Equals("1"))
                //    Response.Redirect("UnAuthorised.aspx");

                //string userID = "sridevi_srirangan";
                //string userID = Session["UserID"] + "";



                LoadComboBox(userID);
            // loadPU();

            string FA_Rate_Date = objbe.get_FA_MaxDate();

            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Value");



            DataRow dr = dtCurrency.NewRow();
            dr["Text"] = "USD in latest Ex rate"; dr["Value"] = "USD"; dtCurrency.Rows.Add(dr);
            dr = dtCurrency.NewRow(); dr["Text"] = "USD in " + FA_Rate_Date + " Fx"; dr["Value"] = "USDFA"; dtCurrency.Rows.Add(dr);
            dr = dtCurrency.NewRow(); dr["Text"] = "Native Currency"; dr["Value"] = "NC"; dtCurrency.Rows.Add(dr);
            dr = dtCurrency.NewRow(); dr["Text"] = "All"; dr["Value"] = "All"; dtCurrency.Rows.Add(dr);

            rdlRTBR.DataSource = dtCurrency.DefaultView;
            rdlRTBR.DataTextField = "Text";
            rdlRTBR.DataValueField = "Value";
            rdlRTBR.DataBind();

            rdlRTBR.SelectedIndex = 0;

            //string type = Convert.ToString(chkbxAllpu.Checked);
            //if (type == "False")
            //    type = "No";






            // string pu = ddlPU.SelectedValue;

            //string pu = ddlPU.Text;
            //if (pu.ToLowerTrim() == "all")
            //{
            //    /ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}

            //ddlMCC.DataTextField = "txtmcc";
            //ddlMCC.DataValueField = "txtmcc";
            //ddlMCC.DataSource = objbe.RTBRGetCustomerList(userID,"", "");
            //ddlMCC.DataBind();
            //ddlMCC.Items.Insert(0, "ALL");
            string type = ddlSU.SelectedValue.ToString();
                ddlMCC.DataTextField = "txtmcc";
                ddlMCC.DataValueField = "txtmcc";
                ddlMCC.DataSource = objbe.RTBRGetCustomerList(userID, type);
                ddlMCC.DataBind();
                ddlMCC.Items.Insert(0, "ALL");

            

        }
        }



        private void LoadComboBox(string userID)
        {
            try
            {
                int status = 0;
                List<string> lstSU = objbe.GetSUForuser(userID);

                if (lstSU.Count > 1)
                {
                    ddlSU.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                    ddlSU.DataBind();
                    ddlSU.Items.Insert(0, "ALL");
                }
                else if (lstSU.Count == 1)
                {
                    ddlSU.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                    ddlSU.DataBind();

                }
                else
                    status = 1;
                //ddlSU.Items.Insert(0, "ALL");

                List<string> lstPU = objbe.GetNSOForuser(userID);
                if (lstPU.Count > 1)
                {
                    ddlNSO.DataSource = lstPU.Select(k => k.ToString()).Distinct().ToList();
                    ddlNSO.DataBind();
                    ddlNSO.Items.Insert(0, "ALL");
                }
                else if (lstPU.Count == 1)
                {
                    ddlNSO.DataSource = lstPU.Select(k => k.ToString()).Distinct().ToList();
                    ddlNSO.DataBind();

                }
                else
                    status = 1;

                List<string> lstYear = new List<string>();
                lstYear = objbe.GetAllFinYearForRTBR();

                if (lstYear.Count == 0)
                    status = 1;
                else
                {
                    List<Bullets> lstYr = new List<Bullets>();
                    string r = lstYear[0].ToString();
                    for (int i = 0; i < lstYear.Count; i++)
                    {
                        lstYr.Add(new Bullets() { Name = lstYear[i].ToString(), Value = lstYear[i].ToString() });

                    }
                    //lstYr.Add(new Bullets() { Name = "2017", Value = "2017" });

                    rdlYear.DataSource = lstYr;
                    rdlYear.DataTextField = "Name";
                    rdlYear.DataValueField = "Value";
                    rdlYear.DataBind();
                    rdlYear.Items[0].Selected = true;
                }
                //ddlyear.DataSource = lstYear;
                //ddlyear.DataBind();
                //ddlyear.Items.Add("All");
                if (status == 1)
                {
                    lbl.Text = "";
                    Session["key"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No RTBR data!');</script>");
                   // lbldisplayRTBR.Text = "No data!";
                   // tblRTBR.Visible = false;
                    //btnreport.Visible = false;
                    return;
                }
                else
                {
                    btnreport.Visible = true;
                    lbldisplayRTBR.Text = lbldisplayRTBR.Text + objbe.GetRTBRDumpDate();
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

        protected void ddlSU_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string userID = "sridevi_srirangan";
            string type = ddlSU.SelectedValue.ToString();
            //loadPU();

            string userID = Session["UserID"] + "";
            //string type = Convert.ToString(chkbxAllpu.Checked);
            //if (type == "False")
            //    type = "No";
            //else
            //    type = "Yes";


            // string pu = ddlPU.Text;

            //string pu = ddlPU.Text;
            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}

            ddlNSO.DataTextField = "newOffering";
            ddlNSO.DataValueField = "newOffering";
            ddlNSO.DataSource = objbe.RTBRGetPUList(userID, type);
            ddlNSO.DataBind();
            ddlNSO.Items.Insert(0, "ALL");

            ddlMCC.DataTextField = "txtmcc";
            ddlMCC.DataValueField = "txtmcc";
            ddlMCC.DataSource = objbe.RTBRGetCustomerList(userID, type);
            ddlMCC.DataBind();
            ddlMCC.Items.Insert(0, "ALL");
        }

        //public void loadPU()
        //{
        //    string userID = Session["UserID"] + "";
        //    List<DUPUCCMap> lstMapping = new List<DUPUCCMap>();
        //    lstMapping = objbe.GetPU(userID, ddlSU.SelectedItem.Text);
        //    ddlPU.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
        //    ddlPU.DataBind();
        //    ddlPU.Items.Insert(0, "ALL");
        //}

        private void DownloadFile(string FileNAme)
        {



            try
            {


                Session["Key"] = FileNAme;

                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();

                iframe.Attributes.Add("src", "Download.aspx");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);





            }
            catch (Exception ex)
            {

            }
        }


        protected void btnreport_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "isvalidupload", "isvalidupload();", true);

            bool flag = true;
            //loading.Visible = true;
            //string userID = "sridevi_srirangan";
            string su = ddlSU.SelectedValue.ToString();
            string NSO = ddlNSO.SelectedItem.Text;
            string userID = Session["UserID"] + "";
            // DataTable dtRTBR,dtRTBR1,dtRTBR2,dtRTBR3;
            string dtRTBRexcel = string.Empty;
            string dtRTBRexcel1 = string.Empty;
            string dtRTBRexcel2 = string.Empty;
            string dtRTBRexcel3 = string.Empty;

            DataTable dtRTBR = new DataTable();
            DataTable dtRTBR1 = new DataTable();
            DataTable dtRTBR2 = new DataTable();
            DataTable dtRTBR3 = new DataTable();


            string mcc = ddlMCC.SelectedValue.ToString();

        //if (rdlRTBR.Text == "Both")
        //{

        //    dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Text, "USD", NSO, mcc, su);
        //    dtRTBRexcel = "RTBR_" + "USD" + "_" + rdlYear.Text;
        //    dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, "NC", NSO, mcc, su);
        //    dtRTBRexcel1 = "RTBR_" + "NC" + "_" + rdlYear.Text;

        //}
        //else if (rdlRTBR.Text == "USD")
        //{
        //    //dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Items[0].Text, rdlRTBR.SelectedValue, pu, mcc);
        //    //dtRTBRexcel = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Items[0].Text;
        //    dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, rdlRTBR.SelectedValue, NSO, mcc, su);
        //    dtRTBRexcel1 = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Text;
        //}

        //else if (rdlRTBR.Text == "NC")
        //{
        //    //dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Items[0].Text, rdlRTBR.SelectedValue, pu, mcc);
        //    //dtRTBRexcel = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Items[0].Text;
        //    dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, rdlRTBR.SelectedValue, NSO, mcc, su);
        //    dtRTBRexcel1 = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Text;
        //}

        //if (rdlRTBR.Text == "Both")
        //{
        //    if ((dtRTBR == null || dtRTBR.Rows.Count == 0) && (dtRTBR1 == null || dtRTBR1.Rows.Count == 0))
        //    {
        //        lbl.Text = "";
        //        Session["key"] = null;
        //        //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //        PopUp("No Data to download!");
        //        //hdnfldFlag.Value = "0";
        //        flag = false;
        //        return;
        //    }
        //}
        //else if (rdlRTBR.Text == "USD")
        //{
        //    if (dtRTBR1 == null || dtRTBR1.Rows.Count == 0)
        //    {
        //        lbl.Text = "";
        //        Session["key"] = null;
        //        PopUp("No Data to download!");
        //        flag = false;
        //        //hdnfldFlag.Value = "0";
        //        //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //        return;
        //    }

        //}

        //else if (rdlRTBR.Text == "NC")
        //{
        //    if (dtRTBR1 == null || dtRTBR1.Rows.Count == 0)
        //    {
        //        lbl.Text = "";
        //        Session["key"] = null;
        //        PopUp("No Data to download!");
        //        flag = false;
        //        //hdnfldFlag.Value = "0";
        //        //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //        return;
        //    }
        //}
        string FA_Rate_Date = objbe.get_FA_MaxDate();
        if (rdlRTBR.SelectedValue == "All")
        {

            dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Text, "USD", NSO, mcc, su);
            dtRTBRexcel = "RTBR_" + "USD in latest Ex rate" + "_" + rdlYear.Text;
            dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, "NC", NSO, mcc, su);
            dtRTBRexcel1 = "RTBR_" + "NC" + "_" + rdlYear.Text;
            dtRTBR2 = objbe.GetRTBRDetails(userID, rdlYear.Text, "USDFA", NSO, mcc, su);
            dtRTBRexcel2 = "RTBR_" + "USD in " + FA_Rate_Date + " Fx" + "_" + rdlYear.Text;


        }
        else if (rdlRTBR.SelectedValue == "USD")
        {
            //dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Items[0].Text, rdlRTBR.SelectedValue, pu, mcc);
            //dtRTBRexcel = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Items[0].Text;
            dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, rdlRTBR.SelectedValue, NSO, mcc, su);
            dtRTBRexcel1 = "RTBR_" + "USD in latest Ex rate" + "_" + rdlYear.Text;
        }
        else if (rdlRTBR.SelectedValue == "USDFA")
        {
            //dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Items[0].Text, rdlRTBR.SelectedValue, pu, mcc);
            //dtRTBRexcel = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Items[0].Text;
            dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, rdlRTBR.SelectedValue, NSO, mcc, su);
            dtRTBRexcel1 = "RTBR_" + "USD in " + FA_Rate_Date + " Fx" + "_" + rdlYear.Text;
        }
        else if (rdlRTBR.SelectedValue == "NC")
        {
            //dtRTBR = objbe.GetRTBRDetails(userID, rdlYear.Items[0].Text, rdlRTBR.SelectedValue, pu, mcc);
            //dtRTBRexcel = "RTBR_" + rdlRTBR.SelectedValue + "_" + rdlYear.Items[0].Text;
            dtRTBR1 = objbe.GetRTBRDetails(userID, rdlYear.Text, rdlRTBR.SelectedValue, NSO, mcc, su);
            dtRTBRexcel1 = "RTBR_" + "NC" + "_" + rdlYear.Text;
        }

        if (rdlRTBR.SelectedValue == "All")
        {
            if ((dtRTBR == null || dtRTBR.Rows.Count == 0) && (dtRTBR1 == null || dtRTBR1.Rows.Count == 0) && (dtRTBR2 == null || dtRTBR2.Rows.Count == 0))
            {
                lbl.Text = "";
                Session["key"] = null;
                //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                PopUp("No Data to download!");
                //hdnfldFlag.Value = "0";
                flag = false;
                return;
            }
        }
        else if (rdlRTBR.SelectedValue == "USD")
        {
            if (dtRTBR1 == null || dtRTBR1.Rows.Count == 0)
            {
                lbl.Text = "";
                Session["key"] = null;
                PopUp("No Data to download!");
                flag = false;
                //hdnfldFlag.Value = "0";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                return;
            }

        }
        else if (rdlRTBR.SelectedValue == "USDFA")
        {
            if (dtRTBR1 == null || dtRTBR1.Rows.Count == 0)
            {
                lbl.Text = "";
                Session["key"] = null;
                PopUp("No Data to download!");
                flag = false;
                //hdnfldFlag.Value = "0";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                return;
            }

        }

        else if (rdlRTBR.SelectedValue == "NC")
        {
            if (dtRTBR1 == null || dtRTBR1.Rows.Count == 0)
            {
                lbl.Text = "";
                Session["key"] = null;
                PopUp("No Data to download!");
                flag = false;
                //hdnfldFlag.Value = "0";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                return;
            }
        }

        //string[] strArry = { "Project Code", "Project Start Date", "Project End date", "Project Currency", "Project Type", "TBB Enabled", "Service Offering", "Client Code", "Master Client Code", "SU Code", "IBU Code", "CU Code", "PM Mail ID", "DM Mail ID", "Master PU Code", "Customer Portfolio", "Fin Year End", "Participating PU", "Participating Company", "Apr Value", "May Value", "Jun Value", "Q1 Total", "Jul Value", "Aug Value", "Sep Value", "Q2 Total", "Oct Value", "Nov Value", "Dec Value", "Q3 Total", "Jan Value", "Feb Value", "Mar Value", "Q4 Total", "Annual RTBR", "Company Code", "Consulting Involved?", "DumpDate", "Master Client Code", "Practice Line Code", "Credit Unit", "Practice Line Description", "Service Line Code", "Master Practice Line Code", "Master Practice Line Description", "Child Source Company", "Reporting PU", "Reporting Subunit", "Reporting Unit", "Group Master", "Child Project", "Mapped to PU", "Credit Sub Unit","Region Code" };


        string[] strArry = { "Project Code", "Project Start Date", "Project End date", "Project Currency", "Project Type", "TBB Enabled", "Service Offering", "Client Code", "Master Client Code", "SU Code", "IBU Code", "CU Code", "PM Mail ID", "DM Mail ID", "Master PU Code", "Customer Portfolio", "Fin Year End", "Participating PU", "Participating Company", "Apr Value", "May Value", "Jun Value", "Q1 Total", "Jul Value", "Aug Value", "Sep Value", "Q2 Total", "Oct Value", "Nov Value", "Dec Value", "Q3 Total", "Jan Value", "Feb Value", "Mar Value", "Q4 Total", "Annual RTBR", "Company Code", "Consulting Involved?", "Practice Line Code", "Practice Line Description", "Service Line Code", "Credit Unit", "Master Client Name", "Master Practice Line Code", "Master Practice Line Description", "Child Source Company", "Reporting PU", "Reporting Subunit", "Reporting Unit", "Group Master", "Child Project", "Mapped to PU", "Credit Sub Unit", "Region Code", "Child Start Date", "Child End date", "LOE No", "LOE Version No", "txtPU","NSO Code", "DumpDate" };


            lstRTBRMapping = strArry.ToList();


            if (dtRTBR != null)
            {
                if (dtRTBR.Rows.Count > 0)
                {
                    for (int i = 0; i < lstRTBRMapping.Count; i++)
                        dtRTBR.Columns[lstRTBRMapping[i]].SetOrdinal(i);

                }

            }

        int i8 = dtRTBR1.Columns.Count;

            if (dtRTBR1 != null)
            {
                if (dtRTBR1.Rows.Count > 0)
                {
                    for (int i = 0; i < lstRTBRMapping.Count; i++)
                        dtRTBR1.Columns[lstRTBRMapping[i]].SetOrdinal(i);

                }
            }

            string folder = @"ExcelOperations\DownloadFiles";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));
            //string role = objbe.GetUserRole(userID);
            //string currentQtr = DateUtility.GetQuarter("current");

            string name1 = "RTBR_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
            Session["NAME"] = name1.ToString();
            string name = Session["NAME"].ToString();
            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == name) != null)
                System.IO.File.Delete(MyDir.FullName + "\\" + name);

            // string name = "RTBR_Dump1" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls";
            FileInfo file = new FileInfo(MyDir.FullName + "\\" + name);
            string fullFileName = file.ToString();
            //using (StreamWriter sw = new StreamWriter(MyDir.FullName + "\\RTBR_Finpulse_Dump.xls"))
            //{
            //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            //    {
            //        grid.RenderControl(hw);
            //    }

            //}

            ExcelPackage pck = new ExcelPackage();

            //Create the worksheet
            // ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue_Volume_BE_Dump");


            ExcelWorksheet ws = null;
            ExcelWorksheet ws1 = null;
            ExcelWorksheet ws2 = null;
            ExcelWorksheet ws3 = null;
            ExcelWorksheet ws4 = null;

            if (dtRTBR.Rows.Count > 0)
            {
                int rowcountSheet0 = dtRTBR.Rows.Count;
                int colcountSheet0 = dtRTBR.Columns.Count;

                ws = pck.Workbook.Worksheets.Add(dtRTBRexcel);
                ws.Cells["A1"].LoadFromDataTable(dtRTBR, true);
                ws.Cells[1, 1, 1, 61].Style.Font.Bold = true;
                var fill = ws.Cells[1, 1, 1, 61].Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws.Cells[1, 1, 1, 61].AutoFitColumns();

                ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
                ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;

            }
            //ws.Cells[1, 1, 1, 60].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);

            if (dtRTBR1.Rows.Count > 0)
            {
                int rowcountSheet0 = dtRTBR1.Rows.Count;
                int colcountSheet0 = dtRTBR1.Columns.Count;

                ws1 = pck.Workbook.Worksheets.Add(dtRTBRexcel1);
                ws1.Cells["A1"].LoadFromDataTable(dtRTBR1, true);
                ws1.Cells[1, 1, 1, 61].Style.Font.Bold = true;
                var fill1 = ws1.Cells[1, 1, 1, 61].Style.Fill;
                fill1.PatternType = ExcelFillStyle.Solid;
                fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws1.Cells[1, 1, 1, 61].AutoFitColumns();

                ws1.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
                ws1.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;
            }
            if (dtRTBR2.Rows.Count > 0)
            {
                int rowcountSheet0 = dtRTBR2.Rows.Count;
                int colcountSheet0 = dtRTBR2.Columns.Count;

                ws2 = pck.Workbook.Worksheets.Add(dtRTBRexcel2);
                ws2.Cells["A1"].LoadFromDataTable(dtRTBR2, true);
                ws2.Cells[1, 1, 1, 61].Style.Font.Bold = true;
                var fill2 = ws2.Cells[1, 1, 1, 61].Style.Fill;
                fill2.PatternType = ExcelFillStyle.Solid;
                fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws2.Cells[1, 1, 1, 61].AutoFitColumns();

                ws2.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
                ws2.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;
            }
            if (dtRTBR3.Rows.Count > 0)
            {
                int rowcountSheet0 = dtRTBR3.Rows.Count;
                int colcountSheet0 = dtRTBR3.Columns.Count;

                ws3 = pck.Workbook.Worksheets.Add(dtRTBRexcel3);
                ws3.Cells["A1"].LoadFromDataTable(dtRTBR3, true);
                ws3.Cells[1, 1, 1, 61].Style.Font.Bold = true;
                var fill3 = ws3.Cells[1, 1, 1, 61].Style.Fill;
                fill3.PatternType = ExcelFillStyle.Solid;
                fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws3.Cells[1, 1, 1, 61].AutoFitColumns();

                ws3.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
                ws3.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;
            }

            pck.SaveAs(file);
            //Response.BinaryWrite(pck.GetAsByteArray());
            pck.Dispose();
            ReleaseObject(pck);
            ReleaseObject(ws);
            ReleaseObject(ws1);
            ReleaseObject(ws2);
            ReleaseObject(ws3);
            //ReleaseObject(ws4);
            //loading.Visible = false;
            //btnreport.Visible = true;

            GenerateReport(name);



        }

        void GenerateReport(string fname)
        {

            string UserId = Session["UserId"].ToString();
            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            //try
            {
                string folder = @"ExcelOperations\DownloadFiles";
                var myDir = new DirectoryInfo(Server.MapPath(folder));

                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();

                oBook = oExcel.Workbooks.
                    Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
                string filename = "";
                filename = "RTBR_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

                if (myDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    System.IO.File.Delete(myDir.FullName + "\\" + filename);

                String excelFile1 = "~\\ExcelOperations\\DownloadFiles\\" + filename;
                String destPath = Server.MapPath(excelFile1);

                //Adding permission to excel file//
               // oBook.Activate();
               // oBook.Permission.Enabled = true;
               // oBook.Permission.RemoveAll();
               // string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
               // DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
               // DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
               // UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                
               //// userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
               // userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////

                oBook.SaveCopyAs(destPath);
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                oExcel = null;
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                GC.Collect();


                DownloadFile(filename);

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

     
        private void ClearDataSet(DataSet dataSet)
        {
            // To test, print the number rows in each table. 
            foreach (DataTable table in dataSet.Tables)
            {
                Console.WriteLine(table.TableName + "Rows.Count = "
                    + table.Rows.Count.ToString());
            }
            // Clear all rows of each table.
            dataSet.Clear();

            // Print the number of rows again. 
            foreach (DataTable table in dataSet.Tables)
            {
                Console.WriteLine(table.TableName + "Rows.Count = "
                    + table.Rows.Count.ToString());
            }
        }

        protected void btnhidden_Click(object sender, ImageClickEventArgs e)
        {
            //string name = "RTBR_Dump1" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls";

            string name = Session["NAME"].ToString();
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", "attachment;  filename=" + name);



            string folder = @"ExcelOperations\DownloadFiles";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));
            //string path = MyDir.FullName + "\\RTBR_Dump1.xlsx";
            string path = MyDir.FullName +"\\"+ name;
            //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
            //string name = "RTBR_Dump1" + ".xlsx";
            string ext = Path.GetExtension(path);
            string type = "";

            // set known types based on file extension  
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;

                    case ".txt":
                        type = "text/plain";
                        break;

                    case ".csv":
                    case ".xls":
                        type = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        type = "application/vnd.ms-excel.12";
                        break;
                }
            }


            Response.AppendHeader("content-disposition", "attachment;" + "filename=" + name + "; " + "creation-date=" + DateTime.Now.ToString("R").Replace(",", "") + "; " +
                    "modification-date=" + DateTime.Now.ToString("R").Replace(",", "") + "; " +
                    "read-date=" + DateTime.Now.ToString("R").Replace(",", ""));


            if (type != "")
                Response.ContentType = type;
            Response.WriteFile(path);

            Response.Flush();
            Response.End();
        }
    
    


        protected void PopUp(string msg)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "", "alert('" + msg + "');", true);

        } // EO PopUp()

        protected void ddlNSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  loadPU();
            //string userID = Session["UserID"] + "";
            //string type = Convert.ToString(chkbxAllpu.Checked);
            //if (type == "False")
            //    type = "No";
            //else
            //    type = "Yes";


            //string pu = ddlPU.Text;

            ////string pu = ddlPU.Text;
            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}

            string userID = Session["UserID"] + "";
            string SU = ddlSU.SelectedItem.Text;
            string NSO = ddlNSO.SelectedItem.Text;

            ddlMCC.DataTextField = "txtmcc";
            ddlMCC.DataValueField = "txtmcc";
            ddlMCC.DataSource = objbe.RTBRGetCustomerListForSUMCC(userID, SU, NSO);
            ddlMCC.DataBind();
            ddlMCC.Items.Insert(0, "ALL");
        }

        //    protected void chkbxAllpu_CheckedChanged(object sender, EventArgs e)
        //    {


        //        string userID = Session["UserID"] + "";
        //        string type = Convert.ToString(chkbxAllpu.Checked);
        //        string pu = string.Empty;

        //        if (chkbxAllpu.Checked == false)
        //        {

        //            ddlSU.Enabled = true;
        //            ddlPU.Enabled = true;
        //            type = "No";

        //            pu = ddlPU.SelectedValue;
        //            if (pu.ToLowerTrim() == "all")
        //            {
        //                ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
        //                pu = pu.Replace("ALLALL,", string.Empty);
        //                pu = pu.Trim().TrimEnd(',').TrimStart(',');
        //            }

        //        }
        //        else
        //        {
        //            ddlSU.Text = "ALL";
        //           // ddlPU.Text = "ALL";
        //            type = "Yes";
        //            pu = "All";
        //            ddlSU.Enabled = false;
        //            //ddlPU.Enabled = false;

        //        }
        //        ddlMCC.DataTextField = "txtmcc";
        //        ddlMCC.DataValueField = "txtmcc";
        //        ddlMCC.DataSource = objbe.RTBRGetCustomerList(userID, type, pu);
        //        ddlMCC.DataBind();
        //        ddlMCC.Items.Insert(0, "ALL");
        //    }
        //}
    }
