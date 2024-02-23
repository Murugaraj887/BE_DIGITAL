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
using System.Data.SqlClient;
using Microsoft.Office.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace BECodeProd
{
    public partial class BEDataMainVsBEData_Bk : BasePage
    {
        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public string fileName = "Reports";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();
            if (!IsPostBack)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                //string cmd = "select distinct FinMonth from BESDMComp_BEDataVsBEData_Bk order by FinMonth desc";
                string cmd = "EXEC SPROC_GetMonthYearInOrder"; 
                ds = service.GetDataSet(cmd);
                dt = ds.Tables[0];
                ddlFinpulseMonth.DataSource = dt;
                ddlFinpulseMonth.DataTextField = "FinMonth";
                ddlFinpulseMonth.DataValueField = "FinMonth";
                ddlFinpulseMonth.DataBind();

                string cmd1 = "select distinct SnapShotDateTime from BESDMComp_BEDataVsBEData_Bk order by SnapShotDateTime desc";
                ds1 = service.GetDataSet(cmd1);
                dt1 = ds1.Tables[0];
                ddlSnapshot.DataSource = dt1;
                ddlSnapshot.DataTextField = "SnapShotDateTime";
                ddlSnapshot.DataValueField = "SnapShotDateTime";
                ddlSnapshot.DataBind();

                Session["key"] = null;
            }
        }

        protected void ddlFinpulseMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string finmonth = ddlFinpulseMonth.SelectedItem.Text;
            string cmd = "select distinct SnapShotDateTime from BESDMComp_BEDataVsBEData_Bk where FinMonth='" + finmonth + "' order by SnapShotDateTime desc";
            ds = service.GetDataSet(cmd);
            dt = ds.Tables[0];
            ddlSnapshot.DataSource = dt;
            ddlSnapshot.DataTextField = "SnapShotDateTime";
            ddlSnapshot.DataValueField = "SnapShotDateTime";
            ddlSnapshot.DataBind();
        }

   

        protected void btnreport_Click(object sender, EventArgs e)
        {
            Session["key"] = null;
            GenerateReport("BEDataValidation.xlsx");
            //
            

        }

        void GenerateReport(string fname)
        {


            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            Microsoft.Office.Interop.Excel.Sheets WRss = null;
            Excel.Worksheet excel_AlconVsPBSProjectCode = null;
            Excel.Worksheet excel_BEVsAlconVsPBSMCC = null;
            //try
            {
                string cmdtext = "EAS_BEComp_BEDataVsBEData_Bk_DataDump";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = cmdtext;
                cmd.Parameters.AddWithValue("@finMonth", ddlFinpulseMonth.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Snapshot", ddlSnapshot.SelectedItem.Text);

                DataSet ds = GetDataSet(cmd);
                DataTable dtSDM = ds.Tables[0];
                DataTable dtDM = ds.Tables[1];


                string folder = "ExcelOperations";
                var myDir = new DirectoryInfo(Server.MapPath(folder));

                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();
                string Path = myDir.FullName + "\\" + fname + "";
                oBook = oExcel.Workbooks.
                    Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                 WRss = oBook.Sheets;

                //Adding permission to excel file//
                //Microsoft.Office.Interop.Excel.Worksheet excel_AlconVsPBSProjectCode = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("SDM");
                //Microsoft.Office.Interop.Excel.Worksheet excel_BEVsAlconVsPBSMCC = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("DM");


               excel_AlconVsPBSProjectCode = WRss.Item["SDM"] as Excel.Worksheet;
               excel_BEVsAlconVsPBSMCC = WRss.Item["DM"] as Excel.Worksheet;


               FillExcelSheet(dtSDM, excel_AlconVsPBSProjectCode);
               FillExcelSheet(dtDM, excel_BEVsAlconVsPBSMCC);

               RefreshPivots(WRss);
               string UserId = Session["UserId"].ToString();
               string filename = "BEFinpulseDataValidationReport_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";
                String excelFile1 = "~\\ExcelOperations\\" + filename;
                String destPath = Server.MapPath(excelFile1);

                if (myDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                {
                    System.IO.File.Delete(destPath);
                }


                oBook.Permission.Enabled = true;
                oBook .Permission.RemoveAll();
                string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
                userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////

                oBook.SaveAs(destPath);
                oExcel.DisplayAlerts = false;

                oBook.Close(false, Path, null);

                
               
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel_AlconVsPBSProjectCode);
                excel_AlconVsPBSProjectCode = null;
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel_BEVsAlconVsPBSMCC);
                excel_BEVsAlconVsPBSMCC = null;

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                WRss = null;
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                oBook = null;
                oExcel.Quit();
                oExcel = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Session["key"] = filename;
                //Session["data"] = table;
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();

                iframe.Attributes.Add("src", "Download.aspx");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);

            }

        }

   

        public DataSet GetDataSet(SqlCommand cmd)
        {
            DataSet dsPU = new DataSet();
            string connString = System.Configuration.ConfigurationManager.AppSettings["DemandCaptureConnectionString"];
            using (SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = connString;
                cmd.Connection = conn;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                conn.Open();
                da.Fill(dsPU, "Table");

                return dsPU;
            }
        }

        static void KillExcelProcess(Excel.Application xlApp)
        {
            if (xlApp != null)
            {
                int excelProcessId = 0;
                GetWindowThreadProcessId(xlApp.Hwnd, out excelProcessId);
                Process p = Process.GetProcessById(excelProcessId);
                p.Kill();
                xlApp = null;
            }
        }

        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);
     
        public void FillExcelSheet(DataTable dt, Microsoft.Office.Interop.Excel.Worksheet excel)
        {
            int rowsExcelConsolidated = 0;

            if (excel.Name == "Consolidated Data")
            {
                Microsoft.Office.Interop.Excel.Range xlRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[excel.Rows.Count, 1];
                rowsExcelConsolidated = (int)xlRange.get_End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;
                rowsExcelConsolidated = rowsExcelConsolidated - 1;

                ReleaseObject(xlRange);
            }

            try
            {
                int rows = dt.Rows.Count;
                int columns = dt.Columns.Count;
                int r = 0; int c = 0; int d = 0;
                object[,] DataArray = new object[rows + 1, columns + 1];

                // column headings
                if ((excel.Name != "PUView") && (excel.Name != "SDMView"))
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        excel.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;

                    }
                }
                else
                {
                    rowsExcelConsolidated = 1;
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
                if ((excel.Name == "PUView") || (excel.Name == "SDMView"))
                {
                    //c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[3, 1];
                    //c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[dt.Rows.Count + 2, 1];
                    //range_excel = excel.get_Range(c1, c2);
                    //range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                    c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[3, 2];
                    c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[dt.Rows.Count + 2, 4];
                    range_excel = excel.get_Range(c1, c2);
                    range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGoldenrodYellow);

                    c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[3, 5];
                    c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[dt.Rows.Count + 2, 9];
                    range_excel = excel.get_Range(c1, c2);
                    range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PowderBlue);

                    c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[3, 10];
                    c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[dt.Rows.Count + 2, 12];
                    range_excel = excel.get_Range(c1, c2);
                    range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                    c1 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[3, 13];
                    c2 = (Microsoft.Office.Interop.Excel.Range)excel.Cells[dt.Rows.Count + 2, 13];
                    range_excel = excel.get_Range(c1, c2);
                    range_excel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightCoral);


                    
                }

                
                ReleaseObject(c1);
                ReleaseObject(c2);
                ReleaseObject(range_excel);
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
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(o);
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


                        pivotTables.Item(i).RefreshTable();

                    }
                }
            }
        }


      

    }
}

