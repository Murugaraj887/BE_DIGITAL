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
    public partial class ReportClientCategorization : BasePage
    {
        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public DateTime dateTime = DateTime.Today;
        public string fileName = "Reports";
        static string yr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (!IsPostBack)
            {             
                List<string> lstYear = service.GetAllBEYearFin();
                ddFinYr.DataSource = lstYear.Select(k => k.ToString()).Distinct().ToList();
                ddFinYr.DataBind();

                //string cmdtext1 = "EXEC dbo.[SPROC_GetServiceLine]";
                
                //DataSet dsn1 = new DataSet();
                //dsn1 = service.GetDataSet(cmdtext1);

                //ddlServiceLine.DataTextField = "txtserviceline";
                //ddlServiceLine.DataValueField = "txtserviceline";
                //ddlServiceLine.DataSource = dsn1;
                //ddlServiceLine.DataBind();
                string userid = Session["UserID"].ToString();
                List<string> lstSU = service.GetSUForuser(userid);

                if (lstSU.Count > 1)
                {
                    ddlServiceLine.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                    ddlServiceLine.DataBind();
                    ddlServiceLine.Items.Insert(0, "ALL");
                }
                else if (lstSU.Count == 1)
                {
                    ddlServiceLine.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                    ddlServiceLine.DataBind();

                }
            }
        }
        protected void btnreport_Click(object sender, EventArgs e)
        {
            Session["key"] = null;
            string message = "alert('Invalid Value!')";
           
                
                    string connString = System.Configuration.ConfigurationManager.AppSettings["DemandCaptureConnectionString"];
                    SqlConnection con = new SqlConnection(connString);

                    string finYr = ddFinYr.SelectedItem.Text;
                    string ServiceLine = ddlServiceLine.SelectedValue;
                    string UserId = Session["UserID"].ToString();

               

                string cmdtext = "sp_ClientCategorization";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = cmdtext;
                cmd.Parameters.AddWithValue("@fyr", ddFinYr.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@SU", ServiceLine);
                cmd.Parameters.AddWithValue("@duration", ddlDuration.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@quarter", ddlQuarter.SelectedItem.Text);
                DataSet ds = GetDataSet(cmd);
                DataTable dtClient = ds.Tables[0];

                if (dtClient.Rows.Count > 0)
                {
                    GenerateReport("ClientCategorization_report",dtClient);
                }
                else
                {
                    lbl.Text = "";
                    up.Update();
                    ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Message", "Message()", true);
                    return;
                }
                   
                
            
        }

       

        void GenerateReport(string fname, DataTable dtClient)
        {

            string UserId = Session["UserId"].ToString();

            string folder = "ExcelOperations";
            var myDir = new DirectoryInfo(Server.MapPath(folder));

            string filename = "ClientCategorization_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";
            String excelFile1 = "~\\ExcelOperations\\" + filename;
            String destPath = Server.MapPath(excelFile1);

            if (myDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
            {
                System.IO.File.Delete(destPath);
            }


            Microsoft.Office.Interop.Excel.Application oExcel = null;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            Microsoft.Office.Interop.Excel.Sheets WRss = null;
            VBIDE.VBComponent oModule;
            Excel.Worksheet excel_Data = null;
           
                    String sCode;
                    Object oMissing = System.Reflection.Missing.Value;
                    //instance of excel
                    oExcel = new Microsoft.Office.Interop.Excel.Application();
                    string Path = myDir.FullName + "\\" + fname + "";
                    oBook = oExcel.Workbooks.
                        Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                        Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    WRss = oBook.Sheets;

                    excel_Data = WRss.Item["Data"] as Excel.Worksheet;

                    FillExcelSheet(dtClient, excel_Data);

                   

                    oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
                    sCode = "sub RevenueMomentumMacro()\r\n" + System.IO.File.ReadAllText(myDir.FullName + "\\FillFormulaMacro.txt") + "\nend sub";
                    oModule.CodeModule.AddFromString(sCode);

                    oExcel.GetType().InvokeMember("Run",
                                    System.Reflection.BindingFlags.Default |
                                    System.Reflection.BindingFlags.InvokeMethod,
                                    null, oExcel, new string[] { "RevenueMomentumMacro" });

                    RefreshPivots(WRss);
                    oBook.Activate();
                    oBook.Permission.Enabled = true;
                    oBook.Permission.RemoveAll();
                    string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                    DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                    DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                    UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                    userper.ExpirationDate = dtExpireDate;
                  
                    oExcel.DisplayAlerts = false;
                    oBook.SaveAs(destPath);
                
                    oBook.Close(false, Path, null);

                   
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oModule);
                    oModule = null;       

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel_Data);
                    excel_Data = null;

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

        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                Process.GetProcessById((int)processID).Kill();
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

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

        protected void ddlDuration_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlDuration.SelectedItem.Text == "Quarter")
            {
                SqlDataAdapter sqldata = new SqlDataAdapter();
                ddlQuarter.Visible = true;
                ddlQuarter.Enabled = true;
                DataSet dsn1 = new DataSet();
                string cmd1 = "exec dbo.[GetQuarter_clientCat] '" + ddFinYr.SelectedItem.Text + "'";
                dsn1 = service.GetDataSet(cmd1);
                ddlQuarter.DataSource = dsn1;
                ddlQuarter.DataTextField = "Quarter";
                ddlQuarter.DataValueField = "Quarter";
                ddlQuarter.DataBind();
            }
            else
            {
                ddlQuarter.Enabled = false;
                ddlQuarter.Items.Clear();
                ddlQuarter.Items.Add("NA");
                ddlQuarter.ClearSelection();
                
                
            }
        }
    }
}