using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using BEData;
using System.Globalization;
using Microsoft.Office.Core;

namespace BECodeProd
{
    public partial class UserDetails : BasePage
    {
        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public string fileName = "Reports";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();
            try
            {
                string userid = Session["UserID"] + "";

                //var tblUserDownload0 = service.DownloadUserDetails(userid).Tables[0];
                //var tblUserDownload1 = service.DownloadUserDetails(userid).Tables[1];


                string cmdtext = "EXEC dbo.SP_UserDetails '" + userid + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmdtext);
                DataTable dt0 = new DataTable();
                DataTable dt1 = new DataTable();
                dt0 = ds.Tables[0];
                dt1 = ds.Tables[1];

                var tblUserDownload0 = dt0;
                var tblUserDownload1 = dt1;

                string UserId = Session["UserId"].ToString();
                string filename = "User Details_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + filename);

                FileInfo file = new FileInfo(MyDir.FullName + "\\" + filename);

                ExcelPackage pck = new ExcelPackage();

                ExcelWorksheet ws;
                ExcelWorksheet ws1;

                int rowcountSheet0 = tblUserDownload0.Rows.Count;
                int colcountSheet0 = tblUserDownload0.Columns.Count;

                //Create the worksheet
                // if (tableBEREV.Rows.Count > 0)
                {
                    ws = pck.Workbook.Worksheets.Add("User List");
                    ws.Cells["A1"].LoadFromDataTable(tblUserDownload0, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws.Cells[1, 1, rowcountSheet0+1, colcountSheet0+1].Style.Font.Name = "calibri";
                    ws.Cells[1, 1, rowcountSheet0+1, colcountSheet0+1].Style.Font.Size = 9;
                    ws.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                    //ws.Cells[
                }


                int rowcountSheet1 = tblUserDownload1.Rows.Count;
                int colcountSheet1 = tblUserDownload1.Columns.Count;

                //Create the worksheet
                // if (tableBEREV.Rows.Count > 0)
                {
                    ws1 = pck.Workbook.Worksheets.Add("Anchor Access Details");
                    ws1.Cells["A1"].LoadFromDataTable(tblUserDownload1, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill = ws1.Cells[1, 1, 1, colcountSheet1].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws1.Cells[1, 1, 1, colcountSheet1].Style.Font.Bold = true;
                    ws1.Cells[1, 1, rowcountSheet1+1, colcountSheet1+1].Style.Font.Name = "calibri";
                    ws1.Cells[1, 1, rowcountSheet1+1, colcountSheet1+1].Style.Font.Size = 9;
                    ws1.Cells[1, 1, rowcountSheet1, colcountSheet1].AutoFitColumns();
                    //ws.Cells[
                }

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                //  ws.Cells["A1"].LoadFromDataTable(tableBEREV, true);
                pck.SaveAs(file);
                pck.Dispose();
                ReleaseObject(pck);
                ReleaseObject(ws);
                ReleaseObject(ws1);
                GenerateReport(filename);
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

        void GenerateReport(string filename)
        {


            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            //try
            {
                string folder = "ExcelOperations";
                var myDir = new DirectoryInfo(Server.MapPath(folder));

                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();
               

                oBook = oExcel.Workbooks.
                      Open(myDir.FullName + "\\" + filename, 0, false, 5, "", "", true,
                      Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
              
                //Adding permission to excel file//
             
                oBook.Permission.Enabled = true;
                oBook.Permission.RemoveAll();
                string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
                userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////

                oBook.Save();
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                GC.Collect();


                string path = myDir.FullName + "\\" + filename;
               
                string ext = Path.GetExtension(path);
                

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename);

                Response.WriteFile(path);

                Response.Flush();
                Response.End();

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



        private void DownloadFileUser()
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
                    Open(MyDir.FullName + "\\User Details.xlsx", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Adding permission to excel file//
                oBook.Activate();
                oBook.Permission.Enabled = true;
                oBook.Permission.RemoveAll();
                string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
                userper.ExpirationDate = dtExpireDate;
           
                /////////////////////////////////////




                oBook.Save();


                oBook.Close();
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();




                string path = MyDir.FullName + "\\User Details.xlsx";
                string name = "User Details" + ".xlsx";
                string ext = Path.GetExtension(path);
                string type = "";

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=User Details.xlsx");

                Response.WriteFile(path);

                Response.Flush();
                Response.End();

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
    }
}