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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BECodeProd
{

    public partial class BETrendReport : BasePage
    {

        public string fileName = "BEData.BETrendReport.cs";
        private BEDL service = new BEDL();
        BEDL objbe = new BEDL();
        Logger logger = new Logger();
        static string userid;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();
          
            if(!IsPostBack)
            {
                userid = Session["UserID"].ToString();
                //string cmdtext1 = "EXEC dbo.[SPROC_GetServiceLine]";
                //DataSet dsn1 = new DataSet();
                //dsn1 = service.GetSUForuser(cmdtext1);

                List<string> lstSU = objbe.GetSUForuser(userid);

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

                //ddlServiceLine.DataTextField = "txtserviceline";
                //ddlServiceLine.DataValueField = "txtserviceline";
                //ddlServiceLine.DataSource = dsn1;
                //ddlServiceLine.DataBind();
                lblError.Text = "";
                string MachineRole = Session["MachineRole"].ToString();
                //onload
                //string isValidEntry = Session["Login"].ToString();
                //if (!isValidEntry.Equals("1"))
                //    Response.Redirect("UnAuthorised.aspx");

                //string userID = Session["UserID"] + "";

                if (MachineRole.Equals("Admin") || MachineRole.Equals("UH") || MachineRole.Equals("PnA"))
                {


                    lblYear.Visible = true;

                    ddlyear.Visible = true;
                    btnNewProjectList.Visible = true;
                    lblError.Visible = false;

                    //String sDate = DateTime.Now.ToString();
                    //DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                    //int yy = datevalue.Year;
                    //int fy1 = 0;
                    //int fy2 = 0;
                    //string fy;

                    //ddlyear.Items.Clear();
                    //for (int i = yy-1; i <= yy; i++)
                    //{
                    //    fy = i.ToString().Substring(2, 2);
                    //    fy1 = Convert.ToInt32(fy);

                    //    fy2 = fy1 + 1;
                    //    fy = i.ToString() + "-" + fy2.ToString();
                    //    ddlyear.Items.Add(fy);
                    //}

                    string cmdtext = "select distinct txtFYYR  from EAS_BEData_SDM_NSO order by txtFYYR desc";
                    DataSet ds = new DataSet();
                    ds = service.GetDataSet(cmdtext);
                    ddlyear.DataSource = ds.Tables[0];
                    ddlyear.DataTextField = "txtFYYR";
                    ddlyear.DataValueField = "txtFYYR";
                    ddlyear.DataBind();

                    string cmd = "exec [spDemQuarterCalc_betrend]";
                    DataSet ds1 = new DataSet();
                    ds1 = service.GetDataSet(cmd);
                    string a = ds1.Tables[0].Rows[0][0].ToString();

                    ddlQuarter.DataTextField = "txtqtr";
                    ddlQuarter.DataValueField = "txtqtr";
                    ddlQuarter.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
                    ddlQuarter.DataBind();

                    ddlQuarter.Items.FindByText(a).Selected = true;
                }

                else
                {

                    lblYear.Visible = false;

                    ddlyear.Visible = false;
                    btnNewProjectList.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "Invalid access.";
                }

            }
        }

        //code for btnNewProjectList

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

        protected void btnNewProjectList_Click(object sender, EventArgs e)
        {

            
            var year = ddlyear.SelectedValue;
            string quarter = ddlQuarter.SelectedValue;
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            string ServiceLine = ddlServiceLine.SelectedValue;
            try
            {
                string cmdtext = "EXEC dbo.[BE_trend_Modified] '" + quarter + "','" + year + "','" + userid + "','" + ServiceLine + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmdtext);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    DataTable dtyear = new DataTable();
                    dtyear = ds.Tables[1];

                    //string cmdtext1 = "EXEC dbo.[sp_betrend_maxweek] '" + quarter + "','" + year + "'";
                    //DataSet dsn1 = new DataSet();
                    //dsn1 = service.GetDataSet(cmdtext1);
                    //DataTable dtn1 = new DataTable();
                    //dtn1 = dsn1.Tables[0];
                    //DataTable dtn2 = new DataTable();
                    //dtn2 = dsn1.Tables[1];
                    var tblProjectDownload0 = dt;

                    //string folderadress = @"~/ExcelOperations\EAS_BEvsActuals_trend_template.xlsx"; 
                    string folderadress = @"~/ExcelOperations\EAS_BE_Trend_template.xlsx";
                    folderadress = HttpContext.Current.Server.MapPath(folderadress);
                    string storefolderadress = @"~/Template";
                    storefolderadress = HttpContext.Current.Server.MapPath(storefolderadress);
                    Microsoft.Office.Interop.Excel.Application WRExcel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks WRwbs = null;

                    //Microsoft.Office.Interop.Excel.Workbook WRwb = new Microsoft.Office.Interop.Excel.Workbook();
                    object objOpt = System.Reflection.Missing.Value;
                    Microsoft.Office.Interop.Excel.Workbook WRwb = WRExcel.Workbooks.Add(objOpt);
                    Microsoft.Office.Interop.Excel.Sheets WRss = null;
                    Microsoft.Office.Interop.Excel.Worksheet excelSheet1 = null;
                    //Microsoft.Office.Interop.Excel.Worksheet excelSheetvol = null;
                    //Microsoft.Office.Interop.Excel.Worksheet excelSheetRev = null;
                    //Microsoft.Office.Interop.Excel.Worksheet excelSheetMAxweek = null;
                    Microsoft.Office.Interop.Excel.Worksheet excelSheetYear = null;

                    Microsoft.Office.Interop.Excel.Range c1 = null;
                    Microsoft.Office.Interop.Excel.Range c2 = null;
                    Microsoft.Office.Interop.Excel.Range range = null;
                   
                    WRExcel.Visible = false;
                    WRwbs = WRExcel.Workbooks;



                    WRwb = WRwbs.Open(folderadress, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, objOpt,
                        objOpt, objOpt, objOpt, objOpt, objOpt, objOpt);
                   
                    WRss = WRwb.Sheets;

             

                    string name = "EAS_BEvsActuals_trend_" + userid + "_" + DateTime.Now.ToString("ddMMM_HHmm") + "IST.xlsx";
                    string folder = "ExcelOperations";
                    var MyDir = new DirectoryInfo(Server.MapPath(folder));

                    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == name) != null)
                        System.IO.File.Delete(MyDir.FullName + "\\" + name);

                    String excelFile1 = "~\\ExcelOperations\\"+name;
                    String destPath = Server.MapPath(excelFile1);

                  

                    excelSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("Data");
                    //excelSheetvol = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("Volume");
                    //excelSheetRev = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("Revenue");
                    //excelSheetMAxweek = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("BE Max Week");
                    excelSheetYear = (Microsoft.Office.Interop.Excel.Worksheet)WRss.get_Item("Trend");
                    //excelSheetvol.Cells[1, 7] = quarter + " " + year + " BE Volume Trend";
                    //excelSheetRev.Cells[1, 7] = quarter + " " + year + " BE Revenue Trend";
                    //excelSheetMAxweek.Cells[1, 1] = dtn1.Rows[0][0].ToString();
                    excelSheetYear.Cells[1, 17] = dtyear.Rows[0][0].ToString();

                    //for (int i = 1; i <= dtn2.Rows.Count; i++)
                    //{
                    //    excelSheetMAxweek.Cells[i + 1, 2] = dtn2.Rows[i - 1][0].ToString();
                    //}
                  
                    DataTable dt1 = new DataTable();

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

                    c1 = (Microsoft.Office.Interop.Excel.Range)excelSheet1.Cells[2, 1];
                    c2 = (Microsoft.Office.Interop.Excel.Range)excelSheet1.Cells[1 + dt.Rows.Count, dt.Columns.Count];
                    range = excelSheet1.get_Range(c1, c2);


                    //Fill Array in Excel
                    range.Value2 = DataArray;
                    WRwb.RefreshAll();



                    WRExcel.DisplayAlerts = false;
                    WRwb.SaveAs(destPath);

                    WRwb.Close(false, folderadress, null);
                    if (range != null)
                    {
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(range);
                        range = null;
                    }
                    
                    if (c2 != null)
                    {
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(c2);
                        c2 = null;
                    }

                    if (c1 != null)
                    {
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(c1);
                        c1 = null;
                    }
                    if (excelSheetYear != null)
                    {
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelSheetYear);
                        excelSheetYear = null;
                    }
                    //if (excelSheetMAxweek != null)
                    //{
                    //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelSheetMAxweek);
                    //    excelSheetMAxweek = null;
                    //}
                    //if (excelSheetRev != null)
                    //{
                    //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelSheetRev);
                    //    excelSheetRev = null;
                    //}
                    //if (excelSheetvol != null)
                    //{
                    //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelSheetvol);
                    //    excelSheetvol = null;
                    //}
                    if (excelSheet1 != null)
                    {
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelSheet1);
                        excelSheet1 = null;
                    }
                   
                   
                   
                   
                    //if (excelSheetvol != null)
                    //{
                    //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelSheetvol);
                    //    excelSheetvol = null;
                    //}
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                    WRss = null;
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRwb);
                    WRwb = null;
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRwbs);
                    WRwbs = null;



                    WRExcel.Quit();
                    WRExcel = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();



                    DownloadFileBEReport_new(quarter, year, name);
                    
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
        }


        void DownloadFileBEReport_new(string qtr, string fy, string name)
        {
            string UserId = Session["UserID"].ToString();

            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            Microsoft.Office.Interop.Excel.Sheets WRss = null;
            VBIDE.VBComponent oModule;
            try
            {
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                String sCode;
                Object oMissing = System.Reflection.Missing.Value;

                oExcel = new Excel.Application();



                FileInfo file = new FileInfo(MyDir.FullName + "\\" + name);

                oBook = oExcel.Workbooks.
                    Open(file.ToString() + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                WRss = oBook.Sheets;

                oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);

                //sCode = "sub Betrend_Macro()\r\n" +
                // System.IO.File.ReadAllText(MyDir.FullName + "\\Betrend_Macro.txt");


                //oModule.CodeModule.AddFromString(sCode);

                //oExcel.GetType().InvokeMember("Run",
                //                System.Reflection.BindingFlags.Default |
                //                System.Reflection.BindingFlags.InvokeMethod,
                //                null, oExcel, new string[] { "Betrend_Macro" });

                string fy1 = fy.Substring(5, 2);
                string finalname = "EAS_" + qtr + fy1 + "_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";


                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);

                //Adding permission to excel file//

                oBook.Permission.Enabled = true;
                oBook.Permission.RemoveAll();
                string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                userper.ExpirationDate = dtExpireDate;
           

                oExcel.DisplayAlerts = false;
                oBook.SaveAs(MyDir.FullName + "\\" + finalname);
                oBook.Close(false, file.ToString(), null);

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

                Session.Add("key", finalname);

                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();
                iframe.Attributes.Add("src", "Download.aspx");

                ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);

            }
            catch (Exception ex)
            {

            }

        }

         void DownloadFileBEReport(string qtr, string fy, string name)
        {
            string UserId = Session["UserID"].ToString();

            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            Microsoft.Office.Interop.Excel.Sheets WRss = null;
            VBIDE.VBComponent oModule;
            try
            {
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                String sCode;
                Object oMissing = System.Reflection.Missing.Value;

                oExcel = new Excel.Application();



                FileInfo file = new FileInfo(MyDir.FullName + "\\" + name);

                oBook = oExcel.Workbooks.
                    Open(file.ToString() + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                WRss = oBook.Sheets;

                oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);

                sCode = "sub Betrend_Macro()\r\n" +
                 System.IO.File.ReadAllText(MyDir.FullName + "\\Betrend_Macro.txt");


                oModule.CodeModule.AddFromString(sCode);

                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "Betrend_Macro" });
            
                string fy1 = fy.Substring(5, 2);
                string finalname = "EAS_" + qtr + fy1 + "_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";

              
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);

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

                oExcel.DisplayAlerts = false;
                oBook.SaveAs(MyDir.FullName + "\\" + finalname);
                oBook.Close(false, file.ToString(),null);
                
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
                
                Session.Add("key", finalname);

                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();
                iframe.Attributes.Add("src", "Download.aspx");

                ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);

            }
            catch (Exception ex)
            {

            }

        }
    

       
    }
}