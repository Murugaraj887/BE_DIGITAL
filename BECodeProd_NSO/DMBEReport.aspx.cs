using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using BEData;
using System.Data;

public partial class DMBEReport : BasePage
{
    Logger logger = new Logger();
    public string fileName = "BEData.DMBEReport.cs";
    BEDL objbe = new BEDL();
    int mon = DateTime.Now.Month;
    string curqtr = string.Empty;
    public DateTime dateTime = DateTime.Today;
    string actyesm1 = string.Empty;
    string actyesm2 = string.Empty;
    string pactyesm3 = string.Empty;

    public int month = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.ValidateSession();

        if (Page.IsPostBack)
        { }
        else
        {
            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";

            //onload
            string isValidEntry = Session["Login"] + "";
            if (!isValidEntry.Equals("1"))
            Response.Redirect("UnAuthorised.aspx");
            string userID = Session["UserID"] + "";

            ddlSU.DataTextField = "SU";
            ddlSU.DataValueField = "SU";
            ddlSU.DataSource = objbe.GetSUBeReport(userID);
            ddlSU.DataBind();
            ddlSU.Items.Insert(0, "ALL");

            List<string> lstPU = objbe.GetNSOForuser(userID);
            ddlPU.DataSource = lstPU.Select(k => k.ToString()).Distinct().ToList();
            ddlPU.DataBind();
            ddlPU.Items.Insert(0, "ALL");
           
            ddlQtr.DataTextField = "txtqtr";
            ddlQtr.DataValueField = "txtqtr";
            ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
            ddlQtr.DataBind();
            
            ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));
            string qtr = ddlQtr.SelectedValue.Trim();

            ddlYear.DataTextField = "txtyear";
            ddlYear.DataValueField = "txtyear";
            ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            ddlYear.DataBind();

            string year = ddlYear.SelectedValue.ToString().Trim();            
        }
    }

    protected void btnreport_Click(object sender, EventArgs e)
    {
        try
        {
            var qtr = ddlQtr.SelectedValue.ToString().Trim();
            var year = ddlYear.SelectedValue.ToString();           
            DateTime date = DateTime.Now;
            var userid = Session["UserId"] + "";
            
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();

            var tblComparisonReport = dt1;
            if (ddlSU.SelectedItem.Text == "ALL")
            {              
                dt1 = objbe.GetBEReportforDMsplit(qtr, year, userid, "ORC",ddlPU.SelectedItem.Text);
                dt2 = objbe.GetBEReportforDMsplit(qtr, year, userid, "SAP", ddlPU.SelectedItem.Text);
                dt3 = objbe.GetBEReportforDMsplit(qtr, year, userid, "ECAS", ddlPU.SelectedItem.Text);
                dt4 = objbe.GetBEReportforDMsplit(qtr, year, userid, "EAIS", ddlPU.SelectedItem.Text);

                dt1.Merge(dt2);
                dt1.Merge(dt3);
                dt1.Merge(dt4);
                tblComparisonReport = dt1;
            }
            else
            {
                tblComparisonReport = objbe.GetBEReportforDMsplit(qtr, year, userid, ddlSU.SelectedItem.Text, ddlPU.SelectedItem.Text);
            }
            if (tblComparisonReport == null || tblComparisonReport.Rows.Count == 0)
            {
                lbl.Text = "";
                Session["key"] = null;
                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                return;
            }                                
            string folder = "ExcelOperations";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));
            string fileName = "DMBEReport_" + Session["UserId"] + "";
            Session["fileName"] = fileName;
            FileInfo file = new FileInfo(MyDir.FullName + "\\" + fileName + ".xlsx");
            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == fileName) != null)
                System.IO.File.Delete(MyDir.FullName +"\\"+ fileName);

            Session["FullfileName"] = file;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws;
            ExcelWorksheet ws1;            
            string sht = "BE_Data";
            int row = tblComparisonReport.Rows.Count;
            int col = tblComparisonReport.Columns.Count;
            {
                ws = pck.Workbook.Worksheets.Add(sht);
                ws.Cells["A1"].LoadFromDataTable(tblComparisonReport, true);
                //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                var fill = ws.Cells[1, 1, 1, col].Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
                ws.Cells[1, 1, row, col].AutoFitColumns();
            }
            pck.SaveAs(file);
            pck.Dispose();
            ws = null;
            pck = null;

            DownloadFileBEReport();
            //hdnfldFlag.Value = "1";
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

    protected void btnhidden_Click(object sender, ImageClickEventArgs e)
    {

        //string year = Convert.ToString(dateTime.Year);
        string year = ddlYear.Text.Substring(5, 2);
        string folder = "ExcelOperations";
        var MyDir = new DirectoryInfo(Server.MapPath(folder));
        string path = MyDir.FullName + "\\BEReport.xlsx";
        string name = string.Empty;
        //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        if (ddlSU.Text == "ALL")
        {
            name = "ECS" + "_" + ddlQtr.Text + year + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
        }
        else
        {
            name = ddlSU.Text + "_" + ddlQtr.Text + year + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
        }
        string ext = Path.GetExtension(path);
        string type = "";



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
                case ".xlsx":
                    type = "Application/x-msexcel";
                    break;
            }
        }

        Response.AppendHeader("content-disposition",
            "attachment; filename=" + name);

        if (type != "")
            Response.ContentType = type;
        Response.WriteFile(path);
        Response.End();


    }
    
    private void DownloadFileBEReport()
    {
        Excel.Application oExcel;
        Excel.Workbook oBook = default(Excel.Workbook);
        VBIDE.VBComponent oModule;
        try
        {
            bool forceDownload = true;
            //string path = MapPath(fname);
            string folder = "ExcelOperations";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));
            String sCode;
            Object oMissing = System.Reflection.Missing.Value;
            //Create an instance of Excel.
            oExcel = new Excel.Application();
            oBook = oExcel.Workbooks.
                Open(Session["FullfileName"].ToString(), 0, false, 5, "", "", true,
                Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);


            sCode = "sub BEReportMacro()\r\n" +
                System.IO.File.ReadAllText(MyDir.FullName + "\\BEReportDMMacro.txt") +
                    "\nend sub";

            oModule.CodeModule.AddFromString(sCode);

            oExcel.GetType().InvokeMember("Run",
                            System.Reflection.BindingFlags.Default |
                            System.Reflection.BindingFlags.InvokeMethod,
                            null, oExcel, new string[] { "BEReportMacro" });

            string finalname = Session["fileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                System.IO.File.Delete(MyDir.FullName +"\\"+ finalname);

            oBook.SaveCopyAs(MyDir.FullName +"\\"+ finalname);
            Session["key"] = finalname;
            oBook.Save();
            oBook.Close();
            oExcel.Quit();
            oExcel = null;
            oModule = null;
            oBook = null;
            GC.Collect();
            string year = Convert.ToString(dateTime.Year);
            string path = MyDir.FullName +"\\"+ finalname;            
            string ext = Path.GetExtension(path);
            string type = "";


            loading.Style.Add("visibility", "visible");
            lbl.Text = "Downloaded";
            up.Update();
            iframe.Attributes.Add("src", "Download.aspx");

            ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
            ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);


            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AppendHeader("content-disposition", "attachment;  filename=" + finalname);
            //Response.WriteFile(path);
            //Response.Flush();
            //Response.End();
            //if (ext != null)
            //{
            //    switch (ext.ToLower())
            //    {
            //        case ".htm":
            //        case ".html":
            //            type = "text/HTML";
            //            break;

            //        case ".txt":
            //            type = "text/plain";
            //            break;
            //        case ".csv":
            //        case ".xls":
            //        case ".xlsx":
            //            type = "Application/x-msexcel";
            //            break;
            //    }
            //}
            //if (forceDownload)
            //{
            //    Response.AppendHeader("content-disposition",
            //        "attachment; filename=" + finalname);
            //}
            //if (type != "")
            //    Response.ContentType = type;
            //Response.WriteFile(path);
            //Response.End();
            //loading.Visible = false;
        }

        catch (Exception ex)
        {
            if ((ex.Message + "").Contains("Thread was being aborted."))
            {
                oModule = null;
                oBook = null;
                oExcel = null;
                GC.Collect();
                logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
            else
            {
                oModule = null;
                oBook = null;
                oExcel = null;
                GC.Collect();
                logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
    protected void ddlSU_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (mon == 4 || mon == 5 || mon == 6)
            curqtr = "Q1";
        else if (mon == 7 || mon == 8 || mon == 9)
            curqtr = "Q2";
        else if (mon == 10 || mon == 11 || mon == 12)
            curqtr = "Q3";
        else
            curqtr = "Q4";

        string userID = Session["UserID"] + "";

        string su = ddlSU.SelectedValue;

        if (su.ToLowerTrim() == "all")
        {
            ddlSU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { su += k + ","; });
            su = su.Replace("ALLALL,", string.Empty);
            su = su.Trim().TrimEnd(',').TrimStart(',');
        }

        string type = ddlSU.SelectedItem.Text;

        ddlPU.DataTextField = "PU";
        ddlPU.DataValueField = "PU";
        ddlPU.DataSource = objbe.RTBRGetPUList(userID, type);
        ddlPU.DataBind();
        ddlPU.Items.Insert(0, "ALL");
           
        //hdnfldFlag.Value = "0";
    }

    protected void ddlPU_SelectedIndexChanged(object sender, EventArgs e)
    {
         
    }

    protected void ddlQtr_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qtr = ddlQtr.SelectedValue;
        ddlYear.DataTextField = "txtyear";
        ddlYear.DataValueField = "txtyear";
        ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
        ddlYear.DataBind();     
        string yr = ddlYear.SelectedValue;
       // hdnfldFlag.Value = "0";
    }   
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qtr = ddlQtr.SelectedValue;
        string yr = ddlYear.SelectedValue;
       //hdnfldFlag.Value = "0";
    }
}

    
