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
    public partial class RTBRVSActualsComparison : BasePage
    {
        public string fileName = "BEData.RTBRVSActualsComparison.cs";
        private BEDL service = new BEDL();
        BEDL objbe = new BEDL();
        Logger logger = new Logger();

        string PhysicalPath_Macro = "";
        string PhysicalPath_DownloadFiles = "";
        string PhysicalPath_Template = "";

        private string GetPathAndFileName(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            PhysicalPath_DownloadFiles = Server.MapPath("ExcelOperations\\DownloadFiles");
            PhysicalPath_Template = Server.MapPath("ExcelOperations\\Template");
            PhysicalPath_Macro = Server.MapPath("ExcelOperations\\Macro");

            if (Page.IsPostBack)
            { }
            else
            {
                lblError.Text = "";
                string MachineRole = Session["MachineRole"].ToString();
                string userID = Session["UserID"] + "";
                //onload
                //string isValidEntry = Session["Login"].ToString();
                //if (!isValidEntry.Equals("1"))
                //    Response.Redirect("UnAuthorised.aspx");

                //string userID = Session["UserID"] + "";

                //if (MachineRole.Equals("Admin") || MachineRole.Equals("UH") || MachineRole.Equals("PnA"))
                //{

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
                ddlyear.SelectedValue = yy.ToString();
                string monthName;

                ddlMonth.Items.Clear();
                for (int i = 1; i < 13; i++)
                {
                    monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                    ddlMonth.Items.Add(monthName);
                }

                DateTime Today = (Convert.ToDateTime(sDate.ToString()));
                var mn = datevalue.Month;

                DateTime LastMonthDate = DateTime.Now.AddMonths(-1);
                var lastmomnth = (Convert.ToDateTime(LastMonthDate.ToString()));
                var lastmn = lastmomnth.Month;
                string lastmonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lastmn);
                ddlMonth.SelectedValue = ddlMonth.Items.FindByText(lastmonthName).Value;

                List<string> lstSU = objbe.GetSUForuser(userID);

                if (lstSU.Count > 1)
                {
                    ddlSU.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                    ddlSU.DataBind();
                    ddlSU.Items.Insert(0, "ALL");
                }
                else if (lstSU.Count == 1)
                {
                    ddlSU.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                    ddlSU.DataBind();

                }

                //ddlQuarter.DataTextField = "txtqtr";
                //ddlQuarter.DataValueField = "txtqtr";
                //ddlQuarter.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
                //ddlQuarter.DataBind();
                //}

                //else 
                //{
                //    lblMonth.Visible = false;
                //    lblYear.Visible = false;
                //    ddlMonth.Visible = false;
                //    ddlyear.Visible = false;
                //    btnNewProjectList.Visible = false;
                //    lblError.Visible = true;
                //    lblError.Text = "Invalid access."; 
                //}

            }
        }

        //code for btnNewProjectList

        protected void btnNewProjectList_Click(object sender, EventArgs e)
        {
            //string monthName = "August";
            //int yy = 2015;

            var year = ddlyear.SelectedValue;
            string monthN = ddlMonth.SelectedValue;
            //string quarter = ddlQuarter.SelectedValue;
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            var mn = datevalue.Month;
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mn);
            string fy = year.Substring(2, 2);
           
            string  monthNo;
            switch (monthN)
            {
                case "January":
                    monthNo ="01";
                    break;
                case "February":
                    monthNo ="02";
                    break;
                case "March":
                    monthNo ="03";
                    break;
                case "April":
                    monthNo ="04";
                    break;
                case "May":
                    monthNo = "05";
                    break;
                case "June":
                    monthNo = "06";
                    break;
                case "July":
                    monthNo = "07";
                    break;
                case "August":
                    monthNo = "08";
                    break;
                case "September":
                    monthNo = "09";
                    break;
                case "October":
                    monthNo = "10";
                    break;
                case "November":
                    monthNo = "11";
                    break;
                case "December":
                    monthNo = "12";
                    break;
                 default:
                    monthNo = "0";
                    break;
            }
            int fy1 = Convert.ToInt32(fy);
            int yr = Convert.ToInt32(year);
            if ((monthNo == "01") || (monthNo == "02") || (monthNo == "03"))
            {
                fy1 = fy1 - 1;
                yr = yr - 1;
            }
            int fy2 = fy1 + 1;
            fy = yr.ToString() + "-" + fy2.ToString();

            //switch (monthN)
            //{
            //    case "January":
            //        year = "2016";
            //        break;
            //    case "February":
            //        year = "2016";
            //        break;
            //    case "March":
            //        year = "2016";
            //        break;
                
            //}
          
            monthName = year + monthNo;
            monthN = monthN.Substring(0,3);
            try
            {               
                //string userid = Session["UserID"] + "";
                //string cmdtext = "EXEC dbo.[EAS_SP_RTBR_Actuals_Comp] '" + quarter + "','" + monthName +  "','" + monthN + "','" + fy + "'";
                string cmdtext = "EXEC dbo.[EAS_SP_RTBR_Actuals_Comp_modified_NSO] '" + monthName + "','" + fy + "','"+Session["UserID"].ToString()+"','"+ddlSU.SelectedItem.Text+"'";
                logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, cmdtext, cmdtext); 
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmdtext);
                DataTable dt0 = new DataTable();
                dt0 = ds.Tables[0];
                DataTable dt1 = new DataTable();
                dt1 = ds.Tables[1];
                DataTable dt2 = new DataTable();
                dt2 = ds.Tables[2];
                DataTable dt3 = new DataTable();
                dt3 = ds.Tables[3];
                //if (dt0.Rows.Count == 0)
                //    break;
                var tblProjectDownload0 = dt0;
                var tblProjectDownload1 = dt1;
                var tblProjectDownload2 = dt2;
                var tblProjectDownload3 = dt3;

                //string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(PhysicalPath_DownloadFiles);

               // string pathAndFile = GetPathAndFileName(PhysicalPath_DownloadFiles, fname);


                var userid = Session["UserId"];
                string finalname = "RTBRVSActuals" + "_Digital_" + monthN + ddlyear.SelectedItem.Text + "_" + userid + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);

                FileInfo file = new FileInfo(MyDir.FullName + "\\" + finalname);
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;

                ExcelWorksheet ws1;
                ExcelWorksheet ws2;
                ExcelWorksheet ws3;
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

                    ws = pck.Workbook.Worksheets.Add("Data");
                    ws.Cells["A1"].LoadFromDataTable(tblProjectDownload0, true);
                    var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                   
                    ws1 = pck.Workbook.Worksheets.Add("WithProjectCode");
                    ws1.Cells["A1"].LoadFromDataTable(tblProjectDownload1, true);
                    ws2 = pck.Workbook.Worksheets.Add("ActionItems + or - above 50K");
                    ws2.Cells["A1"].LoadFromDataTable(tblProjectDownload2, true);
                    ws3 = pck.Workbook.Worksheets.Add("ProjectCode with +50 and -50 Gap");
                    ws3.Cells["A1"].LoadFromDataTable(tblProjectDownload3, true);
                    var fill1 = ws1.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws1.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws1.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                    ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                    ws2.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws2.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                    ws3.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws3.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                    var fill11 = ws1.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill11.PatternType = ExcelFillStyle.Solid;
                    fill11.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    var fill2 = ws2.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill2.PatternType = ExcelFillStyle.Solid;
                    fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    var fill3 = ws3.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill3.PatternType = ExcelFillStyle.Solid;
                    fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    pck.SaveAs(file);
                    pck.Dispose();
                    ReleaseObject(pck);
                    ReleaseObject(ws);
                    ReleaseObject(ws1);
                    ReleaseObject(ws2);
                    ReleaseObject(ws3);


                    GenerateReport(finalname);
                        //DownloadFileProject();
              
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
            VBIDE.VBComponent oModule;
            //try
            {
                //string folder = "ExcelOperations";
                var myDir = new DirectoryInfo(PhysicalPath_DownloadFiles);
                String sCode;
                Object oMissing = System.Reflection.Missing.Value;
                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();

                oBook = oExcel.Workbooks.
                    Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
              
                oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
                sCode = "sub Macro()\r\n" +
                    System.IO.File.ReadAllText(PhysicalPath_Macro + "\\datamacro_test.txt") +
                        "\nend sub";
                oModule.CodeModule.AddFromString(sCode);
                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "Macro" });
                //Adding permission to excel file//

                //oBook.Permission.Enabled = true;
                //oBook.Permission.RemoveAll();
                //string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                //DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                //DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                //UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);

                //userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////

                oBook.Save();
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                GC.Collect();


                Session["Key"] = fname;
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();
                iframe.Attributes.Add("src", "Download.aspx");

                ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);
            }

        }



       
    }
}