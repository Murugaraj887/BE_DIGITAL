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
using Microsoft.Office.Core;
namespace BECodeProd
{
    public partial class NewProjectList : BasePage
    {
        public string fileName = "BEData.NewProjectList.cs";
        private BEDL service = new BEDL();
        BEDL objbe = new BEDL();
        Logger logger = new Logger();

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (Page.IsPostBack)
            { }
            else
            {
                string cmdtext1 = "EXEC dbo.[SPROC_GetServiceLine]";
                DataSet dsn1 = new DataSet();
                dsn1 = service.GetDataSet(cmdtext1);

                ddlServiceLine.DataTextField = "txtserviceline";
                ddlServiceLine.DataValueField = "txtserviceline";
                ddlServiceLine.DataSource = dsn1;
                ddlServiceLine.DataBind();
                lblError.Text = "";
                string MachineRole = Session["MachineRole"].ToString();
                //onload
                //string isValidEntry = Session["Login"].ToString();
                //if (!isValidEntry.Equals("1"))
                //    Response.Redirect("UnAuthorised.aspx");

                //string userID = Session["UserID"] + "";

                if (MachineRole.Equals("Admin") || MachineRole.Equals("UH") || MachineRole.Equals("PnA"))
                {

                    lblMonth.Visible = true;
                    lblYear.Visible = true;
                    ddlMonth.Visible = true;
                    ddlyear.Visible = true;
                    btnNewProjectList.Visible = true;
                    lblError.Visible = false;

                 String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                int yy = datevalue.Year;

                ddlyear.Items.Clear();
                for (int i = 2015; i <= yy; i++)
                {

                    ddlyear.Items.Add(i.ToString());
                }

                string monthName;

                ddlMonth.Items.Clear();
                for (int i = 1; i <13; i++)
                {
                    monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    ddlMonth.Items.Add(monthName);
                }
                }

                else 
                {
                    lblMonth.Visible = false;
                    lblYear.Visible = false;
                    ddlMonth.Visible = false;
                    ddlyear.Visible = false;
                    btnNewProjectList.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "Invalid access."; 
                }

            }
        }

        //code for btnNewProjectList

        protected void btnNewProjectList_Click(object sender, EventArgs e)
        {
            //string monthName = "August";
            //int yy = 2015;

            var year = ddlyear.SelectedValue;
            string monthN = ddlMonth.SelectedValue;

            string UserId = Session["UserID"].ToString();
            string ServiceLine = ddlServiceLine.SelectedValue;

            try
            {
                //string userid = Session["UserID"] + "";
                string cmdtext = "EXEC dbo.EAS_NewProject_list '" + UserId + "','" + ServiceLine + "','" + monthN + "','" + year + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmdtext);
                DataTable dt0 = new DataTable();
                dt0 = ds.Tables[0];
                var tblProjectDownload0 = dt0;
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));
                string userID = Session["UserID"] + "";
               
                string filename = "";
                filename = "NewProjectDetails" + "_" + userID + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";




                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + filename);

                FileInfo file = new FileInfo(MyDir.FullName + "\\" + filename);
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;


                int rowcountSheet0 = tblProjectDownload0.Rows.Count;
                int colcountSheet0 = tblProjectDownload0.Columns.Count;
                if (tblProjectDownload0 == null || tblProjectDownload0.Rows.Count == 0)
                {

                    lbl.Text = "";
                    Session["key"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");                    
                    return;

                }           
                else
                {
                    lblError.Visible = false;

                    ws = pck.Workbook.Worksheets.Add("Project List");
                    ws.Cells["A1"].LoadFromDataTable(tblProjectDownload0, true);
                    var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
                    ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;
                    ws.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                               
                    pck.SaveAs(file);
                    pck.Dispose();
                    ReleaseObject(pck);
                    ReleaseObject(ws);
                    GenerateReport(filename);
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