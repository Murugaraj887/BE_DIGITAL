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
    public partial class BECurrencyCompositionReport : BasePage
    {
        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public string fileName = "Reports";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession(); 
            if (!IsPostBack)
            {
                txtAbove.Text = "0";
                txtBelow.Text = "0";
                SliderExtender1.DataBind();
                SliderExtender2.DataBind();

                string userID = Session["UserID"] + "";

                ddlSU.DataTextField = "SU";
                ddlSU.DataValueField = "SU";
                ddlSU.DataSource = service.GetSUBeReport(userID);
                ddlSU.DataBind();
                ddlSU.Items.Insert(0, "ALL");

                ddlBEQtr.Items.Clear();
                string PrevQtr = DateUtility.GetQuarter("prev");
                string currentQtr = DateUtility.GetQuarter("current");
                string nextQtr = DateUtility.GetQuarter("next");
                string nextQtrPlus1 = DateUtility.GetQuarter("next1");

                ddlBEQtr.Items.Insert(0, PrevQtr);
                ddlBEQtr.Items.Insert(1, currentQtr);
                ddlBEQtr.Items.Insert(2, nextQtr);
                ddlBEQtr.Items.Insert(3, nextQtrPlus1);
                ddlBEQtr.Text = currentQtr;

                ddlActualsQtr.Items.Clear();
                ddlActualsQtr.Items.Insert(0, PrevQtr);
                ddlActualsQtr.Items.Insert(1, currentQtr);
                ddlActualsQtr.Text = PrevQtr;
            }
        }

        protected void btnreport_Click(object sender, EventArgs e)
        {
            try
            {
                string SU = ddlSU.SelectedItem.Text;             
                string FYqtr = "";
                string Fyear = "";

                string BEQtr1 = ddlBEQtr.SelectedItem.Text.Remove(2, 3);
                string BEQtr2 = ddlBEQtr.SelectedItem.Text.Remove(0, 3);
                string BEQtr = BEQtr1 + BEQtr2;

                string ActualsQtr1 = ddlActualsQtr.SelectedItem.Text.Remove(2, 3);
                string ActualsQtr2 = ddlActualsQtr.SelectedItem.Text.Remove(0, 3);
                string ActualsQtr = ActualsQtr1 + ActualsQtr2;

             

                string UserId = Session["UserID"].ToString();

              

                string rangeAbove = txtAbove.Text;
                string rangeBelow = txtBelow.Text;

                string cmd = "exec BECurrencyCompositionMismatchReport'" + UserId + "','" + SU + "','" + BEQtr + "','" + ActualsQtr + "','" + rangeAbove + "','" + rangeBelow + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmd);

                DataTable dtReport = new DataTable();
                dtReport = ds.Tables[0];
                var tblUserDownload0 = dtReport;

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;

                string userID = Session["UserID"] + "";
                string name = "BECurrCompMismatchReport_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";

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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "some", "some()", true);
                    return;
                }
                ws = pck.Workbook.Worksheets.Add("BECurrCompMismatchReport");
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


                DownloadFile(fname);

            }

        }

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

    }
}