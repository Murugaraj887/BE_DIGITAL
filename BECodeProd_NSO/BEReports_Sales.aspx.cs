using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using BEData;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.OleDb;
using System.Data;
using Microsoft.Office.Core;
using Microsoft.Reporting.WebForms; 
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;
using Ionic.Zip;

namespace BECodeProd
{
    public partial class BEReports_Sales : BasePage
    {
        

        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public DateTime dateTime = DateTime.Today;
        public string fileName = "Reports";
        static string yr = "";
        string cmdParam = "";
        DataAccess DA = new DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (!IsPostBack)
            {
                FetchGeneratedDate();
            }
        }

        private void FetchGeneratedDate()
        {
            DataTable dt = new DataTable();
            SqlCommand cmdParam = new SqlCommand("select max(dumpdate) from PipelineReports");
            dt = DA.ExecuteSP(cmdParam);
            string dtdate = Convert.ToDateTime(dt.Rows[0][0]).ToString("dd-MMM-yyyy");

            string path = @"D:\DefaultApplication\PIPELINE_HOME\PIPELINE_PROD\Pipeline_pdf_RegionWise\" + dtdate + @"\";
            DateTime lastHigh = new DateTime(1900, 1, 1);

            if (!Directory.Exists(path))
            {
                dt = new DataTable();
                cmdParam = new SqlCommand(" select max(dumpdate) from PipelineReports where DumpDate <> ( select max(dumpdate) from PipelineReports)");
                dt = DA.ExecuteSP(cmdParam);
                dtdate = Convert.ToDateTime(dt.Rows[0][0]).ToString("dd-MMM-yyyy");
                path = @"D:\DefaultApplication\PIPELINE_HOME\PIPELINE_PROD\Pipeline_pdf_RegionWise\" + dtdate + @"\";
            }

            string highDir;
            foreach (string subdir in Directory.GetFiles(path))
            {
                FileInfo fi1 = new FileInfo(subdir);
                DateTime created = fi1.LastWriteTime;

                if (created > lastHigh)
                {
                    highDir = subdir;
                    lastHigh = created;
                }
            }

            DateTime _date = DateTime.Parse(lastHigh.ToString());
            lblasondate.Text += _date.ToString("dd-MMM-yyyy hh:mm:ss tt");

            string folder = "BE_Sales";
            var myDir = new DirectoryInfo(Server.MapPath(folder));

            string ReportPath = Server.MapPath(folder) + "\\" + "Americas_Sales_Dashboard.xlsm" + "";

            DateTime modification = File.GetLastWriteTime(ReportPath);

            lblBE.Text += modification.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }

        protected void btnBEreportSales_Click(object sender, EventArgs e)
        {
            Array.ForEach(Directory.GetFiles(Server.MapPath("~/BE_Sales/")), File.Delete);
            BEReport_Excel_Sales();
        }

