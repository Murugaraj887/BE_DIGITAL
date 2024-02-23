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
    public partial class ClientCodePortfolioDump : BasePage
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
                var tblCCPDownload = service.DownloadCCP(userid);


                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));

                string UserId = Session["UserId"].ToString();
                string filename = "ClientCodePortfolioDump_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + filename);


                //using (StreamWriter sw = new StreamWriter(MyDir.FullName + "\\Revenue_Volume_BE_Dump.xls"))
                //{
                //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                //    {
                //        grid1.RenderControl(hw);
                //        //Response.Write(sw.ToString());
                //        ////Response.End(); 
                //    }
                //}



                FileInfo file = new FileInfo(MyDir.FullName + "\\" + filename);

                ExcelPackage pck = new ExcelPackage();

                //Create the worksheet
                // ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue_Volume_BE_Dump");


                ExcelWorksheet ws;
                ExcelWorksheet ws1;

                int rowcount = tblCCPDownload.Rows.Count;
                int colcount = tblCCPDownload.Columns.Count;

                //Create the worksheet
                // if (tableBEREV.Rows.Count > 0)
                {
                    ws = pck.Workbook.Worksheets.Add("ClientCodePortfolioDump");
                    ws.Cells["A1"].LoadFromDataTable(tblCCPDownload, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill = ws.Cells[1, 1, 1, colcount].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws.Cells[1, 1, 1, colcount].Style.Font.Bold = true;
                    ws.Cells[1, 1, rowcount+1, colcount+1].Style.Font.Name = "calibri";
                    ws.Cells[1, 1, rowcount+1, colcount+1].Style.Font.Size = 9;
                    ws.Cells[1, 1, rowcount, colcount].AutoFitColumns();
                    //ws.Cells[
                }


                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                //  ws.Cells["A1"].LoadFromDataTable(tableBEREV, true);
                pck.SaveAs(file);


                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;  filename=DineshReport.xlsx");
                //Response.BinaryWrite(pck.GetAsByteArray());
                pck.Dispose();
                ReleaseObject(pck);
                ReleaseObject(ws);

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


        private void DownloadFile()
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
                    Open(MyDir.FullName + "\\ClientCodePortfolioDump.xlsx", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);



                //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "DineshReport1.xlsx") != null)
                //    System.IO.File.Delete(MyDir.FullName + "\\DineshReport1.xlsx");

                //oBook.SaveCopyAs(MyDir.FullName + "\\DineshReport.xlsx");

                //if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "DECSBITSUtilOutput.xls") == null)
                //    System.IO.File.Delete(MyDir.FullName + "\\DECSBITSUtilOutput.xls");

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




                string path = MyDir.FullName + "\\ClientCodePortfolioDump.xlsx";
                //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                string name = "ClientCodePortfolioDump" + ".xlsx";
                string ext = Path.GetExtension(path);
                string type = "";


                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ClientCodePortfolioDump.xlsx");

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