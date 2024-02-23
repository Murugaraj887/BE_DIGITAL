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
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.OleDb;
using Microsoft.Office.Core;

namespace BECodeProd
{
    public partial class VolumeGapReport : BasePage
    {
        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public string fileName = "Reports";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();
            //sm.RegisterAsyncPostBackControl(txtAbove);
            //sm.RegisterAsyncPostBackControl(txtBelow);
            if (!IsPostBack)
            {
                int sliderValue = 0;

                // Get the actual value of sliderValue from database
                txtAbove.Text = "0";
                txtBelow.Text = "0";
                SliderExtender1.DataBind();
                SliderExtender2.DataBind();
                
              
                ddlQtr.Items.Clear();
                string currentQtr = DateUtility.GetQuarter("current");
                string nextQtr = DateUtility.GetQuarter("next");
                string nextQtrPlus1 = DateUtility.GetQuarter("next1");
                ddlQtr.Text = currentQtr;
               
                string month1;
                string month2;
                string month3;

                string currqtr = currentQtr.Substring(0, 2);
                if (currqtr == "Q4")
                {
                    month1 = "Jan";
                    month2 = "Feb";
                    month3 = "Mar";
                }
                else if (currqtr == "Q1")
                {
                    month1 = "Apr";
                    month2 = "May";
                    month3 = "Jun";
                }
                else if (currqtr == "Q2")
                {
                    month1 = "Jul";
                    month2 = "Aug";
                    month3 = "Sep";
                }
                else
                {
                    month1 = "Oct";
                    month2 = "Nov";
                    month3 = "Dec";
                }

                ddlQtr.Items.Insert(0, month1);
                ddlQtr.Items.Insert(1, month2);
                ddlQtr.Items.Insert(2, month3);
                ddlQtr.Items.Insert(3, currentQtr);

                string cmd = "exec LatestActualsMonthName";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmd);
                ddlQtr.SelectedValue = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        protected void btnreport_Click(object sender, EventArgs e)
        {
            try
            {
                string qtr = ddlQtr.SelectedItem.Text;
                string FYqtr = "";
                string Fyear = "";
                if (qtr.Length > 3)
                {
                    qtr = ddlQtr.SelectedItem.Text.Substring(0, 2);
                    FYqtr = ddlQtr.Text.Remove(0, 3);
                    if (qtr == "Q4")
                    {
                        Fyear = 20 + Convert.ToString(System.DateTime.Now.Year - 2000 - 1) + '-' + FYqtr;
                    }
                    else
                    {
                        Fyear = 20 + Convert.ToString(System.DateTime.Now.Year - 2000) + '-' + FYqtr;
                    }
                }

                else
                {
                    string currentQtr = DateUtility.GetQuarter("current");
                    FYqtr = currentQtr.Substring(3, 2);
                    Fyear = 20 + Convert.ToString(Convert.ToInt32(FYqtr) - 1) + '-' + FYqtr;
                }
                                
                string rangeAbove = txtAbove.Text;
                string rangeBelow = txtBelow.Text;

                string cmd = "exec EAS_BE_VolumeGapReport'" + qtr + "','" + Fyear + "','" + rangeAbove + "','" + rangeBelow + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmd);

                DataTable dtReport = new DataTable();
                dtReport = ds.Tables[0];
                var tblUserDownload0 = dtReport;

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;

                string userID = Session["UserID"] + "";
                string name = "VolumeGapReport_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";

                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == name) != null)
                    System.IO.File.Delete(MyDir.FullName + name);
                FileInfo file = new FileInfo(MyDir.FullName + "\\" + name);

                int rowcountSheet0 = tblUserDownload0.Rows.Count;
                int colcountSheet0 = tblUserDownload0.Columns.Count;

                if (tblUserDownload0 == null || tblUserDownload0.Rows.Count == 0)
                {
                    lbl.Text = "";
                    Session["key"] = null;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                    ///Page.ClientScript.RegisterStartupScript(this.GetType(), "some", "some()", true);
                    ScriptManager.RegisterClientScriptBlock(this,this.GetType(), "some", "some()", true);
                    return;
                }
                ws = pck.Workbook.Worksheets.Add("VolumeGapReport");
                ws.Cells["A1"].LoadFromDataTable(tblUserDownload0, true);
                var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                ws.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
                ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;

                pck.SaveAs(file);
                pck.Dispose();
                ReleaseObject(pck);
                ReleaseObject(ws);

                GenerateReport(name);
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

        void GenerateReport(string fname)
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
                    Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
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


                Session["key"] = fname;
                //Session["data"] = table;
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();

                iframe.Attributes.Add("src", "Download.aspx");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);

            }

        }


        private void DownloadReport(string name)
        {
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            try
            {
                //Adding permission to excel file//
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));
                oExcel = new Excel.Application();
                oBook = oExcel.Workbooks.
                    Open(MyDir + "\\" + name, 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
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
                oBook.Save();
                oBook.Close();
                oExcel.Quit();
                /////////////////////////////////////

                Session["key"] = name;
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();                
                iframe.Attributes.Add("src", "Download.aspx");
                //ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);
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