        private void BEReport_Excel_Sales()
        {
            int year = DateTime.Today.Year;
            DateTime todaydate = dateTime;
            string strcurrent = "Q1";
            int currentYear = dateTime.Year; //DateTime.Now.Year;
            



            string MachineUser = Session["MachineUser"].ToString();
            string MachineRole = Session["MachineRole"].ToString();

            //string userID = Session["UserID"] + "";
            string currentQuarter = strcurrent.Substring(0, 2);
            //string cmdtext = "select txtServiceLine from BEUserAccess where txtUserId='" + MachineUser + "'";
            //DataSet ds = new DataSet();
            //ds = service.GetDataSet(cmdtext);
            //DataTable dt = new DataTable();
            //dt = ds.Tables[0];
            FileInfo file;
            try
            {
                var qtr = currentQuarter;

                var userid = MachineUser;
                DataSet dsORC = new DataSet();
                DataSet dsSAP = new DataSet();



                if (MachineRole == "Admin" || MachineRole == "PnA")
                {

                    DataTable dtBESummary_Current = new System.Data.DataTable();
                    DataTable dtBESummary_Future = new System.Data.DataTable();

                    DataTable dtBEORC = new System.Data.DataTable();
                    DataTable dtBESAP = new System.Data.DataTable();
                    DataSet dtSales = new System.Data.DataSet();

                    DataTable dtReadMe = new System.Data.DataTable();
                    DataTable dtFin_Summary = new System.Data.DataTable();
                    DataTable dtPipelineOpen = new System.Data.DataTable();
                    DataTable dtPipelineClose = new System.Data.DataTable();
                    DataTable dtDigitalTagged = new System.Data.DataTable();
                    DataTable dtActuals = new System.Data.DataTable();
                    dsORC = service.EAS_SP_BEReport_Sales_Current(userid, "ORC", "BE");
                    dsSAP = service.EAS_SP_BEReport_Sales_Current(userid, "SAP", "BE");

                    //dtBEORC = dsORC.Tables[0];
                    //dtBESAP = dsSAP.Tables[0];
                    //dtBEORC.Merge(dtBESAP);

                    dtBESummary_Current = dsORC.Tables[0];
                    dtBESummary_Current.Merge(dsSAP.Tables[0]);

                    dsORC = service.EAS_SP_BEReport_Sales_Future(userid, "ORC", "BE");
                    dsSAP = service.EAS_SP_BEReport_Sales_Future(userid, "SAP", "BE");

                    dtBESummary_Future = dsORC.Tables[0];
                    dtBESummary_Future.Merge(dsSAP.Tables[0]);

                    dtSales = service.EAS_SP_BEReport_Sales();
                    dtActuals = dtSales.Tables[0];
                    dtBESummary_Current.Merge(dtActuals);

                    dtDigitalTagged = dtSales.Tables[1];
                    dtPipelineOpen = dtSales.Tables[2];
                    dtPipelineClose = dtSales.Tables[3];
                    dtFin_Summary = dtSales.Tables[4];
                    dtReadMe = dtSales.Tables[5];

                    if (dtBESummary_Current == null || dtBESummary_Current.Rows.Count == 0)
                    {
                        lbl.Text = "";
                        Session["key"] = null;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                        return;
                    }

                    string fileName = "Americas_Sales_Dashboard" + ".xlsm";

                    string PreviousQtr, CurrentQtr, FutureQtr = string.Empty;
                    int month = System.DateTime.Now.Month;
                    string Year = System.DateTime.Now.Year.ToString();
                    int yr = Convert.ToInt32(Year.Substring(2, 2));

                    yr = month == 1 || month == 2 || month == 3 ? yr : yr + 1;

                    PreviousQtr = month == 4 || month == 5 || month == 6 ? "Q4" + "'" + Convert.ToInt32(yr - 1) : month == 7 || month == 8 || month == 9 ? "Q1" + "'" + yr : month == 10 || month == 11 || month == 12 ? "Q2" + "'" + yr : "Q3" + "'" + yr;
                    CurrentQtr = month == 4 || month == 5 || month == 6 ? "Q1" + "'" + yr : month == 7 || month == 8 || month == 9 ? "Q2" + "'" + yr : month == 10 || month == 11 || month == 12 ? "Q3" + "'" + yr : "Q4" + "'" + yr;
                    FutureQtr = month == 4 || month == 5 || month == 6 ? "Q2" + "'" + yr : month == 7 || month == 8 || month == 9 ? "Q3" + "'" + yr : month == 10 || month == 11 || month == 12 ? "Q4" + "'" + yr : "Q1" + "'" + Convert.ToInt32(yr + 1);

                    Revenue_Sales(dtBESummary_Current, dtBESummary_Future, dtFin_Summary, dtPipelineOpen, dtPipelineClose, dtDigitalTagged, dtReadMe, fileName, PreviousQtr, CurrentQtr, FutureQtr);

                    Generate_SL(fileName, "ORC", CurrentQtr, FutureQtr);
                    Generate_SL(fileName, "SAP", CurrentQtr, FutureQtr);

                    //Session["key"] = fileName;
                    loading.Style.Add("visibility", "visible");
                    lbl.Text = "Generated";
                    up.Update();
                    //iframe.Attributes.Add("src", "Download.aspx");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('You are not authorized to download the report!');</script>");
                    return;
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


        private string Generate_SL(string fileName, string ServiceLine, string CurrentQtr, string FutureQtr)
        {
            string ddl_SL = ServiceLine;
            fileName = ddl_SL + " Americas_Sales_Dashboard" + ".xlsm";
            string folder = "BE_Sales";
            string folder_Macro = "ExcelOperations";
            var myDir = new DirectoryInfo(Server.MapPath(folder));
            var myDir_Macro = new DirectoryInfo(Server.MapPath(folder_Macro));

            //string path = Server.MapPath("~/BE_Sales/" + dtdate + "/" + filepath);
            string path = Server.MapPath("~/BE_Sales/" + fileName);
            Microsoft.Office.Interop.Excel.Application oExcel = null;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            Microsoft.Office.Interop.Excel.Sheets ws = null;


            VBIDE.VBComponent oModule;
            String sCode;
            Object oMissing = System.Reflection.Missing.Value;

            //instance of excel
            oExcel = new Microsoft.Office.Interop.Excel.Application();
            string templatePath = Server.MapPath(folder) + "\\" + "Americas_Sales_Dashboard.xlsm" + "";
            oBook = oExcel.Workbooks.
                Open(templatePath, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            ws = oBook.Sheets;


            oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            sCode = "sub Macro()\r\n" +
                    GetVariableDeclaration("SL", MacroDataType.String, ddl_SL) +
                    GetVariableDeclaration("C", MacroDataType.String, CurrentQtr) +
                    GetVariableDeclaration("F", MacroDataType.String, FutureQtr) +
                System.IO.File.ReadAllText(myDir_Macro.FullName + "\\HeaderMacroSales.txt") +
                    "\nend sub";
            oModule.CodeModule.AddFromString(sCode);
            oExcel.GetType().InvokeMember("Run",
                            System.Reflection.BindingFlags.Default |
                            System.Reflection.BindingFlags.InvokeMethod,
                            null, oExcel, new string[] { "Macro" });

            //oBook.Permission.Enabled = true;
            //oBook.Permission.RemoveAll();
            //string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
            //DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
            //DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
            //UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
            //userper.ExpirationDate = dtExpireDate;

            //oExcel.DisplayAlerts = true;




            //oBook.SaveAs(Server.MapPath(folder) + "\\" + filepath);
            oBook.SaveAs(path);
            oBook.Close(false, templatePath, null);



            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);
            ws = null;
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
            oBook = null;

            oExcel.Quit();
            oExcel = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return fileName;
        }

        protected void Revenue_Sales(DataTable dtBE_Summary_Current, DataTable dtBE_Summary_Future, DataTable dtFin_Summary, DataTable dtPipelineOpen, DataTable dtPipelineClose, DataTable dtDigitalTagged, DataTable dtReadMe, string filename,string PreviousQtr, string CurrentQtr, string FutureQtr)
        {

            string ddl_SL = "EAS";
           
            
            string folder = "ExcelOperations";
            var myDir = new DirectoryInfo(Server.MapPath(folder));

            //string path = Server.MapPath("~/BE_Sales/" + dtdate + "/" + filepath);
            string path = Server.MapPath("~/BE_Sales/"+ filename);
            Microsoft.Office.Interop.Excel.Application oExcel = null;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            Microsoft.Office.Interop.Excel.Sheets ws = null;

            Excel.Worksheet EAS_BE_Data_Current = null;
            Excel.Worksheet EAS_BE_Data_Future = null;
            Excel.Worksheet EAS_BE_Finpulse_Data = null;
            Excel.Worksheet EAS_BE_PipelineOpen_Data = null;
            Excel.Worksheet EAS_BE_PipelineClose_Data = null;
            Excel.Worksheet EAS_BE_DigitalTagged = null;
            Excel.Worksheet EAS_ReadMe = null; 
            VBIDE.VBComponent oModule;
            String sCode;
            Object oMissing = System.Reflection.Missing.Value;

            //instance of excel
            oExcel = new Microsoft.Office.Interop.Excel.Application();
            string templatePath = Server.MapPath(folder) + "\\" + "BEReports_Americas_Template.xlsm" + "";
            oBook = oExcel.Workbooks.
                Open(templatePath, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            ws = oBook.Sheets;

            EAS_BE_Data_Current = ws.Item["BE_Summary_Data_Current"];
            EAS_BE_Data_Future = ws.Item["BE_Summary_Data_Future"];
            EAS_BE_Finpulse_Data = ws.Item["Fin_Summary_Data"];
            EAS_BE_PipelineOpen_Data = ws.Item["OpenOpp_Data"];
            EAS_BE_PipelineClose_Data = ws.Item["ClosedOpp_data"];
            EAS_BE_DigitalTagged = ws.Item["DigitalTagged_Data"];
            EAS_ReadMe = ws.Item["Read Me"];

            FillExcelSheet(dtBE_Summary_Current, EAS_BE_Data_Current);
            FillExcelSheet(dtBE_Summary_Future, EAS_BE_Data_Future);

            FillExcelSheet(dtFin_Summary, EAS_BE_Finpulse_Data);
            FillExcelSheet(dtPipelineOpen, EAS_BE_PipelineOpen_Data);
            FillExcelSheet(dtPipelineClose, EAS_BE_PipelineClose_Data);
            FillExcelSheet(dtDigitalTagged, EAS_BE_DigitalTagged);
            FillExcelSheet1(dtReadMe, EAS_ReadMe);
            //RefreshPivots(ws);

       
            oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            sCode = "sub Macro()\r\n" +
                    GetVariableDeclaration("SL", MacroDataType.String, ddl_SL) +
                    GetVariableDeclaration("P", MacroDataType.String, PreviousQtr) +
                    GetVariableDeclaration("C", MacroDataType.String, CurrentQtr) +
                    GetVariableDeclaration("F", MacroDataType.String, FutureQtr) +
                System.IO.File.ReadAllText(myDir.FullName + "\\HeaderMacroSales.txt") +
                    "\nend sub";
            oModule.CodeModule.AddFromString(sCode);
            oExcel.GetType().InvokeMember("Run",
                            System.Reflection.BindingFlags.Default |
                            System.Reflection.BindingFlags.InvokeMethod,
                            null, oExcel, new string[] { "Macro" });

            //oBook.Permission.Enabled = true;
            //oBook.Permission.RemoveAll();
            //string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
            //DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
            //DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
            //UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
            //userper.ExpirationDate = dtExpireDate;

            //oExcel.DisplayAlerts = true;


            

            //oBook.SaveAs(Server.MapPath(folder) + "\\" + filepath);
            oBook.SaveAs(path);
            oBook.Close(false, templatePath, null);

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_ReadMe);
            EAS_ReadMe = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_BE_DigitalTagged);
            EAS_BE_DigitalTagged = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_BE_PipelineClose_Data);
            EAS_BE_PipelineClose_Data = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_BE_PipelineOpen_Data);
            EAS_BE_PipelineOpen_Data = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_BE_Finpulse_Data);
            EAS_BE_Finpulse_Data = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_BE_Data_Future);
            EAS_BE_Data_Future = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EAS_BE_Data_Current);
            EAS_BE_Data_Current = null;

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);
            ws = null;
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
            oBook = null;

            oExcel.Quit();
            oExcel = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();



        }

        enum MacroDataType
        {
            String, Integer

        }
        private string GetVariableDeclaration(string variableName, MacroDataType type, object value)
        {
            string formatString = "Dim {0} as {1} \n {0} =\"{2}\" \n";
            string formatNumber = "Dim {0} as {1} \n {0} ={2} \n";
            string returnValue = "";
            switch (type)
            {
                case MacroDataType.String:
                    returnValue = string.Format(formatString, variableName, type.ToString(), value);
                    break;
                case MacroDataType.Integer:
                    returnValue = string.Format(formatNumber, variableName, type.ToString(), value);
                    break;
                default:
                    break;
            }
            return returnValue;
        }

        public void FillExcelSheet(DataTable dt, Microsoft.Office.Interop.Excel.Worksheet excel)
        {

            try
            {
                // Copy the DataTable to an object array
                object[,] rawData = new object[dt.Rows.Count, dt.Columns.Count];

                // Copy the column names to the first row of the object array
                //for (int col = 0; col < dt.Columns.Count; col++)
                //{
                //    rawData[0, col] = dt.Columns[col].ColumnName;
                //}
                // Copy the values to the object array

                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        // rawData[row + 1, col] = dt.Rows[row].ItemArray[col];
                        rawData[row, col] = dt.Rows[row][col];
                    }
                }

                Microsoft.Office.Interop.Excel.Range c1;
                Microsoft.Office.Interop.Excel.Range c2;
                Microsoft.Office.Interop.Excel.Range range_excel;


                c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[2, 1];
                c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                range_excel = excel.get_Range(c1, c2);



                //Fill Array in Excel
                range_excel.Value2 = rawData;
                range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                range_excel.Interior.Pattern = Microsoft.Office.Interop.Excel.XlPattern.xlPatternSolid;
                range_excel.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);


                ReleaseObject(range_excel);
                ReleaseObject(c2);
                ReleaseObject(c1);
                ReleaseObject(excel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillExcelSheet1(DataTable dt, Microsoft.Office.Interop.Excel.Worksheet excel)
        {

            try
            {
               
                object[,] rawData = new object[dt.Rows.Count, dt.Columns.Count];
       
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        // rawData[row + 1, col] = dt.Rows[row].ItemArray[col];
                        rawData[row, col] = dt.Rows[row][col];
                    }
                }

                Microsoft.Office.Interop.Excel.Range c1;
                Microsoft.Office.Interop.Excel.Range c2;
                Microsoft.Office.Interop.Excel.Range range_excel;


                c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[5, 6];
                c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[10, 6];
                range_excel = excel.get_Range(c1, c2);



                //Fill Array in Excel
                range_excel.Value2 = rawData;
                range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                range_excel.Interior.Pattern = Microsoft.Office.Interop.Excel.XlPattern.xlPatternSolid;
                range_excel.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);


                ReleaseObject(range_excel);
                ReleaseObject(c2);
                ReleaseObject(c1);
                ReleaseObject(excel);
            }
            catch (Exception ex)
            {
                throw ex;
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

        public void RefreshPivots(Microsoft.Office.Interop.Excel.Sheets excelsheets)
        {

            foreach (Microsoft.Office.Interop.Excel.Worksheet pivotSheet in excelsheets)
            {
                Microsoft.Office.Interop.Excel.PivotTables pivotTables = (Microsoft.Office.Interop.Excel.PivotTables)pivotSheet.PivotTables();
                int pivotTablesCount = pivotTables.Count;
                if (pivotTablesCount > 0)
                {
                    for (int i = 1; i <= pivotTablesCount; i++)
                    {
                        Microsoft.Office.Interop.Excel.PivotTable pivotTable = pivotTables.Item(i);
                        pivotTable.RefreshTable();
                    }
                }
            }
        }

        private void BEReport_PDF_Sales(string dtdate)
        {
            bool file = false;
           
            // Session["dtdate"] = dtdate;
            var directory = new DirectoryInfo(Server.MapPath("BE_Sales"));

            foreach (var f in directory.GetFiles())
            {
                if ((f.Name.Contains("ORC") && f.Name.Contains(dtdate)) || (f.Name.Contains("SAP") && f.Name.Contains(dtdate)))
                {
                    file = true;
                }
            }

            List<string> SL = new List<string>();
            SL.Add("SAP");
            SL.Add("ORC");
            // ReportViewer1.Visible = true;

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();//Web Address of your report server (ex: http://rserver/reportserver (http://rserver/reportserver)) 

            ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://nebula:1212/ReportServer"); // Report Server URL

            ReportViewer1.ServerReport.ReportPath = "/DashboardReportsTest/EAS_SnapShots-RegionWise/Mainreport";

            ReportParameter[] param = new ReportParameter[3];
            DateTime dt1 = Convert.ToDateTime(dtdate);

            if (!Directory.Exists(Server.MapPath("~/BE_Sales/" + dt1.ToString("dd-MMM-yyyy"))))
            {
                Directory.CreateDirectory(Server.MapPath("~/BE_Sales/" + dt1.ToString("dd-MMM-yyyy")));
            }
            else
            {
                Array.ForEach(Directory.GetFiles(Server.MapPath("~/BE_Sales/" + dt1.ToString("dd-MMM-yyyy"))), File.Delete);  
            }

            foreach (string item in SL)
            {
                string ServiceLine = item.ToString();


                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];
                reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
                reportParameterCollection[0].Name = "PL";                                                         //Parameter Name
                reportParameterCollection[0].Values.Add(ServiceLine);                                                 //Parameter Value
                reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
                reportParameterCollection[1].Name = "value";                                                         //Parameter Name
                reportParameterCollection[1].Values.Add("DPSandPreSalesOppValKUSD");
                reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter();//Parameter Value
                reportParameterCollection[2].Name = "RegionGroup";                                                         //Parameter Name
                reportParameterCollection[2].Values.Add(ddlRegion.SelectedValue);
                ReportViewer1.ServerReport.SetParameters(reportParameterCollection);
                ReportViewer1.ServerReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, extension, deviceInfo;

                deviceInfo = "True";

                byte[] bytes = ReportViewer1.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                byte[] words = ReportViewer1.ServerReport.Render("Word", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                //string PL = ddlPL.SelectedValue;

                string PL = ServiceLine;
                Session["data"] = bytes;
                Session["Data2"] = words;

                //string Date = DateTime.Now.ToShortDateString();

                //DateTime dt = Convert.ToDateTime(Date);

                //Date = dt.ToString("dd-MMM-yyyy");
                DateTime dtnew = Convert.ToDateTime(dtdate);
                string pdfPath = Server.MapPath("~/BE_Sales/" + dtnew.ToString("dd-MMM-yyyy") + "/" + PL + "_PipeLine - " + dtdate + " - v1.0.pdf");
                //string wordPath = Server.MapPath("~/BE_Sales/" + dtnew.ToString("dd-MMM-yyyy") + "/" + PL + "_PipeLine - " + dtdate + " - v1.0.doc");// Path to export Report.

                System.IO.FileStream pdfFile = new System.IO.FileStream(pdfPath, System.IO.FileMode.Create);
                //System.IO.FileStream wordFile = new System.IO.FileStream(wordPath, System.IO.FileMode.Create);

                // Response.ContentType = "application/msword";
                pdfFile.Write(bytes, 0, bytes.Length);
                pdfFile.Close();
                //wordFile.Write(words, 0, words.Length);
                //wordFile.Close();

            }

            generateSummaryReport(SL, dtdate);
            ReportViewer1.Visible = false;
           
        }

        public void generateSummaryReport(List<string> SL, string dtdate)
        {
            

          
            foreach (string item in SL)
            {
                string ServiceLine = item.ToString();
                ReportViewer1.ServerReport.ReportPath = "/DashboardReportsTest/EAS_SnapShots-RegionWise/EAS Open Opportunity Summary Report_test";
                Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection1 = new Microsoft.Reporting.WebForms.ReportParameter[2];
                reportParameterCollection1[0] = new Microsoft.Reporting.WebForms.ReportParameter();
                reportParameterCollection1[0].Name = "PL";
                reportParameterCollection1[0].Values.Add(ServiceLine);
                reportParameterCollection1[1] = new Microsoft.Reporting.WebForms.ReportParameter();
                reportParameterCollection1[1].Name = "RegionGroup";
                reportParameterCollection1[1].Values.Add(ddlRegion.SelectedValue);

                ReportViewer1.ServerReport.SetParameters(reportParameterCollection1);
                ReportViewer1.ServerReport.Refresh();
                Warning[] warningseas;
                string[] streamidseas;
                string mimeTypeeas, encodingeas, extensioneas, deviceInfoeas;

                deviceInfoeas = "True";
                string PL = ServiceLine;
                //string EAS = string.Join("_", SL);
                byte[] byteseas = ReportViewer1.ServerReport.Render("PDF", null, out mimeTypeeas, out encodingeas, out extensioneas, out streamidseas, out warningseas);
                //byte[] wordseas = ReportViewer1.ServerReport.Render("Word", null, out mimeTypeeas, out encodingeas, out extensioneas, out streamidseas, out warningseas);
                DateTime dtnew = Convert.ToDateTime(dtdate);
                string pdfPathEAS = Server.MapPath("~/BE_Sales/" + dtnew.ToString("dd-MMM-yyyy") + "/" + PL + "_Open_Opps_Snapshot - " + dtdate + " - v1.0.pdf");
               // string wordPathEAS = Server.MapPath("~/BE_Sales/" + dtnew.ToString("dd-MMM-yyyy") + "/" + SL + "_Open_Opps_Snapshot - " + dtdate + " - v1.0.doc");// Path to export Report.             
                System.IO.FileStream pdfFileEAS = new System.IO.FileStream(pdfPathEAS, System.IO.FileMode.Create);
                //System.IO.FileStream wordFileEAS = new System.IO.FileStream(wordPathEAS, System.IO.FileMode.Create);
                // Response.ContentType = "application/msword";
                pdfFileEAS.Write(byteseas, 0, byteseas.Length);
                pdfFileEAS.Close();
               // wordFileEAS.Write(wordseas, 0, wordseas.Length);
                //wordFileEAS.Close();
                
        }

        }

        protected void btn_BE_Pipeline_SalesDownload_Click(object sender, EventArgs e)
        {
            download_BE_Pipeline();
        }

        protected void btn_BE_SalesDownload_Click(object sender, EventArgs e)
        {
            download_BE();
        }

        public void download_BE_Pipeline()
        {
            Array.ForEach(Directory.GetFiles(Server.MapPath("~/BE_Sales/"), "*.*", SearchOption.AllDirectories).Where(d => !d.Contains("Americas_Sales_Dashboard")).ToArray(), File.Delete);

            DataTable dt = new DataTable();
            SqlCommand cmdParam = new SqlCommand("select max(dumpdate) from PipelineReports");
            dt = DA.ExecuteSP(cmdParam);
            string dtdate = Convert.ToDateTime(dt.Rows[0][0]).ToString("dd-MMM-yyyy");

            string pathPDF = @"D:\DefaultApplication\PIPELINE_HOME\PIPELINE_PROD\Pipeline_pdf_RegionWise\" + dtdate + @"\";
            IEnumerable<string> filesPDF = Directory.GetFiles(pathPDF);

            string pathBE = Server.MapPath("~/BE_Sales/" );
            foreach (string filedetails in filesPDF)
            {

                var ext = System.IO.Path.GetExtension(filedetails);

                string FileName = System.IO.Path.GetFileNameWithoutExtension(filedetails);

                File.Copy(pathPDF + FileName + ext, pathBE + FileName + ext);
            }


            IEnumerable<string> files = Directory.GetFiles(pathBE);
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("BE_Reports_Sales_" + ddlRegion.SelectedItem.Text );
                List<string> DirFile = new List<string>();
                foreach (string file in files)
                {
                    var ext = System.IO.Path.GetExtension(file);
                    var str = System.IO.Path.GetFileNameWithoutExtension(file);
                    zip.AddFile(pathBE + str + ext, "BE_Reports_Sales_" + ddlRegion.SelectedItem.Text);
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("{0}.zip", "BE_Reports_Sales_" + ddlRegion.SelectedItem.Text);
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
            }
            Response.End();
        }

        public void download_BE()
        {
            Array.ForEach(Directory.GetFiles(Server.MapPath("~/BE_Sales/"), "*.*", SearchOption.AllDirectories).Where(d => !d.Contains("Americas_Sales_Dashboard")).ToArray(), File.Delete);

            string pathBE = Server.MapPath("~/BE_Sales/");

            IEnumerable<string> files = Directory.GetFiles(pathBE);
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("BE_Reports_Sales_" + ddlRegion.SelectedItem.Text);
                List<string> DirFile = new List<string>();
                foreach (string file in files)
                {
                    var ext = System.IO.Path.GetExtension(file);
                    var str = System.IO.Path.GetFileNameWithoutExtension(file);
                    zip.AddFile(pathBE + str + ext, "BE_Reports_Sales_" + ddlRegion.SelectedItem.Text);
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("{0}.zip", "BE_Reports_Sales_" + ddlRegion.SelectedItem.Text);
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);

            Response.End();
        }
    }
}