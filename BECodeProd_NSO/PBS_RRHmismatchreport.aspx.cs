using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using BEData;
using Excel = Microsoft.Office.Interop.Excel;
using VBIDE = Microsoft.Vbe.Interop;

namespace BECodeProd
{
    public partial class PBS_RRHmismatchreport : System.Web.UI.Page
    {
        public string fileName = "BEData.BETrendReport.cs";
        private BEDL service = new BEDL();
        BEDL objbe = new BEDL();
        Logger logger = new Logger();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string cmdtext1 = "EXEC dbo.[SPROC_GetServiceLine]";
                DataSet dsn1 = new DataSet();
                dsn1 = service.GetDataSet(cmdtext1);

                ddlServiceLine.DataTextField = "txtserviceline";
                ddlServiceLine.DataValueField = "txtserviceline";
                ddlServiceLine.DataSource = dsn1;
                ddlServiceLine.DataBind();
            }

        }
        protected void btnNewProjectList_Click(object sender, EventArgs e)
        {
            string ServiceLine=string.Empty;

            ServiceLine = ddlServiceLine.SelectedItem.ToString();

            string folderadress = @"~/ExcelOperations\PBS_RRH_Mismatch_Report_template.xlsx";
            folderadress = HttpContext.Current.Server.MapPath(folderadress);
            string storefolderadress = @"~/Template";
            storefolderadress = HttpContext.Current.Server.MapPath(storefolderadress);

            Microsoft.Office.Interop.Excel.Application WRExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks WRwbs = null;
            //Microsoft.Office.Interop.Excel.Workbook WRwb = new Microsoft.Office.Interop.Excel.Workbook();
            object objOpt = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook WRwb = WRExcel.Workbooks.Add(objOpt);
            Microsoft.Office.Interop.Excel._Worksheet WRws = null;

            string folder = "ExcelOperations";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            WRExcel.Visible = false;
            WRwbs = WRExcel.Workbooks;


            WRwb = WRwbs.Open(folderadress, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt,
                objOpt, objOpt, objOpt, objOpt, objOpt, objOpt);
            Microsoft.Office.Interop.Excel.Sheets WRss = null;
            WRss = WRwb.Sheets;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("PBS_RRH_Mismatch_Report");

            try
            {
                //string userid = Session["UserID"] + "";
                string cmdtext = "EXEC dbo.[SPROC_PBS_RRH_Mismatch_Report] '" + ServiceLine + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmdtext);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    //string cmdtext1 = "EXEC dbo.[sp_betrend_maxweek] '" + quarter + "','" + year + "'";
                    //DataSet dsn1 = new DataSet();
                    //dsn1 = service.GetDataSet(cmdtext1);
                    //DataTable dtn1 = new DataTable();
                    //dtn1 = dsn1.Tables[0];
                    //DataTable dtn2 = new DataTable();
                    //dtn2 = dsn1.Tables[1];
                    //var tblProjectDownload0 = dt;

                    //string folderadress = @"~/ExcelOperations\PBS_RRH_Mismatch_Report_template.xlsx";
                    //folderadress = HttpContext.Current.Server.MapPath(folderadress);
                    //string storefolderadress = @"~/Template";
                    //storefolderadress = HttpContext.Current.Server.MapPath(storefolderadress);

                    string filename = "PBS_RRH_Mismatch_Report.xlsx";
                    String excelFile1 = "~\\ExcelOperations\\PBS_RRH_Mismatch_Report.xlsx";
                    String destPath = Server.MapPath(excelFile1);

                    // Microsoft.Office.Interop.Excel.Worksheet excelSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("PBS_RRH_Mismatch_Report");
                    //Microsoft.Office.Interop.Excel.Worksheet excelSheetvol = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("Volume");
                    //Microsoft.Office.Interop.Excel.Worksheet excelSheetRev = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("Revenue");
                    //Microsoft.Office.Interop.Excel.Worksheet excelSheetMAxweek = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("BE Max Week");
                    //excelSheetvol.Cells[1, 7] = quarter + " " + year + " BE Volume Trend";
                    //excelSheetRev.Cells[1, 7] = quarter + " " + year + " BE Revenue Trend";
                    //excelSheetMAxweek.Cells[1, 1] = dtn1.Rows[0][0].ToString();

                    //for (int i = 1; i <= dtn2.Rows.Count; i++)
                    //{
                    //    excelSheetMAxweek.Cells[i + 1, 2] = dtn2.Rows[i - 1][0].ToString();
                    //}
                    //excelSheetvol.PivotTableUpdate

                    //excelSheetvol.Cells[26, 1] = "w0";
                    //var wsPivot = excelSheetRev.PivotTables("PivotTable2");
                    //excelSheetRev.PivotTables("PivotTable2")[]

                    //Excel.PivotField fieldA = pvttable.PivotFields("Answer");
                    //excelSheetvol.Cells.PivotTable.GetData("Week");
                    //excelSheetvol.Cells[1,36] = "w0";
                    int ColumnIndex = 0;
                    int rowIndex = 0;
                    DataTable dt1 = new DataTable();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        excelSheet1.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;

                    }



                    int rows = dt.Rows.Count;
                    int columns = dt.Columns.Count;
                    int r = 0; int c = 0;
                    object[,] DataArray = new object[rows + 1, columns + 1];
                    for (c = 0; c <= columns - 1; c++)
                    {
                        DataArray[r, c] = dt.Columns[c].ColumnName;
                        for (r = 0; r <= rows - 1; r++)
                        {
                            DataArray[r, c] = dt.Rows[r][c];
                        } //end row loop
                    } //end column loop

                    Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)excelSheet1.Cells[2, 1];
                    Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)excelSheet1.Cells[1 + dt.Rows.Count, dt.Columns.Count];
                    Microsoft.Office.Interop.Excel.Range range = excelSheet1.get_Range(c1, c2);

                    //Fill Array in Excel
                    range.Value2 = DataArray;
                    WRwb.RefreshAll();
                    //excelSheet1.Name = "PBS_RRH_Mismatch_Report";
                    //System.Threading.Thread.Sleep(20000);

                    //excelSheet1.Acti;

                    //excelSheet1.get_Range("A1", "AE1").AutoFilter(1, objOpt, Excel.XlAutoFilterOperator.xlAnd, objOpt, true);

                    //excelSheet1.Protect(4321, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, 1, 1, objOpt);
                    //excelSheetvol.Protect(4321, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, 1, 1, objOpt);
                    //excelSheetRev.Protect(4321, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, 1, 1, objOpt);

                    excelSheet1.get_Range("G2", "G" + dt.Rows.Count + 1).NumberFormat = "MM/DD/YYYY";
                    excelSheet1.get_Range("H2", "H" + dt.Rows.Count + 1).NumberFormat = "MM/DD/YYYY";
                    excelSheet1.get_Range("V2", "V" + dt.Rows.Count + 1).NumberFormat = "MM/DD/YYYY";
                    excelSheet1.get_Range("AJ2", "AJ" + dt.Rows.Count + 1).NumberFormat = "MM/DD/YYYY";
                    RefreshPivots(WRss);
                    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    {
                        System.IO.File.Delete(destPath);
                    }
                    WRwb.SaveCopyAs(destPath);



                    //WRwb.Save();


                    //WRwb.Close(null, null, null);
                    //WRExcel.Quit();
                    //WRExcel = null;
                    //objOpt = null;
                    //WRwb = null;

                    //GC.Collect();
                    //if (File.Exists(destPath))
                    //{
                    //    FileInfo objFileInfo;
                    //    objFileInfo = new FileInfo(destPath);
                    //    string filefullpath = "~\\ExcelOperations\\EAS_BEvsActuals_trend.xls";
                    //    Response.Clear();

                    //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "EAS_BEvsActuals_trend.xls");

                    //    Response.ContentType = "application/vnd.xls";

                    //    Response.WriteFile(filefullpath);

                    //    Response.Flush();

                    //    //Response.Close();
                    //    Response.End();
                    //}
                    WRwb.Close(false);
                    WRExcel.Quit();

                    DownloadFileBEReport(ServiceLine);
                    //DownloadFileProject();
                }
                else
                {
                    lbl.Text = "";
                    Session["key"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
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
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRwb);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRwbs);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRws);
            }
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


                        pivotTables.Item(i).RefreshTable();

                    }
                }
            }
        }
        private void DownloadFileBEReport(string ServiceLine)
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
                oBook = oExcel.Workbooks.Add(1);


                FileInfo file = new FileInfo(MyDir.FullName + "\\PBS_RRH_Mismatch_Report.xlsx");

                oBook = oExcel.Workbooks.Open(file.ToString(), 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);



                //oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
                string finalname = "";
                   

                if (ddlServiceLine.SelectedValue == "All")
                {
                    finalname = "PBS_RRH_Mismatch_Report As on_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";
                }
                else if (ddlServiceLine.SelectedValue == "ORC")
                {
                    finalname = "PBS_RRH_Mismatch_Report ORC As on_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";
                }
                else if (ddlServiceLine.SelectedValue == "SAP")
                {
                    finalname = "PBS_RRH_Mismatch_Report SAP As on_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";
                }


                // string finalname = "RevenueMomentum_rupali03_07Aug2015_1052.xlsx";

             
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);


                oBook.SaveCopyAs(MyDir.FullName + "\\" + finalname);
                //oBook.Save();

                oBook.Close(false);
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();
                DownloadFileProject(finalname);


            }
            catch (Exception ex)
            {

            }

          
        }
        private void DownloadFileProject(string finalname)
        {
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            oExcel = new Excel.Application();
            bool forceDownload = true;
            //string path = MapPath(fname);
            string folder = "ExcelOperations";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));
            oBook = oExcel.Workbooks.
            Open(MyDir.FullName + "\\" + finalname + "", 0, false, 5, "", "", true,
             Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            try
            {
                


                //String sCode;
                Object oMissing = System.Reflection.Missing.Value;

                //Create an instance of Excel.
             
             


                //oBook.Save();

                Session.Add("key", finalname);
                oBook.Close(false);
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();

                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();
                iframe.Attributes.Add("src", "Download.aspx");

                ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);
             

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

                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRwb);
                
            }

        }
    }
}