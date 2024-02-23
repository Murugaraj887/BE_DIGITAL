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
using System.Globalization;



    public partial class BEReport : BasePage
    {
        Logger logger = new Logger();
        public string fileName = "BEData.BEReport.cs";
        BEDL objbe = new BEDL();
        int mon = DateTime.Now.Month;
        string curqtr = string.Empty;
        public DateTime dateTime = DateTime.Now;
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


                //if (rdbtnlst.SelectedValue == "As Of Today")
                //{
                //    ddlDate.Enabled = false;
                //}
                //else
                //{
                //    ddlDate.Enabled = true;
                //}

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
                if (lstPU.Count > 1)
                {
                    ddlPU.DataSource = lstPU.Select(k => k.ToString()).Distinct().ToList();
                    ddlPU.DataBind();
                    ddlPU.Items.Insert(0, "ALL");
                }
                else if (lstPU.Count == 1)
                {
                    ddlPU.DataSource = lstPU.Select(k => k.ToString()).Distinct().ToList();
                    ddlPU.DataBind();

                }
               
                //string su = ddlSU.SelectedValue;

                //if (su.ToLowerTrim() == "all")
                //{
                //    ddlSU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { su += k + ","; });
                //    su = su.Replace("ALLALL,", string.Empty);
                //    su = su.Trim().TrimEnd(',').TrimStart(',');
                //}

                //ddlDH.DataTextField = "txtDHMailid";
                //ddlDH.DataValueField = "txtDHMailid";
                //ddlDH.DataSource = objbe.GetDHFromSuBeReport(userID, su);
                //ddlDH.DataBind();
                //ddlDH.Items.Insert(0, "ALL");
                //string dh = ddlDH.SelectedValue;

                //if (dh.ToLowerTrim() == "all")
                //{
                //    ddlDH.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { dh += k + ","; });
                //    dh = dh.Replace("ALLALL,", string.Empty);
                //    dh = dh.Trim().TrimEnd(',').TrimStart(',');
                //}

                //dh = dh.Trim();
                //ddlPU.DataTextField = "txtSBUCode";
                //ddlPU.DataValueField = "txtSBUCode";
                //ddlPU.DataSource = objbe.GetPUBeReport(dh);
                //ddlPU.DataBind();
                //ddlPU.Items.Insert(0, "ALL");
                //string pu = ddlPU.SelectedValue;

                //if (pu.ToLowerTrim() == "all")
                //{
                //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
                //    pu = pu.Replace("ALLALL,", string.Empty);
                //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
                //}
                //pu = pu.Trim();
                ddlQtr.DataTextField = "txtqtr";
                ddlQtr.DataValueField = "txtqtr";
                ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
                ddlQtr.DataBind();

                //ddlQtr.SelectedItem.Text = curqtr;
                //ddlQtr.SelectedItem.Value = curqtr;
                //int index = Convert.ToInt32(ddlQtr.Items.IndexOf(curqtr).ToString().Trim());
                ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

                string qtr = ddlQtr.SelectedValue.Trim();

                ddlYear.DataTextField = "txtyear";
                ddlYear.DataValueField = "txtyear";
                ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
                ddlYear.DataBind();

                string year = ddlYear.SelectedValue.ToString().Trim();

                //List<string> lstdates = new List<string>();
                //lstdates = objbe.GetDatesForDropDown(qtr, year);
                //ddlDate.DataTextField = "dtdate";
                //ddlDate.DataValueField = "dtdate";
                //ddlDate.DataSource = lstdates.Select(k => k.ToString()).Distinct().ToList();
                //ddlDate.DataBind();


                //rdbtnlst.Items.Add("As of Week Ending: ");

                //ddlfetchingType.Items.Insert(0,"Full");
                //ddlfetchingType.Items.Insert(1, "Partial");

            }
        }

        protected void btnreport_Click(object sender, EventArgs e)
        {
            try
            {
                
                var qtr = ddlQtr.SelectedValue.ToString().Trim();
                var year = ddlYear.SelectedValue.ToString();                             
                var date = DateTime.Now.ToString();              
                //string date = date1.ToString("yyyy-MM-dd");                               
                var userid = Session["UserId"] + "";
                var PU = ddlPU.SelectedItem.Text;
                
                DataSet ds1=new DataSet();
                DataSet ds2=new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                var tblComparisonReport = dt1;
                if (chkType.Checked ==true)
                {
                    if (ddlSU.SelectedItem.Text == "ALL")
                    {
                        //dt1 = objbe.GetBEReportforsplit(qtr, year, userid, "ORC");
                        //dt2 = objbe.GetBEReportforsplit(qtr, year, userid, "SAP");
                        ds1 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "ORC", "", date, "Full",  PU);
                        ds2 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "SAP", "", date, "Full",  PU);
                        ds3 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "ECAS", "", date, "Full", PU);
                        ds4 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "EAIS", "", date, "Full", PU);
                        
                        dt1 = ds1.Tables[0];                     
                        dt2 = ds2.Tables[0];
                        dt3 = ds3.Tables[0];
                        dt4 = ds4.Tables[0];

                        //DataSet dsdata = new DataSet();
                        //dsdata.Tables.Add(dt1);
                        //dsdata.Tables.Add(dt2);
                        //dsdata.Tables.Add(dt3);
                        //dsdata.Tables.Add(dt4);

                        dt1.Merge(dt2);
                        dt1.Merge(dt3);
                        dt1.Merge(dt4);
                                            
                        tblComparisonReport = dt1;
                    }
                    else
                    {
                        //tblComparisonReport = objbe.GetBEReportforsplit(qtr, year, userid, ddlSU.SelectedItem.Text);
                        ds1 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, ddlSU.SelectedItem.Text, "", date, "Full", PU);
                        dt1 = ds1.Tables[0];
                        tblComparisonReport = dt1;
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
                    string fileName = "BEReport_" + Session["UserId"] + "";
                    Session["fileName"] = fileName;

                    FileInfo file = new FileInfo(MyDir.FullName + "\\" + fileName + ".xlsx");
                    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName + ".xlsx")) != null)
                        System.IO.File.Delete(MyDir.FullName + "\\" + fileName + ".xlsx");

                    Session["FullfileName"] = file;
                    ExcelPackage pck = new ExcelPackage();

                    //Create the worksheet
                    // ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue_Volume_BE_Dump");


                    ExcelWorksheet ws;
                    ExcelWorksheet ws1;

                    // string yr = Convert.ToString(dateTime.Year);

                    //int tempyear = Convert.ToInt32(ddlQtr.Text.Remove(0, 3)) + 2000 - 1;
                    //string yr = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

                    //string sht = ddlQtr.Text + ddlYear.Text.Substring(5, 2) + "_" + "BE_Details";
                    string sht = "BE_Data";
                    int row = tblComparisonReport.Rows.Count;
                    int col = tblComparisonReport.Columns.Count;

                    //Create the worksheet
                    // if (tableBEREV.Rows.Count > 0)
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


                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    //  ws.Cells["A1"].LoadFromDataTable(tableBEREV, true);
                    pck.SaveAs(file);


                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=DineshReport.xlsx");
                    //Response.BinaryWrite(pck.GetAsByteArray());
                    pck.Dispose();
                    ws = null;
                    pck = null;

                    DownloadFileBEReport_Full();
                    //hdnfldFlag.Value = "1";

                }
                else
                {
                    if (ddlSU.SelectedItem.Text == "ALL")
                    {
                        //dt1 = objbe.GetBEReportforsplit(qtr, year, userid, "ORC");
                        //dt2 = objbe.GetBEReportforsplit(qtr, year, userid, "SAP");
                        ds1 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "ORC", "", date, "Partial", PU);
                        ds2 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "SAP", "", date, "Partial", PU);
                        ds3 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "ECAS", "", date, "Partial", PU);
                        ds4 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, "EAIS", "", date, "Partial", PU);
                        dt1 = ds1.Tables[0];
                        dt2 = ds2.Tables[0];
                        dt3 = ds3.Tables[0];
                        dt4 = ds4.Tables[0];

                     

                        dt1.Merge(dt2);
                        dt1.Merge(dt3);
                        dt1.Merge(dt4);
                        tblComparisonReport = dt1;
                    }
                    else
                    {
                        //tblComparisonReport = objbe.GetBEReportforsplit(qtr, year, userid, ddlSU.SelectedItem.Text);
                        ds1 = objbe.GetBEReportRevenueMomentum(qtr, year, userid, ddlSU.SelectedItem.Text, "", date, "Partial", PU);
                        dt1 = ds1.Tables[0];
                        tblComparisonReport = dt1;
                    }

                    if (tblComparisonReport == null || tblComparisonReport.Rows.Count == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                        return;
                    }
                    string folder = "ExcelOperations";
                    var MyDir = new DirectoryInfo(Server.MapPath(folder));
                    string fileName = "BEReport_" + Session["UserId"] + "";
                    Session["fileName"] = fileName;

                    FileInfo file = new FileInfo(MyDir.FullName + "\\" + fileName + ".xlsx");
                    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName + ".xlsx")) != null)
                        System.IO.File.Delete(MyDir.FullName + "\\" + fileName + ".xlsx");

                    Session["FullfileName"] = file;
                    ExcelPackage pck = new ExcelPackage();

                    //Create the worksheet
                    // ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue_Volume_BE_Dump");


                    ExcelWorksheet ws;
                    ExcelWorksheet ws1;

                    // string yr = Convert.ToString(dateTime.Year);

                    //int tempyear = Convert.ToInt32(ddlQtr.Text.Remove(0, 3)) + 2000 - 1;
                    //string yr = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

                    //string sht = ddlQtr.Text + ddlYear.Text.Substring(5, 2) + "_" + "BE_Details";
                    string sht = "BE_Data";
                    int row = tblComparisonReport.Rows.Count;
                    int col = tblComparisonReport.Columns.Count;

                    //Create the worksheet
                    // if (tableBEREV.Rows.Count > 0)
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


                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    //  ws.Cells["A1"].LoadFromDataTable(tableBEREV, true);
                    pck.SaveAs(file);


                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=DineshReport.xlsx");
                    //Response.BinaryWrite(pck.GetAsByteArray());
                    pck.Dispose();
                    ws = null;
                    pck = null;

                    DownloadFileBEReport_Partial();
                    //hdnfldFlag.Value = "1";

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

                //loading.Style.Add("visibility", "hidden");
            }

        }

        protected void btnhidden_Click(object sender, ImageClickEventArgs e)
        {

            ////string year = Convert.ToString(dateTime.Year);
            //string year = ddlYear.Text.Substring(5, 2);
            //string folder = "ExcelOperations";
            //var MyDir = new DirectoryInfo(Server.MapPath(folder));
            //string path = MyDir.FullName + "\\BEReport.xlsx";
            //string name = string.Empty;
            ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
            //if (ddlSU.Text == "ALL")
            //{
            //    name = "ECS" + "_" + ddlQtr.Text + year + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
            //}
            //else
            //{
            //    name = ddlSU.Text + "_" + ddlQtr.Text + year + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
            //}
            //string ext = Path.GetExtension(path);
            //string type = "";

           

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
          
            //Response.AppendHeader("content-disposition",
            //    "attachment; filename=" + name);

            //if (type != "")
            //    Response.ContentType = type;
            //Response.WriteFile(path);
            //Response.End();


        }
        //private void DownloadFileBEReport()
        //{
        //    Excel.Application oExcel;
        //    Excel.Workbook oBook = default(Excel.Workbook);
        //    VBIDE.VBComponent oModule;


        //    try
        //    {
        //        bool forceDownload = true;
        //        //string path = MapPath(fname);
        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));


        //        String sCode;
        //        Object oMissing = System.Reflection.Missing.Value;

        //        //Create an instance of Excel.
        //        oExcel = new Excel.Application();


        //        oBook = oExcel.Workbooks.
        //            Open(MyDir.FullName + "\\BEReport1.xlsx", 0, false, 5, "", "", true,
        //            Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);


        //        //oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);


        //        //sCode = "sub BEReportMacro()\r\n" +
        //        //    System.IO.File.ReadAllText(MyDir.FullName + "\\bereportmacro.txt") +
        //        //        "\nend sub";
        //        //string qtrcal = ddlQtr.Text + ddlYear.Text.Substring(5, 2);
        //        //sCode = string.Format(sCode, qtrcal);



        //        //oModule.CodeModule.AddFromString(sCode);

        //        //oExcel.GetType().InvokeMember("Run",
        //        //                System.Reflection.BindingFlags.Default |
        //        //                System.Reflection.BindingFlags.InvokeMethod,
        //        //                null, oExcel, new string[] { "BEReportMacro" });

        //        //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "BEReport.xlsx") != null)
        //        //    System.IO.File.Delete(MyDir.FullName + "\\BEReport.xlsx");

        //        //oBook.SaveCopyAs(MyDir.FullName + "\\BEReport.xlsx");


        //        oBook.Save();


        //        oBook.Close();
        //        oExcel.Quit();
        //        oExcel = null;
        //        oModule = null;
        //        oBook = null;

        //        GC.Collect();



        //        //string path = MyDir.FullName + "\\BEReport.xlsx";
        //        //string name = string.Empty;
        //        ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        //        //if (ddlSU.Text == "ALL")
        //        //{
        //        //    name = "ECS" + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
        //        //}
        //        //else
        //        //{
        //        //    name = ddlSU.Text + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
        //        //}
        //        //string ext = Path.GetExtension(path);
        //        //string type = "";



        //        //if (ext != null)
        //        //{
        //        //    switch (ext.ToLower())
        //        //    {
        //        //        case ".htm":
        //        //        case ".html":
        //        //            type = "text/HTML";
        //        //            break;

        //        //        case ".txt":
        //        //            type = "text/plain";
        //        //            break;



        //        //        case ".csv":
        //        //        case ".xls":
        //        //        case ".xlsx":
        //        //            type = "Application/x-msexcel";
        //        //            break;
        //        //    }
        //        //}
        //        //if (forceDownload)
        //        //{
        //        //    Response.AppendHeader("content-disposition",
        //        //        "attachment; filename=" + name);
        //        //}
        //        //if (type != "")
        //        //    Response.ContentType = type;
        //        //Response.WriteFile(path);
        //        //Response.End();



        //        //string path = MyDir.FullName + "\\BEReport1.xlsx";
        //        ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        //        //string name = "BEReport1" + ".xlsx";
        //        //string ext = Path.GetExtension(path);
        //        //string type = "";

        //        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //Response.AddHeader("content-disposition", "attachment;  filename=BEReport1.xlsx");

        //        //Response.WriteFile(path);

        //        //Response.Flush();
        //        //Response.End();

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        }
        //        else
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //private void DownloadFileBEReport()
        //{
        //    Excel.Application oExcel;
        //    Excel.Workbook oBook = default(Excel.Workbook);
        //    VBIDE.VBComponent oModule;


        //    try
        //    {
        //        bool forceDownload = true;
        //        //string path = MapPath(fname);
        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));


        //        String sCode;
        //        Object oMissing = System.Reflection.Missing.Value;

        //        //Create an instance of Excel.
        //        oExcel = new Excel.Application();


        //        oBook = oExcel.Workbooks.
        //            Open(MyDir.FullName + "\\BEReport1.xlsx", 0, false, 5, "", "", true,
        //            Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);


        //        oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);


        //        if (actyesm1 == "Y" && actyesm2 == "N")
        //        {
        //            //if (chkbxOnOff.Checked)
        //            //{
        //                sCode = "sub BEReportMacro()\r\n" +
        //                    System.IO.File.ReadAllText(MyDir.FullName + "\\BEReportMacroM2split.txt") +
        //                        "\nend sub";
        //            //}
        //            //else
        //            //{
        //                sCode = "sub BEReportMacro()\r\n" +
        //                    System.IO.File.ReadAllText(MyDir.FullName + "\\bereportmacro_M2.txt") +
        //                        "\nend sub";
        //            //}
        //        }
        //        else if (actyesm2 == "Y" && actyesm1 == "Y")
        //        {
        //            sCode = "sub BEReportMacro()\r\n" +
        //                System.IO.File.ReadAllText(MyDir.FullName + "\\bereportmacro_M3.txt") +
        //                    "\nend sub";
        //        }
        //        else
        //        {
        //            //if (chkbxOnOff.Checked)
        //            //{
        //                sCode = "sub BEReportMacro()\r\n" +
        //                    System.IO.File.ReadAllText(MyDir.FullName + "\\BEReportMacroM1split.txt") +
        //                        "\nend sub";
        //            //}
        //            //else
        //            //{
        //                sCode = "sub BEReportMacro()\r\n" +
        //                    System.IO.File.ReadAllText(MyDir.FullName + "\\bereportmacro_M1.txt") +
        //                        "\nend sub";
        //            //}
        //        }
        //        string qtrcal = ddlQtr.Text + ddlYear.Text.Substring(5, 2);

        //        int param = 0;
        //        //if (chkbxOnOff.Checked == true)
        //        //    param = 1;
        //        //else
        //        //    param = 0;

        //        sCode = string.Format(sCode, qtrcal, param);



        //        oModule.CodeModule.AddFromString(sCode);

        //        oExcel.GetType().InvokeMember("Run",
        //                        System.Reflection.BindingFlags.Default |
        //                        System.Reflection.BindingFlags.InvokeMethod,
        //                        null, oExcel, new string[] { "BEReportMacro" });

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "BEReport.xlsx") != null)
        //            System.IO.File.Delete(MyDir.FullName + "\\BEReport.xlsx");

        //        oBook.SaveCopyAs(MyDir.FullName + "\\BEReport.xlsx");


        //        oBook.Save();


        //        oBook.Close();
        //        oExcel.Quit();
        //        oExcel = null;
        //        oModule = null;
        //        oBook = null;

        //        GC.Collect();



        //        //string path = MyDir.FullName + "\\BEReport.xlsx";
        //        //string name = string.Empty;
        //        ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        //        //if (ddlSU.Text == "ALL")
        //        //{
        //        //    name = "ECS" + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
        //        //}
        //        //else
        //        //{
        //        //    name = ddlSU.Text + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy") + ".xlsx";
        //        //}
        //        //string ext = Path.GetExtension(path);
        //        //string type = "";



        //        //if (ext != null)
        //        //{
        //        //    switch (ext.ToLower())
        //        //    {
        //        //        case ".htm":
        //        //        case ".html":
        //        //            type = "text/HTML";
        //        //            break;

        //        //        case ".txt":
        //        //            type = "text/plain";
        //        //            break;



        //        //        case ".csv":
        //        //        case ".xls":
        //        //        case ".xlsx":
        //        //            type = "Application/x-msexcel";
        //        //            break;
        //        //    }
        //        //}
        //        //if (forceDownload)
        //        //{
        //        //    Response.AppendHeader("content-disposition",
        //        //        "attachment; filename=" + name);
        //        //}
        //        //if (type != "")
        //        //    Response.ContentType = type;
        //        //Response.WriteFile(path);
        //        //Response.End();



        //        //string path = MyDir.FullName + "\\BEReport1.xlsx";
        //        ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        //        //string name = "BEReport1" + ".xlsx";
        //        //string ext = Path.GetExtension(path);
        //        //string type = "";

        //        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //Response.AddHeader("content-disposition", "attachment;  filename=BEReport1.xlsx");

        //        //Response.WriteFile(path);

        //        //Response.Flush();
        //        //Response.End();

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        }
        //        else
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        private void DownloadFileBEReport_Full()
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
                    System.IO.File.ReadAllText(MyDir.FullName + "\\BEReportmacro.txt") +
                        "\nend sub";

                oModule.CodeModule.AddFromString(sCode);

                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "BEReportMacro" });

              string finalname = Session["fileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //string finalname = "RevenueMomentum_rupali03_07Aug2015_1052.xlsx";
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
               // string name = "RupaliExel_Test.xlsx";
                ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                //if (ddlSU.Text == "ALL")
                //{
                //    name = "ECS" + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //}
                //else
                //{
                //    name = ddlSU.Text + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //}
                string ext = Path.GetExtension(path);
                string type = "";

                //string path = MyDir.FullName + "\\BEReport.xlsx";
                ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                //string name = "BEReport" + ".xlsx";
                //string ext = Path.GetExtension(path);
                //string type = "";
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
                if (forceDownload)
                {
                    //Response.AppendHeader("content-disposition",
                    //    "attachment; filename=" + finalname);
                }
                //if (type != "")
                //    Response.ContentType = type;
                //Response.WriteFile(path);
                //Response.End();
               

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
                //loading.Style.Add("visibility", "hidden");
            }
        }


        private void DownloadFileBEReport_Partial()
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
                    System.IO.File.ReadAllText(MyDir.FullName + "\\NewSheetsMacro_updated.txt") +
                        "\nend sub";

                oModule.CodeModule.AddFromString(sCode);

                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "BEReportMacro" });

                string finalname = Session["fileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //string finalname = "RevenueMomentum_rupali03_07Aug2015_1052.xlsx";
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);

                oBook.SaveCopyAs(MyDir.FullName + "\\" + finalname);
                Session["key"] = finalname;

                oBook.Save();


                oBook.Close();
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();

                string year = Convert.ToString(dateTime.Year);

                string path = MyDir.FullName + "\\" + finalname;
                // string name = "RupaliExel_Test.xlsx";
                ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                //if (ddlSU.Text == "ALL")
                //{
                //    name = "ECS" + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //}
                //else
                //{
                //    name = ddlSU.Text + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //}
                string ext = Path.GetExtension(path);
                string type = "";

                //string path = MyDir.FullName + "\\BEReport.xlsx";
                ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                //string name = "BEReport" + ".xlsx";
                //string ext = Path.GetExtension(path);
                //string type = "";
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
                if (forceDownload)
                {
                    //Response.AppendHeader("content-disposition",
                    //    "attachment; filename=" + finalname);
                }
                //if (type != "")
                //    Response.ContentType = type;
                //Response.WriteFile(path);
                //Response.End();


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
                //loading.Style.Add("visibility", "hidden");
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

            ddlPU.DataTextField = "PU";
            ddlPU.DataValueField = "PU";
            ddlPU.DataSource = objbe.RTBRGetPUList(userID, su);
            ddlPU.DataBind();
            ddlPU.Items.Insert(0, "ALL");

            if (su.ToLowerTrim() == "all")
            {
                ddlSU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { su += k + ","; });
                su = su.Replace("ALLALL,", string.Empty);
                su = su.Trim().TrimEnd(',').TrimStart(',');
            }

            

            //ddlDH.DataTextField = "txtDHMailid";
            //ddlDH.DataValueField = "txtDHMailid";
            //ddlDH.DataSource = objbe.GetDHFromSuBeReport(userID, su);
            //ddlDH.DataBind();
            //ddlDH.Items.Insert(0, "ALL");
            //string dh = ddlDH.SelectedValue;

            //if (dh.ToLowerTrim() == "all")
            //{
            //    ddlDH.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { dh += k + ","; });
            //    dh = dh.Replace("ALLALL,", string.Empty);
            //    dh = dh.Trim().TrimEnd(',').TrimStart(',');
            //}

            //dh = dh.Trim();
            //ddlPU.DataTextField = "txtSBUCode";
            //ddlPU.DataValueField = "txtSBUCode";
            //ddlPU.DataSource = objbe.GetPUBeReport(dh);
            //ddlPU.DataBind();
            //ddlPU.Items.Insert(0, "ALL");

            //string pu = ddlPU.SelectedValue;

            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}
            //pu = pu.Trim();
            //hdnfldFlag.Value = "0";

            //ddlQtr.DataTextField = "txtqtr";
            //ddlQtr.DataValueField = "txtqtr";
            //ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
            //ddlQtr.DataBind();
            //ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            //string qtr = ddlQtr.SelectedValue.Trim();
            //ddlYear.DataTextField = "txtyear";
            //ddlYear.DataValueField = "txtyear";
            //ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            //ddlYear.DataBind();
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

            //string dh = ddlDH.SelectedValue;

            //if (dh.ToLowerTrim() == "all")
            //{
            //    ddlDH.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { dh += k + ","; });
            //    dh = dh.Replace("ALLALL,", string.Empty);
            //    dh = dh.Trim().TrimEnd(',').TrimStart(',');
            //}
            //dh = dh.Trim();

            //ddlPU.DataTextField = "txtSBUCode";
            //ddlPU.DataValueField = "txtSBUCode";
            //ddlPU.DataSource = objbe.GetPUBeReport(dh);
            //ddlPU.DataBind();
            //ddlPU.Items.Insert(0, "ALL");


            //string pu = ddlPU.SelectedValue;

            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}
            //pu = pu.Trim();
            //hdnfldFlag.Value = "0";
            //ddlQtr.DataTextField = "txtqtr";
            //ddlQtr.DataValueField = "txtqtr";
            //ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
            //ddlQtr.DataBind();

            //ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            //string qtr = ddlQtr.SelectedValue.Trim();

            //ddlYear.DataTextField = "txtyear";
            //ddlYear.DataValueField = "txtyear";
            //ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            //ddlYear.DataBind();
        }

        protected void ddlQtr_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qtr = ddlQtr.SelectedValue;
            ddlYear.DataTextField = "txtyear";
            ddlYear.DataValueField = "txtyear";
            ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            ddlYear.DataBind();

            //string qtr = ddlQtr.SelectedValue;
            string yr = ddlYear.SelectedValue;

            //ddlDate.DataSource = objbe.GetDatesForDropDown(qtr, yr);
            //ddlDate.DataBind();
            //hdnfldFlag.Value = "0";
            GG.Update();

        }
       
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qtr = ddlQtr.SelectedValue;
            string yr = ddlYear.SelectedValue;

            //ddlDate.DataSource = objbe.GetDatesForDropDown(qtr, yr);
            //ddlDate.DataBind();

           // hdnfldFlag.Value = "0";


        }
    }
